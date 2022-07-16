using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusicTrigger : MonoBehaviour
{

    public AudioClip track1, track2;
    private AudioClip oldTrack;
    private MusicManager musicManager;

    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {           
            if (oldTrack == track1)
            {
                musicManager.ChangeBackgroundMusic(track2);
                oldTrack = track2;

            }

            else
            {
                musicManager.ChangeBackgroundMusic(track1);
                oldTrack = track1;
            }
        }
    }

  
}
