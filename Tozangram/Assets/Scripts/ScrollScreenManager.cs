using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScreenManager : MonoBehaviour
{

    private Transform m_camera;
    [SerializeField, Range(0f, 1f)] private float scrollSpeed;

    private void Start()
    {
        m_camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("target"))
        {
            StopCoroutine("MoveCamera");
            StartCoroutine("MoveCamera", collision.GetComponent<RectTransform>());
        }
    }

    private IEnumerator MoveCamera(RectTransform rectTransform)
    {
        Debug.Log("a");
        while(Vector3.Distance(m_camera.position, rectTransform.position) > 0)
        {
            m_camera.position = Vector3.Lerp(m_camera.position, rectTransform.position, scrollSpeed);
            yield return null;
        }
        yield break;
    }
}
