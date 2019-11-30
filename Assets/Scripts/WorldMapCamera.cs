using UnityEngine;

public class WorldMapCamera : MonoBehaviour
{
    public Transform camTarget;
    public Vector3 interpolationRate;
    public Vector3 camOffsets;

    private Transform t;

    private void Awake()
    {
        t = transform;
    }

    private void LateUpdate()
    {
        float xPos = Mathf.Lerp(t.position.x, camTarget.position.x + camOffsets.x, Time.deltaTime * interpolationRate.x);
        float yPos = Mathf.Lerp(t.position.y, camTarget.position.y + camOffsets.y, Time.deltaTime * interpolationRate.y);
        float zPos = Mathf.Lerp(t.position.z, camTarget.position.z + camOffsets.z, Time.deltaTime * interpolationRate.z);

        t.position = new Vector3(xPos, yPos, zPos);
    }
}
