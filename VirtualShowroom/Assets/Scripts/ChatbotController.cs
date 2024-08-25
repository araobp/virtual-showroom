using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ChatbotController : MonoBehaviour
{

    [SerializeField] Animator m_LadyBotAnimator;
    [SerializeField] Animator m_GentlemanBotAnimator;

    [SerializeField] Camera m_Camera1;
    [SerializeField] Camera m_Camera2;
    [SerializeField] Camera m_Camera3;
    [SerializeField] Camera m_Camera4;
    [SerializeField] Camera m_Camera5;
    [SerializeField] GameObject m_Screen;

    [SerializeField] Button m_ButtonCameraLeft;
    [SerializeField] Button m_ButtonCameraRight;

    [SerializeField] Button m_ButtonContentLeft;
    [SerializeField] Button m_ButtonContentRight;

    [SerializeField] Button m_ButtonModelLeft;
    [SerializeField] Button m_ButtonModelRight;

    [SerializeField] List<GameObject> m_Models;
    
    [SerializeField] Button m_ButtonToggleChatWindow;

    [SerializeField] GameObject m_ChatPanel;

    [SerializeField] TMP_Text m_Text;

    [SerializeField] TMP_InputField m_InputField;

    [SerializeField] string m_BaseUrl;

    List<Camera> m_Cameras;

    int m_ScreenIdx = 0;
    int m_CameraIdx = 0;

    int m_ModelIdx = 0;

    List<Object> m_Pictures;

    ChatAPI m_Api;

    const int TIME_TO_WORDS = 30;

    const int TIME_TO_SITDOWN = 1;

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
        m_ButtonCameraLeft.onClick.AddListener(() => CameraBack());
        m_ButtonCameraRight.onClick.AddListener(() => CameraForward());

        m_ButtonContentLeft.onClick.AddListener(() => ContentBack());
        m_ButtonContentRight.onClick.AddListener(() => ContentForward());

        m_ButtonModelLeft.onClick.AddListener(() => ModelBack());
        m_ButtonModelRight.onClick.AddListener(() => ModelForward());

        m_ButtonToggleChatWindow.onClick.AddListener(() => ToggleChatWindow());

        // Initializing virtual showroom set up
        SelectCamera(m_Cameras[0]);
        SelectContent();
        SelectModel();

        // Initializing API
        m_Api = GetComponent<ChatAPI>();
        m_Api.Init(m_BaseUrl);
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
    }

    // Update is called once per frame
    void Update()
    {

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

    void SelectContent() {
        Texture2D tex = (Texture2D)m_Pictures[m_ScreenIdx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);        
    }

    void ContentForward()
    {
        m_ScreenIdx += 1;
        if (m_ScreenIdx >= m_Pictures.Count - 1)
        {
            m_ScreenIdx = m_Pictures.Count - 1;
        }
        SelectContent();
    }

    void ContentBack()
    {
        m_ScreenIdx -= 1;
        if (m_ScreenIdx < 0)
        {
            m_ScreenIdx = 0;
        }
        SelectContent();
    }

    void SelectModel()
    {
        m_Models.ForEach(model => model.SetActive(false));
        m_Models[m_ModelIdx].SetActive(true);
        int layerIdx = Animator().GetLayerIndex("SitDownAndStandUp");
        if (Animator().GetCurrentAnimatorStateInfo(layerIdx).IsName("Idle")) {
            StartCoroutine(SitDown(TIME_TO_SITDOWN));
        }
    }

    void ModelForward(){
        Animator().Rebind();
        m_ModelIdx += 1;
        if (m_ModelIdx >= m_Models.Count - 1)
        {
            m_ModelIdx = m_Models.Count - 1;
        }
        SelectModel();
    }

    void ModelBack(){
        Animator().Rebind();
        m_ModelIdx -= 1;
        if (m_ModelIdx < 0)
        {
            m_ModelIdx = 0;
        }
        SelectModel();
    }

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

    Animator Animator() {
        return m_Models[m_ModelIdx].GetComponent<Animator>();
    }

    IEnumerator Speak(int period) {
        Animator().SetTrigger("speak");
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("stopSpeaking");
    }

    IEnumerator SitDown(int period) {
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("sitDown");
    }

    IEnumerator StandUp(int period) {
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("standUp");
    }

    public void OnEndEdit(string text)
    {
        m_Api.Chat(text, (err, resp) =>
        {
            if (err)
            {
                Debug.Log("Chat failed");
            }
            else
            {
                Debug.Log(resp.answer);
                int period = resp.answer.Length / TIME_TO_WORDS;
                StartCoroutine(Speak(period));
                string qa = $"Q: {text}\nA: {resp.answer}";
                m_Text.text = m_Text.text + "\n\n" + qa;
                m_InputField.text = "";
            }
        });
    }
}

