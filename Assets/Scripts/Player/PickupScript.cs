using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [Tooltip("Layer for pickable props")]
    [SerializeField] private LayerMask _pickupLayer;
    [Tooltip("Layer for interactable props")]
    [SerializeField] private LayerMask _interactPropsLayer;
    [Tooltip("Range of ray that checks if player can pick up prop")]
    [SerializeField] private float _pickupRange;
    [Tooltip("Layer for interactable machines")]
    [SerializeField] private LayerMask _interactMachineLayer;
    [Tooltip("Layer for crack pipes")]
    [SerializeField] private LayerMask _interactPipeLayer;
    [Tooltip("Transform of invisible hand that will hold our object")]
    [SerializeField] private Transform _handPlacement;
    [Tooltip("Timer for cooldown between player interactions with objects")]
    [SerializeField] private float _interactCooldown = 0.6f;
    [Tooltip("Speed for MoveTowards from dispencer to Players hand")]
    [SerializeField] private float _speedOfPickup = 3f;
    [Tooltip("Speed of object throwing")]
    [SerializeField] private float _throwSpeed = 10f;
    [Tooltip("List of Crack Prefabs")]
    public List<GameObject> crackList = new();


    [Header("Objects on the ground")]
    [Tooltip("List that contains all pickable crack objects on the ground")]
    public List<GameObject> _objectsOnGround = new();
    [Tooltip("Maximum allowed object on the ground")]
    [SerializeField] private float _maxObjectsOnGround;
    public GameObject raycastHitGameObject;
    public GameObject raycastHitInteractableProps;

    private float _interactCooldownDelta;
    private Camera _cam;
    private StarterAssetsInputs _inputs;

    private Transform _targetedObject;
    private GameObject _currentObject;
    [SerializeField] private GameObject _cameraContainer;
    [SerializeField] private GameObject _throwPlace;

    [SerializeField] private AudioClip _grabClip;
    [SerializeField] private AudioClip _grabMetallicClip;
    public AudioSource _playerAudioSource;
    private void Start()
    {
        _cam = Camera.main;
        _inputs = GetComponent<StarterAssetsInputs>();
        _interactCooldownDelta = _interactCooldown;
    }


    void Update()
    {
        if (_inputs.interact)
        {
            HandlePickupCrack();
            HandlePickupPipe();
        }
        HandlePropDrag();
        HandleTimer();
        DestroyExtraObjects();
        //DebugCurrentObj();
        InvokeRepeating(nameof(CheckTargetItem), 0, 0.2f);

        if (_inputs.throwing)
        {
            ThrowCurrentObject();
        }
    }
    void HandlePickupCrack()
    {
        void AssignCrackFromDispenser()
            {
                _interactCooldown = 0;
                _targetedObject = raycastHitGameObject.transform;
                var crackType = raycastHitGameObject.GetComponent<Interactable>().typeOfCrack.ReturnCrackType();
            for (int i = 0; i < crackList.Count; i++)
                {
                    if (crackList[i].GetComponent<Interactable>().typeOfCrack.ReturnCrackType() == crackType)
                    {
                        _currentObject = Instantiate(crackList[i], _targetedObject.position, _targetedObject.rotation, _handPlacement);
                        ChangeRbStatus(_currentObject, true);
                    }
                }
            _playerAudioSource.PlayOneShot(_grabClip);
        }
        if (raycastHitGameObject == null)
            return;
        if (raycastHitGameObject.layer == LayerMask.NameToLayer("InteractableCrack"))
        {
            if (_interactCooldown == _interactCooldownDelta)
            {
                if (_currentObject == null)
                {
                    AssignCrackFromDispenser();
                }
                if (_currentObject != null && _interactCooldown == _interactCooldownDelta)
                {

                    GameObject _notCurrentObject = Instantiate(_currentObject, _currentObject.transform.position, _currentObject.transform.rotation);
                    ChangeRbStatus(_notCurrentObject, false);
                    _objectsOnGround.Add(_notCurrentObject);
                    Destroy(_currentObject);
                    _currentObject = null;
                    AssignCrackFromDispenser();
                }
            }
        }
        if (raycastHitGameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            if (_interactCooldown == _interactCooldownDelta)
            {
                if (_currentObject == null)
                {
                    AssignCrackFromDispenser();
                    _objectsOnGround.Remove(raycastHitGameObject);
                    Destroy(raycastHitGameObject);
                }
                if (_currentObject != null && _interactCooldown == _interactCooldownDelta)
                {
                    GameObject _notCurrentObject = Instantiate(_currentObject, raycastHitGameObject.transform.position, raycastHitGameObject.transform.rotation);
                    ChangeRbStatus(_notCurrentObject, false);
                    _objectsOnGround.Add(_notCurrentObject);
                    _objectsOnGround.Remove(raycastHitGameObject);
                    Destroy(_currentObject);
                    Destroy(raycastHitGameObject);
                    _currentObject = null;
                    AssignCrackFromDispenser();
                }
            }
        }
    }

    void HandlePickupPipe()
    {
        void AssignPipe()
        {
            _interactCooldown = 0;
            _currentObject = Instantiate(raycastHitGameObject, raycastHitGameObject.transform.position, Quaternion.identity, _handPlacement);
            ChangeRbStatus(_currentObject, true);
            _playerAudioSource.PlayOneShot(_grabMetallicClip);

        }
        if (raycastHitGameObject == null)
            return;
        if (raycastHitGameObject.layer == LayerMask.NameToLayer("InteractablePipe"))
        {
            if (_interactCooldown == _interactCooldownDelta)
            {
                if (_currentObject == null)
                {
                    AssignPipe();
                    _objectsOnGround.Remove(raycastHitGameObject);
                    Destroy(raycastHitGameObject);
                }
                if (_currentObject != null && _interactCooldown == _interactCooldownDelta)
                {
                    GameObject _notCurrentObject = Instantiate(_currentObject, raycastHitGameObject.transform.position, Quaternion.identity);
                    ChangeRbStatus(_notCurrentObject, false);
                    _objectsOnGround.Add(_notCurrentObject);
                    _objectsOnGround.Remove(raycastHitGameObject);
                    Destroy(_currentObject);
                    Destroy(raycastHitGameObject);
                    _currentObject = null;
                    AssignPipe();
                }
            }
        }
    }
    void HandlePropDrag()
    {
        if (_currentObject && _interactCooldown < _interactCooldownDelta)
        {
            _currentObject.transform.position = Vector3.MoveTowards(_currentObject.transform.position, _handPlacement.position,
             _speedOfPickup * Time.deltaTime);

        }

    }


    void HandleTimer()
    {
        _interactCooldown += Time.deltaTime;
        if (_interactCooldown >= _interactCooldownDelta)
        {
            _interactCooldown = _interactCooldownDelta;
        }
       
    }
    
    void DestroyExtraObjects()
    {
        if (_objectsOnGround.Count > _maxObjectsOnGround)
        {
            Destroy(_objectsOnGround[0]);
            _objectsOnGround.Remove(_objectsOnGround[0]);
        }
    }
    void CheckTargetItem()
    {
        Ray pickupRay = new(_cam.transform.position, _cam.transform.forward);
        if (Physics.Raycast(pickupRay, out RaycastHit _hitInfo, _pickupRange, _pickupLayer) || 
            Physics.Raycast(pickupRay, out  _hitInfo, _pickupRange, _interactPropsLayer) ||
            Physics.Raycast(pickupRay, out _hitInfo, _pickupRange, _interactPipeLayer))
        {
            raycastHitGameObject = _hitInfo.rigidbody.gameObject;
        }
        else
        {
            raycastHitGameObject = null;
        }

        Ray pickupProps = new(_cam.transform.position, _cam.transform.forward);
        if (Physics.Raycast(pickupProps, out RaycastHit hitInfo, _pickupRange, _interactMachineLayer))
            raycastHitInteractableProps = hitInfo.rigidbody.gameObject;
        else
        {
            raycastHitInteractableProps = null;
        }
    }


    void DebugCurrentObj()
    {
        if (_inputs.jump)
        {
            Destroy(_currentObject);
            _currentObject = null;
        }
    }

    void ChangeRbStatus(GameObject gameObject, bool isInHand)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = isInHand;
        gameObject.GetComponent<Rigidbody>().useGravity = !isInHand;
        foreach (Collider collider in gameObject.GetComponents<Collider>())
        {
            collider.enabled = !isInHand;
        }
    }

    void ThrowCurrentObject()
    {
        if (_currentObject != null)
        {
            _currentObject.transform.position = _throwPlace.transform.position;
            _currentObject.transform.SetParent(null);
            ChangeRbStatus(_currentObject, false);
            _currentObject.GetComponent<Rigidbody>().velocity = Vector3.forward;
            _currentObject.GetComponent<Rigidbody>().AddForce(_throwPlace.transform.forward * _throwSpeed, ForceMode.Impulse);
            _objectsOnGround.Add(_currentObject);
            _currentObject = null;
        }
    }
    public void StartInteractTimer()
    {
        _interactCooldown = 0;
    }
}

