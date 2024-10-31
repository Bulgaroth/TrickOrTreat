using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
	public enum ChoiceType
	{
		Health, Dmg, Speed, FireRate, PowerUp,
	}

	[SerializeField] private ChoiceType type;
	[SerializeField] private float value;

	public delegate void ApplyChoice(ChoiceType type, float value);
	public event ApplyChoice OnChose;

	public void OnClick() => OnChose?.Invoke(type, value);
}
