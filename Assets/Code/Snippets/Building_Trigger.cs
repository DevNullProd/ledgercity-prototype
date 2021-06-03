using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Trigger : MonoBehaviour
{
    //**MANAGE Selecting Building*//
    //**ATTACHED TO CubeTrigger on top of the Building*//
    /*NOTES:
     * 
     */

    public Building building = null;
    Building Oldbuilding = null;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Touched");
    //}

    public void SelectBuilding()
    {
        building.Select_Building();
        building.ShowInfo();
    }

    public void HideInfo()
    {
        building.HideInfo();
    }
}
