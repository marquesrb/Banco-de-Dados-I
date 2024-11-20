# arquivo: rotas/produto.py

from flask import Blueprint, jsonify, request
from datetime import datetime
from servicos.produto import ProdutosDatabase

produtos_blueprint = Blueprint("produto", __name__)

@produtos_blueprint.route("/produtos", methods=["GET"])
def get_produtos():
    """
    Rota para listar todos os produtos com suas informações e quantidades disponíveis.
    :return: JSON com a lista de produtos.
    """
    try:
        produtos_db = ProdutosDatabase()
        produtos = produtos_db.get_produtos()
        return jsonify(produtos), 200  
    except Exception as e:
        return jsonify({'error': f'Erro ao listar produtos: {str(e)}'}), 500  
    
@produtos_blueprint.route("/produtos/<int:produto_id>", methods=["GET"])
def get_detalhes_produto(produto_id):
    """
    Rota para buscar os detalhes de um produto específico pelo ID, incluindo informações do fornecedor.
    :param produto_id: ID do produto.
    :return: JSON com os detalhes do produto e do fornecedor, ou erro 404 se não encontrado.
    """
    try:
        produtos_db = ProdutosDatabase()
        produto = produtos_db.get_detalhes_produto(produto_id)
        
        if produto is None:
            return jsonify({"error": "Produto não encontrado"}), 404
        
        return jsonify(produto), 200 
    except Exception as e:
        return jsonify({'error': f'Erro ao buscar detalhes do produto: {str(e)}'}), 500

@produtos_blueprint.route("/produtos/fornecedores/<int:produto_id>", methods=["GET"])
def get_fornecedores_produto_route(produto_id):
    """
    Rota para buscar o histórico de fornecedores para um produto específico.
    :param produto_id: ID do produto.
    :return: JSON com o histórico de fornecedores para o produto ou erro 404 se não encontrado.
    """
    try:
        produtos_db = ProdutosDatabase()
        historico = produtos_db.get_fornecedores_produto(produto_id)
        
        if not historico:
            return jsonify({"mensagem": "Nenhum fornecedor encontrado para este produto."}), 404
        
        return jsonify({"historico": historico}), 200
    except Exception as e:
        return jsonify({"erro": "Erro ao processar a solicitação", "detalhes": str(e)}), 500
