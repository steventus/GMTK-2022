using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using Random = UnityEngine.Random;
public class SlotMachine : MonoBehaviour
{
    public bool rolled = false;

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
        //Ini
        bool _playerCrit = false;
        bool _enemyCrit = false;
        bool _arenaCrit = false;


        //Roll for new stuff
        int _player = Random.Range(0, FindObjectOfType<UiSlotMachine>().playerUpgrades.Count); //Phaser, Shotgun, Uzi, Sniper, Critical;
        int _enemy = Random.Range(0, FindObjectOfType<UiSlotMachine>().enemyUpgrades.Count); //Upgrade 1, 2, 3, 4, Critical 
        int _arena = Random.Range(0, FindObjectOfType<UiSlotMachine>().arenaPerks.Count); //Double Time, Sugar Rush, No Change, Critical

        Debug.Log("BEFORE: " + "Player: " + _player + "Enemy: " + _enemy + "Arena: " + _arena);

        //Apply new stuff
        //PLAYER CHECK CRITICAL
        if (_player == FindObjectOfType<UiSlotMachine>().playerUpgrades.Count - 1)
        {
            BulletManager[] _weapons = GameObject.Find("Player").GetComponents<BulletManager>();
            BulletManager _temporaryWeapon = null;

            //Roll Secondary
            foreach (BulletManager _temp in _weapons)
            {
                if (_temp.isTemporary)
                    _temporaryWeapon = _temp;
            }

            _player = Random.Range(0, _temporaryWeapon.desiredWeapon.Count);

            //Don't choose duplicate weapon
            if (_temporaryWeapon.oldWeapon != null)
                if (_temporaryWeapon.oldWeapon = _temporaryWeapon.desiredWeapon[_player])
                {
                    List<int> _choices = new List<int>() { 0, 1, 2, 3 };
                    _choices.Remove(_player);

                    _player = _choices[Random.Range(0, _choices.Count)];
                }

            //Initialise Weapon
            _temporaryWeapon.InitialiseWeapon(_player);

            //Roll Primary
            _player = Random.Range(0, FindObjectOfType<BulletManager>().desiredWeapon.Count);
            RollPlayer(_player);
            Messenger<bool>.Broadcast(UiEvent.player_gunChangeCrit, true);

            _playerCrit = true;
        }
        else 
        { 
            RollPlayer(_player);
            Messenger<bool>.Broadcast(UiEvent.player_gunChangeCrit, false);
        }

        //ENEMY CHECK CRITICAL
        if (_enemy == FindObjectOfType<UiSlotMachine>().enemyUpgrades.Count - 1)
        {
            _enemy = Random.Range(0, 5); //0,1,2,3
            WaveManager.CriticalRoll = true;
            RollEnemy(_enemy);
            Messenger<bool>.Broadcast(UiEvent.enemy_upgradeChangeCrit, true);
            _enemyCrit = true;
        }
        else
        {
            WaveManager.CriticalRoll = false;
            RollEnemy(_enemy);
            Messenger<bool>.Broadcast(UiEvent.enemy_upgradeChangeCrit, false);
        }

        //ARENA CHECK CRITICAL
        if (_arena == FindObjectOfType<UiSlotMachine>().arenaPerks.Count - 1)
        {
            _arena = Random.Range(0, FindObjectOfType<WaveManager>().ArenaPerk.Perks.Count - 1);
            //Apply Critical
            RollArenaCrit(_arena);
            Messenger<bool>.Broadcast(UiEvent.arena_perkChangeCrit, true);

            _arenaCrit = true;
        }
        else 
        {
            RollArena(_arena);
            Messenger<bool>.Broadcast(UiEvent.arena_perkChangeCrit, false);
        }

        FindObjectOfType<UiSlotMachine>().SlotEnter(_player, _enemy, _arena, _playerCrit, _enemyCrit, _arenaCrit);

        Debug.Log("AFTER: " + "Player: " + _player + "Enemy: " + _enemy + "Arena: " + _arena);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (rolled) return;

        Debug.Log("test");
        rolled = true;
        Roll();
    }

    private void RollPlayer(int _choice)
    {
        //Player Normal
        BulletManager[] _weapons = GameObject.Find("Player").GetComponents<BulletManager>();
        BulletManager _chosen = null;

        //Choose correct bullet manager
        foreach (BulletManager _wep in _weapons)
        {
            _wep.isTemporaryReady = false;

            if (!_wep.isTemporary)
                _chosen = _wep;
        }

        //Disable temporary bullet managers
        _chosen.SlotMachine();

        //Don't choose duplicate weapon
        if (_chosen.oldWeapon != null)
            if (_chosen.oldWeapon = _chosen.desiredWeapon[_choice])
            {
                List<int> _choices = new List<int>() { 0, 1, 2, 3 };
                _choices.Remove(_choice);

                _choice = _choices[Random.Range(0, _choices.Count)];
            }

        //Initialise Weapon
        _chosen.InitialiseWeapon(_choice);
        //Debug.Log("RollPlayer choice: " + _choice);
    }
    private void RollEnemy(int _choice)
    {
        FindObjectOfType<WaveManager>().UpdateEnemyUpgrade(_choice);
        Messenger<Sprite>.Broadcast(UiEvent.enemy_upgradeChange, FindObjectOfType<UiSlotMachine>().enemyUpgrades[_choice]);
    }
    private void RollArena(int _choice)
    {
        WaveManager _waveManager = FindObjectOfType<WaveManager>();

        _waveManager.ArenaPerk.Perks.ForEach((perk => perk.ResetPerks()));
        _waveManager.ArenaPerk.Perks[_choice].RunPerk();

        Messenger<Sprite>.Broadcast(UiEvent.arena_perkChange, FindObjectOfType<UiSlotMachine>().arenaPerks[_choice]);
    }

    private void RollArenaCrit(int _choice)
    {
        WaveManager _waveManager = FindObjectOfType<WaveManager>();

        _waveManager.ArenaPerk.Perks.ForEach((perk => perk.ResetPerks()));
        _waveManager.ArenaPerk.Perks[_choice].isCritical = true;
        _waveManager.ArenaPerk.Perks[_choice].RunPerk();

        Messenger<Sprite>.Broadcast(UiEvent.arena_perkChange, FindObjectOfType<UiSlotMachine>().arenaPerks[_choice]);

    }

    public void StartNextRound()
    {
        FindObjectOfType<WaveManager>().StartNextRound();
        GetComponent<Animator>().Play("slotMachine_flyOut");
    }
}
