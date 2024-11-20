import datetime
from servicos.database.conector import DatabaseManager

class ProdutosDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider
    
    def get_produtos(self):
        """
        Lista todos os produtos, informando id, nome, preço de venda e a quantidade disponível.
        :return: Lista de dicionários contendo as informações dos produtos.
        """
        query = """
            SELECT 
                p.id, 
                p.nome, 
                p.preco_venda, 
                COALESCE(SUM(pf.quantidade), 0) - COALESCE(SUM(vp.quantidadeVendida), 0) AS quantidade_disponivel
            FROM 
                Produto p
            LEFT JOIN 
                ProdutosFornecidos pf ON p.id = pf.idProduto
            LEFT JOIN 
                VendaDeProduto vp ON p.id = vp.idProduto
            GROUP BY 
                p.id, p.nome, p.preco_venda
            ORDER BY 
                p.nome;
        """
        try:
            # Executa a consulta e obtém os resultados
            produtos = self.db.execute_select_all(query)
            return produtos
        except Exception as e:
            print(f"Erro ao listar produtos: {e}")
            return []
    
    def get_detalhes_produto(self, produto_id: int):
        """
        Retorna os detalhes de um produto específico pelo ID, incluindo informações sobre o fornecedor.
        :param produto_id: ID do produto.
        :return: Dicionário com os detalhes do produto ou None se não encontrado.
        """
        query = """
            SELECT 
                p.id, 
                p.nome,  
                pf.preco_custo, 
                p.preco_venda, 
                COALESCE(SUM(pf.quantidade), 0) - COALESCE(SUM(vp.quantidadeVendida), 0) AS quantidade_disponivel,
                COALESCE(SUM(vp.quantidadeVendida), 0) AS quantidade_vendida,
                f.razao_social AS fornecedor_nome,
                f.cnpj AS fornecedor_cnpj
            FROM 
                Produto p
            LEFT JOIN 
                ProdutosFornecidos pf ON p.id = pf.idProduto
            LEFT JOIN 
                VendaDeProduto vp ON p.id = vp.idProduto
            LEFT JOIN 
                Fornecedor f ON pf.idFornecedor = f.cnpj  -- Adicionando a tabela Fornecedor
            WHERE 
                p.id = %s
            GROUP BY 
                p.id, p.nome, pf.preco_custo, p.preco_venda, f.cnpj;
        """
        try:
            # Busca os detalhes do produto pelo ID
            produto = self.db.execute_select_one(query, (produto_id,))
            return produto
        except Exception as e:
            print(f"Erro ao obter detalhes do produto: {e}")
            return None
        
    def get_fornecedores_produto(self, produto_id):
        query = """SELECT 
            p.id AS produto_id,
            p.nome AS produto_nome,
            f.cnpj AS fornecedor_id,
            f.razao_social AS fornecedor_nome,
            pf.quantidade AS quantidade_fornecida,
            pf.data AS data_fornecimento
        FROM 
            ProdutosFornecidos pf
        JOIN 
            Fornecedor f ON pf.idFornecedor = f.cnpj
        JOIN 
            Produto p ON pf.idProduto = p.id
        WHERE 
            p.id = %s  
        ORDER BY 
            pf.data DESC;
        """
        
        try:
            historico = self.db.execute_select_all(query, (produto_id,))
            
            for item in historico:
                item["data_fornecimento"] = item["data_fornecimento"].strftime("%Y-%m-%d")
            
            return historico
        except Exception as e:
            print(f"Erro ao buscar histórico de fornecedores para o produto {produto_id}: {e}")
            return []

   