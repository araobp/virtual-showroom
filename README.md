# Digital Human with LangChain

(Work in Progress)

## Goal

It is a dream from 25 years ago to realize Chatbot with humanity for me.

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

## Architecture

```
[DigitalHuman/Unity]--- REST API ---[ChatApp/LangChain/Flask]--- REST API ---[OpenAI API Services]
```

## Human model generated with Blender and MPFB2 (EEVEE NEXT with Ray Tracing enabled)

<img src="doc/Lady_blender.png" width=800>

## Human model imported into Unity (URP)

https://github.com/user-attachments/assets/8bce04d2-df38-4438-85d2-9779e745b87d

## Blender MPFB => Unity Import Tips

...

