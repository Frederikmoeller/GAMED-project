using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransFormMovement : MonoBehaviour
{
    public LayerMask groundLayerMask;

    //public Transform groundCheck;
    private bool Isground;
    private bool MustNotFall;
    public float VerticalSpeed;
    public int engergy = 3;

    public float jumpSpeed = 5;

    private bool jumping = false;
    private float gravity = -5f;
    public Vector2 movement;
    public float movespeed;

    // Update is called once per frame
    void Update()
    {
        //CheckGround();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        ApplyMovement();
    }

    /*void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayerMask);
        Isground = hit.collider !=null;
        if (Isground)
        {
            VerticalSpeed=0;
        }
        else if(!Isground)
        {
            VerticalSpeed =gravity;
        }
        
    }*/

    public void SetGround(bool ground)
    {
        Isground = ground;
        MustNotFall = ground;
    }
    
    
    void ApplyMovement()
    {
        float walking = Input.GetAxis("Horizontal")*movespeed;

        if (!MustNotFall)
        {
            VerticalSpeed = gravity;
        }

       
        else
        {
            VerticalSpeed = 0;
        }
        movement = new Vector2(walking, VerticalSpeed);
        
        transform.Translate(movement*Time.deltaTime);
    }


    private void jump()
    {
        if (!Isground)
        {
            if (engergy > 0)
            {
                engergy--;
            }
            if(engergy<=0)
            {
                //but in a grunt sound or something here
            }
        }

        if (Isground)
        {
            StopCoroutine(Jumprutine());
            StartCoroutine(Jumprutine());
        }
        
    }
    private IEnumerator Jumprutine()
    {

        jumping = true;
        movement = new Vector2(this.gameObject.transform.position.x, VerticalSpeed);
        yield return new WaitForSeconds(0.5f);
        jumping = false;

    }
    
    
    
    
}
