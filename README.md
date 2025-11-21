# Ambev Developer Evaluation API

Este documento descreve como eu **concluí todos os requisitos do projeto** e apresenta uma **documentação em Markdown** para uso da API, com exemplos práticos baseados no `swagger.json` e nos dados de seed do banco.

---

## ✅ Requisitos Atendidos

### 1. Banco de Dados
- Banco implementado com **EF Core** e mapeamento objeto-relacional.
- Entidades principais: `User`, `Category`, `Product`, `Cart`, `CartItem`, `Rating`, `Address`, `Name`, `Geolocation`.
- Seed inicial configurado em `DatabaseSeeder`:
  - 2 usuários (Admin e Cliente).
  - 2 categorias (Cervejas, Refrigerantes).
  - 3 produtos.
  - 1 carrinho com itens.

### 2. Classes de Manipulação de Dados
- Implementadas classes para manipulação de dados com **design patterns** e **DTOs** claros (ex.: `CreateUserRequest`, `CreateProductRequest`, `ListUsersResponse`).

### 3. Camada de Serviços
- Camada de serviços exposta através de controllers REST:
  - `/api/Auth`
  - `/api/Users`
  - `/api/Products`
  - `/api/Categories`
  - `/api/Carts`
- Mensagens de retorno padronizadas com `ApiResponse` e `ApiResponseWithData<T>`.

### 4. Regras de Negócio
- Cálculo de total do carrinho.
- Segurança com perfis (`UserRole`) e status (`UserStatus`).
- Paginação, filtros e ordenação em listagens.
- Validações com retorno estruturado de erros (`ValidationErrorDetail`).

### 5. Registro em Application Log
- Registro de eventos relevantes em **Application Log** (operações CRUD, falhas de autenticação, etc.).

### 6. Testes Manuais de CRUD
- Testes manuais de CRUD realizados via **API** (Postman/Swagger):
  - `Users`: criação, listagem, atualização, exclusão.
  - `Products`: criação, listagem com filtros, atualização, exclusão.
  - `Categories`: CRUD completo.
  - `Carts`: criação, listagem, atualização, exclusão.
- Filtros de paginação, ordenação e busca verificados.

### 7. Geração de Massa de Dados de Teste
- Seed inicial de dados implementado em `DatabaseSeeder.cs`, com:
  - Usuários de exemplo.
  - Categorias e produtos reais para testes de carrinho.
  - Carrinho pré-criado para o usuário `cliente1`.

### 8. Testes de Unidade
- Testes de unidade com **xUnit**, cobrindo regras de negócio principais.

---

## 👤 Usuários Seedados no Banco

### 1. Administrador

```json
{
  "username": "admin",
  "email": "admin@ambev.dev",
  "phone": "(11) 9999-0000",
  "role": "Admin",
  "status": "Active",
  "name": {
    "firstName": "Admin",
    "lastName": "User"
  },
  "address": {
    "city": "São Paulo",
    "street": "Av. Paulista",
    "number": 1000,
    "zipCode": "01310-000",
    "geolocation": {
      "lat": "-23.561414",
      "long": "-46.655881"
    }
  }
}
```

### 2. Cliente

```json
{
  "username": "cliente1",
  "email": "cliente1@ambev.dev",
  "phone": "(11) 98888-0000",
  "role": "Customer",
  "status": "Active",
  "name": {
    "firstName": "João",
    "lastName": "Silva"
  },
  "address": {
    "city": "São Paulo",
    "street": "Rua das Flores",
    "number": 123,
    "zipCode": "01000-000",
    "geolocation": {
      "lat": "-23.550520",
      "long": "-46.633308"
    }
  }
}
```

> Senhas usadas no seed (hashes gerados via `passwordHasher`):
> - Admin: `Admin@123`
> - Cliente: `Cliente@123`

---

## 🔐 Autenticação (JWT)

A API utiliza **JWT** com esquema `Bearer`.

Header obrigatório nas rotas protegidas:

```http
Authorization: Bearer {seu_token_jwt}
```

### `POST /api/Auth`

Autentica um usuário e retorna um token JWT.

**Request**

```json
{
  "email": "admin@ambev.dev",
  "password": "Admin@123"
}
```

**Response 200 (exemplo)**

```json
{
  "success": true,
  "message": "Authenticated successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "tokenType": "Bearer",
    "email": "admin@ambev.dev",
    "name": "Admin User",
    "role": "Admin"
  }
}
```

---

## 👥 Users

### Criar Usuário – `POST /api/Users`

```http
POST /api/Users
Authorization: Bearer {token_admin}
Content-Type: application/json
```

```json
{
  "username": "cliente2",
  "password": "Cliente2@123",
  "phone": "(11) 97777-0000",
  "email": "cliente2@ambev.dev",
  "firstName": "Maria",
  "lastName": "Oliveira",
  "latitude": "-23.550000",
  "longitude": "-46.630000",
  "city": "São Paulo",
  "number": "200",
  "street": "Rua Nova",
  "zipCode": "01001-000",
  "status": 1,
  "role": 2
}
```

### Listar Usuários – `GET /api/Users`

```http
GET /api/Users?Name=Jo%C3%A3o&Email=cliente1@ambev.dev&_page=1&_size=10&_order=name
Authorization: Bearer {token_admin}
```

---

## 🛍 Products & Categories

### Listar Categorias – `GET /api/Categories`

```http
GET /api/Categories
Authorization: Bearer {token}
```

### Listar Produtos – `GET /api/Products`

Parâmetros: `page`, `size`, `order`, `title`, `category`, `minPrice`, `maxPrice`.

```http
GET /api/Products?page=1&size=10
Authorization: Bearer {token}
```

Exemplo de resposta baseado no seed:

```json
{
  "success": true,
  "data": [
    {
      "title": "Cerveja Lager 350ml",
      "price": 4.99,
      "category": "Cervejas"
    },
    {
      "title": "Cerveja IPA 600ml",
      "price": 12.9,
      "category": "Cervejas"
    },
    {
      "title": "Refrigerante Cola 2L",
      "price": 8.5,
      "category": "Refrigerantes"
    }
  ]
}
```

### Criar Produto – `POST /api/Products`

```http
POST /api/Products
Authorization: Bearer {token_admin}
Content-Type: application/json
```

```json
{
  "title": "Cerveja Pilsen 269ml",
  "price": 3.5,
  "description": "Cerveja tipo Pilsen, lata 269ml",
  "image": "https://example.com/images/cerveja-pilsen-269ml.jpg",
  "categoryId": "GUID_CERVEJAS",
  "rating": {
    "rate": 4.3,
    "count": 50
  }
}
```

---

## 🧺 Carts

O seed cria um carrinho inicial para o usuário `cliente1` com dois itens.

Estrutura conceitual:

```json
{
  "userId": "GUID_CLIENTE1",
  "products": [
    { "productId": "GUID_PROD_1", "quantity": 3 },
    { "productId": "GUID_PROD_2", "quantity": 6 }
  ]
}
```

### Criar Carrinho – `POST /api/Carts`

```http
POST /api/Carts
Authorization: Bearer {token_cliente}
Content-Type: application/json
```

```json
{
  "userId": "GUID_CLIENTE1",
  "date": "2025-11-21T14:00:00Z",
  "products": [
    { "productId": "GUID_PROD_1", "quantity": 2 },
    { "productId": "GUID_PROD_3", "quantity": 1 }
  ]
}
```

### Listar Carts – `GET /api/Carts`

```http
GET /api/Carts?UserId=GUID_CLIENTE1&_page=1&_size=10&_order=date_desc
Authorization: Bearer {token}
```

---

## Fluxo Sugerido de Uso

1. **Autenticar como admin** (`POST /api/Auth`).
2. **Criar usuários, categorias e produtos** (`/api/Users`, `/api/Categories`, `/api/Products`).
3. **Autenticar como cliente** (`cliente1@ambev.dev`).
4. **Criar e consultar carrinhos** (`/api/Carts`).

Este README demonstra que todos os requisitos do template foram atendidos e mostra exemplos reais usando os dados seedados no banco.
