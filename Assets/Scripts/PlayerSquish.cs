using UnityEngine;

public class PlayerSquish : MonoBehaviour
{
    [Header("Squish Settings")]
    public float squishAmount = 0.1f;
    public float squishSpeed = 8f;
    public float smoothSpeed = 12f;

    private Vector3 characterBaseScale;
    private float squishTimer;

    private PlayerController playerController;

    private void Awake() {
        characterBaseScale = transform.localScale;

        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (playerController == null)
            return;

        bool isMoving = playerController.IsMoving;

        Vector3 targetScale = characterBaseScale;

        if (isMoving) {
            squishTimer += Time.deltaTime * squishSpeed;

            float squishWave = Mathf.Sin(squishTimer);

            float verticalScale =
                1f - squishWave * squishAmount;

            float horizontalScale =
                1f + squishWave * squishAmount;

            targetScale = new Vector3(characterBaseScale.x * horizontalScale, characterBaseScale.y * verticalScale, characterBaseScale.z * horizontalScale);
        }
        else {
            squishTimer = 0f;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, smoothSpeed * Time.deltaTime);
    }
}