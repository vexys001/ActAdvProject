using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?redirectedfrom=MSDN&view=net-6.0

    // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-6.0
    // https://docs.unity3d.com/ScriptReference/Experimental.GraphView.StackNode.html

    public GameObject samplePog;
    public PogScriptableObject[] PogDatas;
    public int pogCount;
    GameObject firstPogGO, lastPogGO;

    // Start is called before the first frame update
    void Start()
    {
        firstPogGO = this.transform.GetChild(0).gameObject;
        lastPogGO = firstPogGO;
        pogCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) AddPog();
        if (Input.GetKeyDown(KeyCode.O)) AddKeyPog();
        if (Input.GetKeyDown(KeyCode.Q) && pogCount > 1) ShootPog();
        if (Input.GetKeyDown(KeyCode.E) && pogCount > 1) ShieldPog();
    }

    void AddPog()
    {
        pogCount++;
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[0]);
    }

    void AddKeyPog()
    {
        Debug.Log("Spawning Key");
        pogCount++;
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[1]);
    }

    public void AddPog(GameObject pPog)
    {
        Debug.Log("Adde existing pog");
        pogCount++;

        Transform temp = lastPogGO.transform.GetChild(0);
        lastPogGO = pPog;

        lastPogGO.transform.SetParent(temp);
    }

    void ShootPog()
    {
        pogCount--;

        //Save first Pog gameobject temporarly
        var temp = firstPogGO;

        // Replace the first pog with the next Pog gameobject
        firstPogGO = firstPogGO.transform.GetChild(0).GetChild(0).gameObject;

        //Unparent the the stack for the top
        temp.transform.GetChild(0).DetachChildren();

        //Parent back the stack to the holder
        firstPogGO.transform.SetParent(transform);

        //Destroy TEMP
        //Destroy(temp);
        temp.SendMessage("StartShoot");
    }

    void ShieldPog()
    {
        pogCount--;

        //Save first Pog gameobject temporarly
        var temp = firstPogGO;

        // Replace the first pog with the next Pog gameobject
        firstPogGO = firstPogGO.transform.GetChild(0).GetChild(0).gameObject;

        //Unparent the the stack for the top
        temp.transform.GetChild(0).DetachChildren();

        //Parent back the stack to the holder
        firstPogGO.transform.SetParent(transform);

        //Destroy TEMP
        //Destroy(temp);
        temp.SendMessage("StartShield", gameObject);
    }
}
