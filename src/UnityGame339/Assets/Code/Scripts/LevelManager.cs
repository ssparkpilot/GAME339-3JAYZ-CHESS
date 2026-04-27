using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

public class LevelManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerHealth = 100;
    public int currency = 100;
    public bool isGameOver;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameWinCanvas;

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    private ILevelService levelService;

    private void Awake()
    {
        main = this;
        levelService = new LevelService();
    }

    private void Start()
    {
        isGameOver = false;
        UpdateHealthUI();

        gameOverCanvas?.SetActive(false);
        gameWinCanvas?.SetActive(false);

        Time.timeScale = 1f;
    }

    public void IncreaseCurrency(int amount)
    {
        currency = levelService.IncreaseCurrency(currency, amount);
    }

    public bool SpendCurrency(int amount)
    {
        if (!levelService.CanSpendCurrency(currency, amount))
            return false;

        currency = levelService.SpendCurrency(currency, amount);
        return true;
    }

    public void LoseHealth(int amount)
    {
        if (isGameOver)
            return;

        playerHealth = levelService.LoseHealth(playerHealth, amount);
        UpdateHealthUI();

        if (levelService.IsGameOver(playerHealth))
            GameOver();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = playerHealth.ToString();
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverCanvas?.SetActive(true);
        gameWinCanvas?.SetActive(false);
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        gameWinCanvas?.SetActive(true);
        gameOverCanvas?.SetActive(false);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public bool IsGameOver => isGameOver;

    public bool CanAfford(int amount)
    {
        return levelService.CanSpendCurrency(currency, amount);
    }

    public bool TrySpendCurrency(int amount)
    {
        if (!levelService.CanSpendCurrency(currency, amount))
            return false;

        currency = levelService.SpendCurrency(currency, amount);
        return true;
    }
}
