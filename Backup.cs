using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System;



public class Backup : MonoBehaviour
{
    public float x_coord;
    public float y_coord;
    public float z_coord;
    public Transform[] Joints;
    public static double[] inittheta = new double[6];
    public Slider S_Slider;    //PositionX min=4 max=12
    public Slider L_Slider;    //PositionY min=-4 max=4
    public Slider U_Slider;    //PositionZ min=4 max=12
    public Slider R_Slider;    //RotationX min=-90 max=90
    public Slider B_Slider;    //RotationY min=-90 max=90
    public Slider T_Slider;    //RotationZ min=-90 max=90
    public static double[] theta = new double[6];    //angle of the joints
    double startx, endx, starty, endy, startz, endz;
    float stepx, stepy, stepz;
    private float L1, L2, L3, L4, L5, L6;    //arm length in order from base
    private float C3;
    InverseCalc I = new InverseCalc();
    public float px = 8f, py = 0f, pz = 8f;
    public float rx = 0f, ry = 0f, rz = 0f;
    float intmdx, intmdy, intmdz;
    float diffx, diffy, diffz;
    GameObject endeffector;
    Vector3 coords;

    //public InverseCalc invcal;
    // Use this for initialization

    //void onCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("6dof_robotR"))
    //    {
    //        Rigidbody rbdy = collision.gameObject.GetComponent<Rigidbody>();
    //        rbdy.velocity = Vector3.zero;
    //        rbdy.angularVelocity = Vector3.zero;
    //    }
    //}
    int flagx, flagy, flagz;
    void Start()
    {
        //invcal = gameObject.GetComponent<InverseCalc>();
        theta[0] = theta[1] = theta[2] = theta[3] = theta[4] = theta[5] = 0.0;
        L1 = 4.0f;
        L2 = 6.0f;
        L3 = 3.0f;
        L4 = 4.0f;
        L5 = 2.0f;
        L6 = 1.0f;
        C3 = 0.0f;
        endeffector = GameObject.Find("EffectorR");
        px = 8f; py = 0f; pz = 8f;
        rx = 0f; ry = 0f; rz = 0f;
        inittheta = I.CalcInverse(px, py, pz, rx, ry, rz);
        //Debug.Log("START METHOD"+inittheta[3]);
        stepx = 0f;
        stepy = 0f;
        stepz = 0f;
        coords.x = -5.0f;
        coords.y = -8.0f;
        coords.z = 8.0f;
    }
    void Delay()
    {
        Debug.Log("Delay");
    }
    // Update is called once per frame
    void Update()
    {


        int i = 0;
        px = S_Slider.value;
        py = L_Slider.value;
        pz = U_Slider.value;
        rx = R_Slider.value;
        ry = B_Slider.value;
        rz = T_Slider.value;

        flagx = SetPointx();
        flagy = SetPointy();
        flagz = SetPointz();
        Debug.Log("The DIFFErence is" + ((Math.Round(-coords.y, 1, MidpointRounding.AwayFromZero) - ((x_coord)))));
        if (flagx == 1)
        {
            Debug.Log("step=" + stepx);
            Debug.Log(stepx);
            Debug.Log("start = " + startx);
            Debug.Log("end = " + endx);
            if (stepx <= 1)
            {
                intmdx = (float)(endx + stepx * (startx - endx));

                //    if ((x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
                //    {
                //        //Debug.Log(intmdx);
                //        //theta = I.CalcInverse(intmdx, y_coord, z_coord, rx, ry, rz, inittheta);
                //        //Debug.Log("Hello MC");
                //    }
                //    else
                //    {
                //        theta = inittheta;
                //    SetJoints();
                //}


                stepx = stepx + 0.005f;


            }


        }
        else
        {
            intmdx = x_coord;
            theta = inittheta;
            //SetJoints();
        }

        if (flagy == 1)
        {
            Debug.Log("stepy=" + stepy);
            Debug.Log(stepy);
            Debug.Log("starty = " + starty);
            Debug.Log("endy = " + endy);
            if (stepy <= 1)
            {
                intmdy = (float)(endy + stepy * (starty - endy));

                //if ((x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
                //{
                //    Debug.Log(intmdy);
                //    //theta = I.CalcInverse(x_coord, intmdy, z_coord, rx, ry, rz, inittheta);
                //    Debug.Log("Hello MCY");
                //}
                //else
                //{
                //    theta = inittheta;
                //    SetJoints();
                //}


                stepy = stepy + 0.005f;


            }


        }
        else
        {
            intmdy = y_coord;
            theta = inittheta;
            // SetJoints();
        }

        if (flagz == 1)
        {
            Debug.Log("stepz=" + stepz);
            Debug.Log(stepz);
            Debug.Log("startz = " + startz);
            Debug.Log("endz = " + endz);
            if (stepz <= 1)
            {
                intmdz = (float)(endz + stepz * (startz - endz));

                //if ((x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
                //{
                //    Debug.Log(intmdz);
                //    //theta = I.CalcInverse(x_coord, intmdy, z_coord, rx, ry, rz, inittheta);
                //    Debug.Log("Hello MCZ");
                //}
                //else
                //{
                //    theta = inittheta;
                //    SetJoints();
                //}


                stepz = stepz + 0.005f;


            }


        }
        else
        {
            intmdz = z_coord;
            theta = inittheta;
            // SetJoints();
        }






        //if(start==end)
        //{ step = 0; }

        //theta = I.CalcInverse(px, py, pz, rx, ry, rz, inittheta);



        if (flagx == 1 || flagy == 1 || flagz == 1)
            theta = I.CalcInverse(intmdx, intmdy, intmdz, rx, ry, rz, inittheta);
        else
        {
            theta = inittheta;

        }
        inittheta = theta;
        SetJoints();
        flagx = 0;
        flagy = 0;
        flagz = 0;
        coords = endeffector.transform.position;
        Debug.Log(coords);
        // Debug.Log("First " + GameObject.Find("EndEff").transform.position);
        //Debug.Log("Px" + px + "Py" + py + "pz" + pz);
        //Debug.Log(((GameObject.Find("Cylinder0").transform.position)- (GameObject.Find("EffectorR").transform.position)));
        // Debug.Log(GameObject.Find("Cylinder4").transform.position);
        // Debug.Log(GameObject.Find("EffectorR").transform.position);
    }
    void SetJoints()
    {
        Debug.Log(theta[0] + "    " + theta[1] + "    " + theta[2] + "    " + theta[3] + "    " + theta[4] + "    " + theta[5]);
        if (!double.IsNaN(theta[0]))
        {

            Joints[0].transform.localEulerAngles = new Vector3(0, 0, (float)theta[0] * Mathf.Rad2Deg);
        }
        if (!double.IsNaN(theta[1]))
        {

            Joints[1].transform.localEulerAngles = new Vector3((float)theta[1] * Mathf.Rad2Deg, 0, 0);
        }
        if (!double.IsNaN(theta[2]))
        {

            Joints[2].transform.localEulerAngles = new Vector3((float)theta[2] * Mathf.Rad2Deg, 0, 0);


        }
        if (!double.IsNaN(theta[3]))
        {

            Joints[3].transform.localEulerAngles = new Vector3(0, 0, (float)theta[3] * Mathf.Rad2Deg);
        }
        if (!double.IsNaN(theta[4]))
        {

            Joints[4].transform.localEulerAngles = new Vector3((float)theta[4] * Mathf.Rad2Deg, 0, 0);
        }
        if (!double.IsNaN(theta[5]))
        {

            Joints[5].transform.localEulerAngles = new Vector3(0, 0, (float)theta[5] * Mathf.Rad2Deg);
        }
    }
    int SetPointx()
    {
        if ((Math.Round(-coords.y, 1, MidpointRounding.AwayFromZero) - ((x_coord))) != 0 && (x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
        {
            Debug.Log("cy" + Math.Round(-coords.y));
            Debug.Log("xc" + x_coord);
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
        if ((Math.Round(coords.x + 5, 1, MidpointRounding.AwayFromZero) - ((y_coord))) != 0 && (x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
        {
            Debug.Log("cx" + Math.Round(coords.x + 5));
            Debug.Log("xc" + y_coord);
            diffx = (y_coord) - (float)Math.Round(coords.x + 5);

            starty = y_coord;
            endy = Math.Round(coords.x + 5, 1, MidpointRounding.AwayFromZero);


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
            Debug.Log("cx" + Math.Round(coords.z));
            Debug.Log("xc" + z_coord);
            diffx = (z_coord) - (float)Math.Round(coords.z);

            startz = z_coord;
            endz = Math.Round(coords.z, 1, MidpointRounding.AwayFromZero);


            return 1;
        }
        else
            stepz = 0;
        return 0;
    }
}


