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

	// Use this for initialization
	void Start () {
        canvasTran = transform.parent.parent.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {

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
        this.gameObject.GetComponent<RectTransform>().position += new Vector3(e.delta.x, e.delta.y, 0.0f);
    }
     public void OnEndDrag(BaseEventData ev)
    {
        PointerEventData e = (PointerEventData)ev;
        gameObject.GetComponent<Image>().color = Vector4.one;
        Destroy(draggingObject);
    }




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
