using System;
using UnityEngine;
using System.Collections;
public class PushAwayPlayer : MonoBehaviour
{

    private float _pushForce = 50f;
    private Rigidbody2D _rb;
    private Vector2 _pushDirection;
    private bool _isPushing = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&!_isPushing)
        {
            _isPushing = true;
            _rb = other.attachedRigidbody;
            _pushDirection = (other.transform.position - transform.position).normalized;
            StartCoroutine(Push());

        }
        
        
    }

    private IEnumerator Push()
    {
        _rb.constraints=  RigidbodyConstraints2D.FreezeAll;
        _rb.constraints= RigidbodyConstraints2D.None;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.AddForce(_pushDirection*_pushForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        _isPushing = false;
    }
}
