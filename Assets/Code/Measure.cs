using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : MonoBehaviour
{
    //**MANAGE Object Dimensions*//
    //**Can be ATTACHED TO any 3D object*//
    /*NOTES:
     * 
     */


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bounds = " + GetObject_Bounds(transform));
        Debug.Log("Y = " + GetObject_Height(this.gameObject));
    }


    // MEASURE GAME OBJECT
    public static Bounds GetObject_Bounds(Transform objectTransform)
    {
        return GetTotalMeshFilterBounds(objectTransform);
    }

    public static float GetObject_Height(GameObject obj)
    {
        Bounds bounds = GetTotalMeshFilterBounds(obj.transform);

        //Debug.Log(obj.name + " height = " + bounds.extents.y + "  > bounds.extents = " + bounds.extents + "  > bounds = " + bounds + "  > bounds.size = " + bounds.size);

        return bounds.extents.y;
    }

    private static Bounds GetTotalMeshFilterBounds(Transform objectTransform)
    {
        var meshFilter = objectTransform.GetComponent<MeshFilter>();
        var result = meshFilter != null ? meshFilter.mesh.bounds : new Bounds();

        foreach (Transform transform in objectTransform)
        {
            var bounds = GetTotalMeshFilterBounds(transform);
            result.Encapsulate(bounds.min);
            result.Encapsulate(bounds.max);
        }
        var scaledMin = result.min;
        scaledMin.Scale(objectTransform.localScale);
        result.min = scaledMin;
        var scaledMax = result.max;
        scaledMax.Scale(objectTransform.localScale);
        result.max = scaledMax;

        //Debug.Log("Bounds = " + result);

        return result;
    }

    //Determine Building HEIGHT
    public static float GetBuildigHeight(int xrp_Sum)
    {
        return xrp_Sum;
    }
}
