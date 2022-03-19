using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2 : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private float _targetStoppingDistance = 0f;
    [SerializeField] private float _locationStoppingDistance = 0f;

    private Vector3 _initialPosition = Vector3.zero;

    private NavMeshAgent _enemy = null;
    private bool _isInsideAggro = false;

    private bool _moving = false;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
        _enemy.SetDestination(_initialPosition);

        _initialPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (_isInsideAggro == true)
        {
            SetDestination(_target.transform.position);
        }
        else if (_isInsideAggro == false)
        {
            _enemy.isStopped = true;
            SetDestination(_initialPosition);    
        }

        if (_enemy.remainingDistance == 0)
        {
            _enemy.isStopped = true;
        }
    }

    private void SetDestination(Vector3 destination)
    {
        if (/*_moving == true*/ true)
        {
            _enemy.isStopped = false;
            _enemy.SetDestination(destination);
            _enemy.transform.LookAt(destination);
            _moving = false;

            if (_target.tag == "Player")
            {
                _enemy.stoppingDistance = _targetStoppingDistance;
            }
            else if (_target.tag != "Player")
            {
                _enemy.stoppingDistance = _locationStoppingDistance;
            }
        }
    }

    private void MoveUpdate(float updateDelay)
    {
        float counter = 0f;

        if (_moving == false)
        {
            counter += Time.deltaTime;

            if (counter >= updateDelay)
            {
                counter = 0f;
                _moving = true;
            }
        }
    }

    public void IsAggro()
    {
        _isInsideAggro = true;
        _moving = true;
    }

    public void NotAggro()
    {
        _isInsideAggro = false;
        _moving = false;
    }
}
