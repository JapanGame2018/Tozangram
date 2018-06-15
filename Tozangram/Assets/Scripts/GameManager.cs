using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

/// <summary>季節の一覧</summary>
public enum SEASON
{
    NONE, SPRING, SUMMER, AUTUMN, WINTER
}

public enum STATE
{
    GAME, POSE, TRANS
}

public enum SPOT
{
    NORMAL, PHOTO, GOOD, BEST
}

public enum LOAD
{
    OK, LOADING
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] flameList;
    public Transform player;

    public SEASON season = SEASON.NONE;
    public STATE state = STATE.GAME;
    public LOAD load = LOAD.LOADING;
    public int stage = 1;

    SpringFlameManager spring;
    SummerFlameManager summer;
    WinterFlameManager winter;
    SceneTransitionManager stm;
    SnapManager snap;

    public static GameManager instance;

    private void Awake()
    {
        spring = GetComponent<SpringFlameManager>();
        summer = GetComponent<SummerFlameManager>();
        winter = GetComponent<WinterFlameManager>();
        snap = GetComponent<SnapManager>();
        stm = GameObject.Find("SceneManager").GetComponent<SceneTransitionManager>();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("STAGE"))
        {
            //stage = PlayerPrefs.GetInt("STAGE");
        }

        StartCoroutine(ChangeStage(stage));

        state = STATE.GAME;
        Time.timeScale = 1.0f;

        ReStart();

        load = LOAD.OK;
    }

    void Update()
    {
        GetKey();
    }

    /// <summary>
    /// 入力を取得
    /// </summary>
    private void GetKey()
    {
        if (state == STATE.GAME)
        {
            // フレーム変更
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                StartCoroutine(ChangeFlame());
            }

            // 撮影
            if (Input.GetKeyDown(KeyCode.C))
            {
                snap.ClickShootButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!SceneManager.GetSceneByName("Album").isLoaded)
            {
                stm.OpenScene("Album");
                ChangeState(STATE.POSE);
            }
            else
            {
                stm.CloseScene("Album");
                if (!SceneManager.GetSceneByName("Pose").isLoaded)
                {
                    ChangeState(STATE.GAME);
                }
            }
        }

        // ポーズ画面
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (state == STATE.POSE)
            {
                if (SceneManager.GetSceneByName("Option").isLoaded)
                {
                    stm.CloseScene("Option");
                }
                else if (SceneManager.GetSceneByName("Album").isLoaded)
                {
                    stm.CloseScene("Album");
                    if (!SceneManager.GetSceneByName("Pose").isLoaded)
                    {
                        ChangeState(STATE.GAME);
                    }
                }
                else
                {
                    stm.CloseScene("Pose");
                }
            }
            else
            {
                stm.OpenScene("Pose");
            }
        }
    }

    private IEnumerator ChangeFlame()
    {
        SEASON current = season;

        ChangeState(STATE.POSE);

        flameList[0].SetActive(true);

        int time = 60;

        for (int i = time; i > 0; i--)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                i = time;
                season--;
                if (season < SEASON.NONE)
                {
                    season = SEASON.WINTER;
                }

                SelectFlame(season);
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                i = time;
                season++;
                if (season > SEASON.WINTER)
                {
                    season = SEASON.NONE;
                }

                SelectFlame(season);
            }
            yield return null;
        }

        // 現在と違うものを選択していた場合
        if (season != current)
        {
            Fetch(current, season);
        }

        ChangeState(STATE.GAME);
        flameList[0].SetActive(false);
        yield break;
    }

    /// <summary>
    /// 選択したフレームをハイライトする
    /// </summary>
    /// <param name="select">選択中のフレーム</param>
    private void SelectFlame(SEASON select)
    {
        for (int i = 1; i < flameList.Length; i++)
        {
            flameList[i].SetActive(false);
        }
        flameList[(int)select].SetActive(true);
    }

    private void Fetch(SEASON before, SEASON after)
    {
        switch (before)
        {
            case SEASON.NONE:
                break;
            case SEASON.SPRING:
                spring.Disabled();
                break;
            case SEASON.SUMMER:
                summer.Disabled();
                break;
            case SEASON.AUTUMN:
                break;
            case SEASON.WINTER:
                winter.Disabled();
                break;
            default:
                break;
        }

        switch (after)
        {
            case SEASON.NONE:
                break;
            case SEASON.SPRING:
                spring.Enabled();
                break;
            case SEASON.SUMMER:
                summer.Enabled();
                break;
            case SEASON.AUTUMN:
                break;
            case SEASON.WINTER:
                winter.Enabled();
                break;
            default:
                break;
        }
    }

    public void Save()
    {

        StreamWriter sw = new StreamWriter(@"Assets/Resources/AlbumData.csv", false, Encoding.GetEncoding("Shift_JIS"));
        foreach (string item in snap.pathList)
        {
            sw.WriteLine(item);
        }
        sw.Close();

    }

    // リスタート地点へ移動
    public void ReStart()
    {
        string pos = PlayerPrefs.GetString("reStart");
        string[] posArray = pos.Split('_');

        player.position = new Vector2(float.Parse(posArray[0]), float.Parse(posArray[1]));
    }

    /// <summary>
    /// STATEの変更に伴う処理
    /// </summary>
    /// <param name="st">変更先STATE</param>
    public void ChangeState(STATE st)
    {
        state = st;
        switch (st)
        {
            case STATE.GAME:
                Time.timeScale = 1f;
                break;
            case STATE.POSE:
                Time.timeScale = 0f;
                break;
            case STATE.TRANS:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
    }

    public IEnumerator ChangeStage(int index = 1)
    {
        if (!SceneManager.GetSceneByName("Stage" + index).isLoaded)
        {
            load = LOAD.LOADING;
            for (int i = 1; i <= 5; i++)
            {
                if (SceneManager.GetSceneByName("Stage" + i).isLoaded)
                {
                    SceneManager.UnloadSceneAsync("Stage" + i);
                }
            }

            yield return SceneManager.LoadSceneAsync("Stage" + index, LoadSceneMode.Additive);
            load = LOAD.OK;
        }
        yield break;
    }
}
