from flask import Blueprint, request, jsonify
from . import chat

main = Blueprint("main", __name__)

@main.route("/")
def hello_world():
  return "Hello, Virtual Showroom!"

@main.route("/chat_with_rag", methods=['GET'])
def chat_query_with_rag():
  query = request.args.get("query", default=None, type=str)
  resp = chat.query_with_rag(query)
  return jsonify(resp)

@main.route("/chat_with_image", methods=['PUT'])
def chat_query_with_image():
  query = request.args.get("query", default=None, type=str)
  data = request.json
  b64image = data["b64image"]
  with open('./tmp/b64image.txt', 'w') as f:
    f.write(b64image)
  
  resp = chat.query_with_image(query, b64image)
  return jsonify(resp)
