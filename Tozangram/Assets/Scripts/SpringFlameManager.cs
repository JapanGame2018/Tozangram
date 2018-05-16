using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFlameManager : MonoBehaviour
{
    private GameObject[] flowers;
    [SerializeField] private Sprite close;
    [SerializeField] private Sprite open;

    private void Start()
    {
        flowers = GameObject.FindGameObjectsWithTag("Flower");
    }

    public void Desabled()
    {
        foreach (var item in flowers)
        {
            item.GetComponent<SpriteRenderer>().sprite = close;
            item.GetComponent<CapsuleCollider2D>().enabled = !enabled;
            item.GetComponent<PlatformEffector2D>().enabled = !enabled;
        }
    }

    public void Enabled()
    {
        foreach (var item in flowers)
        {
            item.GetComponent<SpriteRenderer>().sprite = open;
            item.GetComponent<CapsuleCollider2D>().enabled = enabled;
            item.GetComponent<PlatformEffector2D>().enabled = enabled;
        }
    }

}
