using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

	[SerializeField] private AudioSource musicSource;

	// Use this for initialization
	void Start()
	{
		musicSource = GetComponentInChildren<AudioSource>();
		StartCoroutine(Delay());
	}

	public void ChangeBackgroundMusic(AudioClip music)
    {
		musicSource.Stop();
		musicSource.clip = music;
		musicSource.Play();
    }

	IEnumerator Delay()
    {
		yield return new WaitForSeconds(1f);
		musicSource.Play();
	}

}
