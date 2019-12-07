using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private LinkedEventRunner linkedEvent;

    private bool alreadyInteractedWithThisButton;

    private static readonly int ButtonPress = Animator.StringToHash("ButtonPress");

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyInteractedWithThisButton)
        {
            var barfBoy = other.GetComponent<BarfboyController>();

            if (barfBoy != null)
            {
                if (barfBoy.Stomping)
                {
                    ButtonPressed();
                }
            }
        }
    }

    public void ButtonPressed()
    {
        alreadyInteractedWithThisButton = true;
        _anim.SetTrigger(ButtonPress);
        if(linkedEvent != null) linkedEvent.RunEvent();
    }
}
