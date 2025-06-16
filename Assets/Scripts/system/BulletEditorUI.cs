using UnityEngine;
using TMPro;

public class BulletEditorUI : MonoBehaviour
{


    public TMP_Dropdown typeDropdown;
    public TMP_InputField bulletCountInput;
    public TMP_InputField speedInput;
    public TMP_InputField intervalInput;
    public TMP_InputField durationInput;
    public TMP_InputField spreadAngleInput;
    public ContainerUIManager containerUIManager;
    public BulletSpawn bulletSpawn;

    public BulletData CurrentData { get; private set; } = new BulletData();


    public void ApplyInput()
    {
        try
        {
            CurrentData.bulletCount = int.Parse(bulletCountInput.text);
            CurrentData.speed = float.Parse(speedInput.text);
            CurrentData.interval = float.Parse(intervalInput.text);
            CurrentData.duration = float.Parse(durationInput.text);
            CurrentData.spreadAngle = float.Parse(spreadAngleInput.text);

            CurrentData.type = (BulletType)typeDropdown.value;

            Debug.Log(" BulletData 적용됨:\n" + JsonUtility.ToJson(CurrentData, true));

            bulletSpawn.FirePattern(CurrentData);
            containerUIManager.AddSlot(CurrentData);
            containerUIManager.OpenContainer(CurrentData); //  Canvas 전환
        }
        catch
        {
            Debug.LogError("잘못된 입력입니다");
        }
    }
}
