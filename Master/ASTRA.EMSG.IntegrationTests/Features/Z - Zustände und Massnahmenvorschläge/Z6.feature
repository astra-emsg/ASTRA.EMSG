Funktionalität: Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren
	Als Data-Manager,
	will ich Inspektionsresultate aus EMSG-Mobile in EMSG-Master importieren
	damit ich die Daten, die ich mit EMSG-Mobile erfasst habe in EMSG-Master zur Verfügung habe

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann auf dem EMSG-Mobile eine Änderungsdatei (ChangeLog) erzeugen und diese lokal abspeichern.
Gegeben sei einen beliebige Manipulation nach Z3.1 von Zustandsabschnitten
Und ich wähle im EMSG Mobile 'Änderungsdatei / ChangeLog erzeugen'
Und ich wähle einen gültigen Pfad im Dateidialog
Dann werden alle Änderungen in die Änderungsdatei geschrieben und gespeichert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das ChangeLog auf den EMSG-Master hochladen
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |

Und ich bin Data-Manager von 'Mandant_1'
Und ich wähle die Seite 'Insepktionsergebnisse hochladen'
Und ich wähle eine gültige 'Änderungsdatei/ChangeLog' 
Dann wird diese 'Änderungsdatei/ChangeLog' auf den Server hochgeladen
Und der Import gestartet

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Hochgeladene ChangeLogs werden automatisch in EMSG importiert

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann im EMSG-Master die (zuvor ausgecheckten) Daten wieder bearbeiten
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
Und ich bin Data-Manager von 'Mandant_1'
Und ich exportiere eine Inspektionsroute 
Und ich importiere die Änderungen für diese Inspektionsroute wieder am EMSG Master
Und ich wähle einen Zustandsabschnitt, welcher Teil dieser Inspektionsroute ist
Dann kann ich diesen nach Z3.2 manipulieren

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager sieht die Änderungen nach erfolgreichem Check-In auf der Karte und in den Auswertungen
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
Und ich bin Data-Manager von 'Mandant_1'
Und ich exportiere eine Inspektionsroute 
Und ich erstelle einen neuen Zustandabschnitt am EMSG Mobile (nach Z3.1)
Und ich importiere die Änderungen im EMSG Master
Dann wird mit der Zustandabschnitt auf der Karte visualisiert und in der Liste angezeigt und ich kann diesen nach Z3.2 bearbeiten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager importiert das ChangeLog nur einmal in EMSG-Master
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |

Und ich bin Data-Manager von 'Mandant_1'
Und es liegt ein ChangLog vor
Und ich lade und importiere dieses erfolgreich am EMSG Master
Und ich versuche dieses nochmals hochzuladen und zu importieren
Dann erhalte ich eine Fehlermeldung


#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager erhält eine Erfolgsmeldung wenn die Check-In der Daten erfolgreich war

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager erhält eine Fehlermeldung wenn ein Fehler während des Check-In's aufgetreten ist.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager importiert immer nur alle getätigten Änderungen vom EMSG-Mobile in EMSG-Master
