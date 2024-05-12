using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class QuitGame : MonoBehaviour
{
    public static QuitGame Instance;
    public GameObject finishPanel;
    public GameObject exitBtn;
    public TextMeshProUGUI winnerPlayerNameTMP;
    private void Awake()
    {
        Instance = this;
    }
    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void WinnerPlayerName(string playerName)
    {
        finishPanel.SetActive(true);
        winnerPlayerNameTMP.text = "KAZANAN OYUNCU " + playerName;
        Time.timeScale = 0;
    }
    public void WinnerPlayerNameSling(string playerName)
    {
        finishPanel.SetActive(true);
        winnerPlayerNameTMP.text = "KAZANAN " + playerName;
        Time.timeScale = 0;
    }
    public void timeScale(int value)
    {
        Time.timeScale =value;
    }
}
