using System.Collections;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    public Transform camTarget;
    public Vector3 interpolationRate;
    public Vector3 camOffsets;

    private Transform t;
    private Coroutine currentCoroutine;

    [SerializeField] private Transform CameraLeftBound;
    [SerializeField] private Transform CameraRightBound;

    private void Awake()
    {
        t = transform;

        CameraLeftBound.parent = null;
        CameraRightBound.parent = null;
    }

    private void OnEnable()
    {
        GameLevelManager.instance.EndLevelHandler += EndLevelCameraEffect;
    }

    private void OnDisable()
    {
        GameLevelManager.instance.EndLevelHandler += EndLevelCameraEffect;
    }

    private void LateUpdate()
    {
        if (!GameLevelManager.instance.EndedLevel)
        {
            if (camTarget.transform.position.x > CameraLeftBound.position.x && camTarget.transform.position.x < CameraRightBound.transform.position.x)
            {
                float xPos = Mathf.Lerp(t.position.x, camTarget.position.x + camOffsets.x, Time.deltaTime * interpolationRate.x);
                float yPos = Mathf.Lerp(t.position.y, camTarget.position.y + camOffsets.y, Time.deltaTime * interpolationRate.y);
                float zPos = Mathf.Lerp(t.position.z, camTarget.position.z + camOffsets.z, Time.deltaTime * interpolationRate.z);

                t.position = new Vector3(xPos, yPos, t.position.z);
            }
        }
    }

    public void EndLevelCameraEffect()
    {
        IEnumerator RepositionCamera()
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += .001f;
                t.position = Vector3.Lerp(t.position, camTarget.position, progress);
                t.forward = Vector3.Lerp(t.forward, camTarget.position - transform.position, progress);
                yield return new WaitForSeconds(.0001f);
            }
        }
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(RepositionCamera());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CameraLeftBound.position, 3f);
        Debug.DrawLine(CameraLeftBound.position, CameraLeftBound.position + Vector3.up * 500, Color.red);
        Debug.DrawLine(CameraLeftBound.position, CameraLeftBound.position + Vector3.down * 500, Color.red);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(CameraRightBound.position, 3f);
        Debug.DrawLine(CameraRightBound.position, CameraRightBound.position + Vector3.up * 500, Color.blue);
        Debug.DrawLine(CameraRightBound.position, CameraRightBound.position + Vector3.down * 500, Color.blue);
    }
}