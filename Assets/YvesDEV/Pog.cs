using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pog : MonoBehaviour
{
    GameObject nextPog;
    string currentState;
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

    }

    void EndShoot()
    {
        Destroy(this.gameObject);
    }

    
}
