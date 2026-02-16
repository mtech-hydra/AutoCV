I tried cross compiling; but it always produced PE32+ executable.
What does PE32+ format indicate in .NET DLLs?,It is the Portable Executable format used for .NET assemblies; all .NET DLLs are PE files even on Linux, the OS does not execute them directly.
Does .NET IL (Intermediate Language) depend on host OS?,No, IL is platform-independent; BadImageFormatException usually indicates a native dependency mismatch, not IL itself.
Why do Linux .NET apps throw BadImageFormatException when running Windows builds?,Because native dependencies (like Microsoft.Data.SqlClient SNI DLL) are Windows-only and not compatible with Linux.
What is the correct way to publish a Linux .NET app from Linux?,Use dotnet publish with -r linux-x64 on Linux, optionally --self-contained false for framework-dependent, ensure all native dependencies are present.
Can you cross-publish a framework-dependent Linux DLL from Windows?,No, framework-dependent builds may still produce Windows artifacts; self-contained cross-publish works but is larger.
Which native library does Microsoft.Data.SqlClient require on Linux?,libMicrosoft.Data.SqlClient.SNI.so
What happens if libMicrosoft.Data.SqlClient.SNI.so is missing on Linux?,BadImageFormatException occurs when trying to connect to SQL Server.
How can you fix missing native SNI libraries on Linux?,Install system dependencies (libkrb5-dev libssl-dev), clean bin/obj folders, publish from Linux.
Does the file command reliably indicate .NET compatibility?,No, it will report PE32+ for all .NET DLLs even if they run on Linux.
Where should wwwroot be for an ASP.NET Core publish?,Included in the project and copied to the publish folder, otherwise static files (CSS/JS) won’t load.
How do you run an ASP.NET Core app on Linux?,dotnet /path/to/AutoCV.Web.dll, optionally set ASPNETCORE_URLS to 0.0.0.0 to allow remote access.
How do you run an ASP.NET Core app on Windows Server Core?,Copy published Windows build, dotnet AutoCV.Web.dll, optionally set ASPNETCORE_URLS and run as a Windows service.
Why did Docker hide the native dependency problem?,Because volume mounts preserved old Windows artifacts, masking the missing Linux native SNI library.
What is the industry practice for cross-platform .NET builds?,Clean build directories, restore, publish on target OS, or use self-contained multi-stage Docker builds to ensure correct native libraries.
What is the role of DataProtection keys in ASP.NET Core?,Used for cookies, CSRF, and other cryptographic features; can warn if stored unencrypted at rest.
What is a DTO in .NET?,A Data Transfer Object; a simple class used to move data between layers, contains no business logic.
Is a DTO the same as a DAO?,No, a DAO (Data Access Object) encapsulates database operations; DTO is for transferring data.
What is a “contract” in .NET architecture?,An interface or set of interfaces defining expected behavior between components.
Why is the “happy path” called that?,It is the normal, error-free execution path through code, used for simple testing or prototypes.
Where should DTOs be placed in a .NET solution?,Typically in a separate project (e.g., AutoCV.Contracts) so multiple layers can reference them without circular dependencies.
Where should contracts be placed in a .NET solution?,Also in a shared project like AutoCV.Contracts, separate from implementation to allow multiple implementations.
How do you share projects in a single solution with Git?,Add all projects to the same .sln file, commit them into one repository; avoid nested repositories (submodules) unless intentional.
What is the effect of mounting a Windows volume for SQL Server Linux container?,It often breaks Linux permissions, causing SQL Server startup failures.
Why did BadImageFormatException appear when copying Windows build to Linux?,Windows-native binaries or leftover SNI DLLs were present, incompatible with Linux runtime.
What is the correct workflow to deploy ASP.NET Core on Server Core?,Publish a Windows build from Windows, copy to Server Core, ensure .NET runtime installed, set ASPNETCORE_URLS, run via dotnet or as a Windows service.
How to make SQL Server initial database creation happen in Docker?,Place a create-database.sql script in docker-entrypoint-initdb.d; SQL Server must have permissions to write database files.
How to clean Docker volumes for SQL Server?,docker compose down -v and optionally docker volume rm <volume>, but only if not in use.
Why does .NET Core require different builds per OS architecture?,Because some libraries have native dependencies (.dll vs .so) and IL alone is not sufficient for runtime.
What is the recommended way to build multi-platform .NET apps?,Use multi-stage Docker build or build on the target OS to ensure all native libraries match.

Available models:

codestral:latest                                                               0898a8b286d5    12 GB     6 months ago
deepseek-coder-v2:latest                                                       63fb193b3a9b    8.9 GB    6 months ago
llama3.3:latest                                                                a6eb4748fd29    42 GB     6 months ago
hf.co/sm54/FuseO1-DeepSeekR1-QwQ-SkyT1-Flash-32B-Preview-Q4_K_M-GGUF:latest    4b36bdac56b3    19 GB     6 months ago
llama3.1:8b                                                                    46e0c10c039e    4.9 GB    6 months ago
reader-lm:latest                                                               33da2b9e0afe    934 MB    6 months ago
command-r:latest                                                               7d96360d357f    18 GB     6 months ago
granite3.1-dense:latest                                                        34d3be74ec54    5.0 GB    6 months ago
llava:7b                                                                       8dd30f6b0cb1    4.7 GB    6 months ago
llava:13b                                                                      0d0eb4d7f485    8.0 GB    6 months ago
llava:34b                                                                      3d2d24f46674    20 GB     6 months ago
dolphin-mistral:7b                                                             5dc8c5a2be65    4.1 GB    6 months ago
gemma3:latest                                                                  a2af6cc3eb7f    3.3 GB    6 months ago
deepseek-r1:latest                                                             6995872bfe4c    5.2 GB    6 months ago
mario:latest                                                                   24a28eafbc72    2.0 GB    8 months ago
dolphin-mixtral:latest                                                         cfada4ba31c7    26 GB     14 months ago
gemma:latest                                                                   a72c7f4d0a15    5.0 GB    14 months ago
qwen2.5-coder:latest                                                           2b0496514337    4.7 GB    14 months ago
qwq:latest                                                                     46407beda5c0    19 GB     14 months ago
llama3.2:latest                                                                a80c4f17acd5    2.0 GB    14 months ago