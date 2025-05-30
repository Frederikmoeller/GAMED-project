using System;
using UnityEngine;
using System.Collections;
public class selfDestructOnCollision : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _bombAnimator;
    [SerializeField] private float _timer,_selfdestructTime;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _selfdestructTime)
        {
            StartCoroutine(selfDestruc());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(selfDestruc());

        }
        
        
    }

    private IEnumerator selfDestruc()
    {
        _rb.constraints=  RigidbodyConstraints2D.FreezeAll;
        _bombAnimator.SetBool("Exsplode", true);
        yield return new WaitForSeconds(_bombAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }
    
}
