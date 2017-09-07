Funktionalität: A6 - ausgewählte Einstellungen einzelner Organisationen verwalten
	Als Applikationsadministrator,
	will ich ausgewählte Einstellungen einzelner Organisationen (Mandanten) verwalten
	damit ich eine zentrale Steuerungsmöglichkeit wichtiger Basiseinstellungen habe

Grundlage: 
Gegeben sei ich bin Applikationsadministrator
Und es gibt die OwnerID G161-ZH

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Applikationsadministrator kann ausgewählte Einstellungen für die EMSG Mandanten verwalten
Dann ist das Menu für Einstellungen für die EMSG Mandanten verfügbar

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Applikationsadministrator kann einem Mandanten eine Owner ID (der Achsen) zuordnen
Gegeben sei die OwnerID G161-ZH ist noch keinen Mandantem zugeordnet
Wenn ich nach der Gemeindenummer 161 suche
Und ich für diese Gemeinde die OwnerID G161-ZH eintrage
Dann sieht der Mandant 161 (Zollikon) die Achsen seiner Gemeinde

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass die Owner ID, die vom Applikationsadministrator eingegeben wird, exisitiert
Gegeben sei es gibt die OwnerID 666 nicht
Wenn ich nach der Gemeindenummer 161 suche
Und ich für diese Gemeinde die OwnerID 666 eintrage
Dann erscheint eine Fehlermeldung

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System stellt sicher, dass eine Owner ID nur einem Mandanten zugeordnet wurde
Gegeben sei die OwnerID G161-ZH ist bereits einem Mandanten zugeordnet
Wenn ich nach der Gemeindenummer 261 suche
Und ich für diese Gemeinde die OwnerID G161-ZH eintrage
Dann erscheint eine Fehlermeldung