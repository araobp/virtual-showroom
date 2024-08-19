# Digital Human with LangChain

(Work in Progress)

## Goal

The goal is to realize Chatbot with humanity. It is a dream from 25 years ago to realize Chatbot with humanity for me.

My another dream is to realize a (real) VR theater with 240-degree images or movies. Chatbots in this project work in a (virtual) VR theater.

This project uses human models made with Blender and MPFB2 to realize Digital Human.

## Development Environment

### Tools

- Blender
- Unity
- VS Code

### Libraries

- [MPFB2](https://static.makehumancommunity.org/mpfb.html) (Blender)
- [LangChain](https://python.langchain.com/v0.2/docs/introduction/) and [OpenAI API](https://openai.com/index/openai-api/) (Python)
- [Flask](https://flask.palletsprojects.com/en/3.0.x/) (Python)

### LLM

This project uses "gpt-3.5-turbo" from OpenAI with RAG.

### Relevant projects (my other projects on github.com)

- https://github.com/araobp/blender-3d/tree/main/scenes/BluesHarp
- https://github.com/araobp/unity-ar

## Architecture

```
[DigitalHuman/Unity]--- REST API ---[ChatApp/LangChain/Flask]--- REST API ---[OpenAI API Services]
```

## Human model generated with Blender and MPFB2 (EEVEE NEXT with Ray Tracing enabled)

<img src="doc/Lady_blender.png" width=800>

## Human model imported into Unity (URP)

Animations made on Blender works well on Unity.

<img src="doc/blender_graph_editor.jpg" width=800>

https://github.com/user-attachments/assets/8bce04d2-df38-4438-85d2-9779e745b87d

## Blender MPFB => Unity Import Tips

### Eyes

The eye shader of MPFB2 is not compatible with Unity. Apply the eyes texture from MakeHuman to the shader instead.

<img src="doc/blender_settings1.jpg" width=600>

<img src="doc/Eyes_diffuse.png" width=200>

### Body

The body generated with MPFB2 uses Blender's Active Modifer to remove a part of the body mesh under the cloth. On Unity, it causes a problem when the character wears a cloth with a diffuse texture having an Alpha channel. Just disable the modifer before exporting the model from Blender to Unity.

<img src="doc/blender_settings2.jpg" width=600>

### Exporting the model to Unity

Select the model to be exported

<img src="doc/blender_settings3.jpg" width=600>

Set the following options for exporting the model as FBX to Unity

<img src="doc/blender_settings4.jpg" width=600>

## Testing Chatbot animation on Unity 

The background photo image of Yokohama is a 240-degree panorama picture taken with my iPhone SE.

https://github.com/user-attachments/assets/1d187965-81df-4c1b-8d6f-106e5b35480a


