Funktionalität: N4 - Strassenabschnitte im GIS-Modus verwalten
	Als Data-Manager
	will ich Strassenabschnitte im GIS-Modus verwalten
	damit ich einen Überblick über mein Strassennetz bekomme und ich die erfassten Informationen als Grundlage für die Zustandserfassung verwenden kann

Grundlage: 
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
	
	Und ich bin Data-Manager von 'Mandant_1'


@Manuell	
Szenario: Der Data-Manager kann einen Strassenabschnitt auf der Karte digitalisieren

#----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Anfangs- und Endpunkt auf der Karte festlegen
Gegeben sei ich öffne die Seite 'Netzverwaltung\Netzdefinition'
	Und ich wähle einen beliebigen Strassenabschnitt aus
	Und ich aktiviere das Tool 'Strassenabschnitt Geometrie bearbeiten'
	Wenn ich einen Anfangs oder Endpunkt entlang dem darunter liegenden Achssegment bewege
	Dann wird der Strassenabschnitt entsprechend verlängert oder verkürzt
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell @ignore
Szenario: Strassenabschnitte dürfen sich nicht überlappen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann pro Achssegment Anfangs- und Endpunkt eines Strassenabschnitts auf der Karte festlegen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Der Data-Manager kann Attribute zu Strassenabschnitten erfassen

Gegeben sei ich öffne die Seite 'Netzdefinition'

	Wenn ich folgende GIS-Netzinformationen eingebe:
	    | Strassenname   | LaengenkorrekturAnfang   | LaengenkorrekturEnde   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   |
	    | <Strassenname> | <LaengenkorrekturAnfang> | <LaengenkorrekturEnde> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> |

	Dann liefert Feldbezeichnung '<Feldbezeichnung>' einen Validationsfehler '<Validationsfehler>'

	Dann sind folgende GIS-Netzinformationen im System:
		| Strassenname   | LaengenkorrekturAnfang   | LaengenkorrekturEnde   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   |
		| <Strassenname> | <LaengenkorrekturAnfang> | <LaengenkorrekturEnde> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> | 

Beispiele: 
	| TF | Strassenname | LaengenkorrekturAnfang | LaengenkorrekturEnde | Belastungskategorie | Belag   | BreiteFahrbahn | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | Validationsfehler | Feldbezeichnung        | Kommentar                                                  |
	| 1  | Bahnstrasse  | -                      | -                    | IC                  | Beton   | 5,75           | KeinTrottoir     | -                   | -                    | Private            | Mitterndorf             | Nein              | -                      | Gutfall                                                    |
	| 2  | Moosgasse    | 0                      | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Nein              | -                      | Gutfall                                                    |
	| 3  | Hauptstrasse | 2,25                   | 1,5                  | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Nein              | -                      | Gutfall                                                    |
	| 4  | Hauptstrasse | 2                      | -                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl | Nein              | -                      | Gutfall                                                    |
	| 5  | -            | 2,25                   | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Ja                | Strassenname           | Strassenname ist Pflichtfeld                               |
	| 6  | Hauptstrasse | 2,257                  | 2                    | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Nein              | LaengenkorrekturAnfang | LängenkorrekturAnfang wird auf 2 Nachkommastellen gerundet |
	| 7  | Hauptstrasse | 2,25                   | 1,125                | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Nein              | LaengenkorrekturEnde   | LängenkorrekturEnde wird auf 2 Nachkommastellen gerundet   |
	| 8  | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7,525          | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Nein              | BreiteFahrbahn         | BreiteFahrbahn wird auf 2 Nachkommastellen gerundet        |
	| 9  | Hauptstrasse | -2                     | 2                    | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturAnfang | Ungültige LängenkorrekturAnfang < 0                        |
	| 10 | Hauptstrasse | 2,25                   | -0,25                | II                  | Beton   | 7              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LängekorrekturEnde     | Ungültige LängenkorrekturEnde < 0                          |
	| 11 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | -1             | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn < 0                               |
	| 12 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | -0,1           | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn < 0                               |
	| 13 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 0              | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn darf nicht 0 sein                 |
	| 14 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Rechts           | -                   | -2                   | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirRechts   | Ungültige BreiteTrottoirRechts < 0                         |
	| 15 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Links            | -2,1                | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirLinks    | Ungültige BreiteTrottoirLinks < 0                          |
	| 16 | Moosgasse    | 2,25                   | 2                    | -                   | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Ja                | Belastungskategorie    | Belastungskategorie ist Pflichtfeld                        |
	| 17 | Moosgasse    | 2,25                   | 2                    | IB                  | -       | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Ja                | Belagsart              | Belagsart ist Pflichtfeld                                  |
	| 18 | Moosgasse    | 2,25                   | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | Ja                | BreiteFahrbahn         | BreiteFahrbahn ist Pflichtfeld                             |
	| 19 | Moosgasse    | adc                    | 2                    | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5                 | 2                    | -                  | Ortsteil Mossbrunn      | Ja                | Strasseneigentümer     | Strasseneigentümer ist Pflichtfeld                         |
	| 20 | Hauptstrasse | 15da                   | 2                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturAnfang | Ungültige LängenkorrekturAnfang string                     |
	| 21 | Hauptstrasse | 3,§                    | 2                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturAnfang | Ungültige LängenkorrekturAnfang string                     |
	| 22 | Hauptstrasse | 2,25                   | pav                  | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturAnfang | Ungültige LängenkorrekturAnfang string                     |
	| 23 | Hauptstrasse | 2,25                   | fa34                 | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturEnde   | Ungültige LängenkorrekturEnde string                       |
	| 24 | Hauptstrasse | 2,25                   | !§;"                 | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturEnde   | Ungültige LängenkorrekturEnde string                       |
	| 25 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | LaengenkorrekturEnde   | Ungültige LängenkorrekturEnde string                       |
	| 26 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | ab             | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn string                            |
	| 27 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 1s             | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn string                            |
	| 28 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | ?`F            | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteFahrbahn         | Ungültige BreiteFahrbahn string                            |
	| 29 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Links            | asb                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirLinks    | Ungültige BreiteTrottoirLinks string                       |
	| 30 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Links            | 2da                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirLinks    | Ungültige BreiteTrottoirLinks string                       |
	| 31 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Links            | "&1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirLinks    | Ungültige BreiteTrottoirLinks string                       |
	| 32 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Rechts           | -                   | asb                  | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirRechts   | Ungültige BreiteTrottoirRechts string                      |
	| 33 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Rechts           | -                   | 2da                  | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirRechts   | Ungültige BreiteTrottoirRechts string                      |
	| 34 | Hauptstrasse | 2,25                   | 2                    | II                  | Beton   | 7              | Rechts           | -                   | "&1                  | Korporation        | Ortsteil Gramatneusiedl | Ja                | BreiteTrottoirRechts   | Ungültige BreiteTrottoirRechts string                      |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann einen Strassenabschnitt durch Auswahl auf der Karte löschen
	Gegeben sei für Mandant 'Mandant_1' existieren folgende Netzinformationen:

		| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung    | Abschnittsnummer |
		| 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton   | 5,75           | 150   | KeinTrottoir | -                   | -                    | Gemeinde           | Mitterndorf        | 1                |
		| 2  | Moosgasse    | -              | -              | IB                  | Asphalt | 5,50           | 200,2 | BeideSeiten  | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn | 1                |
	
	Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich den Strassenabschnitt (ID 2) durch Auswahl in der Karte selektiere
	Und ich die Netzinformationen für diesen Strassenabschnitt lösche

	Dann sind folgende Netzinformationen im System:
        | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
        | 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton | 5,75           | 150   | KeinTrottoir | -                   | -                    | Gemeinde           | Mitterndorf     | 1                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass Strassenabschnitte lückenlos aufeinander folgen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Anfang und Ende des Strassenabschnitts werden visualisiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt beim Löschen von Strassenabschnitten sicher, dass diese keiner Inspektionsroute zugeordnet sind

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Archivierte Daten können nicht verändert werden

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Werden Strassenabschnitte gelöscht, hat das keine Auswirkungen auf historische Daten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Strassenabschnitte über mehrere Achssegmente anlegen
Gegeben sei ich öffne die Seite 'Netzdefinition'
Und ich wähle das Tool 'Strassenabschnitte erstellen'
Wenn ich auf ein beliebiges Achssegment klicke
Und ich auf ein weiteres beliebiges Achssegment klicke
Dann wird auf beiden ausgewählten Segmenten jeweils ein Strassenabschnittsegment über die gesamte Länge erstellt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann die räumliche Definition nur pro Achssegment bearbeiten
Gegeben sei ich öffne die Seite 'Netzdefinition'
Und ich wähle einen beliebigen Strassenabschnitt aus
Und ich wähle das Tool 'Strassenabschnitt bearbeiten'
Wenn ich versuche einen der Endpunkte des Strassenabschnittsegments über das aktuelle Achsegment hinaus auf ein anderes Achsegment zu ziehen
Dann stoppt der Endpunkt am Ende des aktuellen Achssegments

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in einem Textfeld nach Strassennamen suchen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir (m²) nachdem der Data-Manager den Strassenabschnitt gespei-chert hat