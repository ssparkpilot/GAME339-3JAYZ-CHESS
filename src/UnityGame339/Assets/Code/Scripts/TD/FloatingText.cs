using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    public float speed = 1;
    
    void Update()
    {
        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
    }

    public void SetText(int amount)
    {
        textMesh.text = "+ " + amount;
    }

}
