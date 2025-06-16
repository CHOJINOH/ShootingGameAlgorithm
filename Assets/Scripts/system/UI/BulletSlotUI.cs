using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BulletSlotUI : MonoBehaviour
{
    public TextMeshProUGUI slotLabel;
    public Image backgroundImage;
    private BulletData data;
    private bool isSelected = false;

    public System.Action<BulletSlotUI> OnSlotSelected;

    public void Setup(BulletData bulletData, string slotName)
    {
        data = bulletData;
        slotLabel.text = slotName;
        Deselect();
    }

    public void OnClick()
    {
        isSelected = !isSelected;
        UpdateVisual();
        OnSlotSelected?.Invoke(this); // UIManager에 알림
    }

    public BulletData GetData() => data;
    public bool IsSelected() => isSelected;

    public void Deselect()
    {
        isSelected = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        backgroundImage.color = isSelected ? Color.yellow : Color.white;
    }
}
