using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penthouse : MonoBehaviour
{
    //**MANAGE Collectables*//
    //**ATTACHED TO Top_Point*//
    /*NOTES:
     * 
     */

    public GameObject Penthouse_Base = null;
    public GameObject[] Colectables = null;

    private void Awake()
    {
        for (int x = 0; x < Colectables.Length; x++)
            Colectables[x].SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_Colectables(int toggle_selectable)
    {
        if (toggle_selectable < Colectables.Length)
        {
            if (Colectables[toggle_selectable].activeInHierarchy)
                Colectables[toggle_selectable].SetActive(false);
            else
            {
                Colectables[toggle_selectable].SetActive(true);
                //Colectables[toggle_selectable].transform.localPosition = new Vector3(Colectables[toggle_selectable].transform.localPosition.x / transform.localScale.x,
                //    Colectables[toggle_selectable].transform.localPosition.y / transform.localScale.y,
                //    Colectables[toggle_selectable].transform.localPosition.z / transform.localScale.z);
            }
        }
    }

    public int GetValue()
    {
        int val = 0;

        for (int x = 0; x < Colectables.Length; x++)
        {
            if (Colectables[x].activeInHierarchy)
                val += 100;
        }

        return val;
    }
}
