using System.Collections;
using System.Collections.Generic;
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
    MeshRenderer[] meshRenderers;
    [SerializeField] InputActionReference aimAction;

    float moveSpeed = 5f;
    float dashSpeed = 20f;
    float dashStamina = 1f;
    float jumpForce = 12f;
    float wallPushForce = 8f;
    float wallJumpCooldown = 0.1f;
    float wallJumpTimer = 0f;
    int jumpCount = 0;
    int maxJumpCount = 2;
    bool isDash = false;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponentInChildren<PlayerCollisionCheck>();
        wallCollisionCheck = GetComponent<PlayerWallCollisionCheck>();
        playerStamina = GetComponent<PlayerStamina>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // New Input System���� �Է��� �޾� �����̴� �Լ�
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            SetMoveState(MoveState.MOVE);
            moveDirection = new Vector2(input.x, input.y);
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
            SetMoveState(MoveState.NONE);
        }
    }

    // ���� �پ��� ��, �����ϴ� �Լ�
    bool OnGroundJump()
    {
        if (collisionCheck.GroundCheck)        
            jumpCount = 0;

        if (maxJumpCount > jumpCount)
        {
            jumpCount++;
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SetMoveState(MoveState.JUMP);
            return true;
        }
        return false;
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
            
            switch (wallCollisionCheck.wallContacted)
            {
                case ContactInfo.RIGHTWALL:
                    {
                        wallJumpTimer = wallJumpCooldown;
                        rigidBody2D.AddForce(GetWallJumpForce(ContactInfo.RIGHTWALL), ForceMode2D.Impulse);
                        SetMoveState(MoveState.JUMP);
                        SoundManager.Instance.PlaySound(SoundType.Jump);
                        break;
                    }
                case ContactInfo.LEFTWALL:
                    {
                        wallJumpTimer = wallJumpCooldown;
                        rigidBody2D.AddForce(GetWallJumpForce(ContactInfo.LEFTWALL), ForceMode2D.Impulse);
                        SetMoveState(MoveState.JUMP);
                        SoundManager.Instance.PlaySound(SoundType.Jump);
                        break;
                    }
                case ContactInfo.NONE:
                    {
                        bool jumpsuccess = OnGroundJump();
                        if(jumpsuccess)
                            SoundManager.Instance.PlaySound(SoundType.Jump);
                        break;
                    }
            }
        }
    }

    // �Է��� �޾� Dash�� �ϴ� �Լ�
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerStamina.CanDash(dashStamina) && moveDirection.x != 0 && !isDash)
            {
                SetMoveState(MoveState.DASH);
                playerStamina.UseStamina(dashStamina);
                SoundManager.Instance.PlaySound(SoundType.Dash);
                StartCoroutine(Dash());
            }
        }
        else if (context.canceled)
        {         
            SetMoveState(MoveState.NONE);
        }
    }

    // ���� ��� ���� ���� �Լ�
    IEnumerator Dash()
    {
        isDash = true;
        float speed = moveSpeed;
        moveSpeed = dashSpeed;

        // ���Ϳ� �÷��̾���� ���̾�� �浹 ����
        // 9 �÷��̾�, 13 enemies
        Physics2D.IgnoreLayerCollision(9, 13, true);

        List<float> alphas = new List<float>();
        foreach(var mesh in meshRenderers)
        {
            alphas.Add(mesh.material.color.a);
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0.6f);
        }

        yield return new WaitForSeconds(0.2f);

        int index = 0;
        foreach (var mesh in meshRenderers)
        {
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, alphas[index++]);
        }
        // ���Ϳ� �÷��̾���� ���̾� �浹 �ٽ� �ѱ�
        Physics2D.IgnoreLayerCollision(9, 13, false);
        SetMoveState(MoveState.MOVE);
        moveSpeed = speed;
        isDash = false;
    }

    void SetMoveState(MoveState moveState) => this.moveState = moveState;

    // �� ���� ��Ÿ������ �ƴ��� Ȯ���� ��, �ӵ� �����ϴ� �Լ�
    void FixedUpdate()
    {
        Vector2 mouseScreenPos = aimAction.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);        

        float offsetX = mouseWorldPos.x - transform.position.x;

        Vector3 scale = transform.localScale;

        if (offsetX > 0)
            scale.x = Mathf.Abs(scale.x); // ������ �� ���
        else if (offsetX < 0)
            scale.x = -Mathf.Abs(scale.x); // ���� �� ����

        transform.localScale = scale;

        if (wallJumpTimer <= 0)
        {           
            rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidBody2D.velocity.y);
        }
        else
        {
            wallJumpTimer -= Time.fixedDeltaTime;
        }
    }
}
