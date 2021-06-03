using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCreator : MonoBehaviour
{
    //**MANAGE Populating Scene*//
    //**ATTACHED TO Scene_Creator.GameObject*//
    /*NOTES:
     * Call with SceneCreator.SceneCreatorInst
     */

    [Header("References")]
    public static SceneCreator SceneCreatorInst;

    void Awake()
    {
        SceneCreatorInst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
