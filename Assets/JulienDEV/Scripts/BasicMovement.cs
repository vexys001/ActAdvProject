using UnityEngine;

public class BasicMovement : MonoBehaviour
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

    //[Header("Hidden")]
    private float _gCheckModifier = 0.9f;

    private void Awake()
    {
        _rb = _player.GetComponent<Rigidbody>();
        _col = _player.GetComponent<CapsuleCollider>();
    }

    private void LateUpdate()
    {
        if (IsGrounded()) { Movement(); };
        Jump();
        UltraGravity();
    }

    private void Movement()
    {
        //Change to movement relative to camera.

        float h = Input.GetAxis("Horizontal") * _speed;
        float v = Input.GetAxis("Vertical") * _speed;

        Vector3 rbMovement = new Vector3(h * _speed, 0f, v * _speed) * Time.deltaTime;

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
}
