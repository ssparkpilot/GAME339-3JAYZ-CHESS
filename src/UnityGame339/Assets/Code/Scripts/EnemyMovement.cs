using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;
    
    [Header("Sprites")]
    [SerializeField] private Sprite UpSprite;
    [SerializeField] private Sprite DownSprite;
    [SerializeField] private Sprite LeftSprite;
    [SerializeField] private Sprite RightSprite;

    public AudioClip SpawnSound;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }
    
    private void Update() {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length){
                LevelManager.main.LoseHealth(15);
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            } else {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate() {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
        Debug.Log("direction X: " + direction.x + ", direction Y: " + direction.y);

        //change the sprite based on the direction
        if (Mathf.RoundToInt(direction.y) < 0&& Mathf.RoundToInt(direction.x) == 0)
        {
            GetComponent<SpriteRenderer>().sprite  = DownSprite;

        } else if (Mathf.RoundToInt(direction.y) > 0 && Mathf.RoundToInt(direction.x) == 0) {
            GetComponent<SpriteRenderer>().sprite  = UpSprite;

        }
         else if (Mathf.RoundToInt(direction.x) < 0) {
            GetComponent<SpriteRenderer>().sprite  = LeftSprite;

        }  else if (Mathf.RoundToInt(direction.x) > 0) {
            GetComponent<SpriteRenderer>().sprite  = RightSprite;

        }
        else
        {
            //GetComponent<SpriteRenderer>().sprite  = DownSprite;

        }
        
    }
    
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}
