using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AlubmManager : MonoBehaviour
{

    //public string screenShotPath;
    [SerializeField] private GameObject targetImage;
    private Transform content;
    SnapManager snap;


    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        content = GameObject.Find("Content").transform;
        snap = GameObject.Find("GameManager").GetComponent<SnapManager>();
        ShowSSImage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSSImage()
    {
        Texture2D[] imageArray = Resources.LoadAll<Texture2D>("Album");
        foreach (var item in imageArray)
        {
            try
            {
                Debug.Log(item.name);
                var point = item.name.Split('_');

                RawImage target = Instantiate(targetImage, content).GetComponent<RawImage>();
                target.texture = item;

                target.GetComponent<RectTransform>().localPosition = new Vector2(int.Parse(point[0]) * 10 + 800, int.Parse(point[1]) * 10 - 500);


                target.name = item.name;
            }
            catch
            {
                Debug.Log(item.name + " is fail");
            }

        }
    }

    public void ShowSSImage()
    {
        foreach (string item in snap.pathList)
        {
            string[] point = item.Split('_', '.');

            Debug.Log(point[1] + point[2]);

            byte[] image = File.ReadAllBytes(item);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            RawImage target = Instantiate(targetImage, content).GetComponent<RawImage>();
            target.texture = tex;
            target.name = item;

            target.GetComponent<RectTransform>().localPosition = new Vector2(int.Parse(point[1]) * 10 + 800, int.Parse(point[2]) * 10 - 500);
        }
    }
}
