@Automatisch
Funktionalität: B4 - eine Benchmarkauswertung zu meinen Inventarkennwerten erhalten
	Als Benchmarkteilnehmer,
	will ich eine Benchmarkauswertung zu meinen Inventarkennwerten erhalten
	damit die Werte meiner Gemeinde mit anderen Gemeinden vergleichen kann

#WARNING: Currently only the current year and the previous 9 years can be closed -> these tests will break in 2022

#------------------------------------------------------------------------------------------------------------------------------------------------------

Szenario: Das System stellt sicher, dass die Benchmarkauswertung zu meinen Inventarkennwerten nur dann erzeugt wird, wenn zumindest 5 Gemeinden (4 zusätzliche zu der Gemeinde des Benchmarkteilnehmers) für die Benchmarkauswertung herangezogen werden können
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
Und die folgenden Strassenabschnitte existieren:
		| Mandant   | Jahr | Strassenname     | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
		| Mandant_1 | 2012 | 1Bahnstrasse     | IA                  | Beton   | 5,75           | 150   | KeinTrottoir | -                   | -                    | Privat             |
		| Mandant_1 | 2012 | 1Moosgasse       | IA                  | Asphalt | 5              | 266   | Rechts       | -                   | 1,5                  | Gemeinde           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | IB                  | Asphalt | 7              | 650   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Gartenstrasse   | IC                  | Asphalt | 5              | 430   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Brunngasse      | II                  | Asphalt | 4,75           | 1200  | Links        | 1,5                 | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Föhrenweg       | III                 | Asphalt | 4,75           | 200   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Bachstrasse     | III                 | Beton   | 7,25           | 620   | Links        | 2                   | -                    | Kanton             |
		| Mandant_1 | 2012 | 1Friedhofstrasse | IV                  | Beton   | 8              | 350   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Bahnstrasse     | IV                  | Beton   | 7,75           | 200   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Moosgasse       | IV                  | Asphalt | 6              | 300   | Rechts       | -                   | 1,5                  | Gemeinde           |
		| Mandant_3 | 2012 | 3Hauptstrasse    | III                 | Asphalt | 5              | 700   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Gartenstrasse   | II                  | Asphalt | 7              | 500   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Brunngasse      | IC                  | Asphalt | 14,75          | 1000  | Links        | 1,5                 | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Föhrenweg       | IC                  | Asphalt | 7,75           | 700   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Bachstrasse     | IC                  | Beton   | 4,25           | 900   | Links        | 2                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Friedhofstrasse | IC                  | Beton   | 7              | 50    | KeinTrottoir | -                   | -                    | Gemeinde           |
Und die folgenden StrassenabschnitteGIS existieren:
		| Mandant   | Jahr | Strassenname     | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
		| Mandant_2 | 2012 | 2Bahnstrasse     | IA                  | Beton   | 6              | 420   | Links            | 1,5                 | -                    | Korporation        |
		| Mandant_2 | 2012 | 2Moosgasse       | IA                  | Asphalt | 7,75           | 444   | Links            | 1                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | IB                  | Asphalt | 5,75           | 488   | Links            | 2                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Gartenstrasse   | IB                  | Asphalt | 6              | 700   | BeideSeiten      | 1                   | 1,5                  | Korporation        |
		| Mandant_2 | 2012 | 2Brunngasse      | II                  | Asphalt | 7,75           | 788   | BeideSeiten      | 1,5                 | 1,5                  | Gemeinde           |
		| Mandant_2 | 2012 | 2Föhrenweg       | II                  | Asphalt | 10,75          | 699   | NochNichtErfasst | -                   | -                    | Korporation        |
		| Mandant_2 | 2012 | 2Bachstrasse     | IV                  | Beton   | 14,25          | 333   | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | IV                  | Beton   | 10             | 444   | Rechts           | -                   | 2                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | IA                  | Beton   | 4,75           | 120   | Links            | 1,5                 | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Moosgasse       | IB                  | Asphalt | 6,25           | 75    | Links            | 1                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Hauptstrasse    | IC                  | Asphalt | 6              | 160   | KeinTrottoir     | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | IC                  | Asphalt | 7,75           | 50    | KeinTrottoir     | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Brunngasse      | II                  | Asphalt | 9              | 55    | BeideSeiten      | 1,5                 | 1,5                  | Gemeinde           |
		| Mandant_5 | 2012 | 5Föhrenweg       | III                 | Asphalt | 10             | 75    | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Bachstrasse     | III                 | Beton   | 14             | 98    | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | IV                  | Beton   | 14             | 133   | Rechts           | -                   | 2                    | Gemeinde           |
Und die folgenden Details zum NetzSummarisch existieren: 
	#max 6 rows one for each Belastungskategorie
		| Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche |
		| Mandant_4 | 2012 | IA                  | 2,2              | 3560           | 16910           |
		| Mandant_4 | 2012 | IB                  | 2,7              | 250            | 1250            |
		| Mandant_4 | 2012 | IC                  | 3,8              | 460            | 2530            |
		| Mandant_4 | 2012 | II                  | 4,8              | 1200           | 8400            |
		| Mandant_4 | 2012 | III                 | 1,2              | 720            | 10080           |
		| Mandant_4 | 2012 | IV                  | 1,3              | 360            | 5760            |
		| Mandant_7 | 2012 | IA                  | 4,2              | 1200           | 5700            |
		| Mandant_7 | 2012 | IB                  | 4,5              | 750            | 4125            |
		| Mandant_7 | 2012 | IC                  | 4,6              | 6200           | 43400           |
		| Mandant_7 | 2012 | II                  | 2,6              | 780            | 7800            |
		| Mandant_7 | 2012 | III                 | 3,6              | 930            | 13485           |
		| Mandant_7 | 2012 | IV                  | 3,9              | 450            | 6750            |
Und die folgenden KenngroessenFruehererJahre existieren:
    #max 6 rows per Jahr for each Belastungskategorie
        | Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche |
        | Mandant_6 | 2012 | IA                  | 1,3              | 100            | 475             |
        | Mandant_6 | 2012 | IB                  | 2,6              | 300            | 1725            |
        | Mandant_6 | 2012 | IC                  | 4,6              | 10000          | 70000           |
        | Mandant_6 | 2012 | II                  | 1,7              | 900            | 9000            |
        | Mandant_6 | 2012 | III                 | 0,2              | 800            | 9600            |
        | Mandant_6 | 2012 | IV                  | 3,4              | 200            | 2800            |
        | Mandant_8 | 2012 | IA                  | 2,6              | 1234           | 6170            |
        | Mandant_8 | 2012 | IB                  | 1,5              | 789            | 4734            |
        | Mandant_8 | 2012 | IC                  | 0,9              | 6789           | 47523           |
        | Mandant_8 | 2012 | II                  | 1,8              | 789            | 6312            |
        | Mandant_8 | 2012 | III                 | 2,4              | 901            | 8109            |
        | Mandant_8 | 2012 | IV                  | 3,7              | 456            | 5016            |
Und ich bin Benchmarkteilnehmer von 'Mandant_1'
Wenn ich die Benchmarkauswertung zu meinen Inventarkennwerten für das Jahre '2012' unter Berücksichtigung der Klassen 'NetzGroesse,Gemeinde' generiere
#Klassen: NetzGroesse,EinwohnerGroesse,Gemeinde,MittlereHoehenlageSiedlungsgebieteGroesse,OeffentlicheVerkehrsmittel,SteuerertragGroesse
Dann informiert mich das System, dass zu wenige Mandanten für die Benchmarkauswertung zur Verfügung stehen und die Auswertung nicht generiert werden kann

Szenario: Das System liefert eine Tabelle gemäss Abbildung 76 
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
Und die folgenden Strassenabschnitte existieren:
		| Mandant   | Jahr | Strassenname     | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir     | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
		| Mandant_1 | 2012 | 1Bahnstrasse     | IA                  | Beton   | 5,75           | 150   | KeinTrottoir | -                   | -                    | Privat             |
		| Mandant_1 | 2012 | 1Moosgasse       | IA                  | Asphalt | 5              | 266   | Rechts       | -                   | 1,5                  | Gemeinde           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | IB                  | Asphalt | 7              | 650   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Gartenstrasse   | IC                  | Asphalt | 5              | 430   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Brunngasse      | II                  | Asphalt | 4,75           | 1200  | Links        | 1,5                 | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Föhrenweg       | III                 | Asphalt | 4,75           | 200   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_1 | 2012 | 1Bachstrasse     | III                 | Beton   | 7,25           | 620   | Links        | 2                   | -                    | Kanton             |
		| Mandant_1 | 2012 | 1Friedhofstrasse | IV                  | Beton   | 8              | 350   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Bahnstrasse     | IV                  | Beton   | 7,75           | 200   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Moosgasse       | IV                  | Asphalt | 6              | 300   | Rechts       | -                   | 1,5                  | Gemeinde           |
		| Mandant_3 | 2012 | 3Hauptstrasse    | III                 | Asphalt | 5              | 700   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Gartenstrasse   | II                  | Asphalt | 7              | 500   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Brunngasse      | IC                  | Asphalt | 14,75          | 1000  | Links        | 1,5                 | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Föhrenweg       | IC                  | Asphalt | 7,75           | 700   | KeinTrottoir | -                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Bachstrasse     | IC                  | Beton   | 4,25           | 900   | Links        | 2                   | -                    | Gemeinde           |
		| Mandant_3 | 2012 | 3Friedhofstrasse | IC                  | Beton   | 7              | 50    | KeinTrottoir | -                   | -                    | Gemeinde           |
Und die folgenden StrassenabschnitteGIS existieren:
		| Mandant   | Jahr | Strassenname     | Belastungskategorie | Belag   | BreiteFahrbahn | Länge | Trottoir         | BreiteTrottoirLinks | BreiteTrottoirRechts | Strasseneigentümer |
		| Mandant_2 | 2012 | 2Bahnstrasse     | IA                  | Beton   | 6              | 420   | Links            | 1,5                 | -                    | Korporation        |
		| Mandant_2 | 2012 | 2Moosgasse       | IA                  | Asphalt | 7,75           | 444   | Links            | 1                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | IB                  | Asphalt | 5,75           | 488   | Links            | 2                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Gartenstrasse   | IB                  | Asphalt | 6              | 700   | BeideSeiten      | 1                   | 1,5                  | Korporation        |
		| Mandant_2 | 2012 | 2Brunngasse      | II                  | Asphalt | 7,75           | 788   | BeideSeiten      | 1,5                 | 1,5                  | Gemeinde           |
		| Mandant_2 | 2012 | 2Föhrenweg       | II                  | Asphalt | 10,75          | 699   | NochNichtErfasst | -                   | -                    | Korporation        |
		| Mandant_2 | 2012 | 2Bachstrasse     | IV                  | Beton   | 14,25          | 333   | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | IV                  | Beton   | 10             | 444   | Rechts           | -                   | 2                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | IA                  | Beton   | 4,75           | 120   | Links            | 1,5                 | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Moosgasse       | IB                  | Asphalt | 6,25           | 75    | Links            | 1                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Hauptstrasse    | IC                  | Asphalt | 6              | 160   | KeinTrottoir     | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | IC                  | Asphalt | 7,75           | 50    | KeinTrottoir     | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Brunngasse      | II                  | Asphalt | 9              | 55    | BeideSeiten      | 1,5                 | 1,5                  | Gemeinde           |
		| Mandant_5 | 2012 | 5Föhrenweg       | III                 | Asphalt | 10             | 75    | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Bachstrasse     | III                 | Beton   | 14             | 98    | NochNichtErfasst | -                   | -                    | Gemeinde           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | IV                  | Beton   | 14             | 133   | Rechts           | -                   | 2                    | Gemeinde           |
Und die folgenden Details zum NetzSummarisch existieren: 
	#max 6 rows one for each Belastungskategorie
		| Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche |
		| Mandant_4 | 2012 | IA                  | 2,2              | 3560           | 16910           |
		| Mandant_4 | 2012 | IB                  | 2,7              | 250            | 1250            |
		| Mandant_4 | 2012 | IC                  | 3,8              | 460            | 2530            |
		| Mandant_4 | 2012 | II                  | 4,8              | 1200           | 8400            |
		| Mandant_4 | 2012 | III                 | 1,2              | 720            | 10080           |
		| Mandant_4 | 2012 | IV                  | 1,3              | 360            | 5760            |
		| Mandant_7 | 2012 | IA                  | 4,2              | 1200           | 5700            |
		| Mandant_7 | 2012 | IB                  | 4,5              | 750            | 4125            |
		| Mandant_7 | 2012 | IC                  | 4,6              | 6200           | 43400           |
		| Mandant_7 | 2012 | II                  | 2,6              | 780            | 7800            |
		| Mandant_7 | 2012 | III                 | 3,6              | 930            | 13485           |
		| Mandant_7 | 2012 | IV                  | 3,9              | 450            | 6750            |
Und die folgenden KenngroessenFruehererJahre existieren:
    #max 6 rows per Jahr for each Belastungskategorie
        | Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche |
        | Mandant_6 | 2012 | IA                  | 1,3              | 100            | 475             |
        | Mandant_6 | 2012 | IB                  | 2,6              | 300            | 1725            |
        | Mandant_6 | 2012 | IC                  | 4,6              | 10000          | 70000           |
        | Mandant_6 | 2012 | II                  | 1,7              | 900            | 9000            |
        | Mandant_6 | 2012 | III                 | 0,2              | 800            | 9600            |
        | Mandant_6 | 2012 | IV                  | 3,4              | 200            | 2800            |
        | Mandant_8 | 2012 | IA                  | 2,6              | 1234           | 6170            |
        | Mandant_8 | 2012 | IB                  | 1,5              | 789            | 4734            |
        | Mandant_8 | 2012 | IC                  | 0,9              | 6789           | 47523           |
        | Mandant_8 | 2012 | II                  | 1,8              | 789            | 6312            |
        | Mandant_8 | 2012 | III                 | 2,4              | 901            | 8109            |
        | Mandant_8 | 2012 | IV                  | 3,7              | 456            | 5016            |
Und ich bin Benchmarkteilnehmer von 'Mandant_1'
Wenn ich die Benchmarkauswertung zu meinen Inventarkennwerten für das Jahre '2012' unter Berücksichtigung der Klassen 'EinwohnerGroesse' generiere
#Klassen: NetzGroesse,EinwohnerGroesse,Gemeinde,MittlereHoehenlageSiedlungsgebieteGroesse,OeffentlicheVerkehrsmittel,SteuerertragGroesse
Dann zeigt die Grafik folgende Ergebnisse 'B4_Inventarkennwerten_EinwohnerGroesse'
| Beschreigung           | Bezugroesse      | Einheit | Organisation | AnzahlGruppe | MittelwertGruppe | MininmalwertGruppe | MaximalwertGruppe | AnzahlAll | MittelwertAll | MininmalwertAll | MaximalwertAll |
| Gesamtlaenge           | Siedlungsflaeche | km/ha   | 0,12         | 5            |                  |                    |                   | 8         |               |                 |                |
| Gesamtlaenge           | Einwohner        | m/E     | 2,18         | 5            |                  |                    |                   | 8         |               |                 |                |
| Fahrbahnflaeche        | Siedlungsflaeche | m2/ha   | 699,2        | 5            |                  |                    |                   | 8         |               |                 |                |
| Fahrbahnflaeche        | Einwohner        | m2/E    | 12,31        | 5            |                  |                    |                   | 8         |               |                 |                |
| Gesamtstrassenflaeche  | Siedlungsflaeche | %       | 6,99         | 5            |                  |                    |                   | 8         |               |                 |                |
| Gesamtstrassenflaeche  | Einwohner        | m2/E    | 12,31        | 5            |                  |                    |                   | 8         |               |                 |                |
| IA                     |                  | %       | 7,61         | 5            |                  |                    |                   | 8         |               |                 |                |
| IB                     |                  | %       | 26,03        | 5            |                  |                    |                   | 8         |               |                 |                |
| IC                     |                  | %       | 12,30        | 5            |                  |                    |                   | 8         |               |                 |                |
| II                     |                  | %       | 32,61        | 5            |                  |                    |                   | 8         |               |                 |                |
| III                    |                  | %       | 5,43         | 5            |                  |                    |                   | 8         |               |                 |                |
| IV                     |                  | %       | 16,02        | 5            |                  |                    |                   | 8         |               |                 |                |
| Wiederbeschaffungswert | Fahrbahn         | CHF/m2  | 313,34       | 5            |                  |                    |                   | 8         |               |                 |                |
| Wiederbeschaffungswert | Einwohner        | CHF/E   | 3857,22      | 5            |                  |                    |                   | 8         |               |                 |                |
| Wertverlust            | Fahrbahn         | CHF/m2  | 4,61         | 5            |                  |                    |                   | 8         |               |                 |                |
| Wertverlust            | Einwohner        | CHF/E   | 56,80        | 5            |                  |                    |                   | 8         |               |                 |                |