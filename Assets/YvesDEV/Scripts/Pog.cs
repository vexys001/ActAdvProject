using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pog : MonoBehaviour
{
    [SerializeField]
    private string _currentState;
    [SerializeField]
    private Collider _collider;
    private Rigidbody _rb;
    public GameObject itself, stack;

    [Header("Shooting Vars")]
    public float Speed = 10f;
    public float Lifespan = 3f;
    public static float ShieldDuration = 5f;

    [Header("Info Vars")]
    PogScriptableObject PogSO;
    public bool IsKey;

    public SystemEnums.Partys belongsTo;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = "none";

        _collider = GetComponentInChildren<Collider>();
        _rb = GetComponentInChildren<Rigidbody>();
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

    #region Utility
    void EnableGravity()
    {
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void DisableGravity()
    {
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public string GetState()
    {
        return _currentState;
    }

    public void SetBelong(SystemEnums.Partys newBelong)
    {
        belongsTo = newBelong;
    }
    #endregion

    void RunStates()
    {
        if (_currentState == "none") StartIdle();

        if (_currentState.Equals("Idle")) Idle();
        else if (_currentState.Equals("Dropped")) Dropped();
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
        belongsTo = SystemEnums.Partys.None;

        itself.transform.rotation = Quaternion.identity;

        _collider.enabled = true;
        EnableGravity();
    }

    void Dropped()
    {
    }

    void StopDropped()
    {
        _currentState = "none";
        _collider.enabled = false;
        itself.transform.localPosition = Vector3.zero;

        DisableGravity();
    }
    #endregion

    #region Shoot State
    void StartShoot()
    {
        transform.SetParent(null);
        _currentState = "isShooting";

        _collider.enabled = true;

        Invoke("EndShoot", Lifespan);
    }

    void Shoot()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
    }

    void EndShoot()
    {
        _currentState = "none";
        StartDropped();
    }
    #endregion

    #region Shield State
    void StartShield(GameObject pStack)
    {
        stack = pStack;
        //Remove this maybe
        //transform.SetParent(null);
        _currentState = "isShielding";
        transform.position += Vector3.forward * 2;

        _collider.enabled = true;

        Invoke("EndShield", ShieldDuration);
    }

    void Shield()
    {
        transform.RotateAround(stack.transform.position, Vector3.up, 1f);
    }

    void EndShield()
    {
        stack.GetComponent<StackObject>().AddPog(gameObject);

        _collider.enabled = false;

        transform.localPosition = Vector3.zero;
        _currentState = "none";
    }

    private void CollisionDetected(Collision collision)
    {
        Debug.Log("Entered collision");
        if (belongsTo == SystemEnums.Partys.Ally && !collision.gameObject.CompareTag("Player"))
        {
            if (_currentState == "isShooting")
            {
                CancelInvoke("EndShoot");
                EndShoot();
            }
            if (_currentState == "isShielding")
            {
                CancelInvoke("EndShield");
                StartDropped();
            }
        }
    }
    #endregion
}
