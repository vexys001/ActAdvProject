using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Transform _player = null;
    [SerializeField] private LayerMask _whatIsGround, _whatIsPlayer;

    [SerializeField] private Vector3 _walkPoint;
    private bool _walkPointSet = false;
    [SerializeField] private float _walkPointRange = 0f;

    [SerializeField] private float _timeBetweenAttacks = 0f;
    private bool _alreadyAttacked = false;

    [SerializeField] private float _sightRange = 0f, _attackRange = 0f;
    private bool _playerInSightRange, _playerInAttackRange;

    [SerializeField] GameObject _projectile = null;

    [SerializeField] private float _health = 10f;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if (!_playerInSightRange && ! _playerInAttackRange) Patroling();
        if (_playerInSightRange && ! _playerInAttackRange) ChasePlayer();
        if (_playerInSightRange && !_playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet) _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _whatIsGround)) _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!_alreadyAttacked)
        {
            //Attack.
            Rigidbody rb = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0) Invoke(nameof(DestroyEnemy), 2f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
