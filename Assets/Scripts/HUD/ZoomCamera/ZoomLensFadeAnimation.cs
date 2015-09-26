using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZoomLensFadeAnimation : MonoBehaviour
{
    public RawImage Lens;
    public Image Border;
    public Image Background;
    public float FadeOutDuration;

    public void ReentrantFadeIn()
    {
        if (!IsInvoking("FadeInProgress") && _progress < 0.99f)
        {
            FadeIn();
        }
    }

    public void FadeOut()
    {        
        CancelInvoke("FadeOutProgress");
        CancelInvoke("FadeInProgress");        
        InvokeRepeating("FadeOutProgress",0,1/60f);
    }

    public void FadeIn()
    {
        CancelInvoke("FadeOutProgress");
        CancelInvoke("FadeInProgress");
        InvokeRepeating("FadeInProgress", 0, 1 / 60f);
    }

    public void Hide()
    {
        SetOpacity(0);
        CancelInvoke("FadeOutProgress");
        CancelInvoke("FadeInProgress");
        _progress = 0;
    }

    private float _progress = 0;

    private void FadeInProgress()
    {
        _progress += (1 / 60f) / FadeOutDuration;
        if (_progress >= 1)
        {
            _progress = 1f;
            CancelInvoke("FadeInProgress");
        }
        SetOpacity(_progress);
    }

    private void SetOpacity(float opacity)
    {
        Border.color = new Color(Border.color.r, Border.color.g, Border.color.b, opacity);
        Lens.color = new Color(Lens.color.r, Lens.color.g, Lens.color.b, opacity);
        Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, opacity);
    }

    private void FadeOutProgress()
    {
        _progress -= (1/60f)/FadeOutDuration;
        if (_progress <= 0)
        {
            _progress = 0;            
            CancelInvoke("FadeOutProgress");
        }
        SetOpacity(_progress);
    }
}
