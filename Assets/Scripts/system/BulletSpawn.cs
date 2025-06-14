using System.Collections;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
   
    public GameObject bulletPrefab;

    public void FirePattern(BulletData data)
    {
        StartCoroutine(FireRoutine(data));
    }

    private IEnumerator FireRoutine(BulletData data)
    {
        float elapsed = 0f;

        while (elapsed < data.duration)
        {
            FireBullets(data);
            yield return new WaitForSeconds(data.interval);
            elapsed += data.interval;
        }
    }

    private void FireBullets(BulletData data)
    {
        float angleStep = data.spreadAngle / Mathf.Max(data.bulletCount - 1, 1);
        float startAngle = -data.spreadAngle / 2f;

        for (int i = 0; i < data.bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * data.speed;
        }
    }
}
