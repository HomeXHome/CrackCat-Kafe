using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatScript : MonoBehaviour
{
    [SerializeField] private float _irritationTimer;
    private float _maxIrritationTime;
    public ScriptableCat scriptableCatData;
    private GameObject _timerUI;
    private Image _timerImage;
    [SerializeField] private CrackPipeScriptable _crackPipeNeeded;
    private PickupScript pickupScript;
    private GameManager gameManagerScript;

    [SerializeField] private AudioClip _meowing_0;
    [SerializeField] private AudioClip _angryMeowing_0;
    [SerializeField] private AudioClip _eat_sound_0;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioSource _sourceEat;
    private bool _isGameStarted = false;
    private int currentAudioClip;


    [Header("Animations")]
    [SerializeField] private bool _isSitting;
    [SerializeField] private bool _isEating;

    [Header("Colors for Timer Feedback")]
    [SerializeField] private Color _defaultTimerColor;
    [SerializeField] private Color _wrongTimerColor;
    [SerializeField] private Color _correctTimerColor;

    private bool _isSittingOnStart;
    private bool _isEatingOnStart;
    private Animator animator;
    private void OnEnable()
    {
        EventManager.StartEvent += OnGameStart;
    }
    private void OnDisable()
    {
        EventManager.StartEvent -= OnGameStart;
    }


    private void Start()
    {
        _irritationTimer = scriptableCatData.irritationTime;
        _maxIrritationTime = scriptableCatData.irritationTime;
        pickupScript = FindObjectOfType<PickupScript>();
        gameManagerScript = FindObjectOfType<GameManager>();
        _timerUI = gameManagerScript.targetUI;
        AssignTimerUI(scriptableCatData);
        _source.clip = _meowing_0;
        currentAudioClip = 0;
        animator = GetComponent<Animator>();
        _isSittingOnStart = _isSitting;
        _isEatingOnStart = _isEating;
    }

    private void Update()
    {
        if (_isGameStarted)
        {
        DepleteTimer();
        UITimerUpdate();
        ControlAudioVolume();
        ChangeAudioClip();
        ControlAnimations();
        HandleEndgameTimer();
        }
    }

    private void OnGameStart()
    {
        _isGameStarted = true;
        _source.Play();
    }



    private void DepleteTimer()
    {
        _irritationTimer -= Time.deltaTime;
        if (_irritationTimer <= 0)
            _irritationTimer = 0;
        if (_irritationTimer >= _maxIrritationTime)
            _irritationTimer = _maxIrritationTime;
    }

    private void UITimerUpdate()
    {
        _timerImage.fillAmount = _irritationTimer / _maxIrritationTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractablePipe"))
        {
            if (_crackPipeNeeded == other.gameObject.GetComponent<Interactable>().typeOfCrackPipe)
            {
                StartCoroutine(HandleTimerColor(true));
                _irritationTimer += 35f;
            }
            else
            {
                StartCoroutine(HandleTimerColor(false));
                _irritationTimer -= 20f;
            }
        Destroy(other.gameObject);
        pickupScript._objectsOnGround.Remove(other.gameObject);
        _isSitting = true;
        _isEating = true;
        _sourceEat.PlayOneShot(_eat_sound_0, 1f);
        Invoke(nameof(ResetAnimations), 5f);
        }
        
    }

    void ControlAudioVolume()
    {
        _source.volume = 1f - _timerImage.fillAmount;
    }

    void ChangeAudioClip()
    {
        if (_irritationTimer > _maxIrritationTime / 2 && currentAudioClip == 0)
        {
            _source.clip = _meowing_0;
            _source.Play();
            currentAudioClip = 1;
        }
        if (_irritationTimer < _maxIrritationTime / 2 && currentAudioClip == 1)
        {
            _source.clip = _angryMeowing_0;
            _source.Play();
            currentAudioClip = 0;
        }
    }
    void ControlAnimations()
    {
        animator.SetBool("isSitting", _isSitting);
        animator.SetBool("isEating", _isEating);
    }

    void ResetAnimations()
    {
        _isSitting = _isSittingOnStart;
        _isEating = _isEatingOnStart;
        //Debug.Log("Invoked");
    }

    void HandleEndgameTimer()
    {
        if (_irritationTimer <= 0f)
        {
            gameManagerScript.GameTimer += Time.deltaTime;
        }
    }

    void AssignTimerUI(ScriptableCat cat)
    {
        var children = _timerUI.GetComponentsInChildren<Transform>();
        foreach (Transform image in children)
        {
            if (image.gameObject.CompareTag(cat.catColor))
                _timerImage = image.GetComponent<Image>();
        }
        _timerImage.color = _defaultTimerColor;
    }

    IEnumerator HandleTimerColor(bool isCorrect)
    {
        switch (isCorrect) 
        {
            case true:
                _timerImage.color = _correctTimerColor;
                break;
                case false:
                _timerImage.color = _wrongTimerColor;
                break;
        }
        yield return new WaitForSeconds(1f);
        _timerImage.color = _defaultTimerColor;
    }
}
