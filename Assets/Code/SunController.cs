using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunController : MonoBehaviour
{
    //**MANAGE Sun*//
    //**ATTACHED TO SunBase*//
    /*NOTES:
     * 
     */

    [Header("References")]
    [SerializeField] GameObject Sunbase = null;
    [SerializeField] GameObject MainLight = null;
    [SerializeField] Text Text_TimeOfDay = null;
    [SerializeField] Slider Slider_TimeOfDay = null;
    Populate populate = null;
    [SerializeField] GameObject RainPrefab = null;
    [SerializeField] GameObject TerrainSnow = null;
    [SerializeField] ParticleSystem ParticleSnow = null;
    [SerializeField] GameObject CarSnow = null;

    [Header("Light & Fog, Color & Intensity")]
    [SerializeField] Color[] Light_Color_Day = null;
    [SerializeField] Color[] Light_Color_Night = null;
    [SerializeField] float[] Light_Color_Day_Intensity = null;
    [SerializeField] float[] Light_Color_Night_Intensity = null;

    [Header("Fog, Color & SkyBox")]
    [SerializeField] Color[] Fog_Color_Day = null;
    [SerializeField] Color[] Fog_Color_Night = null;
    [SerializeField] Material[] SkyBox_Day_Ref = null;
    [SerializeField] Material[] SkyBox_Night_Ref = null; 
    [SerializeField] Text Text_Weather = null;

    [Header("Variables")]
    bool IsNight = false;
    int Weather = -1;


    // Start is called before the first frame update
    void Start()
    {
        //Set Initial Values
        Fog_Color_Day[0] = RenderSettings.fogColor;
        Light_Color_Day[0] = MainLight.GetComponent<Light>().color;
        Light_Color_Day_Intensity[0] = MainLight.GetComponent<Light>().intensity;
        //SkyBox_Day_Ref[0] = RenderSettings.skybox;

        Update_Slider_TimeOfDay();
        Toggle_Weather();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Update_Slider_TimeOfDay()
    {
        float angle = Slider_TimeOfDay.value;

        angle = (angle - 12f) * (85f / 7f);

        Text_TimeOfDay.text = /*"Time of day: " + */((int)Slider_TimeOfDay.value).ToString() + "h";

        if (Mathf.Abs(angle) < 85)
        {
            Sunbase.transform.eulerAngles = new Vector3(angle, 0, 0);
            if (IsNight)
                Toggle_Day_Night(false);
        }
        else
        {
            if (IsNight == false)
                Toggle_Day_Night(true);
        }
        Set_Sky_Mat();
    }

    void Set_Sky_Mat()
    {
        //SkyBox
        int timeOfDay = (int)Slider_TimeOfDay.value;

        //Night
        if (timeOfDay < 3)
        {
            RenderSettings.skybox = SkyBox_Night_Ref[3];
            Debug.Log("time of Day = " + timeOfDay + "  >N3");
        }
        else if (timeOfDay < 5)
        { 
            RenderSettings.skybox = SkyBox_Night_Ref[2];
            Debug.Log("time of Day = " + timeOfDay + "  >N2");
        }
        else if (timeOfDay < 6)
        { 
            RenderSettings.skybox = SkyBox_Night_Ref[1];
            Debug.Log("time of Day = " + timeOfDay + "  >N1");
        }
        else if (timeOfDay < 7)
        { 
            RenderSettings.skybox = SkyBox_Night_Ref[0];
            Debug.Log("time of Day = " + timeOfDay + "  >N0");
        }
        //Day
        else if (timeOfDay < 10)
        { 
            RenderSettings.skybox = SkyBox_Day_Ref[1];
            Debug.Log("time of Day = " + timeOfDay + "  >D1");
        }
        else if (timeOfDay < 16)
        { 
            RenderSettings.skybox = SkyBox_Day_Ref[0];
            Debug.Log("time of Day = " + timeOfDay + "  >D0");
        }
        else if (timeOfDay < 17)
        { 
            RenderSettings.skybox = SkyBox_Day_Ref[1];
            Debug.Log("time of Day = " + timeOfDay + "  >D1");
        }
        //Night
        else if (timeOfDay < 18)
        {
            RenderSettings.skybox = SkyBox_Night_Ref[0];
            Debug.Log("time of Day = " + timeOfDay + "  >N0");
        }
        else if (timeOfDay < 19)
        {
            RenderSettings.skybox = SkyBox_Night_Ref[1];
            Debug.Log("time of Day = " + timeOfDay + "  >N1");
        }
        else if (timeOfDay < 22)
        {
            RenderSettings.skybox = SkyBox_Night_Ref[2];
            Debug.Log("time of Day = " + timeOfDay + "  >N2");
        }
        else
        {
            RenderSettings.skybox = SkyBox_Night_Ref[3];
            Debug.Log("time of Day = " + timeOfDay + "  >N3");
        }
    }

    void Toggle_Day_Night(bool isNight)
    {
        if (populate == null)
            populate = GameManager.gameMngInst.gameObject.GetComponent<Populate>();

        if (isNight)
            Set_Night();
        else
            Set_Day();
    }

    void Set_Day()
    {
        IsNight = false;

        //Light
        MainLight.GetComponent<Light>().color = Light_Color_Day[0];
        MainLight.GetComponent<Light>().intensity = Light_Color_Day_Intensity[0];
        MainLight.GetComponent<Light>().shadows = LightShadows.Hard;
        
        RenderSettings.ambientIntensity = 1;

        //Fog
        RenderSettings.fogColor = Fog_Color_Day[0];

        //enable lights
        GameManager.gameMngInst.IsNight = false;
        Set_Lights(false);
    }

    void Set_Night()
    {
        IsNight = true;

        //Light
        MainLight.GetComponent<Light>().color = Light_Color_Night[0];
        MainLight.GetComponent<Light>().intensity = Light_Color_Night_Intensity[0];
        MainLight.GetComponent<Light>().shadows = LightShadows.None;

        //SkyBox
        //RenderSettings.skybox = SkyBox_Night_Ref[0];
        RenderSettings.ambientIntensity = 1;

        //Fog
        RenderSettings.fogColor = Fog_Color_Night[0];

        //enable lights
        GameManager.gameMngInst.IsNight = true;
        Set_Lights(true);
    }

    void Set_Lights(bool isEnabled)
    {
        //Car Lights
        for (int x = 0; x < populate.Car_List.Count; x++)
        {
            if (populate.Car_List[x] != null)
                populate.Car_List[x].GetComponent<Car_Script>().SetLights(isEnabled);
        }

        //City Block Lights
        for (int x = 0; x < populate.CityBlock_List.Count; x++)
            populate.CityBlock_List[x].GetComponent<City_Block>().Set_Block_Lights(isEnabled);

        //Building Day Night
        for (int x = 0; x < populate.Building_List.Count; x++)
            populate.Building_List[x].GetComponent<Building>().Set_Day_Mat(isEnabled);
    }

    public void Toggle_Weather()
    {
        Weather++;
        if (Weather > 4)
            Weather = 0;

        Set_Weather();
    }

    public void Set_Weather()
    {
        switch (Weather)
        {
            case 0: //Sunny
                Toggle_Rain(false);
                Toggle_Thunder(false);
                Toggle_Snow(false);
                Toggle_Snowfall(false);
                Text_Weather.text = "Sunny";
                break;

            case 1: //Rain
                Toggle_Rain(true);
                Toggle_Thunder(false);
                Toggle_Snow(false);
                Toggle_Snowfall(false);
                Text_Weather.text = "Rain";
                break;

            case 2: //Rain + Thunder
                Toggle_Rain(true);
                Toggle_Thunder(true);
                Toggle_Snow(false);
                Toggle_Snowfall(false);
                Text_Weather.text = "Thunderstorm";
                break;

            case 3: //Snow
                Toggle_Rain(false);
                Toggle_Thunder(false);
                Toggle_Snow(true);
                Toggle_Snowfall(false);
                Text_Weather.text = "Winter";
                break;

            case 4: //SnowFall
                Toggle_Rain(false);
                Toggle_Thunder(false);
                Toggle_Snow(true);
                Toggle_Snowfall(true);
                Text_Weather.text = "Snowfall";
                break;
        }
    }

    void Toggle_Rain(bool isRaining)
    {
        GameObject rain = null;
        Transform[] tran = Camera.main.GetComponentsInChildren<Transform>();
        foreach (var r in tran)
        {
            if (r.name.Contains("RainPrefab"))
            {
                rain = r.gameObject;
            }
        }

        if (rain != null)
        {
            //if (isRaining == false)
                Destroy(rain);
        }

        if (isRaining == true)
        {
            rain = Instantiate(RainPrefab);
            rain.transform.SetParent(Camera.main.transform);
            rain.transform.position = Vector3.zero;
        }
    }

    void Toggle_Thunder(bool isThunder)
    {
        if (isThunder)
            GetComponent<Lightning>().enabled = true;
        else
            GetComponent<Lightning>().enabled = false;
    }

    void Toggle_Snow(bool hasSnow)
    {
        if (hasSnow)
            TerrainSnow.SetActive(true);
        else
            TerrainSnow.SetActive(false);
    }

    void Toggle_Snowfall(bool isSnowing)
    {
        GameObject snow = null;
        Transform[] tran = Camera.main.GetComponentsInChildren<Transform>();
        foreach (var r in tran)
        {
            if (r.name.Contains("ParticleSnow"))
            {
                snow = r.gameObject;
            }
        }

        if (snow != null)
        {
            if (Camera.main.name.Contains("Main Camera") == false)
                Destroy(snow);
        }

        if (isSnowing == true & Camera.main.name.Contains("Main Camera") == false)
        {
            snow = Instantiate(CarSnow);
            snow.transform.SetParent(Camera.main.transform);
            snow.transform.localPosition = new Vector3(0, 4, 5);
            snow.GetComponent<ParticleSystem>().Play();
        }

        if (isSnowing)
            ParticleSnow.Play();
        else
            ParticleSnow.Stop();
    }
}
