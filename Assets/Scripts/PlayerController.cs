using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    private GameInput gameInput;
    private Vector2 movementVector;
    private Rigidbody2D rb;

    private void Awake()
    {
        gameInput = new GameInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        gameInput.Player.Enable();
    }

    private void Update()
    {
        ReadMovement();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void ReadMovement()
    {
        movementVector = gameInput.Player.Movement.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + movementVector * (moveSpeed * Time.deltaTime));
    }

}
