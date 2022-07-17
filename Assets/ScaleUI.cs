using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleUI : MonoBehaviour
{
    public string MainSceneToLoad;
    public int sceneNum;

    private Vector3 scale;


    private Vector3 startScale;


    public void MouseOn() => transform.DOScale(new Vector3(2.2f,2.2f,2.2f), 0.5f);
    public void MouseOff() => transform.DOScale(new Vector3(2,2,2), 0.5f);

    public void LoadLevel()
    {
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