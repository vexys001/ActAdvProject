using UnityEngine;

public class Activator : MonoBehaviour
{
    //Common
    [SerializeField] protected GameObject _target = null;

    [SerializeField] private Color _objectColor = Color.green;
    [SerializeField] protected Color _pressedObjectColor = Color.red;
    protected Renderer _objectRenderer = null;

    //Optional /  case by case
    [SerializeField] protected string _colliderTag = null;

    private void Awake()
    {
        //_objectRenderer = gameObject.GetComponent<Renderer>();
        //_objectRenderer.material.color = _objectColor;
    }

    protected void OnOff()
    {
        if (_target.activeSelf == true)
        {
            _target.SetActive(false);
            //_objectRenderer.material.color = _pressedObjectColor;
        }
        else if (_target.activeSelf == false)
        {
            _target.SetActive(true);
            //_objectRenderer.material.color = _objectColor;
        }
    }

    void PlayAnimation()
    {
        //Play button or plate depress animation.
    }
}
