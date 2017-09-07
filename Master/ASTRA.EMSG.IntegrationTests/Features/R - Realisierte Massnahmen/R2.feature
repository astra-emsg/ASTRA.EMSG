Funktionalität: R2 - Realisierte Massnahmen im Strassennamenmodus erfassen
	Als Data-Manager,
	will ich realisierte Massnahmen im Strassennamenmodus erfassen
	damit ich in Auswertungen einen Überblick über meine Baumassnahmen bekomme

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann realisierte Massnahmen erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung  | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt 1   | Kirche          | Post            | 500   | 10              | 2,0                   | 1,5                    | Deckbelagserneuerung            | Alle Felder   | 20000           | 5000                  | 3000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name | 15000           | 4000                  | 2000                   |
		| 3  | Projekt 2   |                 |                 | 700   | 8               |                       |                        |                                 | Pflichtfelder |                 |                       |                        |

	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung  | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt 1   | Kirche          | Post            | 500   | 10              | 2,0                   | 1,5                    | Deckbelagserneuerung            | Alle Felder   | 20000           | 5000                  | 3000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name | 15000           | 4000                  | 2000                   |
		| 3  | Projekt 2   |                 |                 | 700   | 8               |                       |                        |                                 | Pflichtfelder |                 |                       |                        |
		
#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager die realisierte Massnahme gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
		| Id | Projektname | Länge | Breite Fahrbahn | 
		| 1  | Projekt 1   | 500   | 10              | 
		| 2  | Projekt 2   | 600   | 9               | 

	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Fläche Fahrbahn |
		| 1  | Projekt 1   | 5000            |
		| 2  | Projekt 2   | 5400            |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager die realisierte Massnahme gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
		| Id | Projektname | Länge | Breite Trottoir links |
		| 1  | Projekt 1   | 500   | 2,0                   |
		| 2  | Projekt 2   | 600   | 1,5                   |
									                        
	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Fläche Trottoir rechts | 
		| 1  | Projekt 1   | 1000                   | 
		| 2  | Projekt 2   | 900                    | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager die realisierte Massnahme gespeichert hat
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende realisierte Massnahme eingebe:
		| Id | Projektname | Länge | Breite Trottoir rechts |
		| 1  | Projekt 1   | 500   | 1,5                    |
		| 2  | Projekt 2   | 600   | 1,1                    |

	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Fläche Trottoir rechts |
		| 1  | Projekt 1   | 750                    |
		| 2  | Projekt 2   | 660                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann alle Attribute der realisierten Massnahme bearbeiten
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name  | 15000           | 4000                  | 2000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |

	Wenn ich folgende folgende realisierte Massnahme eingebe:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name  | 15000           | 4000                  | 2000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |

	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name  | 15000           | 4000                  | 2000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |
		
#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Der Data-Manager kann eine realisierte Massnahme löschen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name  | 15000           | 4000                  | 2000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |

	Wenn ich die realisierte Massname mit der Id '2' lösche

	Dann sind folgende realisierte Massnahmen im System:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt die Konsistenz der Trottoir Einträge zu „Breite“ und „Kosten“ sicher
# manual test cases could be added here

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System fordert den Data-Manager nach dem Speichern der realisierten Massnahme zur Anpassung der Zustände auf

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System listet alle realisierten Massnahmen in einer Übersichtstabelle

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in einem Textfeld nach Projektname suchen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
	    | Id | Projektname |
	    | 1  | ABC         |
	    | 2  | CDE         |
	    | 3  | ABC DEF     |
	
	Und ich nach Projektname 'DE' filtere

	Dann werden folgende realisierte Massnahmen angezeigt:
	    | Id | Projektname |
		| 2  | CDE         |
		| 3  | ABC DEF     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Beim Jahresabschluss werden die erfassten realisierten Massnahmen vom System dem Jahr des Jahresabschlusses zugeordnet
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus         |
		| Mandant_1 | strassennamen |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende realisierte Massnahmen existieren:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |
		| 1  | Projekt A   | Post            | Kirche          | 400   | 7               | 1,5                   | 2,0                    | Erneuerung Oberbau              | Alles geändert | 25000           |                       | 4000                   |
		| 2  | Projekt 1   | Post            | Bahnhof         | 600   | 9               | 1,5                   | 1,1                    | Erneuerung Oberbau              | Gleicher Name  | 15000           | 4000                  | 2000                   |
		| 3  | Projekt B   | Kirche          | Friedhof        | 800   | 6               | 1,6                   | 1,8                    | Deckbelagserneuerung            | Alles geändert | 10000           | 3000                  | 4000                   |

	Und ich einen Jahresabschluss für das Jahr '2010' durchführe

	Dann werden folgende realisierte Massnahmen angezeigt:
		| Id | Projektname | Bezeichnung von | Bezeichnung bis | Länge | Breite Fahrbahn | Breite Trottoir links | Breite Trottoir rechts | Massnahmenbeschreibung Fahrbahn | Beschreibung   | Kosten Fahrbahn | Kosten Trottoir links | Kosten Trottoir rechts |