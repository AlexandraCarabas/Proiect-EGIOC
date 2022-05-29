using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    float speed = 50;
    Animator animator;
    public Transform ball;
    public Transform aimTarget;

    public Transform[] targets;

    Vector3 targetPosition;

    ShotManager shotManager;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
    }

    
    void Update()
    {
        Move();   
    }

    void Move()
    {
        targetPosition.x = ball.position.x;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector3 PickTarget() // picks a random target from the targets array to be aimed at
    {
        int randomValue = Random.Range(0, targets.Length); // get a random value from 0 to length of our targets array-1
        return targets[randomValue].position; // return the chosen target
    }

    Shot PickShot() // picks a random shot to be played
    {
        int randomValue = Random.Range(0, 2); // pick a random value 0 or 1 since we have 2 shots possible currently
        if (randomValue == 0) // if equals to 0 return a top spin shot type
            return shotManager.topSpin;
        else                   // else return a flat shot type
            return shotManager.flat;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ball"))
        {
            Shot currentShot = PickShot();

            Vector3 dir = PickTarget() - transform.position; // get the direction to where we want to send the ball
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0); //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir = ball.position - transform.position;
            if (ballDir.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backhand");
            ball.GetComponent<Ball>().hitter = "bot";
        }
    }
}
