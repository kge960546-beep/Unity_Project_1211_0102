using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Registers core GameObjects and provides views for each of them.
/// This service does not instantiate or create any objects.
/// </summary>
public class GameContextService : MonoBehaviour, IGameManagementService
{
    public GameObject Player { get; private set; }
    public GameObject MainCamera { get; private set; }
    public GameObject Background { get; private set; }
    public GameObject BossMonster { get; private set; }

    /// <summary>
    /// Register the player GameObject.
    /// </summary>
    /// <param name="player">Player GameObject to register.</param>
    public void RegisterPlayerObject(GameObject player)
    {
        Player = player;
    }

    /// <summary>
    /// Unregister the player GameObject.
    /// </summary>
    public void UnregisterPlayerObject()
    {
        Player = null;
    }

    /// <summary>
    /// Register the main camera GameObject.
    /// </summary>
    /// <param name="mainCamera">Main camera GameObject to register.</param>
    public void RegisterMainCameraObject(GameObject mainCamera)
    {
        MainCamera = mainCamera;
    }

    /// <summary>
    /// Unregister the main camera GameObject.
    /// </summary>
    public void UnregisterMainCameraObject()
    {
        MainCamera = null;
    }

    /// <summary>
    /// Register the background GameObject.
    /// </summary>
    /// <param name="background">Background GameObject to register.</param>
    public void RegisterBackgroundObject(GameObject background)
    {
        Background = background;
    }

    /// <summary>
    /// Unregister the background GameObject.
    /// </summary>
    public void UnregisterBackgroundObject()
    {
        Background = null;
    }

    /// <summary>
    /// Register the boss monster GameObject.
    /// </summary>
    /// <param name="bossMonster">Boss monster GameObject to register.</param>
    public void RegisterBossMonsterObject(GameObject bossMonster)
    {
        BossMonster = bossMonster;
    }

    /// <summary>
    /// Unregister the boss monster GameObject.
    /// </summary>
    public void UnregisterBossMonsterObject()
    {
        BossMonster = null;
    }
}
