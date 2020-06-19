using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class CalcIKsldr1 : MonoBehaviour
{
    public float x_coord;
    public float y_coord;
    public float z_coord;
    public Transform[] Joints1;
    double startx, endx, starty, endy, startz, endz;
    float stepx, stepy, stepz;
    public Slider S_Slider1;    //PositionX min=4 max=12
    public Slider L_Slider1;    //PositionY min=-4 max=4
    public Slider U_Slider1;    //PositionZ min=4 max=12
    public Slider R_Slider1;    //RotationX min=-90 max=90
    public Slider B_Slider1;    //RotationY min=-90 max=90
    public Slider T_Slider1;    //RotationZ min=-90 max=90
    public static double[] theta = new double[6];    //angle of the joints
    public static double[] inittheta = new double[6];
    private float L1, L2, L3, L4, L5, L6;    //arm length in order from base
    private float C3;
    int flagx, flagy, flagz;
    InverseCalc I = new InverseCalc();
    public float px = 8f, py = 0f, pz = 8f;
    public float rx = 0f, ry = 0f, rz = 0f;
    float intmdx, intmdy, intmdz;
    float diffx, diffy, diffz;
    GameObject endeffector;
    Vector3 coords;
    Canvas c;
    InputField[] coordinates;
    GameObject arm;
    Button move;
    float valueX, valueY, valueZ, valueRX, valueRY, valueRZ;
    public LinearMapping lmX;
    public LinearMapping lmY;
    public LinearMapping lmZ;
    public LinearMapping lmRX;
    public LinearMapping lmRY;
    public LinearMapping lmRZ;
    //void onCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("6dof_robotL"))
    //    {
    //        Rigidbody rbdy = collision.gameObject.GetComponent<Rigidbody>();
    //        rbdy.velocity = Vector3.zero;
    //        rbdy.angularVelocity = Vector3.zero;
    //    }
    //}

    // Use this for initialization
    void Start()
    {
        
        theta[0] = theta[1] = theta[2] = theta[3] = theta[4] = theta[5] = 0.0;
        L1 = 4.0f;
        L2 = 6.0f;
        L3 = 3.0f;
        L4 = 4.0f;
        L5 = 2.0f;
        L6 = 1.0f;
        C3 = 0.0f;
        endeffector = GameObject.Find("EffectorL");
        px = 8f; py = 0f; pz = 8f;
        rx = 0f; ry = 0f; rz = 0f;
        inittheta = I.CalcInverse(x_coord, y_coord, z_coord, rx, ry, rz);
        c = GameObject.Find("CanvasLeft").GetComponent<Canvas>();
        coordinates = c.GetComponentsInChildren<InputField>();
        move = c.GetComponentInChildren<Button>();
        move.onClick.AddListener(MoveLeft);
        coordinates[0].text = "8";
        coordinates[1].text = "0";
        coordinates[2].text = "8";
        stepx = 0f;
        stepy = 0f;
        stepz = 0f;
        coords.x = 5.0f;
        coords.y = -8.0f;
        coords.z = 8.0f;
    }
    void MoveLeft()
    {
        x_coord = float.Parse(coordinates[0].text.ToString());
        y_coord = float.Parse(coordinates[1].text.ToString());
        z_coord = float.Parse(coordinates[2].text.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        valueX = Mapping(lmX.value, 1, 8, 4);
        valueY = Mapping(lmY.value, 1, 8, -4);
        valueZ = Mapping(lmZ.value, 1, 8, 4);
        valueRX = Mapping(lmRX.value, 1, 180, -90);
        valueRY = Mapping(lmRY.value, 1, 180, -90);
        valueRZ = Mapping(lmRZ.value, 1, 180, -90);
        px = valueX;
        //py = L_Slider.value;
        py = valueY;
        //pz = U_Slider.value;
        pz = valueZ;
        //rx = R_Slider.value;
        rx = valueRX;
        //ry = B_Slider.value;
        ry = valueRY;
        //rz = T_Slider.value;
        rz = valueRZ;
        S_Slider1.value = px;
        L_Slider1.value = py;
        U_Slider1.value = pz;
        R_Slider1.value = rx;
        B_Slider1.value = ry;
        T_Slider1.value = rz;
        py = -py;
        rz = -rz;
        
        /*
        flagx = SetPointx();
        flagy = SetPointy();
        flagz = SetPointz();
        //Debug.Log("The DIFFErence is" + ((Math.Round(-coords.y, 1, MidpointRounding.AwayFromZero) - ((x_coord)))));
        if (flagx == 1)
        {
            if (stepx <= 1)
            {
                intmdx = (float)(endx + stepx * (startx - endx));
                stepx = stepx + 0.005f;
            }
        }
        else
        {
            intmdx = x_coord;
            theta = inittheta;
        }

        if (flagy == 1)
        {
            if (stepy <= 1)
            {
                intmdy = (float)(endy + stepy * (starty - endy));
                stepy = stepy + 0.005f;
            }
        }
        else
        {
            intmdy = -y_coord;
            theta = inittheta;
        }

        if (flagz == 1)
        {
            if (stepz <= 1)
            {
                intmdz = (float)(endz + stepz * (startz - endz));
                stepz = stepz + 0.005f;
            }
        }
        else
        {
            intmdz = z_coord;
            theta = inittheta;
        }
        if ((intmdx >= 4 && intmdx <= 12) && (intmdy >= -4 && intmdy <= 4) && (intmdz >= 4 && intmdz <= 12))
            theta = I.CalcInverse(intmdx, intmdy, intmdz, rx, ry, rz, inittheta);
        inittheta = theta;
        */
        theta = I.CalcInverse(px, py, pz, rx, ry, rz, inittheta);
        SetJoints();
        flagx = 0;
        flagy = 0;
        flagz = 0;
        coords = endeffector.transform.position;
      //  Debug.Log(coords);
    }
    void SetJoints()
    {
        //Debug.Log(theta[0] + "    " + theta[1] + "    " + theta[2] + "    " + theta[3] + "    " + theta[4] + "    " + theta[5]);
        if (!double.IsNaN(theta[0]))
        {

            Joints1[0].transform.localEulerAngles = new Vector3(0, 0, (float)theta[0] * Mathf.Rad2Deg);
        }
        if (!double.IsNaN(theta[1]))
        {

            Joints1[1].transform.localEulerAngles = new Vector3((float)theta[1] * Mathf.Rad2Deg, 0, 0);
        }
        if (!double.IsNaN(theta[2]))
        {

            Joints1[2].transform.localEulerAngles = new Vector3((float)theta[2] * Mathf.Rad2Deg, 0, 0);


        }
        if (!double.IsNaN(theta[3]))
        {

            Joints1[3].transform.localEulerAngles = new Vector3(0, 0, (float)theta[3] * Mathf.Rad2Deg);
        }
        if (!double.IsNaN(theta[4]))
        {

            Joints1[4].transform.localEulerAngles = new Vector3((float)theta[4] * Mathf.Rad2Deg, 0, 0);
        }
        if (!double.IsNaN(theta[5]))
        {

            Joints1[5].transform.localEulerAngles = new Vector3(0, 0, (float)theta[5] * Mathf.Rad2Deg);
        }
    }
    int SetPointx()
    {
        if ((Math.Round(-coords.y, 1, MidpointRounding.AwayFromZero) - ((x_coord))) != 0 && (x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
        {
           // Debug.Log("cy" + Math.Round(-coords.y));
           // Debug.Log("xc" + x_coord);
            diffx = (x_coord) - (float)Math.Round(-coords.y);

            startx = x_coord;
            endx = Math.Round(-coords.y, 1, MidpointRounding.AwayFromZero);


            return 1;
        }
        else
        {
            stepx = 0;
        }
        return 0;
    }
    int SetPointy()
    {
        if ((Math.Round(coords.x - 5, 1, MidpointRounding.AwayFromZero) - ((-y_coord))) != 0 && (x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
        {
           // Debug.Log("cx" + Math.Round(coords.x - 5));
           // Debug.Log("xc" + y_coord);
            diffx = (-y_coord) - (float)Math.Round(coords.x - 5);

            starty = -y_coord;
            endy = Math.Round(coords.x - 5, 1, MidpointRounding.AwayFromZero);


            return 1;
        }
        else
            stepy = 0;
        return 0;
    }

    int SetPointz()
    {
        if ((Math.Round(coords.z, 1, MidpointRounding.AwayFromZero) - ((z_coord))) != 0 && (x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
        {
            //Debug.Log("cx" + Math.Round(coords.z));
            //Debug.Log("xc" + z_coord);
            diffx = (z_coord) - (float)Math.Round(coords.z);

            startz = z_coord;
            endz = Math.Round(coords.z, 1, MidpointRounding.AwayFromZero);


            return 1;
        }
        else
            stepz = 0;
        return 0;
    }
    float Mapping(float OldValue, float OldRange, float NewRange, float NewMin)
    {

        return ((((OldValue) * NewRange) / OldRange) + NewMin);
    }
}
