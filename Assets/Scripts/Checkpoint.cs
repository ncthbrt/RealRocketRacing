using System;
using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{

    public Transform StartPoint;
    public Transform EndPoint;
    private Material _material;
    private const int VertexCount=50;
	public const int NumberOfPeriods = 10;
	public const float Amplitude = 0.5f;
    // Use this for initialization
    private LineRenderer _renderer;
    
    private void Start()
    {
        _renderer = gameObject.AddComponent<LineRenderer>();        
        _renderer.SetVertexCount(VertexCount);
        _renderer.SetWidth(0.05f,0.05f);
        _renderer.sortingLayerName = "Player";
		_renderer.sortingOrder =0;
        _renderer.SetColors(new Color(0.25f, 1, 0.1f), new Color(1, 0,0));
        _material= new Material(Shader.Find("Particles/Additive"));
        _renderer.material =_material;
        _renderer.useWorldSpace = true;
    }

    private float _progress = 0;
    
    // Update is called once per frame
    private void Update()
    {
        _renderer = GetComponent<LineRenderer>();
		var delta = (EndPoint.position - StartPoint.position);
		var length = delta.magnitude;
		var angle = delta.normalized;

		var frequency = 1f/ NumberOfPeriods;

		for (int i=0; i<VertexCount; ++i) {
			float height=Amplitude*Mathf.Sin (frequency*i+_progress*Mathf.PI);
			float position=(length/(float)VertexCount)*i;
			_renderer.SetPosition(i, new Vector3(angle.x*height+StartPoint.position.x+position*angle.y,position*angle.x+angle.y*height+StartPoint.position.y,StartPoint.position.z));
		}
        
						
			
		_progress += Time.deltaTime;
	}
        
                
        
}
