using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{
    public Image health;
    public float enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 100;
        TakeDamage(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Heal(10);
        }
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        enemyHealth = Mathf.Clamp(enemyHealth, 0, 100);
        health.fillAmount = enemyHealth / 100f;

    }
    public void Heal(float healAmount)
    {
        enemyHealth += healAmount;
        enemyHealth = Mathf.Clamp(enemyHealth, 0, 100);
        health.fillAmount = enemyHealth / 100;
    }
}
