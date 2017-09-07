Funktionalität: N5 - Achsenupdate visualisiert bekommen
	Als Data-Manager
	will ich Achsenupdates visualisiert bekommen
	damit ich sofort sehe, wo ich meine Strassenabschnitte aufgrund von Achsänderungen manuell bereinigen muss

@Manuell
Szenario: Das System visualisiert ausschliesslich jene Updates, die eine manuelle Bereinigung des Data-Managers erfordern
Gegeben sei eine Änderung der Achsendaten (Achssegmente)
Und diese Änderung konnte automatisiert für Strassenabschnitte durchgeführt werden
Dann wird diese Änderung nicht visualisiert

@Manuell
Szenario: Der Data-Manager kann die Visualisierung der Updates deaktivieren

@Manuell
Szenario: Der Data-Manager kann gemäss Anwendungsfall N4 wo nötig seine Strassenabschnitte bearbeiten

@Manuell
Szenario: Der Data-Manager löscht automatisch durch Bearbeitung von Strassenabschnitten die zugehörige Visualisierung
Gegeben sei eine visualisierte Änderung eines Achssegmentes
Und der Data-Manager bearbeitet nach Anwendungsfall N4 auf diesem Achssegment einen Strassenabschnitt
Dann wird diese Achssegmentänderung nicht mehr auf der Karte angezeigt

@Manuell
Szenario: Das System visualisiert auch neue und gelöschte Achsen
Gegeben sei eine neue Achse wurde im Basissystem hinzugefügt
Und das Achsenupdate wurde durchgeführt
Dann wird das neue Achssegement als Änderung auf der Karte angezeigt