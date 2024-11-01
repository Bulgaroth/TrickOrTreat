using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	// attributes
	[SerializeField] private GameManager gameManager;
	[SerializeField] TextMeshProUGUI TimerText;
	[SerializeField] float starterTime;

	float remaningTime;

	public bool paused;

	// methods
	private void Start()
	{
		remaningTime = starterTime;
		TimerText.color = Color.white;
	}

	void Update()
	{
		if (paused) return;

		remaningTime -= Time.deltaTime;
		

		if (remaningTime <= 0)
		{
			remaningTime = 0;
			TimerText.color = Color.green;
			gameManager.OnWin();
			paused = true;
		}

		int minutes = Mathf.RoundToInt(remaningTime / 60);
		int secondes = Mathf.RoundToInt(remaningTime % 60);
		TimerText.text = string.Format("{0:00}:{1:00}", minutes, secondes);
	}
}
