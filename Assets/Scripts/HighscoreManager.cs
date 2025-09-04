using UnityEngine;
using System.IO;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    private string filePath;
    public int highScore = 0;
    public TMP_Text highScoreText;

    void Start()
    {
        // Set the path to the highscore file (you can adjust this path as needed)
        filePath = Application.dataPath + "/highscore.txt";  // This saves in the project folder

        Debug.Log("Saving high score to: " + filePath);  // Debug log to check the file path
        LoadHighScore();
    }

    public void SaveHighScore(float newScore)
    {
        // Only save if the new score is higher than the existing high score
        if (newScore > highScore)
        {
            highScore = Mathf.FloorToInt(newScore);
            File.WriteAllText(filePath, highScore.ToString());

            // Update the high score text on the UI
            highScoreText.SetText("New Highscore: " + highScore.ToString());
            Debug.Log("New high score saved: " + highScore);
        }
    }

    void LoadHighScore()
    {
        // Check if the high score file exists
        if (File.Exists(filePath))
        {
            string scoreText = File.ReadAllText(filePath);
            int.TryParse(scoreText, out highScore);
            Debug.Log("High score loaded: " + highScore);
            highScoreText.SetText("Highscore: " + highScore.ToString());
        }
        else
        {
            // If no high score file is found, start fresh
            Debug.Log("No high score found. Starting fresh.");
            highScore = 0;
            highScoreText.SetText("Highscore: " + highScore.ToString());
        }
    }
}