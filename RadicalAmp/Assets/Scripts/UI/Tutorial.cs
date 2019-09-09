﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.VFX;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject clone;
    [SerializeField] Animator anim;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI tmproText;
    [SerializeField] GameObject tutorialContainer;
    [SerializeField] GameObject gate;
    [SerializeField] GameObject TutorialText;
    [SerializeField] Slider juiceMeter;

    [SerializeField] float setTimer;

    private TutorialClone cloneAnim;
    private Material gateMaterial;

    public TutorialSteps currentStep;

    public List<GameObject> EmpowerClone;
    public List<GameObject> JuiceDashClone;

    bool tutorialPlay = false;

    float tutorialTimer = 5;
    int hitCounter = 0;

    bool animStageChange = false;

    public void StartTutorial()
    {
        currentStep = TutorialSteps.MovementInfo;
    }
    private void Awake()
    {
        gateMaterial = gate.GetComponent<Renderer>().material;
        if(gateMaterial == null)
        {
            Debug.Log("ARSCHLOCH ARSCHLOCH");
        }

        if (clone != null)
        {
            cloneAnim = clone.GetComponent<TutorialClone>();
        }
        tutorialContainer.SetActive(false);
    }

    private void Update()
    {
        if (currentStep == TutorialSteps.PreTutorial)
        {
            if (Input.GetButtonDown("Dash"))
            {
                StartTutorial();
                TutorialText.SetActive(false);
                clone.SetActive(true);
            }
            if (Input.GetButtonDown("Attack"))
            {
                currentStep = TutorialSteps.TutorialPreFinish;
                TutorialText.SetActive(false);
            }
        }

        if(currentStep == TutorialSteps.MovementInfo && !tutorialPlay)
        {
            cloneAnim.PlayRunning(true);
            tutorialContainer.SetActive(true);
            tmproText.text = "Use joystick for movement";
            anim.Play("AnimMovement");
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if(currentStep == TutorialSteps.MovementTest)
        {
            if((Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || 
                Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0) && 
                !tutorialPlay)
            {
                tutorialTimer = 2;
                tutorialPlay = true;
            }
        }
        if (currentStep == TutorialSteps.DashInfo && !tutorialPlay)
        {
            cloneAnim.PlayRunning(false);
            cloneAnim.PlayDash(true);
            tmproText.text = "Press A to dash, dash three times.";
            tutorialContainer.SetActive(true);
            anim.Play("AnimDash");
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if (currentStep == TutorialSteps.DashTest)
        {
            if (Input.GetButtonDown("Dash") && !tutorialPlay && hitCounter <= 2)
            {
                hitCounter++;
            }
            else if(hitCounter >=3)
            {
                currentStep += 1;
                hitCounter = 0;
            }
        }
        if (currentStep == TutorialSteps.AttackInfo && !tutorialPlay)
        {
            cloneAnim.PlayDash(false);
            cloneAnim.PlayAttack(true);
            tmproText.text = "Press RB to slash, slash three times.";
            tutorialContainer.SetActive(true);
            anim.Play("AnimAttack");
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if (currentStep == TutorialSteps.AttackTest)
        {
            if (Input.GetButtonDown("Attack") && !tutorialPlay && hitCounter <= 2)
            {
                hitCounter++;
            }
            else if(hitCounter >= 3)
            {
                currentStep += 1;
                hitCounter = 0;
            }
        }
        if (currentStep == TutorialSteps.DanceInfo && !tutorialPlay)
        {
            cloneAnim.PlayAttack(false);
            cloneAnim.PlayDance(true);
            tmproText.text = "Press B to dance, dance three times.";
            tutorialContainer.SetActive(true);
            anim.Play("AnimDance");
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if (currentStep == TutorialSteps.DanceTest)
        {
            if (Input.GetButtonDown("Dance") && !tutorialPlay && hitCounter <= 2)
            {
                hitCounter++;
            }
            else if(hitCounter >= 3)
            {
                currentStep += 1;
                hitCounter = 0;
                juiceMeter.value = 0;
            }
        }
        if (currentStep == TutorialSteps.JuiceInfo && !tutorialPlay)
        {
            tutorialContainer.SetActive(true);
            tmproText.text = "Hit the beat three times, use your dash, shlash or dance.";
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if (currentStep == TutorialSteps.JuiceTest)
        {
            if(juiceMeter.value >= 15)
            {
                currentStep += 1;
                cloneAnim.PlayDance(false);
            }
        }
        if (currentStep == TutorialSteps.EmpowerSlashInfo && !tutorialPlay)
        {
            tmproText.text = "Press A to dash, dash three times.";
            tutorialContainer.SetActive(true);
            anim.Play("AnimDash");
            foreach(GameObject clone in EmpowerClone)
            {
                clone.SetActive(true);
            }
            tutorialTimer = setTimer;
            tutorialPlay = true;

        }
        if (currentStep == TutorialSteps.EmpowerSlashTest)
        {
            if (EmpowerClone.Count == 0)
            {
                currentStep += 1;
            }
        }
        if (currentStep == TutorialSteps.JuiceDashInfo && !tutorialPlay)
        {
            tmproText.text = "Press A to dash, dash three times.";
            tutorialContainer.SetActive(true);
            anim.Play("AnimDash");
            foreach (GameObject clone in JuiceDashClone)
            {
                clone.SetActive(true);
            }
            juiceMeter.minValue = 100;
            tutorialTimer = setTimer;
            tutorialPlay = true;
        }
        if (currentStep == TutorialSteps.JuiceDashTest)
        {

            if (JuiceDashClone.Count == 0)
            {
                currentStep += 1;
                juiceMeter.minValue = 0;
                juiceMeter.value = 0;

            }
        }
        if(currentStep == TutorialSteps.TutorialPreFinish)
        {
            gateMaterial.SetFloat("Vector1_36A0E93A", Mathf.Lerp(gateMaterial.GetFloat("Vector1_36A0E93A"), 1f, 0.02f));

            if (gateMaterial.GetFloat("Vector1_36A0E93A") >= 0.73f)
            {
                clone.SetActive(false);
                gate.SetActive(false);
                currentStep += 1;
            }
        }
        if (currentStep == TutorialSteps.TutorialFinish)
        {

        }

        if (tutorialPlay)
        {
            //Debug.Log(tutorialTimer);
            tutorialTimer -= Time.deltaTime;
        }
        if (tutorialTimer <= 0)
        {
            tutorialPlay = false;
            currentStep += 1;
            tutorialTimer = 1;
            tutorialContainer.SetActive(false);
        }
    }

    public void Testing()
    {
        currentStep = TutorialSteps.TutorialPreFinish;
    }

    public enum TutorialSteps
    {
        PreTutorial,
        MovementInfo,
        MovementTest,
        DashInfo,
        DashTest,
        AttackInfo,
        AttackTest,
        DanceInfo,
        DanceTest,
        JuiceInfo,
        JuiceTest,
        EmpowerSlashInfo,
        EmpowerSlashTest,
        JuiceDashInfo,
        JuiceDashTest,
        TutorialPreFinish,
        TutorialFinish
    }
}