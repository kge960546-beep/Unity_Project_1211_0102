using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomService : MonoBehaviour, IGameManagementService
{
    public Unity.Mathematics.Random random;

    private void OnEnable()
    {
        random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        SceneManager.sceneLoaded += SetSeed;
    }

    private void SetSeed(Scene scene, LoadSceneMode loadSceneMode)
    {
        random.InitState((uint)System.DateTime.Now.Ticks);
    }
}
