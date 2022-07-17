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
    private bool rolled = false;
  
    private IEnumerator Spin()
    {
        //FindObjectOfType<WaveManager>().StartNextRound();
        yield return null;
    }
    
    public void Initialise()
    {
        rolled = false;
    }

    public void Roll()
    {

        //Roll for new stuff
        int _player = Random.Range(0, FindObjectOfType<UiSlotMachine>().playerUpgrades.Count); //Phaser, Shotgun, Uzi, Sniper, Critical;
        int _enemy = Random.Range(0, FindObjectOfType<UiSlotMachine>().enemyUpgrades.Count); //Upgrade 1, 2, 3, 4, Critical 
        int _arena = Random.Range(0, FindObjectOfType<UiSlotMachine>().arenaPerks.Count); //Double Time, Sugar Rush, No Change, Critical

        Debug.Log("BEFORE: " + "Player: " + _player + "Enemy: " + _enemy + "Arena: " + _arena);


        //Apply new stuff
        //PLAYER CHECK CRITICAL
        if (_player == FindObjectOfType<UiSlotMachine>().playerUpgrades.Count - 1)
        {
            //Roll Secondary
            TemporaryBullletManager _tempWep = GameObject.Find("Player").GetComponent<TemporaryBullletManager>();
            while (_tempWep.oldWeapon = _tempWep.desiredWeapon[_player])
            {
                _player = Random.Range(0, FindObjectOfType<TemporaryBullletManager>().desiredWeapon.Count);
            }
            _tempWep.InitialiseWeapon(_player);

            //Roll Primary
            _player = Random.Range(0, FindObjectOfType<BulletManager>().desiredWeapon.Count);
            RollPlayer(_player);
        }
        else RollPlayer(_player);

        //ENEMY CHECK CRITICAL
        if (_enemy == FindObjectOfType<UiSlotMachine>().enemyUpgrades.Count - 1)
        {
            _enemy = Random.Range(0, 5); //0,1,2,3
            WaveManager.CriticalRoll = true;
            RollEnemy(_enemy);
        }
        else
        {
            WaveManager.CriticalRoll = false;
            RollEnemy(_enemy);
        }

        //ARENA CHECK CRITICAL
        if (_arena == FindObjectOfType<UiSlotMachine>().arenaPerks.Count - 1)
        {
            _arena = Random.Range(0, FindObjectOfType<WaveManager>().ArenaPerk.Perks.Count - 1);
            RollArena(_arena);

            //Apply Critical
        }
        else RollArena(_arena);

        FindObjectOfType<UiSlotMachine>().SlotEnter(_player, _enemy, _arena);
        Debug.Log("AFTER: " + "Player: " + _player + "Enemy: " + _enemy + "Arena: " + _arena);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player")) return;

        if (rolled) return;
        rolled = true;
        Roll();
    }

    private void RollPlayer(int _choice)
    {
        //Player Normal
        BulletManager[] _weapons = GameObject.Find("Player").GetComponents<BulletManager>();
        BulletManager _chosen = null;
        foreach (BulletManager _wep in _weapons)
        {
            _wep.isTemporaryReady = false;

            if (!_wep.isTemporary)
                _chosen = _wep;
        }

        

        _chosen.SlotMachine();

        //Don't choose duplicate weapon
        while (_chosen.oldWeapon = _chosen.desiredWeapon[_choice])
        {
            _choice = Random.Range(0, FindObjectOfType<BulletManager>().desiredWeapon.Count);
        }

        //Initialise Weapon
        _chosen.InitialiseWeapon(_choice);
    }
    private void RollEnemy(int _choice)
    {
        FindObjectOfType<WaveManager>().UpdateEnemyUpgrade(_choice);
    }

    private void RollArena(int _choice)
    {
        WaveManager _waveManager = FindObjectOfType<WaveManager>();

        _waveManager.ArenaPerk.Perks.ForEach((perk => perk.ResetPerks()));
        _waveManager.ArenaPerk.Randomize();
    }
}
