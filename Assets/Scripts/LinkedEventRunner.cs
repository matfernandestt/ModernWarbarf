using System.Collections;
using UnityEngine;

public class LinkedEventRunner : MonoBehaviour, ILinkedEvent
{
    [SerializeField] private Animator anim;
    private static readonly int RunningEvent = Animator.StringToHash("RunEvent");
    private static readonly int RunBack = Animator.StringToHash("RunBack");

    public void RunEvent()
    {
        anim.SetTrigger(RunningEvent);
        RunBackEvent();
    }

    public void RunBackEvent()
    {
        IEnumerator RunningBackEvent()
        {
            yield return new WaitForSeconds(5f);
            anim.SetTrigger(RunBack);
        }

        StartCoroutine(RunningBackEvent());
    }
}
