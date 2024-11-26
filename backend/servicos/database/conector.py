from typing import Any
import psycopg2
from psycopg2.extras import DictCursor


class DatabaseManager:
    "Classe de Gerenciamento do database"

    def __init__(self) -> None:
        self.conn = psycopg2.connect(
            dbname="postgres",
            user="volpe",
            password="123",
            host="localhost",
            port=5432,
        )
        self.cursor = self.conn.cursor(cursor_factory=DictCursor)

    def execute_statement(self, statement: str, params: tuple = None) -> None:
        """
        Usado para Inserções, Deleções, Alter Tables, e outras consultas que modifiquem dados.
        
        :param statement: A instrução SQL a ser executada.
        :param params: Tupla de parâmetros para a instrução SQL.
        """
        try:
            if params:
                self.cursor.execute(statement, params)
            else:
                self.cursor.execute(statement)
            
            self.conn.commit()
        except Exception as e:
            self.conn.rollback()
            print(f"Erro ao executar a consulta: {e}")
            raise 
        
    def execute_select_all(self, query: str, params: tuple = ()) -> list[dict[str, Any]]:
        "Usado para SELECTS no geral"
        self.cursor.execute(query, params)
        return [dict(item) for item in self.cursor.fetchall()]

    def execute_select_special(self, query: str, params: tuple) -> list[dict[str, Any]]:
        self.cursor.execute(query, params) 
        return [dict(item) for item in self.cursor.fetchall()]

    def execute_select_one(self, query: str, params: tuple = ()) -> dict | None:
        """
        Executa uma consulta SELECT que retorna apenas uma linha.
        :param query: A consulta SQL a ser executada.
        :param params: Parâmetros para a consulta.
        :return: Um dicionário com os resultados ou None se não houver resultados.
        """
        self.cursor.execute(query, params) 
        query_result = self.cursor.fetchone()

        if not query_result:
            return None

        if isinstance(query_result, dict):
            return query_result

        col_names = [desc[0] for desc in self.cursor.description]
        return dict(zip(col_names, query_result))

#Agendamento

    def get_next_servico_id(self):
            """
            Retorna o próximo ID disponível para 'idservico', baseado no último ID registrado no banco de dados.
            Se não houver nenhum registro, retorna 1.
            """
            query = "SELECT MAX(idservico) FROM servico"
            self.cursor.execute(query)
            last_id_servico = self.cursor.fetchone()[0]
            
            return last_id_servico + 1 if last_id_servico is not None else 1

    def get_next_venda_id(self):
        """
        Retorna o próximo ID disponível para 'idvenda', baseado no último ID registrado no banco de dados.
        Se não houver nenhum registro, retorna 1.
        """
        query = "SELECT MAX(id) FROM venda"
        self.cursor.execute(query)
        last_id_venda = self.cursor.fetchone()[0]
        
        return last_id_venda + 1 if last_id_venda is not None else 1