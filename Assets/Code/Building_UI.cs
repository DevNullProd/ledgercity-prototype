using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_UI : MonoBehaviour
{
    //**MANAGE Building UI*//
    //**Attached to CanvasBuilding*//
    /*NOTES:
     * 
     */

    [Header("References")]
    [SerializeField] Text Text_Info = null;

    [SerializeField] Button[] Buttons_Top = null;
    [SerializeField] Button[] Buttons_Bottom = null;

    public GameObject Image_PlayerData = null;
    public Text Text_PlayerName = null;
    public Text Text_XRP = null;

    //[SerializeField] Button Button_Flag = null;
    //[SerializeField] Button Button_Statue= null;
    //[SerializeField] Button Button_Billboard = null;

    [SerializeField] InputField InputField_CustomText = null;
    [SerializeField] Text Placeholder_CustomText = null;
    [SerializeField] Text Text_CustomText = null;

    [Header("Height Slider")]
    [SerializeField] Slider Slider_Height = null;
    float Slider_OldValue;
    bool SliderSet = false;

    Building building = null;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetBuildingUI(Building building_script)
    {
        SliderSet = false;
        building = building_script;

        SetInfo();

        if (building.IsEditable)
        {
            Slider_Height.gameObject.SetActive(true);
            SetSlider();
        }
        else
            Slider_Height.gameObject.SetActive(false);

        Set_Button_Top_Color();
        Set_Button_Bottom_Color();
        InputField_CustomText.text = building.Get_Custom_Text();
        building.Set_Custom_Text(InputField_CustomText.text);
    }

    void SetSlider()
    {
        Slider_Height.minValue = building.Min_Height;
        Slider_Height.maxValue = building.Max_Height;
        Slider_Height.value = building.Height_Actual;
        Slider_OldValue = building.Height_Actual;
        SliderSet = true;
    }

    public void Process_Slider()
    {
        if (SliderSet && Mathf.Abs(Slider_OldValue - Slider_Height.value) > 5)
        {
            building.Create_Building_Again((int)Slider_Height.value);
            building.ShowInfo();
            SetInfo();
        }
    }

    public void HideCanvas()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SliderSet = false;
    }

    public void SetInfo()
    {
        Text_Info.text = /*"Owned By " +*/ building.Owner_Name + "\nXRP: " + building.Get_Total_Building_Value().ToString() + "\nHeight: " + building.Height_Actual.ToString("#.##") + "m";

        Text_PlayerName.text = building.Owner_Name;
        Text_XRP.text = "XRP: " + building.Get_Total_Building_Value().ToString();
    }

    public void Button_Collectable_Top_Pressed(int collectable)
    {
        if (building.Top_Point.GetComponent<Penthouse>().Colectables[collectable].activeInHierarchy)
        {
            building.Top_Point.GetComponent<Penthouse>().Set_Colectables(collectable);
            //building.XRP_Total += 100;
        }
        else
        { 
            building.Top_Point.GetComponent<Penthouse>().Colectables[collectable].SetActive(true);
            //building.XRP_Total -= 100;
        }

        SetInfo();
        building.ShowInfo();

        Set_Button_Top_Color();
    }

    public void Button_Collectable_Bottom_Pressed(int collectable)
    {
        if (building.Bottom_Point.GetComponent<Penthouse>().Colectables[collectable].activeInHierarchy)
        {
            building.Bottom_Point.GetComponent<Penthouse>().Set_Colectables(collectable);
            //building.XRP_Total += 100;
        }
        else
        {
            building.Bottom_Point.GetComponent<Penthouse>().Colectables[collectable].SetActive(true);
            //building.XRP_Total -= 100;
        }

        building.ShowInfo();

        Set_Button_Bottom_Color();
    }

    public void Set_Button_Top_Color()
    {
        if (building == null)
            return;

        for (int x  = 0; x < Buttons_Top.Length; x++)
        {
            if (building.Top_Point.GetComponent<Penthouse>().Colectables[x].activeInHierarchy)
                Buttons_Top[x].GetComponent<Image>().color = new Color(1f, 0.5f, 1f);
            else
                Buttons_Top[x].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        //if (building.Top_Point.GetComponent<Penthouse>().Colectables[0].activeInHierarchy)
        //    Button_Flag.GetComponent<Image>().color = new Color(0.7f, 1f, 0.7f);
        //else
        //    Button_Flag.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        //if (building.Top_Point.GetComponent<Penthouse>().Colectables[1].activeInHierarchy)
        //    Button_Statue.GetComponent<Image>().color = new Color(0.7f, 1f, 0.7f);
        //else
        //    Button_Statue.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        //if (building.Top_Point.GetComponent<Penthouse>().Colectables[2].activeInHierarchy)
        //    Button_Billboard.GetComponent<Image>().color = new Color(0.7f, 1f, 0.7f);
        //else
        //    Button_Billboard.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void Set_Button_Bottom_Color()
    {
        if (building == null)
            return;

        for (int x = 0; x < Buttons_Bottom.Length; x++)
        {
            if (building.Bottom_Point.GetComponent<Penthouse>().Colectables[x].activeInHierarchy)
                Buttons_Bottom[x].GetComponent<Image>().color = new Color(1f, 0.5f, 1f);
            else
                Buttons_Bottom[x].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }

    public void Process_Custom_Text()
    {
        if (InputField_CustomText.text != "Custom text")
            building.Set_Custom_Text(InputField_CustomText.text);
    }
}
