using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    private Vector3 doorEuler;
    public float rotation;
    bool isClosed;

    float cooldown = 2f;

    private void Start()
    {
        doorEuler = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        cooldown += Time.deltaTime;
        if (cooldown >= 2f)
            cooldown = 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && cooldown == 2f && isClosed)
        {

            transform.rotation = Quaternion.Euler(doorEuler.x,doorEuler.y,rotation);
            cooldown = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.rotation = Quaternion.Euler(doorEuler.x, doorEuler.y, doorEuler.z);
            isClosed = true;
        }
    }
}
