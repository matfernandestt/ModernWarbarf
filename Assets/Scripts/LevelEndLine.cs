using UnityEngine;

public class LevelEndLine : MonoBehaviour
{
    private readonly string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PlayerTag))
        {
            GameLevelManager.instance.EndLevel();
        }
    }
}
