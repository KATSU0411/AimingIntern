using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Error : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnPointerClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPointerClick()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
