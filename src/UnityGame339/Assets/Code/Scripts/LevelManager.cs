using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerHealth = 100;
    private bool isGameOver = false;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

    private void Awake(){
        main = this;
    }

    private void Start()
    {
        currency = 1000;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }

    public void LoseHealth(int amount)
    {
        if (isGameOver)
        {
            return;
        } 

        playerHealth = Mathf.Max(playerHealth - amount, 0);

        UpdateHealthUI();

        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = playerHealth.ToString();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}
