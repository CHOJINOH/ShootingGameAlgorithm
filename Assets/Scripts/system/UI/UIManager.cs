using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject editorPanel;
    public GameObject slotPanel;
    public GameObject slotContainer;
    public GameObject bulletSlotPrefab;

    private List<BulletData> savedSlots = new List<BulletData>();
    private char currentSlotLetter = 'A';

    public void ShowSlotPanel(BulletData data)
    {
        editorPanel.SetActive(false);
        slotPanel.SetActive(true);
        AddSlot(data);
    }

    public void AddSlot(BulletData data)
    {
        BulletData copied = CloneData(data);
        savedSlots.Add(copied);

        GameObject slotObj = Instantiate(bulletSlotPrefab, slotContainer.transform);
        var slot = slotObj.GetComponent<BulletSlotUI>();
        slot.Setup(copied, $"Slot {currentSlotLetter++}");
    }
    public void ShowEditorPanel()
    {
        slotPanel.SetActive(false);
        editorPanel.SetActive(true);
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
}
