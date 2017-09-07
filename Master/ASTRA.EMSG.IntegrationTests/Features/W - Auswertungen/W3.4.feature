Funktionalität: W3.4 - Eine Liste der noch nicht inspizierten Strassenabschnitte erhalten
    Als Data-Reader,
	will ich eine Liste der noch nicht inspizierten Strassenabschnitte erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | Belagsart | Strasseneigentümer | Belastungskategorie | Länge | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechtss |
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | Asphalt   | Gemeinde           | II                  | 100   | 10             | Links            | 2,5                 | -                     |
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | Beton     | Gemeinde           | II                  | 100   | 10             | NochNichtErfasst | -                   | -                     |
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | Beton     | Gemeinde           | II                  | 100   | 10             | Keines           | -                   | -                     |
	| 4  | Gartenstrasse   | 1                | 66             | Asphalt   | Gemeinde           | II                  | 100   | 10             | BeideSeiten      | 2                   | 1,5                   |
Und folgende Zustandsinformationen existieren:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Aufnahmedatum | ZustandsIndexFahrbahn | Erfassungsmodus | Länge |
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 1          | Nr. 7          | 12.10.2009    | 2,3                   | Detailliert     | 30    |
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 8          | Nr. 12         | 12.10.2009    | 2,3                   | Detailliert     | 30    |
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 13         | Nr. 55         | -		     | -                     | -			   | 40    |
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 0.0            | 2.1            | 21.05.2009    | 1,1                   | Detailliert     | 30    |
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 2.1            | 5.3            | 21.05.2009    | 1,1                   | Detailliert     | 30    |
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 5.4            | 7.1            | 21.05.2009    | 2,1                   | Zustandsindex   | 30    |
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | Brunner        | Maier          | 21.10.2009    | 3,4                   | Grob            | 100   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generiert werden soll
Gegeben sei es gibt Jahresabschlüsse für folgende Jahre:
	| Jahr |
	| 2008 |
	| 2009 |
Dann kann ich folgende Jahre für ausgefüllte Erfassungsformulare auswählen:
	| Jahr                     |
	| Aktuelles Erfassungsjahr |
	| 2009                     |
	| 2008                     |
Und der Eintrag 'Aktuelles Erfassungsjahr' ist vorausgewählt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System generiert eine Vorschau der Auswertung am UI

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System liefert eine Liste der noch nicht inspizierten Strassenabschnitte des Mandanten
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname |
	| Alle       |              |
Dann erhalte ich folgende Liste der noch nicht inspizierten Strassenabschnitte:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Strasseneigentümer | Belastungskategorie | FlächeFahrbahn | FlächeErfasstesTrottoirLinks | FlächeErfasstesTrottoirRechts |
	| 4  | Gartenstrasse   | 1              | 66             | Gemeinde           | II                  | 1000           | 200                          | 150                           |
	| 1  | Jesuitenbachweg | Hackl          | Schweighofer   | Gemeinde           | II                  | 1000           | 250                          | 0                             |
	| 2  | Lagerstrasse    | Nr. 13         | Nr. 22         | Gemeinde           | II                  | 1000           | 0                            | 0                             |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Liste ist nach Strassennamen sortiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Tabelle auf den Strassennamen und Strasseneigentümer filtern
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname  |
	| Alle       | Gartenstrasse |
Dann erhalte ich folgende Liste der noch nicht inspizierten Strassenabschnitte:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Strasseneigentümer | Belastungskategorie | FlächeFahrbahn | FlächeErfasstesTrottoirLinks | FlächeErfasstesTrottoirRechts |
	| 4  | Gartenstrasse   | 1              | 66             | Gemeinde           | II                  | 1000           | 200                          | 150                           |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau der noch nicht inspizierten Strassenabschnitte als Excel-File exportieren