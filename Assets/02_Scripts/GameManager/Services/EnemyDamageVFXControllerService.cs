using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageVFXControllerService : MonoBehaviour, IGameManagementService
{
    private int playerLayer = -1;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneUnloaded -= SceneUnoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (null == scene) return;
        if (scene.name.StartsWith("PlayerStage")) OnInGameEnter();
    }

    private void SceneUnoaded(Scene scene)
    {
        if (null == scene) return;
        if (scene.name.StartsWith("PlayerStage")) OnInGameExit();
    }


    private void OnInGameEnter()
    {
        playerLayer = GameManager.Instance.GetService<LayerService>().playerLayer;
        GameManager.Instance.GetService<DamageManagementService>().SubscribeOnDamageEvent(ShowVFX);
    }

    private void OnInGameExit()
    {
        GameManager.Instance.GetService<DamageManagementService>().UnsubscribeOnDamageEvent(ShowVFX);
    }

    /// <summary>
    /// int: damage value
    /// GameObject: damage source
    /// IDamageable: damage target
    /// bool: boolean whether the damage was critical damage
    /// </summary>
    private void ShowVFX(int damage, GameObject source, IDamageable target, bool isCritical)
    {
        MonoBehaviour targetMB = target as MonoBehaviour;
        if (null == source || null == targetMB) return;
        if (targetMB.gameObject.layer == playerLayer) return;

        RandomService rs = GameManager.Instance.GetService<RandomService>();

        Vector2 position = (Vector2)targetMB.transform.position + (Vector2)rs.random.NextFloat2(-0.2f, 0.2f);
        DynamicTextManager.CreateText2D(position, FormatWithSIPrefixes(damage), DynamicTextManager.defaultData);
    }

    private string FormatWithSIPrefixes(int value)
    {
        if (value >= 1000000000) return (value / 1000000000f).ToString("0.#") + "G";
        if (value >= 1000000) return (value / 1000000f).ToString("0.#") + "M";
        if (value >= 1000) return (value / 1000f).ToString("0.#") + "K";
        return value.ToString("0");
    }
}
