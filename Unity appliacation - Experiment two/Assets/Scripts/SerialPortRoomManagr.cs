// This code is directly taken form the work Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System;
using System.Drawing.Printing;
using System.IO;
using UnityEngine.SceneManagement;
using System.Net;


public class SerialPortRoomManagr : MonoBehaviour
{
    // source: https://www.hackster.io/raisingawesome/unity-game-engine-and-arduino-serial-communication-12fdd5
    SerialPort sp;
    float next_time;
    // Start is called before the first frame update
    string safe = "";

    private string fileName = Application.dataPath + "/CSV-Data/temp.csv";
    private TextWriter tw;
    private int scenecounter;
    private int envIndex;
    private float readFreq = 2f;

    void Start()
    {
        scenecounter = PlayerPrefs.GetInt("scene counter");
        int userId = PlayerPrefs.GetInt("pid");
        envIndex = PlayerPrefs.GetInt("s" + scenecounter);
        fileName = Application.dataPath + "/CSV-Data/" + userId + "_count" + scenecounter + "_env" + envIndex + "_roomtemperature.csv";

        tw = new StreamWriter(fileName, true);
        string header = "timestamp;humidity;temp;";
        tw.WriteLine(header);
        tw.Close();



        string the_com = "";
        next_time = Time.time;
        foreach (string mysps in SerialPort.GetPortNames())
        {
            print(mysps);
            if (mysps == "COM3")
            {
                the_com = mysps;
                break;
            }
        }
        if (the_com != "")
        {
            print("Setup port");
            sp = new SerialPort("\\\\.\\" + the_com, 9600);
            if (!sp.IsOpen)
            {
                print("Opening" + the_com + ", baud 9600");
                sp.Open();
                sp.ReadTimeout = 100;
                sp.Handshake = Handshake.None;
                if (sp.IsOpen)
                {
                    print("Open");
                }
            }
            else
            {
                print("is allready opened");
            }
        }
        else
        {
            print("the com is empty");
        }

        StartCoroutine(enumData());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time > next_time) 
    }

    private IEnumerator enumData()
    {
        while (true)
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                print("opened sp");
            }
            if (sp.IsOpen)
            {
                try
                {
                    string msg = sp.ReadLine();
                    safe = msg;
                    Debug.Log(msg);
                    printData(msg);

                }
                catch (TimeoutException)
                {
                    printData(safe);
                }
            }


            yield return readFreq;
        }
    }

    private void printData(string msg)
    {
        string[] data = msg.Split(", ");

        if (data.Length == 2)
        {
            string dataPoint = DateTime.Now + ";" + data[0] + ";" + data[1];
            tw = new StreamWriter(fileName, true);
            tw.WriteLine(dataPoint);
            tw.Close();
        }
    }
}
