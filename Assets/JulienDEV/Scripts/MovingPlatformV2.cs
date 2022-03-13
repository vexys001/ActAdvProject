using UnityEngine;

public class MovingPlatformV2 : MonoBehaviour
{
    [SerializeField] private Transform StartPoint = null, EndPoint = null;
    [SerializeField] private float _platSpeed = 3;

    private Transform _objective =  null;

    void Start()
    {
        _objective = EndPoint;
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, _objective.position, _platSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _objective.position) <= 0.1f) ChangeDestination();
    }

    private void ChangeDestination()
    {
        _objective = _objective.position != StartPoint.position ? StartPoint : EndPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
