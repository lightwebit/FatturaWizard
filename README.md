# FatturaWizard

FatturaWizard è un'applicazione dimostrativa **WPF (.NET)** pensata come vetrina per la generazione e la gestione del formato **Fattura Elettronica (FPR12)** conforme alle specifiche dell'Agenzia delle Entrate. Questo repository contiene un'interfaccia d'esempio che permette di creare, caricare, modificare e esportare fatture in formato XML.

---

## Perché questo progetto
FatturaWizard è pensato come progetto esemplificativo per sviluppatori e consulenti per:
- integrazione del formato *Fattura Elettronica* in applicazioni .NET;
- serializzazione / deserializzazione XML con `XmlSerializer`;
- binding e DataGrid in WPF per la compilazione delle linee di fattura;
- best practice di esportazione XML con namespace e formattazione leggibile.

---

## Caratteristiche principali (demo)
- Interfaccia WPF per inserimento dei dati di **Cedente/Prestatore** e **Cessionario/Committente**.
- Gestione dinamica delle **linee di dettaglio** (Aggiungi/Rimuovi/Ricalcolo numeri linea).
- Validazioni base (es. ID trasmittente, numero fattura, presenza di almeno una linea).
- Esportazione XML con namespace ufficiale `http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2`.
- Import di file XML esistenti per popolamento dell'interfaccia.

---

## Requisiti
- **.NET 9 SDK** (target `net9.0`)
- Windows con supporto WPF
- (Opzionale) Visual Studio 2022 / 2023 / 2024 con workload `.NET Desktop Development`

> Nota: il progetto demo qui incluso è pensato per essere un punto di partenza: per l'invio allo SDI (Sistema di Interscambio) è necessario rispettare tutte le regole di validazione, firma digitale e formattazione richieste dall'Agenzia delle Entrate.

---

## Quickstart (CLI)
1. Scarica ed estrai il repository.
2. Apri un terminale nella cartella del progetto.
3. Compila ed esegui:
```bash
dotnet build -f net9.0
dotnet run --project ./FatturaWizard/FatturaWizard.csproj -f net9.0
```

Se apri il progetto con Visual Studio, imposta il progetto WPF come startup e avvia normalmente (F5).

---

## File inclusi (esempio)
- `FatturaWizard.sln` — soluzione Visual Studio (esempio).
- `FatturaWizard/FatturaWizard.csproj` — progetto WPF (target `net9.0`).
- `FatturaWizard/MainWindow.xaml` + `MainWindow.xaml.cs` — interfaccia e logica principale.
- `Models/FatturaElettronica.cs` — modello C# generato dalla struttura XML FPR12 (classi POCO).

---

## Limitazioni e avvisi
- Questo progetto è **un esempio didattico**: non inviare file XML al Sistema di Interscambio (SDI) senza le opportune firme digitali e controlli di conformità.
- Alcuni campi obbligatori e casi particolari (es. ritenute, cassa previdenziale, bollo virtuale avanzato) non sono completamente modellati nella demo: estendi il modello se ne hai bisogno.

---

## Contribuire
Se vuoi migliorare il progetto:
- Apri una issue per proporre funzionalità o segnalare bug.
- Apri una pull request con un branch descrittivo (es. `feat/import-validation`).

---

## Licenza
MIT License — vedi `LICENSE` per i dettagli.

