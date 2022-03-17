using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogCollisions : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SendMessageUpwards("CollisionDetected", collision);
    }
}
