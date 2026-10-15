using UnityEngine;

public class GrabHitbox : MonoBehaviour
{
    private PlayerGrab playerGrab;

    private void Awake() {
        playerGrab = GetComponentInParent<PlayerGrab>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Interactive"))
            return;

        playerGrab.SetInteractiveObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Interactive"))
            return;

        playerGrab.ClearInteractiveObject(other.gameObject);
    }
}