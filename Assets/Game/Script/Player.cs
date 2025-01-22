using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textGoal;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Ball ballAttachedToPlayer;
    private float timeShot = -1f;
    public const int ANIMATION_LAYER_SHOOT = 1;
    private int myScore = 0;
    private int otherScore = 0;
    private float goalTextColorAlpha;
    private AudioSource soundDibble;
    private AudioSource soundCheer;
    private AudioSource soundKick;
    private CharacterController controller;
    private float distanceSinceLastDribble;

    // Property to expose ballAttachedToPlayer
    public Ball BallAttachedToPlayer { get => ballAttachedToPlayer; set => ballAttachedToPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        // Get required components
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        if (starterAssetsInputs == null)
        {
            Debug.LogError("StarterAssetsInputs component is missing! Ensure it is attached to the Player object.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing! Ensure it is attached to the Player object.");
        }
        soundDibble = GameObject.Find("Sound/dribble").GetComponent<AudioSource>();
        soundCheer = GameObject.Find("Sound/cheer").GetComponent<AudioSource>();
        soundKick = GameObject.Find("Sound/kick").GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = new Vector3(controller.velocity.x,0,controller.velocity.z).magnitude;
        if (starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            timeShot = Time.time;
            animator.Play("shoot", ANIMATION_LAYER_SHOOT, 0f);
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT, 1f);
        }

        if (timeShot > 0)
        {
            if (ballAttachedToPlayer != null && Time.time - timeShot > 0.2)
            {
                soundKick.Play();
                ballAttachedToPlayer.StickToPlayer = false;

                Rigidbody rigidbody = ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 shootdirection = transform.forward;
                shootdirection.y += 0.5f;
                rigidbody.AddForce(shootdirection * 20f, ForceMode.Impulse);
                ballAttachedToPlayer = null;
            }

            if (Time.time - timeShot > 0.5)
            {
                timeShot = -1f;
            }
        }
        else
        {
            // Gradually lower the weight of the shooting animation layer
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT, Mathf.Lerp(animator.GetLayerWeight(ANIMATION_LAYER_SHOOT), 0f, Time.deltaTime * 10f));
        }
        if (goalTextColorAlpha > 0)
        {
            goalTextColorAlpha -= Time.deltaTime;
            textGoal.alpha = goalTextColorAlpha;
            textGoal.fontSize = 200 - (goalTextColorAlpha * 1 - 0);
        }

        if (ballAttachedToPlayer != null)
        {
            distanceSinceLastDribble += speed * Time.deltaTime;
            if (distanceSinceLastDribble > 3)
            {
                soundDibble.Play();
                distanceSinceLastDribble = 0;
            }
        }
    }

    public void IncreaseMyScore()
    {
        myScore++;
        UpdateScore();
    }

    public void IncreaseOtherScore()
    {
        otherScore++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        soundCheer.Play();
        textScore.text = "Score: " + myScore.ToString() + "-" + otherScore.ToString();
        goalTextColorAlpha = 1f;
        //scoreText.text = $"Team 1: {team1Score} - Team 2: {team2Score}";
    }
}


