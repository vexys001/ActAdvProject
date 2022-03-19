using UnityEngine;

public class Activator : MonoBehaviour
{
    private enum _modes { button, buttonOneShot, buttonSelfDestruct, pressurePlate };
    [SerializeField] private _modes _Modes;

    //Common
    [SerializeField] protected GameObject _target = null;

    [SerializeField] private Color _objectColor = Color.green;
    [SerializeField] protected Color _pressedObjectColor = Color.red;
    protected Renderer _objectRenderer = null;

    //Optional /  case by case
    [SerializeField] protected string _colliderTag = null;

    private void Awake()
    {
        _objectRenderer = gameObject.GetComponent<Renderer>();
        _objectRenderer.material.color = _objectColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (_Modes)
        {
            /*case _modes.button:
                if (collision.gameObject.tag == _colliderTag)
                {
                    OnOff();
                }
                break;
            */
            /*case _modes.buttonOneShot:
                if (collision.gameObject.tag == _colliderTag && _buttonOneShotPressed == false)
                {
                    OnOff();
                    _buttonOneShotPressed = true;
                }
                break;
            */
            /*case _modes.buttonSelfDestruct:
                if (collision.gameObject.tag == _colliderTag)
                {
                    OnOff();
                    Destroy(gameObject);
                }
                break;
            */
            /*case _modes.pressurePlate:
                //
                break;*/
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        switch (_Modes)
        {
            /*
            case _modes.button:
                //
                break;
            */
            /*
            case _modes.buttonOneShot:
                //
                break;
            
            case _modes.buttonSelfDestruct:
                //
                break;

            /*case _modes.pressurePlate:
                if (collision.gameObject.tag == _colliderTag && _isPressurePlatePressed == false)
                {
                    OnOff();
                    _isPressurePlatePressed = true;
                }
                break;*/
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (_Modes)
        {
            /*
            case _modes.button:
                //
                break;
            
            case _modes.buttonOneShot:
                //
                break;

            case _modes.buttonSelfDestruct:
                //
                break;

            /*case _modes.pressurePlate:
                OnOff();
                _isPressurePlatePressed = false;
                break;*/
        }
    }

    protected void OnOff()
    {
        if (_target.activeSelf == true)
        {
            _target.SetActive(false);
            _objectRenderer.material.color = _pressedObjectColor;
        }
        else if (_target.activeSelf == false)
        {
            _target.SetActive(true);
            _objectRenderer.material.color = _objectColor;
        }
    }

    void PlayAnimation()
    {
        //Play button or plate depress animation.
    }
}
