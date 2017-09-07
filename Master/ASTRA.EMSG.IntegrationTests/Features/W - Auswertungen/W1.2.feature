Funktionalität: W1.2 - Eine Liste der Strassenabschnitte erhalten
	Als Data-Reader
	will ich eine Liste der Strassenabschnitte erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Inventar erhalte

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
| Id | Strassenname   | Abschnittsnummer | BezeichnungVon | BezeichnungBis | Strasseneigentümer | Belastungskategorie | Trottoir           | Länge | Breite Fahrbahn | BreiteTrottoirLinks | BreiteTrottoirRechts |
| 1  | Kantonsstrasse | 1                | 0.2            | 2.0            | Gemeinde           | III                 | kein Trottoir      | 1800  | 19.56           | 0                   | 0                    |
| 2  | Kantonsstrasse | 2                | 2.0            | 7.0            | Gemeinde           | IV                  | links              | 5000  | 19.00           | 2.32                | 0                    |
| 3  | Landstrasse    | 1                | 0.0            | 2.5            | Privat             | III                 | rechts             | 2500  | 20.00           | 0                   | 1.9                  |
| 4  | Hauptstrasse   | 5                | 1.2            | 2.5            | Gemeinde           | II                  | beide              | 1300  | 15.00           | 1.4                 | 1.35                 |
| 5  | Hauptstrasse   | 4                | 0.4            | 1.2            | Korporation        | IC                  | Noch nicht erfasst | 800   | 8.45            |                     |                      |
| 6  | Hauptstrasse   | 1                | 0.0            | 0.1            | Korporation        | IB                  | kein Trottoir      | 100   | 12.87           | 0                   | 0                    |
| 7  | Hauptstrasse   | 2                | 0.1            | 0.2            | Korporation        | IB                  | rechts             | 100   | 12.87           | 0                   | 1.1                  |
| 8  | Hauptstrasse   | 3                | 0.2            | 0.4            | Korporation        | IB                  | links              | 200   | 12.87           | 1.2                 | 0                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die gewünschte Auswertung selektieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertung generiert werden soll

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselektiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System liefert eine Liste aller Strassenabschnitte des Mandanten
Wenn ich die Liste aller Strassenabschnitte des Mandanten generiere
Dann zeigt die Liste folgende Daten:
| Id | Strassenname   | kmVon | kmBis | Strasseneigentümer | Belastungskategorie | Troittoir          | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts |
| 1  | Kantonsstrasse | 0.2   | 2.0   | Gemeinde           | III                 | kein Trottoir      | 35208          | 0                   | 0                    | 
| 2  | Kantonsstrasse | 2.0   | 7.0   | Gemeinde           | IV                  | links              | 95000          | 11600               | 0                    | 
| 3  | Landstrasse    | 0.0   | 2.5   | Privat             | III                 | rechts             | 50000          | 0                   | 4750                 | 
| 4  | Hauptstrasse   | 1.2   | 2.5   | Gemeinde           | II                  | beide              | 19500          | 1820                | 1755                 | 
| 5  | Hauptstrasse   | 0.4   | 1.2   | Korporation        | IC                  | Noch nicht erfasst | 6760           | 0                   | 0                    | 
| 6  | Hauptstrasse   | 0.0   | 0.1   | Korporation        | IB                  | kein Trottoir      | 1287           | 0                   | 0                    |
| 7  | Hauptstrasse   | 0.1   | 0.2   | Korporation        | IB                  | rechts             | 1287           | 0                   | 110                  |
| 8  | Hauptstrasse   | 0.2   | 0.4   | Korporation        | IB                  | links              | 2574           | 240                 | 0                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Liste ist nach Strassenname und Abschnittsnummer sortiert
Wenn ich die Liste aller Strassenabschnitte des Mandanten generiere
Dann zeigt die Liste folgende Daten in dieser Reihenfolge:
| Id | Strassenname   | Abschnittsnummer |
| 6  | Hauptstrasse   | 1              |
| 7  | Hauptstrasse   | 2              |
| 8  | Hauptstrasse   | 3              |
| 5  | Hauptstrasse   | 4              |
| 4  | Hauptstrasse   | 5              |
| 1  | Kantonsstrasse | 1              |
| 2  | Kantonsstrasse | 2              |
| 3  | Landstrasse    | 1              |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Liste kann nach Strasseneigentümer gefiltert werden
Wenn ich die Liste aller Strassenabschnitte des Mandanten für Strasseneigentümer 'Gemeinde' generiere
Dann zeigt die Liste folgende Daten:
| Id | Strassenname   | kmVon | kmBis | Strasseneigentümer | Belastungskategorie | Troittoir          | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts |
| 1  | Kantonsstrasse | 0.2   | 2.0   | Gemeinde           | III                 | kein Trottoir      | 35208          | 0                   | 0                    | 
| 2  | Kantonsstrasse | 2.0   | 7.0   | Gemeinde           | IV                  | links              | 95000          | 11600               | 0                    | 
| 4  | Hauptstrasse   | 1.2   | 2.5   | Gemeinde           | II                  | beide              | 19500          | 1820                | 1755                 | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Liste als Excel-File exportieren