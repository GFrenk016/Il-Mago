# 🧙 Roadmap — Mini RPG/Action 2D Fantasy

## Visione del progetto

Realizzare e completare un piccolo action game **2D top-down fantasy**, ispirato alla costruzione del mondo e alla visuale di *The Escapists*.

Il giocatore controlla un **mago**, affronta mostri fantasy usando tre magie e attraversa **tre livelli** di difficoltà crescente. Al termine di ogni livello riceve un punteggio e una valutazione da **1 a 5 stelle**.

**Durata indicativa dello sviluppo:** 3–5 settimane.  
**Obiettivo principale:** arrivare a una build completa e giocabile, senza espandere il progetto oltre lo scopo iniziale.

### Funzionalità principali

- Movimento top-down in 8 direzioni
- Camera che segue il giocatore
- Combattimento con tre magie
- Tre archetipi di nemici fantasy
- Tre livelli completi
- Boss finale semplice
- Sistema di vita, danno e Game Over
- Punteggio basato su uccisioni, tempo e vita rimasta
- Valutazione da 1 a 5 stelle
- Menu, selezione livelli e salvataggio dei progressi
- Audio, effetti e build finale per Windows

## Milestone 0 — Fondamenta del progetto

**Obiettivo:** creare un progetto Unity pulito e organizzato.

- [X] Creare un nuovo progetto Unity 2D
- [X] Creare le cartelle `Scenes`, `Scripts`, `Prefabs`, `Sprites`, `Animations`, `Audio` e `UI`
- [X] Creare la scena `MainMenu`
- [X] Creare la scena di test `Level_Test`
- [X] Impostare risoluzione e camera
- [X] Configurare i sorting layer: `Ground`, `Environment`, `Player`, `Enemies`, `Projectiles`, `Effects`

**Risultato atteso:** il progetto è pronto per essere sviluppato senza file e scene disorganizzati.

---

## Milestone 1 — Player e camera 🧙

**Obiettivo:** rendere il mago piacevole da controllare.

- [X] Implementare il movimento top-down
- [X] Supportare il movimento in 8 direzioni
- [X] Aggiungere collisioni con muri e ostacoli
- [X] Importare e configurare lo sprite del mago
- [X] Creare l'animazione Idle
- [?] Creare l'animazione Walk
- [X] Gestire correttamente orientamento e direzione del personaggio
- [X] Fare in modo che la camera segua il player
- [X] Limitare la camera ai confini della mappa

**Test di completamento:** il giocatore può muoversi liberamente in una stanza, non attraversa i muri e la camera lo segue correttamente.

---

## Milestone 2 — Sistema di combattimento 🔥

**Obiettivo:** creare un sistema di magie semplice, leggibile e riutilizzabile.

### Fireball — Attacco standard

- [X] Configurare l'input di attacco
- [X] Calcolare la direzione di mira
- [X] Creare e istanziare il proiettile
- [X] Implementare il movimento del proiettile
- [X] Gestire le collisioni
- [X] Infliggere danno
- [X] Distruggere il proiettile dopo l'impatto o dopo un tempo massimo
- [X] Aggiungere un cooldown
- [X] Aggiungere un effetto visivo basilare

### Ice Bolt — Controllo

- [X] Infliggere danno
- [X] Rallentare temporaneamente il nemico
- [X] Configurare un cooldown differente
- [X] Rendere evidente l'effetto di rallentamento

### Arcane Blast — Attacco Speciale

- [X] Creare un attacco ad area
- [X] Definire un raggio limitato
- [X] Colpire più nemici contemporaneamente
- [X] Configurare un cooldown lungo
- [X] Aggiungere un feedback visivo chiaro per l'area colpita
- [X] I nemici colpiti vengono sbalzati via violentemente, quindi se sono in quell area vengono spinti nella direzione opposta alla loro

**Ruolo delle magie:**

```text
Fireball     → attacco standard
Ice Bolt     → controllo e rallentamento
Arcane Blast → attacco speciale
```

---

## Milestone 3 — Nemici 👹

**Obiettivo:** costruire prima un nemico completo, poi riutilizzare il sistema per creare più archetipi.

### Sistema base

- [X] Aggiungere punti vita
- [X] Gestire la ricezione del danno
- [X] Gestire la morte
- [X] Implementare il movimento
- [X] Rilevare il player
- [X] Inseguire il player
- [X] Attaccare il player
- [X] Infliggere danno al player
- [X] Trasformare il nemico funzionante in un Prefab

### Archetipi

- [X] **Goblin:** combattimento melee, veloce e debole
- [X] **Skeleton Archer:** combattimento a distanza
- [X] **Orc:** lento, resistente e molto dannoso

**Risultato atteso:** tutti i nemici condividono i sistemi fondamentali, ma possiedono statistiche e comportamenti distinti.

---

## Milestone 4 — Vita del player e Game Over ❤️

**Obiettivo:** completare il ciclo base di rischio, sconfitta e nuovo tentativo.

- [X] Implementare gli HP del player
- [X] Creare una barra della vita
- [X] Aggiungere una breve invulnerabilità dopo aver subito un colpo
- [X] Mostrare un feedback quando il player viene colpito
- [X] Gestire la morte del player
- [X] Creare la schermata Game Over
- [X] Aggiungere il pulsante per ricominciare il livello

**Gameplay loop ottenuto:**

```text
Esplora → combatti → elimina i nemici → sopravvivi → completa il livello
                                      ↘ muori → riprova
```

---

## Milestone 5 — World building e Tilemap 🗺️

**Obiettivo:** costruire ambienti top-down nello stile visivo scelto.

- [X] Imparare a usare il sistema Tilemap
- [X] Creare una Tile Palette
- [X] Disegnare il terreno
- [X] Posizionare muri
- [X] Aggiungere ostacoli
- [X] Aggiungere decorazioni
- [X] Configurare le collisioni della Tilemap
- [X] Creare livelli più grandi della schermata
- [X] Gestire entrata e uscita dal livello
- [X] Costruire una piccola arena di test prima dei livelli definitivi

**Risultato atteso:** è possibile creare velocemente mappe esplorabili e modificarle senza ricostruire manualmente ogni elemento.

---

## Milestone 6 — Livello 1: Forest Ruins 🌲

**Ruolo:** tutorial naturale.

**Nemici:** Goblin  
**Magia disponibile:** Fireball  
**Durata target:** 3–5 minuti

- [X] Costruire una mappa semplice e leggibile
- [X] Inserire 3–5 Goblin
- [X] Introdurre gradualmente movimento e combattimento
- [X] Insegnare al giocatore a mirare e spararegit 
- [X] Insegnare a evitare gli attacchi
- [X] Creare un obiettivo o un'uscita di fine livello
- [X] Testare il livello dall'inizio alla fine

**Progressione appresa dal giocatore:**

```text
Muoversi → mirare → sparare → evitare gli attacchi → completare il livello
```

---




# Milestone aggiuntive

## Milestone 7 — Livello 2: Haunted Graveyard 💀

**Ruolo:** introdurre gli attacchi a distanza e il controllo dei nemici.

**Nemici:** Goblin e Skeleton Archer  
**Nuova magia:** Ice Bolt  
**Durata target:** 5–7 minuti

- [ ] Costruire una mappa leggermente più complessa
- [ ] Introdurre lo Skeleton Archer
- [ ] Combinare nemici melee e ranged
- [ ] Sbloccare Ice Bolt
- [ ] Creare situazioni in cui il rallentamento sia utile
- [ ] Aumentare gradualmente il numero di nemici
- [ ] Testare difficoltà e leggibilità degli attacchi
- [ ] Testare il livello dall'inizio alla fine

---

## Milestone 8 — Livello 3: Cursed Castle 🏰

**Ruolo:** prova finale con tutti i sistemi e boss conclusivo.

**Nemici:** Goblin, Skeleton Archer e Orc  
**Nuova magia:** Arcane Blast  
**Durata target:** 7–10 minuti

- [ ] Costruire la mappa più impegnativa del gioco
- [ ] Introdurre l'Orc
- [ ] Combinare tutti e tre gli archetipi di nemici
- [ ] Sbloccare Arcane Blast
- [ ] Creare combattimenti in cui l'attacco ad area sia utile
- [ ] Preparare un'arena finale
- [ ] Inserire il boss
- [ ] Testare l'intero livello dall'inizio alla fine

### Boss finale 👹

Il boss deve restare semplice e leggibile.

- [ ] Inseguire il player
- [ ] Eseguire un attacco normale
- [ ] Eseguire un attacco speciale
- [ ] Mostrare chiaramente quando sta per attaccare
- [ ] Avere una barra della vita dedicata
- [ ] Attivare la conclusione del gioco alla sua sconfitta

---

## Milestone 9 — Sistema di punteggio e stelle ⭐

**Obiettivo:** valutare la prestazione del giocatore al termine di ogni livello.

### Dati registrati

- [ ] Numero di nemici eliminati
- [ ] Tempo impiegato
- [ ] Percentuale di vita rimasta

### Calcolo del punteggio

```text
Enemy Score     0–500
Time Bonus      0–250
Health Bonus    0–250
---------------------
Totale          0–1000
```

### Valutazione

```text
0–399       ★
400–549     ★★
550–699     ★★★
700–849     ★★★★
850–1000    ★★★★★
```

- [ ] Creare uno `ScoreManager`
- [ ] Registrare le uccisioni
- [ ] Avviare e fermare il timer del livello
- [ ] Calcolare la percentuale di vita rimasta
- [ ] Calcolare il punteggio finale
- [ ] Convertire il punteggio in stelle
- [ ] Creare la schermata dei risultati
- [ ] Aggiungere i pulsanti `Riprova` e `Livello successivo`

### Esempio di schermata finale

```text
LIVELLO COMPLETATO

Nemici sconfitti:  14
Tempo:              04:37
Vita:               73%

PUNTEGGIO
872

★★★★★

[RIPROVA]     [LIVELLO SUCCESSIVO]
```

---

## Milestone 10 — Menu e progressione 📜

**Obiettivo:** collegare i livelli e conservare i progressi del giocatore.

- [ ] Completare il Main Menu
- [ ] Aggiungere il pulsante `Gioca`
- [ ] Creare la schermata di selezione dei livelli
- [ ] Aggiungere opzioni basilari
- [ ] Aggiungere il pulsante `Esci`
- [ ] Gestire livelli bloccati e sbloccati
- [ ] Memorizzare il miglior punteggio di ogni livello
- [ ] Memorizzare il massimo numero di stelle di ogni livello
- [ ] Implementare il salvataggio locale
- [ ] Sbloccare il Livello 2 completando il Livello 1
- [ ] Sbloccare il Livello 3 completando il Livello 2
- [ ] Permettere di rigiocare i livelli già completati

### Esempio di selezione livelli

```text
FOREST RUINS
★★★★★
MIGLIORE: 913

HAUNTED GRAVEYARD
★★★☆☆
MIGLIORE: 684

CURSED CASTLE
🔒
```

---

## Milestone 11 — Game feel e rifinitura ✨

**Obiettivo:** migliorare la sensazione di gioco dopo aver completato tutte le funzionalità principali.

- [ ] Migliorare le animazioni
- [ ] Aggiungere particelle alle magie
- [ ] Far lampeggiare un nemico quando viene colpito
- [ ] Aggiungere un leggero screen shake agli impatti importanti
- [ ] Creare effetti di morte
- [ ] Aggiungere animazioni all'interfaccia
- [ ] Inserire musica ambientale
- [ ] Inserire suoni per le magie
- [ ] Inserire suoni per i nemici
- [ ] Inserire un suono di impatto
- [ ] Inserire un suono di morte
- [ ] Inserire una musica dedicata al boss
- [ ] Creare transizioni tra le scene

**Obiettivo sensoriale:** trasformare la Fireball da semplice proiettile visibile a un attacco con lancio, impatto, particelle e reazione del bersaglio.

---

## Milestone 12 — Testing e build finale 🛠️

**Obiettivo:** completare, verificare e distribuire una versione giocabile del progetto.

- [ ] Giocare dall'inizio alla fine senza usare scorciatoie di debug
- [ ] Correggere i bug
- [ ] Bilanciare gli HP dei nemici
- [ ] Bilanciare i danni
- [ ] Bilanciare i cooldown delle magie
- [ ] Bilanciare le soglie del sistema stelle
- [ ] Controllare le prestazioni e gli FPS
- [ ] Testare diverse risoluzioni
- [ ] Creare una build Windows `.exe`
- [ ] Far provare il gioco a 2–3 persone
- [ ] Annotare i problemi osservati durante i test
- [ ] Correggere i problemi più importanti
- [ ] Eseguire un ultimo test completo della build
- [ ] Dichiarare il progetto concluso

---

## Percorso complessivo 🏁

```text
SETUP
  ↓
PLAYER + CAMERA
  ↓
COMBATTIMENTO
  ↓
ENEMY AI
  ↓
HEALTH + GAME OVER
  ↓
TILEMAP + WORLD BUILDING
  ↓
LIVELLO 1
  ↓
LIVELLO 2
  ↓
LIVELLO 3 + BOSS
  ↓
PUNTEGGIO + ★★★★★
  ↓
MENU + SALVATAGGIO
  ↓
POLISH
  ↓
TEST
  ↓
BUILD .EXE
  ↓
🎉 PROGETTO FINITO
```

---
