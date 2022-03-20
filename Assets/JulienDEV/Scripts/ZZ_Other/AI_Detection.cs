using UnityEngine;

public class AI_Detection : MonoBehaviour
{
    [SerializeField] private string _targetTag = null;
    [SerializeField] private GameObject _enemy = null;
    private EnemyAI _enemyAI = null;

    private enum _mode {AggroZone, AttackThreshold};
    [SerializeField] private _mode _Mode = _mode.AggroZone;

    private void Start()
    {
        _enemyAI = _enemy.GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_Mode == _mode.AggroZone)
        {
            if (other.tag == _targetTag)
            {
                _enemyAI.IsAggro();
                Debug.Log("Aggro!");
            }
        }
        else if (_Mode == _mode.AttackThreshold)
        {
            if (other.tag == _targetTag)
            {
                _enemyAI.IsInThreshold();
                Debug.Log("Entered attack threshold!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_Mode == _mode.AggroZone)
        {
            if (other.tag == _targetTag)
            {
                _enemyAI.NotAggro();
                Debug.Log("Not Aggro!");
            }
        }
        else if (_Mode == _mode.AttackThreshold)
        {
            if (other.tag == _targetTag)
            {
                _enemyAI.NotInThreshold();
                Debug.Log("Exited attack threshold!");
            }
        }
    }
}
