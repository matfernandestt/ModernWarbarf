using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager instance;

    public UnityAction EndLevelHandler; 

    public bool EndedLevel = false;

    private Coroutine currentCoroutine;
    
    private void Awake()
    {
        instance = this;
        
        BeginLevel();
    }

    public void BeginLevel()
    {
        ScreenFader.instance.OpenFade();
    }

    public void EndLevel()
    {
        if (!EndedLevel)
        {
            EndLevelHandler?.Invoke();
            EndedLevel = true;
            IEnumerator SlowingTime()
            {
                float progress = Time.timeScale;
                while (progress > 0)
                {
                    progress -= .1f;
                    if (progress < 0)
                        Time.timeScale = 0;
                    else
                        Time.timeScale = progress;
                    yield return new WaitForSeconds(.01f);
                }
            }

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(SlowingTime());
        }
    }
}
