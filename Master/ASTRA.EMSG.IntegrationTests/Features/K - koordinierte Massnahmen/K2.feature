Funktionalität: K2 - Koordinierte Massnahmen im GIS-Modus erfassen
	Als Data-Manager,
	will ich koordinierte Massnahmen im GIS-Modus erfassen
	damit ich zeitgleiche Baumassnahmen verschiedener Systeme an einem Ort koordinieren kann 

Grundlage:
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus |
	| Mandant_1 | gis   |
Und ich bin Data-Manager von 'Mandant_1'

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann eine koordinierte Massnahme auf der Karte definieren
Wenn ich  die Seite 'Koordinierte Massnahmen' öffne
Und ich wähle 'Neue Koordinierte Massnahme anlegen' aus
Und ich wähle ein beliebiges dem 'Mandant_1' zugeordnetes Achssegement aus
Dann wird dieses Achssegment der koordinierte Massnahme zugeordnet

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann Anfangs- und Endpunkt der koordinierten Massnahme auf der Karte festlegen
Wenn ich die Seite 'Koordinierte Massnahmen' öffne
Und ich wähle ein beliebiges Achssegment aus
Und ich aktiviere das Tool 'koordinierten Massnahmengeometrie bearbeiten'
Wenn ich einen Anfangs oder Endpunkt entlang dem darunter liegenden Achssegment bewege
Dann wird der Strassenabschnitt entsprechend verlängert oder verkürzt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann alle Attribute der koordinierten Massnahme erfassen

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Fahrbahn (m²) nachdem der Data-Manager die koordinierte Massnahme gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir links (m²) nachdem der Data-Manager die koordinierte Massnahme gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System berechnet die Fläche Trottoir rechts (m²) nachdem der Data-Manager die koordinierte Massnahme gespeichert hat

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann alle Attribute der koordinierten Massnahme bearbeiten

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System visualisiert die erfassten Massnamen der Teilsysteme auf der Karte

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt koordinierte Massnahmen in einer Übersichtsliste dar

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann eine koordinierte Massnahme mittels Selektion auf der Karte löschen
Wenn ich die Seite 'Koordinierte Massnahmen' öffne
Und ich wähle eine korrdinierte Massnahme auf der Karte aus
Dann öffnet sich das Formular für koordinierte Massnahmen
Und ich dort "Löschen der korrdinierten Massnahem" auswähle
Dann wird die koordinierte Massnahme gelöscht und nicht mehr auf der Karte oder in Auswertungen (dieses Jahres) angezeigt

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Data-Manager kann in einem Textfeld nach Projektname suchen

#------------------------------------------------------------------------------------------------------------------------------------------------------