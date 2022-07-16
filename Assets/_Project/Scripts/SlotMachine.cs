using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using Random = UnityEngine.Random;
public class SlotMachine : MonoBehaviour
{
  public float timer = 2;
  
  private IEnumerator Spin()
  {
        yield return new WaitForSeconds(timer);
        // Stopped Spinning
        FindObjectOfType<WaveManager>().StartNextRound();
        GetComponentInChildren<Animator>().Play("slotMachine_flyOut");
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    if(!col.CompareTag("Player")) return;
    StartCoroutine(Spin());
  }
}
