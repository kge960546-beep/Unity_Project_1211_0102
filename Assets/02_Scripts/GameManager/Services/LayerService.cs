using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Layer cache
/// </summary>
public class LayerService : MonoBehaviour, IGameManagementService
{
    public int defaultLayer {  get; private set; }
    public int playerLayer { get; private set; }
    public int enemyLayer { get; private set; }
    public int bossLayer { get; private set; }
    public int playerProjectileLayer { get; private set; }
    public int enemyProjectileLayer { get; private set; }
    public int groundLayer { get; private set; }
    public int destructibleItemLayer { get; private set; }
    public int indestructibleItemLayer { get; private set; }

    private void OnEnable()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        bossLayer = LayerMask.NameToLayer("Boss");
        playerProjectileLayer = LayerMask.NameToLayer("PlayerProjectile");
        enemyProjectileLayer = LayerMask.NameToLayer("EnemyProjectile");
        groundLayer = LayerMask.NameToLayer("Ground");
        destructibleItemLayer = LayerMask.NameToLayer("DestructibleItem");
        indestructibleItemLayer = LayerMask.NameToLayer("IndestructibleItem");
    }
}
