using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BossPatternController : MonoBehaviour
{
    public BulletSpawn bulletSpawner;
    public float patternInterval = 5f;

    private List<BulletData> patternList = new List<BulletData>();
    private int currentPatternIndex = 0;

    private void Start()
    {
        StartCoroutine(PatternLoop());
    }

    public void AddPattern(BulletData pattern)
    {
        patternList.Add(pattern);
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            if (patternList.Count > 0)
            {
                var data = patternList[currentPatternIndex % patternList.Count];
                bulletSpawner.FirePattern(data);
                currentPatternIndex++;
            }
            yield return new WaitForSeconds(patternInterval);
        }
    }
}
