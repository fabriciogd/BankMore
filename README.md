Observações:

- Foi criado um projeto core para centralizar todas as classes em comum.
- Para sair do padrão EF, foi utilizado Dapper para toda a manipulação de dados.
- Banco de dados utilizado foi o Sqlite.
- Para manter os dados seguros no tpken JWT, os mesmos foram criptografados/descriptografados com passphrase.
- Foi utilizado CQRS + Result pattern no projeto.
