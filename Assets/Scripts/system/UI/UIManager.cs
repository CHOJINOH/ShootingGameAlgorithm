using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUIManager : MonoBehaviour
{
    public GameObject editorCanvas;     // BulletEditorUI Canvas
    public GameObject containerCanvas;  // A/B 슬롯 Canvas

    private BulletData currentData;

    public GameObject slotContainer;          // 슬롯이 들어갈 부모 Panel
    public GameObject bulletSlotPrefab;       // 슬롯 버튼 프리팹

    private List<BulletData> savedSlots = new List<BulletData>();
    private char currentSlotLetter = 'A';


    public void OpenContainer(BulletData data)
    {
        currentData = CloneData(data); // 복사해서 저장
        editorCanvas.SetActive(false);
        containerCanvas.SetActive(true);
    }

    public void AddSlot(BulletData data)
    {
        // 데이터 저장
        BulletData copied = CloneData(data);
        savedSlots.Add(copied);

        // 슬롯 버튼 생성
        GameObject slotObj = Instantiate(bulletSlotPrefab, slotContainer.transform);
        var slot = slotObj.GetComponent<BulletSlotUI>();
        slot.Setup(copied, $"Slot {currentSlotLetter}");

        currentSlotLetter++;
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
