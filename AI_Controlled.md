# AI-Controlled Virtual Showroom

## Experiment: ChatGPT can be a tour guide on its own without RAG

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

```
[Unity app] --- PUT request with query and base64-encoded image ---> [API Server] <--> [OpenAI API Service]
                 (Resized Texture2D data encoded into Base64)
```

<img src="doc/image_to_text_test.png" width=700>
