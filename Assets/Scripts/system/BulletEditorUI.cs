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
    public TMP_InputField rotationPerShotInput;
    public UIManager uiManager;
    public BulletSpawn bulletSpawn;

    public BulletData CurrentData { get; private set; } = new BulletData();

    private void Start()
    {
        typeDropdown.onValueChanged.AddListener(OnTypeChanged);
    }

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
            CurrentData.rotationPerShot = float.Parse(rotationPerShotInput.text);
            CurrentData.type = (BulletType)typeDropdown.value;

            Debug.Log("BulletData 적용됨:\n" + JsonUtility.ToJson(CurrentData, true));

            bulletSpawn.FirePattern(CurrentData);

            uiManager.ShowSlotPanel(CurrentData); 
        }
        catch
        {
            Debug.LogError("잘못된 입력입니다");
        }
    }


    private void OnTypeChanged(int selectedIndex)
    {
        switch ((BulletType)selectedIndex)
        {
            case BulletType.None:
                bulletCountInput.text = "1";
                speedInput.text = "3";
                intervalInput.text = "0.5";
                durationInput.text = "2";
                spreadAngleInput.text = "0";
                rotationPerShotInput.text = "0";
                break;

            case BulletType.Radial: // 원형
                bulletCountInput.text = "12";
                speedInput.text = "3";
                intervalInput.text = "0.4";
                durationInput.text = "3";
                spreadAngleInput.text = "360";
                rotationPerShotInput.text = "5"; // 회전 탄막처럼
                break;

            case BulletType.Fan: // 부채꼴
                bulletCountInput.text = "5";
                speedInput.text = "3.5";
                intervalInput.text = "0.35";
                durationInput.text = "2.5";
                spreadAngleInput.text = "90";
                rotationPerShotInput.text = "0";
                break;
        }
    }

}
