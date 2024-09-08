from langchain_openai import ChatOpenAI
from langchain_core.prompts import PromptTemplate
from langchain_core.messages.ai import AIMessage
from langchain_core.messages.system import SystemMessage
from langchain_core.messages.human import HumanMessage
from langchain_openai import OpenAIEmbeddings
from langchain_chroma import Chroma
import os
import sqlite3

EMBEDDINGS_MODEL = "text-embedding-ada-002"
LLM_MODEL = "gpt-4o-mini"

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


def invoke(query, b64image=None, image_id=None):

    if image_id is not None:
        with sqlite3.connect(SQLITE_DB_PATH) as conn:
            cur = conn.cursor()
            scenario = cur.execute(
                f"SELECT scenario FROM scenarios WHERE image LIKE '{image_id}%'"
            ).fetchone()
    else:
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
    print(system_message)

    # Similarity search
    documents = vector_store.similarity_search(query)

    doc_string = ""

    # Reference documents for RAG
    for doc in documents:
        doc_string += f"\n---------------------\n{doc.page_content}"

    user_message_ = prompt_user.format(document=doc_string, query=query)
    print(user_message_)

    if b64image is None:
        user_message = [
            {
                "type": "text",
                "text": user_message_,
            },
        ]
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


if __name__ == "__main__":
    print(invoke("What makes Yokohama attractive?"))
