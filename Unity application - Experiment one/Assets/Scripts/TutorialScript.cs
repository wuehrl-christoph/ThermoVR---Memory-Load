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
    private float targetTimout = 2f;
    private float targetTimer = 0f;
    public GameObject targetHolder;
    private int numTargets;
    private bool isTargetVisible = false;


    // Scene Manager
    private float sceneTimer = 0f;
    private float maxSceneTime = 120f; // 2 minutes
    public GameObject leftControllerRay;
    public GameObject rightControllerRay;

    //Loading letter models
    private Dictionary<char, GameObject> letters_dict = new Dictionary<char, GameObject>();
    private string black_letters_path = "black_letters_outlined/";
    private char[] letters_chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
    'U', 'V', 'W', 'X', 'Y', 'Z'};
    private string outline_sufix = "_Outline";
    private System.Random random = new System.Random();

    // n-back related variables
    private ArrayList letter_sequence = new ArrayList();
    private int sequenceCounter = 0;

    // targed safe
    private int lastTarget = -1;

    // task
    private int N_FOR_N_BACK = 2;
    private ArrayList taskscoreSafe = new ArrayList();
    private bool letterChecked = false;

    // Button related variables
    public InputActionReference buttonPress;
    private bool tutorialStarted = false;
    public GameObject lastCard;

    //Feedback
    public TextMeshProUGUI feedbackText;
    public GameObject ui;
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
        if(tutorialStarted)
        {
            checkSceneTime();
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
        if (!tutorialStarted)
        {
            if(!lastCard.activeInHierarchy)
            {
                Debug.Log("not on the last screen of the tutorial");
                return;
            }
            tutorialStarted = true;
            ui.gameObject.SetActive(false);
            leftControllerRay.gameObject.SetActive(false);
            rightControllerRay.gameObject.SetActive(false);
            return;
        }
        if (letterChecked)
        {
            showFeedback("Already counted");
            return;
        }
        int currentPosition = sequenceCounter;
        int nBackPosition = sequenceCounter - N_FOR_N_BACK;
        if (nBackPosition < 1)
        {
            showFeedback("Not enough letters");
        }
        else if (letter_sequence[currentPosition - 1].ToString() == letter_sequence[nBackPosition - 1].ToString())
        {
            showFeedback("Correct!");
        }
        else
        {
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
        int lastLetterPos = currentPos - N_FOR_N_BACK;
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

        if (sceneTimer >= maxSceneTime)
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
        letter_sequence = new ArrayList(Constants.TEST_SEQUENCE);
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