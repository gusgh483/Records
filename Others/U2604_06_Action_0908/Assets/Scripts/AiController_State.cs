using System;
using UnityEngine;

public partial class AiController
{
    private void SetWaitMode()
    {
        if (WaitMode)
            return;

        ChangeType(StateType.Wait);

        character.Stop();
    }

    private void SetApproachMode()
    {
        if (ApproachMode)
            return;

        ChangeType(StateType.Approach);

        character.Move();
    }

    private void SetAttackMode()
    {
        if (AttackMode)
            return;

        ChangeType(StateType.Attack);

        character.Stop();
        character.OnAttack?.Invoke();
    }

    public void SetDamageMode()
    {
        if (AttackMode)
        {
            Locomotion locomotion = GetComponentInChildren<Locomotion>();
            if (locomotion != null && locomotion.IsAttacking)
                locomotion.End_Attack();
        }

        if (DamagedMode)
            return;

        ChangeType(StateType.Damaged);

        character.Stop();
    }

    public void SetPatrolMode()
    {
        if (PatrolMode)
            return;

        ChangeType(StateType.Patrol);

        character.SetInputMove(new Vector2(-1.0f , 0.0f)); 
    }


    public Action<StateType, StateType> OnStateChanged;

    private void ChangeType(StateType type)
    {
        StateType prev = stateType;
        stateType = type;

        OnStateChanged?.Invoke(prev, type);
    }
}
