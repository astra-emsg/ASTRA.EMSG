Funktionalität: W1.7 - Eine Tabelle mit Menge pro Belastungskategorie erhalten
	Als Data-Reader
	will ich eine Tabelle mit Menge pro Belastungskategorie erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Inventar erhalte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die gewünschte Auswertung selektieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann ein Jahr auswählen, für das vom System eine Inventarauswertung generiert werden soll

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Vom System wird das Jahr des letzten Jahresabschlusses als default Wert vorselektiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Das System liefert eine Tabelle mit Mengen pro Belastungskategorie des Mandanten (summarischer Modus)
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         | Mengentyp                 |
	| Mandant_1 | summarisch    | Gesamtlänge               |
	| Mandant_2 | summarisch    | Gesamtfläche              |
Und für Mandant 'Mandant_1' folgende summarische Zustands- und Netzinformationen existieren:
    | Belastungskategorie | Menge Gesamtfläche |
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
Wenn ich die Tabelle mit Mengen pro Belastungskategorie generiere
    | Filter             | Filter Wert |
    | Erfassungsperiode  | 2010        |
Und zeigt die Tabelle mit Mengen pro Belastungskategorie folgende Mengen:
	| BelastungskategorieTyp | Menge        |
	| IA                     | <Mengen IA>  |
	| IB                     | <Mengen IB>  |
	| IC                     | <Mengen IC>  |
	| II                     | <Mengen II>  |
	| III                    | <Mengen III> |
	| IV                     | <Mengen IV>  |	

Beispiele: 
	| TF | Mandant   | Mengen IA | Mengen IB | Mengen IC | Mengen II | Mengen III | Mengen IV | Summe    |
	| 1  | Mandant_1 | 107760    | 5100      | 795       | 2790000   | 2051200    | 16200     | 613975   |
	| 2  | Mandant_2 | 7950      | 51000     | 1077600   | 20512000  | 27900000   | 162000    | 49710550 |

@Automatisch
Szenariogrundriss: Das System liefert eine Tabelle mit Mengen pro Belastungskategorie des Mandanten (Strassennamen-Modus, GIS-Modus)
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
Wenn ich die Tabelle mit Mengen pro Belastungskategorie generiere
    | Filter             | Filter Wert |
    | Erfassungsperiode  | 2010        |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung '<Referenz Auswertung>'
Und zeigt die Tabelle folgende Mengen: (manuell)
	| Mandant   | Mengen IA FB   | Mengen IB FB   | Mengen IC FB   | Mengen II FB   | Mengen III FB   | Mengen IV FB   | Mengen IA TR   | Mengen IB TR   | Mengen IC TR   | Mengen II TR   | Mengen III TR   | Mengen IV TR   |
	| <Mandant> | <Mengen IA FB> | <Mengen IB FB> | <Mengen IC FB> | <Mengen II FB> | <Mengen III FB> | <Mengen IV FB> | <Mengen IA TR> | <Mengen IB TR> | <Mengen IC TR> | <Mengen II TR> | <Mengen III TR> | <Mengen IV TR> |

Beispiele:
	| TF | Referenz Auswertung         | Mandant   | Strasseneigentümer | Mengen IA FB | Mengen IB FB | Mengen IC FB | Mengen II FB | Mengen III FB | Mengen IV FB | Mengen IA TR | Mengen IB TR | Mengen IC TR | Mengen II TR | Mengen III TR | Mengen IV TR | Summe FB | Summe TR |
	| 4  | W1.7_Strassennamen_Mandant4 | Mandant_4 |                    | 0            | 5148         | 6760         | 58608        | 50000         | 95000        | 0            | 0            | 0            | 5720         | 7250          | 11600        | 215516   | 24570    |
	| 5  | W1.7_Gis_Mandant5           | Mandant_5 |                    | 0            | 5148         | 6760         | 58608        | 95000         | 50000        | 0            | 0            | 0            | 5720         | 11600         | 7250         | 215516   | 24570    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Reader kann die Tabelle auf den Strasseneigentümer filtern (ausgenommen summarischer Modus – hier existiert keine Angabe zum Strasseneigentümer)
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
Wenn ich die Tabelle mit Mengen pro Belastungskategorie generiere
    | Filter             | Filter Wert          |
    | Erfassungsperiode  | 2010                 |
    | Strasseneigentümer | <Strasseneigentümer> |
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung '<Referenz Auswertung>'
Und zeigt die Tabelle folgende Mengen: (manuell)
	| Mandant   | Mengen IA   | Mengen IB   | Mengen IC   | Mengen II   | Mengen III   | Mengen IV   |
	| <Mandant> | <Mengen IA> | <Mengen IB> | <Mengen IC> | <Mengen II> | <Mengen III> | <Mengen IV> |

Beispiele: 
	| TF | Referenz Auswertung                     | Mandant   | Strasseneigentümer | Mengen IA FB | Mengen IB FB | Mengen IC FB | Mengen II FB | Mengen III FB | Mengen IV FB | Mengen IA TR | Mengen IB TR | Mengen IC TR | Mengen II TR | Mengen III TR | Mengen IV TR |
	| 1  | W1.7_Strassennamen_Mandant4_Gemeinde    | Mandant_4 | Gemeinde           | 0            | 0            | 0            | 58608        | 0             | 0            | 0            | 0            | 0            | 5720         | 0             | 0            |
	| 2  | W1.7_Strassennamen_Mandant4_Private     | Mandant_4 | Privat             | 0            | 0            | 6760         | 0            | 0             | 95000        | 0            | 0            | 0            | 0            | 0             | 11600        |
	| 3  | W1.7_Strassennamen_Mandant4_Korporation | Mandant_4 | Korporation        | 0            | 5148         | 0            | 0            | 50000         | 0            | 0            | 0            | 0            | 0            | 7250          | 0            |
	| 4  | W1.7_Gis_Mandant5_Gemeinde              | Mandant_5 | Gemeinde           | 0            | 0            | 0            | 58608        | 0             | 0            | 0            | 0            | 0            | 5720         | 0             | 0            |
	| 5  | W1.7_Gis_Mandant5_Private               | Mandant_5 | Privat             | 0            | 0            | 6760         | 0            | 0             | 95000        | 0            | 0            | 0            | 0            | 0             | 11600        |
	| 6  | W1.7_Gis_Mandant5_Korporation           | Mandant_5 | Korporation        | 0            | 5148         | 0            | 0            | 50000         | 0            | 0            | 0            | 0            | 0            | 7250          | 0            |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die Tabelle als Excel-File exportieren
