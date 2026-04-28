using UnityEngine;

public class CoinPickup : DeathEffectObject
{
    [SerializeField] private int currencyWorth = 100;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float autoPickupDelay = 25;
    
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

    private void Start()
    {
        Invoke(nameof(AutoPickup), autoPickupDelay);
    }

    private void AutoPickup()
    {
        //LevelManager.main.IncreaseCurrency(currencyWorth);
        Destroy(gameObject); // for now will destroy when goes offscreen
    }
    
    private void Update()
    {
        transform.Translate(0, Time.deltaTime * 1f, 0); // coins float up
    }
    
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        sr.color = Color.yellow; // change color on mouse hover
    }

    private void OnMouseExit()
    {
        sr.color = Color.white;
    }
}