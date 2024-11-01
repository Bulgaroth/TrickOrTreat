using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField, Range(0, 1)] private float[] maxVolumes;
	[SerializeField] private float fadeRate = 1;

	private readonly List<AudioSource> tracks = new();
	private int trackLvl;

	private void Awake()
	{
		for (int i = 1; i < transform.childCount; ++i)
			tracks.Add(transform.GetChild(i).GetComponent<AudioSource>());
	}

	public void ChangeTrackLevel(bool add)
	{
		if (add) StartCoroutine(FadeIn(trackLvl++));
		else while(trackLvl > 0) StartCoroutine(FadeOut(--trackLvl));
	}

	private IEnumerator FadeOut(int trackIndex)
	{
		while (tracks[trackIndex].volume >= 0)
		{
			tracks[trackIndex].volume -= Time.deltaTime * fadeRate;
			yield return null;
		}
	}

	private IEnumerator FadeIn(int trackIndex)
	{
		while (tracks[trackIndex].volume <= maxVolumes[trackIndex])
		{
			tracks[trackIndex].volume += Time.deltaTime * fadeRate;
			yield return null;
		}
	}
}
