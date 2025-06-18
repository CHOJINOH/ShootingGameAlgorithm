using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;

    private int killCount = 0;
    public int bossSummonThreshold = 4; 
    private bool bossSpawned = false;

    public GameObject bossPrefab;
    private GameObject bossInstance;

    public GameObject retryButton;

    private void Start()
    {
        if (retryButton != null)
            retryButton.SetActive(false);
    }
    private void Update()
    {
        if (!bossSpawned)
        {
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay >= maxSpawnDelay)
            {
                SpawnEnemy();
                maxSpawnDelay = Random.Range(1f, 5f);
                curSpawnDelay = 0f;
            }
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
    }
    public void RetryGame()
    {
        PatternManager.Instance?.RemoveAllEvoPatterns();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재시작
    }
}
