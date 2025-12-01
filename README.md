# AdventureWorks Sample - Applicazione Didattica C# Windows Forms

## Descrizione

Applicazione Windows Forms .NET 8.0 per la gestione del database AdventureWorks.
Questo progetto è pensato a scopo didattico per un corso di programmazione C#.

**Caratteristiche principali:**
- Utilizzo di **Dapper** come micro-ORM (niente Entity Framework)
- Architettura pulita con separazione tra UI e accesso ai dati
- Codice ampiamente commentato per facilitare l'apprendimento
- Tre form CRUD per le tabelle principali

## Tabelle Gestite

1. **Production.Product** - Gestione anagrafica prodotti
2. **Sales.SalesOrderHeader** - Gestione ordini di vendita
3. **Sales.SalesOrderDetail** - Gestione righe ordine

## Requisiti

- .NET 8.0 SDK
- SQL Server con database AdventureWorks installato
- Visual Studio 2022 o VS Code (opzionale)

## Come Eseguire

### 1. Clona il repository
```bash
git clone https://github.com/spaghettidba/AdventureWorksSample.git
cd AdventureWorksSample/AdventureWorksApp
```

### 2. Configura la connessione al database

Modifica il file `appsettings.json` con la tua stringa di connessione:

```json
{
  "ConnectionStrings": {
    "AdventureWorks": "Server=YOUR_SERVER;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

**Oppure** inserisci la stringa di connessione direttamente nell'applicazione al primo avvio.

### 3. Compila ed esegui
```bash
dotnet restore
dotnet build
dotnet run
```

## Struttura del Progetto

```
AdventureWorksApp/
├── appsettings.json          # Configurazione
├── Program.cs                # Punto di ingresso
├── Models/                   # Classi modello (POCO)
│   ├── Product.cs
│   ├── SalesOrderHeader.cs
│   └── SalesOrderDetail.cs
├── DataAccess/              # Layer accesso dati
│   ├── DbConnectionFactory.cs
│   ├── ProductRepository.cs
│   ├── SalesOrderHeaderRepository.cs
│   └── SalesOrderDetailRepository.cs
└── Forms/                   # Form Windows Forms
    ├── MainForm.cs/.Designer.cs
    ├── ProductForm.cs/.Designer.cs
    ├── SalesOrderHeaderForm.cs/.Designer.cs
    └── SalesOrderDetailForm.cs/.Designer.cs
```

## Pattern Utilizzati

### Repository Pattern
Ogni tabella ha un proprio repository che incapsula le operazioni CRUD:
- `GetAllAsync()` - Legge tutti i record
- `GetByIdAsync(id)` - Legge un singolo record
- `InsertAsync(entity)` - Inserisce un nuovo record
- `UpdateAsync(entity)` - Aggiorna un record esistente
- `DeleteAsync(id)` - Elimina un record

### Dapper
Dapper è un micro-ORM che estende `IDbConnection` con metodi di mapping:

```csharp
// Esempio di query con Dapper
using var connection = DbConnectionFactory.CreateConnection();
var products = await connection.QueryAsync<Product>(
    "SELECT * FROM Production.Product WHERE Name LIKE @Name",
    new { Name = "%bike%" }
);
```

## Note Didattiche

1. **Separazione delle responsabilità**: UI e logica dati sono separate
2. **Async/Await**: Tutte le operazioni DB sono asincrone per non bloccare l'UI
3. **Query parametrizzate**: Prevengono SQL injection
4. **Commenti**: Il codice è ampiamente commentato in italiano

## Esempi di Stringhe di Connessione

**Autenticazione Windows (consigliata):**
```
Server=localhost;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True
```

**SQL Server Express:**
```
Server=localhost\SQLEXPRESS;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True
```

**Autenticazione SQL:**
```
Server=localhost;Database=AdventureWorks2019;User Id=sa;Password=YourPassword;TrustServerCertificate=True
```

## Licenza

Questo progetto è rilasciato a scopo didattico.