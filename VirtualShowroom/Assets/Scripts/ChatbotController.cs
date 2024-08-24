using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatbotController: MonoBehaviour
{

    [SerializeField] Animator m_LadyBotAnimator;
    [SerializeField] Camera m_Camera1;
    [SerializeField] Camera m_Camera2;
    [SerializeField] Camera m_Camera3;
    [SerializeField] Camera m_Camera4;
    [SerializeField] Camera m_Camera5;
    [SerializeField] GameObject m_Screen;

    [SerializeField] Button m_ArrowLeftButton;
    [SerializeField] Button m_ArrowRightButton;

    [SerializeField] Button m_ChatToggleButton;

    [SerializeField] GameObject m_ChatPanel;

    [SerializeField] TMP_Text m_Text;

    [SerializeField] TMP_InputField m_InputField;

    [SerializeField] string m_BaseUrl;

    List<Camera> m_Cameras;

    int m_ScreenIdx = 0;
    int m_CameraIdx = 0;

    List<Object> m_Pictures;

    ChatAPI api;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Camera list
        m_Cameras = new List<Camera>() { m_Camera1, m_Camera2, m_Camera3, m_Camera4, m_Camera5 };

        // 240-degree screen panorama pictures
        var pictures = Resources.LoadAll("Panorama", typeof(Texture2D)).ToList();
        m_Pictures = pictures.OrderBy(x => x.name).ToList();
        Texture2D tex = (Texture2D)m_Pictures[0];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);

        // Chat Window initial state
        m_ChatPanel.SetActive(false);

        // Operation buttons
        m_ArrowLeftButton.onClick.AddListener(() => OnButtonClick("left"));
        m_ArrowRightButton.onClick.AddListener(() => OnButtonClick("right"));
        m_ChatToggleButton.onClick.AddListener(() => OnButtonClick("toggle"));

        // API
        api = GetComponent<ChatAPI>();
        api.Init(m_BaseUrl);
        api.Hello((err, text) => {
            if (err) {
                Debug.LogWarning("API server not running!");
            } else {
                Debug.Log(text);
            }
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

    void OnButtonClick(string operation)
    {
        switch(operation) {
            case "left":
                CameraBack();
                break;
            case "right":
                CameraForward();
                break;
            case "toggle":
                ToggleChatWindow();
                break;
        }
    }

    void SelectCamera(Camera camera)
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

    void CameraForward()
    {
        m_CameraIdx += 1;
        if (m_CameraIdx >= m_Cameras.Count - 1)
        {
            m_CameraIdx = m_Cameras.Count - 1;
        }

        SelectCamera(m_Cameras[m_CameraIdx]);
    }

    void CameraBack()
    {
        m_CameraIdx -= 1;
        if (m_CameraIdx < 0)
        {
            m_CameraIdx = 0;
        }

        SelectCamera(m_Cameras[m_CameraIdx]);
    }

    void ScreenForward()
    {
        m_ScreenIdx += 1;
        if (m_ScreenIdx >= m_Pictures.Count - 1)
        {
            m_ScreenIdx = m_Pictures.Count - 1;
        }
        Texture2D tex = (Texture2D)m_Pictures[m_ScreenIdx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);
    }

    void ScreenBack()
    {
        m_ScreenIdx -= 1;
        if (m_ScreenIdx < 0)
        {
            m_ScreenIdx = 0;
        }
        Texture2D tex = (Texture2D)m_Pictures[m_ScreenIdx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);
    }

    void ToggleChatWindow() {
        if (m_ChatPanel.activeSelf) {
            m_ChatPanel.SetActive(false);
        } else {
            m_ChatPanel.SetActive(true);   
        }
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
            case 'f':
                ScreenForward();
                break;
            case 'b':
                ScreenBack();
                break;
        }
    }
}

