from flask import Blueprint

main = Blueprint("main", __name__)

@main.route("/")
def hello_world():
  return "<p>Hello, World!</p>"