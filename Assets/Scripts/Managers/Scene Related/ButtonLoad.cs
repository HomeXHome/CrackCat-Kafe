using UnityEngine;

public class ButtonLoad : MonoBehaviour
{
    public int sceneIndex;
    public void LoadScene()
    {
        SceneController.LoadSceneFromMenu(sceneIndex);
    }
}
