using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotActivator : Activator
{
    [SerializeField] private bool _buttonOneShotPressed = false;
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _colliderTag && !_buttonOneShotPressed)
        {
            OnOff();
            _buttonOneShotPressed = true;
        }
    }
}
