@echo off

echo Applying database migrations...
dotnet ef database update --project src/CompanyStructure.Infrastructure --startup-project src/CompanyStructure.Api

echo Running CompanyStructure.Api project...
dotnet run --project src/CompanyStructure.Api
