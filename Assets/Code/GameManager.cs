using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //**MANAGE Vasrious aspects of the game*//
    //**ATTACHED TO Game_manager.GameObject*//
    /*NOTES:
     * Call with GameManager.gameMngInst
     */

    [Header("References")]
    public static GameManager gameMngInst;
    public GameObject CityCenter = null;
    public GameObject CanvasMain = null;

    public GameObject CanvasBuilding = null;
    public CameraMotion CamMotion = null;
    public SunController sunController = null;

    [Header("Variables")]
    public int Building_No = 0;
    public bool IsNight = false;
    public string[] XRP_Accounts = null;

    [Header("Building")]
    readonly float Min_Distance_From_City_Center = 300f;
    readonly float Max_Distance_From_City_Center = 3000f;
    readonly float Max_Distance_Reduction = 150f;
    public float Building_Height_Distance_Reduction;

    void Awake()
    {
        gameMngInst = this;
        CanvasMain = GameObject.Find("CanvasMain");
    }

    // Start is called before the first frame update
    void Start()
    {
        CanvasBuilding = GameObject.Find("CanvasBuilding");
        CamMotion = GameObject.Find("CameraBase").GetComponent<CameraMotion>();
        CityCenter = GameObject.Find("CityCenter");
        sunController = GameObject.Find("SunBase").GetComponent<SunController>();

        CanvasBuilding.SetActive(false);
        GameManager.gameMngInst.CanvasBuilding.GetComponent<Building_UI>().Image_PlayerData.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //----------------------------------------------------------------------------------------------------------------------



    #region Service Routines
    //SERVICE ROUTINES XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    //Calculate distance variation
    public float Calculate_Distance_Reduction(float dist, float minHeight, float maxHeight)
    {
        float rangeCityLimits = Max_Distance_From_City_Center - Min_Distance_From_City_Center;
        float rangeBuildingHeight = maxHeight - minHeight;
        dist = dist - Min_Distance_From_City_Center;

        float result = Mathf.Abs(dist / rangeCityLimits) * Max_Distance_Reduction;

        if (result < 30f || dist < rangeCityLimits / 3f)
            result = 0f;
        else if (result > rangeBuildingHeight * 0.8f)
            result = result / 2f;

        //Debug.Log(result);

        return result;
    }

    // Set String Color
    public string SetStringColor(string str, Color col)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(col) + ">" + str + "</color>";
    }

    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    //END OF SERVICE ROUTINES XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    //----------------------------------------------------------------------------------------------------------------------
}
