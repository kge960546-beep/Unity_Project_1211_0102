using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void ShowLevelUp()
    {
        rect.localScale = Vector3.one;
    }
    public void HideLevelUp()
    {
        rect.localScale = Vector3.zero;
    }
}
