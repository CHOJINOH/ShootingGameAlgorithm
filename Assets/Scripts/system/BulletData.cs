using UnityEngine;

public enum BulletType
{
    None = 0,
    Radial = 1, 
    Fan = 2  
}

[System.Serializable]
public class BulletData
{
    public BulletType type = BulletType.None;

    public int bulletCount = 0;
    public float speed = 0f;
    public float interval = 0f;
    public float duration = 0f;
    public float spreadAngle = 0f;
    public float rotationPerShot = 0f;
}
