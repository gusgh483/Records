using UnityEngine;

public struct DamagableData
{
    public GameObject AttackLocomotion;

    public Vector2 HitPoint;
    public CharacterData AttackerData;
    public int ComboIndex;
}

public interface IDamagable
{
    void Damaged(DamagableData data);
}
