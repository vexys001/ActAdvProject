using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _initialPosition = null;
    [SerializeField] private GameObject _target = null;

    private NavMeshAgent _enemy = null;
    private bool _isInsideAggro = false;
    private bool _isInsideThreshold = false;

    //private enum _states { aggro, deaggro };
    //private _states _States = _states.deaggro;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
        _enemy.SetDestination(_initialPosition.position);
    }

    private void Update()
    {
        if (_isInsideAggro == true)
        {
            GoTo(_target.transform.position);
            //Debug.Log("Agrro'd.");
        }
        else if (_isInsideAggro == false)
        {
            GoTo(_initialPosition.position);
            //Debug.Log("De-aggro'd.");
        }
    }

    private void GoTo(Vector3 destination)
    {
        _enemy.isStopped = false;
        gameObject.transform.LookAt(destination);
        _enemy.SetDestination(destination);
    }

    private void KeepAttackRange(Vector3 targetPosition)
    {
        gameObject.transform.LookAt(_initialPosition);
        _enemy.SetDestination(_initialPosition.position);
    }



    private Vector3 GetFleeDirection()
    {
        return (gameObject.transform.position - _target.transform.position).normalized;
    }

    private void Flee()
    {
        if (_isInsideThreshold == true)
        {
            GoTo(GetFleeDirection());
        }
        else if (_isInsideThreshold == false)
        {
            _enemy.isStopped = true;
            //Resume attacking.
        }
        
    }

    public void IsAggro()
    {
        _isInsideAggro = true;
    }

    public void NotAggro()
    {
        _isInsideAggro = false;
    }

    public void IsInThreshold()
    {
        _isInsideThreshold = true;
    }

    public void NotInThreshold()
    {
        _isInsideThreshold = false;
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
