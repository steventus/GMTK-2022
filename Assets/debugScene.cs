using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class debugScene : MonoBehaviour
{
    public UiSlotMachine thisMachine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("test");
            thisMachine.SlotBegin(0, 0, 0);
        }
    }
}
