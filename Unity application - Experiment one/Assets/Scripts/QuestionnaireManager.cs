// This code is heavily based on the code used by Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg). As the new implemented and allready existing code are highly intertwined it can 
//hardly be highlighted which source code is from the work by Wuehrl et al. and which is newly implemented.
//However, the changes are outlined in the master theisis in the implemetiation section for study one.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class QuestionnaireManager : MonoBehaviour
{
    // timer stuff
    private float timer = 0f;
    private float minWaitTime = 180f; 
    private bool isWaiting = true;

    // Comfort
    [Header("Thermal Comfort Data")]
    public GameObject comfortUI;
    public ToggleGroup tgComfort;
    private string comfortAnswer;
    private bool isComfortAnswered = false;
    private DateTime comfortTimeStampe;

    // Metal Effort
    [Header("Mental Effort")]
    public GameObject mentalEffortUI;
    public Slider mentalEffortSlider;
    private float mentalEffortValue = 0;
    private bool isMentalEffortAnswered = false;

    // IPQ
    [Header("IPQ Data")]
    public GameObject ipqUi;
    public ToggleGroup tgIPQ;
    public TextMeshProUGUI textAnchorPositive;
    public TextMeshProUGUI textAnchorNegative;
    public TextMeshProUGUI questionHeader;
    private IPQ_Question[] questions;
    private string[] ipqItemNames;
    public TextAsset csvFile;
    private string[] ipqAnswers;
    private int currentQuestion = 0; 
    public bool isIPQAnswered = false; 

    // csv things
    private TextWriter tw;
    private int scenecounter;
    private int taskCounter;
    private int envIndex;
    private int taskIndex;
    private string filePath = Application.dataPath + "/CSV-Data/qr.csv";

    //wait things
    [Header("Waiting Data")]
    public Image timeFeedback;
    public GameObject timeFeedbackUI;
    public GameObject finishUI;




    // Start is called before the first frame update
    void Start()
    {
        loadIPQ();
        setIPQQuestion(); //it's the first one
        scenecounter = PlayerPrefs.GetInt("scene counter");
        taskCounter = PlayerPrefs.GetInt("task counter");
        int userId = PlayerPrefs.GetInt("pid");
        envIndex = PlayerPrefs.GetInt("s"+scenecounter);
        taskIndex = PlayerPrefs.GetInt("t"+taskCounter);
        filePath = Application.dataPath + "/CSV-Data/" + userId + "_count" + scenecounter + "_env" + envIndex + "_task" + taskIndex + "_ipq_comfort.csv" ;

    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting)
        {
            if (timer <= minWaitTime)
            {
                timer += Time.deltaTime;
                if (isIPQAnswered) {
                    timeFeedback.fillAmount = timer / minWaitTime;
                }
            }
            else
            {
                isWaiting = false;
            }
        }
        else 
        {
            if (isIPQAnswered && isMentalEffortAnswered && isComfortAnswered)
            {
                if (scenecounter < 7) {
                    string scenePref = "s" + scenecounter;
                    int sceneIndex = PlayerPrefs.GetInt(scenePref);
                    SceneManager.LoadScene(sceneIndex);
                }
                
            }
        }
    }

    public void checkComfortQ()
    {
        Toggle toggle = tgComfort.ActiveToggles().First();
        if (toggle != null)
        {
            comfortAnswer = toggle.name;
            comfortTimeStampe = DateTime.Now;
            isComfortAnswered = true;
            comfortUI.SetActive(false);
            mentalEffortUI.SetActive(true);
        }

    }

    public void checkMentalEffort()
    {
        mentalEffortValue = mentalEffortSlider.value;

        isMentalEffortAnswered = true;
        mentalEffortUI.SetActive(false);
        ipqUi.SetActive(true);
    }

    public void checkIPQ()
    {
        Toggle toggle = tgIPQ.ActiveToggles().First();

        if (toggle != null)
        {

            // save answers
            ipqAnswers[currentQuestion] = toggle.name;



            if (currentQuestion < ipqAnswers.Length - 1) // < 13
            {
                currentQuestion += 1;
                resetIPQToggles();
                setIPQQuestion();
            }
            else
            {
                Debug.Log("IPQ_done");
                // writecsv
                writeQuestionnaireCSV();
                isIPQAnswered = true;
                ipqUi.SetActive(false);
                if (scenecounter == 7) {
                    finishUI.gameObject.SetActive(true);
                } else {
                    timeFeedbackUI.SetActive(true);
                }       
            }
        }
    }

    private void loadIPQ()
    {
        string[] lines = csvFile.text.Split('\n');

        questions = new IPQ_Question[lines.Length];
        ipqItemNames = new string[lines.Length];
        ipqAnswers = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length == 4)
            {
                ipqItemNames[i] = values[0];
                IPQ_Question ipq_question = new IPQ_Question(values[1], values[2], values[3]);
                questions[i] = ipq_question;
            }
        }
    }

    private void setIPQQuestion()
    {
        questionHeader.text = questions[currentQuestion].get_question();
        textAnchorPositive.text = questions[currentQuestion].get_positive_anchor();
        textAnchorNegative.text = questions[currentQuestion].get_negative_anchor();
    }

    private void resetIPQToggles()
    {
        Toggle[] toggles = tgIPQ.GetComponentsInChildren<Toggle>();

        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
        }
    }

    private void writeQuestionnaireCSV()
    {
        string header = "scene; task; time;" + "comfort;" + "comfort time;" + "Mental_effort;" + string.Join(";", ipqItemNames);
        tw = new StreamWriter(filePath, true);
        tw.WriteLine(header);

        string answers = envIndex + ";" + taskIndex + ";" + DateTime.Now + ";" + comfortAnswer + ";" + comfortTimeStampe + ";" + mentalEffortValue + ";" +
                            string.Join(";", ipqAnswers);

        tw.WriteLine(answers);
        tw.Close();
        Debug.Log("csv should be written now");
        scenecounter += 1;
        PlayerPrefs.SetInt("scene counter", scenecounter);
        taskCounter += 1;
        PlayerPrefs.SetInt("task counter", taskCounter);
    }


}


public class IPQ_Question
{
    private string question;
    private string anchor_negative;
    private string anchor_positive;

    public IPQ_Question(string question, string anchor_negative, string anchor_positive)
    {
        this.question = question;
        this.anchor_negative = anchor_negative;
        this.anchor_positive = anchor_positive;
    }

    public string get_question()
    {
        return question;
    }

    public string get_positive_anchor()
    {
        return anchor_positive;
    }

    public string get_negative_anchor()
    {
        return anchor_negative;
    }
}