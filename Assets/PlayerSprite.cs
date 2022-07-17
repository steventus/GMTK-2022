using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public void SwitchState(string newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Animator.Play(currentState);
    }
    
    public enum Direction
    {
        down = (int)0,
        right = (int)1,
        diagonaldown = (int)2,
        diagonalup = (int)3,
    }
    private void Awake()
    {
        thisSprite = GetComponent<SpriteRenderer>();
        thisBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetSprite();
        SetDirection();
    }
    public void SetSprite()
    {
        //Player Facing
        Vector3 _playerAim = GetComponent<PlayerController>().desiredAimDir;

        //Get Direction to South
        float _value = Vector3.Dot(Vector3.Normalize(Vector3.down), Vector3.Normalize(_playerAim));

        //Debug.Log(_value);
        


        Direction _choice;

        #region Math
        switch (_value)
        {
            case <= 1 and > 0.7f:
                _choice = Direction.down;
                SwitchState(PlayerController.isMoving ? "Running-1" : "Idle-1");
                break;
            case <= 0.7f and > 0.2f:
                _choice = Direction.diagonaldown;
                SwitchState(PlayerController.isMoving? "Running-1" : "Idle-1");
                break;
            case <= 0.2f and > -0.2f:
                _choice = Direction.right;
                SwitchState(PlayerController.isMoving? "Running-1" : "Idle-1");
                break;
            default:
                _choice = Direction.diagonalup;
                SwitchState(PlayerController.isMoving ? "Running-2" : "Idle-2");
                break;
        }
        #endregion

        //Debug.Log(_choice);

        //GetComponent<SpriteRenderer>().sprite = sprites[((int)_choice)].sprite;
    }

    public void SetDirection()
    {
        //Player Facing
        Vector3 _playerAim = GetComponent<PlayerController>().desiredAimDir;


        if (_playerAim.x > 0)
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
}
