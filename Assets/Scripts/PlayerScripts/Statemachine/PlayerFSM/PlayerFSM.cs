using System.Collections;
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
    public JumpState jumpState;
    public RunState runState;

    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject aimReticle;
    public GameObject healthBar;

    // debug text
    public string text;
    public Vector3 angles;
    public float angle;
    public bool canShoot;

    [SerializeField]
    float jumpHeight;
    public GameObject groundCheck;

    public GameObject fireballPrefab;
    public Vector3 targetTransform;
    public Transform fireballSpawn;

    public float rotationPower;
    Vector3 velocity;

    public Transform followTransform;
    public Animator anim;
    private CharacterController cc;

    private void Start()
    {
        idleState = new IdleState();
        aimState = new AimState();
        runState = new RunState();
        jumpState = new JumpState();

        //anim.SetFloat("AnimSpeed", 2);

        canShoot = true;

        text = "";  // clear debug text

        lastState = null;
        // this is the inital state
        ChangeState(idleState);

        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
        
        //Debug.Log(cc.isGrounded);
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

    public void AimDownSights()
    { 
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("Aiming");
            mainCamera.SetActive(false);
            aimCamera.SetActive(true);

            //Allow time for the camera to blend before enabling the UI
            //StartCoroutine(ShowReticle());
        }
        else
        {
            //Debug.Log("not aiming");
            mainCamera.SetActive(true);
            aimCamera.SetActive(false);
            //aimReticle.SetActive(false);
        }
    }
  /*  IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
        aimReticle.SetActive(true);
    } */

    public void ShootFireBall()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //RaycastHit hit;
            canShoot = false;
            //if(Physics.Raycast(ray, out hit))
            {
                //targetTransform = hit.point;
                Invoke("InstantiateFireBall", 0.1f);
                print("fired ray");

                //InstantiateFireBall();
                //Debug.Log($"Hit: {hit.collider.name}");
            }
            //InstantiateFireBall();
        }
    }
    public void InstantiateFireBall()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawn.position, Quaternion.identity);
        canShoot = true;
    }
    public void InitDebugText()
    {
        string lastStateText;
        string currentStateText = currentState.ToString();

        if (lastState != null)
            lastStateText = lastState.ToString();
        else
            lastStateText = "null";
        text = $"Current State = {currentStateText}\nLast state was {lastStateText}";
    }
    public void MovementCalculationAndCamera()
    {
        //Rotate the Follow Target transform based on the input
        followTransform.transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * rotationPower, Vector3.up);

        followTransform.transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse Y") * rotationPower, -Vector3.right);

        angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        angle = followTransform.transform.localEulerAngles.x;
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

        movement = transform.rotation * movement * moveSpeed;
        velocity.x = movement.x;
        velocity.z = movement.z;

        //Debug.Log(velocity.x);
        //Debug.Log(velocity.y);

        //anim.SetFloat("x", velocity.x);
       // anim.SetFloat("y", velocity.z);


        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    public bool CheckForMove()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //print("mag=" + movement.magnitude);
        if( movement.magnitude > 0.1f )
        {
            return true;
        }
        return false;

    }
    public void AnimateMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y);

        float mag = direction.magnitude;
        //print(mag);
        if(CheckForMove())
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
        }
        else
        {
            anim.SetFloat("x", 0);
            anim.SetFloat("y", 0);
        }
    }

    public void Jump()
    {
        RaycastHit hit;
        float gravity = -9.81f;
        Vector3 rayOffset = new Vector3(0, 0.5f, 0);
        AnimateJump();

        //Debug.Log(velocity.y);
        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            //anim.SetBool("jumping", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, 0.65f))
        {
            if(hit.collider.tag == "Ground")
            {
                //Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.red);
                velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
               //anim.SetBool("jumping", true);
                Debug.Log("Jumping");
            }
            else
            {
                Debug.Log("not grounded");
            }
        }
        Debug.DrawRay(groundCheck.transform.position, Vector3.down * 0.5f, Color.red);
        velocity.y += gravity * Time.deltaTime;
        //cc.Move(velocity * Time.deltaTime);
    }
    public void AnimateJump()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, 0.65f))
        {
            anim.SetBool("jumping", false);
        }
        else
        {
            anim.SetBool("jumping", true);
        }
    }
    public void ApplyGravity()
    {
        float gravity = -9.81f;

        //Debug.Log(velocity.y);
        velocity.y += gravity * Time.deltaTime;
    }
    public void MoveCC()
    {
        cc.Move(velocity * Time.deltaTime);
    }

    // checks to see if we touch any game objects //
    // Is apart of the inventory system //

}
