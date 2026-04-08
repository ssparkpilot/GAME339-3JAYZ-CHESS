using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int currencyWorth = 100;
    [SerializeField] private AudioClip pickupSound;

    private void OnMouseDown()
    {
        LevelManager.main.IncreaseCurrency(currencyWorth);

        // sound
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        
        Destroy(gameObject);
    }
}