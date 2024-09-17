using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetectionController : MonoBehaviour
{
    [SerializeField] bool m_TTSEnabled = false;

    [SerializeField] GameObject m_Screen;

    [SerializeField] RenderTexture m_RobotCameraRenderTexture;

    [SerializeField] Button m_ButtonCameraLeft;
    [SerializeField] Button m_ButtonCameraRight;

    [SerializeField] Button m_ButtonContentLeft;
    [SerializeField] Button m_ButtonContentRight;

    [SerializeField] Button m_ButtonModelLeft;
    [SerializeField] Button m_ButtonModelRight;

    [SerializeField] Button m_ButtonToggleChatWindow;

    [SerializeField] GameObject m_ChatPanel;

    [SerializeField] TMP_Text m_Text;

    [SerializeField] TMP_InputField m_InputField;

    [SerializeField] Voices m_Voice = Voices.alloy;

    AudioSource m_AudioSource;
    enum Voices { alloy, nova };  // OpenAI's Text-to-Speech voices

    ChatAPI m_Api;

    List<Texture2D> m_ContentList;
    int m_ContentIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<UnityEngine.Object> pictures = Resources.LoadAll("LargeDisplay", typeof(Texture2D)).ToList();
        m_ContentList = pictures.OrderBy(x => x.name).ToList().ConvertAll(x => (Texture2D)x);
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", m_ContentList[0]);

        // Chat Window initial state
        m_ChatPanel.SetActive(false);

        // Operation buttons
        /*
        m_ButtonCameraLeft.onClick.AddListener(() => CameraBack());
        m_ButtonCameraRight.onClick.AddListener(() => CameraForward());

        m_ButtonContentLeft.onClick.AddListener(() => ContentBack());
        m_ButtonContentRight.onClick.AddListener(() => ContentForward());

        m_ButtonModelLeft.onClick.AddListener(() => ModelBack());
        m_ButtonModelRight.onClick.AddListener(() => ModelForward());
        */
        m_ButtonToggleChatWindow.onClick.AddListener(() => ToggleChatWindow());
        

        // Initializing API
        m_Api = GetComponent<ChatAPI>();

        m_Api.Hello((err, text) =>
        {
            if (err)
            {
                Debug.LogWarning("API server not running!");
            }
            else
            {
                Debug.Log(text);
            }
        });

        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //*** Chat operations ***
    void ToggleChatWindow()
    {
        if (m_ChatPanel.activeSelf)
        {
            m_ChatPanel.SetActive(false);
        }
        else
        {
            m_ChatPanel.SetActive(true);
        }
    }

    public void OnEndEdit(string text)
    {
        text = m_InputField.text;  // TODO: I don't know why OnEndEdit's argument value is always blank.
        
        void ProcessChatResponse(ChatResponse resp)
        {
            Debug.Log(resp.answer);

            string qa = $"Q: {text}\nA: {resp.answer}";
            m_Text.text = m_Text.text + "\n\n" + qa;
            m_InputField.text = "";

            if (m_TTSEnabled)
            {
                StartCoroutine(m_Api.TextToSpeech(m_AudioSource, m_Voice.ToString(), resp.answer));
            }
        }


        // Read texture data from RenderTexture
        RenderTexture rt = m_RobotCameraRenderTexture;
        RenderTexture.active = rt;
        Texture2D texture2d = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        texture2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        // Encode it to JPEG
        byte[] bytes;
        bytes = texture2d.EncodeToJPG();

        // Convert it to Base64
        string b64image = Convert.ToBase64String(bytes);

        m_Api.ObjectDetectionChatTextAndImage(text, b64image, (err, resp) =>
            {
                if (err)
                {
                    Debug.Log("Chat failed");
                }
                else
                {
                    ProcessChatResponse(resp);
                }
            });
    }

}
