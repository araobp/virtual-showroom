# Virtual Showroom

(Work in Progress)

## Goal

The goal is to realize a virtual showroom with Digital Human as promotional models.

This project uses human models made with Blender and MPFB2 to realize Digital Human.

The final output will be both a console app and an AR app on iOS.

The API server will run on PC or Mac. I will also test if it can also run on Raspberry Pi.

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
- https://github.com/araobp/learning-langchain

## Architecture

```
[DigitalHuman/Unity]--- REST API ---[ChatApp/LangChain/Flask]--- REST API ---[OpenAI API Services]
```

## Human model generated with Blender and MPFB2 (EEVEE NEXT with Ray Tracing enabled)

This model is going to be a promotional model or an exhibition guide at a virtual showroom or a virtual exhibition.

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

## Screen shader made with Unity's Shader Graph

I have made my original shader for swapping images on the screen.

<img src="doc/unity_settings1.jpg" width=600>

## Testing Chatbot animation on Unity 

I think 240-degree panorama screen is ideal for VR experiences with naked eyes at a showroom or an exhibition.

The background images are 240-degree panorama pictures taken with my iPhone SE.

https://github.com/user-attachments/assets/60e179d1-8ad3-498e-95d9-061d237fca20





