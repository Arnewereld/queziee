# Technisch en Functioneel Ontwerp - ROC-Events Quiz Applicatie

## 1. Overzicht

### 1.1 Inleiding
Dit document beschrijft het technische en functionele ontwerp van de ROC-Events Quiz Applicatie. De applicatie is een WPF (Windows Presentation Foundation) desktop applicatie gebouwd met .NET 8.0, ontworpen voor het presenteren van interactieve quizzen in een educatieve of evenement context.

### 1.2 Architectuur Overzicht
De applicatie volgt een gelaagde architectuur met scheiding tussen:
- **Presentatie Laag** (Views): WPF Windows en XAML
- **Business Logic Laag** (Services): Data management en business logica
- **Data Laag** (Models): Data modellen en structuren

---

## 2. Technische Specificaties

### 2.1 Technologie Stack

| Component | Technologie | Versie |
|-----------|------------|--------|
| Framework | .NET | 8.0-windows |
| UI Framework | WPF | Included in .NET 8 |
| Taal | C# | 12.0 |
| Data Formaat | JSON | System.Text.Json |
| Build System | MSBuild | .NET SDK |

### 2.2 Project Configuratie

**Project Type**: WinExe (Windows Executable)  
**Target Framework**: net8.0-windows  
**Nullable**: Enabled  
**ImplicitUsings**: Enabled  
**UseWPF**: true

### 2.3 Afhankelijkheden
De applicatie gebruikt alleen standaard .NET 8.0 libraries:
- System.Windows (WPF)
- System.Text.Json (JSON serialisatie)
- System.IO (Bestandsoperaties)
- System.ComponentModel.DataAnnotations (Validatie)

---

## 3. Applicatie Structuur

### 3.1 Folder Structuur
```
queziee/
??? Models/
?   ??? Quiz.cs
?   ??? Question.cs
??? Services/
?   ??? QuizDataService.cs
??? Views/
?   ??? GameWindow.xaml
?   ??? GameWindow.xaml.cs
?   ??? OperatorControlWindow.xaml
?   ??? OperatorControlWindow.xaml.cs
?   ??? QuestionManagerWindow.xaml
?   ??? QuestionManagerWindow.xaml.cs
??? MainWindow.xaml
??? MainWindow.xaml.cs
??? App.xaml
??? App.xaml.cs
??? quizdata.json
??? queziee.csproj
```

---

## 4. Data Modellen

### 4.1 Quiz Model

**Bestand**: `Models/Quiz.cs`

```csharp
public class Quiz
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public int TimePerQuestion { get; set; } = 30; // seconds
    
    public List<Question> Questions { get; set; } = new List<Question>();
}
```

**Eigenschappen**:
- `Id`: Unieke identifier voor de quiz (auto-increment)
- `Name`: Naam van de quiz (verplicht)
- `Description`: Optionele beschrijving
- `CreatedDate`: Automatisch ingesteld bij aanmaak
- `TimePerQuestion`: Tijd in seconden per vraag (standaard 30)
- `Questions`: Lijst van bijbehorende vragen

### 4.2 Question Model

**Bestand**: `Models/Question.cs`

```csharp
public class Question
{
    public int Id { get; set; }
    
    [Required]
    public string Text { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; }
    
    [Required]
    public string AnswerA { get; set; } = string.Empty;
    
    [Required]
    public string AnswerB { get; set; } = string.Empty;
    
    [Required]
    public string AnswerC { get; set; } = string.Empty;
    
    [Required]
    public string AnswerD { get; set; } = string.Empty;
    
    [Required]
    public char CorrectAnswer { get; set; } // A, B, C, or D
    
    public int QuizId { get; set; }
    
    public Quiz? Quiz { get; set; }
}
```

**Eigenschappen**:
- `Id`: Unieke identifier binnen de quiz
- `Text`: De vraagtekst (verplicht)
- `ImagePath`: Optioneel pad naar afbeelding
- `AnswerA, B, C, D`: De vier antwoordopties (alle verplicht)
- `CorrectAnswer`: Char 'A', 'B', 'C' of 'D' voor correct antwoord
- `QuizId`: Foreign key naar parent Quiz
- `Quiz`: Navigatie property (nullable)

---

## 5. Service Laag

### 5.1 QuizDataService

**Bestand**: `Services/QuizDataService.cs`

**Verantwoordelijkheid**: Beheer van quiz data operaties (CRUD)

#### 5.1.1 Constructor en Bestandspad Bepaling

```csharp
public QuizDataService()
{
    var binDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var projectRoot = Path.GetFullPath(Path.Combine(binDirectory, @"..\..\..\"));
    _dataFilePath = Path.Combine(projectRoot, DataFileName);
}
```

**Logica**:
- Bepaalt runtime locatie (bin/Debug/net8.0-windows)
- Navigeert 3 niveaus omhoog naar project root
- Combineert met "quizdata.json" bestandsnaam
- Debug output voor verificatie

#### 5.1.2 Publieke Methoden

| Methode | Return Type | Beschrijving |
|---------|-------------|--------------|
| `GetQuizzesAsync()` | `Task<List<Quiz>>` | Haalt alle quizzen op uit JSON |
| `SaveQuizzesAsync(List<Quiz>)` | `Task` | Slaat alle quizzen op naar JSON |
| `GetQuizByIdAsync(int)` | `Task<Quiz?>` | Haalt specifieke quiz op op basis van ID |
| `AddQuizAsync(Quiz)` | `Task` | Voegt nieuwe quiz toe |
| `UpdateQuizAsync(Quiz)` | `Task` | Update bestaande quiz |
| `DeleteQuizAsync(int)` | `Task` | Verwijdert quiz op basis van ID |

#### 5.1.3 JSON Serialisatie Configuratie

```csharp
new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
}
```

**Features**:
- Case-insensitive property matching bij deserialisatie
- Geformatteerde output met indentatie voor leesbaarheid
- UTF-8 encoding

#### 5.1.4 Error Handling
- Try-catch blokken voor I/O operaties
- Debug logging voor diagnose
- Lege lijst return bij fouten (fail-safe)
- Expliciete bestand-existentie checks

---

## 6. View Laag

### 6.1 MainWindow

**Bestanden**: `MainWindow.xaml` + `MainWindow.xaml.cs`

**Doel**: Hoofdvenster en startpunt van de applicatie

#### 6.1.1 Functionaliteit

**Quiz Selectie**:
- ComboBox met alle beschikbare quizzen
- Display member path: "Name"
- Selection changed event handler

**Quiz Informatie Weergave**:
- TextBlocks voor naam, beschrijving, statistieken
- ListView met vragenpreview
- DataTemplate voor vraagweergave met correcte antwoord

**Acties**:
1. **Start Quiz**: 
   - Validatie (quiz geselecteerd, minimaal 1 vraag)
   - Opent GameWindow (fullscreen)
   - Opent OperatorControlWindow
   - Link tussen beide vensters
   
2. **Vragen Beheren**:
   - Opent QuestionManagerWindow als modal dialog
   - Refresh quizzes na sluiten

3. **Open Speelscherm**:
   - Brengt bestaande GameWindow naar voren
   - Activeert ook OperatorControlWindow

#### 6.1.2 UI Design Elementen

**Resources**:
- `ModernButton` Style: Afgeronde hoeken, shadow effect, hover animatie
- `PanelGradient`: LinearGradientBrush voor panelen
- Verschillende gradient achtergronden

**Layout**:
- Grid met 3 rijen (Header, Content, Footer)
- Content met 3 kolommen (Left panel, spacer, Right panel)
- Margins en padding voor witruimte

**Kleurenschema**:
- Primary: Blues (#1e40af, #3b82f6, #60a5fa)
- Background: Dark grays (#0f172a, #1e293b)
- Accents: Yellow (#fbbf24), Green (#10b981)
- Effects: Shadow en glow effecten

### 6.2 GameWindow

**Bestanden**: `Views/GameWindow.xaml` + `Views/GameWindow.xaml.cs`

**Doel**: Fullscreen presentatie voor publiek

#### 6.2.1 Architectuur

**Constructor Parameters**:
- `Quiz quiz`: De te presenteren quiz

**Private Fields**:
```csharp
private readonly Quiz _quiz;
private int _currentQuestionIndex = 0;
private DispatcherTimer? _timer;
private int _timeRemaining;
```

**Events**:
```csharp
public event EventHandler<int>? TimerTicked;
public event EventHandler? TimerFinished;
```

#### 6.2.2 Timer Implementatie

**DispatcherTimer Configuratie**:
- Interval: 1 seconde
- Tick event: Decrement time, update UI, fire events

**Timer States**:
```
_timeRemaining > 10:  Yellow  (#fbbf24)
_timeRemaining 5-10:  Yellow  (#fbbf24)
_timeRemaining < 5:   Red     (#dc2626)
_timeRemaining = 0:   "TIJD OM!" Red (#dc2626)
```

**Event Publishing**:
- `TimerTicked(int timeRemaining)`: Elke seconde
- `TimerFinished()`: Bij 0 seconden

#### 6.2.3 Vraag Weergave

**UI Elementen**:
- `QuestionText`: Grote, leesbare vraagtekst
- `QuestionNumberText`: "Vraag X van Y"
- `QuestionImage`: Conditioneel zichtbaar
- Vier antwoord borders met standaard kleuren:
  - A: Rood (#ef4444)
  - B: Blauw (#3b82f6)
  - C: Geel (#eab308)
  - D: Groen (#22c55e)

**Afbeelding Handling**:
```csharp
if (!string.IsNullOrEmpty(question.ImagePath) && File.Exists(question.ImagePath))
{
    QuestionImage.Source = new BitmapImage(new Uri(question.ImagePath));
    QuestionImage.Visibility = Visibility.Visible;
}
```

#### 6.2.4 Publieke Interface voor Operator

| Methode | Beschrijving |
|---------|--------------|
| `ShowCorrectAnswer()` | Markeert correct antwoord groen, rest grijs |
| `MoveToNextQuestion()` | Increment index, display next question |
| `MoveToPreviousQuestion()` | Decrement index, display previous |
| `StartTimer()` | Reset en start timer |
| `StopTimer()` | Stopt timer |

#### 6.2.5 Cleanup

```csharp
protected override void OnClosed(EventArgs e)
{
    _timer?.Stop();
    base.OnClosed(e);
}
```

### 6.3 OperatorControlWindow

**Bestanden**: `Views/OperatorControlWindow.xaml` + `Views/OperatorControlWindow.xaml.cs`

**Doel**: Bedieningspaneel voor quiz master

#### 6.3.1 Architectuur

**Constructor Parameters**:
```csharp
public OperatorControlWindow(Quiz quiz, GameWindow gameWindow)
```

**Private Fields**:
```csharp
private readonly Quiz _quiz;
private readonly GameWindow _gameWindow;
private int _currentQuestionIndex = 0;
private bool _answerRevealed = false;
```

**Event Subscriptions**:
```csharp
_gameWindow.TimerTicked += OnGameWindowTimerTicked;
_gameWindow.TimerFinished += OnGameWindowTimerFinished;
```

#### 6.3.2 Synchronisatie met GameWindow

**Timer Updates**:
```csharp
private void OnGameWindowTimerTicked(object? sender, int timeRemaining)
{
    TimerDisplay.Text = $"{timeRemaining}s";
    // Update kleur gebaseerd op tijd
}
```

**State Management**:
- `_answerRevealed`: Track of antwoord getoond is
- Synchroniseert navigatie tussen beide vensters
- UI button state updates

#### 6.3.3 UI Flow

**Initial State**:
- Toon vraag informatie
- "Toon Antwoord" knop zichtbaar
- "Volgende" knop verborgen
- "Vorige" disabled bij eerste vraag

**Na Antwoord Onthulling**:
- Highlight correct antwoord in operator panel
- Roep `_gameWindow.ShowCorrectAnswer()` aan
- Verberg "Toon Antwoord" knop
- Toon "Volgende" knop

**Navigatie**:
```csharp
private void NextButton_Click(object sender, RoutedEventArgs e)
{
    if (_currentQuestionIndex < _quiz.Questions.Count - 1)
    {
        _currentQuestionIndex++;
        _gameWindow.MoveToNextQuestion();
        DisplayCurrentQuestion();
    }
    else
    {
        // End of quiz dialog
    }
}
```

#### 6.3.4 Informatieweergave

**Operator Informatie**:
- Quiz naam
- Huidige vraag tekst
- Alle vier antwoordopties met kleurcodering
- Correcte antwoord prominent gemarkeerd
- Vraagnummer (X van Y)
- Timer status

**Kleuren Matching**:
- Zelfde kleuren als GameWindow voor consistentie
- Groen highlight voor correct antwoord na onthulling
- Grijs voor incorrecte antwoorden

#### 6.3.5 Memory Management

```csharp
protected override void OnClosed(EventArgs e)
{
    // Unsubscribe to prevent memory leaks
    _gameWindow.TimerTicked -= OnGameWindowTimerTicked;
    _gameWindow.TimerFinished -= OnGameWindowTimerFinished;
    base.OnClosed(e);
}
```

### 6.4 QuestionManagerWindow

**Bestanden**: `Views/QuestionManagerWindow.xaml` + `Views/QuestionManagerWindow.xaml.cs`

**Doel**: CRUD operaties voor quizzen en vragen

#### 6.4.1 Architectuur

**Constructor Parameters**:
```csharp
public QuestionManagerWindow(QuizDataService dataService)
```

**Private Fields**:
```csharp
private readonly QuizDataService _dataService;
private List<Quiz> _quizzes = new List<Quiz>();
private Quiz? _selectedQuiz;
private Question? _selectedQuestion;
private bool _isEditingNewQuiz = false;
private bool _isEditingNewQuestion = false;
private int _currentQuestionIndex = -1;
```

#### 6.4.2 Dual Panel Systeem

**Left Panel**: Quiz Lijst
- ListBox met alle quizzen
- Knoppen: Nieuwe Quiz, Opslaan, Verwijderen
- Quiz editor panel (naam, beschrijving, tijd)

**Right Panel**: Vragen Lijst en Editor
- ListBox met vragen van geselecteerde quiz
- Vraag navigatie knoppen (vorige/volgende)
- Vraag editor panel met alle velden
- Afbeelding browser

**Panel Visibility Management**:
```csharp
private void ShowQuizEditor()
private void ShowQuestionEditor()
private void HideAllEditors()
```

#### 6.4.3 Quiz CRUD Operaties

**Create**:
```csharp
private void NewQuizButton_Click(object sender, RoutedEventArgs e)
{
    _selectedQuiz = new Quiz { Name = "Nieuwe Quiz", ... };
    _isEditingNewQuiz = true;
    ShowQuizEditor();
}
```

**Save**:
```csharp
private async void SaveQuizButton_Click(object sender, RoutedEventArgs e)
{
    // Validatie
    if (string.IsNullOrWhiteSpace(QuizNameTextBox.Text)) return;
    
    // Update object
    _selectedQuiz.Name = QuizNameTextBox.Text.Trim();
    
    // Persist
    if (_isEditingNewQuiz)
        await _dataService.AddQuizAsync(_selectedQuiz);
    else
        await _dataService.UpdateQuizAsync(_selectedQuiz);
    
    LoadQuizzes();
}
```

**Delete**:
```csharp
private async void DeleteQuizButton_Click(object sender, RoutedEventArgs e)
{
    var result = MessageBox.Show(..., MessageBoxButton.YesNo, ...);
    if (result == MessageBoxResult.Yes)
    {
        await _dataService.DeleteQuizAsync(_selectedQuiz.Id);
        LoadQuizzes();
    }
}
```

#### 6.4.4 Vraag CRUD Operaties

**ID Generatie voor Nieuwe Vragen**:
```csharp
var maxId = _selectedQuiz.Questions.Any() 
    ? _selectedQuiz.Questions.Max(q => q.Id) 
    : 0;
_selectedQuestion.Id = maxId + 1;
```

**Vraag Opslaan**:
```csharp
private async void SaveQuestionButton_Click(object sender, RoutedEventArgs e)
{
    // Validatie alle velden
    if (string.IsNullOrWhiteSpace(QuestionTextBox.Text) || ...) return;
    
    // Bepaal correct antwoord van radio buttons
    char correctAnswer = 'A';
    if (CorrectAnswerB.IsChecked == true) correctAnswer = 'B';
    // ...
    
    // Update object
    _selectedQuestion.Text = QuestionTextBox.Text.Trim();
    _selectedQuestion.CorrectAnswer = correctAnswer;
    
    // Add to quiz if new
    if (_isEditingNewQuestion)
        _selectedQuiz.Questions.Add(_selectedQuestion);
    
    // Persist entire quiz
    await _dataService.UpdateQuizAsync(_selectedQuiz);
    
    // Refresh UI
    QuestionsListBox.ItemsSource = null;
    QuestionsListBox.ItemsSource = _selectedQuiz.Questions;
}
```

#### 6.4.5 Vraag Navigatie

**Vorige/Volgende Knoppen**:
```csharp
private void UpdateQuestionNavigationButtons()
{
    PreviousQuestionButton.IsEnabled = _currentQuestionIndex > 0;
    NextQuestionButton.IsEnabled = _currentQuestionIndex < _selectedQuiz.Questions.Count - 1;
}

private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
{
    _currentQuestionIndex++;
    _selectedQuestion = _selectedQuiz.Questions[_currentQuestionIndex];
    QuestionsListBox.SelectedIndex = _currentQuestionIndex;
    ShowQuestionEditor();
}
```

#### 6.4.6 Afbeelding Selectie

```csharp
private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
{
    var openFileDialog = new OpenFileDialog
    {
        Title = "Selecteer een afbeelding",
        Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*"
    };
    
    if (openFileDialog.ShowDialog() == true)
    {
        ImagePathTextBox.Text = openFileDialog.FileName;
    }
}
```

#### 6.4.7 Validatie

**Quiz Validatie**:
- Naam: Verplicht, niet leeg
- Tijd per vraag: Positief integer

**Vraag Validatie**:
- Vraagtekst: Verplicht
- Alle vier antwoorden: Verplicht
- Correct antwoord: Verplicht (via radio button - altijd geselecteerd)

**Feedback**:
- MessageBox met Warning icon bij validatiefout
- MessageBox met Information icon bij succes
- MessageBox met Error icon bij excepties

---

## 7. XAML Design Patterns

### 7.1 Resource Definitions

**Button Styles**:
```xaml
<Style x:Key="ModernButton" TargetType="Button">
    <Setter Property="Cursor" Value="Hand"/>
    <Setter Property="FontWeight" Value="Bold"/>
    <Setter Property="BorderThickness" Value="0"/>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border CornerRadius="8" Padding="{TemplateBinding Padding}">
                    <Border.Effect>
                        <DropShadowEffect Color="Black" BlurRadius="10" 
                                        ShadowDepth="3" Opacity="0.4"/>
                    </Border.Effect>
                    <!-- Content -->
                </Border>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter Property="BorderBrush" Value="White"/>
                        <Setter Property="BorderThickness" Value="2"/>
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

**Brushes**:
```xaml
<LinearGradientBrush x:Key="PanelGradient" StartPoint="0,0" EndPoint="0,1">
    <GradientStop Color="#1e293b" Offset="0"/>
    <GradientStop Color="#0f172a" Offset="1"/>
</LinearGradientBrush>
```

### 7.2 Data Templates

**Vraag Preview Template** (MainWindow):
```xaml
<ListView.ItemTemplate>
    <DataTemplate>
        <Border Background="#0f172a" CornerRadius="6" Padding="12" Margin="0,4">
            <StackPanel>
                <TextBlock Text="{Binding Text}" FontWeight="Bold" 
                           TextWrapping="Wrap" Foreground="White"/>
                <TextBlock Text="{Binding CorrectAnswer, StringFormat='? Correct: {0}'}" 
                           FontSize="11" Foreground="#10b981"/>
            </StackPanel>
        </Border>
    </DataTemplate>
</ListView.ItemTemplate>
```

### 7.3 Layout Patterns

**Grid met Responsive Rows**:
```xaml
<Grid.RowDefinitions>
    <RowDefinition Height="Auto"/>    <!-- Header -->
    <RowDefinition Height="*"/>       <!-- Content (flexible) -->
    <RowDefinition Height="Auto"/>    <!-- Footer -->
</Grid.RowDefinitions>
```

**Two-Column Layout**:
```xaml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="340"/>   <!-- Fixed sidebar -->
    <ColumnDefinition Width="25"/>    <!-- Spacer -->
    <ColumnDefinition Width="*"/>     <!-- Flexible content -->
</Grid.ColumnDefinitions>
```

---

## 8. Data Flow Diagrammen

### 8.1 Applicatie Startup

```
App.xaml.cs (Application_Startup)
    ?
MainWindow Constructor
    ?
QuizDataService Constructor
    ?
LoadQuizzes() async
    ?
QuizDataService.GetQuizzesAsync()
    ?
Lees quizdata.json
    ?
Deserialize naar List<Quiz>
    ?
Bind aan QuizComboBox.ItemsSource
```

### 8.2 Quiz Start Flow

```
User clicks "Start Quiz"
    ?
StartQuizButton_Click validatie
    ?
new GameWindow(_selectedQuiz)
    ?
GameWindow.Show() + WindowState = Maximized
    ?
new OperatorControlWindow(_selectedQuiz, _gameWindow)
    ?
OperatorControlWindow.Show()
    ?
Subscribe to GameWindow events (TimerTicked, TimerFinished)
    ?
DisplayCurrentQuestion() in beide vensters
```

### 8.3 Timer Synchronisatie

```
User clicks "Start Timer" in OperatorControlWindow
    ?
_gameWindow.StartTimer()
    ?
DispatcherTimer.Start() in GameWindow
    ?
Elke seconde: Timer_Tick event
    ?
_timeRemaining--
    ?
Fire TimerTicked event met timeRemaining
    ?
OperatorControlWindow.OnGameWindowTimerTicked handler
    ?
Update TimerDisplay.Text in operator panel
    ?
Bij 0: Fire TimerFinished event
    ?
OperatorControlWindow.OnGameWindowTimerFinished handler
    ?
Display "TIJD OM!" in beide vensters
```

### 8.4 Quiz Data Save Flow

```
User edits quiz in QuestionManagerWindow
    ?
SaveQuizButton_Click
    ?
Valideer input
    ?
Update Quiz object in memory
    ?
QuizDataService.UpdateQuizAsync(quiz)
    ?
GetQuizzesAsync() - haal huidige data op
    ?
Find index van quiz in lijst
    ?
Replace quiz in lijst
    ?
SaveQuizzesAsync(updatedList)
    ?
Serialize lijst naar JSON
    ?
Schrijf naar quizdata.json
    ?
ShowDialog "Quiz opgeslagen!"
    ?
LoadQuizzes() - refresh UI
```

---

## 9. State Management

### 9.1 MainWindow State

| State Variable | Type | Purpose |
|----------------|------|---------|
| `_dataService` | QuizDataService | Singleton voor data operaties |
| `_quizzes` | List<Quiz> | Cached lijst van alle quizzen |
| `_selectedQuiz` | Quiz? | Currently selected quiz |
| `_gameWindow` | GameWindow? | Reference naar actieve game venster |
| `_operatorWindow` | OperatorControlWindow? | Reference naar operator panel |

### 9.2 GameWindow State

| State Variable | Type | Purpose |
|----------------|------|---------|
| `_quiz` | Quiz | De actieve quiz (readonly) |
| `_currentQuestionIndex` | int | Huidige vraag index |
| `_timer` | DispatcherTimer? | Timer instance |
| `_timeRemaining` | int | Resterende seconden |

### 9.3 OperatorControlWindow State

| State Variable | Type | Purpose |
|----------------|------|---------|
| `_quiz` | Quiz | De actieve quiz (readonly) |
| `_gameWindow` | GameWindow | Reference naar game venster (readonly) |
| `_currentQuestionIndex` | int | Huidige vraag index (sync met GameWindow) |
| `_answerRevealed` | bool | Of antwoord al getoond is |

### 9.4 QuestionManagerWindow State

| State Variable | Type | Purpose |
|----------------|------|---------|
| `_dataService` | QuizDataService | Data service instance |
| `_quizzes` | List<Quiz> | All quizzes |
| `_selectedQuiz` | Quiz? | Currently selected quiz |
| `_selectedQuestion` | Question? | Currently selected question |
| `_isEditingNewQuiz` | bool | Indicates new vs existing quiz |
| `_isEditingNewQuestion` | bool | Indicates new vs existing question |
| `_currentQuestionIndex` | int | Voor navigatie tussen vragen |

---

## 10. Event Handling

### 10.1 Event Patterns

**WPF Click Events**:
```csharp
private void ButtonName_Click(object sender, RoutedEventArgs e)
{
    // Event handling logic
}
```

**Selection Changed Events**:
```csharp
private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var selected = ComboBox.SelectedItem as DataType;
}
```

**Custom Events**:
```csharp
// Definition
public event EventHandler<int>? TimerTicked;

// Raise
TimerTicked?.Invoke(this, _timeRemaining);

// Subscribe
_gameWindow.TimerTicked += OnGameWindowTimerTicked;

// Handler
private void OnGameWindowTimerTicked(object? sender, int timeRemaining)
{
    // Handle event
}

// Unsubscribe (belangrijk!)
_gameWindow.TimerTicked -= OnGameWindowTimerTicked;
```

### 10.2 Event Flow: Antwoord Onthulling

```
OperatorControlWindow: ShowAnswerButton_Click
    ?
HighlightCorrectAnswer() (local UI update)
    ?
_gameWindow.ShowCorrectAnswer() (method call)
    ?
GameWindow.ShowCorrectAnswer() method
    ?
HighlightCorrectAnswer() in GameWindow (public view update)
    ?
Update UI state in OperatorControlWindow
    - ShowAnswerButton.Visibility = Collapsed
    - NextButton.Visibility = Visible
    - _answerRevealed = true
```

---

## 11. UI/UX Design Principes

### 11.1 Kleurensysteem

**Primary Palette**:
- Background Dark: #0f172a, #1e293b
- Primary Blue: #1e40af, #3b82f6, #60a5fa
- Accent Yellow: #fbbf24, #eab308
- Accent Green: #10b981, #22c55e
- Accent Red: #dc2626, #ef4444

**Antwoord Kleuren**:
- A: Rood (#ef4444 / #dc2626)
- B: Blauw (#3b82f6 / #2563eb)
- C: Geel (#eab308 / #ca8a04)
- D: Groen (#22c55e / #16a34a)
- Correct (revealed): #10b981
- Incorrect (revealed): #6b7280

**Timer Kleuren**:
- Normal (>10s): #fbbf24
- Warning (5-10s): #fbbf24
- Urgent (<5s): #dc2626
- Finished: #dc2626

### 11.2 Typografie

**Font Sizes**:
- Headers: 32pt (Main header), 20pt (Section headers)
- Game Question: 28pt-36pt
- Game Answers: 20pt-24pt
- Operator Info: 14pt-18pt
- Labels: 12pt-14pt
- Footer: 12pt

**Font Weights**:
- Headers: Bold / SemiBold
- Buttons: Bold
- Content: Normal / SemiBold
- Footer: SemiBold

### 11.3 Spacing en Layout

**Margins**:
- Window content: 30px
- Panel content: 25px
- Between elements: 12px-20px
- Compact spacing: 8px

**Padding**:
- Buttons: 18px, 12px (horizontal, vertical)
- Borders: 12px-20px
- Compact: 10px-12px

**Corner Radius**:
- Panels: 15px
- Buttons: 8px
- Inner elements: 6px-10px

### 11.4 Visual Effects

**Shadows**:
```xaml
<DropShadowEffect Color="Black" BlurRadius="10" 
                  ShadowDepth="3" Opacity="0.4"/>
```

**Glow Effects**:
```xaml
<DropShadowEffect Color="#3b82f6" BlurRadius="30" 
                  ShadowDepth="0" Opacity="0.7"/>
```

**Gradients**:
- LinearGradientBrush voor achtergronden
- Multi-stop gradients voor depth
- Horizontale en verticale orientaties

---

## 12. Performance Overwegingen

### 12.1 Async/Await Pattern

Alle I/O operaties zijn asynchroon:
```csharp
private async void LoadQuizzes()
{
    _quizzes = await _dataService.GetQuizzesAsync();
    // UI blijft responsive tijdens load
}
```

**Voordelen**:
- UI blijft responsive
- Geen blocking van UI thread
- Betere gebruikerservaring

### 12.2 Timer Optimalisatie

```csharp
_timer = new DispatcherTimer();
_timer.Interval = TimeSpan.FromSeconds(1);
```

- DispatcherTimer draait op UI thread (veilig voor UI updates)
- 1 seconde interval is efficient
- Stop timer bij OnClosed om resources vrij te geven

### 12.3 Memory Management

**Event Cleanup**:
```csharp
protected override void OnClosed(EventArgs e)
{
    _gameWindow.TimerTicked -= OnGameWindowTimerTicked;
    _gameWindow.TimerFinished -= OnGameWindowTimerFinished;
    _timer?.Stop();
    base.OnClosed(e);
}
```

**Disposal Pattern**:
- Timer wordt gestopt in OnClosed
- Event subscriptions worden verwijderd
- Voorkomt memory leaks

### 12.4 Data Caching

- Quizzes worden gecached in `_quizzes` lijst
- Refresh alleen bij data wijziging
- Voorkomt onnodige bestandsoperaties

---

## 13. Error Handling Strategie

### 13.1 Defensive Programming

**Null Checks**:
```csharp
if (_selectedQuiz == null)
{
    MessageBox.Show("Selecteer eerst een quiz!", ...);
    return;
}
```

**Range Checks**:
```csharp
if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _quiz.Questions.Count)
    return;
```

### 13.2 Try-Catch Blokken

**Service Layer**:
```csharp
try
{
    var json = await File.ReadAllTextAsync(_dataFilePath);
    var quizzes = JsonSerializer.Deserialize<List<Quiz>>(json, ...);
    return quizzes ?? new List<Quiz>();
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
    return new List<Quiz>();
}
```

**UI Layer**:
```csharp
try
{
    await _dataService.UpdateQuizAsync(_selectedQuiz);
    MessageBox.Show("Quiz opgeslagen!", "Succes", ...);
}
catch (Exception ex)
{
    MessageBox.Show($"Error saving quiz: {ex.Message}", "Error", ...);
}
```

### 13.3 User Feedback

**MessageBox Types**:
- **Warning**: Validatie fouten, lege selecties
- **Information**: Succes berichten, informatie
- **Error**: Exceptions, kritieke fouten
- **Question**: Bevestigingen voor destructieve acties

**Debug Logging**:
```csharp
System.Diagnostics.Debug.WriteLine($"?? Data file locatie: {_dataFilePath}");
System.Diagnostics.Debug.WriteLine($"? {quizzes.Count} quizzen opgeslagen");
```

---

## 14. Testing Scenarios

### 14.1 Unit Testing Scenario's

Hoewel niet geïmplementeerd, zijn dit de aanbevolen test scenario's:

**QuizDataService Tests**:
1. GetQuizzesAsync met leeg bestand
2. GetQuizzesAsync met geldig JSON
3. GetQuizzesAsync met corrupt JSON
4. AddQuizAsync en valideer ID generatie
5. UpdateQuizAsync en valideer wijzigingen
6. DeleteQuizAsync en valideer verwijdering

**Model Validation Tests**:
1. Quiz met ontbrekende naam (Required)
2. Question met ontbrekende velden
3. Correcte default waardes

### 14.2 Integration Testing Scenario's

**Happy Flow**:
1. Start applicatie
2. Selecteer quiz
3. Start quiz
4. Navigeer door vragen
5. Start timer
6. Toon antwoord
7. Ga naar volgende vraag
8. Beëindig quiz

**Edge Cases**:
1. Selecteer quiz zonder vragen
2. Timer bij 0 seconden
3. Laatste vraag navigatie
4. Sluiten tijdens actieve quiz
5. Corrupt JSON bestand

### 14.3 Manual Testing Checklist

- [ ] Quiz selectie werkt correct
- [ ] Quiz informatie wordt correct weergegeven
- [ ] Nieuwe quiz aanmaken
- [ ] Quiz bewerken en opslaan
- [ ] Quiz verwijderen met bevestiging
- [ ] Nieuwe vraag aanmaken
- [ ] Vraag bewerken (alle velden)
- [ ] Vraag verwijderen
- [ ] Afbeelding toevoegen aan vraag
- [ ] Quiz starten met valide quiz
- [ ] Quiz starten zonder selectie (error)
- [ ] GameWindow fullscreen weergave
- [ ] OperatorControlWindow opent correct
- [ ] Timer start en loopt correct
- [ ] Timer kleuren wijzigen op juiste momenten
- [ ] Antwoord onthulling werkt
- [ ] Navigatie naar volgende vraag
- [ ] Navigatie naar vorige vraag
- [ ] Laatste vraag beëindigings dialoog
- [ ] Operator window sluiten sluit game window
- [ ] "Open Speelscherm" knop werkt

---

## 15. Deployment

### 15.1 Build Configuratie

**Debug Build**:
```
dotnet build --configuration Debug
```

**Release Build**:
```
dotnet build --configuration Release
```

### 15.2 Output Locatie

**Debug**:
```
queziee\bin\Debug\net8.0-windows\
    queziee.exe
    queziee.dll
    quizdata.json (copied from project root)
    ... (runtime dependencies)
```

**Release**:
```
queziee\bin\Release\net8.0-windows\
    queziee.exe
    queziee.dll
    quizdata.json
    ... (optimized runtime dependencies)
```

### 15.3 Distributie

**Single-file Deployment** (optioneel):
```
dotnet publish -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true
```

**Dependencies**:
- .NET 8.0 Runtime (indien niet self-contained)
- quizdata.json moet in project root blijven
- Afbeeldingen moeten toegankelijk zijn op opgegeven paden

### 15.4 Installatie Instructies

1. Installeer .NET 8.0 Runtime (indien nodig)
2. Kopieer applicatie folder naar gewenste locatie
3. Zorg dat quizdata.json in de juiste locatie staat (3 niveaus boven exe)
4. Start queziee.exe
5. Optioneel: Voeg shortcut toe aan Desktop/Start Menu

---

## 16. Toekomstige Uitbreidingen

### 16.1 Geplande Features

**Scorekeeping Systeem**:
- Team registratie
- Punten toekenning per correct antwoord
- Scoreboard weergave
- Highscore persistentie

**Multiplayer**:
- Buzzer systeem voor teams
- Real-time antwoord registratie
- Team competitie modus

**Database Migratie**:
- SQLite of SQL Server backend
- Entity Framework Core integratie
- Betere data relaties en queries

**Export/Import**:
- Export quiz naar JSON/XML
- Import van externe quiz formats
- Template systeem

**Multimedia**:
- Video support in vragen
- Audio clips
- Animated transitions

### 16.2 Architectuur Aanpassingen

Voor bovenstaande features zijn deze wijzigingen nodig:

**Repository Pattern**:
```csharp
interface IQuizRepository
{
    Task<List<Quiz>> GetAllAsync();
    Task<Quiz?> GetByIdAsync(int id);
    Task AddAsync(Quiz quiz);
    Task UpdateAsync(Quiz quiz);
    Task DeleteAsync(int id);
}

class JsonQuizRepository : IQuizRepository { }
class DatabaseQuizRepository : IQuizRepository { }
```

**Score Model**:
```csharp
public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
}

public class QuizSession
{
    public int Id { get; set; }
    public Quiz Quiz { get; set; }
    public List<Team> Teams { get; set; }
    public DateTime StartTime { get; set; }
}
```

**MVVM Pattern**:
Voor betere testability en separation of concerns:
```csharp
public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Quiz> _quizzes;
    public ICommand StartQuizCommand { get; }
    public ICommand ManageQuestionsCommand { get; }
}
```

---

## 17. Conclusie

### 17.1 Sterke Punten

? **Schone Architectuur**:
- Duidelijke scheiding tussen layers
- Herbruikbare componenten
- Maintainable code

? **Gebruiksvriendelijke Interface**:
- Modern design
- Duidelijke visuele feedback
- Intuïtieve workflow

? **Robuuste Functionaliteit**:
- Betrouwbare data persistentie
- Goede error handling
- Stabiele timer implementatie

? **Dual-Screen Systeem**:
- Effectieve scheiding presentatie/bediening
- Goede synchronisatie
- Professional presentation

### 17.2 Verbeterpunten

?? **Testing**:
- Geen unit tests
- Beperkte error scenario coverage

?? **Data Validatie**:
- Basis validatie aanwezig
- Meer input sanitization mogelijk

?? **Schaalbaarheid**:
- JSON bestand kan groot worden
- Geen pagination in UI

?? **Configuratie**:
- Hardcoded waarden
- Geen app settings/configuratie bestand

### 17.3 Best Practices Gevolgd

?? **Async/Await** voor I/O operaties  
?? **SOLID Principes**: Single responsibility voor services  
?? **WPF MVVM Elementen**: Data binding, commands pattern  
?? **Resource Management**: Event cleanup, timer disposal  
?? **User Feedback**: Duidelijke dialogen en meldingen  
?? **Code Organisatie**: Logische folder structuur  

---

**Document Versie**: 1.0  
**Datum**: November 2025  
**Status**: Finaal  
**Auteur**: ROC-Events Development Team  
**Framework**: .NET 8.0 WPF  
**Taal**: C# 12.0
