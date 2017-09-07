@Automatisch
Funktionalität: B6 – eine Benchmarkauswertung zu meinen Kennwerten der realisierten Massnahmen erhalten
	Als Benchmarkteilnehmer,
    will ich eine Benchmarkauswertung zu meinen Kennwerten der realisierten Massnahmen erhalten
    damit die Werte meiner Gemeinde mit anderen Gemeinden vergleichen kann

#WARNING: Currently only the current year and the previous 9 years can be closed -> these tests will break in 2022

Szenario: Das System stellt sicher, dass die Benchmarkauswertung zu meinen Kennwerten der realisierten Massnahmen nur dann erzeugt wird, wenn zumindest 5 Gemeinden (4 zusätzliche zu der Gemeinde des Benchmarkteilnehmers) für die Benchmarkauswertung herangezogen werden können
Gegeben sei folgende Einstellungen für Benchmarkauswertung existieren:
#Gemeindetyp: Zentrum,Periurbane,Industrielle,Laendliche,Agrargemischte,Einkommensstarke,Touristisch,Agrarische
#OeffentlicheVerkehrsmittel: Vorhanden,NichtVorhanden
		| Mandant   | Einwohner | Siedlungsflaeche | Gemeindeflaeche | Gemeindetyp      | MittlereHöhenlageSiedlungsgebiete | DifferenzHöhenlageSiedlungsgebiete | Steuerertrag | OeffentlicheVerkehrsmittel |
		| Mandant_1 | 1420      | 25               | 26              | Zentrum          | 167                               | 23                                 | 12345        | Vorhanden                  |
		| Mandant_2 | 5000      | 71               | 100             | Periurbane       | 279                               | 17                                 | 54321        | Vorhanden                  |
		| Mandant_3 | 1500      | 12               | 20              | Agrargemischte   | 920                               | 55                                 | 99999        | Vorhanden                  |
		| Mandant_4 | 2533      | 14               | 15              | Industrielle     | 860                               | 150                                | 10000        | Vorhanden                  |
		| Mandant_5 | 1378      | 5                | 7               | Agrargemischte   | 550                               | 8                                  | 12000        | Vorhanden                  |
		| Mandant_6 | 1234      | 21               | 25              | Agrargemischte   | 620                               | 45                                 | 80000        | Vorhanden                  |
		| Mandant_7 | 160       | 11               | 16              | Laendliche       | 1100                              | 120                                | 5000         | NichtVorhanden             |
		| Mandant_8 | 40000     | 33               | 123             | Einkommensstarke | 170                               | 2                                  | 777000       | Vorhanden                  |
Und die folgende RealisierteMassnahmeSummarsich existieren:
        | Mandant   | Jahr | Projektname  | Belastungskategorie | Fahrbahnflaeche | KostenFahrbahn |
        | Mandant_2 | 2012 | Projektname1 | IA                  | 15000           | 15000          |
        | Mandant_2 | 2012 | Projektname2 | IB                  | 20000           | 20000          |
        | Mandant_2 | 2012 | Projektname3 | IC                  | 30000           | 30000          |
Und die folgende RealisierteMassnahme existieren:
        | Mandant   | Jahr | Projektname  | Belastungskategorie | Laenge | BreiteFahrbahn | KostenFahrbahn |
        | Mandant_1 | 2012 | Projektname1 | IA                  | 1500   | 10             | 15000          |
        | Mandant_1 | 2012 | Projektname2 | IB                  | 2000   | 20             | 20000          |
        | Mandant_1 | 2012 | Projektname3 | IC                  | 3000   | 30             | 30000          |
Und die folgende RealisierteMassnahmeGIS existieren:
        | Mandant   | Jahr | Projektname  | Belastungskategorie | Laenge | BreiteFahrbahn | KostenFahrbahn |
        | Mandant_2 | 2012 | Projektname1 | IA                  | 1500   | 10             | 15000          |
        | Mandant_2 | 2012 | Projektname2 | IB                  | 2000   | 20             | 20000          |
        | Mandant_2 | 2012 | Projektname3 | IC                  | 3000   | 30             | 30000          |
Und die folgenden KenngroessenFruehererJahre existieren:
    #max 6 rows per Jahr for each Belastungskategorie
        | Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche | KostenFuerWerterhaltung |
        | Mandant_2 | 2012 | IA                  | 1,3              | 100            | 475             | 15000                   |
        | Mandant_2 | 2012 | IB                  | 2,6              | 300            | 1725            | 20000                   |
        | Mandant_2 | 2012 | IC                  | 4,6              | 10000          | 70000           | 30000                   |
Und ich bin Benchmarkteilnehmer von 'Mandant_1'
Wenn ich die Benchmarkauswertung zu meinen Kennwerten der realisierten Massnahmen für das Jahre '2012' unter Berücksichtigung der Klassen 'Netzgrosse,Gemeinde' generiere
#Klassen: Netzgroesse,Einwohnergroesse,Gemeindetyp,Siedlungsgebiete,OeffentlicheVerkehrsmittel,Steureikommen
#Dann zeigt die Grafik folgende Ergebnisse 'B6_RealisiertenMassnahmen_NetzgrosseGemeindetyp'
#| Beschreigung                    | Bezugroesse | Einheit | Organisation | AnzahlGruppe | MittelwertGruppe | MininmalwertGruppe | MaximalwertGruppe | AnzahlAll | MittelwertAll | MininmalwertAll | MaximalwertAll |
#| RealisierteMassnahme            | Fahrbahn    | CHF/m2  |              |              |                  |                    |                   |           |               |                 |                |
#| RealisierteMassnahme            | Einwohner   | CHF/E   |              |              |                  |                    |                   |           |               |                 |                |
#| RealisierteMassnahmeWertverlust |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| RealisierteMassnahmeWBW         |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| IA                              |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| IB                              |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| IC                              |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| II                              |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| III                             |             | %       |              |              |                  |                    |                   |           |               |                 |                |
#| IV                              |             | %       |              |              |                  |                    |                   |           |               |                 |                |
Dann informiert mich das System, dass zu wenige Mandanten für die Benchmarkauswertung zur Verfügung stehen und die Auswertung nicht generiert werden kann