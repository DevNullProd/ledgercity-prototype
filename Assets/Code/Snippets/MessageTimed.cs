using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTimed : MonoBehaviour
{
    //**MANAGE Sound & Music*//
    //**ATTACHED TO GameManager.GameObject*//
    /*NOTES:
     * 
     */


    [SerializeField] Text messageText = null;
    float Timer;
    float ScaleSpeed = 4f;
    private readonly Color defaultColor = new Color(0, 0.5f, 0);


    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > Timer)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * ScaleSpeed);
            if (transform.localScale.x < 0.1f)
                this.gameObject.SetActive(false);
        }
        else
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * ScaleSpeed);
    }

    public void SetMessage(string msg)
    {
        SetMessage(msg, defaultColor, 4);
    }

    public void SetMessage(string msg, float stayTime)
    {
        SetMessage(msg, defaultColor, stayTime);
    }

    public void SetMessage(string msg, Color col, float stayTime = 4f)
    {
        this.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;

        messageText.text = msg;
        messageText.color = col;
        //messageText.color = Color.white;

        Timer = Time.realtimeSinceStartup + stayTime;
    }
}
