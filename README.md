Observações:

- Foi criado um projeto core para centralizar todas as classes em comum.
- Para sair do padrão EF, foi utilizado Dapper para toda a manipulação de dados.
- Banco de dados utilizado foi o Sqlite.
- Para manter os dados seguros no token JWT, os mesmos foram criptografados/descriptografados com passphrase.
- Foi utilizado CQRS + Result pattern no projeto.
- Testes foram realizados para fins ilustrativos. O diferencial é que foi feito teste de integração, desde a controller até a inserção em um Sqlite em memória. Feito uso de testcontainers para executar docker redis durantes os testes.
- Polly para retry policy de chamadas para api.
- Idempotência sendo executada através de atributo da action. Dados salvos e recuperados do redis.
- Log de exemplo usando serilog
- Mensageria usando Rabbitmq e Rebus
- O projeto de tarifas basicamente é um Consoler.Application transformado em um Host. Diferente do Worker service, precisa configurar o Host manualmente.
