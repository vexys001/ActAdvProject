using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Control")]
    Rigidbody _rb;
    BoxCollider _col;
    StackObject _stack;
    public GameObject _stackHolder;

    public float speed = 6;
    public float gravity = -9.81f;
    public float _jumpForce = 3;
    public Vector3 velocity;

    public LayerMask groundMask;

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    [Header("Camera")]
    public Transform cam;
    public GameObject thirdPersonCamera;
    public GameObject aimCamera;
    public GameObject theCursor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<BoxCollider>();
        _stack = GetComponentInChildren<StackObject>();

        groundMask = LayerMask.NameToLayer("Ground");

        thirdPersonCamera.SetActive(true);
        aimCamera.SetActive(false);
        theCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
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

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O)) ChangeCollider();

        if (_stack.pogCount > 1)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeCollider();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeCollider();
                Invoke("ChangeCollider", Pog.ShieldDuration);
            }

        }
    }

    private void LateUpdate()
    {
        if (IsGrounded())
        {
            Move();
            Jump();
        }
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

    private void ChangeCollider()
    {
        _col.center += Vector3.down * 0.05f;

        _col.size = new Vector3(1, 0.1f * _stack.pogCount, 1);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundMask);
    }
}
