using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private float _moveRange = 0f;
    [SerializeField] private float _moveSpeed = 0f;

    private enum _axis {x, y, z};
    [SerializeField] private _axis _Axis;

    private float _position = 0f;
    private Transform _tr = null;
    private float _mv = 0f;

    private void Awake()
    {
        _tr = gameObject.transform;
        _mv = _moveSpeed;
    }

    private void Update()
    {
        if (_position * (_mv / Mathf.Abs(_mv)) <= -_moveRange)
        {
            _mv = -_mv;
        }
        else if (_position * (_mv / Mathf.Abs(_mv)) >= _moveRange)
        {
            _mv = -_mv;
        }

        switch (_Axis)
        {
            case _axis.x:
                _tr.position = new Vector3(_tr.position.x + _mv, _tr.position.y, _tr.position.z);
                _position += _mv;
                break;
            case _axis.y:
                _tr.position = new Vector3(_tr.position.x, _tr.position.y + _mv, _tr.position.z);
                _position += _mv;
                break;
            case _axis.z:
                _tr.position = new Vector3(_tr.position.x, _tr.position.y, _tr.position.z + _mv);
                _position += _mv;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
