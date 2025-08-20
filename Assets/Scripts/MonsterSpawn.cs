using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnInterval = 3f;
    public Vector2 minSpawnPos;
    public Vector2 maxSpawnPos;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMonster), 1f, spawnInterval);
    }

    void SpawnMonster()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(minSpawnPos.x, maxSpawnPos.x),
            Random.Range(minSpawnPos.y, maxSpawnPos.y)
        );

        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
    }
}