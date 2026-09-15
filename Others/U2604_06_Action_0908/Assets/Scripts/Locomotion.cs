using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Locomotion : MonoBehaviour
{
    [SerializeField]
    private ColliderData[] colliderDatas;

    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;

    private Character character;
    private int environmentLayer;

    private Sprite currentSprite;

    private Dictionary<Sprite, SpriteHitboxData> hitboxDataTable;
    private List<IDamagable> hittedList;


    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        character = GetComponentInParent<Character>();


        hitboxDataTable = new Dictionary<Sprite, SpriteHitboxData>();
        hittedList = new List<IDamagable>();


        foreach (ColliderData data in colliderDatas)
        {
            for (int i = 0; i < data.Count; i++)
                hitboxDataTable.Add(data[i].sprite, data[i]);
        }
    }

    private void Start()
    {
        if(character != null)
            character.OnAttack += Attack;

        environmentLayer = LayerMask.NameToLayer("Environments");

        End_Collision();
    }

    private void Update()
    {
        // 임시 (속도조절)
        if (transform.parent.GetComponent<Enemy>() != null)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Attack"))
                animator.speed = 0.5f;
            else
                animator.speed = 1.0f;

        }
    }

    private void LateUpdate()
    {
        if (currentSprite != spriteRenderer.sprite)
        {
            currentSprite = spriteRenderer.sprite;

            SpriteHitboxData data = hitboxDataTable[currentSprite];
            boxCollider2D.offset = data.offset;
            boxCollider2D.size = data.size;
        }
    }

    #region Attacking

    bool bAttacking = false;
    public bool IsAttacking => bAttacking;

    bool bComboEnabled = false;
    bool bComboExist = false;
    int comboIndex = 0;

    public void Combo_Section(int enabled)
    {
        bComboEnabled = (enabled == 1) ? true : false;
    }

    private string debugString = "";

    private void Attack()
    {
        if (bComboEnabled)
        {
            bComboEnabled = false;
            bComboExist = true;

            return;
        }


        if (bAttacking)
            return;

        
        bAttacking = true;

        character.Stop();
        animator.SetBool("IsAttack", true);
    }

    public void Begin_Attack()
    {
        if (bComboExist == false)
            return;


        bComboExist = false;

        comboIndex++;
        character.Stop();
        animator.SetTrigger("Combo");

        debugString = $"Begin_Attack : { comboIndex }";
    }

    public void End_Attack()
    {
        //debugString = $"End_Attack : {comboIndex}";
        //Debug.Log(debugString);

        bAttacking = false;
        bComboEnabled = false;
        bComboExist = false;
        comboIndex = 0;

        character.Move();
        animator.SetBool("IsAttack", false);

        GetComponentInParent<AiController>()?.End_Attack();
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == environmentLayer)
            return;


        MonoBehaviour[] scripts = collision.gameObject.GetComponents<MonoBehaviour>();

        IDamagable damage = null;
        foreach (MonoBehaviour script in scripts)
        {
            if (script is IDamagable)
            {
                damage = script as IDamagable;

                break;
            }
        }

        if (damage == null)
            return;


        //print(collision.name);

        IDamagable result = hittedList.Find(x => x == damage);
        if (result != null)
            return;

        hittedList.Add(damage);


        BoxCollider2D parentBox = GetComponentInParent<BoxCollider2D>();
        AttackData attackData = character.Data.AttackDatas[comboIndex];

        Vector2 point = transform.parent.position;
        point += parentBox.offset;
        point += attackData.ImpactOffset;
        point = collision.ClosestPoint(point);

        DamagableData data = new DamagableData()
        {
            AttackLocomotion = gameObject,
            
            HitPoint = point,
            AttackerData = character.Data,
            ComboIndex = comboIndex,
        };

        damage.Damaged(data);
    }

    public void Begin_Collision()
    {
        boxCollider2D.enabled = true;
    }

    public void End_Collision()
    {
        boxCollider2D.enabled = false;

        hittedList.Clear();
    }

    public void End_Damaged()
    {
        GetComponentInParent<AiController>()?.End_Damaged();
    }

    public void Play_Impulse()
    {
        AttackData data = character.Data.AttackDatas[comboIndex];

        Play_Impulse(data.ImpulseDuration, data.ImpulseDirection);
    }

    public void Play_Impulse(float duration, Vector2 direction)
    {
        if (impulseSource == null)
            return;

        
        if (duration <= 0.0f)
            return;

        if (direction.magnitude <= 0.0f)
            return;

        
        impulseSource.ImpulseDefinition.ImpulseDuration = duration;
        impulseSource.DefaultVelocity = direction;
        impulseSource.GenerateImpulse();
    }


    private void OnGUI()
    {
        //if (currentSprite != null)
        //GUILayout.Label(currentSprite.name);

        GUILayout.Label(debugString);
    }
}
