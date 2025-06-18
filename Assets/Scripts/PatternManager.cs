using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;

    public List<BulletData> sharedPatterns = new(); // 게임 전반에서 공유되는 탄막 유전자

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 살아남음
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RemoveAllEvoPatterns()
    {
        sharedPatterns.RemoveAll(p => p.patternName.StartsWith("Evo#"));
    }
    public void Clear()
    {
        sharedPatterns.Clear();
    }
}
