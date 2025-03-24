using System;
using System.Collections;
using UnityEngine;

public class CheckGround : MonoBehaviour
{

    public TransFormMovement move;
    [SerializeField] private float _coyoteTime = 0.15f;
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (((1 << collision.gameObject.layer) & move.groundLayerMask) != 0)
        {
            move.SetGround(true);
            
            move.energy = 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & move.groundLayerMask) != 0)
        {
            move.SetGround(true);
            
            move.energy = 3;
        }
    }

    // Called when groundCheck exits the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & move.groundLayerMask) != 0)
        {
            StartCoroutine(coyoteTime());
        }
    }


    private IEnumerator coyoteTime()
    {
        yield return new WaitForSeconds(_coyoteTime);
        move.SetGround(false);
    }
}
