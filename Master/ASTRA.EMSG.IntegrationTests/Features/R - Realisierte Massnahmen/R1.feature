Funktionalität: R1 - Realisierte Massnahmen im summarischen Modus erfassen
	Als Data-Manager,
	will ich realisierte Massnahmen im summarischen Modus erfassen
	damit ich in Auswertungen einen Überblick über meine Baumassnahmen bekomme

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann realisierte Massnahmen erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      |
		| Mandant_1 | summarisch |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
	    | Mengentyp   | Projektname | Beschreibung      | Kosten Fahrbahn | Belastungskategorie | Menge |
	    | Gesamtlänge | Projekt 1   | Alle Felder       | 10000           | 1A                  | 1000  |
	    | Gesamtlänge | Projekt 1   | Gleicher Name     |                 |                     |       |
	    | Gesamtlänge | Projekt 2   | Nur Pflichtfelder |                 |                     |       |
		
	Dann sind folgende realisierte Massnahmen im System:
		| Mandant   | Mengentyp   | Projektname | Beschreibung      | Kosten Fahrbahn | Belastungskategorie | Menge Gesamtlänge |
		| Mandant_1 | Gesamtlänge | Projekt 1   | Alle Felder       | 10000           | 1A                  | 1000              |
		| Mandant_1 | Gesamtlänge | Projekt 1   | Gleicher Name     |                 |                     |                   |
		| Mandant_1 | Gesamtlänge | Projekt 2   | Nur Pflichtfelder |                 |                     |                   |
		
#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann entweder die ‚Gesamtlänge‘ oder ‚Gesamtfläche der Fahrbahnen‘ oder ‚Gesamtfläche der Fahrbahnen und Trottoirs‘ der Massnahme (Mengentyp entsprechend der Selektion des Benutzeradministrators) erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      |
		| Mandant_1 | summarisch |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
	    | Id | Mengentyp                 | Projektname  | Menge |
	    | 1  | Gesamtlänge               | Projekt L    | 300   |
	    | 2  | Gesamtfläche              | Projekt F    | 300   |
		
	Dann sind folgende realisierte Massnahmen im System:
		| Id | Mandant   | Mengentyp                 | Projektname  | Menge Gesamtlänge | Menge Gesamtfläche | Menge Gesamtfläche und Trottoir |
		| 1  | Mandant_1 | Gesamtlänge               | Projekt L    | 300               | -                  | -                               |
		| 2  | Mandant_1 | Gesamtfläche              | Projekt F    | -                 | 300                | -                               |
		
#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann alle Attribute der realisierten Massnahme bearbeiten
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp   |
		| Mandant_1 | summarisch | <Mengentyp> |

	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
	    | Id | Mengentyp   | Projektname | Beschreibung      | Kosten Fahrbahn | Belastungskategorie | Menge |
	    | 1  | Gesamtlänge | Projekt 1   | Alle Felder       | 10000           | 1A                  | 1000  |
	    | 2  | Gesamtlänge | Projekt 1   | Gleicher Name     |                 |                     |       |
	    | 3  | Gesamtlänge | Projekt 2   | Nur Pflichtfelder |                 |                     |       |
	
	Wenn ich folgende folgende realisierte Massnahme eingebe:
	    | Id | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge |
	    | 1  | Projekt A   | Alles geändert | 20000           | 1C                  | 10000 |
	    | 2  | Projekt 1   | Gleicher Name  |                 |                     |       |
	    | 3  | Projekt B   | Alles geändert | 10000           | 2                   | 2000  |
	
	Dann sind folgende realisierte Massnahmen im System:
	    | Id | Mengentyp   | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge Gesamtlänge |
	    | 1  | Gesamtlänge | Projekt A   | Alles geändert | 20000           | 1C                  | 10000             |
	    | 2  | Gesamtlänge | Projekt 1   | Gleicher Name  |                 |                     |                   |
	    | 3  | Gesamtlänge | Projekt B   | Alles geändert | 10000           | 2                   | 2000              |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann eine realisierte Massnahme löschen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp   |
		| Mandant_1 | summarisch | <Mengentyp> |

	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
	    | Id | Mengentyp   | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge |
	    | 1  | Gesamtlänge | Projekt A   | Alles geändert | 20000           | 1C                  | 10000 |
	    | 2  | Gesamtlänge | Projekt 1   | Gleicher Name  |                 |                     |       |
	    | 3  | Gesamtlänge | Projekt B   | Alles geändert | 10000           | 2                   | 2000  |
	
	Wenn ich die realisierte Massname mit der Id '2' lösche

	Dann sind folgende realisierte Massnahmen im System:
	    | Id | Mengentyp   | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge Gesamtlänge |
	    | 1  | Gesamtlänge | Projekt A   | Alles geändert | 20000           | 1C                  | 10000             |
	    | 3  | Gesamtlänge | Projekt B   | Alles geändert | 10000           | 2                   | 2000              |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System fordert den Data-Manager nach Speichern der realisierten Massnahme zur Anpassung des mittleren Zustands auf

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System listet alle realisierten Massnahmen in einer Übersichtstabelle

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in einem Textfeld nach Projektname suchen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp   |
		| Mandant_1 | summarisch | <Mengentyp> |

	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
	    | Id | Mengentyp   | Projektname |
	    | 1  | Gesamtlänge | ABC         |
	    | 2  | Gesamtlänge | CDE         |
	    | 3  | Gesamtlänge | ABC DEF     |
	
	Und ich nach Projektname 'DE' filtere

	Dann werden folgende realisierte Massnahmen angezeigt:
	    | Id | Mengentyp   | Projektname |
		| 2  | Gesamtlänge | CDE         |
		| 3  | Gesamtlänge | ABC DEF     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Beim Jahresabschluss werden die erfassten realisierten Massnahmen vom System dem Jahr des Jahresabschlusses zugeordnet
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp   |
		| Mandant_1 | summarisch | <Mengentyp> |

	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
	    | Id | Mengentyp   | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge |
	    | 1  | Gesamtlänge | Projekt A   | Alles geändert | 20000           | 1C                  | 10000 |
	    | 2  | Gesamtlänge | Projekt 1   | Gleicher Name  |                 |                     |       |
	    | 3  | Gesamtlänge | Projekt B   | Alles geändert | 10000           | 2                   | 2000  |

	Und ich einen Jahresabschluss für das Jahr '2010' durchführe

	Dann werden folgende realisierte Massnahmen angezeigt:
	    | Id | Mengentyp   | Projektname | Beschreibung   | Kosten Fahrbahn | Belastungskategorie | Menge Gesamtlänge |