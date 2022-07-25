using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class MusicTrack
{
	public AudioClip battle, idle;
}

public class MusicManager : MonoBehaviour
{
	public List<MusicTrack> listsOfMusic;
	[Range(0,2)] public float timeToSwap;

	private AudioSource musicSource;
	private AudioClip currentBattle, currentIdle;
	private bool fading = false;
	private int selector = 0;

	// Use this for initialization
	void Start()
	{
		DOTween.Init(false, false);
		musicSource = GetComponent<AudioSource>();

		Initialise();
	}
    private void OnEnable()
    {
		Messenger.AddListener(GameEvent.SpawnNewRound, ChangeList);
		Messenger.AddListener(GameEvent.OnRoundEnd, ChangeToIdle);
    }
    private void OnDisable()
    {
		Messenger.RemoveListener(GameEvent.SpawnNewRound, ChangeList);
		Messenger.RemoveListener(GameEvent.OnRoundEnd, ChangeToIdle);
	}
	private void Initialise()
    {
		currentBattle = listsOfMusic[selector].battle;
		currentIdle = listsOfMusic[selector].idle;
    }
    public void ChangeList()
    {
		selector++;
		if (selector > listsOfMusic.Count)
			selector = 0;

		currentBattle = listsOfMusic[selector].battle;
		currentIdle = listsOfMusic[selector].idle;

		ChangeTrack(currentBattle);
    }

	public void ChangeTrack(AudioClip _desiredClip)
    {
		StartCoroutine(FadeTrack(_desiredClip));
    }

	public void ChangeToIdle()
    {
		StartCoroutine(FadeTrack(currentIdle));
	}

	private IEnumerator FadeTrack(AudioClip _clip)
    {
		if (!fading)
        {
			fading = true;
			musicSource.DOFade(0, timeToSwap / 2);
			yield return new WaitForSeconds(timeToSwap / 2);
			musicSource.clip = _clip;
			musicSource.Play();
			musicSource.DOFade(1, timeToSwap / 2);
			yield return new WaitForSeconds(timeToSwap / 2);
			fading = false;
		}
	}
}
