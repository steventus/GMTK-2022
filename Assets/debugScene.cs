using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class debugScene : MonoBehaviour
{
    public UiSlotMachine thisMachine;
    public SlotMachine thisSlot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            thisSlot.Initialise();
            thisSlot.Roll();
        }
    }
}
