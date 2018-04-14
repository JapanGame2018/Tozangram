using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEASON
{
    NONE, SPRING, SUMMER, AUTUMN, WINTER
}

public enum STATE
{
    GAME, POSE, TRANS
}

public class GameManager : MonoBehaviour
{
    public SEASON season = SEASON.NONE;
    public STATE state = STATE.GAME;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetKey();
    }

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
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                i = time;
                season++;
                if (season > SEASON.WINTER)
                {
                    season = SEASON.NONE;
                }
            }
            yield return null;
        }

        state = STATE.GAME;
        Time.timeScale = 1f;
        yield break;
    }
}
