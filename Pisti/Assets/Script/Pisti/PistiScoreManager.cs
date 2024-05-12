using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class PistiScoreManager : MonoBehaviour
{
    public static PistiScoreManager Instance;

    public int[] playerScore;
    public int[] playerTotalScore;

    public PistiPlayerCards[] players;

    [Header("Score And Name TMP")]
    [SerializeField] TextMeshProUGUI[] scoreInScene_TMP;
    [SerializeField] TextMeshProUGUI[] scoreInBorad_TMP;
    [SerializeField] TextMeshProUGUI[] totalScoreInScene_TMP;
    [SerializeField] TextMeshProUGUI[] playerNameInBoard_TMP;

    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject finishBoard;
    [SerializeField] TextMeshProUGUI winnerPlayerName_TMP;
    [SerializeField] TextMeshProUGUI winnerPlayerScore_TMP;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        TotalScore();
    }
    public void TotalScore()
    {
        for (int i = 0; i < playerScore.Length; i++)
        {
            scoreInScene_TMP[i].text = playerScore[i].ToString();
        }
    }
    public void AddScore(int playerIndex, int score)
    {
        playerScore[playerIndex] += score;
      //  playerTotalScore[playerIndex] += score;
    }
    public bool isGameFinish = false;
    public void ShowWinnerPlayer()
    {
        //Array.Sort(playerScore);
        //Array.Reverse(playerScore);
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerTotalScore[i] = players[i].totalScore;
        }
        Array.Sort(playerTotalScore);
        Array.Reverse(playerTotalScore);

        for (int i = 0; i < playerScore.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (playerTotalScore[i] == players[j].totalScore && !players[j].isScoreWrite)
                {
                    scoreInBorad_TMP[i].text = playerTotalScore[i].ToString()+"/101";
                    playerNameInBoard_TMP[i].text = players[j].name;
                    players[j].isScoreWrite = true;
                    if (playerTotalScore[i] >= 101)
                        isGameFinish = true;
                    winnerPlayerName_TMP.text= playerNameInBoard_TMP[0].text;
                    winnerPlayerScore_TMP.text= scoreInBorad_TMP[0].text;
                    break;
                }
            }
            totalScoreInScene_TMP[i].text = players[i].totalScore.ToString();

        }
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerScore[i] = 0;
            playerTotalScore[i] = players[i].totalScore;
            scoreInScene_TMP[i].text = playerScore[i].ToString();
            players[i].score = 0;
            players[i].isScoreWrite = false;
        }
        Invoke("ShowBoard", 2);

    }
    public void Restart()
    {
        if (!isGameFinish)
        {
            PistiDealCard.Instance.AgainStartGame();
            scoreBoard.SetActive(false);
        }
        //else
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ShowBoard()
    {
        scoreBoard.SetActive(true);
        if (isGameFinish)
            Invoke("ShowFinishBoard", 2);
    }
    void ShowFinishBoard()
    {
        finishBoard.SetActive(true);
    }

}
