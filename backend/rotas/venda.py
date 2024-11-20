# arquivo: rotas/venda.py

from flask import Blueprint, jsonify, request
from datetime import datetime
from servicos.venda import VendasDatabase

vendas_blueprint = Blueprint("venda", __name__)

#Rota para listagem de vendas
@vendas_blueprint.route("/vendas", methods=["GET"])
def get_vendas():
    try:
        start_date_str = request.args.get("start_date")
        end_date_str = request.args.get("end_date")
        
        """
        if not start_date_str or not end_date_str:
            return jsonify({"error": "Parâmetros 'start_date' e 'end_date' são obrigatórios"}), 400
        """

        start_date = datetime.strptime(start_date_str, "%Y-%m-%d")
        end_date = datetime.strptime(end_date_str, "%Y-%m-%d")

        vendas_db = VendasDatabase()
        vendas_info = vendas_db.get_vendas(start_date, end_date)

        return jsonify(vendas_info), 200

    except ValueError:
        return jsonify({"error": "Formato de data inválido. Use 'YYYY-MM-DD'."}), 400
    except Exception as e:
        return jsonify({"error": str(e)}), 500
    
#Rota para os detalhes de uma venda    
@vendas_blueprint.route("/vendas/<int:id_venda>", methods=["GET"])
def get_detalhes_venda(id_venda):
    try:
        vendas_db = VendasDatabase()
        venda_detalhes = vendas_db.get_detalhes_venda(id_venda)

        if not venda_detalhes:
            return jsonify({"error": "Venda não encontrada"}), 404

        return jsonify(venda_detalhes), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500