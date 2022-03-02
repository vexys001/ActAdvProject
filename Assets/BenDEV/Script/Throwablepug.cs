using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwablepug : MonoBehaviour
{
    public float m_Speed = 10f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)
    

    void Start()
    {
      
    }
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * m_Speed;
        Destroy(gameObject, m_Lifespan);
    }
}
