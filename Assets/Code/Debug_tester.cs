using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_tester : MonoBehaviour
{
    [SerializeField] TextMesh TextDistance = null;
    [SerializeField] TextMesh TextHitVector = null;
    [SerializeField] GameObject OtherObjRef = null;
    [SerializeField] string LayerName = "Default";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Distance measure
        if (TextDistance != null)
            TextDistance.text = "Dist to " + OtherObjRef.name + " = " + Vector3.Distance(transform.position, OtherObjRef.transform.position);

        //Hit Vector Tester
        if (TextHitVector != null)
            TextHitVector.text = Check_If_Populate_Possible();

        Debug.Log(TextDistance.text + "  >*  " + TextHitVector.text);
    }

    string Check_If_Populate_Possible(string thisLayer = "Default", bool alignRotation = false)
    {
        //make platform adjust terrain rotation
        RaycastHit rcHit;

        //Make raycast direction down
        Vector3 theRay = transform.TransformDirection(Vector3.down);

        //Get LayerMask for desired Layer
        LayerMask mask = LayerMask.GetMask(thisLayer);

        string str = "No Hit";

        if (Physics.Raycast(transform.position /*- MoveDown*/, theRay, out rcHit, Mathf.Infinity, mask))
        {
            //this is for getting distance from object to the ground
            var GroundDis = rcHit.distance;

            //with this you rotate object to adjust with terrain
            if (alignRotation)
                transform.rotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);

            ////finally, this is for putting object IN the ground
            //float y = (transform.localPosition.y - GroundDis)/* + 1*/;
            //transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if (alignRotation)
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, Random.Range(-180f, 180f));

            str = "obj = " + rcHit.collider.name + "  > dist = " + rcHit.distance.ToString("#.##") + "  > rot = " + Quaternion.FromToRotation(Vector3.up, rcHit.normal).ToString();
        }

        return str;
    }
}
