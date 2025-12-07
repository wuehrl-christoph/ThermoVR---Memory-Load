// This code is heavily based on the code used by Wuehrl, C., Hoessl, S., & Ho, T. J. (2023). The effect of hue and 
//envrionmental cues on thermal perception in virtual reality. (currently unpublished paper from a university 
//seminar, University of Regensburg). As the new implemented and allready existing code are highly intertwined it can 
//hardly be highlighted which source code is from the work by Wuehrl et al. and which is newly implemented.
//However, the changes are outlined in the master theisis in the implemetiation section for study one.
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class NStepManager : MonoBehaviour
    {
        [Serializable]
        class Step
        {
            [SerializeField]
            public GameObject stepObject;

            [SerializeField]
            public string buttonText;
        }

        [SerializeField]
        public GameObject tutorialUI;

        [SerializeField]
        public TextMeshProUGUI m_StepButtonTextField;

        [SerializeField]
        List<Step> m_StepList = new List<Step>();
        public GameObject tutorialButton;

        int m_CurrentStepIndex = 0;

        public void Next()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
            m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
            if (m_CurrentStepIndex >= 3)
            {
                tutorialButton.gameObject.SetActive(false);
            }
        }
    }
}
