Funktionalität: W3.6 - Eine Liste mit Massnahmenvorschlag pro Zustandsabschnitt erhalten
    Als Data-Reader,
	will ich eine Liste mit Massnahmenvorschlag pro Zustandsabschnitt erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | Belastungskategorie | Belagsart | Eigentümer | Ortsbezeichnung | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechtss |
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | II                  | Asphalt   | Gemeinde   | 1. Bezirk       | 4,5            | Links            | 2,5                 | -                     |
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | II                  | Beton     | Gemeinde   | 1. Bezirk       | 5,75           | NochNichtErfasst | -                   | -                     |
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | II                  | Beton     | Gemeinde   | 1. Bezirk       | 7              | Keines           | -                   | -                     |
	| 4  | Gartenstrasse   | 1                | 66             | II                  | Asphalt   | Gemeinde   | 2. Bezirk       | 5              | BeideSeiten      | 2                   | 1,5                   |
Und folgende Zustandsinformationen existieren:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | Abschnittsnummer | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn                 | KostenFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | KostenTrottoirLinks | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | KostenTrottoirRechts | DringlichkeitTrottoirRechtsErneuerung |
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | 1                | Nr. 1          | Nr. 7          | 1000  | 12.14.2009    | Drei         | Nass    | 2,3                   | -                          | -                           | Oberflächenverbesserung                     | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | 2                | Nr. 8          | Nr. 12         | 800   | 12.14.2009    | Drei         | Nass    | 2,3                   | -                          | -                           | Deckbelagserneuerung                        | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | 3                | Nr. 13         | Nr. 55         | 5000  | 23.03.2009    | A            | Trocken | 1,2                   | Gut                        | -                           | Belagserneuerung mit teilweiser Verstärkung | 100000         | mittelfristig                            | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 1                | 0.0            | 2.1            | 2100  | 21.05.2009    | B            | Trocken | 1,1                   | -                          | -                           | Erneuerung Oberbau                          | 100000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 2                | 2.1            | 5.3            | 300   | 21.05.2009    | B            | Trocken | 1,1                   | -                          | -                           | Erneuerung Oberbau                          | 200000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 3                | 5.4            | 7.1            | 100   | 21.05.2009    | B            | Trocken | 2,1                   | -                          | -                           | Erneuerung Oberbau                          | 200000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | 1                | Brunner        | Maier          | 1300  | 21.10.2009    | Meyer        | Trocken | 3,4                   | -                          | -                           | -                                           | 300000         | -                                        | -                       | -                   | -                                    | -                        | -                    | -                                     |
	| 12 | Gartenstrasse   | 1                     | 66                    | 1                | 1              | 66             | 700   | 12.01.2010    | -            | Trocken | 3                     | Ausreichend                | Mittel                      | -                                           | 300000         | -                                        | Ja                      | 100000              | dringlich                            | Ja                       | 50000                | langfristig                           |
	
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
Szenario: Das System liefert eine Liste mit Massnahmenvorschlag pro Zustandsabschnitt des Mandanten
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der Liste mit Zustand pro Zustandsabschnitt  mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | Dringlichkeit | Zustand |
	| Alle       |              |               |         |
Dann erhalte ich folgende Liste mit Massnahmenvorschlag pro Zustandsabschnitt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Eigentümer | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn                 | KostenFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | KostenTrottoirLinks | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | KostenTrottoirRechts | DringlichkeitTrottoirRechtsErneuerung | Abschnittsnummer |
	| 11 | Föhrenweg       | Brunner        | Maier          | Gemeinde   | 3,4                   | -                          | -                           | -                                           | 300000         | -                                        | -                       | -                   | -                                    | -                        | -                    | -                                     | 1                |
	| 12 | Gartenstrasse   | 1              | 66             | Gemeinde   | 3                     | Ausreichend                | Mittel                      | -                                           | 300000         | -                                        | Ja                      | 100000              | dringlich                            | Ja                       | 50000                | langfristig                           | 1                |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | Gemeinde   | 2,3                   | -                          | -                           | Oberflächenverbesserung                     | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     | 1                |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | Gemeinde   | 2,3                   | -                          | -                           | Deckbelagserneuerung                        | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     | 2                |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | Gemeinde   | 1,2                   | Gut                        | -                           | Belagserneuerung mit teilweiser Verstärkung | 100000         | mittelfristig                            | -                       | -                   | -                                    | -                        | -                    | -                                     | 3                |
	| 8  | Lagerstrasse    | 0.0            | 2.1            | Gemeinde   | 1,1                   | -                          | -                           | Erneuerung Oberbau                          | 100000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     | 1                |
	| 9  | Lagerstrasse    | 2.1            | 5.3            | Gemeinde   | 1,1                   | -                          | -                           | Erneuerung Oberbau                          | 200000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     | 2                |
	| 10 | Lagerstrasse    | 5.4            | 7.1            | Gemeinde   | 2,1                   | -                          | -                           | Erneuerung Oberbau                          | 200000         | langfristig                              | -                       | -                   | -                                    | -                        | -                    | -                                     | 3                |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Liste ist nach Strassennamen sortiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Liste filtern
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der Liste mit Zustand pro Zustandsabschnitt  mit folgenden Filtern generiere:
	| Eigentümer | Strassenname    | Dringlichkeit | Zustand |
	| Alle       | Jesuitenbachweg | dringlich     |         |
Dann erhalte ich folgende Liste mit Massnahmenvorschlag pro Zustandsabschnitt:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Eigentümer | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn                 | KostenFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | KostenTrottoirLinks | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | KostenTrottoirRechts | DringlichkeitTrottoirRechtsErneuerung | Abschnittsnummer |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | Gemeinde   | 2,3                   | -                          | -                           | Oberflächenverbesserung                     | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     | 1                |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | Gemeinde   | 2,3                   | -                          | -                           | Deckbelagserneuerung                        | 200000         | dringlich                                | -                       | -                   | -                                    | -                        | -                    | -                                     | 2                |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau mit Zustand pro Zustandsabschnitt als Excel-File exportieren