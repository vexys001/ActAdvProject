using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    private Transform _target = null;
    [SerializeField] private Transform _player = null;
    [SerializeField] private Transform _initialPosition = null;
    private NavMeshAgent _agent = null;

    [SerializeField] private bool _playerInSightRange = false;
    [SerializeField] private float _sightRange = 20f;
    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private float _targetStoppingDistance = 10f;
    [SerializeField] private float _locationStoppingDistance = 0.1f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        GoTo(_initialPosition);
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);

        if (_playerInSightRange == true) GoTo(_player);
        else if (_playerInSightRange == false) GoTo(_initialPosition);
    }

    private void GoTo(Transform target)
    {
        _target = target;
        _agent.SetDestination(_target.transform.position);
        if (_target == _player)
        {
            _agent.stoppingDistance = _targetStoppingDistance;
            _agent.transform.LookAt(_target.transform);
        }
        else if (_target != _player)
        {
            _agent.stoppingDistance = _locationStoppingDistance;
        }
    }
}
