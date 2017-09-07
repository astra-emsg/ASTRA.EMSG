Funktionalität: W3.8 - Eine Karte mit Zustandsabschnitten, symbolisiert nach Massnahmenvorschlag erhalten
	Als Data-Reader
	will ich eine Karte mit Zustandsabschnitten, symbolisiert nach Massnahmenvorschlag erhalten
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
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | ZustandsIndexFahrbahn | MassnahmenvorschlagFahrbahn                 | DringlichkeitMassnahmenvorschlagFahrbahn |
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | 2,3                   | Oberflächenverbesserung                     | langfristig                              |
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | 2,3                   | Deckbelagserneuerung                        | dringlich                                |
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | 1,2                   | Belagserneuerung mit teilweiser Verstärkung | mittelfristig                            |
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 1,1                   | Erneuerung Oberbau                          | langfristig                              |
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 1,1                   | Erneuerung Oberbau                          | langfristig                              |
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 2,1                   | Erneuerung Oberbau                          | langfristig                              |
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | 3,4                   | -                                           | -                                        |
	| 12 | Gartenstrasse   | 1                     | 66                    | 3                     | -                                           | -                                        |

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
Szenario: Der Data-Reader sieht auf der Karte die Zustandsabschnitte symbolisiert nach den Massnahmenvorschlägen
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Karte mit Zustandsabschnitten, symbolisiert nach Massnahmenvorschlag mit folgenden Filtern generiere:
	| Eigentümer | Dringlichkeit | Strassenname | Zustand |
	| Alle       |               |              |         |
Dann werden die Zustandsabschnitte in folgenden Farben angezeigt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Farbe    |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | hellgrün |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | hellblau |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | magenta  |
	| 8  | Lagerstrasse    | 0.0            | 2.1            | orange   |
	| 9  | Lagerstrasse    | 2.1            | 5.3            | orange   |
	| 10 | Lagerstrasse    | 5.4            | 7.1            | orange   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann aktuelle Ansicht filtern
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Karte mit Zustandsabschnitten, symbolisiert nach Massnahmenvorschlag mit folgenden Filtern generiere:
	| Eigentümer | Dringlichkeit | Strassenname    | Zustand |
	| Alle       | langfristig   | Jesuitenbachweg | mittel  |
Dann werden die Zustandsabschnitte in folgenden Farben angezeigt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Farbe    |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | hellgrün |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als PDF exportieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann mit der Bearbeitung und Navigation fortfahren sobald das PDF erzeugt wurde