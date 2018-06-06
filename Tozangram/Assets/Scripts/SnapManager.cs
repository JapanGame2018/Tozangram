using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System;
using UnityEngine.UI;

public class SnapManager : MonoBehaviour
{

    Camera cam;
    GameObject canvas;
    GameObject targetImage;
    GameManager gm;
    string screenShotPath;
    public Vector2 snapPos;
    public SPOT spot;
    public List<string> pathList = new List<string>();

    void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas");
        targetImage = GameObject.Find("RawImage");
        gm = GetComponent<GameManager>();

        try
        {
            StreamReader sr = new StreamReader(@"Assets/Resources/AlbumData.csv", Encoding.GetEncoding("Shift_JIS"));
            string line;
            while((line = sr.ReadLine()) != null)
            {
                pathList.Add(line);
            }

            sr.Close();
        }
        catch
        {
            Debug.Log("アルバム一覧を読み込めませんでした。");
        }
    }

    private string GetScreenShotPath(Vector2 pos, int stageIndex = 0)
    {
        string path = "Assets/Resources/Album/" + "_" + pos.x + "_" + pos.y + ".png";

        return path;
    }

    // UIを消したい場合はcanvasを非アクティブにする
    private void UIStateChange()
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    private IEnumerator CreateScreenShot(string path)
    {
        UIStateChange();

        // レンダリング完了まで待機
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = renderTexture;

        Texture2D texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        texture.Apply();

        // 保存する画像のサイズを変えるならResizeTexture()を実行
        //		texture = ResizeTexture(texture,320,240);

        byte[] pngData = texture.EncodeToPNG();
        // ファイルとして保存するならFile.WriteAllBytes()を実行
        File.WriteAllBytes(path, pngData);

        string data = path + "," + spot;

        if (!pathList.Contains(data))
        {
            pathList.Add(data);
        }


        cam.targetTexture = null;

        Debug.Log("Done!");
        UIStateChange();
        gm.Save();
    }

    Texture2D ResizeTexture(Texture2D src, int dst_w, int dst_h)
    {
        Texture2D dst = new Texture2D(dst_w, dst_h, src.format, false);

        float inv_w = 1f / dst_w;
        float inv_h = 1f / dst_h;

        for (int y = 0; y < dst_h; ++y)
        {
            for (int x = 0; x < dst_w; ++x)
            {
                dst.SetPixel(x, y, src.GetPixelBilinear((float)x * inv_w, (float)y * inv_h));
            }
        }
        return dst;
    }

    public void ClickShootButton()
    {
        screenShotPath = GetScreenShotPath(snapPos);
        StartCoroutine(CreateScreenShot(screenShotPath));

        if(spot != SPOT.NORMAL)
        {
            PlayerPrefs.SetString("reStart", snapPos.x + "_" + snapPos.y);
        }
    }

    public void ShowSSImage()
    {
        if (!String.IsNullOrEmpty(screenShotPath))
        {
            byte[] image = File.ReadAllBytes(screenShotPath);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            // NGUI の UITexture に表示
            RawImage target = targetImage.GetComponent<RawImage>();
            target.texture = tex;
        }
    }
}