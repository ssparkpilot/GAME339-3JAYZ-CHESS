using UnityEngine;

public class EconomyTower : DeathEffectObject
{
    [Header("Attribute")]
    [SerializeField] private float mps = 4f; // money per second
    [SerializeField] private int currencyWorth = 10;
    
    public GameObject FloatingScorePrefab;
    
    private float timeUntilFire;
    
    void Update()
    {
        timeUntilFire += Time.deltaTime;
        
        if (timeUntilFire >= 1f / mps)
        {
            timeUntilFire -= 1f / mps;
            MakeMoney();
        }
    }

    private void MakeMoney()
    {
        LevelManager.main.IncreaseCurrency(currencyWorth);
        Debug.Log("Tower made money " + currencyWorth);
        timeUntilFire = 0f;
        
        CreateDeathEffect();
        
        FloatingText floatingText = FloatingScorePrefab.GetComponent<FloatingText>();
        floatingText.SetText(currencyWorth);

        Instantiate(FloatingScorePrefab, transform.position, Quaternion.identity);
    }
}
