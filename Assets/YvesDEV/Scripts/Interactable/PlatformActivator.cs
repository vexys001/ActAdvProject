using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : Activator
{
    private bool _buttonOneShotPressed = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _colliderTag && !_buttonOneShotPressed)
        {
            ActivatePlatform();
            _buttonOneShotPressed = true;
        }
    }

    private void ActivatePlatform()
    {
        _target.SendMessage("ActivatePlat");
        _objectRenderer.material.color = _pressedObjectColor;
    }
}
