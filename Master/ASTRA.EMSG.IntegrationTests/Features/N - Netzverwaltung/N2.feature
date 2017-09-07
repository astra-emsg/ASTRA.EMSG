Funktionalität: N2 - Strassenabschnitte mit Strassennamen verwalten
	Als Data-Manager
	will ich Strassenabschnitte mit Strassennamen verwalten
	damit ich einen Überblick über mein Strassennetz bekomme und ich die erfassten Informationen als Grundlage für die Zustandserfassung verwenden kann

#------------------------------------------------------------------------------------------------------------------------------------------------------
	
@Automatisch
Szenariogrundriss: Der Data-Manager kann Strassenabschnitte erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen eingebe:
	    | Strassenname   | BezeichnungVon   | BezeichnungBis   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Länge   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   | Abschnittsnummer   |
	    | <Strassenname> | <BezeichnungVon> | <BezeichnungBis> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Länge> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> | <Abschnittsnummer> |

	Dann liefert Feldbezeichnung '<Feldbezeichnung>' einen Validationsfehler '<Validationsfehler>'

	Dann sind folgende Netzinformationen im System:
		| Strassenname   | BezeichnungVon   | BezeichnungBis   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Länge   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   | Abschnittsnummer   |
		| <Strassenname> | <BezeichnungVon> | <BezeichnungBis> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Länge> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> | <Abschnittsnummer> |

Beispiele: 
	| TF | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | Abschnittsnummer | Validationsfehler | Feldbezeichnung      | Kommentar                                         |
	| 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton   | 5,75           | 150    | KeinTrottoir     | -                   | -                    | Privat             | Mitterndorf             | 1                | Nein              | -                    | Gutfall                                           |
	| 2  | Moosgasse    | -              | -              | IB                  | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Nein              | -                    | Gutfall                                           |
	| 3  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Nein              | -                    | Gutfall                                           |
	| 4  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl | 0                | Nein              | -                    | Gutfall                                           |
	| 5  | -            | Anfang         | Ende           | IB                  | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Ja                | Strassenname         | Strassenname ist Pflichtfeld                      |
	| 6  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,25 | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge max. 1 Nachkommastellen           |
	| 7  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7,525          | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn max. 2 Nachkommastellen  |
	| 8  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | -25    | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge < 0                               |
	| 9  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | -25,1  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge < 0                               |
	| 10 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | -1             | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn < 0                      |
	| 11 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | -0,1           | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn < 0                      |
	| 12 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 0      | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge darf nicht 0 sein                 |
	| 13 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 0              | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn darf nicht 0 sein        |
	| 14 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Rechts           | -                   | -2                   | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirRechts | Ungültige BreiteTrottoirRechts < 0                |
	| 15 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Links            | -2,1                | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirLinks  | Ungültige BreiteTrottoirLinks < 0                 |
	| 16 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Rechts           | -                   | 2                    | Korporation        | Ortsteil Gramatneusiedl | -1               | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer < 0                    |

@Manuell
Beispiele: 
	| TF | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | Abschnittsnummer | Validationsfehler | Feldbezeichnung      | Kommentar                                         |
 	| 17 | Moosgasse    | Anfang         | Ende           | -                   | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Ja                | Belastungskategorie  | Belastungskategorie ist Pflichtfeld               |
 	| 18 | Moosgasse    | Anfang         | Ende           | IB                  | -       | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Ja                | Belagsart            | Belagsart ist Pflichtfeld                         |
 	| 19 | Moosgasse    | Anfang         | Ende           | IB                  | Asphalt | 5,50           | -      | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Ja                | BreiteFahrbahn       | BreiteFahrbahn ist Pflichtfeld                    |
 	| 20 | Moosgasse    | Anfang         | Ende           | IB                  | Asphalt | -              | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Moosbrunn      | 1                | Ja                | Länge                | Länge ist Pflichtfeld                             |
 	| 21 | Moosgasse    | Anfang         | Ende           | IB                  | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | -                  | Ortsteil Mossbrunn      | 1                | Ja                | Strasseneigentümer   | Strasseneigentümer ist Pflichtfeld                |
 	| 22 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | 1,5              | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer keine Nachkommastellen |
 	| 23 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -1,2             | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer < 0                    |
 	| 24 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | ABC    | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge string                            |
 	| 25 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | !*-12  | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge string                            |
 	| 26 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 12s1   | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge string                            |
 	| 27 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | ab             | 120    | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn string                   |
 	| 28 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 1s             | 120    | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn string                   |
 	| 29 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | ?`F            | 120    | Links            | 2,1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn string                   |
 	| 30 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Links            | asb                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirLinks  | Ungültige BreiteTrottoirLinks string              |
 	| 31 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Links            | 2da                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirLinks  | Ungültige BreiteTrottoirLinks string              |
 	| 32 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Links            | "&1                 | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirLinks  | Ungültige BreiteTrottoirLinks string              |
 	| 33 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | asb                  | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirRechts | Ungültige BreiteTrottoirRechts string             |
 	| 34 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | 2da                  | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirRechts | Ungültige BreiteTrottoirRechts string             |
 	| 35 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | "&1                  | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirRechts | Ungültige BreiteTrottoirRechts string             |
 	| 36 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | 2                    | Korporation        | Ortsteil Gramatneusiedl | de               | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer string                 |
 	| 37 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | 2                    | Korporation        | Ortsteil Gramatneusiedl | 1c               | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer string                 |
 	| 38 | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 120    | Rechts           | -                   | 2                    | Korporation        | Ortsteil Gramatneusiedl | 7;               | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer string                 |

# ACHTUNG: Konsistenzcheck Trottoir noch nicht berücksichtigt (Änderungsantrag 02 noch nicht beauftragt!)

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann alle Attribute des Strassenabschnitts bearbeiten
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
		| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
		| 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton | 5,75           | 150   | KeinTrottoir | -                   | -                    | Privat             | Mitterndorf     | 1                |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen für ID '1' eingebe:
	    | Strassenname   | BezeichnungVon   | BezeichnungBis   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Länge   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   | Abschnittsnummer   |
	    | <Strassenname> | <BezeichnungVon> | <BezeichnungBis> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Länge> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> | <Abschnittsnummer> |

	Dann liefert Feldbezeichnung '<Feldbezeichnung>' einen Validationsfehler '<Validationsfehler>'

	Dann sind folgende Netzinformationen im System:
		| Id | Strassenname   | BezeichnungVon   | BezeichnungBis   | Belastungskategorie   | Belag   | BreiteFahrbahn   | Länge   | Trottoir   | BreiteTrottoirLinks   | BreiteTrottoirRechts   | Strasseneigentümer   | Ortsbezeichnung   | Abschnittsnummer   |
		| 1  | <Strassenname> | <BezeichnungVon> | <BezeichnungBis> | <Belastungskategorie> | <Belag> | <BreiteFahrbahn> | <Länge> | <Trottoir> | <BreiteTrottoirLinks> | <BreiteTrottoirRechts> | <Strasseneigentümer> | <Ortsbezeichnung> | <Abschnittsnummer> |
	
Beispiele: 
	| TF | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge  | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung         | Abschnittsnummer | Validationsfehler | Feldbezeichnung      | Kommentar                                        |
	| 1  | 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton   | 5,75           | 150    | KeinTrottoir     | -                   | -                    | Privat             | Mitterndorf             | 1                | Nein              | -                    | Gutfall                                          |
	| 2  | 1  | Moosgasse    | -              | -              | IB                  | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Nein              | -                    | Gutfall                                          |
	| 3  | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Nein              | -                    | Gutfall                                          |
	| 4  | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Links            | 2,1                 | -                    | Gemeinde           | Ortsteil Gramatneusiedl | 0                | Nein              | -                    | Gutfall                                          |
	| 5  | 1  | -            | Anfang         | Ende           | IB                  | Asphalt | 5,50           | 200,2  | BeideSeiten      | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn      | 1                | Ja                | Strassenname         | Strassenname ist Pflichtfeld                     |
	| 10 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,25 | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge max. 1 Nachkommastellen          |
	| 11 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7,525          | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn max. 2 Nachkommastellen |
	| 12 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | -25    | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge < 0                              |
	| 13 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | -25,1  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge < 0                              |
	| 14 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | -1             | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn < 0                     |
	| 15 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | -0,1           | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn < 0                     |
	| 16 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 0      | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | Länge                | Ungültige Länge darf nicht 0 sein                |
	| 17 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 0              | 702,5  | NochNichtErfasst | -                   | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteFahrbahn       | Ungültige BreiteFahrbahn darf nicht 0 sein       |
	| 18 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Rechts           | -                   | -2                   | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirRechts | Ungültige BreiteTrottoirRechts < 0               |
	| 19 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Links            | -2,1                | -                    | Korporation        | Ortsteil Gramatneusiedl | -                | Ja                | BreiteTrottoirLinks  | Ungültige BreiteTrottoirLinks < 0                |
	| 21 | 1  | Hauptstrasse | Kirche         | Post           | II                  | Beton   | 7              | 702,5  | Rechts           | -                   | 2                    | Korporation        | Ortsteil Gramatneusiedl | -1               | Ja                | Abschnittsnummer     | Ungültige Abschnittsnummer < 0                   |	

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen speichere
		| Id | BreiteFahrbahn | Länge | BreiteTrottoirLinks | BreiteTrottoirRechts |
		| 1  | 5,75           | 150   | -                   | -                    |
		| 2  | 5,50           | 200,2 | 1,5                 | 2                    | 

	Dann ist folgende FlächeFahrbahn im System:
        | Id | FlächeFahrbahn |
        | 1  | 862,5          |
        | 2  | 1101,1         |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen speichere
		| Id | BreiteFahrbahn | Länge | BreiteTrottoirLinks | BreiteTrottoirRechts |
		| 1  | 5,75           | 150   | -                   | -                    |
		| 2  | 5,50           | 200,2 | 1,5                 | 2                    | 

	Dann ist folgende FlächeTrottoirLinks im System:
        | Id | FlächeTrottoirLinks |
        | 1  | 0                   |
        | 2  | 300,3               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen speichere
		| Id | BreiteFahrbahn | Länge | BreiteTrottoirLinks | BreiteTrottoirRechts |
		| 1  | 5,75           | 150   | -                   | -                    |
		| 2  | 5,50           | 200,2 | 1,5                 | 2                    | 

	Dann ist folgende FlächeTrottoirRechts im System:
        | Id | FlächeTrottoirRechts |
        | 1  | 0                    |
        | 2  | 400,4                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System berechnet die Fläche Trottoir (m²) nachdem der Data-Manager den Strassenabschnitt gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich folgende Netzinformationen speichere
		| Id | BreiteFahrbahn | Länge | BreiteTrottoirLinks | BreiteTrottoirRechts |
		| 1  | 5,75           | 150   | -                   | -                    |
		| 2  | 5,50           | 200,2 | 1,5                 | 2                    |
		| 3  | 5              | 120   | 3                   | -                    |
		| 4  | 5              | 350   | -                   | 1,5                  | 

	Dann ist folgende FlächeTrottoir im System:
        | Id | FlächeTrottoir |
        | 1  | 0              |
        | 2  | 700,7          |
        | 3  | 360            |
        | 4  | 525            |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Der Data-Manager kann Strassenabschnitte über die Übersichtstabelle löschen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
		| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung    | Abschnittsnummer |
		| 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton   | 5,75           | 150   | KeinTrottoir | -                   | -                    | Gemeinde           | Mitterndorf        | 1                |
		| 2  | Moosgasse    | -              | -              | IB                  | Asphalt | 5,50           | 200,2 | BeideSeiten  | 1,5                 | 2                    | Gemeinde           | Ortsteil Mossbrunn | 1                |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich Netzinformationen für ID '2' lösche

	Dann sind folgende Netzinformationen im System:
        | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
        | 1  | Bahnstrasse  | Nr 11.         | Nr. 22         | IC                  | Beton | 5,75           | 150   | KeinTrottoir | -                   | -                    | Gemeinde           | Mitterndorf     | 1                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Das System schlägt aufgrund der selektierten Belastungskategorie eine Standardbreite (Breite Fahrbahn, Breite Trottoir links und Breite Trottoir rechts) vor

	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich Belastungskategorie '<BelastungskategorieTyp>' selektiere

	Dann ist folgende DefaultBreiteFahrbahn '<DefaultBreiteFahrbahn>', DefaultBreiteTrottoirLinks '<DefaultBreiteTrottoirLinks>' und DefaultBreiteTrottoirRechts '<DefaultBreiteTrottoirRechts>' vorgeschlagen
     
	Beispiele:
		| TF | BelastungskategorieTyp | DefaultBreiteFahrbahn | DefaultBreiteTrottoirLinks | DefaultBreiteTrottoirRechts |
		| 1  | IA                     | 4,50                  | -                          | 1,5                         |
		| 2  | IB                     | 5,50                  | 1,5                        | 1,5                         |
		| 3  | IC                     | 5,75                  | -                          | -                           |
		| 4  | II                     | 7,00                  | 2                          | 2                           |
		| 5  | III                    | 7,00                  | 2,5                        | 2,5                         |
		| 6  | IV                     | 14,00                 | 2,5                        | 2,5                         |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Die Default Werte werden bei Änderung der Belastungskategorie vom System nur dann neu gesetzt, wenn der Data-Manger den ursprünglichen Default Wert noch nicht verändert hat

	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
        | Id | Belastungskategorie | BreiteFahrbahn | BreiteTrottoirLinks | BreiteTrottoirRechts |
        | 1  | IA                  | 4,5            | -                   | 1,5                  |
        | 2  | IB                  | 5,50           | 1,5                 | 1,5                  |
        | 3  | IC                  | 5,75           | -                   | -                    |
        | 4  | II                  | 7,00           | 2                   | 2                    |
        | 5  | III                 | 7,00           | 2,5                 | 2,5                  |
        | 6  | IV                  | 14,00          | 2,5                 | 2,5                  |
        | 7  | IA                  | 4              | -                   | 1,5                  |
        | 8  | IB                  | 5,50           | 2                   | 1,5                  |
        | 9  | IC                  | 5,75           | -                   | 1                    |
        | 10 | II                  | 5              | 1                   | 1                    |
        | 11 | III                 | 7,5            | 2,5                 | 2,5                  |
        | 12 | IV                  | 14,00          | 2                   | 2                    |

	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'


	Wenn ich Belastungskategorie '<BelastungskategorieTyp>' ändere

	Dann ist folgende DefaultBreiteFahrbahn '<DefaultBreiteFahrbahn>', DefaultBreiteTrottoirLinks '<DefaultBreiteTrottoirLinks>' und DefaultBreiteTrottoirRechts '<DefaultBreiteTrottoirRechts>' vorgeschlagen
     
	Beispiele:
		| TF | Id | BelastungskategorieTyp | DefaultBreiteFahrbahn | DefaultBreiteTrottoirLinks | DefaultBreiteTrottoirRechts |
		| 1  | 1  | IB                     | 5,50                  | 1,5                        | 1,5                         |
		| 2  | 2  | IC                     | 5,75                  | -                          | -                           |
		| 3  | 3  | II                     | 7,00                  | 2                          | 2                           |
		| 4  | 4  | III                    | 7,00                  | 2,5                        | 2,5                         |
		| 5  | 5  | IV                     | 14,00                 | 2,5                        | 2,5                         |
		| 6  | 6  | IA                     | 4,50                  | -                          | 1,5                         |
		| 7  | 7  | IB                     | 4                     | -                          | 1,5                         |
		| 8  | 8  | IC                     | 5,50                  | 2                          | 1,5                         |
		| 9  | 9  | II                     | 5,75                  | -                          | 1                           |
		| 10 | 10 | III                    | 5                     | 1                          | 1                           |
		| 11 | 11 | IV                     | 7,5                   | 2,5                        | 2,5                         |
		| 12 | 12 | IA                     | 14,00                 | 2                          | 2                           |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die Übersichtstabelle ist nach Strassennamen und Abschnittsnummer sortiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann in einem Textfeld nach Strassennamen suchen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |

	Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
		| Id | Strassenname |
		| 1  | Bahnstrasse  |
		| 2  | Hauptstrasse |
		| 3  | Lagerstrasse |
		| 4  | Moosgasse    |
		| 5  | Jägerstrasse |
		| 6  | Hauptstrasse |
		| 7  | Moosfeld     |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich nach Filterkriterium '<Filterkriterium>' suche

	Dann existieren folgende Netzinformationen in der Übersichtstabelle: '<IdList>'

	Beispiele: 
		| TF | Filterkriterium | IdList        |
		| 1  | Bahn            | 1             |
		| 2  | Moos            | 4, 7          |
		| 3  | Feld            | 7             |
		| 4  | Strasse         | 1, 2, 3, 5, 6 |
		| 5  | Jägerstrasse    | 5             |
		| 6  | Jaegerstrasse   | -             |
		| 7  | Lagergasse      | -             |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Archivierte Daten können nicht verändert werden

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Werden Strassenabschnitte gelöscht, hat das keine Auswirkungen auf historische Daten
