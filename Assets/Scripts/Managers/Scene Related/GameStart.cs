using UnityEngine;

public class GameStart : MonoBehaviour
{

    private bool isStarted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isStarted == false)
            EventManager.OnStartEvent();
            isStarted = true;
        }
    }
}
