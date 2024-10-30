using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreats/HeadBobbing")]
public class SC_HeadBobbingData : ScriptableObject
{
    [Range(0f, 30.0f)] public float frequency;

    [Range(0.01f, 0.1f)] public float amplitude;
}
