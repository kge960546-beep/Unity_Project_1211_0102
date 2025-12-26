using System.Collections;
using Unity;
using UnityEngine;

public class BoosSpawnController : MonoBehaviour
{
	[SerializeField] private EnemyData bossData;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private float spawnDelay = 2f;

	[Header("Dependencies")]
	[SerializeField] private SpawnPatternController spawnPatternController;
	[SerializeField] private BossWarningUI warningUI;
	[SerializeField] private BossHpSlider hpSlider;

	private bool isSpawned;

	public void TrySpawnBoss()
	{
		if (isSpawned) return;

		isSpawned = true;
		StartCoroutine(SpawnFlow());
	}
	private IEnumerator SpawnFlow()
	{
		spawnPatternController.StopStage();

		war
	}
}