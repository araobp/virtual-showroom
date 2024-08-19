using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatbotController : MonoBehaviour
{

    [SerializeField] Animator m_LadyBotAnimator;
    [SerializeField] Camera m_Camera1;
    [SerializeField] Camera m_Camera2;
    [SerializeField] Camera m_Camera3;
    [SerializeField] Camera m_Camera4;
    [SerializeField] Camera m_Camera5;

    List<Camera> m_Cameras = new List<Camera>();

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
        m_Cameras.Add(m_Camera1);
        m_Cameras.Add(m_Camera2);
        m_Cameras.Add(m_Camera3);
        m_Cameras.Add(m_Camera4);
        m_Cameras.Add(m_Camera5);
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

        }
    }
}

