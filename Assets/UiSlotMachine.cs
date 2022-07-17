using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UiSlotMachine : MonoBehaviour
{
    public List<UiWheel> wheels;

    public int playerUpgradeNumber, enemyUpgradeNumber, arenaUpgradeNumber;

    [Space]
    public List<Sprite> playerUpgrades, enemyUpgrades, arenaPerks;

    private bool readyToBegin = false;
    private bool readyToExit = false;

    public UnityEvent onSlotEnter, onSlotBegin, onSlotStop;

    private void Awake()
    {
        InitialiseWheels();
    }
    public void InitialiseWheels()
    {
        foreach(UiWheel _wheel in wheels)
        {
            _wheel.Initialise();
        }

        readyToBegin = readyToExit = false;
    }
    public void SlotEnter()
    {
        onSlotEnter.Invoke();
    }

    public void UpdateReadyToBegin(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber)
    {
        playerUpgradeNumber = _playerUpgradeNumber;
        enemyUpgradeNumber = _enemyUpgradeNumber;
        arenaUpgradeNumber = _enemyUpgradeNumber;

        readyToBegin = true;
    }
    public void SlotBegin()
    {
        readyToBegin = false;

        //Run through each wheel slowly and show results
        StartCoroutine(Coro_Slots(playerUpgradeNumber, enemyUpgradeNumber, arenaUpgradeNumber));

        //Prompt for SlotExit
        readyToExit = true;

        onSlotBegin.Invoke();
    }

    private IEnumerator Coro_Slots(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber)
    {
        //Run animations for a few seconds
        yield return new WaitForSeconds(3f);

        //Run through each wheel slowly and show results
        wheels[0].ShowWheel(playerUpgrades[_playerUpgradeNumber]);
        yield return new WaitForSeconds(0.3f);
        wheels[1].ShowWheel(enemyUpgrades[_enemyUpgradeNumber]);
        yield return new WaitForSeconds(0.3f);
        wheels[2].ShowWheel(arenaPerks[_arenaUpgradeNumber]);
        yield return null;

        onSlotStop.Invoke();
    }
    public void SlotExit()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToExit)
        {
            readyToExit = false;
            SlotExit();
        }
    }
}
