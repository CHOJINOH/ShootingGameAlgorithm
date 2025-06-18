using UnityEngine;
using TMPro;

public class BulletEditorUI : MonoBehaviour
{
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown shapeDropdown;

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
        shapeDropdown.onValueChanged.AddListener(OnShapeChanged);

        // 초기 모양 값 적용
        ApplyShapeDefaults((BulletShape)shapeDropdown.value);
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
            CurrentData.rotationPerShot = float.Parse(rotationPerShotInput.text);

            CurrentData.type = (BulletType)typeDropdown.value;
            CurrentData.shape = (BulletShape)shapeDropdown.value;

            // 💡 모양에 따른 추가 속성은 내부에서 기본값 자동 할당
            ApplyShapeDefaults(CurrentData.shape);

            Debug.Log("BulletData 적용됨:\n" + JsonUtility.ToJson(CurrentData, true));

            bulletSpawn.FirePattern(CurrentData);
            uiManager.ShowSlotPanel(CurrentData);
        }
        catch
        {
            Debug.LogError("❌ 잘못된 입력입니다");
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
            case BulletType.Radial:
                bulletCountInput.text = "12";
                speedInput.text = "3";
                intervalInput.text = "0.4";
                durationInput.text = "3";
                spreadAngleInput.text = "360";
                rotationPerShotInput.text = "5";
                break;
            case BulletType.Fan:
                bulletCountInput.text = "5";
                speedInput.text = "3.5";
                intervalInput.text = "0.35";
                durationInput.text = "2.5";
                spreadAngleInput.text = "90";
                rotationPerShotInput.text = "0";
                break;
        }
    }

    private void OnShapeChanged(int selectedIndex)
    {
        ApplyShapeDefaults((BulletShape)selectedIndex);
    }

    // 🔹 모양에 따른 내부 속성 기본값 자동 지정
    private void ApplyShapeDefaults(BulletShape shape)
    {
        switch (shape)
        {
            case BulletShape.Spiral:
                CurrentData.spiralSpeed = 10f;
                break;
            case BulletShape.Wave:
                CurrentData.waveAmplitude = 1f;
                CurrentData.waveFrequency = 5f;
                break;
            case BulletShape.Radial:
            default:
                CurrentData.spiralSpeed = 0f;
                CurrentData.waveAmplitude = 0f;
                CurrentData.waveFrequency = 0f;
                break;
        }
    }
}
