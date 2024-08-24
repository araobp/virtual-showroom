from flask import Blueprint, request, jsonify
from . import chat

main = Blueprint("main", __name__)

@main.route("/")
def hello_world():
  return "Hello, Virtual Showroom!"

@main.route("/chat")
def chat_query():
  query = request.args.get("query", default=None, type=str)
  resp = chat.query(query)
  return jsonify(resp)