using UnityEngine;

public class collectable : MonoBehaviour
{

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.GameManagerInstance.collectibles = +1;
            Destroy(this.gameObject);
        }
    }
    
}
