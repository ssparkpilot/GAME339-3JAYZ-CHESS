using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlowmo : Turret
{
    [Header("References")]
    
    [Header("Attribute")]
    [SerializeField] private float freezeTime = 1f;
    
    
    public AudioClip fireSound;
    
    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            Debug.Log("Slowed Enemies");
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            //make the audiosource play at half the volume
            audioSource.volume = 0.75f;
            //play the fire sound at the randomized pitch
            audioSource.PlayOneShot(fireSound);
            CreateDeathEffect();
            
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        
        em.ResetSpeed();
    }
    
    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
