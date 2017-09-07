Funktionalität: N3 - Das Strassennetz aus Excel-Files importieren
	Als Data-Manager
	will ich das Strassennetz aus Excel-Files importieren
	damit ich Daten aus anderen Systemen übernehmen kann

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Der Data-Manager kann neue Strassenabschnitte über ein Excel-File importieren
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| Kantonsstrasse | a              | b              |            |                  | Gemeinde           | Oberdorf        | II                  | Beton   | 19,56          | 1800,0 | KeinTrottoir |                     |                      |
	| Kantonsstrasse | c              | d              |            |                  | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19,00          | 5000,0 | Links        | 2,32                |                      |
Wenn ich die XLSX-Datei importiere
Dann sind folgende Netzinformationen im System:
	| Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| Kantonsstrasse | a              | b              | -          | -                | Gemeinde           | Oberdorf        | II                  | Beton   | 19,56          | 1800,0 | KeinTrottoir | -                   | -                    |
	| Kantonsstrasse | c              | d              | -          | -                | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19,00          | 5000,0 | Links        | 2,32                | -                    |
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Der Data-Manager kann Attribute von Strassenabschnitten über ein Excel-File bearbeiten
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
    | Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | b              | -			| KeinTrottoir     | -                   | -                    |
    | Landstrasse    | c              | d              | -			| Rechts           | -                   | 2,90                 |
    | Hauptstrasse   | e              | f              | -			| NochNichtErfasst | -                   | -                    |
Und eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
    | Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | b              |			|                  | Gemeinde           | Oberdorf        | II                  | Beton   | 19,56          | 1800,0 | Links        | 2,50                |                      |
    | Kantonsstrasse | x              | x              |			|                  | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19,00          | 5000,0 | Links        | 2,32                |                      |
    | Landstrasse    | c              | d              |			|                  | Privat             | Unterdorf       | III                 | Asphalt | 20,00          | 2500,0 | KeinTrottoir |                     |                      |
    | Hauptstrasse   | j              | j              |			|                  | Gemeinde           | Unterdorf       | II                  | Beton   | 18,00          | 1300,0 | BeideSeiten  | 1,40                | 3,00                 |
    | Hauptstrasse   | e              | f              |			|                  | Gemeinde           | Oberdorf        | IC                  | Beton   | 8,45           | 800,0  | Rechts       |                     | 3                    |
    | Hauptallee     | k              | k              |			|                  | Korporation        | Flussnähe       | IB                  | Asphalt | 12,87          | 400,0  | KeinTrottoir |                     |                      |
Wenn ich die XLSX-Datei importiere
Dann sind folgende Netzinformationen im System:
	| Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| Kantonsstrasse | a              | b              | -			| -                | Gemeinde           | Oberdorf        | II                  | Beton   | 19,56          | 1800  | Links        | 2,50                | -                    |
	| Kantonsstrasse | x              | x              | -			| -                | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19             | 5000  | Links        | 2,32                | -                    |
	| Landstrasse    | c              | d              | -			| -                | Privat             | Unterdorf       | III                 | Asphalt | 20             | 2500  | KeinTrottoir | -                   | -                    |
	| Hauptstrasse   | j              | j              | -			| -                | Gemeinde           | Unterdorf       | II                  | Beton   | 18             | 1300  | BeideSeiten  | 1,40                | 3                    |
	| Hauptstrasse   | e              | f              | -			| -                | Gemeinde           | Oberdorf        | IC                  | Beton   | 8,45           | 800   | Rechts       | -                   | 3                    |
	| Hauptallee     | k              | k              | -			| -                | Korporation        | Flussnähe       | IB                  | Asphalt | 12,87          | 400   | KeinTrottoir | -                   | -                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Struktur des Excel-Files ist flach
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und  eine XLSX-Datei mit folgenden Zeilen:
	| Tab | Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir      | Breite Trottoir links (m) | Breite Trottoir rechts (m) |
	| 1   | Kantonsstrasse | 0.2             | 2.0             |                  | Gemeinde           | Oberdorf        | II                  | Beton   | 19.56               | 1800.0          | kein Trottoir | 0                         | 0                          |
	| 1   | Kantonsstrasse | 2.0             | 7.0             |                  | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19.00               | 5000.0          | links         | 2.32                      | 0                          |
	| 2   | Landstrasse    | 0.0             | 2.5             |                  | Privat             | Unterdorf       | III                 | Asphalt | 20.00               | 2500.0          | rechts        | 0.00                      | 2.90                       |
	| 2   | Hauptstrasse   | 1.2             | 2.5             |                  | Gemeinde           | Unterdorf       | II                  | Beton   | 18.00               | 1300.0          | beide         | 1.40                      | 3.00                       |
Wenn ich die XLSX-Datei importiere
Dann sind folgende Netzinformationen im System:
	| Strassenname   | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir           | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	| Kantonsstrasse | 0.2            | 2.0            | II                  | Beton   | 19,56          | 1800  | kein Trottoir      | 0                   | 0                    | Gemeinde           | Oberdorf        |                  |
	| Kantonsstrasse | 2.0            | 7.0            | IV                  | Asphalt | 19             | 5000  | links              | 2,32                | 0                    | Gemeinde           | Unterdorf       |                  |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System führt vor dem Import Validierungen durch (Kolonnenanzahl - zusätzliche Kolonne)
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und  eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir           | Breite Trottoir links (m) | Breite Trottoir rechts (m) | Winterdienst auszuführen |
	| Kantonsstrasse | 0.2             | 2.0             | 1                | Gemeinde           | Oberdorf        | II                  | Beton   | 19.56               | 1800.0          | kein Trottoir      | 0                         | 0                          | Ja                       |
	| Kantonsstrasse | 2.0             | 7.0             | 2                | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19.00               | 5000.0          | links              | 2.32                      | 0                          | Ja                       |
	| Landstrasse    | 0.0             | 2.5             | 1                | Privat             | Unterdorf       | III                 | Asphalt | 20.00               | 2500.0          | rechts             | 0.00                      | 2.90                       | Teilw.                   |
	| Hauptstrasse   | 1.2             | 2.5             | 1                | Gemeinde           | Unterdorf       | II                  | Beton   | 18.00               | 1300.0          | beide              | 1.40                      | 3.00                       | Teilw.                   |
	| Hauptstrasse   | 0.4             | 1.2             | 2                | Gemeinde           | Oberdorf        | IC                  | Beton   | 8.45                | 800.0           | Noch nicht erfasst |                           |                            | Nein                     |
	| Hauptstrasse   | 0.0             | 0.4             | 3                | Korporation        | Flussnähe       | IB                  | Asphalt | 12.87               | 400.0           | kein Trottoir      | 0                         | 0                          | Ja                       |
Wenn ich die XLSX-Datei importiere
Dann liefert der Import einen Validationsfehler

@Manuell
Szenario: Das System führt vor dem Import Validierungen durch (Kolonnenanzahl - fehlende Kolonne)
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und  eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir           | Breite Trottoir links (m) | 
	| Kantonsstrasse | 0.2             | 2.0             | 1                | Gemeinde           | Oberdorf        | II                  | Beton   | 19.56               | 1800.0          | kein Trottoir      | 0                         | 
	| Kantonsstrasse | 2.0             | 7.0             | 2                | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19.00               | 5000.0          | links              | 2.32                      | 
	| Landstrasse    | 0.0             | 2.5             | 1                | Privat             | Unterdorf       | III                 | Asphalt | 20.00               | 2500.0          | rechts             | 0.00                      | 
	| Hauptstrasse   | 1.2             | 2.5             | 1                | Gemeinde           | Unterdorf       | II                  | Beton   | 18.00               | 1300.0          | beide              | 1.40                      | 
	| Hauptstrasse   | 0.4             | 1.2             | 2                | Gemeinde           | Oberdorf        | IC                  | Beton   | 8.45                | 800.0           | Noch nicht erfasst |                           | 
	| Hauptstrasse   | 0.0             | 0.4             | 3                | Korporation        | Flussnähe       | IB                  | Asphalt | 12.87               | 400.0           | kein Trottoir      | 0                         | 
Wenn ich die XLSX-Datei importiere
Dann liefert der Import einen Validationsfehler

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System führt vor dem Import Validierungen durch (Verletzung Business Logik)
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

# Zwei Zeilen ohne Belastungskategorie und ein Tippfehler in "Gemeinde" (fehlendes 'e')
Und  eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | länge  | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| Kantonsstrasse | a              | a              |			| 1                | Gemeinde           | Oberdorf        | II                  | Beton   | 19,56          | 1800,0 | KeinTrottoir     | 0                   | 0                    |
	| Kantonsstrasse | s              | s              |			| 2                | Gemeinde           | Unterdorf       | IV                  | Asphalt | 19,00          | 5000,0 | Links            | 2,32                | 0                    |
	| Landstrasse    | d              | d              |			| 1                | Privat             | Unterdorf       |                     | Asphalt | 20,00          | 2500,0 | Rechts           | 0,00                | 2,90                 |
	| Hauptstrasse   | f              | f              |			| 1                | Gemeinde           | Unterdorf       | II                  | Beton   | 18,00          | 1300,0 | BeideSeiten      | 1,40                | 3,00                 |
	| Hauptstrasse   | g              | g              |			| 2                | Gemeinde           | Oberdorf        |                     | Beton   | 8,45           | 800,0  | NochNichtErfasst |                     |                      |
	| Hauptstrasse   | h              | h              |			| 3                | Korporation        | Flussnähe       | IB                  | Asphalt | 12,87          | 400,0  | KeinTrottoir     | 0                   | 0                    |
	| Hauptstrasse   | j              | j              |			| 4                | Gemeind            | Flussnähe       | III                 | Asphalt | 20,00          | 300,0  | Rechts           | 0                   | 3                    |
Wenn ich die XLSX-Datei importiere
Dann liefert der Import drei Validationsfehler

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Wird eine Validierung verletzt, führt das System den Import nicht durch
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
    | Strassenname   | BezeichnungVon | BezeichnungBis | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | b              | KeinTrottoir     | 0                   | 0                    |
    | Landstrasse    | c              | d              | Rechts           | 0                   | 2,90                 |
    | Hauptstrasse   | e              | f              | NochNichtErfasst | -                   | -                    |

# Fehlender Belastungskategorien
Und eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
    | Strassenname   | BezeichnungVon | BezeichnungBis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | b              |                  | Gemeinde           | Oberdorf        |                     | Beton   | 19,56          | 1800,0 | Links        | 2,50                | 0                    |
    | Kantonsstrasse | x              | x              |                  | Gemeinde           | Unterdorf       |                     | Asphalt | 19,00          | 5000,0 | Links        | 2,32                | 0                    |
    | Landstrasse    | c              | d              |                  | Privat             | Unterdorf       |                     | Asphalt | 20,00          | 2500,0 | KeinTrottoir | 0                   | 0                    |
    | Hauptstrasse   | j              | j              |                  | Gemeinde           | Unterdorf       |                     | Beton   | 18,00          | 1300,0 | BeideSeiten  | 1,40                | 3,00                 |
    | Hauptstrasse   | e              | f              |                  | Gemeinde           | Oberdorf        |                     | Beton   | 8,45           | 800,0  | Rechts       | 0                   | 3                    |
    | Hauptallee     | k              | k              |                  | Korporation        | Flussnähe       |                     | Asphalt | 12,87          | 400,0  | KeinTrottoir | 0                   | 0                    |
Wenn ich die XLSX-Datei importiere
Dann sind folgende Netzinformationen im System:
    | Strassenname   | BezeichnungVon | BezeichnungBis | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | b              | KeinTrottoir     | 0                   | 0                    |
    | Landstrasse    | c              | d              | Rechts           | 0                   | 2,90                 |
    | Hauptstrasse   | e              | f              | NochNichtErfasst | -                   | -                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager muss sehen, ob ein Import erfolgreich war oder nicht
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und  eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir           | Breite Trottoir links (m) | Breite Trottoir rechts (m) |
	| Kantonsstrasse | 0.2             | 2.0             | 1                | Gemeinde           | Oberdorf        | II                  | Beton   | 19.56               | Tausend         | kein Trottoir      | 0                         | 0                          |
Wenn ich die XLSX-Datei importiere
Dann liefert der Import einen Validationsfehler in Zelle J2

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager muss warten bis der Import abgeschlossen ist, erst dann kann er mit EMSG weiterarbeiten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Bereits erfasste Strassenabschnitte bleiben bestehen
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
    | Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Kantonsstrasse | a              | a              | -          | KeinTrottoir     | -                   | -                    |
    | Landstrasse    | b              | b              | -          | Rechts           | -                   | 2,90                 |
    | Hauptstrasse   | c              | c              | -          | NochNichtErfasst | -                   | -                    |
Und eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
    | Strassenname | BezeichnungVon | BezeichnungBis | ExternalId | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts |
    | Hauptstrasse | j              | j              |			  |                  | Gemeinde           | Unterdorf       | II                  | Beton   | 18,00          | 1300,0 | BeideSeiten  | 1,40                | 3,00                 |
    | Hauptstrasse | e              | f              |			  |                  | Gemeinde           | Oberdorf        | IC                  | Beton   | 8,45           | 800,0  | Rechts       |                     | 3,00                 |
    | Hauptallee   | k              | k              |			  |                  | Korporation        | Flussnähe       | IB                  | Asphalt | 12,87          | 400,0  | KeinTrottoir |                     |                      |
Wenn ich die XLSX-Datei importiere
Dann sind folgende Netzinformationen im System:
	| Strassenname   | BezeichnungVon | BezeichnungBis | ExternalId |
	| Kantonsstrasse | a              | a              | -          |
	| Landstrasse    | b              | b              | -          |
	| Hauptstrasse   | c              | c              | -          |
	| Hauptstrasse   | j              | j              | -          |
	| Hauptstrasse   | e              | f              | -          |
	| Hauptallee     | k              | k              | -          |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Flächen (‚Fläche Fahrbahn‘, ‚Fläche Trottoir links‘, ‚Fläche Trottoir rechts‘, ‚Fläche Trottoir‘) analog zur Erfassung von Strassenabschnitten über das UI
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
	
Und ich bin Data-Manager von 'Mandant_1'

Und  eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag     | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir           | Breite Trottoir links (m) | Breite Trottoir rechts (m) |
	| Kantonsstrasse | 0.2             | 2.0             |                  | Gemeinde           | Oberdorf        | II                  | Beton     | 19.56               | 1800.0          | kein Trottoir      | 0                         | 0                          |
	| Kantonsstrasse | 2.0             | 7.0             |                  | Gemeinde           | Unterdorf       | IV                  | Asphalt   | 19.00               | 5000.0          | links              | 2.32                      | 0                          |
	| Landstrasse    | 0.0             | 2.5             |                  | Privat             | Unterdorf       | III                 | Asphalt   | 20.00               | 2500.0          | rechts             | 0.00                      | 2.90                       |
	| Hauptstrasse   | 1.2             | 2.5             |                  | Gemeinde           | Unterdorf       | II                  | Beton     | 18.00               | 1300.0          | beide              | 1.40                      | 3.00                       |
	| Hauptstrasse   | 0.4             | 1.2             |                  | Gemeinde           | Oberdorf        | IC                  | Beton     | 8.45                | 800.0           | Noch nicht erfasst |                           |                            |
	| Hauptstrasse   | 0.0             | 0.4             |                  | Korporation        | Flussnähe       | IB                  | Asphalt   | 12.87               | 400.0           | kein Trottoir      | 0                         | 0                          |
Wenn ich die XLSX-Datei importiere
Dann werden folgende Flächen berechnet:
	| Strassenname   | BezeichnungVon | BezeichnungBis | Fläche Fahrbahn | Fläche Trottoir links | Fläche Trottoir rechts | Fläche Trottoir |
	| Kantonsstrasse | 0.2            | 2.0            | 35208           | 0                     | 0                      | 0               |
	| Kantonsstrasse | 2.0            | 7.0            | 95000           | 11600                 | 0                      | 11600           |
	| Landstrasse    | 0.0            | 2.5            | 50000           | 0                     | 7250                   | 7250            |
	| Hauptstrasse   | 1.2            | 2.5            | 23400           | 1820                  | 3900                   | 5720            |
	| Hauptstrasse   | 0.4            | 1.2            | 6760            |                       |                        |                 |
	| Hauptstrasse   | 0.0            | 0.4            | 5148            | 0                     | 0                      | 0               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann ein Beispiel-Excel herunterladen
Wenn ich das Excel-Template herunterlade
Dann bekomme ich eine XLSX-Datei mit folgenden Zeilen (im ersten Tab):
	| Strassenname   | Bezeichnung von | Bezeichnung bis | Abschnittsnummer | Strasseneigentümer | Ortsbezeichnung | Belastungskategorie | Belag     | Breite Fahrbahn (m) | Gesamtlänge (m) | Trottoir           | Breite Trottoir links (m) | Breite Trottoir rechts (m) |
