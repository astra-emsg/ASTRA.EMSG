Funktionalität: Z3.2 - Mit EMSG-Master Schäden und Massnahmenvorschläge für Zustandsabschnitte nachbearbeiten
	Als Data-Manager
	will ich auf EMSG-Master Schäden und Massnahmenvorschläge für Zustandsabschnitte nachbearbeiten
	damit ich die Qualität der Inspektionsergebnisse verbessern bzw. Daten effizienter eingeben kann

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
| Id | Strassenname   | BezeichnungVon     | BezeichnungBis   | Belagsart | BreiteFahrbahn | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechtss |
| 1  | Kantonsstrasse | Abschnitt A Anfang | Abschnitt A Ende | Asphalt   | 4,5            | beide Seiten | 2,5                 | 3                     |
| 2  | Kantonsstrasse | Abschnitt B Anfang | Abschnitt B Ende | Asphalt   | 5,5            | beide Seiten | 2,5                 | 3                     |
| 3  | Landstrasse    | Abschnitt A Anfang | Abschnitt A Ende | Beton     | 5,75           | beide Seiten | 2,5                 | 3                     |
| 4  | Hauptstrasse   | Abschnitt A Anfang | Abschnitt A Ende | Beton     | 7              | beide Seiten | 2,5                 | 3                     |
| 5  | Hauptstrasse   | Abschnitt B Anfang | Abschnitt B Ende | Asphalt   | 7              | beide Seiten | 2,5                 | 3                     |
| 6  | Hauptstrasse   | Abschnitt C Anfang | Abschnitt C Ende | Asphalt   | 14             | beide Seiten | 2,5                 | 3                     |
Und folgende Zustandsinformationen existieren:
| Id | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
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
Szenario: Der Data-Manager kann Schäden und Massnahmenvorschläge bestehender Zustandsabschnitte vollumfänglich nachbearbeiten
Wenn ich die Schäden und Massnahmenvorschläge folgendermassen editere:
| Id | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 1  | 3.2                   |                            | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
| 2  | 2.3                   |                            | Ersatz                      | dringlich                                |                         |                                      |                          |                                       |
| 3  | 1.2                   | Gut                        | Instandsetzung              | dringlich                                | ja                      | langfristig                          | ja                       | dringend                              |
Dann existieren folgende Zustandsinformationen:
| Id | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 1  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 0.2            | 1.2            | 1000  | 12.14.2009    | Drei         | Nass    | 3,2                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
| 2  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 1.2            | 2.0            | 800   | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Ersatz                      | dringlich                                |                         |                                      |                          |                                       |
| 3  | Kantonsstrasse | Abschnitt B Anfang    | Abschnitt B Ende      | 2.0            | 7.0            | 5000  | 23.03.2009    | A            | Trocken | 1,2                   | Gut                        |                             | Instandsetzung              | dringlich                                | ja                      | langfristig                          | ja                       | dringend                              |
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
Szenario: Der Data-Manager kann Zustandsabschnitte bearbeiten
Wenn ich die Zustandsabschnitte für die Strasse mit der Id '3' folgendermassen editiere:
| Id | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 4  | 0.0            | 2.1            | 2100  | 31.03.2010    | A            | Nass    | 1,5                   | Ausreichend                 | Instandsetzung              | dringlich                                |                         |                                      | nein                     |                                       |
| 6  | 2.4            | 2.6            | 200   | 21.05.2009    | B            | Trocken | 2,1                   | Ausreichend                 | Ersatz                      | langfristig                              |                         |                                      | ja                       | mittelfristig                         |
Dann existieren folgende Zustandsinformationen für die Strasse mit der Id '3':
| Id | Strassenname | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
| 4  | Landstrasse  | Abschnitt A Anfang    | Abschnitt A Ende      | 0.0            | 2.1            | 2100  | 31.03.2010    | A            | Nass    | 1,5                   |                            | Ausreichend                 | Instandsetzung              | dringlich                                |                         |                                      | nein                     |                                       |
| 5  | Landstrasse  | Abschnitt A Anfang    | Abschnitt A Ende      | 2.1            | 2.4            | 300   | 21.05.2009    | B            | Trocken | 1,1                   |                            | Kritisch                    | Ersatz                      | langfristig                              |                         |                                      | ja                       | dringend                              |
| 6  | Landstrasse  | Abschnitt A Anfang    | Abschnitt A Ende      | 2.4            | 2.6            | 200   | 21.05.2009    | B            | Trocken | 2,1                   |                            | Ausreichend                 | Ersatz                      | langfristig                              |                         |                                      | ja                       | mittelfristig                         |

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
| Strassenname   | BezeichnungVon | BezeichnungBis |
| Kantonsstrasse | 0.2            | 1.2            |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario:Das System stellt sicher, dass die Summe der Längen der Zustandsabschnitte nicht grösser ist, als die Länge des Strassenabschnitts

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt bei grober und detaillierter Erfassung das Formular entsprechend der Belagsart des Strassenabschnitts dar

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Erfassungsformular Oberflächenschäden Betonbelag (Beton) - detailliert
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '5' öffne
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
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '5' öffne
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
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '5' öffne
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
Szenario: Das System berechnet die Schadensumme bei Grober- und detaillierter Erfassung
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '5' öffne
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
Wenn ich das Erfassungsformular für Oberflächenschäden für den Zustandsabschnitt mit der Id '5' öffne
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
Dann sehe ich für den Zustandsabschnitt mit der Id '5' den Zustandsindex '4.4'

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Zustandsabschnitte über die Karte löschen
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus |
		| Mandant_1 | gis   |
	Und ich bin Data-Manager von 'Mandant_1'
	Und ich öffne die Seite 'Netzdefinition und Hauptabschnitt mit Strassennamen'
	Und ich wähle den Modus "Zustandabschnitt erfassen"

	Wenn ich den Zustandsabschnitt (ID 2) durch Auswahl in der Karte selektiere
	Und ich die Netzinformationen für diesen Zustandsabschnitt lösche

	Dann sind folgende Netzinformationen im System:
	| Id | Strassenname   | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Länge | Aufnahmedatum | Aufnahmeteam | Wetter  | ZustandsIndexFahrbahn | ZustandsindexTrottoirLinks | ZustandsindexTrottoirRechts | MassnahmenvorschlagFahrbahn | DringlichkeitMassnahmenvorschlagFahrbahn | TrottoirLinksErneuerung | DringlichkeitTrottoirLinksErneuerung | TrottoirRechtsErneuerung | DringlichkeitTrottoirRechtsErneuerung |
	| 1  | Kantonsstrasse | Abschnitt A Anfang    | Abschnitt A Ende      | 0.2            | 1.2            | 1000  | 12.14.2009    | Drei         | Nass    | 2,3                   |                            |                             | Instandsetzung              | dringlich                                |                         |                                      |                          |                                       |
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
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
Wenn ich die Zustandsabschnitte für die Strasse mit der Id '3' folgendermassen editiere:
| Id | Länge |
| 4  | 2000  |
| 6  | 200   |
Dann werden folgende Zustandsinformationen für die Strasse mit der Id '3' angezeigt:
| Id | FlächeFahrbahn |
| 4  | 11500          |
| 5  | 1725           |
| 6  | 1150           |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
Wenn ich die Zustandsabschnitte für die Strasse mit der Id '3' folgendermassen editiere:
| Id | Länge |
| 4  | 2000  |
| 6  | 200   |
Dann werden folgende Zustandsinformationen für die Strasse mit der Id '3' angezeigt:
| Id | FlächeTrottoirLinks |
| 4  | 5000                |
| 5  | 750                 |
| 6  | 500                 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager den Zustandsabschnitt gespeichert hat
Wenn ich die Zustandsabschnitte für die Strasse mit der Id '3' folgendermassen editiere:
| Id | Länge |
| 4  | 2000  |
| 6  | 200   |
Dann werden folgende Zustandsinformationen für die Strasse mit der Id '3' angezeigt:
| Id | FlächeTrottoirLinks |
| 4  | 6000                |
| 5  | 900                 |
| 6  | 600                 |

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
Szenario: Archivierte Daten können nicht verändert werden

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Werden Zustandsabschnitte gelöscht, hat das keine Auswirkungen auf historische Daten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System listet alle Zustandsabschnitte in einer Übersichtstabelle
Wenn ich die Übersichtstabelle für die Strasse mit der Id '3' aufrufe
Dann sehe ich folgende Informationen zu den Zustandsabschnitten:
| Id | Strassenname | BezeichnungVon | BezeichnungBis | ZustandsIndexFahrbahn | FlächeFahrbahn | FlächeTrottoirLinks | FlächeTrottoirRechts |
| 4  | Landstrasse  | 0.0            | 2.1            | 1,1                   | 12075          | 5250                | 6300                 |
| 5  | Landstrasse  | 2.1            | 2.4            | 1,1                   | 1725           | 750                 | 900                  |
| 6  | Landstrasse  | 2.4            | 2.5            | 2,1                   | 575            | 250                 | 300                  |
#------------------------------------------------------------------------------------------------------------------------------------------------------

#------------------------------------------------------------------------------------------------------------------------------------------------------
# Ab hier Akzeptanzkritieren von Z3.1, die auch für Z3.2 gelten

@Manuell
Szenario: Der Data-Manager kann einen Zustandsabschnitt auf der Karte digitalisieren
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell @ignore
Szenario: Der Benutzer wählt eine zu bearbeitende Inspektionsroute aus. Die dazugehörigen Strassenabschnitte werden auf der Karte angezeigt.
# ACHTUNG: Nicht für Iteration 1 vorgesehen

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
Szenario: Anfangs und Endpunkt beziehen sich auf einen einzelnes Segment eines Strassenabschnittes
Wenn ich versuche einen End- oder Anfangspunkt über das Segment des darunterliegenden Strassenabschnittes hinauszuziehen
Dann stoppt der Punkt am Ende des Segments des Strassenabschnitts

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Zustandsabschnitte dürfen sich nicht überlappen
Wenn ich versuche eine den Endpunkt eines Zustandabschnittes über die räumliche Definition eines für den selben Strassenabschnitt vorliegenden Zustandabschnittes  auf der Karte zu ziehen
Dann stoppt der Punkt am Ende, bzw. Anfang des Zustandabschnittes

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt muss immer einem Strassenabschnitt zugeordnet sein

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Zustandsabschnitt erbt ‚Strassenname‘, ‚Belagsart‘, ‚Breite Fahrbahn‘, ‚Trottoir‘, ‚Breite Trottoir links‘ und ‚Breite Trottoir rechts‘ vom Strassenabschnitt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Angaben zum Trottoir wie Zustandsindex, Erneuerung usw. können nur erfasst werden, sofern der Strassenabschnitt auch einen entsprechenden Trottoir(typ) aufweist

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Ein Strassenabschnitt kann mehrere Zustandsabschnitte umfassen.
Gegeben sei ein Strassenabschnitt auf welchem ein Zustandsabschnitt vorliegt, welcher nicht den gesamten Strassenabschnitt umfasst
Wenn ich einen neuen Zustandabschnitt auf diesem Strassenabschnitt anlege
Dann wird dieser auf der verbleibenden Länge des Strassenabschnittes erzeugt 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das  System schlägt Anfangs- und Endkilometer des Zustandsabschnitts vor
# ACHTUNG: Nicht für Iteration 1 vorgesehen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System verwendet die vom Data-Manager eingetragenen Kilometerwerte zur Definition des genauen Anfangs bzw. des Ende des Abschnitts
# ACHTUNG: Nicht für Iteration 1 vorgesehen

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
