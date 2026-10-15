using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
                
    private Rigidbody rb;
    private Vector2 moveInput;
    private GroundController groundController;
    private bool jumpTriggered;
    public bool IsMoving => moveInput.sqrMagnitude > 0.01f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        
        groundController = GetComponent<GroundController>();
    }

    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump() {
        if (groundController.IsGrounded) {
            jumpTriggered = true;
        }
    }

    private void FixedUpdate() {
        rb.angularVelocity = Vector3.zero;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        if (movement.sqrMagnitude > 0.01f) {
            movement.Normalize();

            // Movement
            Vector3 velocity = movement * moveSpeed;

            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            // Face Movement Direction
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            float smoothFactor = 1f - Mathf.Exp(-rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, smoothFactor));
        }
        else {
            // Stop horizontal movement while preserving gravity
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        // Jump
        if (jumpTriggered) {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );

            jumpTriggered = false;
        }
    }
}
