Funktionalität: Z3.1 - Mit EMSG-Mobile Schäden und Massnahmenvorschläge für Strassenabschnitte bearbeiten
	Als Data-Manager,
	will ich mit EMSG-Mobile Schäden und Massnahmenvorschläge für Strassenabschnitte erfassen
	damit ich meine Inspektionsergebnisse im Feld ohne die Notwendigkeit einer Internet-Verbindung erfassen kann

Grundlage: 
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus |
		| Mandant_1 | gis   |
	
	Und ich bin Data-Manager von 'Mandant_1'
	Und folgende Netzinformationen existieren:
		| Id | Strassenname   | BezeichnungVon     | BezeichnungBis   | Belagsart | BreiteFahrbahn | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechtss |
		| 1  | Kantonsstrasse | Abschnitt A Anfang | Abschnitt A Ende | Asphalt   | 4,5            | beide Seiten | 2,5                 | 3                     |
		| 2  | Kantonsstrasse | Abschnitt B Anfang | Abschnitt B Ende | Asphalt   | 5,5            | beide Seiten | 2,5                 | 3                     |
		| 3  | Landstrasse    | Abschnitt A Anfang | Abschnitt A Ende | Beton     | 5,75           | beide Seiten | 2,5                 | 3                     |
		| 4  | Hauptstrasse   | Abschnitt A Anfang | Abschnitt A Ende | Beton     | 7              | beide Seiten | 2,5                 | 3                     |
		| 5  | Hauptstrasse   | Abschnitt B Anfang | Abschnitt B Ende | Asphalt   | 7              | beide Seiten | 2,5                 | 3                     |
		| 6  | Hauptstrasse   | Abschnitt C Anfang | Abschnitt C Ende | Asphalt   | 14             | beide Seiten | 2,5                 | 3                     |
	
@Manuell
Szenario: Der Data-Manager kann einen Zustandsabschnitt auf der Karte digitalisieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Anfangs- und Endpunkt eines Zustandsabschnitts auf der Karte festlegen
Gegeben sei ich öffne die Seite 'Netzverwaltung\Zustände und Massnahmenvorschläge'
	Und ich wähle einen beliebigen Zustandsabschnitt aus
	Und ich aktiviere das Tool 'Zustandsabschnitts Geometrie bearbeiten'
	Wenn ich einen Anfangs oder Endpunkt  entlang des darunterliegendem Strassenabschnitt bewege
	Dann wird der Zustandsabschnitt entsprechend verlängert oder verkürzt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Zustandsabschnitte dürfen sich nicht überlappen
Wenn ich versuche eine den Endpunkt eines Zustandabschnittes über die räumliche Definition eines für den selben Strassenabschnitt vorliegenden Zustandabschnittes  auf der Karte zu ziehen
Dann stoppt der Punkt am Ende, bzw. Anfang des Zustandabschnittes

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt muss immer einem Strassenabschnitt zugeordnet sein
Wenn ich versuche einen End- oder Anfangspunkt über das Segment des darunterliegenden Strassenabschnittes hinauszuziehen
Dann stoppt der Punkt am Ende des Segments des Strassenabschnitts

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt erbt ‚Strassenname‘, ‚Belagsart‘, ‚Breite Fahrbahn‘, ‚Trottoir‘, ‚Breite Trottoir links‘ und ‚Breite Trottoir rechts‘ vom Strassenabschnitt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Angaben zum Trottoir wie Zustandsindex, Erneuerung usw. können nur erfasst werden, sofern der Strassenabschnitt auch einen entsprechenden Trottoir(typ) aufweist

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass die Summe der Längen der Zustandsabschnitte nicht grösser ist, als die Länge des Strassenabschnitts

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenariogrundriss: Der Data-Manager kann Attribute von Zustandsabschnitten erfassen
Wenn ich folgende GIS-Netzinformationen eingebe:
		| Strassenname   | BezeichnungVonStrasse   | BezeichnungBisStrasse   | BezeichnungVon   | BezeichnungBis   | Länge   | Aufnahmedatum   | Aufnahmeteam   | Wetter   | ZustandsIndexFahrbahn   | ZustandsindexTrottoirLinks   | ZustandsindexTrottoirRechts   | MassnahmenvorschlagFahrbahn   | DringlichkeitMassnahmenvorschlagFahrbahn   | TrottoirLinksErneuerung   | DringlichkeitTrottoirLinksErneuerung   | TrottoirRechtsErneuerung   | DringlichkeitTrottoirRechtsErneuerung   |
		| <Strassenname> | <BezeichnungVonStrasse> | <BezeichnungBisStrasse> | <BezeichnungVon> | <BezeichnungBis> | <Länge> | <Aufnahmedatum> | <Aufnahmeteam> | <Wetter> | <ZustandsIndexFahrbahn> | <ZustandsindexTrottoirLinks> | <ZustandsindexTrottoirRechts> | <MassnahmenvorschlagFahrbahn> | <DringlichkeitMassnahmenvorschlagFahrbahn> | <TrottoirLinksErneuerung> | <DringlichkeitTrottoirLinksErneuerung> | <TrottoirRechtsErneuerung> | <DringlichkeitTrottoirRechtsErneuerung> |

	Dann sind folgende GIS-Netzinformationen im System:
		| Strassenname   | BezeichnungVonStrasse   | BezeichnungBisStrasse   | BezeichnungVon   | BezeichnungBis   | Länge   | Aufnahmedatum   | Aufnahmeteam   | Wetter   | ZustandsIndexFahrbahn   | ZustandsindexTrottoirLinks   | ZustandsindexTrottoirRechts   | MassnahmenvorschlagFahrbahn   | DringlichkeitMassnahmenvorschlagFahrbahn   | TrottoirLinksErneuerung   | DringlichkeitTrottoirLinksErneuerung   | TrottoirRechtsErneuerung   | DringlichkeitTrottoirRechtsErneuerung   |
		| <Strassenname> | <BezeichnungVonStrasse> | <BezeichnungBisStrasse> | <BezeichnungVon> | <BezeichnungBis> | <Länge> | <Aufnahmedatum> | <Aufnahmeteam> | <Wetter> | <ZustandsIndexFahrbahn> | <ZustandsindexTrottoirLinks> | <ZustandsindexTrottoirRechts> | <MassnahmenvorschlagFahrbahn> | <DringlichkeitMassnahmenvorschlagFahrbahn> | <TrottoirLinksErneuerung> | <DringlichkeitTrottoirLinksErneuerung> | <TrottoirRechtsErneuerung> | <DringlichkeitTrottoirRechtsErneuerung> |

	Beispiele: 
		| TF | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
		| 1  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 0.2            | 1.2            | 1000  | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
		| 2  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 1.2            | 2.0            | 800   | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
		| 3  | Kantonsstrasse | Abschnitt B Anfang    | Abschnitt B Ende      | 2.0            | 7.0            | 5000  | 23.03.2009    | A            | Trocken | 1,2                   | Gut                        |                             | Instandsetzung              | mittelfristig                            |                         |                                      |                          |                                       |
		| 4  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 0.0            | 2.1            | 2100  | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
		| 5  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 2.1            | 2.4            | 300   | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
		| 6  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 2.4            | 2.5            | 100   | 21.05.2009    | B            | Trocken | 2,1                   |                            | Ausreichend                 | Ersatz                      | langfristig                              |                         |                                      | ja                       | mittelfristig                         |
		| 7  | Hauptstrasse   | Abschnitt A Anfang    | Abschnitt A Ende      | 1.2            | 2.5            | 1300  | 21.10.2009    | Meyer        | Trocken | 3,4                   | Mittel                     | Gut                         |                             |                                          | ja                      | langfristig                          |                          |                                       |
		| 8  | Hauptstrasse   | Abschnitt B Anfang    | Abschnitt B Ende      | 0.4            | 0.8            | 400   | 13.09.2009    | A            | Trocken | 2,3                   |                            |                             |                             |                                          |                         |                                      |                          |                                       |
		| 9  | Hauptstrasse   | Abschnitt B Anfang    | Abschnitt B Ende      | 0.8            | 1.2            | 400   | 13.09.2009    | A            | Trocken | 2,3                   |                            |                             |                             |                                          |                         |                                      |                          |                                       |
		| 10 | Hauptstrasse   | Abschnitt C Anfang    | Abschnitt C Ende      | 0.0            | 0.1            | 100   | 24.07.2009    | B            | Trocken | 3,3                   |                            |                             |                             |                                          |                         |                                      |                          |                                       |
		| 11 | Hauptstrasse   | Abschnitt C Anfang    | Abschnitt C Ende      | 0.1            | 0.2            | 100   | 24.07.2009    | B            | Trocken | 4,3                   |                            |                             |                             |                                          |                         |                                      |                          |                                       |
		| 12 | Hauptstrasse   | Abschnitt C Anfang    | Abschnitt C Ende      | 0.2            | 0.4            | 200   | 24.07.2009    | B            | Trocken | 4,8                   |                            |                             |                             |                                          |                         |                                      |                          |                                       |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das Zustandserfassungsformular (Erfassungsformular für Oberflächenschäden) ausfüllen

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

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das Schadenausmass erfassen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System errechnet die Matrixwerte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Schadenbewertung

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
Szenario: Der Data-Manager kann Zustandsabschnitte über die Karte löschen
Gegeben sei für Mandant 'Mandant_1' existieren folgende Netzinformationen:
		| ID | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
		| 1  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 0.2            | 1.2            | 1000  | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
		| 2  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 1.2            | 2.0            | 800   | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
		| 3  | Kantonsstrasse | Abschnitt B Anfang    | Abschnitt B Ende      | 2.0            | 7.0            | 5000  | 23.03.2009    | A            | Trocken | 1,2                   | Gut                        |                             | Instandsetzung              | mittelfristig                            |                         |                                      |                          |                                       |
		| 4  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 0.0            | 2.1            | 2100  | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
		| 5  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 2.1            | 2.4            | 300   | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
Wenn ich den Graphen mit der ID '3' auf der Karte auswähle
Und ich Zustandsinformation für ID '3' lösche
Dann sind folgende Zustandsinformationen im System:
         | ID | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
         | 1  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 0.2            | 1.2            | 1000  | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
         | 2  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 1.2            | 2.0            | 800   | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
         | 4  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 0.0            | 2.1            | 2100  | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
         | 5  | Landstrasse    | Abschnitt A Anfang    | Abschnitt A Ende      | 2.1            | 2.4            | 300   | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat

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
Szenario:Archivierte Daten können nicht verändert werden

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario:Werden Zustandsabschnitte gelöscht, hat das keine Auswirkungen auf historische Daten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System führt alle Berechnungen on the fly durch
