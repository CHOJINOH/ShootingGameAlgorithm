using System.Collections.Generic;
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
    public BossPatternController bossController;

    private char currentSlotLetter = 'A';

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
}

    public void ShowEditorPanel()
    {
        slotPanel.SetActive(false);
        editorPanel.SetActive(true);
    }
    private void HandleSlotSelection(BulletSlotUI clickedSlot)
    {
        if (clickedSlot.IsSelected())
        {
            if (!selectedSlots.Contains(clickedSlot))
                selectedSlots.Add(clickedSlot);
        }
        else
        {
            selectedSlots.Remove(clickedSlot);
        }

        // 두 개 이상 선택 시 교배 버튼 활성화
        breedButton.interactable = selectedSlots.Count == 2;
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


}
