using UnityEngine;

public class _Detect : MonoBehaviour
{
    [SerializeField] private string _targetTag = null;
    [SerializeField] private GameObject _enemy = null;
    private _Enemy _enemyAI = null;

    private void Start()
    {
        _enemyAI = _enemy.GetComponent<_Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _targetTag)
        {
            //_enemyAI.IsAggro();
            Debug.Log("Aggro!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == _targetTag)
        {
            //_enemyAI.NotAggro();
            Debug.Log("Not Aggro!");
        }
    }
}
