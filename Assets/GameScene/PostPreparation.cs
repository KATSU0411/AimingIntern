using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class PostPreparation : MonoBehaviour
{

    Button button;

    // Use this for initialization
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnPointerClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick()
    {
        PostInitPieces();
    }

    // ------------------------------------------------
    // コマの初期情報送信
    // ------------------------------------------------
    void PostInitPieces()
    {
        ApiClient.Instance.ResponsePrepareGame = ResponsePrepareGame;
        var param = new RequestPrepareGame();

        param.game_id = UserInfo.game_id;
        param.piece_preparations = new List<PiecePreparationInfo>();

        var pieces = Utility.GetPieceInfo();
        foreach(var piece in pieces)
        {
            PiecePreparationInfo info = new PiecePreparationInfo();
            PieceInfo p = piece.GetComponent<Piece>().info;
            info.point_y = p.point_y;
            info.point_x = p.point_x;
            info.kind = p.kind;

            param.piece_preparations.Add(info);
        }

        ApiClient.Instance.RequestPrepareGame(param);
    }

    public void ResponsePrepareGame(ResponsePrepareGame response)
    {
        GameObject.Find("Canvas/Panel/Image_wait").SetActive(true);
        GameObject.Find("Canvas/Panel/Toggle_Auto").SetActive(true);
        Destroy(GameObject.Find("Canvas/Panel/Button_Shuffle"));
        Destroy(this.gameObject);
    }
}
