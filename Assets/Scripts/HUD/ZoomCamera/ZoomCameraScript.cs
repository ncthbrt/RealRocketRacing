using UnityEngine;
using System.Collections;

public class ZoomCameraScript : MonoBehaviour
{
    public Rigidbody2D Rocket;
    private Camera _camera;
    private float _prev = 0;

    public float StartSize = 8;
    public float MaxSize = 20;
    public float MaxSpeed;
    public float ExponentialEasingFactor = 0.25f;

    private float _invExpFactor;
    private float _sizeDelta;
    
    // Use this for initialization
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();    
        _invExpFactor = 1 - ExponentialEasingFactor;
        _sizeDelta = MaxSize - StartSize;
    }

    public void OnEnable()
    {
        var speed=Rocket.velocity.magnitude;
        _prev = speed * ExponentialEasingFactor + _invExpFactor * _prev;        
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Rocket.transform.position.x, Rocket.transform.position.y, transform.position.z);
        var speed = Rocket.velocity.magnitude;
        _camera.orthographicSize = Mathf.Min(MaxSize, _sizeDelta*(_prev/MaxSpeed) + StartSize);
        _prev = speed* ExponentialEasingFactor + _invExpFactor * _prev;
    }
}
