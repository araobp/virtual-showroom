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
    [SerializeField] GameObject m_Cameras;

    [SerializeField] GameObject m_Screen;

    [SerializeField] List<GameObject> m_Models;

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

    [SerializeField] string m_BaseUrl;


    List<Camera> m_CameraList;
    List<Object> m_ContentList;

    int m_CameraIdx = 0;
    int m_ContentIdx = 0;
    int m_ModelIdx = 0;

    const int TIME_TO_WORDS = 30;
    const int TIME_TO_SITDOWN = 1;

    ChatAPI m_Api;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Camera list
        m_CameraList = m_Cameras.GetComponentsInChildren<Camera>().ToList();

        // 240-degree screen panorama pictures
        var pictures = Resources.LoadAll("Panorama", typeof(Texture2D)).ToList();
        m_ContentList = pictures.OrderBy(x => x.name).ToList();
        Texture2D tex = (Texture2D)m_ContentList[0];
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
        SelectCamera(m_CameraList[0]);
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

    //*** Camera selection ***
    void SelectCamera(Camera camera)
    {
        m_CameraList.ForEach(c =>
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
        if (m_CameraIdx >= m_CameraList.Count - 1)
        {
            m_CameraIdx = m_CameraList.Count - 1;
        }

        SelectCamera(m_CameraList[m_CameraIdx]);
    }

    void CameraBack()
    {
        m_CameraIdx -= 1;
        if (m_CameraIdx < 0)
        {
            m_CameraIdx = 0;
        }

        SelectCamera(m_CameraList[m_CameraIdx]);
    }

    //*** Content selection ***
    void SelectContent()
    {
        Texture2D tex = (Texture2D)m_ContentList[m_ContentIdx];
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", tex);
    }

    void ContentForward()
    {
        m_ContentIdx += 1;
        if (m_ContentIdx >= m_ContentList.Count - 1)
        {
            m_ContentIdx = m_ContentList.Count - 1;
        }
        SelectContent();
    }

    void ContentBack()
    {
        m_ContentIdx -= 1;
        if (m_ContentIdx < 0)
        {
            m_ContentIdx = 0;
        }
        SelectContent();
    }

    //*** Promotional model selection ***
    void SelectModel()
    {
        m_Models.ForEach(model => model.SetActive(false));
        m_Models[m_ModelIdx].SetActive(true);
        int layerIdx = Animator().GetLayerIndex("SitDownAndStandUp");
        if (Animator().GetCurrentAnimatorStateInfo(layerIdx).IsName("Idle"))
        {
            StartCoroutine(SitDown(TIME_TO_SITDOWN));
        }
    }

    void ModelForward()
    {
        Animator().Rebind();
        m_ModelIdx += 1;
        if (m_ModelIdx >= m_Models.Count - 1)
        {
            m_ModelIdx = m_Models.Count - 1;
        }
        SelectModel();
    }

    void ModelBack()
    {
        Animator().Rebind();
        m_ModelIdx -= 1;
        if (m_ModelIdx < 0)
        {
            m_ModelIdx = 0;
        }
        SelectModel();
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

    //*** Animations ***
    Animator Animator()
    {
        return m_Models[m_ModelIdx].GetComponent<Animator>();
    }

    IEnumerator Speak(int period)
    {
        Animator().SetTrigger("speak");
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("stopSpeaking");
    }

    IEnumerator SitDown(int period)
    {
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("sitDown");
    }

    IEnumerator StandUp(int period)
    {
        yield return new WaitForSecondsRealtime(period);
        Animator().SetTrigger("standUp");
    }
}

