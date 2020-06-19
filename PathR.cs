using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System;



public class PathR : MonoBehaviour
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
    double start, end;
    float step;
    private float L1, L2, L3, L4, L5, L6;    //arm length in order from base
    private float C3;
    InverseCalc I = new InverseCalc();
    public float px = 8f, py = 0f, pz = 8f;
    public float rx = 0f, ry = 0f, rz = 0f;
    float intmd;
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
    int flag;
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
        step = 0f;
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

        Debug.Log(coords);
        flag = 0;
        int i = 0;
        px = S_Slider.value;
        py = L_Slider.value;
        pz = U_Slider.value;
        rx = R_Slider.value;
        ry = B_Slider.value;
        rz = T_Slider.value;
        Debug.Log("The DIFFErence is" + ((Math.Round(-coords.y) - ((x_coord)))));
        if ((Math.Round(-coords.y) - ((x_coord))) != 0)
        {
            Debug.Log("cy" + Math.Round(-coords.y));
            Debug.Log("xc" + x_coord);
            diffx = (x_coord) - (float)Math.Round(-coords.y);
            if (diffx > 0)
            {
                start = Math.Round(-coords.y);
                end = x_coord;
            }
            else if (diffx < 0)
            {
                start = x_coord;
                end = Math.Round(-coords.y);
            }
            while (step < 1.1)
            {
                Debug.Log(step);
                Debug.Log("start = " + start);
                Debug.Log("end = " + end);
                if (diffx < 0)
                    intmd = (float)(end + step * (start - end));
                else if (diffx > 0)
                {
                    intmd = (float)(start + step * (end - start));
                }
                if ((x_coord >= 4 && x_coord <= 12) && (y_coord >= -4 && y_coord <= 4) && (z_coord >= 4 && z_coord <= 12))
                {
                    Debug.Log(intmd);
                    theta = I.CalcInverse(intmd, y_coord, z_coord, rx, ry, rz, inittheta);
                    Debug.Log("Hello MC");
                }
                else
                {
                    theta = inittheta;
                }
                SetJoints();
                step = step + 0.1f;
                //if (step == 1)
                //{
                //    flag = 1;
                //}
                //if (step > 1)
                //{ step = 1;}

            }

            //Debug.Log("LOOP ke bahar");
        }
        else
        {
            theta = inittheta;
            SetJoints();
        }
        step = 0f;
        //theta = I.CalcInverse(px, py, pz, rx, ry, rz, inittheta);
        coords = endeffector.transform.position;
        //foreach (double t in theta)
        //{
        //    Debug.Log(i+":"+t);
        //    i++;
        //}
        //i = 0;
        //flag = 0;
        //for(i=0;i<6;i++)
        //{
        //    if(theta[i]!=inittheta[i])
        //    {
        //        flag = 1;
        //    }
        //}
        //if(flag == 0)
        //{
        //    return;
        //}

        //Debug.Log(inittheta[2]==theta[2]);
        inittheta = theta;

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
}
