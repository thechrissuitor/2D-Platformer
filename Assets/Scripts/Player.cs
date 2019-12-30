using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float jumpHeight = 1;
    Animator playerAnimator;
    Rigidbody2D playerRigidbody;
    BoxCollider2D feet;

    bool canDoubleJump = true;

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        feet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
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

        // if the jump button(s) is pressed,
        // then jump, double jump, or do neither.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // if the player is touching the ground and there is no vertical movement,
            // jump.
            // raise flag to signal double jump is ready.
            if (feet.IsTouchingLayers(LayerMask.GetMask("Foreground")))
            {

                canDoubleJump = true;
                Vector2 jumpVelocity = new Vector2(0f, Input.GetAxisRaw("Vertical") * jumpHeight);
                playerRigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);

                Debug.Log("jump");
            }
            // if the player is not touching the ground, and the double jump flag is raised,
            // double jump.
            // lower double jump flag.
            else if (canDoubleJump == true)
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