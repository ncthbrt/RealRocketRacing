using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace RealRocketRacing.Rocket
{
    public class RocketController : MonoBehaviour {
		public enum Players{Player1,Player2};

        private PlayerIndex _playerIndex=PlayerIndex.One;        
        public Players RocketPlayer=Players.Player1;

        public GameObject[] PrimaryThrusters;
        public GameObject[] LeftThrusters;
        public GameObject[] RightThrusters;

        public float LargeDamageShakeDuration;
        [Range(0,1)]
        public float LargeDamageShakeLeftIntensity;
        [Range(0, 1)]
        public float LargeDamageShakeRightIntensity;

        public float MediumDamageShakeDuration;
        [Range(0, 1)]
        public float MediumDamageShakeLeftIntensity;
        [Range(0, 1)]
        public float MediumDamageShakeRightIntensity;

        public float SmallDamageShakeDuration;
        [Range(0, 1)]
        public float SmallDamageShakeLeftIntensity;
        [Range(0, 1)]
        public float SmallDamageShakeRightIntensity;



        public float SmallDamage=0.01f;
        public float MediumDamage=0.07f;
        public float LargeDamage=0.5f;


        public float DeathShakeDuration;
        public float DeathShakeLeftIntensity;
        public float DeathShakeRightIntensity;
        // Use this for initialization

        public bool ControlsEnabled=false;

        private bool _primaryThrusterOn=false;
        private bool _leftThrusterOn=false;
        private bool _rightThrusterOn=false;


		private string _up = "up";
		private string _left = "left";
		private string _right  = "right";        
		
	//	private string _thrustButton = "Fire1";
	//	private string _steeringAxis = "Horizontal";
    		       


        void Start()
        {        
			Reset();
            var damageSys=GetComponent<RocketDamageSystem>();
            damageSys.AddOnDamageCallback(RocketDamage);
            damageSys.AddRespawnCallback(Respawn);
        }

        public void Reset()
        {
            if (RocketPlayer == Players.Player2)
            {
                _up = "w";
                _right = "d";
                _left = "a";
                _playerIndex = PlayerIndex.Two;
            }
            else
            {
                _up = "up";
                _left = "left";
                _right = "right";
                _playerIndex = PlayerIndex.One;
            }
        }

        

        public void RocketDamage(GameObject rocket, float damage, float remainingHealth)
        {            
            if (GamePad.GetState(_playerIndex).IsConnected)
            {
                if (damage >= LargeDamage)
                {                    
                    Vibrate(Easing.Linear, LargeDamageShakeDuration, LargeDamageShakeLeftIntensity,LargeDamageShakeRightIntensity);                    
                }else if (damage >= MediumDamage)
                {                 
                    Vibrate(Easing.Linear, MediumDamageShakeDuration, MediumDamageShakeLeftIntensity, MediumDamageShakeRightIntensity);
                }
                else if(damage>=SmallDamage)
                {                    
                    Vibrate(Easing.Linear, SmallDamageShakeDuration, SmallDamageShakeLeftIntensity, SmallDamageShakeRightIntensity);
                }
            }            
        }


        public void Respawn(GameObject rocket, Vector2 contactPoint)
        {


                ThrusterStateChange(false, ThrusterGroup.PRIMARY);
                ThrusterStateChange(false, ThrusterGroup.LEFT);            
                ThrusterStateChange(false, ThrusterGroup.RIGHT);

                if (GamePad.GetState(_playerIndex).IsConnected)
                {
                    Vibrate(Easing.Linear, DeathShakeDuration,  DeathShakeLeftIntensity, DeathShakeRightIntensity);
                }
        }



        // Update is called once per frame
        void Update ()
        {
			if (Input.GetKey ("r")) {
				Application.LoadLevel("Loading");
			}
            
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

        private float _vibrationProgress = 0;
        private float _vibrationTime = 0;
        private float _vibrationLeftStart = 0;
        private float _vibrationRightStart = 0;
        private EasingFunction _vibrationFunction;

        public delegate double EasingFunction(double t, double start, double end, double duration);
        public void Vibrate(EasingFunction function, float duration, float leftIntensity, float rightIntensity)
        {
            GamePad.SetVibration(_playerIndex, leftIntensity, rightIntensity);
            CancelInvoke("StopVibrate");
            CancelInvoke("VibrationEase");
            if (function == null)
            {
                Invoke("StopVibrate", duration);
            }
            else
            {
                _vibrationFunction = function;
                _vibrationProgress = 0;
                _vibrationTime = duration;
                _vibrationLeftStart = leftIntensity;
                _vibrationRightStart = rightIntensity;
                InvokeRepeating("VibrationEase", 0, 1 / 60f);
            }
        }

        private void StopVibrate()
        {
            GamePad.SetVibration(_playerIndex, 0, 0);
        }

        private void VibrationEase()
        {
            _vibrationProgress += (1 / 60f) / _vibrationTime;
            if (_vibrationProgress >= 1)
            {
                CancelInvoke("VibrationEase");
                StopVibrate();
                return;
            }
            float progress = (float)(1f - _vibrationFunction(_vibrationProgress, 0, 1, 1));
            var left = progress * _vibrationLeftStart;
            var right = progress * _vibrationRightStart;
            GamePad.SetVibration(_playerIndex, left, right);
        }

        public float AxisThreshold=0.3f;
        void HandleControllerInput()
        {

            GamePadState gamePadState = GamePad.GetState(_playerIndex);
            bool left=false;
            bool right = false;
            bool thrustPressed=false;

            if (gamePadState.IsConnected)
            {
                float horizontal = gamePadState.ThumbSticks.Left.X;
                if (horizontal >= AxisThreshold)
                {
                    right = true;
                    left = false;
                }else if (horizontal < -AxisThreshold)
                {
                    right = false;
                    left = true;                    
                }
                else
                {
                    right = false;
                    left  = false;
                }
                thrustPressed = gamePadState.Buttons.A == ButtonState.Pressed;
            }
            else
            {
                thrustPressed = Input.GetKey(_up);
                left = Input.GetKey(_left);
                right = Input.GetKey(_right);
            }

            if (!_primaryThrusterOn && thrustPressed)
            {
                _primaryThrusterOn = true;
            }
            else if (_primaryThrusterOn && !thrustPressed)
            {
                _primaryThrusterOn = false;
            }


            if (!_leftThrusterOn && right)
            {
                _leftThrusterOn = true;
            }
            else if (_leftThrusterOn && !right)
            {
                _leftThrusterOn = false;
            }

            if (!_rightThrusterOn && left)
            {
                _rightThrusterOn = true;
            }
            else if (_rightThrusterOn && !left)
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
                    thrusterPhysicsScript.ApplyThrust(GetComponent<Rigidbody2D>());
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

