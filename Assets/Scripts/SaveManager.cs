using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class SaveManager 
{
    public enum Difficulty
    {
        Easy = 0,
        Noramal = 1,
        Hard = 2,
        Expert = 3,
    }

    public static bool[] ClearFlag = new bool[4];
    //åªç›ÇÃìÔà’ìx
    public static Difficulty Difficult = Difficulty.Easy;
    public static int HighScore = 0;
}
