using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float health;

    [SerializeField]
    float jumpHeight;

    float horizontalInput;
    float verticalInput;
    public float rotationPower;
    Vector3 velocity;

    public Transform followTransform;
    private CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAndCamera();
        Jump();
    }

    private void FixedUpdate()
    {
        
        //Jump();
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
        else if (angle < 180 && angle > 80)
        {
            angles.x = 80;
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

        if(IsGrounded() && velocity.y < 0)
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

        if(Physics.Raycast(transform.position, down, 0.1f))
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
