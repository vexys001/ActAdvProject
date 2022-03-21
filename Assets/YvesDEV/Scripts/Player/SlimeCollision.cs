using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCollision : MonoBehaviour
{
    public SystemEnums.Partys isA;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pog"))
        {

            Pog collidedPog = collision.gameObject.transform.parent.GetComponent<Pog>();

            if (collidedPog.GetState().Equals("isShooting") || collidedPog.GetState().Equals("isShielding"))
            {
                if (collidedPog.belongsTo != isA)
                {
                    SendMessageUpwards("GetHit");
                }
            }
        }
    }
}
