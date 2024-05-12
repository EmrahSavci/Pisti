using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PistiDealCard : MonoBehaviour
{
    public static PistiDealCard Instance;
    public int PlayerIndex = 0;
    public int totalCardInPlaceCount = 52;
    [SerializeField] TextMeshProUGUI totalCardCount_TMP;
    public EnumHolder.GameState gameState;
    public List<PistiCardList> cardLists = new List<PistiCardList>();
    public List<PistiCardList> copyCardList = new List<PistiCardList>();
    public List<UnoCard> cardOnPlace = new List<UnoCard>();
    public int randomColor;
    int randomNumber;

    public GameObject pistiCard;
    public Sprite cardBackSprite;
    public Transform[] firstCardsPos;
    public Transform CardsParentInPlace;
    public Transform dealCardPos;
    public List<PistiPlayerCards> player = new List<PistiPlayerCards>();
    public int lastGetCardPlayerIndex = 0;
    public int tourCount = 0;
    public int startPlayerIndex = 0;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        StartCoroutine(DealingCard());
    }
    public void AgainStartGame()
    {
        for (int i = 0; i < copyCardList.Count; i++)
        {
            for (int j = 0; j < copyCardList[i].cards.Count; j++)
            {
                cardLists[i].cards.Add(copyCardList[i].cards[j]);
            }
        }
        startPlayerIndex++;
        if (startPlayerIndex >= player.Count)
            startPlayerIndex = 0;
        PlayerIndex = startPlayerIndex;
        lastGetCardPlayerIndex = 0;
        totalCardInPlaceCount = 52;
        sentCardCountToCenter = 4;
        totalCardCount_TMP.text = totalCardInPlaceCount.ToString();
        tourCount++;
        firsCards = true;
        StartCoroutine(DealingCard());
    }
    public IEnumerator DealingCard()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        for (int j = 0; j < 4; j++)
        {
            SelectFirstStartCard(j);
            yield return delay;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                DealCard(i, j);
                yield return delay;
            }
        }

        yield return new WaitForSeconds(0.5f);
        player[PlayerIndex].arrow.SetActive(true);
        isCanClickNewCard = true;
    }
    public IEnumerator AgainCardDeal(int playerIndex)
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);


        for (int j = 0; j < 4; j++)
        {
            DealCard(playerIndex, j);
            yield return delay;
        }

    }

    private void SelectFirstStartCard(int i)
    {
        GameObject _card = Instantiate(pistiCard, firstCardsPos[i].position, firstCardsPos[i].rotation, CardsParentInPlace);
        _card.GetComponent<Image>().sprite = SelectSpriteToCard();
        _card.GetComponent<UnoCard>().mySprite = _card.GetComponent<Image>().sprite;
        if (i < 3)
            _card.GetComponent<Image>().sprite = cardBackSprite;



        _card.GetComponent<UnoCard>().colorIndex = randomColor;
        _card.GetComponent<UnoCard>().numberStr = cardNumber();
        cardOnPlace.Add(_card.GetComponent<UnoCard>());
        GetColor(_card.GetComponent<UnoCard>().colorIndex, _card.GetComponent<UnoCard>().numberStr, false);
        totalCardInPlaceCount--;
        totalCardCount_TMP.text = totalCardInPlaceCount.ToString();
    }

    public void DealCard(int playerIndex, int cardIndex)
    {
        if (totalCardInPlaceCount <= 0)
        {

            return;
        }


        GameObject _card = Instantiate(pistiCard, dealCardPos.position, Quaternion.identity, dealCardPos);
        LeanTween.rotateAround(_card, new Vector3(0, 0, 360), 360, 0.5f).setRepeat(3);
        UnoSoundManager.Instance.cardDrawSoundPlay();
        // centerCard.Add(_card);
        LeanTween.move(_card, player[playerIndex].childCardPos[cardIndex], 0.15f).setOnComplete(() =>
        {
            LeanTween.cancel(_card);
            _card.transform.parent = player[playerIndex].transform;
            _card.transform.eulerAngles = player[playerIndex].childCardPos[cardIndex].transform.eulerAngles;
            _card.GetComponent<Button>().onClick.AddListener(() => SentToCenterTheCard(_card));
            _card.GetComponent<Image>().sprite = SelectSpriteToCard();
            _card.GetComponent<UnoCard>().mySprite = _card.GetComponent<Image>().sprite;
            _card.GetComponent<UnoCard>().colorIndex = randomColor;
            _card.GetComponent<UnoCard>().numberStr = cardNumber();
            _card.GetComponent<UnoCard>().playerIndex = playerIndex;
            player[playerIndex].cardsInHand.Add(_card);


        });
        totalCardInPlaceCount--;
        totalCardCount_TMP.text = totalCardInPlaceCount.ToString();
    }

    Sprite cardSprite;
    public Sprite SelectSpriteToCard()
    {

        againCardSelect:
        int samecount = 0;
        randomColor = Random.Range(0, cardLists.Count);
        randomNumber = Random.Range(0, cardLists[randomColor].cards.Count);

        if (cardLists[randomColor].cards.Count >= 1)
            cardSprite = cardLists[randomColor].cards[randomNumber];

        else
        {
            for (int i = 0; i < cardLists.Count; i++)
            {
                if (cardLists[i].cards.Count >= 1)
                    samecount++;
            }
        }

        if (samecount >= 1)
            goto againCardSelect;



        return cardSprite;
    }
    public bool isCanClickNewCard = false;
    int sentCardCountToCenter = 4;
    public void SentToCenterTheCard(GameObject card)
    {
        if (card.GetComponent<UnoCard>().playerIndex != PlayerIndex || !isCanClickNewCard)
            return;

        isCanClickNewCard = false;
        player[PlayerIndex].arrow.SetActive(false);

        card.transform.parent = CardsParentInPlace;
        //card.transform.SetSiblingIndex(0);
        // totalCardCountInCenter++;

        LeanTween.moveLocal(card, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            card.transform.eulerAngles = new Vector3(0, 0, Random.Range(-90, 90));
            cardOnPlace.Add(card.GetComponent<UnoCard>());
            player[PlayerIndex].cardsInHand.Remove(card);
            if (player[PlayerIndex].cardsInHand.Count == 0)
                StartCoroutine(AgainCardDeal(PlayerIndex));


            sentCardCountToCenter++;
            if (sentCardCountToCenter >= 52)
            {
                StartCoroutine(SentLastCards());

            }
            else
                GetColor(card.GetComponent<UnoCard>().colorIndex, card.GetComponent<UnoCard>().numberStr, true);






        });
        UnoSoundManager.Instance.SendCardToCenterPlay();

    }
    public void PassNextPlayer()
    {


        player[PlayerIndex].arrow.SetActive(false);

        if (sentCardCountToCenter >= 52)
            return;

        PlayerIndex++;
        if (PlayerIndex > 3)
            PlayerIndex = 0;

        player[PlayerIndex].arrow.SetActive(true);
        isCanClickNewCard = true;
    }
    public string cardNumber()
    {
        string number = cardLists[randomColor].cards[randomNumber].name;
        cardLists[randomColor].cards.RemoveAt(randomNumber);
        return number;

    }
    public string cardNumberStr;
    public int colorIndex = 0;
    public void GetColor(int index, string number, bool nextPlayer)
    {
        if (cardOnPlace.Count > 1 && (cardNumberStr == number || number == "joker") && nextPlayer)
        {
            isCanClickNewCard = false;
            StartCoroutine(SentCardsToPlayer());
        }

        else if (nextPlayer)
            PassNextPlayer();
        cardNumberStr = number;
        colorIndex = index;

    }
    bool firsCards = true;
    IEnumerator SentCardsToPlayer()
    {

        if (sentCardCountToCenter < 52)
        {

            int cardCount = cardOnPlace.Count;
            lastGetCardPlayerIndex = PlayerIndex;
            if (cardCount == 2 && cardOnPlace[0].numberStr == cardOnPlace[1].numberStr && (cardOnPlace[0].numberStr != "joker" || cardOnPlace[1].numberStr != "joker"))
            {
                player[PlayerIndex].GetScore(10);
                for (int i = 0; i < 2; i++)
                {
                    SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);

                }
            }

            else if (cardCount == 2 && cardOnPlace[0].numberStr == "joker" && cardOnPlace[1].numberStr == "joker")
                player[PlayerIndex].GetScore(20);
            else if (cardCount == 2 && cardOnPlace[1].numberStr == "joker")
            {
                for (int i = 0; i < 2; i++)
                {
                    SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);

                }
            }
            if (firsCards)
            {
                firsCards = false;
                for (int j = 0; j < 3; j++)
                {
                    player[lastGetCardPlayerIndex].ShowFaceDownCards(cardOnPlace[j].mySprite, j);
                    SetPlayerScore(cardOnPlace[j], lastGetCardPlayerIndex);
                    LeanTween.move(cardOnPlace[j].gameObject, player[lastGetCardPlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                    {
                        cardOnPlace[j].gameObject.SetActive(false);



                    });
                    yield return new WaitForSeconds(0.05f);
                }
                for (int i = 3; i < cardCount; i++)
                {
                    SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);
                    LeanTween.move(cardOnPlace[i].gameObject, player[lastGetCardPlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                    {
                        cardOnPlace[i].gameObject.SetActive(false);



                    });
                    yield return new WaitForSeconds(0.05f);
                }
            }
            else
            {
                for (int i = 0; i < cardCount; i++)
                {


                    if (cardCount > 2)
                        SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);
                    LeanTween.move(cardOnPlace[i].gameObject, player[lastGetCardPlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                    {
                        cardOnPlace[i].gameObject.SetActive(false);



                    });
                    yield return new WaitForSeconds(0.05f);
                }
            }



            yield return new WaitForSeconds(1);
            cardOnPlace.Clear();
            PassNextPlayer();

        }



    }
    IEnumerator SentLastCards()
    {
        int cardCount = cardOnPlace.Count;

        if (cardCount == 2 && cardOnPlace[0].numberStr == cardOnPlace[1].numberStr && (cardOnPlace[0].numberStr != "joker" || cardOnPlace[1].numberStr != "joker"))

        {
            player[PlayerIndex].GetScore(10);
            for (int i = 0; i < 2; i++)
            {
                SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);
                LeanTween.move(cardOnPlace[i].gameObject, player[PlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                {
                    cardOnPlace[i].gameObject.SetActive(false);



                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (cardCount == 2 && cardOnPlace[0].numberStr == "joker" && cardOnPlace[1].numberStr == "joker")

        {
            player[PlayerIndex].GetScore(20);
            for (int i = 0; i < 2; i++)
            {

                LeanTween.move(cardOnPlace[i].gameObject, player[PlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                {
                    cardOnPlace[i].gameObject.SetActive(false);



                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (cardCount == 2 && cardOnPlace[1].numberStr == "joker")
        {
            for (int i = 0; i < 2; i++)
            {
                SetPlayerScore(cardOnPlace[i], PlayerIndex);
                LeanTween.move(cardOnPlace[i].gameObject, player[PlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                {
                    cardOnPlace[i].gameObject.SetActive(false);



                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (cardOnPlace[cardOnPlace.Count - 1].numberStr == "joker")
        {
            for (int i = 0; i < cardCount; i++)
            {
                SetPlayerScore(cardOnPlace[i], PlayerIndex);
                LeanTween.move(cardOnPlace[i].gameObject, player[PlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                {
                    cardOnPlace[i].gameObject.SetActive(false);



                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (cardCount > 1 && cardOnPlace[cardOnPlace.Count - 2].numberStr == cardOnPlace[cardOnPlace.Count - 1].numberStr)
        {
            for (int i = 0; i < cardCount; i++)
            {


                if (cardCount > 2)
                    SetPlayerScore(cardOnPlace[i], PlayerIndex);

                LeanTween.move(cardOnPlace[i].gameObject, player[PlayerIndex].transform.position, 0.05f).setOnComplete(() =>
                {
                    cardOnPlace[i].gameObject.SetActive(false);



                });
                yield return new WaitForSeconds(0.05f);
            }
        }

        for (int i = 0; i < cardCount; i++)
        {


            if (cardCount > 2)
                SetPlayerScore(cardOnPlace[i], lastGetCardPlayerIndex);

            LeanTween.move(cardOnPlace[i].gameObject, player[lastGetCardPlayerIndex].transform.position, 0.05f).setOnComplete(() =>
            {
                cardOnPlace[i].gameObject.SetActive(false);



            });
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(1);
        cardOnPlace.Clear();
        PistiScoreManager.Instance.ShowWinnerPlayer();
    }
    void SetPlayerScore(UnoCard card, int playerIndex)
    {
        if (card.numberStr == "joker")
            player[playerIndex].GetScore(1);
        else if (card.numberStr == "10" && card.colorIndex == 1)
            player[playerIndex].GetScore(3);
        else if (card.numberStr == "2" && card.colorIndex == 3)
            player[playerIndex].GetScore(2);
        else if (card.numberStr == "a")
            player[playerIndex].GetScore(1);
    }
}
[System.Serializable]
public class PistiCardList
{
    public List<Sprite> cards = new List<Sprite>();

}
