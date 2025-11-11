# Programma van Eisen - ROC-Events Quiz Applicatie

## 1. Inleiding

### 1.1 Doel van het document
Dit document beschrijft de eisen en specificaties voor de ROC-Events Quiz Applicatie. De applicatie is ontwikkeld voor educatieve doeleinden en stelt gebruikers in staat om interactieve quizzen te maken, beheren en presenteren.

### 1.2 Doelgroep
- Docenten en trainers die quizzen willen organiseren
- Evenementorganisatoren voor interactieve presentaties
- Onderwijsinstellingen (ROC-Events)

### 1.3 Scope
De applicatie biedt een complete quiz-oplossing met:
- Quiz- en vraagbeheer
- Live presentatiemodus
- Operator bedieningspaneel
- Timer functionaliteit
- Visuele feedback systeem

---

## 2. Functionele Eisen

### 2.1 Quiz Beheer

#### FE-001: Quiz Overzicht
- **Prioriteit**: Hoog
- **Beschrijving**: De applicatie moet een overzicht tonen van alle beschikbare quizzen
- **Acceptatiecriteria**:
  - Quizzen worden getoond in een dropdown lijst
  - Quiznaam is zichtbaar voor selectie
  - Quiz kan geselecteerd worden voor weergave of gebruik

#### FE-002: Quiz Selectie
- **Prioriteit**: Hoog
- **Beschrijving**: Gebruiker moet een quiz kunnen selecteren
- **Acceptatiecriteria**:
  - Bij selectie worden quiz details getoond
  - Naam, beschrijving, aantal vragen en tijd per vraag zijn zichtbaar
  - Preview van vragen wordt getoond

#### FE-003: Quiz Informatie Weergave
- **Prioriteit**: Hoog
- **Beschrijving**: Geselecteerde quiz moet gedetailleerde informatie tonen
- **Acceptatiecriteria**:
  - Quiz naam wordt prominent weergegeven
  - Beschrijving is leesbaar
  - Aantal vragen wordt getoond
  - Tijd per vraag wordt weergegeven in seconden
  - Preview lijst met alle vragen inclusief correcte antwoorden

### 2.2 Vraag Beheer

#### FE-004: Quiz Aanmaken
- **Prioriteit**: Hoog
- **Beschrijving**: Gebruiker moet nieuwe quizzen kunnen aanmaken
- **Acceptatiecriteria**:
  - "Nieuwe Quiz" functionaliteit beschikbaar
  - Verplichte velden: naam
  - Optionele velden: beschrijving, tijd per vraag (standaard 30 seconden)
  - Quiz wordt opgeslagen in quizdata.json

#### FE-005: Quiz Bewerken
- **Prioriteit**: Hoog
- **Beschrijving**: Bestaande quizzen moeten bewerkt kunnen worden
- **Acceptatiecriteria**:
  - Quiz naam kan worden gewijzigd
  - Beschrijving kan worden aangepast
  - Tijd per vraag kan worden ingesteld
  - Wijzigingen worden opgeslagen

#### FE-006: Quiz Verwijderen
- **Prioriteit**: Gemiddeld
- **Beschrijving**: Quizzen moeten verwijderd kunnen worden
- **Acceptatiecriteria**:
  - Bevestigingsdialoog voor verwijderen
  - Quiz en alle bijbehorende vragen worden verwijderd
  - Feedback na succesvolle verwijdering

#### FE-007: Vraag Aanmaken
- **Prioriteit**: Hoog
- **Beschrijving**: Nieuwe vragen kunnen worden toegevoegd aan een quiz
- **Acceptatiecriteria**:
  - Vraagtekst invoerveld (verplicht)
  - Vier antwoordopties (A, B, C, D) - alle verplicht
  - Selectie van het correcte antwoord (verplicht)
  - Optionele afbeelding kan worden toegevoegd
  - Vraag wordt toegevoegd aan de geselecteerde quiz

#### FE-008: Vraag Bewerken
- **Prioriteit**: Hoog
- **Beschrijving**: Bestaande vragen moeten bewerkt kunnen worden
- **Acceptatiecriteria**:
  - Alle vraaggegevens kunnen worden gewijzigd
  - Navigatie tussen vragen (vorige/volgende knoppen)
  - Wijzigingen worden opgeslagen

#### FE-009: Vraag Verwijderen
- **Prioriteit**: Gemiddeld
- **Beschrijving**: Vragen moeten verwijderd kunnen worden
- **Acceptatiecriteria**:
  - Bevestigingsdialoog
  - Vraag wordt verwijderd uit quiz
  - Feedback na verwijdering

#### FE-010: Afbeelding Toevoegen
- **Prioriteit**: Laag
- **Beschrijving**: Optionele afbeelding kan aan vraag worden toegevoegd
- **Acceptatiecriteria**:
  - Bestand browser voor afbeelding selectie
  - Ondersteunde formaten: JPG, JPEG, PNG, BMP
  - Afbeeldingspad wordt opgeslagen
  - Afbeelding wordt getoond tijdens quiz

### 2.3 Quiz Presentatie

#### FE-011: Quiz Starten
- **Prioriteit**: Hoog
- **Beschrijving**: Geselecteerde quiz moet gestart kunnen worden
- **Acceptatiecriteria**:
  - "Start Quiz" knop is beschikbaar
  - Validatie: quiz moet geselecteerd zijn
  - Validatie: quiz moet minimaal 1 vraag bevatten
  - Game window opent in volledig scherm
  - Operator control window opent automatisch

#### FE-012: Dual Screen Functionaliteit
- **Prioriteit**: Hoog
- **Beschrijving**: Twee gescheiden schermen voor presentatie en bediening
- **Acceptatiecriteria**:
  - Game window: volledig scherm presentatie voor publiek
  - Operator window: bedieningspaneel voor quiz master
  - Beide vensters zijn gesynchroniseerd
  - Sluiten van operator window sluit ook game window

#### FE-013: Vraag Weergave
- **Prioriteit**: Hoog
- **Beschrijving**: Vragen moeten duidelijk gepresenteerd worden
- **Acceptatiecriteria**:
  - Vraagtekst groot en leesbaar
  - Vier antwoordopties met verschillende kleuren:
    - A: Rood
    - B: Blauw
    - C: Geel
    - D: Groen
  - Vraagnummer weergave (bijv. "Vraag 1 van 10")
  - Quiz naam in header
  - Afbeelding wordt getoond indien beschikbaar

#### FE-014: Timer Functionaliteit
- **Prioriteit**: Hoog
- **Beschrijving**: Elke vraag heeft een instelbare timer
- **Acceptatiecriteria**:
  - Timer toont resterende seconden
  - Start/stop knoppen in operator panel
  - Timer reset bij nieuwe vraag
  - Kleurcodering:
    - > 10 seconden: Geel
    - 5-10 seconden: Geel (waarschuwing)
    - < 5 seconden: Rood (urgent)
    - 0 seconden: "TIJD OM!" bericht in rood
  - Timer is zichtbaar in beide vensters

#### FE-015: Antwoord Onthulling
- **Prioriteit**: Hoog
- **Beschrijving**: Correcte antwoord moet getoond kunnen worden
- **Acceptatiecriteria**:
  - "Toon Antwoord" knop in operator panel
  - Correcte antwoord wordt gemarkeerd in helder groen
  - Andere antwoorden worden grijs
  - Zelfde visuele feedback in beide vensters
  - Knop verandert naar "Volgende" na onthulling

#### FE-016: Vraag Navigatie
- **Prioriteit**: Hoog
- **Beschrijving**: Operator moet door vragen kunnen navigeren
- **Acceptatiecriteria**:
  - "Vorige" knop (uitgeschakeld bij eerste vraag)
  - "Volgende" knop (verschijnt na antwoord onthulling)
  - Bij laatste vraag: bevestiging voor quiz beëindigen
  - Navigatie werkt in beide vensters synchroon

### 2.4 Gebruikersinterface

#### FE-017: Modern Design
- **Prioriteit**: Gemiddeld
- **Beschrijving**: Applicatie heeft een moderne, aantrekkelijke interface
- **Acceptatiecriteria**:
  - Donker kleurenschema met blauwe accenten
  - Gradient achtergronden
  - Drop shadow effecten
  - Afgeronde hoeken
  - Hover effecten op knoppen
  - Emoji iconen voor visuele duidelijkheid

#### FE-018: Responsive Feedback
- **Prioriteit**: Gemiddeld
- **Beschrijving**: Gebruiker krijgt feedback bij acties
- **Acceptatiecriteria**:
  - Bevestigingsdialogen bij kritieke acties (verwijderen, sluiten)
  - Succesmeldingen bij opslaan
  - Foutmeldingen bij validatiefouten
  - Visuele feedback bij hover over knoppen

#### FE-019: Operator Panel Informatie
- **Prioriteit**: Hoog
- **Beschrijving**: Operator heeft volledig overzicht van quiz status
- **Acceptatiecriteria**:
  - Huidige vraag tekst zichtbaar
  - Alle antwoordopties zichtbaar met kleurcodering
  - Correcte antwoord prominent weergegeven
  - Vraagnummer en totaal
  - Timer status
  - Bedieningsknoppen duidelijk gelabeld

---

## 3. Niet-Functionele Eisen

### 3.1 Prestatie

#### NFE-001: Responsiviteit
- **Beschrijving**: Applicatie moet snel reageren op gebruikersacties
- **Criteria**: UI updates binnen 100ms, data laden binnen 1 seconde

#### NFE-002: Timer Nauwkeurigheid
- **Beschrijving**: Timer moet nauwkeurig seconden tellen
- **Criteria**: Maximaal 50ms afwijking per seconde

### 3.2 Betrouwbaarheid

#### NFE-003: Data Persistentie
- **Beschrijving**: Quiz data moet betrouwbaar opgeslagen worden
- **Criteria**: 
  - Data wordt opgeslagen in JSON formaat
  - Automatische backup bij wijzigingen
  - Foutafhandeling bij lees/schrijf operaties

#### NFE-004: Error Handling
- **Beschrijving**: Applicatie moet fouten correct afhandelen
- **Criteria**:
  - Geen crashes bij ongeldige input
  - Duidelijke foutmeldingen voor gebruiker
  - Debug logging voor ontwikkelaars

### 3.3 Bruikbaarheid

#### NFE-005: Gebruiksvriendelijkheid
- **Beschrijving**: Applicatie moet intuïtief te gebruiken zijn
- **Criteria**:
  - Nieuwe gebruiker kan binnen 5 minuten een quiz starten
  - Duidelijke labels en iconen
  - Logische workflow

#### NFE-006: Toegankelijkheid
- **Beschrijving**: Tekst moet goed leesbaar zijn
- **Criteria**:
  - Minimale fontsize 12pt voor normale tekst
  - Hoog contrast tussen tekst en achtergrond
  - Duidelijke kleurcodering

### 3.4 Compatibiliteit

#### NFE-007: Platform
- **Beschrijving**: Applicatie draait op Windows platform
- **Criteria**:
  - .NET 8.0 Windows
  - WPF framework
  - Windows 10 of nieuwer

#### NFE-008: Bestandsformaten
- **Beschrijving**: Ondersteunde afbeeldingsformaten
- **Criteria**: JPG, JPEG, PNG, BMP

---

## 4. Data Eisen

### 4.1 Quiz Data Model

#### DE-001: Quiz Structuur
- **Velden**:
  - Id (integer, uniek, automatisch gegenereerd)
  - Name (string, verplicht)
  - Description (string, optioneel)
  - CreatedDate (datetime, automatisch)
  - TimePerQuestion (integer, standaard 30 seconden)
  - Questions (lijst van Question objecten)

#### DE-002: Question Structuur
- **Velden**:
  - Id (integer, uniek binnen quiz)
  - Text (string, verplicht)
  - ImagePath (string, optioneel)
  - AnswerA (string, verplicht)
  - AnswerB (string, verplicht)
  - AnswerC (string, verplicht)
  - AnswerD (string, verplicht)
  - CorrectAnswer (char: 'A', 'B', 'C', of 'D', verplicht)
  - QuizId (integer, referentie naar Quiz)

### 4.2 Data Opslag

#### DE-003: JSON Bestand
- **Beschrijving**: Data wordt opgeslagen in quizdata.json
- **Locatie**: Project root directory
- **Format**: JSON met indentatie voor leesbaarheid
- **Encoding**: UTF-8

---

## 5. Beveiligingseisen

### BE-001: Data Validatie
- Alle invoervelden moeten gevalideerd worden
- Geen SQL injectie mogelijk (niet van toepassing - geen database)
- Bestandspaden voor afbeeldingen valideren

### BE-002: Bevestigingen
- Destructieve acties (verwijderen) vereisen bevestiging
- Quiz afsluiten tijdens presentatie vereist bevestiging

---

## 6. Onderhoud en Uitbreidbaarheid

### OE-001: Code Structuur
- Gescheiden Models, Views en Services (MVC-achtig patroon)
- Duidelijke naamgeving en code comments
- Herbruikbare componenten

### OE-002: Toekomstige Uitbreidingen
De architectuur moet ruimte bieden voor:
- Score tracking systeem
- Multi-player functionaliteit
- Database backend (vervanging van JSON)
- Export/import functionaliteit
- Statistieken en rapportage

---

## 7. Gebruikersrollen

### 7.1 Quiz Master / Operator
- Kan quizzen aanmaken, bewerken en verwijderen
- Kan vragen beheren
- Bedient quiz presentatie
- Controleert timer
- Bepaalt wanneer antwoorden worden getoond

### 7.2 Publiek / Deelnemers
- Ziet game window presentatie
- Geen interactie met systeem (toekomstige uitbreiding)

---

## 8. Acceptatiecriteria

De applicatie wordt geaccepteerd wanneer:
1. Alle hoge prioriteit functionele eisen zijn geïmplementeerd
2. Quiz kan worden aangemaakt met minimaal 1 vraag
3. Quiz kan succesvol worden gepresenteerd op twee schermen
4. Timer werkt correct en accuraat
5. Antwoorden kunnen worden onthuld
6. Navigatie tussen vragen werkt vlekkeloos
7. Data wordt correct opgeslagen en geladen
8. Geen kritieke bugs of crashes
9. Interface is gebruiksvriendelijk en visueel aantrekkelijk

---

## 9. Beperkingen en Aannames

### Beperkingen
- Geen netwerkfunctionaliteit (standalone applicatie)
- Geen echte multi-player (alleen presentatie)
- Beperkte afbeelding ondersteuning (geen video/audio)
- Windows platform only

### Aannames
- Gebruiker heeft basiskennis van Windows
- Systeem heeft voldoende schermruimte (minimaal 1920x1080 aanbevolen)
- Afbeeldingsbestanden zijn lokaal toegankelijk
- Applicatie draait op één computer

---

## 10. Glossary

| Term | Definitie |
|------|-----------|
| Quiz | Een verzameling van vragen met antwoordopties |
| Vraag | Individuele vraag met 4 antwoordopties |
| Game Window | Volledig scherm presentatie voor publiek |
| Operator Panel | Bedieningspaneel voor quiz master |
| Timer | Afteller voor tijd per vraag |
| Quiz Master | Persoon die de quiz bedient en presenteert |

---

**Document versie**: 1.0  
**Datum**: November 2025  
**Status**: Finaal  
**Auteur**: ROC-Events Development Team
