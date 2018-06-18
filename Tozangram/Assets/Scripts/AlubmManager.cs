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
    [SerializeField] private Transform contentPre;
    private Transform content;
    SnapManager snap;
    int stage;

    void Start()
    {
        snap = GameObject.Find("GameManager").GetComponent<SnapManager>();
        player = GameObject.Find("Player").transform;
        stage = GameManager.instance.stage;
        ShowSSImage(stage);
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

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            ChangeAlubum();
        }
    }

    public void ShowSSImage(int stageIndex = 1)
    {
        content = Instantiate(contentPre, GameObject.Find("Viewport").transform);
        foreach (string item in snap.pathList)
        {
            string[] path = item.Split(',');
            string[] pos = path[0].Split('_', '.');

            if (pos[0] == "Assets/Resources/Album/Stage" + stageIndex + "/")
            {
                byte[] image = File.ReadAllBytes(path[0]);

                Texture2D tex = new Texture2D(0, 0);
                tex.LoadImage(image);

                RawImage target = Instantiate(targetImage, content).GetComponent<RawImage>();
                target.texture = tex;

                PhotoSpot ps = target.GetComponent<PhotoSpot>();
                ps.spot = (SPOT)Enum.Parse(typeof(SPOT), path[1]);
                ps.albumPos = new Vector2(float.Parse(path[2]), float.Parse(path[3]));
                ps.stage = stageIndex;

                target.name = item;

                target.GetComponent<RectTransform>().localPosition = new Vector2(int.Parse(pos[1]) * 10 + 1200, int.Parse(pos[2]) * 10 - 750);
            }
        }
    }

    public void SelectPic()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (hit.collider.GetComponent<PhotoSpot>().spot == SPOT.GOOD)
        {
            PhotoSpot ps = hit.collider.GetComponent<PhotoSpot>();
            Vector2 pos = ps.albumPos;

            player.position = pos;

            StartCoroutine( GameManager.instance.ChangeStage(ps.stage) );
            Debug.Log(pos.x + ":" + pos.y);

        }
    }

    private void MoveAlbum(float value_x, float value_y)
    {
        content.Translate(value_x * cursorSpeed , value_y * cursorSpeed, 0);
    }

    private void ChangeAlubum()
    {
        Destroy(content.gameObject);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stage--;
            if (stage < 1)
            {
                stage = 5;
            }

            ShowSSImage(stage);
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            stage++;
            if (stage > 5)
            {
                stage = 1;
            }

            ShowSSImage(stage);
        }
    }
}
