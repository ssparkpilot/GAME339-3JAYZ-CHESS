using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    
    private GameObject tower;
    private Color startColor;
    public AudioSource audioSource;
    public AudioClip placeSound;


    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Start()
    {
        startColor = sr.color;
    }
    
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        // fix for preventing tower plotting when clicking on a UI button

        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You are a broke chud!");
            return;
        }
        
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        audioSource.volume = 0.5f;
        //play the place sound at the randomized pitch
        audioSource.PlayOneShot(placeSound);
        
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
