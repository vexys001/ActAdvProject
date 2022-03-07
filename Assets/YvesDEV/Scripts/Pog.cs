using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pog : MonoBehaviour
{
    string _currentState;
    public GameObject itself, stack;

    [Header("Shooting Vars")]
    public float Speed = 10f;
    public float Lifespan = 3f;
    public static float ShieldDuration = 5f;

    [Header("Info Vars")]
    PogScriptableObject PogSO;
    public bool IsKey;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = "none";
    }

    public void SetScriptable(PogScriptableObject SO)
    {
        PogSO = SO;
        IsKey = PogSO.IsKey;
        itself.GetComponent<MeshRenderer>().material = PogSO.Material;
    }

    // Update is called once per frame
    void Update()
    {
        RunStates();
    }

    void RunStates()
    {
        if (_currentState == "none") StartIdle();

        if (_currentState.Equals("Idle")) Idle();
        else if (_currentState.Equals("Dropped")) Idle();
        else if (_currentState.Equals("isShooting")) Shoot();
        else if (_currentState.Equals("isShielding")) Shield();
    }
    #region Idle State
    void StartIdle()
    {
        _currentState = "Idle";
    }

    void Idle()
    {

    }

    void StopIdle()
    {
        _currentState = "none";
    }
    #endregion

    #region Dropped State
    void StartDropped()
    {
        transform.SetParent(null);
        _currentState = "Dropped";
    }

    void Dropped()
    {

    }

    void StopDropped()
    {
        _currentState = "none";
    }
    #endregion

    #region Shoot State
    void StartShoot()
    {
        transform.SetParent(null);
        _currentState = "isShooting";
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
    #endregion

    #region Shield State
    void StartShield(GameObject pStack)
    {
        stack = pStack;
        transform.SetParent(null);
        _currentState = "isShielding";
        transform.position += Vector3.forward;
        Invoke("EndShield", ShieldDuration);
    }

    void Shield()
    {
        transform.RotateAround(stack.transform.position, Vector3.up, 1f);
    }

    void EndShield()
    {
        stack.GetComponent<StackObject>().AddPog(gameObject);
        transform.localPosition = Vector3.zero;
        _currentState = "none";
    }
    #endregion
}
