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
    private bool playerCrit, enemyCrit, arenaCrit;

    [Space]
    public List<Sprite> playerUpgrades, enemyUpgrades, arenaPerks;

    private bool readyToBegin = false;
    private bool readyToExit = false;

    public UnityEvent onSlotEnter, onSlotBegin, onSlotStop, onSlotExit;
    public UnityEvent onRoundBegin;

    private void Awake()
    {
        InitialiseWheels();
        DOTween.Init(false, false);
    }
    public void InitialiseWheels()
    {
        foreach (UiWheel _wheel in wheels)
        {
            _wheel.Initialise();
        }

        readyToBegin = false;
        readyToExit = false;
    }
    private void OnEnable()
    {
        Messenger<Sprite>.AddListener(UiEvent.player_gunChange, wheels[0].SetImage);
    }
    private void OnDisable()
    {
        Messenger<Sprite>.RemoveListener(UiEvent.player_gunChange, wheels[0].SetImage);

    }
    public void SlotEnter(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber, bool _ifPlayerCrit, bool _ifEnemyCrit, bool _ifArenaCrit)
    {
        //Animation - wait for 1 seconds
        StartCoroutine(Coro_SlotEnter(_playerUpgradeNumber, _enemyUpgradeNumber, _arenaUpgradeNumber, _ifPlayerCrit, _ifEnemyCrit, _ifArenaCrit));
        onSlotEnter.Invoke();
    }
    private IEnumerator Coro_SlotEnter(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber, bool _ifPlayerCrit, bool _ifEnemyCrit, bool _ifArenaCrit)
    {
        yield return new WaitForSeconds(1f);
        UpdateReadyToBegin(_playerUpgradeNumber, _enemyUpgradeNumber, _arenaUpgradeNumber, _ifPlayerCrit, _ifEnemyCrit, _ifArenaCrit);
    }

    public void UpdateReadyToBegin(int _playerUpgradeNumber, int _enemyUpgradeNumber, int _arenaUpgradeNumber, bool _ifPlayerCrit, bool _ifEnemyCrit, bool _ifArenaCrit)
    {
        playerUpgradeNumber = _playerUpgradeNumber;
        enemyUpgradeNumber = _enemyUpgradeNumber;
        arenaUpgradeNumber = _arenaUpgradeNumber;

        playerCrit = _ifPlayerCrit;
        enemyCrit = _ifEnemyCrit;
        arenaCrit = _ifArenaCrit;

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
        wheels[0].ShowWheelSpecial(playerCrit);
        Debug.Log("player:" + playerUpgradeNumber);
        yield return new WaitForSeconds(0.3f);
        wheels[1].ShowWheel(enemyUpgrades[enemyUpgradeNumber], enemyCrit);
        Debug.Log("enemy: " + playerUpgradeNumber);

        yield return new WaitForSeconds(0.3f);
        wheels[2].ShowWheel(arenaPerks[arenaUpgradeNumber], arenaCrit);
        Debug.Log("arena: " + playerUpgradeNumber);

        yield return null;

        //Prompt for SlotExit
        readyToExit = true;

        onSlotStop.Invoke();
    }
    public void SlotExit()
    {
        StartCoroutine(Coro_Exit());
        
    }
    private IEnumerator Coro_Exit()
    {
        //Update relevant ui with new images
        yield return new WaitForSeconds(1);
        
        // Exit SlotUI
        onSlotExit.Invoke();

        // wait for UI to hide
        yield return new WaitForSeconds(2f);
        
        onRoundBegin?.Invoke();

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
