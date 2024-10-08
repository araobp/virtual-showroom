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
        {"serene", "#87CEEB"},
        {"bustling", "#FFA500"},
        {"nostalgic", "#F4A460"},
        {"lonely", "#708090"},
        {"picturesque", "#ADD8E6"},
        {"chaotic", "#8B0000"},
        {"gloomy", "#2F4F4F"},
        {"vibrant", "#FF4500"},
        {"unsure", "#AAAAAA"}
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
