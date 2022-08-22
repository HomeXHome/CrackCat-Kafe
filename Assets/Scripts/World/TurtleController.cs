using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    [Tooltip("List of all places turtle will move during the game, needs to be 4 Transform")]
    [SerializeField] private List<Transform> turtleMoveList = new();

    private Animator animator;
    public enum Animatoins
    {
        idle,
        walking,
        smashing
    }
    public Animatoins activeAnimation;

    private GameManager gameManager;

    private float dangerLevel0;
    private float dangerLevel1;
    private float dangerLevel2;
    private float dangerLevel3;

    private float turtleSpeed = 10f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            dangerLevel0 = 0f;
            dangerLevel1 = gameManager.LoseTime * 0.3f;
            dangerLevel2 = gameManager.LoseTime * 0.6f;
            dangerLevel3 = gameManager.LoseTime * 0.9f;
        }
        transform.position = turtleMoveList[0].position;
    }

    private void Update()
    {   
        if (gameManager != null)
        MoveTurtle();
        HandleAnimations();
    }

    private void MoveTurtle()
    {
        if (gameManager.GameTimer <= dangerLevel0)
            transform.position = turtleMoveList[0].position;
        if (gameManager.GameTimer <= dangerLevel1 && gameManager.GameTimer >= dangerLevel0)
            transform.position = ReturnMoveTowards(0,1);
        if (gameManager.GameTimer <= dangerLevel2 && gameManager.GameTimer > dangerLevel1)
            transform.position = ReturnMoveTowards(1, 2);
        if (gameManager.GameTimer <= dangerLevel3 && gameManager.GameTimer > dangerLevel2)
            transform.position = ReturnMoveTowards(2, 3);
    }

    private Vector3 ReturnMoveTowards(int currentPlaceInList, int nextPlaceInList)
    {
        return Vector3.MoveTowards(turtleMoveList[currentPlaceInList].position, turtleMoveList[nextPlaceInList].position, turtleSpeed * Time.deltaTime);
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
