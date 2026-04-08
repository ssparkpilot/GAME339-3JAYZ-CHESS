using UnityEngine;

public class CoinPickup : DeathEffectObject
{
    [SerializeField] private int currencyWorth = 100;
    [SerializeField] private AudioClip pickupSound;
    
    public GameObject FloatingScorePrefab;

    private void OnMouseDown()
    {
        LevelManager.main.IncreaseCurrency(currencyWorth);
        
        // floating score text
        FloatingText floatingText = FloatingScorePrefab.GetComponent<FloatingText>();
        floatingText.SetText(currencyWorth);
        Instantiate(FloatingScorePrefab, transform.position, Quaternion.identity);

        // sound
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        
        Destroy(gameObject);
    }
}