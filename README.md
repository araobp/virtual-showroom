# Virtual Showroom

(Work in progress)

<img src="doc/sample_scene2.jpg" width=600>

## Background

Showrooms can be costly, but their ROI is unclear. Therefore, companies generally aim to achieve semi-automation using Multimodal AI.

## Virtual Showroom Concept

Virtual Showroom is a very cost-effective. It only requires a 240-degree panorama screen for VR experiences with naked-eyes. I want to realize a **"real"** virtual showroom someday at work, but this project develops a **"virtual"** virtual showroom running on Unity in the form of an AR app or a console app, as my hobby project.

https://github.com/user-attachments/assets/a55178b7-909f-4702-ad78-8f9438cf234d

Althugh the video shows Japanese chat, ChatGPT is multilingual.

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

The 240-degree panorama screen in this project also supports perspective drawing like a driving experience in NISSAN GARELLY above.

<img src="doc/PerspectiveView.jpg" width=700>

## System Architecture

<img src="doc/architecture.jpg" width=800>

The Flask-based API server will run on PC or Mac. It can run on a virtual machine in the cloud as well.

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

This project uses "gpt-4o-mini" from OpenAI with Multimodal RAG (text and image).

## Scenes

(Work in progress)

This project currently has two secenes:
- Virtual Showroom (Nearly completed)
- Objec Detection (Work in progress) 

I am going to add other scenes.

### Virtual Showroom scene

#### 1. Designing Models (Blender)

=> [MODELS.md](./MODELS.md)

#### 2. Creating Virtual Showroom (Unity)

=> [SHOWROOM.md](./SHOWROOM.md)

#### 3. Developing API server (LangChain/Flask/Python)

=> [API_SERVER.md](./API_SERVER.md)

#### 4. AI-Controlled Virtual Showroom

=> [AI_Controlled.md](./AI_Controlled.md)

### Object Detection scene

#### AI Robot

I was moved by [the demo video fo Figure 01](https://www.figure.ai/). If such a robot existed, human guides might no longer be needed in the showroom.

I modified the robot included in [the Unity's Starter Assets](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526?srsltid=AfmBOorNEgguMafoHDJOo979IJvgntvA-M-28pK6o9Jrkv7cI-6tt2P9), and use it to see what Generative AI can do.

<table>
  <tr>
    <td>
      <img src="doc/robot_3.jpg" width=300>      
    </td>
    <td>
      <img src="doc/robot_1.jpg" width=460>      
    </td>
    <td>
      <img src="doc/robot_2.jpg" width=460>
    </td>
  </tr>
</table>

#### Tests

A few years ago, I used to create many image recognition and object detection apps utilizing TensorFlow.

This time, I use OpenAI's multimodal AI for image recognition and object detection.

***Image Recognition test (gpt-4o-mini)***

After trying various experiments, I found that an image size of at least 512px x 512px seems to work best.

```
Received: {"answer":"I see an image of a cat grooming itself. The cat appears to be licking its paw while resting on a red surface.","query":"What can you see in this image?"}

```

***Object Recognition test (gpt-4o-mini)***

(Coming soon)
