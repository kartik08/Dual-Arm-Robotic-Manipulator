using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
public class DistanceCalc : MonoBehaviour
{
    float dist;
    Button button,resetbutton;
    GameObject RobotR, RobotL;
    Canvas canvas;
    Slider[] slider;

    public HoverButton SyncButton;
    
    void Awake()
    {
        button = GetComponent<Button>();
        resetbutton = GetComponent<Button>();
        canvas = GetComponent<Canvas>();
    }
    // Start is called before the first frame update
    void Start()
    {

        button = GameObject.Find("Button").GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = "Synchronize";
        //button.onClick.AddListener(SynchMode);
        
        resetbutton = GameObject.Find("ResetButton").GetComponent<Button>();
        resetbutton.GetComponentInChildren<Text>().text = "Reset";
        resetbutton.onClick.AddListener(ResetFunc);
        RobotR = GameObject.Find("6dof_robotR");
        RobotL = GameObject.Find("6dof_robotL");
    }
    void ResetFunc()
    {
        
        StartCoroutine(waiter());
        


    }
    IEnumerator waiter()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        slider = canvas.GetComponentsInChildren<Slider>();
       
        RobotR.GetComponent<CalcIKsldr>().x_coord = 8;
        RobotR.GetComponent<CalcIKsldr>().y_coord = 0;
        RobotR.GetComponent<CalcIKsldr>().z_coord = 8;
        RobotL.GetComponent<CalcIKsldr1>().x_coord = 8;
        RobotL.GetComponent<CalcIKsldr1>().y_coord = 0;
        RobotL.GetComponent<CalcIKsldr1>().z_coord = 8;
        yield return new WaitForSeconds(1);
        foreach (Slider i in slider)
        {
            i.value = 0;
        }
        slider[0].value = slider[2].value = slider[6].value = slider[8].value = 8;


    }
    //private void OnButtonDown(Hand hand)
    //{
    //    StartCoroutine(SynchMode());
    //}
    private void SynchMode(Hand hand)
    {
        
        if(RobotR.GetComponent<CalcIKsldr>().enabled == true && RobotL.GetComponent<CalcIKsldr1>().enabled == true)
        {
            button.GetComponentInChildren<Text>().text = "Synchronized mode";
            RobotR.GetComponent<CalcIKsldr>().enabled = false;
            RobotL.GetComponent<CalcIKsldr1>().enabled = false;
            RobotR.GetComponent<SyncModeR>().enabled = true;
            RobotL.GetComponent<SyncModeL>().enabled = true;
        }
        else
        {
            button.GetComponentInChildren<Text>().text = "Desynchronized";
            RobotR.GetComponent<CalcIKsldr>().enabled = true;
            RobotL.GetComponent<CalcIKsldr1>().enabled = true;
            RobotR.GetComponent<SyncModeR>().enabled = false;
            RobotL.GetComponent<SyncModeL>().enabled = false;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("First " + GameObject.Find("EffectorR").transform.position);
        //Debug.Log("Second " + GameObject.Find("EffectorL").transform.position);
        dist = Vector3.Distance(GameObject.Find("EffectorR").transform.position, GameObject.Find("EffectorL").transform.position);
        //Debug.Log(dist);
        if(dist<2.2)
        {
            
            SyncButton.onButtonDown.AddListener(SynchMode);
            //button.interactable = true;
        }
        else
        {
            SyncButton.onButtonDown.RemoveAllListeners();
            //button.interactable = false;
        }
    }
}
