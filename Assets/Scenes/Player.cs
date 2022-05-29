using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget;
    float speed = 3f;
    //float force = 13;

    bool hitting;

    public Transform ball;

    Animator animator;

    Vector3 aimTargetInitialPosition;

    ShotManager shotManager;
    Shot currentShot;

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;

    bool servedRight = true;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topSpin;
    }

    // Update is called once per frame
    void Update()
    {
         
        
        float h = Input.GetAxisRaw("Horizontal"); // get the horizontal axis of the keyboard
        float v = Input.GetAxisRaw("Vertical"); // get the vertical axis of the keyboard
       
        

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true;
            currentShot = shotManager.topSpin;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true;
            currentShot = shotManager.flat;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hitting = true;
            currentShot = shotManager.kickServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            hitting = true;
            currentShot = shotManager.flatServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }

        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
        {
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            animator.Play("serve");
            ball.GetComponent<Ball>().hitter = "player";
            ball.GetComponent<Ball>().playing = true;
        }

        

        if (hitting)
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime);
        }


        if ((h != 0 || v != 0) && !hitting)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0,currentShot.upForce,0 ); //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir = ball.position - transform.position;
            if (ballDir.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backhand");


            ball.GetComponent<Ball>().hitter = "player";
            aimTarget.position = aimTargetInitialPosition;
        }
    }

    public void Reset()
    {
        if(servedRight)
            transform.position = serveLeft.position;
        else
            transform.position = serveRight.position;

        servedRight = !servedRight; 
    }

}
