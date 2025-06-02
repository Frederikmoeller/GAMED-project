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

    [SerializeField] private collectable _collectable;
    public bool _collectableHeld;
    
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
        //Debug.Log("Running the death script");
        pcRigid.constraints  = RigidbodyConstraints2D.FreezeAll;
  
        isDying=true;
        _playerAnimation.Death();
        

        
        yield return new WaitForSeconds(_playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        
      
        transform.position = _checkPoint.position;
        if (_collectableHeld)
        {
            _collectable.turnOn();
            yield return new WaitForSeconds(0.1f);
            ResetCollectible();
        }

        
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

    public void ResetCollectible()
    {
        _collectableHeld = false;
        _collectable = null;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("DeathTrigger")&&!_isDying)
        {

            _isDying = true;
            Debug.Log("run corutine");
            StartCoroutine(Death()); 
 

        }

        if (other.CompareTag("collectable"))
        {
            _collectable = other.GetComponent<collectable>();
            _collectableHeld = true;
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
