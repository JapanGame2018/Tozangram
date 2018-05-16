using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>季節の一覧</summary>
public enum SEASON
{
    NONE, SPRING, SUMMER, AUTUMN, WINTER
}

public enum STATE
{
    GAME, POSE, TRANS, SCROLL
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] flameList;

    public SEASON season = SEASON.NONE;
    public STATE state = STATE.GAME;

    void Update()
    {
        GetKey();
    }

    /// <summary>
    /// 入力を取得
    /// </summary>
    private void GetKey()
    {
        if(state == STATE.GAME && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
        {
            StartCoroutine(ChangeFlame());
        }
    }

    private IEnumerator ChangeFlame()
    {
        state = STATE.TRANS;
        Time.timeScale = 0f;
        flameList[0].SetActive(true);

        int time = 60;

        for(int i = time;i > 0;i--)
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

        state = STATE.GAME;
        Time.timeScale = 1f;
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
}
