// This code is heavily based on the code used by Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg). As the new implemented and allready existing code are highly intertwined it can 
//hardly be highlighted which source code is from the work by Wuehrl et al. and which is newly implemented.
//However, the changes are outlined in the master theisis in the implemetiation section for study one.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnvGameManager : MonoBehaviour
{
    // diplaying the letters
    [Header("TargetManager")]
    private float targetTimout = 2f; 
    private float targetTimer = 0f;
    public GameObject targetHolder;
    private int numTargets;
    private bool isTargetVisible = false;

    // manages the questionairs
    [Header("Questionnaire Manager")]
    private float questionnaireTimeInterval = 90f;
    private int questionnaireCount = 0; 
    private bool isQuestionaireDone = false;
    public Slider slider;
    public GameObject thermalSensationUI;
    private bool isQuestionTime = false;
    private float[] thermalSensationScores = new float[4];
    private DateTime[] questionTimestamps = new DateTime[4];


    // Scene Manager
    private float sceneTimer = 0f;
    private float questionTimer = 0f;
    private float maxSceneTime = 360f;
    private TextWriter tw;
    private string filePath = Application.dataPath + "/CSV-Data/env_.csv";
    public GameObject leftControllerRay;
    public GameObject rightControllerRay;

    // loads letters for the n-back tasks
    private Dictionary<char, GameObject> letters_dict = new Dictionary<char, GameObject>();
    private string white_letters_path = "white_letters_outlined/";
    private string black_letters_path = "black_letters_outlined/";
    private char[] letters_chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
    'U', 'V', 'W', 'X', 'Y', 'Z'};
    private string outline_sufix = "_Outline";
    private System.Random random = new System.Random();

    // n-back related constance
    private static int blockCount = 0;
    private ArrayList letter_sequence = new ArrayList();
    private int sequenceCounter = 0;
    private int NUMBER_OF_LETTERS = 45;
    private string filePathPerformance;
    private string filePathPerformanceBack;

    // last target safe
    private int lastTarget = -1;

    // task related variables
    private int N_FOR_N_BACK = 2;
    private int taskIndex = -2;
    private int taskscore = 0;
    private int taskscorePositive = 0;
    private int taskscoreNegative = 0;
    private int backTaskScore = 0;
    private int backTaskScorePositive = 0;
    private int backTaskScoreNegative = 0;
    private bool backLetterChecked = false;
    private ArrayList taskscoreSafe = new ArrayList();
    private ArrayList taskscoreSafePos = new ArrayList();
    private ArrayList taskscoreSafeNeg = new ArrayList();
    private ArrayList backTaskScoreSafe = new ArrayList();
    private ArrayList backTaskScorePositiveSafe = new ArrayList();
    private ArrayList backTaskScoreNegativeSafe = new ArrayList();
    private bool letterChecked = false;

    // Button related variables
    public InputActionReference buttonPress;
    public TextMeshProUGUI explanation;
    public GameObject explanationCard;
    private bool trackTime = false;

    // back button
    public InputActionReference backButtonPress;

    // Start is called before the first frame update
    void Start()
    {
        numTargets = targetHolder.transform.childCount;
        int currentTask = PlayerPrefs.GetInt("task counter");
        taskIndex = PlayerPrefs.GetInt("t" + currentTask);
        Debug.Log("task: " + taskIndex);
        int currentScene = PlayerPrefs.GetInt("scene counter");
        int userId = PlayerPrefs.GetInt("pid");
        int envIndex = PlayerPrefs.GetInt("s"+currentScene);
        Debug.Log("counter: " + currentScene + ", index: " + envIndex);
        string letter_path = "";
        string explanationText;
        if(taskIndex == 2) // 2- back
        {
            explanationText = Constants.N_BACK_EXPLANATION;
        }
        else if (taskIndex == 0) // 0 -back
        {
            explanationText = Constants.NO_BACK_EXPLANATION;
        }
        else // idle
        {
            explanationText = Constants.IDLE_EXPLANATION;
        }
        explanation.text = explanationText;
        if(envIndex == 1)
        {
            //black letters
            letter_path = black_letters_path;
        }
        else if (envIndex == 6)
        {
            // white letters
            letter_path = white_letters_path;
        }
        foreach (char char_letter in letters_chars)
        {
            letters_dict.Add(char_letter, Resources.Load<GameObject>(letter_path + char_letter.ToString() + outline_sufix));
        }
        setLetterSequence();        
        filePath = Application.dataPath + "/CSV-Data/" + userId + "_count" + currentScene + "_env" + envIndex + "_task" + taskIndex + "_sensation.csv" ;
        filePathPerformance = Application.dataPath + "/CSV-Data/" + userId + "_count" + currentScene + "_env" + envIndex + "_task" + taskIndex + "_performance.csv" ;
        filePathPerformanceBack = Application.dataPath + "/CSV-Data/" + userId + "_count" + currentScene + "_env" + envIndex + "_task" + taskIndex + "_performance_back.csv" ;
        setupButton();
        setupBackButton();
    }

    // Update is called once per frame
    void Update()
    {
        // check if its questionnaire time or scene change
        if(trackTime && !isQuestionTime)
        {
            checkSceneTime();
        }

        if (!isQuestionTime && taskIndex != -1 && trackTime)
        {
            // checks if targets are shown
            checkTargets();
        }

    }

    private void setupButton()
    {
        Debug.Log("Setup button");
        buttonPress.action.started += onButtonPressed;
        buttonPress.action.Enable();
    }

    private void onButtonPressed(InputAction.CallbackContext context)
    {
        if(explanationCard.activeInHierarchy)
        {
            leftControllerRay.gameObject.SetActive(false);
            rightControllerRay.gameObject.SetActive(false);
            explanationCard.gameObject.SetActive(false);
            trackTime = true;
            return;
        }
        //intaraction only if task is correct
        if(taskIndex != 2)
        {
            return;
        }
        if (isQuestionTime)
        {
            return;
        }
        if (letterChecked)
        {
            return;
        }
        int currentPosition = sequenceCounter;
        int nBackPosition = sequenceCounter - N_FOR_N_BACK;
        if (nBackPosition < 1)
        {
            taskscore--;
            taskscoreNegative--;
        }
        else if (letter_sequence[currentPosition - 1].ToString() == letter_sequence[nBackPosition - 1].ToString())
        {
            taskscore++;
            taskscorePositive++;
        }
        else
        {
            taskscore--;
            taskscoreNegative--;
        }
        letterChecked = true;
        Debug.Log("current score" + taskscore);
    }

    private void setupBackButton()
    {
        Debug.Log("Setup button");
        buttonPress.action.started += onBackButtonPressed;
        buttonPress.action.Enable();
    }

    private void onBackButtonPressed(InputAction.CallbackContext context)
    {
        if(taskIndex != 2)
        {
            return;
        }
        if (isQuestionTime)
        {
            return;
        }
        if (letterChecked)
        {
            return;
        }
        if (backLetterChecked)
        {
            return;
        }
        int currentPosition = sequenceCounter;
        int nBackPosition = sequenceCounter - N_FOR_N_BACK;
        if (nBackPosition < 1)
        {
            backTaskScore--;
            backTaskScoreNegative--;
        }
        else if (letter_sequence[currentPosition - 1].ToString() == letter_sequence[nBackPosition - 1].ToString())
        {
            backTaskScore++;
            backTaskScorePositive++;
        }
        else
        {
            backTaskScore--;
            backTaskScoreNegative--;
        }
        backLetterChecked = true;
        Debug.Log("current back score" + taskscore);
    }

    private void checkTargets()
    {
        targetTimer += Time.deltaTime;

        if (!isTargetVisible)
        {
            int targetNumber = random.Next(0, numTargets);
            while (targetNumber == lastTarget)
            {
                targetNumber = random.Next(0, numTargets);
            }
            lastTarget = targetNumber;
            Transform selceted_targetholder = targetHolder.transform.GetChild(targetNumber);
            Destroy(selceted_targetholder.GetChild(0).gameObject);
            Instantiate(letters_dict[(char)letter_sequence[sequenceCounter]], selceted_targetholder);
            selceted_targetholder.gameObject.SetActive(true);
            letterChecked = false;
            backLetterChecked = false;
            sequenceCounter++;
            isTargetVisible = true;
        }
        else if (targetTimer > targetTimout)
        {
            resetTargets();
            targetTimer = 0f;
        }
    }

    private void checkSceneTime()
    {
        sceneTimer += Time.deltaTime;
        questionTimer += Time.deltaTime;
        int intQuestionTimer = (int)questionTimer;
        int modolQuestionTimer = intQuestionTimer % 5;

        if (modolQuestionTimer == 0)
        {
            checkControllerVisability();
        }

        // relevant logic for the questionnaires
        if (questionTimer >= questionnaireTimeInterval)
        {
            // Show the questionnaire
            setLetterSequence();
            //front buttons
            taskscoreSafe.Add(taskscore);
            taskscoreSafeNeg.Add(taskscoreNegative);
            taskscoreSafePos.Add(taskscorePositive);

            // back buttons
            backTaskScoreSafe.Add(backTaskScore);
            backTaskScoreNegativeSafe.Add(backTaskScoreNegative);
            backTaskScorePositiveSafe.Add(backTaskScorePositive);

            taskscore = 0;
            taskscoreNegative = 0;
            taskscorePositive = 0;

            backTaskScore = 0;
            backTaskScoreNegative = 0;
            backTaskScorePositive = 0;
            Debug.Log("Question Time!");
            resetTargets();
            ShowQuestionnaire();
            isQuestionTime = true;
            // Reset sceneTimer for the next interval
            questionTimer = 0f;
        }

        if (sceneTimer >= maxSceneTime && isQuestionaireDone)
        {
            writeCSVPerformance();
            buttonPress.action.started -= onButtonPressed;
            buttonPress.action.Disable();
            backButtonPress.action.started -= onBackButtonPressed;
            backButtonPress.action.Disable();
            SceneManager.LoadScene(7);
        }
    }

    private void ShowQuestionnaire()
    {
        slider.value = 50;
        thermalSensationUI.SetActive(true);
        leftControllerRay.gameObject.SetActive(true);
        rightControllerRay.gameObject.SetActive(true);
    }

    // called when UI-Button is pressed 
    public void checkQuestionnaireDone()
    {
        // set active false
        if (questionnaireCount <= 3)
        {
            thermalSensationScores[questionnaireCount] = slider.value;
            questionTimestamps[questionnaireCount] = DateTime.Now;
            questionnaireCount += 1;
        }

        // third questionnaire taken
        if (questionnaireCount > 3)
        {
            writeCSV();
            isQuestionaireDone = true;
        }
        thermalSensationUI.SetActive(false);
        leftControllerRay.gameObject.SetActive(false);
        rightControllerRay.gameObject.SetActive(false);
        Invoke(nameof(setQuestionaireVariableDelayed), 0.5f);
        targetTimer = 0;
    }

    private void setQuestionaireVariableDelayed()
    {
        isQuestionTime = false;
    }

    private void writeCSV()
    {
        string header = "thermal_sensation_90; thermal_sensation_180; thermal_sensation_270; thermal_sensation_360; time_90; time_180; time_270; time_360";
        tw = new StreamWriter(filePath, true);
        tw.WriteLine(header);

        string answers = string.Join(";", thermalSensationScores);
        string timestamps = string.Join(";", questionTimestamps);
        tw.WriteLine(answers + ";" + timestamps);
        tw.Close();
        Debug.Log("csv written");
    }

    private void writeCSVPerformance()
    {
        string header = "block_1; block_2; block_3; block_4; block_1_pos; block_2_pos; block_3_pos; block_4_pos; block_1_neg; block_2_neg; block_3_neg; block_4_neg";
        tw = new StreamWriter(filePathPerformance, true);
        tw.WriteLine(header);
        string[] taskscoreSafeStrings = taskscoreSafe.Cast<object>().Select(o => o.ToString()).ToArray();
        string[] taskscoreSafeStringsPos = taskscoreSafePos.Cast<object>().Select(o => o.ToString()).ToArray();
        string[] taskscoreSafeStringsNeg = taskscoreSafeNeg.Cast<object>().Select(o => o.ToString()).ToArray();


        string answers = string.Join(";", taskscoreSafeStrings.Concat(taskscoreSafeStringsPos).Concat(taskscoreSafeStringsNeg));
        tw.WriteLine(answers);
        tw.Close();
        Debug.Log("csv performance written");

        //log performance for the back trigger
        string headerBack = "block_1; block_2; block_3; block_4; block_1_pos; block_2_pos; block_3_pos; block_4_pos; block_1_neg; block_2_neg; block_3_neg; block_4_neg";
        tw = new StreamWriter(filePathPerformanceBack, true);
        tw.WriteLine(headerBack);
        string[] taskscoreSafeStringsBack = backTaskScoreSafe.Cast<object>().Select(o => o.ToString()).ToArray();
        string[] taskscoreSafeStringsPosBack = backTaskScorePositiveSafe.Cast<object>().Select(o => o.ToString()).ToArray();
        string[] taskscoreSafeStringsNegBack = backTaskScoreNegativeSafe.Cast<object>().Select(o => o.ToString()).ToArray();


        string answersBack = string.Join(";", taskscoreSafeStringsBack.Concat(taskscoreSafeStringsPosBack).Concat(taskscoreSafeStringsNegBack));
        tw.WriteLine(answersBack);
        tw.Close();
        Debug.Log("csv performance back written");
    }

    private void resetTargets()
    {
        foreach (Transform target in targetHolder.gameObject.transform)
        {
            if (target.gameObject.activeInHierarchy)
            {
                target.gameObject.SetActive(false);
            }
        }

        isTargetVisible = false;
    }

    private void checkControllerVisability()
    {
        if(leftControllerRay.gameObject.activeSelf)
        {
            leftControllerRay.gameObject.SetActive(false);
        }
        if(rightControllerRay.gameObject.activeSelf)
        {
            rightControllerRay.gameObject.SetActive(false);
        }
    }

    private void setLetterSequence()
    {
        sequenceCounter = 0;
        letter_sequence = new ArrayList();
        if(taskIndex == 2)
        {
            letter_sequence = new ArrayList(Constants.BLOCKS[blockCount]);
            blockCount += 1;
        }
        else 
        {
            for (int i = 0; i <= NUMBER_OF_LETTERS; i++)
            {
                letter_sequence.Add(letters_chars[i % 26]);
            }
        }
    }
}