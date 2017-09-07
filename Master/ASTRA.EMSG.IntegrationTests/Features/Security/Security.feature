@Automatisch
Funktionalität: Security
	Als EMSG-Benutzer
	will ich Zugriff auf die Bereiche der Fachapplikation haben, die ich gemäss meiner Rolle bearbeiten darf
	damit ich meine Aufgaben mittels EMSG erfüllen kann

Szenariogrundriss: Security
	Gegeben sei ich bin Benutzer von EMSG
        Und folgende Einstellungen existieren:
		    | Mandant | Modus   | Mengentyp   |
		    | Mandant | <Modus> | Gesamtlänge |
	
	    Und ich bin Data-Manager von 'Mandant'
	
	Wenn ich die Seite '<Seite>' öffne
	
	Dann habe ich Zugriff als:
		 | DataManager    | DataReader    | Benutzeradministrator   | Benchmarkteilnehmer   | Applikationsadministrator   | Applikationssupporter   |
		 | <Data-Manager> | <Data-Reader> | <Benutzeradministrator> | <Benchmarkteilnehmer> | <Applikationsadministrator> | <Applikationssupporter> |
	
Beispiele: 
	| TF | Seite                                                                                                       | Data-Manager | Data-Reader | Benutzeradministrator | Benchmarkteilnehmer | Applikationsadministrator | Applikationssupporter | Modus         |
	| 1  | Strassenmenge und Zustand                                                                                   | ja           | nein        | nein                  | nein                | nein                      | nein                  | summarisch    |
	| 3  | Netzdefinition (strassennamen)                                                                              | ja           | nein        | nein                  | nein                | nein                      | nein                  | strassennamen |
	| 4  | Netzdefinition (gis)                                                                                        | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 5  | Zustände und Massnahmenvorschläge (strassennamen)                                                           | ja           | nein        | nein                  | nein                | nein                      | nein                  | strassennamen |
	| 6  | Zustände und Massnahmenvorschläge (gis)                                                                     | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 7  | Inspektionsrouten                                                                                           | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 8  | Massnahmenvorschläge anderer Teilsysteme                                                                    | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 9  | Koordinierte Massnahmen                                                                                     | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 10 | Realisierte Massnehmen (summarisch)                                                                         | ja           | nein        | nein                  | nein                | nein                      | nein                  | summarisch    |
	| 11 | Realisierte Massnehmen (strassennamen)                                                                      | ja           | nein        | nein                  | nein                | nein                      | nein                  | strassennamen |
	| 12 | Realisierte Massnehmen (gis)                                                                                | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
	| 13 | Kenngrössen früherer Jahre (summarisch)                                                                     | ja           | nein        | nein                  | nein                | nein                      | nein                  | summarisch    |
	| 14 | Kenngrössen früherer Jahre (strassennamen)                                                                  | ja           | nein        | nein                  | nein                | nein                      | nein                  | strassennamen |
	| 15 | Kenngrössen früherer Jahre (gis)                                                                            | ja           | nein        | nein                  | nein                | nein                      | nein                  | gis           |
#	| 16 | Inventarauswertungen (summarisch)                                                                           | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 17 | Inventarauswertungen (strassennamen)                                                                        | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 18 | Inventarauswertungen (gis)                                                                                  | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 19 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (summarisch)                                           | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 20 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (strassennamen)                                        | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 21 | Wiederbeschaffungswert- und Wertverlust-Auswertungen (gis)                                                  | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 22 | Zustands- und Massnahmenvorschläge-Auswertungen (summarisch)                                                | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 23 | Zustands- und Massnahmenvorschläge-Auswertungen (strassennamen)                                             | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 24 | Zustands- und Massnahmenvorschläge-Auswertungen (gis)                                                       | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 25 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (strassennamen) | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 26 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (strassennamen) | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 27 | Massnahmenvorschläge anderer Teilsysteme und koordinierte Massnahmenvorschläge-Auswertungen (gis)           | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 28 | Fortschreibungs-Auswertungen (summarisch)                                                                   | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 29 | Fortschreibungs-Auswertungen (strassennamen)                                                                | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 30 | Fortschreibungs-Auswertungen (gis)                                                                          | nein         | ja          | nein                  | nein                | nein                      | nein                  |
#	| 31 | Arbeitsmodus wechseln (summarisch)                                                                          | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 32 | Arbeitsmodus wechseln (strassennamen)                                                                       | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 33 | Arbeitsmodus wechseln (gis)                                                                                 | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 34 | Jahresabschluss (summarisch)                                                                                | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 35 | Jahresabschluss (strassennamen)                                                                             | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 36 | Jahresabschluss (gis)                                                                                       | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 37 | Achsenupdates                                                                                               | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 38 | Benchmarkteilnehmer (summarisch)                                                                            | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 39 | Benchmarkteilnehmer (strassennamen)                                                                         | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 40 | Benchmarkteilnehmer (gis)                                                                                   | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 41 | Mandantenspezifische Werte (summarisch)                                                                     | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 42 | Mandantenspezifische Werte (strassennamen)                                                                  | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 43 | Mandantenspezifische Werte (gis)                                                                            | nein         | nein        | ja                    | nein                | nein                      | nein                  |
#	| 44 | Benchmarking (summarisch)                                                                                   | nein         | nein        | nein                  | ja                  | nein                      | nein                  |
#	| 45 | Benchmarking (strassennamen)                                                                                | nein         | nein        | nein                  | ja                  | nein                      | nein                  |
#	| 46 | Benchmarking (gis)                                                                                          | nein         | nein        | nein                  | ja                  | nein                      | nein                  |
#	| 47 | Systmparameter                                                                                              | nein         | nein        | nein                  | nein                | ja                        | nein                  |
#	| 48 | Mandanten-Einstellungen                                                                                     | nein         | nein        | nein                  | nein                | ja                        | nein                  |
#	| 49 | Applikations-Log                                                                                            | nein         | nein        | nein                  | nein                | ja                        | nein                  |
#	| 50 | Rolle einnehmen                                                                                             | nein         | nein        | nein                  | nein                | nein                      | ja                    |

#-------------------------------------------------------------------------------------------------------------------------------

