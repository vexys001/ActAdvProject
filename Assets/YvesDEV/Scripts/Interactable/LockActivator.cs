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
            Pog pogObject = collision.gameObject.GetComponentInParent<Pog>();
            if(pogObject.KeyColor == _requiredColor)
            {
                _target.SendMessage("RemovedLock");
                Destroy(collision.transform.parent.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
