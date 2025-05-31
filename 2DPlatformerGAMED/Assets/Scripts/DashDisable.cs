using UnityEngine;
using System.Collections;

public class DashDisable : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerDestruction(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        triggerDestruction(other);
    }
    private void triggerDestruction(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            if (target.GetComponent<PlayerController>().checkdashin())
            {


                    StartCoroutine(destruction());

            }
        }
    }

    private IEnumerator destruction()
    {
       
        _objectToDestroy.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);


        _objectToDestroy.gameObject.SetActive(true);
    }
    
}
