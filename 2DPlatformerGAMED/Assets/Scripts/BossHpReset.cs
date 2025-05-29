using System.Collections.Generic;
using UnityEngine;

public class BossHpReset : MonoBehaviour
{

    [SerializeField] private DashDestroy _target;

    [SerializeField] private int _deathCount = 0;

    [SerializeField] private int _resetAtThisCount=3;

    [SerializeField] private HealthbarScript _heroHealthBar;
    private void resetBoss()
    {
        
        _target.Reset();
    }

    

    public bool CountDeath()
    {
        _deathCount++;
        _heroHealthBar.SetHealthBar(3-_deathCount);
        if (_deathCount >= _resetAtThisCount)
        {
            _deathCount = 0;
            resetBoss();
            _heroHealthBar.SetHealthBar(3-_deathCount);
            return true;
        }
        else
        {
            return false;
        }
    }

}
