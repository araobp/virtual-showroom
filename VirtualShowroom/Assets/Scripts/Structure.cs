using System;
using Microsoft.Unity.VisualStudio.Editor;

[Serializable]
public class ChatResponse
{
    public string query;
    public string answer;
}

public class ChatImage{
    public string b64image;
}