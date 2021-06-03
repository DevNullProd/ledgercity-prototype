using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Building : MonoBehaviour
{
    //**MANAGE Building Look*//
    //**ATTACHED TO Base of the Building Prefab*//
    /*NOTES:
     * 
     */

    [Header("References")]
    [SerializeField] GameObject Building_Base = null;
    [SerializeField] GameObject Building_Floor = null;
    [SerializeField] GameObject Building_Top = null;
    [SerializeField] GameObject ShadowCasterParent = null;
    public GameObject Top_Point = null;
    public GameObject Bottom_Point = null;
    [SerializeField] GameObject LookAt_base = null;


    [Header("Info")]
    [SerializeField] TextMesh Text3D_Name = null;
    [SerializeField] TextMesh Text3D_Info = null;
    [SerializeField] TextMeshPro Text3D_Custom = null;

    [Header("Variables")]
    public bool IsEditable = true;
    public string Owner_Name = "Jack";
    public int XRP_Total;
    public int XRP_Building_Value;
    public float Scale_Fix = 0.8f;
    public float Base_Fix = 1f;
    public float Floor_Fix = 1f;
    public bool IsSelected = false;
    public bool Selected = false;
    [SerializeField] bool IsTB = false;
    [SerializeField] Material DayMat = null;
    [SerializeField] Material NightMat = null;

    [Header("Start Animation")]
    bool StartAnimation = true;
    float StartAnimationTimer;
    Vector3 FinalLocalPos;

    [Header("Building")]
    public float Min_Height = 41;
    public float Max_Height = 300;
    public float Total_Height = 0;
    public float Height_Actual = 0;
    float Base_Height;
    Vector3 Base_Offset;
    float Floor_Height;
    float Top_Height;
    public float Measured_Height;
    GameObject[] Floors_Arrays;
    Color UnselectedColor = Color.white;
    Color SelectedColor = new Color(1f, 0.8f, 0.8f);
    bool RandomColor_Set = false;

    // Start is called before the first frame update
    void Start()
    {
        //Building_Base.transform.localPosition = Vector3.zero;

        if (IsEditable)
        {
            Base_Offset = Building_Base.transform.localPosition;

            Base_Height = Measure.GetObject_Height(Building_Base) * Building_Base.transform.localScale.y * Scale_Fix * Base_Fix;
            Floor_Height = Measure.GetObject_Height(Building_Floor) * Building_Floor.transform.localScale.y * Scale_Fix * Floor_Fix;
            Top_Height = Measure.GetObject_Height(Building_Top) * Building_Top.transform.localScale.y * Scale_Fix;
        }

        //BookMark DEBUG Random Building Parameters
        float dist_from_Center = Vector3.Distance(this.gameObject.transform.position, GameManager.gameMngInst.gameObject.transform.position);
        float dist_Height_Decrease = GameManager.gameMngInst.Calculate_Distance_Reduction(dist_from_Center, Min_Height, Max_Height);

        if (dist_Height_Decrease > (Max_Height - Min_Height) / 2f)
            dist_Height_Decrease = 0;

        XRP_Total = 100 * Random.Range(25, 120);

        if (IsEditable)
            XRP_Building_Value = (int)(Random.Range(Min_Height, Max_Height - dist_Height_Decrease));
        else
            XRP_Building_Value = (int)(Random.Range(Height_Actual - Height_Actual / 4, Max_Height + Height_Actual / 4));

        //Calculate Building Height
        Total_Height = Measure.GetBuildigHeight(XRP_Building_Value);

        if (GameManager.gameMngInst.Building_No >= GameManager.gameMngInst.XRP_Accounts.Length)
            GameManager.gameMngInst.Building_No = 0;

        Owner_Name = GameManager.gameMngInst.XRP_Accounts[GameManager.gameMngInst.Building_No];
        this.gameObject.name = Owner_Name;
        GameManager.gameMngInst.Building_No++;

        //Owner_Name = "Player " + GameManager.gameMngInst.Building_No++.ToString();
        //this.gameObject.name = Owner_Name;

        CreateBuilding();

        //Top_Point.GetComponent<Penthouse>().Penthouse_Base.transform.localScale = new Vector3(1f / transform.localScale.x, 1f, 1f / transform.localScale.z);
        //Set_Penthouse_Element(Random.Range(0, 80));

        //Bottom_Point.GetComponent<Penthouse>().Penthouse_Base.transform.localScale = new Vector3(1f / transform.localScale.x, 1f, 1f / transform.localScale.z);
        //Set_Bottom_Element(Random.Range(0, 80));

        SetBuildingColor(false);
        Set_Day_Mat(GameManager.gameMngInst.IsNight);

        //Start Anmation
        StartAnimation = true;
        FinalLocalPos = this.gameObject.transform.localPosition;
        StartAnimationTimer = Time.realtimeSinceStartup + 1f + dist_from_Center / 200f;
        this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(0, Measured_Height + 100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartAnimation)
        {
            if (Time.realtimeSinceStartup > StartAnimationTimer)
                this.gameObject.transform.localPosition = Vector3.Lerp(this.gameObject.transform.localPosition, FinalLocalPos, Time.deltaTime * 3f);

            if (Vector3.Distance(this.gameObject.transform.localPosition, FinalLocalPos) < 0.01f)
            {
                this.gameObject.transform.localPosition = FinalLocalPos;
                StartAnimation = false;
            }
        }
    }

    public void Create_Building_Again(int xrp_sum)
    {
        if (IsEditable == false)
            return;

        IsSelected = true;
        XRP_Building_Value = xrp_sum;

        //Calculate Building Height
        Total_Height = Measure.GetBuildigHeight(XRP_Building_Value);

        CreateBuilding();

        Set_Select_Material();
        Set_Day_Mat(GameManager.gameMngInst.IsNight);

        GameManager.gameMngInst.CamMotion.GetComponent<CameraMotion>().Set_New_targetPos(Top_Point.transform.position);
    }

    void CreateBuilding()
    {
        //Name & Info
        if (IsSelected)
            ShowInfo();
        else
            HideInfo();

        //Units Check
        if (Total_Height < Min_Height || IsEditable == false)
            return;

        //Reset Scale before building
        Vector3 oldScale = transform.localScale;
        transform.localScale = Vector3.one;
        
        Height_Actual = Base_Height;
        Building_Floor.SetActive(true);

        int NoOfFloors = (int)((Total_Height - (Base_Height + Top_Height)) / Floor_Height );

        if (NoOfFloors < 0)
            NoOfFloors = 0;

        //Destroy Old Floors if any
        DestroyOldFloors();

        //Dim Floor Array
        //Debug.Log("Building Name " + name + "  > No of Floors = " + NoOfFloors);
        Floors_Arrays = new GameObject[NoOfFloors];

        //Create Floors
        for (int x = 0; x < NoOfFloors; x++)
        {
            GameObject obb = Instantiate(Building_Floor);

            obb.transform.SetParent(this.gameObject.transform);
            obb.transform.localPosition = new Vector3(0, Height_Actual, 0) + Base_Offset;

            Height_Actual += Floor_Height;
            Floors_Arrays[x] = obb;
        }

        Building_Floor.SetActive(false);

        //Position Top
        Building_Top.transform.localPosition = new Vector3(0, Height_Actual, 0) + Base_Offset;

        //Revert scale after Building
        transform.localScale = oldScale;

        //Calculate REAL Height
        Measured_Height = Vector3.Distance(transform.position, Top_Point.transform.position);

        if (ShadowCasterParent != null)
            ShadowCasterParent.transform.localScale = new Vector3(1, 0.035f * Height_Actual, 1);

        ////Debug
        //Debug.Log("Building Created -" + name + "  > XRP_sum = " + XRP_Sum + "  > Total_Height = " + Total_Height
        //    + "  > Base_Height = " + Base_Height
        //    + "  > Floor_Height = " + Floor_Height
        //    + "  > Top_Height = " + Top_Height
        //    + "  > NoOfFloors = " + NoOfFloors + "  > Total Height = " + Height_Actual + "  >>> Measured_Height = " + Measured_Height);
    }

    void DestroyOldFloors()
    {
        if (Floors_Arrays != null)
        {
            for (int x = 0; x < Floors_Arrays.Length; x++)
                Destroy(Floors_Arrays[x]);
        }
    }

    public void Select_Building()
    {
        Selected = true;

        GameManager.gameMngInst.CamMotion.GetComponent<CameraMotion>().Unselect_Building();
        GameManager.gameMngInst.CamMotion.GetComponent<CameraMotion>().Select_Building(this.gameObject);

        GameManager.gameMngInst.CanvasBuilding.SetActive(true);
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().SetBuildingUI(this);

        Set_Select_Material();
    }

    public void Unselect_Building()
    {
        Selected = false;
        Set_Unslect_Material();
        HideInfo();
    }

    void Set_Select_Material()
    {
        SetBuildingColor(true);
    }

    void Set_Unslect_Material()
    {
        SetBuildingColor(false);

        Debug.Log(GameManager.gameMngInst.SetStringColor("Unselect TRIGGERED !", Color.red));
    }

    void SetBuildingColor(bool isSelected)
    {
        Color col = SelectedColor;

        float randomColorRange = 0.05f;

        if (isSelected == false)
        {
            col = UnselectedColor;

            if (RandomColor_Set == false)
            {
                col = col + new Color(Random.Range(-randomColorRange, randomColorRange), Random.Range(-randomColorRange, randomColorRange), Random.Range(-randomColorRange, randomColorRange));
                UnselectedColor = col;
                RandomColor_Set = true;
            }
        }

        if (Building_Base != null)
        {
            if (Building_Base.GetComponent<Renderer>() != null)
                Building_Base.GetComponent<Renderer>().material.SetColor("_BaseColor", col);
            else
            {
                Renderer[] renderers = Building_Base.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                {
                    r.material.SetColor("_BaseColor", col);
                }
            }
        }

        if (Building_Top != null)
        {
            if (Building_Top.GetComponent<Renderer>() != null)
                Building_Top.GetComponent<Renderer>().material.SetColor("_BaseColor", col);
            else
            {
                Renderer[] renderers = Building_Top.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                {
                    if (r.gameObject.name.Contains("SM_Bld") || r.gameObject.name.Contains("TB_CITY"))
                        r.material.SetColor("_BaseColor", col);
                }
            }
        }

        if (Floors_Arrays != null)
        {
            for (int x = 0; x < Floors_Arrays.Length; x++)
            {
                if (Floors_Arrays[x].GetComponent<Renderer>() != null)
                    Floors_Arrays[x].GetComponent<Renderer>().material.SetColor("_BaseColor", col);
                else
                {
                    Renderer[] renderers = Floors_Arrays[x].GetComponentsInChildren<Renderer>();
                    foreach (var r in renderers)
                    {
                        r.material.SetColor("_BaseColor", col);
                    }
                }
            }
        }
    }

    public void Set_Day_Mat(bool isNight)
    {
        if (IsTB)
        {
            Material mat = DayMat;
            if (isNight)
                mat = NightMat;

            if (Building_Base != null)
            {
                if (Building_Base.GetComponent<Renderer>() != null)
                    Building_Base.GetComponent<Renderer>().material = mat;
                else
                {
                    Renderer[] renderers = Building_Base.GetComponentsInChildren<Renderer>();
                    foreach (var r in renderers)
                    {
                        r.material = mat;
                    }
                }
            }

            if (Building_Top != null)
            {
                if (Building_Top.GetComponent<Renderer>() != null)
                    Building_Top.GetComponent<Renderer>().material = mat;
                else
                {
                    Renderer[] renderers = Building_Top.GetComponentsInChildren<Renderer>();
                    foreach (var r in renderers)
                    {
                        if (r.gameObject.name.Contains("SM_Bld") || r.gameObject.name.Contains("TB_CITY"))
                            r.material = mat;
                    }
                }
            }

            if (Floors_Arrays != null)
            {
                for (int x = 0; x < Floors_Arrays.Length; x++)
                {
                    if (Floors_Arrays[x].GetComponent<Renderer>() != null)
                        Floors_Arrays[x].GetComponent<Renderer>().material = mat;
                    else
                    {
                        Renderer[] renderers = Floors_Arrays[x].GetComponentsInChildren<Renderer>();
                        foreach (var r in renderers)
                        {
                            r.material = mat;
                        }
                    }
                }
            }

            SetBuildingColor(IsSelected);
        }
    }

    public void ShowInfo()
    {
        //Name & Info
        //if (LookAt_base.activeInHierarchy == false)
        //{
            LookAt_base.SetActive(true);

            Text3D_Name.text = Owner_Name;
            Text3D_Info.text = "XRP: " + Get_Total_Building_Value().ToString();

            LookAt_base.transform.localScale = new Vector3(1f / transform.localScale.x, 1f, 1f / transform.localScale.z);

            if (Selected)
                GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().SetInfo();
        //}
    }

    public void ShowMessage()
    {
        //GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().TextMessage.text = Text3D_Name.text + "\n" + Text3D_Info.text + "\n" + Text3D_Custom.text;
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().Image_PlayerData.SetActive(true);
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().Text_PlayerName.text = Text3D_Name.text;
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().Text_XRP.text = Text3D_Info.text;
    }

    public void ResetMessage()
    {
        //GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().TextMessage.text = "";
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().Image_PlayerData.SetActive(false);
    }

    public void Set_Custom_Text(string str)
    {
        Text3D_Custom.text = str;
    }

    public string Get_Custom_Text()
    {
        return Text3D_Custom.text;
    }

    public void HideInfo()
    {
        if (LookAt_base.activeInHierarchy)
            LookAt_base.SetActive(false);
    }

    public void Set_Penthouse_Element(int collectable)
    {
        Top_Point.GetComponent<Penthouse>().Set_Colectables(collectable);
    }

    public void Set_Bottom_Element(int collectable)
    {
        Bottom_Point.GetComponent<Penthouse>().Set_Colectables(collectable);
    }

    public int Get_Total_Building_Value()
    {
        int xrp = XRP_Total - Top_Point.GetComponent<Penthouse>().GetValue() - Bottom_Point.GetComponent<Penthouse>().GetValue() - XRP_Building_Value;

        return xrp;
    }
}
