using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    private InputS inputS;
    private Vector2 playerInputMove;
    private bool playerMoves = false;

    [SerializeField]
    private float movementSpeed;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputS = new InputS();
    }
    private void FixedUpdate()
    {
        if (playerMoves)
        {
            MovePlayer();
        }
    }
    private void OnEnable()
    {
        inputS.Enable();
        inputS.Player.Move.performed += PerformMove;
        inputS.Player.Move.canceled += StopMove;
    }
    private void OnDisable()
    {
        inputS.Player.Move.performed -= PerformMove;
        inputS.Player.Move.canceled -= StopMove;
    }
    
    void PerformMove(InputAction.CallbackContext ctx)
    {
        playerInputMove = ctx.ReadValue<Vector2>();
        playerMoves = true;
    }
    void StopMove(InputAction.CallbackContext ctx)
    {
        playerInputMove = Vector2.zero;
        playerMoves = false;
        rb.linearVelocity = Vector2.zero;
    }
    void MovePlayer()
    {
        playerInputMove = playerInputMove.normalized * movementSpeed;
        Vector3 moveVec = new Vector3(playerInputMove.x, 0, playerInputMove.y);
        rb.linearVelocity = moveVec;
    }
}
