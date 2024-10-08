# Virtual Showroom

(Work in progress)

<img src="doc/sample_scene2.jpg" width=600>

Note: I am not very good at English. This README.md was proofread by ChatGPT.

## Background

Showrooms can be costly, but their ROI is unclear. Therefore, companies generally aim to achieve semi-automation using Multimodal AI.

## Virtual Showroom Concept

The Virtual Showroom is highly cost-effective. It only requires a 240-degree panoramic screen to provide VR experiences without the need for headsets. I aspire to create a "real" virtual showroom at work someday, but for now, this project is focused on developing a "virtual" virtual showroom using Unity, either as an AR app or a console app, as part of my hobby project.

https://github.com/user-attachments/assets/a55178b7-909f-4702-ad78-8f9438cf234d

Although the video showcases Japanese chat, ChatGPT is multilingual.

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

My original design for a virtual showroom.

<img src="doc/schematic.jpg" width=500>

The 240-degree panoramic screen in this project also supports perspective drawing, similar to the driving experience at the NISSAN GALLERY mentioned above.

<img src="doc/PerspectiveView.jpg" width=700>

## System Architecture

<img src="doc/architecture.jpg" width=800>

The Flask-based API server will run on a PC or Mac. It can also be deployed on a virtual machine in the cloud.

## Development Environment

### Tools

- Blender with [MPFB2](https://static.makehumancommunity.org/mpfb.html) (Blender)
- Unity
- VS Code

### LLM

This project utilizes OpenAI's "gpt-4o-mini" with Multimodal RAG (text and image).

## Scenes

(Work in progress)

This project currently has two secenes:
- Virtual Showroom (Nearly completed)
- Objec Detection (Work in progress) 

More scenes will be added in the future.

### Virtual Showroom scene

#### 1. Designing Models (Blender)

=> [MODELS.md](./MODELS.md)

#### 2. Creating the Virtual Showroom (Unity)

=> [SHOWROOM.md](./SHOWROOM.md)

#### 3. Developing the API server

The API server runs on Raspberry Pi.

Run the "app.py" that is in my another repo: [Compact RAG](https://github.com/araobp/compact-rag)

#### 4. AI-Controlled Virtual Showroom

=> [AI_Controlled.md](./AI_Controlled.md)

### Object Detection scene

#### AI Robot

I was impressed by [the demo video for Figure 01](https://www.figure.ai/). If such a robot were to exist, human guides in showrooms might no longer be necessary.

I modified the robot included in Unity's Starter Assets to experiment with what Generative AI can achieve.

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

A few years ago, I developed several image recognition and object detection apps using TensorFlow.

This time, I’m utilizing OpenAI's multimodal AI for image recognition and object detection.

***Image Recognition test (gpt-4o-mini)***

After trying various experiments, I found that an image size of at least 512px x 512px seems to work best.

```
Received: {"answer":"I see an image of a cat grooming itself.
The cat appears to be licking its paw while resting on a red surface.",
"query":"What can you see in this image?"}

```

***Object Recognition test (gpt-4o-mini)***

(Coming soon)
