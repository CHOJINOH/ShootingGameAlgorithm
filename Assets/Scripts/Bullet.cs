using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public BulletData originData;
    public bool isFromBoss = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderBullet"))
        {
            Destroy(gameObject);
        }

        // 플레이어 탄이 적에게
        else if (collision.CompareTag("Enemy") && !isFromBoss)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                enemy.OnHit(dmg); // ✅ SendMessage → 직접 호출

            Destroy(gameObject);
        }

        // 보스 탄이 플레이어에 명중
        else if (collision.CompareTag("Player") && isFromBoss)
        {
            if (originData != null)
                FitnessManager.Instance.RegisterHit(originData);

            Destroy(gameObject);
        }

    }
}
