using UnityEngine;

public class BaseCharacterAnimations : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private static readonly int Stomp1 = Animator.StringToHash("Stomp");
    private static readonly int Stomping = Animator.StringToHash("Stomping");

    public void Stomp()
    {
        anim.SetBool(Stomping, true);
        anim.SetTrigger(Stomp1);
    }

    public void ReleaseStomp()
    {
        anim.SetBool(Stomping, false);
    }
}
