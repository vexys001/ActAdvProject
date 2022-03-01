using UnityEngine;

public class BasicMovement2 : MonoBehaviour
{
    [SerializeField] private GameObject _player = null;
    private Rigidbody _rb = null;
    private CapsuleCollider _col = null;

    [Header("Player Attributes")]
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _jumpForce = 0f;
    [SerializeField] private float _ultraGrav = 0f;

    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayers;
    [Header("Camera")]
    public Camera MainCamera;
    public Transform CameraRoot;
    public float CameraSensitivityX = 350;
    public float CameraSensitivityY = 350;
    public float MaxDownwardAngle = 20;
    public float MaxUpwardAngle = -60;

    private float _targetRotationH = 0;
    private float _targetRotationV = 0;
    private float _maxCameraDistance;
    private Vector3 _originalCameraLocalPosition;

    //[Header("Hidden")]
    private float _gCheckModifier = 0.9f;

    private void Awake()
    {
        _rb = _player.GetComponent<Rigidbody>();
        _col = _player.GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MainCamera.transform.LookAt(CameraRoot);
        _originalCameraLocalPosition = MainCamera.transform.localPosition;
        _maxCameraDistance = Vector3.Distance(CameraRoot.transform.position, MainCamera.transform.position);
    }
    private void Update()
    {
        if (Time.timeScale != 0)
        {
            CameraCheck();
        }
        this.gameObject.transform.rotation = CameraRoot.transform.rotation; //à retravailler pour pas que le character bend par en avant ou arrière
    }
    private void LateUpdate()
    {
        if (IsGrounded())
        {
            Movement();
            Jump();
        };

        UltraGravity();
    }

    private void Movement()
    {
        //Change to movement relative to camera.

        float h = Input.GetAxis("Horizontal") * _speed;
        float v = Input.GetAxis("Vertical") * _speed;

        Vector3 rbMovement = new Vector3(h, 0f, v).normalized * _speed * Time.deltaTime;


        _rb.AddRelativeForce(rbMovement, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void UltraGravity()
    {
        _rb.AddForce(Vector3.down * _ultraGrav * Time.deltaTime, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(_col.bounds.center, new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z), _col.radius * _gCheckModifier, _groundLayers);
    }
    void CameraCheck()
    {
        //Calculate Camera Rotation based of mouse movement
        _targetRotationH += Input.GetAxis("Camera X") * CameraSensitivityX * Time.deltaTime;
        _targetRotationV += Input.GetAxis("Camera Y") * CameraSensitivityY * Time.deltaTime;

        //Clamp Vertical Rotation
        _targetRotationV = Mathf.Clamp(_targetRotationV, MaxUpwardAngle, MaxDownwardAngle);

        CameraRoot.transform.rotation = Quaternion.Euler(_targetRotationV, _targetRotationH, 0.0f);
        //Check for Environement Collision
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 dir = MainCamera.transform.position - CameraRoot.transform.position;
        bool collided = Physics.Raycast(CameraRoot.transform.position, dir.normalized, out hit, _maxCameraDistance, layerMask);
        if (collided && hit.collider.name != "Player")
        {
            MainCamera.transform.localPosition = CameraRoot.transform.InverseTransformPoint(hit.point);
        }
        else
        {
            MainCamera.transform.localPosition = _originalCameraLocalPosition;
        }
    }
}
