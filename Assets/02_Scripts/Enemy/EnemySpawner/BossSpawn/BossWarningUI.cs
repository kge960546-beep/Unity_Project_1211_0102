using UnityEngine;

public class BossWarningUI : MonoBehaviour
{
    [SerializeField] private GameObject root;

    public void BWShow()
    {
        root.SetActive(true);
    }
    public void BWHide()
    {
        root.SetActive(false);
    }
}
