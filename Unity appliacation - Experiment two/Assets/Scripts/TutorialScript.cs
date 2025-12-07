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

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TutorialScript : MonoBehaviour
{
    // Target related variables
    [Header("TargetManager")]
    private float targetTimout = 2f; // TODO
    private float targetTimer = 0f;
    public GameObject targetHolder;
    private int numTargets;
    private bool isTargetVisible = false;


    // Scene Manager
    private float sceneTimer = 0f;
    private float trainingTime = 120f;
    public GameObject leftControllerRay;
    public GameObject rightControllerRay;

    //Letter related variables
    private Dictionary<char, GameObject> letters_dict = new Dictionary<char, GameObject>();
    private string black_letters_path = "black_letters_outlined/";
    private char[] letters_chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
    'U', 'V', 'W', 'X', 'Y', 'Z'};
    private string outline_sufix = "_Outline";
    private System.Random random = new System.Random();

    // n-back 
    private ArrayList letter_sequence = new ArrayList();
    private int sequenceCounter = 0;

    // target safe
    private int lastTarget = -1;

    // task
    private static int FIRST_N = 1;
    private static int SECOND_N = 3;
    private int nForNBack = FIRST_N;
    private ArrayList taskscoreSafe = new ArrayList();
    private bool letterChecked = false;

    // Button related variables
    public InputActionReference buttonPress;
    private bool tutorialStarted = false;
    public GameObject firstUiLastCard;
    public GameObject secondUiLastCard;

    //Feedback 
    public TextMeshProUGUI feedbackText;
    public GameObject ui;
    public GameObject secondUI;
    private float displayTime = 1f;
    private float floatSpeed = 0.5f;
    private Vector3 initalPosition;

    // Start is called before the first frame update
    void Start()
    {
        feedbackText.gameObject.SetActive(false);
        initalPosition = feedbackText.transform.localPosition;
        numTargets = targetHolder.transform.childCount;
        string letter_path = "";
        letter_path = black_letters_path;
        foreach (char char_letter in letters_chars)
        {
            letters_dict.Add(char_letter, Resources.Load<GameObject>(letter_path + char_letter.ToString() + outline_sufix));
        }
        setLetterSequence();        
        setupButton();
    }

    // Update is called once per frame
    void Update()
    {
        // check if its questionnaire time or scene change
        if(tutorialStarted)
        {
            checkTargets();
            checkSceneTime();
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
        if (!tutorialStarted)
        {
            if (nForNBack == FIRST_N && !firstUiLastCard.activeInHierarchy)
            {
                Debug.Log("not on the last screen of the tutorial");
                return;
            }
            else if (nForNBack == SECOND_N && !secondUiLastCard.activeInHierarchy)
            {
                Debug.Log("not on the last screen of the tutorial");
                return;
            }
            tutorialStarted = true;
            ui.gameObject.SetActive(false);
            secondUI.SetActive(false);
            leftControllerRay.gameObject.SetActive(false);
            rightControllerRay.gameObject.SetActive(false);
            return;
        }
        if (letterChecked)
        {
            Debug.Log("You allready pressed the button idiot");
            showFeedback("Already counted");
            return;
        }
        int currentPosition = sequenceCounter;
        int nBackPosition = sequenceCounter - nForNBack;
        if (nBackPosition < 1)
        {
            Debug.Log("To early bro");
            showFeedback("Not enough letters");
        }
        else if (letter_sequence[currentPosition - 1].ToString() == letter_sequence[nBackPosition - 1].ToString())
        {
            Debug.Log("You are great");
            showFeedback("Correct!");
        }
        else
        {
            Debug.Log("just wrong are you even trying");
            showFeedback("Incorrect!");
        }
        letterChecked = true;
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
            checkIfNotHit();
            letterChecked = false;
            sequenceCounter++;
            isTargetVisible = true;
        }
        else if (targetTimer > targetTimout)
        {
            resetTargets();
            targetTimer = 0f;
        }
    }

    private void checkIfNotHit()
    {
        int currentPos = sequenceCounter - 1;
        int lastLetterPos = currentPos - nForNBack;
        if (lastLetterPos < 0)
        {
            return;
        }
        if (letterChecked)
        {
            return;
        }
        if((char)letter_sequence[currentPos] == (char)letter_sequence[lastLetterPos])
        {
            Debug.Log("Push me and then just touch me");
            showFeedback("Missed");
        }
    }

    private void checkSceneTime()
    {
        sceneTimer += Time.deltaTime;

        if (sceneTimer >= trainingTime && nForNBack == FIRST_N)
        {
            sceneTimer = 0f;
            nForNBack = SECOND_N;
            letterChecked = false;
            lastTarget = -1;
            sequenceCounter = 0;
            targetTimer = 0;
            setLetterSequence();

            // Stop Tutorial mode
            tutorialStarted = false;

            // Show UI
            resetTargets();
            secondUI.SetActive(true);
            leftControllerRay.gameObject.SetActive(true);
            rightControllerRay.gameObject.SetActive(true);
        }
        else if (sceneTimer >= trainingTime && nForNBack == SECOND_N)
        {
            // scene change
            buttonPress.action.started -= onButtonPressed;
            buttonPress.action.Disable();
            string sceneName = "s" + PlayerPrefs.GetInt("scene counter");
            int nextSceneIndex = PlayerPrefs.GetInt(sceneName);
            SceneManager.LoadScene(nextSceneIndex);
        }
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

    private void setLetterSequence()
    {
        if (nForNBack == FIRST_N)
        {
            letter_sequence = new ArrayList(Constants.ONE_BACK_TEST_SEQUENCE);
        }
        else if (nForNBack == SECOND_N)
        {
            letter_sequence = new ArrayList(Constants.THREE_BACK_TEST_SEQUENCE);
        }
    }

    private void showFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        feedbackText.transform.localPosition = initalPosition;
        StartCoroutine(fadeOut());
    }

    private IEnumerator fadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = feedbackText.color;
        Vector3 startPosition = feedbackText.transform.position;

        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            feedbackText.transform.position = startPosition + Vector3.up * (floatSpeed * elapsedTime);
            feedbackText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - (elapsedTime / displayTime));
            yield return null;
        }

        feedbackText.gameObject.SetActive(false);
        feedbackText.color = originalColor; 
    }
}