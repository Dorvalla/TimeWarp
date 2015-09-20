﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingThemes.GUI;
using UnityEngine;
using ColossalFramework.UI;

namespace TimeWarpMod
{
    class SunGUI : UIPanel
    {

        UILabel timeOfDay;
        
        UILabel lattitude;
        UISlider lattitudeControl;

        UILabel longitude;
        UISlider longitudeControl;

        UISlider sunSize, sunIntensity;

        UILabel speed;
        UISlider speedControl;

        public SunController sunControl;

        private uint[] speeds = { 0, 1, 2, 4, 8, 16, 32, 64, 128 };

        public override void Awake()
        {
            size = new Vector2(200, 100);
            //gameObject.transform.localPosition = new Vector3(-1.7f,-0.6f,0.0f);
            anchor = UIAnchorStyle.Bottom & UIAnchorStyle.Left;
            backgroundSprite = "InfoPanelBack";

            UILabel title = AddUIComponent<UILabel>();
            title.text = "Day/Night Settings";
            title.textAlignment = UIHorizontalAlignment.Center;
            title.verticalAlignment = UIVerticalAlignment.Middle;
            title.textScale = 1.1f;
            title.autoSize = false;
            title.size = new Vector2(width - 20, 40);
                        
            autoLayoutPadding = new RectOffset(10, 10, 4, 4);
            autoLayout = true;
            autoFitChildrenVertically = true;
            autoLayoutDirection = LayoutDirection.Vertical;


            timeOfDay = AddUIComponent<UILabel>();

            //0, 1, 2, 4, 8, 16, 32, 64, 128 
            speed = AddUIComponent<UILabel>();
            speedControl = createSlider(0f, 8f);
           
            lattitude = AddUIComponent<UILabel>();
            lattitudeControl = createSlider(-80f,80f);

            longitude = AddUIComponent<UILabel>();
            longitudeControl = createSlider(-180f,180f);

            AddUIComponent<UILabel>().text = "Sun Size";
            sunSize = createSlider(0.01f, 10.0f);

            AddUIComponent<UILabel>().text = "Sun Intensity";
            sunIntensity = createSlider(0, 8f);
            sunIntensity.stepSize = 0.1f;


            UILabel endPadding = AddUIComponent<UILabel>();
            endPadding.text = "    ";
            
        }



        void ValueChanged(UIComponent component, float value)
        {
            Debug.Log("New Value: " + value);

            if (DayNightProperties.instance == null) return;

            if(component == lattitudeControl) {
                DayNightProperties.instance.m_Latitude = value;    
            }

            if(component == longitudeControl) {
                DayNightProperties.instance.m_Longitude = value;    
            }

            if (component == sunSize)
            {
                DayNightProperties.instance.m_SunSize = value;
            }

            if (component == sunIntensity)
            {
                DayNightProperties.instance.m_SunIntensity = value;
            }

            if (component == speedControl)
            {
                sunControl.speed = speeds[(uint)value];
            }
        }


        UISlider createSlider(float min, float max)
        {
            UISlider slider = AddUIComponent<UISlider>();
            slider.width = width - (autoLayoutPadding.left+autoLayoutPadding.right);
            slider.height = 17;
            slider.autoSize = false;
            slider.backgroundSprite = "OptionsScrollbarTrack";

            slider.maxValue = max;
            slider.minValue = min;

            UISprite thumb = slider.AddUIComponent<UISprite>();
            thumb.size = new Vector2(16, 16);
            thumb.position = new Vector2(0, 0);
            thumb.spriteName = "OptionsScrollbarThumb";

            slider.value = 0.0f;
            slider.thumbObject = thumb;


            slider.eventValueChanged += ValueChanged;
            return slider;
        }

        void Update()
        {

            if (DayNightProperties.instance != null) {

                lattitudeControl.value = DayNightProperties.instance.m_Latitude;
                lattitude.text = "Lattitude: " + Math.Floor(DayNightProperties.instance.m_Latitude) + "°";
                
                longitudeControl.value = DayNightProperties.instance.m_Longitude;
                longitude.text = "Longitude: " + Math.Floor(DayNightProperties.instance.m_Longitude) + "°";

                sunSize.value = DayNightProperties.instance.m_SunSize;
                sunIntensity.value = DayNightProperties.instance.m_SunIntensity;

                float tod = sunControl.TimeOfDay;

                int hour = (int)Math.Floor(tod);
                int minute = (int)Math.Floor((tod - hour) * 60.0f);

                timeOfDay.text = String.Format("Current Time: {0,2:00}:{1,2:00}", hour, minute);

                switch (sunControl.speed)
                {
                    case 0:
                        speed.text = "Speed: Paused";
                        break;
                    case 1:
                        speed.text = "Speed: Normal";
                        break;
                    default:
                        speed.text = "Speed: " + sunControl.speed + "x";
                        break;
                }

                speedControl.value = Array.IndexOf(speeds, sunControl.speed);
            }

            base.Update();
        }

    }
}
