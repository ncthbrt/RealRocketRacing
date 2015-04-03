using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class RocketController : MonoBehaviour {

        public GameObject[] PrimaryThrusters;
        public GameObject[] LeftThrusters;
        public GameObject[] RightThrusters;
        // Use this for initialization
        
        public bool ControlsEnabled { get; set; } 

        private bool _primaryThrusterOn=false;
        private bool _leftThrusterOn=false;
        private bool _rightThrusterOn=false;
    

        private Rigidbody2D rigidbody2D;


        void Start()
        {
            ControlsEnabled = false;
        }
        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update ()
        {

            if (ControlsEnabled)
            {
                bool lastMain = _primaryThrusterOn;
                bool lastLeft = _leftThrusterOn;
                bool lastRight = _rightThrusterOn;

                HandleControllerInput();

                if (lastMain ^ _primaryThrusterOn)
                {
                    ThrusterStateChange(_primaryThrusterOn, ThrusterGroup.PRIMARY);
                }
                if (lastLeft ^ _leftThrusterOn)
                {
                    ThrusterStateChange(_leftThrusterOn, ThrusterGroup.LEFT);
                }
                if (lastRight ^ _rightThrusterOn)
                {
                    ThrusterStateChange(_rightThrusterOn, ThrusterGroup.RIGHT);
                }
            }
        }

        public enum ThrusterGroup
        {
            PRIMARY,
            LEFT,
            RIGHT
        };

        private void ThrusterStateChange(bool on, ThrusterGroup group)
        {

            switch (group)
            {
                case ThrusterGroup.PRIMARY:
                    if (on && _primaryThrusterOnCallback!= null)
                    {
                        _primaryThrusterOnCallback();

                    }
                    else if (_primaryThrusterOffCallback!= null)
                    {
                        _primaryThrusterOffCallback(); ;
                    }
                    break;
                case ThrusterGroup.LEFT:
                    if (on && _leftThrusterOnCallback != null)
                    {
                        _leftThrusterOnCallback();

                    }
                    else if (_leftThrusterOffCallback != null)
                    {
                        _leftThrusterOffCallback(); ;
                    }
                    break;
                case ThrusterGroup.RIGHT:
                    if (on && _rightThrusterOnCallback != null)
                    {
                        _rightThrusterOnCallback();

                    }
                    else if (_rightThrusterOffCallback!= null)
                    {
                        _rightThrusterOffCallback(); ;
                    }
                    break;                
            }
         
        }

        void HandleControllerInput()
        {
            if (!_primaryThrusterOn && Input.GetKey("up"))
            {
                _primaryThrusterOn = true;
            }
            else if (_primaryThrusterOn && !Input.GetKey("up"))
            {
                _primaryThrusterOn = false;            
            }


            if (!_leftThrusterOn && Input.GetKey("right"))
            {
                _leftThrusterOn = true;
            }
            else if (_leftThrusterOn && !Input.GetKey("right"))
            {
                _leftThrusterOn = false;            
            }

            if (!_rightThrusterOn && Input.GetKey("left"))
            {
                _rightThrusterOn = true;            
            }
            else if (_rightThrusterOn && !Input.GetKey("left"))
            {
                _rightThrusterOn = false;            
            }
        }


        public delegate void ThrustChangeCallback ();
   

        private ThrustChangeCallback _primaryThrusterOnCallback;
        private ThrustChangeCallback _primaryThrusterOffCallback;
        private ThrustChangeCallback _leftThrusterOnCallback;
        private ThrustChangeCallback _rightThrusterOnCallback;
        private ThrustChangeCallback _leftThrusterOffCallback;
        private ThrustChangeCallback _rightThrusterOffCallback;


        public void AddLeftThrustOnCallback(ThrustChangeCallback callback)
        {
            _leftThrusterOnCallback += callback;
        }

        public void AddRightThrustOnCallback(ThrustChangeCallback callback)
        {
            _rightThrusterOnCallback += callback;
        }

        public void AddLeftThrustOffCallback(ThrustChangeCallback callback)
        {
            _leftThrusterOffCallback+= callback;
        }

        public void AddRightThrustOffCallback(ThrustChangeCallback callback)
        {
            _rightThrusterOffCallback+= callback;
        }


        public void AddPrimaryThurstOnCallback(ThrustChangeCallback callback)
        {
            _primaryThrusterOnCallback += callback;
        }
        public void AddPrimaryThurstOffCallback(ThrustChangeCallback callback)
        {
            _primaryThrusterOffCallback+= callback;
        }
    
    
        private void ApplyThrust(IEnumerable<GameObject> thrusters)
        {
            foreach (var thruster in thrusters)
            {            
                ThrusterPhysics thrusterPhysicsScript = thruster.GetComponent<ThrusterPhysics>();
                if (thrusterPhysicsScript != null)
                {
                    thrusterPhysicsScript.ApplyThrust(rigidbody2D);
                }
            }        
        }


        void FixedUpdate()
        {
            if (ControlsEnabled)
            {
                if (_primaryThrusterOn)
                {
                    ApplyThrust(PrimaryThrusters);
                }
                if (_leftThrusterOn)
                {
                    ApplyThrust(LeftThrusters);
                }
                if (_rightThrusterOn)
                {
                    ApplyThrust(RightThrusters);
                }
            }
        }
    }
}

