using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;

    private int killCount = 0;
    public int bossSummonThreshold = 4; // 예: 4마리 처치 시 보스 소환
    private bool bossSpawned = false;

    public GameObject bossPrefab; // 에디터에서 할당하거나 Resources에서 로드
    private GameObject bossInstance;



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

        GameObject enemy = Instantiate(
            enemies[randomEnemyIndex],
            spawnPoints[randomSpawnPointIndex].position,
            spawnPoints[randomSpawnPointIndex].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if (randomSpawnPointIndex == 5 || randomSpawnPointIndex == 6)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.linearVelocity = new Vector2(enemyLogic.speed, -1);
        }//leftspawn
        else if(randomSpawnPointIndex == 7 || randomSpawnPointIndex == 8)
        {
            enemy.transform.Rotate(Vector3.back*90);
            rigid.linearVelocity = new Vector2(enemyLogic.speed*(-1), -1);
        }//rightspawn
        else {
            rigid.linearVelocity = new Vector2(0, enemyLogic.speed*(-1));
        }//Frontspawn
    }
    public void RespawnPlayer()
    {
        Invoke("ResetPlayer", 1f);
        
    }

    public void RespawnPlayerEX()
    {
        player.transform.position = new Vector3(0, -4, 0);
        player.SetActive(true);
        player.GetComponent<Player>().health = 3;
        player.GetComponent<Player>().power = 1;
        player.GetComponent<Player>().speed = 5;
    }

    public void OnEnemyKilled()
    {
        killCount++;
        Debug.Log($"적 처치됨! 현재 킬 수: {killCount}");

        if (!bossSpawned && killCount >= bossSummonThreshold)
        {
            bossSpawned = true;
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        bossInstance = Instantiate(bossPrefab, new Vector3(0, 4, 0), Quaternion.identity);
        Debug.Log("보스 소환 완료 (자동 패턴 실행 중)");
    }

}
