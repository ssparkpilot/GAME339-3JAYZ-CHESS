using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TMP_Text waveText;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    
    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // enemies per second
    private bool isSpawning = false;

    public AudioSource audioSource;
    public AudioClip chudSound;
    public AudioClip startSound;
    public AudioClip endSound;


    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Awake()
    {
        onEnemyDestroy.AddListener(onEnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }
    
    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = currentWave.ToString();
            Debug.Log("Wave: " + currentWave);
        }
    }

    private void onEnemyDestroyed()
    {
        enemiesAlive--;
        
        //play the enemy death sound at a random pitch
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        //play the prefanb's death sound at the randomized pitch
        audioSource.PlayOneShot(chudSound); 
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
        
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        audioSource.volume = 0.5f;
        //play the place sound at the randomized pitch
        audioSource.PlayOneShot(startSound);
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        UpdateWaveUI();
        
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        audioSource.volume = 0.5f;
        //play the place sound at the randomized pitch
        audioSource.PlayOneShot(endSound);
        
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn =  enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
        
        audioSource.pitch = Random.Range(minPitch, maxPitch); //make the audiosource play at half the volume
        audioSource.volume = 0.75f; //play the place sound at the randomized pitch
        audioSource.PlayOneShot(prefabToSpawn.GetComponent<EnemyMovement>().SpawnSound);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
    
    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }
}
