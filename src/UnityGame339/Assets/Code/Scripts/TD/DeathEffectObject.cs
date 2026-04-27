using UnityEngine;

public class DeathEffectObject : MonoBehaviour
{
    public GameObject EffectPrefab;
    
    public void CreateDeathEffect()
    {
        Instantiate(EffectPrefab, transform.position, Quaternion.identity);
    }
}
