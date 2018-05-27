using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimbableFlag : MonoBehaviour
{

    PlayerControl pc;

    private void Awake()
    {
        pc = GetComponent<PlayerControl>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Crimbable"))
        {
            StopCoroutine("CheckCrimb");
            StartCoroutine("CheckCrimb");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Crimbable"))
        {
            StopCoroutine("CheckCrimb");
            pc.crimbable = false;
            pc.ActiveGravity(true);
        }
    }

    private IEnumerator CheckCrimb()
    {
        pc.rb.velocity = new Vector2(0, 0);
        if (pc.gm.season != SEASON.SUMMER)
        {
            pc.ActiveGravity(false);

            yield return new WaitForSeconds(1f);

            pc.crimbable = false;
            pc.ActiveGravity(true);
            yield break;
        }
        else
        {
            while (true)
            {
                if (pc.rb.IsTouching(pc.filter2d))
                {
                    pc.crimbable = false;
                    pc.ActiveGravity(true);
                }
                else
                {
                    pc.crimbable = true;
                    pc.ActiveGravity(false);
                }
                yield return null;
            }
        }
    }
}
