using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pog : MonoBehaviour
{
    string currentState;
    GameObject nextPog;

    [Header("Shooting Vars")]
    public float Speed = 10f;
    public float Lifespan = 3f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = "none";
    }

    // Update is called once per frame
    void Update()
    {
        RunStates();
    }

    public void AssignNextPog(GameObject pNextPog)
    {
        nextPog = pNextPog;
    }

    void RunStates()
    {
        if (currentState.Equals("isShooting")) Shoot();
    }

    void StartShoot()
    {
        currentState = "isShooting";
    }

    void Shoot()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
        Destroy(gameObject, Lifespan);
    }

    void EndShoot()
    {
        Destroy(this.gameObject);
    }

    
}
