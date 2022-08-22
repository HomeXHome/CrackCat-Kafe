using UnityEngine;

public class TutorialMusic : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip _ambient;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _ambient;
        _source.PlayOneShot(_ambient);
    }

    
}
