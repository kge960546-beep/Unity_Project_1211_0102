using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomService : MonoBehaviour, IGameManagementService
{
    public Unity.Mathematics.Random Random { get; private set; }

    private void OnEnable()
    {
        Random = new Unity.Mathematics.Random();
    }
}
