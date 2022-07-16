using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemey : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
