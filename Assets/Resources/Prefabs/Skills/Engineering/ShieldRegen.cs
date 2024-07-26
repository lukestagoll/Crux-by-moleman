using UnityEngine;
using System.Collections;

public class ShieldRegen : SkillBase
{
    private bool isRegenerating = false;
    private Coroutine regenCoroutine;

    public ShieldRegen(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "ShieldRegen";
    }

    public override void Activate()
    {
        TargetShip.OnHit += OnHit;
        TargetShip.OnUpdate += OnUpdate;
    }

    private float DetermineRegenRate()
    {
        switch (Level)
        {
            case 1:
                return 0.025f; // 2.5% per second
            case 2:
                return 0.05f; // 5% per second
            case 3:
                return 0.1f; // 10% per second
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 0f;
        }
    }

    private void OnHit()
    {
        isRegenerating = false;
        if (regenCoroutine != null)
        {
            TargetShip.StopCoroutine(regenCoroutine);
        }
        regenCoroutine = TargetShip.StartCoroutine(StartRegenCountdown());
    }

    private IEnumerator StartRegenCountdown()
    {
        yield return new WaitForSeconds(5f);
        isRegenerating = true;
    }

    private void OnUpdate()
    {
        if (isRegenerating && TargetShip.Shield < TargetShip.MaxShield)
        {
            float regenAmount = TargetShip.MaxShield * DetermineRegenRate() * Time.deltaTime;
            TargetShip.AddShield(regenAmount);
        }
    }

    public override void Deactivate()
    {
        // TargetShip.OnHit -= OnHit;
        // TargetShip.OnUpdate -= OnUpdate;
        // if (regenCoroutine != null)
        // {
        //     TargetShip.StopCoroutine(regenCoroutine);
        // }
    }
}
