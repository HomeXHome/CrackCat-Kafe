using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public PersistentTutorial tutorial;


    public void UpdateTutorialState()
    {
        tutorial.isTutorialCompleted = true;
    }
}
