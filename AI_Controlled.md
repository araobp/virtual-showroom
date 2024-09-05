# AI-Controlled Virtual Showroom

## Experiment: ChatGPT can be a tour guide on its own without text-only RAG

<table>
  <tr>
    <td>
      <img src="doc/ImageToTextSample1.png" width=500>      
    </td>
    <td>
      <img src="doc/ImageToTextSample2.png" width=500>
  </tr>
</table>


## Image-to-Text implementation

Chatting with images.

```
[Unity app] --- PUT request with query and base64-encoded image ---> [API Server] <--> [OpenAI API Service]
                 (Resized Texture2D data encoded into Base64)                           gpt-4o-mini model
```

<img src="doc/image_to_text_test.png" width=700>

## Lighting in the showroom controlled by commands from the OpenAI's LLM (gpt-4o-mini)

(Work in progress)

