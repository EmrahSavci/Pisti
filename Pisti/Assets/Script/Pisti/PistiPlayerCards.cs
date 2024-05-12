using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PistiPlayerCards : MonoBehaviour
{
    public Transform[] childCardPos;

    public List<GameObject> cardsInHand = new List<GameObject>();
    public TextMeshProUGUI score_TMP;
    public int score = 0;
    public int totalScore = 0;
    public Image[] facedownCardList;
    public GameObject faceDownCardPanel;

    public GameObject arrow;
    public bool isScoreWrite = false;
    void Start()
    {
        score_TMP.text = score.ToString();
    }

    
   public void GetScore(int _score)
    {
        score += _score;
        score_TMP.text = score.ToString();
        totalScore += _score;
        PistiScoreManager.Instance.AddScore(PistiDealCard.Instance.PlayerIndex, _score);
    }
    public void ShowFaceDownCards(Sprite cardSprite,int cardIndex)
    {
        faceDownCardPanel.SetActive(true);
        facedownCardList[cardIndex].sprite = cardSprite;
    }
    
}
