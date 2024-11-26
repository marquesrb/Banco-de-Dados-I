# arquivo: rotas/fornecedor.py

from flask import Blueprint, jsonify, request
from servicos.fornecedor import FornecedorDatabase

fornecedores_blueprint = Blueprint("fornecedor", __name__)

fornecedor_db = FornecedorDatabase()

# Rota para obter o histórico de produtos fornecidos por um fornecedor
@fornecedores_blueprint.route("/fornecedores", methods=["GET"])
def get_historico_produtos_fornecedor():
    try:
        fornecedor_id = request.args.get('fornecedor_id')
        
        if not fornecedor_id:
            return jsonify({"erro": "fornecedor_id é obrigatório"}), 400
        
        historico = fornecedor_db.get_historico_produtos_fornecedor(fornecedor_id)
        
        if not historico:
            return jsonify({"mensagem": "Nenhum produto fornecido encontrado para este fornecedor."}), 404
        
        return jsonify({"historico": historico}), 200
    except Exception as e:
        return jsonify({"erro": "Erro ao processar a solicitação", "detalhes": str(e)}), 500

@fornecedores_blueprint.route("/fornecedores/lista", methods=["GET"])
def listar_fornecedores():
    try:
        fornecedores = fornecedor_db.listar_todos_fornecedores()
        
        if not fornecedores:
            return jsonify({"mensagem": "Nenhum fornecedor encontrado."}), 404
        
        return jsonify({"fornecedores": fornecedores}), 200
    except Exception as e:
        return jsonify({"erro": "Erro ao processar a solicitação", "detalhes": str(e)}), 500