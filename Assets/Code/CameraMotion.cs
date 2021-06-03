using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMotion : MonoBehaviour
{
    //**MANAGE Camera motion*//
    //**ATTACHED TO GameManager.gameObject*//
    /*NOTES:
     * 
     */

    public GameObject CamMain = null;
    float CamRelativePos;

    [SerializeField] Slider SliderHeight = null;
    [SerializeField] Text TextAltitude = null;
    [SerializeField] GameObject ButtonCloseCar = null;
    [SerializeField] GameObject ButtonOptions = null;
    readonly float MaxHeight = 1000f;
    readonly float MinHeight = -600f;
    readonly public float AverageDistance = 400f;

    [SerializeField] GameObject CityCenter = null;

    bool BuildingSelected = false;
    public GameObject Selected = null;
    public Vector3 TargetBuildingPos;
    Vector3 CamStart_localPos;
    Vector3 CamStart_Rotation;
    Vector3 CamZoom_LocalPos = new Vector3(0, 200, 270);
    float CamMinDistance;
    public float SelectedDistance;

    //Car selected
    public GameObject Car_Selected = null;

    Vector3 OldPos;

    public float SpeedMax = 800f;
    float ActualSpeed;
    float RotationSpeed;
    Vector2 JoyInput = Vector2.zero;

    //Camera START anim
    public bool CameraStartAnim = true;
    float CameraAnimDelay;
    Vector3 CamStartRot = new Vector3(0f, -170f, 0);
    Quaternion CamEndRot;
    Vector3 CamStartPos = new Vector3(2000f, 0f, 2000f);
    Vector3 CamEndPos;

    //RenderSettings
    float RenderAmbienceIntensity_StarVal;

    [Header("variables")]
    float MouseWheelValue;
    public bool FreeRotation;

    // Start is called before the first frame update
    void Start()
    {
        ButtonCloseCar.SetActive(false);
        CamStart_localPos = CamMain.transform.localPosition;
        CamStart_Rotation = transform.eulerAngles;
        this.gameObject.GetComponent<Rotator>().enabled = false;
        SliderHeight.maxValue = MaxHeight;
        SliderHeight.minValue = MinHeight;
        SliderHeight.value = transform.position.y;

        //Camera START anim
        CamEndRot = this.gameObject.transform.rotation;
        transform.eulerAngles = CamStartRot;
        CamEndPos = transform.position;
        transform.position = transform.position + CamStartPos;
        CameraAnimDelay = Time.realtimeSinceStartup + 0.2f;

        //render effects animation
        RenderAmbienceIntensity_StarVal = RenderSettings.ambientIntensity;
        RenderSettings.ambientIntensity = 20f;

        
        GameManager.gameMngInst.CanvasMain.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.(transform.eulerAngles, CamEndRot.eulerAngles) + "  > transform.eulerAngles = " + transform.eulerAngles + "  > CamEndRot.eulerAngles = " + CamEndRot.eulerAngles);
        if (CameraStartAnim)
        {
            Cam_Start_Animation();

            return;
        }

        if (Selected != null)
            Select_Animation();
        //else
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, CamStart_Rotation, Time.deltaTime * speed / 1000f);

        //if (Car_Selected != null)
        //    Car_Select_Animation();

        CameraZoom();

        ManageKeys();

        Joy_Move_Rotate();
        CameraMove();

        SliderHeight.value = transform.position.y;

        TextAltitude.text = "Altitude\n" + ((int)CamMain.transform.position.y).ToString();

        if (FreeRotation == false)
        {
            MouseWheelValue = Input.mouseScrollDelta.y * 10f;
            if (MouseWheelValue != 0 && Selected == null)
            {
                SliderHeight.value += MouseWheelValue;
                SliderHeight_Handle();
            }
        }
    }

    void ManageKeys()
    {
        //Unselect
        if (Input.GetKey(KeyCode.X))
        {
            //if (Selected != null)
            //{
            //    GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().HideCanvas();
            //    Unselect_Building();
            //}
            Unselect_Car();
            GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().Hide_PanelSettings();
        }

        if (GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().PanelSettings.activeInHierarchy == false)
        {
            if (Selected)
                Manage_Keys_Selected();
            else
                Manage_Keys_Unselected();
        }
    }

    void Manage_Keys_Selected()
    {
        //Translation
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.gameObject.GetComponent<Rotator>().RotSpeed += 20f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.gameObject.GetComponent<Rotator>().RotSpeed -= 20f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            SelectedDistance += 50f * Time.deltaTime * -AverageDistance / 2f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            SelectedDistance -= 50f * Time.deltaTime * -AverageDistance / 2f;
        }

        Fix_Selected_Limits();
    }

    public void Fix_Selected_Limits()
    {
        //FIX for max values
        if (this.gameObject.GetComponent<Rotator>().RotSpeed > 40f)
            this.gameObject.GetComponent<Rotator>().RotSpeed = 40f;

        if (this.gameObject.GetComponent<Rotator>().RotSpeed < -40f)
            this.gameObject.GetComponent<Rotator>().RotSpeed = -40f;

        if (SelectedDistance < -70)
            SelectedDistance = -70;
        else if (SelectedDistance > 200)
            SelectedDistance = 200;
    }

    void Manage_Keys_Unselected()
    {
        if (CamMain.activeInHierarchy)
        {
            float motionRange = 2500f;

            //Translation
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Transform target = this.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                Vector3 offset = Vector3.left * SpeedMax * Time.deltaTime;
                transform.position = target.TransformPoint(offset); // this will transform your local position to world position
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Transform target = this.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                Vector3 offset = Vector3.right * SpeedMax * Time.deltaTime;
                transform.position = target.TransformPoint(offset); // this will transform your local position to world position
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                Transform target = this.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                Vector3 offset = Vector3.forward * SpeedMax * Time.deltaTime;
                transform.position = target.TransformPoint(offset); // this will transform your local position to world position
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                Transform target = this.gameObject.transform; // transform matrix of the gameobject that you will position relatively
                Vector3 offset = Vector3.back * SpeedMax * Time.deltaTime;
                transform.position = target.TransformPoint(offset); // this will transform your local position to world position
            }

            ////Rotation
            //if (Input.GetKey(KeyCode.Q))
            //{
            //    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 40f * RotationSpeed * Time.deltaTime, 0);
            //}

            //if (Input.GetKey(KeyCode.E))
            //{
            //    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - 40f * RotationSpeed * Time.deltaTime, 0);
            //}

            //Altitude
            if (Input.GetKey(KeyCode.R))
            {
                SliderHeight.value += 400f * Time.deltaTime;
                SliderHeight_Handle();
            }

            if (Input.GetKey(KeyCode.F))
            {
                SliderHeight.value -= 400f * Time.deltaTime;
                SliderHeight_Handle();
            }

            //Fix Motion Range
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -motionRange, motionRange), transform.position.y, Mathf.Clamp(transform.position.z, -motionRange, motionRange));
        }
    }

    void Cam_Start_Animation()
    {
        if (CameraAnimDelay > Time.realtimeSinceStartup)
            return;

        //Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, CamEndRot, Time.deltaTime * 1f);

        //Position
        transform.position = Vector3.Lerp(transform.position, CamEndPos, Time.deltaTime * 1f);

        //Debug.Log("Quaternion.Angle(transform.rotation, CamEndRot) = " + Quaternion.Angle(transform.rotation, CamEndRot));

        //if (((int)(Mathf.Abs(transform.eulerAngles.y)) == ((int)(Mathf.Abs(CamEndRot.eulerAngles.y)))))
        if (Quaternion.Angle(transform.rotation, CamEndRot) < 1f)
        {
            transform.rotation = CamEndRot;
            transform.position = CamEndPos;
            CameraStartAnim = false;
            RenderSettings.ambientIntensity = RenderAmbienceIntensity_StarVal;
            GameManager.gameMngInst.CanvasMain.SetActive(true);
        }

        if (RenderSettings.ambientIntensity > 1f)
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, RenderAmbienceIntensity_StarVal, Time.deltaTime * 1f);
    }

    void Select_Animation()
    {
        transform.position = Vector3.Lerp(transform.position, TargetBuildingPos, Time.deltaTime * SpeedMax / 1000f);

        if (Vector3.Distance(transform.position, TargetBuildingPos) < 50)
        {
            if (this.gameObject.GetComponent<Rotator>().enabled == false)
            {
                this.gameObject.GetComponent<Rotator>().enabled = true;
                this.gameObject.GetComponent<Rotator>().RotSpeed = 0f;
                JoyInput = Vector2.zero;
            }
        }
        else
        {
            Debug.Log("transform.eulerAngles.x = " + transform.eulerAngles.x);
            transform.rotation = Quaternion.Euler (new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 30f, 1f * Time.deltaTime), transform.eulerAngles.y, transform.eulerAngles.z));
        }
    }

    //void Car_Select_Animation()
    //{
    //    transform.position = Vector3.Lerp(transform.position, Car_Selected.transform.position, Time.deltaTime * SpeedMax / 1000f);

    //    if (Vector3.Distance(transform.position, TargetBuildingPos) < 50)
    //    {
            
    //    }
    //}

    void CameraZoom()
    {
        //if (Selected != null)
        //    CamMain.transform.localPosition = Vector3.Lerp(CamMain.transform.localPosition, CamZoom_LocalPos, Time.deltaTime * SpeedMax / 1000f);
        //else
        //    CamMain.transform.localPosition = Vector3.Lerp(CamMain.transform.localPosition, CamStart_localPos, Time.deltaTime * SpeedMax / 1000f);
    }

    public void Select_Building(GameObject targetObj)
    {
        if (GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().PanelSettings.activeInHierarchy == false)
        {
            //Select Building
            if (targetObj.name.Contains("Car_Holder") == false)
            {
                Debug.Log("targetObj.name = " + targetObj.name);
                Selected = targetObj;
                Unselect_Car();
                TargetBuildingPos = Selected.GetComponent<Building>().Top_Point.transform.position;
                SliderHeight.gameObject.SetActive(false);
                this.gameObject.GetComponent<Rotator>().RotSpeed = 0f;
                ButtonOptions.SetActive(false);
            }
            //Select Car
            else
            {
                //if (Selected != null)
                //{
                GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().HideCanvas();
                Unselect_Building();
                //}
                Unselect_Car();
                Car_Selected = targetObj;
                CamMain.SetActive(false);
                Car_Selected.GetComponent<Car_Script>().Set_Car_Camera_Active(true);

                GameManager.gameMngInst.sunController.Set_Weather();

                ButtonCloseCar.SetActive(true);
                ButtonOptions.SetActive(false);
            }
        }
    }

    public void Unselect_Car()
    {
        if (Car_Selected != null)
        {
            Car_Selected.GetComponent<Car_Script>().Set_Car_Camera_Active(false);
            CamMain.SetActive(true);
            Car_Selected = null;
            ButtonCloseCar.SetActive(false);
            ButtonOptions.SetActive(true);

            GameManager.gameMngInst.sunController.Set_Weather();
        }
    }

    public void Set_New_targetPos(Vector3 pos)
    {
        TargetBuildingPos = pos;
    }

    public void Unselect_Building()
    {
        if (Selected != null)
        {
            Selected.GetComponent<Building>().Unselect_Building();
            GetComponent<MouseInput>().Unselect_Building();
            Selected = null;
            this.gameObject.GetComponent<Rotator>().enabled = false;
            ButtonOptions.SetActive(true);
            //SliderHeight.gameObject.SetActive(true);
        }
    }

    public void SliderHeight_Handle()
    {
        transform.position = new Vector3(transform.position.x, SliderHeight.value, transform.position.z); 
    }

    public void JoyStick_Motion(Vector2 input)
    {
        Debug.Log("Input = " + input);

        JoyInput = input;
    }

    void Joy_Move_Rotate()
    {
        CalculateSpeed_for_Height();

        if (Selected == null)
        {
            //Rotation
            if (FreeRotation == false)
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - JoyInput.x * RotationSpeed /** Time.deltaTime*/, 0);

            //motion
            float dist = Vector3.Distance(transform.position, new Vector3(CityCenter.transform.position.x, transform.position.y, CityCenter.transform.position.z));
            if (dist < 5000)
            {
                if (dist < 4900)
                    OldPos = transform.position;
                Vector3 direction = (CamMain.transform.position - transform.position);
                direction.y = 0f;
                transform.position += direction.normalized * Time.deltaTime * (JoyInput.y * -1f) * ActualSpeed;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, OldPos, Time.deltaTime * ActualSpeed);
            }

            SelectedDistance = 0f;

            if (GameManager.gameMngInst.CamMotion.gameObject.GetComponent<MouseInput>().MouseEnabled == false)
                JoyInput = Vector2.Lerp(JoyInput, Vector2.zero, Time.deltaTime * 1f);
            else
                JoyInput = Vector2.zero;
        }
        else
        {
            if (this.gameObject.GetComponent<Rotator>().enabled & Mathf.Abs (JoyInput.x) > 0.05f)
                this.gameObject.GetComponent<Rotator>().RotSpeed = JoyInput.x * -40f * Time.deltaTime;

            SelectedDistance += JoyInput.y * Time.deltaTime * -AverageDistance / 2f;

            if (SelectedDistance < -70)
                SelectedDistance = -70;
            else if(SelectedDistance > 200)
                SelectedDistance = 200;
        }
    }

    void CameraMove()
    {
        //CamMain.transform.localPosition = Vector3.Lerp(CamMain.transform.localPosition, new Vector3(CamMain.transform.localPosition.x, CamMain.transform.localPosition.y, 100f), Time.deltaTime * 1f);
        //CamMain.transform.Translate(new Vector3(0f, 0f, CamRelativePos) * Time.deltaTime);

        float dist = Vector3.Distance(CamMain.transform.position, this.gameObject.transform.position) - SelectedDistance;
        float moveSpeed = AverageDistance;
        if ((dist - CamMinDistance) < moveSpeed)
            moveSpeed = Mathf.Abs(dist - CamMinDistance);

        //Debug.Log("CamMinDistance = " + CamMinDistance + "  > SelectedDistance = " + SelectedDistance + "  > dist = " + dist + "  > moveSpeed = " + moveSpeed);

        if (dist > CamMinDistance)
            CamMain.transform.Translate(new Vector3(0f, 0f, moveSpeed) * Time.deltaTime);
        else
            CamMain.transform.Translate(new Vector3(0f, 0f, -moveSpeed) * Time.deltaTime);

    }

    void CalculateSpeed_for_Height()
    {
        float range = MaxHeight - MinHeight;
        float heightRelative = Mathf.Abs(MinHeight) + transform.position.y;
        float muliplyIndex = heightRelative / range;

        CamRelativePos = /*600f +*/ (AverageDistance * (1f - muliplyIndex));
        if (Selected == null)
            CamMinDistance = AverageDistance /** 3f*/ + (AverageDistance * 1.5f * muliplyIndex);
        else
            CamMinDistance = AverageDistance / 10f + (200f * muliplyIndex);

        RotationSpeed = 1f + 1f * muliplyIndex;
        ActualSpeed = AverageDistance / 1f + SpeedMax * muliplyIndex;


        //Debug.LogError("MinHeight = " + MinHeight + "  > MaxHeight = " + MaxHeight + "  > transform.position.y = " + transform.position.y 
        //    + "  >range = " + range + "  > heightRelative = " + heightRelative + "  > muliplyIndex = " + muliplyIndex + "  > RotationSpeed = " + RotationSpeed + "  > ActualSpeed = " + ActualSpeed);
    }
}
