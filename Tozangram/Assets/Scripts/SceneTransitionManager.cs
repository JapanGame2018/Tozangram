using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject selectObj;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(selectObj);
    }

    public void TransitonScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void OpenScene(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if(sceneName == "Pose")
            {
                PoseOpen();
            }
        }
    }

    public void CloseScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        if(sceneName == "Option")
        {
            OptionClose();
        }
        else if(sceneName == "Pose")
        {
            PoseClose();
        }
    }

    private void OptionClose()
    {
        PlayerPrefs.SetFloat("SE", StaticValue.seValue);
        PlayerPrefs.SetFloat("BGM", StaticValue.bgmValue);

        if (SceneManager.GetSceneByName("Pose").isLoaded)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Continue"));
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject);
        }
    }

    private void PoseOpen()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().state = STATE.POSE;
        Time.timeScale = 0f;
    }

    private void PoseClose()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().state = STATE.GAME;
        Time.timeScale = 1.0f;
    }

}
