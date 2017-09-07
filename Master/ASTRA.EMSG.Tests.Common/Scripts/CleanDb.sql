BEGIN
FOR i IN (SELECT table_name FROM user_tables) 
	LOOP 
		--IF SUBSTR(i.table_name, 0, 3) <> 'MVW' AND SUBSTR(i.table_name, 0, 2) <> 'VD' AND SUBSTR(i.table_name, 0, 4) <> 'MDRT' THEN
			EXECUTE IMMEDIATE('DROP TABLE ' || user || '.' || i.table_name || ' CASCADE CONSTRAINTS');    
		--END IF;
	END LOOP;
END;