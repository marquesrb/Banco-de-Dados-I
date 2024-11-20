"Camada que gerencia o Database"

from flask import Flask, jsonify
from flask_cors import CORS
from rotas.venda import vendas_blueprint
from rotas.produto import produtos_blueprint
from rotas.cliente import clientes_blueprint
from rotas.fornecedor import fornecedores_blueprint
from rotas.agendamento import agendamentos_blueprint

app = Flask(__name__)

CORS(app, origins="*")


@app.route("/", methods=["GET"])
def get_autor():
    return jsonify("It's alive"), 200

app.register_blueprint(vendas_blueprint)
app.register_blueprint(produtos_blueprint)
app.register_blueprint(clientes_blueprint)
app.register_blueprint(fornecedores_blueprint)
app.register_blueprint(agendamentos_blueprint)

app.run("0.0.0.0", port=8000, debug=False)