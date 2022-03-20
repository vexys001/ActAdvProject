using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    public GameObject samplePog;
    public PogScriptableObject[] PogDatas;
    public int pogCount;
    public GameObject firstPogGO, lastPogGO;


    [Header("Removal Specific")]
    public int ncountrdNonRemovblPogs;
    public enum Positions { Top, Middle, Bottom }

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    public enum AnimClips { Idle, Walk, Jump}

    // Start is called before the first frame update
    void Start()
    {
        firstPogGO = this.transform.GetChild(0).gameObject;
        lastPogGO = firstPogGO;
        pogCount = GetComponentsInChildren<Pog>().Length;

        _animator = GetComponent<Animator>();
    }

    public void AnimateStack(AnimClips clipToPlay)
    {
        switch (clipToPlay)
        {
            case AnimClips.Jump:
                _animator.SetBool("Isjumping", true);
                _animator.SetBool("Isrunning", false);
                _animator.SetBool("Isidle", false);
                //Debug.Log("Jumping Stack");
                break;
            case AnimClips.Idle:
                _animator.SetBool("Isidle", true);
                _animator.SetBool("Isrunning", false);
                _animator.SetBool("Isjumping", false);

                //Debug.Log("Idling Stack");
                break;
            case AnimClips.Walk:
                _animator.SetBool("Isrunning", true);
                _animator.SetBool("Isidle", false);
                _animator.SetBool("Isjumping", false);
                //Debug.Log("Walking Stack");
                break;
        }
    }

    #region Adding to the stack
    void TempAddPog()
    {
        pogCount++;
        Debug.Log("Add a temp pog");
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[0]);
        lastPogGO.GetComponent<Pog>().SetBelong(SystemEnums.Partys.Ally);
    }

    void AddKeyPog()
    {
        pogCount++;
        lastPogGO = Instantiate(samplePog, lastPogGO.transform.GetChild(0));
        lastPogGO.GetComponent<Pog>().SetScriptable(PogDatas[1]);
        lastPogGO.GetComponent<Pog>().SetBelong(SystemEnums.Partys.Ally);
    }

    public void AddPog(GameObject pPog)
    {
        pogCount++;

        //Transform temp = lastPogGO.transform.GetChild(0);
        Transform temp = lastPogGO.transform;

        lastPogGO = pPog;

        lastPogGO.transform.SetParent(temp.GetChild(0));
        gameObject.SendMessageUpwards("ChangeCollider", false);
        lastPogGO.SendMessage("StopDropped");

        lastPogGO.transform.localRotation = temp.localRotation;
        lastPogGO.transform.localPosition = Vector3.zero;

        lastPogGO.GetComponent<Pog>().SetBelong(SystemEnums.Partys.Ally);
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

    void DropPog(Positions positionToRemove)
    {
        GameObject objToDrop = null;

        switch (positionToRemove)
        {
            case Positions.Top:
                objToDrop = RemoveTopPog();
                break;
            case Positions.Middle:
                objToDrop = RemoveMiddlePog();
                break;
            case Positions.Bottom:
                objToDrop = RemoveBottomPog();
                break;
            default:
                break;
        }

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

        //Parent and place back the bottom of the stack
        next.transform.SetParent(previous.transform.GetChild(0));
        next.transform.localPosition = Vector3.zero;

        return toRemove;
    }

    GameObject RemoveBottomPog()
    {
        pogCount--;

        GameObject toRemove = lastPogGO;

        lastPogGO = lastPogGO.transform.parent.parent.gameObject;

        return toRemove;
    }

    public void RemoveXNonKeys(int x)
    {
        ncountrdNonRemovblPogs = 0;

        //Get the First pog
        GameObject pogToRemove = firstPogGO;

        for (int i = 0; i < x && ncountrdNonRemovblPogs < pogCount;)
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
                i++;

                // If youve seen no keys yet
                if (ncountrdNonRemovblPogs == 0)
                {
                    //Drop the top pog
                    DropPog(Positions.Top);

                    //Get the new Top pog
                    pogToRemove = firstPogGO;

                    gameObject.SendMessageUpwards("ChangeCollider", false);
                }
                else
                {
                    if (pogToRemove.transform.GetChild(0).childCount > 0)
                    {
                        GameObject nextPog = pogToRemove.transform.GetChild(0).GetChild(0).gameObject;
                        //Drop Middle Pog
                        DropPog(Positions.Middle);

                        //Pog to remove becomes the next one
                        pogToRemove = nextPog;
                        gameObject.SendMessageUpwards("ChangeCollider", true);
                    }
                    else
                    {
                        DropPog(Positions.Bottom);
                        gameObject.SendMessageUpwards("ChangeCollider", true);
                    }
                }
            }
        }
    }
    #endregion
}
