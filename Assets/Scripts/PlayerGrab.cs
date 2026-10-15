using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Grab Settings")]
    public GameObject holdPoint;
    public float throwForce = 10f;

    private GameObject interactiveObject;
    private GameObject grabbedObject;
    private Rigidbody grabbedRigidBody;
    private Collider grabbedCollider;

    public void SetInteractiveObject(GameObject obj)
    {
        if (grabbedObject != null)
            return;

        interactiveObject = obj;
    }

    public void ClearInteractiveObject(GameObject obj)
    {
        if (interactiveObject == obj)
        {
            interactiveObject = null;
        }
    }

    private void LateUpdate()
    {
        if (grabbedObject == null) return;

        grabbedObject.transform.position = holdPoint.transform.position;
        grabbedObject.transform.rotation = holdPoint.transform.rotation;
    }

    public void OnGrab()
    {
        if (grabbedObject != null) return;
        if (interactiveObject == null) return;

        grabbedObject = interactiveObject;
        grabbedRigidBody = grabbedObject.GetComponent<Rigidbody>();
        grabbedCollider = grabbedObject.GetComponent<Collider>();

        if (grabbedRigidBody != null) {
            grabbedRigidBody.isKinematic = true;
        }

        if (grabbedCollider != null) {
            Collider[] playerColliders = GetComponentsInChildren<Collider>();

            foreach (Collider playerCollider in playerColliders) {
                Physics.IgnoreCollision(
                    grabbedCollider,
                    playerCollider,
                    true
                );
            }
        }
        grabbedObject.tag = "Untagged";
        grabbedObject.transform.position = holdPoint.transform.position;
        grabbedObject.transform.rotation = holdPoint.transform.rotation;
    }

    public void OnRelease() {
        if (grabbedObject == null) return;

        if (grabbedCollider != null) {
            Collider[] playerColliders = GetComponentsInChildren<Collider>();

            foreach (Collider playerCollider in playerColliders) {
                Physics.IgnoreCollision(
                    grabbedCollider,
                    playerCollider,
                    false
                );
            }
        }

        if (grabbedRigidBody != null) {
            grabbedRigidBody.isKinematic = false;

            grabbedRigidBody.AddForce(
                transform.forward * throwForce,
                ForceMode.Impulse
            );
        }
        grabbedObject.tag = "Interactive";
        grabbedObject = null;
        grabbedRigidBody = null;
        grabbedCollider = null;
    }
}