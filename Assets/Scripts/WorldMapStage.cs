using UnityEngine;

public class WorldMapStage : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private readonly string PlayerTag = "Player";
    private readonly string AnimatorTag = "Contact";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PlayerTag))
        {
            anim.SetBool(AnimatorTag, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            anim.SetBool(AnimatorTag, false);
        }
    }
}
