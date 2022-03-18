using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateActivator : Activator
{
    [SerializeField] private int _numOfNeededPogs = 3;
    private bool _isPressurePlatePressed = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _colliderTag && _isPressurePlatePressed == false)
        {
            if (collision.gameObject.GetComponentInChildren<StackObject>().pogCount >= _numOfNeededPogs)
            {
                OnOff();
                _isPressurePlatePressed = true;
            }

        }
    }
}
