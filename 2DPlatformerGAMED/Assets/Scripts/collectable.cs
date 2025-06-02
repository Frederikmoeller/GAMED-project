using UnityEngine;

public class collectable : MonoBehaviour
{

    private SpriteRenderer _sprite;
    private BoxCollider2D _box;
   
   
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _box = GetComponent<BoxCollider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.GameManagerInstance.collectibles = +1;
            _sprite.enabled = false;
            _box.enabled = false;
        }
    }

    public void turnOn()
    {
        Debug.Log("turning the star on");
        _sprite.enabled = true;
        _box.enabled = true;
    }
    
}
