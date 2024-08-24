from langchain_openai import OpenAIEmbeddings
from langchain_chroma import Chroma
from langchain_text_splitters import SpacyTextSplitter
import glob

embeddings = OpenAIEmbeddings(model="text-embedding-ada-002")

DOCUMENTS = "../doc/*.txt"

# Note: the following code requires "en_core_web_lg".
# python3 -m spacy download en_core_web_lg
text_spliter = SpacyTextSplitter(
  chunk_size=300,
  pipeline="en_core_web_lg"
)

vector_store = Chroma(
    embedding_function=embeddings,
    persist_directory="./db"
)

# Clear all
ids = vector_store.get(include=[])['ids']
for id in ids:
  vector_store.delete(id)
print(vector_store.get())

# Document paths
file_paths = glob.glob(DOCUMENTS)

# Add embeddings
for path in file_paths:
  with open(path) as f:
    text = f.read()
    texts = text_spliter.split_text(text)
    vector_store.add_texts(texts)

print("db created")

