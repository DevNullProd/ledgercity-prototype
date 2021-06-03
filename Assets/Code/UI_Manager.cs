using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    //**MANAGE UI*//
    //**ATTACHED TO CanvasMain*//
    /*NOTES:
     * 
     */

    [Header("References")]
    public GameObject PanelSettings = null;
    [SerializeField] Slider Slider_Quality = null;
    [SerializeField] Text Text_Quality = null;
    [SerializeField] Slider Slider_Fog = null;
    [SerializeField] Text Text_Fog = null;
    [SerializeField] Button Button_Quality = null;
    [SerializeField] Text Text_Quality_Button = null;
    public Text TextMessage = null;


    // Start is called before the first frame update
    void Start()
    {
        //slider fog
        Slider_Fog.minValue = 1000f;
        Slider_Fog.maxValue = 6000f;
        Slider_Fog.value = 6000f;
        SetFog();

        Hide_PanelSettings();
        SetFog();

    }

    // Update is called once per frame
    void Update()
    {
        Input.GetAxis("Mouse ScrollWheel");
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //Settings
    public void Toggle_PanelSettings()
    {
        if (PanelSettings.activeInHierarchy == false)
            Show_PanelSettings();
        else
            Hide_PanelSettings();
    }

    public void Show_PanelSettings()
    {
        PanelSettings.SetActive(true);
        Refresh_Quality_Text();
    }

    public void Hide_PanelSettings()
    {
        PanelSettings.SetActive(false);
    }

    //Fog
    public void SetFog()
    {
        float val = Slider_Fog.value;

        RenderSettings.fogEndDistance = val;
        RenderSettings.fogStartDistance = val / 2f;
        Camera.main.farClipPlane = val + 500f;

        //Text_Fog.text = "Fog distance " + ((int)val).ToString() + "m";
    }

    //Quality
    public void Toggle_Quality()
    {
        GameManager.gameMngInst.gameObject.GetComponent<SetQuality>().Debug_DisableAutoQuality();

        Refresh_Quality_Text();
    }

    public void SetQuality()
    {
        int val = (int)Slider_Quality.value;

        GameManager.gameMngInst.gameObject.GetComponent<SetQuality>().Manual_Quality_Set(val);

        Refresh_Quality_Text();
    }

    public void Refresh_Quality_Text()
    {
        if (GameManager.gameMngInst.gameObject.GetComponent<SetQuality>().DisableAutoQuality)
            Text_Quality_Button.text = "MANUAL Quality";
        else
            Text_Quality_Button.text = "Auto Quality";

        switch (GameManager.gameMngInst.gameObject.GetComponent<SetQuality>().RenderPipelineAsset_Selected)
        {
            //case 0:
            //    Text_Quality.text = "High Quality";
            //    break;

            //case 1:
            //    Text_Quality.text = "Medium Quality";
            //    break;

            //case 2:
            //    Text_Quality.text = "Low Quality";
            //    break;
        }

        Slider_Quality.value = GameManager.gameMngInst.gameObject.GetComponent<SetQuality>().RenderPipelineAsset_Selected;
    }
}
