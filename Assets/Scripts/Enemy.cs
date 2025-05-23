using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public Sprite[] sprites;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = Vector2.down * speed;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderBullet"))
        {
            // 화면 밖으로 나간 탄환만 파괴
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("PlayerBullet"))
        {
            // 데미지 적용
            Bullet bullet = collision.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            // 적이 아니라 탄환만 파괴
            Destroy(collision.gameObject);
        }
    }
}
