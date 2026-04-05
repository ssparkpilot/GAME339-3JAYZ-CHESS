using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerHealth = 100;
    public bool isGameOver = false;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverCanvas;

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

    public void IncreaseCurrency(int ammount)
    {
        currency += ammount;
    }

    public bool SpendCurrency(int ammount)
    {
        if (currency >= ammount)
        {
            currency -= ammount;
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
        
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
