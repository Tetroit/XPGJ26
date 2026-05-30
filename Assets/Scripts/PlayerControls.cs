using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    private InputS inputS;
    private Vector2 playerInputMove;
    private bool playerMoves = false;
    public Vector2 tilt;
    
    [SerializeField]
    private float movementSpeed;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputS = new InputS();
        tilt = Vector2.zero;
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
        Vector3 playerInputMoveCalib = (playerInputMove.normalized + tilt * 0.5f) * movementSpeed;
        Vector3 moveVec = new Vector3(playerInputMoveCalib.x, 0, playerInputMoveCalib.y);
        rb.linearVelocity = moveVec;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(tilt.x, 0, tilt.y));
    }
}
