from flask import Blueprint, request, jsonify, make_response
from . import chat, tts
from typing_extensions import deprecated

main = Blueprint("main", __name__)

# OpenAI's Text-to-Speech
tts_alloy = tts.TTS(voice="alloy", format="mp3")
tts_nova = tts.TTS(voice="nova", format="mp3")
voices = {"alloy": tts_alloy, "nova": tts_nova}


@main.route("/")
def hello_world():
    return "Hello!"


@main.route("/mood_judgement", methods=["PUT"])
def mood_judgement():
    data = request.json
    b64image = data["b64image"]
    #with open("./tmp/b64image.txt", "w") as f:
    #    f.write(b64image)

    resp = chat.mood_judgement(b64image)
    return jsonify(resp)


@main.route("/tts", methods=["GET"])
def text_to_speech():
    voice = request.args.get("voice", default="alloy", type=str)
    text = request.args.get("text", default="hello", type=str)

    speech = voices[voice].speak(text)
    #with open("tmp/tts.mp3", "wb") as f:
    #    f.write(speech)

    response = make_response()
    response.data = speech

    response.headers["Content-Disposition"] = "attachment; filename=tts.mp3"
    response.mimetype = "audio/mpeg"

    return response


### Virtual Showroom ###

@main.route("/virtual_showroom/chat_with_image", methods=["GET", "PUT"])
def virtual_showroom_chat_with_image():
    query = request.args.get("query", default=None, type=str)
    image_id = request.args.get("image_id", default=None, type=str)

    if request.method == "PUT":
        data = request.json
        b64image = data["b64image"]
        #with open("./tmp/b64image.txt", "w") as f:
        #    f.write(b64image)
    else:
        b64image = None

    # Use LangChain with OpenAI's APIs for chat completions
    resp = chat.chat_for_virtual_showroom(query, b64image, image_id)

    return jsonify(resp)


### Object Detection ###

@main.route("/object_detection/chat_with_image", methods=["GET", "PUT"])
def object_detection_chat_with_image():
    query = request.args.get("query", default=None, type=str)

    if request.method == "PUT":
        data = request.json
        b64image = data["b64image"]
        with open("./tmp/b64image.txt", "w") as f:
            f.write(b64image)
    else:
        b64image = None

    # Use LangChain with OpenAI's APIs for chat completions
    resp = chat.chat_for_object_detection(query, b64image)

    return jsonify(resp)

