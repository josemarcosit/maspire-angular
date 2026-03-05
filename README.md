# MARSPIRE MOTORS

Um sistema completo de gerenciamento de veículos construído com **ASP.NET Core 7** e **Angular**, com autenticação JWT, autorização baseada em roles e HTTPS seguro.

## Descritivo do Projeto

Esta é uma aplicação web moderna que permite:
- Gerir um catálogo de veículos (criar, editar, visualizar, deletar)
- Gerenciar makes (fabricantes) e features (características)
  * endpoint de features aceita parâmetro `lang` ou cabeçalho `Accept-Language` para devolver nomes traduzidos conforme o idioma solicitado
- Upload de fotos para veículos
- Sistema de autenticação com JWT
- Controle de acesso baseado em roles
- HTTPS seguro em desenvolvimento local
- Interface responsiva com Bootstrap

## Arquitetura

```
┌──────────────────────────────────────────────────────────┐
│                  Vehicle Management System               │
│                  (Monolithic Architecture)               │
├──────────────────────────────────────────────────────────┤
│                                                          │
│  Frontend (Single Page Application)                      │
│  ├── Angular 14+                                         │
│  ├── TypeScript                                          │
│  └── Bootstrap 5                                         │
│                      │                                   │
│                      ↓ (HTTP/HTTPS)                      │
│                                                          │
│  Backend (API Server)                                    │
│  ├── ASP.NET Core 7                                      │
│  ├── Feature/Controllers (REST APIs)                             │
│  ├── Feature/Services (Business Logic)                           │
│  └── Persistence/Repositories (Data Access)                          │
│                      │                                   │
│                      ↓                                   │
│                                                          │
│  Data Layer                                              │
│  ├── Entity Framework Core                               │
│  ├── SQL Server Database                                 │
│  └── Migrations                                          │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

### Stack Tecnológico

**Backend:**
- ASP.NET Core 7.0
- Entity Framework Core (Code-First)
- SQL Server
- AutoMapper
- JWT (JSON Web Tokens)

**Frontend:**
- Angular 14+
- TypeScript
- Bootstrap 5
- ngx-toastr (notificações)
- Font Awesome (ícones)
- Reactive Forms

## Getting Started

### Pré-requisitos

- .NET 7 SDK
- Node.js (v14+) e npm
- SQL Server (LocalDB ou express)
- Visual Studio Code ou Visual Studio

### Instalação

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/maspire-angular.git
cd maspire-angular
```

2. **Configure o banco de dados**

Atualize a string de conexão em `appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\mssqllocaldb;Database=maspire;Integrated Security=true;Encrypt=false;"
}
```

3. **Execute as migrations**
```bash
dotnet ef database update
```

4. **Configure a chave JWT** (IMPORTANTE!)

Em `appsettings.json`, altere a chave secreta para algo mais seguro:
```json
"JwtSettings": {
  "Secret": "sua_chave_muito_secreta_com_minimo_32_caracteres!!!",
  "Issuer": "maspire-angular-issuer",
  "Audience": "maspire-angular-audience",
  "ExpirationMinutes": 480
}
```

### Executar a Aplicação

**Terminal 1 - Backend (ASP.NET Core)**
```bash
cd h:\Github\maspire-angular
dotnet run
```
Acessar: `https://localhost:7257`

**Terminal 2 - Frontend (Angular)**
```bash
cd ClientApp
npm install
npm start
```
Acessar: `http://localhost:4200`

Angular fará proxy automático das requisições `/api/*` para `https://localhost:7257`

## Autenticação e Autorização

### Sistema de Autenticação

A aplicação usa **JWT (JSON Web Tokens)** para autenticação:

1. Usuário faz login com email e senha
2. Backend gera um token JWT assinado
3. Frontend armazena o token no `localStorage`
4. Token é enviado em cada requisição no header `Authorization: Bearer <token>`
5. Backend valida o token em endpoints protegidos

### Registro e Login

**Endpoints de Autenticação:**

- `POST /api/auth/register` - Criar nova conta
  ```json
  {
    "email": "user@example.com",
    "fullName": "João da Silva",
    "password": "senha123"
  }
  ```

- `POST /api/auth/login` - Fazer login
  ```json
  {
    "email": "user@example.com",
    "password": "senha123"
  }
  ```

### Proteção de Rotas

Todas as rotas de veículos requerem autenticação:
- `GET /api/vehicles` - Listar veículos
- `POST /api/vehicles` - Criar veículo
- `PUT /api/vehicles/{id}` - Editar veículo
- `DELETE /api/vehicles/{id}` - Deletar veículo

A rota é protegida no Angular com o `AuthGuard`:
```typescript
{ 
  path: 'vehicles', 
  component: VehicleComponent, 
  canActivate: [AuthGuard] 
}
```

Se o usuário não estiver autenticado, é redirecionado para `/login`.

## HTTPS Local

A aplicação usa HTTPS seguro em desenvolvimento:

- **Backend**: `https://localhost:7257`
- **Frontend**: `http://localhost:4200` (proxy automático com HTTPS)
- **Certificado**: Auto-assinado e confiável no Windows

Se precisar regenerar o certificado:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

Veja [HTTPS_SETUP.md](HTTPS_SETUP.md) para mais detalhes.

## Configuração de Ambiente

### Arquivo de Configuração

**Localização**: `ClientApp/src/app/shared/config/environment.ts`

```typescript
export const environment = {
  apiUrl: 'https://localhost:7257',
  production: false
};
```

Use `environment.apiUrl` em todos os serviços para centralizar as URLs.

### Adicionar Novo Serviço

```typescript
// ClientApp/src/app/services/novo.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../shared/config/environment';

@Injectable({
  providedIn: 'root'
})
export class NovoService {
  private apiUrl = `${environment.apiUrl}/api/novo`;

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<any[]>(this.apiUrl);
  }
}
```

## Modelos de Dados

### Diagrama do Banco de Dados

![Schema](Docs/Schema.png)

O diagrama acima mostra a estrutura completa do banco de dados com todos os relacionamentos entre as entidades.



### Relacionamentos

- **User**: Usuários do sistema
- **Vehicle**: Veículo (belongs to Make e Model, has many Features e Photos)
- **Make**: Marca de veículos (has many Models)
- **Model**: Modelo de veículo (belongs to Make)
- **Feature**: Características disponíveis (many-to-many com Vehicle via VehicleFeature)
  - nomes traduzidos são guardados em tabela `FeatureTranslations` e servidos automaticamente quando um idioma é informado

  * disponível via endpoint `GET /api/vehicle/features` que agora aceita consulta `?lang=` para retornar nomes de acordo com o idioma informado.
- **VehicleFeature**: Junction table (N:N)
- **Photo**: Fotos do veículo (one-to-many)
- **Contact**: Contato do veículo (one-to-one)

## Testando a API

### Com REST Client (VS Code)

Use a extensão REST Client para testar os endpoints.

**Arquivo**: `Tests/RestClient.http`

```http
### Login
POST https://localhost:7257/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "senha123"
}

### Criar Veículo
POST https://localhost:7257/api/vehicles
Content-Type: application/json
Authorization: Bearer <seu-token-jwt>

{
  "makeId": 1,
  "modelId": 1,
  "isRegistered": true,
  "contact": {
    "name": "João Silva",
    "phone": "11999999999",
    "email": "joao@example.com"
  },
  "features": [1, 2, 3]
}
```

## Localização e Internacionalização (i18n)

A aplicação suporta **múltiplos idiomas** com localização completa no frontend e backend.

### Idiomas Suportados

- 🇧🇷 **Português (Brasil)** - `pt-BR` (padrão original)
- 🇺🇸 **English (US)** - `en-US` (padrão atual)
- 🇪🇸 **Español** - `es`

### Frontend - Seletor de Idioma

O componente `LanguageSelectorComponent` exibe um seletor dropdown com bandeiras de cada idioma:

```typescript
// ClientApp/src/app/shared/components/language-selector/language-selector.component.ts
selector: 'app-language-selector'
```

**Características:**
- Exibe bandeira emoji do país atual
- Dropdown com opções de idiomas
- Armazena a seleção em `localStorage` (persiste entre sessões)
- Emite evento ao `LanguageService` quando o idioma muda

**Uso no template:**
```html
<app-language-selector></app-language-selector>
```

### Frontend - Interceptor de Accept-Language

Um `HttpInterceptor` automaticamente adiciona o header `Accept-Language` em todas as requisições HTTP:

```typescript
// ClientApp/src/app/services/language-header.interceptor.ts
intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  const lang = this.languageService.getLanguage() || 'en-US';
  const modified = req.clone({
    setHeaders: {
      'Accept-Language': lang
    }
  });
  return next.handle(modified);
}
```

Isso permite que o backend saiba qual idioma o cliente deseja.

### Backend - Localization Services

O backend implementa **ASP.NET Core Localization** com arquivos `.resx`:

**Registro em `Program.cs`:**
```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Configurar culturas suportadas
var supportedCultures = new[] { "pt-BR", "en-US", "es" }
    .Select(c => new CultureInfo(c))
    .ToList();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);
```

**Classe marcadora:**
```csharp
// SharedResources.cs
namespace maspire_angular
{
    public class SharedResources { }
}
```

### Injeção de IStringLocalizer

Controllers injetam `IStringLocalizer<SharedResources>` para obter mensagens localizadas:

```csharp
// AuthController.cs
public class AuthController : Controller
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public AuthController(/* ... */, IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // ...
        return Ok(new AuthResult
        {
            Success = true,
            Message = _localizer["LoginSuccess"],  // Mensagem localizada!
            // ...
        });
    }
}
```

### Arquivos de Recurso (.resx)

Localização com chaves e valores em diferentes idiomas:

**Estrutura:**
```
Resources/
├── SharedResources.resx          # Padrão (fallback)
├── SharedResources.en-US.resx    # English
├── SharedResources.pt-BR.resx    # Português
└── SharedResources.es.resx       # Español
```

**Exemplo de conteúdo (`SharedResources.pt-BR.resx`):**
```xml
<data name="LoginSuccess" xml:space="preserve">
  <value>Login realizado com sucesso!</value>
</data>
<data name="FeatureNotFound" xml:space="preserve">
  <value>Característica não encontrada</value>
</data>
<data name="InvalidFileType" xml:space="preserve">
  <value>Tipo de arquivo inválido</value>
</data>
```

### Chaves de Recurso Disponíveis

| Chave | en-US | pt-BR | es |
|-------|-------|-------|-----|
| `LoginSuccess` | Login successful! | Login realizado com sucesso! | Inicio de sesión exitoso! |
| `FeatureFetchError` | Error fetching features | Erro ao obter características | Error al obtener características |
| `FeatureNotFound` | Feature not found | Característica não encontrada | Característica no encontrada |
| `IdMismatch` | ID does not match | ID não corresponde | El ID no coincide |
| `InvalidFileType` | Invalid file type | Tipo de arquivo inválido | Tipo de archivo inválido |
| `FileMaxSizeExceeded` | Maximum file size exceeded | Tamanho máximo de arquivo excedido | Tamaño de archivo máximo excedido |
| `EmptyFile` | Empty file | Arquivo vazio | Archivo vacío |
| `NullFile` | File is required | Arquivo é obrigatório | El archivo es obligatorio |

### Controllers com Localização

- **AuthController**: Mensagens de login
- **FeaturesController**: Mensagens de características (CRUD)
- **PhotosController**: Mensagens de validação de upload

### Fluxo de Localização

1. Usuário seleciona idioma no dropdown (frontend)
2. `LanguageService` armazena a seleção em `localStorage`
3. Cada requisição HTTP carrega o header `Accept-Language`
4. Backend recebe o header via `LanguageHeaderInterceptor`
5. ASP.NET Core `RequestLocalization` middleware define a cultura atual
6. `IStringLocalizer<SharedResources>` busca a mensagem no idioma correto
7. Resposta é enviada com mensagem localizada

### Adicionar Nova Mensagem Localizada

1. **Adicione a chave aos`.resx` em todos os idiomas:**
   ```xml
   <data name="MeusMensagem" xml:space="preserve">
     <value>My Message</value>
   </data>
   ```

2. **Use no controller:**
   ```csharp
   return BadRequest(_localizer["MinhaMsg"]);
   ```

### Alterar Idioma Padrão

Mude em `Program.cs` na configuração de `RequestLocalizationOptions`:
```csharp
DefaultRequestCulture = new RequestCulture("pt-BR")  // ou seu idioma
```

E em `ClientApp/src/app/services/language.service.ts`:
```typescript
return localStorage.getItem('language') || 'pt-BR';
```

## Dependências Principais

### Backend
- Microsoft.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- AutoMapper
- Azure.Storage.Blobs (para upload)

### Frontend
- @angular/core
- @angular/forms
- bootstrap
- ngx-toastr
- @fortawesome/angular-fontawesome

## Fluxo de Desenvolvimento com Watch

**Backend com watch mode:**
```bash
dotnet watch run
```

**Frontend com hot reload:**
```bash
cd ClientApp
npm start
```

As alterações serão compiladas automaticamente e o navegador recarregará.

## Variáveis de Ambiente

**Backend** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "Default": "Server=...;Database=maspire;..."
  },
  "JwtSettings": {
    "Secret": "sua-chave-secreta",
    "Issuer": "maspire-angular",
    "Audience": "maspire-angular-app",
    "ExpirationMinutes": 480
  }
}
```

## Deploy

### Deploy para Produção

1. **Backend - Azure App Service:**
   ```bash
   dotnet publish -c Release
   ```

2. **Frontend - Azure Static Web Apps:**
   ```bash
   cd ClientApp
   npm run build
   ```

3. Altere o `environment.ts` com a URL de produção:
   ```typescript
   export const environment = {
     apiUrl: 'https://seu-dominio.com',
     production: true
   };
   ```

## Troubleshooting

### Erro: "Cannot GET /"
- Certifique-se de que o backend está rodando
- Verifique a porta HTTPS (7257)

### Erro: "401 Unauthorized"
- Faça login novamente
- Verifique se o token está no localStorage
- Regenere o certificado se necessário

### Erro de CORS
- Verifique `launchSettings.json`
- Confirme que a porta HTTPS está correta

## Documentação Adicional

- [HTTPS_SETUP.md](HTTPS_SETUP.md) - Configuração de HTTPS local
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core)
- [Angular Documentation](https://angular.io/docs)

## Desenvolvimento

### Criar nova feature

1. Criar model em `Features/NewFeature/Model`
2. Criar migrations: `dotnet ef migrations add FeatureName`
3. Atualizar banco: `dotnet ef database update`
4. Criar controller em `Controllers/`
5. Criar serviço Angular em `ClientApp/src/app/services/`
6. Criar componentes em `ClientApp/src/app/`

## Licença

Este projeto está sob licença MIT.

## Contribuições

Contribuições são bem-vindas! 

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Contato

Para dúvidas ou sugestões, entre em contato através da seção Issues do GitHub.

---

Desenvolvido com ASP.NET Core e Angular
