using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
public class BossPatternController : MonoBehaviour
{
    public BulletSpawn bulletSpawner;
    public float patternInterval = 5f;
    public float evolutionInterval = 15f;

    private List<BulletData> patternList = new List<BulletData>();
    private int currentPatternIndex = 0;

    public TMP_Text patternLabel;

    private void Start()
    {
        StartCoroutine(PatternLoop());
        StartCoroutine(EvolutionLoop());
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

                // TMP 텍스트에 출력
                if (patternLabel != null)
                    patternLabel.text = $" 현재 패턴: {DescribePattern(data)}";

                bulletSpawner.FirePattern(data);
                currentPatternIndex++;
            }
            yield return new WaitForSeconds(patternInterval);
        }
    }

    private IEnumerator EvolutionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(evolutionInterval);

            var top2 = FitnessManager.Instance.GetTopPatterns(2);
            if (top2.Length < 2) continue;

            var child = PatternBreeder.Cross(top2[0], top2[1]);

            // 이름 설정
            child.patternName = $"{top2[0].patternName} + {top2[1].patternName}";
            AddPattern(child);

            Debug.Log($"🧬 보스 진화: {child.patternName} 추가됨");
        }
    }

    // 패턴 출력
    private string DescribePattern(BulletData data)
    {
        return $"[{data.patternName}]\n 타입: {data.type}\n, 속도: {data.speed:F1}\n, 탄수: {data.bulletCount},\n 간격: {data.interval:F2}";
    }
}
