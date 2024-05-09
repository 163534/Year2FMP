using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface IState
{
    public void UpdateState();
    public void PhysicsUpdateState();
    public void OnEnterState(PlayerFSM sm);
    public void OnExitState();
}

public class PlayerFSM : MonoBehaviour
{
    // attach this script to your player or enemy object that requires a state machine
    public IState currentState, lastState;
    public IdleState idleState;
    public AimState aimState;
    

    // debug text
    public string text;

    public float health;

    [SerializeField]
    float jumpHeight;

    float horizontalInput;
    float verticalInput;
    public float rotationPower;
    Vector3 velocity;

    public Transform followTransform;
    private CharacterController cc;

    private void Start()
    {
        idleState = new IdleState();
        aimState = new AimState();

        text = "";  // clear debug text

        lastState = null;
        // this is the inital state
        ChangeState(idleState);

        health = 100;
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
            MovementAndCamera();
            Jump();
        }

    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsUpdateState();
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        lastState = currentState;
        currentState = newState;
        currentState.OnEnterState(this);
    }


    public IState GetState()
    {
        return currentState;
    }


    // Debug Text Draw - draws the string 'text' to the canvas
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, 1600f, 1600f));
        GUILayout.Label($"<color=white><size=24>{text}</size></color>");
        GUILayout.EndArea();
    }

    public void ExampleSharedMethod()
    {
        // this in an example method that can be shared by any state
    }

    public void InitDebugText()
    {
        string lastStateText;

        if (lastState != null)
            lastStateText = lastState.ToString();
        else
            lastStateText = "null";
        text = $"Current State = Idle\nLast state was {lastStateText}\nPress R to change to Run state\nPress I to change to Idle state";
    }
    void MovementAndCamera()
    {
        //Rotate the Follow Target transform based on the input
        followTransform.transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * rotationPower, Vector3.up);

        followTransform.transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse Y") * rotationPower, -Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;
        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 320)
        {
            angles.x = 320;
        }
        else if (angle < 180 && angle > 60)
        {
            angles.x = 60;
        }

        followTransform.transform.localEulerAngles = angles;

        //Movement speed of the player
        int moveSpeed = 5;

        // Takes in the horizontal and vertical input moves the character controller based on input * moveSpeed
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        cc.Move(transform.rotation * movement * Time.deltaTime * moveSpeed);

        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }
    void Jump()
    {

        float gravity = -9.81f;

        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            Debug.Log("Jumping");
        }
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
        //Debug.Log(velocity.y);
    }
    bool IsGrounded()
    {
        Vector3 down = transform.TransformDirection(-Vector3.up);

        if (Physics.Raycast(transform.position, down, 0.1f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 0.05f, Color.white);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 0.05f, Color.red);
            return false;
        }

    }
}
