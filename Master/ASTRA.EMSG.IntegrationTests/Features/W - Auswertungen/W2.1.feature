Funktionalität: W2.1 - Eine Tabelle mit Wiederbeschaffungswert und Wertverlust gegliedert nach Belastungskategorien erhalten
    Als Data-Reader,
	will ich eine Tabelle mit Wiederbeschaffungswert und Wertverlust gegliedert nach Belastungskategorien erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Wiederbeschaffungswert und jährlichen Wertverlust erhalte

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich habe alle Rollen für 'Mandant_1'
Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
    | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Belag   |
    | 1  | AsphaltII    | Bahnhof        | Spital         | IA                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    | Asphalt |
    | 2  | AsphaltIV    |                |                | IV                  | 19             | 5000  | Links            | 2,32                | 0                    | Asphalt |
    | 3  | AsphaltIII   |                |                | III                 | 20             | 2500  | Rechts           | 0                   | 2,90                 | Asphalt |
    | 4  | BetonII      |                |                | II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    | Beton   |
    | 5  | BetonIC      |                |                | IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    | Beton   |
    | 6  | BetonIB      |                |                | IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    | Beton   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generiert werden soll
Gegeben sei es gibt Jahresabschlüsse für folgende Jahre:
	| Jahr |
	| 2008 |
	| 2009 |
Dann kann ich folgende Jahre für Tabelle mit Wiederbeschaffungswert und Wertverlust gegliedert nach Belastungskategorien auswählen:
	| Jahr                     |
	| Aktuelles Erfassungsjahr |
	| 2009                     |
	| 2008                     |
Und der Eintrag 'Aktuelles Erfassungsjahr' ist vorausgewählt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System generiert eine Vorschau der Auswertung am UI

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
@Ignore
Szenario: Das System liefert eine Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
Gegeben sei ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
	| BelastungskategorieTyp | FlächeFahrbahn | FlächeErfasstesTrottoir |
	| IA                     | 35208          | 0                       |
	| IB                     | 5148           | 0                       |
	| IC                     | 6760           | 0                       |
	| II                     | 23400          | 5720                    |
	| III                    | 50000          | 7250                    |
	| IV                     | 95000          | 11600                   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
@Ignore
Szenario: Im System gibt es für jede Belastungskategorie einen Alterungsbeiwert I 
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
	| BelastungskategorieTyp | AlterungsbeiwertI |
	| IA                     | 1,6               |
	| IB                     | 1,6               |
	| IC                     | 1,4               |
	| II                     | 1,8               |
	| III                    | 2,2               |
	| IV                     | 2,6               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
@Ignore
Szenario: Im System gibt es für jede Belastungskategorie einen Alterungsbeiwert II
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
	| BelastungskategorieTyp | AlterungsbeiwertII |
	| IA                     | 1,3                |
	| IB                     | 1,3                |
	| IC                     | 0,9                |
	| II                     | 1,4                |
	| III                    | 1,9                |
	| IV                     | 2,1                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet den Wiederbeschaffungswert
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Und es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
	| BelastungskategorieTyp | Wiederbeschaffungswert |
	| IA                     | 13379040               |
	| IB                     | 1647360                |
	| IC                     | 946400                 |
	| II                     | 7969000                |
	| III                    | 17906250               |
	| IV                     | 29834000               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet den Wertverlust
Gegeben sei es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Und es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Tabelle mit Wiederbeschaffungswert und Wertverlust nach Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann zeigt die Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie folgende Daten:  Tabelle mit Wiederbeschaffungswert und Wertverlust des Strassennetzes des Mandanten gegliedert nach Belastungskategorie
	| BelastungskategorieTyp | WertlustI | WertlustII |
	| IA                     | 214064,64 | 173927,52  |
	| IB                     | 26357,76  | 21415,68   |
	| IC                     | 13249,6   | 8517,6     |
	| II                     | 143442    | 111566     |
	| III                    | 393937,5  | 340218,75  |
	| IV                     | 775684    | 626514     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Tabelle auf den Strasseneigentümer filtern
 
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als Excel-File exportieren

