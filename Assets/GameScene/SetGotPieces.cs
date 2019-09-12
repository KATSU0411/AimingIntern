using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGotPieces : MonoBehaviour
{

    private Transform pieces;
    private float SIZE_W;
    private float SIZE_H;

    private int childcnt = 0;

    // Use this for initialization
    void Start()
    {
        pieces = this.gameObject.transform;
        SIZE_W = this.gameObject.GetComponent<RectTransform>().rect.width / 4;
        SIZE_H = this.gameObject.GetComponent<RectTransform>().rect.height / 2;
        childcnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int cnt = 0;
        if (childcnt != this.transform.childCount)
        {
            foreach (Transform piece in pieces)
            {
                piece.GetComponent<RectTransform>().localPosition = new Vector2(SIZE_W * (cnt % 4 + 0.5f), SIZE_H * (cnt / 4 + 0.5f));
                cnt++;
            }
            childcnt = this.transform.childCount;
        }
    }
}
