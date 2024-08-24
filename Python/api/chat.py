from langchain_openai import ChatOpenAI
from langchain_core.messages import HumanMessage, SystemMessage
from langchain_core.prompts import PromptTemplate
from langchain_openai import OpenAIEmbeddings
from langchain_chroma import Chroma
import os

# ChromaDB as vector store
embeddings = OpenAIEmbeddings(model="text-embedding-ada-002")
DB_PATH = os.path.join(os.path.dirname(__file__), "../embeddings/db")
vector_store = Chroma(embedding_function=embeddings, persist_directory=DB_PATH)

# Chat model
chat_model = ChatOpenAI(model="gpt-3.5-turbo")

# Query template for RAG
TEMPLATE = """Please answer the questions based on the following text. If you don't know, please answer that you don't know.

Text:
{document}

Query: {query}
"""

prompt = PromptTemplate(
    template=TEMPLATE,
    input_variables=["document", "query"],
)

# Query with RAG
def query(query):

    # Similarity search
    documents = vector_store.similarity_search(query)

    doc_string = ""

    # Reference documents for RAG
    for doc in documents:
        doc_string += f"\n---------------------\n{doc.page_content}"

    # Invoke query
    content = prompt.format(document=doc_string, query=query)
    #print(content)
    result = chat_model.invoke(
        [
            SystemMessage(
                content="You are a showroom guide. Please respond to questions from showroom visitors using honorific language."
            ),
            HumanMessage(content=content),
        ]
    )

    resp = {"query": query, "answer": result.content}

    return resp


if __name__ == "__main__":
    print(query("What makes Yokohama attractive?"))
    print(query("What makes Berlin attractive?"))
