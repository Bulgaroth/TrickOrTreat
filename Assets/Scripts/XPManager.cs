using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
	[SerializeField] private Slider xpBar;

	[SerializeField] private Transform[] slotParents;

	[SerializeField] private GameObject choiceMenu;
	[SerializeField] private GameObject[] choicePrefabs;

	[SerializeField] private PlayerController playerController;

	private int currentXP;
	private int nextLvlXP = 2;

	#region Singleton

	public static XPManager instance;
	private void Awake() => instance = this;
	#endregion

	private void Start() => xpBar.maxValue = nextLvlXP;

	public void AddXP(int amount)
	{
		int nextXP = currentXP + amount;
		if (nextXP < nextLvlXP) currentXP += amount;
		else
		{
			NextLvl();
			if (nextXP > nextLvlXP) currentXP = nextXP - nextLvlXP;
		}

		UpdateXPBar();
	}

	private void NextLvl()
	{
		currentXP = 0;
		nextLvlXP = Mathf.RoundToInt(nextLvlXP * 1.5f);
		UpdateXPBar(true);
		ShowNextLvlChoices();
	}

	private void UpdateXPBar(bool newMax = false)
	{
		if (newMax) xpBar.maxValue = nextLvlXP;
		xpBar.value = currentXP;
	}

	private void ShowNextLvlChoices()
	{
		List<GameObject> prefabStack = new(choicePrefabs);
		for(int i=0; i<3; ++i)
		{
			int prefabIndex = Random.Range(0, prefabStack.Count);
			GameObject obj = Instantiate(prefabStack[prefabIndex], slotParents[i]);
			obj.GetComponent<ChoiceButton>().OnChose += UpdatePlayerData;
			prefabStack.RemoveAt(prefabIndex);
		}

		choiceMenu.SetActive(true);
		// TODO pause.
		// TODO Lock camera & free cursor.
	}

	private void UpdatePlayerData(ChoiceButton.ChoiceType type, float value)
	{
		switch (type)
		{
			case ChoiceButton.ChoiceType.Health: playerController.AddMaxLife.Invoke((int)value); break;
			case ChoiceButton.ChoiceType.FireRate: playerController.AddFireRate.Invoke(value); break;
			case ChoiceButton.ChoiceType.Dmg: playerController.AddDmg.Invoke((int)value); break;
			case ChoiceButton.ChoiceType.Speed: playerController.AddSpeed.Invoke(value); break;
			case ChoiceButton.ChoiceType.PowerUp: playerController.AddPowerUpEfficiency.Invoke(value); break;
		}

		HideNextLvlChoices();
	}

	private void HideNextLvlChoices()
	{
		// TODO unpause
		// TODO Free camera & lock cursor.
		choiceMenu.SetActive(false);
	}
}
