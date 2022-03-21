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
    public GameObject _slimeModel;
    private Vector3 _topPogOffset;

    [SerializeField]
    private float _acceleration = 6;
    [SerializeField]
    private float _maxSpeed = 6;
    public float gravity = -9.81f;
    public float _jumpForce = 3;
    public bool hasJumped = false;
    public bool goingDown = false;

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

    [Header("Sounds")]
    public AudioClip ShootingAClip;
    public AudioClip JumpingAClip;
    public AudioClip LandingAClip;
    private AudioSource _audioSource;

    [Header("Other")]
    private Vector3 _respawnPosition;

    [Header("Debugging")]
    public bool DEBUG;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<BoxCollider>();
        _stack = GetComponentInChildren<StackObject>();

        //bottomGO = _stack.lastPogGO;

        _topPogOffset = new Vector3(0, 0.032f, 0);

        groundMask = LayerMask.NameToLayer("Ground");

        thirdPersonCamera.SetActive(true);
        aimCamera.SetActive(false);
        theCursor.SetActive(false);
        _audioSource = GetComponent<AudioSource>();

        _respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            thirdPersonCamera.SetActive(false);
            aimCamera.SetActive(true);
            theCursor.SetActive(true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            thirdPersonCamera.SetActive(true);
            aimCamera.SetActive(false);
            theCursor.SetActive(false);

            _stackHolder.transform.rotation = Quaternion.identity;
        }

        if (Input.GetButton("Fire2"))
        {
            _stackHolder.transform.rotation = cam.transform.rotation;
            //_stackHolder.transform.rotation = new Quaternion(0, cam.transform.rotation.y, cam.transform.rotation.z, cam.transform.rotation.w);

            if (Input.GetButtonDown("Fire1") && _stack.pogCount > 1)
            {
                _audioSource.PlayOneShot(ShootingAClip);
                _stackHolder.SendMessage("ShootPog");
                ChangeCollider(false);
            }
        }

        Inputs();
    }

    private void FixedUpdate()
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
            /*if (Input.GetButtonDown("Fire1"))
            {
                _audioSource.PlayOneShot(ShootingAClip);
                _stackHolder.SendMessage("ShootPog");
                ChangeCollider(false);
            }*/

            if (Input.GetButtonDown("Shield"))
            {
                _stackHolder.SendMessage("ShieldPog");
                _audioSource.PlayOneShot(ShootingAClip);
                ChangeCollider(false);
            }

            /*if (Input.GetKeyDown(KeyCode.Y))
            {
                _audioSource.PlayOneShot(LandingAClip);
                _stackHolder.SendMessage("DropPog", StackObject.Positions.Top);
                ChangeCollider(false);
            }*/

        }

        /*if (Input.GetKeyDown(KeyCode.T) && _stack.pogCount > 5)
        {
            _stackHolder.SendMessage("RemoveXNonKeys", 5);
        }*/

        if (FindGround().Length >= 1)
        {
            Move();

            if (hasJumped && _rb.velocity.y <= -1) goingDown = true;

            if (hasJumped && goingDown && _rb.velocity.y > -1)
            {
                hasJumped = false;
                _audioSource.PlayOneShot(LandingAClip);
            }

            if (Input.GetButtonDown("Jump") && !hasJumped) Jump();
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
            _stackHolder.SendMessage("AnimateStack", StackObject.AnimClips.Walk);
        }
        else if (direction.magnitude <= 0.1f) _stackHolder.SendMessage("AnimateStack", StackObject.AnimClips.Idle);
    }

    private void Jump()
    {
        goingDown = false;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _stackHolder.SendMessage("AnimateStack", StackObject.AnimClips.Jump);
        Invoke("DelayJump", 0.1f);
        _audioSource.PlayOneShot(JumpingAClip);
    }

    private void DelayJump()
    {
        hasJumped = true;
    }

    private void ChangeCollider(bool removeFromMiddle)
    {
        //_col.center += Vector3.down * 0.05f;
        if (!removeFromMiddle) _stack.transform.localPosition += _topPogOffset;
        else _stack.transform.localPosition -= _topPogOffset;

        _col.size = new Vector3(1, 0.064f * _stack.pogCount, 1);
        _slimeModel.transform.localPosition = new Vector3(0, 0.05f * _stack.pogCount, 0);

        bottomGO = _stack.lastPogGO;
    }

    private RaycastHit[] FindGround()
    {
        return Physics.RaycastAll(bottomGO.transform.position, Vector3.down / 20, groundMask);
    }

    private void Respawn()
    {
        transform.position = _respawnPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HeightChecker"))
        {
            int maxHeight = other.GetComponent<HeightChecker>().MaxHeight;
            if (_stack.pogCount > maxHeight)
            {
                int toRemove = _stack.pogCount - maxHeight;
                _stackHolder.SendMessage("RemoveXNonKeys", toRemove);
            }
        }
        else if (other.CompareTag("OutofBounds"))
        {
            Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pog"))
        {
            Pog collidedPog = collision.gameObject.transform.parent.GetComponent<Pog>();
            if (collidedPog.GetState() == "Dropped")
            {
                collidedPog.SetBelong(SystemEnums.Partys.Ally);
                _stackHolder.SendMessage("AddPog", collision.transform.parent.gameObject);
            }
            else if (collidedPog.GetState().Equals("isShooting") || collidedPog.GetState().Equals("isShielding"))
            {
                if (collidedPog.belongsTo == SystemEnums.Partys.Enemy)
                {
                    GetHit();
                }
            }
        }
    }

    private void GetHit()
    {
        Debug.Log("Getting hit Friendly");
        if (_stack.pogCount > 1)
        {
            _stackHolder.SendMessage("DropPog", StackObject.Positions.Top);
            ChangeCollider(false);
        }
        else Respawn();
    }

    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            //Debug.DrawRay(bottomGO.transform.position, Vector3.down, Color.red, 50f, false);
            Gizmos.DrawWireSphere(bottomGO.transform.position, 1f);
        }
    }
}
