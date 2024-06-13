using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyCollision : MonoBehaviour
{
    GameObject playerHealth;
    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindWithTag("PlayerHealth");
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        print("ChestMonster collision " + col.gameObject.name);
        if (col.tag == "Player")
        {
            var healthBar = playerHealth.GetComponent<HealthBarScript>();
            healthBar.TakeDamage(10);
            print("game won");
        }
    }
}
