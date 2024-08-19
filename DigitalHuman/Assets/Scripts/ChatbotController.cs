using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatbotController : MonoBehaviour
{

    [SerializeField] Animator m_LadyBotAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
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
        switch (obj) {
            case '1':  // SitDown
                m_LadyBotAnimator.SetBool("sitDown", true);
                break;
        }
    }
}

