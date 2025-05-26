using System;
using UnityEngine;

public class BossHP : MonoBehaviour
{

    [SerializeField] private int _bossHp = 3;
    [SerializeField] private int _bossMaxHp = 3;
    [SerializeField] private HealthbarScript _bossHealthBar;
    
    public void damage()
    {
        _bossHp--;
        _bossHealthBar.SetHealthBar(_bossHp);
        if (_bossHp == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _bossHp = _bossMaxHp;
        _bossHealthBar.SetHealthBar(_bossHp);
    }
}
