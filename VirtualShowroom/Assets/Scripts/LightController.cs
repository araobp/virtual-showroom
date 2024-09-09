using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightController : MonoBehaviour
{

    [SerializeField] float m_HSVSaturation = 0.25F;
    [SerializeField] float m_HSVValue = 1.0F;


    GameObject[] m_Lights;

    static Dictionary<string, string> MOOD_COLORS = new Dictionary<String, string>{
        {"Serene", "#87CEEB"},
        {"Bustling", "#FFA500"},
        {"Nostalgic", "#F4A460"},
        {"Lonely", "#708090"},
        {"Picturesque", "#ADD8E6"},
        {"Chaotic", "#8B0000"},
        {"Gloomy", "#2F4F4F"},
        {"Vibrant", "#FF4500"},
        {"Unsure", "#AAAAAA"}
    };

    // Start is called before the first frame update
    void Start()
    {
        m_Lights = GameObject.FindGameObjectsWithTag("Mood");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMood(string mood) {
        string htmlColor = MOOD_COLORS[mood];
        
        float r = int.Parse(htmlColor.Substring(1,2), System.Globalization.NumberStyles.HexNumber) / 255.0F;
        float g = int.Parse(htmlColor.Substring(3,2), System.Globalization.NumberStyles.HexNumber) / 255.0F;
        float b = int.Parse(htmlColor.Substring(5,2), System.Globalization.NumberStyles.HexNumber) / 255.0F;

        Color moodColorRGB = new Color(r, g, b);
        
        float h, s, v;
        Color.RGBToHSV(moodColorRGB, out h, out s, out v);

        // ... adjust v here ...

        if (s >= m_HSVSaturation) {
            s = m_HSVSaturation;
        }
        Color color = Color.HSVToRGB(h, s, m_HSVValue);

        Debug.Log($"{r}, {g}, {v}, {h}, {s}, {v}, {color.r}, {color.g}, {color.b}");

        m_Lights.ToList().ForEach(obj => {
            Light l = obj.GetComponent<Light>();
            l.color = color;
        });
    }

}
