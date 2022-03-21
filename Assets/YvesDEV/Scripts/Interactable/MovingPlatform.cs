using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform StartPoint, EndPoint;
    //public bool _oneMove = false;
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
            this.transform.position = Vector3.MoveTowards(this.transform.position, _objective.position, _platSpeed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, _objective.position) <= 0.1f)
            {
                 ChangeDestination();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name.Equals("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name.Equals("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
