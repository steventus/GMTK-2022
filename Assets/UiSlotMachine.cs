using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UiSlotMachine : MonoBehaviour
{
    public List<UiWheel> wheels;

    public static int playerUpgradeNumber, enemyUpgradeNumber, arenaUpgradeNumber;

    [Space]
    public List<Sprite> playerUpgrades, enemyUpgrades, arenaPerks;

    private bool readyToBegin = false;
    private bool readyToExit = false;

    public UnityEvent onSlotEnter, onSlotBegin, onSlotStop, onSlotExit;

    private void Awake()
    {
        InitialiseWheels();
        DOTween.Init(false, false);
    }
    public void InitialiseWheels()
    {
        foreach(UiWheel _wheel in wheels)
        {
            _wheel.Initialise();
        }

        readyToBegin = false;
        readyToExit = false;
    }
    public void SlotEnter(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber)
    {

        //Animation - wait for 1 seconds
        StartCoroutine(Coro_SlotEnter(_playerUpgradeNumber, _enemyUpgradeNumber, _arenaUpgradeNumber));
        
        onSlotEnter.Invoke();
    }
    private IEnumerator Coro_SlotEnter(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber)
    {
        yield return new WaitForSeconds(1f);
        UpdateReadyToBegin(_playerUpgradeNumber, _enemyUpgradeNumber, _arenaUpgradeNumber);
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
        StartCoroutine(Coro_Slots());

        onSlotBegin.Invoke();
    }

    private IEnumerator Coro_Slots()
    {
        //Run animations for a few seconds
        yield return new WaitForSeconds(3f);

        //Run through each wheel slowly and show results
        wheels[0].ShowWheel(playerUpgrades[playerUpgradeNumber]);
        yield return new WaitForSeconds(0.3f);
        wheels[1].ShowWheel(enemyUpgrades[enemyUpgradeNumber]);
        yield return new WaitForSeconds(0.3f);
        wheels[2].ShowWheel(arenaPerks[arenaUpgradeNumber]);
        yield return null;

        //Prompt for SlotExit
        readyToExit = true;

        onSlotStop.Invoke();
    }
    public void SlotExit()
    {
        StartCoroutine(Coro_Exit());

        onSlotExit.Invoke();
    }
    private IEnumerator Coro_Exit()
    {
        yield return new WaitForSeconds(1);

        //Update relevant ui with new images
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToBegin)
        {
            readyToBegin = false;
            SlotBegin();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToExit)
        {
            readyToExit = false;
            SlotExit();
        }
    }
}
