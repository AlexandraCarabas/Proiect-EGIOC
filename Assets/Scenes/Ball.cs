using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;

    int playerScore;
    int botScore;

    [SerializeField] Text playerScoreText;
    [SerializeField] Text botScoreText;

    public bool playing = true;

    private void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //transform.position = initialPos;

            GameObject.Find("Player").GetComponent<Player>().Reset();

            if (playing)
            {
                if (hitter == "player")
                {
                    playerScore++;
                }
                else if (hitter == "bot")
                {
                    botScore++;
                }
                playing = false;
            }
            
        }
        else if (collision.transform.CompareTag("Out"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            GameObject.Find("Player").GetComponent<Player>().Reset();

            if (playing)
            {
                if (hitter == "player")
                {
                    botScore++;
                }
                else if (hitter == "bot")
                {
                    playerScore++;
                }
                updateScores();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") && playing)
        {
            if(hitter == "player")
            {
                botScore++;
            }
            else if (hitter == "bot")
            {
                playerScore++;
            }
            playing=false;
            updateScores();
        }
    }

    void updateScores()
    {
        playerScoreText.text = "Player: " + playerScore;
        botScoreText.text = "Bot: " + botScore;
    }
}
