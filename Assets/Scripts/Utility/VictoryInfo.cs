using System;
using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;

namespace RealRocketRacing.Rocket
{
    public class VictoryInfo : MonoBehaviour
    {
        public float LoadingTime;
        public TimeSpan Time { get; set; }
        public Vector2 Velocity{ get;private set; }
        public float Rotation{ get; private set; }
        public Rigidbody2D RocketRigidBody;
        public UnityEngine.UI.Image FadeImage;
        public RocketController.Players WinningPlayer { get; set; }
        private static VictoryInfo Info=null;

        // Use this for initialization
        private void Start()
        {
            if (Info == null)
            {
                Info = this;
                GameObject.DontDestroyOnLoad(this.gameObject);
                Invoke("LoadScoreScreen", LoadingTime);
                InvokeRepeating("FadeOut", 0,1/60f);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        private float _progress=0;
        private void FadeOut()
        {
            _progress += (1/60f)/LoadingTime;
            if (_progress >= 1f)
            {
                _progress = 1f;
                CancelInvoke("FadeOut");
                InvokeRepeating("FadeIn",1/60f,1/60f);
            }
            FadeImage.color=new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b,_progress);
        }

        private void FadeIn()
        {
            _progress -= (1 / 60f) / LoadingTime;
            if (_progress <= 0f)
            {
                _progress = 0;
                CancelInvoke("FadeIn");
                FadeImage.color = Color.clear;
            }
            else
            {
                FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, _progress);
            }
        }
        
        private void LoadScoreScreen()
        {
            Velocity = RocketRigidBody.velocity;
            Rotation = RocketRigidBody.rotation;
            RocketRigidBody = null;
            Debug.Log("Loading Level");
            Application.LoadLevelAsync("ScoreScreen");
        }

    }
}