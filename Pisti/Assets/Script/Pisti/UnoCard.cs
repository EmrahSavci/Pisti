using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCard : MonoBehaviour
{
    public enum Colors
    {
        Red,
        Yellow,
        Blue,
        Green
    }
    public Colors colors;

    public enum number
    {
        zero,
        one,
        two,
        three,
        four,
        fife,
        six,
        seven,
        eight,
        nine,
        ten,
        next,
        turn,
        twoplus,
        fourplus,
        joker
    }
    public number cardNumber;
    public int colorIndex = 0;
    public string numberStr;
    public int playerIndex = 0;
    public Sprite mySprite;
    public void SelectColor(int index)
    {
        switch (index)
        {
            case 0:
                colors = Colors.Blue;
                break;
            case 1:
                colors = Colors.Green;
                break;
            case 2:
                colors = Colors.Red;
                break;
            case 3:
                colors = Colors.Yellow;
                break;
            default:
                break;
        }
        colorIndex = index;
    }
}
