using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatbotController : RestClient
{

    [SerializeField] Animator m_LadyBotAnimator;
    [SerializeField] Camera m_Camera1;
    [SerializeField] Camera m_Camera2;
    [SerializeField] Camera m_Camera3;
    [SerializeField] Camera m_Camera4;
    [SerializeField] Camera m_Camera5;
    [SerializeField] GameObject m_Screen;

    List<Camera> m_Cameras;

    int m_Idx = 0;

    List<Object> m_Pictures;

    EndPoint m_EndPoint;

    // Flask API server base URL
    const string BASE_URL = "http://127.0.0.1:5000";

    void selectCamera(Camera camera)
    {
        m_Cameras.ForEach(c =>
        {
            if (c == camera)
            {
                c.gameObject.SetActive(true);
            }
            else
            {
                c.gameObject.SetActive(false);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Camera list
        m_Cameras = new List<Camera>(){m_Camera1, m_Camera2, m_Camera3, m_Camera4, m_Camera5};

        // 240-degree screen panorama pictures
        var pictures = Resources.LoadAll("Panorama", typeof(Texture2D)).ToList();
        m_Pictures = pictures.OrderBy(x => x.name).ToList();
        Texture2D tex = (Texture2D)m_Pictures[0];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);

        // REST API client init
        m_EndPoint = new EndPoint();
        m_EndPoint.baseUrl = BASE_URL;
        HelloAPI();

    }

    // Test API server
    void HelloAPI() {
        Get(m_EndPoint, "/", (err, text) => {
            Debug.Log(text);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        Keyboard.current.onTextInput += GetKeyInput;
    }

    void OnDisable()
    {
        Keyboard.current.onTextInput -= GetKeyInput;
    }

    public void Forward()
    {
        m_Idx += 1;
        if (m_Idx >= m_Pictures.Count - 1)
        {
            m_Idx = m_Pictures.Count - 1;
        }
        Texture2D tex = (Texture2D)m_Pictures[m_Idx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);
    }

    public void Back()
    {
        m_Idx -= 1;
        if (m_Idx < 0)
        {
            m_Idx = 0;
        }
        Texture2D tex = (Texture2D)m_Pictures[m_Idx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);
    }

    private void GetKeyInput(char obj)
    {
        Debug.Log(obj);
        switch (obj)
        {
            case '1':  // sitDown
                m_LadyBotAnimator.SetTrigger("sitDown");
                break;
            case '2':  // standUp
                m_LadyBotAnimator.SetTrigger("standUp");
                break;
            case '3':  // speak
                m_LadyBotAnimator.SetTrigger("speak");
                break;
            case '4':  // stopSpeaking
                m_LadyBotAnimator.SetTrigger("stopSpeaking");
                break;
            case '5':  // Camera1
                selectCamera(m_Camera1);
                break;
            case '6':  // Camera2
                selectCamera(m_Camera2);
                break;
            case '7':  // Camera3
                selectCamera(m_Camera3);
                break;
            case '8':  // Camera4
                selectCamera(m_Camera4);
                break;
            case '9':  // Camera5
                selectCamera(m_Camera5);
                break;
            case 'f':
                Forward();
                break;
            case 'b':
                Back();
                break;
        }
    }
}

