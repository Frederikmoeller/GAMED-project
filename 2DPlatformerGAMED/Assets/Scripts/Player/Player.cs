using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;

    private GameManager _gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameManager.GameManagerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathTrigger"))
        {
            transform.position = _checkPoint.position;
            _gameManager.deaths++;
        }

        if (other.CompareTag("CheckPoint"))
        {
            _checkPoint = other.transform.Find("CheckpointSpawnPosition");
        }

        if (other.CompareTag("Goal"))
        {
            
        }
    }
}
