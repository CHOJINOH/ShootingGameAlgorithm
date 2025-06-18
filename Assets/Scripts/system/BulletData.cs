using UnityEngine;

public enum BulletType
{
    None = 0,
    Radial = 1, 
    Fan = 2  
}
public enum BulletShape
{
    Radial,
    Spiral,
    Wave
}
[System.Serializable]
public class BulletData
{
    public string patternName = "Unnamed";
    public BulletType type = BulletType.None;

    public int bulletCount = 0;
    public float speed = 0f;
    public float interval = 0f;
    public float duration = 0f;
    public float spreadAngle = 0f;
    public float rotationPerShot = 0f;

    public BulletShape shape = BulletShape.Radial;
    public float spiralSpeed = 0f;
    public float waveAmplitude = 0f;
    public float waveFrequency = 0f;
}
