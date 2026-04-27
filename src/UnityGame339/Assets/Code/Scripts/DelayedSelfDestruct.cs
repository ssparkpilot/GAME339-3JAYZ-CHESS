using UnityEngine;

public class DelayedSelfDestruct : MonoBehaviour
{
    public float DelayInSeconds = 1f;
    
    void Start()
    {
        Destroy(gameObject, DelayInSeconds);
    }
}