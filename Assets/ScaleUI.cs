using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleUI : MonoBehaviour
{
    public string MainSceneToLoad;
    public int sceneNum;
    private bool playingLevel;

    private Vector3 scale;


    private Vector3 startScale;


    public void MouseOn() => transform.DOScale(new Vector3(2.2f, 2.2f, 2.2f), 0.5f);
    public void MouseOff() => transform.DOScale(new Vector3(2, 2, 2), 0.5f);
    public void InstantLoad()
    {
        SceneManager.LoadScene(MainSceneToLoad);

    }
    public void LoadLevel()
    {
        if (!playingLevel)
            StartCoroutine(DelayLoad());
    }
    private IEnumerator DelayLoad()
    {
        playingLevel = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(MainSceneToLoad);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}