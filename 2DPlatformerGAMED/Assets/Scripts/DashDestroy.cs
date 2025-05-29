using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class DashDestroy : MonoBehaviour
{

    [SerializeField] private GameObject _objectToDestroy;
    [SerializeField] private BossHP _bossHp;
    private bool _isDestroying = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerDestruction(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        triggerDestruction(other);
    }


    private void triggerDestruction(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            if (target.GetComponent<PlayerController>().checkdashin())
            {
                if (!_isDestroying)
                {
                    _isDestroying = true;
                    StartCoroutine(destruction());
                }
            }
        }
    }
    private IEnumerator destruction()
    {
         //never set it as true again, since this function dosnt need to be called more than once 
        //per script
        if (_bossHp !=null)
        {
            _bossHp.damage();
        }
       
        _objectToDestroy.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        _isDestroying = false;
        _objectToDestroy.gameObject.SetActive(true);
    }

    public void Reset()
    {
   
            //_objectToDestroy.gameObject.SetActive(true);
            //_isDestroying = false;
            _bossHp.Reset();
       
    }
}
