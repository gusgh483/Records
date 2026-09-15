using System;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(HealthComponent))]
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected string dataFile;

    [SerializeField]
    private LayerMask characterLayerMask;

    protected CharacterData data;
    public CharacterData Data => data;

    protected Animator animator;
    protected new Rigidbody2D rigidbody2D;
    protected BoxCollider2D boxCollider2D;

    protected bool bMove;
    protected bool bFacingRight;
    protected Vector2 inputMove;

    public Action OnAttack;

    protected HealthComponent healthComponent;
    public bool IsDead => healthComponent.IsDead;

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        characterLayerMask |= 1 << LayerMask.NameToLayer("Player");
        characterLayerMask |= 1 << LayerMask.NameToLayer("Character");
        characterLayerMask |= 1 << LayerMask.NameToLayer("Blocking");
    }
#endif

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        healthComponent = GetComponent<HealthComponent>();


        data = Resources.Load<CharacterData>("Characters/" + dataFile);
    }

    protected virtual void Start()
    {
        
    }

    protected void Update()
    {
        animator.SetFloat("MoveSpeed", Mathf.Abs(inputMove.x));

        Update_Facing();
    }

    protected void FixedUpdate()
    {
        if (bMove == false)
            return;

        if(inputMove.x == 0.0f)
        {
            rigidbody2D.linearVelocityX = 0.0f;

            return;
        }


        float distance = (data.MoveSpeed * Time.fixedDeltaTime) + 0.1f;

        Vector2 origin = boxCollider2D.bounds.center;
        
        Vector2 size = boxCollider2D.bounds.size;
        size.x -= 0.1f;
        size.y -= 0.1f;

        Vector2 direction = inputMove.x > 0.0f ? Vector2.right : Vector2.left;

        boxCollider2D.enabled = false;
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0.0f, direction, distance, characterLayerMask);
        boxCollider2D.enabled = true;

        //print(gameObject.name + " / " + hit.collider?.gameObject.name);

        if(hit.collider == null)
        {
            rigidbody2D.linearVelocityX = inputMove.x * data.MoveSpeed;

            return;
        }

        rigidbody2D.linearVelocityX = 0.0f;
    }

    private void Update_Facing()
    {
        if (bMove == false)
            return;

        if (inputMove.x == 0.0f)
            return;

        if ((inputMove.x > 0.0f) == bFacingRight)
            return;

        
        bFacingRight = !bFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
    }

    public void Move()
    {
        bMove = true;
    }

    public virtual void Stop()
    {
        bMove = false;

        rigidbody2D.linearVelocity = Vector2.zero;
    }

    public void SetInputMove(Vector2 value)
    {
        inputMove = value;
    }

    protected virtual void OnGUI()
    {

    }
}
