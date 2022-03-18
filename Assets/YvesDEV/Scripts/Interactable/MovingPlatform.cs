using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform StartPoint, EndPoint;
    [SerializeField]
    private float _platSpeed = 3;

    Transform _objective;

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
