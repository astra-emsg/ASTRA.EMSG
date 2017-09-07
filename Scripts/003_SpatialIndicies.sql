-- ===================================================
-- Description: Create Stpatial indicies
-- Author:	    Florian Kuster
-- Create date: 10.11.2016
-- ===================================================

create procedure #drop_index @p_index_name nvarchar(50), @p_table_name nvarchar(50) as

  
  BEGIN TRY
    EXEC('drop index ' + @p_index_name + ' on '+ @p_table_name)
	END TRY
  BEGIN CATCH
  SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
  
  GO
create procedure #drop_constraint @p_table_name nvarchar(50), @p_constraint_name nvarchar(50) as    
  
  
  BEGIN TRY
    EXEC ('alter table ' + @p_table_name + ' drop constraint ' + @p_constraint_name)
  END TRY
    BEGIN CATCH
	SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
  
  go

create procedure #create_index @p_table_name nvarchar(50), @p_spatial_index_name nvarchar(50), @p_shapefield_name nvarchar(50) as
  
    EXEC
      ('CREATE SPATIAL INDEX ' + @p_spatial_index_name +
			' ON ' + @p_table_name + ' (' + @p_shapefield_name + ') USING  GEOMETRY_GRID '+
			'WITH (BOUNDING_BOX =(485869.5728, 76443.1884, 837076.5648, 299941.7864), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), '+
			'CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)')

  
GO
  
#drop_index 'SPAT_IX_SRD_ACHSSEG_MSG', 'SRD_ACHSSEG_MSG'
GO
#drop_index 'SPAT_IX_ADD_ACHSREF_MSG', 'ADD_ACHSREF_MSG'
GO
#drop_index 'SPAT_IX_ADD_STRAGIS_MSG', 'ADD_STRAGIS_MSG'
GO
#drop_index 'SPAT_IX_ADD_ZSTGIS_MSG', 'ADD_ZSTGIS_MSG'
GO
#drop_index 'SPAT_IX_ADD_INSPEKROUTE_MSG', 'ADD_INSPEKROUTE_MSG'
GO
#drop_index 'SPAT_IX_SRD_KOPACHSSEG_MSG', 'SRD_KOPACHSSEG_MSG'
GO
#drop_index 'SPAT_IX_ADD_KOORMASSGIS_MSG', 'ADD_KOORMASSGIS_MSG'
GO
#drop_index 'SPAT_IX_ADD_MASSTEILGIS_MSG', 'ADD_MASSTEILGIS_MSG'
GO
#drop_index 'SPAT_IX_ADD_ACHSUPDCON_MSG', 'ADD_ACHSUPDCON_MSG'
GO
#drop_index 'SPAT_IX_ADD_REALMASSGIS_MSG', 'ADD_REALMASSGIS_MSG'
GO
#create_index 'SRD_ACHSSEG_MSG', 'SPAT_IX_SRD_ACHSSEG_MSG', 'ACS_SHAPE_VL'
GO
#create_index 'ADD_ACHSREF_MSG', 'SPAT_IX_ADD_ACHSREF_MSG', 'ACR_SHAPE_VL'
GO
#create_index 'ADD_STRAGIS_MSG', 'SPAT_IX_ADD_STRAGIS_MSG', 'STG_SHAPE_VL'
GO
#create_index 'ADD_ZSTGIS_MSG', 'SPAT_IX_ADD_ZSTGIS_MSG', 'ZSG_SHAPE_VL'
GO
#create_index 'ADD_INSPEKROUTE_MSG', 'SPAT_IX_ADD_INSPEKROUTE_MSG', 'IRG_SHAPE_VL'
GO
#create_index 'SRD_KOPACHSSEG_MSG', 'SPAT_IX_SRD_KOPACHSSEG_MSG', 'KSG_SHAPE_VL'
GO
#create_index 'ADD_KOORMASSGIS_MSG', 'SPAT_IX_ADD_KOORMASSGIS_MSG', 'KMG_SHAPE_VL'
GO
#create_index 'ADD_MASSTEILGIS_MSG', 'SPAT_IX_ADD_MASSTEILGIS_MSG', 'mtg_shape_vl'
GO
#create_index 'ADD_ACHSUPDCON_MSG', 'SPAT_IX_ADD_ACHSUPDCON_MSG', 'AUC_SHAPE_VL'
GO
#create_index 'ADD_REALMASSGIS_MSG', 'SPAT_IX_ADD_REALMASSGIS_MSG', 'RMG_SHAPE_VL'
GO


--
-- constraints for achskopie tables (not declared in fluent)

#drop_constraint 'SRD_KOPSEKTOR_MSG', 'CFK_KSG_KSK_NOR'
GO

EXEC('alter table SRD_KOPSEKTOR_MSG add constraint CFK_KSG_KSK_NOR foreign key (KSK_KSK_KSG_NOR_ID) references SRD_KOPACHSSEG_MSG')
GO

#drop_constraint 'SRD_KOPACHSSEG_MSG','CFK_KAC_KSG_NOR'
GO

EXEC('alter table SRD_KOPACHSSEG_MSG add constraint CFK_KAC_KSG_NOR foreign key (KSG_KSG_KAC_NOR_ID) references SRD_KOPACHSE_MSG')
GO

drop procedure #drop_index
GO
drop procedure #drop_constraint
GO
drop procedure #create_index
GO