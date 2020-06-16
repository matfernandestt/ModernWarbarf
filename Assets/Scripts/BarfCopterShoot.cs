using UnityEngine;

public class BarfCopterShoot : MonoBehaviour
{
    [SerializeField] private float speed;
    
    void Update()
    {
        transform.Translate(Vector3.right * speed);
    }
}
