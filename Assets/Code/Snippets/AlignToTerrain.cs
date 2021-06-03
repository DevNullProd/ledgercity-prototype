using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToTerrain : MonoBehaviour
{
    //**MANAGE Alligning to Terrain*//
    //**ATTACHED TO GameObject*//
    /*NOTES:
     * 
     */

    Vector3 SpawnAngle;
    public bool AlignRotation = true;
    [SerializeField] float ShiftY = 0;
    SceneCreator sceneCreator = null;

    void Start()
    {
        sceneCreator = GameManager.gameMngInst.GetComponent<SceneCreator>();
        SpawnAngle = transform.eulerAngles;

        if (this.gameObject.layer != 11)
            this.gameObject.layer = 10;

        DoAlign();
    }

    //void Update()
    //{
        
    //}

    public void DoAlign()
    {
        //make platform adjust terrain rotation
        RaycastHit rcHit;

        //Make raycast direction down
        Vector3 theRay = transform.TransformDirection(Vector3.down);

        //Get LayerMask for desired Layer
        LayerMask mask = LayerMask.GetMask("Default");

//        //DEBUG
//#if UNITY_EDITOR
//        if (Physics.Raycast(transform.position - new Vector3(0, 1, 0), Vector3.down, out RaycastHit hit))
//        {
//            if (hit.collider.gameObject.tag == "Terrain")
//            {
//                Debug.DrawRay(transform.position, Vector3.down, Color.green);
//                Debug.Log("Hit.point = " + hit.point);
//            }
//        }
//#endif

//        //fix bug which appear in stairs from time to time
//        Vector3 MoveDown = Vector3.zero;
//        if (name.Contains("Stairs"))
//            MoveDown = new Vector3(0, 1, 0);

        if (Physics.Raycast(transform.position /*- MoveDown*/, theRay, out rcHit, Mathf.Infinity, mask))
        {
            //this is for getting distance from object to the ground
            var GroundDis = rcHit.distance;

            //with this you rotate object to adjust with terrain
            if (AlignRotation)
                transform.rotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);

            //finally, this is for putting object IN the ground
            float y = (transform.localPosition.y - GroundDis)/* + 1*/;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if (AlignRotation)
                transform.eulerAngles = transform.eulerAngles + SpawnAngle; 

            if (ShiftY != 0)
                transform.position = new Vector3(transform.position.x, y + ShiftY, transform.position.z);

            if (this.gameObject.layer != 11 && this.gameObject.name.Contains("Stairs") == false & this.gameObject.name.Contains("Prop01") == false)
            {
                //Destroy All objects which are not on grass to awoid covering road, water, stone, etc
                if (GameManager.gameMngInst.GetComponent<TerrainManager>().GetTerrainAtPosition(transform.position) > 0)
                    this.gameObject.SetActive(false);
                else
                {
                    ////Destroy all objects which obstacle PowerUPs
                    //for (int x = 0; x < sceneCreator.PowerUPs_Array.Length; x++)
                    //{
                    //    if (Vector3.Distance(transform.position, sceneCreator.PowerUPs_Array[x].transform.position) < 3)
                    //        this.gameObject.SetActive(false);
                    //}
                }
            }

            //if (name.Contains("Stairs"))
            //    GetComponent<BoxCollider>().enabled = true;

            if (rcHit.collider.name != "Terrain2")
                Debug.Log("Terain Layer = "  + GameManager.gameMngInst.GetComponent<TerrainManager>().GetTerrainAtPosition(transform.position) + "  > LayerHit = " + rcHit.collider.gameObject.layer
                    + "  > rcHit.point = " + rcHit.point + "  > rcHit.distance = " + rcHit.distance + "  > rcHit.colider.name = " + rcHit.collider.name);
        }
    }
}
