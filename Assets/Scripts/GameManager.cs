using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay >= maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(1f, 5f); // Randomize the spawn delay for the next enemy
            curSpawnDelay = 0f;
        }
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemies.Length);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemies[randomEnemyIndex], spawnPoints[randomSpawnPointIndex].position, spawnPoints[randomSpawnPointIndex].rotation);
    }
}
