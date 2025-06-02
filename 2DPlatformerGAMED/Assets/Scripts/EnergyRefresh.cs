using System;
using UnityEngine;

public class EnergyRefresh : MonoBehaviour
{

    private SpriteRenderer _sprite;
    private CircleCollider2D _circle;
    private Player _player;
   
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _circle = GetComponent<CircleCollider2D>();
        _player=FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.RefilEnergy();
            turnOnOrOff(false);
        }
    }

    private void Update()
    {
        if (_player.isDying)
        {
            turnOnOrOff(true);
        }
    }

    private void turnOnOrOff(bool onOrOff)
    {
        _sprite.enabled = onOrOff;
        _circle.enabled = onOrOff;
    }
}
