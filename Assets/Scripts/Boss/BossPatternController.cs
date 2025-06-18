using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
public class BossPatternController : MonoBehaviour
{
    public BulletSpawn bulletSpawner;
    public float patternInterval = 5f;
    public float evolutionInterval = 15f;

    private List<BulletData> patternList = new List<BulletData>();
    private int currentPatternIndex = 0;

    public TMP_Text patternLabel;

    [SerializeField] private float moveSpeed = 0.5f;       // 이동 속도
    [SerializeField] private float directionChangeInterval = 2f; // 방향 전환 간격
    [SerializeField] private Vector2 fieldMin = new Vector2(-2.5f, 1f);
    [SerializeField] private Vector2 fieldMax = new Vector2(2.5f, 4.5f);
    private Coroutine moveRoutine;
    private Vector2 moveDirection;

    private void Start()
    {
        StartCoroutine(PatternLoop());
        StartCoroutine(EvolutionLoop());
        if (SceneManager.GetActiveScene().name == "03Shooting")
            moveRoutine = StartCoroutine(RandomMovement());
        if (PatternManager.Instance != null)
        {
            foreach (var pattern in PatternManager.Instance.sharedPatterns)
            {
                AddPattern(pattern);
            }
        }
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

            var top2 = FitnessManager.Instance.GetTopByHitRate(2);
            bool usedFallback = false;

            if (top2.Length < 2)
            {
                Debug.LogWarning("🎯 명중률 기반 진화 불가 → 사용량 기준으로 대체");
                top2 = FitnessManager.Instance.GetFallbackTopByUsage(2);
                usedFallback = true;
            }

            if (top2.Length < 2) continue;

            float mutationRate = usedFallback ? 0.3f : 0.1f;

            var child = PatternBreeder.Cross(top2[0], top2[1], mutationRate);
            child.patternName = $"{top2[0].patternName} + {top2[1].patternName}";
            AddPattern(child);

            Debug.Log($"🧬 보스 진화: {child.patternName} (변이율 {mutationRate * 100:F0}%)");
        }
    }


    // 패턴 출력
    private string DescribePattern(BulletData data)
    {
        return $"[{data.patternName}]\n 타입: {data.type}\n, 속도: {data.speed:F1}\n, 탄수: {data.bulletCount},\n 간격: {data.interval:F2}";
    }
    private IEnumerator RandomMovement()
    {
        while (true)
        {
            moveDirection = Random.insideUnitCircle.normalized;
            float t = 0f;

            while (t < directionChangeInterval)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                // 필드 영역 제한
                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, fieldMin.x, fieldMax.x);
                pos.y = Mathf.Clamp(pos.y, fieldMin.y, fieldMax.y);
                transform.position = pos;

                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
