using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemey : MonoBehaviour
{
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        WaveManager.enemies.Remove(gameObject);
        WaveManager.
        Destroy(gameObject);
    }
}
