using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(int.MaxValue)]
public class PlayerFeedService : MonoBehaviour, IGameManagementService
{
    public Vector2 userInputDirection = Vector2.zero;
    public Vector2 playerPosition = Vector2.zero;
}
