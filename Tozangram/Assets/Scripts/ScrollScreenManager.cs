using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScreenManager : MonoBehaviour
{

    private Transform m_camera;     // 移動させるカメラ
    SnapManager sm;
    [SerializeField, Range(0f, 1f)] private float scrollSpeed;  // スクロール速度：０～１の間で設定

    private void Awake()
    {
        // カメラのトランスフォームを取得
        m_camera = GameObject.Find("Main Camera").GetComponent<Transform>();

        sm = GameObject.Find("GameManager").GetComponent<SnapManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("target"))
        {
            StopCoroutine("MoveCamera");
            StartCoroutine("MoveCamera", collision.GetComponent<RectTransform>());

            sm.snapPos = collision.transform.position;
            sm.spot = collision.GetComponent<PhotoSpot>().spot;
        }
    }

    private IEnumerator MoveCamera(RectTransform rectTransform)
    {
        while(Vector3.Distance(m_camera.position, rectTransform.position) > 0)
        {
            m_camera.position = Vector3.Lerp(m_camera.position, rectTransform.position, scrollSpeed);
            yield return null;
        }
        yield break;
    }
}
