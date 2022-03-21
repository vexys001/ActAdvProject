using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform StartPoint, EndPoint;
    public bool _oneMove = false;
    [SerializeField]
    private float _platSpeed = 3;
    [SerializeField] private bool _activated = false;

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
        if (_activated)
        {
            transform.position = Vector3.MoveTowards(transform.position, _objective.position, _platSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _objective.position) <= 0.1f)
            {
                if (_oneMove) _activated = false;
                else ChangeDestination();
            }
        }

    }

    private void ChangeDestination()
    {
        _objective = _objective.position != StartPoint.position ? StartPoint : EndPoint;
    }

    private void ActivatePlat()
    {
        _activated = true;
    }

    /*private void OnCollisionEnter(Collision collision)
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
    }*/
}
