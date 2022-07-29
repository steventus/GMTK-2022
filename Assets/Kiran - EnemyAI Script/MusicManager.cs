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
	[Range(0,10)] public float timeToSwap;

	public AudioSource battleSource, idleSource;
	private bool fading = false;
	public int selector = 0;

	// Use this for initialization
	void Start()
	{
		DOTween.Init(false, false);

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
		battleSource.clip = listsOfMusic[selector].battle;
		idleSource.clip = listsOfMusic[selector].idle;

		battleSource.volume = 1;
		idleSource.volume = 0;

		battleSource.Play();
		idleSource.Play();
    }
    public void ChangeList()
    {
		selector++;
		if (selector > listsOfMusic.Count-1)
			selector = 0;

		Initialise();
    }

	public void ChangeTrack(AudioSource _desiredSource)
    {
		StartCoroutine(FadeTrack(_desiredSource));
    }

	public void ChangeToIdle()
    {
		StartCoroutine(FadeTrack(idleSource));
	}

	private IEnumerator FadeTrack(AudioSource _desiredSource)
    {
		if (!fading)
        {
			fading = true;

			Sequence _seq = DOTween.Sequence();


			foreach (AudioSource _source in GetComponents<AudioSource>())
            {
				if (_source != _desiredSource)
					_seq.Append(_source.DOFade(0, timeToSwap));
            }

			_seq.Join(_desiredSource.DOFade(1, timeToSwap));
			
			yield return new WaitForSeconds(timeToSwap);
			fading = false;
		}
	}
}
