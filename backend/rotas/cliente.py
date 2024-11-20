# arquivo: rotas/cliente.py

from flask import Blueprint, jsonify, request
from datetime import datetime
from servicos.cliente import ClientesDatabase


clientes_blueprint = Blueprint("cliente", __name__)

clientes_db = ClientesDatabase()

# Rota para obter clientes com tosa vencida
@clientes_blueprint.route("/clientes/vencidos", methods=["GET"])
def get_clientes_vencidos():
    try:
        clientes = clientes_db.get_cliente_vencido()
        
        if not clientes:
            return jsonify({"mensagem": "Nenhum cliente com tosa vencida encontrado."}), 404
    
        return jsonify({"clientes": clientes}), 200
    except Exception as e:
        return jsonify({"erro": "Erro ao processar a solicitação", "detalhes": str(e)}), 500
