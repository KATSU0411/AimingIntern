using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class ShufflePiece : MonoBehaviour {

	// Use this for initialization
	void Start () {
       this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        var pieces = Utility.GetPieceInfo();
        PieceInfo p1 = new PieceInfo();
        PieceInfo p2 = new PieceInfo();
        int tmp;
        for(int i = 0; i < 50; i++)
        {
            p1 = pieces[Random.Range(0, 8)].GetComponent<Piece>().info;
            p2 = pieces[Random.Range(0, 8)].GetComponent<Piece>().info;

            tmp = p1.point_x;
            p1.point_x = p2.point_x;
            p2.point_x = tmp;

            tmp = p1.point_y;
            p1.point_y = p2.point_y;
            p2.point_y = tmp;
        }
    }
}
