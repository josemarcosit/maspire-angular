# MARSPIRE MOTORS

A comprehensive vehicle management system built with **ASP.NET Core 7** and **Angular**, featuring JWT authentication, role-based authorization, and secure HTTPS in a **monolithic architecture**.

## Project Overview

This is a modern web application that demonstrates a **monolithic architecture** where both frontend (Angular) and backend (ASP.NET Core) are tightly integrated and deployed as a single unit. The application allows:

- Manage a catalog of vehicles (create, edit, view, delete)
- Manage makes (manufacturers) and features (vehicle characteristics)
  * features endpoint supports a `lang` query parameter (or `Accept-Language` header) returning names translated according to the requested locale
- Upload photos for vehicles
- User authentication system with JWT
- Access control based on user roles
- Secure HTTPS in local development
- Responsive user interface with Bootstrap

## Monolithic Architecture

This project uses a **monolithic architecture** pattern, where:

- All application functionality is contained in a single codebase
- Backend (C# with ASP.NET Core) and Frontend (TypeScript with Angular) are tightly integrated
- Single deployment unit - backend serves both API and static Angular files
- Shared database for all application layers
- Simpler initial development compared to microservices
- Easier to debug and test during development phase
- Single point of scaling for the entire application

### Architecture Diagram

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
│  ├── Controllers (REST APIs)                             │
│  ├── Services (Business Logic)                           │
│  └── Repositories (Data Access)                          │
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

## Technology Stack

### Backend
- ASP.NET Core 7.0 (Monolithic Framework)
- Entity Framework Core (Code-First ORM)
- SQL Server (Relational Database)
- AutoMapper (Object Mapping)
- JWT (JSON Web Tokens for Authentication)

### Frontend
- Angular 14+ (Monolithic SPA)
- TypeScript (Typed JavaScript)
- Bootstrap 5 (CSS Framework)
- ngx-toastr (Toast Notifications)
- Font Awesome (Icon Library)
- Reactive Forms (Form Management)

## Getting Started

### Prerequisites

- .NET 7 SDK
- Node.js (v14+) with npm
- SQL Server (LocalDB or Express Edition)
- Visual Studio Code or Visual Studio 2022

### Installation

1. Clone the repository
```bash
git clone https://github.com/your-username/angular-vega.git
cd angular-vega
```

2. Configure the database connection in `appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\mssqllocaldb;Database=VegaDb;Integrated Security=true;Encrypt=false;"
}
```

3. Apply database migrations
```bash
dotnet ef database update
```

4. Configure JWT settings in `appsettings.json` (IMPORTANT!):
```json
"JwtSettings": {
  "Secret": "your_very_secret_key_with_minimum_32_characters!!!",
  "Issuer": "angular-vega-issuer",
  "Audience": "angular-vega-audience",
  "ExpirationMinutes": 480
}
```

### Running the Application

As a monolithic application, everything runs as a single unit:

**Terminal 1 - Start the entire application (Backend + Frontend)**
```bash
cd h:\Github\angular-vega
dotnet run
```
Access at: `https://localhost:7257`

The backend automatically serves:
- REST APIs at `/api/*`
- Angular SPA at `/` (wwwroot directory)

**Terminal 2 - Optional: Run Angular dev server with live reload**
```bash
cd ClientApp
npm install
npm start
```
Angular development server at: `http://localhost:4200`

This is perfect for development with hot module reload (HMR).

## Authentication and Authorization

### JWT Authentication System

The application uses **JWT (JSON Web Tokens)** for stateless authentication:

1. User submits credentials via login form
2. Backend validates credentials and generates JWT token
3. Frontend stores token in `localStorage`
4. Token is sent with every request in the `Authorization: Bearer <token>` header
5. Backend validates token signature and expiration on protected endpoints

### Authentication Endpoints

- `POST /api/auth/register` - Create new user account
  ```json
  {
    "email": "user@example.com",
    "fullName": "John Silva",
    "password": "password123"
  }
  ```

- `POST /api/auth/login` - User login
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

### Protected Routes

All vehicle-related routes require authentication:
- `GET /api/vehicles` - List all vehicles
- `POST /api/vehicles` - Create vehicle
- `PUT /api/vehicles/{id}` - Update vehicle
- `DELETE /api/vehicles/{id}` - Delete vehicle
- `GET /api/vehicles/{id}/photos` - Vehicle photos

Protected in Angular with `AuthGuard`:
```typescript
{ 
  path: 'vehicles', 
  component: VehicleComponent, 
  canActivate: [AuthGuard] 
}
```

Unauthenticated users are redirected to `/login`.

## HTTPS Local Development

The application uses secure HTTPS even in local development:

- Backend: `https://localhost:7257`
- Frontend: `http://localhost:4200` (proxies to HTTPS backend)
- Certificate: Self-signed and trusted in Windows

To regenerate the certificate if needed:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

See [HTTPS_SETUP.md](HTTPS_SETUP.md) for detailed information.

## Environment Configuration

### Environment File

**Location**: `ClientApp/src/app/shared/config/environment.ts`

```typescript
export const environment = {
  apiUrl: 'https://localhost:7257',
  production: false
};
```

All services use `environment.apiUrl` for centralized API configuration.

### Adding a New Service

```typescript
// ClientApp/src/app/services/example.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../shared/config/environment';

@Injectable({
  providedIn: 'root'
})
export class ExampleService {
  private apiUrl = `${environment.apiUrl}/api/example`;

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<any[]>(this.apiUrl);
  }
}
```

## Data Models

### Database Schema Diagram

![Database Schema](Docs/Schema.png)

The diagram above shows the complete database structure with all entity relationships.

### Entity Relationships

- **User**: System users with authentication
- **Vehicle**: Vehicle entity (belongs to Make and Model, has many Features and Photos)
- **Make**: Vehicle brand (has many Models)
- **Model**: Vehicle model (belongs to Make)
- **Feature**: Available features (many-to-many with Vehicle via VehicleFeature)
  - translated names are stored in `FeatureTranslations` table and served automatically when a language is specified
- **VehicleFeature**: Junction table (N:N relationship)
- **Photo**: Vehicle photos (one-to-many relationship)
- **Contact**: Vehicle contact (one-to-one relationship)

## Localization and Internationalization (i18n)

The application supports **multiple languages** with complete localization on both frontend and backend.

### Supported Languages

- 🇧🇷 **Portuguese (Brazil)** - `pt-BR` (original default)
- 🇺🇸 **English (US)** - `en-US` (current default)
- 🇪🇸 **Spanish** - `es`

### Frontend - Language Selector

The `LanguageSelectorComponent` displays a dropdown selector with flags for each language:

```typescript
// ClientApp/src/app/shared/components/language-selector/language-selector.component.ts
selector: 'app-language-selector'
```

**Features:**
- Displays flag emoji of the current language
- Dropdown with language options
- Stores selection in `localStorage` (persists across sessions)
- Emits event to `LanguageService` when language changes

**Usage in template:**
```html
<app-language-selector></app-language-selector>
```

### Frontend - Accept-Language Interceptor

An `HttpInterceptor` automatically adds the `Accept-Language` header to all HTTP requests:

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

This allows the backend to know which language the client prefers.

### Backend - Localization Services

The backend implements **ASP.NET Core Localization** with `.resx` resource files:

**Registration in `Program.cs`:**
```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Configure supported cultures
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

**Marker class:**
```csharp
// SharedResources.cs
namespace angular_vega
{
    public class SharedResources { }
}
```

### Injecting IStringLocalizer

Controllers inject `IStringLocalizer<SharedResources>` to retrieve localized messages:

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
            Message = _localizer["LoginSuccess"],  // Localized message!
            // ...
        });
    }
}
```

### Resource Files (.resx)

Localization with keys and values in different languages:

**Structure:**
```
Resources/
├── SharedResources.resx          # Default (fallback)
├── SharedResources.en-US.resx    # English
├── SharedResources.pt-BR.resx    # Portuguese
└── SharedResources.es.resx       # Spanish
```

**Example content (`SharedResources.en-US.resx`):**
```xml
<data name="LoginSuccess" xml:space="preserve">
  <value>Login successful!</value>
</data>
<data name="FeatureNotFound" xml:space="preserve">
  <value>Feature not found</value>
</data>
<data name="InvalidFileType" xml:space="preserve">
  <value>Invalid file type</value>
</data>
```

### Available Resource Keys

| Key | en-US | pt-BR | es |
|-----|-------|-------|-----|
| `LoginSuccess` | Login successful! | Login realizado com sucesso! | Inicio de sesión exitoso! |
| `FeatureFetchError` | Error fetching features | Erro ao obter características | Error al obtener características |
| `FeatureNotFound` | Feature not found | Característica não encontrada | Característica no encontrada |
| `IdMismatch` | ID does not match | ID não corresponde | El ID no coincide |
| `InvalidFileType` | Invalid file type | Tipo de arquivo inválido | Tipo de archivo inválido |
| `FileMaxSizeExceeded` | Maximum file size exceeded | Tamanho máximo de arquivo excedido | Tamaño de archivo máximo excedido |
| `EmptyFile` | Empty file | Arquivo vazio | Archivo vacío |
| `NullFile` | File is required | Arquivo é obrigatório | El archivo es obligatorio |

### Controllers with Localization

- **AuthController**: Login messages
- **FeaturesController**: Feature CRUD messages
- **PhotosController**: Photo upload validation messages

### Localization Flow

1. User selects language in dropdown (frontend)
2. `LanguageService` stores selection in `localStorage`
3. Each HTTP request carries the `Accept-Language` header
4. Backend receives header via `LanguageHeaderInterceptor`
5. ASP.NET Core `RequestLocalization` middleware sets the current culture
6. `IStringLocalizer<SharedResources>` retrieves the message in the correct language
7. Response is sent with localized message

### Adding a New Localized Message

1. **Add the key to all `.resx` files:**
   ```xml
   <data name="MyMessage" xml:space="preserve">
     <value>My Message</value>
   </data>
   ```

2. **Use in controller:**
   ```csharp
   return BadRequest(_localizer["MyMessage"]);
   ```

### Changing Default Language

Modify in `Program.cs` in the `RequestLocalizationOptions` configuration:
```csharp
DefaultRequestCulture = new RequestCulture("pt-BR")  // or your default
```

And in `ClientApp/src/app/services/language.service.ts`:
```typescript
return localStorage.getItem('language') || 'pt-BR';
```

## Testing the API

### Using REST Client Extension

Use VS Code REST Client extension to test endpoints.

**File**: `Tests/RestClient.http`

```http
### User Registration
POST https://localhost:7257/api/auth/register
Content-Type: application/json

{
  "email": "newuser@example.com",
  "fullName": "John Doe",
  "password": "password123"
}

### User Login
POST https://localhost:7257/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}

### Create Vehicle (requires authentication)
POST https://localhost:7257/api/vehicles
Content-Type: application/json
Authorization: Bearer <your-jwt-token>

{
  "makeId": 1,
  "modelId": 1,
  "isRegistered": true,
  "contact": {
    "name": "John Silva",
    "phone": "+5511999999999",
    "email": "john@example.com"
  },
  "features": [1, 2, 3]
}
```

## Development Workflow

### Watch Mode (Backend)
```bash
dotnet watch run
```
Backend automatically recompiles when source files change.

### Hot Module Reload (Frontend)
```bash
cd ClientApp
npm start
```
Angular development server with automatic browser reload.

## Main Dependencies

### Backend
- Microsoft.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- AutoMapper
- Microsoft.IdentityModel.Tokens

### Frontend
- @angular/core
- @angular/forms
- bootstrap
- ngx-toastr
- @fortawesome/angular-fontawesome

## Production Deployment

### Publishing the Monolithic Application

Since this is a monolithic architecture, you deploy everything as one unit:

```bash
cd h:\Github\angular-vega
dotnet publish -c Release -o ./publish
```

This creates a single deployment package containing:
- Compiled backend code
- Pre-built Angular SPA
- All assets and dependencies

### Deploy to Azure App Service

```bash
# Create publish profile first, then:
dotnet publish -c Release
az webapp deployment source config-zip --resource-group myGroup --name myApp --src publish.zip
```

### Advantages of Monolithic Deployment
- Simple deployment process (one package)
- No cross-service communication issues
- Shared authentication and authorization
- Easier to maintain consistency
- Lower operational complexity

## Environment Variables

### Backend Configuration
**File**: `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=VegaDb;Integrated Security=true;"
  },
  "JwtSettings": {
    "Secret": "development-secret-key",
    "Issuer": "angular-vega",
    "Audience": "angular-vega-app",
    "ExpirationMinutes": 480
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Troubleshooting

### Error: "Cannot GET /"
- Ensure backend is running on port 7257
- Check that `Program.cs` maps SPA fallback

### Error: "401 Unauthorized"
- Verify JWT token is stored in localStorage
- Check token hasn't expired
- Regenerate certificate if needed

### CORS Errors with HTTPS
- Verify port 7257 in `launchSettings.json`
- Check CORS policy in `Program.cs`

### Database Connection Issues
- Verify connection string in `appsettings.json`
- Ensure SQL Server is running
- Run migrations: `dotnet ef database update`

## Documentation

- [HTTPS_SETUP.md](HTTPS_SETUP.md) - HTTPS configuration guide
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)

## Development Resources

### Creating a New Feature (Monolithic Way)

1. Create data model in `Core/Models/`
2. Add DbSet to `Persistence/VegaDbContext.cs`
3. Create migration: `dotnet ef migrations add FeatureName`
4. Apply migration: `dotnet ef database update`
5. Create repository in `Core/`
6. Create controller in `Controllers/`
7. Create Angular service in `ClientApp/src/app/services/`
8. Create Angular components in `ClientApp/src/app/`

### Best Practices for Monolithic Architecture

- Keep controllers focused and single-responsibility
- Use dependency injection for loose coupling
- Implement repository pattern for data access
- Use DTOs for API responses
- Implement proper error handling middleware
- Use async/await for scalability
- Implement proper logging
- Use migrations for database versioning
- Implement comprehensive testing

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome!

1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Support

For questions or issues, please open an issue in the GitHub repository.

---

Built with ASP.NET Core and Angular - Monolithic Architecture Pattern
