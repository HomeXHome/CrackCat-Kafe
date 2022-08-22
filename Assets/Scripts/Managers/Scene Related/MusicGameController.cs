using UnityEngine;

public class MusicGameController : MonoBehaviour
{
    public AudioSource _audioSource;

    [SerializeField] private AudioClip _outsideAmbient;
    [SerializeField] private AudioClip _insideMusic;

    private void OnEnable()
    {
        EventManager.StartEvent += ChangeMusic;
    }

    private void OnDisable()
    {
        EventManager.StartEvent -= ChangeMusic;
    }

    private void Start()
    {
        _audioSource.PlayOneShot(_outsideAmbient);
    }


    void ChangeMusic()
    {
        _audioSource.PlayOneShot(_insideMusic);
    }
}
