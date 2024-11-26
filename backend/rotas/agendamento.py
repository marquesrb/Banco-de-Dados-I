from flask import Blueprint, jsonify, request
from datetime import datetime
from servicos.agendamento import AgendamentosDatabase
from servicos.funcionario import FuncionariosDatabase
from servicos.venda import VendasDatabase

agendamentos_blueprint = Blueprint("agendamento", __name__)

@agendamentos_blueprint.route('/agendamentos/funcionario', methods=['GET'])
def get_agendamentos_funcionario():
    try:
        funcionario_id = request.args.get('funcionario_id')
        start_date_str = request.args.get('start_date')
        end_date_str = request.args.get('end_date')

    
        start_date = datetime.strptime(start_date_str, "%Y-%m-%d")
        end_date = datetime.strptime(end_date_str, "%Y-%m-%d")

        agendamentos_db = AgendamentosDatabase()
        agendamentos = agendamentos_db.get_agendamentos_funcionario(funcionario_id, start_date, end_date)

        if not agendamentos:
            return jsonify({"message": "Nenhum agendamento encontrado para o funcionário no período."}), 404

        return jsonify(agendamentos)

    except ValueError:
        return jsonify({"error": "Formato de data inválido. Use 'YYYY-MM-DD'."}), 400
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@agendamentos_blueprint.route('/historico_cliente', methods=['GET'])
def get_historico_cliente():
    try:
   
        cliente_id = request.args.get('cliente_id')

        if not cliente_id:
            return jsonify({"error": "O parâmetro 'cliente_id' é obrigatório."}), 400

        agendamentos_db = AgendamentosDatabase()
        historico = agendamentos_db.get_agendamento_historico_cliente(cliente_id)

        if not historico:
            return jsonify({"message": "Nenhum agendamento encontrado para o cliente."}), 404

        return jsonify(historico)

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@agendamentos_blueprint.route('/agendamentos/ficha_horarios', methods=['GET'])
def get_ficha_horarios():
    try:
        funcionario_id = request.args.get('funcionario_id')

        if not funcionario_id:
            return jsonify({"error": "O parâmetro 'funcionario_id' é obrigatório."}), 400
        
        funcionarios_db = FuncionariosDatabase()
        ficha_horarios = funcionarios_db.get_funcionario_ficha_horario(funcionario_id)

        if not ficha_horarios:
            return jsonify({"message": "Nenhuma ficha de horários encontrada para o funcionário."}), 404

        return jsonify(ficha_horarios)

    except Exception as e:
        return jsonify({"error": str(e)}), 500
    
@agendamentos_blueprint.route('/agendamentos/criar', methods=['POST'])
def criar_agendamento():
    try:
        data = request.get_json()

     
        tipo = data.get('tipo')
        preco = data.get('preco')
        status = data.get('status')
        data_servico = data.get('data')  
        horario_servico = data.get('horario')
        idCaixa = data.get('idCaixa')
        idPet = data.get('idPet')
        idBanhista = data.get('idBanhista')
        idVenda = None

   
        if data.get('produtos_comprados'):
            venda_db = VendasDatabase()  
            idVenda = venda_db.criar_venda(data.get('cliente_id'), data.get('produtos_comprados'), preco)

        agendamento_db = AgendamentosDatabase()
        agendamento = agendamento_db.criar_agendamento(
            tipo, preco, status, data_servico, horario_servico, 
            idCaixa, idPet, idBanhista, idVenda
        )

        return jsonify(agendamento), 201

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@agendamentos_blueprint.route('/agendamentos', methods=['GET'])
def get_agendamentos():
    try:

        start_date_str = request.args.get('start_date')
        end_date_str = request.args.get('end_date')

     
        if start_date_str and end_date_str:
            start_date = datetime.strptime(start_date_str, "%Y-%m-%d")
            end_date = datetime.strptime(end_date_str, "%Y-%m-%d")
        else:
     
            start_date = datetime.now()
            end_date = start_date

        agendamentos_db = AgendamentosDatabase()

    
        agendamentos = agendamentos_db.get_agendamentos(start_date, end_date)

        if not agendamentos:
            return jsonify({"message": "Nenhum agendamento encontrado para o período."}), 404

        return jsonify(agendamentos)

    except ValueError:
        return jsonify({"error": "Formato de data inválido. Use 'YYYY-MM-DD'."}), 400
    except Exception as e:
        return jsonify({"error": str(e)}), 500