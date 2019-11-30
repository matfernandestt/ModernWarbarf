using UnityEngine;

public class BaseMovementAI : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float gravity;

    private float hSpeed;
    private float vSpeed;
    private float runningSpeedMultiplier = 1;
    private float zPosInitial;

    private bool running;
    public bool Running
    {
        get => running;
        set
        {
            running = value;
            if (Running)
                runningSpeedMultiplier = runningSpeed;
            else
                ResetSpeedMultiplier();
        }
    }

    private void Awake()
    {
        zPosInitial = transform.position.z;
    }

    public void Movement()
    {
        hSpeed = Mathf.Sin(Time.time) * runningSpeedMultiplier;
        CheckRotation();
        hSpeed *= movementSpeed;
    }

    public void CheckRotation()
    {
        if (hSpeed > 0)
            transform.eulerAngles = transform.TransformDirection(new Vector3(0, 0, 0));
        else if (hSpeed < 0)
            transform.eulerAngles = transform.TransformDirection(new Vector3(0, 180, 0));
    }

    public void ApplyMovement()
    {
        controller.Move(new Vector3(hSpeed, vSpeed, 0) * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, zPosInitial);

        if (!controller.isGrounded)
            vSpeed -= gravity * Time.deltaTime;
        ResetSpeedMultiplier();
        running = false;
    }

    public void Jump()
    {
        if (controller.isGrounded)
            vSpeed = jumpSpeed;
    }

    public void StartRunning()
    {
        Running = true;
    }

    public void StopRunning()
    {
        Running = false;
    }

    private void ResetSpeedMultiplier()
    {
        runningSpeedMultiplier = 1;
    }

    public static bool RandomBool()
    {
        int v = Random.Range(0, 2);
        return (v != 0);
    }
}
