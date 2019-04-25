using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenConstants
{
    public enum ChickenTitle
    {
        None,
        Miss,
        Madam,
        Mister,
        Doctor,
        BishopM,
        BishopF,
        Lady,
        Lord,
        PresidentF,
        PresidentM,
        FL,
        FG
    }

    public static Dictionary<ChickenTitle, string> Titles = new Dictionary<ChickenTitle, string>()
    {
        {ChickenTitle.None, ""},
        {ChickenTitle.Miss, "Ms"},
        {ChickenTitle.Mister, "Mr"},
        {ChickenTitle.Madam, "Madam"},
        {ChickenTitle.Doctor, "Doctor"},
        {ChickenTitle.BishopM, "His Excellency"},
        {ChickenTitle.BishopF, "Her Excellency"},
        {ChickenTitle.Lady, "Her Ladyship"},
        {ChickenTitle.Lord, "His Lordship"},
        {ChickenTitle.PresidentF, "Madam President"},
        {ChickenTitle.PresidentM, "Mr. President"},
        {ChickenTitle.FL, "First Lady"},
        {ChickenTitle.FG, "First Gentleman"}
    };

    public static int BoredomThreshold;
    public static int HungerThreshold;

    public static int BoredomDecrement;
    public static int HungerDecrement;

    public static int AdulthoodThreshold = 30;

    public static MinMaxRange WaitIdleRange = new MinMaxRange(1f, 22f);
}
