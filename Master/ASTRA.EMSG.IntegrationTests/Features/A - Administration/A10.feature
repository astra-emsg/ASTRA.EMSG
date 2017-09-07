Funktionalität: A10 - Andere Benutzerrollen einnehmen können
	Als Applikationssupporter,
	will ich andere Benutzerrollen einnehmen können
	damit ich bei Supportanfragen besser unterstützen kann
		
#------------------------------------------------------------------------------------------------------------------------------------------------------
Grundlage: 
Gegeben sei ich bin Applikationssupporter

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Der Applikationssupporter kann beliebige Rollen eines Mandanten einnehmen
	Wenn ich folgende Rolle einnehme
		 | Rolle                 |
		 | DataManager           |
		 | DataReader            |
		 | Benutzeradministrator |
		 | Benchmarkteilnehmer   |

	Und ich die Seite '<Seite>' öffne
	
	Dann habe ich Zugriff als
		 | DataManager    | DataReader    | Benutzeradministrator   | Benchmarkteilnehmer   | 
		 | <Data-Manager> | <Data-Reader> | <Benutzeradministrator> | <Benchmarkteilnehmer> | 
		
Beispiele: 
	| TF | Seite                                                                                                       | Data-Manager | Data-Reader | Benutzeradministrator | Benchmarkteilnehmer |
	| 1  | Strassenmenge und Zustand                                                                                   | ja           | nein        | nein                  | nein                |
	| 3  | Netzdefinition (strassennamen)                                                                              | ja           | nein        | nein                  | nein                |
	| 4  | Netzdefinition (gis)                                                                                        | ja           | nein        | nein                  | nein                |
	| 5  | Zustände und Massnahmenvorschläge (strassennamen)                                                           | ja           | nein        | nein                  | nein                |
	| 6  | Zustände und Massnahmenvorschläge (gis)                                                                     | ja           | nein        | nein                  | nein                |
	| 7  | Inspektionsrouten                                                                                           | ja           | nein        | nein                  | nein                |
	| 8  | Massnahmenvorschläge anderer Teilsysteme                                                                    | ja           | nein        | nein                  | nein                |
	| 9  | Koordinierte Massnahmen                                                                                     | ja           | nein        | nein                  | nein                |
	| 10 | Realisierte Massnehmen (summarisch)                                                                         | ja           | nein        | nein                  | nein                |
	| 11 | Realisierte Massnehmen (strassennamen)                                                                      | ja           | nein        | nein                  | nein                |
	| 12 | Realisierte Massnehmen (gis)                                                                                | ja           | nein        | nein                  | nein                |
	| 13 | Kenngrössen früherer Jahre (summarisch)                                                                     | ja           | nein        | nein                  | nein                |
	| 14 | Kenngrössen früherer Jahre (strassennamen)                                                                  | ja           | nein        | nein                  | nein                |
	| 15 | Kenngrössen früherer Jahre (gis)                                                                            | ja           | nein        | nein                  | nein                |
	| 16 | Inventarauswertungen (summarisch)                                                                           | nein         | ja          | nein                  | nein                |
	| 17 | Inventarauswertungen (strassennamen)                                                                        | nein         | ja          | nein                  | nein                |
	| 18 | Inventarauswertungen (gis)                                                                                  | nein         | ja          | nein                  | nein                |
	| 19 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (summarisch)                                           | nein         | ja          | nein                  | nein                |
	| 20 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (strassennamen)                                        | nein         | ja          | nein                  | nein                |
	| 21 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (gis)                                                  | nein         | ja          | nein                  | nein                |
	| 22 | Zustands- und Massnahmenvorschläge-Auswertungen (summarisch)                                                | nein         | ja          | nein                  | nein                |
	| 23 | Zustands- und Massnahmenvorschläge-Auswertungen (strassennamen)                                             | nein         | ja          | nein                  | nein                |
	| 24 | Zustands- und Massnahmenvorschläge-Auswertungen (gis)                                                       | nein         | ja          | nein                  | nein                |
	| 25 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (strassennamen) | nein         | ja          | nein                  | nein                |
	| 26 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (strassennamen) | nein         | ja          | nein                  | nein                |
	| 27 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (gis)           | nein         | ja          | nein                  | nein                |
	| 28 | Fortschreibungs-Auswertungen (summarisch)                                                                   | nein         | ja          | nein                  | nein                |
	| 29 | Fortschreibungs-Auswertungen (strassennamen)                                                                | nein         | ja          | nein                  | nein                |
	| 30 | Fortschreibungs-Auswertungen (gis)                                                                          | nein         | ja          | nein                  | nein                |
	| 31 | Arbeitsmodus wechseln (summarisch)                                                                          | nein         | nein        | ja                    | nein                |
	| 32 | Arbeitsmodus wechseln (strassennamen)                                                                       | nein         | nein        | ja                    | nein                |
	| 33 | Arbeitsmodus wechseln (gis)                                                                                 | nein         | nein        | ja                    | nein                |
	| 34 | Jahresabschluss (summarisch)                                                                                | nein         | nein        | ja                    | nein                |
	| 35 | Jahresabschluss (strassennamen)                                                                             | nein         | nein        | ja                    | nein                |
	| 36 | Jahresabschluss (gis)                                                                                       | nein         | nein        | ja                    | nein                |
	| 37 | Achsenupdates                                                                                               | nein         | nein        | ja                    | nein                |
	| 38 | Benchmarkteilnehmer (summarisch)                                                                            | nein         | nein        | ja                    | nein                |
	| 39 | Benchmarkteilnehmer (strassennamen)                                                                         | nein         | nein        | ja                    | nein                |
	| 40 | Benchmarkteilnehmer (gis)                                                                                   | nein         | nein        | ja                    | nein                |
	| 41 | Mandantenspezifische Werte (summarisch)                                                                     | nein         | nein        | ja                    | nein                |
	| 42 | Mandantenspezifische Werte (strassennamen)                                                                  | nein         | nein        | ja                    | nein                |
	| 43 | Mandantenspezifische Werte (gis)                                                                            | nein         | nein        | ja                    | nein                |
	| 44 | Benchmarking (summarisch)                                                                                   | nein         | nein        | nein                  | ja                  |
	| 45 | Benchmarking (strassennamen)                                                                                | nein         | nein        | nein                  | ja                  |
	| 46 | Benchmarking (gis)                                                                                          | nein         | nein        | nein                  | ja                  |
	| 47 | Systmparameter                                                                                              | nein         | nein        | nein                  | nein                |
	| 48 | Mandanten-Einstellungen                                                                                     | nein         | nein        | nein                  | nein                |
	| 49 | Applikations-Log                                                                                            | nein         | nein        | nein                  | nein                |
	| 50 | Rolle einnehmen                                                                                             | ja           | ja          | ja                    | ja                  | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Das System liefert dem Applikationssupporter dieselbe Sicht auf EMSG, die auch der Benutzer des Mandanten hat, der die entsprechende Rolle innehat
Wenn ich die Rolle von '<Mandant>' einnehme
Dann habe ich Zugriff als '<Mandant>'

Beispiele: 
| TF | Mandant  |
| 1  | Mandant1 |
| 2  | Mandant2 |
| 3  | Mandant3 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Führt der Applikationssupporter mit einer Rolle des Mandanten Objektmutationen durch, die im Anwendungs-Log protokolliert werden, so protokolliert das System jedenfalls den Namen des Applikationssupporters zur jeweiligen Objektmutation
