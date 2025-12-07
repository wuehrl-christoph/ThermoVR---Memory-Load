using UnityEngine;
using System.Collections.Generic;

public static class Constants
{
    // Letter sequences for the 2-back task
    public static Dictionary<int, char[]> BLOCKS = new Dictionary<int, char[]>
    {
        {0, new char[] {'L', 'N', 'A', 'C', 'U', 'F', 'B', 'Q', 'I', 'Q', 'B', 'J', 'S', 'W', 'U', 'F', 'C', 'F', 'A', 'Q', 'J', 'X', 'M', 'M', 'X', 'M', 'M', 'Z', 'L', 'L', 'Y', 'L', 'B', 'S', 'H', 'G', 'C', 'L', 'C', 'O', 'J', 'M', 'I', 'U', 'F', 'W'} },
        {1, new char[] {'O', 'N', 'K', 'P', 'C', 'U', 'C', 'B', 'K', 'E', 'K', 'D', 'T', 'Q', 'N', 'Y', 'P', 'N', 'C', 'R', 'F', 'Q', 'U', 'T', 'U', 'T', 'T', 'P', 'D', 'P', 'Q', 'W', 'X', 'K', 'B', 'J', 'M', 'V', 'O', 'M', 'Q', 'A', 'W', 'L', 'D', 'V'} },
        {2, new char[] {'B', 'D', 'L', 'D', 'C', 'N', 'G', 'O', 'F', 'S', 'W', 'A', 'T', 'D', 'A', 'W', 'W', 'Y', 'W', 'M', 'X', 'Q', 'Y', 'K', 'Q', 'O', 'Q', 'R', 'Y', 'R', 'J', 'G', 'M', 'G', 'U', 'P', 'N', 'X', 'M', 'U', 'Z', 'T', 'W', 'W', 'L', 'N'} },
        {3, new char[] {'B', 'H', 'C', 'H', 'E', 'T', 'M', 'T', 'K', 'Q', 'E', 'Q', 'B', 'Q', 'H', 'L', 'O', 'V', 'D', 'Y', 'X', 'V', 'U', 'T', 'L', 'S', 'N', 'E', 'C', 'K', 'G', 'K', 'T', 'F', 'U', 'X', 'C', 'L', 'R', 'V', 'J', 'A', 'B', 'I', 'R', 'G'} },
        {4, new char[] {'A', 'B', 'C', 'D'}},
        {5, new char[] {'M', 'S', 'E', 'A', 'F', 'V', 'G', 'J', 'R', 'B', 'C', 'U', 'E', 'X', 'H', 'H', 'X', 'V', 'V', 'V', 'F', 'L', 'C', 'A', 'C', 'E', 'Z', 'E', 'V', 'K', 'V', 'O', 'K', 'H', 'N', 'K', 'B', 'K', 'I', 'H', 'F', 'D', 'Z', 'K', 'D', 'O'} },
        {6, new char[] {'R', 'X', 'Q', 'O', 'Q', 'V', 'E', 'A', 'M', 'J', 'W', 'V', 'O', 'S', 'X', 'V', 'L', 'V', 'L', 'L', 'B', 'N', 'B', 'W', 'R', 'G', 'A', 'L', 'T', 'S', 'N', 'B', 'P', 'L', 'L', 'R', 'J', 'D', 'F', 'A', 'F', 'S', 'P', 'M', 'N', 'X'} },
        {7, new char[] {'V', 'V', 'A', 'U', 'C', 'T', 'X', 'Q', 'E', 'F', 'E', 'U', 'B', 'G', 'V', 'G', 'L', 'S', 'D', 'S', 'K', 'D', 'U', 'R', 'N', 'O', 'L', 'T', 'U', 'B', 'I', 'I', 'N', 'I', 'Y', 'W', 'Y', 'M', 'R', 'V', 'S', 'R', 'H', 'T', 'M', 'X'} },
        {8, new char[] {'Y', 'E', 'S', 'U', 'H', 'C', 'C', 'H', 'F', 'S', 'F', 'C', 'E', 'D', 'B', 'F', 'W', 'P', 'M', 'A', 'M', 'Z', 'Z', 'V', 'Z', 'D', 'K', 'D', 'C', 'B', 'C', 'Q', 'X', 'U', 'B', 'A', 'O', 'P', 'X', 'S', 'P', 'W', 'H', 'K', 'K', 'L'} },
        {9, new char[] {'A', 'B', 'C', 'D'}}
    };
    // The letter sequence used in the tutorial room
    public static char[] TEST_SEQUENCE = { 'L', 'S', 'G', 'S', 'Z', 'G', 'J', 'G', 'J', 'J', 'J', 'J', 'V', 'G', 'M', 'A', 'I', 'E', 'I', 'Y', 'Z', 'G', 'Z', 'P', 'K', 'P', 'S', 'P', 'Z', 'M', 'O', 'C', 'K', 'A', 'U', 'D', 'T', 'W', 'E', 'C', 'D', 'A', 'L', 'A', 'I', 'L', 'W', 'N', 'J', 'N', 'J', 'Z', 'T', 'N', 'O', 'L', 'I', 'Z', 'Y', 'X', 'X', 'B', 'J', 'J', 'E', 'B', 'N', 'H', 'E', 'Y', 'T', 'U', 'M', 'H', 'V', 'N', 'H', 'F', 'Y', 'O', 'Q' };
    // explanations for the tasks
    public static string N_BACK_EXPLANATION = "In this scene, you have to do the 2-back task. Observe the letters and press the interaction button (on the top) every time the letter is the same as the one two letters before. Start by pressing the interaction button.";
    public static string NO_BACK_EXPLANATION = "In this scene, just observe the environment. The letters will simply display the alphabet. Start by pressing the interaction button (on the top).";
    public static string IDLE_EXPLANATION = "In this scene, just observe the environment. Start by pressing the interaction button (on the top).";
}
