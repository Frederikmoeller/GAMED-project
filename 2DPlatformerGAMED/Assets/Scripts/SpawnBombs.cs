using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnBombs : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private int _minSpawn, _maxSpawn;
    [SerializeField] private float _minForce, _maxForce,_minHorForce,_maxHorForce,_timer,_spawnTime;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnTime)
        {
            _timer = 0;
            SpawnBoms();
        }
    }


    private void SpawnBoms()
    {
        int count = Random.Range(_minSpawn, _maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = transform.position;
            GameObject obj = Instantiate(_bombPrefab, spawnPos, quaternion.identity);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(Random.Range(_minHorForce,_maxHorForce),Random.Range(_minForce, _maxForce));
            rb.AddForce(force);
        }
    }
}
