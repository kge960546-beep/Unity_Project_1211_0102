using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MergeButton : MonoBehaviour
{
    public void OpenMergeScene()
    {
        SceneManager.LoadScene("MergeScene", LoadSceneMode.Additive);
    }
}
