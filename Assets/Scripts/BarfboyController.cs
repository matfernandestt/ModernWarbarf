using UnityEngine;

public class BarfboyController : BaseMovement, IKnockbackable
{
    [SerializeField] private Transform shootSource;
    [SerializeField] private BaseCharacterAnimations baseAnim;
    [SerializeField] private BaseCharacterAnimationEvents animEvents;
    [SerializeField] private ObjectPool barfPool;
    [SerializeField] private float shootForce;

    private bool stomping;

    public bool Stomping
    {
        get => stomping;
        set => stomping = value;
    }

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
        ActionStomp += Stomp;
        ActionReleasedStomp += ReleasedStomp;
        ActionTaunt += TauntAction;

        animEvents.animationActionEvent += PlayerLoseGainControl;
    }

    private void DisablePlayerEvents()
    {
        ActionAttackNormal -= BarfboyNormalAction;
        ActionAttackSpecial -= SpecialAttack;
        ActionStomp -= Stomp;
        ActionReleasedStomp -= ReleasedStomp;
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

    private void Stomp()
    {
        baseAnim.Stomp();
        stomping = true;
        CanMoveHorizontally = false;

        AddImpact(Vector3.down, 3000);
    }

    private void ReleasedStomp()
    {
        baseAnim.ReleaseStomp();
        stomping = false;
        CanMoveHorizontally = true;
    }

    private void TauntAction()
    {

    }

    [ContextMenu("Impact Test")]
    public void ImpactTest()
    {
        AddImpact(-transform.right + Vector3.up, 3000);
    }
}