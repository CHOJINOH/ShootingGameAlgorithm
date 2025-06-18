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

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float directionChangeInterval = 2f;
    [SerializeField] private Vector2 fieldMin = new Vector2(-2.5f, 1f);
    [SerializeField] private Vector2 fieldMax = new Vector2(2.5f, 4.5f);

    private Coroutine moveRoutine;
    private Vector2 moveDirection;

    private int evolutionCount = 0;

    private void Start()
    {
        StartCoroutine(PatternLoop());
        StartCoroutine(EvolutionLoop());

        if (SceneManager.GetActiveScene().name == "03Shooting")
            moveRoutine = StartCoroutine(RandomMovement());
        if (patternLabel == null)
        {
            var label = GameObject.Find("BossPatternLabel");
            if (label != null)
            {
                patternLabel = label.GetComponent<TMP_Text>();
                Debug.Log("✅ BossPatternLabel 연결됨");
            }
            else
            {
                Debug.LogWarning("⚠️ BossPatternLabel UI 텍스트를 찾을 수 없습니다");
            }
        }
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

            // ✅ 중복 검사
            if (ContainsSamePattern(PatternManager.Instance.sharedPatterns, child))
            {
                Debug.Log("⚠️ 동일한 유전자의 탄막이 이미 존재함 → 진화 무시");
                continue;
            }

            evolutionCount++;
            child.patternName = $"Evo#{evolutionCount} ({top2[0].shape}+{top2[1].shape})";

            AddPattern(child);
            PatternManager.Instance?.sharedPatterns.Add(child);

            Debug.Log($"🧬 보스 진화: {child.patternName} (변이율 {mutationRate * 100:F0}%)");
        }
    }

    private string DescribePattern(BulletData data)
    {
        return $"[{data.patternName}]\n 타입: {data.type}\n 모양: {data.shape}\n 속도: {data.speed:F1}, 탄수: {data.bulletCount}, 간격: {data.interval:F2}";
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

                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, fieldMin.x, fieldMax.x);
                pos.y = Mathf.Clamp(pos.y, fieldMin.y, fieldMax.y);
                transform.position = pos;

                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    // 🔍 두 유전자가 동일한지 비교
    private bool IsSamePattern(BulletData a, BulletData b)
    {
        return a.type == b.type &&
               a.shape == b.shape &&
               a.bulletCount == b.bulletCount &&
               Mathf.Approximately(a.speed, b.speed) &&
               Mathf.Approximately(a.interval, b.interval) &&
               Mathf.Approximately(a.duration, b.duration) &&
               Mathf.Approximately(a.spreadAngle, b.spreadAngle) &&
               Mathf.Approximately(a.rotationPerShot, b.rotationPerShot) &&
               Mathf.Approximately(a.spiralSpeed, b.spiralSpeed) &&
               Mathf.Approximately(a.waveAmplitude, b.waveAmplitude) &&
               Mathf.Approximately(a.waveFrequency, b.waveFrequency);
    }

    private bool ContainsSamePattern(List<BulletData> list, BulletData newData)
    {
        foreach (var existing in list)
        {
            if (IsSamePattern(existing, newData))
                return true;
        }
        return false;
    }
}
