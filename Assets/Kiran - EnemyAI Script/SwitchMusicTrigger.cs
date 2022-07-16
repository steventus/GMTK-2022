using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusicTrigger : MonoBehaviour
{

    public AudioClip newTrack;

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
            if (newTrack != null)
            {
                musicManager.ChangeBackgroundMusic(newTrack);
                Debug.Log("Music Changed!");
            }
        }
    }

  
}
