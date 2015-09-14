using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.Hud
{
    public class HudInterface : MonoBehaviour
    {				        
        public Text TotalTimeText;
		public RocketRegistry Registry;
        private RocketRaceMetrics _player1RaceMetrics;
		private RocketDamageSystem _player1DamageSystem;
		private RocketRaceMetrics _player2RaceMetrics;
		private RocketDamageSystem _player2DamageSystem;

        public Image Player1HealthBar;
		public Image Player2HealthBar;                      
        


        public float MaxDeltaHealthPerSecond = 0.4f;
                       
        // Use this for initialization
        private void Start()
        {
        
			_player1RaceMetrics=Registry.Rockets [0].GetComponent<RocketRaceMetrics> ();
			_player2RaceMetrics=Registry.Rockets [1].GetComponent<RocketRaceMetrics> ();
			_player1DamageSystem=Registry.Rockets [0].GetComponent<RocketDamageSystem> ();
			_player2DamageSystem=Registry.Rockets [1].GetComponent<RocketDamageSystem> ();

			Player1HealthBar.color = Registry.Rockets [0].GetComponent<SpriteRenderer>().color;
			Player2HealthBar.color = Registry.Rockets [1].GetComponent<SpriteRenderer>().color;
            
            _player1DamageSystem.AddOnDamageCallback(RocketDamagePlayer1);
            _player1DamageSystem.AddRespawnCallback(RocketRespawnPlayer1);						                     

			_player2DamageSystem.AddOnDamageCallback(RocketDamagePlayer2);
			_player2DamageSystem.AddRespawnCallback(RocketRespawnPlayer2);						                     
            _currentHealthPlayer1 = 1;
			_currentHealthPlayer2 = 1;
        }

        private float _currentHealthPlayer1;
		private float _currentHealthPlayer2;
        private float _nextHealthPlayer1;
		private float _nextHealthPlayer2;
        private float _progressPlayer1 = 0;
		private float _progressPlayer2 = 0;
        private float _deltaPlayer1;
		private float _deltaPlayer2;
       

		private void RocketDamagePlayer1(GameObject rocket, float damage, float remainingHealth)
        {   
            CancelInvoke("SetLife");
            _currentHealthPlayer1 = Mathf.Lerp(_currentHealthPlayer1, _nextHealthPlayer1, _progressPlayer1);
            _nextHealthPlayer1 = remainingHealth;
            _progressPlayer1 = 0;
            _deltaPlayer1 = Time.fixedDeltaTime/Mathf.Abs((_nextHealthPlayer1 - _currentHealthPlayer1) / MaxDeltaHealthPerSecond);
            
            InvokeRepeating("SetLife",0,Time.fixedDeltaTime);
        }

		private void RocketDamagePlayer2(GameObject rocket, float damage, float remainingHealth)
		{   
			CancelInvoke("SetLife");
			_currentHealthPlayer2 = Mathf.Lerp(_currentHealthPlayer2, _nextHealthPlayer2, _progressPlayer2);
			_nextHealthPlayer2 = remainingHealth;
			_progressPlayer2 = 0;
			_deltaPlayer2 = Time.fixedDeltaTime/Mathf.Abs((_nextHealthPlayer2 - _currentHealthPlayer2) / MaxDeltaHealthPerSecond);
			
			InvokeRepeating("SetLife",0,Time.fixedDeltaTime);
		}

        private void SetLife()
        {
            _progressPlayer1 += _deltaPlayer1;
            if (_progressPlayer1 > 1)
            {
                _progressPlayer1 = 1f;
				_deltaPlayer1=0;
            }

			_progressPlayer2 += _deltaPlayer2;
			if (_progressPlayer2 > 1)
			{
				_progressPlayer2 = 1f;
				_deltaPlayer2=0;
			}

            var remainingHealthP1 = Mathf.Lerp(_currentHealthPlayer1, _nextHealthPlayer1, _progressPlayer1);
			var remainingHealthP2 = Mathf.Lerp(_currentHealthPlayer2, _nextHealthPlayer2, _progressPlayer2);
			Player1HealthBar.fillAmount = remainingHealthP1;
			Player2HealthBar.fillAmount = remainingHealthP2;

            if (_progressPlayer1 >= 1f)
            {
                _currentHealthPlayer1 = _nextHealthPlayer1;                
             
            }
			if(_progressPlayer1>=1f && _progressPlayer2>=1f){
				CancelInvoke("SetLife");
			}
        }

        private void RocketRespawnPlayer1(GameObject rocket,Vector2 other)
        {
            CancelInvoke("SetLife");
            _currentHealthPlayer1 = 0f;
            _nextHealthPlayer1 = 1f;
            _progressPlayer1 = 0;
            _deltaPlayer1 =4* Time.fixedDeltaTime / Mathf.Abs((_nextHealthPlayer1 - _currentHealthPlayer1) / MaxDeltaHealthPerSecond);            
            InvokeRepeating("SetLife", 0, Time.fixedDeltaTime);
        }

		
		private void RocketRespawnPlayer2(GameObject rocket,Vector2 other)
		{
			CancelInvoke("SetLife");
			_currentHealthPlayer2 = 0f;
			_nextHealthPlayer2 = 1f;
			_progressPlayer2 = 0;
			_deltaPlayer2 =4* Time.fixedDeltaTime / Mathf.Abs((_nextHealthPlayer2 - _currentHealthPlayer2) / MaxDeltaHealthPerSecond);            
			InvokeRepeating("SetLife", 0, Time.fixedDeltaTime);
		}

        // Update is called once per frame
        private void Update()
        {            
            TotalTimeText.text = StaticUtils.ToRaceTimeString(_player1RaceMetrics.TotalTime);            
        }


    }
}
