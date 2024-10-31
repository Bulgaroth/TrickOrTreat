using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    #region Attributes
    public float radius = 1;
    public Vector3 regionSize = Vector3.one;
    public int rejectionSamples = 15;
    public float displayRadius =1;

    List<Vector3> points;

    public bool CanContinueSpawn;


    [SerializeField] private const float SPAWN_COOLDOW = 3.0f;
    [SerializeField] private const float DISTANCE_PLAYER = 10.0f;
    [SerializeField] private const String POOL_TAG = "Enemy_";
    
    public enum EnemyType
    {
        Ghost,
        Clown
    }

    [SerializeField] private List<EnemyType> enemyTypes;
    
    #endregion

    #region Events

    [HideInInspector] public UnityEvent SpawnEnemy;

    #endregion
    
    #region Unity API

    private void Awake()
    {
        Instance = this;
        
        points = PoissonDiscSampling.Get3DPointsFrom2DPointList(radius, new Vector2(regionSize.x, regionSize.z), rejectionSamples);
    }

    void Start()
    {
        StartCoroutine(SpawnCooldownCoroutine());
    }
    
    #endregion

    #region Methods

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 position;
        int security = 20;
        
        do
        {
            if (security <= 0) return Vector3.zero;
            security--;
            
            position = points[Random.Range(0, points.Count)];

        } while (Vector3.Distance(position, GameManager.Instance.GetPlayer().transform.position) < DISTANCE_PLAYER);


        position = points[Random.Range(0, points.Count)];
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            position = hit.position;
            
        }
        
        
        
        
        return position;
    }

    private bool RandomPosition(out Vector3 result)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint =  points[Random.Range(0, points.Count)];
            if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
            {
                Debug.Log($"Hit.position:{hit.position}");
                if (Vector3.Distance(hit.position, GameManager.Instance.GetPlayer().transform.position) >
                    DISTANCE_PLAYER)
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        
        
        
        
        result = Vector3.zero;
        return false;
    }
    
    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        SpawnEnemy.AddListener(OnSpawnEnemy);
    }

    private void OnDisable()
    {
        SpawnEnemy.RemoveListener(OnSpawnEnemy);
    }

    void OnSpawnEnemy()
    {
        var enemy = PoolManager.instance.GetElement(POOL_TAG + enemyTypes[Random.Range(0, enemyTypes.Count)]);
        
        //Debug.Log("Start Spawn enemy");

        if (RandomPosition(out var position))
        {
            Debug.Log("Spawn enemy");
            enemy.transform.position = position;
            enemy.SetActive(true);
        }
    }

    #endregion


    #region Editor

    void OnValidate() {
        points = PoissonDiscSampling.Get3DPointsFrom2DPointList(radius, new Vector2(regionSize.x, regionSize.z), rejectionSamples);
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position + regionSize/2, regionSize);
        if (points != null) {
            foreach (Vector3 point in points) {
                Gizmos.DrawSphere(transform.position + point, displayRadius);
            }
        }
    }

    #endregion

    #region Coroutines

    IEnumerator SpawnCooldownCoroutine()
    {
        SpawnEnemy.Invoke();
        yield return new WaitForSeconds(SPAWN_COOLDOW);
        
        if (CanContinueSpawn)
            StartCoroutine(SpawnCooldownCoroutine());
    }

    #endregion
}
