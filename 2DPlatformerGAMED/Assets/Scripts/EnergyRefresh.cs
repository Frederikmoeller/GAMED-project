using System;
using UnityEngine;

public class EnergyRefresh : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.RefilEnergy();
        }
    }
}
