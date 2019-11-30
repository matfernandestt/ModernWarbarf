using UnityEngine;

public class BarfboyController : BaseMovement, IKnockbackable
{
    [SerializeField] private Transform shootSource;
    [SerializeField] private BaseCharacterAnimationEvents animEvents;
    [SerializeField] private ObjectPool barfPool;
    [SerializeField] private float shootForce;

    private void OnEnable()
    {
        SetPlayerEvents();
    }

    private void OnDisable()
    {
        DisablePlayerEvents();
    }

    private void SetPlayerEvents()
    {
        ActionAttackNormal += BarfboyNormalAction;
        ActionAttackSpecial += SpecialAttack;
        ActionTaunt += TauntAction;

        animEvents.animationActionEvent += PlayerLoseGainControl;
    }

    private void DisablePlayerEvents()
    {
        ActionAttackNormal -= BarfboyNormalAction;
        ActionAttackSpecial -= SpecialAttack;
        ActionTaunt -= TauntAction;

        animEvents.animationActionEvent -= PlayerLoseGainControl;
    }

    public void BarfboyNormalAction()
    {
        Debug.Log("normal action");
    }

    private void SpecialAttack()
    {
        if(GameLevelManager.instance.EndedLevel)
        {
            DisablePlayerEvents();
            return;
        }

        var barf = barfPool.RequestObject(shootSource.position, SideRotated()).GetComponent<PoolableObject>();
        var barfRB = barf.GetComponent<Rigidbody>();
        barfRB.AddForce(transform.right * shootForce, ForceMode.Force);
        barfPool.ReturnObject(barf, 1f);
    }

    private void TauntAction()
    {

    }
}