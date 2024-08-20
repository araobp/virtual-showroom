# Virtual Showroom

(Work in Progress)

## Goal

The goal is to realize a virtual showroom with Digital Human as promotional models. This project uses human models made with Blender and MPFB2 to realize Digital Human. The final output will be both a console app and an AR app on iOS.

## Virtual Showroom Concept

Virtual Showroom is a very cost-effective. It only requires a 240-degree panorama screen for VR experiences with naked-eyes. I want to realize a **"real"** virtual showroom someday, but this project develops a **"virtual"** virtual showroom running on Unity.

<img src="doc/smaple_scene.jpeg" width=600>

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

The Flask-based API server will run on PC or Mac. I will also test if it can also run on Raspberry Pi.

## Models

=> [MODELS.md](./MODELS.md)

## Virtual Showroom

=> [SHOWROOM.md](./SHOWROOM.md)


