# Aplicação Keener
### _Cadastro de produtos e gestão de estoque_

### Requisitos

- Login
- Cadastro
- Cadastro de produtos
- Listagem de produtos
- Cadastro de movimentações de estoque (tanto saídas como entradas)
- Visualização/Listagem das movimentações realizadas

# Desenvolvimento

> Eu desenvolvi esta aplicação com Asp Net core 6 utilizando EF, Identity e SQL server.

#### Login e cadastro

> Nela projetei o sistema de login e cadastro permitindo visualização de tabelas e detalhes a anonimos.

#### Cadastro e listagem de produtos

> O sistema de cadastro e listagem de produtos foi feito com base em um CRUD com SQL server.

#### Cadastro e listagem de movimentações

> O sistema de Cadastro e Visualização de movimentações foi feito usando outra tabela com chave estrangeira a tabela de produtos, assim as tabelas tem relação de um para muitos.

> Para conectar a quantidade de movimentações de X produto ao produto X da tabela produtos, fiz um codigo que Atualiza a quantidade sempre que executado.

> Para não permitir que se movimentem produtos em quantidade negativa um erro é acionado.

### Funcionalidades adicionais (podem ser removidas ou não)

> Ao apagar um produtos suas movimentações seram excluidas Então cuidado.

> Ao apagar movimentações é possivel deixar a quantidade de produtos negativa, mas esta ocasião serve apenas para facilitar a remoção de movimentações.

> Sistema de Edição e Remoção podem ser removidos

> Botão de movimentação exclusivo para cada produto

