using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _camera = null;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private Vector3 _rotationStart = Vector3.zero;

    private void Awake()
    {
        _camera.rotation = Quaternion.Euler(_rotationStart);
    }

    private void LateUpdate()
    {
        _camera.position = transform.position + _offset;
        Look();
    }

    private void Look()
    {
        float h = Input.GetAxisRaw("Mouse X");
        float v = Input.GetAxisRaw("Mouse Y");

        //Camera rotates around player.
    }
}
