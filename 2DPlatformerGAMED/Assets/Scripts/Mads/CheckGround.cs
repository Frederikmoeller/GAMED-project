using System;
using UnityEngine;

public class CheckGround : MonoBehaviour
{

    public TransFormMovement move;
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (((1 << collision.gameObject.layer) & move.groundLayerMask) != 0)
        {
            move.SetGround(true);
            move.VerticalSpeed = 0;
            move.engergy = 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & move.groundLayerMask) != 0)
        {
            move.SetGround(true);
            move.VerticalSpeed = 0;
            move.engergy = 3;
        }
    }

    // Called when groundCheck exits the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & move.groundLayerMask) != 0)
        {
            move.SetGround(false);
        }
    }
    
}
