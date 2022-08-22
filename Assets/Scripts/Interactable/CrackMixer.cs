using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using static CrackScriptable;

public class CrackMixer : MonoBehaviour
{
    [SerializeField] private GameObject _firstObjectInside;
    [SerializeField] private GameObject _secondObjectInside;

    [Header("Lights")]
    [SerializeField] protected GameObject _firstObjectLight;
    [SerializeField] protected GameObject _secondObjectLight;

    [Header("Buttons")]
    [SerializeField] private GameObject _mixButton;
    [SerializeField] private GameObject _resetButton;
    [SerializeField] private Material _defaultMaterial;

    [SerializeField] private GameObject _crackPipeDispencer;
    [Tooltip("Crack Pipe List")]
    [Header("Crack Pipes")]
    [SerializeField] private List<GameObject> crackPipeList = new();

    private PickupScript pickupScript;
    private StarterAssetsInputs _inputs;

    private CrackType _typeOfCrackInside_0;
    private CrackType _typeOfCrackInside_1;

    [SerializeField] private AudioClip _vendingSound;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _confirmSound;
    private AudioSource _audioSource;

    [SerializeField] private List<Material>  lightButtonList = new ();
    private float _timer = 0.5f;
    private float _timerDelta;
    private void Start()
    {
        pickupScript = FindObjectOfType<PickupScript>();
        _inputs = FindObjectOfType<StarterAssetsInputs>();
        _timerDelta = _timer;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_inputs.interact)
        {
            pickupScript.StartInteractTimer();
            if (pickupScript.raycastHitInteractableProps == _resetButton)
                HandleResetButton();
            CreateCrackPipe();
        }
        HandleTimer();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            
            if (_firstObjectInside == null && _secondObjectInside == null)
            {
                _timer = 0f;
                _typeOfCrackInside_0 = collision.gameObject.GetComponent<Interactable>().typeOfCrack.type;
                for (int i = 0; i < pickupScript.crackList.Count; i++)
                {
                    if (pickupScript.crackList[i].GetComponent<Interactable>().typeOfCrack.type == _typeOfCrackInside_0)
                        _firstObjectInside = pickupScript.crackList[i];
                }
                //Change material after models are done
                //_firstObjectLight.GetComponent<Renderer>().material = collision.gameObject.GetComponent<Renderer>().material;
                
                switch (collision.gameObject.GetComponent<Interactable>().typeOfCrack.type)
                {
                    case CrackType.TypeOne:
                        _firstObjectLight.GetComponent<Renderer>().material = lightButtonList[0];
                        break;
                    case CrackType.TypeTwo:
                        _firstObjectLight.GetComponent<Renderer>().material = lightButtonList[1];
                        break;
                    case CrackType.TypeThree:
                        _firstObjectLight.GetComponent<Renderer>().material = lightButtonList[2];
                        break;
                }
                _audioSource.PlayOneShot(_confirmSound, 1f);
                pickupScript._objectsOnGround.Remove(collision.gameObject);
                Destroy(collision.gameObject);
            }
            if (_firstObjectInside != null && _secondObjectInside == null && _timer == _timerDelta)
            {
                _timer = 0f;
                //Change material after models are done
                //_secondObjectLight.GetComponent<Renderer>().material = collision.gameObject.GetComponent<Renderer>().material;

                _typeOfCrackInside_1 = collision.gameObject.GetComponent<Interactable>().typeOfCrack.type;
                for (int i = 0; i < pickupScript.crackList.Count; i++)
                {
                    if (pickupScript.crackList[i].GetComponent<Interactable>().typeOfCrack.type == _typeOfCrackInside_1)
                        _secondObjectInside = pickupScript.crackList[i];
                }

                switch (collision.gameObject.GetComponent<Interactable>().typeOfCrack.type)
                {
                    case CrackType.TypeOne:
                        _secondObjectLight.GetComponent<Renderer>().material = lightButtonList[0];
                        break;
                    case CrackType.TypeTwo:
                        _secondObjectLight.GetComponent<Renderer>().material = lightButtonList[1];
                        break;
                    case CrackType.TypeThree:
                        _secondObjectLight.GetComponent<Renderer>().material = lightButtonList[2];
                        break;
                }
                _audioSource.PlayOneShot(_confirmSound, 1f);
                pickupScript._objectsOnGround.Remove(collision.gameObject);
                Destroy(collision.gameObject);
            }
        }
    }

    private void HandleResetButton()
    {
        
        {
            _firstObjectLight.GetComponent<Renderer>().material = _defaultMaterial;
            _secondObjectLight.GetComponent<Renderer>().material = _defaultMaterial;
            _firstObjectInside = null; 
            _secondObjectInside = null;
            _audioSource.PlayOneShot(_errorSound);
        }
    }

    private void HandleTimer()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timerDelta)
            _timer = _timerDelta;
    }

    private void CreateCrackPipe()
    {
        if (pickupScript.raycastHitInteractableProps == _mixButton && _firstObjectInside != null && _secondObjectInside != null)
        {
            if ((_typeOfCrackInside_0 == CrackType.TypeOne && _typeOfCrackInside_1 == CrackType.TypeTwo && _timer == _timerDelta) ||
                (_typeOfCrackInside_0 == CrackType.TypeTwo && _typeOfCrackInside_1 == CrackType.TypeOne && _timer == _timerDelta))
            {
                Instantiate(crackPipeList[0], _crackPipeDispencer.transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_vendingSound);
                HandleResetButton();
            }
            if ((_typeOfCrackInside_0 == CrackType.TypeOne && _typeOfCrackInside_1 == CrackType.TypeThree && _timer == _timerDelta) ||
                (_typeOfCrackInside_0 == CrackType.TypeThree && _typeOfCrackInside_1 == CrackType.TypeOne && _timer == _timerDelta))
            {
                Instantiate(crackPipeList[1], _crackPipeDispencer.transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_vendingSound);
                HandleResetButton();
            }
            if ((_typeOfCrackInside_0 == CrackType.TypeTwo && _typeOfCrackInside_1 == CrackType.TypeThree && _timer == _timerDelta) ||
                (_typeOfCrackInside_0 == CrackType.TypeThree && _typeOfCrackInside_1 == CrackType.TypeTwo && _timer == _timerDelta))
            {
                _timer = 0;
                Instantiate(crackPipeList[2], _crackPipeDispencer.transform.position, _crackPipeDispencer.transform.rotation);
                _audioSource.PlayOneShot(_vendingSound);
                HandleResetButton();
            }
            if (_typeOfCrackInside_0 == _typeOfCrackInside_1)
            {

                HandleResetButton();
            }
        }
    }

    //private GameObject ReturnCorrectPipe(CrackPipeType pipeType, GameObject gameObject)
    //{
    //    if (crackPipeList.Contains(gameObject))
    //        return gameObject;
    //    else
    //        return null;
    //}

    
}
