using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Select : MonoBehaviour
{
    //**MANAGE Selecting Building*//
    //**ATTACHED TO GameManager.gameObject*//
    /*NOTES:
     * 
     */

    Building OldBuilding = null;
    Building OldBuildingHover = null;
    float SelectTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TapSelect();
    }

    void TapSelect()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Input.mousePosition.x > (Screen.width * 0.7) & Input.mousePosition.y < (Screen.height * 0.3))
        //        return;

        //    if (Input.mousePosition.x < (Screen.width * 0.2) /*& Input.mousePosition.y < (Screen.height * 0.3)*/)
        //        return;

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Debug.Log(Input.mousePosition);
        //    RaycastHit hit = new RaycastHit();

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log("Touch hit Colider = " + hit.collider.ToString());

        //        //Debug.DrawLine(ray.origin, hit.point * 10f, Color.red, 100f);
        //        Debug.Log("ray.origin = " + ray.origin + "   > hit.point = " + hit.point + "  > hit.collider" + hit.collider);

        //        if (hit.collider.ToString().Contains("Building_Top_Trigger"))
        //        {
        //            if (OldBuilding != null)
        //                OldBuilding.HideInfo();

        //            hit.collider.GetComponent<Building_Trigger>().SelectBuilding();

        //            OldBuilding = hit.collider.GetComponent<Building_Trigger>().building;
        //        }
        //    }
        //}
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.gameMngInst.CamMotion.gameObject.GetComponent<MouseInput>().MouseEnabled)
                SelectTimer = Time.realtimeSinceStartup + 0.3f;
            else
                SelectObject();
        }
        else
            Hover_Select();

        if (Input.GetMouseButtonUp(0) & Time.realtimeSinceStartup < SelectTimer)
        {
            if (GameManager.gameMngInst.CamMotion.gameObject.GetComponent<MouseInput>().MouseEnabled)
                SelectObject();
        }
    }

    void Hover_Select()
    {
        //To AVOID hitting trough UI elements
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ////Select Building
                //Debug.Log(hit.collider.gameObject.name + " Building Hovered!");

                if (hit.collider.ToString().Contains("Building_Top_Trigger"))
                {
                    if (OldBuildingHover != null && OldBuildingHover.Selected == false & OldBuildingHover.name != hit.collider.GetComponent<Building_Trigger>().building.gameObject.name)
                    {
                        OldBuildingHover.HideInfo();
                        OldBuildingHover.ResetMessage();
                    }

                    OldBuildingHover = hit.collider.GetComponent<Building_Trigger>().building;
                    OldBuildingHover.ShowInfo();
                    OldBuildingHover.ShowMessage();
                }
                else if (OldBuildingHover != null)
                    OldBuildingHover.ResetMessage();
            }
            else if (OldBuildingHover != null)
                OldBuildingHover.ResetMessage();
        }
    }

    void SelectObject()
    {
        //To AVOID hitting trough UI elements
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false & GameManager.gameMngInst.CanvasMain.GetComponent<UI_Manager>().PanelSettings.activeInHierarchy == false)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ////Select Building
                //Debug.Log(hit.collider.gameObject.name + " Building touched!");

                if (hit.collider.ToString().Contains("Building_Top_Trigger"))
                {
                    if (OldBuilding != null)
                        OldBuilding.HideInfo();

                    if (OldBuildingHover != null && OldBuildingHover.Selected == false)
                    {
                        OldBuildingHover.HideInfo();
                        OldBuilding = null;
                    }

                    hit.collider.GetComponent<Building_Trigger>().SelectBuilding();

                    OldBuilding = hit.collider.GetComponent<Building_Trigger>().building;
                }

                if (hit.collider.ToString().Contains("Car_Holder"))
                {
                    Debug.Log("Car Selected = " + hit.collider.ToString());
                    GameManager.gameMngInst.CamMotion.GetComponent<CameraMotion>().Unselect_Building();
                    GameManager.gameMngInst.CamMotion.GetComponent<CameraMotion>().Select_Building(hit.collider.gameObject);
                }
            }
        }
    }
}
