using System.Collections;
using System.Data;
using UnityEngine;

public class Enemy : Character, IDamagable
{
    private AiController aiController;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        dataFile = gameObject.name;
    }
#endif

    protected override void Awake()
    {
        base.Awake();

        aiController = GetComponent<AiController>();
    }

    protected override void Start()
    {
        base.Start();

        if(string.IsNullOrEmpty(dataFile))
            Debug.LogWarning(dataFile + " ¾øÀ½");
    }

    public override void Stop()
    {
        base.Stop();

        inputMove = Vector2.zero;
    }

    public void Damaged(DamagableData data)
    {
        AttackData attackData = data.AttackerData.AttackDatas[data.ComboIndex];
        healthComponent.Damaged(attackData.Power);

        //Change Color
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null)
                StartCoroutine(Co_RestoreColor(spriteRenderer, 0.2f));
        }

        //Hit Stop
        {
            Animator attackAnimator = data.AttackLocomotion.GetComponent<Animator>();
            Animator damagerAnimator = GetComponentInChildren<Animator>();

            if (attackAnimator != null && damagerAnimator != null)
                StartCoroutine(Co_HitStop(attackData.Frame, attackAnimator, damagerAnimator));
        }

        //Particle
        {
            Instantiate<GameObject>(attackData.ImpactParticle, data.HitPoint, Quaternion.identity);
        }

        //Impulse
        {
            Locomotion locomotion = GetComponentInChildren<Locomotion>();
            
            locomotion.Play_Impulse(attackData.ImpulseDuration, attackData.ImpulseDirection);
        }

        //Motion
        {
            Animator animator = GetComponentInChildren<Animator>();

            if(healthComponent.IsDead)
            {
                animator?.SetTrigger("Dead");

                boxCollider2D.enabled = false;
                healthComponent.Dead();

                Destroy(gameObject, 3.0f);
            }
            else
            {
                aiController?.SetDamageMode();

                animator?.SetInteger("Damage_Index", data.ComboIndex);
                animator?.SetTrigger("Damage");
            }
        }
    }

    private IEnumerator Co_RestoreColor(SpriteRenderer spriteRenderer, float time)
    {
        spriteRenderer.color = Color.red;
        
        yield return new WaitForSeconds(time);

        spriteRenderer.color = Color.white;
    }

    private IEnumerator Co_HitStop(int frame, Animator attacker, Animator damager)
    {
        attacker.speed = damager.speed = 0.0f;

        for (int i = 0; i < frame; i++)
            yield return new WaitForFixedUpdate();

        attacker.speed = damager.speed = 1.0f;
    }
}
