Funktionalität: Z5 - Inspektionsroutendaten auf EMSG-Mobile exportieren
	Als Data-Manager,
	will ich Inspektionsroutendaten auf EMSG-Mobile exportieren
	damit ich mit EMSG-Mobile Schäden und Massnahmenvorschläge erfassen kann

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager wählt beliebig viele Inspektionsrouten für den Export auf der Karte aus

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System exportiert die selektierten Inspektionsrouten auf EMSG-Mobile
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
Und ich wähle 'Selektierte Inspektionsrouten exportieren' 
Und ich wähle eine gültigen Pfad im Dateidialog aus 
Dann werden die für die im EMSG Mobile notwendigen Daten für die ausgewählten Inspektionsrouten gepseichert.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann zusätzliche Attribute zur Beschreibung der Inspektionsroute erfassen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann das Paket auch offline auf den Ziel Tablet-PC übertragen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System verhindert, dass der Data-Manager Strassenabschnitte und Zustandsabschnitte in EMSG-Master editieren kann, die auf den EMSG-Mobile exportiert wurden
Gegeben sei ein Zustandsabschnitt
Und dieser Zustandabschnitt liegt auf einem Strassenabschnitt, welcher einer exportierten Inspektionsroute zugeordnet ist
Und der Data-Manager bearbeitet nach Anwendungsfall Z3.2 den Zustandabschnitt
Dann erhält der Data-Manager eine Fehlermeldung und es werden keine Änderungen des Zustandsabschnittes gepseichert
#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt Hintergrundkarten auf EMSG-Mobile zur Verfügung
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |

Und ich habe eine Inspektionsroute selektiert
Und ich wähle 'Selektierte Inspektionsrouten exportieren' 
Dann werden Hintergrundkarten für den Berreich (Extent) der ausgewählten Inspektionsroute heruntergeladen.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Benutzeradministrator kann den "Check-Out" in EMSG-Master rückgängig machen
Gegeben sei folgende Einstellungen existieren:
		| Mandant   | Modus|
		| Mandant_1 | gis  |
Und ich bin Benutzeradministrator von 'Mandant_1'
Und ich öffne die Seite 'Inspektionsroute planen'
Und ich wähle in der Liste der Inspektionsroute 'Check-Out' rückgängig machen
Dann werden die Inspektionsrouten und die dazugehörigen Zustandsabschnitte wieder als nicht ausgecheckt markiert und können im EMSG Master bearbeitet werden.
