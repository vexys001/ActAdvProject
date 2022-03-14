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
    private Vector3 _topPogOffset;

    [SerializeField]
    private float _acceleration = 6;
    [SerializeField]
    private float _maxSpeed = 6;
    public float gravity = -9.81f;
    public float _jumpForce = 3;
    public Vector3 velocity;

    [SerializeField]
    private GameObject bottomGO = null;
    public LayerMask groundMask;

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    [Header("Camera")]
    public Transform cam;
    public GameObject thirdPersonCamera;
    public GameObject aimCamera;
    public GameObject theCursor;

    [Header("Debugging")]
    public bool DEBUG;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<BoxCollider>();
        _stack = GetComponentInChildren<StackObject>();

        bottomGO = _stack.lastPogGO;

        _topPogOffset = new Vector3(0, 0.05f, 0);

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

        Inputs();

        // TODO: Remove those

        //END OF TODO
    }

    private void LateUpdate()
    {

    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _stackHolder.SendMessage("TempAddPog");
            ChangeCollider(false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _stackHolder.SendMessage("AddKeyPog");
            ChangeCollider(false);
        }
        if (_stack.pogCount > 1)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _stackHolder.SendMessage("ShootPog");
                ChangeCollider(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _stackHolder.SendMessage("ShieldPog");
                ChangeCollider(false);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _stackHolder.SendMessage("DropPog", StackObject.Positions.Top);
                ChangeCollider(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.T) && _stack.pogCount > 5)
        {
            _stackHolder.SendMessage("RemoveXNonKeys", 5);
        }

        if (IsGrounded().Length >= 1)
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

        if (direction.magnitude >= 0.1f && _rb.velocity.magnitude < _maxSpeed)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * _acceleration;
            _rb.AddForce(moveDir);
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void ChangeCollider(bool removeFromMiddle)
    {
        //_col.center += Vector3.down * 0.05f;
        if (!removeFromMiddle) _stack.transform.localPosition += _topPogOffset;
        else _stack.transform.localPosition -= _topPogOffset;

        _col.size = new Vector3(1, 0.1f * _stack.pogCount, 1);
        bottomGO = _stack.lastPogGO;
    }

    private RaycastHit[] IsGrounded()
    {
        return Physics.RaycastAll(bottomGO.transform.position, Vector3.down, groundMask);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HeightChecker"))
        {
            int maxHeight = other.GetComponent<HeightChecker>().MaxHeight;
            if(_stack.pogCount > maxHeight)
            {
                int toRemove = _stack.pogCount - maxHeight;
                _stackHolder.SendMessage("RemoveXNonKeys", toRemove);
            }   
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pog"))
        {
            Debug.Log("Colliding with a pog");
            _stackHolder.SendMessage("AddPog", collision.transform.parent.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            Debug.DrawRay(bottomGO.transform.position, Vector3.down, Color.red, 50f, false);
        }
    }
}
