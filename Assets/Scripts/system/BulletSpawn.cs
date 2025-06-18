using System.Collections;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Coroutine currentCoroutine;
    private float currentRotation = 0f; // 누적 회전값

    public void FirePattern(BulletData data)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning(" bulletPrefab이 할당되지 않았습니다.");
            return;
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(FireRoutine(data));
    }

    private IEnumerator FireRoutine(BulletData data)
    {
        float elapsed = 0f;
        currentRotation = 0f; // 회전 초기화

        while (elapsed < data.duration)
        {
            FireBullets(data, currentRotation);
            currentRotation += data.rotationPerShot; // 누적 회전 적용
            yield return new WaitForSeconds(data.interval);
            elapsed += data.interval;
        }
    }

    private void FireBullets(BulletData data, float baseRotation)
    {
        float angleStep = data.spreadAngle / Mathf.Max(data.bulletCount - 1, 1);
        float startAngle = -data.spreadAngle / 2f;

        for (int i = 0; i < data.bulletCount; i++)
        {
            float angle = baseRotation + startAngle + i * angleStep;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.down;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 💡 탄환 방향 설정
            var rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir.normalized * data.speed;

            // 💡 보스 탄환 정보 설정
            var bulletComp = bullet.GetComponent<Bullet>();
            if (bulletComp != null)
            {
                bulletComp.originData = data;
                bulletComp.isFromBoss = true;
            }
        }
    }
}
