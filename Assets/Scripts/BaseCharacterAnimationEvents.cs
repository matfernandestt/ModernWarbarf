using UnityEngine;
using UnityEngine.Events;

public class BaseCharacterAnimationEvents : MonoBehaviour
{
    public UnityAction animationActionEvent;

    public void AnimationActionEvent()
    {
        animationActionEvent?.Invoke();
    }
}
