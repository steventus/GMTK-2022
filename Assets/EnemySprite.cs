using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    private SpriteRenderer thisRenderer;
    private Transform player;

    private void Start()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        if (player.transform.position.x > transform.position.x)
        {
            //facing right
            thisRenderer.flipX = true;
        }

        else thisRenderer.flipX = false;
    }
}
