using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PipeTracker : MonoBehaviour
{
    public GameObject pipeTop;
    public GameObject pipeBot;
    public AudioSource dieAudio;
    public AudioSource hitAudio;
    public AudioSource flapAudio;
    public GameObject player;
    public float speedtimer;

    public BirdTracker birdTracker;
    private Vector3 startingPosition;
    public Vector3 pipeTopPosition;
    public Vector3 pipeBotPosition;

    public RectTransform playerRect;
    public RectTransform pipeTopRect;
    public RectTransform pipeBotRect;

    public WordSelection wordSelection;
    public WordTyping wordTyping;
    public HighscoreManager highscoreManager;
    public ScreenManager screenManager;

    public float score;
    public TMP_Text scoreText;
    public TMP_Text userScore;
    public float timer;
    private bool collisionTop = false;
    private bool collisionBot = false;
    public bool isDead = false;
    public bool isGameActive = false;  // Track whether the game is active

    void Start()
    {
        // Ensure audio is stopped at the start
        dieAudio.Stop();
        flapAudio.Stop();
        hitAudio.Stop();
        isGameActive = false;
        birdTracker.canMove = false;

        // Get the RectTransform components of the player and pipes
        playerRect = player.GetComponent<RectTransform>();
        pipeTopRect = pipeTop.GetComponent<RectTransform>();
        pipeBotRect = pipeBot.GetComponent<RectTransform>();
        startingPosition = player.transform.position;
        pipeTopPosition = pipeTop.transform.position;
        pipeBotPosition = pipeBot.transform.position;
    }

    void Update()
    {
        if (!isGameActive) return;  // If the game is not active, do nothing (stop pipes from moving)

        // Timer for how long the player has survived
        timer += Time.deltaTime;
        score += Time.deltaTime;
        scoreText.SetText("Score: " + Mathf.FloorToInt(score));

        // Move the pipes, overtime gets faster
        speedtimer += Time.deltaTime;
        float speedMultiplier = Mathf.Pow(3f, speedtimer / 10f); // Speed increases with time
        pipeTop.transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * speedMultiplier;
        pipeBot.transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * speedMultiplier;

        // Collision detection separately
        if (IsRectTransformIntersecting(playerRect, pipeTopRect) && !collisionTop)
        {
            dieAudio.Play();
            hitAudio.Play();
            collisionTop = true;
        }

        if (IsRectTransformIntersecting(playerRect, pipeBotRect) && !collisionBot)
        {
            dieAudio.Play();
            hitAudio.Play();
            collisionBot = true;
        }

        // Death Killer Machine (Game Over Logic)
        if ((collisionBot || collisionTop) && !isDead)
        {
            isDead = true;
            birdTracker.canMove = false;
            player.transform.position = startingPosition;
            wordSelection.wordDisplay.SetText("Game Over!");
            wordTyping.typedTextDisplay.SetText(" ");
            highscoreManager.SaveHighScore(score);
            screenManager.SwitchMenus("Death");
            userScore.SetText("Score: " + Mathf.FloorToInt(score));
        }
    }

    // Function to check if two RectTransforms are intersecting (based on true world corners)
    bool IsRectTransformIntersecting(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];

        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Vector2 rect1Min = corners1[0];
        Vector2 rect1Max = corners1[2];
        Vector2 rect2Min = corners2[0];
        Vector2 rect2Max = corners2[2];

        bool isIntersecting = rect1Max.x > rect2Min.x && rect1Min.x < rect2Max.x &&
                            rect1Max.y > rect2Min.y && rect1Min.y < rect2Max.y;

        return isIntersecting;
    }

    public void FlapAudio()
    {
        if (!collisionBot && !collisionTop && isGameActive)
        {
            flapAudio.Play();
        }
    }

    public void Restart()
    {
        player.transform.position = startingPosition;
        pipeBot.transform.position = pipeBotPosition;
        pipeTop.transform.position = pipeTopPosition;
        timer = 0;
        score = 0;
        collisionBot = false;
        collisionTop = false;
        birdTracker.ResetVelocity();
        birdTracker.StartingFlap();
        dieAudio.Stop();
        hitAudio.Stop();
        Time.timeScale = 1f;
        wordSelection.wordDisplay.SetText("New Word Soon");
        wordTyping.typedTextDisplay.SetText(" ");
        wordSelection.wordExist = false;
    }

    // Method to set the game state (start or stop movement)
    public void SetGameActive(bool isActive)
    {
        isGameActive = isActive;
        if (isGameActive)
        {
            Time.timeScale = 1f;  // Resume time when the game is active
        }
        else
        {
            Time.timeScale = 0f;  // Pause the game
        }
    }
}