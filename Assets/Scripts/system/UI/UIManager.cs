using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject editorPanel;
    public GameObject slotPanel;
    public GameObject slotContainer;
    public GameObject bulletSlotPrefab;
    public Button breedButton;
    private List<BulletData> savedSlots = new List<BulletData>();
    private List<BulletSlotUI> selectedSlots = new List<BulletSlotUI>();
    private List<BulletSlotUI> allSlots = new(); // 전체 슬롯 추적
    public BossPatternController bossController;
    public TMP_Text selectedPatternLabel;
    private char currentSlotLetter = 'A';
    public Button deleteButton;
    public int maxSlotCount = 6;
    
    public void ShowSlotPanel(BulletData data)
    {
        editorPanel.SetActive(false);
        slotPanel.SetActive(true);
        AddSlot(data);
    }

    public void AddSlot(BulletData data)
    {
        if (slotContainer.transform.childCount >= maxSlotCount)
        {
            Debug.LogWarning("슬롯 최대 개수 초과!");
            return;
        }

        GameObject slotObj = Instantiate(bulletSlotPrefab, slotContainer.transform);
        var slot = slotObj.GetComponent<BulletSlotUI>();
        slot.Setup(data, $"Slot {currentSlotLetter++}");
        slot.OnSlotSelected = HandleSlotSelection;

        allSlots.Add(slot); // 💡 리스트에 추가!
    }
    public void ExportSlotsToPatternManager()
    {
        if (PatternManager.Instance == null) return;

        PatternManager.Instance.sharedPatterns.Clear();

        foreach (var slot in allSlots)
        {
            var data = slot.GetData();
            if (data != null)
            {
                PatternManager.Instance.sharedPatterns.Add(data);
                Debug.Log($"✅ Pattern 저장됨: {data.patternName}");
            }
        }
    }


    public void ShowEditorPanel()
    {
        slotPanel.SetActive(false);
        editorPanel.SetActive(true);
    }
    private void HandleSlotSelection(BulletSlotUI slot)
    {
        if (slot == null) return;

        if (slot.IsSelected())
        {
            if (!selectedSlots.Contains(slot))
                selectedSlots.Add(slot);
        }
        else
        {
            selectedSlots.Remove(slot);
        }

        // 선택된 슬롯 1개일 때 TMP 텍스트 표시
        if (selectedSlots.Count == 1)
        {
            var data = selectedSlots[0].GetData();
            if (selectedPatternLabel != null)
                selectedPatternLabel.text = $" 선택된 슬롯: {DescribePattern(data)}";
        }
        else
        {
            if (selectedPatternLabel != null)
                selectedPatternLabel.text = "슬롯을 선택하세요.";
        }

        breedButton.interactable = (selectedSlots.Count == 2);
        deleteButton.interactable = (selectedSlots.Count == 1);
    }
    private BulletData CloneData(BulletData source)
    {
        return new BulletData
        {
            type = source.type,
            bulletCount = source.bulletCount,
            speed = source.speed,
            interval = source.interval,
            duration = source.duration,
            spreadAngle = source.spreadAngle,
            rotationPerShot = source.rotationPerShot
        };
    }

    public void OnBreedButtonClicked()
    {
        if (selectedSlots.Count != 2)
        {
            Debug.LogWarning("❗ 2개의 슬롯을 선택해야 교배 가능합니다.");
            return;
        }

        BulletData a = selectedSlots[0].GetData();
        BulletData b = selectedSlots[1].GetData();

        BulletData child = PatternBreeder.Cross(a, b);
        AddSlot(child);
        bossController.AddPattern(child); // ✅ 교배 후 자동 등록
        ClearSlotSelection();
    }

    private void ClearSlotSelection()
    {
        foreach (var slot in selectedSlots)
            slot.Deselect();
        selectedSlots.Clear();
        breedButton.interactable = false;
    }

    public void DeleteSelectedSlot()
    {
        if (selectedSlots.Count != 1)
        {
            return;
        }

        var slot = selectedSlots[0];
        selectedSlots.Clear();
        Destroy(slot.gameObject);
        breedButton.interactable = false;
    }
    private string DescribePattern(BulletData data)
    {
        return $"[{data.patternName}]\n타입: {data.type}\n속도: {data.speed:F1}\n탄수: {data.bulletCount}\n간격: {data.interval:F2}"; ;
    }

}
