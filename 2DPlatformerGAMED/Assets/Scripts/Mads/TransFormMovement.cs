using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class TransFormMovement : MonoBehaviour
{
    public LayerMask groundLayerMask;

    //public Transform groundCheck;
    private bool Isground;
    [SerializeField] private bool MustNotFall;
    public float VerticalSpeed;
    public float HorizontalSpeed;
    public int engergy = 3;

    public float jumpSpeed = 5;

    [SerializeField] private bool jumping = false;
    [SerializeField] private bool dashing = false;
    [SerializeField] private float gravity = -5f;
    [SerializeField] private float dashpeed = 5f;
    [SerializeField] private float jumptime=0.1f;
    [SerializeField] private float dashtime=0.1f;
    public Vector2 movement;
    public float movespeed;

    // Update is called once per frame
    void Update()
    {
        //CheckGround();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Activity(Jumprutine());
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)&&!Isground)
        {
            
            Activity(Dashrutine());
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
        float walking = 0;
        if (!dashing)
        {
            walking = Input.GetAxis("Horizontal") * movespeed; 
            HorizontalSpeed = walking;
        }
        

        if (walking != 0)
        {
            // Flip the character's facing direction based on movement direction
            transform.localScale = new Vector3(Mathf.Sign(walking), 1, 1);
           
        }
      

        if (dashing)
        {
            HorizontalSpeed = dashpeed*transform.localScale.x;
        }
     


        if (MustNotFall)
        {
            VerticalSpeed = 0;
        }
        else if (!jumping)
        {
            VerticalSpeed += gravity * Time.deltaTime; 
        }

        
        if (jumping)
        {
            VerticalSpeed = jumpSpeed;
        }
        movement = new Vector2(HorizontalSpeed, VerticalSpeed);
        
        transform.Translate(movement*Time.deltaTime);
    }


    private void Activity(IEnumerator routine)
    {
        if (!Isground)
        {
            if (engergy > 0)
            {
                engergy--;
                StopCoroutine(routine);
                StartCoroutine(routine);
            }
            if(engergy<=0)
            {
                //but in a grunt sound or something here
            }
        }

        if (Isground)
        {
            StopCoroutine(routine);
            StartCoroutine(routine);
        }
        
    }
    
   
    
    private IEnumerator Jumprutine()
    {

        jumping = true;
        
        
        yield return new WaitForSeconds(jumptime);
        
        jumping = false;

    }

    private IEnumerator Dashrutine()
    {
        MustNotFall = true;
        dashing = true;
        yield return new WaitForSeconds(dashtime);
        dashing = false;
        MustNotFall = Isground;
    }

}
