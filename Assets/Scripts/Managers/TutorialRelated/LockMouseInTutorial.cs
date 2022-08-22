using UnityEngine;
using StarterAssets;

public class LockMouseInTutorial : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _inputs;

    private void Start()
    {
        LockMouseOnStart();
    }


    private void LockMouseOnStart()
    {
        Cursor.visible = false;
        _inputs.cursorLocked = true;
    }
}
