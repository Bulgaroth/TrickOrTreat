using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class XPManager : MonoBehaviour
{
	private const int NB_KILLS_TREAT = 1;
	private const int NB_KILLS_DOUBLE_TREAT = 2;
	private const int NB_KILLS_TRIPLE_TREAT = 3;
	private const int NB_KILLS_MONSTER_TREAT = 4;
	private const int NB_KILLS_SUGAR_RUSH = 5;

	private const float COMBO_DECAY_TIME = 10;

	[SerializeField] private Slider xpBar;
	[SerializeField] private Image ringBar;
	[SerializeField] private Image medalImg;
	[SerializeField] private Sprite[] medalSprites;
	[SerializeField] private GameObject medal;

	[SerializeField] private Transform[] slotParents;

	[SerializeField] private GameObject choiceMenu;
	[SerializeField] private GameObject[] choicePrefabs;

	[SerializeField] private PlayerController playerController;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private MusicManager musicManager;

	[SerializeField] private AudioClip[] narratorComments;
	[SerializeField] private AudioSource aS;
	[SerializeField] private TextMeshProUGUI scoreTxt;

	private int score;
	private int scoreIncrement = 1;

	private int currentXP;
	private int nextLvlXP = 300;

	private int killStreak;
	private float comboTimer = 10;
	private bool onCombo;

	private bool paused;

	private GameObject[] currentChoices = new GameObject[3];

	#region Singleton

	public static XPManager instance;
	private void Awake() => instance = this;
	#endregion

	private void Start()
	{
		xpBar.maxValue = nextLvlXP;
		playerController.TakeDamage.AddListener(StopKillStreak);
	}
	

	private void Update()
	{
		if (!onCombo || paused) return;

		comboTimer -= Time.deltaTime;
		UpdateComboBar();
		if (comboTimer <= 0) StopKillStreak();
	}

	public void RegisterDeath(int scoreGained)
	{
		if (++currentXP == nextLvlXP) NextLvl();

		UpdateXPBar();
		comboTimer = COMBO_DECAY_TIME;
		UpdateComboBar();
		score += scoreIncrement * scoreGained;
		scoreTxt.text = $"Score : {score}";
		UpdateKillStreak();
	}

	public void StopKillStreak(int _ = 0)
	{
		killStreak = 0;
		onCombo = false;
		LeanTween.moveLocalX(medal, -1400, 0.2f).setEaseInOutBack();
		musicManager.ChangeTrackLevel(false);
	}

	private void NextLvl()
	{
		aS.PlayOneShot(narratorComments[^1]);
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

	private void UpdateComboBar() => ringBar.fillAmount = comboTimer / COMBO_DECAY_TIME;

	private void StartCombo()
	{
		comboTimer = COMBO_DECAY_TIME;
		onCombo = true;
		ringBar.gameObject.SetActive(true);
	}

	private void UpdateKillStreak()
	{
		switch(++killStreak)
		{
			case NB_KILLS_TREAT:
				aS.PlayOneShot(narratorComments[0]);
				medalImg.sprite = medalSprites[0];
				LeanTween.moveLocalX(medal, -860, 0.2f).setEaseInOutBack().setOnComplete(StartCombo);
				score += 100;
				break;
			case NB_KILLS_DOUBLE_TREAT:
				aS.PlayOneShot(narratorComments[1]);
				medalImg.sprite = medalSprites[1];
				musicManager.ChangeTrackLevel(true);
				score += 200;
				break;
			case NB_KILLS_TRIPLE_TREAT:
				aS.PlayOneShot(narratorComments[2]);
				medalImg.sprite = medalSprites[2];
				musicManager.ChangeTrackLevel(true);
				score += 300;
				break;
			case NB_KILLS_MONSTER_TREAT:
				aS.PlayOneShot(narratorComments[3]);
				medalImg.sprite = medalSprites[3];
				musicManager.ChangeTrackLevel(true);
				score += 1000;
				break;
			case NB_KILLS_SUGAR_RUSH:
				aS.PlayOneShot(narratorComments[4]);
				medalImg.sprite = medalSprites[4];
				musicManager.ChangeTrackLevel(true);
				score += 2000;
				scoreIncrement = 3;
				scoreTxt.color = Color.yellow;
				break;
		}
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
			currentChoices[i] = obj;
		}
		paused = true;
		choiceMenu.SetActive(true);
		playerController.ToggleCamera(false);
		gameManager.Pause();
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
		foreach(var obj in currentChoices) Destroy(obj);
		choiceMenu.SetActive(false);
		playerController.ToggleCamera(true);
		gameManager.Play();
		paused = false;
	}
}
