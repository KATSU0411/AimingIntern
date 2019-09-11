using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class Piece : MonoBehaviour {

    private GameObject draggingObject;
    private Transform canvasTran;

    public int position_x { set; get; }
    public int position_y { set; get; }
    public bool flgFirst { set; get; }
    private float PIECE_SIZE;
    public int piece_id { set; get; }
    public string kind { set; get; }


	// Use this for initialization
	void Start () {
        canvasTran = GameObject.Find("Canvas/Panel").GetComponent<Transform>();

        var field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        var field_rect = field.GetComponent<RectTransform>();
        // 1コマ当たりのサイズ
        PIECE_SIZE = field_rect.rect.width / 6.0f;

        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PIECE_SIZE, PIECE_SIZE);
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.GetComponent<RectTransform>().localPosition = FieldPos(position_x, position_y);
	}

    // フィールドの座標をUIの座標に変換
    private Vector2 FieldPos(int x, int y)
    {
        if (flgFirst) return new Vector2(PIECE_SIZE * (x - 1), PIECE_SIZE * (y - 1));
        else return new Vector2(PIECE_SIZE * (x - 1), PIECE_SIZE * (7 - y - 1));
    }

    // コマのドラッグイベント
     public void OnBeginDrag(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        CreateDragObject();
        draggingObject.transform.position = e.position;
    }
    public void OnDrag(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        draggingObject.transform.position = e.position;
    }
     public void OnEndDrag(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        gameObject.GetComponent<Image>().color = Vector4.one;
        Destroy(draggingObject);

        var raycastResluts = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e, raycastResluts);

        foreach(var obj in raycastResluts)
        {
            if(obj.gameObject.tag == "Piece")
            {
                // ドラッグ先と位置を交換
                Piece swapper = obj.gameObject.GetComponent<Piece>();
                int tmp;
                tmp = swapper.position_x;
                swapper.position_x = position_x;
                position_x = tmp;
                tmp = swapper.position_y;
                swapper.position_y = position_y;
                position_y = tmp;
            }
        }

    }

    // ドラッグ中マウスオーバーでハイライト
    public void OnPointerEnter(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        if (e.pointerDrag == null) return;
        if (e.pointerDrag == this.gameObject) return;
        this.gameObject.transform.Find("Image").GetComponent<Image>()
                                        .color = new Vector4(1, 1, 0, 0.2f);
    }
    public void OnPointerExit(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        this.gameObject.transform.Find("Image").GetComponent<Image>()
                                        .color = Vector4.zero;
    }

     // ドラッグオブジェクト作成
    private void CreateDragObject()
    {
        draggingObject = new GameObject("Dragging Object");
        draggingObject.transform.SetParent(canvasTran);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;

        // レイキャストがブロックされないように
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image draggingImage = draggingObject.AddComponent<Image>();
        Image sourceImage = GetComponent<Image>();

        draggingImage.sprite = sourceImage.sprite;
        draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
        draggingImage.color = sourceImage.color;
        draggingImage.material = sourceImage.material;

        gameObject.GetComponent<Image>().color = Vector4.one * 0.6f;
    }

}
