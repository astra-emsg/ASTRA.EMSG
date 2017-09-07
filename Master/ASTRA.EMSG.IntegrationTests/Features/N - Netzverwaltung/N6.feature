Funktionalität: N6 - Einen Strassenabschnitt in mehrere Strassenabschnitte teilen
	Als Data-Manager
	will ich einen Strassenabschnitt in mehrere Strassenabschnitte teilen
	damit ich die Erfassung meins Netz hinsichtlich der tatsächlichen Breiten und Trottoirs vornehmen kann

Grundlage: 
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus           | Mengentyp   |
	| Mandant_1 | strassennamen   | Gesamtlänge |
Und ich bin Data-Manager von 'Mandant_1'
Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
	| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	| 1  | Hauptstrasse | 0.0            | 0.4            | IB                  | Asphalt | 12,87          | 400   | KeinTrottoir | 0                   | 0                    | Korporation        | Glarus-Nord     |  -               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manger kann selektieren, in wie viele Strassenabschnitte er einen bestehenden Strassenabschnitt teilen will
Wenn ich beim Strassenabschnitt mit der Id '1' auf Aufteilen klicke
Dann kann ich 2 bis 10 Teilabschnitte auswählen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System generiert automatisch die Anzahl an Abschnitten, die vom Data-Manager selektiert wurde
Wenn ich den Strassenabschnitt mit der Id '1' auf 3 Teilabschnitte aufteile
Und ich für die Teilabschnitte folgende Daten erfasse:
    | Id | Länge |
    | 2  | 100   |
    | 3  | 200   |
    | 4  | 300   |
Und ich die neuen Teilabschnitte speichere
Dann sind folgende Netzinformationen im System:
    | Id | Strassenname | BezeichnungVon | BezeichnungBis | Länge |
    | 2  | Hauptstrasse |                |                | 100   |
    | 3  | Hauptstrasse |                |                | 200   |
    | 4  | Hauptstrasse |                |                | 300   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System übernimmt teilweise Parameter aus dem bestehenden Strassenabschnitt für die gesplitteten Strassenabschnitte
Wenn ich den Strassenabschnitt mit der Id '1' auf 2 Teilabschnitte aufteile
Und ich für die Teilabschnitte folgende Daten erfasse:
    | Id | Länge |
    | 2  | 200   |
    | 3  | 200   |
Und ich die neuen Teilabschnitte speichere
Dann sind folgende Netzinformationen im System:
	| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	| 2  | Hauptstrasse |                |                | IB                  | Asphalt | 12,87          | 200   | NochNichtErfasst |                     |                      | Korporation        |                 |                  |
	| 3  | Hauptstrasse |                |                | IB                  | Asphalt | 12,87          | 200   | NochNichtErfasst |                     |                      | Korporation        |                 |                  |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Der Data-Manger kann alle Attribute der neuen Strassenabschnitte bearbeiten
Wenn ich den Strassenabschnitt mit der Id '1' auf '3' Teilabschnitte aufteile
    Und ich für die Teilabschnitte folgende Daten eingebe:
	   | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	   | Hauptstrasse | 0.0            | 0.1            | IB                  | Asphalt | 12,87          | 100   | KeinTrottoir | 0                   | 0                    | Korporation        | Glarus-Nord     | 1                |
	   | Hauptstrasse | 0.1            | 0.2            | IB                  | Asphalt | 12,87          | 100   | Links        | 0                   | 1,1                  | Korporation        | Glarus-Mitte    | 2                |
	   | Hauptstrasse | 0.2            | 0.4            | IB                  | Asphalt | 12,87          | 200   | Rechts       | 1,2                 | 0                    | Korporation        | Glarus-Süd      | 3                |
Dann sind folgende Netzinformationen im System:
	| Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	| Hauptstrasse | 0.0            | 0.1            | IB                  | Asphalt | 12,87          | 100   | KeinTrottoir | 0                   | 0                    | Korporation        | Glarus-Nord     | 1                |
	| Hauptstrasse | 0.1            | 0.2            | IB                  | Asphalt | 12,87          | 100   | Links        | 0                   | 1,1                  | Korporation        | Glarus-Mitte    | 2                |
	| Hauptstrasse | 0.2            | 0.4            | IB                  | Asphalt | 12,87          | 200   | Rechts       | 1,2                 | 0                    | Korporation        | Glarus-Süd      | 3                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System löscht beim Speichern der neuen Abschnitte den ursprünglichen Strassenabschnitt, der vom Data-Reader gesplittet wurde
Wenn ich den Strassenabschnitt mit der Id '1' auf '2' Teilabschnitte aufteile
    Und ich für die Teilabschnitte folgende Daten eingebe:
       | Strassenname | BezeichnungVon | BezeichnungBis | Länge |
       | Hauptstrasse | 0.0            | 0.2            | 200   |
       | Hauptstrasse | 0.2            | 0.4            | 200   |
Dann sind folgende Netzinformationen im System:
	| Strassenname | BezeichnungVon | BezeichnungBis | Länge |
	| Hauptstrasse | 0.0            | 0.2            | 200   |
	| Hauptstrasse | 0.2            | 0.4            | 200   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenario: Das System löscht zu den Strassenabschnitten gehörende Zustände und Massnahmen
Gegeben sei für Mandant 'Mandant_1' existieren folgende Zustände und Massnahmen:
	| Strassenabschnitt | BezeichnungVon | BezeichnungBis | Zustandsindex |
	| 1                 | 0.0            | 0.4            | 1             |
Wenn ich den Strassenabschnitt mit der Id '1' auf '2' Teilabschnitte aufteile
    Und ich für die Teilabschnitte folgende Daten eingebe:
	    | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Abschnittsnummer |
	    | Hauptstrasse | 0.0            | 0.1            | IB                  | Asphalt | 12,87          | 100   | KeinTrottoir | -                   | -                    | Korporation        | Glarus-Nord     | -                |
	    | Hauptstrasse | 0.1            | 0.2            | IB                  | Asphalt | 12,87          | 100   | Links        | 1,1                 | -                    | Korporation        | Glarus-Mitte    | -                |	    
Dann sind für den Strassenabschnitt mit der Id '1' keine Zustände und Massnahmen im System

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System löscht erst nach Warnung und Bestätigung durch den Data-Manager

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in der Karte einen Strassenabschnitt durch Aufruf einer entsprechenden Funktion und Klick auf den Strassenabschnitt auf einen Strassenabschnitt teilen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Entspricht die ursprüngliche Länge des Strassenabschnitts nicht der Summe der Längen der neuen Strassenabschnitte, wird vom System eine Warnmeldung ausgegeben
