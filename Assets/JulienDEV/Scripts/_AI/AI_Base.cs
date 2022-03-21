using UnityEngine;
using UnityEngine.AI;

public class AI_Base : MonoBehaviour
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

    private enum _states { idle, chase, flee };
    private _states _States = _states.idle;

    private bool _inCombat = false;
    private float _whenToAttack = 0;
    [SerializeField] private float _attackTimer = 5f;

    [SerializeField] GameObject _stackHolder = null;
    private StackObject _stackObject = null;
    private BoxCollider _col = null;

    [SerializeField] GameObject _slimeModel = null;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _stackObject = _stackHolder.GetComponent<StackObject>();
        _col = GetComponent<BoxCollider>();

        GoTo(_initialPosition.position);
    }

    private void Start()
    {
        for(int i = 1; i < _stackObject.pogCount; i++) ChangeCollider(false);
    }

    private void Update()
    {
        //Test
        TestPog();
        //Attack();
        //

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
            _inCombat = false;
            GoTo(_initialPosition.position);
        }
    }

    private void ChaseState()
    {
        if (_States == _states.chase)
        {
            _inCombat = true;
            _whenToAttack += Time.deltaTime;
            GoTo(_player.position);
            if (_agent.velocity.magnitude <= 0.15f 
                && _stackObject.pogCount > 1 
                && _whenToAttack > _attackTimer
                && Mathf.Abs(_target.y - transform.position.y) < 3f) Attack();
        }
    }

    private void FleeState()
    {
        if (_States == _states.flee)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance <= _fleeDistance)
            {
                //Debug.Log("Fleeing!");

                Vector3 directionToPlayer = transform.position - _player.transform.position;
                Vector3 newPosition = transform.position + directionToPlayer;
                _target = newPosition;

                GoTo(_target);
            }
        }
    }

    private void Attack()
    {
        _whenToAttack = 0;
        _stackHolder.SendMessage("ShootPog");
        ChangeCollider(true);

        /*if (Input.GetKeyDown(KeyCode.Alpha9) && _stackObject.pogCount > 1)
        {
            _stackHolder.SendMessage("ShootPog");
            ChangeCollider(true);
        }*/
    }

    private void ChangeCollider(bool remove)
    {
        //_col.center += Vector3.down * 0.05f;
        if (!remove)
        {
            _stackObject.transform.localPosition += new Vector3(0, 0.032f, 0);
            _agent.baseOffset += 0.032f;
        }
        else
        {
            _stackObject.transform.localPosition += new Vector3(0, 0.032f, 0);
            _agent.baseOffset -= 0.032f;
        }

        _col.size = new Vector3(1, 0.064f * _stackObject.pogCount, 1);
        _slimeModel.transform.localPosition = new Vector3(0, 0.05f * _stackObject.pogCount, 0);

        //bottomGO = _stack.lastPogGO;

        //_agent.height = 0.064f * _stackObject.pogCount;
    }

    private void TestPog()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _stackHolder.SendMessage("TempAddPog");
            ChangeCollider(false);
        }
    }

    public void AggroPull()
    {
        if (_inCombat == false)
        {
            _States = _states.chase;
            Debug.Log("AggroPull");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Pog"))
        {
            Debug.Log("-----------");
            Debug.Log("Hit by a pog");
            Pog collidedPog = collision.gameObject.transform.parent.GetComponent<Pog>();
            if (collidedPog.GetState().Equals("isShooting") || collidedPog.GetState().Equals("isShielding"))
            {
                if (collidedPog.belongsTo == SystemEnums.Partys.Ally)
                {
                    Debug.Log("Ouchie!");
                    if (_stackObject.pogCount > 1)
                    {
                        Debug.Log("Oh no i dropped my patatoes");
                        _stackHolder.SendMessage("DropPog", StackObject.Positions.Top);
                        ChangeCollider(true);
                    }
                    else
                    {
                        Debug.Log("I die");
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
