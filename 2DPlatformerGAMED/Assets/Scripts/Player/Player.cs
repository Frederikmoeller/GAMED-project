using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool isDying;
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private Rigidbody2D pcRigid;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private bool _isDying=false;
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

    private IEnumerator Death()
    {
        bool bossfight = false;
        BossHpReset bossHpReset = FindObjectOfType<BossHpReset>();
        if (bossHpReset != null)
        {
            
            
            if (!bossHpReset.CountDeath())
            {
                yield return new WaitForSeconds(0.5f);
                _isDying = false;
                yield break; 
            }
        }
        
        pcRigid.constraints  = RigidbodyConstraints2D.FreezeAll;
        

        isDying=true;
        _playerAnimation.Death();
        //yield return new WaitForSeconds(1f);

        
        yield return new WaitForSeconds(_playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        
      
        transform.position = _checkPoint.position;
        
        
        //_gameManager.deaths++;
        isDying = false;
        _playerAnimation.Death();
        _playerAnimation.Respawn(true);

        yield return new WaitForSeconds(_playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        _playerAnimation.Respawn(false);
        pcRigid.constraints = RigidbodyConstraints2D.None;
        pcRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isDying = false;

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("DeathTrigger"))
        {
            if (!_isDying)
            {
                Debug.Log("run corutine");
                _isDying = true;
                StartCoroutine(Death()); 
            }

        }

        if (other.CompareTag("CheckPoint"))
        {
            _checkPoint = other.transform.Find("CheckpointSpawnPosition");
        }

        if (other.CompareTag("Goal"))
        {
            GameManager.GameManagerInstance.MarkLevelCleared(SceneManager.GetActiveScene().name);
            StartCoroutine(GameManager.GameManagerInstance.LoadLevel(GameManager.GameManagerInstance.currentLevel + 1));
        }
    }
}
