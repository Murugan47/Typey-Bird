using UnityEngine;
using TMPro;

public class WordTyping : MonoBehaviour
{
    public WordSelection wordSelection;
    public PipeTracker pipeTracker;
    public TMP_Text typedTextDisplay;
    public WordValueSheet wordValueSheet;
    public AudioSource points;
    public AudioSource charFail;
    private int totalScore;
    private string userInput = "";

    void Start()
    {
        points.Stop();
        charFail.Stop();
    }

    void Update()
    {

        if (wordSelection.wordExist != true)
        {
            userInput = "";
            typedTextDisplay.SetText(userInput);
        }

        foreach (char c in Input.inputString)
        {
            if (wordSelection.wordExist)
            {
                if (char.IsLetter(c))
                {
                    userInput += c;
                    UpdateColoredText();
                    CheckLetters();
                }
            }
        }
    }

    void UpdateColoredText()
    {
        string coloredText = "";
        int compareLength = Mathf.Min(userInput.Length, wordSelection.chosenWord.Length);

        for (int i = 0; i < userInput.Length; i++)
        {
            if (i < compareLength)
            {
                if (userInput[i] == wordSelection.chosenWord[i])
                {
                    coloredText += $"<color=white>{userInput[i]}</color>";
                }
                else
                {
                    coloredText += $"<color=red>{userInput[i]}</color>";
                }
            }
            else
            {
                coloredText += userInput[i]; // Letters beyond chosenWord (if any)
            }
        }

        typedTextDisplay.SetText(coloredText);
    }

    void CheckLetters()
    {
        int correctLetterCount = 0;
        int incorrectLetterCount = 0;

        int compareLength = Mathf.Min(userInput.Length, wordSelection.chosenWord.Length);

        for (int i = 0; i < compareLength; i++)
        {
            if (userInput[i] == wordSelection.chosenWord[i])
            {
                correctLetterCount++;

                string letter = userInput[i].ToString().ToLower();
                if (wordValueSheet.wordValue.ContainsKey(letter))
                {
                    totalScore += wordValueSheet.wordValue[letter];
                }
            }
            else
            {
                incorrectLetterCount++;
                charFail.Play();
            }
        }

        if (userInput.Length == wordSelection.chosenWord.Length)
        {
            if (correctLetterCount == wordSelection.chosenWord.Length)
            {
                wordSelection.wordDisplay.SetText("Correct!");
                pipeTracker.pipeBot.transform.position = pipeTracker.pipeBotPosition;
                pipeTracker.pipeTop.transform.position = pipeTracker.pipeTopPosition;
                pipeTracker.speedtimer = 0;
                points.Play();
            }
            else if (correctLetterCount >= incorrectLetterCount)
            {
                wordSelection.wordDisplay.SetText("Close!");
                pipeTracker.pipeBot.transform.position = pipeTracker.pipeBotPosition;
                pipeTracker.pipeTop.transform.position = pipeTracker.pipeTopPosition;
                points.Play();
            }
            else
            {
                wordSelection.wordDisplay.SetText("Failed!");
                points.Play();
            }

            pipeTracker.score += totalScore;

            // Reset
            userInput = "";
            typedTextDisplay.SetText(userInput);
            wordSelection.wordExist = false;
            totalScore = 0;
        }
    }
}