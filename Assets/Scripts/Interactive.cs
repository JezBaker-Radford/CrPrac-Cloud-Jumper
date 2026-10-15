using UnityEngine;

public class Interactive : MonoBehaviour
{
    public virtual void OnThrow(Vector3 force) { 
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null) { 
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
