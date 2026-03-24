using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    public GameObject CowPrefab;
    public float spawnDelay = 2f;
    public float minSpawnHeight = -2f;
    public float maxSpawnHeight = 2f;

    private void Start()
    {
        InvokeRepeating("SpawnCow", 0f, spawnDelay);
    }

    private void SpawnCow()
    {
        float randomHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
        Vector2 spawnPosition = new Vector2(transform.position.x, randomHeight);
        Instantiate(CowPrefab, spawnPosition, Quaternion.identity);
    }
}