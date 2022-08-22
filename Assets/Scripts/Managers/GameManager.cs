using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Cat Placement")]
    [Tooltip("List of all places cats can spawn")]
    [SerializeField] private List<Transform> catSpawnsPlaces = new List<Transform>();
    [Tooltip("List of all usable cats")]
    [SerializeField] private List<ScriptableCat> cats = new List<ScriptableCat>();

    public float GameTimer;
    public float LoseTime;
    public float WinTime;
    public GameObject targetUI;
    private StarterAssetsInputs _inputs;

    [SerializeField] private AudioClip _winSound;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _winUI;
    private bool _isGameStarted;
    private bool _isGameWon = false;

    private void OnEnable()
    {
        EventManager.StartEvent += OnMainGameStart;
        EventManager.WinEvent += HandleWinSound; 
    }

    private void OnDisable()
    {
        EventManager.StartEvent -= OnMainGameStart;
        EventManager.WinEvent -= HandleWinSound;
    }


    private void Update()
    {
        //HandleEndgameTimerDebugUI();
        //HandleDebug();
        HandleGameOver();

        if (_isGameStarted)
        {
            HandleWinTime();
        }
    }
    private void Start()
    {
        _inputs = FindObjectOfType<StarterAssetsInputs>();
        _winUI.SetActive(false);
        SpawnCats();
        GameTimer = 0f;
        _isGameStarted = false;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _winSound;
        _inputs.cursorLocked = true;
        _inputs.cursorInputForLook = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        targetUI.SetActive(false);
    }



    public void MainGameStart()
    {
        EventManager.OnStartEvent();
    }

    private void OnMainGameStart()
    {
        _isGameStarted = true;
        targetUI.SetActive(true);
    }

    private void SpawnCats()
    {
        List<Transform> _catSpawnPlaces = catSpawnsPlaces;
        List<ScriptableCat> _cats = cats;

        for (int i = 0; i < _cats.Count; i++)
        {
            int spawnPointIndex = Random.Range(0, _catSpawnPlaces.Count);
            Transform deltaTransform = _catSpawnPlaces[spawnPointIndex];
            Instantiate(_cats[i].catPrefab, _catSpawnPlaces[spawnPointIndex].position, deltaTransform.rotation);
            _catSpawnPlaces.Remove(_catSpawnPlaces[spawnPointIndex]);
        }
    }

    private void HandleWinTime()
    {
        if (_isGameStarted)
        WinTime -= Time.deltaTime;
    }

    private void HandleGameOver()
    {
        if (GameTimer >= LoseTime)
        {
            EventManager.OnLoseEvent();
            SceneController.LoadSceneFromMenu(3, 1, 0);
            Debug.Log("Lose");
            targetUI.SetActive(false);
        }
        if (WinTime <= 0)
        {
            EventManager.OnWinEvent();

            _winUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            _inputs.cursorInputForLook = false;
            _inputs.cursorLocked = false;
            Cursor.visible = true;
            targetUI.SetActive(false);
            GameTimer = 0f;
        }
    }

    private void HandleWinSound()
    {
        StartCoroutine(PlaySound());
    }
   
    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.5f);
        if (!_isGameWon)
        {
            _audioSource.PlayOneShot(_winSound, 0.5f);
            _isGameWon = true;
        }
    }
}
