Funktionalität: W1.5 - Eine Karte mit Strassenabschnitten, symbolisiert nach Belastungskategorien erhalten
	Als Data-Reader
	will ich eine Karte mit Strassenabschnitten, symbolisiert nach Belastungskategorien erhalten
	damit ich für Entscheidungsträger meiner Gemeinde eine Informationsbasis habe und einen Überblick zu meinem Inventar erhalte

Grundlage: 
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
	
	Und ich bin Data-Reader von 'Mandant_1'
	Und folgende Netzinformationen existieren:
		| ID | Strassenname | LaengenkorrekturAnfang | LaengenkorrekturEnde | Belastungskategorie | Belag   | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | Validationsfehler | 
		| 1  | Bahnstrasse  | -                      | -                    | IC                  | Beton   | 5,75           | KeinTrottoir     | -                   | -                    | Private            | Mitterndorf             | Nein              |
		| 2  | Moosgasse    | 0                      | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Nein              | 
		| 3  | Hauptstrasse | 2,25                   | 1,5                  | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Nein              |
		| 4  | Hauptstrasse | 2                      | -                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl | Nein              | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die gewünschte Auswertung selektieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader muss ein Jahr auswählen, für das vom System eine Inventarauswertung generiert werden soll

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Vom System wird das Jahr des letzen Jahresabschlusses als default Wert vorselektiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader sieht auf der Karte die Strassenabschnitte farblich symbolisiert nach deren Belastungskategorie
Gegeben sei folgende Belastungskategorien und Farben sind definiert:
| Belastungskategorie | Farbe   | Kommentar    |
| IA                  | #b7e9b1 | (hellgrün)   |
| IB                  | #90d188 | (grün)       |
| IC                  | #60ab57 | (dunkelgrün) |
| II                  | #38aabb | (türkis)     |
| III                 | #3882bb | (hellblau)   |
| IV                  | #194e95 | (dunkelblau) |

Und ich öffne die Seite Auswertungen\Strassenabschnitte Geographisch
Dann sind folgende Informationen sichtbar:
		| ID | Strassenname | LaengenkorrekturAnfang | LaengenkorrekturEnde | Belastungskategorie | Belag   | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung          |  Kommentar                          |
		| 1  | Bahnstrasse  | -                      | -                    | IC                  | Beton   | 5,75           | KeinTrottoir     | -                   | -                    | Private            | Mitterndorf              |  Graphenfarbe: #60ab57 (dunkelgrün) |
		| 2  | Moosgasse    | 0                      | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn       |  Graphenfarbe: #90d188 (grün)       |
		| 3  | Hauptstrasse | 2,25                   | 1,5                  | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl  |  Graphenfarbe: #38aabb (türkis)     |
		| 4  | Hauptstrasse | 2                      | -                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl  |  Graphenfarbe: #38aabb (türkis)     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die darzustellenden Strassenabschnitte über einen Filter auf den Strasseneigentümer einschränken
Gegeben sei ich öffne die Seite Auswertungen\Strassenabschnitte Geographisch
Wenn ich den Filter auf Eigentümer: "Gemeinde" setze
Dann sind folgende Informationen sichtbar:
		| ID | Strassenname | LaengenkorrekturAnfang | LaengenkorrekturEnde | Belastungskategorie | Belag   | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | 
		| 2  | Moosgasse    | 0                      | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 
		| 4  | Hauptstrasse | 2                      | -                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl | 
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die aktuelle Ansicht als PDF exportieren
# ACHTUNG: Noch nicht implementiert