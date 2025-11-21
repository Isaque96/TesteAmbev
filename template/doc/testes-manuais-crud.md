### 6. Testes Manuais de CRUD

Este documento descreve testes manuais básicos para validação das operações de **criação, leitura, atualização e exclusão (CRUD)** da API **Ambev Developer Evaluation API**, utilizando ferramentas como **Postman**.

Os testes cobrem os recursos:

- Autenticação (`/api/Auth`)
- Usuários (`/api/Users`)
- Produtos (`/api/Products`)
- Categorias (`/api/Categories`)
- Carrinhos (`/api/Carts`)

---

#### 6.1. Pré-requisitos

1. **Base URL**
   - Definir no Postman uma variável de ambiente `{{baseUrl}}`, apontando para a URL da API.  
     Exemplo:
     - `{{baseUrl}} = https://localhost:5001` (ajustar conforme o ambiente).

2. **Autenticação (JWT Bearer)**  
   - A API utiliza autenticação `Bearer` com JWT (ver `securitySchemes.Bearer` no Swagger).
   - Antes de executar os testes de CRUD, obtenha um token JWT via endpoint `/api/Auth`.

   **Request**
   - Método: `POST`
   - URL: `{{baseUrl}}/api/Auth`
   - Body (JSON – exemplo):
     ```json
     {
       "email": "usuario@teste.com",
       "password": "SenhaForte123"
     }
     ```

   **Response esperada (200)**
   ```json
   {
     "success": true,
     "message": null,
     "errors": null,
     "data": {
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
       "tokenType": "Bearer",
       "email": "usuario@teste.com",
       "name": "Nome do Usuário",
       "role": "..."
     }
   }
   ```

3. **Configurar Authorization no Postman**
   - Em cada requisição autenticada:
     - Header `Authorization`: `Bearer {{token}}`
   - Armazene `{{token}}` como variável de ambiente após o login.

---

#### 6.2. CRUD de Usuários (`/api/Users`)

##### 6.2.1. Criar Usuário (Create)

- **Endpoint:** `POST /api/Users`
- **URL:** `{{baseUrl}}/api/Users`
- **Headers:**
  - `Authorization: Bearer {{token}}`
  - `Content-Type: application/json`

**Body (exemplo)**  
`UserStatus` e `UserRole` são inteiros (0–3). Usar valores válidos conforme a implementação.

```json
{
  "username": "joao.silva",
  "password": "SenhaForte123",
  "phone": "+55 11 99999-0000",
  "email": "joao.silva@teste.com",
  "status": 1,
  "role": 1
}
```

**Resultados esperados**

- **201 Created**
  - Corpo parecido com:
    ```json
    {
      "success": true,
      "data": {
        "id": "UUID",
        "name": "joao.silva",
        "email": "joao.silva@teste.com",
        "phone": "+55 11 99999-0000",
        "role": 1,
        "status": 1
      }
    }
    ```
- **400 Bad Request** para dados inválidos (campos obrigatórios ausentes, formato de e-mail inválido etc.).

---

##### 6.2.2. Listar Usuários (Read – List)

- **Endpoint:** `GET /api/Users`
- **URL base:** `{{baseUrl}}/api/Users`
- **Parâmetros de query (opcionais):**
  - `Name`: filtra por nome.
  - `Email`: filtra por e-mail.
  - `_page`: número da página (int).
  - `_size`: tamanho da página (int).
  - `_order`: ordenação (ex.: `"name"`, `"email"`, `"name_desc"` – ajustar conforme implementação).

**Exemplos de chamadas**

1. Listar todos:
   - `GET {{baseUrl}}/api/Users`

2. Filtrar por nome:
   - `GET {{baseUrl}}/api/Users?Name=joao`

3. Paginação:
   - `GET {{baseUrl}}/api/Users?_page=1&_size=10`

4. Ordenação:
   - `GET {{baseUrl}}/api/Users?_order=name`

**Resultado esperado (200)**  
Lista de usuários no campo `data`.

---

##### 6.2.3. Obter Usuário por ID (Read – Get by Id)

- **Endpoint:** `GET /api/Users/{id}`
- **URL:** `{{baseUrl}}/api/Users/{{userId}}`

**Cenários**

1. **ID existente**
   - Esperado: `200 OK` com objeto `GetUserResponse` no `data`.

2. **ID inexistente ou inválido**
   - Esperado: `404 Not Found` ou `400 Bad Request` conforme o caso.

---

##### 6.2.4. Atualizar Usuário (Update)

- **Endpoint:** `PUT /api/Users/{id}`
- **URL:** `{{baseUrl}}/api/Users/{{userId}}`

**Body (exemplo)**

```json
{
  "id": "{{userId}}",
  "name": "João da Silva Atualizado",
  "email": "joao.atualizado@teste.com",
  "isActive": true
}
```

**Resultados esperados**

- `200 OK` com objeto atualizado.
- `400 Bad Request` para payload inválido.
- `404 Not Found` se `id` não existir.

---

##### 6.2.5. Excluir Usuário (Delete)

- **Endpoint:** `DELETE /api/Users/{id}`
- **URL:** `{{baseUrl}}/api/Users/{{userId}}`

**Cenários**

1. **Usuário existente**
   - `200 OK` com `success = true`.
   - Após exclusão, um `GET /api/Users/{id}` deve retornar `404 Not Found`.

2. **Usuário inexistente**
   - `404 Not Found`.

---

#### 6.3. CRUD de Categorias (`/api/Categories`)

##### 6.3.1. Criar Categoria

- **POST {{baseUrl}}/api/Categories**

```json
{
  "name": "Eletrônicos",
  "description": "Produtos eletrônicos em geral"
}
```

- Esperado: `201 Created` com `CreateCategoryResponse` em `data`.

---

##### 6.3.2. Listar Categorias (+ filtros, paginação, ordenação)

- **GET {{baseUrl}}/api/Categories**
- Parâmetros:
  - `Name`
  - `_page`
  - `_size`
  - `_order`

**Exemplos**

- Filtro por nome:
  - `GET {{baseUrl}}/api/Categories?Name=Eletrônicos`
- Paginação:
  - `GET {{baseUrl}}/api/Categories?_page=1&_size=5`
- Ordenação:
  - `GET {{baseUrl}}/api/Categories?_order=name`

Esperado: `200 OK` com lista em `data`.

---

##### 6.3.3. Obter Categoria por ID

- **GET {{baseUrl}}/api/Categories/{{categoryId}}**

Cenários:
- ID existente → `200 OK`
- ID inexistente → `404 Not Found`
- ID inválido (não UUID) → `400 Bad Request`

---

##### 6.3.4. Atualizar Categoria

- **PUT {{baseUrl}}/api/Categories/{{categoryId}}**

```json
{
  "id": "{{categoryId}}",
  "name": "Eletrônicos Atualizado",
  "description": "Nova descrição"
}
```

Esperado:
- `200 OK` com objeto atualizado.
- `400/404` conforme erro.

---

##### 6.3.5. Excluir Categoria

- **DELETE {{baseUrl}}/api/Categories/{{categoryId}}**

Esperado:
- `200 OK` se existir.
- `404 Not Found` se não existir.

---

#### 6.4. CRUD de Produtos (`/api/Products`)

##### 6.4.1. Criar Produto

- **POST {{baseUrl}}/api/Products**

```json
{
  "title": "Smartphone X",
  "price": 1999.90,
  "description": "Smartphone de teste",
  "image": "https://exemplo.com/imagens/smartphone-x.png",
  "categoryId": "{{categoryId}}",
  "rating": {
    "rate": 4.5,
    "count": 10
  }
}
```

Esperado:
- `201 Created` com `CreateProductResponse` em `data`.
- Conferir se o campo `category` está coerente com a categoria associada.

---

##### 6.4.2. Listar Produtos (filtros, paginação, ordenação)

- **GET {{baseUrl}}/api/Products**
- Parâmetros:
  - `page` (default 1)
  - `size` (default 10)
  - `order`
  - `title`
  - `category`
  - `minPrice`
  - `maxPrice`

**Casos a validar:**

1. **Listagem simples**
   - `GET {{baseUrl}}/api/Products`
   - Esperado: `200 OK` com lista em `data`.

2. **Filtro por título**
   - `GET {{baseUrl}}/api/Products?title=Smartphone`

3. **Filtro por categoria**
   - `GET {{baseUrl}}/api/Products?category=Eletrônicos`

4. **Filtro por faixa de preço**
   - `GET {{baseUrl}}/api/Products?minPrice=1000&maxPrice=3000`

5. **Paginação**
   - `GET {{baseUrl}}/api/Products?page=1&size=5`
   - `GET {{baseUrl}}/api/Products?page=2&size=5`

6. **Ordenação**
   - `GET {{baseUrl}}/api/Products?order=price`
   - Verificar se ordem dos itens respeita o critério configurado na API.

---

##### 6.4.3. Obter Produto por ID

- **GET {{baseUrl}}/api/Products/{{productId}}**

Cenários:
- Existente → `200 OK`
- Inexistente → `404 Not Found`

---

##### 6.4.4. Atualizar Produto

- **PUT {{baseUrl}}/api/Products/{{productId}}**

```json
{
  "id": "{{productId}}",
  "title": "Smartphone X Atualizado",
  "price": 1899.90,
  "description": "Nova descrição",
  "image": "https://exemplo.com/imagens/smartphone-x-v2.png",
  "categoryId": "{{categoryId}}",
  "rating": {
    "rate": 4.7,
    "count": 20
  }
}
```

Esperado:
- `200 OK` com objeto atualizado.
- `400` se payload inválido.
- `404` se `productId` não existir.

---

##### 6.4.5. Excluir Produto

- **DELETE {{baseUrl}}/api/Products/{{productId}}**

Após exclusão, tentar:
- `GET /api/Products/{{productId}}` → esperado `404`.

---

##### 6.4.6. Listar Categorias de Produtos

- **GET {{baseUrl}}/api/Products/categories**
- Esperado: `200 OK` e `data` como array de strings (nomes de categorias).

---

##### 6.4.7. Listar Produtos por Categoria (com paginação e ordenação)

- **GET {{baseUrl}}/api/Products/category/{category}**
- Parâmetros:
  - `category` (path)
  - `_page` (default 1)
  - `_size` (default 10)
  - `_order`

**Exemplo**

- `GET {{baseUrl}}/api/Products/category/Eletrônicos?_page=1&_size=5&_order=price`

Esperado:
- `200 OK` com `ListProductsByCategoryResponse` em `data`, incluindo `totalItems`, `currentPage`, `totalPages`.

---

#### 6.5. CRUD de Carrinhos (`/api/Carts`)

##### 6.5.1. Criar Carrinho

- **POST {{baseUrl}}/api/Carts**

```json
{
  "userId": "{{userId}}",
  "date": "2025-01-01T10:00:00Z",
  "products": [
    {
      "productId": "{{productId1}}",
      "quantity": 2
    },
    {
      "productId": "{{productId2}}",
      "quantity": 1
    }
  ]
}
```

Esperado:
- `201 Created` com `CreateCartResponse` em `data`.
- Campos retornados: `id`, `userId`, `date`, `products`, `createdAt`.

---

##### 6.5.2. Listar Carrinhos (filtros, paginação, ordenação)

- **GET {{baseUrl}}/api/Carts**
- Parâmetros:
  - `UserId` (UUID)
  - `MinDate` (date-time)
  - `MaxDate` (date-time)
  - `_page`
  - `_size`
  - `_order`

**Casos de teste:**

1. Listar todos:
   - `GET {{baseUrl}}/api/Carts`

2. Filtrar por usuário:
   - `GET {{baseUrl}}/api/Carts?UserId={{userId}}`

3. Filtrar por intervalo de datas:
   - `GET {{baseUrl}}/api/Carts?MinDate=2025-01-01T00:00:00Z&MaxDate=2025-01-31T23:59:59Z`

4. Paginação:
   - `GET {{baseUrl}}/api/Carts?_page=1&_size=5`

5. Ordenação:
   - `GET {{baseUrl}}/api/Carts?_order=date`

Esperado:
- `200 OK` com lista em `data`, contendo `totalPrice` calculado.

---

##### 6.5.3. Obter Carrinho por ID

- **GET {{baseUrl}}/api/Carts/{{cartId}}**

Cenários:
- ID válido/existente → `200 OK`
- ID inexistente → `404 Not Found`
- ID inválido (não UUID) → `400 Bad Request`

---

##### 6.5.4. Atualizar Carrinho

- **PUT {{baseUrl}}/api/Carts/{{cartId}}**

```json
{
  "id": "{{cartId}}",
  "userId": "{{userId}}",
  "date": "2025-01-02T10:00:00Z",
  "products": [
    {
      "productId": "{{productId1}}",
      "quantity": 3
    }
  ]
}
```

Esperado:
- `200 OK` com `UpdateCartResponse` em `data`, incluindo `totalPrice` atualizado e `updatedAt` preenchido.

---

##### 6.5.5. Excluir Carrinho

- **DELETE {{baseUrl}}/api/Carts/{{cartId}}**

Cenários:
- Carrinho existente → `200 OK`.
- Nova tentativa de `GET /api/Carts/{{cartId}}` deve retornar `404`.
- Carrinho inexistente → `404 Not Found`.

---

### 6.6. Validações Negativas Gerais

Para cada recurso (Users, Products, Categories, Carts), recomenda-se ainda:

- Enviar payload sem campos obrigatórios → esperar `400 Bad Request` com `errors` preenchido.
- Enviar IDs com formato inválido (não UUID) → `400 Bad Request`.
- Acessar endpoints protegidos sem header `Authorization` → `401 Unauthorized`.
- Testar paginação com `_page`/`page` <= 0 ou `_size`/`size` <= 0 → verificar tratamento (idealmente `400`).
