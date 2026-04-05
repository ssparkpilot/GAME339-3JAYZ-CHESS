using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float bps = 1f; // bullets per second

    private Transform target;
    private float timeUntilFire;

    public AudioSource audioSource;
    public AudioClip placeSound;


    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;


    private void Update(){
        if (LevelManager.main.isGameOver){
            return;
        }
        
        if(target == null){
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange()){
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps){
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot(){
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        audioSource.volume = 0.25f;
        //play the place sound at the randomized pitch
        audioSource.PlayOneShot(placeSound);
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0){
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
