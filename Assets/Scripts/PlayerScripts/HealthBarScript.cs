using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image health;
    public float playerHealth;
    public GameObject deathMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100;
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
        if (playerHealth >= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            deathMenu.SetActive(true);
        }
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);
        health.fillAmount = playerHealth / 100f;

    }
    public void Heal(float healAmount)
    {
        playerHealth += healAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);
        health.fillAmount = playerHealth / 100;
    }
}
