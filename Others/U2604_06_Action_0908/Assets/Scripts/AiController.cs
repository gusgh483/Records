using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public partial class AiController : MonoBehaviour
{
    [SerializeField]
    private GameObject statePrefab;
    private GameObject stateOrigin;


    [Header("- Attack")]
    [SerializeField]
    private float attackDistance = 2.0f;

    [SerializeField]
    private float attackDelay = 2.0f; //공격 후 지연 기준 시간

    [SerializeField]
    private float attackDelayDeviation = 0.5f; //공격 후 지연 랜덤 시간


    [Header("- Damaged")]
    [SerializeField]
    private float damagedDelay = 1.5f; //피격 후 지연 기준 시간

    [SerializeField]
    private float damagedDelayDeviation = 0.5f; //피격 후 지연 랜덤 시간


    private TextMeshProUGUI stateText;

    private Character character;
    private PerceptionComponent perception;

    private float coolTime = 0.0f;

    
    public enum StateType
    {
        Wait, Patrol, Approach, Attack, Damaged, 
    }
    private StateType stateType;

    
    public bool WaitMode => stateType == StateType.Wait;
    public bool PatrolMode => stateType == StateType.Patrol;
    public bool ApproachMode => stateType == StateType.Approach;
    public bool AttackMode => stateType == StateType.Attack;
    public bool DamagedMode => stateType == StateType.Damaged;

#if UNITY_EDITOR
    private void Reset()
    {
        string file = "Prefabs/AiState";
        statePrefab = Resources.Load<GameObject>(file);
        if (statePrefab == null) Debug.LogWarning(file + " 프리팹 없음");
    }
#endif


    private void Awake()
    {
        character = GetComponent<Character>();
        perception = GetComponent<PerceptionComponent>();
    }

    private void Start()
    {
        stateOrigin = Instantiate<GameObject>(statePrefab, transform);
        stateText = stateOrigin.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        stateText.text = $"{stateType}\n({coolTime.ToString("F3")})";
    }

    private void FixedUpdate()
    {
        if (FixedUpdate_DecreaseCoolTime())
            return;

        if (AttackMode || DamagedMode)
            return;

        if(character.IsDead)
        {
            SetWaitMode();
            
            return;
        }    


        GameObject player = perception.GetPercievedPlayer();

        //플레이어 감지 않됨
        if (player == null)
        {
            SetWaitMode();

            return;
        }


        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= attackDistance)
        {
            SetAttackMode();
            
            return;
        }

        SetApproachMode();
    }

    private void LateUpdate()
    {
        if(ApproachMode)
        {
            GameObject obj =  perception.GetPercievedPlayer();
            if (obj == null) return;


            Vector2 direction = obj.transform.position - transform.position;
            
            direction.x = Mathf.Sign(direction.x);
            direction.y = 0.0f;
            debugString = direction.ToString();
       
            character.SetInputMove(direction);
        }
    }

    private void SetCoolTime(float delayTime, float delayTimeDeviation)
    {
        //if(coolTime <= 0.0f)
        //{
        //    coolTime = 0.0f;

        //    return;
        //}

        float time = 0.0f;
        time += delayTime;
        time += Random.Range(-delayTimeDeviation, +delayTimeDeviation);

        coolTime = time;
    }

    private bool FixedUpdate_DecreaseCoolTime()
    {
        if (WaitMode == false)
            return false;

        if (coolTime <= 0.0f)
            return false;

        
        coolTime -= Time.fixedDeltaTime;

        
        bool bFinished = false;
        bFinished |= coolTime <= 0.0f;
        bFinished |= perception.GetPercievedPlayer() == null;

        if(bFinished)
        {
            coolTime = 0.0f;

            return false;
        }


        return true;
    }

    public void End_Damaged()
    {
        SetCoolTime(damagedDelay, damagedDelayDeviation);

        SetWaitMode();
    }

    public void End_Attack()
    {
        SetCoolTime(attackDelay, attackDelayDeviation);

        SetWaitMode();
    }

    private string debugString = "";
    private void OnGUI()
    {
        GUILayout.Label(debugString);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying == false)
            return;


        Vector2 position = GetComponent<BoxCollider2D>().bounds.center;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackDistance);
    }
#endif
}
