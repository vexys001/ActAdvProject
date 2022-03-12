using UnityEngine;

public class TestFPCC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb = null;

    [Header("Parameters")]
    [SerializeField] private float _moveSpeed = 12f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 3f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 _velocity;
    private bool _isGrounded;

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _velocity.y <= 0)
        {
            _velocity.y = -2f; //Could be 0.
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        _rb.AddForce(move * _moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;

        _rb.AddForce(_velocity * Time.deltaTime, ForceMode.VelocityChange);
    }
}