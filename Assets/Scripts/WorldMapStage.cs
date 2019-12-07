using System;
using UnityEngine;

public class WorldMapStage : MonoBehaviour
{
    [SerializeField] private string LevelToGoTag;
    [SerializeField] private string _levelInfo;
    [SerializeField] private Animator anim;

    private static readonly int Contact = Animator.StringToHash(elementAnimatorTag);

    private const string elementPlayerTag = "Player";
    private const string elementAnimatorTag = "Contact";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(elementPlayerTag))
        {
            anim.SetBool(Contact, true);
            WorldMapHUD.instance.CloseToLevel(_levelInfo);
            WorldMapHUD.instance.ClosestTouchedLevel = LevelToGoTag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(elementPlayerTag))
        {
            anim.SetBool(Contact, false);
            WorldMapHUD.instance.FarToLevel();
            WorldMapHUD.instance.ClosestTouchedLevel = "";
        }
    }
}
