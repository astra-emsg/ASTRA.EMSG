-- ===================================================
-- Description: Initial tables to be able to use the EMSG
-- Author:	    Balazs Epresi
-- Create date: 14.08.2017
-- ===================================================

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_ACH_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSE_MSG'))
alter table SRD_ACHSE_MSG  drop constraint CFK_MAN_ACH_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_ACH_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSE_MSG'))
alter table SRD_ACHSE_MSG  drop constraint CFK_EPR_ACH_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACH_ACH_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSE_MSG'))
alter table SRD_ACHSE_MSG  drop constraint CFK_ACH_ACH_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_ALK_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSLOCK_MSG'))
alter table ADD_ACHSLOCK_MSG  drop constraint CFK_MAN_ALK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACR_ACR_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSREF_MSG'))
alter table ADD_ACHSREF_MSG  drop constraint CFK_ACR_ACR_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACS_ACR_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSREF_MSG'))
alter table ADD_ACHSREF_MSG  drop constraint CFK_ACS_ACR_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_ACR_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSREF_MSG'))
alter table ADD_ACHSREF_MSG  drop constraint CFK_RFG_ACR_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACH_ACS_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSSEG_MSG'))
alter table SRD_ACHSSEG_MSG  drop constraint CFK_ACH_ACS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_ACS_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSSEG_MSG'))
alter table SRD_ACHSSEG_MSG  drop constraint CFK_EPR_ACS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_ACS_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSSEG_MSG'))
alter table SRD_ACHSSEG_MSG  drop constraint CFK_MAN_ACS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACS_ACS_NOR]') AND parent_object_id = OBJECT_ID('SRD_ACHSSEG_MSG'))
alter table SRD_ACHSSEG_MSG  drop constraint CFK_ACS_ACS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_AUC_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSUPDCON_MSG'))
alter table ADD_ACHSUPDCON_MSG  drop constraint CFK_MAN_AUC_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_AUC_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSUPDCON_MSG'))
alter table ADD_ACHSUPDCON_MSG  drop constraint CFK_EPR_AUC_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_AUL_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSUPDLOG_MSG'))
alter table ADD_ACHSUPDLOG_MSG  drop constraint CFK_MAN_AUL_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_AUL_NOR]') AND parent_object_id = OBJECT_ID('ADD_ACHSUPDLOG_MSG'))
alter table ADD_ACHSUPDLOG_MSG  drop constraint CFK_EPR_AUL_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK3933E69CC3E55D6A]') AND parent_object_id = OBJECT_ID('ADD_ALBELI_MSG'))
alter table ADD_ALBELI_MSG  drop constraint FK3933E69CC3E55D6A
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_BDD_NOR]') AND parent_object_id = OBJECT_ID('ADD_BEDATADET_MSG'))
alter table ADD_BEDATADET_MSG  drop constraint CFK_BLK_BDD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BDT_BDD_NOR]') AND parent_object_id = OBJECT_ID('ADD_BEDATADET_MSG'))
alter table ADD_BEDATADET_MSG  drop constraint CFK_BDT_BDD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_BDT_NOR]') AND parent_object_id = OBJECT_ID('ADD_BENCHDATA_MSG'))
alter table ADD_BENCHDATA_MSG  drop constraint CFK_EPR_BDT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_BDT_NOR]') AND parent_object_id = OBJECT_ID('ADD_BENCHDATA_MSG'))
alter table ADD_BENCHDATA_MSG  drop constraint CFK_MAN_BDT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_COG_NOR]') AND parent_object_id = OBJECT_ID('ADD_CHECKOUT_MSG'))
alter table ADD_CHECKOUT_MSG  drop constraint CFK_MAN_COG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_IRG_COG_NOR]') AND parent_object_id = OBJECT_ID('ADD_CHECKOUT_MSG'))
alter table ADD_CHECKOUT_MSG  drop constraint CFK_IRG_COG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_EPR_NOR]') AND parent_object_id = OBJECT_ID('ADD_ERFPERIODE_MSG'))
alter table ADD_ERFPERIODE_MSG  drop constraint CFK_MAN_EPR_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_GMK_NOR]') AND parent_object_id = OBJECT_ID('VAT_GMASSVOR_MSG'))
alter table VAT_GMASSVOR_MSG  drop constraint CFK_BLK_GMK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_GMK_NOR]') AND parent_object_id = OBJECT_ID('VAT_GMASSVOR_MSG'))
alter table VAT_GMASSVOR_MSG  drop constraint CFK_MTK_GMK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_GWK_NOR]') AND parent_object_id = OBJECT_ID('VAT_GWBBKAT_MSG'))
alter table VAT_GWBBKAT_MSG  drop constraint CFK_BLK_GWK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_IRG_NOR]') AND parent_object_id = OBJECT_ID('ADD_INSPEKROUTE_MSG'))
alter table ADD_INSPEKROUTE_MSG  drop constraint CFK_MAN_IRG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_IRG_NOR]') AND parent_object_id = OBJECT_ID('ADD_INSPEKROUTE_MSG'))
alter table ADD_INSPEKROUTE_MSG  drop constraint CFK_EPR_IRG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_IRG_IRV_NOR]') AND parent_object_id = OBJECT_ID('ADD_INSPEKSTATU_MSG'))
alter table ADD_INSPEKSTATU_MSG  drop constraint CFK_IRG_IRV_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_STG_IRS_NOR]') AND parent_object_id = OBJECT_ID('ADD_INSPEKSTRA_MSG'))
alter table ADD_INSPEKSTRA_MSG  drop constraint CFK_STG_IRS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_IRG_IRS_NOR]') AND parent_object_id = OBJECT_ID('ADD_INSPEKSTRA_MSG'))
alter table ADD_INSPEKSTRA_MSG  drop constraint CFK_IRG_IRS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_KFD_NOR]') AND parent_object_id = OBJECT_ID('ADD_KENGRFJDET_MSG'))
alter table ADD_KENGRFJDET_MSG  drop constraint CFK_BLK_KFD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_KFJ_KFD_NOR]') AND parent_object_id = OBJECT_ID('ADD_KENGRFJDET_MSG'))
alter table ADD_KENGRFJDET_MSG  drop constraint CFK_KFJ_KFD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_KFJ_NOR]') AND parent_object_id = OBJECT_ID('ADD_KENGRFJ_MSG'))
alter table ADD_KENGRFJ_MSG  drop constraint CFK_MAN_KFJ_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_KMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_KOORMASSGIS_MSG'))
alter table ADD_KOORMASSGIS_MSG  drop constraint CFK_MTK_KMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_KMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_KOORMASSGIS_MSG'))
alter table ADD_KOORMASSGIS_MSG  drop constraint CFK_RFG_KMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_KMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_KOORMASSGIS_MSG'))
alter table ADD_KOORMASSGIS_MSG  drop constraint CFK_MAN_KMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK457374AC70368936]') AND parent_object_id = OBJECT_ID('ADD_BETSYS_MSG'))
alter table ADD_BETSYS_MSG  drop constraint FK457374AC70368936
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_GEK_MAD_NOR]') AND parent_object_id = OBJECT_ID('ADD_MANDANTDET_MSG'))
alter table ADD_MANDANTDET_MSG  drop constraint CFK_GEK_MAD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_OVG_MAD_NOR]') AND parent_object_id = OBJECT_ID('ADD_MANDANTDET_MSG'))
alter table ADD_MANDANTDET_MSG  drop constraint CFK_OVG_MAD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_MAD_NOR]') AND parent_object_id = OBJECT_ID('ADD_MANDANTDET_MSG'))
alter table ADD_MANDANTDET_MSG  drop constraint CFK_MAN_MAD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_MAD_NOR]') AND parent_object_id = OBJECT_ID('ADD_MANDANTDET_MSG'))
alter table ADD_MANDANTDET_MSG  drop constraint CFK_EPR_MAD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_MAL_NOR]') AND parent_object_id = OBJECT_ID('ADD_MANDANTLOGO_MSG'))
alter table ADD_MANDANTLOGO_MSG  drop constraint CFK_MAN_MAL_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_MVK_NOR]') AND parent_object_id = OBJECT_ID('VAT_MASSVORSCH_MSG'))
alter table VAT_MASSVORSCH_MSG  drop constraint CFK_MTK_MVK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_MVK_NOR]') AND parent_object_id = OBJECT_ID('VAT_MASSVORSCH_MSG'))
alter table VAT_MASSVORSCH_MSG  drop constraint CFK_BLK_MVK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_MVK_NOR]') AND parent_object_id = OBJECT_ID('VAT_MASSVORSCH_MSG'))
alter table VAT_MASSVORSCH_MSG  drop constraint CFK_MAN_MVK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_MVK_NOR]') AND parent_object_id = OBJECT_ID('VAT_MASSVORSCH_MSG'))
alter table VAT_MASSVORSCH_MSG  drop constraint CFK_EPR_MVK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_MTG_NOR]') AND parent_object_id = OBJECT_ID('ADD_MASSTEILGIS_MSG'))
alter table ADD_MASSTEILGIS_MSG  drop constraint CFK_RFG_MTG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_MTG_NOR]') AND parent_object_id = OBJECT_ID('ADD_MASSTEILGIS_MSG'))
alter table ADD_MASSTEILGIS_MSG  drop constraint CFK_MAN_MTG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_NSD_NOR]') AND parent_object_id = OBJECT_ID('ADD_NETZSUMDET_MSG'))
alter table ADD_NETZSUMDET_MSG  drop constraint CFK_BLK_NSD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_NSU_NSD_NOR]') AND parent_object_id = OBJECT_ID('ADD_NETZSUMDET_MSG'))
alter table ADD_NETZSUMDET_MSG  drop constraint CFK_NSU_NSD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_NSU_NOR]') AND parent_object_id = OBJECT_ID('ADD_NETZSUM_MSG'))
alter table ADD_NETZSUM_MSG  drop constraint CFK_MAN_NSU_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_NSU_NOR]') AND parent_object_id = OBJECT_ID('ADD_NETZSUM_MSG'))
alter table ADD_NETZSUM_MSG  drop constraint CFK_EPR_NSU_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_RMT_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSTAB_MSG'))
alter table ADD_RELMASSTAB_MSG  drop constraint CFK_MTK_RMT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_RMT_TR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSTAB_MSG'))
alter table ADD_RELMASSTAB_MSG  drop constraint CFK_MTK_RMT_TR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_RMT_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSTAB_MSG'))
alter table ADD_RELMASSTAB_MSG  drop constraint CFK_BLK_RMT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_RMT_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSTAB_MSG'))
alter table ADD_RELMASSTAB_MSG  drop constraint CFK_EPR_RMT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_RMT_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSTAB_MSG'))
alter table ADD_RELMASSTAB_MSG  drop constraint CFK_MAN_RMT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_RMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_MTK_RMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MTK_RMG_TR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_MTK_RMG_TR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_RMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_BLK_RMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_RMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_RFG_RMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_RMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_MAN_RMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_RMG_NOR]') AND parent_object_id = OBJECT_ID('ADD_REALMASSGIS_MSG'))
alter table ADD_REALMASSGIS_MSG  drop constraint CFK_EPR_RMG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2991BD4A94BB00D]') AND parent_object_id = OBJECT_ID('ADD_BETSYSRM_MSG'))
alter table ADD_BETSYSRM_MSG  drop constraint FK2991BD4A94BB00D
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_RMS_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSSUM_MSG'))
alter table ADD_RELMASSSUM_MSG  drop constraint CFK_EPR_RMS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_RMS_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSSUM_MSG'))
alter table ADD_RELMASSSUM_MSG  drop constraint CFK_MAN_RMS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_RMS_NOR]') AND parent_object_id = OBJECT_ID('ADD_RELMASSSUM_MSG'))
alter table ADD_RELMASSSUM_MSG  drop constraint CFK_BLK_RMS_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_RFG_NOR]') AND parent_object_id = OBJECT_ID('ADR_REFGRUPPE_MSG'))
alter table ADR_REFGRUPPE_MSG  drop constraint CFK_RFG_RFG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ZST_SCD_NOR]') AND parent_object_id = OBJECT_ID('ADD_SCHADDET_MSG'))
alter table ADD_SCHADDET_MSG  drop constraint CFK_ZST_SCD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ZSG_SCD_NOR]') AND parent_object_id = OBJECT_ID('ADD_SCHADDET_MSG'))
alter table ADD_SCHADDET_MSG  drop constraint CFK_ZSG_SCD_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ZST_SCG_NOR]') AND parent_object_id = OBJECT_ID('ADD_SCHADGRUPPE_MSG'))
alter table ADD_SCHADGRUPPE_MSG  drop constraint CFK_ZST_SCG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ZSG_SCG_NOR]') AND parent_object_id = OBJECT_ID('ADD_SCHADGRUPPE_MSG'))
alter table ADD_SCHADGRUPPE_MSG  drop constraint CFK_ZSG_SCG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ACS_SEK_NOR]') AND parent_object_id = OBJECT_ID('SRD_SEKTOR_MSG'))
alter table SRD_SEKTOR_MSG  drop constraint CFK_ACS_SEK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_SEK_SEK_NOR]') AND parent_object_id = OBJECT_ID('SRD_SEKTOR_MSG'))
alter table SRD_SEKTOR_MSG  drop constraint CFK_SEK_SEK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_STG_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRAGIS_MSG'))
alter table ADD_STRAGIS_MSG  drop constraint CFK_BLK_STG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_STG_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRAGIS_MSG'))
alter table ADD_STRAGIS_MSG  drop constraint CFK_MAN_STG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_STG_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRAGIS_MSG'))
alter table ADD_STRAGIS_MSG  drop constraint CFK_EPR_STG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_STG_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRAGIS_MSG'))
alter table ADD_STRAGIS_MSG  drop constraint CFK_RFG_STG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_STG_STG_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRAGIS_MSG'))
alter table ADD_STRAGIS_MSG  drop constraint CFK_STG_STG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_STT_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRATAB_MSG'))
alter table ADD_STRATAB_MSG  drop constraint CFK_BLK_STT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_STT_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRATAB_MSG'))
alter table ADD_STRATAB_MSG  drop constraint CFK_MAN_STT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_STT_NOR]') AND parent_object_id = OBJECT_ID('ADD_STRATAB_MSG'))
alter table ADD_STRATAB_MSG  drop constraint CFK_EPR_STT_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_BLK_WBK_NOR]') AND parent_object_id = OBJECT_ID('VAT_WBBKAT_MSG'))
alter table VAT_WBBKAT_MSG  drop constraint CFK_BLK_WBK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MAN_WBK_NOR]') AND parent_object_id = OBJECT_ID('VAT_WBBKAT_MSG'))
alter table VAT_WBBKAT_MSG  drop constraint CFK_MAN_WBK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_EPR_WBK_NOR]') AND parent_object_id = OBJECT_ID('VAT_WBBKAT_MSG'))
alter table VAT_WBBKAT_MSG  drop constraint CFK_EPR_WBK_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZSG_FB]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_MVK_ZSG_FB
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZSG_TRR]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_MVK_ZSG_TRR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZSG_TRL]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_MVK_ZSG_TRL
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_STG_ZSG_NOR]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_STG_ZSG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_RFG_ZSG_NOR]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_RFG_ZSG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_ZSG_ZSG_NOR]') AND parent_object_id = OBJECT_ID('ADD_ZSTGIS_MSG'))
alter table ADD_ZSTGIS_MSG  drop constraint CFK_ZSG_ZSG_NOR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZST_FB]') AND parent_object_id = OBJECT_ID('ADD_ZSTTAB_MSG'))
alter table ADD_ZSTTAB_MSG  drop constraint CFK_MVK_ZST_FB
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZST_TRR]') AND parent_object_id = OBJECT_ID('ADD_ZSTTAB_MSG'))
alter table ADD_ZSTTAB_MSG  drop constraint CFK_MVK_ZST_TRR
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_MVK_ZST_TRL]') AND parent_object_id = OBJECT_ID('ADD_ZSTTAB_MSG'))
alter table ADD_ZSTTAB_MSG  drop constraint CFK_MVK_ZST_TRL
;
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[CFK_STT_ZST_NOR]') AND parent_object_id = OBJECT_ID('ADD_ZSTTAB_MSG'))
alter table ADD_ZSTTAB_MSG  drop constraint CFK_STT_ZST_NOR
;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_ACHSE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_ACHSE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ACHSIMPLOG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ACHSIMPLOG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ACHSLOCK_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ACHSLOCK_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ACHSREF_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ACHSREF_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_ACHSSEG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_ACHSSEG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ACHSUPDCON_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ACHSUPDCON_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ACHSUPDLOG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ACHSUPDLOG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_BELKAT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_BELKAT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ALBELI_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ALBELI_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_BEDATADET_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_BEDATADET_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_BENCHDATA_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_BENCHDATA_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_BENCHGRCFG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_BENCHGRCFG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_CHECKOUT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_CHECKOUT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_EREIGNISLOG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_EREIGNISLOG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ERFPERIODE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ERFPERIODE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_GEMKAT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_GEMKAT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_GMASSVOR_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_GMASSVOR_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_GWBBKAT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_GWBBKAT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_INSPEKROUTE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_INSPEKROUTE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_INSPEKSTATU_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_INSPEKSTATU_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_INSPEKSTRA_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_INSPEKSTRA_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_KENGRFJDET_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_KENGRFJDET_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_KENGRFJ_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_KENGRFJ_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_KOORMASSGIS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_KOORMASSGIS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_BETSYS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_BETSYS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_KOPACHSE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_KOPACHSE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_KOPACHSSEG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_KOPACHSSEG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_KOPSEKTOR_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_KOPSEKTOR_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_MANDANTDET_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_MANDANTDET_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_MANDANTLOGO_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_MANDANTLOGO_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_MANDANT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_MANDANT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_MASSTYP_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_MASSTYP_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_MASSVORSCH_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_MASSVORSCH_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_MASSTEILGIS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_MASSTEILGIS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_NETZSUMDET_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_NETZSUMDET_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_NETZSUM_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_NETZSUM_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_OEFFVERKAT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_OEFFVERKAT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_RELMASSTAB_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_RELMASSTAB_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_REALMASSGIS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_REALMASSGIS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_BETSYSRM_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_BETSYSRM_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_RELMASSSUM_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_RELMASSSUM_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADR_REFGRUPPE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADR_REFGRUPPE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_SCHADDET_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_SCHADDET_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_SCHADGRUPPE_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_SCHADGRUPPE_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_SCRIPTLOG_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_SCRIPTLOG_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'SRD_SEKTOR_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SRD_SEKTOR_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_STRAGIS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_STRAGIS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_STRATAB_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_STRATAB_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADP_TESTUSER_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADP_TESTUSER_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'VAT_WBBKAT_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table VAT_WBBKAT_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ZSTGIS_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ZSTGIS_MSG;
    if exists (select * from dbo.sysobjects where id = object_id(N'ADD_ZSTTAB_MSG') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ADD_ZSTTAB_MSG;
    create table SRD_ACHSE_MSG (
        ACH_ID UNIQUEIDENTIFIER not null,
       ACH_CREATEDAT_VL DATETIME null,
       ACH_CREATEDBY_VL NVARCHAR(255) null,
       ACH_UPDATEDAT_VL DATETIME null,
       ACH_UPDATEDBY_VL NVARCHAR(255) null,
       ACH_BSID_VL UNIQUEIDENTIFIER null,
       ACH_VALIDFROM_DT DATETIME null,
       ACH_NAME_VL NVARCHAR(255) null,
       ACH_OPERATION_VL INT null,
       ACH_IMPNR_VL INT null,
       ACH_ACH_MAN_NOR_ID UNIQUEIDENTIFIER null,
       ACH_ACH_EPR_NOR_ID UNIQUEIDENTIFIER null,
       ACH_ACH_ACH_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ACH_ID)
    );
    create table ADD_ACHSIMPLOG_MSG (
        AIL_ID UNIQUEIDENTIFIER not null,
       AIL_CREATEDAT_VL DATETIME null,
       AIL_CREATEDBY_VL NVARCHAR(255) null,
       AIL_UPDATEDAT_VL DATETIME null,
       AIL_UPDATEDBY_VL NVARCHAR(255) null,
       AIL_IMPNR_VL INT null unique,
       AIL_PATH_VL NVARCHAR(255) null,
       AIL_PROGRESS_VL INT null,
       AIL_SENDERTIMES_DT DATETIME null,
       AIL_TIMESTAMP_DT DATETIME null,
       AIL_ACHSINSERTS_VL INT null,
       AIL_ACHSUPDATES_VL INT null,
       AIL_ACHSDELETES_VL INT null,
       AIL_SEGMINSERTS_VL INT null,
       AIL_SEGMUPDATES_VL INT null,
       AIL_SEGMDELETES_VL INT null,
       AIL_SEKTINSERTS_VL INT null,
       AIL_SEKTUPDATES_VL INT null,
       AIL_SEKTDELETES_VL INT null,
       primary key (AIL_ID)
    );
    create table ADD_ACHSLOCK_MSG (
        ALK_ID UNIQUEIDENTIFIER not null,
       ALK_CREATEDAT_VL DATETIME null,
       ALK_CREATEDBY_VL NVARCHAR(255) null,
       ALK_UPDATEDAT_VL DATETIME null,
       ALK_UPDATEDBY_VL NVARCHAR(255) null,
       ALK_ISLOCKED_VL BIT null,
       ALK_LOCKSTART_DT DATETIME null,
       ALK_LOCKEND_VL DATETIME null,
       ALK_LOCKTYPE_VL INT null,
       ALK_ALK_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ALK_ID)
    );
    create table ADD_ACHSREF_MSG (
        ACR_ID UNIQUEIDENTIFIER not null,
       ACR_CREATEDAT_VL DATETIME null,
       ACR_CREATEDBY_VL NVARCHAR(255) null,
       ACR_UPDATEDAT_VL DATETIME null,
       ACR_UPDATEDBY_VL NVARCHAR(255) null,
       ACR_STRASSENNAM_VL NVARCHAR(255) null,
       ACR_VERSION_VL INT null,
       ACR_VONRBBS_VL INT null,
       ACR_NACHRBBS_VL INT null,
       ACR_SHAPE_VL GEOMETRY null,
       ACR_ACR_ACR_NOR_ID UNIQUEIDENTIFIER null,
       ACR_ACR_ACS_NOR_ID UNIQUEIDENTIFIER null,
       ACR_ACR_RFG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ACR_ID)
    );
    create table SRD_ACHSSEG_MSG (
        ACS_ID UNIQUEIDENTIFIER not null,
       ACS_CREATEDAT_VL DATETIME null,
       ACS_CREATEDBY_VL NVARCHAR(255) null,
       ACS_UPDATEDAT_VL DATETIME null,
       ACS_UPDATEDBY_VL NVARCHAR(255) null,
       ACS_BSID_VL UNIQUEIDENTIFIER null,
       ACS_OPERATION_VL INT default -1  null,
       ACS_NAME_VL NVARCHAR(255) null,
       ACS_SEQUENCE_VL INT default -1  null,
       ACS_IMPNR_VL INT default -1  null,
       ACS_SHAPE_VL GEOMETRY null,
       ACS_VERSION_VL INT null,
       ACS_ACHSENID_VL UNIQUEIDENTIFIER null,
       ACS_ISINVERTED_VL BIT null,
       ACS_ACS_ACH_NOR_ID UNIQUEIDENTIFIER null,
       ACS_ACS_EPR_NOR_ID UNIQUEIDENTIFIER null,
       ACS_ACS_MAN_NOR_ID UNIQUEIDENTIFIER null,
       ACS_ACS_ACS_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ACS_ID)
    );
    create table ADD_ACHSUPDCON_MSG (
        AUC_ID UNIQUEIDENTIFIER not null,
       AUC_CREATEDAT_VL DATETIME null,
       AUC_CREATEDBY_VL NVARCHAR(255) null,
       AUC_UPDATEDAT_VL DATETIME null,
       AUC_UPDATEDBY_VL NVARCHAR(255) null,
       AUC_NAME_VL NVARCHAR(255) null,
       AUC_CONFLICTTYP_VL INT null,
       AUC_ITEMTYPE_VL INT null,
       AUC_ITEMID_VL UNIQUEIDENTIFIER null,
       AUC_SEGMENTID_VL UNIQUEIDENTIFIER null,
       AUC_SHAPE_VL GEOMETRY null,
       AUC_AUC_MAN_NOR_ID UNIQUEIDENTIFIER null,
       AUC_AUC_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (AUC_ID)
    );
    create table ADD_ACHSUPDLOG_MSG (
        AUL_ID UNIQUEIDENTIFIER not null,
       AUL_CREATEDAT_VL DATETIME null,
       AUL_CREATEDBY_VL NVARCHAR(255) null,
       AUL_UPDATEDAT_VL DATETIME null,
       AUL_UPDATEDBY_VL NVARCHAR(255) null,
       AUL_IMPNR_VL INT null,
       AUL_STATISTICS_VL NVARCHAR(255) null,
       AUL_TIMESTAMP_DT DATETIME null,
       AUL_ACHSINSERTS_VL INT null,
       AUL_ACHSUPDATES_VL INT null,
       AUL_ACHSDELETES_VL INT null,
       AUL_SEGMINSERTS_VL INT null,
       AUL_SEGMUPDATES_VL INT null,
       AUL_SEGMDELETES_VL INT null,
       AUL_SEKTINSERTS_VL INT null,
       AUL_SEKTUPDATES_VL INT null,
       AUL_SEKTDELETES_VL INT null,
       AUL_UPDATEDREFE_VL INT null,
       AUL_DELETEDREFE_VL INT null,
       AUL_UPDATEDSTRA_VL INT null,
       AUL_DELETEDSTRA_VL INT null,
       AUL_UPDATEDZUST_VL INT null,
       AUL_DELETEDZUST_VL INT null,
       AUL_UPDATEDKOOR_VL INT null,
       AUL_DELETEDKOOR_VL INT null,
       AUL_UPDATEDMASS_VL INT null,
       AUL_DELETEDMASS_VL INT null,
       AUL_AUL_MAN_NOR_ID UNIQUEIDENTIFIER null,
       AUL_AUL_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (AUL_ID),
      unique (AUL_IMPNR_VL, AUL_AUL_MAN_NOR_ID)
    );
    create table VAT_BELKAT_MSG (
        BLK_ID UNIQUEIDENTIFIER not null,
       BLK_CREATEDAT_VL DATETIME null,
       BLK_CREATEDBY_VL NVARCHAR(255) null,
       BLK_UPDATEDAT_VL DATETIME null,
       BLK_UPDATEDBY_VL NVARCHAR(255) null,
       BLK_TYP_VL NVARCHAR(255) null,
       BLK_REIHENFOLGE_VL INT null,
       BLK_DEFBRFB_VL DECIMAL(19,5) null,
       BLK_DEFBRTRR_VL DECIMAL(19,5) null,
       BLK_DEFBRTRL_VL DECIMAL(19,5) null,
       BLK_FARBCODE_VL NVARCHAR(255) null,
       primary key (BLK_ID)
    );
    create table ADD_ALBELI_MSG (
        BET_BET_BLK_NOR_ID UNIQUEIDENTIFIER not null,
       BET_BTY_VL INT null
    );
    create table ADD_BEDATADET_MSG (
        BDD_ID UNIQUEIDENTIFIER not null,
       BDD_CREATEDAT_VL DATETIME null,
       BDD_CREATEDBY_VL NVARCHAR(255) null,
       BDD_UPDATEDAT_VL DATETIME null,
       BDD_UPDATEDBY_VL NVARCHAR(255) null,
       BDD_FBFLANTEIL_NR DECIMAL(19,5) null,
       BDD_ZUSTANDSIND_VL DECIMAL(19,5) null,
       BDD_REMAPWBW_NR DECIMAL(19,5) null,
       BDD_BDD_BLK_NOR_ID UNIQUEIDENTIFIER null,
       BDD_BDD_BDT_NOR_ID UNIQUEIDENTIFIER null,
       primary key (BDD_ID)
    );
    create table ADD_BENCHDATA_MSG (
        BDT_ID UNIQUEIDENTIFIER not null,
       BDT_CREATEDAT_VL DATETIME null,
       BDT_CREATEDBY_VL NVARCHAR(255) null,
       BDT_UPDATEDAT_VL DATETIME null,
       BDT_UPDATEDBY_VL NVARCHAR(255) null,
       BDT_FAFLPEIN_NR DECIMAL(19,5) null,
       BDT_FAFLPSIE_NR DECIMAL(19,5) null,
       BDT_GESTRPEIN_NR DECIMAL(19,5) null,
       BDT_GESTRPSIE_NR DECIMAL(19,5) null,
       BDT_GESTFLPEIN_NR DECIMAL(19,5) null,
       BDT_GESTFLPSIE_NR DECIMAL(19,5) null,
       BDT_WVLPEIN_NR DECIMAL(19,5) null,
       BDT_WVLPFB_NR DECIMAL(19,5) null,
       BDT_WBWPEIN_NR DECIMAL(19,5) null,
       BDT_WBWPFB_NR DECIMAL(19,5) null,
       BDT_ZUSTANDNETZ_VL DECIMAL(19,5) null,
       BDT_MITALTZUSNE_VL DATETIME null,
       BDT_REMAPEIN_NR DECIMAL(19,5) null,
       BDT_REMAPFB_NR DECIMAL(19,5) null,
       BDT_REMAWV_NR DECIMAL(19,5) null,
       BDT_REMAWBW_NR DECIMAL(19,5) null,
       BDT_CALCAT_DT DATETIME null,
       BDT_NEEDSRECALC_VL BIT null,
       BDT_BDT_EPR_NOR_ID UNIQUEIDENTIFIER null,
       BDT_BDT_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (BDT_ID)
    );
    create table ADD_BENCHGRCFG_MSG (
        BGC_ID UNIQUEIDENTIFIER not null,
       BGC_CREATEDAT_VL DATETIME null,
       BGC_CREATEDBY_VL NVARCHAR(255) null,
       BGC_UPDATEDAT_VL DATETIME null,
       BGC_UPDATEDBY_VL NVARCHAR(255) null,
       BGC_EIGENSCHAFT_VL NVARCHAR(255) null,
       BGC_GRENZWERT_NR DECIMAL(19,5) null,
       primary key (BGC_ID)
    );
    create table ADD_CHECKOUT_MSG (
        COG_ID UNIQUEIDENTIFIER not null,
       COG_CREATEDAT_VL DATETIME null,
       COG_CREATEDBY_VL NVARCHAR(255) null,
       COG_UPDATEDAT_VL DATETIME null,
       COG_UPDATEDBY_VL NVARCHAR(255) null,
       COG_CHECKINDATU_VL DATETIME null,
       COG_CHECKOUTDAT_DT DATETIME null,
       COG_INSPECTIONB_VL NVARCHAR(255) null,
       COG_DESCRIPTION_VL NVARCHAR(255) null,
       COG_COMMENTS_VL NVARCHAR(255) null,
       COG_COG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       COG_COG_IRG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (COG_ID)
    );
    create table ADD_EREIGNISLOG_MSG (
        ERL_ID UNIQUEIDENTIFIER not null,
       ERL_CREATEDAT_VL DATETIME null,
       ERL_CREATEDBY_VL NVARCHAR(255) null,
       ERL_UPDATEDAT_VL DATETIME null,
       ERL_UPDATEDBY_VL NVARCHAR(255) null,
       ERL_BENUTZER_VL NVARCHAR(255) null,
       ERL_ZEIT_DT DATETIME null,
       ERL_EREIGNISTYP_VL INT null,
       ERL_EREIGNISDAT_VL NVARCHAR(MAX) null,
       ERL_MANDANTNAME_VL NVARCHAR(255) null,
       primary key (ERL_ID)
    );
    create table ADD_ERFPERIODE_MSG (
        EPR_ID UNIQUEIDENTIFIER not null,
       EPR_CREATEDAT_VL DATETIME null,
       EPR_CREATEDBY_VL NVARCHAR(255) null,
       EPR_UPDATEDAT_VL DATETIME null,
       EPR_UPDATEDBY_VL NVARCHAR(255) null,
       EPR_NAME_VL NVARCHAR(255) null,
       EPR_NETZMODUS_VL INT null,
       EPR_ISTABGESCHL_VL BIT null,
       EPR_ERFJAHR_DT DATETIME null,
       EPR_EPR_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (EPR_ID)
    );
    create table VAT_GEMKAT_MSG (
        GEK_ID UNIQUEIDENTIFIER not null,
       GEK_CREATEDAT_VL DATETIME null,
       GEK_CREATEDBY_VL NVARCHAR(255) null,
       GEK_UPDATEDAT_VL DATETIME null,
       GEK_UPDATEDBY_VL NVARCHAR(255) null,
       GEK_TYP_VL NVARCHAR(255) null,
       primary key (GEK_ID)
    );
    create table VAT_GMASSVOR_MSG (
        GMK_ID UNIQUEIDENTIFIER not null,
       GMK_CREATEDAT_VL DATETIME null,
       GMK_CREATEDBY_VL NVARCHAR(255) null,
       GMK_UPDATEDAT_VL DATETIME null,
       GMK_UPDATEDBY_VL NVARCHAR(255) null,
       GMK_DEFKOSTEN_NR DECIMAL(19,5) null,
       GMK_GMK_BLK_NOR_ID UNIQUEIDENTIFIER null,
       GMK_GMK_MTK_NOR_ID UNIQUEIDENTIFIER null,
       primary key (GMK_ID)
    );
    create table VAT_GWBBKAT_MSG (
        GWK_ID UNIQUEIDENTIFIER not null,
       GWK_CREATEDAT_VL DATETIME null,
       GWK_CREATEDBY_VL NVARCHAR(255) null,
       GWK_UPDATEDAT_VL DATETIME null,
       GWK_UPDATEDBY_VL NVARCHAR(255) null,
       GWK_FLAEGESFB_NR DECIMAL(19,5) null,
       GWK_FLAECHEFB_NR DECIMAL(19,5) null,
       GWK_FLAECHETR_NR DECIMAL(19,5) null,
       GWK_ALTERUNGI_NR DECIMAL(19,5) null,
       GWK_ALTERUNGII_NR DECIMAL(19,5) null,
       GWK_GWK_BLK_NOR_ID UNIQUEIDENTIFIER null,
       primary key (GWK_ID)
    );
    create table ADD_INSPEKROUTE_MSG (
        IRG_ID UNIQUEIDENTIFIER not null,
       IRG_CREATEDAT_VL DATETIME null,
       IRG_CREATEDBY_VL NVARCHAR(255) null,
       IRG_UPDATEDAT_VL DATETIME null,
       IRG_UPDATEDBY_VL NVARCHAR(255) null,
       IRG_SHAPE_VL GEOMETRY null,
       IRG_BEZEICHNUNG_VL NVARCHAR(255) null,
       IRG_BEMERKUNGEN_VL NVARCHAR(MAX) null,
       IRG_BESCHREIBUN_VL NVARCHAR(MAX) null,
       IRG_ININSPBEI_VL NVARCHAR(255) null,
       IRG_ININSPBIS_VL DATETIME null,
       IRG_LEGENDNUMBE_VL INT null,
       IRG_IRG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       IRG_IRG_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (IRG_ID)
    );
    create table ADD_INSPEKSTATU_MSG (
        IRV_ID UNIQUEIDENTIFIER not null,
       IRV_CREATEDAT_VL DATETIME null,
       IRV_CREATEDBY_VL NVARCHAR(255) null,
       IRV_UPDATEDAT_VL DATETIME null,
       IRV_UPDATEDBY_VL NVARCHAR(255) null,
       IRV_DATUM_DT DATETIME null,
       IRV_STATUS_VL INT null,
       IRV_IRV_IRG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (IRV_ID)
    );
    create table ADD_INSPEKSTRA_MSG (
        IRS_ID UNIQUEIDENTIFIER not null,
       IRS_CREATEDAT_VL DATETIME null,
       IRS_CREATEDBY_VL NVARCHAR(255) null,
       IRS_UPDATEDAT_VL DATETIME null,
       IRS_UPDATEDBY_VL NVARCHAR(255) null,
       IRS_REIHENFOLGE_VL INT null,
       IRS_IRS_STG_NOR_ID UNIQUEIDENTIFIER null,
       IRS_IRS_IRG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (IRS_ID)
    );
    create table ADD_KENGRFJDET_MSG (
        KFD_ID UNIQUEIDENTIFIER not null,
       KFD_CREATEDAT_VL DATETIME null,
       KFD_CREATEDBY_VL NVARCHAR(255) null,
       KFD_UPDATEDAT_VL DATETIME null,
       KFD_UPDATEDBY_VL NVARCHAR(255) null,
       KFD_MITZST_VL DECIMAL(19,5) null,
       KFD_FBLAENGE_NR DECIMAL(19,5) null,
       KFD_FBFLAECHE_VL INT null,
       KFD_KFD_BLK_NOR_ID UNIQUEIDENTIFIER null,
       KFD_KFD_KFJ_NOR_ID UNIQUEIDENTIFIER null,
       primary key (KFD_ID)
    );
    create table ADD_KENGRFJ_MSG (
        KFJ_ID UNIQUEIDENTIFIER not null,
       KFJ_CREATEDAT_VL DATETIME null,
       KFJ_CREATEDBY_VL NVARCHAR(255) null,
       KFJ_UPDATEDAT_VL DATETIME null,
       KFJ_UPDATEDBY_VL NVARCHAR(255) null,
       KFJ_JAHR_VL INT null,
       KFJ_KOSTENFUERW_NR DECIMAL(19,5) null,
       KFJ_KFJ_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (KFJ_ID)
    );
    create table ADD_KOORMASSGIS_MSG (
        KMG_ID UNIQUEIDENTIFIER not null,
       KMG_CREATEDAT_VL DATETIME null,
       KMG_CREATEDBY_VL NVARCHAR(255) null,
       KMG_UPDATEDAT_VL DATETIME null,
       KMG_UPDATEDBY_VL NVARCHAR(255) null,
       KMG_PROJEKTNAME_VL NVARCHAR(255) null,
       KMG_BEZVON_VL NVARCHAR(255) null,
       KMG_BEZBIS_VL NVARCHAR(255) null,
       KMG_LAENGE_NR DECIMAL(19,5) null,
       KMG_BREITEFB_NR DECIMAL(19,5) null,
       KMG_BREITETRL_VL DECIMAL(19,5) null,
       KMG_BREITETRR_VL DECIMAL(19,5) null,
       KMG_KOSTGESAMT_VL DECIMAL(19,5) null,
       KMG_KOSTENFB_VL DECIMAL(19,5) null,
       KMG_KOSTENTRL_VL DECIMAL(19,5) null,
       KMG_KOSTENTRR_VL DECIMAL(19,5) null,
       KMG_BESCHREIBUN_VL NVARCHAR(MAX) null,
       KMG_AUSFANF_VL DATETIME null,
       KMG_AUSFENDE_VL DATETIME null,
       KMG_LEITENDEORG_VL NVARCHAR(255) null,
       KMG_STATUS_VL INT null,
       KMG_SHAPE_VL GEOMETRY null,
       KMG_KMG_MTK_NOR_ID UNIQUEIDENTIFIER null,
       KMG_KMG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       KMG_KMG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (KMG_ID)
    );
    create table ADD_BETSYS_MSG (
        BST_BST_KMG_NOR_ID UNIQUEIDENTIFIER not null,
       BST_TST_VL INT null
    );
    create table SRD_KOPACHSE_MSG (
        KAC_ID UNIQUEIDENTIFIER not null,
       KAC_CREATEDAT_VL DATETIME null,
       KAC_CREATEDBY_VL NVARCHAR(255) null,
       KAC_UPDATEDAT_VL DATETIME null,
       KAC_UPDATEDBY_VL NVARCHAR(255) null,
       KAC_IMPNR_VL INT null,
       KAC_NAME_VL NVARCHAR(255) null,
       KAC_OPERATION_VL INT null,
       KAC_OWNER_VL NVARCHAR(255) null,
       KAC_VALIDFROM_DT DATETIME null,
       primary key (KAC_ID)
    );
    create table SRD_KOPACHSSEG_MSG (
        KSG_ID UNIQUEIDENTIFIER not null,
       KSG_CREATEDAT_VL DATETIME null,
       KSG_CREATEDBY_VL NVARCHAR(255) null,
       KSG_UPDATEDAT_VL DATETIME null,
       KSG_UPDATEDBY_VL NVARCHAR(255) null,
       KSG_IMPNR_VL INT null,
       KSG_NAME_VL NVARCHAR(255) null,
       KSG_OPERATION_VL INT null,
       KSG_SEQUENCE_VL INT null,
       KSG_SHAPE_VL GEOMETRY null,
       KSG_KSG_KAC_NOR_ID UNIQUEIDENTIFIER null,
       primary key (KSG_ID)
    );
    create table SRD_KOPSEKTOR_MSG (
        KSK_ID UNIQUEIDENTIFIER not null,
       KSK_CREATEDAT_VL DATETIME null,
       KSK_CREATEDBY_VL NVARCHAR(255) null,
       KSK_UPDATEDAT_VL DATETIME null,
       KSK_UPDATEDBY_VL NVARCHAR(255) null,
       KSK_IMPNR_VL INT null,
       KSK_NAME_VL NVARCHAR(255) null,
       KSK_OPERATION_VL INT null,
       KSK_SEQUENCE_VL FLOAT(53) null,
       KSK_MARKERGEOM_VL GEOMETRY null,
       KSK_KSK_KSG_NOR_ID UNIQUEIDENTIFIER null,
       KSK_KM_VL FLOAT(53) null,
       KSK_SEKTORLEN_VL FLOAT(53) null,
       primary key (KSK_ID)
    );
    create table ADD_MANDANTDET_MSG (
        MAD_ID UNIQUEIDENTIFIER not null,
       MAD_CREATEDAT_VL DATETIME null,
       MAD_CREATEDBY_VL NVARCHAR(255) null,
       MAD_UPDATEDAT_VL DATETIME null,
       MAD_UPDATEDBY_VL NVARCHAR(255) null,
       MAD_DIFFERENZHO_VL INT null,
       MAD_EINWOHNER_VL INT null,
       MAD_GEMEINDEFLA_VL INT null,
       MAD_MITTLEREHOE_VL INT null,
       MAD_SIEDLUNGSFL_VL INT null,
       MAD_STEUERERTRA_VL INT null,
       MAD_NETZLAENGE_NR DECIMAL(19,5) null,
       MAD_ISCOMPLETED_VL BIT null,
       MAD_ISACHSENEDI_VL BIT null,
       MAD_MAD_GEK_NOR_ID UNIQUEIDENTIFIER null,
       MAD_MAD_OVG_NOR_ID UNIQUEIDENTIFIER null,
       MAD_MAD_MAN_NOR_ID UNIQUEIDENTIFIER null,
       MAD_MAD_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (MAD_ID)
    );
    create table ADD_MANDANTLOGO_MSG (
        MAL_ID UNIQUEIDENTIFIER not null,
       MAL_CREATEDAT_VL DATETIME null,
       MAL_CREATEDBY_VL NVARCHAR(255) null,
       MAL_UPDATEDAT_VL DATETIME null,
       MAL_UPDATEDBY_VL NVARCHAR(255) null,
       MAL_LOGO_VL VARBINARY(MAX) null,
       MAL_HEIGHT_VL INT null,
       MAL_WIDTH_VL INT null,
       MAL_MAL_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (MAL_ID)
    );
    create table ADD_MANDANT_MSG (
        MAN_ID UNIQUEIDENTIFIER not null,
       MAN_CREATEDAT_VL DATETIME null,
       MAN_CREATEDBY_VL NVARCHAR(255) null,
       MAN_UPDATEDAT_VL DATETIME null,
       MAN_UPDATEDBY_VL NVARCHAR(255) null,
       MAN_MANDANTNAME_VL NVARCHAR(255) null,
       MAN_MANDANTBEZE_VL NVARCHAR(255) null,
       MAN_OWNERID_VL NVARCHAR(255) null,
       primary key (MAN_ID)
    );
    create table VAT_MASSTYP_MSG (
        MTK_ID UNIQUEIDENTIFIER not null,
       MTK_CREATEDAT_VL DATETIME null,
       MTK_CREATEDBY_VL NVARCHAR(255) null,
       MTK_UPDATEDAT_VL DATETIME null,
       MTK_UPDATEDBY_VL NVARCHAR(255) null,
       MTK_TYP_VL NVARCHAR(255) null,
       MTK_KATALOGTYP_VL INT null,
       MTK_LEGENDNUMBE_VL INT null,
       primary key (MTK_ID)
    );
    create table VAT_MASSVORSCH_MSG (
        MVK_ID UNIQUEIDENTIFIER not null,
       MVK_CREATEDAT_VL DATETIME null,
       MVK_CREATEDBY_VL NVARCHAR(255) null,
       MVK_UPDATEDAT_VL DATETIME null,
       MVK_UPDATEDBY_VL NVARCHAR(255) null,
       MVK_DEFKOSTEN_NR DECIMAL(19,5) null,
       MVK_ISCUSTOMIZE_VL BIT null,
       MVK_MVK_MTK_NOR_ID UNIQUEIDENTIFIER null,
       MVK_MVK_BLK_NOR_ID UNIQUEIDENTIFIER null,
       MVK_MVK_MAN_NOR_ID UNIQUEIDENTIFIER null,
       MVK_MVK_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (MVK_ID)
    );
    create table ADD_MASSTEILGIS_MSG (
        MTG_ID UNIQUEIDENTIFIER not null,
       MTG_CREATEDAT_VL DATETIME null,
       MTG_CREATEDBY_VL NVARCHAR(255) null,
       MTG_UPDATEDAT_VL DATETIME null,
       MTG_UPDATEDBY_VL NVARCHAR(255) null,
       MTG_PROJEKTNAME_VL NVARCHAR(255) null,
       MTG_BEZVON_VL NVARCHAR(255) null,
       MTG_BEZBIS_VL NVARCHAR(255) null,
       MTG_BESCHREIBUN_VL NVARCHAR(MAX) null,
       MTG_ZUSTAENDORG_VL NVARCHAR(255) null,
       MTG_TEILSYSTEM_VL INT null,
       MTG_DRINGLICHKE_VL INT null,
       MTG_STATUS_VL INT null,
       MTG_KOSTEN_VL DECIMAL(19,5) null,
       MTG_LAENGE_NR DECIMAL(19,5) null,
       MTG_SHAPE_VL GEOMETRY null,
       MTG_MTG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       MTG_MTG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (MTG_ID)
    );
    create table ADD_NETZSUMDET_MSG (
        NSD_ID UNIQUEIDENTIFIER not null,
       NSD_CREATEDAT_VL DATETIME null,
       NSD_CREATEDBY_VL NVARCHAR(255) null,
       NSD_UPDATEDAT_VL DATETIME null,
       NSD_UPDATEDBY_VL NVARCHAR(255) null,
       NSD_MITZST_VL DECIMAL(19,5) null,
       NSD_FBLAENGE_NR DECIMAL(19,5) null,
       NSD_FBFLAECHE_VL INT null,
       NSD_NSD_BLK_NOR_ID UNIQUEIDENTIFIER null,
       NSD_NSD_NSU_NOR_ID UNIQUEIDENTIFIER null,
       primary key (NSD_ID)
    );
    create table ADD_NETZSUM_MSG (
        NSU_ID UNIQUEIDENTIFIER not null,
       NSU_CREATEDAT_VL DATETIME null,
       NSU_CREATEDBY_VL NVARCHAR(255) null,
       NSU_UPDATEDAT_VL DATETIME null,
       NSU_UPDATEDBY_VL NVARCHAR(255) null,
       NSU_MITERHJAHR_VL DATETIME null,
       NSU_NSU_MAN_NOR_ID UNIQUEIDENTIFIER null,
       NSU_NSU_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (NSU_ID)
    );
    create table VAT_OEFFVERKAT_MSG (
        OVG_ID UNIQUEIDENTIFIER not null,
       OVG_CREATEDAT_VL DATETIME null,
       OVG_CREATEDBY_VL NVARCHAR(255) null,
       OVG_UPDATEDAT_VL DATETIME null,
       OVG_UPDATEDBY_VL NVARCHAR(255) null,
       OVG_TYP_VL NVARCHAR(255) null,
       primary key (OVG_ID)
    );
    create table ADD_RELMASSTAB_MSG (
        RMT_ID UNIQUEIDENTIFIER not null,
       RMT_CREATEDAT_VL DATETIME null,
       RMT_CREATEDBY_VL NVARCHAR(255) null,
       RMT_UPDATEDAT_VL DATETIME null,
       RMT_UPDATEDBY_VL NVARCHAR(255) null,
       RMT_PROJEKTNAME_VL NVARCHAR(255) null,
       RMT_BEZVON_VL NVARCHAR(255) null,
       RMT_BEZBIS_VL NVARCHAR(255) null,
       RMT_LAENGE_NR DECIMAL(19,5) null,
       RMT_BREITEFB_NR DECIMAL(19,5) null,
       RMT_BREITETRL_VL DECIMAL(19,5) null,
       RMT_BREITETRR_VL DECIMAL(19,5) null,
       RMT_BESCHR_VL NVARCHAR(MAX) null,
       RMT_FBKOSTEN_VL DECIMAL(19,5) null,
       RMT_TRRKOSTEN_VL DECIMAL(19,5) null,
       RMT_TRLKOSTEN_VL DECIMAL(19,5) null,
       RMT_EIGENTUEMER_VL INT null,
       RMT_RMT_MTK_NOR_ID UNIQUEIDENTIFIER null,
       RMT_RMT_MTK_TR_ID UNIQUEIDENTIFIER null,
       RMT_RMT_BLK_NOR_ID UNIQUEIDENTIFIER null,
       RMT_RMT_EPR_NOR_ID UNIQUEIDENTIFIER null,
       RMT_RMT_MAN_NOR_ID UNIQUEIDENTIFIER null,
       primary key (RMT_ID)
    );
    create table ADD_REALMASSGIS_MSG (
        RMG_ID UNIQUEIDENTIFIER not null,
       RMG_CREATEDAT_VL DATETIME null,
       RMG_CREATEDBY_VL NVARCHAR(255) null,
       RMG_UPDATEDAT_VL DATETIME null,
       RMG_UPDATEDBY_VL NVARCHAR(255) null,
       RMG_PROJEKTNAME_VL NVARCHAR(255) null,
       RMG_BEZVON_VL NVARCHAR(255) null,
       RMG_BEZBIS_VL NVARCHAR(255) null,
       RMG_LAENGE_NR DECIMAL(19,5) null,
       RMG_BREITEFB_NR DECIMAL(19,5) null,
       RMG_BREITETRL_VL DECIMAL(19,5) null,
       RMG_BREITETRR_VL DECIMAL(19,5) null,
       RMG_KOSTGESAMT_VL DECIMAL(19,5) null,
       RMG_KOSTENFB_VL DECIMAL(19,5) null,
       RMG_KOSTENTRL_VL DECIMAL(19,5) null,
       RMG_KOSTENTRR_VL DECIMAL(19,5) null,
       RMG_EIGENTUEMER_VL INT null,
       RMG_BESCHREIBUN_VL NVARCHAR(MAX) null,
       RMG_LEITENDEORG_VL NVARCHAR(255) null,
       RMG_SHAPE_VL GEOMETRY null,
       RMG_RMG_MTK_NOR_ID UNIQUEIDENTIFIER null,
       RMG_RMG_MTK_TR_ID UNIQUEIDENTIFIER null,
       RMG_RMG_BLK_NOR_ID UNIQUEIDENTIFIER null,
       RMG_RMG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       RMG_RMG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       RMG_RMG_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (RMG_ID)
    );
    create table ADD_BETSYSRM_MSG (
        BST_BST_RMG_NOR_ID UNIQUEIDENTIFIER not null,
       BST_TST_VL INT null
    );
    create table ADD_RELMASSSUM_MSG (
        RMS_ID UNIQUEIDENTIFIER not null,
       RMS_CREATEDAT_VL DATETIME null,
       RMS_CREATEDBY_VL NVARCHAR(255) null,
       RMS_UPDATEDAT_VL DATETIME null,
       RMS_UPDATEDBY_VL NVARCHAR(255) null,
       RMS_BESCHR_VL NVARCHAR(MAX) null,
       RMS_FBKOSTEN_VL INT null,
       RMS_PROJEKTNAME_VL NVARCHAR(255) null,
       RMS_FBFLAECHE_VL INT null,
       RMS_EIGENTUEMER_VL INT null,
       RMS_RMS_EPR_NOR_ID UNIQUEIDENTIFIER null,
       RMS_RMS_MAN_NOR_ID UNIQUEIDENTIFIER null,
       RMS_RMS_BLK_NOR_ID UNIQUEIDENTIFIER null,
       primary key (RMS_ID)
    );
    create table ADR_REFGRUPPE_MSG (
        RFG_ID UNIQUEIDENTIFIER not null,
       RFG_CREATEDAT_VL DATETIME null,
       RFG_CREATEDBY_VL NVARCHAR(255) null,
       RFG_UPDATEDAT_VL DATETIME null,
       RFG_UPDATEDBY_VL NVARCHAR(255) null,
       RFG_RFG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (RFG_ID)
    );
    create table ADD_SCHADDET_MSG (
        SCD_ID UNIQUEIDENTIFIER not null,
       SCD_CREATEDAT_VL DATETIME null,
       SCD_CREATEDBY_VL NVARCHAR(255) null,
       SCD_UPDATEDAT_VL DATETIME null,
       SCD_UPDATEDBY_VL NVARCHAR(255) null,
       SCD_DETAILTYP_VL INT null,
       SCD_SCHWERETYP_VL INT null,
       SCD_AUSMASSTYP_VL INT null,
       SCD_SCD_ZST_ID UNIQUEIDENTIFIER null,
       SCD_SCD_ZSG_ID UNIQUEIDENTIFIER null,
       primary key (SCD_ID)
    );
    create table ADD_SCHADGRUPPE_MSG (
        SCG_ID UNIQUEIDENTIFIER not null,
       SCG_CREATEDAT_VL DATETIME null,
       SCG_CREATEDBY_VL NVARCHAR(255) null,
       SCG_UPDATEDAT_VL DATETIME null,
       SCG_UPDATEDBY_VL NVARCHAR(255) null,
       SCG_GRUPPETYP_VL INT null,
       SCG_SCHWERETYP_VL INT null,
       SCG_AUSMASSTYP_VL INT null,
       SCG_SCG_ZST_ID UNIQUEIDENTIFIER null,
       SCG_SCG_ZSG_ID UNIQUEIDENTIFIER null,
       primary key (SCG_ID)
    );
    create table ADD_SCRIPTLOG_MSG (
        SCL_ID UNIQUEIDENTIFIER not null,
       SCL_CREATEDAT_VL DATETIME null,
       SCL_CREATEDBY_VL NVARCHAR(255) null,
       SCL_UPDATEDAT_VL DATETIME null,
       SCL_UPDATEDBY_VL NVARCHAR(255) null,
       SCL_SCRIPTNAME_VL NVARCHAR(255) null,
       SCL_VERSION_VL INT null,
       SCL_EXECUTIONDA_DT DATETIME null,
       primary key (SCL_ID)
    );
    create table SRD_SEKTOR_MSG (
        SEK_ID UNIQUEIDENTIFIER not null,
       SEK_CREATEDAT_VL DATETIME null,
       SEK_CREATEDBY_VL NVARCHAR(255) null,
       SEK_UPDATEDAT_VL DATETIME null,
       SEK_UPDATEDBY_VL NVARCHAR(255) null,
       SEK_BSID_VL UNIQUEIDENTIFIER null,
       SEK_KM_VL FLOAT(53) null,
       SEK_SEKTORLEN_VL FLOAT(53) null,
       SEK_NAME_VL NVARCHAR(255) null,
       SEK_SEQUENCE_VL FLOAT(53) null,
       SEK_MARKERGEOM_VL GEOMETRY null,
       SEK_OPERATION_VL INT null,
       SEK_IMPNR_VL INT null,
       SEK_SEK_ACS_NOR_ID UNIQUEIDENTIFIER null,
       SEK_SEK_SEK_NOR_ID UNIQUEIDENTIFIER null,
       primary key (SEK_ID)
    );
    create table ADD_STRAGIS_MSG (
        STG_ID UNIQUEIDENTIFIER not null,
       STG_CREATEDAT_VL DATETIME null,
       STG_CREATEDBY_VL NVARCHAR(255) null,
       STG_UPDATEDAT_VL DATETIME null,
       STG_UPDATEDBY_VL NVARCHAR(255) null,
       STG_STRASSENNAM_VL NVARCHAR(255) null,
       STG_BEZVON_VL NVARCHAR(255) null,
       STG_BEZBIS_VL NVARCHAR(255) null,
       STG_BELAG_VL INT null,
       STG_LAENGE_NR DECIMAL(19,5) null,
       STG_BREITEFB_NR DECIMAL(19,5) null,
       STG_TROTTOIR_VL INT null,
       STG_BREITETRL_VL DECIMAL(19,5) null,
       STG_BREITETRR_VL DECIMAL(19,5) null,
       STG_EIGENTUEMER_VL INT null,
       STG_ORTSBEZ_VL NVARCHAR(255) null,
       STG_ABSCHNITTSNR_VL INT null,
       STG_SHAPE_VL GEOMETRY null,
       STG_ISLOCKED_VL BIT null,
       STG_STG_BLK_NOR_ID UNIQUEIDENTIFIER null,
       STG_STG_MAN_NOR_ID UNIQUEIDENTIFIER null,
       STG_STG_EPR_NOR_ID UNIQUEIDENTIFIER null,
       STG_STG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       STG_STG_STG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (STG_ID)
    );
    create table ADD_STRATAB_MSG (
        STT_ID UNIQUEIDENTIFIER not null,
       STT_CREATEDAT_VL DATETIME null,
       STT_CREATEDBY_VL NVARCHAR(255) null,
       STT_UPDATEDAT_VL DATETIME null,
       STT_UPDATEDBY_VL NVARCHAR(255) null,
       STT_STRASSENNAM_VL NVARCHAR(255) null,
       STT_BEZVON_VL NVARCHAR(255) null,
       STT_BEZBIS_VL NVARCHAR(255) null,
       STT_EXTERNALID_VL NVARCHAR(255) null,
       STT_BELAG_VL INT null,
       STT_LAENGE_NR DECIMAL(19,5) null,
       STT_BREITEFB_NR DECIMAL(19,5) null,
       STT_TROTTOIR_VL INT null,
       STT_BREITETRL_VL DECIMAL(19,5) null,
       STT_BREITETRR_VL DECIMAL(19,5) null,
       STT_EIGENTUEMER_VL INT null,
       STT_ORTSBEZ_VL NVARCHAR(255) null,
       STT_ABSCHNITTSNR_VL INT null,
       STT_STT_BLK_NOR_ID UNIQUEIDENTIFIER null,
       STT_STT_MAN_NOR_ID UNIQUEIDENTIFIER null,
       STT_STT_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (STT_ID)
    );
    create table ADP_TESTUSER_MSG (
        TUI_ID UNIQUEIDENTIFIER not null,
       TUI_CREATEDAT_VL DATETIME null,
       TUI_CREATEDBY_VL NVARCHAR(255) null,
       TUI_UPDATEDAT_VL DATETIME null,
       TUI_UPDATEDBY_VL NVARCHAR(255) null,
       TUI_USERNAME_VL NVARCHAR(255) null,
       TUI_MANDATOR_VL NVARCHAR(255) null,
       TUI_ROLLE_VL INT null,
       primary key (TUI_ID)
    );
    create table VAT_WBBKAT_MSG (
        WBK_ID UNIQUEIDENTIFIER not null,
       WBK_CREATEDAT_VL DATETIME null,
       WBK_CREATEDBY_VL NVARCHAR(255) null,
       WBK_UPDATEDAT_VL DATETIME null,
       WBK_UPDATEDBY_VL NVARCHAR(255) null,
       WBK_FLAEGESFB_NR DECIMAL(19,5) null,
       WBK_FLAECHEFB_NR DECIMAL(19,5) null,
       WBK_FLAECHETR_NR DECIMAL(19,5) null,
       WBK_ALTERUNGI_NR DECIMAL(19,5) null,
       WBK_ALTERUNGII_NR DECIMAL(19,5) null,
       WBK_ISCUSTOMIZE_VL BIT null,
       WBK_WBK_BLK_NOR_ID UNIQUEIDENTIFIER null,
       WBK_WBK_MAN_NOR_ID UNIQUEIDENTIFIER null,
       WBK_WBK_EPR_NOR_ID UNIQUEIDENTIFIER null,
       primary key (WBK_ID)
    );
    create table ADD_ZSTGIS_MSG (
        ZSG_ID UNIQUEIDENTIFIER not null,
       ZSG_CREATEDAT_VL DATETIME null,
       ZSG_CREATEDBY_VL NVARCHAR(255) null,
       ZSG_UPDATEDAT_VL DATETIME null,
       ZSG_UPDATEDBY_VL NVARCHAR(255) null,
       ZSG_ZUSTANDSIND_NR DECIMAL(19,5) null,
       ZSG_BEZVON_VL NVARCHAR(255) null,
       ZSG_BEZBIS_VL NVARCHAR(255) null,
       ZSG_ERFASSUNGSM_VL INT null,
       ZSG_LAENGE_NR DECIMAL(19,5) null,
       ZSG_ABSCHNITTSN_VL INT null,
       ZSG_AUFNAHMEDAT_DT DATETIME null,
       ZSG_AUFNAHMETEA_VL NVARCHAR(255) null,
       ZSG_WETTER_VL INT null,
       ZSG_BEMERKUNG_VL NVARCHAR(MAX) null,
       ZSG_ZSTINDTRL_VL INT null,
       ZSG_ZSTINDTRR_VL INT null,
       ZSG_FBKOSTEN_VL DECIMAL(19,5) null,
       ZSG_FBDRINGLICH_VL INT null,
       ZSG_TRRKOSTEN_VL DECIMAL(19,5) null,
       ZSG_TRRDRINGLIC_VL INT null,
       ZSG_TRLKOSTEN_VL DECIMAL(19,5) null,
       ZSG_TRLDRINGLIC_VL INT null,
       ZSG_SHAPE_VL GEOMETRY null,
       ZSG_ZSG_MVK_FB_ID UNIQUEIDENTIFIER null,
       ZSG_ZSG_MVK_TRR_ID UNIQUEIDENTIFIER null,
       ZSG_ZSG_MVK_TRL_ID UNIQUEIDENTIFIER null,
       ZSG_ZSG_STG_NOR_ID UNIQUEIDENTIFIER null,
       ZSG_ZSG_RFG_NOR_ID UNIQUEIDENTIFIER null,
       ZSG_ZSG_ZSG_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ZSG_ID)
    );
    create table ADD_ZSTTAB_MSG (
        ZST_ID UNIQUEIDENTIFIER not null,
       ZST_CREATEDAT_VL DATETIME null,
       ZST_CREATEDBY_VL NVARCHAR(255) null,
       ZST_UPDATEDAT_VL DATETIME null,
       ZST_UPDATEDBY_VL NVARCHAR(255) null,
       ZST_ZSTIND_NR DECIMAL(19,5) null,
       ZST_ERFASSUNGSM_VL INT null,
       ZST_BEZVON_VL NVARCHAR(255) null,
       ZST_BEZBIS_VL NVARCHAR(255) null,
       ZST_EXTERNALID_VL NVARCHAR(255) null,
       ZST_LAENGE_NR DECIMAL(19,5) null,
       ZST_ABSCHNITTSN_VL INT null,
       ZST_AUFNDAT_DT DATETIME null,
       ZST_AUFNTEAM_VL NVARCHAR(255) null,
       ZST_WETTER_VL INT null,
       ZST_BEMERKUNG_VL NVARCHAR(MAX) null,
       ZST_ZSTINDTRL_VL INT null,
       ZST_ZSTINDTRR_VL INT null,
       ZST_FBKOSTEN_VL DECIMAL(19,5) null,
       ZST_FBDRINGLICH_VL INT null,
       ZST_TRRKOSTEN_VL DECIMAL(19,5) null,
       ZST_TRRDRINGLIC_VL INT null,
       ZST_TRLKOSTEN_VL DECIMAL(19,5) null,
       ZST_TRLDRINGLIC_VL INT null,
       ZST_ZST_MVK_FB_ID UNIQUEIDENTIFIER null,
       ZST_ZST_MVK_TRR_ID UNIQUEIDENTIFIER null,
       ZST_ZST_MVK_TRL_ID UNIQUEIDENTIFIER null,
       ZST_ZST_STT_NOR_ID UNIQUEIDENTIFIER null,
       primary key (ZST_ID)
    );
    create index IDX_ACHSE_BSID on SRD_ACHSE_MSG (ACH_BSID_VL);
    alter table SRD_ACHSE_MSG 
        add constraint CFK_MAN_ACH_NOR 
        foreign key (ACH_ACH_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table SRD_ACHSE_MSG 
        add constraint CFK_EPR_ACH_NOR 
        foreign key (ACH_ACH_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table SRD_ACHSE_MSG 
        add constraint CFK_ACH_ACH_NOR 
        foreign key (ACH_ACH_ACH_NOR_ID) 
        references SRD_ACHSE_MSG;
    alter table ADD_ACHSLOCK_MSG 
        add constraint CFK_MAN_ALK_NOR 
        foreign key (ALK_ALK_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_ACHSREF_MSG 
        add constraint CFK_ACR_ACR_NOR 
        foreign key (ACR_ACR_ACR_NOR_ID) 
        references ADD_ACHSREF_MSG;
    alter table ADD_ACHSREF_MSG 
        add constraint CFK_ACS_ACR_NOR 
        foreign key (ACR_ACR_ACS_NOR_ID) 
        references SRD_ACHSSEG_MSG;
    alter table ADD_ACHSREF_MSG 
        add constraint CFK_RFG_ACR_NOR 
        foreign key (ACR_ACR_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    create index IDX_SEGMENT_BSID on SRD_ACHSSEG_MSG (ACS_BSID_VL);
    create index IDX_ACHSSEG_ACHSE on SRD_ACHSSEG_MSG (ACS_ACS_ACH_NOR_ID);
    create index IDX_ACHSSEG_ERFPERIOD on SRD_ACHSSEG_MSG (ACS_ACS_EPR_NOR_ID);
    create index IDX_ACHSSEG_MANDANT on SRD_ACHSSEG_MSG (ACS_ACS_MAN_NOR_ID);
    alter table SRD_ACHSSEG_MSG 
        add constraint CFK_ACH_ACS_NOR 
        foreign key (ACS_ACS_ACH_NOR_ID) 
        references SRD_ACHSE_MSG;
    alter table SRD_ACHSSEG_MSG 
        add constraint CFK_EPR_ACS_NOR 
        foreign key (ACS_ACS_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table SRD_ACHSSEG_MSG 
        add constraint CFK_MAN_ACS_NOR 
        foreign key (ACS_ACS_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table SRD_ACHSSEG_MSG 
        add constraint CFK_ACS_ACS_NOR 
        foreign key (ACS_ACS_ACS_NOR_ID) 
        references SRD_ACHSSEG_MSG;
    alter table ADD_ACHSUPDCON_MSG 
        add constraint CFK_MAN_AUC_NOR 
        foreign key (AUC_AUC_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_ACHSUPDCON_MSG 
        add constraint CFK_EPR_AUC_NOR 
        foreign key (AUC_AUC_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_ACHSUPDLOG_MSG 
        add constraint CFK_MAN_AUL_NOR 
        foreign key (AUL_AUL_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_ACHSUPDLOG_MSG 
        add constraint CFK_EPR_AUL_NOR 
        foreign key (AUL_AUL_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_ALBELI_MSG 
        add constraint FK3933E69CC3E55D6A 
        foreign key (BET_BET_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_BEDATADET_MSG 
        add constraint CFK_BLK_BDD_NOR 
        foreign key (BDD_BDD_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_BEDATADET_MSG 
        add constraint CFK_BDT_BDD_NOR 
        foreign key (BDD_BDD_BDT_NOR_ID) 
        references ADD_BENCHDATA_MSG;
    alter table ADD_BENCHDATA_MSG 
        add constraint CFK_EPR_BDT_NOR 
        foreign key (BDT_BDT_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_BENCHDATA_MSG 
        add constraint CFK_MAN_BDT_NOR 
        foreign key (BDT_BDT_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_CHECKOUT_MSG 
        add constraint CFK_MAN_COG_NOR 
        foreign key (COG_COG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_CHECKOUT_MSG 
        add constraint CFK_IRG_COG_NOR 
        foreign key (COG_COG_IRG_NOR_ID) 
        references ADD_INSPEKROUTE_MSG;
    alter table ADD_ERFPERIODE_MSG 
        add constraint CFK_MAN_EPR_NOR 
        foreign key (EPR_EPR_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table VAT_GMASSVOR_MSG 
        add constraint CFK_BLK_GMK_NOR 
        foreign key (GMK_GMK_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table VAT_GMASSVOR_MSG 
        add constraint CFK_MTK_GMK_NOR 
        foreign key (GMK_GMK_MTK_NOR_ID) 
        references VAT_MASSTYP_MSG;
    alter table VAT_GWBBKAT_MSG 
        add constraint CFK_BLK_GWK_NOR 
        foreign key (GWK_GWK_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_INSPEKROUTE_MSG 
        add constraint CFK_MAN_IRG_NOR 
        foreign key (IRG_IRG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_INSPEKROUTE_MSG 
        add constraint CFK_EPR_IRG_NOR 
        foreign key (IRG_IRG_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_INSPEKSTATU_MSG 
        add constraint CFK_IRG_IRV_NOR 
        foreign key (IRV_IRV_IRG_NOR_ID) 
        references ADD_INSPEKROUTE_MSG;
    alter table ADD_INSPEKSTRA_MSG 
        add constraint CFK_STG_IRS_NOR 
        foreign key (IRS_IRS_STG_NOR_ID) 
        references ADD_STRAGIS_MSG;
    alter table ADD_INSPEKSTRA_MSG 
        add constraint CFK_IRG_IRS_NOR 
        foreign key (IRS_IRS_IRG_NOR_ID) 
        references ADD_INSPEKROUTE_MSG;
    alter table ADD_KENGRFJDET_MSG 
        add constraint CFK_BLK_KFD_NOR 
        foreign key (KFD_KFD_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_KENGRFJDET_MSG 
        add constraint CFK_KFJ_KFD_NOR 
        foreign key (KFD_KFD_KFJ_NOR_ID) 
        references ADD_KENGRFJ_MSG;
    alter table ADD_KENGRFJ_MSG 
        add constraint CFK_MAN_KFJ_NOR 
        foreign key (KFJ_KFJ_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_KOORMASSGIS_MSG 
        add constraint CFK_MTK_KMG_NOR 
        foreign key (KMG_KMG_MTK_NOR_ID) 
        references VAT_MASSTYP_MSG;
    alter table ADD_KOORMASSGIS_MSG 
        add constraint CFK_RFG_KMG_NOR 
        foreign key (KMG_KMG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_KOORMASSGIS_MSG 
        add constraint CFK_MAN_KMG_NOR 
        foreign key (KMG_KMG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_BETSYS_MSG 
        add constraint FK457374AC70368936 
        foreign key (BST_BST_KMG_NOR_ID) 
        references ADD_KOORMASSGIS_MSG;
    alter table ADD_MANDANTDET_MSG 
        add constraint CFK_GEK_MAD_NOR 
        foreign key (MAD_MAD_GEK_NOR_ID) 
        references VAT_GEMKAT_MSG;
    alter table ADD_MANDANTDET_MSG 
        add constraint CFK_OVG_MAD_NOR 
        foreign key (MAD_MAD_OVG_NOR_ID) 
        references VAT_OEFFVERKAT_MSG;
    alter table ADD_MANDANTDET_MSG 
        add constraint CFK_MAN_MAD_NOR 
        foreign key (MAD_MAD_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_MANDANTDET_MSG 
        add constraint CFK_EPR_MAD_NOR 
        foreign key (MAD_MAD_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_MANDANTLOGO_MSG 
        add constraint CFK_MAN_MAL_NOR 
        foreign key (MAL_MAL_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table VAT_MASSVORSCH_MSG 
        add constraint CFK_MTK_MVK_NOR 
        foreign key (MVK_MVK_MTK_NOR_ID) 
        references VAT_MASSTYP_MSG;
    alter table VAT_MASSVORSCH_MSG 
        add constraint CFK_BLK_MVK_NOR 
        foreign key (MVK_MVK_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table VAT_MASSVORSCH_MSG 
        add constraint CFK_MAN_MVK_NOR 
        foreign key (MVK_MVK_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table VAT_MASSVORSCH_MSG 
        add constraint CFK_EPR_MVK_NOR 
        foreign key (MVK_MVK_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_MASSTEILGIS_MSG 
        add constraint CFK_RFG_MTG_NOR 
        foreign key (MTG_MTG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_MASSTEILGIS_MSG 
        add constraint CFK_MAN_MTG_NOR 
        foreign key (MTG_MTG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_NETZSUMDET_MSG 
        add constraint CFK_BLK_NSD_NOR 
        foreign key (NSD_NSD_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_NETZSUMDET_MSG 
        add constraint CFK_NSU_NSD_NOR 
        foreign key (NSD_NSD_NSU_NOR_ID) 
        references ADD_NETZSUM_MSG;
    alter table ADD_NETZSUM_MSG 
        add constraint CFK_MAN_NSU_NOR 
        foreign key (NSU_NSU_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_NETZSUM_MSG 
        add constraint CFK_EPR_NSU_NOR 
        foreign key (NSU_NSU_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_RELMASSTAB_MSG 
        add constraint CFK_MTK_RMT_NOR 
        foreign key (RMT_RMT_MTK_NOR_ID) 
        references VAT_MASSTYP_MSG;
    alter table ADD_RELMASSTAB_MSG 
        add constraint CFK_MTK_RMT_TR 
        foreign key (RMT_RMT_MTK_TR_ID) 
        references VAT_MASSTYP_MSG;
    alter table ADD_RELMASSTAB_MSG 
        add constraint CFK_BLK_RMT_NOR 
        foreign key (RMT_RMT_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_RELMASSTAB_MSG 
        add constraint CFK_EPR_RMT_NOR 
        foreign key (RMT_RMT_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_RELMASSTAB_MSG 
        add constraint CFK_MAN_RMT_NOR 
        foreign key (RMT_RMT_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_MTK_RMG_NOR 
        foreign key (RMG_RMG_MTK_NOR_ID) 
        references VAT_MASSTYP_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_MTK_RMG_TR 
        foreign key (RMG_RMG_MTK_TR_ID) 
        references VAT_MASSTYP_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_BLK_RMG_NOR 
        foreign key (RMG_RMG_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_RFG_RMG_NOR 
        foreign key (RMG_RMG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_MAN_RMG_NOR 
        foreign key (RMG_RMG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_REALMASSGIS_MSG 
        add constraint CFK_EPR_RMG_NOR 
        foreign key (RMG_RMG_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_BETSYSRM_MSG 
        add constraint FK2991BD4A94BB00D 
        foreign key (BST_BST_RMG_NOR_ID) 
        references ADD_REALMASSGIS_MSG;
    alter table ADD_RELMASSSUM_MSG 
        add constraint CFK_EPR_RMS_NOR 
        foreign key (RMS_RMS_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_RELMASSSUM_MSG 
        add constraint CFK_MAN_RMS_NOR 
        foreign key (RMS_RMS_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_RELMASSSUM_MSG 
        add constraint CFK_BLK_RMS_NOR 
        foreign key (RMS_RMS_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADR_REFGRUPPE_MSG 
        add constraint CFK_RFG_RFG_NOR 
        foreign key (RFG_RFG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_SCHADDET_MSG 
        add constraint CFK_ZST_SCD_NOR 
        foreign key (SCD_SCD_ZST_ID) 
        references ADD_ZSTTAB_MSG;
    alter table ADD_SCHADDET_MSG 
        add constraint CFK_ZSG_SCD_NOR 
        foreign key (SCD_SCD_ZSG_ID) 
        references ADD_ZSTGIS_MSG;
    alter table ADD_SCHADGRUPPE_MSG 
        add constraint CFK_ZST_SCG_NOR 
        foreign key (SCG_SCG_ZST_ID) 
        references ADD_ZSTTAB_MSG;
    alter table ADD_SCHADGRUPPE_MSG 
        add constraint CFK_ZSG_SCG_NOR 
        foreign key (SCG_SCG_ZSG_ID) 
        references ADD_ZSTGIS_MSG;
    create index IDX_SEKTOR_BSID on SRD_SEKTOR_MSG (SEK_BSID_VL);
    create index IDX_SEKTOR_SEGMENT on SRD_SEKTOR_MSG (SEK_SEK_ACS_NOR_ID);
    alter table SRD_SEKTOR_MSG 
        add constraint CFK_ACS_SEK_NOR 
        foreign key (SEK_SEK_ACS_NOR_ID) 
        references SRD_ACHSSEG_MSG;
    alter table SRD_SEKTOR_MSG 
        add constraint CFK_SEK_SEK_NOR 
        foreign key (SEK_SEK_SEK_NOR_ID) 
        references SRD_SEKTOR_MSG;
    alter table ADD_STRAGIS_MSG 
        add constraint CFK_BLK_STG_NOR 
        foreign key (STG_STG_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_STRAGIS_MSG 
        add constraint CFK_MAN_STG_NOR 
        foreign key (STG_STG_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_STRAGIS_MSG 
        add constraint CFK_EPR_STG_NOR 
        foreign key (STG_STG_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_STRAGIS_MSG 
        add constraint CFK_RFG_STG_NOR 
        foreign key (STG_STG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_STRAGIS_MSG 
        add constraint CFK_STG_STG_NOR 
        foreign key (STG_STG_STG_NOR_ID) 
        references ADD_STRAGIS_MSG;
    alter table ADD_STRATAB_MSG 
        add constraint CFK_BLK_STT_NOR 
        foreign key (STT_STT_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table ADD_STRATAB_MSG 
        add constraint CFK_MAN_STT_NOR 
        foreign key (STT_STT_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table ADD_STRATAB_MSG 
        add constraint CFK_EPR_STT_NOR 
        foreign key (STT_STT_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table VAT_WBBKAT_MSG 
        add constraint CFK_BLK_WBK_NOR 
        foreign key (WBK_WBK_BLK_NOR_ID) 
        references VAT_BELKAT_MSG;
    alter table VAT_WBBKAT_MSG 
        add constraint CFK_MAN_WBK_NOR 
        foreign key (WBK_WBK_MAN_NOR_ID) 
        references ADD_MANDANT_MSG;
    alter table VAT_WBBKAT_MSG 
        add constraint CFK_EPR_WBK_NOR 
        foreign key (WBK_WBK_EPR_NOR_ID) 
        references ADD_ERFPERIODE_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_MVK_ZSG_FB 
        foreign key (ZSG_ZSG_MVK_FB_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_MVK_ZSG_TRR 
        foreign key (ZSG_ZSG_MVK_TRR_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_MVK_ZSG_TRL 
        foreign key (ZSG_ZSG_MVK_TRL_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_STG_ZSG_NOR 
        foreign key (ZSG_ZSG_STG_NOR_ID) 
        references ADD_STRAGIS_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_RFG_ZSG_NOR 
        foreign key (ZSG_ZSG_RFG_NOR_ID) 
        references ADR_REFGRUPPE_MSG;
    alter table ADD_ZSTGIS_MSG 
        add constraint CFK_ZSG_ZSG_NOR 
        foreign key (ZSG_ZSG_ZSG_NOR_ID) 
        references ADD_ZSTGIS_MSG;
    alter table ADD_ZSTTAB_MSG 
        add constraint CFK_MVK_ZST_FB 
        foreign key (ZST_ZST_MVK_FB_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTTAB_MSG 
        add constraint CFK_MVK_ZST_TRR 
        foreign key (ZST_ZST_MVK_TRR_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTTAB_MSG 
        add constraint CFK_MVK_ZST_TRL 
        foreign key (ZST_ZST_MVK_TRL_ID) 
        references VAT_MASSVORSCH_MSG;
    alter table ADD_ZSTTAB_MSG 
        add constraint CFK_STT_ZST_NOR 
        foreign key (ZST_ZST_STT_NOR_ID) 
        references ADD_STRATAB_MSG;