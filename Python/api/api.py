from flask import Blueprint, request, jsonify
from . import chat
from typing_extensions import deprecated

main = Blueprint("main", __name__)


@main.route("/")
def hello_world():
    return "Hello, Virtual Showroom!"


@main.route("/chat_with_image", methods=["GET", "PUT"])
def chat_with_image():
    query = request.args.get("query", default=None, type=str)
    image_id = request.args.get("image_id", default=None, type=str)
    if request.method == "PUT":
        data = request.json
        b64image = data["b64image"]
        with open("./tmp/b64image.txt", "w") as f:
            f.write(b64image)
    else:
        b64image = None

    resp = chat.invoke(query, b64image, image_id)
    return jsonify(resp)


@main.route("/mood_judgement", methods=["PUT"])
def mood_judgement():
    data = request.json
    b64image = data["b64image"]
    with open("./tmp/b64image.txt", "w") as f:
        f.write(b64image)

    resp = chat.mood_judgement(b64image)
    return jsonify(resp)