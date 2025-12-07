// This code is heavily based on the code used by Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg). As the new implemented and allready existing code are highly intertwined it can 
//hardly be highlighted which source code is from the work by Wuehrl et al. and which is newly implemented.
//However, the changes are outlined in the master theisis in the implemetiation section for study one.
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class HMDTracker : MonoBehaviour
{
    public GameObject HMD;
    private TextWriter tw;
    private int taskCount;
    private string fileName = Application.dataPath + "/CSV-Data/hmd.csv";
    private WaitForSeconds freq = new WaitForSeconds(0.05f); // 30 fps

    // Start is called before the first frame update
    void Start()
    {
        int currentScene = PlayerPrefs.GetInt("scene counter"); // 1 - Feuer, 2 - Eis
        int userId = PlayerPrefs.GetInt("pid");
        int envIndex = PlayerPrefs.GetInt("s"+currentScene);
        int taskIndex = PlayerPrefs.GetInt("task counter");
        taskCount = PlayerPrefs.GetInt("t"+taskIndex);

        fileName = Application.dataPath + "/CSV-Data/" + userId + "_count" + currentScene + "_env" + envIndex + "_task" + taskCount + "_hmd.csv" ;

        tw = new StreamWriter(fileName, true);
        string header = "timestamp;x;y;z;rx;ry;rz";
        tw.WriteLine(header);
        tw.Close();

        StartCoroutine(collectCamData());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator collectCamData()
    {
        while (true)
        {
            float xPos = HMD.transform.position.x;
            float yPos = HMD.transform.position.y;
            float zPos = HMD.transform.position.z;

            float xRot = HMD.transform.rotation.eulerAngles.x;
            float yRot = HMD.transform.rotation.eulerAngles.y;
            float zRot = HMD.transform.rotation.eulerAngles.z;

            string dataPoint = DateTime.Now + ";" + xPos + ";" + yPos + ";" + zPos + ";" + xRot + ";" + yRot + ";" + zRot;
            tw = new StreamWriter(fileName, true);
            tw.WriteLine(dataPoint);
            tw.Close();
            yield return freq;
        }
    }
}
