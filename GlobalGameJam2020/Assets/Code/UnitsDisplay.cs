using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitsDisplay : MonoBehaviour
{

	// Here you should drag game object which has SelectUnits.cs script attached
	[SerializeField] SelectUnits selectUnitsScript;
	[SerializeField] Canvas canvas;

	private float canvasWidth, canvasHeight;
	private Dictionary<GameObject, GameObject> unitToIcon = new Dictionary<GameObject, GameObject>();	
	private Dictionary<GameObject, GameObject> unitToSelected = new Dictionary<GameObject, GameObject>();

    void Start()
    {
    	RectTransform rt = canvas.gameObject.GetComponent<RectTransform>();
    	canvasWidth = rt.rect.width;
    	canvasHeight = rt.rect.height;

    	GameObject[] units = GameObject.FindGameObjectsWithTag("Player");

    	float pos = 10-canvasWidth/2;
    	foreach(GameObject unit in units) {
    		Vector3 posVec = new Vector3(pos, 10-canvasHeight/2, 0);
    		SpriteRenderer sr = unit.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    		unitToSelected[unit] = addIcon("selected", posVec - new Vector3(5, 5, 0), 40, null, Color.red);
    		unitToSelected[unit].SetActive(false);
    		unitToIcon[unit] = addIcon("icon", posVec, 30, sr.sprite, sr.color);
    		pos += 50;
    	}
    }

    void Update()
    {
    	// Select on HUD
    	if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < 50) {
            Vector2 pos = Input.mousePosition - new Vector3(canvasWidth/2, canvasHeight/2, 0);
            foreach(KeyValuePair<GameObject, GameObject> pair in unitToIcon) {
	    		RectTransform rt = pair.Value.GetComponent<Image>().GetComponent<RectTransform>();
	    		if(rt.localPosition.x <= pos.x && pos.x <= rt.localPosition.x + rt.sizeDelta.x
	    				&& rt.localPosition.y <= pos.y && pos.y <= rt.localPosition.y + rt.sizeDelta.y
	    				&& pair.Key.GetComponent<Unit>().GetState() != Unit.State.Dead) {
	    			selectUnitsScript.ClickSelect(pair.Key);
	    		}
	    	}
	    }

    	foreach(KeyValuePair<GameObject, GameObject> pair in unitToSelected) {
    		pair.Value.SetActive(false);
    		Unit u = pair.Key.GetComponent<Unit>();
    		if(u.GetState() == Unit.State.Dead) {
    			unitToIcon[pair.Key].SetActive(false);
    		}
    	}
    	List<GameObject> selected = selectUnitsScript.GetSelected();
    	foreach(GameObject sel in selected) {
    		Unit u = sel.GetComponent<Unit>();
    		if(u.GetState() != Unit.State.Dead) {
    			unitToSelected[sel].SetActive(true);
    		}
    	}
    }

    GameObject addIcon(string name, Vector3 pos, float size, Sprite sprite, Color color) {
    	GameObject icon = new GameObject();
    	icon.name = name;
    	icon.transform.parent = canvas.transform;
    	Image image = icon.AddComponent<Image>();
    	image.sprite = sprite;
    	image.color = color;
    	RectTransform rectTransform = image.GetComponent<RectTransform>();
    	rectTransform.pivot = new Vector2(0, 0);
        rectTransform.localPosition = pos;
        rectTransform.sizeDelta = new Vector2(size, size);
        return icon;
    }
}
