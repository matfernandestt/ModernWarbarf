using System.Collections;
using UnityEngine;

public class TransparencyDetector : MonoBehaviour
{
    [SerializeField] private MeshRenderer targetMaterial;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<BaseMovement>();

        if (player != null)
        {
            StartCoroutine(TransparencyOn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<BaseMovement>();

        if (player != null)
        {
            StartCoroutine(TransparencyOff());
        }
    }

    private IEnumerator TransparencyOn()
    {
        float progress = 0;
        Color target = targetMaterial.material.color;
        while (progress < 1)
        {
            progress += .01f;
            target = Color.Lerp(targetMaterial.material.color, new Color(target.r, target.g, target.b, progress), progress);
            targetMaterial.material.color = target;
            yield return new WaitForSeconds(.01f);
        }
    }

    private IEnumerator TransparencyOff()
    {
        float progress = 1;
        Color target = targetMaterial.material.color;
        while (progress > 0)
        {
            progress -= .01f;
            target = Color.Lerp(targetMaterial.material.color, new Color(target.r, target.g, target.b, progress), 1 - progress);
            targetMaterial.material.color = target;
            yield return new WaitForSeconds(.01f);
        }
    }
}
