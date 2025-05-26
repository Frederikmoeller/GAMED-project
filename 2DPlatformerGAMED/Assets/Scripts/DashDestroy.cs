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
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().checkdashin())
            {
                if (!_isDestroying)
                {
                    destruction();
                }
            }
        }
    }

    private void destruction()
    {
        _isDestroying = true; //never set it as true again, since this function dosnt need to be called more than once 
        //per script
        if (_bossHp !=null)
        {
            _bossHp.damage();
        }
       
        _objectToDestroy.gameObject.SetActive(false);
       
       
    }

    public void Reset()
    {
        if (_isDestroying)
        {
            _objectToDestroy.gameObject.SetActive(true);
            _isDestroying = false;
            _bossHp.Reset();
        }
       
    }
}
