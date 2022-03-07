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
    public GameObject firstPogGO, lastPogGO;

    [Header("Removal Specific")]
    public int ncountrdNonRemovblPogs;

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
        if (Input.GetKeyDown(KeyCode.Y) && pogCount > 1) DropPog(true);
        if (Input.GetKeyDown(KeyCode.T) && pogCount > 5) RemoveXNonKeys(5);
    }

    #region Adding to the stack
    void AddPog()
    {
        pogCount++;
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[0]);
    }

    void AddKeyPog()
    {
        pogCount++;
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[1]);
    }

    public void AddPog(GameObject pPog)
    {
        pogCount++;

        Transform temp = lastPogGO.transform.GetChild(0);
        lastPogGO = pPog;

        lastPogGO.transform.SetParent(temp);
    }

    #endregion

    #region Using pogs
    void ShootPog()
    {
        RemoveTopPog().SendMessage("StartShoot");
    }

    void ShieldPog()
    {
        RemoveTopPog().SendMessage("StartShield", gameObject);
    }

    void DropPog(bool RemoveFrmTop)
    {
        GameObject objToDrop = RemoveFrmTop ? RemoveTopPog() : RemoveMiddlePog();

        objToDrop.transform.position = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        objToDrop.SendMessage("StartDropped");
    }
    #endregion

    #region Removal from the stack

    GameObject RemoveTopPog()
    {
        pogCount--;

        //Save first Pog gameobject temporarly
        GameObject toRemove = firstPogGO;

        // Replace the first pog with the next Pog gameobject
        firstPogGO = firstPogGO.transform.GetChild(0).GetChild(0).gameObject;

        //Unparent the bottom the stack from the top
        toRemove.transform.GetChild(0).DetachChildren();

        //Parent back the stack to the holder
        firstPogGO.transform.SetParent(transform);

        return toRemove;
    }

    GameObject RemoveMiddlePog()
    {
        pogCount--;

        GameObject toRemove = firstPogGO, previous = null, next;

        for (int i = 0; i < ncountrdNonRemovblPogs; i++)
        {
            previous = toRemove;
            toRemove = toRemove.transform.GetChild(0).GetChild(0).gameObject;
        }

        next = toRemove.transform.GetChild(0).GetChild(0).gameObject;

        //Unparent the bottom of the stack
        toRemove.transform.GetChild(0).DetachChildren();

        //Parent back the bottom of the stack to the top
        next.transform.SetParent(previous.transform.GetChild(0));
        next.transform.localPosition = Vector3.zero;

        return toRemove;
    }

    void RemoveXNonKeys(int x)
    {
        ncountrdNonRemovblPogs = 0;

        //Get the First pog
        GameObject pogToRemove = firstPogGO;

        for (int i = 0; i < x;)
        {
            //If the pog is a key
            if (pogToRemove.GetComponent<Pog>().IsKey)
            {
                //Count the number of keys encountered
                ncountrdNonRemovblPogs++;

                //Get the next pog to remove
                pogToRemove = pogToRemove.transform.GetChild(0).GetChild(0).gameObject;
            }
            else
            {
                //IF IT WASNT A KEY
                Debug.Log(pogToRemove.name + " pog is not a key");
                i++;

                // If youve seen no keys yet
                if (ncountrdNonRemovblPogs == 0)
                {
                    //Drop the top pog
                    DropPog(true);

                    //Get the new Top pog
                    pogToRemove = firstPogGO;

                    gameObject.SendMessageUpwards("ChangeCollider", false);
                }
                else
                {
                    //Get the next pog to remove
                    var nextPog = pogToRemove.transform.GetChild(0).GetChild(0).gameObject;

                    //Drop Middle Pog
                    DropPog(false);

                    //Pog to remove becomes the next one
                    pogToRemove = nextPog;
                    gameObject.SendMessageUpwards("ChangeCollider", true);
                }
            }

        }
    }
    #endregion
}
