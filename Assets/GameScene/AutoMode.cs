using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class AutoMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Toggle>().onValueChanged.AddListener(Auto);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Auto(bool change)
    {
        Auto();
    }
    public void Auto()
    {
        if (!this.gameObject.GetComponent<Toggle>().isOn) return;
        if (!UserInfo.flg_turn) return;
        List<GameObject> pieces = Utility.GetPieceInfo();
        Auto(pieces);
    }

    public void Auto(List<GameObject> pieces)
    {
        if (!this.gameObject.GetComponent<Toggle>().isOn) return;
        if (!UserInfo.flg_turn) return;

        List<Piece> ps = new List<Piece>();
        foreach (GameObject piece in pieces)
        {
            Piece p = piece.GetComponent<Piece>();
            ps.Add(p);
        }

        while (true) {
            Piece p = ps[Random.Range(0, ps.Count)];
            if (p.info.captured || (p.info.owner_user_id != UserInfo.user_id)) continue;

            Vector2Int move = new Vector2Int(0, 0);

            if (p.info.kind == "evil") move = WeightRandomEvil(p.info);
            else if (p.info.kind == "good") move = WeightRandomGood(p.info);

            if (!p.flgFirst) move.y *= -1;

            if (!p.CanMove(p.info.point_x + move.x, p.info.point_y + move.y)) continue;

            p.Move(p.info.point_x + move.x, p.info.point_y + move.y);
            break;
        }
    }

    private Vector2Int WeightRandomEvil(PieceInfo info)
    {
        float ran = Random.value;
        if (ran <= 0.5f) return new Vector2Int(0, 1);
        else if (ran <= 0.6f) return new Vector2Int(0, -1);
        else if (ran <= 0.8f) return new Vector2Int(1, 0);
        else return new Vector2Int(-1, 0);
    }
    private Vector2Int WeightRandomGood(PieceInfo info)
    {
        float ran = Random.value;
        float thresh = 0.35f;
        if (info.point_x <= 3) thresh = 0.05f;
        if (ran <= 0.6f) return new Vector2Int(0, 1);
        else if (ran <= 0.6f + thresh) return new Vector2Int(1, 0);
        else return new Vector2Int(-1, 0);
    }
}
