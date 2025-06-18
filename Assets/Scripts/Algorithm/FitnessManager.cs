using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FitnessManager : MonoBehaviour
{
    private static FitnessManager instance;
    public static FitnessManager Instance => instance;

    private Dictionary<BulletData, int> fitnessScores = new();
    private Dictionary<BulletData, int> totalFired = new();
    private Dictionary<BulletData, int> hits = new();
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
    public void RegisterFire(BulletData data)
    {
        if (data == null) return;
        if (!totalFired.ContainsKey(data)) totalFired[data] = 0;
        totalFired[data]++;
    }
    //public BulletData[] GetTopPatterns(int count)
    //{
    //    var sorted = new List<KeyValuePair<BulletData, int>>(fitnessScores);
    //    sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

    //    List<BulletData> result = new();
    //    for (int i = 0; i < Mathf.Min(count, sorted.Count); i++)
    //        result.Add(sorted[i].Key);

    //    return result.ToArray();
    //}
    public BulletData[] GetTopByHitRate(int count)
    {
        var list = totalFired
            .Where(pair => hits.ContainsKey(pair.Key))
            .Select(pair => new
            {
                pattern = pair.Key,
                hitRate = (float)hits[pair.Key] / pair.Value
            })
            .OrderByDescending(x => x.hitRate)
            .Take(count)
            .Select(x => x.pattern)
            .ToArray();

        return list;
    }
    public BulletData[] GetFallbackTopByUsage(int count)
    {
        return totalFired
            .OrderByDescending(x => x.Value)
            .Take(count)
            .Select(x => x.Key)
            .ToArray();
    }
    public void ClearAll()
    {
        fitnessScores.Clear();
    }
}
