using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLockDoorActivator : MonoBehaviour
{
    public GameObject LocksHolder;
    private int lockNum;
    // Start is called before the first frame update
    void Start()
    {
        lockNum = LocksHolder.GetComponentsInChildren<LockActivator>().Length;
    }

    private void RemovedLock()
    {
        lockNum--;

        if (lockNum <= 0) Destroy(gameObject);
    }
}
