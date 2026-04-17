using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 3f;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 4f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;

    private float _obstacleSpawnTime;
    private float _obstacleSpeed;

    private float timeAlive;
    private float timeUntilObstacleSpawn;

    public void Start()
    {
        GameManager.Instance.onGameOver.AddListener(() => ClearObstacles());
        GameManager.Instance.onGameOver.AddListener(() => ResetFactors());
        GameManager.Instance.onPhaseChange.AddListener(OnPhaseChange);
    }
    
    private void OnPhaseChange(int phase)
    {
        // Boost difficulty when phase increases
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
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles()
    {
        foreach (Transform obstacle in obstacleParent)
        {
            Destroy(obstacle.gameObject);
        }
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        
        // Emergence 1: Height Variation
        // Randomize the Y spawn position slightly so you can't just memorize the jump height.
        // We ensure this ONLY happens to projectile obstacles, so logs stay grounded!
        Vector3 spawnPos = transform.position;
        if (obstacleToSpawn.name.Contains("Projectile"))
        {
            spawnPos.y += Random.Range(0f, 1.5f);
        }

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, spawnPos, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent;

        // Emergence 2: Speed Variability (Clumping)
        // By randomly altering the speed of each obstacle slightly, faster obstacles 
        // will naturally catch up to slower ones, creating emergent "clustered" obstacles out of nowhere!
        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        float individualSpeed = _obstacleSpeed * Random.Range(0.7f, 1.3f);
        obstacleRB.linearVelocity = Vector2.left * individualSpeed;
    }
}