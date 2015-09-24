using UnityEngine;
using System.Collections;

public class ZoomLensUI : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject Rocket;
    private RectTransform _rect;
    public RectTransform ZoomLensBackground;
	// Use this for initialization
	void Start ()
	{
	    _rect = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var screenPosition=MainCamera.WorldToScreenPoint(Rocket.transform.position);
	    _rect.position = screenPosition;
	    ZoomLensBackground.position = screenPosition;
	}
}
