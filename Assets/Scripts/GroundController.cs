using UnityEngine;

public class GroundController : MonoBehaviour
{
    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayerMask;

    public bool IsGrounded { get; private set; }

    private void Update() {
        CheckGround();
    }

    private void CheckGround() {
        IsGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayerMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnDrawGizmosSelected() {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}