using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInflate : MonoBehaviour
{
    [Header("Inflate Settings")]
    public float inflateMultiplier = 2f;
    public float inflationSpeed = 0.25f;
    public float inflatedDuration = 2f;
    public float shrinkDuration = 3f;
    public float inflateCooldown = 3f;
    public float inflateForce = 0.5f;
    public Collider playerHitbox;

    private Rigidbody rb;
    private Vector3 originalScale;
    private bool isInflating = false;
    private bool isOnCooldown = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }

    public void OnInflate() {
        if (!isInflating && !isOnCooldown) {
            StartCoroutine(InflateSequence());
        }
    }

    private void PushInteractiveObjects() {
        Collider[] hits = Physics.OverlapBox(playerHitbox.bounds.center, playerHitbox.bounds.extents, playerHitbox.transform.rotation);

        if (playerHitbox == null)
            return;

        foreach (Collider hit in hits) {
            if (!hit.CompareTag("Interactive"))
                continue;

            Rigidbody objectRb = hit.attachedRigidbody;

            if (objectRb == null || objectRb == rb)
                continue;

            Vector3 direction = objectRb.worldCenterOfMass - transform.position;

            if (direction.sqrMagnitude > 0.001f) {
                direction.Normalize();

                objectRb.AddForce(direction * inflateForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator InflateSequence() {
        isInflating = true;
        Vector3 targetScale = originalScale * inflateMultiplier;

        float elapsedInflateTime = 0f;
        while (elapsedInflateTime < inflationSpeed) {
            elapsedInflateTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedInflateTime / inflationSpeed);
            PushInteractiveObjects();

            yield return null;
        }
        transform.localScale = targetScale;

        for (float timer = 0; timer < inflatedDuration; timer += Time.deltaTime) {
            transform.localScale = targetScale;
            yield return null;
        }

        float elapsedShrinkTime = 0f;
        while (elapsedShrinkTime < shrinkDuration) {
            elapsedShrinkTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedShrinkTime / shrinkDuration);

            yield return null;
        }
        transform.localScale = originalScale;

        isInflating = false;
        StartCoroutine(CooldownSequence());
    }
    
    private IEnumerator CooldownSequence() { 
        isOnCooldown = true;
        yield return new WaitForSeconds(inflateCooldown);
        isOnCooldown = false;
    }
}
