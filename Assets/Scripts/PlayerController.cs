using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    private const string MOVE_X_ANIMATOR = "moveX";
    private const string MOVE_Y_ANIMATOR = "moveY";

    private bool usingController = false;
    private bool flipSprite = false;

    private GameInput gameInput;
    private Vector2 movementVector;
    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        gameInput = new GameInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        gameInput.Player.Enable();
    }

    private void Update()
    {
        CheckInputType();
        ReadMovement();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFaceDirection();
        MovePlayer();
    }

    private void CheckInputType()
    {
        if (Gamepad.current != null)
        {
            Vector2 rightStickInput = Gamepad.current.rightStick.ReadValue();

            if (Mathf.Abs(rightStickInput.x) > 0.125f || Mathf.Abs(rightStickInput.y) > 0.125f)
            {
                usingController = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Mathf.Abs(Mouse.current.delta.x.ReadValue()) > 0.1f || Mathf.Abs(Mouse.current.delta.y.ReadValue()) > 0.1f)
        {
            usingController = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void ReadMovement()
    {
        movementVector = gameInput.Player.Movement.ReadValue<Vector2>();

        animator.SetFloat(MOVE_X_ANIMATOR, movementVector.x);
        animator.SetFloat(MOVE_Y_ANIMATOR, movementVector.y);
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + movementVector * (moveSpeed * Time.deltaTime));
    }

    private void AdjustPlayerFaceDirection()
    {
        if (usingController)
        {
            Vector2 rightStickInput = Gamepad.current != null ? Gamepad.current.rightStick.ReadValue() : Vector2.zero;
            if (Mathf.Abs(rightStickInput.x) > 0.125f)
            {
                flipSprite = rightStickInput.x < -0.125f;
            }
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            flipSprite = mousePosition.x < transform.position.x;
        }
        
        if (flipSprite)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

}
