﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitsDisplay : MonoBehaviour
{

	// Here you should drag game object which has SelectUnits.cs script attached
	[SerializeField] SelectUnits selectUnitsScript;
	[SerializeField] Canvas canvas;

	private float canvasWidth, canvasHeight;
	private List<GameObject> texts = new List<GameObject>();

    void Start()
    {
    	//addText("Bla bla!", new Vector3(0, 0, 0), Color.black);
    	RectTransform rt = canvas.gameObject.GetComponent<RectTransform>();
    	canvasWidth = rt.rect.width;
    	canvasHeight = rt.rect.height;
    	Debug.Log(canvasWidth + " " + canvasHeight);
    }

    void Update()
    {
    	foreach(GameObject t in texts) {
    		Destroy(t);
    	}
    	texts = new List<GameObject>();
    	float pos = 20 - canvasWidth / 2;
    	GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
    	List<GameObject> selected = selectUnitsScript.GetSelected();
    	foreach(GameObject unit in units) {
    		if(selected.Contains(unit)) {
    			texts.Add(addText(unit.name, new Vector3(pos, 20 - canvasHeight / 2, 0), Color.red));
    		}
    		else {
    			texts.Add(addText(unit.name, new Vector3(pos, 20 - canvasHeight / 2, 0), Color.black));
    		}
    		pos += 120;
    	}
    }

    GameObject addText(string txt, Vector3 pos, Color color) {
    	GameObject text = new GameObject();
    	text.transform.parent = canvas.transform;
  		Text myText = text.AddComponent<Text>();
    	myText.text = txt;
    	myText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    	myText.color = color;
    	RectTransform rectTransform = myText.GetComponent<RectTransform>();
    	rectTransform.pivot = new Vector2(0, 0);
        rectTransform.localPosition = pos;
        rectTransform.sizeDelta = new Vector2(100, 20);
        return text;
    }
}
