Funktionalität: Z2.1 - Zustände und Massnahmenvorschläge über das UI erfassen
	Als Data-Manager
	will ich Zustände und Massnahmenvorschläge über das UI erfassen
	damit ich einen Überblick zum Zustand und eine Basis für die Planung meiner Massnahmen habe

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und für Mandant 'Mandant_1' existieren folgende Netzinformationen:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | Belag   | BreiteFahrbahn | Laenge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts |
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | Asphalt | 4,5            | 100    | Links            | 2,5                 | -                    |
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | Beton   | 5,75           | 200    | NochNichtErfasst | -                   | -                    |
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | Beton   | 7              | 300    | KeinTrottoir     | -                   | -                    |
	| 4  | Gartenstrasse   | 1                | 66             | Asphalt | 5              | 900    | BeideSeiten      | 2                   | 1,5                  |

Und für Mandant 'Mandant_1' existieren folgende Zustandsinformationen:
	| Id | Strassenabschnitt | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter    | Zustandsindex | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahnTyp | MassnahmenvorschlagFahrbahnDringlichkeit | MassnahmenvorschlagTrottoirLinksTyp | MassnahmenvorschlagTrottoirLinksDringlichkeit | MassnahmenvorschlagTrottoirRechtsTyp | MassnahmenvorschlagTrottoirRechtsDringlichkeit |
	| 5  | 1                 | Nr. 1          | Nr. 7          | 1000  | 14.12.2009    | Drei         | Regen     | 2,3           | -                          | -                           | Oberflaechenverbesserung       | Dringlich                                | -                                   | -                                             | -                                    | -                                              |
	| 6  | 1                 | Nr. 8          | Nr. 12         | 800   | 14.12.2009    | Drei         | Regen     | 2,3           | -                          | -                           | Deckbelagserneuerung           | Dringlich                                | -                                   | -                                             | -                                    | -                                              |
	| 7  | 1                 | Nr. 13         | Nr. 55         | 5000  | 23.03.2009    | A            | KeinRegen | 1,2           | Gut                        | -                           | Belagserneuerung               | Mittelfristig                            | -                                   | -                                             | -                                    | -                                              |
	| 8  | 2                 | 0.0            | 2.1            | 2100  | 21.05.2009    | B            | KeinRegen | 1,1           | -                          | -                           | ErneuerungOberbau              | Langfristig                              | -                                   | -                                             | -                                    | -                                              |
	| 9  | 2                 | 2.1            | 5.3            | 300   | 21.05.2009    | B            | KeinRegen | 1,1           | -                          | -                           | ErneuerungOberbau              | Langfristig                              | -                                   | -                                             | -                                    | -                                              |
	| 10 | 2                 | 5.4            | 7.1            | 100   | 21.05.2009    | B            | KeinRegen | 2,1           | -                          | -                           | ErneuerungOberbau              | Langfristig                              | -                                   | -                                             | -                                    | -                                              |
	| 11 | 3                 | Brunner        | Maier          | 1300  | 21.10.2009    | Meyer        | KeinRegen | 3,4           | -                          | -                           | -                              | -                                        | -                                   | -                                             | -                                    | -                                              |
	| 12 | 4                 | 1              | 66             | 700   | 12.01.2012    | -            | KeinRegen | 3             | Ausreichend                | Mittel                      | -                              | -                                        | Erneuerung                          | Dringlich                                     | Erneuerung                           | Langfristig                                    |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@ToBind
@Automatisch
Szenariogrundriss: Der Data-Manager kann Zustandsabschnitte erfassen
    #Gegeben sei ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'

	Wenn ich für Id '4' folgende Zustandsabschnitte erfasse
		| BezeichnungVon   | BezeichnungBis   | Länge   | Aufnahmedatum   | Aufnahmeteam   | Wetter   | Bemerkung   | Zustandsindex |
		| <BezeichnungVon> | <BezeichnungBis> | <Länge> | <Aufnahmedatum> | <Aufnahmeteam> | <Wetter> | <Bemerkung> | 1             |

	Dann liefert Feldbezeichnung '<Feldbezeichnung>' einen Validationsfehler '<Validationsfehler>'
	
	Dann sind folgende Zustandsabschnitte im System
		| Strassenabschnitt   | BezeichnungVon   | BezeichnungBis   | Länge   | FlächeFahrbahn   | FlächeTrottoirLinks   | FlächeTrottoirRechts   | Aufnahmedatum   | Aufnahmeteam   | Wetter   | Bemerkung   |
		| <Strassenabschnitt> | <BezeichnungVon> | <BezeichnungBis> | <Länge> | <FlächeFahrbahn> | <FlächeTrottoirLinks> | <FlächeTrottoirRechts> | <Aufnahmedatum> | <Aufnahmeteam> | <Wetter> | <Bemerkung> |
     
	Beispiele:
		| TF | Strassenabschnitt | BezeichnungVon | BezeichnungBis | Länge   | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts | Aufnahmedatum | Aufnahmeteam | Wetter    | Bemerkung | Validationsfehler | Feldbezeichnung   | Kommentar                               |
		| 1  | 4                 | Post           | Bank           | 200     | 1000           | 400                 | 300                  | 12.01.2012    | Müller       | Regen     | keine     | Nein              | -                 | Gutfall                                 |
		| 2  | 4                 | Post           | Bank           | 40,5    | 202,5          | 81                  | 60,75                | 12.01.2012    | Müller       | KeinRegen | -         | Nein              | -                 | Gutfall                                 |
		| 3  | 4                 | Post           | Bank           | 100,253 | -              | -                   | -                    | 12.01.2012    | Müller       | Regen     | keine     | Ja                | Stammdaten.Laenge | Ungültige Länge max. 2 Nachkommastellen |
		| 4  | 4                 | Post           | Bank           | -20     | -              | -                   | -                    | 12.01.2012    | Müller       | Regen     | keine     | Ja                | Stammdaten.Laenge | Ungültige Länge < 0                     |		

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt muss immer einem Strassenabschnitt zugeordnet sein

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt erbt ‚Strassenname‘, ‚Belagsart‘, ‚Breite Fahrbahn‘, ‚Trottoir‘, ‚Breite Trottoir links‘ und ‚Breite Trottoir rechts‘ vom Strassenabschnitt
Wenn ich einen für folgende Strassenabschnitte einen neuen Zustandsabschnitt anlege:
		| Id |
		| 3  |
		| 4  |
Dann existieren folgende Zustandsinformationen für die Strasse mit der Id 4:
		| Strassenname  | Belagsart | BreiteFahrbahn | Trottoir    | BreiteTrottoirLinks | BreiteTrottoirRechts |
		| Föhrenweg     | Beton     | 7              | Keines      | -                   | -                    |
		| Gartenstrasse | Asphalt   | 5              | BeideSeiten | 2                   | 1,5                  |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Angaben zum Trottoir wie Zustandsindex, Erneuerung usw. können nur erfasst werden, sofern der Strassenabschnitt auch einen entsprechenden Trottoir(typ) aufweist 

# ACHTUNG: Konsistenzcheck Trottoir noch nicht berücksichtigt (Änderungsantrag 02 wird nicht in Iteration 1 umgesetzt!)

#---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann alle Attribute des Zustandsabschnitts bearbeiten (gilt nicht für Attribute, die als read only ausgewiesen sind)
Wenn ich die Zustandsabschnitte für die Strasse mit der Id '1' folgendermassen editiere:
| Id | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 5  | Anfang         | Mitte          | 100   | 12.01.2012    | Müller       | Nass    | 0                     | Gut                        | -                           | -                           | -                                        | -                       | -                                    | -                        | -                                     |
| 6  | Mitte          | Ende           | 20    | 13.04.2011    | Huber        | Trocken | 4,5                   | Ausreichend                | -                           | Oberflächenverbesserung     | dringlich                                | ja                      | mittelfristig                        | -                        | -                                     |

Dann existieren folgende Zustandsinformationen für die Strasse mit der Id '3':
| Id | Strassenname     | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn                 | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 5  | Jesuitenbachweg  | Hackl                 | Schweighofer          | Anfang         | Mitte          | 100   | 12.01.2012    | Müller       | Nass    | 0                     | Gut                        | -                           | -                                           | -                                        | -                       | -                                    | -                        | -                                     |
| 6  | Jesuitenbachweg  | Hackl                 | Schweighofer          | Mitte          | Ende           | 20    | 13.04.2011    | Huber        | Trocken | 4,5                   | Ausreichend                | -                           | Oberflächenverbesserung                     | dringlich                                | ja                      | mittelfristig                        |                          |                                       |
| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 13         | Nr. 55         | 5000  | 23.03.2009    | A            | Trocken | 1,2                   | Gut                        | -                           | Belagserneuerung mit teilweiser Verstärkung | mittelfristig                            | -                       | -                                    | -                        | -                                     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass die Summe der Längen der Zustandsabschnitte nicht grösser ist, als die Länge des Strassenabschnitts

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das Zustandserfassungsformular (Erfassungsformular für Oberflächenschäden) ausfüllen
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '1' öffne
Dann habe ich folgende Möglichkeiten zur Auswahl:
| Möglichkeit                    |
| Erfassung des Zustandsindex    |
| Grobe Zustandserfassung        |
| Detaillierte Zustandserfassung |
Und sehe folgende Informationen:
| Strassenname    | BezeichnungVon | BezeichnungBis |
| Jesuitenbachweg | Hackl          | Schweighofer   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Unabhängig von der Art der Zustandserfassung werden der „Strassenname“ die „Bezeichnung von“ sowie die „Bezeichnung bis“ des Zustandsabschnitts read only angezeigt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt bei grober und detaillierter Erfassung das Formular entsprechend der Belagsart des Strassenabschnitts dar

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Erfassungsformular Oberflächenschäden Betonbelag (Beton) - detailliert
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '2' öffne
Dann sehe ich das Schadenerfassungsformular mit folgenden Möglichkeiten:
| Kategorie                | Schaden                             |
| Oberflächenglätte        | Polieren                            |
| Materialverluste         | Abrieb                              |
| Materialverluste         | Abblätterung                        |
| Materialverluste         | Abplatzungen                        |
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        |
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss |
| Vertikalverschiebung     | Setzungen, Frosthebungen            |
| Vertikalverschiebung     | Stufenbildung                       |
| Vertikalverschiebung     | Pumpen                              |
| Vertikalverschiebung     | Blow-up                             |
| Risse, Brüche            | Risse                               |
| Risse, Brüche            | Zerstörte Platten                   |
| Flicke                   | Flicke                              |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Erfassungsformular Oberflächenschäden bitumenhaltiger Belag (Asphalt) - detailliert
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '1' öffne
Dann sehe ich das Schadenerfassungsformular mit folgenden Möglichkeiten:
| Kategorie            | Schaden                 |
| Oberflächenglätte    | Polieren                |
| Oberflächenglätte    | Schwitzen               |
| Belagschäden         | Abrieb                  |
| Belagschäden         | Ausmagerung, Absanden   |
| Belagschäden         | Kornausbrüche           |
| Belagschäden         | Ablösungen              |
| Belagschäden         | Schlaglöcher            |
| Belagschäden         | Offene Nähte            |
| Belagschäden         | Querrisse               |
| Belagschäden         | Wilde Risse             |
| Belagsverformungen   | Spurrinnen              |
| Belagsverformungen   | Aufwölbungen            |
| Belagsverformungen   | Wellblechverformungen   |
| Belagsverformungen   | Schubverformungen       |
| Strukturelle Schäden | Anrisse von Setzungen   |
| Strukturelle Schäden | Setzungen, Einsenkungen |
| Strukturelle Schäden | Abgedrückte Ränder      |
| Strukturelle Schäden | Frosthebungen           |
| Strukturelle Schäden | Längsrisse              |
| Strukturelle Schäden | Netzrisse               |
| Strukturelle Schäden | Belagsrandrisse         |
| Flicke               | Flicke                  |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann die Schadenschwere erfassen
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '1' öffne
Dann sehe ich das Schadenerfassungsformular mit folgenden Möglichkeiten:
| Schwere |
| S0      |
| S1      |
| S2      |
| S3      |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das Schadenausmass erfassen
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '1' öffne
Dann sehe ich das Schadenerfassungsformular mit folgenden Möglichkeiten:
| Schadenausmass |
| A1             |
| A2             |
| A3             |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System errechnet die Matrixwerte
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '2' öffne
Und ich folgende Schäden erfasse:
| Kategorie                | Schaden                             | Schwere | Schadenausmass |
| Oberflächenglätte        | Polieren                            | S0      | A1             |
| Materialverluste         | Abrieb                              | S1      | A1             |
| Materialverluste         | Abblätterung                        | S2      | A1             |
| Materialverluste         | Abplatzungen                        | S3      | A1             |
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        | S0      | A2             |
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss | S1      | A2             |
| Vertikalverschiebung     | Setzungen, Frosthebungen            | S2      | A2             |
| Vertikalverschiebung     | Stufenbildung                       | S3      | A2             |
| Vertikalverschiebung     | Pumpen                              | S0      | A3             |
| Vertikalverschiebung     | Blow-up                             | S1      | A3             |
| Risse, Brüche            | Risse                               | S2      | A3             |
| Risse, Brüche            | Zerstörte Platten                   | S3      | A3             |
| Flicke                   | Flicke                              | S0      | A1             |
Und ich die Schäden speichere
Dann sehe ich folgende Matrixwerte:
| Kategorie                | Schaden                             | Matrix | 
| Oberflächenglätte        | Polieren                            | 0      | 
| Materialverluste         | Abrieb                              | 1      | 
| Materialverluste         | Abblätterung                        | 2      | 
| Materialverluste         | Abplatzungen                        | 3      | 
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        | 0      | 
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss | 2      | 
| Vertikalverschiebung     | Setzungen, Frosthebungen            | 4      | 
| Vertikalverschiebung     | Stufenbildung                       | 6      | 
| Vertikalverschiebung     | Pumpen                              | 0      | 
| Vertikalverschiebung     | Blow-up                             | 3      | 
| Risse, Brüche            | Risse                               | 6      | 
| Risse, Brüche            | Zerstörte Platten                   | 9      | 
| Flicke                   | Flicke                              | 0      | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Schadenbewertung
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '2' öffne
Und ich folgende Schäden erfasse:
| Kategorie                | Schaden                             | Schwere | Schadenausmass |
| Oberflächenglätte        | Polieren                            | S0      | A1             |
| Materialverluste         | Abrieb                              | S1      | A1             |
| Materialverluste         | Abblätterung                        | S2      | A1             |
| Materialverluste         | Abplatzungen                        | S3      | A1             |
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        | S0      | A2             |
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss | S1      | A2             |
| Vertikalverschiebung     | Setzungen, Frosthebungen            | S2      | A2             |
| Vertikalverschiebung     | Stufenbildung                       | S3      | A2             |
| Vertikalverschiebung     | Pumpen                              | S0      | A3             |
| Vertikalverschiebung     | Blow-up                             | S1      | A3             |
| Risse, Brüche            | Risse                               | S2      | A3             |
| Risse, Brüche            | Zerstörte Platten                   | S3      | A3             |
| Flicke                   | Flicke                              | S0      | A1             |
Und ich die Schäden speichere
Dann sehe ich folgende Schadenbewertung:
| Kategorie                | Bewertung |
| Oberflächenglätte        | 0         |
| Materialverluste         | 6         |
| Fugen- und Kantenschäden | 2         |
| Vertikalverschiebung     | 18        |
| Risse, Brüche            | 18        |
| Flicke                   | 0         |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Schadensumme bei grober und detaillierter Erfassung
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '2' öffne
Und ich folgende Schäden erfasse:
| Kategorie                | Schaden                             | Schwere | Schadenausmass |
| Oberflächenglätte        | Polieren                            | S0      | A1             |
| Materialverluste         | Abrieb                              | S1      | A1             |
| Materialverluste         | Abblätterung                        | S2      | A1             |
| Materialverluste         | Abplatzungen                        | S3      | A1             |
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        | S0      | A2             |
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss | S1      | A2             |
| Vertikalverschiebung     | Setzungen, Frosthebungen            | S2      | A2             |
| Vertikalverschiebung     | Stufenbildung                       | S3      | A2             |
| Vertikalverschiebung     | Pumpen                              | S0      | A3             |
| Vertikalverschiebung     | Blow-up                             | S1      | A3             |
| Risse, Brüche            | Risse                               | S2      | A3             |
| Risse, Brüche            | Zerstörte Platten                   | S3      | A3             |
| Flicke                   | Flicke                              | S0      | A1             |
Und ich die Schäden speichere
Dann sehe ich die Schadenssumme '44'

#------------------------------------------------------------------------------------------------------------------------------------------------------


@Manuell
Szenario: Das System berechnet den Zustandsindex (I1) bei grober und detaillierter Erfassung
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '2' öffne
Und ich folgende Schäden erfasse:
| Kategorie                | Schaden                             | Schwere | Schadenausmass |
| Oberflächenglätte        | Polieren                            | S0      | A1             |
| Materialverluste         | Abrieb                              | S1      | A1             |
| Materialverluste         | Abblätterung                        | S2      | A1             |
| Materialverluste         | Abplatzungen                        | S3      | A1             |
| Fugen- und Kantenschäden | Kantenschäden, Absplitterung        | S0      | A2             |
| Fugen- und Kantenschäden | Fehlender oder spröder Fugenverguss | S1      | A2             |
| Vertikalverschiebung     | Setzungen, Frosthebungen            | S2      | A2             |
| Vertikalverschiebung     | Stufenbildung                       | S3      | A2             |
| Vertikalverschiebung     | Pumpen                              | S0      | A3             |
| Vertikalverschiebung     | Blow-up                             | S1      | A3             |
| Risse, Brüche            | Risse                               | S2      | A3             |
| Risse, Brüche            | Zerstörte Platten                   | S3      | A3             |
| Flicke                   | Flicke                              | S0      | A1             |
Und ich die Schäden speichere
Und ich das Formular schliesse
Dann sehe ich für den Zustandsabschnitt mit der Id '2' den Zustandsindex '4.4'

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Zustandsabschnitte über die Übersichtstabelle löschen
	Wenn ich den Zustandsabschnitt Id 7 lösche
	Dann sind folgende Zustandsabschnitte im System:
	| Id |
	| 5  |
	| 6  |
	| 8  |
	| 9  |
	| 10 |
	| 11 |
	| 12 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
	Wenn ich die Zustandsabschnitte für die Strasse mit der Id '1' folgendermassen editiere:
		| Id | Länge |
		| 5  | 150   |
		| 6  | 888,5 |

	Dann werden folgende Zustandsinformationen für die Strasse mit der Id '3' angezeigt:
		| Id | FlächeFahrbahn |
		| 5  | 675            |
		| 6  | 3998           |
		| 7  | 22500          |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
	Wenn ich die Zustandsabschnitte für die Strasse mit der Id '1' folgendermassen editiere:
		| Id | Länge |
		| 5  | 150   |
		| 6  | 888,5 |
	Dann werden folgende Zustandsinformationen für die Strasse mit der Id '3' angezeigt:
		| Id | FlächeTrottoirLinks |
		| 5  | 375                 |
		| 6  | 2221                |
		| 7  | 12500               |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
	Wenn ich die Zustandsabschnitte für die Strasse mit der Id '4' folgendermassen editiere:
		| Id | Länge |
		| 12  | 500  |
	Dann werden folgende Zustandsinformationen für die Strasse mit der Id '4' angezeigt:
		| Id | FlächeTrottoirLinks |
		| 12 | 750                 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann zu jedem Zustandsabschnitt einen Massnahmenvorschlag erfassen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Das System berechnet die Kosten für den gewählten Massnahmenvorschlag
	Wenn folgende Kosten pro m² für 'Mandant_1' gelten:
	| Massnahme                                   | Kosten |
	| Oberflächenverbesserung                     | 50     |
	| Deckbelagserneuerung                        | 100    |
	| Belagserneuerung mit teilweiser Verstärkung | 200    |
	| Erneuerung Oberbau                          | 300    |
	| Trottoir                                    | 50     |
	Und ich für den Zustandsabschnitt mit der Id '<Id>' die Massnahme '<Massnahme>' vorschlage
	Und ich die Schäden speichere
	Dann werden folgende '<Kosten>' für den Massnahmenvorschlag angezeigt
	
	Beispiele: 
	| Id | Massnahme                                   | Kosten      |
	| 1  | Oberflächenverbesserung                     | CHF 225000  |
	| 2  | Deckbelagserneuerung                        | CHF 360000  |
	| 3  | Belagserneuerung mit teilweiser Verstärkung | CHF 5500000 |
	| 4  | Erneuerung Oberbau                          | CHF 3622500 |
	| 7  | Erneuerung Trottoir links                   | CHF 162500  |
	| 6  | Erneuerung Trottoir rechts                  | CHF 15000   |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Die vom System berechneten Kosten von Massnahmenvorschlägen können vom Benutzer nachträglich angepasst werden

# ACHTUNG: Gemäss Projektleitersitzung 12.01.2012 nicht erforderlich!

#---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System listet alle Zustandsabschnitte in einer Übersichtstabelle

#---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Archivierte Daten können nicht verändert werden

#---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Werden Zustandsabschnitte gelöscht, hat das keine Auswirkungen auf historische Daten

#---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


