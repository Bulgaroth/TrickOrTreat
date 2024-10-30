using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region Attributes

    [SerializeField] private const int _baseHP = 5;
    [SerializeField] private int _currentHP;

    [SerializeField] private int damage = 1;
    
    private NavMeshAgent agent;
    private PlayerController player;

    #endregion

    #region Events

    public UnityEvent<int> TakeDamage;
    public UnityEvent Die;

    #endregion
    
    #region Unity API
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _currentHP = _baseHP;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        //transform.LookAt(player.transform, Vector3.up);
        
        Vector3 targetPostition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z );
        transform.LookAt(targetPostition);
        
        agent.SetDestination(player.transform.position);
    }

    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        TakeDamage.AddListener(OnTakeDamage);
        Die.AddListener(OnDie);
    }


    private void OnDisable()
    {
        TakeDamage.RemoveListener(OnTakeDamage);
        Die.RemoveListener(OnDie);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;
    
        other.GetComponent<PlayerController>().TakeDamage.Invoke(damage);
        

    }
    
    void OnTakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} take {damage} damage");
        _currentHP -= damage;
        
        if (_currentHP <= 0)
            Die.Invoke();
    }

    void OnDie()
    {
        Debug.Log($"{gameObject.name} take damage");
        Destroy(gameObject);
    }

    #endregion
}