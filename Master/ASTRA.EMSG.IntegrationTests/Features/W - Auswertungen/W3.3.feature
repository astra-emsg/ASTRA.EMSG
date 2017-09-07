Funktionalität: W3.3 - Ausgefüllte Erfassungsformulare für Oberflächenschäden zu jedem Strassenabschnitt erhalten
    Als Data-Reader,
	will ich ausgefüllte Erfassungsformulare für Oberflächenschäden zu jedem Strassenabschnitt erhalten
	damit ich diese als Basis für neue Erfassungen verwenden kann

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
Und folgende Netzinformationen existieren:
	| Id | Strassenname    | BezeichnungVon   | BezeichnungBis | Belagsart | 
	| 1  | Jesuitenbachweg | Hackl            | Schweighofer   | Asphalt   | 
	| 2  | Lagerstrasse    | Nr. 13           | Nr. 22         | Beton     | 
	| 3  | Föhrenweg       | Unterer Ortsteil | Lager          | Beton     | 
	| 4  | Gartenstrasse   | 1                | 66             | Asphalt   | 
Und folgende Zustandsinformationen existieren:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis | Aufnahmedatum | ZustandsIndexFahrbahn | Erfassungsmodus | 
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 1          | Nr. 7          | 12.10.2009    | 2,3                   | Detailliert     | 
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 8          | Nr. 12         | 12.10.2009    | 2,3                   | Detailliert     | 
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 13         | Nr. 55         | 23.03.2009    | 1,2                   | Detailliert     | 
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 0.0            | 2.1            | 21.05.2009    | 1,1                   | Detailliert     | 
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 2.1            | 5.3            | 21.05.2009    | 1,1                   | Detailliert     | 
	| 10 | Lagerstrasse    | Nr. 13                | Nr. 22                | 5.4            | 7.1            | 21.05.2009    | 2,1                   | Zustandsindex   | 
	| 11 | Föhrenweg       | Unterer Ortsteil      | Lager                 | Brunner        | Maier          | 21.10.2009    | 3,4                   | Grob            | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann ein Jahr auswählen, für das vom System die Auswertung generiert werden soll
Gegeben sei es gibt Jahresabschlüsse für folgende Jahre:
	| Jahr |
	| 2008 |
	| 2009 |
Dann kann ich folgende Jahre für ausgefüllte Erfassungsformulare auswählen:
	| Jahr                     |
	| Aktuelles Erfassungsjahr |
	| 2009                     |
	| 2008                     |
Und der Eintrag 'Aktuelles Erfassungsjahr' ist vorausgewählt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Aufbau des Erfassungsformulars entspricht Pflichtenheft, Abbildung 17

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System liefert die ausgefüllten detaillierten Erfassungsformulare für den Strassenabschnitt des Mandanten
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname    | AufnahemdatumVon | AufnahmedatumBis | ZustandsindexVon | ZustandsindexBis |
	| Alle       | Jesuitenbachweg |                  |                  | 0,0              | 5,0              |
Und ich die ausgefüllten  Erfassungsformulare herunterlade
Dann erhalte ich folgende ausgefüllten Erfassungsformulare:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis |
	| 5  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 1          | Nr. 7          |
	| 6  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 8          | Nr. 12         |
	| 7  | Jesuitenbachweg | Hackl                 | Schweighofer          | Nr. 13         | Nr. 55         | 

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System fügt jedem Schadenserfassungsformular den Header „Strassennamen“, „Bezeichnung von“ und „Bezeichnung bis“ des Zustandsabschnitts hinzu

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System bietet eine Vorschau der Zustandsabschnitte in Form einer Übersichtstabelle an
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | AufnahemdatumVon | AufnahmedatumBis | ZustandsindexVon | ZustandsindexBis |
	| Alle       |              |                  |                  | 0,0              | 5,0              |
Dann erhalte ich folgende Tabelle:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Aufnahmedatum | ZustandsIndexFahrbahn | DetailliertsSchadenserfassungsformular |
	| 5  | Jesuitenbachweg | Nr. 1          | Nr. 7          | 12.10.2009    | 2,3                   | Ja                                     |
	| 6  | Jesuitenbachweg | Nr. 8          | Nr. 12         | 12.10.2009    | 2,3                   | Ja                                     |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | 23.03.2009    | 1,2                   | Ja                                     |
	| 8  | Lagerstrasse    | 0.0            | 2.1            | 21.05.2009    | 1,1                   | Ja                                     |
	| 9  | Lagerstrasse    | 2.1            | 5.3            | 21.05.2009    | 1,1                   | Ja                                     |
	| 10 | Lagerstrasse    | 5.4            | 7.1            | 21.05.2009    | 2,1                   | Nein                                   |
	| 11 | Föhrenweg       | Brunner        | Maier          | 21.10.2009    | 3,4                   | Nein                                   |
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System generiert den Report ausschliesslich für Zustandsabschnitte deren Zustand mittels des detaillierten Schadenserfassungsformulars für Oberflächenschäden erfasst wurde
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | AufnahemdatumVon | AufnahmedatumBis | ZustandsindexVon | ZustandsindexBis |
	| Alle       | Lagerstrasse |                  |                  | 0,0              | 5,0              |
Und ich die ausgefüllten  Erfassungsformulare herunterlade
Dann erhalte ich folgende ausgefüllten Erfassungsformulare:
	| Id | Strassenname    | BezeichnungVonStrasse | BezeichnungBisStrasse | BezeichnungVon | BezeichnungBis |
	| 8  | Lagerstrasse    | Nr. 13                | Nr. 22                | 0.0            | 2.1            |  
	| 9  | Lagerstrasse    | Nr. 13                | Nr. 22                | 2.1            | 5.3            |  

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System kennzeichnet die getroffene Auswahl im Schadenserfassungsformular im Report mittels „x“

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann die zu liefernden Erfassungsformulare filtern
Wenn ich einen Jahresabschluss für das Jahr '2010' durchführe
Und ich die Vorschau der ausgefüllten Erfassungsformulare für Oberflächenschäden mit folgenden Filtern generiere:
	| Eigentümer | Strassenname | AufnahemdatumVon | AufnahmedatumBis | ZustandsindexVon | ZustandsindexBis |
	| Alle       |              | 23.03.2009       | 21.05.2009       | 1,1              | 1,2              |
Dann erhalte ich folgende Tabelle:
	| Id | Strassenname    | BezeichnungVon | BezeichnungBis | Aufnahmedatum | ZustandsIndexFahrbahn | DetailliertsSchadenserfassungsformular |
	| 7  | Jesuitenbachweg | Nr. 13         | Nr. 55         | 23.03.2009    | 1,2                   | Ja                                     |
	| 8  | Lagerstrasse    | 0.0            | 2.1            | 21.05.2009    | 1,1                   | Ja                                     |
	| 9  | Lagerstrasse    | 2.1            | 5.3            | 21.05.2009    | 1,1                   | Ja                                     |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann das Erfassungsformular als Excel exportieren