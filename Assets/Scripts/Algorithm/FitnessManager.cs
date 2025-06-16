using System.Collections.Generic;
using UnityEngine;

public class FitnessManager : MonoBehaviour
{
    private static FitnessManager instance;
    public static FitnessManager Instance => instance;

    private Dictionary<BulletData, int> fitnessScores = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void RegisterHit(BulletData data)
    {
        if (data == null) return;
        if (!fitnessScores.ContainsKey(data))
            fitnessScores[data] = 0;

        fitnessScores[data]++;
        //Debug.Log($"[Fitness] {data.name} → 명중 횟수: {fitnessScores[data]}");
    }

    public BulletData[] GetTopPatterns(int count)
    {
        var sorted = new List<KeyValuePair<BulletData, int>>(fitnessScores);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

        List<BulletData> result = new();
        for (int i = 0; i < Mathf.Min(count, sorted.Count); i++)
            result.Add(sorted[i].Key);

        return result.ToArray();
    }

    public void ClearAll()
    {
        fitnessScores.Clear();
    }
}
