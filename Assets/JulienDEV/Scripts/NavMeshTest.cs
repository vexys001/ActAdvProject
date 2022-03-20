using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    private Vector3 _target = Vector3.zero;
    [SerializeField] private Transform _player = null;
    [SerializeField] private Transform _initialPosition = null;
    private NavMeshAgent _agent = null;

    [SerializeField] private bool _playerInSightRange = false;
    [SerializeField] private float _sightRange = 50f;
    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private float _targetStoppingDistance = 25f;
    [SerializeField] private float _locationStoppingDistance = 0.1f;

    [SerializeField] private bool _playerInFleeDistance = false;
    [SerializeField] private float _fleeDistance = 0f;

    private enum _states {idle, chase, flee};
    private _states _States = _states.idle;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        GoTo(_initialPosition.position);
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInFleeDistance = Physics.CheckSphere(transform.position, _fleeDistance, _whatIsPlayer);

        if (_playerInSightRange == true) _States = _states.chase;
        else if (_playerInSightRange == false) _States = _states.idle;

        if (_playerInFleeDistance == true) _States = _states.flee;

        RunStates();
    }

    private void GoTo(Vector3 target)
    {
        _target = target;
        _agent.SetDestination(_target);
        if (_target == _player.transform.position)
        {
            _agent.stoppingDistance = _targetStoppingDistance;
            _agent.transform.LookAt(_target);
        }
        else if (_target != _player.transform.position)
        {
            _agent.stoppingDistance = _locationStoppingDistance;
        }
    }

    private void RunStates()
    {
        IdleState();
        ChaseState();
        FleeState();
    }

    private void IdleState()
    {
        if (_States == _states.idle)
        {
            GoTo(_initialPosition.position);
        }
    }

    private void ChaseState()
    {
        if (_States == _states.chase)
        {
            GoTo(_player.position);
        }
    }

    private void FleeState()
    {
        if (_States == _states.flee)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance <= _fleeDistance)
            {
                Debug.Log("Fleeing!");

                Vector3 directionToPlayer = transform.position - _player.transform.position;
                Vector3 newPosition = transform.position + directionToPlayer;
                _target = newPosition;

                GoTo(_target);
            }
        }
    }
}
