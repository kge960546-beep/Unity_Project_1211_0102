using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton game manager.
/// Services are separated into parts and can be accessed using their types as keys.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Service")]
    [Tooltip("List of 'service' MonoBehaviours to be created for globally unique 'singleton' GameObject. Requires to extend IGameManagementService.")]
    [SerializeField] private List<MonoBehaviour> serviceDefinitions;
    private Dictionary<Type, IGameManagementService> registeredServices;

    [Header("Misc")]
    [Tooltip("Base event system for UGUI to be converted into a singleton object. Required for additive scene loading.")]
    // should preserve only one unique event system to enable additive scene loading
    [SerializeField] private GameObject eventSystem;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (null != Instance && this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        if (null != eventSystem) DontDestroyOnLoad(eventSystem);

        foreach (MonoBehaviour svc in serviceDefinitions)
        {
            if (svc is not IGameManagementService) continue;
            registeredServices.Add(svc.GetType(), gameObject.AddComponent(svc.GetType()) as IGameManagementService);
        }
    }

    public T GetService<T>() where T : class, IGameManagementService
    {
        return registeredServices[typeof(T)] as T;
    }
}
