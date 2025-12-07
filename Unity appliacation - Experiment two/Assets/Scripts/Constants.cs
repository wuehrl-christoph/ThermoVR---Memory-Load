using UnityEngine;
using System.Collections.Generic;

public static class Constants
{
    // letter sequences for the 1-back task
    public static Dictionary<int, char[]> BLOCKS_1_BACK = new Dictionary<int, char[]>
    {
        {0, new char[] {'X', 'Q', 'X', 'X', 'G', 'K', 'C', 'G', 'K', 'X', 'A', 'A', 'A', 'H', 'W', 'C', 'D', 'J', 'Z', 'G', 'W', 'U', 'Q', 'Q', 'P', 'Z', 'J', 'S', 'S', 'K', 'U', 'E', 'J', 'H', 'B', 'U', 'Q', 'G', 'N', 'K', 'T', 'R', 'A', 'W', 'Z', 'E'} },
        {1, new char[] {'R', 'K', 'S', 'F', 'O', 'O', 'M', 'J', 'I', 'S', 'K', 'L', 'R', 'S', 'R', 'Z', 'B', 'A', 'X', 'I', 'R', 'P', 'B', 'A', 'A', 'V', 'M', 'C', 'W', 'W', 'R', 'J', 'B', 'P', 'P', 'Z', 'Z', 'F', 'M', 'E', 'D', 'R', 'V', 'P', 'F', 'I'} },
        {2, new char[] {'B', 'S', 'S', 'F', 'I', 'M', 'K', 'H', 'R', 'P', 'P', 'K', 'Z', 'D', 'X', 'T', 'U', 'U', 'S', 'M', 'Q', 'F', 'F', 'U', 'D', 'F', 'V', 'B', 'F', 'F', 'H', 'L', 'I', 'Z', 'E', 'M', 'X', 'P', 'U', 'Z', 'D', 'Q', 'J', 'O', 'S', 'H'} },
        {3, new char[] {'S', 'I', 'C', 'C', 'M', 'B', 'B', 'R', 'B', 'Y', 'B', 'K', 'F', 'F', 'W', 'Y', 'Z', 'W', 'C', 'E', 'E', 'X', 'C', 'P', 'C', 'P', 'X', 'Y', 'Y', 'O', 'L', 'S', 'H', 'E', 'H', 'D', 'P', 'J', 'X', 'A', 'V', 'L', 'Z', 'H', 'T', 'C'} },
        {4, new char[] {'A', 'B', 'C', 'D'}},
        {5, new char[] {'P', 'G', 'V', 'S', 'M', 'M', 'T', 'Y', 'B', 'Z', 'E', 'H', 'F', 'K', 'V', 'Y', 'H', 'T', 'B', 'H', 'E', 'N', 'F', 'Q', 'A', 'W', 'W', 'G', 'I', 'I', 'I', 'F', 'E', 'B', 'W', 'G', 'M', 'V', 'P', 'P', 'S', 'X', 'N', 'D', 'L', 'F'} },
        {6, new char[] {'U', 'J', 'S', 'H', 'S', 'E', 'T', 'P', 'E', 'X', 'A', 'F', 'J', 'N', 'D', 'M', 'W', 'W', 'T', 'T', 'W', 'S', 'U', 'P', 'I', 'A', 'A', 'F', 'W', 'O', 'A', 'B', 'B', 'Z', 'T', 'T', 'U', 'R', 'P', 'V', 'G', 'U', 'J', 'A', 'T', 'V'} },
        {7, new char[] {'C', 'I', 'F', 'T', 'Y', 'K', 'Z', 'S', 'A', 'A', 'J', 'R', 'F', 'U', 'Z', 'R', 'G', 'N', 'D', 'D', 'I', 'I', 'E', 'O', 'T', 'U', 'U', 'G', 'A', 'X', 'X', 'B', 'N', 'K', 'C', 'T', 'K', 'Y', 'E', 'W', 'V', 'N', 'C', 'A', 'H', 'I'} },
        {8, new char[] {'R', 'E', 'D', 'Q', 'Q', 'N', 'A', 'I', 'U', 'D', 'Q', 'Q', 'V', 'S', 'T', 'N', 'M', 'J', 'R', 'O', 'O', 'R', 'T', 'F', 'H', 'K', 'K', 'C', 'E', 'W', 'M', 'L', 'L', 'H', 'Z', 'E', 'I', 'Z', 'S', 'X', 'U', 'S', 'G', 'E', 'G', 'H'} },
        {9, new char[] {'A', 'B', 'C', 'D'}}
    };

    // letter sequences for the 3-back
    public static Dictionary<int, char[]> BLOCKS_3_BACK = new Dictionary<int, char[]>
    {
        {0, new char[] {'M', 'R', 'K', 'U', 'O', 'O', 'P', 'S', 'Z', 'P', 'L', 'A', 'F', 'O', 'H', 'O', 'M', 'S', 'O', 'B', 'K', 'S', 'O', 'W', 'Z', 'F', 'W', 'M', 'M', 'Q', 'W', 'H', 'Q', 'W', 'M', 'F', 'J', 'G', 'Q', 'X', 'Y', 'J', 'R', 'X', 'O', 'N'} },
        {1, new char[] {'L', 'Y', 'Z', 'U', 'K', 'Z', 'Y', 'W', 'H', 'X', 'Y', 'K', 'R', 'Y', 'G', 'E', 'K', 'M', 'E', 'X', 'C', 'Y', 'Y', 'X', 'T', 'N', 'X', 'J', 'M', 'H', 'O', 'M', 'M', 'J', 'F', 'E', 'U', 'O', 'L', 'I', 'U', 'B', 'A', 'I', 'H', 'K'} },
        {2, new char[] {'J', 'C', 'F', 'T', 'T', 'U', 'N', 'P', 'H', 'R', 'J', 'H', 'R', 'G', 'G', 'X', 'B', 'T', 'Z', 'Q', 'S', 'G', 'Z', 'V', 'G', 'A', 'X', 'V', 'Q', 'V', 'G', 'V', 'V', 'D', 'D', 'K', 'J', 'I', 'K', 'M', 'Z', 'E', 'T', 'Y', 'P', 'J'} },
        {3, new char[] {'O', 'X', 'Y', 'P', 'M', 'F', 'K', 'M', 'I', 'W', 'F', 'P', 'W', 'J', 'Y', 'O', 'Q', 'E', 'I', 'K', 'O', 'O', 'U', 'O', 'S', 'A', 'D', 'Q', 'Q', 'Y', 'C', 'U', 'P', 'T', 'U', 'K', 'Y', 'F', 'V', 'Y', 'H', 'K', 'P', 'O', 'Q', 'C'} },
        {4, new char[] {'A', 'B', 'C', 'D'}},
        {5, new char[] {'E', 'S', 'K', 'Q', 'Y', 'G', 'N', 'C', 'A', 'H', 'C', 'F', 'W', 'X', 'F', 'Y', 'R', 'F', 'I', 'C', 'W', 'B', 'C', 'O', 'Q', 'X', 'E', 'B', 'O', 'B', 'V', 'E', 'U', 'D', 'E', 'Y', 'B', 'O', 'C', 'H', 'V', 'F', 'Q', 'N', 'Q', 'D'} },
        {6, new char[] {'U', 'M', 'H', 'J', 'N', 'B', 'M', 'U', 'B', 'F', 'V', 'X', 'T', 'Z', 'G', 'A', 'Z', 'B', 'H', 'R', 'L', 'O', 'W', 'H', 'O', 'X', 'I', 'G', 'S', 'T', 'P', 'C', 'Q', 'O', 'Z', 'W', 'O', 'N', 'R', 'S', 'N', 'N', 'R', 'X', 'Z', 'L'} },
        {7, new char[] {'W', 'D', 'M', 'P', 'E', 'K', 'P', 'C', 'B', 'I', 'M', 'U', 'I', 'Z', 'N', 'U', 'M', 'X', 'U', 'E', 'L', 'B', 'R', 'L', 'A', 'G', 'H', 'A', 'R', 'V', 'S', 'Q', 'Q', 'G', 'D', 'G', 'C', 'U', 'H', 'X', 'J', 'Q', 'C', 'E', 'N', 'U'} },
        {8, new char[] {'I', 'H', 'J', 'K', 'P', 'S', 'B', 'L', 'J', 'R', 'L', 'D', 'F', 'L', 'X', 'F', 'H', 'J', 'R', 'Z', 'P', 'E', 'Z', 'X', 'B', 'X', 'T', 'L', 'U', 'U', 'K', 'I', 'T', 'X', 'Z', 'D', 'J', 'Z', 'U', 'G', 'V', 'A', 'X', 'C', 'L', 'H'} },
        {9, new char[] {'A', 'B', 'C', 'D'}}
    };
    
    // 1-back letter sequence for the tutorial
    public static char[] ONE_BACK_TEST_SEQUENCE = { 'Q', 'V', 'Y', 'T', 'X', 'X', 'G', 'G', 'K', 'L', 'L', 'X', 'X', 'G', 'K', 'E', 'L', 'A', 'T', 'T', 'F', 'K', 'B', 'O', 'H', 'Y', 'Y', 'C', 'K', 'H', 'K', 'L', 'O', 'M', 'M', 'M', 'M', 'X', 'T', 'T', 'G', 'L', 'K', 'H', 'H', 'Z', 'F', 'T', 'J', 'B', 'O', 'G', 'G', 'G', 'G', 'F', 'B', 'R', 'M', 'I', 'F', 'T', 'I', 'Y', 'Y', 'Z', 'D', 'Q', 'Z', 'S', 'L', 'C', 'O', 'T', 'I', 'D', 'X', 'J', 'E', 'Z', 'X', 'V', 'N', 'M', 'T', 'I' };
    // 3-back letter sequence for the tutorial
    public static char[] THREE_BACK_TEST_SEQUENCE = { 'W', 'J', 'S', 'S', 'I', 'R', 'Y', 'O', 'G', 'U', 'O', 'L', 'F', 'V', 'O', 'F', 'H', 'D', 'B', 'L', 'D', 'B', 'Q', 'A', 'B', 'Z', 'Z', 'L', 'F', 'K', 'L', 'L', 'I', 'K', 'M', 'L', 'K', 'U', 'Z', 'L', 'V', 'T', 'G', 'V', 'V', 'R', 'K', 'C', 'U', 'K', 'C', 'P', 'S', 'V', 'R', 'Y', 'V', 'Z', 'F', 'S', 'G', 'J', 'T', 'U', 'L', 'J', 'H', 'A', 'K', 'Y', 'V', 'S', 'X', 'Z', 'P', 'G', 'M', 'I', 'C', 'T', 'O', 'F', 'V', 'I', 'K', 'M' };
    // Task explanations
    public static string ONE_BACK_EXPLANATION = "In this scene, you have to do the 1-back task. Observe the letters and press the interaction button (on the top) every time the letter is the same as the letters before. Start by pressing the interaction button.";
    public static string THREE_Back_EXPLANATION = "In this scene, you have to do the 3-back task. Observe the letters and press the interaction button (on the top) every time the letter is the same as the one three letters before. Start by pressing the interaction button.";
}
