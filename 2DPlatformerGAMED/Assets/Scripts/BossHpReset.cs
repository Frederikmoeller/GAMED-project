using System.Collections.Generic;
using UnityEngine;

public class BossHpReset : MonoBehaviour
{

    [SerializeField] private List<DashDestroy> _targets;

    [SerializeField] private int _deathCount = 0;

    [SerializeField] private int _resetAtThisCount=3;

    [SerializeField] private HealthbarScript _heroHealthBar;
    private void resetBoss()
    {
        foreach (var target in _targets)
        {
            target.Reset();
        }
    }


    public void CountDeath()
    {
        _deathCount++;
        _heroHealthBar.SetHealthBar(3-_deathCount);
        if (_deathCount >= _resetAtThisCount)
        {
            _deathCount = 0;
            resetBoss();
            _heroHealthBar.SetHealthBar(3-_deathCount);
        }
    }

}
