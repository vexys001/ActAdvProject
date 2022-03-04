using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pog : MonoBehaviour
{
    string currentState;
    public GameObject nextPog, stack;

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
        if (currentState.Equals("isShielding")) Shield();
    }

    void StartShoot()
    {
        transform.SetParent(null);
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

    void StartShield(GameObject pStack)
    {
        stack = pStack;
        transform.SetParent(null);
        currentState = "isShielding";
        transform.position += Vector3.forward;
        Invoke("EndShield", 5f);
    }

    void Shield()
    {
        transform.RotateAround(stack.transform.position, Vector3.up, 1f);
    }

    void EndShield()
    {
        stack.SendMessage("AddPog", gameObject);
        Destroy(gameObject);
    }
}
