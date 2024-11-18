jak uruchomić poradnik dla opornych 

stwórz dwie bazy danych w projekcie WebaApi w pliku appsettings.json musisz podmienić connection stringi
- dodaj migracje Package menager console)


ustaw projekt  domyślny na Infrastructure ( góra konsoli PM )


![infrastruktura!](pm.jpg)

wpisz komendy:

Add-Migration InitialMigrationForApplicationDb -Context ApplicationDbContext -OutputDir Migrations/ApplicationDbMigration -StartupProject WebApi

Update-Database -Context ApplicationDbContext -StartupProject WebApi

Add-Migration InitialMigrationForUserDb -Context UserDbContext -OutputDir Migrations/UserDbMigration -StartupProject WebApi

Update-Database -Context UserDbContext -StartupProject WebApi


sprawdź czy utworzyło tabele 

wybierz profil https 
![https!!!](https.jpg)
uruchom projekt (powinien działać )

w przypadku braku komunikacji z reactem sprawdź konfiguracja portuw (po stronie react pliku \mmm-platform-main\src\api.ts linijka 4 po  stronie C# znajduje się to w pliku MarketingMixModeling\WebApi\Properties\launchSettings.json ) 

