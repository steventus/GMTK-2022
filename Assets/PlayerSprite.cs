using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class PlayerDirSprite
{
    [SerializeField]
    public int Dir; //In order of down, right, diagonal down, diagonal up
    public Sprite sprite;
}


[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSprite : MonoBehaviour
{
    [SerializeField] public List<PlayerDirSprite> sprites;
    private SpriteRenderer thisSprite;

    private Rigidbody2D thisBody;
    private Vector3 oldVel;
    private bool ifFacingRight, oldFacingRight;

    public Animator Animator;

    private string currentState;
    public PlayerController PlayerController;

    private bool isInvul = false;
    private Coroutine coroInvul;

    private void SwitchState(string newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        
        Animator.CrossFadeInFixedTime(currentState, 0.1f);
    }
    
    private enum Direction
    {
        Down = 0,
        Right = 1,
        Diagonaldown = 2,
        Diagonalup = 3,
    }
    private void Awake()
    {
        thisSprite = GetComponent<SpriteRenderer>();
        thisBody = GetComponent<Rigidbody2D>();

        PlayerController = GetComponent<PlayerController>();
        Animator = GetComponent<Animator>();

        DOTween.Init(false, false);

    }


    private void Update()
    {
        SetSprite();
        SetDirection();
    }

    private void SetSprite()
    {
        //Player Facing
        Vector3 playerAim = PlayerController.desiredAimDir;

        //Get Direction to South
        float value = Vector3.Dot(Vector3.zero, Vector3.Normalize(playerAim));

        #region Math
        switch (value)
        {
            case <= 1 and > 0.7f:
                SwitchState(PlayerController.isMoving ? "Running-1" : "Idle-1");
                break;
            case <= 0.7f and > 0.2f:
                SwitchState(PlayerController.isMoving? "Running-1" : "Idle-1");
                break;
            case <= 0.2f and > -0.2f:
                SwitchState(PlayerController.isMoving? "Running-1" : "Idle-1");
                break;
            default:
                SwitchState(PlayerController.isMoving ? "Running-2" : "Idle-2");
                break;
        }
        #endregion
    }

    private void SetDirection()
    {
        //Player Facing
        Vector3 playerAim = GetComponent<PlayerController>().desiredAimDir;


        if (playerAim.x > 0)
        {
            ifFacingRight = true;
            thisSprite.flipX = false;
        }

        else
        {
            ifFacingRight = false;
            thisSprite.flipX = true;
        }

        oldFacingRight = ifFacingRight;
    }
    public void Invul(bool _ifInvul)
    {
        if (coroInvul != null)
            StopCoroutine(coroInvul);

        if (_ifInvul)
        {   
            coroInvul = StartCoroutine(Coro_Invul());
        }

        else
        {
            isInvul = false;
            thisSprite.DOFade(1, 0);
        }
    }
    private IEnumerator Coro_Invul()
    {
        isInvul = true;

        float _dur = GetComponent<PlayerHealth>().invulDur;
        float _rateFrame = 0.05f;

        for (int _t = 0; _t < (_dur/ (_rateFrame*2)); _t++)
        {
            thisSprite.DOFade(1, 0);
            yield return new WaitForSeconds(_rateFrame);
            thisSprite.DOFade(0, 0);
            yield return new WaitForSeconds(_rateFrame);
        }

        thisSprite.DOFade(1, 0);
        Invul(false);

    }


}
