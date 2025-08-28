# 📚 LibraryApp – .NET 8 + Angular 17 + MySQL (EF Core)

Projeto full-stack para CRUD de **Autores**, **Gêneros** e **Livros** com API .NET 8 e SPA Angular 17.  
Tecnologias: **.NET 8**, **EF Core + Pomelo MySQL**, **API Versioning**, **Swagger**, **xUnit**, **Angular 17 (standalone)**, **Angular Material**.

## 📦 Requisitos
- **.NET SDK 8.x**
- **Node 20+** e **Angular CLI 17+** (`npm i -g @angular/cli@17`)
- **MySQL 8.x**
- (Opcional) Docker (se quiser subir MySQL via container)

## 🧭 Estrutura de Pastas
```
/LibraryApp
  ├─ LibraryApp.Domain/
  ├─ LibraryApp.Infrastructure/
  ├─ LibraryApp.Api/
  ├─ LibraryApp.Tests/
  └─ library-spa/        # projeto Angular
```

---

## 🚀 Backend (.NET 8 + EF Core + MySQL)

### 1) Configurar Connection String
Arquivo: `LibraryApp.Api/appsettings.json`
```json
{
  "ConnectionStrings": {
    "Default": "server=localhost;port=3306;database=library_app;user=root;password=YourStrong@Password;TreatTinyAsBoolean=true;"
  }
}
```
### 2) Rodar update-database
```
na camada de infra abrir o terminal e rodar update-database
```

### 3) Rodar a API
```bash
dotnet run --project LibraryApp.Api
```
Swagger:
- Dev HTTPS: `https://localhost:5001/swagger`
- Dev HTTP : `http://localhost:5000/swagger`

### 4) Rotas (API v1)
- **Authors**
  - `GET /api/v1/authors`
  - `GET /api/v1/authors/{id}`
  - `POST /api/v1/authors` — `{ name }`
  - `PUT /api/v1/authors/{id}` — `{ name }`
  - `DELETE /api/v1/authors/{id}` *(bloqueia se houver livros)*
- **Genres**
  - `GET /api/v1/genres`
  - `GET /api/v1/genres/{id}`
  - `POST /api/v1/genres` — `{ name }`
  - `PUT /api/v1/genres/{id}` — `{ name }`
  - `DELETE /api/v1/genres/{id}` *(bloqueia se houver livros)*
- **Books**
  - `GET /api/v1/books`
  - `GET /api/v1/books/{id}`
  - `POST /api/v1/books` — `{ title, authorId, genreId }`
  - `PUT /api/v1/books/{id}` — `{ title, authorId, genreId }`
  - `DELETE /api/v1/books/{id}`

### 5) Testes de Unidade (xUnit)
```bash
dotnet test
```

---

## 🖥️ Frontend (Angular 17 + Angular Material)

### 1) Instalar dependências
```bash
cd library-spa
npm i
```

Se estiver usando Angular Material:
```bash
npm i @angular/material @angular/cdk @angular/animations
```

Se optou por **tema pré-pronto** do Material, no `angular.json` adicione:
```json
"styles": [
  "node_modules/@angular/material/prebuilt-themes/indigo-pink.css",
  "src/styles.scss"
]
```
E verifique se o `main.ts` possui:
```ts
import { provideAnimations } from '@angular/platform-browser/animations';
// ...
providers: [ /* ... */, provideAnimations() ]
```

### 2) Configurar URL da API
Arquivo: `library-spa/src/environments/environment.ts`
```ts
export const environment = {
  production: false,
  apiBase: 'https://localhost:5001/api/v1'
};
```

### 3) Rodar o Frontend
```bash
npm start
# ou
ng serve -o
```
Aplicação: `http://localhost:4200/`

---

## 🧩 Regras de Negócio
- Um **Autor** possui N **Livros**.
- Um **Gênero** possui N **Livros**.
- Cada **Livro** pertence a **1 Autor** e **1 Gênero**.
- Exclusão de Autor/Gênero é bloqueada se houver Livros relacionados.

---

## 🧱 Padrões e Boas Práticas Atendidas
- **Camadas**: Domain / Infrastructure / Api / Tests
- **DTOs & ViewModels**
- **Injeção de Dependência**
- **Versionamento de API** (`/api/v1`)
- **Swagger** (documentação automática)
- **Respostas padronizadas**: `{ success, data, errors }`
- **Migrations EF Core**
- **Testes de Unidade** (xUnit)
- **Angular Standalone** + **Services** + **Models** + **Rotas**
- **Angular Material**: UI profissional (toolbar, tables, forms, dialog)
- **Filtro nas Listagens** e **Modal de Confirmação** de exclusão

---

## 🛠️ Comandos Rápidos

### Backend
```bash
# criar migration (se necessário)
dotnet ef migrations add InitialCreate -p LibraryApp.Infrastructure -s LibraryApp.Api

# atualizar banco
dotnet ef database update -p LibraryApp.Infrastructure -s LibraryApp.Api

# executar API
dotnet run --project LibraryApp.Api
```

### Frontend
```bash
cd library-spa
npm i
npm start  # ou ng serve -o
```

### Testes
```bash
dotnet test
```

---

## 🧪 Troubleshooting

**Erro:** “Your startup project doesn't reference Microsoft.EntityFrameworkCore.Design”  
**Fix:**
```bash
dotnet add LibraryApp.Infrastructure package Microsoft.EntityFrameworkCore.Design --version 8.*
```

**Erro Sass/Material:** “Undefined function” (`mat.define-palette`)  
**Fix:** Use tema pré-pronto (ver seção Frontend) **ou** garanta que `@use '@angular/material' as mat;` está no `styles.scss`.

**CORS bloqueando o front**  
Atualize o CORS no `Program.cs` da API para incluir `http://localhost:4200`.

---

## 📤 Como rodar localmente (passo a passo rápido)

```bash
# 1) Backend
dotnet tool restore
dotnet ef database update -p LibraryApp.Infrastructure -s LibraryApp.Api
dotnet run --project LibraryApp.Api

# 2) Frontend
cd library-spa
npm i
ng serve -o
```
