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
    public Vector3 startPos;
    
    [SerializeField]
    public Animator animator;
    
    [SerializeField]
    public float movementSpeed = 5;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputS = new InputS();
        tilt = Vector2.zero;
    }
    private void FixedUpdate()
    {
        if (rb.position.y < -10)
        {
            GameManager.instance.DieOOM();
        }
        if (playerMoves)
        {
            MovePlayer();
        }
    }
    public void StartGame()
    {
        rb.isKinematic = false;
    }
    public void StopGame()
    {
        rb.isKinematic = true;
        rb.position = startPos;
    }
    void Start()
    {
        startPos = transform.position;
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
        if (GameManager.instance.gameRunning)
        {
            animator.SetBool("IsWalking", true);
            playerInputMove = ctx.ReadValue<Vector2>();
            playerMoves = true;
        }
    }
    void StopMove(InputAction.CallbackContext ctx)
    {
        animator.SetBool("IsWalking", false);
        playerInputMove = Vector2.zero;
        playerMoves = false;
    }
    void MovePlayer()
    {
        Vector3 playerInputMoveCalib = (playerInputMove.normalized + tilt * 0.8f) * movementSpeed;
        Vector3 moveVec = new Vector3(playerInputMoveCalib.x, 0, playerInputMoveCalib.y);
        // rb.AddForce(moveVec, ForceMode.VelocityChange);
        rb.MovePosition(
            rb.position + moveVec * Time.fixedDeltaTime
        );
        rb.angularVelocity = Vector2.zero;
        Quaternion targetRotation = Quaternion.LookRotation(moveVec);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(tilt.x, 0, tilt.y));
    }
    void Reset()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = Vector2.zero;
        rb.position = startPos;
    }
}
