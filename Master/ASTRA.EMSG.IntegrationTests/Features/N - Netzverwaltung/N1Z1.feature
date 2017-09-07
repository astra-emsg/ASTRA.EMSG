Funktionalität: N1 - Strassenmengen pro Belastungskategorie verwalten; Z1 - Zustand summarisch erfassen
	Als Data-Manager
	will ich Strassenmengen pro Belastungskategorie verwalten
	damit ich Auswertungen des Grundmodells Werterhaltung durchführen kann

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Es gibt 6 Belastungskategorien

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann entweder ‚Gesamtlänge‘ und ‚Gesamtfläche der Fahrbahnen‘ je Belastungskategorie erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      |
		| Mandant_1 | summarisch |
	
	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende summarische Zustands- und Netzinformationen eingebe:
	    | Belastungskategorie   | Mittlerer Zustand | Menge | Fahrbahnlaenge |
	    | <Belastungskategorie> | 2,1               | 20    | 30             |

	Dann sind folgende summarische Zustands- und Netzinformationen im System:
		| Mandant   | Belastungskategorie   | Mittlerer Zustand | Menge Gesamtlänge   | Menge Gesamtfläche   |
		| Mandant_1 | <Belastungskategorie> | 2,1               | <Menge Gesamtlänge> | <Menge Gesamtfläche> |

Beispiele: 
	| TF | Mengentyp    | Belastungskategorie | Menge Gesamtlänge | Menge Gesamtfläche |
	| 1  | Gesamtlänge  | IA                  | 30                | 20                 |
	| 3  | Gesamtlänge  | IB                  | 30                | 20                 |
	| 5  | Gesamtlänge  | IC                  | 30                | 20                 |
	| 7  | Gesamtlänge  | II                  | 30                | 20                 |
	| 9  | Gesamtlänge  | III                 | 30                | 20                 |
	| 11 | Gesamtlänge  | IV                  | 30                | 20                 |
	| 12 | Gesamtfläche | IV                  | 30                | 20                 |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann erfasste Mengen bearbeiten
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      |
		| Mandant_1 | summarisch |

	Und ich bin Data-Manager von 'Mandant_1'

	Und für Mandant 'Mandant_1' folgende summarische Zustands- und Netzinformationen existieren:
		| Belastungskategorie   | Mittlerer Zustand | Menge Gesamtlänge | Menge Gesamtfläche |
		| <Belastungskategorie> | 3,4               | 100,5             | 200                |
	
	Wenn ich folgende summarische Zustands- und Netzinformationen eingebe:
		| Belastungskategorie   | Mengentyp   | Mittlerer Zustand | Menge | Fahrbahnlaenge |
		| <Belastungskategorie> | <Mengentyp> | 2,1               | 20    |	30             |
	
	Dann sind folgende summarische Zustands- und Netzinformationen im System:
		| Mandant   | Belastungskategorie   | Mittlerer Zustand | Menge Gesamtlänge   | Menge Gesamtfläche   |
		| Mandant_1 | <Belastungskategorie> | 2,1               | <Menge Gesamtlänge> | <Menge Gesamtfläche> |

Beispiele: 
	| TF | Mengentyp   | Belastungskategorie | Menge Gesamtlänge | Menge Gesamtfläche |
	| 1  | Gesamtlänge | IA                  | 30                | 20                 |
	| 3  | Gesamtlänge | IB                  | 30                | 20                 |
	| 5  | Gesamtlänge | IC                  | 30                | 20                 |
	| 7  | Gesamtlänge | II                  | 30                | 20                 |
	| 9  | Gesamtlänge | III                 | 30                | 20                 |
	| 11 | Gesamtlänge | IV                  | 30                | 20                 |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Alle Felder (Mengen) sind numerische Werte
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      |
		| Mandant_1 | summarisch |

	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende summarische Zustands- und Netzinformationen eingebe:
		| Belastungskategorie | Mengentyp   | Mittlerer Zustand | Menge   | Fahrbahnlaenge   |
		| IA                  | <Mengentyp> | 2,1               | <Menge> | <Fahrbahnlaenge> |

	Dann '<Menge Feldname>' liefert einen Validationsfehler '<Validationfehler>'	

Beispiele: 
	| TF | Mengentyp    | Menge Feldname     | Fahrbahnlaenge | Menge      | Validationfehler | Kommentar               |
	| 1  | Gesamtlänge  | Gesamtlänge Menge  | 0              | 20         | Nein             | Gutfall untere Schranke |
	| 2  | Gesamtlänge  | Gesamtlänge Menge  | 2147483647     | 20         | Nein             | Gutfall obere Schranke  |
	| 3  | Gesamtlänge  | Gesamtlänge Menge  | 200,5          | 20         | Nein             | Gutfall Kommastelle     |
	| 5  | Gesamtlänge  | Gesamtlänge Menge  | -1             | 20         | Ja               | Ungültige Menge < 0     |
	| 6  | Gesamtlänge  | Gesamtlänge Menge  | -0,1           | 20         | Ja               | Ungültige Menge < 0     |
	| 11 | Gesamtfläche | Gesamtfläche Menge | 20             | 0          | Nein             | Gutfall untere Schranke |
	| 12 | Gesamtfläche | Gesamtfläche Menge | 20             | 2147483647 | Nein             | Gutfall obere Schranke  |
	| 13 | Gesamtfläche | Gesamtfläche Menge | 20             | 200        | Nein             | Gutfall                 |
	| 15 | Gesamtfläche | Gesamtfläche Menge | 20             | -1         | Ja               | Ungültige Menge < 0     |

@Manuell
Beispiele:
    | TF | Mengentyp    | Menge Feldname     | Fahrbahnlaenge      | Menge | Validationfehler | Kommentar                         |
    | 7  | Gesamtlänge  | Gesamtlänge Menge  | ABCD       | ABCD           | Ja               | Ungültiger Menge string           |
    | 8  | Gesamtlänge  | Gesamtlänge Menge  | 1ABCD1     | 1ABCD1         | Ja               | Ungültiger Menge string           |
    | 9  | Gesamtlänge  | Gesamtlänge Menge  | 2147483648 | 2147483648     | Ja               | Ungültige Menge > Maximalwert     |
    | 16 | Gesamtfläche | Gesamtfläche Menge | -0,1       | -0,1           | Ja               | Ungültige Menge < 0 + Kommastelle |
    | 17 | Gesamtfläche | Gesamtfläche Menge | ABCD       | ABCD           | Ja               | Ungültiger Menge string           |
    | 18 | Gesamtfläche | Gesamtfläche Menge | 1ABCD1     | 1ABCD1         | Ja               | Ungültiger Menge string           |
    | 19 | Gesamtfläche | Gesamtfläche Menge | 2147483648 | 2147483648     | Ja               | Ungültige Menge > Maximalwert     |
    | 20 | Gesamtfläche | Gesamtfläche Menge | 250,1      | 250,1          | Ja               | Ungültige Menge keine Kommastelle |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Bei Änderung des Mengentyps bleiben vorhandene Eingaben von anderen Mengen gespeichert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann einen mittleren Zustand der Fahrbahn pro Belastungskategorie erfassen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp    |
		| Mandant_1 | summarisch | Gesamtfläche |

	Und ich bin Data-Manager von 'Mandant_1'

	Wenn ich folgende summarische Zustands- und Netzinformationen eingebe:
	    | Belastungskategorie   | Mengentyp    | Mittlerer Zustand   | Menge |
	    | IA                    | Gesamtfläche | <Mittlerer Zustand> | 20    |

	Dann 'Mittlerer Zustand' liefert einen Validationsfehler '<Validationfehler>'
	
    Dann sind folgende summarische Zustands- und Netzinformationen im System:
    	| Mandant   | Belastungskategorie | Mittlerer Zustand            |
    	| Mandant_1 | IA                  | <Mittlerer Zustand Ergebnis> |
    
Beispiele: 
	| TF | Mittlerer Zustand | Mittlerer Zustand Ergebnis | Validationfehler | Kommentar                                      |
	| 1  | 0                 | 0                          | Nein             | Gutfall untere Schranke                        |
	| 2  | 5                 | 5                          | Nein             | Gutfall obere Schranke                         |
	| 3  | 3,3               | 3,3                        | Nein             | Gutfall Kommazahl                              |
	| 4  | -                 | -                          | Nein             | Gutfall Leereintrag                            |
	| 5  | 5,1               | -                          | Ja               | Ungültiger Mittlerer Zustand > 5               |
	| 6  | 6                 | -                          | Ja               | Ungültiger Mittlerer Zustand > 5               |
	| 7  | -1                | -                          | Ja               | Ungültiger Mittlerer Zustand < 0               |
	| 8  | -0,1              | -                          | Ja               | Ungültiger Mittlerer Zustand < 0               |
	| 9  | 3,33              | -                          | Ja               | Ungültiger Mittlerer Zustand max 1 Kommastelle |

@Manuell
Beispiele: 
    | TF | Mittlerer Zustand | Mittlerer Zustand Ergebnis | Validationfehler | Kommentar                           |
    | 10 | ABCD              | Nein                       | 0                | Ungültiger Mittlerer Zustand string |
    | 11 | 1abc1             | Nein                       | 0                | Ungültiger Mittlerer Zustand string |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann den mittleren Zustand der Fahrbahn bearbeiten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Automatisch
Szenariogrundriss: Der Data-Manager kann den zur aktuellen Jahresperiode erfassen Zustandsdaten ein mittleres Erhebungsdatum (Alter) zuweisen
	Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus      | Mengentyp   |
		| Mandant_1 | summarisch | Gesamtlänge |
	
	Und ich bin Data-Manager von 'Mandant_1'

    Und ich öffne die Seite 'Zustands- und Netzinformationen im Summarischen Modus'

	Wenn ich Mittleres Alter '<Mittleres Alter>' eingebe

	Dann Mittleres Alter hat einen Validationsfehler '<Validationfehler>'
	
    Dann sind folgende Mittlere Alter im System:
		| Mandant   | Mittleres Alter            |
		| Mandant_1 | <Mittleres Alter Ergebnis> |

Beispiele: 
	| TF | Mittleres Alter | Validationfehler | Mittleres Alter Ergebnis | Kommentar                                |
	| 1  | 01.01.1900      | Nein             | 01.01.1900               | Gutfall untere Schranke                  |
	| 2  | 31.10.2009      | Nein             | 31.10.2009               | Gutfall                                  |
	| 3  | 31.12.9999      | Nein             | 31.12.9999               | Gutfall obere Schranke                   |
	| 4  | -               | Nein             | -                        | Gutfall Leereintrag                      |
    
@Manuell
Beispiele: 
	| TF | Mittleres Alter | Validationfehler | Mittleres Alter Ergebnis | Kommentar                                |
    | 6  | abcd            | Nein             | abcd                     | Ungültiges Mittleres Alter string        |
    | 7  | 19xy            | Nein             | 19xy                     | Ungültiges Mittleres Alter string        |
    | 8  | 2009,1          | Ja               | -                        | Ungültiges Mittleres Alter Kommastelle   |
    | 9  | !^'~            | Nein             | !^'~                     | Ungültiges Mittleres Alter Sonderzeichen |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Archivierte Daten können nicht verändert werden

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Am UI wird nur der Mengentyp angezeigt, der aktuell für den Mandanten konfiguriert ist
