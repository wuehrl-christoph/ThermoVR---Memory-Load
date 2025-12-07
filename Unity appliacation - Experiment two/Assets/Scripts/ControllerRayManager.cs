// This code is directly taken form the work Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftControllerRay;
    public GameObject rightControllerRay;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRayActive() {
        leftControllerRay.gameObject.SetActive(true);
        rightControllerRay.gameObject.SetActive(true);
    }

    public void setRayInactive() {
        leftControllerRay.gameObject.SetActive(false);
        rightControllerRay.gameObject.SetActive(false);
    }
}
