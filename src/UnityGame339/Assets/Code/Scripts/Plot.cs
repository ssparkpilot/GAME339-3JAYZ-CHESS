using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    
    [Header("Placement Colors")]
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;
    [SerializeField] private Color unaffordableColor = Color.gray;
    
    public GameObject towerObj;
    public Turret turret;
    private Color startColor;
    public AudioSource audioSource;
    public AudioClip placeSound;

    public AudioClip cantplaceSound;

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Start()
    {
        startColor = sr.color;
    }
    
    private void OnMouseEnter()
    {
        if (LevelManager.main.isGameOver) return;

        if (BuildManager.main.GetSelectedTower() == null)
        {
            ResetPlotColor();
            return;
        }

        if (!IsCorrectPlacementType())
        {
            sr.color = invalidColor; // invalid upgrade
            return;
        }

        if (!CanAffordPlacement())
        {
            sr.color = unaffordableColor;
            return;
        }

        sr.color = validColor;
    }

    private void OnMouseExit()
    {
        if (LevelManager.main.isGameOver) return;
        ResetPlotColor();
    }

    private void OnMouseDown()
    {
        if (LevelManager.main.isGameOver) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (BuildManager.main.GetSelectedTower() != null)
        {
            if (!IsCorrectPlacementType() || !CanAffordPlacement())
            {
                audioSource.PlayOneShot(cantplaceSound);
                return;
            }
        }

        Tower selectedTowerData = BuildManager.main.GetSelectedTower();

        // If plot has tower already
        if (towerObj != null)
        {
            // if no tower selected then open upgrade UI
            if (selectedTowerData == null)
            {
                //turret.OpenUpgradeUI();
                return;
            }

            int selectedIndex = BuildManager.main.SelectedTowerIndex;
            int currentIndex = turret.towerIndex;

            // if same tower then replace with upgraded tower
            if (selectedIndex == currentIndex && currentIndex % 2 == 0)
            {
                int upgradeIndex = currentIndex + 1;

                // check
                if (upgradeIndex >= BuildManager.main.TowerCount)
                    return;

                Tower upgradeTower = BuildManager.main.GetTowerByIndex(upgradeIndex);
                if (upgradeTower == null) return;

                if (upgradeTower.cost > LevelManager.main.currency)
                {
                    audioSource.PlayOneShot(cantplaceSound);
                    return;
                }

                LevelManager.main.SpendCurrency(upgradeTower.cost);

                Destroy(towerObj);

                BuildManager.main.SetSelectedTower(upgradeIndex);
                towerObj = BuildManager.main.PlaceTower(transform.position);
                turret = towerObj.GetComponent<Turret>();

                audioSource.PlayOneShot(placeSound);
                ResetPlotColor();
            }

            return;
        }

        // empty plot
        if (selectedTowerData == null) return;

        if (selectedTowerData.cost > LevelManager.main.currency)
        {
            audioSource.PlayOneShot(cantplaceSound);
            return;
        }

        LevelManager.main.SpendCurrency(selectedTowerData.cost);

        towerObj = BuildManager.main.PlaceTower(transform.position);
        turret = towerObj.GetComponent<Turret>();

        audioSource.PlayOneShot(placeSound);
        ResetPlotColor();
    }
    
    private bool IsCorrectPlacementType()
    {
        Tower selectedTower = BuildManager.main.GetSelectedTower();
        if (selectedTower == null)
            return false;
        
        if (towerObj == null)
            return true;
        
        int selectedIndex = BuildManager.main.SelectedTowerIndex;
        int currentIndex = turret.towerIndex;

        return selectedIndex == currentIndex && currentIndex % 2 == 0;
    }

    private int GetRequiredCost()
    {
        Tower selectedTower = BuildManager.main.GetSelectedTower();
        if (selectedTower == null)
            return int.MaxValue;

        // empty plot
        if (towerObj == null)
            return selectedTower.cost;

        // upgrade tower
        int upgradeIndex = turret.towerIndex + 1;
        Tower upgradeTower = BuildManager.main.GetTowerByIndex(upgradeIndex);
        if (upgradeTower == null)
            return int.MaxValue;

        return upgradeTower.cost;
    }

    private bool CanAffordPlacement()
    {
        return LevelManager.main.currency >= GetRequiredCost();
    }
    
    private void ResetPlotColor()
    {
        sr.color = startColor;
    }
}
