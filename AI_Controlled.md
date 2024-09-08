# AI-Controlled Virtual Showroom (Multimodal LLM use case)

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

### Eight words describing mood

I asked ChatGPT:

```
Q: "Please provide eight words in English that describe the mood of photos of the city, port, or station,
including negative expressions."

A:
Serene (穏やか)
Bustling (賑やか)
Nostalgic (懐かしい)
Lonely (寂しい)
Picturesque (絵のように美しい)
Chaotic (混沌とした)
Gloomy (陰鬱な)
Vibrant (活気に満ちた)
```

So the prompt will be like
```
Please choose one word from the following options that best describes the mood of this photo. If you are unsure, please respond with 'Unknown."

Options: Serene, Bustling, Nostalgic, Lonely, Picturesque, Chaotic, Gloomy, Vibrant.
```

(Work in progress)

