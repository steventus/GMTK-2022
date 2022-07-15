using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Weapons
{
  Rifle,
  Sniper,
  Shotgun,
  Pistol,
  SMG
}
public class SlotMachine : MonoBehaviour
{
  private Weapons Weapons;
  public TMP_Text slotNum;
  public float timer = 2;
  Array values = Enum.GetValues(typeof(Weapons));
  public bool ShouldStop => timer <= 0;
  private void Update()
  {
    if (ShouldStop) return;
    timer -= Time.deltaTime;
  }

  private IEnumerator Spin()
  {

    while (!ShouldStop)
    {
      Weapons = (Weapons)values.GetValue(Random.Range(0,values.Length));
      slotNum.text = Weapons.ToString();
      yield return new WaitForSeconds(0.1f);
    }

    // Stopped Spinning
    Debug.Log($"Selected Gun is {Weapons}");
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    if(!col.CompareTag("Player")) return;
    StartCoroutine(Spin());
  }
}
