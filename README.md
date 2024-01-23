# Webová Aplikace pro Správu Povinných Prací na SPŠ a VOŠ Písek

## Anotace

Tato webová aplikace vznikla jako maturitní projekt během mého posledního roku studia na SPŠ a VOŠ Písek. Práce se zabývá návrhem a implementací webové aplikace, zaměřené na správu povinných prací odevzdaných studenty. Aplikace poskytuje všem uživatelům přehled o přidělených úkolech, tématech a hodnoceních. Vyučujícím umožňuje vytvářet, upravovat, přidělovat a odstraňovat zadání. Aplikace rovněž umožňuje vyučujícím přidělovat úkoly v rámci ročníků, tříd a skupin ve třídách. Studentům naopak nabízí možnost procházet aktuální i minulé práce, které již do aplikace nahráli.

## Klíčová Slova

C#, ASP.NET Core, Razor, Entity Framework Core, ASP.NET Core Identity, HTML, JavaScript, CSS, Bootstrap, MySQL, WebApp

## Funkce Aplikace

- **Přehledné zadávání úkolů:** Vyučující může snadno vytvářet, upravovat, a přidělovat úkoly studentům.
- **Strukturované hodnocení:** Aplikace poskytuje uživatelům přehledné hodnocení a zpětnou vazbu k odevzdaným pracím.
- **Organizace podle ročníků, tříd a skupin:** Vyučující může přidělovat úkoly nejen do konkrétních tříd a ročníků, ale i skupin v rámci tříd.
- **Prohlížení aktuálních a minulých prací:** Studenti mohou sledovat své aktuální úkoly a procházet své dřívější odevzdané práce.

## Technické Detaily

- **Programovací Jazyky:** C#
- **Framework:** ASP.NET Core
- **Šablony:** Využití Razor pro tvorbu dynamických webových stránek.
- **Databáze:** MySQL s využitím Entity Framework Core pro snadnou správu dat.
- **Uživatelské Rozhraní:** HTML, JavaScript, CSS, Bootstrap pro moderní a responzivní design.
- **Správa Identit:** Využití ASP.NET Core Identity pro autentizaci a autorizaci uživatelů.
## Instalace A Spuštění

Pro úspěšnou instalaci a spuštění této webové aplikace postupujte podle následujících kroků:

### Požadavky

Před zahájením instalace se ujistěte, že máte nainstalovaný následující software:

- [.NET Core SDK](https://dotnet.microsoft.com/download) (verze 3.1 nebo novější)
- [MySQL Server](https://dev.mysql.com/downloads/)

### Klonování repozitáře

1. Naklonujte tento repozitář do vašeho lokálního prostoru:
    ```bash
    git clone https://github.com/Kropi11/PPSPS.git
    ```
   
2. Přejděte do složky projektu:
    ```bash
    cd PPSPS/PPSPS
    ```

### Nastavení Databáze

1. Vytvořte prázdnou databázi MySQL pro aplikaci.
2. Aktualizujte připojovací řetězec v souboru `appsettings.json` s příslušnými informacemi o databázi.

### Spuštění Aplikace

1. Otevřete příkazový řádek v hlavní složce projektu.
2. Spusťte příkaz pro sestavení a spuštění aplikace:
    ```bash
    dotnet run
    ```

Aplikace by nyní měla být dostupná na [http://localhost:5000](http://localhost:5000).

### Další Kroky

Pro další pokyny ohledně konfigurace a používání si přečtěte [dokumentaci](./Documentation.pdf).


## Licence

Tato aplikace je licencována jako školní dílo dle §60 Autorského zákona České republiky. Podrobnosti o licenci a povoleních naleznete v [dokumentaci](./Documentation.pdf).

## Dokumentace

Pro více informací o implementaci a používání aplikace, můžete nalézt v [dokumentaci](./Documentation.pdf).

--- 
