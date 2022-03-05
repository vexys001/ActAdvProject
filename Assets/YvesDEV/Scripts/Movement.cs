using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Control")]
    //public CharacterController controller;
    Rigidbody _rb;
    private Collider _col = null;

    public Transform cam;

    public float speed = 6;
    public float gravity = -9.81f;
    public float _jumpForce = 3;
    public Vector3 velocity;

    private bool isGrounded;
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    [Header("Control")]
    public GameObject thirdPersonCamera;
    public GameObject aimCamera;
    public GameObject theCursor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        thirdPersonCamera.SetActive(true);
        aimCamera.SetActive(false);
        theCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //****************************JUMPING & GRAVITY************************
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        /*if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = jumpHeight;
        }

        velocity.y += gravity * Time.deltaTime;
        _rb.AddForce(velocity);*/
        //controller.Move(velocity * Time.deltaTime);


        //*******************MOVEMENT************************
        /*if ((!Input.GetButton("Fire2")) /* || Mathf.Round(Input.GetAxisRaw("Fire2")) < 0))
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                //controller.Move(moveDir.normalized * speed * Time.deltaTime);
                _rb.AddForce(moveDir);
            }
        }*/

        // bla bla bla something about health and number of pug in an array or something
        if (Input.GetButton("Fire2")) /*|| Mathf.Round(Input.GetAxisRaw("Fire2")) < 0)*/
        {
            thirdPersonCamera.SetActive(false);
            aimCamera.SetActive(true);
            theCursor.SetActive(true);
        }
        else
        {
            thirdPersonCamera.SetActive(true);
            aimCamera.SetActive(false);
            theCursor.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (IsGrounded())
        {
            Debug.Log("Is Grounded");
            Move();
            Jump();
        }
        else Debug.Log("Is not Grounded");
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
            _rb.AddForce(moveDir);
        }
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        //return false;
        /*Debug.Log("Look for ground");*/
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        Debug.Log(Physics.Raycast(transform.position, Vector3.down, LayerMask.NameToLayer("Ground")));
        return Physics.Raycast(transform.position, Vector3.down, LayerMask.NameToLayer("Ground"));
        //return false;
        //return Physics.CheckCapsule(_col.bounds.center, new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z), _col.radius * _gCheckModifier, _groundLayers);
    }
}
