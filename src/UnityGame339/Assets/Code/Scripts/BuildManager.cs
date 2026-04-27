using UnityEngine;

public enum BuildMode
{
    None,
    PlaceTower,
    Shovel
}

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;
    public int TowerCount => towers.Length;
    public int SelectedTowerIndex => selectedTower;
    
    [Header("Ghost Previews")]
    [SerializeField] private GameObject shovelGhostPrefab;

    private GameObject shovelGhost;

    private void Awake()
    {
        main = this;
    }

    [SerializeField] private Tower[] towers;

    private int selectedTower = -1;
    private GameObject previewTower;

    public Tower GetSelectedTower()
    {
        if (selectedTower < 0 || selectedTower >= towers.Length)
            return null;

        return towers[selectedTower];
    }

    private void Update()
    {
        switch (CurrentMode)
        {
            case BuildMode.PlaceTower:
                if (previewTower != null)
                    FollowMouse(previewTower);
                break;

            case BuildMode.Shovel:
                if (shovelGhost != null)
                    FollowMouse(shovelGhost);
                break;
        }
    }
    
    public GameObject PlaceTower(Vector3 position)
    {
        if (previewTower == null) return null;

        previewTower.transform.position = position;
        EnablePreview(previewTower);
        
        GameObject placed = previewTower;

        Turret turret = placed.GetComponent<Turret>();
        if (turret != null)
            turret.towerIndex = selectedTower;

        previewTower = null;
        selectedTower = -1;

        return placed;
    }
    
    private void DisablePreview(GameObject tower)
    {
        if (tower.TryGetComponent(out Turret turret))
            turret.enabled = false;

        if (tower.TryGetComponent(out Collider2D col))
            col.enabled = false;

        if (tower.TryGetComponent(out SpriteRenderer sr))
            sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void EnablePreview(GameObject tower)
    {
        if (tower.TryGetComponent(out Turret turret))
            turret.enabled = true;

        if (tower.TryGetComponent(out Collider2D col))
            col.enabled = true;

        if (tower.TryGetComponent(out SpriteRenderer sr))
            sr.color = Color.white;
    }
    
    public Tower GetTowerByIndex(int index)
    {
        if (index < 0 || index >= towers.Length)
            return null;

        return towers[index];
    }
    
    public BuildMode CurrentMode { get; private set; } = BuildMode.None;
    
    public void SetSelectedTower(int index)
    {
        ClearPreview();

        selectedTower = index;
        CurrentMode = BuildMode.PlaceTower;

        previewTower = Instantiate(towers[index].prefab);
        DisablePreview(previewTower);
    }

    public void SelectShovel()
    {
        ClearPreview();

        selectedTower = -1;
        CurrentMode = BuildMode.Shovel;

        if (shovelGhost == null && shovelGhostPrefab != null)
            shovelGhost = Instantiate(shovelGhostPrefab);
    }
    
    public void ClearSelection()
    {
        ClearPreview();
        selectedTower = -1;
        CurrentMode = BuildMode.None;
    }
    
    private void ClearPreview()
    {
        if (previewTower != null)
            Destroy(previewTower);

        if (shovelGhost != null)
            Destroy(shovelGhost);
    }
    
    private void FollowMouse(GameObject obj)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        obj.transform.position = pos;
    }
}