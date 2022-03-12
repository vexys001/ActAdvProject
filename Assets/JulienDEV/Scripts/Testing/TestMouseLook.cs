using UnityEngine;

public class TestMouseLook : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivityX = 1000f;
    [SerializeField] private float _mouseSensitivityY = 1000f;

    [SerializeField] private Transform _playerBody;

    private float _xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivityX * Time.deltaTime;


        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivityY * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}