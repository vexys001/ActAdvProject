using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] GameObject _target = null;

    private void OnCollisionEnter(Collision collision)
    {
        _target.SetActive(false);
    }
}
