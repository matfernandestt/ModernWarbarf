using UnityEngine;
using UnityEngine.Events;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    
    [SerializeField] private Animator anim;

    public UnityAction OnCloseFader;
    public UnityAction OnOpenFader;
    
    private static readonly int Close = Animator.StringToHash("Close");
    private static readonly int Open = Animator.StringToHash("Open");
    private static readonly int ClosedIdle = Animator.StringToHash("ClosedIdle");
    private static readonly int OpenedIdle = Animator.StringToHash("OpenedIdle");

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CloseFade()
    {
        anim.SetTrigger(Close);
    }
    
    public void OpenFade()
    {
        anim.SetTrigger(Open);
    }

    public void OnCloseFadeAnimationEnd()
    {
        OnCloseFader?.Invoke();
        ClearEvents();
    }
    
    public void OnOpenFadeAnimationEnd()
    {
        OnOpenFader?.Invoke();
        ClearEvents();
    }

    private void ClearEvents()
    {
        OnCloseFader = null;
        OnOpenFader = null;
    }
}
