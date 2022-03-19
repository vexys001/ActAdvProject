using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockActivator : Activator
{
    [SerializeField] private SystemEnums.KeyColors _requiredColor;


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == _colliderTag)
        {
            Pog pogObject = collision.gameObject.GetComponentInChildren<Pog>();
            if(pogObject.KeyColor == _requiredColor)
            {
                //OnOff();
                Destroy(gameObject);
            }
        }
    }
}
