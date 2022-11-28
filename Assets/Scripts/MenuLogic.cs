using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameSettings[] settings = default;
    [SerializeField] private TMP_Text difficultyLabel = default;
    
    private KatakanaController _katakanaController;
    
    private void Awake()
    {
        _katakanaController = KatakanaController.Instance();
        difficultyLabel.text = Enum.GetName(typeof(Difficulty),PlayerPrefs.GetInt("difficulty"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) LoadGame();
    }

    public void CycleDifficulty()
    {
        var cur = PlayerPrefs.GetInt("difficulty");
        PlayerPrefs.SetInt("difficulty", Enum.IsDefined(typeof(Difficulty), cur + 1) ? cur + 1 : 0);
        difficultyLabel.text = Enum.GetName(typeof(Difficulty),PlayerPrefs.GetInt("difficulty"));
    }

    public void LoadGame()
    {
        _katakanaController.ApplySettings(settings[PlayerPrefs.GetInt("difficulty")]);
        SceneManager.LoadScene("Game");
    }
}