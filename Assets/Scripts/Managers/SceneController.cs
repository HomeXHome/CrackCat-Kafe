using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public  PersistentTutorial tutorial;
    public Image fader;
    
    private static SceneController instance;
    private static bool isTutorialCompleted;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
            fader.gameObject.SetActive(false);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        isTutorialCompleted = tutorial.isTutorialCompleted;
    }

    public static void LoadSceneFromMenu(int index = 1, float duration = 1f, float waitTime = 2f)
    {
        if (isTutorialCompleted == true && index == 1)
            index = 2;

        instance.StartCoroutine(instance.FadeScene(index, duration, waitTime));
    }


    private IEnumerator FadeScene(int index, float duration, float waitTime)
    {
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        fader.gameObject.SetActive(false);
    }

    public static void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Quitted");
    }

    
}
