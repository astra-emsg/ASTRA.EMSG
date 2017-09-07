Funktionalität: W2.2 - Eine Tabelle mit Wiederbeschaffungswert und Wertverlust pro Strassenabschnitt erhalten
    Als Data-Reader,
	will ich eine Tabelle mit Wiederbeschaffungswert und Wertverlust pro Strassenabschnitt erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Wiederbeschaffungswert und jährlichen Wertverlust erhalte

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
    | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Belag   |
    | 1  | AsphaltII    | Bahnhof        | Spital         | II                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    | Asphalt |
    | 2  | AsphaltIV    |                |                | IV                  | 19             | 5000  | Links            | 2,32                | 0                    | Asphalt |
    | 3  | AsphaltIII   |                |                | III                 | 20             | 2500  | Rechts           | 0                   | 2,90                 | Asphalt |
    | 4  | BetonII      |                |                | II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    | Beton   |
    | 5  | BetonIC      |                |                | IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    | Beton   |
    | 6  | BetonIB      |                |                | IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    | Beton   |
Und ich habe alle Rollen für '<Mandant>'    

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
Szenario: Das System liefert eine Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte des Mandanten
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts | Trottoir         |
	| 1  | AsphaltII    | Bahnhof        | Spital         | II                  | 35208          | 0                   | 0                    | KeinTrottoir     |
	| 2  | AsphaltIV    |                |                | IV                  | 95000          | 11600               | -                    | Links            |
	| 3  | AsphaltIII   |                |                | III                 | 50000          | -                   | 7250                 | Rechts           |
	| 4  | BetonII      |                |                | II                  | 23400          | 1820                | 3900                 | BeideSeiten      |
	| 5  | BetonIC      |                |                | IC                  | 6760           | -                   | -                    | NochNichtErfasst |
	| 6  | BetonIB      |                |                | IB                  | 5148           | 0                   | 0                    | KeinTrottoir     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Im System gibt es für jede Belastungskategorie einen durchschnittlichen Wiederbeschaffungswert 
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | DurchschnittlicherWiederbeschaffungswert |
	| 1  | AsphaltII    | FB: 310 TR: 125                          |
	| 2  | AsphaltIV    | FB: 300 TR: 115                          |
	| 3  | AsphaltIII   | FB: 340 TR: 125                          |
	| 4  | BetonII      | FB: 310 TR: 125                          |
	| 5  | BetonIC      | FB: 140                                  |
	| 6  | BetonIB      | FB: 320 TR: 150                          |  

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Wurden vom Benutzeradministrator mandantenspezifische Wiederbeschaffungswerte definiert, werden diese für die Auswertungen herangezogen 
Gegeben sei es wurden vom Benutzeradministrator folgende mandantenspezifische Wiederbeschaffungswerte definiert:
	| Belastungskategorie | WiederbeschaffungswertFlächeFahrbahn | WiederbeschaffungswertFlächeTrottoir | WiederbeschaffungswertGesamtflächeFahrbahn | 
	| IB                  | 120                                  | 130                                  | 140                                        | 
	| IC                  | 130                                  | 140                                  | 150                                        | 
	| II                  | 200                                  | 210                                  | 220                                        | 
	| III                 | 300                                  | 310                                  | 320                                        | 
	| IV                  | 400                                  | 410                                  | 420                                        | 
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | DurchschnittlicherWiederbeschaffungswert |
	| 1  | AsphaltII    | FB: 200 TR: 210                          |
	| 2  | AsphaltIV    | FB: 400 TR: 410                          |
	| 3  | AsphaltIII   | FB: 300 TR: 310                          |
	| 4  | BetonII      | FB: 200 TR: 210                          |
	| 5  | BetonIC      | FB: 150                                  |
	| 6  | BetonIB      | FB: 120 TR: 130                          |
#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Im System gibt es für jede Belastungskategorie einen Alterungsbeiwert I 
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | AlterungsbeiwertI |
	| 1  | AsphaltII    | 1,8               |
	| 2  | AsphaltIV    | 2,6               |
	| 3  | AsphaltIII   | 2,2               |
	| 4  | BetonII      | 1,8               |
	| 5  | BetonIC      | 1,4               |
	| 6  | BetonIB      | 1,6               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Im System gibt es für jede Belastungskategorie einen Alterungsbeiwert II
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | AlterungsbeiwertII |
	| 1  | AsphaltII    | 1,4                |
	| 2  | AsphaltIV    | 2,1                |
	| 3  | AsphaltIII   | 1,9                |
	| 4  | BetonII      | 1,4                |
	| 5  | BetonIC      | 0,9                |
	| 6  | BetonIB      | 1,3                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das System berechnet den Wiederbeschaffungswert für den Strassenabschnitt
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Und es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | Wiederbeschaffungswert | 
	| 1  | AsphaltII    | 10914480               | 
	| 2  | AsphaltIV    | 29834000               | 
	| 3  | AsphaltIII   | 17906250               | 
	| 4  | BetonII      | 7969000                | 
	| 5  | BetonIC      | 946400                 | 
	| 6  | BetonIB      | 1647360                | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das System berechnet den Wertverlust für den definierten Mengentyp des Mandanten
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Und es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Tabelle mit Wiederbeschaffungswert und Wertverlust der Strassenabschnitte generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Liste folgende Daten:
	| Id | Strassenname | WertverlustI | WertverlustII |
	| 1  | AsphaltII    | 196460,64    | 207375,12     |
	| 2  | AsphaltIV    | 775684       | 626514        |
	| 3  | AsphaltIII   | 393937,5     | 340218,75     |
	| 4  | BetonII      | 143442       | 111566        |
	| 5  | BetonIC      | 13249,6      | 8517,6        |
	| 6  | BetonIB      | 26357,76     | 21415,68      |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Tabelle filtern
 
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als Excel-File exportieren

