using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
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
            StartCoroutine(loadScene());
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

    private IEnumerator loadScene()
    {
        yield return new WaitForSeconds(3f);
        GameManager.GameManagerInstance.MarkLevelCleared(SceneManager.GetActiveScene().name);
        StartCoroutine(GameManager.GameManagerInstance.LoadLevel(GameManager.GameManagerInstance.currentLevel + 1));
    }
    
}
