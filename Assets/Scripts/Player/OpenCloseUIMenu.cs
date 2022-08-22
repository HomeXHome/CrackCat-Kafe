using UnityEngine;
using StarterAssets;

public class OpenCloseUIMenu : MonoBehaviour
{
    private StarterAssetsInputs _inputs;

    private float _menuTimer = 2f;
    public GameObject menuUI;


    private bool _isGameWon = false;

    private void OnEnable()
    {
        EventManager.WinEvent += HandleEndgame;
    }

    private void OnDisable()
    {
        EventManager.WinEvent += HandleEndgame;
    }

    private void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        menuUI.SetActive(false);
    }

    private void Update()
    {
        HandleMenu();
    }


    private void HandleMenu()
    {
        _menuTimer += Time.deltaTime;
        if (_menuTimer >= 2f)
            _menuTimer = 2f;
        if (!_isGameWon)
        {
            if (!menuUI.activeInHierarchy)
            {
                _inputs.cursorInputForLook = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                _inputs.cursorInputForLook = false;
                Cursor.lockState = CursorLockMode.None;
            }

            if (_menuTimer == 2f && _inputs.esc)
            {
                _inputs.cursorLocked = (!menuUI.activeInHierarchy);
                menuUI.SetActive(!menuUI.activeInHierarchy);
                _menuTimer = 0f;
                Cursor.visible = menuUI.activeInHierarchy;
                Debug.Log(Cursor.visible);
            }
        }

    }

    private void HandleEndgame()
    {
        _isGameWon = true;
    }
}
