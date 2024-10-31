using System;
using UnityEngine;

public class Deleting : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        //deleting the clone when he hit an other colider

        if (other.transform.CompareTag("Player")) return;

        Debug.Log("Hit");

        if (other.transform.CompareTag("Enemy"))
            other.gameObject.GetComponent<Enemy>().TakeDamage.Invoke(damage);

        Destroy(gameObject);
    }
}