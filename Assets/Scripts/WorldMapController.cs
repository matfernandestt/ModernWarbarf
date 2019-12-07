using System;
using Rewired;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
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
    private float zSpeed;
    private float vSpeed;
    private bool stopInputs;
    private bool stopGravity;

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
        anim.SetBool("IsGrounded", false);
    }

    private void OnEnable()
    {
        ActionAttackSpecial += ActionButton;
    }

    private void OnDisable()
    {
        ActionAttackSpecial -= ActionButton;
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
        zSpeed = GetVerticalAxis();
        CheckRotation();
        hSpeed *= movementSpeed;
        zSpeed *= movementSpeed;
        AnimationChecks();
    }

    private void CheckRotation()
    {
        if (hSpeed != 0 || zSpeed != 0)
        {
            var dir = new Vector3(GetHorizontalAxis(), 0, GetVerticalAxis());
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15);
        }
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

    private void ActionButton()
    {
        string closestLevel = WorldMapHUD.instance.ClosestTouchedLevel;
        if (!string.IsNullOrEmpty(closestLevel))
        {
            SceneManager.LoadScene(closestLevel);
        }
    }

    private void ApplyMovement()
    {
        if (controller.enabled)
        {
            if (stopInputs)
            {
                hSpeed = 0;
                zSpeed = 0;
                vSpeed -= (gravity * Time.deltaTime) / 2;
            }

            controller.Move(new Vector3(hSpeed, vSpeed, zSpeed) * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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

    #region Inputs
    private float GetHorizontalAxis()
    {
        return input.GetAxisRaw("MoveHorizontal");
    }

    private float GetVerticalAxis()
    {
        return input.GetAxisRaw("MoveVertical");
    }

    private bool IsPressingMovement()
    {
        return input.GetButton("MoveHorizontal") || input.GetButton("MoveVertical");
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
