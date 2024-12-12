using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public string playerName;
    public int highScore;
    public string highScorePlayerName;

    public Text highScores;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load player name and high score on awakening
        LoadName();
    }

    void Start()
    {
        //highScores = GameObject.Find("Best Score2");
        if (highScores != null)
        {
            highScores = highScores.GetComponent<Text>();
            UpdateHighScoreText(); // İlk değerleri güncelle
        }
        // Optionally, initialize UI elements here
        LoadName();
    }

    void Update()
    {
        // Update UI or other elements if needed
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game.");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ReadString(string s)
    {
        playerName = s;
        Debug.Log(playerName);
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
        public string highScorePlayerName;
    }
    
    public void SaveName()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highScore = highScore;
        data.highScorePlayerName = highScorePlayerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
    }

    // Call this method to set a new high score
    public void UpdateHighScore(int score, string name)
    {
        if (score > highScore)
        {
            highScore = score;
            highScorePlayerName = name;
            SaveName(); // Save the updated high score
        }
    }
    private void UpdateHighScoreText()
    {
        // Eğer Text bileşeni varsa, UI'yi güncelle
        if (highScores != null)
        {
            highScores.text = $"Best Score: {highScorePlayerName} - {highScore}";
        }
    }
}
