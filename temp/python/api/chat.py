from langchain_openai import ChatOpenAI
from langchain_core.messages import HumanMessage, SystemMessage

chat_model = ChatOpenAI(model="gpt-3.5-turbo")

def query(query):
  result = chat_model.invoke([
    SystemMessage(content="You are a showroom guide. Please respond to questions from showroom visitors using honorific language."),
    HumanMessage(content=query)
  ])
  
  resp = {
    "query": query,
    "answer": result.content
  }

  return resp

if __name__ == '__main__':
  print(query('What is your name?'))
