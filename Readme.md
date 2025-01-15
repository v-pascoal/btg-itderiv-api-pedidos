
# BTG - IT Derivativos - Pedidos API

## Descrição

Este microserviço é responsável por gerenciar pedidos. Ele utiliza RabbitMQ para a fila, MongoDB como banco de dados, e expõe APIs para operações relacionadas a pedidos.

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [BTG-Infra](https://github.com/v-pascoal/btg-infra)

## Configuração

### 1. Configurar BTG-Infra

O projeto [BTG-Infra](https://github.com/v-pascoal/btg-infra) é responsável por fornecer a infraestrutura necessária para rodar este projeto no docker. É através dela que terá a disposição o MongoDB e o RabbitMQ. Após baixá-la e configurá-la, siga o passo-a-passo do repositório.

### 2. Configurar Variáveis de Ambiente

Após baixar o projeto, altere o arquivo [appsettings.json](./appsettings.json), na raiz do projeto, e ajuste as portas, urls, usuários e senhas para que fiquem de acordo com o [docker-compose.yaml](https://github.com/v-pascoal/btg-infra/blob/main/docker-compose.yaml) do BTG-Infra. 

### 3. Usando as APIs

Abaixo estão os detalhes dos endpoints disponíveis e o que deve ser enviado para cada um.

#### 3.1. Valor Total do Pedido

**Endpoint:** `GET /api/pedido/{codigoCliente}/{codigoPedido}/valor-total`

**Descrição:** Retorna o valor total do pedido para um cliente específico.

**Parâmetros:**
- `codigoCliente` (int): Código do cliente.
- `codigoPedido` (int): Código do pedido.

**Exemplo de Requisição Local:**
```bash
curl -X GET "http://localhost/api/pedido/123/456/valor-total"
```

#### 3.2. Quantidade de Pedidos por Cliente

**Endpoint:** `GET /api/pedido/cliente/{codigoCliente}/quantidade`

**Descrição:** Retorna a quantidade de pedidos para um cliente específico.

**Parâmetros:**
- `codigoCliente` (int): Código do cliente.

**Exemplo de Requisição:**
```bash
curl -X GET "http://localhost/api/pedido/cliente/123/quantidade"
```

#### 3.3. Lista de Pedidos por Cliente

**Endpoint:** `GET /api/pedido/cliente/{codigoCliente}/pedidos`

**Descrição:** Retorna a lista de pedidos para um cliente específico.

**Parâmetros:**
- `codigoCliente` (int): Código do cliente.

**Exemplo de Requisição:**
```bash
curl -X GET "http://localhost/api/pedido/cliente/123/pedidos"
```

#### 3.4. Health Check

**Endpoint:** `GET /health`

**Descrição:** Verifica a saúde da aplicação.

**Exemplo de Requisição:**
```bash
curl -X GET "http://localhost/health"
```