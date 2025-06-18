using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;

    public List<BulletData> sharedPatterns = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Clear()
    {
        sharedPatterns.Clear();
    }
}
