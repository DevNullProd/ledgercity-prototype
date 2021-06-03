using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseInput : MonoBehaviour
{
    //**MANAGE Mouse motion*//
    //**ATTACHED TO CameraBase*//
    /*NOTES:
     * 
     */

    [Header("References")]
    CameraMotion camMotion;
    [SerializeField] UnityEngine.UI.Text Button_Mouse = null;

    [Header("Variables")]
    public bool MouseEnabled = false;
    public Vector2 Sensitivity = new Vector2(1, 1);
    //bool MouseDrag = false;
    bool doMouse = false;
    Vector2 OldMousePos = Vector2.zero;
    Vector2 NewMousePos = Vector2.zero;
    Vector2 MouseBuffer = Vector2.zero;
    float MouseTimer;
    float Mouse_Timer_Interval = 0.3f;

    public float speedH = 2.0f;
    public float speedV = 1000000000000000.0f;


    readonly bool FreeRotate = true;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    //For FreeRotate Only
    float pitchMin = -31f;
    float pitchOld;
    float pitchMax = 80f;
    float MinCamHeight = 60f;

    // Start is called before the first frame update
    void Start()
    {
        camMotion = this.gameObject.GetComponent<CameraMotion>();
        TogleMouse();

        if (FreeRotate)
        {
            speedV = 2;
            camMotion.FreeRotation = FreeRotate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseEnabled && camMotion.CameraStartAnim == false)
        {
            if (Input.GetMouseButtonDown(0))
                doMouse = true;

            if (Input.GetMouseButtonUp(0))
            {
                doMouse = false;
                if (FreeRotate == false)
                    pitch = 0;
            }

            if (doMouse)
            {
                if (GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().PanelSettings.activeInHierarchy == false & camMotion.Car_Selected == null)
                {
                    //To AVOID hitting trough UI elements
                    if (EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        if (camMotion.Selected == null)
                        {
                            yaw += speedH * Input.GetAxis("Mouse X");
                            pitch += speedV * Input.GetAxis("Mouse Y");

                            if (FreeRotate == false)
                            {
                                //Rotation
                                camMotion.gameObject.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);

                                //Motion
                                Transform target = this.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                                Vector3 offset = Vector3.forward * pitch * speedV /** Time.deltaTime*/;
                                camMotion.gameObject.transform.position = target.TransformPoint(offset); // this will transform your local position to world position
                            }
                            else
                            {
                                if (camMotion.CamMain.transform.position.y < MinCamHeight)
                                {
                                    if (pitch < pitchOld)
                                        pitch = pitchOld;
                                }
                                else
                                    pitchOld = pitch;

                                if (pitch >= pitchMax)
                                    pitch = pitchMax;
                                else if (pitch < pitchMin)
                                    pitch = pitchMin;

                                //Rotation
                                camMotion.gameObject.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
                            }

                            Debug.Log("Yaw = " + yaw + "  > Pitch = " + pitch + "  > camMotion.CamMain.transform.position.y = " + camMotion.CamMain.transform.position.y);
                        }
                        else
                        {
                            yaw = speedH * Input.GetAxis("Mouse X");
                            pitch = speedV * Input.GetAxis("Mouse Y");

                            Debug.Log("Yaw = " + yaw + "  > Pitch = " + pitch);

                            this.gameObject.GetComponent<Rotator>().RotSpeed += yaw /** Time.deltaTime*/;
                            camMotion.SelectedDistance += pitch * Time.deltaTime * -camMotion.AverageDistance / 2f;

                            camMotion.Fix_Selected_Limits();
                        }
                    }
                }
            }

            //Mouse Wheel move
            if (FreeRotate)
            {
                float MouseWheelValue = Input.mouseScrollDelta.y * 10f;
                if (MouseWheelValue != 0)
                {
                    if (GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().PanelSettings.activeInHierarchy == false & camMotion.Car_Selected == null)
                    {
                        if (camMotion.Selected == null)
                        {
                            if (camMotion.CamMain.transform.position.y > MinCamHeight
                                || (camMotion.CamMain.transform.position.y < MinCamHeight & pitch < 0 & MouseWheelValue > 0)
                                || (camMotion.CamMain.transform.position.y < MinCamHeight & pitch >= 0 & MouseWheelValue < 0))
                            {
                                Transform target = camMotion.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                                Vector3 offset = Vector3.forward * (camMotion.SpeedMax / 50f) * MouseWheelValue * Time.deltaTime;
                                camMotion.transform.position = target.TransformPoint(offset); // this will transform your local position to world position
                            }
                        }
                        else
                        {
                            camMotion.TargetBuildingPos = camMotion.TargetBuildingPos + new Vector3(0, MouseWheelValue * 100f * Time.deltaTime, 0);

                            if (camMotion.TargetBuildingPos.y < 10f)
                                camMotion.TargetBuildingPos.y = 10f;

                            if (camMotion.TargetBuildingPos.y > camMotion.Selected.GetComponent<Building>().Height_Actual + 10f)
                                camMotion.TargetBuildingPos.y = camMotion.Selected.GetComponent<Building>().Height_Actual + 10f;
                        }
                    }
                }
            }
        }

        //Process_Mouse();
    }

    public void Unselect_Building()
    {
        if (FreeRotate)
        {
            yaw = camMotion.transform.eulerAngles.y;
            pitch = camMotion.transform.eulerAngles.x;
            Debug.Log("Unselect CamBase.pos = " + camMotion.transform.position + "  > CamBase.rot = " + camMotion.transform.eulerAngles + "  > yaw = " + yaw + "  > pitch = " + pitch);
        }
    }

    //void Process_Mouse()
    //{
    //    if (MouseEnabled)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            camMotion.JoyStick_Motion(Vector2.zero);
    //            OldMousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/*.normalized*/;
    //            doMouse = true;
    //            MouseTimer = Time.realtimeSinceStartup + Mouse_Timer_Interval;

    //            Debug.Log("NewMousePos = " + NewMousePos + "  > OldMousePos" + OldMousePos + "  > Input.GetAxis(Mouse X) = "
    //                + Input.GetAxis("Mouse X") + "  > Input.GetAxis(Mouse Y) = " + Input.GetAxis("Mouse Y") + "  > mouse coords = " + Input.mousePosition);
    //        }

    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            OldMousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/*.normalized*/;
    //            camMotion.JoyStick_Motion(Vector2.zero);
    //            doMouse = false;

    //            Debug.Log("NewMousePos = " + NewMousePos + "  > OldMousePos" + OldMousePos + "  > Input.GetAxis(Mouse X) = "
    //                + Input.GetAxis("Mouse X") + "  > Input.GetAxis(Mouse Y) = " + Input.GetAxis("Mouse Y") + Input.mousePosition);
    //        }

    //        if (doMouse)
    //        {
    //            if (Time.realtimeSinceStartup < MouseTimer)
    //            {
    //                if (Input.mousePosition.y < Screen.height / 2)
    //                    NewMousePos = new Vector2(Input.GetAxis("Mouse X") - OldMousePos.x, Input.GetAxis("Mouse Y") - OldMousePos.y)/*.normalized*/;
    //                else
    //                    NewMousePos = new Vector2((Input.GetAxis("Mouse X") - OldMousePos.x) * -1f, Input.GetAxis("Mouse Y") - OldMousePos.y)/*.normalized*/;

    //                //MouseBuffer = Vector2.Lerp(NewMousePos, OldMousePos, Time.deltaTime * 0.1f);
    //                camMotion.JoyStick_Motion(NewMousePos * Sensitivity);

    //                //Debug.Log("NewMousePos = " + NewMousePos + "  > OldMousePos" + OldMousePos + "  > Input.GetAxis(Mouse X) = " 
    //                //    + Input.GetAxis("Mouse X") + "  > Input.GetAxis(Mouse Y) = " + Input.GetAxis("Mouse Y"));
    //            }
    //            else
    //            {
    //                OldMousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/*.normalized*/;
    //                //camMotion.JoyStick_Motion(Vector2.zero);
    //                MouseTimer = Time.realtimeSinceStartup + Mouse_Timer_Interval;
    //                //doMouse = false;
    //            }
    //        }
    //    }
    //}



    public void TogleMouse()
    {
        MouseEnabled = !MouseEnabled;

        if (MouseEnabled)
            Button_Mouse.text = "Mouse: ON";
        else
            Button_Mouse.text = "Mouse: OFF";
    }
}
