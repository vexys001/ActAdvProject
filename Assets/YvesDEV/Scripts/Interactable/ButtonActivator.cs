using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivator : Activator
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _colliderTag)
        {
            OnOff();
        }
    }
}
