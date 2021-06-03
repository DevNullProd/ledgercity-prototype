using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_Block : MonoBehaviour
{
    [SerializeField] GameObject Block_Lights = null;
    public Car_Spawner car_Spawner = null;

    public int Xcoord = 0;
    public int Ycoord = 0;
    public bool IsNearRiver = false;
    public string RiverSide = "";
    public bool HasTwoBridges = false;

    public GameObject Paired_Block = null;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void Set_Block_Lights(bool isEnabled, int xcoord = -1, int ycoord = -1, float step = 0)
    {
        Block_Lights.SetActive(isEnabled);

        if (xcoord >= 0)
            Xcoord = xcoord;

        if (ycoord >= 0)
            Ycoord = ycoord;

        if (step > 100 & Mathf.Abs(transform.position.x) < step * 1.9f)
        {
            IsNearRiver = true;
            if (transform.position.x > 100f)
                RiverSide = "R";
            else
                RiverSide = "L";
        }
    }

    public void Set_Car_Spawner()
    {
        if (RiverSide == "L")
        {
            car_Spawner.WayPoints[2] = Paired_Block.GetComponent<City_Block>().car_Spawner.WayPoints[2];
            car_Spawner.WayPoints[3] = Paired_Block.GetComponent<City_Block>().car_Spawner.WayPoints[3];
        }
        else
        {
            car_Spawner.WayPoints[0] = Paired_Block.GetComponent<City_Block>().car_Spawner.WayPoints[0];
            car_Spawner.WayPoints[1] = Paired_Block.GetComponent<City_Block>().car_Spawner.WayPoints[1];
        }
    }
}
