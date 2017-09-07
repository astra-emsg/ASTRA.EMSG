Funktionalität: A13 - Für den summarischen Modus auswählen, welcher Mengentyp für meinen Mandanten am UI angezeigt wird
	Als Benutzeradministrator,
	will ich für den summarischen Modus auswählen welcher Mengentyp für meinen Mandanten am UI angezeigt wird
	damit ich den Arbeitsaufwand für die summarische Erfassung meines Mandanten steuern kann

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Benutzeradministrator muss im Rahmen eines Moduswechsels zum summarischen Modus angeben, welcher Mengentyp für seinen Mandanten am UI angezeigt wird
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen | 
Und ich bin Benutzeradministrator von 'Mandant_1'
Wenn in den 'summarischen Modus' wechsel
Dann kann ich den Mengentyp für meinen Mandanten auswählen

#------------------------------------------------------------------------------------------------------------------------------------------------------
