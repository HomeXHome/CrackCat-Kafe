using UnityEngine;

public class DebugRay : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float _debugRayLenght = 5f;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * _debugRayLenght, Color.red);
    }
}
