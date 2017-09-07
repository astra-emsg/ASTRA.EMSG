@Automatisch
Funktionalität: B5 – eine Benchmarkauswertung zu meinen Zustandskennwerten erhalten
	Als Benchmarkteilnehmer
	will ich eine Benchmarkauswertung zu meinen Zustandskennwerten erhalten
	damit die Werte meiner Gemeinde mit anderen Gemeinden vergleichen kann

#WARNING: Currently only the current year and the previous 9 years can be closed -> these tests will break in 2022

Szenario: Das System stellt sicher, dass die Benchmarkauswertung zu meinen Zustandskennwerten nur dann erzeugt wird, wenn zumindest 5 Gemeinden (4 zusätzliche zu der Gemeinde des Benchmarkteilnehmers) für die Benchmarkauswertung herangezogen werden können
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
Und die folgenden Zustandsabschnitte existieren:
		| Mandant   | Jahr | Strassenname     | Laenge | Aufnahmedatum | Zustandsindex |
		| Mandant_1 | 2012 | 1Bahnstrasse     | 68     | 17.06.2006    | 1,2           |
		| Mandant_1 | 2012 | 1Bahnstrasse     | 82     | 21.03.2012    | 4,2           |
		| Mandant_1 | 2012 | 1Moosgasse       | 266    | 17.07.2005    | 0,1           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 112    | 12.10.2012    | 3,5           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 207    | 14.06.2012    | 2,1           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 331    | 25.07.2006    | 2,7           |
		| Mandant_1 | 2012 | 1Gartenstrasse   | 430    | 07.03.2002    | 4,2           |
		| Mandant_1 | 2012 | 1Brunngasse      | 677    | 31.05.2012    | 4,7           |
		| Mandant_1 | 2012 | 1Brunngasse      | 523    | 02.07.2006    | 1,6           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 44     | 09.06.2005    | 3,2           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 82     | 16.10.2005    | 1,4           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 74     | 22.04.2006    | 3,2           |
		| Mandant_1 | 2012 | 1Bachstrasse     | 620    | 22.07.2006    | 4,1           |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 210    | 14.05.2012    | 2             |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 69     | 05.08.2005    | 4,8           |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 71     | 12.07.2001    | 0,7           |
		| Mandant_3 | 2012 | 3Bahnstrasse     | 200    | 15.05.2012    | 0,8           |
		| Mandant_3 | 2012 | 3Moosgasse       | 35     | 08.08.2012    | 1,9           |
		| Mandant_3 | 2012 | 3Moosgasse       | 265    | 19.03.2006    | 0,8           |
		| Mandant_3 | 2012 | 3Hauptstrasse    | 700    | 10.04.2012    | 2,2           |
		| Mandant_3 | 2012 | 3Gartenstrasse   | 500    | 25.06.2012    | 1,5           |
		| Mandant_3 | 2012 | 3Brunngasse      | 623    | 22.08.2005    | 4,3           |
		| Mandant_3 | 2012 | 3Brunngasse      | 285    | 16.07.2004    | 1,5           |
		| Mandant_3 | 2012 | 3Brunngasse      | 92     | 30.08.2005    | 4,3           |
		| Mandant_3 | 2012 | 3Föhrenweg       | 145    | 27.10.2006    | 1,7           |
		| Mandant_3 | 2012 | 3Föhrenweg       | 555    | 12.09.2006    | 3             |
		| Mandant_3 | 2012 | 3Bachstrasse     | 664    | 02.08.2005    | 0,4           |
		| Mandant_3 | 2012 | 3Bachstrasse     | 236    | 07.05.2012    | 2,5           |
		| Mandant_3 | 2012 | 3Friedhofstrasse | 27     | 11.04.2012    | 2             |
		| Mandant_3 | 2012 | 3Friedhofstrasse | 23     | 26.08.2005    | 3,5           |
Und die folgenden ZustandsabschnitteGIS existieren:
		| Mandant   | Jahr | Strassenname     | Laenge | Aufnahmedatum | Zustandsindex |
		| Mandant_2 | 2012 | 2Bahnstrasse     | 226    | 18.03.2005    | 2,6           |
		| Mandant_2 | 2012 | 2Bahnstrasse     | 194    | 10.09.2006    | 3,5           |
		| Mandant_2 | 2012 | 2Moosgasse       | 65     | 18.08.2012    | 4,2           |
		| Mandant_2 | 2012 | 2Moosgasse       | 95     | 10.09.2006    | 1,4           |
		| Mandant_2 | 2012 | 2Moosgasse       | 284    | 13.05.2005    | 1,5           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | 386    | 29.10.2006    | 2,7           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | 102    | 08.10.2006    | 1,5           |
		| Mandant_2 | 2012 | 2Gartenstrasse   | 700    | 03.07.2006    | 0,6           |
		| Mandant_2 | 2012 | 2Brunngasse      | 594    | 10.09.2005    | 3,5           |
		| Mandant_2 | 2012 | 2Brunngasse      | 194    | 12.07.2006    | 2,4           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 144    | 25.06.2006    | 1,6           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 258    | 14.05.2012    | 4,5           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 219    | 09.04.2012    | 3,7           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 78     | 04.09.2005    | 1,5           |
		| Mandant_2 | 2012 | 2Bachstrasse     | 246    | 20.04.2012    | 2,8           |
		| Mandant_2 | 2012 | 2Bachstrasse     | 87     | 13.08.2006    | 3,6           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | 126    | 16.10.2005    | 2,8           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | 318    | 19.07.2012    | 4,2           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 52     | 05.10.2012    | 0,8           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 34     | 21.07.2005    | 0,2           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 34     | 03.04.2012    | 3,9           |
		| Mandant_5 | 2012 | 5Moosgasse       | 75     | 28.10.2005    | 1,2           |
		| Mandant_5 | 2012 | 5Hauptstrasse    | 160    | 23.03.2005    | 2,1           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | 17     | 08.08.2012    | 3,6           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | 33     | 04.10.2006    | 3,7           |
		| Mandant_5 | 2012 | 5Brunngasse      | 7      | 07.10.2012    | 2,1           |
		| Mandant_5 | 2012 | 5Brunngasse      | 13     | 21.09.2005    | 4,1           |
		| Mandant_5 | 2012 | 5Brunngasse      | 17     | 18.04.2006    | 4,3           |
		| Mandant_5 | 2012 | 5Brunngasse      | 18     | 06.10.2006    | 3,1           |
		| Mandant_5 | 2012 | 5Föhrenweg       | 56     | 01.10.2005    | 3,5           |
		| Mandant_5 | 2012 | 5Föhrenweg       | 19     | 24.05.2006    | 3,8           |
		| Mandant_5 | 2012 | 5Bachstrasse     | 98     | 09.10.2006    | 2,7           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | 101    | 06.03.2012    | 0,5           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | 32     | 10.08.2012    | 1,7           |
Und die folgenden Details zum NetzSummarisch existieren: 
	#max 6 rows one for each Belastungskategorie
		| Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche | MittleresErhebungsJahr |
		| Mandant_4 | 2012 | IA                  | 2,2              | 3560           | 16910           | 08.08.2012             |
		| Mandant_4 | 2012 | IB                  | 2,7              | 250            | 1250            | 08.08.2012             |
		| Mandant_4 | 2012 | IC                  | 3,8              | 460            | 2530            | 08.08.2012             |
		| Mandant_4 | 2012 | II                  | 4,8              | 1200           | 8400            | 08.08.2012             |
		| Mandant_4 | 2012 | III                 | 1,2              | 720            | 10080           | 08.08.2012             |
		| Mandant_4 | 2012 | IV                  | 1,3              | 360            | 5760            | 08.08.2012             |
		| Mandant_7 | 2012 | IA                  | 4,2              | 1200           | 5700            | 03.04.2012             |
		| Mandant_7 | 2012 | IB                  | 4,5              | 750            | 4125            | 03.04.2012             |
		| Mandant_7 | 2012 | IC                  | 4,6              | 6200           | 43400           | 03.04.2012             |
		| Mandant_7 | 2012 | II                  | 2,6              | 780            | 7800            | 03.04.2012             |
		| Mandant_7 | 2012 | III                 | 3,6              | 930            | 13485           | 03.04.2012             |
		| Mandant_7 | 2012 | IV                  | 3,9              | 450            | 6750            | 03.04.2012             |
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
Wenn ich die Benchmarkauswertung zu meinen Zustandskennwerten für das Jahre '2012' unter Berücksichtigung der Klassen 'Gemeinde,OeffentlicheVerkehrsmittel' generiere
#Klassen: Netzgroesse,Einwohnergroesse,Gemeindetyp,Siedlungsgebiete,OeffentlicheVerkehrsmittel,Steureikommen
Dann informiert mich das System, dass zu wenige Mandanten für die Benchmarkauswertung zur Verfügung stehen und die Auswertung nicht generiert werden kann


Szenario: Das System liefert eine Tabelle gemäss Abbildung 77
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
Und die folgenden Zustandsabschnitte existieren:
		| Mandant   | Jahr | Strassenname     | Laenge | Aufnahmedatum | Zustandsindex |
		| Mandant_1 | 2012 | 1Bahnstrasse     | 68     | 17.06.2006    | 1,2           |
		| Mandant_1 | 2012 | 1Bahnstrasse     | 82     | 21.03.2012    | 4,2           |
		| Mandant_1 | 2012 | 1Moosgasse       | 266    | 17.07.2005    | 0,1           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 112    | 12.10.2012    | 3,5           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 207    | 14.06.2012    | 2,1           |
		| Mandant_1 | 2012 | 1Hauptstrasse    | 331    | 25.07.2006    | 2,7           |
		| Mandant_1 | 2012 | 1Gartenstrasse   | 430    | 07.03.2002    | 4,2           |
		| Mandant_1 | 2012 | 1Brunngasse      | 677    | 31.05.2012    | 4,7           |
		| Mandant_1 | 2012 | 1Brunngasse      | 523    | 02.07.2006    | 1,6           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 44     | 09.06.2005    | 3,2           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 82     | 16.10.2005    | 1,4           |
		| Mandant_1 | 2012 | 1Föhrenweg       | 74     | 22.04.2006    | 3,2           |
		| Mandant_1 | 2012 | 1Bachstrasse     | 620    | 22.07.2006    | 4,1           |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 210    | 14.05.2012    | 2             |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 69     | 05.08.2005    | 4,8           |
		| Mandant_1 | 2012 | 1Friedhofstrasse | 71     | 12.07.2001    | 0,7           |
		| Mandant_3 | 2012 | 3Bahnstrasse     | 200    | 15.05.2012    | 0,8           |
		| Mandant_3 | 2012 | 3Moosgasse       | 35     | 08.08.2012    | 1,9           |
		| Mandant_3 | 2012 | 3Moosgasse       | 265    | 19.03.2006    | 0,8           |
		| Mandant_3 | 2012 | 3Hauptstrasse    | 700    | 10.04.2012    | 2,2           |
		| Mandant_3 | 2012 | 3Gartenstrasse   | 500    | 25.06.2012    | 1,5           |
		| Mandant_3 | 2012 | 3Brunngasse      | 623    | 22.08.2005    | 4,3           |
		| Mandant_3 | 2012 | 3Brunngasse      | 285    | 16.07.2004    | 1,5           |
		| Mandant_3 | 2012 | 3Brunngasse      | 92     | 30.08.2005    | 4,3           |
		| Mandant_3 | 2012 | 3Föhrenweg       | 145    | 27.10.2006    | 1,7           |
		| Mandant_3 | 2012 | 3Föhrenweg       | 555    | 12.09.2006    | 3             |
		| Mandant_3 | 2012 | 3Bachstrasse     | 664    | 02.08.2005    | 0,4           |
		| Mandant_3 | 2012 | 3Bachstrasse     | 236    | 07.05.2012    | 2,5           |
		| Mandant_3 | 2012 | 3Friedhofstrasse | 27     | 11.04.2012    | 2             |
		| Mandant_3 | 2012 | 3Friedhofstrasse | 23     | 26.08.2005    | 3,5           |
Und die folgenden ZustandsabschnitteGIS existieren:
		| Mandant   | Jahr | Strassenname     | Laenge | Aufnahmedatum | Zustandsindex |
		| Mandant_2 | 2012 | 2Bahnstrasse     | 226    | 18.03.2005    | 2,6           |
		| Mandant_2 | 2012 | 2Bahnstrasse     | 194    | 10.09.2006    | 3,5           |
		| Mandant_2 | 2012 | 2Moosgasse       | 65     | 18.08.2012    | 4,2           |
		| Mandant_2 | 2012 | 2Moosgasse       | 95     | 10.09.2006    | 1,4           |
		| Mandant_2 | 2012 | 2Moosgasse       | 284    | 13.05.2005    | 1,5           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | 386    | 29.10.2006    | 2,7           |
		| Mandant_2 | 2012 | 2Hauptstrasse    | 102    | 08.10.2006    | 1,5           |
		| Mandant_2 | 2012 | 2Gartenstrasse   | 700    | 03.07.2006    | 0,6           |
		| Mandant_2 | 2012 | 2Brunngasse      | 594    | 10.09.2005    | 3,5           |
		| Mandant_2 | 2012 | 2Brunngasse      | 194    | 12.07.2006    | 2,4           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 144    | 25.06.2006    | 1,6           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 258    | 14.05.2012    | 4,5           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 219    | 09.04.2012    | 3,7           |
		| Mandant_2 | 2012 | 2Föhrenweg       | 78     | 04.09.2005    | 1,5           |
		| Mandant_2 | 2012 | 2Bachstrasse     | 246    | 20.04.2012    | 2,8           |
		| Mandant_2 | 2012 | 2Bachstrasse     | 87     | 13.08.2006    | 3,6           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | 126    | 16.10.2005    | 2,8           |
		| Mandant_2 | 2012 | 2Friedhofstrasse | 318    | 19.07.2012    | 4,2           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 52     | 05.10.2012    | 0,8           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 34     | 21.07.2005    | 0,2           |
		| Mandant_5 | 2012 | 5Bahnstrasse     | 34     | 03.04.2012    | 3,9           |
		| Mandant_5 | 2012 | 5Moosgasse       | 75     | 28.10.2005    | 1,2           |
		| Mandant_5 | 2012 | 5Hauptstrasse    | 160    | 23.03.2005    | 2,1           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | 17     | 08.08.2012    | 3,6           |
		| Mandant_5 | 2012 | 5Gartenstrasse   | 33     | 04.10.2006    | 3,7           |
		| Mandant_5 | 2012 | 5Brunngasse      | 7      | 07.10.2012    | 2,1           |
		| Mandant_5 | 2012 | 5Brunngasse      | 13     | 21.09.2005    | 4,1           |
		| Mandant_5 | 2012 | 5Brunngasse      | 17     | 18.04.2006    | 4,3           |
		| Mandant_5 | 2012 | 5Brunngasse      | 18     | 06.10.2006    | 3,1           |
		| Mandant_5 | 2012 | 5Föhrenweg       | 56     | 01.10.2005    | 3,5           |
		| Mandant_5 | 2012 | 5Föhrenweg       | 19     | 24.05.2006    | 3,8           |
		| Mandant_5 | 2012 | 5Bachstrasse     | 98     | 09.10.2006    | 2,7           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | 101    | 06.03.2012    | 0,5           |
		| Mandant_5 | 2012 | 5Friedhofstrasse | 32     | 10.08.2012    | 1,7           |
Und die folgenden Details zum NetzSummarisch existieren: 
	#max 6 rows one for each Belastungskategorie
		| Mandant   | Jahr | Belastungskategorie | MittlererZustand | Fahrbahnlaenge | Fahrbahnflaeche | MittleresErhebungsJahr |
		| Mandant_4 | 2012 | IA                  | 2,2              | 3560           | 16910           | 08.08.2012             |
		| Mandant_4 | 2012 | IB                  | 2,7              | 250            | 1250            | 08.08.2012             |
		| Mandant_4 | 2012 | IC                  | 3,8              | 460            | 2530            | 08.08.2012             |
		| Mandant_4 | 2012 | II                  | 4,8              | 1200           | 8400            | 08.08.2012             |
		| Mandant_4 | 2012 | III                 | 1,2              | 720            | 10080           | 08.08.2012             |
		| Mandant_4 | 2012 | IV                  | 1,3              | 360            | 5760            | 08.08.2012             |
		| Mandant_7 | 2012 | IA                  | 4,2              | 1200           | 5700            | 03.04.2012             |
		| Mandant_7 | 2012 | IB                  | 4,5              | 750            | 4125            | 03.04.2012             |
		| Mandant_7 | 2012 | IC                  | 4,6              | 6200           | 43400           | 03.04.2012             |
		| Mandant_7 | 2012 | II                  | 2,6              | 780            | 7800            | 03.04.2012             |
		| Mandant_7 | 2012 | III                 | 3,6              | 930            | 13485           | 03.04.2012             |
		| Mandant_7 | 2012 | IV                  | 3,9              | 450            | 6750            | 03.04.2012             |
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
Wenn ich die Benchmarkauswertung zu meinen Zustandskennwerten für das Jahre '2012' unter Berücksichtigung der Klassen 'OeffentlicheVerkehrsmittel' generiere
#Klassen: Netzgroesse,Einwohnergroesse,Gemeindetyp,Siedlungsgebiete,OeffentlicheVerkehrsmittel,Steureikommen
Dann zeigt die Grafik folgende Ergebnisse 'B5_Zustandskennwerten_OeffentlicheVerkehrsmittel'
| Beschreigung                 | Einheit | Organisation | AnzahlGruppe | MittelwertGruppe | MininmalwertGruppe | MaximalwertGruppe | AnzahlAll | MittelwertAll | MininmalwertAll | MaximalwertAll |
| Zustandindex                 |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| IA                           |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| IB                           |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| IC                           |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| II                           |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| III                          |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| IV                           |         |              | 7            |                  |                    |                   | 8         |               |                 |                |
| MittleresAltZustandsaufnamen | datum   |              | 7            |                  |                    |                   | 8         |               |                 |                |