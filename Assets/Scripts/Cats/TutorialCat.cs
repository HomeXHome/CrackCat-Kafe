using System.Collections;
using UnityEngine;

public class TutorialCat : MonoBehaviour
{
    [SerializeField] private AudioSource _completeAudioSource;
    [SerializeField] private AudioSource _foodAudioSource;
    [SerializeField] AudioClip _completedClip;
    [SerializeField] AudioClip _foodSound;
    public PersistentTutorial tutorial;

   


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractablePipe"))
        {
            Destroy(other.gameObject);
            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        _foodAudioSource.PlayOneShot(_foodSound, 1f);
        _completeAudioSource.PlayOneShot(_completedClip, 1f);
        tutorial.isTutorialCompleted = true;
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneController.LoadSceneFromMenu(2, 2, 1);
    }

}
