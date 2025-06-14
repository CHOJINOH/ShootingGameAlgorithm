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

    public BulletSpawn bulletSpawn;
    public BulletData CurrentData { get; private set; } = new BulletData();

    public void ApplyInput()
    {
        bulletSpawn.FirePattern(CurrentData); // 
        Debug.Log("✅ ApplyInput() 호출됨");
    }
}
