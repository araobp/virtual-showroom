# Virtual Showroom

<img src="doc/sample_scene2.jpg" width=600>

## Goal

The goal is to realize a virtual showroom with Digital Human as promotional models.

## Virtual Showroom Concept

Virtual Showroom is a very cost-effective. It only requires a 240-degree panorama screen for VR experiences with naked-eyes. I want to realize a **"real"** virtual showroom someday at work, but this project develops a **"virtual"** virtual showroom running on Unity in the form of an AR app or a console app, as my hobby project.

https://github.com/user-attachments/assets/a0e8d2fd-37db-4ae1-b8d4-5c2a6fb2d8be

https://github.com/user-attachments/assets/91b72fe4-4f80-4af2-9d1a-4db6ef495eec

## Reference Showroom

I visited NISSAN GALLERY at [Nissan global headquarters](https://maps.app.goo.gl/Z5GTQqjRTFXAtd3D8) in Yokohama in May 2024. This was for a driving experience in harsh environments.

<table>
  <tr>
    <td>
      <img src="doc/nissan_showroom1.jpg" width=400>      
    </td>
    <td>
      <img src="doc/nissan_showroom2.jpg" width=400>
    </td>
  </tr>
</table>

## Schematic

My original design of such a virtual showroom.

<img src="doc/schematic.jpg" width=500>

## System Architecture

<img src="doc/architecture.jpg" width=600>

The Flask-based API server will run on PC or Mac. I will also test if it can also run on Raspberry Pi.

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

This project uses "gpt-4o-mini" from OpenAI with RAG.

## Development

### 1. Designing Models (Blender)

=> [MODELS.md](./MODELS.md)

### 2. Creating Virtual Showroom (Unity)

=> [SHOWROOM.md](./SHOWROOM.md)

### 3. Developing API server (LangChain/Flask/Python)

=> [API_SERVER.md](./API_SERVER.md)

### 4. AI-Controlled Virtual Showroom

=> [AI_Controlled.md](./AI_Controlled.md)
