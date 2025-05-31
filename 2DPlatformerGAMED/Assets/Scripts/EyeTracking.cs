using System;
using UnityEngine;

public class EyeTracking : MonoBehaviour
{
    [SerializeField] private Transform _trackingTarget;
    [SerializeField] private CircleCollider2D  _boundry;

    private Vector2 _center;
    private float _radius;

    private void Start()
    {
      
        _center  = _boundry.bounds.center;
        _radius = _boundry.radius * _boundry.transform.localScale.x;
    }

    
    void Update()
    {
        _center  = _boundry.bounds.center;
        _radius = _boundry.radius * _boundry.transform.localScale.x;
        Vector2 desiredPosition = (Vector2)_trackingTarget.position-_center;

        Vector2 newPos;
        if (desiredPosition.magnitude <= _radius)
        {
            newPos = _trackingTarget.position;
        }
        else
        {
            newPos = _center + desiredPosition.normalized * (_radius);
        }

        transform.position = newPos;

    }
}
