Funktionalität: W2.3 - Eine Grafik zum Wiederbeschaffungswert und Wertverlust erhalten
    Als Data-Reader,
	will ich eine Grafik zum Wiederbeschaffungswert und Wertverlust erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Wiederbeschaffungswert und jährlichen Wertverlust erhalte
	
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
Szenario: Das System liefert eine Grafik zum Wiederbeschaffungswert und jährlichen Wertverlust des Strassennetzes des Mandanten
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
Und es wurden vom Benutzeradministrator keine mandantenspezifische Wiederbeschaffungswerte definiert
Und es wurden vom Benutzeradministrator keine mandantenspezifische Alterungsbeiwerte definiert
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Grafik zum Wiederbeschaffungswert und jährlichen Wertverlust des Strassennetzes generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung 'W2.3_Strassennamen_Mandant1'
Dann zeigt die Grafik folgende Verteilung: (manuell)
	| Belastungskategorie | Wiederbeschaffungswert | WertverlustI | Wertverlust II |
	| IA                  | 0%                     | 0%           | 0              |
	| IB                  | 1,7%                   | 1,7%         | 1,6%           |
	| IC                  | 0,9%                   | 0,9%         | 0,6%           |
	| II                  | 21,9%                  | 21,9%        | 24,2%          |
	| III                 | 25,4%                  | 25,4%        | 25,9%          |
	| IV                  | 50,1%                  | 50,1%        | 47,6%          |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Grafik auf den Strasseneigentümer filtern
 
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Vorschau als PDF exportieren