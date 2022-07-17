using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugWaveManager : MonoBehaviour
{
    public KeyCode debugSlotmachine;
    void Update()
    {
        if (Input.GetKeyDown(debugSlotmachine))
            GetComponent<WaveManager>().RandomizeSlotMachine();
    }
}
