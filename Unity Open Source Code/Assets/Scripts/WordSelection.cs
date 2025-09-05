using System;
using UnityEngine;
using TMPro;

public class WordSelection : MonoBehaviour
{
    private float wordTimer;
    private float wordFrequency = 2f;
    public bool wordExist = false;
    public WordBank wordBank;
    public PipeTracker pipeTracker;
    public TMP_Text wordDisplay;
    public string chosenWord;

    private const float timeStep = 10f;
    private const float maxTime = 1000f;
    private const float maxBias = 0.2f;

    void Update()
    {
        wordTimer += Time.deltaTime;
        pipeTracker.timer += Time.deltaTime;

        if (wordExist != true)
        {
            wordDisplay.SetText("New Word Soon");
        }

        if (wordTimer > wordFrequency && wordExist != true)
        {

            int randomNumber = GenerateBiasedRandom();
            chosenWord = wordBank.wordList[randomNumber];
            wordDisplay.SetText(chosenWord);
            Time.timeScale = 1f;
            wordExist = true;
            wordTimer = 0;

        }
    }

    int GenerateBiasedRandom()
    {
        float stepProgress = Mathf.Floor(pipeTracker.timer / timeStep);
        float totalProgress = stepProgress * timeStep / maxTime;
        totalProgress = Mathf.Clamp01(totalProgress);

        float biasFactor = totalProgress * maxBias;

        float random = UnityEngine.Random.value;
        random = Mathf.Pow(random, 1f - biasFactor);

        int result = Mathf.RoundToInt(random * 1880);
        return result;
    }
}
