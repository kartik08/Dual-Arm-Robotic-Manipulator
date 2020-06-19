using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class SyncModeL : MonoBehaviour
{
    public Transform[] Joints;
    InputField[] field;
    public Slider S_Slider;    //PositionX min=4 max=12
    public Slider L_Slider;    //PositionY min=-4 max=4
    public Slider U_Slider;    //PositionZ min=4 max=12
    public Slider R_Slider;    //RotationX min=-90 max=90
    public Slider B_Slider;    //RotationY min=-90 max=90
    public Slider T_Slider;    //RotationZ min=-90 max=90
    public static double[] theta = new double[6];    //angle of the joints
    public CalcIKsldr1 c;
    private float L1, L2, L3, L4, L5, L6;    //arm length in order from base
    private float C3;
    private Canvas canvas;
    InverseCalc I = new InverseCalc();
    public float px = 8f, py = 0f, pz = 8f;
    public float rx = 0f, ry = 0f, rz = 0f;
    public float px1 = 8f, py1 = 0f, pz1 = 8f;
    public float rx1 = 0f, ry1 = 0f, rz1 = 0f;
    float xval, yval, zval;
    float step = 0f;
    public LinearMapping lmX, lmY, lmZ;
    float valueX, valueY, valueZ;
    // Start is called before the first frame update
    void Start()
    {
        c = gameObject.GetComponent<CalcIKsldr1>();
        Debug.Log("Synch Mode ENTERED");
        theta[0] = theta[1] = theta[2] = theta[3] = theta[4] = theta[5] = 0.0;
        L1 = 4.0f;
        L2 = 6.0f;
        L3 = 3.0f;
        L4 = 4.0f;
        L5 = 2.0f;
        L6 = 1.0f;
        C3 = 0.0f;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        field = canvas.GetComponentsInChildren<InputField>();
        //px = 8f; py = 0f; pz = 8f;
        //rx = 0f; ry = 0f; rz = 0f;
        px1 = c.S_Slider1.value;
        py1 = -c.L_Slider1.value;
        pz1 = c.U_Slider1.value;
        //px1 = c.x_coord;
        //py1 = -c.y_coord;
        //pz1 = c.z_coord;
        rx1 = c.R_Slider1.value;
        ry1 = c.B_Slider1.value;
        rz1 = -c.T_Slider1.value;
        //xfield = GameObject.Find("Xsync").GetComponent<InputField>();
        //yfield = GameObject.Find("Ysync").GetComponent<InputField>();
        //zfield = GameObject.Find("Zsync").GetComponent<InputField>();
    }
    void OnEnable()
    {
        px1 = c.S_Slider1.value;
        py1 = -c.L_Slider1.value;
        pz1 = c.U_Slider1.value;
        //px1 = c.x_coord;
        //py1 = -c.y_coord;
        //pz1 = c.z_coord;
        rx1 = c.R_Slider1.value;
        ry1 = c.B_Slider1.value;
        rz1 = -c.T_Slider1.value;
    }

    // Update is called once per frame
    void Update()
    {
        valueX = Mapping(lmX.value, 1, 6, -3);
        valueY = Mapping(lmY.value, 1, 6, -3);
        valueZ = Mapping(lmZ.value, 1, 6, -3);
        //xval = float.Parse(field[0].text.ToString());
        //yval = float.Parse(field[1].text.ToString());
        //zval = float.Parse(field[2].text.ToString());

        //if (xval >= -3 && xval <= 3 && yval >= -3 && yval <= 3 && zval >= -3 && zval <= 3)
        //{
        px = px1 +valueX;
            py = py1 + valueY;
            pz = pz1 + valueZ;
            rx = rx1 - R_Slider.value;
            ry = ry1 + B_Slider.value;
            rz = rz1 + T_Slider.value;

            theta = I.CalcInverse(px, py, pz, rx, ry, rz);

            if (!double.IsNaN(theta[0]))
                Joints[0].transform.localEulerAngles = new Vector3(0, 0, (float)theta[0] * Mathf.Rad2Deg);
            if (!double.IsNaN(theta[1]))
                Joints[1].transform.localEulerAngles = new Vector3((float)theta[1] * Mathf.Rad2Deg, 0, 0);
            if (!double.IsNaN(theta[2]))
                Joints[2].transform.localEulerAngles = new Vector3((float)theta[2] * Mathf.Rad2Deg, 0, 0);
            if (!double.IsNaN(theta[3]))
                Joints[3].transform.localEulerAngles = new Vector3(0, 0, (float)theta[3] * Mathf.Rad2Deg);
            if (!double.IsNaN(theta[4]))
                Joints[4].transform.localEulerAngles = new Vector3((float)theta[4] * Mathf.Rad2Deg, 0, 0);
            if (!double.IsNaN(theta[5]))
                Joints[5].transform.localEulerAngles = new Vector3(0, 0, (float)theta[5] * Mathf.Rad2Deg);
        //}
    }
    float Mapping(float OldValue, float OldRange, float NewRange, float NewMin)
    {

        return ((((OldValue) * NewRange) / OldRange) + NewMin);
    }
}
