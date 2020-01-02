using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] Vector2 dyingVector = new Vector2(2f, 2f);
    Animator playerAnimator;
    Rigidbody2D playerRigidbody;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feet;

    bool canDoubleJump = true;
    bool isAlive;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feet = GetComponent<BoxCollider2D>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        movement();
        death();
    }

    private void death()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
        }
        if (!isAlive)
        {
            playerAnimator.SetTrigger("isDying");
            playerRigidbody.velocity = dyingVector;
            FindObjectOfType<GameController>().playerDeath();
        }
    }

    // Move the player according to user input. Also, execute running animaiton.
    private void movement()
    {
        playerRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRigidbody.velocity.y); // move left and right when the key is pressed

        // if there is movement on the x-axis,
        // have the sprite facing the correct direction.
        // then run/stop running animation.
        bool isRunningCheck = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if (isRunningCheck)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
        playerAnimator.SetBool("isRunning", isRunningCheck);

        // set the double jump flag to true while the player is touching the floor.
        // this way the player can jump if they simply walk off an edge
        if (feet.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            canDoubleJump = true;
        }

        // jump block
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // jump
            if (feet.IsTouchingLayers(LayerMask.GetMask("Foreground")))
            {

                canDoubleJump = true;
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // stop vertical
                Vector2 jumpVelocity = new Vector2(0f, Input.GetAxisRaw("Vertical") * jumpHeight);
                playerRigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);

                Debug.Log("jump");
            }
            // double jump
            else if (!feet.IsTouchingLayers(LayerMask.GetMask("Foreground")) && canDoubleJump == true && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                Debug.Log("double jump");
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // stop vertical movement to counter gravity
                Vector2 doubleJumpVelocity = new Vector2(0f, Input.GetAxisRaw("Vertical") * jumpHeight);
                playerRigidbody.AddForce(doubleJumpVelocity, ForceMode2D.Impulse);
                canDoubleJump = false;
            }
        }
    }
}