# FitnessTracker

Aplicatie web full-stack pentru studentii care doresc sa-si monitorizeze progresul fitness - platforma de tracking al exercitiilor, nutritiei si progresului fizic, cu analitice avansate si panou admin.

## Stack tehnic

- **Backend**: ASP.NET Core 8 Web API + Razor Views/MVC, EF Core (SQL Server 2022), ASP.NET Core Identity (cookie-based)
- **Frontend**: Razor Views (5 Views Razor), HTML/CSS/JavaScript vanilla
- **Baza de date**: SQL Server 2022
- **Containerizare**: Docker + Docker Compose (Nginx pentru reverse proxy)
- **Architectura**: Clean Architecture cu Dependency Injection

## Arhitectura backend

```
FitnessTrackerPAW            <- Controllers (API + MVC), DTOs, Views, Middleware, Hubs
FitnessTrackerPAW.Tests      <- xUnit Tests
Domain                       <- Entities (User, Exercise, WorkoutSession, Supplement, WorkoutExercise, Role)
Application                  <- Services (ISupplementService), DTOs, Interfaces
Infrastructure               <- DbContext, Repositories (ISupplementRepository), Migrations
```

## Entitati principale

| Entitate          | Relatii                                      |
|---|---|
| User (Identity)   | 1-N WorkoutSession, N-1 Supplement (1-N)    |
| WorkoutSession    | N-1 User, 1-N WorkoutExercise                |
| WorkoutExercise   | N-1 WorkoutSession, N-1 Exercise (junction)  |
| Exercise          | 1-N WorkoutExercise                          |
| Supplement        | N-1 User, N-1 Role                           |
| Role (Identity)   | Admin, User                                  |

## Controllere API + MVC

| Controller        | Ruta              | Metoda        | Protejat         |
|---|---|---|---|
| AuthController    | `/api/auth`       | Register/Login| —                |
| AccountController | `/account`        | Razor MVC     | partial          |
| HomeController    | `/`               | Razor MVC     | —                |
| ExerciseController| `/api/exercise`   | REST API      | Admin only       |
| SupplementController | `/api/supplement` | REST API  | Authorized (User+) |
| WorkoutSessionController | `/api/workoutsession` | REST API | Authorized |
| NutritionController | `/nutrition`    | Razor MVC     | Authorized       |
| UserController    | `/api/user`       | REST API      | Admin only       |
| AdminApiController| `/api/admin`      | REST API      | Admin only       |

## Pagini frontend (Razor Views + MVC)

| Pagina            | Ruta                    | Protejata | Descriere                          |
|---|---|---|---|
| Home / Dashboard  | `/home` + `/`           | —         | Pagina principala cu informatii    |
| Login             | `/account/login`        | —         | Formular de login (Identity)       |
| Register          | `/account/register`     | —         | Formular de inregistrare           |
| LogWorkout        | `/home/logworkout`      | Auth      | Formular de log sesiune antrenament|
| Analytics         | `/home/analytics`       | Auth      | Grafice si statistici fitness      |
| Nutrition         | `/nutrition`            | Auth      | Tracking suplimente si nutritie    |

## Pasi de rulare

### Prerequisite

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-2022) (local sau Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional)

### Variabile de mediu

Creeaza/editeaza `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=fitnesstracker;Trusted_Connection=true;Encrypt=False"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Optiunea 1 — Docker Compose (recomandat pentru mediu controlat)

```sh
# Creeaza network-ul daca nu exista
docker network create studlab_net

# Construieste si ruleaza containerele
docker compose -f docker-compose.yml up -d --build

# Acces
# API/Frontend: http://localhost:80 (via Nginx reverse proxy)
# Direct backend: http://localhost:8080
```

### Optiunea 2 — Rulare manuala

**Backend (Visual Studio sau CLI):**

```sh
cd FitnessTrackerPAW
dotnet restore
dotnet ef database update
dotnet run

# Disponibil la: http://localhost:5000 (Kestrel default)
```

**Teste (xUnit):**

```sh
cd FitnessTrackerPAW.Tests
dotnet test
```

Baza de date si seed-ul se aplica automat la prima rulare. Migratiile sunt aplicate in `Program.cs` cu retry logic pentru Docker.

## Autentificare si Autorizare

- **Sistem**: ASP.NET Core Identity + cookie-based sessions
- **Roluri**: Admin, User
- **Protectie**: `[Authorize]`, `[Authorize(Roles="Admin")]`
- **Middleware**: GlobalExceptionMiddleware pentru exceptii HTTP (400, 403, 404, 500)

## Credentiale implicite (seed)

| Email          | Parola         | Rol   | Observatii                   |
|---|---|---|---|
| admin@test.ro  | ParolaAdmin123 | Admin | Creat automat la prima rulare|

## Functionalitati implementate

### Core Features
- Autentificare cu ASP.NET Core Identity (cookie-based, nu JWT)
- CRUD complet Exercise (Admin pode crea, edit, delete)
- CRUD complet Supplement cu tracking nutritie (caloriile, proteine, masa gainer)
- Log Workout Sessions cu PumpLevel rating (1-10)
- Dashboard Analytics cu statistici agregare

### Backend Architecture
- **Services Layer**: ISupplementService (dependency injection)
- **Repository Pattern**: ISupplementRepository
- **Data Access**: EF Core DbContext (ApplicationDbContext)
- **Validation**: DataAnnotations + ModelState validation
- **Error Handling**: GlobalExceptionMiddleware cu logging
- **Logging**: Microsoft.Extensions.Logging
- **Protected Routes**: `[Authorize]` si `[Authorize(Roles="Admin")]`

### Frontend (Razor Views)
- Formular Login/Register cu validare client-side si server-side
- Layout principal (_Layout.cshtml) cu navbar responsive
- _LoginPartial cu logout
- _ValidationScriptsPartial pentru client-side validation (jQuery Unobtrusive)
- Formularele folosesc `asp-for` tag helpers (Razor syntax)

### Testing
- xUnit test suite (FitnessTrackerPAW.Tests)
- SupplementServiceTests pentru validare logica serviciilor

### DevOps
- Dockerfile cu multi-stage build
- Docker Compose cu SQL Server container
- Retry logic pentru conexiuni DB in mediu containerizat
- Nginx reverse proxy configurat in docker-compose

## Structura folderelor

```
FitnessTrackerPAW/
├── Controllers/              # AuthController, AccountController, API controllers
├── Domain/                   # Entities: User, Exercise, WorkoutSession, Supplement
├── Application/
│   ├── Services/            # SupplementService
│   ├── Interfaces/          # ISupplementService, ISupplementRepository
│   └── DTOs/                # RegisterDto, LoginDto, SupplementDto, WorkoutSessionDto, ExerciseDto
├── Infrastructure/
│   ├── ApplicationDbContext.cs
│   └── Repositories/        # SupplementRepository
├── Middlewares/             # GlobalExceptionMiddleware
├── Views/                   # Razor Views (Account, Home, Nutrition)
├── Program.cs               # Configurare (DbContext, Identity, DI, migrations)
├── Dockerfile
├── docker-compose.yml
└── appsettings.json
```

## Repository

- **URL**: https://github.com/davidutzuu/FitnessTrackerDAW
- **Branch**: main
