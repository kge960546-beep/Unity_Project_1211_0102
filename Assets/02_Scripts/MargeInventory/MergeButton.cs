using UnityEngine;
using UnityEngine.SceneManagement;

public class MergeButton : MonoBehaviour
{
    [SerializeField] private GameObject mainScrollView;
    public void OpenMergeScene()
    {
        SceneManager.LoadScene("MergeScene", LoadSceneMode.Additive);       
    }
}
