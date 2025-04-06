using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _animationTransform;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Player _playerCharacter;
    private bool _facingRight=true;
    private bool _isDashing;
    [SerializeField] private GameObject [] _objects;
    private Vector2[] originalPositions;


    public void Exsplode()
    {
        originalPositions = new Vector2[_objects.Length];
        for (int i = 0; i < _objects.Length; i++)
        {
            originalPositions[i] = _objects[i].transform.position;
            _objects[i].SetActive(true);
        }
    }
    
    public void AnimationGroundCheck()
    {
        _animator.SetBool("falling", false);
    }

    public void Respawn(bool spawning)
    {
        _animator.SetBool("respawning", spawning);
    }


    public void Jump()
    {
        _animator.SetBool("jumping", true);
    }

    public void DashMove(bool dashing)
    {
        _animator.SetBool("dash", dashing);
        _isDashing = dashing;
    }

    public void Death()
    {
        _animator.SetBool("death",_playerCharacter.isDying);
        Debug.Log("character is dying = " +_playerCharacter.isDying);
    }
    private void Update()
    {
        if (Mathf.Abs(_playerRigidbody.linearVelocityX) > 0.1)
        {
            _animator.SetBool("running", true);
          
            _animationTransform.localScale = new Vector3(Mathf.Sign(_playerRigidbody.linearVelocityX), 1, 1);
            
        }
        else
        {
            _animator.SetBool("running", false);
        }

        if (_playerRigidbody.linearVelocityY < 0&& !_controller.isGrounded&&!_isDashing)
        {
            //_animator.SetBool("dash", false);
            _animator.SetBool("jumping", false);
            _animator.SetBool("falling", true);
        }
    }
}
