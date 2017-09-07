Funktionalität: W1.1 - Eine Grafik mit Mengen pro Belastungskategorie erhalten
    Als Data-Reader
    will ich eine Grafik mit Mengen pro Belastungskategorie erhalten
    damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Inventar erhalte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die gewünschte Auswertung selektieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader muss ein Jahr auswählen, für das vom System eine Inventarauswertung generiert werden soll

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Vom System wird das Jahr des letzten Jahresabschlusses als default Wert vorselektiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Das System liefert eine Grafik mit Mengen pro Belastungskategorie des Mandanten (summarischer Modus)
Gegeben sei folgende Einstellungen existieren:
    | Mandant   | Modus         | Mengentyp                 |
    | Mandant_1 | summarisch    | Gesamtlänge               |
    | Mandant_2 | summarisch    | Gesamtfläche              |
Und für Mandant 'Mandant_1' folgende summarische Zustands- und Netzinformationen existieren:
        | Belastungskategorie | Menge Gesamtfläche|
        | IA                  | 107760            |
        | IB                  | 5100              |
        | IC                  | 795               |
        | II                  | 2790000           |
        | III                 | 2051200           |
        | IV                  | 16200             |
Und für Mandant 'Mandant_2' folgende summarische Zustands- und Netzinformationen existieren:
        | Belastungskategorie | Menge Gesamtfläche |
        | IA                  | 7950               |
        | IB                  | 51000              |
        | IC                  | 1077600            |
        | II                  | 20512000           |
        | III                 | 27900000           |
        | IV                  | 162000             |
Und ich habe alle Rollen für '<Mandant>'    
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Grafik mit Mengen pro Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung '<Referenz Auswertung>'
Dann zeigt die Grafik folgende Verteilung: (manuell)
    | Mandant   | Mengenanteil IA   | Mengenanteil IB   | Mengenanteil IC   | Mengenanteil II   | Mengenanteil III   | Mengenanteil IV   |
    | <Mandant> | <Mengenanteil IA> | <Mengenanteil IB> | <Mengenanteil IC> | <Mengenanteil II> | <Mengenanteil III> | <Mengenanteil IV> |
Beispiele: 
    | TF | Referenz Auswertung      | Mandant   | Mengenanteil IA | Mengenanteil IB | Mengenanteil IC | Mengenanteil II | Mengenanteil III | Mengenanteil IV |
    | 1  | W1.1_Summarisch_Mandant1 | Mandant_1 | 2%              | 0%              | 0%              | 56%             | 41%              | 0%              |
    | 2  | W1.1_Summarisch_Mandant2 | Mandant_2 | 0%              | 0%              | 2%              | 41%             | 56%              | 0%              |

@Automatisch
Szenariogrundriss: Das System liefert eine Grafik mit Mengen pro Belastungskategorie des Mandanten (Strassennamen-Modus, GIS-Modus)
Gegeben sei folgende Einstellungen existieren:
    | Mandant   | Modus         |
    | Mandant_4 | strassennamen |
    | Mandant_5 | GIS           |
Und für Mandant 'Mandant_4' existieren folgende Netzinformationen:
    | Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | II                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    |
    | IV                  | 19             | 5000  | Links            | 2,32                | 0                    |
    | III                 | 20             | 2500  | Rechts           | 0                   | 2,90                 |
    | II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    |
    | IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    |
    | IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    |    
Und für Mandant 'Mandant_5' existieren folgende GIS Netzinformationen:
	| Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| II                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    |
	| III                 | 19             | 5000  | Links            | 2,32                | 0                    |
	| IV                  | 20             | 2500  | Rechts           | 0                   | 2,90                 |
	| II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    |
	| IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    |
	| IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    |
Und ich habe alle Rollen für '<Mandant>'    
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Grafik mit Mengen pro Belastungskategorie generiere
    | Filter            | Filter Wert |
    | Erfassungsperiode | 2010        |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung '<Referenz Auswertung>'
Dann zeigt die Grafik folgende Verteilung: (manuell)
    | Mandant   | Mengenanteil IA FB   | Mengenanteil IB FB   | Mengenanteil IC FB   | Mengenanteil II FB   | Mengenanteil III FB   | Mengenanteil IV FB   | Mengenanteil IA TR   | Mengenanteil IB TR   | Mengenanteil IC TR   | Mengenanteil II TR   | Mengenanteil III TR   | Mengenanteil IV TR   |
    | <Mandant> | <Mengenanteil IA FB> | <Mengenanteil IB FB> | <Mengenanteil IC FB> | <Mengenanteil II FB> | <Mengenanteil III FB> | <Mengenanteil IV FB> | <Mengenanteil IA TR> | <Mengenanteil IB TR> | <Mengenanteil IC TR> | <Mengenanteil II TR> | <Mengenanteil III TR> | <Mengenanteil IV TR> |

Beispiele: 
    | TF | Referenz Auswertung         | Mandant   | Strasseneigentümer | Mengenanteil IA FB | Mengenanteil IB FB | Mengenanteil IC FB | Mengenanteil II FB | Mengenanteil III FB | Mengenanteil IV FB | Mengenanteil IA TR | Mengenanteil IB TR | Mengenanteil IC TR | Mengenanteil II TR | Mengenanteil III TR | Mengenanteil IV TR |
    | 4  | W1.1_Strassennamen_Mandant4 | Mandant_4 |                    | 0%                 | 2%                 | 3%                 | 27%                | 23%                 | 44%                | 0%                 | 0%                 | 0%                 | 23%                | 30%                 | 47%                |
    | 5  | W1.1_Gis_Mandant5           | Mandant_5 |                    | 0%                 | 2%                 | 3%                 | 27%                | 44%                 | 23%                | 0%                 | 0%                 | 0%                 | 23%                | 47%                 | 30%                |
		   
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Die Grafik kann über einen Filter auf den Strasseneigentümer eingeschränkt werden
Gegeben sei folgende Einstellungen existieren:
    | Mandant   | Modus         |
    | Mandant_4 | strassennamen |
    | Mandant_5 | GIS			|
Und für Mandant 'Mandant_4' existieren folgende Netzinformationen:
    | Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
    | II                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    | Gemeinde           |
    | IV                  | 19             | 5000  | Links            | 2,32                | 0                    | Privat             |
    | III                 | 20             | 2500  | Rechts           | 0                   | 2,90                 | Korporation        |
    | II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    | Gemeinde           |
    | IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    | Privat             |
    | IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    | Korporation        |
Und für Mandant 'Mandant_5' existieren folgende GIS Netzinformationen:
	| Belastungskategorie | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
	| II                  | 19,56          | 1800  | KeinTrottoir     | 0                   | 0                    | Gemeinde           |
	| III                 | 19             | 5000  | Links            | 2,32                | 0                    | Korporation        |
	| IV                  | 20             | 2500  | Rechts           | 0                   | 2,90                 | Privat             |
	| II                  | 18             | 1300  | BeideSeiten      | 1,40                | 3                    | Gemeinde           |
	| IC                  | 8,45           | 800   | NochNichtErfasst | -                   | -                    | Privat             |
	| IB                  | 12,87          | 400   | KeinTrottoir     | 0                   | 0                    | Korporation        |
Und ich habe alle Rollen für '<Mandant>'    
Und ich einen Jahresabschluss für das Jahr '2010' durchführe
Wenn ich die Grafik mit Mengen pro Belastungskategorie generiere
    | Filter             | Filter Wert          |
    | Erfassungsperiode  | 2010                 |
    | Strasseneigentümer | <Strasseneigentümer> |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung '<Referenz Auswertung>'
Dann zeigt die Grafik folgende Verteilung: (manuell)
    | Mandant   | Strasseneigentümer   | Mengenanteil IA FB   | Mengenanteil IB FB   | Mengenanteil IC FB   | Mengenanteil II FB   | Mengenanteil III FB   | Mengenanteil IV FB   | Mengenanteil IA TR   | Mengenanteil IB TR   | Mengenanteil IC TR   | Mengenanteil II TR   | Mengenanteil III TR   | Mengenanteil IV TR   |
    | <Mandant> | <Strasseneigentümer> | <Mengenanteil IA FB> | <Mengenanteil IB FB> | <Mengenanteil IC FB> | <Mengenanteil II FB> | <Mengenanteil III FB> | <Mengenanteil IV FB> | <Mengenanteil IA TR> | <Mengenanteil IB TR> | <Mengenanteil IC TR> | <Mengenanteil II TR> | <Mengenanteil III TR> | <Mengenanteil IV TR> |

Beispiele: 
    | TF | Referenz Auswertung                     | Mandant   | Strasseneigentümer | Mengenanteil IA FB | Mengenanteil IB FB | Mengenanteil IC FB | Mengenanteil II FB | Mengenanteil III FB | Mengenanteil IV FB | Mengenanteil IA TR | Mengenanteil IB TR | Mengenanteil IC TR | Mengenanteil II TR | Mengenanteil III TR | Mengenanteil IV TR |
    | 1  | W1.1_Strassennamen_Mandant4_Gemeinde    | Mandant_4 | Gemeinde           | 0%                 | 0%                 | 0%                 | 100%               | 0%                  | 0%                 | 0%                 | 0%                 | 0%                 | 100%               | 0%                  | 0%                 |
    | 2  | W1.1_Strassennamen_Mandant4_Private     | Mandant_4 | Privat             | 0%                 | 0%                 | 7%                 | 0%                 | 0%                  | 93%                | 0%                 | 0%                 | 0%                 | 0%                 | 0%                  | 100%               |
    | 3  | W1.1_Strassennamen_Mandant4_Korporation | Mandant_4 | Korporation        | 0%                 | 9%                 | 0%                 | 0%                 | 91%                 | 0%                 | 0%                 | 0%                 | 0%                 | 0%                 | 100%                | 0%                 |
    | 4  | W1.1_Gis_Mandant5_Gemeinde              | Mandant_5 | Gemeinde           | 0%                 | 0%                 | 0%                 | 100%               | 0%                  | 0%                 | 0%                 | 0%                 | 0%                 | 100%               | 0%                  | 0%                 |
    | 5  | W1.1_Gis_Mandant5_Private               | Mandant_5 | Privat             | 0%                 | 0%                 | 12%                | 0%                 | 0%                  | 88%                | 0%                 | 0%                 | 0%                 | 0%                 | 0%                  | 100%               |
    | 6  | W1.1_Gis_Mandant5_Korporation           | Mandant_5 | Korporation        | 0%                 | 5%                 | 0%                 | 0%                 | 95%                 | 0%                 | 0%                 | 0%                 | 0%                 | 0%                 | 100%                | 0%                 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Grafik als PDF exportieren
