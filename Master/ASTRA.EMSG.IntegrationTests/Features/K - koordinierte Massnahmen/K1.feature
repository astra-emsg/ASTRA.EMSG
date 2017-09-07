Funktionalität: K1 - Massnahmen der Teilsysteme verwalten
	Als Data-Manager,
	will ich im GIS-Modus Massnahmen der Teilsysteme verwalten
	damit ich einen Überblick über mögliche Erhaltungsmassnahmen der Teilsysteme gewinne

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus |
	| Mandant_1 | gis   |
Und ich bin Data-Manager von 'Mandant_1'

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann eine Massnahmen eines Teilsystems auf der Karte digitalisieren
Wenn ich eine Massnahmen eines Teilsystems eingebe
Und ich auf der Karte die entsprechenden Achssegemente auswähle
Und ich 'Massnahmen eines Teilsystems speichern' auswähle
Dann wird die Massnahme eines Teilsystems mit den zugehörigen Achssegmenten gespeichert
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Anfangs- und Endpunkt einer Massnahmen eines Teilsystems auf der Karte festlegen
Wenn ich die Seite 'Massnahmen eines Teilsystems' öffne
Und ich wähle ein beliebiges Achssegment aus
Und ich aktiviere das Tool 'Massnahme eines Teilsystems Geometrie bearbeiten'
Und ich einen Anfangs oder Endpunkt entlang dem darunter liegenden Achssegment bewege
Dann wird der Strassenabschnitt entsprechend verlängert oder verkürzt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann alle Attribute einer Massnahmen eines Teilsystems erfassen
Wenn ich folgende realisierte Massnahme eingebe:
	| Id | Projektname | Bezeichnung von | Bezeichnung bis | Teilsystem               | Zuständige Organisation | Beschreibung  | Kosten | Status          |
	| 1  | Projekt 1   | Kirche          | Post            | Wasserversorgungsanlagen | Strabag                 | Alle Felder   | 20000  | Vorgeschlagen   |
	| 2  | Projekt 1   | Post            | Bahnhof         | Fernwäremeversorgung     | Porr                    | Gleicher Name | 15000  | In Koordination |
	| 3  | Projekt 2   |                 |                 | Wasserbau                |                         | Pflichtfelder |        | Abgeschlossen   |
Dann sind folgende realisierte Massnahmen im System:
	| Id | Projektname | Bezeichnung von | Bezeichnung bis | Teilsystem               | Zuständige Organisation | Beschreibung  | Kosten | Status          |
	| 1  | Projekt 1   | Kirche          | Post            | Wasserversorgungsanlagen | Strabag                 | Alle Felder   | 20000  | Vorgeschlagen   |
	| 2  | Projekt 1   | Post            | Bahnhof         | Fernwäremeversorgung     | Porr                    | Gleicher Name | 15000  | In Koordination |
	| 3  | Projekt 2   |                 |                 | Wasserbau                |                         | Pflichtfelder |        | Abgeschlossen   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann alle Attribute einer Massnahme eines Teilsystems bearbeiten
Gegeben sei für Mandant 'Mandant_1' existieren folgende Massnahmen der Teilsysteme:
	| Id | Projektname             | Bezeichnung von | Bezeichnung bis | Teilsystem              | Zuständige Organisation | Beschreibung  | Kosten | Status          |
	| 1  | Hauptstrasse Erneuerung | Post            | Kirche          | Strassen                | Strabag                 | Alle Felder   | 20000  | Vorgeschlagen   |
	| 2  | Hauptstrasse Erneuerung | Post            | Bahnhof         | Elektrizitätsversorgung | Porr                    | Gleicher Name | 15000  | In Koordination |
	| 3  | Projekt B               |                 |                 | Kommunikationsanlagen   |                         | Pflichtfelder |        | Abgeschlossen   |
Wenn ich folgende folgende Massnahmen der Teilsysteme eingebe:
	| Id | Projektname                | Bezeichnung von | Bezeichnung bis | Teilsystem            | Zuständige Organisation | Beschreibung   | Kosten | Status          |
	| 1  | Projektname geändert       | Dorfplatz       | Gemeindeamt     | Öffentlicher Verkehr  | Gemeinde                | Alles geändert | 25000  | In Koordination |
	| 2  | Hauptstrasse Erneuerung    | Nr. 12          | Nr. 44          | Kunstbauten           | Strabag                 | Gleicher Name  | 20000  | Abgeschlossen   |
	| 3  | Dorferneuerungsaktion 2012 | Lindenweg 12    | Brunngasse 22   | Kommunikationsanlagen | Örtliches Bauamt        | Alles geändert | 10000  | Vorgeschlagen   |
Dann sind folgende Massnahmen der Teilsysteme im System:
	| Id | Projektname                | Bezeichnung von | Bezeichnung bis | Teilsystem            | Zuständige Organisation | Beschreibung   | Kosten | Status          |
	| 1  | Projektname geändert       | Dorfplatz       | Gemeindeamt     | Öffentlicher Verkehr  | Gemeinde                | Alles geändert | 25000  | In Koordination |
	| 2  | Hauptstrasse Erneuerung    | Nr. 12          | Nr. 44          | Kunstbauten           | Strabag                 | Gleicher Name  | 20000  | Abgeschlossen   |
	| 3  | Dorferneuerungsaktion 2012 | Lindenweg 12    | Brunngasse 22   | Kommunikationsanlagen | Örtliches Bauamt        | Alles geändert | 10000  | Vorgeschlagen   |

#------------------------------------------------------------------------------------------------------------------------------------------------------
	
@Manuell
Szenario: Das System visualisiert die erfassten Massnamen der Teilsysteme auf der Karte
Wenn ich die Seite 'Massnahmen eines Teilsystems' öffne
Dann werden die dem 'Mandant_1' zugeordneten 'Massnahmen eines Teilsystems' auf der Karte visualisiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann eine Massnahme eines Teilsystems über Selektion auf der Karte löschen
Gegeben sei für Mandant 'Mandant_1' existieren folgende Massnahmen der Teilsysteme:
	| Id | Projektname                | Bezeichnung von | Bezeichnung bis | Teilsystem            | Zuständige Organisation | Beschreibung   | Kosten | Status          |
	| 1  | Projektname geändert       | Dorfplatz       | Gemeindeamt     | Öffentlicher Verkehr  | Gemeinde                | Alles geändert | 25000  | In Koordination |
	| 2  | Hauptstrasse Erneuerung    | Nr. 12          | Nr. 44          | Kunstbauten           | Strabag                 | Gleicher Name  | 20000  | Abgeschlossen   |
	| 3  | Dorferneuerungsaktion 2012 | Lindenweg 12    | Brunngasse 22   | Kommunikationsanlagen | Örtliches Bauamt        | Alles geändert | 10000  | Vorgeschlagen   |
Wenn ich die Massnahme der Teilsysteme mit der Id '2' lösche
Dann sind folgende Massnahmen der Teilsysteme im System:
	| Id | Projektname                | Bezeichnung von | Bezeichnung bis | Teilsystem            | Zuständige Organisation | Beschreibung   | Kosten | Status          |
	| 1  | Projektname geändert       | Dorfplatz       | Gemeindeamt     | Öffentlicher Verkehr  | Gemeinde                | Alles geändert | 25000  | In Koordination |
	| 3  | Dorferneuerungsaktion 2012 | Lindenweg 12    | Brunngasse 22   | Kommunikationsanlagen | Örtliches Bauamt        | Alles geändert | 10000  | Vorgeschlagen   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt Massnahmen der Teilsysteme in einer Übersichtsliste dar

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in einem Textfeld nach Projektname suchen
Gegeben sei für Mandant 'Mandant_1' existieren folgende Massnahmen der Teilsysteme:
	| Id | Projektname |
	| 1  | ABC         |
	| 2  | CDE         |
	| 3  | ABC DEF     |
Wenn ich nach Projektname 'DE' filtere
Dann werden folgende Massnahmen der Teilsysteme angezeigt:
	| Id | Projektname |
	| 2  | CDE         |
	| 3  | ABC DEF     |