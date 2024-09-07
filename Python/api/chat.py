from langchain_openai import ChatOpenAI
from langchain_core.messages import AIMessage, HumanMessage, SystemMessage
from langchain_core.prompts import PromptTemplate
from langchain_openai import OpenAIEmbeddings
from langchain_chroma import Chroma
import os
import sqlite3
from typing_extensions import deprecated

EMBEDDINGS_MODEL = "text-embedding-ada-002"
LLM_MODEL = "gpt-4o-mini"

# ChromaDB as vector store
embeddings = OpenAIEmbeddings(model=EMBEDDINGS_MODEL)
CHROMA_DB_PATH = os.path.join(os.path.dirname(__file__), "../embeddings/db")
vector_store = Chroma(embedding_function=embeddings, persist_directory=CHROMA_DB_PATH)

SQLITE_DB_PATH = os.path.join(os.path.dirname(__file__), "../scenarios/db/scenarios.db")

# Chat model
llm = ChatOpenAI(model=LLM_MODEL)

# Query template for RAG
TEMPLATE = """Please answer the questions based on the following text. If you don't know, please answer that you don't know.

Text:
{document}

Query: {query}
"""

TEMPLATE_FOR_IMAGE = """Please answer the questions based on the following texts and the image. If you don't know, please answer that you don't know.

Text:
{document}

Query: {query}
"""

prompt = PromptTemplate(
    template=TEMPLATE,
    input_variables=["document", "query"],
)

prompt_for_image = PromptTemplate(
    template=TEMPLATE_FOR_IMAGE,
    input_variables=["document", "query"],
)

# Query with RAG
def query_with_rag(query):

    # Similarity search
    documents = vector_store.similarity_search(query)

    doc_string = ""

    # Reference documents for RAG
    for doc in documents:
        doc_string += f"\n---------------------\n{doc.page_content}"

    # Invoke query
    content = prompt.format(document=doc_string, query=query)
    # print(content)
    result = llm.invoke(
        [
            SystemMessage(
                content="You are a showroom guide. Please respond to questions from showroom visitors using honorific language."
            ),
            HumanMessage(content=content),
        ]
    )

    resp = {"query": query, "answer": result.content}

    return resp


@deprecated('To be removed')
def query_with_image(query, b64image):
    result = llm.invoke(
        [
            AIMessage(content="You are a bot that is good at anlyzing images."),
            SystemMessage(
                content="You are a tourist guide in Yokohama, Japan. Please respond to questions from showroom visitors using honorific language in English, referring to the attached image."
            ),
            HumanMessage(
                content=[
                    {"type": "text", "text": query},
                    {
                        "type": "image_url",
                        "image_url": {"url": f"data:image/jpeg;base64,{b64image}"},
                    },
                ]
            ),
        ]
    )

    resp = {"query": query, "answer": result.content}

    return resp


def query_with_image2(query, b64image, image_id = None):

    if image_id is not None:
        with sqlite3.connect(SQLITE_DB_PATH) as conn:
            cur = conn.cursor()
            scenario = cur.execute(f"SELECT scenario FROM scenarios WHERE image LIKE '{image_id}%'").fetchone()
    else:
        scenario = ''

    ai_message = "You are a bot that is good at anlyzing texts and images."
    # TODO: Protect from prompt injection attacks
    system_message = f"You are a tour guide in Yokohama and Tokyo, Japan. Please respond to questions from the visitors. {scenario}"

    # Similarity search
    documents = vector_store.similarity_search(query)

    doc_string = ""

    # Reference documents for RAG
    for doc in documents:
        doc_string += f"\n---------------------\n{doc.page_content}"

    print(prompt_for_image.format(document=doc_string, query=query))

    result = llm.invoke(
        [
            AIMessage(content=ai_message),
            SystemMessage(content=system_message),
            HumanMessage(
                content=[
                    {"type": "text", "text": prompt_for_image.format(document=doc_string, query=query)},
                    {
                        "type": "image_url",
                        "image_url": {"url": f"data:image/jpeg;base64,{b64image}"},
                    },
                ]
            ),
        ]
    )

    resp = {"query": query, "answer": result.content}

    return resp


if __name__ == "__main__":
    print(query_with_rag("What makes Yokohama attractive?"))
