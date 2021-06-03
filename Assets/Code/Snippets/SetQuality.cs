using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SetQuality : MonoBehaviour
{
    //**MANAGE GRAPHICS QUALITY*//
    //**ATTACHED TO GameManager*//
    /*NOTES:
     * 
     */

    [Header("References")]
    //Camera
    [SerializeField] Camera MainCam = null;
    //POST PROCESSING
    [SerializeField] GameObject PostProcessingVolume = null;
    //RENDER SETTINGS
    [SerializeField] RenderPipelineAsset[] RPA = null;
    public int RenderPipelineAsset_Selected = 0;
    //UI_Manager
    UI_Manager uiManager;

    [Header("Variables")]
    [SerializeField] int Fps_High = 20;
    [SerializeField] int Fps_Middle = 15;
    [SerializeField] int Fps_Low = 10;

    [Header("Debug")]
    [SerializeField] Text TextInputQuality = null;
    public bool DisableAutoQuality = false;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>();
        QualitySettings.SetQualityLevel(3);

        ////Debug
        //uiManager.DebugQuality = "q" + QualitySettings.GetQualityLevel() + " rpa" + RenderPipelineAsset_Selected + " pp" + PostProcessingVolume.activeInHierarchy;
    }

    // Update is called once per frame
    void Update()
    {
        CameraManagement();

        if (Time.realtimeSinceStartup > 120f)
            this.enabled = false;
    }

    //----------------------------------------------------------------------------------------------------------------------



    #region CAMERA
    //CAMERA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public float MeasuredFPS;

    //BookMark Camera management
    void CameraManagement()
    {
        //Measure average FPS
        MeasuredFPS = MeasureFPS();
        //uiManager.MeasuredFps = MeasuredFPS;

        ////Debug
        //MeasuredFPS = 8;
        if (DisableAutoQuality)
            return;

        if (GameTimeForFPS_measure < 4)
            return;

        if (MeasuredFPS > Fps_High + 5)
        {
            SetHighQuality();
        }
        else if (MeasuredFPS < Fps_High)
        {
            LowerQuality();
        }
    }

    void LowerQuality()
    {
        ResetFPS();

        if (RenderPipelineAsset_Selected == 0)
        {
            SetRenderScale(0.5f);
            Debug.Log(GameManager.gameMngInst.SetStringColor("Set RENDER HALF, RenderPipelineAsset_Selected = " + RenderPipelineAsset_Selected, Color.red));
        }
        else
        {
            if (RenderPipelineAsset_Selected == 1)
            {
                SetRenderScale(0.3f);
                Debug.Log(GameManager.gameMngInst.SetStringColor("Set RENDER 1/3, RenderPipelineAsset_Selected = " + RenderPipelineAsset_Selected, Color.red));
            }
            else if (PostProcessingVolume.activeInHierarchy & MeasuredFPS < Fps_Middle)
            {
                //SetPostProcessing();
                //Debug.Log(GameManager.gameMngInst.SetStringColor("DISABLE POST, RenderPipelineAsset_Selected = " + RenderPipelineAsset_Selected, Color.red));
            }
        }
    }

    int RevertToHighQuality = 0;
    void SetHighQuality()
    {
        ResetFPS();

        if (RenderPipelineAsset_Selected > 0 & RevertToHighQuality > 5)
        {
            SetRenderScale(1f);
            RevertToHighQuality = 0;
        }

        RevertToHighQuality++;
    }

    int NumOfFrames;
    float GameTimeForFPS_measure;
    float MeasureFPS()
    {
        if (Time.realtimeSinceStartup < 7)
            return 30;

        if (GameTimeForFPS_measure > 6)
        {
            ResetFPS();
        }

        NumOfFrames++;
        GameTimeForFPS_measure += Time.deltaTime;

        //return 10;
        return (float)NumOfFrames / GameTimeForFPS_measure;
    }

    void ResetFPS()
    {
        NumOfFrames = 0;
        GameTimeForFPS_measure = 0;

        ////Debug
        //uiManager.DebugQuality = "q" + QualitySettings.GetQualityLevel() + " rpa" + RenderPipelineAsset_Selected + " pp" + PostProcessingVolume.activeInHierarchy;
    }

    public void SetRenderScale(float scale = 1f)
    {
        if (scale > 0.9f)
        {
            GraphicsSettings.renderPipelineAsset = RPA[0];
            RenderPipelineAsset_Selected = 0;
        }
        else if (scale > 0.4f)
        {
            GraphicsSettings.renderPipelineAsset = RPA[1];
            RenderPipelineAsset_Selected = 1;
        }
        else
        {
            GraphicsSettings.renderPipelineAsset = RPA[2];
            RenderPipelineAsset_Selected = 2;
        }


        uiManager.Refresh_Quality_Text();
    }

    public void SetPostProcessing(float scale = 1f)
    {
        if (PostProcessingVolume.activeInHierarchy)
            PostProcessingVolume.SetActive(false);
        else
            PostProcessingVolume.SetActive(true);
    }

    public void ManualQualitySet()
    {
        int qual = ParseNumber(TextInputQuality.text);

        if (qual > 5)
            qual = 5;

        if (qual < 0)
            qual = 0;

        QualitySettings.SetQualityLevel(qual);
        Debug.Log("Manual Quality Set to " + qual);
    }

    public void Debug_DisableAutoQuality()
    {
        DisableAutoQuality = !DisableAutoQuality;
    }

    int ParseNumber(string str)
    {
        int val = 0;
        int.TryParse(str, out val);
        return val;
    }

    public void Manual_Quality_Set(int val)
    {
        switch(val)
        {
            case 0:
                GraphicsSettings.renderPipelineAsset = RPA[0];
                RenderPipelineAsset_Selected = 0;
                break;

            case 1:
                GraphicsSettings.renderPipelineAsset = RPA[1];
                RenderPipelineAsset_Selected = 1;
                break;

            case 2:
                GraphicsSettings.renderPipelineAsset = RPA[2];
                RenderPipelineAsset_Selected = 2;
                break;
        }


        uiManager.Refresh_Quality_Text();
    }

    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    //END OF CAMERA XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    //----------------------------------------------------------------------------------------------------------------------
}
