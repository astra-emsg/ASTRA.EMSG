Funktionalität: A2 - Einen Jahresabschluss machen
Grundlage: 
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
| Id | Strassenname   | BezeichnungVon | BezeichnungBis |
| 1  | Kantonsstrasse | 0.0            | 1.0            |
| 2  | Landstrasse    | 0.0            | 1.0            |
Und es wurde noch kein Jahresabschluss für 2010 durchgeführt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System prüft, ob zu allen Strassenabschnitten eine Zustandsaufnahme vorliegt
Gegeben sei folgende Zustandsinformationen existieren:
| Id | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse |
| 1  | Kantonsstrasse | 0.0                   | 1.0                   |
Wenn ich den Jahresabschluss durchführe
Dann bekomme ich die Warnmeldung „Achtung: Nicht für alle Strassenabschnitte wurden Zustandsaufnahmen durchgeführt. Wollen Sie den Jahresabschluss trotzdem durchführen?“

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System setzt die erfassten Zustandsabschnitte auf read only

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System setzt die Realisierten Massnahmen auf read only

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System verwendet die Daten aus dem abgeschlossen Jahr für Auswertungen und Benchmarkauswertungen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System erzeugt eine Kopie der Strassenabschnitte und setzt die ursprünglichen auf read only
Wenn ich den Jahresabschluss für das Jahr 2010 durchführe
Und ich die Strassenabschnitte folgendermassen editiere
| Id | Strassenname   | BezeichnungVon | BezeichnungBis |
| 1  | Kantonsstrasse | 0.0            | 2.0            |
| 2  | Hauptstrasse   | 1.0            | 2.0            |
Und ich den Jahresabschluss für das Jahr 2011 durchführe
Dann zeigt die Auswertung Strassenabschnitte für das Jahr 2010 folgende Daten:
| Id | Strassenname   | BezeichnungVon | BezeichnungBis |
| 1  | Kantonsstrasse | 0.0            | 1.0            |
| 2  | Landstrasse    | 0.0            | 1.0            |
Und zeigt die Auswertung Strassenabschnitte für das Jahr 2010 folgende Daten:
| Id | Strassenname   | BezeichnungVon | BezeichnungBis |
| 1  | Kantonsstrasse | 0.0            | 1.0            |
| 2  | Landstrasse    | 0.0            | 1.0            |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System kopiert beim Jahresabschluss die erfassten Mengen und setzt die ursprünglichen auf read only
Wenn ich den Arbeitsmodus auf 'summarisch' ändere
Und ich folgende summarische Zustands- und Netzinformationen eingebe:
	| Belastungskategorie | Mengentyp    | Mittlerer Zustand | Menge |
	| IA                  | Gesamtfläche | 3,1               | 250   |
Und ich den Jahresabschluss für das Jahr 2010 durchführe
Dann zeigt die Auswertung Menge pro Belastungskategorie für das Jahr 2010 folgende Daten:
	| Belastungskategorie | Mengentyp    |  Menge |
	| IA                  | Gesamtfläche |  250   |
Und zeigt die Tabelle Zustands- und Netzinformation folgende Daten
	| Belastungskategorie | Mengentyp    | Mittlerer Zustand | Menge |
	| IA                  | Gesamtfläche | -                 | 250   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System übernimmt den Mengentypauch für eine neue Erfassungsperiode

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System löscht Daten, die in einem Arbeitsmodus erfasst wurden, der beim Durchführen des Jahresabschlusses nicht aktiv war
Wenn ich den Arbeitsmodus auf 'summarisch' ändere
Und ich den Jahresabschluss für das Jahr 2010 durchführe
Und ich den Arbeitsmodus auf 'strassennamen' ändere
Dann zeigt die Auswertung Strassenabschnitte für das Jahr 2010 folgende Daten:
| Id | Strassenname   | BezeichnungVon | BezeichnungBis |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System löscht Daten, die in einem Mengentyp erfasst wurden, der beim Durchführen des Jahresabschlusses nicht aktiv war

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System erzeugt eine Kopie von ausgewählten Systemparametern
- Wiederbeschaffungswert
- Alterungsbeiwert
- Massnahmenvorschläge
- Massnahmen

#------------------------------------------------------------------------------------------------------------------------------------------------------
