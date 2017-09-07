Funktionalität: A14 - Ausgewählte Systemparameter für meinen Mandanten pflegen
	Als Benutzeradministrator
	will ich ausgewählte Systemparameter für meinen Mandanten pflegen
	damit ich eine zentrale Steuerungsmöglichkeit wichtiger Basiseinstellungen habe

#------------------------------------------------------------------------------------------------------------------------------------------------------

Grundlage: 
Gegeben sei folgende Einstellungen existieren:
	| Mandant   | Modus         |
	| Mandant_1 | strassennamen |
Und ich bin Benutzeradministrator von Mandant_1 bin

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Benutzeradministrator kann ausgewählte Systemparameter, die für seinen Mandanten angewendet werden, pflegen
Wenn ich folgende Systemparameter je Belastungskategorie anpasse:
| Systemparameter                        | IA  | IB  | IC  | II  | III | IV  |
| Wiederbeschaffungswert Fläche Fahrbahn | 430 | 400 | 160 | 380 | 430 | 340 |
| Alterungsbeiwert I                     | 1,5 | 1,5 | 1,5 | 1,8 | 2,2 | 2,6 |
Und ich folgende Standard-Kosten-Werte für Massnahmenvorschläge je Belastungskategorie anpasse:
| Massnahmenvorschlag     | Typ      | IA | IB | IC | II | III | IV |
| Oberflächenverbesserung | Fahrbahn | 60 | 60 | 60 | 60 | 60  | 60 |
Und ich ein neues Logo für meinen Mandanten hochlade
Dann stehen die veränderten Parameter nur meinem Mandanten zur Verfügung.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System wendet die definierten Systemparameter für die Auswertungen des Mandanten an

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Wenn Systemparameter verändert werden, so soll das keine Auswirkung auf die Auswertungen abgeschlossener Jahre haben
Wenn ich folgende Systemparameter je Belastungskategorie anpasse:
| Systemparameter                        | IA  | IB  | IC  | II  | III | IV  |
| Wiederbeschaffungswert Fläche Fahrbahn | 430 | 400 | 160 | 380 | 430 | 340 |
| Alterungsbeiwert I                     | 1,5 | 1,5 | 1,5 | 1,8 | 2,2 | 2,6 |
Und ich folgende Standard-Kosten-Werte für Massnahmenvorschläge je Belastungskategorie anpasse:
| Massnahmenvorschlag     | Typ      | IA | IB | IC | II | III | IV |
| Oberflächenverbesserung | Fahrbahn | 60 | 60 | 60 | 60 | 60  | 60 |
Dann sind alle Auswertungen abgeschlossener Jahre aller Mandanten unverändert.