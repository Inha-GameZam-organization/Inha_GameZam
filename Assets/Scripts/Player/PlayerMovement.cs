using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveState
{
    NONE,
    MOVE,
    DASH,
    JUMP,
}
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public MoveState moveState;
    Vector2 moveDirection;
    Rigidbody2D rigidBody2D;
    PlayerCollisionCheck collisionCheck;
    PlayerWallCollisionCheck wallCollisionCheck;
    PlayerStamina playerStamina;

    float moveSpeed = 5f;
    float dashSpeed = 20f;
    float dashStamina = 1f;
    float jumpForce = 15f;
    float wallPushForce = 8f;
    float wallJumpCooldown = 0.1f;
    float wallJumpTimer = 0f;
    int jumpCount = 0;
    int maxJumpCount = 2;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponentInChildren<PlayerCollisionCheck>();
        wallCollisionCheck = GetComponent<PlayerWallCollisionCheck>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    // New Input System���� �Է��� �޾� �����̴� �Լ�
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector2(input.x, input.y);
        }
    }

    // ���� �پ��� ��, �����ϴ� �Լ�
    void OnGroundJump()
    {
        if (collisionCheck.GroundCheck)
        {
            jumpCount = 0;
        }
        
        if (maxJumpCount > jumpCount)
        {            
            jumpCount++;
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);            
        }
    }
    Vector2 GetWallJumpForce(ContactInfo contact)
    {
        float horizontal = (contact == ContactInfo.RIGHTWALL) ? -wallPushForce : wallPushForce;
        return new Vector2(horizontal, jumpForce);
    }

    // New Input System���� �Է��� �޾� �����ϴ� �Լ�
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetMoveState(MoveState.JUMP);
            switch (wallCollisionCheck.wallContacted)
            {
                case ContactInfo.RIGHTWALL:
                    {
                        wallJumpTimer = wallJumpCooldown;
                        rigidBody2D.AddForce(GetWallJumpForce(ContactInfo.RIGHTWALL), ForceMode2D.Impulse);
                        break;
                    }
                case ContactInfo.LEFTWALL:
                    {
                        wallJumpTimer = wallJumpCooldown;
                        rigidBody2D.AddForce(GetWallJumpForce(ContactInfo.LEFTWALL), ForceMode2D.Impulse);
                        break;
                    }
                case ContactInfo.NONE:
                    {
                        OnGroundJump();
                        break;
                    }
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (this.moveState == MoveState.MOVE)
            {
                SetMoveState(MoveState.DASH);
                if(playerStamina.CanDash(dashStamina))
                {
                    playerStamina.UseStamina(dashStamina);
                    StartCoroutine(Dash());
                }                
            }            
        }
    }

    IEnumerator Dash()
    {
        float speed = moveSpeed;
        moveSpeed = dashSpeed;

        // ���Ϳ� �÷��̾���� ���̾�� �浹 ����
        // 9 �÷��̾�, 13 enemies
        Physics2D.IgnoreLayerCollision(9, 13, true);

        yield return new WaitForSeconds(0.2f);

        // ���Ϳ� �÷��̾���� ���̾� �浹 �ٽ� �ѱ�
        Physics2D.IgnoreLayerCollision(9, 13, false);
        moveSpeed = speed;
    }

    void SetMoveState(MoveState moveState) => this.moveState = moveState;

    // �� ���� ��Ÿ������ �ƴ��� Ȯ���� ��, �ӵ� �����ϴ� �Լ�
    void FixedUpdate()
    {
        if (wallJumpTimer <= 0)
        {
            SetMoveState(MoveState.MOVE);
            rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidBody2D.velocity.y);
        }
        else
        {
            wallJumpTimer -= Time.fixedDeltaTime;
        }
    }
}
