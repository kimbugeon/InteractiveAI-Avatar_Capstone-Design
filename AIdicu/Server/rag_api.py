# rag_api.py

from flask import Flask, request, jsonify
from langchain.vectorstores import Chroma
from langchain.embeddings import OpenAIEmbeddings
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.llms import OpenAI
from langchain.chains import RetrievalQA
from langchain.document_loaders import TextLoader
from langchain.document_loaders import DirectoryLoader

app = Flask(__name__)

persist_directory = 'db'

# Initialize Chroma vector store
embedding = OpenAIEmbeddings()
vectordb = Chroma(
    persist_directory=persist_directory,
    embedding_function=embedding
)

# Initialize retrieval QA chain
retriever = vectordb.as_retriever(search_kwargs={"k": 3})
qa_chain = RetrievalQA.from_chain_type(
    llm=OpenAI(),
    chain_type="stuff",
    retriever=retriever,
    return_source_documents=True
)

def process_llm_response(llm_response):
    result = llm_response['result']
    sources = [source.metadata['source'] for source in llm_response["source_documents"]]
    return result, sources

@app.route('/ask', methods=['POST'])
def ask_question():
    data = request.json
    question = data.get('question')
    
    llm_response = qa_chain(question)
    result, sources = process_llm_response(llm_response)
    
    return jsonify({'result': result, 'sources': sources})

if __name__ == '__main__':
    app.run(port=5000)
