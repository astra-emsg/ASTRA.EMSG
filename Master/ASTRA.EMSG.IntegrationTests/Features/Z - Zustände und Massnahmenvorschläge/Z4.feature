Funktionalität: Z4 - Im GIS-Modus Inspektionsrouten planen
	Als Data-Manager,
	will ich im GIS-Modus Inspektionsrouten planen
	damit ich meine Strassenabschnitte in der optimalen Reihenfolge inspizieren kann

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus |
	| Mandant_1 | GIS   |
Und ich bin Data-Manager von 'Mandant_1'
Und ich öffne die Seite 'Inspektionsroute planen'
Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
         | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge            | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer      | Ortsbezeichnung | 
         | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir     | -        | -                   | Private              | Mitterndorf             | Nein            | 
         | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn      | Nein            | 
         | 3  | Hauptstrasse | 2,25           | 1,5            | II                  | Beton   | 7              | NochNichtErfasst | -        | -                   | Korporation          | Ortsteil Gramatneusiedl | Nein            | 
         | 4  | Klostergasse | 2              | -              | II                  | Beton   | 7              | Links            | 2,1      | -                   | Gemeinde             | Ortsteil Gramatneusiedl | Nein            | 

@Manuell
Szenario: Der Data-Manager kann auf der Karte Strassenabschnitte selektieren
Gegeben sei ich will eine Insepktionsroute zusammenstellen 
Und ich öffne die Seite 'Inspektionsroute planen'
Und ich wähle mit einem Klick auf die Karte einen Strassenabschnitt 
Dann wird der Strassenabschnitt der Inpektionsroute zugeordnet

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System visualisiert selektierte Strassenabschnitte auf der Karte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System fügt selektierte Strassenabschnitte zu einer Inspektionsroute hinzu
Gegeben sei ich erstelle eine neue Inspektionsroute mit der Bezeichnung 'Inspektionsroute1'
Wenn ich den Strassenabschnitt mit der Id '1' auf der Karte selektiere
Dann ist folgender Strassenabschnitt der 'Inspektionsroute1' zugeordnet:
| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag | BreiteFahrbahn | Länge        | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | 
| 1  | Bahnstrasse  | -              | -              | IC                  | Beton | 5,75           | KeinTrottoir | -        | -                   | Private              | Mitterndorf        | Nein            | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann die Reihenfolge der Strassenabschnitte auf der Inspektionsroute verändern
Gegeben sei für Mandant 'Mandant_1' existiert eine Inspektionsroute mit der Bezeichnung 'Inspektionsroute1' 
Und dieser Inspektionsroute sind folgende Strassenabschnitte zugeordnet:
 | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge            | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer      | Ortsbezeichnung | Reihenfolge |
 | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir     | -        | -                   | Private              | Mitterndorf             | Nein            | 1           |
 | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn      | Nein            | 2           |
 | 3  | Hauptstrasse | 2,25           | 1,5            | II                  | Beton   | 7              | NochNichtErfasst | -        | -                   | Korporation          | Ortsteil Gramatneusiedl | Nein            | 3           |
 | 4  | Klostergasse | 2              | -              | II                  | Beton   | 7              | Links            | 2,1      | -                   | Gemeinde             | Ortsteil Gramatneusiedl | Nein            | 4           |
 Wenn ich die Platzierung von dem Strassenabschnitt mit der Id '3' erhöhe
 Dann sind folgende Daten im System:
 | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge            | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer      | Ortsbezeichnung | Reihenfolge |
 | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir     | -        | -                   | Private              | Mitterndorf             | Nein            | 1           |
 | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn      | Nein            | 3           |
 | 3  | Hauptstrasse | 2,25           | 1,5            | II                  | Beton   | 7              | NochNichtErfasst | -        | -                   | Korporation          | Ortsteil Gramatneusiedl | Nein            | 2           |
 | 4  | Klostergasse | 2              | -              | II                  | Beton   | 7              | Links            | 2,1      | -                   | Gemeinde             | Ortsteil Gramatneusiedl | Nein            | 4           |
 
 #------------------------------------------------------------------------------------------------------------------------------------------------------
 
@Manuell
Szenario: Der Data-Manager kann die Liste der Strassenabschnitte als Inspektionsroute (mit einem Namen) speichern
Gegeben sei für Mandant 'Mandant_1' existieren folgende Inspektionsrouten:
| Id | Bezeichnung     | Checkoutgis_id |
| 1  | Stadtteil_nord  |                |
| 2  | Stadtteil_süden |                |
| 3  | Stadtteil_osten |                |
Und ich habe folgende Strassenabschnitte ausgewählt:
| Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge        | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Reihenfolge |
| 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir | -        | -                   | Private              | Mitterndorf        | Nein            | 1           |
| 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten  | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn | Nein            | 2           |
Wenn ich diese Liste mit Strassenabschnitten mit der Bezeichnung 'Stadtteil_westen' speichere
Dann sind folgende Daten im System:
| Id | Bezeichnung      | Checkoutgis_id |
| 1  | Stadtteil_nord   |                |
| 2  | Stadtteil_süden  |                |
| 3  | Stadtteil_osten  |                |
| 4  | Stadtteil_westen |                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann bestehende Inspektionsrouten bearbeiten
Gegeben sei eine bestehende Inspektionsroute 
Und der Data-Manager wählt 'Bearbeiten Inspektionsroute' aus
Und der Data-Manager wählt zugeordnete Strassenabschnitte auf der Karte aus
Und der Data-Manager wählt 'Inspektionsroute' speichern aus
Dann wird die bearbeitet Inpektionsroute gespeichert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Strassenabschnitte aus einer Inspektionsrouten löschen.
Gegeben sei es existiert für Mandant 'Mandant_1' folgende Inspektionsroute:
| Id | Bezeichnung        | Checkoutgis_id |
| 1  | Inspektionsroute_1 |                |
Und dieser Inspektionsroute sind folgende Strassenabschnitte zugeordnet:
         | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge        | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Reihenfolge |
         | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir | -        | -                   | Private              | Mitterndorf        | Nein            | 1           |
         | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten  | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn | Nein            | 2           |
Wenn ich den Strassenabschnitt mit der Id '1' lösche
Dann sind folgende Strassenabschnitte der Inspektionsroute zugeordnet:
          | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge       | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Reihenfolge |
          | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn | Nein            | 1           |
Und folgende Netzinformationen im System:
		  | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge            | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer      | Ortsbezeichnung |
		  | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir     | -        | -                   | Private              | Mitterndorf             | Nein            |
		  | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn      | Nein            |
		  | 3  | Hauptstrasse | 2,25           | 1,5            | II                  | Beton   | 7              | NochNichtErfasst | -        | -                   | Korporation          | Ortsteil Gramatneusiedl | Nein            |
		  | 4  | Klostergasse | 2              | -              | II                  | Beton   | 7              | Links            | 2,1      | -                   | Gemeinde             | Ortsteil Gramatneusiedl | Nein            | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann neue Strassenabschnitte in eine vorhanden Inspektionsroute aufnehmen.
Gegeben sei es existiert für Mandant 'Mandant_1' folgende Inspektionsroute:
| Id | Bezeichnung        | Checkoutgis_id |
| 1  | Inspektionsroute_1 |                |
Und dieser Inspektionsroute sind folgende Strassenabschnitte zugeordnet:
         | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge        | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer | Ortsbezeichnung | Reihenfolge |
         | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir | -        | -                   | Private              | Mitterndorf        | Nein            | 1           |
         | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten  | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn | Nein            | 2           |
Wenn ich den Strassenabschnitt mit der Id '3' auf der Karte selektiere
Dann sind folgende Strassenabschnitte der Inspektionsroute zugeordnet:
          | Id | Strassenname | BezeichnungVon | BezeichnungBis | Belastungskategorie | Belag   | BreiteFahrbahn | Länge            | Trottoir | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer      | Ortsbezeichnung | Reihenfolge |
          | 1  | Bahnstrasse  | -              | -              | IC                  | Beton   | 5,75           | KeinTrottoir     | -        | -                   | Private              | Mitterndorf             | Nein            | 1           |
          | 2  | Moosgasse    | 0              | 2              | IB                  | Asphalt | 5,50           | BeideSeiten      | 1,5      | 2                   | Gemeinde             | Ortsteil Mossbrunn      | Nein            | 2           |
          | 3  | Hauptstrasse | 2,25           | 1,5            | II                  | Beton   | 7              | NochNichtErfasst | -        | -                   | Korporation          | Ortsteil Gramatneusiedl | Nein            | 3           |
 
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass ein Strassenabschnitt nur auf einer Inspektionsroute vorkommt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass der Benutzer Strassenabschnitte, die bereits einer Inspektionsroute zugeordnet wurden, nicht mehr für (die Planung von) weiteren Inspektionsrouten selektieren kann

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System visualisiert Strassenabschnitte, die bereits einer Inspektionsroute zugeordnet sind bzw. noch zur Aufnahme in eine Inspektionsroute zur Verfügung stehen.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Inspektionsrouten löschen
Gegeben sei für Mandant 'Mandant_1' existieren folgende Inspektionsrouten:
| Id | Bezeichnung     | Checkoutgis_id |
| 1  | Stadtteil_nord  |                |
| 2  | Stadtteil_süden |                |
| 3  | Stadtteil_osten |                |
Wenn ich die Inspektionsroute mit der Id '2' lösche sind folgende Inspektionsrouten im System:
| Id | Bezeichnung     | Checkoutgis_id |
| 1  | Stadtteil_nord  |                |
| 3  | Stadtteil_osten |                |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann eine exportierte Inspektionsroute nur nach entsprechender Warnmeldung löschen (Z5).

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt beim Löschen von Strassenabschnitten sicher, dass diese keiner Inspektionsroute zugeordnet sind