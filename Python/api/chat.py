from langchain_openai import ChatOpenAI
from langchain_core.prompts import PromptTemplate
from langchain_core.messages.ai import AIMessage
from langchain_core.messages.system import SystemMessage
from langchain_core.messages.human import HumanMessage
from langchain_openai import OpenAIEmbeddings
from langchain_chroma import Chroma
import os
import sqlite3
import re

EMBEDDINGS_MODEL = "text-embedding-ada-002"
LLM_MODEL = "gpt-4o-mini"

last_b64image = None

# Chat model
llm = ChatOpenAI(model=LLM_MODEL)

# ChromaDB as vector store
embeddings = OpenAIEmbeddings(model=EMBEDDINGS_MODEL)
CHROMA_DB_PATH = os.path.join(os.path.dirname(__file__), "../embeddings/db")
vector_store = Chroma(embedding_function=embeddings, persist_directory=CHROMA_DB_PATH)

# SQLite3 as scenario store
SQLITE_DB_PATH = os.path.join(os.path.dirname(__file__), "../scenarios/db/scenarios.db")

# Templates
TEMPLATE_AI = "You are a bot that is good at anlyzing texts and images."

TEMPLATE_SYSTEM = """You are a tour guide in Japan.

Now you are in the area as described below:
{scenario}
"""

TEMPLATE_USER = """Please answer the questions based on the following texts and the following image if attached. If you don't know, please answer that you don't know.

Text:
{document}

Query: {query}
"""

def invoke(query:str, b64image:str=None, image_id:str=None):
    global last_b64image

    if image_id is not None:
        with sqlite3.connect(SQLITE_DB_PATH) as conn:
            cur = conn.cursor()
            scene_id, scenario = cur.execute(
                f"SELECT scene_id, scenario FROM scenarios WHERE image LIKE '{image_id}%'"
            ).fetchone()
    else:
        scene_id = None
        scenario = ""

    prompt_ai = PromptTemplate(template=TEMPLATE_AI, input_variables=[])
    prompt_system = PromptTemplate(
        template=TEMPLATE_SYSTEM, input_variables=["scenario"]
    )
    prompt_user = PromptTemplate(
        template=TEMPLATE_USER,
        input_variables=["document", "query"],
    )

    ai_message = prompt_ai.format()
    system_message = prompt_system.format(scenario=scenario)

    # Similarity search
    if scene_id is None:
        documents = vector_store.similarity_search(query)
    else:  # Query with filter (metadata)
        documents = vector_store.similarity_search(query, filter={'scene_id': scene_id})

    doc_string = ""

    # Reference documents for RAG
    for doc in documents:
        doc_string += f"\n---------------------\n{doc.page_content}"

    user_message_ = prompt_user.format(document=doc_string, query=query)

    if b64image is None:
        user_message = [
            {
                "type": "text",
                "text": user_message_,
            },
        ]
        last_b64image = None
    else:
        user_message = [
            {
                "type": "text",
                "text": user_message_,
            },
            {
                "type": "image_url",
                "image_url": {"url": f"data:image/jpeg;base64,{b64image}"},
            },
        ]
        last_64image = b64image

    print(">>>>>>>>>>>>>>>>>>>")
    print(ai_message)
    print(system_message)
    print(user_message)

    result = llm.invoke(
        [
            AIMessage(content=ai_message),
            SystemMessage(content=system_message),
            HumanMessage(content=user_message),
        ]
    )

    resp = {"query": query, "answer": result.content}
    print(resp)

    return resp

PROMPT_MOOD_AI = "You are a bot that is good at anlyzing images."
PROMPT_MOOD_SYSTEM = "You are a tour guide in Japan."

PROMPT_MOOD_HUMAN = """Please choose one word from the following options that best describes the mood of this photo.
If you are unsure, please respond with 'Unsure."

Options: Serene, Bustling, Nostalgic, Lonely, Picturesque, Chaotic, Gloomy, Vibrant.
"""

PROMPT_MOOD_HUMAN_JUST_AFTER_INVOKE = """Please choose one word from the following options that best describes the mood of the photo a moment ago.
If you are unsure, please respond with 'Unsure."

Options: Serene, Bustling, Nostalgic, Lonely, Picturesque, Chaotic, Gloomy, Vibrant.
"""


def mood_judgement(b64image: str):
    
    if last_b64image is not None and last_b64image == b64image:
        user_message = [
            {
                    "type": "text",
                    "text": PROMPT_MOOD_HUMAN_JUST_AFTER_INVOKE
            }
        ]
    else:
        user_message = [
            {
                    "type": "text",
                    "text": PROMPT_MOOD_HUMAN,
            },
            {
                    "type": "image_url",
                    "image_url": {"url": f"data:image/jpeg;base64,{b64image}"},
            },
        ]

    ai_message = PROMPT_MOOD_AI
    system_message = PROMPT_MOOD_SYSTEM

    print(">>>>>>>>>>>>>>>>>>>")
    print(ai_message)
    print(system_message)
    print(user_message)

    result = llm.invoke(
        [
            AIMessage(content=ai_message),
            SystemMessage(content=system_message),
            HumanMessage(content=user_message),
        ]
    )

    print(result.content)

    # Search a mood word in the result
    match = re.search('(Serene|Bustling|Nostalgic|Lonely|Picturesque|Chaotic|Gloomy|Vibrant|Unsure)', result.content)
    mood = match.group()
    
    resp = {'mood': mood}
    return resp



if __name__ == "__main__":
    print(invoke("What makes Yokohama attractive?"))
    with open('../samples/b64image.txt') as f:
        b64image = f.read()
    print(mood_judgement(b64image))
    