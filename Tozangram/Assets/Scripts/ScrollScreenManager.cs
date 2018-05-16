using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScreenManager : MonoBehaviour
{

    private Transform m_camera;
    private RectTransform target;

    private void Start()
    {
        m_camera = GameObject.Find("Main Camera").GetComponent<Transform>();
        target = GetComponent<RectTransform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(MoveCamera());
            m_camera.position = new Vector3(target.position.x, target.position.y, -10);
        }
    }

    private IEnumerator MoveCamera()
    {

        yield break;
    }
}
