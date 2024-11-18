# Poradnik dla opornych 🚀
Ten poradnik przeprowadzi Cię krok po kroku przez proces konfiguracji i uruchamiania projektu.

---

## 📋 Wymagania wstępne

- Zainstalowany **Visual Studio** z obsługą .NET Core.
- Zainstalowany **SQL Server** (lub kompatybilna baza danych).
- Zainstalowane **Node.js** i **npm** (jeśli pracujesz z Reactem).
- Zainstalowany **Git**.

---
## 📂 Kroki konfiguracji

### 1️⃣ Stwórz dwie bazy danych

W pliku `appsettings.json` w projekcie **WebApi** skonfiguruj connection stringi do dwóch baz danych.

Przykład sekcji `ConnectionStrings` w pliku `appsettings.json`:

```json
"ConnectionStrings": {
  "ApplicationDbConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketingMixModelingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
  "UserDbConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
}
```

---

### 2️⃣ Dodaj migracje w Package Manager Console

a) Ustaw projekt domyślny
W górnej części Package Manager Console ustaw projekt domyślny na Infrastructure:

![infrastruktura!](pm.jpg)

b) wpisz komendy:
```bash
Add-Migration InitialMigrationForApplicationDb -Context ApplicationDbContext -OutputDir Migrations/ApplicationDbMigration -StartupProject WebApi

Update-Database -Context ApplicationDbContext -StartupProject WebApi

Add-Migration InitialMigrationForUserDb -Context UserDbContext -OutputDir Migrations/UserDbMigration -StartupProject WebApi

Update-Database -Context UserDbContext -StartupProject WebApi
```

c) Zweryfikuj, czy tabele zostały poprawnie utworzone:
Otwórz SQL Server Management Studio (SSMS) lub inne narzędzie do zarządzania SQL.
Sprawdź, czy w bazach danych MarketingMixModelingDb i UserDb utworzyły się tabele na podstawie migracji.

---

### 3️⃣ Wybierz profil HTTPS

![https!!!](https.jpg)

---

### 4️⃣  Kliknij przycisk Run w Visual Studio lub użyj skrótu klawiszowego F5. Projekt powinien uruchomić się na profilu HTTPS.

---

# 🛠️ Rozwiązywanie problemów

## Brak komunikacji z Reactem?
Jeśli projekt backendowy (WebApi) nie komunikuje się poprawnie z frontendem React:

### 1️⃣ Sprawdź konfigurację portów:

Po stronie React (frontend): W pliku `mmm-platform-main/src/api.ts`, upewnij się, że port backendu jest poprawny. Zmień linijkę 2 na odpowiedni port, np.:

```javascript
const api = axios.create({
  baseURL: 'https://localhost:7104', 
  headers: {
    'Content-Type': 'application/json',
  },
});
```

Po stronie WebApi (backend): W pliku `MarketingMixModeling/WebApi/Properties/launchSettings.json`, upewnij się, że profil HTTPS jest poprawnie skonfigurowany, np.:

```json
   "https": {
     "commandName": "Project",
     "launchBrowser": true,
     "launchUrl": "swagger",
     "environmentVariables": {
       "ASPNETCORE_ENVIRONMENT": "Development"
     },
     "dotnetRunMessages": true,
     "applicationUrl": "https://localhost:7104;http://localhost:5112"
```

### 2️⃣ Zrestartuj zarówno backend (WebApi), jak i frontend (React).

---

# 🌟 Gotowe!
Po wykonaniu wszystkich kroków Twój projekt powinien działać poprawnie. Jeśli masz pytania lub napotkasz problemy, skontaktuj się z Autorem projektu.

---
