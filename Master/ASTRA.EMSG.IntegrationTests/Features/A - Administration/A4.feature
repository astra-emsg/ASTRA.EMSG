Funktionalität: A4 - Achsenupdates einspielen
	Als Benutzeradministrator,
	will ich Achsenupdates einspielen
	damit ich mit den aktuellen Daten zu Achsen arbeite

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Benutzeradministrator kann die Aktualisierung der Achsen aufrufen
Gegeben sei ich öffne die Seite 'Arbeitsmodus wechseln'
Und ich wähle den Arbeitsmodus 'GIS Modus'
Wenn ich auf Achsenupdate klicke
Dann wird der Achseupdateprozess gestartet und die Achsen werden aktualisiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System holt sich automatisch die neuen Achsen für den Mandanten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System zieht automatisch Referenzen auf Achssegmente von Strassenabschnitten nach

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Strassenabschnitte immer nur auf den aktuellen Stand der Achsen definieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Dem Data-Manager steht eine Liste mit den Status der letzten Achsenaktualisierungen zur Verfügung
