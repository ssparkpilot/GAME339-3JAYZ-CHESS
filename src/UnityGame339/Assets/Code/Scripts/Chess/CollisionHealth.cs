using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class CollisionHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " collided with " + other.name);
        //Debug.Log("MONEY MAKE EM DO SOMETHING PUT EM IN A TRANCE");
        
        int towerHealth = GetComponent<Health>().CurrentHP;
        int enemyHealth = other.gameObject.GetComponent<Health>().CurrentHP;
        
        if (towerHealth > enemyHealth)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(enemyHealth);
            gameObject.GetComponent<Health>().TakeDamage(enemyHealth);
        }
        else if(towerHealth < enemyHealth)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(towerHealth);
            gameObject.GetComponent<Health>().TakeDamage(towerHealth);
        }
        else
        {
            other.gameObject.GetComponent<Health>().TakeDamage(towerHealth);
            gameObject.GetComponent<Health>().TakeDamage(enemyHealth);
        }
    }
}