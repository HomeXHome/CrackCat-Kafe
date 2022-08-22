using UnityEngine;

public class TrashDestroyer : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickable") || collision.gameObject.layer == LayerMask.NameToLayer("InteractablePipe"))
        {
            Destroy(collision.gameObject);
        }
    }
}
