using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HudInterface : MonoBehaviour
    {


        public Text LapCountText;

        public Text LapTimeText;

        public Text TotalTimeText;

        public RocketRaceMetrics RaceMetrics;
        public RocketDamageSystem DamageSystem;

        public Image HealthRight;
        public Image HealthLeft;
        public Image HealthCenter;
        
        public Image BorderCenter;
        public Image TemplateLife;
        public float MaxDeltaHealthPerSecond = 0.4f;
        

        public float LifeIndicatorSpacing = 32;
        public Color HealthBarStartColor;
        public Color HealthBarEndColor;
        // Use this for initialization
        private void Start()
        {
        
            RaceMetrics.AddLapCompleteCallback(LapComplete);
            LapCountText.text = (RaceMetrics.LapCount + 1) + "/" + RaceMetrics.NumberOfLaps;

            HealthRight.color = HealthBarStartColor;
            HealthLeft.color = HealthBarStartColor;
            HealthCenter.color = HealthBarStartColor;
            DamageSystem.AddOnDamageCallback(RocketDamage);
            DamageSystem.AddRespawnCallback(RocketRespawn);

            _canvas= GetComponent<Canvas>();
            _lifeIcons=new LinkedList<Image>();
            for (int i = 0; i < DamageSystem.LivesAtStart; i++)
            {
                AddLife();
            }
            _lifeIcons.Last.Value.color = HealthBarStartColor;
            _currentHealth = 1;
        }

        private Canvas _canvas;

        private LinkedList<Image> _lifeIcons;

        private void AddLife()
        {
            var lifeIcon = (Instantiate(TemplateLife, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as Image);
            var rectTransform=lifeIcon.rectTransform;
            var image = lifeIcon;
            rectTransform.SetParent(BorderCenter.rectTransform);
            rectTransform.pivot=new Vector2(0,1);
            
            
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,_lifeIcons.Count*(LifeIndicatorSpacing)*rectTransform.localScale.x,image.preferredWidth);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, image.preferredHeight);
            _lifeIcons.AddLast(image);                                                           
        }

        private void RemoveLife()
        {
            if (_lifeIcons.Count > 0)
            {
                var last = _lifeIcons.Last;
                _lifeIcons.RemoveLast();
                Destroy(last.Value);
            }
        }


        private float _currentHealth;
        private float _nextHealth;
        private float _progress = 0;
        private float _delta;
        private void RocketDamage(GameObject rocket, float damage, float remainingHealth)
        {   
            CancelInvoke("SetLife");
            _currentHealth = Mathf.Lerp(_currentHealth, _nextHealth, _progress);
            _nextHealth = remainingHealth;
            _progress = 0;
            _delta = Time.fixedDeltaTime/Mathf.Abs((_nextHealth - _currentHealth) / MaxDeltaHealthPerSecond);
            
            InvokeRepeating("SetLife",0,Time.fixedDeltaTime);
        }

        private void SetLife()
        {
            _progress += _delta;
            if (_progress > 1)
            {
                _progress = 1f;
            }
            var remainingHealth = Mathf.Lerp(_currentHealth, _nextHealth, _progress);
            
            Color healthBarColor = Color.Lerp(HealthBarEndColor, HealthBarStartColor,remainingHealth);

            HealthRight.color = healthBarColor;
            HealthLeft.color = healthBarColor;
            HealthCenter.color = healthBarColor;

            var healthIcon = _lifeIcons.LastOrDefault();
            if (healthIcon != null)
            {
                healthIcon.color = healthBarColor;
            }

            float healthLeftWidth = HealthLeft.GetPixelAdjustedRect().width;
            float healthCenterWidth = HealthCenter.GetPixelAdjustedRect().width;
            float healthRightWidth = HealthRight.GetPixelAdjustedRect().width;
            float healthBarLength = healthLeftWidth + healthRightWidth + healthCenterWidth;

            var healthBarStartRight = healthLeftWidth + healthCenterWidth;
            var healthBarStartCenter = healthLeftWidth;

            float remainingHealthBarLength = remainingHealth * healthBarLength;


            float remainingRightLength = remainingHealthBarLength - healthBarStartRight;

            if (remainingRightLength < 0)
            {
                remainingRightLength = 0;
            }

            float remainingCenterLength = remainingHealthBarLength - healthBarStartCenter;

            if (remainingCenterLength > healthCenterWidth)
            {
                remainingCenterLength = healthCenterWidth;
            }
            else if (remainingCenterLength < 0)
            {
                remainingCenterLength = 0;
            }

            float remainingLeftLength = remainingHealthBarLength;
            if (remainingLeftLength > healthLeftWidth)
            {
                remainingLeftLength = healthLeftWidth;
            }

            HealthLeft.fillAmount = remainingLeftLength / healthLeftWidth;
            HealthRight.fillAmount = remainingRightLength / healthRightWidth;
            HealthCenter.fillAmount = remainingCenterLength / healthCenterWidth;
            if (_progress >= 1f)
            {
                _currentHealth = _nextHealth;                
                CancelInvoke("SetLife");
            }
        }

        private void RocketRespawn(GameObject rocket,Collision2D other)
        {
            CancelInvoke("SetLife");
            _currentHealth = 0f;
            _nextHealth = 1f;
            _progress = 0;
            _delta =4* Time.fixedDeltaTime / Mathf.Abs((_nextHealth - _currentHealth) / MaxDeltaHealthPerSecond);            
            RemoveLife();
            InvokeRepeating("SetLife", 0, Time.fixedDeltaTime);
        }

        void LapComplete(GameObject rocket, TimeSpan lapTime, int lapNumber)
        {
            LapCountText.text= (RaceMetrics.LapCount+2)+"/"+RaceMetrics.NumberOfLaps;
        }

        // Update is called once per frame
        private void Update()
        {            
            TotalTimeText.text = StaticUtils.ToRaceTimeString(RaceMetrics.TotalTime);
            LapTimeText.text = StaticUtils.ToRaceTimeString(RaceMetrics.CurrentLapTime);            
        }


    }
}
