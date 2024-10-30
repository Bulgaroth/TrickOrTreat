using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private const float SPAWN_COOLDOWN = 10f;
    private const string POOL_TAG = "PowerUp_";

    [SerializeField] private string[] spawnedPowerUps;
    [SerializeField] private PoolManager poolManager;

    private bool CanSpawn => cooldownOk && !playerInTheWay && currentSpawnedPowerUp == null;

    private bool cooldownOk;
    private bool playerInTheWay;

    private PowerUp currentSpawnedPowerUp;

    private void Update()
    {
        if (CanSpawn) Spawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) playerInTheWay = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInTheWay = false;
    }

    public void Spawn()
    {
        var powerUpGo = poolManager.GetElement(POOL_TAG + spawnedPowerUps[Random.Range(0, spawnedPowerUps.Length)]);
        currentSpawnedPowerUp = powerUpGo.GetComponent<PowerUp>();
        powerUpGo.transform.position = transform.position + Vector3.up;
        // TODO Anims.

        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(SPAWN_COOLDOWN);
        cooldownOk = true;
    }
}
