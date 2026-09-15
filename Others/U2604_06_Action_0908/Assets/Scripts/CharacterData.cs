using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    [SerializeField, Range(1, 50)]
    private float power = 25;
    public float Power => power;

    
    [SerializeField, Range(1, 100)]
    private int frame = 10;
    public int Frame => frame;

    
    [SerializeField]
    private GameObject impactParticle;
    public GameObject ImpactParticle => impactParticle;

    [SerializeField]
    private Vector2 impactOffset = Vector2.zero;
    public Vector2 ImpactOffset => impactOffset;


    [SerializeField]
    private float impulseDuration;
    public float ImpulseDuration => impulseDuration;


    [SerializeField]
    private Vector2 impulseDirection;
    public Vector2 ImpulseDirection => impulseDirection;
}

[CreateAssetMenu(fileName="Data", menuName="CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("- Health")]
    [SerializeField, Range(10, 100)]
    private float maxHealth = 100;
    public float MaxHealth => maxHealth;


    [Header("- Moving")]
    [SerializeField, Range(1, 10)]
    private float moveSpeed = 5;
    public float MoveSpeed => moveSpeed;


    [Header("- Attack")]
    [SerializeField]
    AttackData[] attackDatas;
    public AttackData[] AttackDatas => attackDatas;
}
