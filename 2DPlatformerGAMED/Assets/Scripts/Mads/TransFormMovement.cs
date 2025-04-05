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
    private bool _isground;
    [SerializeField] private bool _mustNotFall;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _horizontalSpeed;
    public int energy = 3;

    [SerializeField] private float _jumpSpeed = 5;

    [SerializeField] private bool _jumping = false;
    [SerializeField] private bool _dashing = false;
    [SerializeField] private float _gravity = -5f;
    [SerializeField] private float _dashpeed = 5f;
    [SerializeField] private float _jumpTime=0.1f;
    [SerializeField] private float _dashTime=0.1f;
    [SerializeField] private Vector2 _movement;
    [SerializeField] private float _moveSpeed;
    private PlayerInputHandler _inputHandler;

    // Update is called once per frame

    private void Awake()
    {
        _inputHandler = PlayerInputHandler.PlayerInputHandlerInstance;
    }
    void Update()
    {
        //CheckGround();
        if (_inputHandler.JumpInput)
        {
            Activity(Jumprutine());
        }

        if (_inputHandler.DashInput&&!_isground)
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
            tticalSpeed=0;
        }
        else if(!Isground)
        {
            VerticalSpeed =gravity;
        }
        
    }*/

    public void SetGround(bool ground)
    {
        _isground = ground;
        _mustNotFall = ground;
        
        
    }
    
    
    void ApplyMovement()
    {
        float walking = 0;
        if (!_dashing)
        {
            walking = _inputHandler.MoveInput.x * _moveSpeed; 
            _horizontalSpeed = walking;
        }
        

        if (walking != 0)
        {
            // Flip the character's facing direction based on movement direction
            transform.localScale = new Vector3(Mathf.Sign(walking), 1, 1);
           
        }
      

        if (_dashing)
        {
            _horizontalSpeed = _dashpeed*transform.localScale.x;
        }
     


        if (_mustNotFall)
        {
            _verticalSpeed = 0;
        }
        else if (!_jumping)
        {
            _verticalSpeed += _gravity * Time.deltaTime; 
        }

        
        if (_jumping)
        {
            _verticalSpeed = _jumpSpeed;
        }
        _movement = new Vector2(_horizontalSpeed, _verticalSpeed);
        
        transform.Translate(_movement*Time.deltaTime);
    }


    private void Activity(IEnumerator routine)
    {
        if (!_isground)
        {
            if (energy > 0)
            {
                energy--;
                StopCoroutine(routine);
                StartCoroutine(routine);
            }
            if(energy<=0)
            {
                //but in a grunt sound or something here
            }
        }

        if (_isground)
        {
            StopCoroutine(routine);
            StartCoroutine(routine);
        }
        
    }
    
   
    
    private IEnumerator Jumprutine()
    {

        _jumping = true;
        
        
        yield return new WaitForSeconds(_jumpTime);
        
        _jumping = false;

    }

    private IEnumerator Dashrutine()
    {
        _mustNotFall = true;
        _dashing = true;
        yield return new WaitForSeconds(_dashTime);
        _dashing = false;
        _mustNotFall = _isground;
    }

}
