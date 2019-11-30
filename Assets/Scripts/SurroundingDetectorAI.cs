using System;
using UnityEngine;

public class SurroundingDetectorAI : MonoBehaviour
{
    [SerializeField] private BaseMovementAI AI_Controller;

    private void OnTriggerEnter(Collider other)
    {
        var knockbackable = other.GetComponent<IKnockbackable>();

        if (knockbackable != null)
        {
            knockbackable.AddImpact(GetRelativeDirection(transform.position, other.transform.position), 300);
        }

        if (other.CompareTag("Player") && !AI_Controller.Running)
        {
            AI_Controller.StartRunning();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && AI_Controller.controller.isGrounded && BaseMovementAI.RandomBool())
        {
            AI_Controller.Jump();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AI_Controller.StopRunning();
        }
    }

    public static Vector3 GetRelativeDirection(Vector3 thisPos, Vector3 otherPos)
    {
        Vector3 direction = new Vector3();
        direction = (otherPos - thisPos).normalized;
        return direction;
    }
}
