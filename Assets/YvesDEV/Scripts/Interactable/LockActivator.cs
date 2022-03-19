using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockActivator : Activator
{
    [SerializeField] private SystemEnums.KeyColors _requiredColor;

    private void Awake()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _colliderTag)
        {
            Debug.Log($"This pog {collision.gameObject.name} collided with the lock");
            Pog pogObject = collision.gameObject.GetComponentInParent<Pog>();
            if(pogObject.KeyColor == _requiredColor)
            {
                //OnOff();
                Destroy(gameObject);
                //_target.SendMessage("RemovedLock");
            }
        }
    }
}
