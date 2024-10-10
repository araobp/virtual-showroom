using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using static System.Web.HttpUtility;

public class ChatAPI : RestClient
{
    [SerializeField] string m_BaseUrl;
    EndPoint m_EndPoint;

    // Callbacks
    public delegate void HelloCallback(bool err, string text);
    public delegate void ChatCallback(bool err, ChatResponse resp);

    public void OnEnable()
    {
        // REST API client init
        m_EndPoint = new EndPoint();
        m_EndPoint.baseUrl = m_BaseUrl;
    }

    // API server
    public void Hello(HelloCallback callback)
    {
        Get(m_EndPoint, "/hello", (err, text) =>
        {
            callback(err, text);
        });
    }

    // OpenAI's TTS Service
    public IEnumerator TextToSpeech(AudioSource audioSource, string voice, string text)
    {
        string urlParam = $"voice={voice}&text={UrlEncode(text)}";
        string url = $"{m_EndPoint.baseUrl}{"/tts"}?{urlParam}";
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    //*** Virtual Showroom ***
    const string VIRTUAL_SHOWROOM_SYSTEM_MESSAGE = "You are a tour guide. You are also good at analyzing images.";

    public void VirtualShowroomChatTextOnly(string query, string context, ChatCallback callback)
    {
        Get(m_EndPoint, $"/chat?system_message={VIRTUAL_SHOWROOM_SYSTEM_MESSAGE}&user_message={query}&context={context}", (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            callback(err, resp);
        });
    }

    public void VirtualShowroomChatTextAndImage(string query, string context, string b64image, ChatCallback callback)
    {
        ChatWithImage chatImage = new ChatWithImage();
        chatImage.b64image = b64image;
        string jsonBody = JsonUtility.ToJson(chatImage);
        Put(m_EndPoint, $"/chat?system_message={VIRTUAL_SHOWROOM_SYSTEM_MESSAGE}&user_message={query}&context={context}", jsonBody, (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            callback(err, resp);
        });
    }

    const string USER_MESSAGE_MOOD = "Please choose one word from the following options that best describes the mood of this image. If you are unsure, please respond with 'Unsure.\n\nOptions: Serene, Bustling, Nostalgic, Lonely, Picturesque, Chaotic, Gloomy, Vibrant.";
    
    readonly List<string> MOODS = new List<string>{"serene", "bustling", "nostalgic", "lonely", "picturesque", "chaotic", "gloomy", "vibrant", "unsure"};

    public void MoodJudgement(string b64image, ChatCallback callback)
    {
        ChatWithImage chatImage = new ChatWithImage();
        chatImage.b64image = b64image;
        string jsonBody = JsonUtility.ToJson(chatImage);
        Put(m_EndPoint, $"/chat?system_message={VIRTUAL_SHOWROOM_SYSTEM_MESSAGE}&user_message={USER_MESSAGE_MOOD}", jsonBody, (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            if (!err) {
                string answer = resp.answer;
                resp.answer = "unsure";
                MOODS.ForEach(m => {
                    if (answer.ToLower().Contains(m)) {
                        resp.answer = m;
                    }
                });
            }
            callback(err, resp);
        });
    }

    //*** Object Detection ***
    public void ObjectDetectionChatTextAndImage(string query, string b64image, ChatCallback callback)
    {
        ChatWithImage chatImage = new ChatWithImage();
        chatImage.b64image = b64image;
        string jsonBody = JsonUtility.ToJson(chatImage);
        Put(m_EndPoint, $"/chat?user_message={query}", jsonBody, (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            callback(err, resp);
        });
    }


}
