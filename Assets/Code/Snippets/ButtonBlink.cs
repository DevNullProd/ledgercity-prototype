using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlink : MonoBehaviour {

    public bool DoNotChangeColor = false;

    Vector2 OldScale;
    Vector2 NewScale;
    Vector2 TmpScale;

    Color OldColor;
    Color NewColor;
    Color TmpColor;
    [SerializeField] Vector2 DefaultScale;
    [SerializeField] bool StartImediatelly = false;
    [SerializeField] bool DoNotScale = false;


    Color OldTextColor;
    bool HaveText = false;

    bool switcher = false;

    bool IsButton = false;
    bool IsImage = false;
    bool IsText = false;
    bool ScaleText = false;

    bool InitDone = false;

    public bool IsSimple = false;
    bool ScaleDirection = true;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(SetWithDelay(0.35f));
    }

    private void OnEnable()
    {
        if (IsSimple)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            this.gameObject.GetComponent<Image>().color = Color.white;
            ScaleDirection = true;
        }
        else
            StartCoroutine(SetWithDelay(0.35f));
    }

    IEnumerator SetWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        NewColor = new Color(1, 1, 1, 0.8f);

        if (DefaultScale.x > 1f)
            OldScale = DefaultScale;
        else
            OldScale = this.gameObject.GetComponent<RectTransform>().sizeDelta;

        NewScale = OldScale * 1.1f;
        TmpScale = OldScale;

        if (this.gameObject.GetComponent<Image>() != null)
            OldColor = this.gameObject.GetComponent<Image>().color;
        else
            OldColor = this.gameObject.GetComponent<Text>().color;
        NewColor = InvertColor(OldColor);
        TmpColor = OldColor;

        if (this.gameObject.GetComponentInChildren<Text>() != null)
        {
            OldTextColor = this.gameObject.GetComponentInChildren<Text>().color;
            HaveText = true;
        }

        if (OldColor.r > 0.99f & OldColor.g < 0.01f & OldColor.b < 0.01f)
            NewColor = new Color(0.3f, 0, 0);

        if (this.gameObject.GetComponent<Button>() != null)
            IsButton = true;
        else
        {
            NewColor = new Color(OldColor.r, OldColor.g, OldColor.b, 0.1f);
        }

        if (this.gameObject.GetComponent<Image>() != null & this.gameObject.GetComponent<Button>() == null)
            IsImage = true;

        if (this.gameObject.GetComponent<Text>() != null)
        {
            IsText = true;
            IsButton = false;
            IsImage = false;
            NewColor = new Color(OldColor.r, OldColor.g, OldColor.b, 0.5f);
        }

        float startDelay = Time.realtimeSinceStartup + 1f - (int)Time.realtimeSinceStartup;

        if (StartImediatelly)
            startDelay = 0.1f;
        InvokeRepeating("BackToOldSize", startDelay, 0.5f); // update at 15 fps

        InitDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSimple)
        {
            if (ScaleDirection)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), 5f * Time.deltaTime);

                if (DoNotChangeColor)
                    this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, new Color(1f, 1f, 1f, 0.9f), 3 * Time.deltaTime);
                else
                    this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, new Color(1, 0, 0, 0.4f), 3 * Time.deltaTime);

                if (transform.localScale.x > 1.15f)
                    ScaleDirection = false;
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.95f, 0.95f, 0.95f), 5f * Time.deltaTime);
                this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, Color.white, 3 * Time.deltaTime);
                if (transform.localScale.x < 1f)
                    ScaleDirection = true;
            }
        }
        else
        {
            if (InitDone == false)
                return;

            if (IsButton)
            {
                if (this.gameObject.GetComponent<Button>().interactable)
                {
                    if (DoNotScale == false)
                        this.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.gameObject.GetComponent<RectTransform>().sizeDelta, TmpScale, 5 * Time.deltaTime);
                    this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, TmpColor, 7 * Time.deltaTime);
                    if (HaveText)
                        this.gameObject.GetComponentInChildren<Text>().color = OldTextColor;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
                    if (HaveText)
                        this.gameObject.GetComponentInChildren<Text>().color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
                }
            }
            else if (IsImage)
            {
                this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, TmpColor, 7 * Time.deltaTime);
                if (DoNotScale == false)
                    this.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.gameObject.GetComponent<RectTransform>().sizeDelta, TmpScale, 5 * Time.deltaTime);
            }
            else if (IsText)
            {
                if (ScaleText)
                    if (DoNotScale == false)
                        this.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.gameObject.GetComponent<RectTransform>().sizeDelta, TmpScale, 5 * Time.deltaTime);
                this.gameObject.GetComponent<Text>().color = Color.Lerp(this.gameObject.GetComponent<Text>().color, TmpColor, 7 * Time.deltaTime);
            }
        }
    }

    //public void ButtonPressed()
    //{
    //    TmpScale = NewScale;
    //    TmpColor = InvertColor(OldColor);
    //    StartCoroutine(BackToOldSize(0.20f));
    //}

    void BackToOldSize()
    {
        if (switcher)
        {
            TmpScale = OldScale;
            TmpColor = OldColor;
            switcher = !switcher;
        }
        else
        {
            TmpScale = NewScale;
            if (DoNotChangeColor == false)
                TmpColor = NewColor;
            switcher = !switcher;
        }
    }

    Color InvertColor(Color col)
    {
        if (col.r > 0.99f & col.g < 0.01f & col.b < 0.01f)
            return NewColor;

        if (col.r > 0.9f & col.g > 0.9f & col.b > 0.9f)
            return NewColor;
        else
            return new Color(1 - col.r, 1 - col.g, 1 - col.b);
    }

    void OnDisable()
    {
        if (DefaultScale.x > 1f)
            OldScale = DefaultScale;

        TmpScale = OldScale;
        TmpColor = OldColor;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = OldScale;
        if (IsButton || IsImage)
            this.gameObject.GetComponent<Image>().color = OldColor;
        else if (IsText)
            this.gameObject.GetComponent<Text>().color = OldColor;
        switcher = false;

        CancelInvoke();
    }

    public void SetColor(Color oldCol, Color newCol)
    {
        if (InitDone)
        {
            OldColor = oldCol;
            NewColor = newCol;
            ScaleText = false;
        }
    }

    public void SetColor(Color oldCol, Color newCol, float scale)
    {
        if (InitDone)
        {
            OldColor = oldCol;
            NewColor = newCol;
            NewScale = OldScale * scale;
            ScaleText = true;
        }
    }
}

