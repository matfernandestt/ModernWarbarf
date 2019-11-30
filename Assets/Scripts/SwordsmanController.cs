using UnityEngine;

public class SwordsmanController : BaseMovement, IKnockbackable
{
    [SerializeField] private BaseCharacterAnimationEvents animEvents;

    private void OnEnable()
    {
        ActionAttackNormal += SwordsmanNormalAttack;
        ActionAttackSpecial += SwordsmanSpecialAttack;
        ActionTaunt += SwordsmanTaunt;

        animEvents.animationActionEvent += PlayerLoseGainControl;
    }

    private void OnDisable()
    {
        ActionAttackNormal -= SwordsmanNormalAttack;
        ActionAttackSpecial -= SwordsmanSpecialAttack;
        ActionTaunt -= SwordsmanTaunt;

        animEvents.animationActionEvent -= PlayerLoseGainControl;
    }

    public void SwordsmanNormalAttack()
    {
        Debug.Log("swordsman normal");
    }

    private void SwordsmanSpecialAttack()
    {
        Debug.Log("swordsman special");
    }

    private void SwordsmanTaunt()
    {
        Debug.Log("swordsman taunt");
        anim.SetTrigger("Taunt");
    }
}