using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Populate : MonoBehaviour
{
    //**MANAGE City Population*//
    //**ATTACHED TO GameManager*//
    /*NOTES:
     * 
     */

    [Header("References")]
    [SerializeField] GameObject[] CityBlockBase_Ref = null;
    [SerializeField] GameObject[] Bridge_Ref = null;

    [SerializeField] GameObject[] City_Ref = null;
    [SerializeField] GameObject[] Local_Center_Ref = null;
    [SerializeField] Vector3[] Local_Center_Ref_StartPos = null;

    [SerializeField] Slider Slider_CitySize = null;
    [SerializeField] Text Text_City_Shape = null;

    [SerializeField] GameObject City_Lights_Ref = null;

    [Header("Building References")]
    [SerializeField] GameObject[] Building_Glass_Ref = null;
    [SerializeField] GameObject[] Building_Old_Ref = null;
    [SerializeField] GameObject[] Building_BIG_Ref = null;

    [Header("Building References TB")]
    [SerializeField] GameObject[] Building_TB_Small = null;
    [SerializeField] GameObject[] Building_TB_Medium = null;
    [SerializeField] GameObject[] Building_TB_Big = null;
    [SerializeField] GameObject[] Park_Ref = null;

    [Header("Variables")]
    Vector3 CityBlock_SizeLimits = new Vector3(300, 0, 300);
    float City_Size = 1000f;
    readonly float City_Size_Step = 500;
    readonly float Iregularity_Index = 0.75f;
    readonly float LocalShape_Index = 0.5f;
    int City_Shape_Type = -1;
    readonly int City_Shape_Type_Max = 5;


    public List<GameObject> CityBlock_List = new List<GameObject>();
    public List<GameObject> Building_List = new List<GameObject>();
    public List<GameObject> Park_List = new List<GameObject>();
    public List<GameObject> Bridge_List = new List<GameObject>();
    public List<GameObject> Car_List = new List<GameObject>();
    //public List<GameObject> Lights_List = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Define local center star pos
        Local_Center_Ref_StartPos = new Vector3[Local_Center_Ref.Length];
        for (int x = 0; x < Local_Center_Ref.Length; x++)
            Local_Center_Ref_StartPos[x] = Local_Center_Ref[x].transform.position;

        Set_City_Shape();
        Create_City();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create_City()
    {
        DeleteOldCity();

        //randomize local centers pos
        if(City_Shape_Type >= 4)
        {
            for (int x = 0; x < Local_Center_Ref.Length; x++)
            {
                //Restore old pos
                Local_Center_Ref[x].transform.position = Local_Center_Ref_StartPos[x];

                //New pos
                Local_Center_Ref[x].transform.localPosition = new Vector3(Local_Center_Ref[x].transform.localPosition.x + Random.Range(-City_Size_Step, City_Size_Step), 0,
                    Local_Center_Ref[x].transform.localPosition.z + Random.Range(-City_Size_Step, City_Size_Step));
            }
        }

        for (int x = 0; x < City_Ref.Length; x++)
            Populate_with_City_Blocks(7, 12, City_Ref[x]);

        Create_Bridges(12, City_Ref[0]);

        //Create_Lights();
    }

    //void Create_Lights()
    //{
    //    for (int x = 0; x < Local_Center_Ref.Length; x++)
    //    {
    //        GameObject obj = Instantiate(City_Lights_Ref);
    //        obj.transform.position = Local_Center_Ref[x].transform.position;
    //        obj.transform.SetParent(this.gameObject.transform);
    //        Lights_List.Add(obj);
    //    }
    //}

    void DeleteOldCity()
    {
        if (Park_List.Count > 0)
        {
            for (int x = Park_List.Count - 1; x >= 0; x--)
            {
                Destroy(Park_List[x].gameObject);
                Park_List.RemoveAt(x);
            }
        }

        if (Building_List.Count > 0)
        {
            for (int x = Building_List.Count - 1; x >= 0; x--)
            {
                Destroy(Building_List[x].gameObject);
                Building_List.RemoveAt(x);
            }
        }

        if (Car_List.Count > 0)
        {
            for (int x = Car_List.Count - 1; x >= 0; x--)
            {
                Destroy(Car_List[x].gameObject);
                Car_List.RemoveAt(x);
            }
        }

        if (Bridge_List.Count > 0)
        {
            for (int x = Bridge_List.Count - 1; x >= 0; x--)
            {
                Destroy(Bridge_List[x].gameObject);
                Bridge_List.RemoveAt(x);
            }
        }

        if (Bridge_List.Count > 0)
        {
            for (int x = Bridge_List.Count - 1; x >= 0; x--)
            {
                Destroy(Bridge_List[x].gameObject);
                Bridge_List.RemoveAt(x);
            }
        }

        if (CityBlock_List.Count > 0)
        {
            for (int x = CityBlock_List.Count - 1; x >= 0; x--)
            {
                Destroy(CityBlock_List[x].gameObject);
                CityBlock_List.RemoveAt(x);
            }
        }

        //if (Lights_List.Count > 0)
        //{
        //    for (int x = Lights_List.Count - 1; x >= 0; x--)
        //    {
        //        Destroy(Lights_List[x].gameObject);
        //        Lights_List.RemoveAt(x);
        //    }
        //}
    }

    void Populate_with_City_Blocks(int matrix_X, int matrix_Y, GameObject parent)
    {
        float step = CityBlock_SizeLimits.x * 1.1f;
        Vector3 offset = new Vector3(step * (float)matrix_X / 2f - step / 2f, 0, step * (float)matrix_Y / 2f - step / 2f);

        for (int x = 0; x < matrix_X; x++)
        {
            for (int y = 0; y < matrix_Y; y++)
            {
                Vector3 pos = new Vector3(step * (float)x, 0, step * (float)y) - offset;
                Create_CityBlock(pos, parent, x, y, step);
            }
        }
    }

    void Create_Bridges(int matrix_Y, GameObject parent)
    {
        float step = CityBlock_SizeLimits.x * 1.1f;
        Vector3 offset = new Vector3(0, 0, step * (float)matrix_Y / 2f - step / 2f);
        for (int y = 0; y < matrix_Y - 1; y++)
        {
            Vector3 pos = new Vector3(0, 0, step * (float)y) - offset + new Vector3(0, 0, 146.5f);
            GameObject obj = Instantiate(Bridge_Ref[Random.Range(0, Bridge_Ref.Length)]);
            obj.transform.position = pos;

            List<GameObject> close_Block_List = new List<GameObject>();

            //test for coast blocks
            int blockCount = 0;
            for (int x = 0; x < CityBlock_List.Count; x++)
            {
                if (Vector3.Distance(obj.transform.position, CityBlock_List[x].transform.position) < 600f)
                {
                    close_Block_List.Add(CityBlock_List[x]);
                    blockCount++;
                }
            }

            if (blockCount >= 3)
            {
                Bridge_List.Add(obj);
            }
            else
                Destroy(obj);
        }

        Check_For_Blocks_With_Two_Bridges();
        Pair_Blocks_With_Two_Bridges();
    }

    void Check_For_Blocks_With_Two_Bridges()
    {
        for (int x = 0; x < CityBlock_List.Count; x++)
        {
            if (CityBlock_List[x].GetComponent<City_Block>().IsNearRiver)
            {
                int hasBridge = 0;
                for (int y = 0; y < Bridge_List.Count; y++)
                {
                    if (Vector3.Distance(CityBlock_List[x].transform.position, Bridge_List[y].transform.position) < 600)
                        hasBridge++;
                }

                if (hasBridge == 2)
                    CityBlock_List[x].GetComponent<City_Block>().HasTwoBridges = true;
            }
        }
    }

    void Pair_Blocks_With_Two_Bridges()
    {
        for (int x = 0; x < CityBlock_List.Count; x++)
        {
            if (CityBlock_List[x].GetComponent<City_Block>().IsNearRiver & CityBlock_List[x].GetComponent<City_Block>().RiverSide == "L" & CityBlock_List[x].GetComponent<City_Block>().HasTwoBridges)
            {
                for (int y = 0; y < CityBlock_List.Count; y++)
                {
                    if (CityBlock_List[y].GetComponent<City_Block>().IsNearRiver & CityBlock_List[y].GetComponent<City_Block>().RiverSide == "R" & CityBlock_List[y].GetComponent<City_Block>().HasTwoBridges
                        & CityBlock_List[x].GetComponent<City_Block>().Ycoord == CityBlock_List[y].GetComponent<City_Block>().Ycoord)
                    {
                        CityBlock_List[x].GetComponent<City_Block>().Paired_Block = CityBlock_List[y];
                        CityBlock_List[y].GetComponent<City_Block>().Paired_Block = CityBlock_List[x];

                        CityBlock_List[x].GetComponent<City_Block>().Set_Car_Spawner();
                        CityBlock_List[y].GetComponent<City_Block>().Set_Car_Spawner();
                    }
                }
            }
        }
    }

    void Pair_Bridge_Linked_Blocks(List<GameObject> block_List)
    {

    }

    void Create_Road_Plane(int matrix_X, int matrix_Y, GameObject parent)
    {

    }

    public void Set_City_Size()
    {
        City_Size = Slider_CitySize.value * City_Size_Step;
    }

    public void Set_City_Shape()
    {
        City_Shape_Type++;
        if (City_Shape_Type > City_Shape_Type_Max)
            City_Shape_Type = 0;

        switch (City_Shape_Type)
        {
            case 0:
                Text_City_Shape.text = "Round Shape";
                break;

            case 1:
                Text_City_Shape.text = "Iregular Shape";
                break;

            case 2:
                Text_City_Shape.text = "Round Local";
                break;

            case 3:
                Text_City_Shape.text = "Iregular Local";
                break;

            case 4:
                Text_City_Shape.text = "Islands";
                break;

            case 5:
                Text_City_Shape.text = "Iregular Islands";
                break;
        }
    }

    void Create_CityBlock(Vector3 pos, GameObject parent, int xcoord, int ycoord, float step)
    {
        int cityBlock_Type = Random.Range(0, CityBlockBase_Ref.Length);
        GameObject obj = Instantiate(CityBlockBase_Ref[cityBlock_Type]);
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = pos;

        //Make City Round
        if (Shape_Condition(obj, parent.transform.position) == false)
        {
            Destroy(obj);
            return;
        }

        if (Iregularity_Condition(obj, parent.transform.position) == false)
        {
            if (Random.Range(0, 2) == 0)
            {
                Destroy(obj);
                return;
            }
        }

        Populate_CityBlock_With_Buildings(cityBlock_Type, obj);

        obj.GetComponent<City_Block>().Set_Block_Lights(GameManager.gameMngInst.IsNight, xcoord, ycoord, step);
        CityBlock_List.Add(obj);
    }

    bool Shape_Condition(GameObject obj, Vector3 pos)
    {
        bool create = true;
        float dist_from_Center = Vector3.Distance(obj.transform.position, GameManager.gameMngInst.CityCenter.transform.position);
        float dist_from_Pos = Vector3.Distance(obj.transform.position, pos);

        switch (City_Shape_Type)
        {
            case 0:
                if (dist_from_Center > City_Size)
                    create = false;
                break;

            case 1:
                if (dist_from_Center > City_Size)
                    create = false;
                break;

            case 2:
                if (dist_from_Pos > City_Size * LocalShape_Index)
                    create = false;
                break;

            case 3:
                if (dist_from_Pos > City_Size * LocalShape_Index)
                    create = false;
                break;

            case 4:
                create = Process_Local_Centers(City_Size * LocalShape_Index , obj);
                break;

            case 5:
                create = Process_Local_Centers(City_Size * LocalShape_Index, obj);
                break;
        }

        return create;
    }

    bool Iregularity_Condition(GameObject obj, Vector3 pos)
    {
        bool create = true;
        float dist_from_Center = Vector3.Distance(obj.transform.position, GameManager.gameMngInst.CityCenter.transform.position);
        float dist_from_Pos = Vector3.Distance(obj.transform.position, pos);

        switch (City_Shape_Type)
        {
            case 1:
                if (dist_from_Center > City_Size - (City_Size_Step * Iregularity_Index))
                    create = false;
                break;

            case 3:
                if (dist_from_Pos > City_Size * LocalShape_Index - (City_Size_Step * Iregularity_Index))
                    create = false;
                break;

            case 5:
                create = Process_Local_Centers(City_Size * LocalShape_Index * Iregularity_Index, obj);
                break;
        }

        return create;
    }

    bool Process_Local_Centers(float dist, GameObject obj)
    {
        bool create = false;

        for (int x = 0; x < Local_Center_Ref.Length; x++)
        {
            float localDist = Vector3.Distance(obj.transform.position, Local_Center_Ref[x].transform.position);
            if (localDist < dist)
                create = true;
        }

        ////Check if there are close blocks
        //if (create && CityBlock_List.Count > 0)
        //{
        //    bool createClose = false;
        //    for (int x = 0; x < CityBlock_List.Count; x++)
        //    {
        //        if (Vector3.Distance(obj.transform.position, CityBlock_List[x].transform.position) < City_Size_Step * 1.1f)
        //            createClose = true;
        //    }
        //    create = createClose;
        //}

        return create;
    }

    void Populate_CityBlock_With_Buildings(int blockType, GameObject cityBlock)
    {
        switch (blockType)
        {
            case 0:
                Populate_CityBlock_WithBuildings(Random.Range(3, 5), cityBlock, Building_Glass_Ref);
                break;

            case 1:
                Populate_CityBlock_WithBuildings(Random.Range(4, 6), cityBlock, Building_Glass_Ref);
                break;

            case 2:
                Populate_CityBlock_WithBuildings(Random.Range(4, 6), cityBlock, Building_Old_Ref);
                break;

            case 3:
                Populate_CityBlock_WithBuildings(1, cityBlock, Building_BIG_Ref);
                break;

            case 4:
                Populate_CityBlock_WithBuildings(Random.Range(4, 6), cityBlock, Building_TB_Small);
                break;

            case 5:
                Populate_CityBlock_WithBuildings(Random.Range(3, 5), cityBlock, Building_TB_Medium);
                break;

            case 6:
                Populate_CityBlock_WithBuildings(Random.Range(2, 4), cityBlock, Building_TB_Big);
                break;
        }
    }

    void Populate_CityBlock_WithBuildings(int buildings_In_a_Row, GameObject cityBlock, GameObject[] objRef)
    {
        if (buildings_In_a_Row == 1)
        {
            Place_BIG_Building(cityBlock, objRef);
            return;
        }

        int totalBuildings = buildings_In_a_Row * 2 + (buildings_In_a_Row - 1) * 2;
        float step = CityBlock_SizeLimits.x / (float)(buildings_In_a_Row + 1) * 1.2f;
        Vector3 offset = new Vector3(step * (float)buildings_In_a_Row / 2f - step / 2f, 0, step * (float)buildings_In_a_Row / 2f - step / 2f);

        //GameObject[] buildArray = new GameObject[totalBuildings];

        //Diceides if there is a PARK in center of the block
        bool havePark = true;
        if (Random.Range(0, 3) == 0)
            havePark = false;
        else
            PlacePark(cityBlock);

        for (int x = 0; x < buildings_In_a_Row; x++)
        {
            for (int y = 0; y < buildings_In_a_Row; y++)
            {
                if (havePark)
                {
                    if (x == 0 || x == buildings_In_a_Row - 1)
                    {
                        Place_Building(x, y, step, offset, cityBlock, objRef);
                    }
                    else
                    {
                        if (y == 0 || y == buildings_In_a_Row - 1)
                        {
                            Place_Building(x, y, step, offset, cityBlock, objRef);
                        }
                    }
                }
                else
                    Place_Building(x, y, step, offset, cityBlock, objRef);
            }
        }
    }

    void Place_Building(int x, int y, float step, Vector3 offset, GameObject cityBlock, GameObject[] objRef)
    {
        GameObject obj = Instantiate(objRef[Random.Range(0, objRef.Length)]);
        obj.transform.SetParent(cityBlock.transform);

        Vector3 pos = new Vector3(step * x, 0, step * y) - offset;
        obj.transform.localPosition = pos;

        Building_List.Add(obj);
    }

    void Place_BIG_Building(GameObject cityBlock, GameObject[] objRef)
    {
        GameObject obj = Instantiate(objRef[Random.Range(0, objRef.Length)]);
        obj.transform.SetParent(cityBlock.transform);

        obj.transform.localPosition = Vector3.zero;

        Building_List.Add(obj);
    }

    void PlacePark(GameObject cityBlock)
    {
        GameObject obj = Instantiate(Park_Ref[Random.Range(0, Park_Ref.Length)]);
        obj.transform.SetParent(cityBlock.transform);
        
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = obj.transform.localScale * 1.6f;

        Park_List.Add(obj);
    }
}
