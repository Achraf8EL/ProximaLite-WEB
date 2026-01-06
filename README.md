
# ProximaLite Web

## Description
ProximaLite est une application web simple pour gérer des processus, écrite en C#. Elle utilise ASP.NET CORE et a une structure en couches : domaine, infrastructure et web.

## Installation
1. Assurez-vous d'avoir .NET installé sur votre machine.
2. Clonez ce dépôt.
3. Ouvrez le terminal dans le dossier du projet.

## Lancer l'application
Pour démarrer l'application web :
```
ASPNETCORE_URLS=http://127.0.0.1:5000 dotnet run --project src/ProximaLite.Web
```

L'application sera disponible sur http://127.0.0.1:5000.

## Tests
Pour lancer les tests unitaires :
```
dotnet test ./tests/ProximaLite.Tests/
```

Pour les tests end-to-end :
```
E2E_BASE_URL=http://127.0.0.1:5000 dotnet test tests/ProximaLite.SeleniumE2E
```

## Structure du projet
- `src/ProximaLite.Domain/` : Logique métier et entités.
- `src/ProximaLite.Infrastructure/` : Accès aux données.
- `src/ProximaLite.Web/` : Interface web avec ASP.NET Core.
- `tests/` : Tests unitaires et end-to-end.

## Technologies
- .NET 7 & C#
- ASP.NET Core
- Entity Framework Core
- Selenium pour les tests E2E