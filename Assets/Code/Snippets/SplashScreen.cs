using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
    //**MANAGE SplashScreen*//
    //**ATTACHED TO Services.GameObject*//
    /*NOTES:
     * ImageLogo shhould be replaced with Game Logo     (set in Canvas)
     * Level_name should be name of the first Scene     (Set in SplashScren, attached to canvas)
     */


    private AsyncOperation Async = null;
    public Texture2D ProgressBarEmpty;
    public Texture2D ProgressBarFull;
    public GameObject TextConnectonFailed;
    public GameObject TextLoading;
    [SerializeField] string Level_name = null;
    bool LoadingStart = false;
    int Percent = 0;
    bool PercentDone = false;
    float PercentTimer;
    [SerializeField] Text TextPercent = null;
    [SerializeField] GameObject ButtonTapToStart = null;

    // Use this for initialization
    void Start () {
        //if (Application.internetReachability != NetworkReachability.NotReachable)
        //StartCoroutine(SetWithDelay(1));
        //else
        //{
        //    TextConnectonFailed.SetActive(true);
        //    InvokeRepeating("Check_Online", 4, 3);
        //}

        //DontDestroyOnLoad(gameObject);

        //StartCoroutine(SetWithDelay_TapToStart(1.5f));

#if UNITY_STANDALONE
        if (Screen.width == 1920 & Screen.height == 1440)
            Screen.SetResolution(1920, 886, false);
        //Display.SetRenderingResolution(1920, 886);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (PercentDone == false)
        {
            if (Time.realtimeSinceStartup > PercentTimer)
            {
                PercentTimer = Time.realtimeSinceStartup + 0.1f;
                Percent += Random.Range(5, 12);

                if (Percent > 100)
                {
                    Percent = 100;
                    PercentDone = true;
                    StartCoroutine(SetWithDelay_TapToStart(0.5f));
                }

                TextPercent.text = Percent.ToString() + "%";
            }
        }
    }

    IEnumerator SetWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadingStart = true;
        StartCoroutine(LoadingLevel());
        TextLoading.SetActive(true);
    }

    IEnumerator SetWithDelay_TapToStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        TextPercent.text = "";
        ButtonTapToStart.SetActive(true);
    }

    void Check_Online()
    {
        if (LoadingStart == false && Application.internetReachability != NetworkReachability.NotReachable)
            LoadScene();
    }

    public void LoadScene()
    {
        ButtonTapToStart.SetActive(false);
        TextLoading.SetActive(true);

        StartCoroutine(SetWithDelay_LoadScene(0.1f));
    }

    IEnumerator SetWithDelay_LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadingStart = true;
        TextConnectonFailed.SetActive(false);
        StartCoroutine(LoadingLevel());
        //TextLoading.SetActive(true);
    }

    //for loading scene
    IEnumerator LoadingLevel()
    {
#if !UNITY_WEBGL
        Async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Level_name);
#else
        Async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Level_name);
#endif
        yield return Async;
    }

    //For progress display during loading
    void OnGUI()
    {
        if (Async != null)
        {
            float xpos = (Screen.width - Screen.width * 0.45f) / 2;
            float ypos = (Screen.height * 0.97f);
            float xsize = (Screen.width * 0.45f);
            GUI.DrawTexture(new Rect(xpos, ypos, xsize, 10), ProgressBarEmpty);
            GUI.DrawTexture(new Rect(xpos + 2.5f, ypos + 2.5f, xsize * Async.progress * 0.8f, 5f), ProgressBarFull);
        }
    }
}
