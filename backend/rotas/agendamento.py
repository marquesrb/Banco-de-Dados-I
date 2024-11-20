from flask import Blueprint, jsonify, request
from datetime import datetime
from servicos.agendamento import AgendamentosDatabase
from servicos.funcionario import FuncionariosDatabase

agendamentos_blueprint = Blueprint("agendamento", __name__)

@agendamentos_blueprint.route('/agendamentos', methods=['GET'])
def get_agendamentos():
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