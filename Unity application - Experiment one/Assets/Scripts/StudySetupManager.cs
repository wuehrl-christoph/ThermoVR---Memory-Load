// This code is heavily based on the code used by Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg). As the new implemented and allready existing code are highly intertwined it can 
//hardly be highlighted which source code is from the work by Wuehrl et al. and which is newly implemented.
//However, the changes are outlined in the master theisis in the implemetiation section for study one.

using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using System.IO;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StudySetupManager : MonoBehaviour
{
    // Latin Square csv load
    public TextAsset latinSquareCsv;
    public int pid;
    private TextWriter tw;

    // Button interaction
    public InputActionReference buttonPress;
    public GameObject lastCard;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        // set PlayerID
        PlayerPrefs.SetInt("pid", pid);  
        // save sceneCounter PlayerPrefs.SetInt(sceneCounter, 0)
        PlayerPrefs.SetInt("scene counter", 1);
        PlayerPrefs.SetInt("task counter", 1);
        // load csv/scene reihenfolge corresponding to pid
        loadCSV();
        // print ID, counter + scenereihenfolge
        setupButton();
    }

    private void setupButton()
    {
        buttonPress.action.started += startExperiment;
        buttonPress.action.Enable();
    }

    private void startExperiment(InputAction.CallbackContext context)
    {
        if(!lastCard.activeInHierarchy)
        {
            Debug.Log("not on the last screen of the tutorial");
            return;
        }
        buttonPress.action.started -= startExperiment;
        buttonPress.action.Disable();
        SceneManager.LoadScene(8);
    }

    private void loadCSV() {
        string[] lines = latinSquareCsv.text.Split('\n');

    	foreach (string line in lines) {

            string[] values = line.Split(";");
            if (values[0] == "ID") {
                continue;
            }
            int readID = int.Parse(values[0]);

            if (readID == pid) {
                Debug.Log("csv line: " + line);

                PlayerPrefs.SetInt("s1", get_new_scene_counter(values[1]));
                PlayerPrefs.SetInt("s2", get_new_scene_counter(values[2]));
                PlayerPrefs.SetInt("s3", get_new_scene_counter(values[3]));
                PlayerPrefs.SetInt("s4", get_new_scene_counter(values[4]));
                PlayerPrefs.SetInt("s5", get_new_scene_counter(values[5]));
                PlayerPrefs.SetInt("s6", get_new_scene_counter(values[6]));

                Debug.Log("scene: " + PlayerPrefs.GetInt("s1"));
                Debug.Log("scene: " + PlayerPrefs.GetInt("s2"));
                Debug.Log("scene: " + PlayerPrefs.GetInt("s3"));
                Debug.Log("scene: " + PlayerPrefs.GetInt("s4"));
                Debug.Log("scene: " + PlayerPrefs.GetInt("s5"));
                Debug.Log("scene: " + PlayerPrefs.GetInt("s6"));

                PlayerPrefs.SetInt("t1", get_task_counter(values[1]));
                PlayerPrefs.SetInt("t2", get_task_counter(values[2]));
                PlayerPrefs.SetInt("t3", get_task_counter(values[3]));
                PlayerPrefs.SetInt("t4", get_task_counter(values[4]));
                PlayerPrefs.SetInt("t5", get_task_counter(values[5]));
                PlayerPrefs.SetInt("t6", get_task_counter(values[6]));

                Debug.Log("task: " + PlayerPrefs.GetInt("t1"));
                Debug.Log("task: " + PlayerPrefs.GetInt("t2"));
                Debug.Log("task: " + PlayerPrefs.GetInt("t3"));
                Debug.Log("task: " + PlayerPrefs.GetInt("t4"));
                Debug.Log("task: " + PlayerPrefs.GetInt("t5"));
                Debug.Log("task: " + PlayerPrefs.GetInt("t6"));

                break;
            }
        }

    }

    private int get_new_scene_counter(string number)
    {
        if(int.Parse(number) <= 3)
        {
            return 1;
        }
        return 6;
    }

    private int get_task_counter(string number)
    {
        if(int.Parse(number) == 1 || int.Parse(number) == 4)
        {
            return -1;
        }
        else if(int.Parse(number) == 2 || int.Parse(number) == 5)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
