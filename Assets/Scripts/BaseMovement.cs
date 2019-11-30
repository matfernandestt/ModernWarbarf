using System.Collections;
using UnityEngine;
using Rewired;
using UnityEngine.Events;

public class BaseMovement : MonoBehaviour
{
    protected Rewired.Player input;

    public UnityAction ActionAttackNormal;
    public UnityAction ActionAttackSpecial;
    public UnityAction ActionTaunt;

    public AnimationCurve jumpCurve;

    [SerializeField] private float jumpSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity;

    [SerializeField] protected CharacterController controller;
    [SerializeField] protected Animator anim;

    private float hSpeed;
    private float vSpeed;
    private float zPosInitial;
    private bool stopInputs;
    private bool stopGravity;

    private float characterMass = 5f;
    private Vector3 impactForce = Vector3.zero;

    public bool IsGrounded
    {
        get => controller.isGrounded;
        set
        {
            if (IsGrounded)
            {
                Debug.Log("Landed!");
            }
        }
    }

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);
        zPosInitial = transform.position.z;
        anim.SetBool("IsGrounded", false);
    }

    private void Update()
    {
        Movement();
        Jump();
        ApplyMovement();

        if (!stopInputs)
        {
            InputCaptureActionAttackNormal();
            InputCaptureActionAttackSpecial();
            InputCaptureActionTaunt();
        }
    }

    private void InputCaptureActionAttackNormal()
    {
        if (input.GetButtonDown("ActionAttackNormal"))
        {
            ActionAttackNormal?.Invoke();
        }
    }

    private void InputCaptureActionAttackSpecial()
    {
        if (input.GetButtonDown("ActionAttackSpecial"))
        {
            Debug.Log("pressed special");
            ActionAttackSpecial?.Invoke();
        }
    }

    private void InputCaptureActionTaunt()
    {
        if (input.GetButtonDown("Taunt") && hSpeed == 0)
        {
            ActionTaunt?.Invoke();
        }
    }

    private void Movement()
    {
        hSpeed = GetHorizontalAxis();
        CheckRotation();
        hSpeed *= movementSpeed;
        AnimationChecks();
    }

    private void CheckRotation()
    {
        if (!stopInputs)
        {
            if (hSpeed > 0)
                transform.eulerAngles = transform.TransformDirection(new Vector3(0, 0, 0));
            else if (hSpeed < 0)
                transform.eulerAngles = transform.TransformDirection(new Vector3(0, 180, 0));
        }
    }

    public Quaternion SideRotated()
    {
        return transform.rotation;
    }

    private void Jump()
    {
        if (GetJumpButton() && controller.isGrounded && !stopInputs)
        {
            vSpeed = jumpSpeed;
        }

        if (!stopGravity && !controller.isGrounded)
            vSpeed -= gravity * Time.deltaTime;
    }

    private void ApplyMovement()
    {
        if (controller.enabled)
        {
            if (impactForce.magnitude > .2f)
            {
                controller.Move(impactForce * Time.deltaTime);
            }

            impactForce = Vector3.Lerp(impactForce, Vector3.zero, Time.deltaTime * 3);
            if (stopInputs)
            {
                hSpeed = 0;
                vSpeed -= (gravity * Time.deltaTime) / 2;
            }

            controller.Move(new Vector3(hSpeed, vSpeed, 0) * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosInitial);
    }

    private void AnimationChecks()
    {
        float t = Mathf.Lerp(anim.GetFloat("BasicMovement"), Mathf.Abs(GetHorizontalAxis()), Time.deltaTime * 5);
        anim.SetFloat("BasicMovement", t);
        
        if (!controller.isGrounded && Mathf.Abs(GetHorizontalAxis()) != 0)
        {
            anim.SetBool("IsGrounded", true);
        }
        else
        {
            anim.SetBool("IsGrounded", controller.isGrounded);
        }
    }

    public void AddImpact(Vector3 direction, float force)
    {
        vSpeed = 0;
        direction.Normalize();
        if (direction.y < 0)
            direction.y = -direction.y;
        impactForce += direction.normalized * force / characterMass;
        PlayerLoseControlUntilGrounded();
        PlayerStopGravity(.5f);
    }

    public void DirectionalDash(Vector3 direction, float force)
    {
        vSpeed = 0;
        direction.Normalize();
        impactForce += direction.normalized * force / characterMass;
    }

    #region Inputs
    private float GetHorizontalAxis()
    {
        return input.GetAxisRaw("MoveHorizontal");
    }

    private float GetVerticalAxis()
    {
        return input.GetAxisRaw("MoveVertical");
    }

    private bool GetJumpButton()
    {
        return input.GetButton("Jump");
    }

    public void PlayerLoseControlUntilGrounded() 
    {
        StartCoroutine(StopInputs());
    }

    public void PlayerLoseGainControl()
    {
        controller.enabled = stopInputs;
        stopInputs = !stopInputs;
    }

    public void PlayerStopGravity(float duration)
    {
        StartCoroutine(StopGravity(duration));
    }

    private IEnumerator StopInputs()
    {
        stopInputs = true;
        yield return new WaitForSeconds(.5f);
        yield return new WaitWhile(() => !controller.isGrounded);
        stopInputs = false;
    }

    private IEnumerator StopGravity(float duration)
    {
        stopGravity = true;
        yield return new WaitForSeconds(duration);
        stopGravity = false;
    }
#endregion
}
 