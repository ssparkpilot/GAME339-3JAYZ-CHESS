using UnityEngine;
using Game339.Shared.Models;
using System.Collections;
using UnityEngine.Events;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float baseSpeed = 4f;
    
    private Health health;
    
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    
    [Header("Colors")]
    [SerializeField] private Color FreezeColor = Color.blue;
    
    private Color baseColor;

    private EnemyUnit unit;
    private Coroutine moveRoutine;
    public AudioClip SpawnSound;
    public AudioClip MoveSound;

    public AudioSource SoundSpawner;

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    
    private void Awake()
    {
        health = GetComponent<Health>();
    }
    
    private void Start()
    {
        baseColor = sr.color;

        SoundSpawner.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        //SoundSpawner.volume = 0.25f;
        //play the place sound at the randomized pitch

        if (SoundSpawner != null && SpawnSound != null)
        {
            SoundSpawner.PlayOneShot(SpawnSound);
        }
    }
    
    public void FreezeTint()
    {
        sr.color = FreezeColor;
    }
    
    private void ResetColor()
    {
        if (sr == null)
            return;

        sr.color = baseColor;
    }

    public void Init(EnemyUnit enemyUnit)
    {
        unit = enemyUnit;

        // Snap instantly on spawn
        ChessPlot plot = BoardManager.main.GetPlot(unit.Position);
        if (plot != null)
            transform.position = plot.transform.position;
    }

    public void UpdatePosition()
    {
        
        if (unit == null)
            return;

        ChessPlot targetPlot = BoardManager.main.GetPlot(unit.Position);
        if (targetPlot == null)
            return;

        // Stop previous movement if still running

        SoundSpawner.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        //SoundSpawner.volume = 0.25f;
        //play the place sound at the randomized pitch

        if (SoundSpawner != null && MoveSound != null)
        {
            SoundSpawner.PlayOneShot(MoveSound);
        }

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(targetPlot.transform.position));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, target);
        float t = 0f;

        if (distance < 0.001f)
        {
            transform.position = target;
            yield break;
        }

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed / distance;
            float eased = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(start, target, eased);
            yield return null;
        }

        transform.position = target;
    }

    private void OnMouseEnter()
    {
        if (HoverHealthUI.main == null) return;
        if (unit == null) return;
        if (health == null) return;

        HoverHealthUI.main.Show(transform.position, health.CurrentHP);
    }

    private void OnMouseExit()
    {
        if (HoverHealthUI.main != null)
        {
            HoverHealthUI.main.Hide();
        }
    }
    
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    public void ResetSpeed()
    {
        
        if (this == null)
            return;

        moveSpeed = baseSpeed;
        ResetColor();
    }
}