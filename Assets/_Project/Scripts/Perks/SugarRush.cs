using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SugarRush : Perk
{

    public PlayerController PlayerController;

    public override void RunPerk()
    {
        usedPerk = true;
        PlayerController.runSpeed = 2;

        var totalEnemies = GameObject.FindGameObjectsWithTag("Fake").ToList();
        foreach (var enemy in totalEnemies)
        {
            enemy.GetComponent<EnemyMovement>().speed *= 2;
        }
    }

    public override void ResetPerks()
    {
        usedPerk = false;
    }
}
