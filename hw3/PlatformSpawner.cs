using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPatternPrefabs;
    [SerializeField] private Transform platformParent;
    
    [Header("Settings - Match these to Spawner.cs")]
    public float platformSpawnTime = 12f; // Slower spawn rate than regular obstacles
    public float obstacleSpeed = 4f; 
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f; 

    private float _obstacleSpeed;
    private float timeAlive;
    private float timeUntilPlatformSpawn;

    public void Start()
    {
        GameManager.Instance.onGameOver.AddListener(() => ClearPlatforms());
        GameManager.Instance.onGameOver.AddListener(() => ResetFactors());
        GameManager.Instance.onPhaseChange.AddListener(OnPhaseChange);
    }
    
    private void OnPhaseChange(int phase)
    {
        // Keeps speed perfectly synced with ground obstacles
        obstacleSpeedFactor += 0.05f;
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;
            CalculateFactors();
            SpawnLoop();
        }
    }

    private void SpawnLoop()
    {
        timeUntilPlatformSpawn += Time.deltaTime;
        
        // Add a tiny bit of random variance so it doesn't feel perfectly metronomic
        float randomizedSpawnTime = platformSpawnTime + Random.Range(-2f, +2f);
        
        if (timeUntilPlatformSpawn >= randomizedSpawnTime)
        {
            Spawn();
            timeUntilPlatformSpawn = 0f;
        }
    }

    private void ClearPlatforms()
    {
        if (platformParent == null) return;
        foreach (Transform platform in platformParent)
        {
            Destroy(platform.gameObject);
        }
    }

    private void CalculateFactors()
    {
        // This ensures the platforms move at the EXACT same speed as the ground obstacles
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn()
    {
        if (platformPatternPrefabs == null || platformPatternPrefabs.Length == 0) return;

        GameObject patternToSpawn = platformPatternPrefabs[Random.Range(0, platformPatternPrefabs.Length)];
        
        GameObject spawnedPattern = Instantiate(patternToSpawn, transform.position, Quaternion.identity);
        
        if (platformParent != null)
        {
            spawnedPattern.transform.parent = platformParent;
        }

        Rigidbody2D rb = spawnedPattern.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Pushes the entire chunk of platforms leftward!
            rb.linearVelocity = Vector2.left * _obstacleSpeed;
        }
    }
}
