# SkillMentor: C# / ASP.NET Core backend

This is a side-by-side port of `../backend` for learning. The original Spring Boot backend and the React frontend have not been edited. All 43 Java application-class **base names** appear as `.cs` files in corresponding directories (including the original `respositories` spelling). C# cannot compile `.java` files or use `pom.xml`: `SkillMentor.csproj` and `Program.cs` are the .NET equivalents. `SkillMentorDbContext.cs` is the EF Core database mapping. The Java Maven wrapper, `package-lock.json`, and property files are not copied because they are Java/Node tooling or contain credentials; the equivalent runtime configuration is `appsettings.json` and environment variables. `Dockerfile2` and `docker-compose.yaml` have .NET counterparts, and the existing Java validation tests are represented in `../backend-dotnet.Tests/ValidationUtilsTest.cs`.

## Run on Windows 11 in Visual Studio

1. Install Visual Studio with the **ASP.NET and web development** workload and the .NET 8 SDK. Open `SkillMentor.csproj` (or open the repository folder). Restore NuGet packages.
2. Set `ConnectionStrings__DefaultConnection` to a PostgreSQL connection string, e.g. `Host=localhost;Port=5432;Database=skill_mentor_v2;Username=postgres;Password=<your password>`.
3. Set `CLERK_JWKS_URL` to the JWKS endpoint for your Clerk instance. Optionally set `CORS_ALLOWED_ORIGINS` (comma-separated), `PORT` (default 8081), and `AUTH_VALIDATOR_TYPE=stem-link` plus `JWT_SECRET` to use the local HMAC validator instead of Clerk.
4. Create the schema before using a new database: `dotnet ef migrations add Initial --project backend-dotnet`, then `dotnet ef database update --project backend-dotnet`. **Do not apply a fresh EF migration to the existing Spring-managed database without comparing the generated SQL to the live schema first.** Entity names, foreign keys, and column names are mapped to the current Java entities, but timestamps and schema migration history need verification.
5. Run `dotnet run --project backend-dotnet/SkillMentor.csproj` and visit `/swagger`. Test with `dotnet test backend-dotnet.Tests/SkillMentor.Tests.csproj`.

Docker: run `docker compose -f backend-dotnet/docker-compose.yaml up --build` with `CLERK_JWKS_URL` defined in your environment. The API is then at `http://localhost:8085`.

## Request path (one example)

`POST /api/v1/subjects` → `AuthenticationFilter` checks the Clerk bearer token → `[Authorize(Roles = "ADMIN")]` checks the role → `SubjectController.CreateSubject` validates/deserializes `SubjectDTO` → `SubjectServiceImpl.AddNewSubject` finds the mentor by the **string `MentorId`** (as Java does) → `SubjectRepository.Save` → `SkillMentorDbContext.SaveChangesAsync` issues SQL against PostgreSQL → controller returns the serialized `Subject`.

## Full Spring Boot ↔ C# / ASP.NET Core comparison

| Existing Java / Spring Boot | This C# / ASP.NET Core port | Example here |
| --- | --- | --- |
| Java is a language; Spring Boot is a Java framework | C# is a language; .NET is the runtime/platform; ASP.NET Core is its web framework | `Program.cs`, `SkillmentorApplication.cs` |
| `.java` source | `.cs` source | `entities/Mentor.java` → `entities/Mentor.cs` |
| `package com.stemlink.skillmentor...;` | `namespace SkillMentor...;` | `services/impl/SessionServiceImpl.cs` |
| `import ...;` | `using ...;` | `using SkillMentor.entities;` |
| `pom.xml`, Maven dependencies | `.csproj`, NuGet `PackageReference` | `SkillMentor.csproj` |
| `mvnw spring-boot:run` | `dotnet run --project backend-dotnet` | command above |
| `@SpringBootApplication`, `main` | `WebApplication.CreateBuilder`, `app.Run()` | `Program.cs` |
| `@Configuration`, `@Bean` | configuration methods and `builder.Services.Add...` | `configs/ValidatorConfiguration.cs` |
| `@Service` with constructor-injected `final` fields | registered interface and implementation with constructor injection | `services/impl/SubjectServiceImpl.cs` |
| `@RestController`, `@RequestMapping` | `[ApiController]`, `[Route]` on `ControllerBase` | `controllers/MentorController.cs` |
| `@GetMapping`, `@PostMapping`, `@PutMapping`, `@DeleteMapping` | `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` | `controllers/SessionController.cs` |
| `@PathVariable`, `@RequestParam`, `@RequestBody` | route parameter / `[FromQuery]` / `[FromBody]` | `GetAllMentors`, `CreateMentor` |
| `ResponseEntity<T>` | `ActionResult<T>` / `Ok`, `StatusCode(201, ...)`, `NoContent` | `controllers/AbstractController.cs` |
| `@Valid` / Jakarta Bean Validation | DataAnnotations plus `[ApiController]` automatic 400 | `dto/SubjectDTO.cs` |
| Lombok `@Data`, getters/setters, `@RequiredArgsConstructor` | ordinary properties and a primary constructor | `dto/MentorDTO.cs`, `controllers/MentorController.cs` |
| `Long`, `Integer`, `String`, `Boolean`, `Date` | `long`, `int`, `string`, `bool`, `DateTime` (nullable versions with `?`) | `entities/Session.cs` |
| `List<T>`, `Optional<T>` | `List<T>`, nullable `T?` with `?? throw` | `services/impl/SubjectServiceImpl.cs` |
| `JpaRepository`, JPA queries | EF Core `DbContext`, `DbSet<T>`, LINQ query | `respositories/MentorRepository.cs` |
| `@Entity`, `@Table`, `@Column`, relationships | EF Core Fluent API in `OnModelCreating` | `respositories/SkillMentorDbContext.cs` |
| Hibernate `@CreationTimestamp`, `@UpdateTimestamp` | timestamps in `SaveChangesAsync` | `respositories/SkillMentorDbContext.cs` |
| `modelMapper.map(...)` | explicit property mapping | `controllers/MentorController.cs`, services |
| `@JsonIgnore` | `[JsonIgnore]` | `entities/Session.cs` |
| `@ExceptionHandler` | exception-handling middleware | `security/SkillMentorAuthenticationEntryPoint.cs` |
| `@PreAuthorize("hasRole('ADMIN')")` | `[Authorize(Roles = "ADMIN")]` | `controllers/SubjectController.cs` |
| `OncePerRequestFilter` and `TokenValidator` | `AuthenticationHandler` and `TokenValidator` | `security/AuthenticationFilter.cs` |
| Auth0 JWKS / JWT; HMAC alternative | `JwtSecurityTokenHandler` with JWKS / symmetric key | `security/ClerkValidator.cs`, `SkillMentorJwtValidator.cs` |
| Springdoc Swagger/OpenAPI | Swashbuckle `/swagger` | `configs/OpenApiConfig.cs` |
| `@Cacheable` and `@CacheEvict`, optional Redis | `IDistributedCache`, ten-minute mentor cache and versioned invalidation | `configs/RedisConfig.cs`, `services/impl/MentorServiceImpl.cs` |
| `application*.properties`, `${ENV}` | `appsettings.json`, `IConfiguration`, `ConnectionStrings__...` | `SkillmentorApplication.cs` |
| JUnit test | xUnit test | `../backend-dotnet.Tests/ValidationUtilsTest.cs` |
| Maven Docker build | `dotnet publish` Docker build | `Dockerfile` |

### OOP concepts in this actual code

**Class and object.** In `entities/Mentor.cs`, `public class Mentor` defines a mentor; `new Mentor` in `controllers/MentorController.cs` creates an object with values from the request. This represents each database record as one instance.

**Encapsulation.** `Mentor` exposes properties such as `Email`, while persistence and query logic live in `MentorRepository`. The controller calls `MentorService` instead of directly changing EF Core's `DbContext`. This keeps the HTTP, business, and SQL responsibilities in separate classes. Properties are public for serialization and EF Core, so this is structural encapsulation rather than strict private-field protection.

**Abstraction.** `services/SubjectService.cs` says *what* the operations are (`Task<Subject> AddNewSubject(long mentorId, Subject subject)`); `services/impl/SubjectServiceImpl.cs` defines *how*, including lookup and save. `security/TokenValidator.cs` defines the same contract for two token formats. Callers need the contract rather than the internal implementation.

**Inheritance.** `public class MentorController(...) : AbstractController` in `controllers/MentorController.cs` inherits `SendOkResponse` and `SendCreatedResponse` from `controllers/AbstractController.cs`. `SkillMentorException : Exception` inherits normal exception behavior while adding `Status`.

**Polymorphism.** `SubjectServiceImpl : SubjectService` is registered with `AddScoped<SubjectService, SubjectServiceImpl>()` in `SkillmentorApplication.cs`. ASP.NET Core injects the concrete object through the `SubjectService` type. `ClerkValidator : TokenValidator` and `SkillMentorJwtValidator : TokenValidator` are interchangeable at the `AuthenticationFilter` constructor, chosen by `ValidatorConfiguration`.

**Composition.** `SessionServiceImpl` receives `SessionRepository`, `StudentRepository`, `MentorRepository`, and `SubjectRepository` in its constructor. It combines those collaborators to create a session. `Session` holds references to a `Student`, `Mentor`, and `Subject`, reflecting database relationships.

### Known source behavior and conversion limits

- Original `SessionDTO.studentId` is required even on `/sessions/enroll`, but the existing React enrollment request omits it and the Java enrollment method does not use it. This port accepts an omitted `studentId` for that endpoint so the existing frontend request can reach the enrollment logic. Administrative creation still looks up `StudentId`; a missing one returns 404.
- Java admin mentor creation accepts `MentorDTO.mentorId` as optional, but the database entity requires a non-null `mentor_id` and the current React admin request does not send it. The port preserves the database constraint. An admin must supply a valid `mentorId` to create a mentor; this is a pre-existing interface mismatch.
- `SubjectServiceImpl` and `SessionServiceImpl` look up the string `Mentor.MentorId` using the numeric `mentorId` from their DTOs, exactly as the source does. This is different from looking up `Mentor.Id`; check which identifier the frontend sends before changing it.
- The React admin booking routes under `/api/v1/admin/sessions` have no corresponding Java controller in this repository. They have not been invented in the C# port.
- Java `@Cacheable` and `@CacheEvict` decorate mentor reads and writes, with caching disabled in the source configuration. The port caches mentor reads for ten minutes when `CACHE_ENABLED=true` and changes a version key on writes to invalidate all previous mentor entries. Old keys expire after ten minutes.
- `ClerkValidator` checks the RS256 signature and token lifetime using the configured JWKS, as the Java validator does. For production, configure strict issuer and audience checking after confirming the Clerk token's intended claims.
- The checked-in Java dev/prod properties include hard-coded secrets or credentials. They are intentionally omitted from the C# settings file. Rotate any live credentials exposed in repository history.
- Database compatibility is defined by code mappings but has not been tested against the actual database. The local execution environment lacks the .NET SDK, PostgreSQL, and Clerk credentials; GitHub CI compiles and runs the translated availability tests on the proposed branch.
