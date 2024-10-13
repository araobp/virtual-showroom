using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Button = UnityEngine.UI.Button;

public class VirtualShowroomController : MonoBehaviour
{
    enum RagMode { TEXT_ONLY, TEXT_AND_IMAGE, TEXT_AND_IMAGE_AND_MOOD };
    [SerializeField] RagMode m_RagMode = RagMode.TEXT_AND_IMAGE;
    [SerializeField] bool m_TTSEnabled = false;

    [SerializeField] int m_ResizedImageHeight = 256;  // 256px is just for development
    [SerializeField] Boolean m_AR_Mode = false;
    [SerializeField] GameObject m_Cameras;

    [SerializeField] GameObject m_Screen;

    [SerializeField] List<GameObject> m_Models;

    [SerializeField] TMP_Text m_TextCamera;
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


    List<Camera> m_CameraList;
    List<Texture2D> m_ContentList;

    int m_CameraIdx = 0;
    int m_ContentIdx = 0;
    int m_ModelIdx = 0;

    const int TIME_TO_WORDS = 30;
    const int TIME_TO_SITDOWN = 1;

    ChatAPI m_Api;

    Resizer m_Resizer;

    LightController m_LightController;

    AudioSource m_AudioSource;
    enum Voices { alloy, nova };  // OpenAI's Text-to-Speech voices


    void Start()
    {
        if (m_ResizedImageHeight > Constants.MAX_RESIZED_IMAGE_HIGHT)
        {
            m_ResizedImageHeight = Constants.MAX_RESIZED_IMAGE_HIGHT;
        }

        // Select either AR app mode or console app mode
        GameObject.FindAnyObjectByType<ARSession>().gameObject.SetActive(m_AR_Mode);
        GameObject.FindAnyObjectByType<XROrigin>().gameObject.SetActive(m_AR_Mode);

        // Set screen orientation to landscape
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        // 240-degree screen panorama pictures
        List<UnityEngine.Object> pictures = Resources.LoadAll("Panorama", typeof(Texture2D)).ToList();
        m_ContentList = pictures.OrderBy(x => x.name).ToList().ConvertAll(x => (Texture2D)x);
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", m_ContentList[0]);

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

        // Camera list
        m_CameraList = m_Cameras.GetComponentsInChildren<Camera>().ToList();

        // Disable all the cameras and the camera selection buttons in case of AR mode is true
        if (m_AR_Mode)
        {
            m_CameraList.ForEach(c => c.gameObject.SetActive(false));

            m_TextCamera.gameObject.SetActive(false);
            m_ButtonCameraLeft.gameObject.SetActive(false);
            m_ButtonCameraRight.gameObject.SetActive(false);
        } else {
            SelectCamera(m_CameraList[0]);
        }

        // Initializing virtual showroom set up
        SelectContent();
        SelectModel();

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

        m_Resizer = new Resizer();
        m_LightController = GetComponent<LightController>();
        m_AudioSource = GetComponent<AudioSource>();
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
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", m_ContentList[m_ContentIdx]);
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
        void ProcessChatResponse(ChatResponse resp, string voice)
        {
            Debug.Log(resp.answer);

            // Text data
            int period = resp.answer.Length / TIME_TO_WORDS;
            StartCoroutine(Speak(period, voice, resp.answer));

            m_Text.text = m_Text.text + $"A: {resp.answer}";
        }

        void ProcessMoodResponse(ChatResponse resp)
        {
            m_LightController.SetMood(resp.answer);
        }

        m_Text.text = m_Text.text + $"\n\nQ: {text}\n";
        m_InputField.text = "";

        // Choose voice of the model
        string voice = null;
        if (m_TTSEnabled)
        {
            if (m_Models[m_ModelIdx].name.StartsWith("Lady"))
            {
                voice = Voices.nova.ToString();
            }
            else if (m_Models[m_ModelIdx].name.StartsWith("Gentleman"))
            {
                voice = Voices.alloy.ToString();
            }
        }

        if (m_RagMode == RagMode.TEXT_ONLY)
        {
            // Get the image ID
            Texture2D texture2d = m_ContentList[m_ContentIdx];
            string[] name_split = texture2d.name.Split('_');
            ArraySegment<string> name_array = new ArraySegment<string>(name_split, 0, 2);
            string imageId = String.Join('_', name_array);

            Debug.Log(imageId);

            var metadata = Metadata.METADATA[imageId];
            m_Api.VirtualShowroomChatTextOnly(text, metadata[0], metadata[1], (err, resp) =>
            {
                if (err)
                {
                    Debug.Log("Chat failed");
                }
                else
                {
                    ProcessChatResponse(resp, voice);
                }
            });
        }
        else if (m_RagMode == RagMode.TEXT_AND_IMAGE || m_RagMode == RagMode.TEXT_AND_IMAGE_AND_MOOD)
        {
            // Resize the current texture
            Texture2D texture2d = m_ContentList[m_ContentIdx];
            int newHeight = m_ResizedImageHeight;
            int newWidth = texture2d.width * newHeight / texture2d.height;
            Texture2D resizedTexture = m_Resizer.Resize(texture2d, newWidth, newHeight);

            // Convert it into JPEG, then into base64 string
            byte[] bytes = ImageConversion.EncodeToJPG(resizedTexture);
            string b64image = Convert.ToBase64String(bytes);

            //Debug.Log(b64image);

            string[] name_split = texture2d.name.Split('_');
            ArraySegment<string> name_array = new ArraySegment<string>(name_split, 0, 2);
            string imageId = String.Join('_', name_array);

            Debug.Log(imageId);

            var metadata = Metadata.METADATA[imageId];
            m_Api.VirtualShowroomChatTextAndImage(text, metadata[0], metadata[1], b64image, (err, resp) =>
            {
                if (err)
                {
                    Debug.Log("Chat failed");
                }
                else
                {
                    ProcessChatResponse(resp, voice);
                }
            });

            if (m_RagMode == RagMode.TEXT_AND_IMAGE_AND_MOOD)
            {
                m_Api.MoodJudgement(b64image, (err, resp) =>
                {
                    if (err)
                    {
                        Debug.Log("Mood failed");
                    }
                    else
                    {
                        ProcessMoodResponse(resp);
                    }
                });
            }
        }
    }

    //*** Animations ***
    Animator Animator()
    {
        return m_Models[m_ModelIdx].GetComponent<Animator>();
    }

    IEnumerator Speak(int period, string voice, string text)
    {
        if (voice != null && text != null)
        {
            StartCoroutine(m_Api.TextToSpeech(m_AudioSource, voice, text));
        }

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

