using UnityEngine;

public static class PatternBreeder
{
    public static BulletData Cross(BulletData a, BulletData b)
    {
        BulletData child = new BulletData();

        child.bulletCount = (a.bulletCount + b.bulletCount) / 2;
        child.speed = (a.speed + b.speed) / 2f;
        child.interval = (a.interval + b.interval) / 2f;
        child.duration = Mathf.Max(a.duration, b.duration);
        child.spreadAngle = (a.spreadAngle + b.spreadAngle) / 2f;
        child.rotationPerShot = (a.rotationPerShot + b.rotationPerShot) / 2f;
        child.type = Random.value > 0.5f ? a.type : b.type;

        return child;
    }
}
