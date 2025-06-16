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
        Debug.Log($" 슬롯 클릭됨: {slotLabel.text}");
        Debug.Log(JsonUtility.ToJson(data, true));
    }
}
