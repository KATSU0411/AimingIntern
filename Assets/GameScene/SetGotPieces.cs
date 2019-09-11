using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGotPieces : MonoBehaviour {

    private Transform pieces;
    private float SIZE_W;
    private float SIZE_H;

	// Use this for initialization
	void Start () {
        pieces = this.gameObject.transform;
        SIZE_W = this.gameObject.GetComponent<RectTransform>().rect.width / 4;
        SIZE_H = this.gameObject.GetComponent<RectTransform>().rect.height / 2;
	}
	
	// Update is called once per frame
	void Update () {
        int cnt = 0;
        foreach(Transform piece in pieces)
        {
            piece.GetComponent<RectTransform>().localPosition = new Vector2(SIZE_W * (cnt % 4), SIZE_H * (cnt / 4));
            cnt++;
        }
	}
}
