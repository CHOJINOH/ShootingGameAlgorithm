using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSlotUI : MonoBehaviour
{
    public TextMeshProUGUI slotLabel;
    private BulletData data;

    public void Setup(BulletData bulletData, string slotName)
    {
        data = bulletData;
        slotLabel.text = slotName;
    }

    public void OnClick()
    {
        Debug.Log($"🔍 {slotLabel.text} 클릭됨: {JsonUtility.ToJson(data, true)}");
        // 나중에 교배 대상 선택 등으로 확장 가능
    }
}
