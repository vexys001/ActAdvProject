using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent _agent = null;
    private Vector3 _startingPosition = Vector3.zero;
    private Vector3 _roamingPosition = Vector3.zero;

    private void Start()
    {
        _startingPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.SetDestination(_roamingPosition);
        float reachedPositionDistance = 1f;
        if (Vector3.Distance(transform.position, _roamingPosition) < reachedPositionDistance)
        {
            //Reached roam position.
            _roamingPosition = GetRoamingPosition();
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * Random.Range(10f, 50f);
    }
}
