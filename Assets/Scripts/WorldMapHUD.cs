using TMPro;
using UnityEngine;

public class WorldMapHUD : MonoBehaviour
{
    public static WorldMapHUD instance;

    [SerializeField] private Animator _levelTextAnim;
    [SerializeField] private TextMeshProUGUI _levelText;
    private static readonly int EnterLevelRange = Animator.StringToHash("EnterLevelRange");
    private static readonly int ExitLevelRange = Animator.StringToHash("ExitLevelRange");

    private string _closestTouchedLevel;

    public string ClosestTouchedLevel
    {
        get => _closestTouchedLevel;
        set => _closestTouchedLevel = value;
    }

    private void Awake()
    {
        instance = this;
    }

    public void CloseToLevel(string levelInfo)
    {
        _levelTextAnim.gameObject.SetActive(true);
        _levelTextAnim.SetTrigger(EnterLevelRange);
        _levelText.text = levelInfo;
    }

    public void FarToLevel()
    {
        _levelTextAnim.SetTrigger(ExitLevelRange);
    }
}
