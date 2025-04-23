using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorAnimation : MonoBehaviour
{
  
  public Animator animator;

  private void OnTriggerEnter2D (Collider2D other)
  {
    Debug.Log("enter");
    if (other.CompareTag("Player"))
    {
      Debug.Log("player enter");
      animator.SetBool("doorOpen", true);
    }
  }
}
