using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlubmManager : MonoBehaviour
{

    [SerializeField] private float cursorSpeed = 1.0f;

    [SerializeField] private GameObject targetImage;
    private Transform player;
    private Transform content;
    SnapManager snap;

    void Start()
    {
        content = GameObject.Find("Content").transform;
        snap = GameObject.Find("GameManager").GetComponent<SnapManager>();
        player = GameObject.Find("Player").transform;
        ShowSSImage();
        EventSystem.current.SetSelectedGameObject(null);
    }


    void Update()
    {
        GetKey();
    }

    private void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectPic();
        }

        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveAlbum(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    public void ShowSSImage()
    {
        foreach (string item in snap.pathList)
        {
            string[] path = item.Split(',');
            string[] pos = path[0].Split('_', '.');

            byte[] image = File.ReadAllBytes(path[0]);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            RawImage target = Instantiate(targetImage, content).GetComponent<RawImage>();
            target.texture = tex;
            target.GetComponent<PhotoSpot>().spot = (SPOT)Enum.Parse(typeof(SPOT), path[1]);
            target.GetComponent<PhotoSpot>().albumPos = new Vector2(float.Parse(path[2]), float.Parse(path[3]));

            target.name = item;

            target.GetComponent<RectTransform>().localPosition = new Vector2(int.Parse(pos[1]) * 10 + 1200, int.Parse(pos[2]) * 10 - 750);
        }
    }

    public void SelectPic()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (hit.collider.GetComponent<PhotoSpot>().spot == SPOT.GOOD)
        {
            Vector2 pos = hit.collider.GetComponent<PhotoSpot>().albumPos;

            player.position = pos;

            Debug.Log(pos.x + ":" + pos.y);

        }
    }

    private void MoveAlbum(float value_x, float value_y)
    {
        content.Translate(value_x * cursorSpeed , value_y * cursorSpeed, 0);
    }
}
