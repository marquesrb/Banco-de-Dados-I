# arquivo: rotas/venda.py

from flask import Blueprint, jsonify, render_template, request
from datetime import datetime
from servicos.venda import VendasDatabase

vendas_blueprint = Blueprint("venda", __name__)

#Rota para listagem de vendas
@vendas_blueprint.route("/vendas", methods=["GET"])
def get_vendas():
    try:
        # Obtendo os parâmetros da query string
        start_date_str = request.args.get("start_date")
        end_date_str = request.args.get("end_date")
        produto = request.args.get("produto")  # Novo filtro opcional para o nome do produto

        # Validação dos parâmetros obrigatórios
        if not start_date_str or not end_date_str:
            return jsonify({"error": "Parâmetros 'start_date' e 'end_date' são obrigatórios"}), 400

        # Conversão das datas para o formato 'YYYY-MM-DD' usando o método strptime
        try:
            start_date = datetime.strptime(start_date_str, "%Y-%m-%d")
            end_date = datetime.strptime(end_date_str, "%Y-%m-%d")
        except ValueError:
            return jsonify({"error": "Formato de data inválido. Use 'YYYY-MM-DD'."}), 400

        # Conexão ao banco de dados e execução da consulta
        vendas_db = VendasDatabase()
        vendas_info = vendas_db.get_vendas(start_date, end_date, produto)

        # Formatar as datas de volta para o formato 'YYYY-MM-DD'
        for venda in vendas_info:
            venda['data_venda'] = venda['data_venda'].strftime("%Y-%m-%d")  # Convertendo de volta para string com formato correto

        # Retorno dos dados no formato JSON
        return jsonify({"vendas": vendas_info}), 200
        
    except Exception as e:
        # Tratamento de erros genéricos
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
        #return render_template('/venda/venda_detalhes.html', venda = venda_detalhes)
    
    except Exception as e:
        return jsonify({"error": str(e)}), 500

def criar_venda(self, valorTotal, data, cliente_id, produtos_comprados) -> int:
    """
    Cria uma venda no banco de dados.
    :param cliente_id: ID do cliente que realizou a compra.
    :param valorTotal: Valor total da venda.
    :param data: Data da venda (formato 'YYYY-MM-DD').
    :param produtos_comprados: Lista de produtos comprados (ex: [{'idProduto': 1, 'quantidade': 2, 'valorTotalProduto': 50.00}]).
    :return: ID da venda criada.
    """
    venda_id = self.db.get_next_venda_id()  # Obter o próximo ID disponível
    query = """
        INSERT INTO Venda (id, valorTotal, data, idCliente)
        VALUES (%s, %s, %s, %s)
    """
    try:
        # Inserir a venda
        self.db.execute_statement(query, (venda_id, valorTotal, data, cliente_id))

        # Criar registros para os produtos comprados (se houver)
        
       
        for produto in produtos_comprados:
            self.criar_item_venda(venda_id, produto['idProduto'], produto['quantidade'], produto['valorTotalProduto'])
    
        return venda_id
    except Exception as e:
        print(f"Erro ao criar venda: {e}")
        return None
