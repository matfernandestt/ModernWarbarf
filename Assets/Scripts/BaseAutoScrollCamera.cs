using System;
using UnityEngine;

public class BaseAutoScrollCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (GameLevelManager.instance.EndedLevel) return;
        
        transform.Translate(-Vector3.right * speed);
    }
}
