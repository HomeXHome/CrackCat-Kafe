using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Material material;
    private PickupScript pickupScript;
    public CrackScriptable typeOfCrack;
    public CrackPipeScriptable typeOfCrackPipe;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        pickupScript = FindObjectOfType<PickupScript>();
    }

    void Update()
    {
        ChangeAlphaIfHit();
    }

    void ChangeAlphaIfHit()
    {
        if (pickupScript != null)
        {
            if (gameObject.layer == LayerMask.NameToLayer("InteractableCrack"))
            {
                if (pickupScript.raycastHitGameObject == gameObject)
                {
                    material.SetFloat("_Fresnel_Power", 0.3f);
                }
                else
                {
                    material.SetFloat("_Fresnel_Power", 3f);
                }
            }
            if (gameObject.layer == LayerMask.NameToLayer("InteractableMachine"))
                {

                if (pickupScript.raycastHitInteractableProps == gameObject)
                {
                    material.SetFloat("_Fresnel_Power", -0.3f);
                }
                else
                {
                    material.SetFloat("_Fresnel_Power", 3f);
                }
                }
            if (gameObject.layer == LayerMask.NameToLayer("Pickable"))
            {
                if (pickupScript.raycastHitGameObject == gameObject)
                {
                    material.SetFloat("_Fresnel_Power", 0.3f);
                }
                else
                {
                    material.SetFloat("_Fresnel_Power", 3f);
                }
            }
            if (gameObject.layer == LayerMask.NameToLayer("InteractablePipe"))
            {
                if (pickupScript.raycastHitGameObject == gameObject)
                {
                    material.SetFloat("_Fresnel_Power", 0.3f);
                }
                else
                {
                    material.SetFloat("_Fresnel_Power", 3f);
                }
            }

        }
    }
}
