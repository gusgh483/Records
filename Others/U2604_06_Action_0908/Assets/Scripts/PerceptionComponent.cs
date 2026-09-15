using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PerceptionComponent : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    private float distance = 5.0f; //감지 거리

    [SerializeField, Range(0.1f, 5)]
    private float lostTime = 1.0f; //감지 후 잃어버리는 시간

    [SerializeField]
    private LayerMask layerMask; //감지할 레이어


    private BoxCollider2D boxCollider;

    private Dictionary<GameObject, float> table;

#if UNITY_EDITOR
    private void Reset()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
        //layerMask |= 1 << LayerMask.NameToLayer("Environments");
    }
#endif

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        table = new Dictionary<GameObject, float>();
    }

    private void Update()
    {
        Vector2 position = transform.position;
        position.y += boxCollider.offset.y;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, distance, layerMask);
        
        //1. 감시 대상 등록 및 시간 업데이트
        foreach(Collider2D collider in colliders)
        {
            //1-1. 최초 감시 - 감시 대상 테이블에 등록
            if(table.ContainsKey(collider.gameObject) == false)
            {
                table.Add(collider.gameObject, Time.time);

                continue;
            }

            //1-2. 감시 대상이 이미 등록되어 있는 경우 - 시간 업데이트
            table[collider.gameObject] = Time.time;
        }

        //2. 감지 실패 후 시간 초과 대상자 선정 후 삭제
        List<GameObject> removeList = new List<GameObject>();
        //foreach(KeyValuePair<GameObject, float> pair in table)
        foreach(var pair in table)
        {
            if ((Time.time - pair.Value) >= lostTime)
                removeList.Add(pair.Key);
        }

        removeList.RemoveAll(remove => table.Remove(remove));
    }

    public GameObject GetPercievedPlayer()
    {
        foreach (var pair in table)
        {
            if(pair.Key.layer == LayerMask.NameToLayer("Player"))
                return pair.Key;
        }

        return null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying == false)
            return;

        Vector2 position = transform.position;
        position.y += boxCollider.offset.y;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, distance);


        GameObject player = GetPercievedPlayer();
        
        if (player == null)
            return;

        Vector2 playerPosition = player.transform.position;
        BoxCollider2D playerBox = player.GetComponent<BoxCollider2D>();
        playerPosition.y += playerBox.offset.y;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(position, playerPosition);
        Gizmos.DrawWireSphere(playerPosition, 0.25f);
    }
#endif
}
