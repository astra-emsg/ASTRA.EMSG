Funktionalität: W3.2 - Eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes Netz erhalten
    Als Data-Reader,
	will ich eine Grafik mit Zustandsspiegel, pro Belastungskategorie für mein ganzes Netz erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechtss | Belastungskategorie | Länge |
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | 4,5            | Links            | 2,5                 | -                     | IA                  | 10000 |
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | 5,75           | NochNichtErfasst | -                   | -                     | IB                  | 2400  |
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | 7              | Keines           | -                   | -                     | IC                  | 1300  |
	| 4  | Gartenstrasse   | 1                | 66             | 5              | BeideSeiten      | 2                   | 1,5                   | II                  | 700   |
Und folgende Zustandsinformationen existieren:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts |
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 1          | Nr. 7          | 1000  | 2,3                   | -                          | -                           |
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 8          | Nr. 12         | 800   | 2,3                   | -                          | -                           |
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 13         | Nr. 55         | 5000  | 1,2                   | Gut                        | -                           |
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 0.0            | 2.1            | 2100  | 1,1                   | -                          | -                           |
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 2.1            | 5.3            | 300   | 1,1                   | -                          | -                           |
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 5.4            | 7.1            | 100   | 2,1                   | -                          | -                           |
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | Brunner        | Maier          | 1300  | 3,4                   | -                          | -                           |
	| 12 | Gartenstrasse   | 1                     | 66                    | 1              | 66             | 700   | 3                     | Ausreichend                | Mittel                      |

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

# should be easy to automate
@Manuell
Szenario: Das System liefert eine Grafik mit Zustandsspiegel pro Belastungskategorie des Strassennetzes des Mandanten
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Grafik mit Zustandsspiegel pro Belastungskategorie für mein ganzes Netz generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung 'W3.2_Strassennamen_Mandant1'
Dann zeigen die Grafiken folgende Verteilung: (manuell)
	| Belastungskategorie | Zustandindex                | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts |
	| IA                  | 0.0 - 0.9                   | 0%             | 100%                | -                    |
	| IA                  | 1.0 - 1.9                   | 74%            | 0%                  | -                    |
	| IA                  | 2.0 - 2.9                   | 26%            | 0%                  | -                    |
	| IA                  | 3.0 - 3.9                   | 0%             | 0%                  | -                    |
	| IA                  | 4.0 - 5.0                   | 0%             | 0%                  | -                    |
	| IA                  | Anteil des erfassten Netzes | 100%           | 74%                 | -                    |
	| IB                  | 0.0 - 0.9                   | 0%             | -                   | -                    |
	| IB                  | 1.0 - 1.9                   | 96%            | -                   | -                    |
	| IB                  | 2.0 - 2.9                   | 4%             | -                   | -                    |
	| IB                  | 3.0 - 3.9                   | 0%             | -                   | -                    |
	| IB                  | 4.0 - 5.0                   | 0%             | -                   | -                    |
	| IB                  | Anteil des erfassten Netzes | 100%           |                     |                      |
	| IC                  | 0.0 - 0.9                   | 0%             | -                   | -                    |
	| IC                  | 1.0 - 1.9                   | 0%             | -                   | -                    |
	| IC                  | 2.0 - 2.9                   | 0%             | -                   | -                    |
	| IC                  | 3.0 - 3.9                   | 100%           | -                   | -                    |
	| IC                  | 4.0 - 5.0                   | 0%             | -                   | -                    |
	| IC                  | Anteil des erfassten Netzes | 100%           |                     |                      |
	| II                  | 0.0 - 0.9                   | 0%             | 0%                  | 0%                   |
	| II                  | 1.0 - 1.9                   | 0%             | 0%                  | 100%                 |
	| II                  | 2.0 - 2.9                   | 0%             | 100%                | 0%                   |
	| II                  | 3.0 - 3.9                   | 100%           | 0%                  | 0%                   |
	| II                  | 4.0 - 5.0                   | 0%             | 0%                  | 0%                   |
	| II                  | Anteil des erfassten Netzes | 100%           | 100%                | 100%                 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Graphik filtern

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als PDF exportieren