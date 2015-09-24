using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZoomLensFadeAnimation : MonoBehaviour
{
    public RawImage Lens;
    public UnityEngine.UI.Image Border;
    public UnityEngine.UI.Image Background;
    public float FadeOutDuration;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        FadeOut();
	    }        
	}

    public void FadeOut()
    {        
        CancelInvoke("FadeOutProgress");
        CancelInvoke("FadeInProgress");
        InvokeRepeating("FadeOutProgress",0,1/60f);
    }

    private float _progress = 1;

    private void FadeInProgress()
    {
        
    }
    private void FadeOutProgress()
    {
        _progress -= (1/60f)/FadeOutDuration;
        if (_progress <= 0)
        {
            _progress = 0;            
            CancelInvoke("FadeOutProgress");
        }
        Border.color=new Color(Border.color.r,Border.color.g,Border.color.b,_progress);
        Lens.color = new Color(Lens.color.r, Lens.color.g, Lens.color.b, _progress);
        Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, _progress);
    }
}
