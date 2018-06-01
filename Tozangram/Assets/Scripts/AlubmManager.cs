using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AlubmManager : MonoBehaviour {

    public string screenShotPath;
    [SerializeField] private GameObject targetImage;
    private Transform content;

    private void Awake()
    {
        content = GameObject.Find("Content").transform;
    }

    // Use this for initialization
    void Start () {
        LoadSSImage();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSSImage()
    {
        Texture2D[] imageArray = Resources.LoadAll<Texture2D>("Album");
        foreach (var item in imageArray)
        {
            Debug.Log(item.name);

            var point = item.name.Split('_');

            RawImage target = Instantiate(targetImage,content).GetComponent<RawImage>();
            target.texture = item;

            target.GetComponent<RectTransform>().localPosition = new Vector2(int.Parse(point[0])*10 + 800, int.Parse(point[1])*10 - 500);
            

            target.name = item.name;
        }
    }
}
