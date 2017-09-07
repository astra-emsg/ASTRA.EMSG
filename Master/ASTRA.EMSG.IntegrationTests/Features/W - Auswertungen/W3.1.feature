Funktionalität: W3.1 - Eine leere Vorlage des Erfassungsformulars für Oberflächenschäden erhalten
    Als Data-Reader,
	will ich eine Leere Vorlage des Erfassungsformulars für Oberflächenschäden erhalten
	damit ich die Zustandsaufnahme am Papier durchführen kann

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Data-Manager von 'Mandant_1'
	
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann wählen, ob er eine leere Vorlage des Erfassungsformulars für Oberflächenschäden für bitumenhaltigen Belag (Asphalt) oder für Betonbelag erhalten möchte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System liefert ein detailliertes Erfassungsformular für Oberflächenschäden entsprechend der Auswahl des Data-Managers

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das Erfassungsformular für bitumenhaltigen Belag umfasst die in Z2.1 definierten Felder für Oberflächenschäden
Wenn ich das leere Erfassungsformular für Asphalt (als XLS) herunterlade
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung 'W3.1_Asphalt'

#------------------------------------------------------------------------------------------------------------------------------------------------------

# should be easy to automate
@Manuell
Szenario: Das Erfassungsformular für Betonbelag umfasst die in Z2.1 definierten Felder für Oberflächenschäden
Wenn ich das leere Erfassungsformular für Asphalt (als XLS) herunterlade
Dann ist das Ergebnis das gleiche wie in der Referenz Auswertung 'W3.1_Beton'

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Aufbau des Erfassungsformulars entspricht Pflichtenheft, Abbildung 17

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Reader kann das leere Erfassungsformular als Excel exportieren
