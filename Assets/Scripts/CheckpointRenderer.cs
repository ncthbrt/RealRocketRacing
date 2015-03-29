using System;
using UnityEngine;
using System.Collections;

public class CheckpointRenderer : MonoBehaviour
{

	public Camera CurrentCamera;
    public Transform StartPoint;
    public Transform EndPoint;
    private Material _material;
    private const int VertexCount=50;
	public const int NumberOfPeriods = 10;
	public const float Amplitude = 0.18f;
    // Use this for initialization
    private LineRenderer _renderer;
    
    private void Start()
    {
        _renderer = gameObject.AddComponent<LineRenderer>();        
        _renderer.SetVertexCount(VertexCount*2);
        _renderer.SetWidth(0.05f,0.05f);
        _renderer.sortingLayerName = "Player";
		_renderer.sortingOrder =0;
        _renderer.SetColors(new Color(1f, 1f, 1f, 0.5f), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                
        _material= new Material(Shader.Find("Particles/Additive"));
        _renderer.material =_material;
        _renderer.useWorldSpace = true;
    }

    private float _progress = 0;



    private bool OnScreen(Vector3 screenCoord)
    {
        return (screenCoord.x >= 0 && screenCoord.x <= 1) && (screenCoord.y >= 0 && screenCoord.y <= 1);
    }
    // Update is called once per frame
    private void Update()
    {

		var screenCoordA= CurrentCamera.WorldToViewportPoint(StartPoint.position);
		var screenCoordB= CurrentCamera.WorldToViewportPoint(EndPoint.position);
        if (OnScreen(screenCoordA) || OnScreen(screenCoordB))
        {
            _renderer = GetComponent<LineRenderer>();
            var delta = (EndPoint.position - StartPoint.position);
            var length = delta.magnitude;
            var angleVector = delta.normalized;
            var angle = -Mathf.Atan2(angleVector.y, angleVector.x);


            for (int i = 0; i < VertexCount; ++i)
            {
                float y = Amplitude*Mathf.Sin(i + _progress*Mathf.PI);
                float x = (length/VertexCount)*i;
                float screenX = x*Mathf.Cos(angle) + y*Mathf.Sin(angle) + StartPoint.position.x;
                float screenY = -x*Mathf.Sin(angle) + y*Mathf.Cos(angle) + StartPoint.position.y;
                _renderer.SetPosition(i, new Vector3(screenX, screenY, StartPoint.position.z));
            }
            for (int i = VertexCount, j = VertexCount; i > 0; --i,++j)
            {
                float y = (Amplitude*Mathf.Sin(-(i + _progress*Mathf.PI)));
                float x = (length/VertexCount)*i;
                float screenX2 = x*Mathf.Cos(angle) + y*Mathf.Sin(angle) + StartPoint.position.x;
                float screenY2 = -x*Mathf.Sin(angle) + y*Mathf.Cos(angle) + StartPoint.position.y;
                _renderer.SetPosition(j, new Vector3(screenX2, screenY2, StartPoint.position.z));
            }
            _progress += (Time.deltaTime*5f);
        }
    }
        
                
        
}
