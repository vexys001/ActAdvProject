using UnityEngine;

public class Activator : MonoBehaviour
{
    //Common
    [SerializeField] protected GameObject _target = null;

    [SerializeField] private Material _objectMaterial;
    [SerializeField] protected Material _pressedObjectMaterial;
    protected Renderer _objectRenderer = null;

    //Optional /  case by case
    [SerializeField] protected string _colliderTag = null;

    private void Awake()
    {
        
        _objectRenderer = gameObject.GetComponent<Renderer>();
        _objectMaterial = _objectRenderer.material;
        //_objectRenderer.material.color = _objectColor;
    }

    protected void OnOff()
    {
        if (_target.activeSelf == true)
        {
            _target.SetActive(false);
            //_objectRenderer.material.color = _pressedObjectColor;
            _objectRenderer.material = _pressedObjectMaterial;
        }
        else if (_target.activeSelf == false)
        {
            _target.SetActive(true);
            //_objectRenderer.material.color = _objectColor;
            _objectRenderer.material = _objectMaterial;
        }
    }

    void PlayAnimation()
    {
        //Play button or plate depress animation.
    }
}
