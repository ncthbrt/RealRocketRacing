using System;
using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{

    public Transform StartPoint;
    public Transform EndPoint;
    private Material _material;
    private const int VertexCount=20;
    // Use this for initialization
    private LineRenderer _renderer;
    
    private void Start()
    {
        _renderer = gameObject.AddComponent<LineRenderer>();        
        _renderer.SetVertexCount(VertexCount);
        _renderer.SetWidth(0.2f,0.1f);
        _renderer.sortingLayerName = "Player";
        _renderer.SetColors(new Color(0.25f, 1, 0.1f), new Color(1, 0,0));
        _material= new Material(Shader.Find("Particles/Additive"));
        _renderer.material =_material;
        _renderer.useWorldSpace = true;
    }

    private float _progress = 0;
    private const float Amplitude=0.2f;
    // Update is called once per frame
    private void Update()
    {
        _renderer = GetComponent<LineRenderer>();
        float deltaY = (EndPoint.position - StartPoint.position).y;
        float deltaX = (EndPoint.position - StartPoint.position).x;
        if (Mathf.Abs(deltaX)>0)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                _renderer.SetPosition(i, new Vector3(i / (float)VertexCount * deltaX + StartPoint.position.x, StartPoint.position.y + Amplitude * Mathf.Sin(i * _progress * 20f),StartPoint.position.z));
            }
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                _renderer.SetPosition(i, new Vector3(StartPoint.position.x + Amplitude * Mathf.Sin(i * _progress * 20f), i / (float)VertexCount * deltaY + StartPoint.position.y, StartPoint.position.z));
            }
        }
        _progress += Time.deltaTime;
                
        
    }
}
