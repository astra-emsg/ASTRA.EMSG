Funktionalität: W3.7 - Eine Karte mit Zustandsabschnitten, symbolisiert nach Zustand erhalten
	Als Data-Reader
	will ich eine Karte mit Zustandsabschnitten, symbolisiert nach Zustand erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus |
	| Mandant_1 | gis   |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | 
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | 
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | 
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | 
	| 4  | Gartenstrasse   | 1                | 66             | 
	
Und folgende Zustandsinformationen existieren:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | ZustandsIndexFahrbahn | 
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | 4,0                   | 
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | 2,3                   | 
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | 1,2                   | 
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 0,9                   | 
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 1,1                   | 
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 2,1                   | 
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | 3,4                   | 
	| 12 | Gartenstrasse   | 1                     | 66                    | -                     | 

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
Szenario: Zustandsabschnitte werden nach deren Zustand (Kategorien) farblich symbolisiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Karte mit Zustandsabschnitten, symbolisiert nach Zustand mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | Zustand |
	| Alle       |              |         |
Dann werden die Zustandsabschnitte in folgenden Farben angezeigt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Farbe      |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | rot        |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | gelb       |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | hellgrün   |
	| 8  | Lagerstrasse    | 0.0            | 2.1            | dunkelgrün |
	| 9  | Lagerstrasse    | 2.1            | 5.3            | hellgrün   |
	| 10 | Lagerstrasse    | 5.4            | 7.1            | gelb       |
	| 11 | Föhrenweg       | Brunner        | Maier          | orange     |
	| 12 | Gartenstrasse   | 1              | 66             | grau       |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die dargestellten Zustandsabschnitte nach deren Strasseneigentümer filtern und sieht dann auf der Karte die Zustandsabschnitte für die ausgewählten Eigentümer
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Karte mit Zustandsabschnitten, symbolisiert nach Zustand mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | Zustand |
	| Gemeinde   |              |         |
Dann werden die Zustandsabschnitte in folgenden Farben angezeigt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Farbe      |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | rot        |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | gelb       |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | hellgrün   |
	| 11 | Föhrenweg       | Brunner        | Maier          | orange     |
	| 12 | Gartenstrasse   | 1              | 66             | grau       |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die dargestellten Zustandsabschnitte auf den Strassennamen, Strasseneigentümer, Zustand  filtern
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Karte mit Zustandsabschnitten, symbolisiert nach Zustand mit folgenden Filtern generiere:
	| Eigentümer | Strassenname    | Zustand |
	| Alle       | Jesuitenbachweg | mittel  |
Dann werden die Zustandsabschnitte in folgenden Farben angezeigt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Farbe      |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | hellgrün   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als PDF exportieren