using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;// Enemy name
    public float speed;
    public int health;

    public Sprite[] sprites;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject player;

    public float maxShotDelay;// Delay between shots
    public float curShotDelay;// Current shot delay

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Fire();
        Reload();
    }
    private void Fire()
    {

        if (curShotDelay < maxShotDelay)
            return;

        if(enemyName == "S")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
        }

        curShotDelay = 0;// Reset the shot delay
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    private void OnHit(int dmg)
    {
        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BorderBullet"))
        {
            // 화면 밖으로 나간 탄환만 파괴
            Destroy(gameObject);
        }
        else if (other.CompareTag("PlayerBullet"))
        {
            // 데미지 적용
            Bullet bullet = other.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            // 적이 아니라 탄환만 파괴
            Destroy(other.gameObject);
        }
    }
}
