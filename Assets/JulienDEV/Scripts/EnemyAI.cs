using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Vector3 _initialPosition = Vector3.zero;
    [SerializeField] private GameObject _target = null;
    [SerializeField] private float _movementUpdateDelay = 0f;

    private NavMeshAgent _enemy = null;
    private bool _isInsideAggro = false;
    private bool _isInsideThreshold = false;

    private bool _moving = false;

    //private enum _states {aggro, deaggro};
    //private _states _States = _states.deaggro;

    //NOTE: Add a third zone? "AttackRange" which will flip a boolean to tell this script to chase player if zone is exited?
    //NOTE2: Aggro will become an external BoxCollider object shared between enemies.

    private void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
        _enemy.SetDestination(_initialPosition);

        _initialPosition = gameObject.transform.position;
    }

    private void Update()
    {
        MoveUpdate(_movementUpdateDelay);

        if (_isInsideAggro == true)
        {
            EnemyMove(_target.transform.position);
            //Debug.Log("Agrro'd.");

            if (_isInsideThreshold == true)
            {
                //BACK AWAY FROM PLAYER.
            }
            else if (_isInsideThreshold == false)
            {
                _enemy.isStopped = true;
                //NOW ATTACK PLAYER.
            }
        }
        else if (_isInsideAggro == false)
        {
            _enemy.isStopped = true;

            //NOW GO BACK TO SPAWN POINT.
            EnemyMove(_initialPosition);    

            //Debug.Log("De-aggro'd.");
        }
    }

    private void EnemyMove(Vector3 destination)
    {
        if (_moving == true)
        {
            _enemy.isStopped = false;
            _enemy.SetDestination(destination);
            _moving = false;
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

    /*private void GoTo(Vector3 destination)
    {
        _enemy.isStopped = false;
        gameObject.transform.LookAt(destination);
        _enemy.SetDestination(destination);
    }*/

    private void KeepAttackRange(Vector3 targetPosition)
    {
        gameObject.transform.LookAt(_initialPosition);
        _enemy.SetDestination(_initialPosition);
    }



    private Vector3 GetFleeDirection()
    {
        return (gameObject.transform.position - _target.transform.position).normalized;
    }

    /*private void Flee()
    {
        if (_isInsideThreshold == true)
        {
            //GoTo(GetFleeDirection());
        }
        else if (_isInsideThreshold == false)
        {
            //_enemy.isStopped = true;
            //Resume attacking.
        }
        
    }*/

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

    public void IsInThreshold()
    {
        _isInsideThreshold = true;
        _moving = true;
    }

    public void NotInThreshold()
    {
        _isInsideThreshold = false;
        _moving = false;
    }

    ///For later use.
    /*
    private void RunStates()
    {
        if (_States == _states.aggro)
        {
            StartAggro();
        }
        else if (_States == _states.aggro)
        {
            StartDeAggro();
        }
    }

    private void StartAggro()
    {

    }

    private void Aggro()
    {

    }

    private void ExitAggro()
    {

    }

    private void StartDeAggro()
    {

    }

    private void DeAggro()
    {

    }

    private void ExitDeAggro()
    {

    }
    */

    //If player enters aggro zone, turn AI towards player and move it towards player up a threshold.
    //When the threshold is met, attack player.
    //If player gets too close, back up until threshold is hit again.
    //If player exits aggro zone, AI returns it
}
