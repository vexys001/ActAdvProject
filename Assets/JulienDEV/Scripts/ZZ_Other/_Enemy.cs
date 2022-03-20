using UnityEngine;
using UnityEngine.AI;

public class _Enemy : MonoBehaviour
{
    //[SerializeField] private GameObject _target = null;
    //[SerializeField] private float _targetStoppingDistance = 0f;
    //[SerializeField] private float _locationStoppingDistance = 0f;

    [SerializeField] private Vector3 _initialPosition = Vector3.zero;

    private NavMeshAgent _enemy = null;

    [SerializeField] private bool _playerInSightRange = false;
    [SerializeField] private float _sightRange = 10f;
    [SerializeField] private LayerMask _whatIsPlayer;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
        _enemy.SetDestination(_initialPosition);

        _initialPosition = gameObject.transform.position;
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);

        if (_playerInSightRange == true)
        {
            //
        }
        else if (_playerInSightRange == false)
        {
            //
        }

        /*if (_enemy.remainingDistance == 0f)
        {
            _enemy.isStopped = true;
        }*/
    }
}
