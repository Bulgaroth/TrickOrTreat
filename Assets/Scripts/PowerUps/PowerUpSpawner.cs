using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private const float SPAWN_COOLDOWN = 10f;
    private const float SPAWN_OFFSET = 1f;
    private const string POOL_TAG = "PowerUp_";

    public enum PowerUpType
    {
        Health,
        Coffee,
        Sugar
    }

    [SerializeField] private PowerUpType[] powerUpTypes;

    private bool CanSpawn => cooldownOk && currentSpawnedPowerUp == null;

    private bool cooldownOk = false;

    private PowerUp currentSpawnedPowerUp;

    private void Awake() => StartCoroutine(SpawnTimer());

    private void Update()
    {
        print($"{cooldownOk} && {currentSpawnedPowerUp}");
        if (CanSpawn) Spawn();
    }

    public void Spawn()
    {
        var powerUp = PoolManager.instance.GetElement(POOL_TAG + powerUpTypes[Random.Range(0, powerUpTypes.Length)]).transform;
        currentSpawnedPowerUp = powerUp.GetComponent<PowerUp>();
        powerUp.position = transform.position + Vector3.up * SPAWN_OFFSET;
        powerUp.parent = transform;
        powerUp.gameObject.SetActive(true);

        cooldownOk = false;
        StartCoroutine(SpawnTimer());
		currentSpawnedPowerUp.spawner = this;
    }

    public void PowerUpUsed() => currentSpawnedPowerUp = null;

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(SPAWN_COOLDOWN);
        cooldownOk = true;
    }
}
