using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;
    public int TowerCount => towers.Length;
    public int SelectedTowerIndex => selectedTower;

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

    public void SetSelectedTower(int index)
    {
        selectedTower = index;

        // remove old ghost
        if (previewTower != null)
            Destroy(previewTower);

        // create ghost
        previewTower = Instantiate(towers[index].prefab);
        DisablePreview(previewTower);
    }

    private void Update()
    {
        if (previewTower == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        previewTower.transform.position = mouseWorldPos;
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
}