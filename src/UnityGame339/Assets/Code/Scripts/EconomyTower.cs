using UnityEngine;

public class EconomyTower : Turret
{
    [Header("Attribute")]
    [SerializeField] private int currencyWorth = 10;
    
    public GameObject FloatingScorePrefab;
    
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
        //Debug.Log("Tower made money " + currencyWorth);
        timeUntilFire = 0f;
        
        CreateDeathEffect();
        
        FloatingText floatingText = FloatingScorePrefab.GetComponent<FloatingText>();
        floatingText.SetText(currencyWorth);

        Instantiate(FloatingScorePrefab, transform.position, Quaternion.identity);
    }
}
