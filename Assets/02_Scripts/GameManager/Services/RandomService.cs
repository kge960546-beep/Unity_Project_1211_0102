using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomService : MonoBehaviour, IGameManagementService
{
    public Unity.Mathematics.Random Random { get; private set; }

    private void OnEnable()
    {
        Random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        SceneManager.sceneLoaded += SetSeed;
    }

    private void SetSeed(Scene scene, LoadSceneMode loadSceneMode)
    {
        Random.InitState((uint)System.DateTime.Now.Ticks);
    }
}
