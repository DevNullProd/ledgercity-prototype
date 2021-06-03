using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Reader : MonoBehaviour
{
    //**MANAGE Reading Data*//
    //**ATTACHED TO Game_manager.GameObject*//
    /*NOTES:
     * 
     */


    //[Header("Variables")]

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Read_XRP_Accounts();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void Read_XRP_Accounts()
    {
        var csvData = Resources.Load<TextAsset>("xrp-accounts");
        string fileData = csvData.ToString();

        //Slit string into lines
        string[] lines = fileData.Split("\n"[0]);

        GameManager.gameMngInst.XRP_Accounts = new string[lines.Length];

        for (int x = 0; x < lines.Length; x++)
        {
            GameManager.gameMngInst.XRP_Accounts[x] = "Account " + lines[x].Replace(" ", "");
        }

        Debug.Log("GameManager.gameMngInst.XRP_Accounts.lenght = " + GameManager.gameMngInst.XRP_Accounts.Length);
    }
}
