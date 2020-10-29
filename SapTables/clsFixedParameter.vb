'===========BM00000007858==============
Imports common
Imports System.Data.SqlClient


Public Class clsFixedParameterType
    Public Const AllowCratePhysicalStock = "AllowCratePhysicalStock"
    Public Const EnableAutoDocNoShipToLocation = "Enable Auto DocNo ShipToLocation"
    Public Const ChangeFATCLRafterspecialApprovalonQC = "Change FAT CLR after special approval on QC"
    Public Const CreateCommonSeriesLocationwiseForAllSale = "Create Common Series Location Wise For All Sale"
    Public Const EnableCustomerPODetailonDairyBooking = "EnableCustomerPODetailonDairyBooking"
    Public Const CreateCommonDairyDispatchforFreshAmbient = "CreateCommonDairyDispatchforFreshAmbient"
    Public Const CreateSeperateTaxInvForFOCIteminNonTaxdispatch = "CreateSeperateTaxInvForFOCIteminNonTaxdispatch"
    Public Const CreateSeperateSeriesforRefDocARinvforCreditdebit = "CreateSeperateSeriesforRefDocARinvforCreditdebit"
    Public Const CreateSeperateSeriesforRefDocAPinvforCreditdebit = "CreateSeperateSeriesforRefDocAPinvforCreditdebit"
    Public Const AllowUniqueNoOnMilkTransferInandTankDis = "Allow UniqueNo On Milk TransferIn and Tanker Dispatch"
    Public Const DoNotConsiderCustomerCreditLimit = "Do not Consider Customer Credit Limit"
    Public Const AllowVSPMasterAutoPrefix = "Allow VSP Master Auto Prefix"
    Public Const AllowBulkProcTransDateSameasGateEntryDate = "Allow BulkProc TransDate Same as GateEntry Date"
    Public Const AllowDifferentStateofChildCustomerOnPS = "Allow Different State of Child Customer On PS"
    Public Const AllowProvisionknokoffOnAPInvoice = "Allow Provision knokoff On APInvoice"
    Public Const AllowTransactionFiltersOnCustomerlegder = "AllowTransactionFiltersOnCustomerlegder"
    Public Const AllowtoSHOWParentChildCustomer = "AllowtoSHOWParentChildCustomer"
    Public Const AllowtoMakeApplyDocOnbyDefault = "AllowtoMakeApplyDocOnbyDefault"
    Public Const CreateProvisionJournalEntryForSale = "Create Provision JournalEntry Sale"
    Public Const ShowDairySaleModuleOnBulkPosting = "Show DairySale Module On BulkPosting"
    Public Const ShowGateEntryTypeonGateEntryBulkProc = "Show GateEntryType on GateEntry BulkProc"
    Public Const AllowAutoBulkMilkSRNonWeighmentBulkProc = "Allow Auto BulkMilkSRN on Weighment BulkProc"
    Public Const PickCorrectionFactorProcurementTypewise = "Pick CorrectionFactor ProcurementType wise"
    Public Const CheckParameterRangerProcurementTypewise = "Check ParameterRanger ProcurementType wise"
    Public Const CalculateTaxRatefromItemwsieTaxOnSale = "CalculateTaxRatefromItemwsieTaxOnSale"
    Public Const AllowBulkProcMCCwithoutTankerDispatch = "Allow Bulk Proc MCC without Tanker Dispatch"
    Public Const AllowJobWorkonGateEntryBulkProc = "Allow Job Work ON Gate Enty BulkProc"
    Public Const ApplyMaTransRateOnMultChamberTankerDis = "ApplyMaTransferRateOnMultilpeChamberTankerDispatch"
    Public Const AllowManualPriceONBulkPO = "Allow Manual Price ON Bulk PO"
    Public Const AllowReverseUnpost = "Allow Reverse and Unpost"
    Public Const AllowtoSetNoOfTransactionsforSetOff = "AllowtoSetNoOfTransactionsforSetOff"
    Public Const AllowtoSetReceiptAmountForCashTransaction = "AllowtoSetReceiptAmountForCashTransaction"
    Public Const AllowtoSetPaymentAmountForCashTransaction = "AllowtoSetPaymentAmountForCashTransaction"
    Public Const AllowtoShowCreditBalanceonCustomerAgeing = "Allow to Show Credit Balance"
    Public Const AllowtoShowDebitBalanceonVendorAgeing = "Allow to Show Debit Balance"
    Public Const AllowtoNegativeStockInventoryAtTankerDispatch = "AllowtoNegativeStockInventoryAtTankerDispatch"
    Public Const AllowtoSkipfunctionalityafterSRNOnBulkProcurement = "AllowtoSkipfunctionalityafterSRNOnBulkProcurement"
    Public Const AllowtoSetoffDocDateWise = "AllowtoSetoffDocDateWise"
    Public Const AllowtoSkipJournalEntryofPaymentandReceiptforAD = "AllowtoSkipJournalEntryofPaymentandReceiptforAD"
    Public Const AllowtoEmployeeSalaryIntegration = "AllowtoEmployeeSalaryIntegration"
    Public Const AllowtoFutureDateTransForPDCCheque = "AllowtoFutureDateTransForPDCCheque"
    Public Const WeightOfCanForCanSale = "Weight Of Can For Can Sale"
    Public Const RunBulkProcWithoutMilkGrade As String = "Run Bulk Proc without Milk Grade"
    Public Const DaysToStartAutoLock As String = "Days ToStart AutoLock"
    Public Const AllowRandomOnlyOneSecondaryQC As String = "Allow Random OnlyOne SecondaryQC"
    Public Const AllowGateEntryAgainstPO As String = "Allow GateEntry Against PO"
    Public Const SeparateDairyDispatchTaxableNonTaxable As String = "Separate DairyDispatch TaxableNon Taxable"
    Public Const RunBatchFifowise As String = "Run Batch Fifowise"
    Public Const PromptTimeToPostTransactions As String = "Prompt Time ToPost Transactions"
    Public Const CreateFreshInvoiceOnDispatchSave As String = "Create FreshInvoice OnDispatch Save"
    Public Const AllowLockTransactionUserwise As String = "AllowLockTransactionUserwise"
    Public Const AllowAutoLockTransaction As String = "AllowAutoLockTransaction"
    Public Const AllowDefaultBankCodeforCreditNote As String = "AllowDefaultBankCodeforCreditNote"
    Public Const AllowUseApplyDocSeriesForReceipt As String = "AllowUseApplyDocSeriesForReceipt"
    Public Const AllowUseApplyDocSeriesForPayment As String = "AllowUseApplyDocSeriesForPayment"
    Public Const AllowCreditNoteWithoutReference As String = "AllowCreditNoteWithoutReference"
    Public Const AllowBranchAcconReceiptPrint As String = "AllowBranchAcconReceiptPrint"
    Public Const AllowCreditNoteWithoutReferenceonAP As String = "AllowCreditNoteWithoutReferenceonAP"
    Public Const SecurityDocumentKnockOffonReceipt As String = "SecurityDocumentKnockOffonReceipt"
    Public Const AllowFreshInvoiceAutoPost As String = "AllowFreshInvoiceAutoPost"
    Public Const AllowReceiptThroughSO As String = "AllowReceiptThroughSO"
    Public Const AllowSetOffUntilTransactionsnotend As String = "AllowSetOffUntilTransactionsnotend"
    Public Const AllowGateReturn As String = "AllowGateReturn"
    Public Const CalculateCommOnCSATransWOConversion As String = "CalculateCommOnCSATransWOConversion"
    Public Const AllowSRNWithoutShortageRejection As String = "AllowSRNWithoutShortageRejection"
    Public Const AllowPurchaseModulewithUniqueItem As String = "AllowPurchaseModulewithUniqueItem"
    Public Const GrossWeightUnit As String = "GrossWeightUnit"
    Public Const ExpiryDaysBulkProcurementPriceChart As String = "ExpiryDays BulkProcurement PriceChart"
    Public Const ShowSchemeItemRateonDairyDispatch As String = "Show Scheme Item Rate on Dairy Dispatch"
    Public Const AutoCalculateCrateOnDairyDispatch As String = "Auto Calculate Crate on Dairy Dispatch"
    Public Const GrossWtFromItemMasterONProductSale As String = "Gross Wt. from item master on Product Sale"
    Public Const CreateVatSeriesForProductExciseinvoice = "Create Vat Series for PS Excise invoice"
    Public Const AllowFreshPriceChartOnProductSale = "Allow Fresh Price Chart on Product Sale"
    Public Const AllowFreshPriceChartOnBookingProductSale = "Allow Fresh Price Chart on Booking PS"
    Public Const ShowUnloadingandWeighmentSequencewise = "Show Unloading Weighment sequence wise"
    Public Const ShowBothTankertypeOnCleaning = "Show Both Tanker Type on Cleaning"
    Public Const isCleaningMandatoryBeforeGateout = "Is Cleaning Mandatory before Gate Out"
    Public Const AllowBulkProcurementSequencewise = "Allow Bulk Procurement Sequence wise"
    Public Const ShowItemLocationWiseonDairyBooking = "Show Item Location wise on Dairy Booking"
    Public Const CheckOutstandingCreditLimitOnBooking = "Check Customer Outstanding on Booking"
    Public Const AllowBulkPriceChartMultiplepriceToMultipleVendor = "Allow BulkPrice Multiple Price to Mult Vendor"
    Public Const ShowOptionOnItemMasterChangeItemRate = "Show option on Item Change Rate on DDispatch"
    Public Const SHowOptionOnLocationForDairyDispatchfromDOorGatepass = "Show option on loc For DDispatch from DO/GP"
    Public Const showPostrequiredforBulkSale = "showPostrequiredforBulkSale"
    Public Const ApplyDocumentDate = "Apply Document Date"
    Public Const AllowStockCheckatDOLevel = "AllowStockCheckatDOLevel"
    Public Const AllowAdditionalWeightinPercentage = "AllowAdditionalWeightinPercentage"
    Public Const EnterAdditionalWeight = "EnterAdditionalWeight"
    Public Const AllowTankerBasedonVendorofGE = "Allow Tanker Based on Vendor of GE"
    Public Const AllowUseBoilingParameteronParameterMaster = "Allow Use Boiling Parameter on Parameter Master"
    Public Const DairyDispatchFromDeliveryNote = "Dairy Dispatch From Delivery Note"
    Public Const ItemTypeForDairyBooking = "Item Type Fresh or Ambient For Dairy Booking"
    Public Const AllowStockToleranceNegative = "Allow Stock Tolerance Negative"
    Public Const StockToleranceLimit = "Stock Tolerance Limit"
    Public Const ShowAllMenu As String = "Show All Menu"
    Public Const POSeriesWithoutItemTypewise As String = "POSeriesWithoutItemTypeWise"
    Public Const WorkApprovalFlowInERP As String = "Work Approval Flow in ERP"
    Public Const OpenAvailorEmptyStckLocationOn_Standardization As String = "Open Avail./Empty Location on Standardization"
    Public Const BOM_Amend_Pswd As String = "Amendment Password for BOM"
    Public Const ProductionFATSNF_KG_Unit As String = "ProductionFATSNF_KG_Unit"
    Public Const ChangeRateAT_CSA_Return As String = "Rate Change At CSA Transfer Return"
    Public Const VehicleCapacityUnit As String = "VehicleCapacityUnit"
    Public Const StopGLEntryForConsignmentAtCSATransfer As String = "Stop GL Consignment at CSA Transfer"
    Public Const CSATransfer_SalePatti_All_Tax_Open As String = "Open All Tax Mapped With Location In CSA"
    Public Const CreateGLForTransfer As String = "CreateGLForTransfer"
    Public Const Active As String = "Active"
    Public Const Agency As String = "Agency"
    Public Const Category1 As String = "Category1"
    Public Const Category2 As String = "Category2"
    Public Const Category3 As String = "Category3"
    Public Const cboPeriodicity As String = "cboPeriodicity"
    Public Const CboRound As String = "CboRound"
    Public Const QuickExport As String = "Q_EX"
    Public Const UploaderPassword As String = "Uploader Password"
    Public Const AllowDesignAtRunTime As String = "AllowDesignRunTime"
    Public Const SkipDiffGLOnPI As String = "SkipDiffGLOnPI"
    Public Const SkipCogsEntry As String = "SkipCogsEntry"
    Public Const ShowPurchaseControlAc As String = "ShowPurchaseControlAc"
    Public Const CreatePOWithRequisition As String = "CreatePOWithRequisition"
    Public Const DisplayReasonOnDelete As String = "DisplayReasonOnDelete"
    Public Const DisplayReasonOnUpdateAfterPost As String = "DisplayReasonOnUpdateAfterPost"
    Public Const Importbulkdatafromexcelsheet As String = "Importbulkdatafromexcelsheet"
    Public Const Distributor As String = "Distributor"
    Public Const Helper As String = "Helper"
    Public Const Driver As String = "Driver"
    Public Const ZM As String = "ZM"
    Public Const TSM As String = "TSM"
    Public Const ASM As String = "ASM"

    Public Const PROVISIONENTRYONSTOCKTRANSFER As String = "ProvisionOnStockTransfer"
    Public Const Inactive As String = "Inactive"
    Public Const ALLOWANYBO As String = "Allow Any Type of BO"
    Public Const ALLOWCBOSBO As String = "Allow Child and SubChild BO"
    Public Const INDUSTRYTYPE As String = "Industry Type"
    Public Const Others As String = "Others"
    Public Const PayHeadSubHead As String = "PayHeadSubHead"
    Public Const PZ As String = "PZ"
    Public Const ROUTE As String = "ROUTE"
    Public Const Route1 As String = "Route"
    Public Const Salesman As String = "Salesman"
    Public Const SIR As String = "SIR"
    Public Const SIRC As String = "SIRC"
    Public Const GEUpdateAfterPost As String = "GEUpdateAfterPost"
    Public Const GEUpdatePriceChart As String = "GEUpdatePriceChart"
    Public Const SetCSATransferwithZeroOnSalePatti As String = "SetCSATransferwithZeroOnSalePatti"
    Public Const POAmendmentType As String = "POAmendmentType"
    Public Const BulkInvoiceDeleteType As String = "BulkInvoiceDeleteType"
    Public Const BulkSaleSequence As String = "BulkSaleSequence"
    Public Const BulkQCTableHavingUniqueKey As String = "BulkQCTableHavingUniqueKey"
    Public Const SrPath As String = "SrPath"
    Public Const TempProvisional As String = "TempProvisional"
    Public Const TF As String = "TF"
    Public Const Z As String = "Z"
    Public Const ZA As String = "ZA"
    Public Const ZB As String = "ZB"
    Public Const ZC As String = "ZC"
    Public Const ZD As String = "ZD"
    Public Const ZE As String = "ZE"
    Public Const ZF As String = "ZF"
    Public Const MilkProc As String = "MilkProc"
    Public Const EnablePopupItemReorderLevel As String = "EnablePopupItemReorderLevel"
    Public Const PrintVerify As String = "Print Verify"
    Public Const LOReceiptDefaultBankForSettlement As String = "Default Bank For Settlement"
    Public Const LOReceiptPaymentTypeForSettlement As String = "Default Payment Type For Settlement"
    Public Const DefaultValue As String = "DefaultValue"
    Public Const SalesmanPhysicalLocation As String = "SPL"
    Public Const IndentTolerence As String = "IndentTolerence"
    Public Const AskForDate As String = "AskForDate"
    Public Const PickMachineDateForTran As String = "PickMachineDateForTran"
    Public Const ReqLimitOnSRN As String = "ReqLimitOnSRN"
    Public Const AutoLoadinFromLocation As String = "AutoLoadinFromLocation"
    Public Const CrreateTransferShipmentJE As String = "CrreateTransferShipmentJE"
    Public Const IsNotIncludeWasteQtyInCal As String = "IsNotIncludeWasteQtyInCal"
    Public Const IsConsiderOutTypeDocForBalance As String = "IsConsiderOutTypeDocForBalance"
    Public Const BankTransferRunPaymentCounter As String = "BankTransferRunPaymentCounter"
    Public Const PaymentReceiptTypeRunReceiptCounter As String = "PaymentReceiptTypeRunReceiptCounter"
    Public Const CounterFinancialYearStyle As String = "CounterFinancialYearStyle"
    Public Const LinkFinancialYearStyleWithGSTDate As String = "LinkFinancialYearStyleWithGSTDate"
    Public Const CashDiscountFromClaimMaster As String = "CashDiscountFromClaimMaster"
    Public Const TransferTransTypeRouteHide As String = "TransferTransTypeRouteHide"
    Public Const AllowNegtiveOfSaleInvoiceBlanceAmt As String = "AllowNegtiveOfSaleInvoiceBlanceAmt"
    Public Const SalesRateEditable As String = "Sales Rate Editable"
    Public Const RunDemoERP As String = "RunDemoERP"
    Public Const IsKDIL As String = "IsKDIL"
    Public Const SendToTally As String = "SendToTally"
    Public Const PromptForTally As String = "PromptForTally"
    Public Const CurrentMaufacturingType As String = "ManufacturingType"
    Public Const TallyCompany As String = "TallyCompany"
    Public Const TallyIP As String = "TallyIP"
    Public Const TallyPort As String = "TallyPort"
    Public Const TaxRoundOffToZeroDecimalPlace As String = "TaxRoundOffToZeroDecimalPlace"
    Public Const BalanceSheetProftAndLossGroupCode As String = "BalanceSheetProftAndLossGroupCode"
    Public Const BalanceSheetProftAndLossGroupDesc As String = "BalanceSheetProftAndLossGroupDesc"
    Public Const ApplyCostingOnPostedDate As String = "ApplyCostingOnPostedDate"
    Public Const isBatchApplyOnInventoryMovement As String = "isBatchApplyOnInventoryMovement"
    Public Const BlankDatabase As String = "BlankDatabase"
    Public Const ServiceDealer As String = "Service Dealer"
    Public Const TDM As String = "TDM"
    Public Const MAILOFF As String = "MAILOFF"
    ''added by shivani
    Public Const AllowToSaveTimeWithDocumentDate As String = "Allow To Save Time With Document Date"
    Public Const AllowToPrintTimeWithDocumentDate As String = "Allow To Print Time With Document Date"
    Public Const AllLevelApprovalIsMandatory As String = "All Level Approval Is Mandatory"
    Public Const AssetGroupPrefix As String = "AssetGroupPrefix"
    Public Const DepreciationCalculationMethod As String = "Depreciation Calculation Method"
    Public Const STDPURRATE As String = "STDPURRATE"
    Public Const AutoPOAtSRN As String = "AUTOPOATSRN"
    Public Const DisableShipToLocation As String = "Disable Ship_To_Location For (PO,PI,SRN)"
    Public Const AllowLargerItemCostThenVendorItemCost As String = "Allow Larger Item Cost Then Vendor Item Cost"
    Public Const PurchasePickItemFromVendorItemDetails As String = "PurchasePickItemFromVendorItemDetails"
    Public Const PurchaseOneItemOneVendor As String = "PurchaseOneItemOneVendor"
    Public Const PostShipmentonAutoSTN As String = "PostShipmentonAutoSTN"
    Public Const IsRemarksMandatoryOnCloseSaleOrder As String = "IsRemarksMandatoryOnCloseSaleOrder"
    Public Const LCCancellationPwd As String = "LCCancellationPwd"
    Public Const ShowQtySum_in_GRN_MRN_SRN As String = "ShowQtySum_in_GRN_MRN_SRN"
    Public Const CreateInvoicewithShipmentonAutoSTN As String = "CreateInvoicewithShipmentonAutoSTN"
    Public Const AllowSingleInvoiceAgainstSingleOrder As String = "AllowSingleInvoiceAgainstSingleOrder"
    Public Const WorkingHours As String = "WorkingHours"
    Public Const TreatExcessLeaveAbsent As String = "TreatExcessLeaveAbsent"
    Public Const VehicleInsuranceAlert As String = "VehicleInsuranceAlert"
    Public Const IsItemRateEditableOnTransfer As String = "IsItemRateEditableOnTransfer"
    Public Const GLACAccordingToTaxRate As String = "GLACAccordingToTaxRate"
    Public Const AutoSchemeOn As String = "AutoSchemeOn"
    Public Const IsTransferQtyEditableOnAutoSTN As String = "IsTransferQtyEditableOnAutoSTN"
    Public Const IsItemRateEditableOnSales As String = "IsItemRateEditableOnSales"
    Public Const IsItemMRPEditableOnSales As String = "IsItemMRPEditableOnSales"
    Public Const ShowSNF9IfSNFGreaterThan9 As String = "ShowSNF9IfSNFGreaterThan9"
    Public Const IsItemRateEditableOnSalesForAprilOnly As String = "ForAprilOnly"
    Public Const PWD As String = "PWD"
    Public Const AllowMilkReceiptAfterSettingsisOn As String = "AllowMilkReceiptAfterSettingsisOn"
    Public Const MilkReceiptTolerancePwd As String = "MilkReceiptTolerancePwd"
    Public Const MCC_DLTDATA_PWD As String = "MCC_DLTDATA_PWD"
    Public Const Allow_Excel_Code_on_Mcc_Master As String = "AllowExCodeONMcc"
    Public Const is_Allow_cancel_Transaction As String = "is_Allow_cancel_Transaction"
    Public Const is_Allow_cancel_Posted_Transaction As String = "is_Allow_cancel_Posted_Transaction"
    Public Const ShiftTiming As String = "ShiftTiming"
    '=shivani
    Public Const MulticurrencyDecimalPlaces As String = "MulticurrencyDecimalPlaces"
    Public Const SMS_User_Name As String = "SMS_User_Name"
    Public Const SMS_User_PWD As String = "SMS_User_PWD"
    Public Const SMS_Sendor_ID As String = "SMS_Sendor_ID"
    Public Const SMS_Provider As String = "SMS_Provider"
    Public Const MCCDefaultMilkItem As String = "MCCDefaultMilkItem"
    Public Const BulkSaleDefaultMilkItem As String = "BulkSaleDefaultMilkItem"
    Public Const BSDefaultMilkItem As String = "BSDefaultMilkItem"
    Public Const DefaultRoundOffGLAccount As String = "DefaultRoundOffGLAccount"
    Public Const MCCSampleRange As String = "MCCSampleRange"
    Public Const MCCReceiptRange As String = "MCCReceiptRange"
    Public Const MCCMinKmRange As String = "MCCMinKmRange"
    Public Const Milk_Can_Weight_Ratio As String = "Can_Weight_Ratio"
    Public Const Milk_Can_Weight_Tolerance_Neg As String = "Can_Weight_Tolerance_Neg"
    Public Const Milk_Can_Weight_Tolerance_Positive As String = "Can_Weight_Tolerance_Positive"
    Public Const MCCFSSAI_DAYS As String = "MCCFSSAI_DAYS"
    Public Const MCCDisplay_All_Parameter As String = "MCCDis_P"
    '==========Rohit on 29,Oct 2014================
    Public Const MCCInvoiceScheduleDate As String = "MCCInvoiceScheduleDate"
    Public Const MCCInvoiceScheduleTime As String = "MCCInvoiceScheduleTime"
    Public Const MCCInvoiceScheduleInterval As String = "MCCInvoiceScheduleInterval"
    Public Const MCCMilkSRNRepost As String = "MCCMilkSRNRepost"
    '==============================================
    '===========Rohit on Jan 31,2015=====
    Public Const Is_Send_Sms As String = "Is_Send_Sms"
    Public Const Send_Sms_Time As String = "Send_Sms_Time"
    Public Const Is_Send_Sms_ForVSP As String = "Is_Send_Sms_ForVSP"
    Public Const Is_Pick_No_from_Mail_Setting As String = "Is_Pick_No_from_Mail_Setting"
    '=====================================
    '------Pankaj Jha
    Public Const ControlSampleMandatory As String = "ControlSampleMandatory"
    Public Const defaultCorrectionFactor As String = "defaultCorrectionFactor"
    '------End 
    Public Const ShowTaxRateColumnOnTransaction As String = "ShowTaxRateColumnOnTransaction"
    Public Const ShowGRN As String = "ShowGRN"
    Public Const SkipMRNGRNinCaseofMT As String = "SkipMRNGRNinCaseofMT"
    Public Const ShowMRN As String = "ShowMRN"

    Public Const LicenceExpiryDate As String = "IsApplyCommonService1" 'A B
    Public Const LicenceNoOfExeConnection As String = "IsApplyCommonService2" 'C
    Public Const LicenceNoOfJournalEntry As String = "IsApplyCommonService3" 'D
    Public Const LicenceNoOfUser As String = "IsApplyCommonService4" 'E

    Public Const EnableProjectFinder As String = "EnableProjectFinder"
    'richa 
    Public Const InvoiceManualNoWithPrefix As String = "InvoiceManualNoWithPrefix"
    Public Const AutoBackUp As String = "AutoBackUp"
    Public Const MCCPurchase As String = "MCCPurchase"
    'richa Ticket No BM00000003045 09/07/2014
    Public Const NotificationSettingforReOrderInPO As String = "NotificationSettingforReOrderInPO"
    'richa Ticket No BM00000003042 09/07/2014
    Public Const NotificationSettingforReOrderInPurchaseRequisition As String = "NotificationSettingforReOrderInPurchaseRequisition"
    Public Const PurchaseOrderAutomaticallyItemQtyBelowReorderLevel As String = "PurchaseOrderAutomaticallyItemQtyBelowReorderLevel"
    Public Const NLevelAtVendor As String = "NLevel_Vendor"
    Public Const NLevelAtCustomer As String = "NLevel_Customer"
    Public Const NLevelAtLocation As String = "NLevel_Location"
    Public Const AutoItemNLevel As String = "NLevel_ItemCode"

    Public Const Princi_Bom As String = "Principle_BOM"
    Public Const AP_INV_COMMSN As String = "AP_INV_COMMSN"
    Public Const Principal_Vendor As String = "Principal_Vendor"
    Public Const Principal_Vendor_Database As String = "Principal_Vendor_Database"
    Public Const Principal_Customer As String = "Principal_Customer"
    'Public Const ExeExpiredDate As String = "ExpiredDate"

    '' Anubhooti 10-July-2014 (BM00000002912)
    Public Const CalculateLTAOnHoliday As String = "CalculateLTAOnHoliday"
    Public Const CalculateLTAOnWeekend As String = "CalculateLTAOnWeekend"
    Public Const CalculateMediclaimOnHoliday As String = "CalculateMediclaimOnHoliday"
    Public Const CalculateMediclaimOnWeekend As String = "CalculateMediclaimOnWeekend"

    Public Const DiscountCodeForArAdj As String = "DiscountCodeForArAdj"
    Public Const DiscountCodeForMPAdj As String = "DiscountCodeForMPAdj"

    Public Const AutoRecieptBankCode As String = "AutoRecieptBankCode"
    Public Const AutoRecieptPaymentMode As String = "AutoRecieptPaymentMode"
    '' Anubhooti 21-Aug-2014 (Setting For Item Is_Purchaseable)
    Public Const Is_Purchaseable_Item As String = "Is_Purchaseable_Item"

    '' Anubhooti 21-Aug-2014 (Setting For Demo Print)
    Public Const Is_AbemdmentForDemo As String = "Is_AbemdmentForDemo"

    '' Anubhooti 26-Aug-2014 (Setting For Item Is_FinishedGoods)
    Public Const Is_FinishedGoods As String = "Is_FinishedGoods"

    '' Anubhooti 28-Aug-2014 (Setting For Demo Print:Purchase Module)
    Public Const ShowStatusForPurchase As String = "ShowStatusForPurchase"

    '' Anubhooti 21-Aug-2014 (Setting For Demo Print: Sales Module)
    Public Const ShowStatusForSales As String = "ShowStatusForSales"

    '' Anubhooti 21-Aug-2014 (Setting For Demo Print: Sales Module)
    Public Const ShowSerialNoForSales As String = "ShowSerialNoForSales"

    '' Anubhooti 02-Sep-2014 (Setting For Vendor Master)
    Public Const AutoGeneratedVendorCode As String = "AutoGeneratedVendorCode"

    Public Const AutoGeneratedVendorCodeForAllCompany As String = "AutoGeneratedVendorCodeForAllCompany"
    '' Anubhooti 02-Sep-2014 (Setting For Customer Master)
    Public Const AutoGeneratedCustomerCode As String = "AutoGeneratedCustomerCode"
    Public Const AutoGeneratedCustomerCodeForAllCompany As String = "AutoGeneratedCustomerCodeeForAllCompany"

    '' Anubhooti 03-Sep-2014 BM00000003437 (Setting For Sub Account in Bank Master)
    Public Const AllowToUseSubAccount As String = "AllowToUseSubAccount"

    '' Anubhooti 17-Dec-2014 BM00000004959 (Setting For Withdrawal/Receipt/Both in Bank Transfer)
    Public Const InTransitFeatureIsRequired As String = "InTransitFeatureIsRequired"
    Public Const PermissionSettingForTransactionWithBank As String = "Permission_Setting_For_Trans_With_Bank"
    Public Const ApplyBrachAccounting As String = "ApplyBrachAccounting"


    '' Anubhooti 12-Sep-2014 BM00000003890 (Setting For Fresh Sale)
    Public Const AllowToEnterMRPManually As String = "AllowToEnterMRPManually"

    '' Anubhooti 24-Sep-2014 BM00000003940 (Setting For Vehicle Master)
    Public Const AllowFieldsToBeManadatory As String = "AllowFieldsToBeManadatory"

    '' Anubhooti 08-Oct-2014 (Setting For Auto Generated Digits For Vendor)
    Public Const AutoGeneratedDigitsForVendor As String = "AutoGeneratedDigitsForVendor"

    '' Anubhooti 08-Oct-2014 (Setting For Auto Generated Digits For Customer)
    Public Const AutoGeneratedDigitsForCustomer As String = "AutoGeneratedDigitsForCustomer"

    '' Anubhooti 02-Dec-2014 (Setting For Unit Cost Editable/Non-Editable On SRN)
    Public Const IsRateEditableOnSRN As String = "IsRateEditableOnSRN"
    '' Anubhooti 23-Jan-2015 (Setting For Creation of GL Acc To Item GL Account(Issue/Return/Transfer))
    Public Const CreateGLAccToItem As String = "CreateGLAccToItem"
    '' Anubhooti 29-Jan-2015 (Setting For Cost Edit/Non-Edit On(Issue/Return/Transfer))
    Public Const IsCostEditableOnIssueReturnTransfer As String = "IsCostEditableOnIssueReturnTransfer"

    Public Const UpdateCrateLinerQty As String = "UpdateCrateLinerQty"

    'Richa Agarwal 05/08/2014 Against Ticket No BM00000003248
    Public Const AllowDispatchOutstandingBS As String = "AllowDispatchOutstandingBS"
    Public Const AllowDispatchOutstandingFS As String = "AllowDispatchOutstandingFS"
    Public Const AllowDispatchOutstandingPS As String = "AllowDispatchOutstandingPS"
    Public Const IsVolumeSchemeBydefault As String = "IsVolumeSchemeBydefault"

    'Richa Agarwal 19/08/2014 Against Ticket No BM00000003110
    Public Const AllowDeliveryOrderIncaseAmountIncreases As String = "AllowDeliveryOrderIncaseAmountIncreases"
    '--------Richa Agarwal 21/08/2014 Against Ticket No BM00000003438
    Public Const AllowAutoMRNGRNonDocumentAcceptance As String = "AllowAutoMRNGRNonDocumentAcceptance"
    Public Const AllowToShowSaleTypeinPaymentTermsReceivable As String = "AllowToShowSaleTypeinPaymentTermsReceivable"
    Public Const AllowToShowMilkTypeinAdjustmentEntry As String = "AllowToShowMilkTypeinAdjustmentEntry"
    Public Const GatePassAfterTransfer As String = "GatePassAfterTransfer"
    Public Const CreateTransferFromBooking As String = "CreateTransferFromBooking"
    Public Const PickRateFromPRICEChrtMasterFORUMang As String = "PickRateFromPRICEChrtMasterFORUMang"
    Public Const IGnoreGITAccount As String = "Ignore GIT Account in Financial Entry"
    Public Const AllowToEditCategoryCodeinItemMaster As String = "AllowToEditCategoryCodeinItemMaster"
    Public Const CreditLimitApproval As String = "CustomerCreditLimit"
    Public Const ViewTDSPwd As String = "ViewTDSPwd"
    '--------Richa Agarwal 28/08/2014 Against Ticket No .BM00000003667
    Public Const InvoiceBasedPO As String = "InvoiceBasedPO"
    Public Const AdvanceAgainstSO As String = "AdvanceAgainstSO"
    ''----------------------------------
    Public Const Purchase_SMSATPOST As String = "SMSATPOST_PUR"
    Public Const Sale_SMSATPOST As String = "SMSATPOST_SALE"
    ''richa 02/09/2014
    Public Const AmountLimitForInvoiceBulkSale As String = "AmountLimitForInvoiceBulkSale"
    Public Const ShowSaleInvoiceNoInPOfinderInSRN As String = "ShowSaleInvoiceNoInPOfinderInSRN"
    ''richa 09/09/2014
    '-----Updated by preeti Gupta-------------
    Public Const CrateValue As String = "CrateValue"
    Public Const CommitedDefaultQty As String = "CommitedDefaultQty"
    Public Const ShowBinMapping As String = "ShowBinMapping"
    Public Const ShowPrintChallanInDairyDispatch As String = "ShowPrintChallanInDairyDispatch"
    Public Const ShowCrateJaaliBoxIntransfer As String = "Show Crate Jaali & Box In transfer"
    '-------------------End-------------------
    Public Const DefaultCorrectionFactorForBulkSale As String = "DefaultCorrectionFactorForBulkSale"
    Public Const MCCdefaultCorrectionFactorBS As String = "MCCdefaultCorrectionFactorBS"
    Public Const JOBdefaultCorrectionFactorBS As String = "JOBdefaultCorrectionFactorBS"
    Public Const PurchasedefaultCorrectionFactorBS As String = "PurchasedefaultCorrectionFactorBS"
    Public Const AllowDeliveryQtygreaterthanBookingQtyPS As String = "AllowDeliveryQtygreaterthanBookingQtyPS"
    Public Const IsPickServerDateForMultipleDispatchInvoice As String = "IsPickServerDateForMultipleDispatchInvoice"
    Public Const TabOrder As String = "TabOrder"
    Public Const LoadLoginScreen As String = "LoadLoginScreen"
    Public Const IsItemWithDifferntUnitConsiderAsOtherItem As String = "ItemWithDifferntUnitConsiderAsOtherItem"
    Public Const IsMRPWiseBalance As String = "IsMRPWiseBalance"
    Public Const showRFQ As String = "showRFQ"
    Public Const CreateDbitNoteForShortPI As String = "CreateDbitNoteForLeakAndShortPI"
    Public Const CreateDbitNoteForRejectPI As String = "CreateDbitNoteForRejectPI"
    Public Const CreateDebitNoteForUnitCost As String = "CreateDebitNoteForUnitCost"
    Public Const TransferWithProductionSale_Retail_Series As String = "CreateTransferWithProductionSale_Retail_Series"
    Public Const TransferLocalInterState As String = "Stock/CSA_Transfer_With_Local/InterState_Series"
    Public Const ProductionQtyDecimalPoint As String = "ProductionQtyDecimalPoint"
    Public Const ProductionFATSNFPerDecimalPoint As String = "ProductionFATSNFPerDecimalPoint"
    Public Const ManualySelectBOMForChildBatch As String = "ManualySelectBOMForChildBatch"
    Public Const CSATransferWithProductionSale_Retail_Series As String = "CreateCSATransferWithProductionSale_Retail_Series"
    Public Const TransferJEForLocationMapping As String = "TransferJEForLocationMapping"
    Public Const AllowToDispalyAlertForBDayAnniversary As String = "AllowToDispalyAlertForBDayAnniversary"
    Public Const AllowToSendEmailForBDayAnniversary As String = "AllowToSendEmailForBDayAnniversary"
    Public Const ItemDescForTankerdispatchPrint As String = "ItemDescForTankerDispatchPrint"
    Public Const AllowPOScheduling As String = "Allow PO Scheduling"
    Public Const ERPStartDate As String = "ERPStartDate"
    Public Const CreateJEForTransfer As String = "CreateJEForTransfer"
    Public Const AllowToSkipStageQLLogSheetInProd As String = "AllowToSkipStageQLLogSheetInProd"
    Public Const IsRemarkReasonMandatoryOnPO As String = "IsRemarkReasonMandatoryOnPO"
    Public Const ShowCostCenterAndHierarchyLevelInPurchaseModule As String = "ShowCostCenterAndHierarchyLevelInPurchaseModule"
    Public Const IsQCColumnRequiredonMRN As String = "IsQCColumnRequiredonMRN"
    Public Const IsRGPAfterPurchaseOrder As String = "Do RGP After Purchase Order"
    Public Const AllowQualityModuleInERP As String = "On Quality Module"
    Public Const SRNReportQuantityWise As String = "SRNReportQuantityWise"
    Public Const IsCustomerGroupFieldsMandatory As String = "IsCustomerGroupFieldsMandatory"
    Public Const IsVendorGroupFieldsMandatory As String = "IsVendorGroupFieldsMandatory"
    Public Const AllowAutoNoForBackLogEntry As String = "AllowAutoNoForBackLogEntry"
    Public Const AllowDiffentSeriesExemptedItemONPS As String = "AllowDiffentSeriesExemptedItemONPS"
    Public Const DisplayFranchiseeinCustomer As String = "DisplayFranchiseeinCustomer"
    Public Const Idle As String = "Idle"
    Public Const AddressOnPaymentVoucher As String = "AddressOnPaymentVoucher"
    'richa agarwal 17/03/2015 against ticket no BM00000005874
    Public Const AllowBankDetailsManualinVM As String = "AllowBankDetailsManualinVM"
    ''--------------------------------
    ''RICHA AGARWAL 17/03/2015 Product Sale
    Public Const AllowToGenerateSaleInvoiceSeriesTaxTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesTaxTypeatPS"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesRetailTypeatPS"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesExciseTypeatPS"
    ''-------------------------
    ''RICHA AGARWAL 17/03/2015 MCC Sale
    Public Const AllowToGenerateSaleInvoiceSeriesTaxatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesTaxatMCCSale"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesRetailatMCCSale"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesExciseatMCCSale"
    ''-------------------------
    ''RICHA AGARWAL 17/03/2015 Misc Sale
    Public Const AllowToGenerateSaleInvoiceSeriesTaxatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesTaxatMiscSale"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesRetailatMiscSale"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesExciseatMiscSale"
    ''-------------------------
    '======================Preeti Gupta===========================
    Public Const ShowHierarchyAndCostCenterInAPInvoiceEntry As String = "ShowHierarchyAndCostCenterInAP"
    Public Const WeighmentNotMandatoryInMCC As String = "WeighmentNotMandatoryInMCC"
    '========================END========================================
    Public Const ShowHierarchyAndCostCenterInARInvoiceEntry As String = "ShowHierarchyAndCostCenterInAR"
    Public Const PartialFADepDays As String = "PartialFADepDays"
    Public Const RateMultPartialFADepDays As String = "RateMultPartialFADepDays"

    Public Const AllowNegativeStock As String = "AllowNegativeStock"
    Public Const SendSalarySlipMailToEmployee As String = "SendSalarySlipMailToEmployee"
    Public Const DoNotMergeAPARAccount As String = "DoNotMergeAPARAccount"
    Public Const ShowVisiDetail As String = "ShowVisiDetail"
    Public Const CustomerNameUniqueOnCM As String = "CustomerNameUniqueOnCM"
    Public Const IsShortageIncludeInLandedCost As String = "IsShortageIncludeInLandedCost"
    Public Const AlowwdateChangeinPaymentEntry As String = "AlowwdateChangeinPaymentEntry"
    Public Const CreateAutoMilkRGPinBulkSRN As String = "CreateAutoMilkRGPinBulkSRN"
    Public Const DisplayAllParameterinQualityCheck As String = "DisplayAllParameterinQualityCheck"
    Public Const DisplayTypeInMilkReceipt As String = "DisplayTypeInMilkReceipt"
    '============Added by Rohit on Aug 03,2015 For Milk Type Validation in Milk sample.============
    Public Const AddValidationofMilkTypeinsample As String = "AddValidationofMilkTypeinsample"

    Public Const FatMinCow As String = "FatMinCow"
    Public Const FatMaxCow As String = "FatMaxCow"
    Public Const SNFMinCow As String = "SNFMinCow"
    Public Const SNFMaxCow As String = "SNFMaxCow"

    Public Const FatMinBuff As String = "FatMinBuff"
    Public Const FatMaxBuff As String = "FatMaxBuff"
    Public Const SNFMinBuff As String = "SNFMinBuff"
    Public Const SNFMaxBuff As String = "SNFMaxBuff"

    Public Const FatMinMix As String = "FatMinMix"
    Public Const FatMaxMix As String = "FatMaxMix"
    Public Const SNFMinMix As String = "SNFMinMix"
    Public Const SNFMaxMix As String = "SNFMaxMix"
    '================================================================================================
    Public Const AddIncentiveDeductioninMilkSample As String = "AddIncentiveDeductioninMilkSample"
    Public Const AllowManualEnterinWeighment As String = "AllowManualEnterinWeighment"
    Public Const SettlementBankOnlyPWD As String = "SettlementBankOnlyPWD"
    Public Const DocumentSequence As String = "DocumentSequence"

    Public Const BOOKINGFINDER_ON_CSASALEPATTI As String = "CSA Sale Patti With Booking Knock-off"
    '=================Added by Rohit on Oct 12,2015================
    Public Const AllowPurchaseAccounting As String = "AllowPurchaseAccounting"
    Public Const SHowBulkMilkWeighment As String = "SHowBulkMilkWeighment"
    '==============================================================
    Public Const StoreADJExportImportAfterPost As String = "StoreADJExportImportAfterPost"
    '""""""""""""""""""""setting for Dairy Production"""""""""""""""""""""""""""""""""""""
    Public Const FatSNFControlOnProductionConsumption As String = "FatSNFControlOnProductionConsumption"
    Public Const QuantityControlToleranceOnProductionConsumption As String = "QuantityControlToleranceOnProductionConsumption"
    Public Const LeaveBalanceAlertTypeOnAttendance As String = "LeaveBalanceAlertTypeOnAttendance"
    Public Const StopNegativeBankBalance As String = "StopNegativeBankBalance"
    Public Const ConsumptionType As String = "ConsumptionType"
    Public Const ValidateFatSnfOnProduction As String = "ValidateFatSnfOnProduction"
    Public Const ShowOverheadCostOnProductionEntry As String = "ShowOverheadCostOnProductionEntry"
    Public Const ActivateProductionWithoutBatch As String = "ActivateProductionWithoutBatch"
    Public Const CreateJEOnProduction As String = "CreateJEOnProduction"
    ''===================================Setting for Payroll=========================================
    Public Const AllowToSaveMultipleEmployeeStatus As String = "AllowToSaveMultipleEmployeeStatus"
    Public Const CreateJEForProvisionEntry As String = "CreateJEForProvisionEntry"
    Public Const DoubleClickOnVC As String = "Double Click On VC"

    Public Const PickManual_CSATransfer_OnTRansferReturn As String = "CSA Transfer Effect on Return is Manual"
    Public Const PickManual_CSATransfer_OnCSASalePatti As String = "CSA Transfer Effect on Sale Patti is Manual"
    Public Const AllowDistributorSaleAtCSA_SaleInvoice As String = "Allow Distributor Sale at CSA Sale Patti"
    Public Const AllowItemWiseCSAAccountingON_CSASale As String = "CSA Account set pick Item-wise"
    Public Const IsAutoTankerWeightment As String = "Auto Tanker Weightment"
    Public Const IsAutoTankerWeighmentForBulkSale As String = "Auto Tanker Weighment for Bulk Sale"
    Public Const IsAdditionalInformationOnVillageMaster As String = "Show Village Add Info"
    Public Const CheckLiveStockInProductionDuringTrans As String = "CheckLiveStockInProductionDuringTrans"

    Public Const VLCTimeTableColumnShow As String = "VLCTimeTableColumnShow"
    Public Const VLCTimeTableColumnMandatory As String = "VLCTimeTableColumnMandatory"

    Public Const isOneMCCOnePrimaryTranporter As String = "One MCC One Primary Tranporter"
    Public Const isIntimationRequired As String = "Show Intimation Screen"
    Public Const QualityThenWeighmentinBulkProcurement As String = "First QC then Weighment"
    Public Const GateEntryTankerFromTankerMaster As String = "Gate Entry tanker From Master"
    Public Const isItemMilkType As String = "Is Item Milk Type"
    Public Const isPriceChartGradeWise As String = "Is Price Chart Grade Wise"
    Public Const isFarmerPaymentCycle As String = "is Farmer Payment Cycle"
    Public Const MilkSamplShowOddEvenTwoGrid As String = "Show Odd and Even Two Grid"
    Public Const OpenODDEvenForm As String = "Open Odd-Even Form"

    Public Const IsApplyEMIOnAssetValue As String = "Is Apply EMI On Asset Value"
    '= KUNAL =================================================================================
    Public Const AllowFutureDateTransaction As String = "AllowFutureDateTransaction"
    'KUNAL > UDIL > DATE : 16-NOV-2016
    Public Const FindNRGP_Request As String = "Show_NRGP_RequestNo"
    '========================================================================================
    Public Const AllowCSAPriceMasterPostedData As String = "Allow CSAPriceMaster Posted Data"
    Public Const AllowItemMasterPostedData As String = "Allow Item Master Posted Data"
    Public Const AllowMilkItemMasterPostedData As String = "Allow Milk Item Master Posted Data"
    Public Const AllowBulkProcItemPostedData As String = "Allow Bulk Proc Milk Item Posted Data"
    Public Const AllowPriceListMasterPostedData As String = "Allow Price List Item Posted Data"

    'Stuti
    Public Const ItemCrateWtinKg As String = "Item Default Crate Wt.(Kg.)"
    Public Const ItemJaaliWtinKg As String = "Item Default Jaali Wt.(Kg.)"
    Public Const ItemBoxWtinKg As String = "Item Default Box Wt.(Kg.)"
    Public Const ItemCrateRate As String = "Item Default Crate Rate"
    Public Const ItemJaaliRate As String = "Item Default Jaali Rate"
    Public Const ItemBoxRate As String = "Item Default Box Rate"
    Public Const ItemCanRate As String = "Item Default Can Rate"

    Public Const CustomerMasterFinderOnLocationwiseARReceipt As String = "Customer master finder location-wise on AR Receipt"

    Public Const SameuserCanNotloginmultipletimes As String = "Same user cannot login multiple times"

    Public Const ShowCancelButtonPO As String = "Show cancel button on purchase order"
    Public Const ShowOptionforSelectingCapex As String = "Show option for selecting capex code/subcode on PO"
    Public Const AutoClosePO As String = "Auto close PO when all qty. received."
    Public Const POCancel As String = "PO Cancel"
    'Public Const CreateJVForAllCasesinRGP = "Crate JV for all cases in RGP"
    Public Const StoreRequisitionMandatoryforstorerequest = "Store Requisition mandatory for store request"
    Public Const MandatoryEmployeeOnVehicleMaster = "Make employee no mandatory"
    Public Const PlantDepotMappingMandatory = "Map location of plant with depot is mandatory"
    Public Const AllowThreeFormatByDefaultForPrint = "Allow printing 3 formats by default"
    Public Const MTCapacityRequired = "MT Capacity Required"
    Public Const AllowBackDateEntry As String = "Allow back date entry for given days"
    Public Const BackDateEntryPwd As String = "BackDateEntryPwd"
    Public Const RevisedBudget As String = "Revised Budget"
    Public Const DipMarkingMendatory As String = "Make dip marking mendatory."
    Public Const AllowDispatchChecklistOnProductDispatch As String = "Allow dispatch checklist on product dispatch"
    Public Const ShowIndentBasedOnCreatedUser As String = "Show indent based on created user"
    Public Const ShowSystemStockinOpenMCC As String = "Show system stock in open MCC shift"
    Public Const Tankerfromtankersalemasteringateentry As String = "Tanker from tanker sale master in gate entry"
    Public Const ApplyMultiChamberInBulkWeighmentEntry As String = "Apply multi-chamber in bulk weighment entry"
    Public Const DefaultItemUOMForBulkSale As String = "Default item uom for bulk sale"
    Public Const InsuranceNoAndSealNoInBulkDispatch As String = "Show option for entering Insurance No and Seal No"
    Public Const ValidateFatSNFOnJobMilkSRN As String = "Validate FAT KG & SNF KG on Job Milk SRN"
    Public Const CancelDocDueToSRNReturn As String = "Cancel document due to SRN Return"
    Public Const AmountInLacsOnMisSaleRegister As String = "Allow amount in lacs on MIS SALE REGISTER"
    Public Const ShortCloseItemWiseOnPO As String = "Allow short close item wise on PO"
    Public Const MakeClosingofPOreadonlyforuser As String = "Make closing of PO read only"
    Public Const AllowModificationOnApprovalByApprovalUser As String = "Allow Modification On Approval By Approval User"
    Public Const AllowAutoCalculateADDREMOVEQty As String = "Auto Calculate Qty of Add/Remove Item"
    '-----------------end here---------------'

    Public Const FATDeductionPercent As String = "FAT Deduction Percent"
    Public Const SNFDeductionPercent As String = "SNF Deduction Percent"
    Public Const RejectionReturnPenaltyPerUnit As String = "Rejection Return Penalty Per Unit"
    Public Const RejectionDrainPenaltyPerUnit As String = "Rejection Drain Penalty Per Unit"
    Public Const GraceTimeForTransporter As String = "Grace Time For Transporter"
    Public Const GraceTimeFromGateEntryToDocWeighing As String = "Grace Time From Gate Entry To Dock Weighing"
    ''==============end here================

    ''===========CSA Sale Settings====================================================================
    Public Const ShowCSAReturnTypeOnScreen As String = "Show CSA Return Type on screen"
    Public Const ShowCSARequestScreen As String = "Enable CSA Request Instead of Booking"
    Public Const AllowSchemeOnCSADeliveryOrder As String = "Allow Scheme at CSA DO Entry"
    Public Const AllowOtherItemOnCSAPriceMaster As String = "Allow Other Items On CSA Price Master"
    Public Const AllowRoundOff_OnCSASalePatti As String = "Inv. Amount Round-off on All Sale Invoice"
    Public Const FreightChargeOnCSASaleInvoice As String = "Comm./Freight itemwise on CSA Sale Invoice"
    Public Const AllowDisabledCommissionOnCSATransfer As String = "Commission disabled on CSA Transfer"
    Public Const DoReadonly_UnitRate_AtCSASale As String = "Allow Rate readonly on CSA Sale"
    Public Const Allow_SaleMfgACONCSAPatti As String = "Allow Sale mfg. A/c on CSA Sale Patti"

    Public Const AllowSchemeItemCondONSchemeMaster As String = "Allow Scheme type item on Scheme Master"
    Public Const ForUDLOnly As String = "CSA Sale changes For UDL only"
    Public Const CheckCreditLimitonCSADO As String = "Check Credit Limit on CSA DO"
    Public Const GrossWtFromItemMasterONCSATransfer As String = "Gross Wt. from item master on CSA Transfer"
    Public Const EnableExciseONCSASalePatti As String = "Enable Excise entry on CSA Sale Patti"
    Public Const BatchSkipCSAReturn As String = "Batch Skip at CSA sale patti/Return"
    ''===========End Here====================================================================


    Public Const IsChamberWiseTanker As String = "Chamber wise Tanker"
    Public Const AllowLoginTypeCNFdistributerRetailer As String = "Allow Login Type CNF , Distributer, Retailer"

    Public Const AllowSchemeItemQty As String = "Allow Scheme Item in Materix Report"
    Public Const AllowDairyDeliveryOrderPrint As String = "Allow Print Button for Delivery Order "

    Public Const ShowSealNumberForTunkerOut As String = "Show Seal Number for Tunker Out"
    Public Const HideRateDispatchCentreCode As String = "Hide Rate and Dispatch Centre Code"
    Public Const AllowPromptPendingDocs As String = "Allow Prompt Pending Docs"
    Public Const AllowAutoGenerateDocNoInMaster As String = "Allow Auto Generate Doc No In Master Screen"
    'kunal
    Public Const ShowDocsStatusFilters As String = "Show Documents Declaration Status Filters"
    Public Const AutoDepartmentMendatroryFieldOnPurcahseCycle As String = "Allow Department Mandatory On Purchase Cycle"
    Public Const AllowVehicleGateOutValidationScrapSale As String = "Allow Vehicle Gate Out Validation For Scrap Sale"
    Public Const AllowVehicleGateOutValidationCSATransfer As String = "Allow Vehicle Gate Out Validation For CSA Transfer"
    Public Const AllowVehicleGateOutValidationSPSale As String = "Allow Vehicle Gate Out Validation For SP Sale"
    Public Const AllowVehicleGateOutValidationTransfer As String = "Allow Vehicle Gate Out Validation For Transfer"
    Public Const AllowWithoutUnitCostIssueReturnEntry As String = "Allow without amount save Issue/Return Entry"
    Public Const ZeroCostForReprocess As String = "Zero Cost For Reprocess"
    '=============Preeti Gupta========================
    Public Const IsAutoReceiptPayment As String = "Auto Receipt Payment"

    Public Const TransferEntryOnInvCtrlAccount As String = "Transfer Entry On Inventory Control Account"
    '' created by Panch Raj against Ticket No:BM00000009815 on date 23/09/2016
    Public Const AutoUpdateVLCUploaderCodeInVLCMaster As String = "AutoUpdateVLCUploaderCodeInVLCMaster"
    Public Const StandardInterfaceForMilkShiftEnd As String = "StandardInterfaceForMilkShiftEnd"
    Public Const ShiftEndAllowManualEntryOfDeduction As String = "Allow Manual Entry Of Deduction"

    Public Const PTMRatePerLtrKGOnStdQty As String = "Rate Ltr/KG On Std Qty"

    'for mobile app cash payment
    Public Const DefaultBank = "Default Bank for Cash Payment"
    Public Const DefaultLocation = "Default Location for Cash Payment"

    '=================added by preeti Gupta 03/10/2016====================
    Public Const ShowParticluarColumnInSalesRegisterForGopalJee As String = "Show Column in sale register report for GopalJee"

    ''Added by Nazia
    Public Const ShowPrintDiscountInDairyDispatchForGopaljee As String = "Show print discount in Dairy Dispatch"
    Public Const MilkReceiptRequiredApproval As String = "Milk Receipt Required Approval"

    Public Const LinkDepartmentBetweenIndentAndIssue As String = "Link Department Between Indent And Issue"

    Public Const CombineExportImportOnSchemeMaster As String = "Combined Export/Import on Scheme Master Dairy"

    Public Const OpenPOforRejectShortageQty As String = "Open PO for Reject/Shortage Qty"
    Public Const AutoSelectMCCRouteVLC As String = "Auto Select MCC Route VLC"

    Public Const PickServerDateWithNoChange As String = "Pick server Date With No Change"
    Public Const PickFinishedItemasBatchItem As String = "Finish Item as BatchItem default on Item Master"
    Public Const ToleranceFixFor_RM_OT_TRADE As String = "Tol.% mandatory for RM,Other,Trade on Item Master"

    Public Const ConsiderAdvancePayment As String = "Consider Advance Payment"
    Public Const PayableAmountZeroForMCCSale As String = "Payable Amount Zero For MCC Sale"
    Public Const Allow_AmountTruncate_BulkMilkSRN As String = "Allow truncate amount on Bulk Milk SRN"
    Public Const AutoPurchaseReturnFromIssueReturn As String = "Auto Purchase Return from Issue/Return screen."
    ''======Sanjeet============
    Public Const ShowAlternateVechileforFreshSale As String = "Gate pass with alternate vechile for fresh sale"
    Public Const ProcessProductionIssue As String = "Allow Journal Entry on Process Producion Issue"

    Public Const GSTApplicable As String = "Allow GST Applicable"

    Public Const GSTApplicableDate As String = "Allow GST Applicable Date"
    Public Const AllowPanNoValidation As String = "Allow PAN No Validation"

    Public Const GSTActiveTaxesRatesGroup As String = "Show only Active Taxes/Rates/Groups for GST"

    Public Const AllowManualRejectionOfTanker As String = "Allow Manual Rejection Of Tanker"

    Public Const RunBulkProcOnAdjustedFATCLR As String = "Run Bulk Proc on adjusted FAT and CLR"
    Public Const BulkProcNetWeightCalculationWithVendorWeight As String = "Bulk Proc NetWeight Calculation by Vendor Weight"

    Public Const BulkProcPriceChartStandardRateWithZero As String = "Bulk Proc Price Chart standard rate with zero"

    Public Const RemoveForceAapprovalofBulkSRN As String = "Remove Force Approval of Bulk SRN"

    Public Const Allow_Plant_Depot_MCC_typeLocation As String = "Allow Plant Depot MCC type Location"

    Public Const ValidateCustomerPANwithName As String = "Allow Validate Customer PAN with Name"
    Public Const ValidateTaxGroupForTransaction As String = "Allow Validate Tax Group Should Not Blank"
    Public Const AllowSeprateSchemeItemPrintDairySaleInvoice As String = "Allow Seprate Scheme Item Print DairySaleInvoice"
    Public Const EnableHirerachyCostCentre As String = "Enable Hirerachy Level Cost Centre"
    Public Const EnableStoreCostCentre As String = "Enable Store Cost Centre"
    Public Const EnableCostingMethod As String = "Enable Costing Method"
    Public Const ShowAllCustomerOnMccMaterialSale As String = "Show All Customer On MCC Material Sale"
    Public Const ShowDefaultUser As String = "Show Default User"
    '(UDL)17/11/2016
    Public Const ShowVatSeriesNoSeprately As String = "Allow Tax Tracking to Show Vat series No Seperatly"

    '(UDL)21/12/2016
    Public Const AllowToGenerate_NEFTUPLOADER As String = "Allow Generate New NEFT UPLOADER File"

    '(UDL)05/01/2017
    Public Const AllowBulkPostingofAllDocuments As String = "Allow Bulk Posting of All Documents"
    '(UDL)10/01/2017
    Public Const AllowSameaAdditionalChargesMultiTime As String = "Allow Same Additaional Charges Multiple time"
    '(01/02/2017)
    Public Const AllowToSaveAndUpdatePasswordBased As String = "Allow Masters To Save and Update Pasword Based"
    Public Const AllowMasterModificationWithSecurity As String = "Allow Master Modification With Security"
    '(02/02/2017)
    Public Const ApplyRTGSAmtMoreThanGiven As String = "Apply RTGS Amount More Than Given"
    ''============================================
    Public Const GenerateSecondryCode As String = "Excise Secondary Series on Transfer"

    '============
    Public Const POWeighmentManual As String = "Manual Weighment"

    ''======Ravi============
    Public Const AddTypeForUserMaster As String = "Add Type(Super User, Driver) in UserMaster"
    Public Const AddParavetEmployeeType As String = "Add Type Paravet in Employee Type"
    Public Const CalculateFIFOAndLIFOCosting As String = "Calculate FIFO And LIFO Costing"
    Public Const AllowDeductionPercentOnIncoming As String = "Allow Deduction(%) on Incoming Quality"
    Public Const AllowLoginType As String = "Allow POS Functionality in ERP"

    Public Const MilkProcurementUploader As String = "Milk Procurement Uploader"

    Public Const TankerDispatchBulkUploader As String = "Bulk Tanker Uploader"

    Public Const EmptyCanWeight As String = "Empty Can Weight"
    Public Const MinuteInLastVehicleForGateEntry As String = "Minute Last Vehicle For Gate Entry"
    Public Const MinuteGateEntryToGrossWeight As String = "Minute Gate Entry To Gross Weight"
    Public Const MinuteGrossWeightToTareWeight As String = "Minute Gross Weight To Tare Weight"
    Public Const NoOfDaysForMultiInceForSameVSPForSamePayCycle As String = "NoOfDaysForMultiInceForSameVSPForSamePayCycle"

    Public Const PurchaseCounterOnTransactionType As String = "Purchase Counter On Transaction Type"
    Public Const BulkProcurementCounterOnEntryType As String = "Bulk Procurement Counter On Entry Type"

    Public Const StopForRepeatedFATSNF As String = "Stop Repeat FAT SNF"
    Public Const SampleFONTSize As String = "Font Size"
    Public Const SMSPrefix As String = "SMS Prefix"

    Public Const PickPendingMilkSRNinNextPaymentCycle As String = "Pick Pending Milk-SRN in Next Payment Cycle"

    '======================Preeti Gupta[29/12/2016]===========================
    Public Const TreatChequeClearDateAsRecoDate As String = "TreatChequeClearDateAsRecoDate"
    '========================END========================================
    '======================Preeti Gupta[29/12/2016]===========================
    Public Const BookWreckageFromSublocationOrSection As String = "BookWreckageFromSublocationOrSection"
    '========================END========================================
    Public Const StopVSPBillIfSomethingWrong As String = "Stop VSP Bill If Something Wrong"
    '===========================added by preeti gupta[03/01/2016]
    Public Const PDCSetting As String = "PDC Setting"
    Public Const AllowRoadPermitNo As String = "AllowRoadPermitNo"
    Public Const ShowMessgForTDS As String = "ShowMessgForTDS"
    Public Const IsShowTreeView As String = "IsShowTreeView"
    Public Const ShowVLCUploaderData As String = "Show VLC Uploader Data"
    '========================added parteek 09/01/2017
    Public Const FatSnfWhenMilktypeSelect As String = "Fat Snf persentage allow When Milk Type Select"
    Public Const DairyFreshTaxableandNonTaxable As String = "Taxable and Non-Taxable Item"
    Public Const SMSEMailPassword As String = "SMS EMail Password"

    Public Const CreateNewDocumentOnUploader As String = "Create New Document On Uploader"
    Public Const PopupJE As String = "Popup JE"
    'KUNAL > DATE : 23-01-2017 > CLIENT : Sahayog Dairy
    Public Const ShowAliasNames As String = "ShowAliasNames"
    Public Const ShowFatAndSnfPercentageFields As String = "ShowFatNSNFPerc"
    'KUNAL > DATE : 24-01-2017 > CLIENT : Sahayog Dairy
    Public Const VehicleFitnessAndInsuranceFields As String = "VehicleFitnessFields"

    ''=======added Parteek 31-01-2017
    Public Const DocumentCancel As String = "Document Cancelation"
    Public Const PICancelUserPwd As String = "PI Cancel"
    Public Const DocumentCancelReturn As String = "Document Cancelation Return"
    Public Const CSADocumentCancel As String = "CSA Transfer Cancelation"



    Public Const FixVSPEMP As String = "Fix VSP EMP"
    Public Const FatSNFStockControl As String = "FatSNFStockControl"
    Public Const CheckBalanceFromInvMoveSummry As String = "CheckBalanceFromInvMoveSummry"
    Public Const ItemwiseFatSNFStockControl As String = "ItemwiseFatSNFStockControl"

    Public Const SepratePriceChartForCowMilk As String = "Seprate Price Chart For Cow Milk"
    Public Const ApplyStdFATSNFRate As String = "Apply Standard FAT SNF Rate"
    '===================Added by preeti gupta[20/02/2017]===============
    Public Const AllowRoundInFixedAsset As String = "Allow Round In Fixed Asset"
    Public Const AllowDecimalInFixedAsset As String = "Allow Decimal In Fixed Asset"
    '===================================================================
    Public Const OpenPriceChartPlanningScreenOnTotalSolid As String = "Open Price Chart Planning on Total Solid"
    Public Const AllowZeroQtyFATSNFInOpenMCCShift As String = "Allow Zero Qty FAT SNF In Open MCC Shift"
    Public Const AllowZeroQtyFATSNFInCloseMCCShift As String = "Allow Zero Qty FAT SNF In Close MCC Shift"
    ''=============Parteek Added setting 03-03-2017
    Public Const POLimit As String = "POLimit"
    Public Const RequiredPOLimit As String = "RequiredPOLimit"
    Public Const UnitCostIncreasePurchaseInvoice As String = "UnitCostIncreasePurchaseInvoice"
    Public Const PromptMsgForPendingDocIntervel As String = "Prompt Messg For Pending Doc Intervel"
    Public Const UDLPurchaseOrderthroughAP As String = "UDL Purchase Order through AP invoice"
    Public Const UpdateInventorySummaryTable As String = "UpdateInventorySummaryTable"

    Public Const CreateConsumeEntry As String = "Create Consume Entry"
    Public Const ShowOptionforSelectingCapexForFA As String = "Showoptionforselectingcapexcode/subcodeonFA"
    Public Const UDLCapexAcquisionEntry As String = "UDL Capex for Acquision Entry"
    Public Const UDLRGPWiseDocument As String = "UDL RGP Wise Document Created"
    Public Const AllowAssetItemOnMiscSale As String = "Allow Asset Item on Misc. Sale"
    Public Const TriggerOfGLEntryForWinTable As String = "Trigger Of GL Entry For Win Table"
    'UDL DATE : 21-04-2017
    Public Const ShowRouteWiseAndVLCWiseReport As String = "ReportOfRouteAndVLCWise"
    Public Const UOMAtDiarySaleReturn As String = "UOMAtDiarySaleReturn"
    Public Const PayableAmountZeroForFarmerPayment As String = "PayableAmountZeroForFarmerPayment"
    Public Const CheckDocAmountInAPInvoiceEntry As String = "Check Doc Amount For AP Invoice Entry"
    Public Const ApplyTSPriceAtBulkSale As String = "Apply TS Price At Bulk Sale"
    'UDL > DATE : 3-MAY-2017 : CHANGING DECLARED DOCUMENT LIST TO PENDING DOCUMENT LIST OR VICE VERSA
    Public Const ShowPendingDocumentsListScreenOverDeclaredDocumentList As String = "Show Pending Documents Screen"
    Public Const MannualySetMPUploaderData As String = "MannualySetMPUploaderData"
    Public Const AllowSNFNotManditoryInBulkSale As String = "Allow SNF Not Manditory in Bulk Sale"
    Public Const VSPMPDiffrenceOnTSBasis As String = "VSP MP Diffrence On TS Basis"
    Public Const MilkProcuremntPickCLRInsteadOfSNF As String = "Milk Procuremnt Pick CLR Instead Of SNF"
    Public Const chkGSTTaxGroupValidity As String = "check GST Tax Group Validity"

    'GHO- Date : 29-Aug-2017
    Public Const ShowShipToPartyInDairyDispatch As String = "Show Ship To Party In Dairy Dispatch"
    Public Const BulkQCWithoutCLR As String = "Bulk Quality Check Without CLR"
    Public Const DOTaggingForDairySaleModule As String = "DO Tagging For Dairy Sale Module"
    Public Const AllowFractionInMCCTankerDispatchGrossQty As String = "Allow Fraction In MCC Tanker Dispatch Gross Qty"

    Public Const PurchaseModulePickFixTaxRate As String = "Purchase Module Pick Fix Tax Rate"


    Public Const TankerDispatchFinancialImpactInTransferIn As String = "Tanker Dispatch Financial Impact In Transfer In"

    Public Const ConvertQtyIntoKG = "Convert Qty into KG Bulk Sale Dispatch"
    Public Const GSTExemptedAmountForNonRegisteredVendor As String = "GST Exempted Amount For Non Registered Vendor"
    Public Const IncreaseCrateQtyOnFiftyPercent As String = "Increase Crate Qty On Fifty Percent"


    Public Const FATSNFDeductionMixMilkFATMinValue As String = "FAT SNF Deduction Mix Milk FAT Min Value"
    Public Const FATSNFDeductionMixMilkFATMaxValue As String = "FAT SNF Deduction Mix Milk FAT Max Value"
    Public Const FATSNFDeductionMixMilkSNFMinValue As String = "FAT SNF Deduction Mix Milk SNF Min Value"
    Public Const FATSNFDeductionMixMilkSNFMaxValue As String = "FAT SNF Deduction Mix Milk SNF Max Value"
    Public Const FATSNFDeductionMixMilkDeductionPer As String = "FAT SNF Deduction Mix Milk Deduction Per"

    Public Const RoundOffPaiseAmount As String = "Round Off Paise Amount"
    Public Const EnableInternalTransfer As String = "Enable Internal Transfer for UDL"
    Public Const FreightProvisionAccount As String = "Freight Provision Account"
    Public Const TreatUnregisteredVendorAsRegisteredVendor As String = "Treat Unregistered Vendor As Registered Vendor"
    Public Const RecreateConsumptionEntry As String = "RecreateConsumptionEntry"
    Public Const BankRecoHidePWD As String = "Bank Reco Hide PWD"
    Public Const EnableItemGroupGLMapping As String = "Enable Item Group GL Mapping"
    Public Const EnableRackBin As String = "Enable Rack Bin Item"
    Public Const ChangeVehicleOnDairySaleBooking = "Change Vehicle On Dairy Sale Booking"
    Public Const VendorSetOffDayWise = "Vendor Set Off Day Wise"
    Public Const ReadOnlyTemplateFieldsOnAcqusition As String = "ReadOnlyTemplateFieldsOnAcqusition"
    Public Const IsAutoStartReading As String = "IsAutoStartReading"
    Public Const AddHighSecurityOnWeighingIntegratedScreen As String = "Add High Security On Weighing Integrated Screen"
    Public Const HighSecurityStableSeconds As String = "High Security Stable Seconds"
    Public Const HighSecurityWeightTolerance As String = "High Security Weight Tolerance"

    Public Const AllowManualvehicleOnDairyBooking As String = "AllowManualvehicleOnDairyBoking"
    Public Const FreeIndentQtyAfterPOClose As String = "Free Indent Qty After PO Close"
    Public Const ShowFATSNFinPaymentProcess As String = "Show FAT SNF in Payment Process"
    Public Const MaxRowsInCSVExport As String = "MaxRowsInCSVExport"
    Public Const MaxRowsInExcelExport As String = "MaxRowsInExcelExport"
    Public Const BigValidity As String = "Big Validity"
    Public Const AllowAssetBookChangeInTemplate As String = "AllowAssetBookChangeInTemplate"
    Public Const AllowSMSSendtoSalePerson As String = "Allow SMS Send to Sale Person"
    Public Const AllowSMSwhenCustomerCreditLimit As String = "SMS when Customer Credit limit reaches on DO."
    Public Const EnableScreenSelection As String = "Enable Screen Selection"
    Public Const SkipJobWorkSRNInPI As String = "Skip JobWork SRN in PI"
    Public Const ShowFatSnfAfterApproval As String = "Show Fat/Snf After Approval"
    Public Const ApplyTotalSolidPriceChart As String = "Apply Total Solid Price Chart"
    Public Const RequiredMgmtApprovalForRateIncrease As String = "Required Mgmt Approval For Rate Increase"
    Public Const AutoRoundOffSeprateAccountOnVendorTransaction As String = "Auto Round Off Seprate Account on Vendor Trans"
    '=================Added by preeti Gupta Against Ticket No[ADV/17/05/18-000032]===================================
    Public Const TreatCRATEAsItems As String = "Treat CRATE as Item"
    Public Const TreatCANAsItems As String = "Treat CAN as Item"
    Public Const DoNotShowDairyTypeItems As String = "Do not show dairy type items"
    '=============================================================================================
    Public Const PasswordRules As String = "Password Rules"

    Public Const AlwaysVSPDefaulter As String = "Always VSP Defaulter"
    Public Const RejectedMilkSendToRejectLocation As String = "Rejected Milk Send To Reject Location"
    Public Const NoOfPreNxtDayToPickAvgFATSNF As String = "No Of Pre Nxt Day To Pick Avg FAT SNF %"

End Class


Public Class clsFixedParameterCode
    Public Const AllowCratePhysicalStock = "AllowCratePhysicalStock"
    Public Const EnableAutoDocNoShipToLocation = "Enable Auto DocNo ShipToLocation"
    Public Const ChangeFATCLRafterspecialApprovalonQC = "Change FAT CLR after special approval on QC"
    Public Const CreateCommonSeriesLocationwiseForAllSale = "Create Common Series Location Wise For All Sale"
    Public Const EnableCustomerPODetailonDairyBooking = "EnableCustomerPODetailonDairyBooking"
    Public Const CreateCommonDairyDispatchforFreshAmbient = "CreateCommonDairyDispatchforFreshAmbient"
    Public Const CreateSeperateTaxInvForFOCIteminNonTaxdispatch = "CreateSeperateTaxInvForFOCIteminNonTaxdispatch"
    Public Const CreateSeperateSeriesforRefDocARinvforCreditdebit = "CreateSeperateSeriesforRefDocARinvforCreditdebit"
    Public Const CreateSeperateSeriesforRefDocAPinvforCreditdebit = "CreateSeperateSeriesforRefDocAPinvforCreditdebit"
    Public Const AllowUniqueNoOnMilkTransferInandTankDis = "Allow UniqueNo On Milk TransferIn and Tanker Dispatch"
    Public Const DoNotConsiderCustomerCreditLimit = "Do not Consider Customer Credit Limit"
    Public Const AllowVSPMasterAutoPrefix = "Allow VSP Master Auto Prefix"
    Public Const AllowBulkProcTransDateSameasGateEntryDate = "Allow BulkProc TransDate Same as GateEntry Date"
    Public Const AllowDifferentStateofChildCustomerOnPS = "Allow Different State of Child Customer On PS"
    Public Const AllowProvisionknokoffOnAPInvoice = "Allow Provision knokoff On APInvoice"
    Public Const AllowTransactionFiltersOnCustomerlegder = "AllowTransactionFiltersOnCustomerlegder"
    Public Const AllowtoSHOWParentChildCustomer = "AllowtoSHOWParentChildCustomer"
    Public Const AllowtoMakeApplyDocOnbyDefault = "AllowtoMakeApplyDocOnbyDefault"
    Public Const CreateProvisionJournalEntryForSale = "Create Provision JournalEntry Sale"
    Public Const ShowDairySaleModuleOnBulkPosting = "Show DairySale Module On BulkPosting"
    Public Const ShowGateEntryTypeonGateEntryBulkProc = "Show GateEntryType on GateEntry BulkProc"
    Public Const AllowAutoBulkMilkSRNonWeighmentBulkProc = "Allow Auto BulkMilkSRN on Weighment BulkProc"
    Public Const CheckParameterRangerProcurementTypewise = "Check ParameterRanger ProcurementType wise"
    Public Const PickCorrectionFactorProcurementTypewise = "Pick CorrectionFactor ProcurementType wise"
    Public Const CalculateTaxRatefromItemwsieTaxOnSale = "CalculateTaxRatefromItemwsieTaxOnSale"
    Public Const AllowBulkProcMCCwithoutTankerDispatch = "Allow Bulk Proc MCC without Tanker Dispatch"
    Public Const AllowJobWorkonGateEntryBulkProc = "Allow Job Work ON Gate Enty BulkProc"
    Public Const ApplyMaTransRateOnMultChamberTankerDis = "ApplyMaTransferRateOnMultilpeChamberTankerDispatch"
    Public Const AllowManualPriceONBulkPO = "Allow Manual Price ON Bulk PO"
    Public Const AllowReverseUnpost = "Allow Reverse and Unpost"
    Public Const AllowtoSetNoOfTransactionsforSetOff = "AllowtoSetNoOfTransactionsforSetOff"
    Public Const AllowtoSetReceiptAmountForCashTransaction = "AllowtoSetReceiptAmountForCashTransaction"
    Public Const AllowtoSetPaymentAmountForCashTransaction = "AllowtoSetPaymentAmountForCashTransaction"
    Public Const AllowtoShowCreditBalanceonCustomerAgeing = "Allow to Show Credit Balance"
    Public Const AllowtoShowDebitBalanceonVendorAgeing = "Allow to Show Debit Balance"
    Public Const AllowtoSetoffDocDateWise = "AllowtoSetoffDocDateWise"
    Public Const AllowtoSkipJournalEntryofPaymentandReceiptforAD = "AllowtoSkipJournalEntryofPaymentandReceiptforAD"
    Public Const AllowtoSkipfunctionalityafterSRNOnBulkProcurement = "AllowtoSkipfunctionalityafterSRNOnBulkProcurement"
    Public Const AllowtoNegativeStockInventoryAtTankerDispatch = "AllowtoNegativeStockInventoryAtTankerDispatch"
    Public Const AllowtoEmployeeSalaryIntegration = "AllowtoEmployeeSalaryIntegration"
    Public Const AllowtoFutureDateTransForPDCCheque = "AllowtoFutureDateTransForPDCCheque"
    Public Const WeightOfCanForCanSale = "Weight Of Can For Can Sale"
    Public Const RunBulkProcWithoutMilkGrade As String = "Run Bulk Proc without Milk Grade"
    Public Const DaysToStartAutoLock As String = "Days ToStart AutoLock"
    Public Const AllowRandomOnlyOneSecondaryQC As String = "Allow Random OnlyOne SecondaryQC"
    Public Const AllowGateEntryAgainstPO As String = "Allow GateEntry Against PO"
    Public Const SeparateDairyDispatchTaxableNonTaxable As String = "Separate DairyDispatch TaxableNon Taxable"
    Public Const RunBatchFifowise As String = "Run Batch Fifowise"
    Public Const PromptTimeToPostTransactions As String = "Prompt Time ToPost Transactions"
    Public Const CreateFreshInvoiceOnDispatchSave As String = "Create FreshInvoice OnDispatch Save"
    Public Const AllowLockTransactionUserwise As String = "AllowLockTransactionUserwise"
    Public Const AllowAutoLockTransaction As String = "AllowAutoLockTransaction"
    Public Const AllowDefaultBankCodeforCreditNote As String = "AllowDefaultBankCodeforCreditNote"
    Public Const AllowCreditNoteWithoutReference As String = "AllowCreditNoteWithoutReference"
    Public Const AllowUseApplyDocSeriesForReceipt As String = "AllowUseApplyDocSeriesForReceipt"
    Public Const AllowUseApplyDocSeriesForPayment As String = "AllowUseApplyDocSeriesForPayment"
    Public Const AllowCreditNoteWithoutReferenceonAP As String = "AllowCreditNoteWithoutReferenceonAP"
    Public Const AllowBranchAcconReceiptPrint As String = "AllowBranchAcconReceiptPrint"
    Public Const SecurityDocumentKnockOffonReceipt As String = "SecurityDocumentKnockOffonReceipt"
    Public Const AllowFreshInvoiceAutoPost As String = "AllowFreshInvoiceAutoPost"
    Public Const AllowReceiptThroughSO As String = "AllowReceiptThroughSO"
    Public Const AllowSetOffUntilTransactionsnotend As String = "AllowSetOffUntilTransactionsnotend"
    Public Const AllowGateReturn As String = "AllowGateReturn"
    Public Const CalculateCommOnCSATransWOConversion As String = "CalculateCommOnCSATransWOConversion"
    Public Const AllowSRNWithoutShortageRejection As String = "AllowSRNWithoutShortageRejection"
    Public Const AllowPurchaseModulewithUniqueItem As String = "AllowPurchaseModulewithUniqueItem"
    Public Const GrossWeightUnit As String = "GrossWeightUnit"
    Public Const ExpiryDaysBulkProcurementPriceChart As String = "ExpiryDays BulkProcurement PriceChart"
    Public Const ShowSchemeItemRateonDairyDispatch As String = "Show Scheme Item Rate on Dairy Dispatch"
    Public Const AutoCalculateCrateOnDairyDispatch As String = "Auto Calculate Crate on Dairy Dispatch"
    Public Const GrossWtFromItemMasterONProductSale As String = "Gross Wt. from item master on Product Sale"
    Public Const AllowFreshPriceChartOnBookingProductSale = "Allow Fresh Price Chart on Booking PS"
    Public Const CreateVatSeriesForProductExciseinvoice = "Create Vat Series for PS Excise invoice"
    Public Const AllowFreshPriceChartOnProductSale = "Allow Fresh Price Chart on Product Sale"
    Public Const ShowUnloadingandWeighmentSequencewise = "Show Unloading Weighment sequence wise"
    Public Const ShowBothTankertypeOnCleaning = "Show Both Tanker Type on Cleaning"
    Public Const isCleaningMandatoryBeforeGateout = "Is Cleaning Mandatory before Gate Out"
    Public Const AllowBulkProcurementSequencewise = "Allow Bulk Procurement Sequence wise"
    Public Const ShowItemLocationWiseonDairyBooking = "Show Item Location wise on Dairy Booking"
    Public Const CheckOutstandingCreditLimitOnBooking = "Check Customer Outstanding on Booking"
    Public Const AllowBulkPriceChartMultiplepriceToMultipleVendor = "Allow BulkPrice Multiple Price to Mult Vendor"
    Public Const isItemMilkType As String = "Is Item Milk Type"
    Public Const isPriceChartGradeWise As String = "Is Price Chart Grade Wise"
    Public Const isFarmerPaymentCycle As String = "is Farmer Payment Cycle"
    Public Const GateEntryTankerFromTankerMaster As String = "Gate Entry tanker From Master"
    Public Const QualityThenWeighmentinBulkProcurement As String = "First QC then Weighment"
    Public Const isIntimationRequired As String = "Show Intimation Screen"
    Public Const ShowOptionOnItemMasterChangeItemRate = "Show option on Item Change Rate on DDispatch"
    Public Const SHowOptionOnLocationForDairyDispatchfromDOorGatepass = "Show option on loc For DDispatch from DO/GP"
    Public Const showPostrequiredforBulkSale = "showPostrequiredforBulkSale"
    Public Const ApplyDocumentDate = "Apply Document Date"
    Public Const AllowStockCheckatDOLevel = "AllowStockCheckatDOLevel"
    Public Const AllowAdditionalWeightinPercentage = "AllowAdditionalWeightinPercentage"
    Public Const EnterAdditionalWeight = "EnterAdditionalWeight"
    Public Const AllowTankerBasedonVendorofGE = "Allow Tanker Based on Vendor of GE"
    Public Const AllowUseBoilingParameteronParameterMaster = "Allow Use Boiling Parameter on Parameter Master"
    Public Const DairyDispatchFromDeliveryNote = "Dairy Dispatch From Delivery Note"
    Public Const ItemTypeForDairyBooking = "Item Type Fresh or Ambient For Dairy Booking"
    Public Const AllowStockToleranceNegative = "Allow Stock Tolerance Negative"
    Public Const StockToleranceLimit = "Stock Tolerance Limit"
    Public Const ShowAllMenu As String = "Show All Menu"
    Public Const POSeriesWithoutItemTypewise As String = "POSeriesWithoutItemTypeWise"
    Public Const WorkApprovalFlowInERP As String = "Work Approval Flow in ERP"
    Public Const BOOKINGFINDER_ON_CSASALEPATTI As String = "CSA Sale Patti With Booking Knock-off"
    Public Const OpenAvailorEmptyStckLocationOn_Standardization As String = "Open Avail./Empty Location on Standardization"
    Public Const BOM_Amend_Pswd As String = "Amendment Password for BOM"
    Public Const ProductionFATSNF_KG_Unit As String = "ProductionFATSNF_KG_Unit"
    Public Const ChangeRateAT_CSA_Return As String = "Rate Change At CSA Transfer Return"
    Public Const VehicleCapacityUnit As String = "VehicleCapacityUnit"
    Public Const StopGLEntryForConsignmentAtCSATransfer As String = "Stop GL Consignment at CSA Transfer"
    Public Const CSATransfer_SalePatti_All_Tax_Open As String = "Open All Tax Mapped With Location In CSA"
    Public Const Emps1 As String = "Emps1"
    Public Const Agency As String = "Agency"
    Public Const UploaderPassword As String = "Uploader Password"
    Public Const CreateGLForTransfer As String = "CreateGLForTransfer"
    Public Const Category1 As String = "Category1"
    Public Const AllowDesignAtRunTime As String = "AllowDesignRunTime"
    Public Const Category2 As String = "Category2"
    Public Const Category3 As String = "Category3"
    Public Const SkipDiffGLOnPI As String = "SkipDiffGLOnPI"
    Public Const A As String = "A"
    Public Const SkipCogsEntry As String = "SkipCogsEntry"
    Public Const HY As String = "HY"
    Public Const M As String = "M"
    Public Const Q As String = "Q"
    Public Const L As String = "L"
    Public Const R As String = "R"
    Public Const U As String = "U"
    Public Const POWITHREQ As String = "POWITHREQ"
    Public Const DisplayReasonOnDelete As String = "DisplayReasonOnDelete"
    Public Const DisplayReasonOnUpdateAfterPost As String = "DisplayReasonOnUpdateAfterPost"
    Public Const Importbulkdatafromexcelsheet As String = "Importbulkdatafromexcelsheet"
    Public Const Distributor As String = "Distributor"
    Public Const POPUPITEMREORDERLEVEL As String = "POPUPITEMREORDERLEVEL"
    Public Const EMP02 As String = "EMP02"
    Public Const Driver As String = "Driver"
    Public Const ZM As String = "ZM"
    Public Const TSM As String = "TSM"
    Public Const ASM As String = "ASM"
    Public Const Emps2 As String = "Emps2"
    Public Const EMP3 As String = "EMP3"
    Public Const ALLOW As String = "ALLOW"
    Public Const BASIC As String = "BASIC"
    Public Const BONUS As String = "BONUS"
    Public Const COEPS As String = "COEPS"
    Public Const COESI As String = "COESI"
    Public Const COPF As String = "COPF"
    Public Const DA As String = "DA"
    Public Const DEDUCT As String = "DEDUCT"
    Public Const EMPESI As String = "EMPESI"
    Public Const MaxRowsForQuickExport As String = "Q-EXP-MX-RW"
    Public Const ShowPurchaseControlAc As String = "ShowPurchaseControlAc"
    Public Const CreateTransferInGL As String = "CreateTransferInGL"
    Public Const CreateTankerDispatchGL As String = "CreateTankerDispatchGL"
    Public Const PostTankerDispatchWithZeroAvgCost As String = "PostTankerDispatchWithZeroAvgCost"
    Public Const EPF As String = "EPF"
    Public Const HRA As String = "HRA"
    Public Const LOAN As String = "LOAN"
    Public Const OT As String = "OT"
    Public Const OTHER As String = "OTHER"
    Public Const RMBT As String = "RMBT"
    Public Const TA As String = "TA"
    Public Const TDS As String = "TDS"
    Public Const Conveyance As String = "Conveyance"
    Public Const LaborUnFairFund As String = "LUF"
    Public Const LaborWelFairFund As String = "LWF"
    Public Const AllowSkippingPrevDocumentsOnPaymentProcess As String = "AllowSkippingPrevDocumentsOnPaymentProcess"
    Public Const PrefixGenerator As String = "PrefixGenerator"
    Public Const DuplicateRoute As String = "Duplicate Route"
    Public Const RJ As String = "RJ"
    Public Const EMP01 As String = "EMP01"
    Public Const SIRevers As String = "SIRevers"
    Public Const SIReversAndCreate As String = "SIReversAndCreate"
    Public Const GEUpdateAfterPost As String = "GEUpdateAfterPost"
    Public Const GEUpdatePriceChart As String = "GEUpdatePriceChart"
    Public Const SetCSATransferwithZeroOnSalePatti As String = "SetCSATransferwithZeroOnSalePatti"
    Public Const POAmendment As String = "POAmendment"
    Public Const BulkInvoiceDelete As String = "BulkInvoiceDelete"
    Public Const BulkSaleSequence As String = "BulkSaleSequence"
    Public Const BulkQCTableHavingUniqueKey As String = "BulkQCTableHavingUniqueKey"
    Public Const SrPath As String = "SrPath"
    Public Const TempProvisional As String = "TempProvisional"
    Public Const LoadInRollback As String = "LoadInRollback"
    Public Const Sunday As String = "Sunday"
    Public Const Monday As String = "Monday"
    Public Const Tuesday As String = "Tuesday"
    Public Const Wednesday As String = "Wednesday"
    Public Const Thursday As String = "Thursday"
    Public Const Friday As String = "Friday"
    Public Const Saturday As String = "Saturday"
    Public Const o As String = "0"
    Public Const EnableMilkProc As String = "EnableMilkProc"
    Public Const LCCreationPwd As String = "LCCreationPwd "
    Public Const ShowQtySum_in_GRN_MRN_SRN As String = "ShowQtySum_in_GRN_MRN_SRN"

    Public Const SalesInvoice As String = "Sales Invoice"
    Public Const LOReceiptDefaultBankForSettlement As String = "Default Bank For Settlement"
    Public Const LOReceiptPaymentTypeForSettlement As String = "Default Payment Type For Settlement"
    Public Const ALLOWANYBO As String = "Allow Any Type of BO"
    Public Const ALLOWCBOSBO As String = "Allow Child and SubChild BO"
    Public Const PROVISIONENTRYONSTOCKTRANSFER As String = "ProvisionOnStockTransfer"
    Public Const INDUSTRYTYPE As String = "Industry Type"
    Public Const Transfer As String = "Transfer"
    Public Const DefaultTypeFC As String = "FC"
    Public Const DefaultTypeFB As String = "FB"
    Public Const DefaultTypeSH As String = "SH"
    Public Const SalesmanPhysicalLocation As String = "SPL"
    Public Const IndentTolerence As String = "IndentTolerence"
    Public Const AskForDate As String = "AskForDate"
    Public Const PickMachineDateForTran As String = "PickMachineDateForTran"
    Public Const ReqLimitOnSRN As String = "ReqLimitOnSRN"
    Public Const AutoLoadinFromLocation As String = "AutoLoadinFromLocation"
    Public Const CrreateTransferShipmentJE As String = "CrreateTransferShipmentJE"
    Public Const IsNotIncludeWasteQtyInCal As String = "IsNotIncludeWasteQtyInCal"
    Public Const IsConsiderOutTypeDocForBalance As String = "IsConsiderOutTypeDocForBalance"
    Public Const BankTransferRunPaymentCounter As String = "BankTransferRunPaymentCounter"
    Public Const PaymentReceiptTypeRunReceiptCounter As String = "PaymentReceiptTypeRunReceiptCounter"
    Public Const CounterFinancialYearStyle As String = "CounterFinancialYearStyle"
    Public Const LinkFinancialYearStyleWithGSTDate As String = "LinkFinancialYearStyleWithGSTDate"
    Public Const CashDiscountFromClaimMaster As String = "CashDiscountFromClaimMaster"
    Public Const TransferTransTypeRouteHide As String = "TransferTransTypeRouteHide"
    Public Const AllowNegtiveOfSaleInvoiceBlanceAmt As String = "AllowNegtiveOfSaleInvoiceBlanceAmt"
    Public Const SalesRateEditable As String = "Sales Rate Editable"
    Public Const RunDemoERP As String = "RunDemoERP"
    Public Const IsKDIL As String = "IsKDIL"
    Public Const SendToTally As String = "SendToTally"
    Public Const PromptForTally As String = "PromptForTally"
    Public Const CurrentMaufacturingType As String = "ManufacturingType"
    Public Const TallyCompany As String = "TallyCompany"
    Public Const TallyIP As String = "TallyIP"
    Public Const TallyPort As String = "TallyPort"
    Public Const TaxRoundOffToZeroDecimalPlace As String = "TaxRoundOffToZeroDecimalPlace"
    Public Const BalanceSheetProftAndLossGroupCode As String = "BalanceSheetProftAndLossGroupCode"
    Public Const BalanceSheetProftAndLossGroupDesc As String = "BalanceSheetProftAndLossGroupDesc"
    Public Const ApplyCostingOnPostedDate As String = "ApplyCostingOnPostedDate"
    Public Const isBatchApplyOnInventoryMovement As String = "isBatchApplyOnInventoryMovement"
    Public Const BlankDatabase As String = "BlankDatabase"
    Public Const ServiceDealer As String = "Service Dealer"
    Public Const TDM As String = "TDM"
    Public Const MAILOFF As String = "MAILOFF"
    '==added by shivani
    Public Const AllowToSaveTimeWithDocumentDate As String = "Allow To Save Time With Document Date"
    Public Const AllowToPrintTimeWithDocumentDate As String = "Allow To Print Time With Document Date"
    Public Const AllLevelApprovalIsMandatory As String = "All Level Approval Is Mandatory"
    Public Const AssetGroupPrefix As String = "AssetGroupPrefix"
    Public Const DepreciationCalculationMethod As String = "Depreciation Calculation Method"
    Public Const STDPURRATE As String = "STDPURRATE"
    Public Const AutoPOAtSRN As String = "AUTOPOATSRN"
    Public Const DisableShipToLocation As String = "Disable Ship_To_Location For (PO,PI,SRN)"
    Public Const PurchasePickItemFromVendorItemDetails As String = "PurchasePickItemFromVendorItemDetails"
    Public Const PurchaseOneItemOneVendor As String = "PurchaseOneItemOneVendor"
    Public Const AllowLargerItemCostThenVendorItemCost As String = "AllowLargerItemCostThenVendorItemCost"
    Public Const ShowGRN As String = "ShowGRN"
    Public Const SkipMRNGRNinCaseofMT As String = "SkipMRNGRNinCaseofMT"
    Public Const ShowMRN As String = "ShowMRN"
    Public Const EnableProjectFinder As String = "EnableProjectFinder"
    Public Const PostShipmentonAutoSTN As String = "PostShipmentonAutoSTN"
    Public Const IsRemarksMandatoryOnCloseSaleOrder As String = "IsRemarksMandatoryOnCloseSaleOrder"
    Public Const CreateInvoicewithShipmentonAutoSTN As String = "CreateInvoicewithShipmentonAutoSTN"
    Public Const AllowSingleInvoiceAgainstSingleOrder As String = "AllowSingleInvoiceAgainstSingleOrder"
    Public Const WorkingHours As String = "WorkingHours"
    Public Const TreatExcessLeaveAbsent As String = "TreatExcessLeaveAbsent"
    Public Const VehicleInsuranceAlert As String = "VehicleInsuranceAlert"
    Public Const IsItemRateEditableOnTransfer As String = "IsItemRateEditableOnTransfer"
    Public Const GLACAccordingToTaxRate As String = "GLACAccordingToTaxRate"
    Public Const AutoSchemeOn As String = "AutoSchemeOn"
    Public Const IsTransferQtyEditableOnAutoSTN As String = "IsTransferQtyEditableOnAutoSTN"
    Public Const IsItemRateEditableOnSales As String = "IsItemRateEditableOnSales"
    Public Const IsItemMRPEditableOnSales As String = "IsItemMRPEditableOnSales"
    Public Const ShowSNF9IfSNFGreaterThan9 As String = "ShowSNF9IfSNFGreaterThan9"
    Public Const IsItemRateEditableOnSalesForAprilOnly As String = "ForAprilOnly"
    Public Const Mediclaim As String = "Mediclaim"
    Public Const LTA As String = "LTA"
    Public Const Gratuity As String = "Gratuity"
    Public Const LeaveEnchashed As String = "LeaveEnchashed"
    Public Const UnpaidAmount As String = "UnpaidAmount"
    Public Const Arrear As String = "Arrear"
    Public Const PT As String = "PT"
    Public Const UserPWD As String = "UserPWD"
    Public Const AllowMilkReceiptAfterSettingsisOn As String = "AllowMilkReceiptAfterSettingsisOn"
    Public Const MilkReceiptTolerancePwd As String = "MilkReceiptTolerancePwd"
    Public Const MCCDLTPWD As String = "MCCDLTPWD"
    Public Const Allow_ExcelCode_On_Mcc As String = "Allow_ExcelCode_On_Mcc"
    Public Const Is_Allow_Cancel_Transaction As String = "Is_Allow_Cancel_Transaction"
    Public Const is_Allow_cancel_Posted_Transaction As String = "is_Allow_cancel_Posted_Transaction"

    Public Const ShiftTiming As String = "ShiftTiming"
    Public Const GetMulitcurrencyDecimalPlaces As String = "GetMulitcurrencyDecimalPlaces"
    Public Const MilkSetting As String = "MilkSetting"
    Public Const ShowTaxRateColumnOnTransaction As String = "ShowTaxRateColumnOnTransaction"

    Public Const LicenceExpiryDate As String = "IsApplyCommonService1" 'A B
    Public Const LicenceNoOfExeConnection As String = "IsApplyCommonService2" 'C
    Public Const LicenceNoOfJournalEntry As String = "IsApplyCommonService3" 'D
    Public Const LicenceNoOfUser As String = "IsApplyCommonService4" 'E

    'richa
    Public Const InvoiceManualNoWithPrefix As String = "InvoiceManualNoWithPrefix"
    Public Const AutoBackUp As String = "AutoBackUp"
    Public Const MCCPurchase As String = "MCCPurchase"
    Public Const BulkSaleDefaultMilkItem As String = "BulkSaleDefaultMilkItem"
    Public Const BSDefaultMilkItem As String = "BSDefaultMilkItem"
    Public Const DefaultRoundOffGLAccount As String = "DefaultRoundOffGLAccount"
    'richa Ticket No BM00000003045 09/07/2014
    Public Const NotificationSettingforReOrderInPO As String = "NotificationSettingforReOrderInPO"
    'richa Ticket No BM00000003042 09/07/2014
    Public Const NotificationSettingforReOrderInPurchaseRequisition As String = "NotificationSettingforReOrderInPurchaseRequisition"
    Public Const PurchaseOrderAutomaticallyItemQtyBelowReorderLevel As String = "PurchaseOrderAutomaticallyItemQtyBelowReorderLevel"
    Public Const NLevelAtVendor As String = "NLevel_Vendor"
    Public Const NLevelAtCustomer As String = "NLevel_Customer"
    Public Const NLevelAtLocation As String = "NLevel_Location"
    Public Const AutoItemNLevel As String = "NLevel_ItemCode"
    Public Const CounterRawMaterial As String = "R"
    Public Const CounterFinishGood As String = "F"
    Public Const CounterSemiFinishGood As String = "S"
    Public Const CounterTradingGood As String = "T"
    Public Const CounterAsset As String = "A"
    Public Const CounterOther As String = "O"


    Public Const Princi_Bom As String = "Principle_BOM"
    Public Const AP_INV_COMMSN As String = "AP_INV_COMMSN"
    Public Const Principal_Vendor As String = "Principal_Vendor"
    Public Const Principal_Vendor_Database As String = "Principal_Vendor_Database"
    Public Const Principal_Customer As String = "Principal_Customer"

    'Public Const ExeExpiredDate As String = "ExpiredDate"

    '' Anubhooti 10-July-2014 (BM00000002912)
    Public Const CalculateLTAOnHoliday As String = "CalculateLTAOnHoliday"
    Public Const CalculateLTAOnWeekend As String = "CalculateLTAOnWeekend"
    Public Const CalculateMediclaimOnHoliday As String = "CalculateMediclaimOnHoliday"
    Public Const CalculateMediclaimOnWeekend As String = "CalculateMediclaimOnWeekend"

    '' Anubhooti 21-Aug-2014 (Setting For Item Is_Purchaseable)
    Public Const Is_Purchaseable_Item As String = "Is_Purchaseable_Item"

    '' Anubhooti 21-Aug-2014 (Setting For Demo Print)
    Public Const Is_AbemdmentForDemo As String = "Is_AbemdmentForDemo"

    '' Anubhooti 26-Aug-2014 (Setting For Item Is_FinishedGoods)
    Public Const Is_FinishedGoods As String = "Is_FinishedGoods"

    '' Anubhooti 28-Aug-2014 (Setting For Demo Print: Purchase Module)
    Public Const ShowStatusForPurchase As String = "ShowStatusForPurchase"

    '' Anubhooti 28-Aug-2014 (Setting For Demo Print: Sales Module)
    Public Const ShowStatusForSales As String = "ShowStatusForSales"

    '' Anubhooti 28-Aug-2014 (Setting For Demo: Sales Module)
    Public Const ShowSerialNoForSales As String = "ShowSerialNoForSales"

    '' Anubhooti 02-Sep-2014 (Setting For Vendor Master)
    Public Const AutoGeneratedVendorCode As String = "AutoGeneratedVendorCode"
    Public Const AutoGeneratedVendorCodeForAllCompany As String = "AutoGeneratedVendorCodeForAllCompany"
    '' Anubhooti 02-Sep-2014 (Setting For Customer Master)
    Public Const AutoGeneratedCustomerCode As String = "AutoGeneratedCustomerCode"
    Public Const AutoGeneratedCustomerCodeForAllCompany As String = "AutoGeneratedCustomerCodeeForAllCompany"


    Public Const ApplyBrachAccounting As String = "ApplyBrachAccounting"


    '' Anubhooti 03-Sep-2014 BM00000003437 (Setting For Sub Account in Bank Master)
    Public Const AllowToUseSubAccount As String = "AllowToUseSubAccount"

    '' Anubhooti 17-Dec-2014 BM00000004959 (Setting For Withdrawal/Receipt/Both in Bank Transfer)
    Public Const InTransitFeatureIsRequired As String = "InTransitFeatureIsRequired"
    Public Const PermissionSettingForTransactionWithBank As String = "Permission_Setting_For_Trans_With_Bank"

    '' Anubhooti 12-Sep-2014 BM00000003890 (Setting For Fresh Sale)
    Public Const AllowToEnterMRPManually As String = "AllowToEnterMRPManually"

    '' Anubhooti 24-Sep-2014 BM00000003940 (Setting For Vehicle Master)
    Public Const AllowFieldsToBeManadatory As String = "AllowFieldsToBeManadatory"

    '' Anubhooti 08-Oct-2014 (Setting For Auto Generated Digits For Vendor)
    Public Const AutoGeneratedDigitsForVendor As String = "AutoGeneratedDigitsForVendor"

    '' Anubhooti 08-Oct-2014 (Setting For Auto Generated Digits For Customer)
    Public Const AutoGeneratedDigitsForCustomer As String = "AutoGeneratedDigitsForCustomer"

    '' Anubhooti 02-Dec-2014 (Setting For Unit Cost Editable/Non-Editable On SRN)
    Public Const IsRateEditableOnSRN As String = "IsRateEditableOnSRN"
    Public Const DisAllowIntermittentTankerForPlantDispatch As String = "DisAllowIntermittentTankerForPlantDispatch"
    '' Anubhooti 23-Jan-2015 (Setting For Creation of GL Acc To Item GL Account(Issue/Return/Transfer))
    Public Const CreateGLAccToItem As String = "CreateGLAccToItem"

    '' Anubhooti 29-Jan-2015 (Setting For Cost Edit/Non-Edit On(Issue/Return/Transfer))
    Public Const IsCostEditableOnIssueReturnTransfer As String = "IsCostEditableOnIssueReturnTransfer"

    Public Const UpdateCrateLinerQty As String = "UpdateCrateLinerQty"

    'Richa Agarwal 05/08/2014 Against Ticket No BM00000003248
    Public Const AllowDispatchOutstandingBS As String = "AllowDispatchOutstandingBS"
    Public Const AllowDispatchOutstandingFS As String = "AllowDispatchOutstandingFS"
    Public Const AllowDispatchOutstandingPS As String = "AllowDispatchOutstandingPS"
    Public Const IsVolumeSchemeBydefault As String = "IsVolumeSchemeBydefault"
    Public Const DiscountCodeForArAdj As String = "DiscountCodeForArAdj"
    Public Const DiscountCodeForMPAdj As String = "DiscountCodeForMPAdj"
    ''=======parteek Added 16-01-2016
    Public Const AutoRecieptBankCode As String = "AutoRecieptBankCode"
    Public Const AutoRecieptPaymentMode As String = "AutoRecieptPaymentMode"

    'Richa Agarwal 19/08/2014 Against Ticket No BM00000003110
    Public Const AllowDeliveryOrderIncaseAmountIncreases As String = "AllowDeliveryOrderIncaseAmountIncreases"
    '--------Richa Agarwal 21/08/2014 Against Ticket No BM00000003438
    Public Const AllowAutoMRNGRNonDocumentAcceptance As String = "AllowAutoMRNGRNonDocumentAcceptance"
    Public Const AllowToShowSaleTypeinPaymentTermsReceivable As String = "AllowToShowSaleTypeinPaymentTermsReceivable"
    Public Const AllowToShowMilkTypeinAdjustmentEntry As String = "AllowToShowMilkTypeinAdjustmentEntry"
    Public Const GatePassAfterTransfer As String = "GatePassAfterTransfer"
    Public Const CreateTransferFromBooking As String = "CreateTransferFromBooking"
    Public Const PickRateFromPRICEChrtMasterFORUMang As String = "PickRateFromPRICEChrtMasterFORUMang"
    Public Const IGnoreGITAccount As String = "Ignore GIT Account in Financial Entry"
    Public Const AllowToEditCategoryCodeinItemMaster As String = "AllowToEditCategoryCodeinItemMaster"
    Public Const CreditLimitApproval As String = "CustomerCreditLimit"
    Public Const ViewTDSPwd As String = "ViewTDSPwd"
    '--------Richa Agarwal 28/08/2014  Against Ticket No .BM00000003667
    Public Const InvoiceBasedPO As String = "InvoiceBasedPO"
    Public Const AdvanceAgainstSO As String = "AdvanceAgainstSO"
    ''-------------------------------------------
    Public Const Purchase_SMSATPOST As String = "SMSATPOST_PUR"
    Public Const Sale_SMSATPOST As String = "SMSATPOST_SALE"
    Public Const showRFQ As String = "showRFQ"
    ''richa 02/09/2014
    Public Const AmountLimitForInvoiceBulkSale As String = "AmountLimitForInvoiceBulkSale"
    Public Const ShowSaleInvoiceNoInPOfinderInSRN As String = "ShowSaleInvoiceNoInPOfinderInSRN"
    ''richa 09/09/2014
    '----------Updated by Preeti Gupta--------------------
    Public Const CrateValue As String = "CrateValue"
    Public Const CommitedDefaultQty As String = "CommitedDefaultQty"
    Public Const ShowBinMapping As String = "ShowBinMapping"
    Public Const ShowPrintChallanInDairyDispatch As String = "ShowPrintChallanInDairyDispatch"
    Public Const ShowCrateJaaliBoxIntransfer As String = "Show Crate Jaali & Box In transfer"
    '------------end---------------------------------------
    Public Const DefaultCorrectionFactorForBulkSale As String = "DefaultCorrectionFactorForBulkSale"
    Public Const MCCdefaultCorrectionFactorBS As String = "MCCdefaultCorrectionFactorBS"
    Public Const JOBdefaultCorrectionFactorBS As String = "JOBdefaultCorrectionFactorBS"
    Public Const PurchasedefaultCorrectionFactorBS As String = "PurchasedefaultCorrectionFactorBS"
    Public Const AllowDeliveryQtygreaterthanBookingQtyPS As String = "AllowDeliveryQtygreaterthanBookingQtyPS"
    Public Const IsPickServerDateForMultipleDispatchInvoice As String = "IsPickServerDateForMultipleDispatchInvoice"
    Public Const AutoTabOrdering As String = "AutoTabOrdering"
    Public Const AutoTabOrderingPattern As String = "AutoTabOrderingPattern"
    Public Const IsItemEditableOnMCCDispatch As String = "IsItemEditableOnMCCDispatch"
    Public Const IsUOMSelectableOnMCCDispatch As String = "IsUOMSelectableOnMCCDispatch"
    Public Const LoadLoginScreenDirectlyAfterStartup As String = "LoadLoginScreenDirectlyAfterStartup"
    Public Const IsItemWithDifferntUnitConsiderAsOtherItem As String = "ItemWithDifferntUnitConsiderAsOtherItem"
    Public Const AutoSetTabStopFalseForReadonlyControls As String = "AutoSetTabStopFalseForReadonlyControls"
    Public Const AutoRestoreGridLayout As String = "AutoRestoreGridLayout"
    Public Const IsMRPWiseBalance As String = "IsMRPWiseBalance"

    Public Const CreateDbitNoteForShortPI As String = "CreateDbitNoteForLeakAndShortPI"
    Public Const CreateDbitNoteForRejectPI As String = "CreateDbitNoteForRejectPI"
    Public Const CreateDebitNoteForUnitCost As String = "CreateDebitNoteForUnitCost"

    Public Const TransferJEForLocationMapping As String = "TransferJEForLocationMapping"
    Public Const TransferWithProductionSale_Retail_Series As String = "CreateTransferWithProductionSale_Retail_Series"
    Public Const ProductionQtyDecimalPoint As String = "ProductionQtyDecimalPoint"
    Public Const ProductionFATSNFPerDecimalPoint As String = "ProductionFATSNFPerDecimalPoint"
    Public Const ManualySelectBOMForChildBatch As String = "ManualySelectBOMForChildBatch"
    Public Const AllowToDispalyAlertForBDayAnniversary As String = "AllowToDispalyAlertForBDayAnniversary"
    Public Const AllowToSendEmailForBDayAnniversary As String = "AllowToSendEmailForBDayAnniversary"
    Public Const ItemDescForTankerDispatchPrint As String = "ItemDescForTankerDispatchPrint"
    Public Const AllowPOScheduling As String = "Allow PO Scheduling"
    Public Const AllowGateEntryInPrevDate As String = "AllowGateEntryInPrevDate"
    Public Const ERPStartDate As String = "ERPStartDate"
    Public Const AllowQcDateBeforeGateEntryDateTime As String = "AllowQcDateBeforeGateEntryDateTime"
    Public Const CreateJEForTransfer As String = "CreateJEForTransfer"
    Public Const AllowToSkipStageQLLogSheetInProd As String = "AllowToSkipStageQLLogSheetInProd"
    Public Const IsRemarkReasonMandatoryOnPO As String = "IsRemarkReasonMandatoryOnPO"
    Public Const ShowCostCenterAndHierarchyLevelInPurchaseModule As String = "ShowCostCenterAndHierarchyLevelInPurchaseModule"
    Public Const IsQCColumnRequiredonMRN As String = "IsQCColumnRequiredonMRN"
    Public Const AllowQcDateAfterCurrentDate As String = "AllowQcDateAfterCurrentDate"
    Public Const AllowWeighmentDateAfterCurrentDate As String = "AllowWeighmentDateAfterCurrentDate"
    Public Const AllowUnloadingDateAfterCurrentDate As String = "AllowUnloadingDateAfterCurrentDate"
    Public Const AllowcleaningDateAfterCurrentDate As String = "AllowcleaningDateAfterCurrentDate"
    Public Const AllowGateOutDateAfterCurrentDate As String = "AllowGateOutDateAfterCurrentDate"
    Public Const AllowSRNDateAfterCurrentDate As String = "AllowSRNDateAfterCurrentDate"
    Public Const IsRGPAfterPurchaseOrder As String = "Do RGP After Purchase Order"
    Public Const AllowQualityModuleInERP As String = "On Quality Module"
    Public Const SRNReportQuantityWise As String = "SRNReportQuantityWise"
    Public Const IsCustomerGroupFieldsMandatory As String = "IsCustomerGroupFieldsMandatory"
    Public Const IsVendorGroupFieldsMandatory As String = "IsVendorGroupFieldsMandatory"
    Public Const AllowAutoNoForBackLogEntry As String = "AllowAutoNoForBackLogEntry"
    Public Const AllowDiffentSeriesExemptedItemONPS As String = "AllowDiffentSeriesExemptedItemONPS"
    Public Const DisplayFranchiseeinCustomer As String = "DisplayFranchiseeinCustomer"
    Public Const isIdleTimerOn As String = "isIdleTimerOn"
    Public Const idleTime As String = "idleTime"
    Public Const AddressOnPaymentVoucherOnBankBasis As String = "AddressOnPaymentVoucherOnBankBasis"
    'richa agarwal 17/03/2015 against ticket no BM00000005874
    Public Const AllowBankDetailsManualinVM As String = "AllowBankDetailsManualinVM"
    ''--------------------------------
    ''RICHA AGARWAL 17/03/2015
    Public Const AllowToGenerateSaleInvoiceSeriesTaxTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesTaxTypeatPS"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesRetailTypeatPS"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseTypeatPS As String = "AllowToGenerateSaleInvoiceSeriesExciseTypeatPS"
    ''-------------------------
    ''RICHA AGARWAL 17/03/2015 MCC Sale
    Public Const AllowToGenerateSaleInvoiceSeriesTaxatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesTaxatMCCSale"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesRetailatMCCSale"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseatMCCSale As String = "AllowToGenerateSaleInvoiceSeriesExciseatMCCSale"
    ''-------------------------
    ''RICHA AGARWAL 17/03/2015 Misc Sale
    Public Const AllowToGenerateSaleInvoiceSeriesTaxatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesTaxatMiscSale"
    Public Const AllowToGenerateSaleInvoiceSeriesRetailatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesRetailatMiscSale"
    Public Const AllowToGenerateSaleInvoiceSeriesExciseatMiscSale As String = "AllowToGenerateSaleInvoiceSeriesExciseatMiscSale"
    ''-------------------------
    '=========================Preeti Gupta===========================
    Public Const ShowHierarchyAndCostCenterInAPInvoiceEntry As String = "ShowHierarchyAndCostCenterInAP"
    Public Const WeighmentNotMandatoryInMCC As String = "WeighmentNotMandatoryInMCC"
    '=================================================================
    Public Const ShowHierarchyAndCostCenterInARInvoiceEntry As String = "ShowHierarchyAndCostCenterInAR"

    Public Const PartialFADepDays As String = "PartialFADepDays"
    Public Const RateMultPartialFADepDays As String = "RateMultPartialFADepDays"
    Public Const AllowNegativeStock As String = "AllowNegativeStock"
    Public Const SendSalarySlipMailToEmployee As String = "SendSalarySlipMailToEmployee"
    Public Const DoNotMergeAPARAccount As String = "DoNotMergeAPARAccount"
    Public Const ShowVisiDetail As String = "ShowVisiDetail"
    Public Const CustomerNameUniqueOnCM As String = "CustomerNameUniqueOnCM"
    Public Const IsShortageIncludeInLandedCost As String = "IsShortageIncludeInLandedCost"
    Public Const AlowwdateChangeinPaymentEntry As String = "AlowwdateChangeinPaymentEntry"


    Public Const CreateAutoMilkRGPinBulkSRN As String = "CreateAutoMilkRGPinBulkSRN"
    Public Const DisplayAllParameterinQualityCheck As String = "DisplayAllParameterinQualityCheck"
    Public Const DisplayTypeInMilkReceipt As String = "DisplayTypeInMilkReceipt"
    '============Added by Rohit on Aug 03,2015 For Milk Type Validation in Milk sample.============
    Public Const AddValidationofMilkTypeinsample As String = "AddValidationofMilkTypeinsample"

    Public Const FatMinCow As String = "FatMinCow"
    Public Const FatMaxCow As String = "FatMaxCow"
    Public Const SNFMinCow As String = "SNFMinCow"
    Public Const SNFMaxCow As String = "SNFMaxCow"

    Public Const FatMinBuff As String = "FatMinBuff"
    Public Const FatMaxBuff As String = "FatMaxBuff"
    Public Const SNFMinBuff As String = "SNFMinBuff"
    Public Const SNFMaxBuff As String = "SNFMaxBuff"

    Public Const FatMinMix As String = "FatMinMix"
    Public Const FatMaxMix As String = "FatMaxMix"
    Public Const SNFMinMix As String = "SNFMinMix"
    Public Const SNFMaxMix As String = "SNFMaxMix"
    '================================================================================================
    Public Const AddIncentiveDeductioninMilkSample As String = "AddIncentiveDeductioninMilkSample"
    Public Const AllowManualEnterinWeighment As String = "AllowManualEnterinWeighment"
    Public Const SettlementBankOnlyPWD As String = "SettlementBankOnlyPWD"
    Public Const DocumentSequence As String = "DocumentSequence"
    Public Const AllowPurchaseAccounting As String = "AllowPurchaseAccounting"
    Public Const SHowBulkMilkWeighment As String = "SHowBulkMilkWeighment"
    Public Const StoreADJExportImportAfterPost As String = "StoreADJExportImportAfterPost"
    Public Const FatSNFControlOnProductionConsumption As String = "FatSNFControlOnProductionConsumption"
    Public Const QuantityControlToleranceOnProductionConsumption As String = "QuantityControlToleranceOnProductionConsumption"
    Public Const LeaveBalanceAlertTypeOnAttendance As String = "LeaveBalanceAlertTypeOnAttendance"
    Public Const StopNegativeBankBalance As String = "StopNegativeBankBalance"
    Public Const ConsumptionTypeMilk As String = "ConsumptionTypeMilk"
    Public Const ConsumptionTypeMilkStandardization As String = "ConsumptionTypeMilkSTD"
    Public Const ConsumptionTypeMilkProduct As String = "ConsumptionTypeMilkProduct"
    Public Const ConsumptionTypeOther As String = "ConsumptionTypeOther"
    Public Const ValidateFatSnfOnProduction As String = "ValidateFatSnfOnProduction"
    Public Const ShowOverheadCostOnProductionEntry As String = "ShowOverheadCostOnProductionEntry"
    Public Const ActivateProductionWithoutBatch As String = "ActivateProductionWithoutBatch"
    Public Const CreateJEOnProduction As String = "CreateJEOnProduction"

    Public Const AllowToSaveMultipleEmployeeStatus As String = "AllowToSaveMultipleEmployeeStatus"

    Public Const CreateJEForProvisionEntrySecondaryTransporter As String = "Secondary Transporter"
    Public Const CreateJEForProvisionEntryMCCLeaseVendor As String = "MCC Lease Vendor"
    Public Const CreateJEForProvisionEntryTransporterForFreshSale As String = "Transporter For Fresh Sale"
    Public Const CreateJEForProvisionEntryTransporterForProductSale As String = "Transporter For Product Sale"
    Public Const CreateJEForProvisionEntryTransporterForBulkSale As String = "Transporter For Bulk Sale"
    Public Const CreateJEForProvisionEntryOthers As String = "Others"
    Public Const CreateJEForProvisionEntryPrimaryTransporter As String = "Primary Transporter"
    Public Const CreateJEForProvisionEntryTransporterForTransfer As String = "Transporter For Transfer"
    Public Const CreateJEForProvisionEntryTransporterForCSATransfer As String = "Transporter For CSA Transfer"

    Public Const DoubleClickOnVC As String = "Double Click On VC"

    Public Const PickManual_CSATransfer_OnTRansferReturn As String = "CSA Transfer Effect on Return is Manual"
    Public Const PickManual_CSATransfer_OnCSASalePatti As String = "CSA Transfer Effect on Sale Patti is Manual"
    Public Const AllowDistributorSaleAtCSA_SaleInvoice As String = "Allow Distributor Sale at CSA Sale Patti"
    Public Const AllowItemWiseCSAAccountingON_CSASale As String = "CSA Account set pick Item-wise"
    Public Const IsAutoTankerWeightment As String = "Auto Tanker Weightment"
    Public Const IsAutoTankerWeighmentForBulkSale As String = "Auto Tanker Weighment for Bulk Sale"
    Public Const IsAdditionalInformationOnVillageMaster As String = "Show Village Add Info"
    Public Const CheckLiveStockInProductionDuringTrans As String = "CheckLiveStockInProductionDuringTrans"

    Public Const VLCTimeTableColumnShow As String = "VLCTimeTableColumnShow"
    Public Const VLCTimeTableColumnMandatory As String = "VLCTimeTableColumnMandatory"
    Public Const isOneMCCOnePrimaryTranporter As String = "One MCC One Primary Tranporter"
    Public Const MilkSamplShowOddEvenTwoGrid As String = "Show Odd and Even Two Grid"
    Public Const OpenODDEvenForm As String = "Open Odd-Even Form"
    Public Const IsApplyEMIOnAssetValue As String = "Is Apply EMI On Asset Value"

    ' KUNAL 6-SEP-2016 ======================================================================
    Public Const AllowFutureDateTransaction As String = "AllowFutureDateTransaction"
    'KUNAL > UDIL > DATE : 16-NOV-2016
    Public Const FindNRGP_Request As String = "Show_NRGP_RequestNo"
    '========================================================================================
    Public Const AllowCSAPriceMasterPostedData As String = "Allow CSAPriceMaster Posted Data"
    Public Const AllowItemMasterPostedData As String = "Allow Item Master Posted Data"
    Public Const AllowMilkItemMasterPostedData As String = "Allow Milk Item Master Posted Data"
    Public Const AllowBulkProcItemPostedData As String = "Allow Bulk Proc Milk Item Posted Data"
    Public Const AllowPriceListMasterPostedData As String = "Allow Price List Item Posted Data"

    'Stuti
    Public Const ItemCrateWtinKg As String = "Item Default Crate Wt.(Kg.)"
    Public Const ItemJaaliWtinKg As String = "Item Default Jaali Wt.(Kg.)"
    Public Const ItemBoxWtinKg As String = "Item Default Box Wt.(Kg.)"
    Public Const ItemCrateRate As String = "Item Default Crate Rate"
    Public Const ItemJaaliRate As String = "Item Default Jaali Rate"
    Public Const ItemBoxRate As String = "Item Default Box Rate"
    Public Const ItemCanRate As String = "Item Default Can Rate"

    Public Const CustomerMasterFinderOnLocationwiseARReceipt As String = "Customer master finder location-wise on AR Receipt"

    Public Const SameuserCanNotloginmultipletimes As String = "Same user can-not login multiple times"
    Public Const MandatoryEmployeeOnVehicleMaster = "Make employee no mandatory"
    Public Const PlantDepotMappingMandatory = "Map location of plant with depot is mandatory"
    Public Const ShowCancelButtonPO As String = "Show cancel button on purchase order"
    Public Const ShowOptionforSelectingCapex As String = "Show option for selecting capex code/subcode on PO"
    Public Const AutoClosePO As String = "Auto close PO when all qty. received."
    Public Const POCancel As String = "PO Cancel"
    'Public Const CreateJVForAllCasesinRGP = "Crate JV for all cases in RGP"
    Public Const StoreRequisitionMandatoryforstorerequest = "Store Requisition mandatory for store request"
    Public Const AllowThreeFormatByDefaultForPrint = "Allow printing 3 formats by default"
    Public Const MTCapacityRequired = "MT Capacity Required"
    Public Const AllowBackDateEntry As String = "Allow back date entry for given days"
    Public Const BackDateEntryPwd As String = "BackDateEntryPwd"
    Public Const RevisedBudget As String = "Revised Budget"
    Public Const DipMarkingMendatory As String = "Make dip marking mendatory."
    Public Const AllowDispatchChecklistOnProductDispatch As String = "Allow dispatch checklist on product dispatch"
    Public Const ShowIndentBasedOnCreatedUser As String = "Show indent based on created user"
    Public Const ShowSystemStockinOpenMCC As String = "Show system stock in open MCC shift"
    Public Const Tankerfromtankersalemasteringateentry As String = "Tanker from tanker sale master in gate entry"
    Public Const ApplyMultiChamberInBulkWeighmentEntry As String = "Apply multi-chamber in bulk weighment entry"
    Public Const DefaultItemUOMForBulkSale As String = "Default item uom for bulk sale"
    Public Const InsuranceNoAndSealNoInBulkDispatch As String = "Show option for entering Insurance No and Seal No"
    Public Const ValidateFatSNFOnJobMilkSRN As String = "Validate FAT KG & SNF KG on Job Milk SRN"
    Public Const CancelDocDueToSRNReturn As String = "Cancel document due to SRN Return"
    Public Const AmountInLacsOnMisSaleRegister As String = "Allow amount in lacs on MIS SALE REGISTER"
    Public Const ShortCloseItemWiseOnPO As String = "Allow short close item wise on PO"
    Public Const MakeClosingofPOreadonlyforuser As String = "Make closing of PO read only"
    Public Const AllowModificationOnApprovalByApprovalUser As String = "Allow Modification On Approval By Approval User"
    Public Const AllowAutoCalculateADDREMOVEQty As String = "Auto Calculate Qty of Add/Remove Item"
    '-----------------end here---------------'
    Public Const FATDeductionPercent As String = "FAT Deduction Percent"
    Public Const SNFDeductionPercent As String = "SNF Deduction Percent"
    Public Const RejectionReturnPaneltyPerUnit As String = "Rejection Return Penelty Per Unit"
    Public Const RejectionDrainPenaltyPerUnit As String = "Rejection Drain Penelty Per Unit"
    Public Const GraceTimeForTransporter As String = "Grace Time For Transporter"
    Public Const GraceTimeFromGateEntryToDocWeighing As String = "Grace Time From Gate Entry To Dock Weighing"
    ''==============end here================

    ''============CSA Sale Setting=====================================================================
    Public Const ShowCSAReturnTypeOnScreen As String = "Show CSA Return Type on screen"
    Public Const ShowCSARequestScreen As String = "Enable CSA Request Instead of Booking"
    Public Const AllowSchemeOnCSADeliveryOrder As String = "Allow Scheme at CSA DO Entry"
    Public Const AllowOtherItemOnCSAPriceMaster As String = "Allow Other Items On CSA Price Master"
    Public Const AllowRoundOff_OnCSASalePatti As String = "Inv. Amount Round-off on All Sale Invoice"
    Public Const FreightChargeOnCSASaleInvoice As String = "Comm./Freight itemwise on CSA Sale Invoice"
    Public Const AllowDisabledCommissionOnCSATransfer As String = "Commission disabled on CSA Transfer"
    Public Const DoReadonly_UnitRate_AtCSASale As String = "Allow Rate readonly on CSA Sale"
    Public Const Allow_SaleMfgACONCSAPatti As String = "Allow Sale mfg. A/c on CSA Sale Patti"

    Public Const AllowSchemeItemCondONSchemeMaster As String = "Allow Scheme type item on Scheme Master"
    Public Const ForUDLOnly As String = "CSA Sale changes For UDL only"
    Public Const CheckCreditLimitonCSADO As String = "Check Credit Limit on CSA DO"
    Public Const GrossWtFromItemMasterONCSATransfer As String = "Gross Wt. from item master on CSA Transfer"
    Public Const EnableExciseONCSASalePatti As String = "Enable Excise entry on CSA Sale Patti"
    Public Const BatchSkipCSAReturn As String = "Batch Skip at CSA sale patti/Return"
    ''====================end here=====================================================


    Public Const IsChamberWiseTanker As String = "Chamber wise Tanker"
    'Prabhakar'
    Public Const AllowLoginTypeCNFdistributerRetailer As String = "Allow Login Type CNF , Distributer, Retailer"

    Public Const AllowSchemeItemQty As String = "Allow Scheme Item in Materix Report"
    Public Const AllowDairyDeliveryOrderPrint As String = "Allow Print Button for Delivery Order "

    Public Const ShowSealNumberForTunkerOut As String = "Show Seal Number for Tunker Out"
    Public Const HideRateDispatchCentreCode As String = "Hide Rate and Dispatch Centre Code"
    Public Const AllowPromptPendingDocs As String = "Allow Prompt Pending Docs"
    Public Const AllowAutoGenerateDocNoInMaster As String = "Allow Auto Generate Doc No In Master Screen"
    'kunal
    Public Const ShowDocsStatusFilters As String = "Show Documents Declaration Status Filters"

    Public Const AutoDepartmentMendatroryFieldOnPurcahseCycle As String = "Allow Department Mandatory On Purchase Cycle"
    Public Const AllowVehicleGateOutValidationScrapSale As String = "Allow Vehicle Gate Out Validation For Scrap Sale "
    Public Const AllowVehicleGateOutValidationCSATransfer As String = "Allow Vehicle Gate Out Validation For CSA Transfer"
    Public Const AllowVehicleGateOutValidationSPSale As String = "Allow Vehicle Gate Out Validation For SP Sale"
    Public Const AllowVehicleGateOutValidationTransfer As String = "Allow Vehicle Gate Out Validation For Transfer"
    Public Const AllowWithoutUnitCostIssueReturnEntry As String = "Allow without amount save Issue/Return Entry"
    Public Const ZeroCostForReprocess As String = "Zero Cost For Reprocess"

    Public Const IsAutoReceiptPayment As String = "IsAutoReceiptPayment"
    Public Const TransferEntryOnInvCtrlAccount As String = "Transfer Entry On Inventory Control Account"
    Public Const AutoUpdateVLCUploaderCodeInVLCMaster As String = "AutoUpdateVLCUploaderCodeInVLCMaster"
    Public Const StandardInterfaceForMilkShiftEnd As String = "StandardInterfaceForMilkShiftEnd"
    Public Const ShiftEndAllowManualEntryOfDeduction As String = "Allow Manual Entry Of Deduction"
    Public Const PTMRatePerLtrKGOnStdQty As String = "Rate Ltr/KG On Std Qty"

    'default bank payment
    Public Const DefaultBank = "Default Bank for Cash Payment"
    Public Const DefaultLocation = "Default Location for Cash Payment"
    'added by preeti gupta 03/10/2016==============
    Public Const ShowParticluarColumnInSalesRegisterForGopalJee As String = "Show Column in sale register report for GopalJee"

    ''Added by Nazia
    Public Const ShowPrintDiscountInDairyDispatchForGopaljee As String = "Show print discount in Dairy Dispatch"
    Public Const MilkReceiptRequiredApproval As String = "Milk Receipt Required Approval"

    Public Const LinkDepartmentBetweenIndentAndIssue As String = "Link Department Between Indent And Issue"
    Public Const CombineExportImportOnSchemeMaster As String = "Combined Export/Import on Scheme Master Dairy"
    Public Const OpenPOforRejectShortageQty As String = "Open PO for Reject/Shortage Qty"
    Public Const AutoSelectMCCRouteVLC As String = "Auto Select MCC Route VLC"
    Public Const PickServerDateWithNoChange As String = "Pick server Date With No Change"
    Public Const PickFinishedItemasBatchItem As String = "Finish Item as BatchItem default on Item Master"
    Public Const ToleranceFixFor_RM_OT_TRADE As String = "Tol.% mandatory for RM,Other,Trade on Item Master"
    Public Const ConsiderAdvancePayment As String = "Consider Advance Payment"
    Public Const PayableAmountZeroForMCCSale As String = "Payable Amount Zero For MCC Sale"
    Public Const Allow_AmountTruncate_BulkMilkSRN As String = "Allow truncate amount on Bulk Milk SRN"
    Public Const AutoPurchaseReturnFromIssueReturn As String = "Auto Purchase Return from Issue/Return screen."
    ''===Sanjeet====
    Public Const ShowAlternateVechileforFreshSale As String = "Gate pass with alternate vechile for fresh sale"
    Public Const ProcessProductionIssue As String = "Allow Journal Entry on Process Producion Issue"

    Public Const GSTApplicable As String = "Allow GST Applicable"

    Public Const GSTApplicableDate As String = "Allow GST Applicable Date"
    Public Const AllowPanNoValidation As String = "Allow PAN No Validation"

    Public Const GSTActiveTaxesRatesGroup As String = "Show only Active Taxes/Rates/Groups for GST"

    Public Const AllowManualRejectionOfTanker As String = "Allow Manual Rejection Of Tanker"

    Public Const RunBulkProcOnAdjustedFATCLR As String = "Run Bulk Proc on adjusted FAT and CLR"
    Public Const BulkProcNetWeightCalculationWithVendorWeight As String = "Bulk Proc NetWeight Calculation by Vendor Weight"

    Public Const BulkProcPriceChartStandardRateWithZero As String = "Bulk Proc Price Chart standard rate with zero"

    Public Const RemoveForceAapprovalofBulkSRN As String = "Remove Force Approval of Bulk SRN"
    Public Const Allow_Plant_Depot_MCC_typeLocation As String = "Allow Plant Depot MCC type Location"
    Public Const ValidateCustomerPANwithName As String = "Allow Validate Customer PAN with Name"
    Public Const ValidateTaxGroupForTransaction As String = "Allow Validate Tax Group Should Not Blank"
    Public Const AllowSeprateSchemeItemPrintDairySaleInvoice As String = "Allow Seprate Scheme Item Print DairySaleInvoice"
    Public Const EnableHirerachyCostCentre As String = "Enable Hirerachy Level Cost Centre"
    Public Const EnableStoreCostCentre As String = "Enable Store Cost Centre"
    Public Const EnableCostingMethod As String = "Enable Costing Method"
    Public Const ShowAllCustomerOnMccMaterialSale As String = "Show All Customer On MCC Material Sale"
    Public Const ShowDefaultUser As String = "Show Default User"
    '(UDL)17/11/2016========
    Public Const ShowVatSeriesNoSeprately As String = "Allow Tax Tracking to Show Vat series No Seperatly"
    '(UDL)21/12/2016========
    Public Const AllowToGenerate_NEFTUPLOADER As String = "Allow Generate New NEFT UPLOADER File"
    '(UDL)05/01/2017
    Public Const AllowBulkPostingofAllDocuments As String = "Allow Bulk Posting of All Documents"
    '(UDL)10/01/2017
    Public Const AllowSameaAdditionalChargesMultiTime As String = "Allow Same Additaional Charges Multiple time"
    '(01/02/2017)
    Public Const AllowToSaveAndUpdatePasswordBased As String = "Allow Masters To Save and Update Pasword Based"
    Public Const AllowMasterModificationWithSecurity As String = "Allow Master Modification With Security"
    '(02/02/2017)
    Public Const ApplyRTGSAmtMoreThanGiven As String = "Apply RTGS Amount More Than Given"
    '====================
    Public Const GenerateSecondryCode As String = "Excise Secondary Series on Transfer"
    ''=====

    Public Const POWeighmentManual As String = "Mannual Weighment"

    ''======Ravi============
    Public Const AddTypeForUserMaster As String = "Add Type(Super User, Driver) in UserMaster"
    Public Const AddParavetEmployeeType As String = "Add Type Paravet in Employee Type"
    Public Const CalculateFIFOAndLIFOCosting As String = "Calculate FIFO And LIFO Costing"
    Public Const AllowDeductionPercentOnIncoming As String = "Allow Deduction(%) on incoming Quality"
    Public Const AllowLoginType As String = "Allow POS Functionality in ERP"

    Public Const MilkProcurementUploader As String = "Milk Procurement Uploader"

    Public Const TankerDispatchBulkUploader As String = "Bulk Tanker Uploader"

    Public Const EmptyCanWeight As String = "Empty Can Weight"
    Public Const MinuteInLastVehicleForGateEntry As String = "Minute Last Vehicle For Gate Entry"
    Public Const MinuteGateEntryToGrossWeight As String = "Minute Gate Entry To Gross Weight"
    Public Const MinuteGrossWeightToTareWeight As String = "Minute Gross Weight To Tare Weight"
    Public Const NoOfDaysForMultiInceForSameVSPForSamePayCycle As String = "NoOfDaysForMultiInceForSameVSPForSamePayCycle"
    Public Const PurchaseCounterOnTransactionType As String = "Purchase Counter On Transaction Type"
    Public Const BulkProcurementCounterOnEntryType As String = "Bulk Procurement Counter On Entry Type"
    Public Const StopForRepeatedFATSNF As String = "Stop Repeat FAT SNF"
    Public Const SampleFONTSize As String = "Font Size"
    Public Const SMSPrefix As String = "SMS Prefix"
    Public Const PickPendingMilkSRNinNextPaymentCycle As String = "Pick Pending Milk-SRN in Next Payment Cycle"
    '======================Preeti Gupta[29/12/2016]===========================
    Public Const TreatChequeClearDateAsRecoDate As String = "TreatChequeClearDateAsRecoDate"
    '========================END========================================
    '======================Preeti Gupta[29/12/2016]===========================
    Public Const BookWreckageFromSublocationOrSection As String = "BookWreckageFromSublocationOrSection"
    '========================END========================================
    Public Const StopVSPBillIfSomethingWrong As String = "Stop VSP Bill If Something Wrong"
    Public Const PDCSetting As String = "PDC Setting"
    Public Const AllowRoadPermitNo As String = "AllowRoadPermitNo"
    Public Const ShowMessgForTDS As String = "ShowMessgForTDS"
    Public Const IsShowTreeView As String = "IsShowTreeView"
    Public Const ShowVLCUploaderData As String = "Show VLC Uploader Data"
    '========================added parteek 09/01/2017
    Public Const FatSnfWhenMilktypeSelect As String = "Fat Snf persentage allow When Milk Type Select"
    Public Const DairyFreshTaxableandNonTaxable As String = "Taxable and Non-Taxable Item"

    Public Const SMSEMailPassword As String = "SMS EMail Password"
    Public Const CreateNewDocumentOnUploader As String = "Create New Document On Uploader"
    Public Const PopupJE As String = "Popup JE"

    'KUNAL > DATE : 23-01-2017 > CLIENT : Sahayog Dairy
    Public Const ShowAliasNames As String = "ShowAliasNames"
    'KUNAL > DATE : 23-01-2017 > CLIENT : Sahayog Dairy
    Public Const ShowFatAndSnfPercentageFields As String = "ShowFatNSNFPerc"
    'KUNAL > DATE : 23-01-2017 > CLIENT : Sahayog Dairy
    Public Const VehicleFitnessAndInsuranceFields As String = "VehicleFitnessFields"

    Public Const DocumentCancel As String = "Document Cancelation"
    Public Const DocumentCancelReturn As String = "Document Cancelation Return"
    Public Const PICancelUserPwd As String = "PI Cancel"
    Public Const CSADocumentCancel As String = "CSA Transfer Cancelation"


    Public Const FixVSPEMP As String = "Fix VSP EMP"
    Public Const FatSNFStockControl As String = "FatSNFStockControl"
    Public Const CheckBalanceFromInvMoveSummry As String = "CheckBalanceFromInvMoveSummry"
    Public Const ItemwiseFatSNFStockControl As String = "ItemwiseFatSNFStockControl"

    Public Const SepratePriceChartForCowMilk As String = "Seprate Price Chart For Cow Milk"
    '=======================Added by preeti gupta [20/02/207]=====================================
    Public Const AllowRoundInFixedAsset As String = "Allow Round In Fixed Asset"
    Public Const AllowDecimalInFixedAsset As String = "Allow Decimal In Fixed Asset"
    '==============================================================================================
    Public Const ApplyStdFATSNFRate As String = "Apply Standard FAT SNF Rate"
    Public Const OpenPriceChartPlanningScreenOnTotalSolid As String = "Open Price Chart Planning on Total Solid"
    Public Const AllowZeroQtyFATSNFInOpenMCCShift As String = "Allow Zero Qty FAT SNF In Open MCC Shift"
    Public Const AllowZeroQtyFATSNFInCloseMCCShift As String = "Allow Zero Qty FAT SNF In Close MCC Shift"
    ''============Parteek Added setting 03-03-2017
    Public Const POLimit As String = "POLimit"
    Public Const RequiredPOLimit As String = "RequiredPOLimit"
    Public Const UnitCostIncreasePurchaseInvoice As String = "UnitCostIncreasePurchaseInvoice"
    Public Const PromptMsgForPendingDocIntervel As String = "Prompt Messg For Pending Doc Intervel"
    Public Const UDLPurchaseOrderthroughAP As String = "UDL Purchase Order through AP invoice"
    Public Const UpdateInventorySummaryTable As String = "UpdateInventorySummaryTable"
    Public Const CreateConsumeEntry As String = "Create Consume Entry"
    Public Const ShowOptionforSelectingCapexForFA As String = "Showoptionforselectingcapexcode/subcodeonFA"
    Public Const UDLCapexAcquisionEntry As String = "UDL Capex for Acquision Entry"
    Public Const UDLRGPWiseDocument As String = "UDL RGP Wise Document Created"
    Public Const AllowAssetItemOnMiscSale As String = "Allow Asset Item on Misc. Sale"
    Public Const TriggerOfGLEntryForWinTable As String = "Trigger Of GL Entry For Win Table"
    'UDL DATE : 21-04-2017
    Public Const ShowRouteWiseAndVLCWiseReport As String = "ReportOfRouteAndVLCWise"
    Public Const UOMAtDiarySaleReturn As String = "UOMAtDiarySaleReturn"
    Public Const PayableAmountZeroForFarmerPayment As String = "PayableAmountZeroForFarmerPayment"
    Public Const CheckDocAmountInAPInvoiceEntry As String = "Check Doc Amount For AP Invoice Entry"
    Public Const ApplyTSPriceAtBulkSale As String = "Apply TS Price At Bulk Sale"

    'UDL > DATE : 3-MAY-2017 : CHANGING DECLARED DOCUMENT LIST TO PENDING DOCUMENT LIST OR VICE VERSA
    Public Const ShowPendingDocumentsListScreenOverDeclaredDocumentList As String = "Show Pending Documents Screen"
    Public Const MannualySetMPUploaderData As String = "MannualySetMPUploaderData"
    Public Const AllowSNFNotManditoryInBulkSale As String = "Allow SNF Not Manditory in Bulk Sale"
    Public Const VSPMPDiffrenceOnTSBasis As String = "VSP MP Diffrence On TS Basis"
    Public Const MilkProcuremntPickCLRInsteadOfSNF As String = "Milk Procuremnt Pick CLR Instead Of SNF"
    Public Const chkGSTTaxGroupValidity As String = "check GST Tax Group Validity"

    'GHO- Date : 29-Aug-2017
    Public Const ShowShipToPartyInDairyDispatch As String = "Show Ship To Party In Dairy Dispatch"
    Public Const BulkQCWithoutCLR As String = "Bulk Quality Check Without CLR"
    Public Const DOTaggingForDairySaleModule As String = "DO Tagging For Dairy Sale Module"
    Public Const AllowFractionInMCCTankerDispatchGrossQty As String = "Allow Fraction In MCC Tanker Dispatch Gross Qty"

    Public Const PurchaseModulePickFixTaxRate As String = "Purchase Module Pick Fix Tax Rate"


    Public Const TankerDispatchFinancialImpactInTransferIn As String = "Tanker Dispatch Financial Impact In Transfer In"

    Public Const ConvertQtyIntoKG = "Convert Qty into KG Bulk Dispatch"
    Public Const GSTExemptedAmountForNonRegisteredVendor As String = "GST Exempted Amount For Non Registered Vendor"

    Public Const IncreaseCrateQtyOnFiftyPercent As String = "Increase Crate Qty On Fifty Percent"

    Public Const FATSNFDeductionMixMilkFATMinValue As String = "FAT SNF Deduction Mix Milk FAT Min Value"
    Public Const FATSNFDeductionMixMilkFATMaxValue As String = "FAT SNF Deduction Mix Milk FAT Max Value"
    Public Const FATSNFDeductionMixMilkSNFMinValue As String = "FAT SNF Deduction Mix Milk SNF Min Value"
    Public Const FATSNFDeductionMixMilkSNFMaxValue As String = "FAT SNF Deduction Mix Milk SNF Max Value"
    Public Const FATSNFDeductionMixMilkDeductionPer As String = "FAT SNF Deduction Mix Milk Deduction Per"

    Public Const RoundOffPaiseAmount As String = "Round Off Paise Amount"
    Public Const EnableInternalTransfer As String = "Enable Internal Transfer for UDL"
    Public Const FreightProvisionAccount As String = "Freight Provision Account"
    Public Const TreatUnregisteredVendorAsRegisteredVendor As String = "Treat Unregistered Vendor As Registered Vendor"
    Public Const RecreateConsumptionEntry As String = "RecreateConsumptionEntry"
    Public Const BankRecoHidePWD As String = "Bank Reco Hide PWD"
    Public Const EnableItemGroupGLMapping As String = "Enable Item Group GL Mapping"
    Public Const EnableRackBin As String = "Enable Rack Bin Item"
    Public Const ChangeVehicleOnDairySaleBooking = "Change Vehicle On Dairy Sale Booking"
    Public Const VendorSetOffDayWise = "Vendor Set Off Day Wise"
    Public Const ReadOnlyTemplateFieldsOnAcqusition As String = "ReadOnlyTemplateFieldsOnAcqusition"
    Public Const IsAutoStartReading As String = "IsAutoStartReading"
    Public Const AddHighSecurityOnWeighingIntegratedScreen As String = "Add High Security On Weighing Integrated Screen"
    Public Const HighSecurityStableSeconds As String = "High Security Stable Seconds"
    Public Const HighSecurityWeightTolerance As String = "High Security Weight Tolerance"
    Public Const AllowManualvehicleOnDairyBooking As String = "AllowManualvehicleOnDairyBoking"
    Public Const FreeIndentQtyAfterPOClose As String = "Free Indent Qty After PO Close"
    Public Const ShowFATSNFinPaymentProcess As String = "Show FAT SNF in Payment Process"
    Public Const MaxRowsInCSVExport As String = "MaxRowsInCSVExport"
    Public Const MaxRowsInExcelExport As String = "MaxRowsInExcelExport"
    Public Const BigValidity As String = "Big Validity"
    Public Const AllowAssetBookChangeInTemplate As String = "AllowAssetBookChangeInTemplate"
    Public Const AllowSMSSendtoSalePerson As String = "Allow SMS Send to Sale Person"
    Public Const AllowSMSwhenCustomerCreditLimit As String = "SMS when Customer Credit limit reaches on DO."
    Public Const EnableScreenSelection As String = "Enable Screen Selection"
    Public Const SkipJobWorkSRNInPI = "Skip JobWork SRN in PI"
    Public Const ShowFatSnfAfterApproval = "Show Fat/Snf After Approval"
    Public Const ApplyTotalSolidPriceChart As String = "Apply Total Solid Price Chart"
    Public Const RequiredMgmtApprovalForRateIncrease As String = "Required Mgmt Approval For Rate Increase"
    Public Const AutoRoundOffSeprateAccountOnVendorTransaction As String = "Auto Round Off Seprate Account on Vendor Trans"
    '=================Added by preeti Gupta Against Ticket No[ADV/17/05/18-000032]===================================
    Public Const TreatCRATEAsItems As String = "Treat CRATE as Item"
    Public Const TreatCANAsItems As String = "Treat CAN as Item"
    Public Const DoNotShowDairyTypeItems As String = "Do not show dairy type items"
    '=============================================================================================
    Public Const PasswordRules As String = "Password Rules"
    Public Const AlwaysVSPDefaulter As String = "Always VSP Defaulter"
    Public Const RejectedMilkSendToRejectLocation As String = "Rejected Milk Send To Reject Location"
    Public Const NoOfPreNxtDayToPickAvgFATSNF As String = "No Of Pre Nxt Day To Pick Avg FAT SNF %"
End Class


Public Class clsFixedParameter
#Region "Variables"
    Public Type As String = ""
    Public Code As String = ""
    Public Description As String = ""
    Public Specification As String = ""
#End Region

    Public Shared Function GetData(ByVal strType As String, ByVal strCode As String, ByVal trans As SqlTransaction) As String
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Description from TSPL_FIXED_PARAMETER where TYPE='" + strType + "' and Code='" + strCode + "'", trans))
    End Function

    Public Shared Function GetSpecification(ByVal strType As String, ByVal strCode As String, ByVal trans As SqlTransaction) As String
        Return clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Specification from TSPL_FIXED_PARAMETER where TYPE='" + strType + "' and Code='" + strCode + "'", trans))
    End Function

    ''Created by Pradeep Sharma on 14/06/13 TO Get Combobox datatable

    Public Shared Function GetCboDataTable(ByVal strType As String, ByVal trans As SqlTransaction) As DataTable
        Dim qry As String = "SELECT Code,DESCRIPTION FROM TSPL_FIXED_PARAMETER where Type= '" + strType + "' "
        Dim dt_Cbo As DataTable = clsDBFuncationality.GetDataTable(qry, trans)
        Return dt_Cbo
    End Function

    Public Shared Sub UpdateData(ByVal Type As String, ByVal Code As String, ByVal Description As String, ByVal trans As SqlTransaction)
        Dim qry As String = "Update TSPL_FIXED_PARAMETER Set Description='" + Description + "' where TYPE='" + Type + "'"
        If clsCommon.myLen(Code) > 0 Then
            qry += " and Code='" + Code + "'"
        End If
        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        qry = String.Empty
    End Sub

    Public Shared Function UpdateFixedParameter(ByVal obj As clsFixedParameter, ByVal trans As SqlTransaction, ByVal isNewEntry As Boolean) As Boolean
        Try
            Dim coll As New Hashtable()
            clsCommon.AddColumnsForChange(coll, "Description", obj.Description)
            clsCommon.AddColumnsForChange(coll, "Specification", obj.Specification)
            If isNewEntry Then
                clsCommon.AddColumnsForChange(coll, "Type", obj.Type)
                clsCommon.AddColumnsForChange(coll, "Code", obj.Code)
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_FIXED_PARAMETER", OMInsertOrUpdate.Insert, "", trans)
            Else
                clsCommonFunctionality.UpdateDataTable(coll, "TSPL_FIXED_PARAMETER", OMInsertOrUpdate.Update, "Type='" + obj.Type + "' AND Code='" + obj.Code + "'", trans)
            End If
            '' update program code of the template, asset category and asset group
         

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetFixedParameter(ByVal trans As SqlTransaction) As DataTable
        Try
            Dim Qry As String = "select Type, Code, Description, Specification  from TSPL_FIXED_PARAMETER"
            Return clsDBFuncationality.GetDataTable(Qry)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    
    'DEBUG
    Public Shared Function InsertDefaultValueFixedParameter(ByVal strType As String, ByVal strCode As String, ByVal strDescription As String, ByVal strSpecification As String) As Boolean
        Dim qry As String = "select Type from TSPL_FIXED_PARAMETER where Code='" + strCode + "' and Type ='" + strType + "'"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)

        Dim coll As New Hashtable()
        clsCommon.AddColumnsForChange(coll, "Type", strType)
        clsCommon.AddColumnsForChange(coll, "Code", strCode)
        clsCommon.AddColumnsForChange(coll, "Description", strDescription)
        clsCommon.AddColumnsForChange(coll, "Specification", strSpecification, True)

        If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
            clsCommonFunctionality.UpdateDataTable(coll, "TSPL_FIXED_PARAMETER", OMInsertOrUpdate.Insert, "")
        End If
        Return True
    End Function

End Class

Public Class clsFixedParameterProgramMapping
    Public Shared Function InsertDefaultValue(ByVal strProgramCode As String, ByVal strType As String, ByVal strCode As String, ByVal ControlType As EnumControlType) As Boolean
        Dim coll As New Hashtable()
        clsCommon.AddColumnsForChange(coll, "Program_Code", strProgramCode)
        clsCommon.AddColumnsForChange(coll, "FP_Type", strType)
        clsCommon.AddColumnsForChange(coll, "FP_Code", strCode)
        clsCommon.AddColumnsForChange(coll, "Control_Type", clsCommon.myCdbl(ControlType))
        clsCommonFunctionality.UpdateDataTable(coll, "TSPL_FIXED_PARAMETER_PROGRAM_MAPPING", OMInsertOrUpdate.Insert, "")
        Return True
    End Function

    
End Class