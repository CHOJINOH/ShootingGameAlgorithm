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
        else if (collision.CompareTag("Enemy"))
        {
            if (isFromBoss && originData != null)
            {
                FitnessManager.Instance.RegisterHit(originData);
            }

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SendMessage("OnHit", dmg);
            }

            Destroy(gameObject);
        }
    }
}
