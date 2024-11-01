using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreats/Enemy")]
public class SC_EnemyData : ScriptableObject
{
    [Space] public int health;
    public int damage;
    public int scoreGained;

}
