using UnityEngine;

public class TurtleLoseScene : MonoBehaviour
{
    private Animator animator;
    public enum Animatoins
    {
        idle,
        walking,
        smashing
    }
    public Animatoins activeAnimation;
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (activeAnimation == Animatoins.smashing)
            animator.SetBool("isBreaking", true);
        if (activeAnimation == Animatoins.walking)
            animator.SetBool("isWalking", true);
        if (activeAnimation == Animatoins.idle)
            animator.SetBool("isWalking", false);

    }
}
