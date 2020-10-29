'===================BM00000007847=======================
Imports common
Imports System.Data.SqlClient

Public Class clsPostCreateTable
    Public Shared Sub Post_AlterOrUpdateAllTables(ByVal exeVersion As String)
        Dim qry As String = ""
        Dim check As Integer = 0
        Dim trans As SqlTransaction
        Dim dt As DataTable = Nothing




        If (clsCommon.CompairString("5.0.0.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.0.91") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "select count(*) from tspl_fixed_parameter where description='Employee Type' and Code='Service Dealer'"
                check = CInt(clsDBFuncationality.getSingleValue(qry, trans))
                If check > 1 Then
                    qry = "delete from TSPL_FIXED_PARAMETER where Type='Service Dealer' and Code='Service Dealer' and Description='Employee Type'"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                    qry = "delete from TSPL_FIXED_PARAMETER where Description='Employee Type' and Code='Service Executive'"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                qry = "update TSPL_FIXED_PARAMETER set Type='Service Executive' where Type='Service Dealer' and Description='Employee Type'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        If (clsCommon.CompairString("5.0.0.92", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.0.92") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try


                '------------check already have primary key or not------------------
                If clsPostCreateTable.CheckPrimaryKey("tspl_vendor_master", "vendor_code", trans) = True Then

                Else
                    qry = "alter table tspl_vendor_master add primary key(vendor_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                '-----------------------------------------------------------------------------

                qry = "ALTER PROCEDURE [dbo].[sp_TSPL_VENDOR_MASTER_DELETE](@Vendor_Code varchar(12),@form_type varchar(10)) AS BEGIN DELETE TSPL_VENDOR_MASTER WHERE Vendor_Code=@Vendor_Code and form_type=@form_type End"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.0.93", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.0.93") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_CUSTOMER_MASTER alter column add1 varchar(150)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_CUSTOMER_MASTER alter column add2 varchar(75)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_CUSTOMER_MASTER alter column add3 varchar(75)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_PRICE_MASTER alter column Modified_Date varchar(10) NOT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_MILK_PRICE_MASTER alter column Created_Date varchar(10) NOT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "select count(*) from TSPL_FIXED_PARAMETER where Description='employee type' and code='ASM'"
                check = clsDBFuncationality.getSingleValue(qry, trans)

                If check <= 0 Then
                    qry = "insert into TSPL_FIXED_PARAMETER select 'ASM/ZM','ASM','Employee Type','Employee Type'"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.0.94", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.0.93") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update tspl_employee_master set emp_type='Service Dealer' where emp_type='Service Executive'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If

        If (clsCommon.CompairString("5.0.0.97", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.0.97", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "select count(*) from information_schema.columns where table_name='tspl_parameter_range_master' and column_name='value' and data_type='float'"
                check = clsDBFuncationality.getSingleValue(qry, trans)
                check = CheckColumnExist("tspl_parameter_range_master", "value", DBDataType.float_Type, 0, 0, trans)

                If check > 0 Then
                    'qry = "drop table tspl_parameter_range_master"
                    'clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.0.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.0.99", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='TSPL_VLC_MASTER_DETAIL' and COLUMN_NAME='route_code'"
                check = clsDBFuncationality.getSingleValue(qry, trans)
                check = CheckColumnExist("TSPL_VLC_MASTER_DETAIL", "route_code", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_VLC_MASTER_DETAIL drop column route_code"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("tspl_vlc_master_detail", "village_code", trans) = True Then
                Else
                    qry = "alter table tspl_vlc_master_detail add FOREIGN KEY(village_code) references TSPL_VILLAGE_MASTER(village_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.13", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.13", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_CUSTOMER_MASTER alter column contact_person_phone varchar(30) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='tspl_fixed_parameter' and COLUMN_NAME='Exe_Expired_Date'"
                check = clsDBFuncationality.getSingleValue(qry, trans)
                check = CheckColumnExist("tspl_fixed_parameter", "Exe_Expired_Date", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    qry = "alter table tspl_fixed_parameter drop column Exe_Expired_Date"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.21", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.21", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_Process_master", "capacaity", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    qry = "alter table TSPL_Process_master drop column capacaity"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_Process_master", "Item_Code", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_Process_master drop column Item_Code"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.33", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.33", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "select count(*) from information_schema.columns where table_name='TSPL_VENDOR_INVOICE_HEAD' and column_name='invoice_type' and isnull(COLUMN_DEFAULT,'')=''"
                check = clsDBFuncationality.getSingleValue(qry, trans)
                If check > 0 Then
                    qry = "alter table TSPL_VENDOR_INVOICE_HEAD add default 'AP' for invoice_type"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                qry = "update TSPL_VENDOR_INVOICE_HEAD set Invoice_Type='AP' where coalesce(Invoice_Type,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.34", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.34", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_FORM_MASTER alter column form_code varchar(30)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_FORM_SERIAL_NO_MASTER alter column form_code varchar(30)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.37", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.37", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_location_master", "category_struct_code", trans)
                DropConstraint("tspl_vendor_master", "category_struct_code", trans)
                DropConstraint("tspl_customer_master", "category_struct_code", trans)
                DropConstraint("TSPL_PP_BOM_ITEM_DETAIL", "bom_code", trans)
                DropConstraint("TSPL_PP_BOM_STAGE_DETAIL", "bom_code", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.43", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.43", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_BulkSalePrice_MASTER", "Fat_Percentage", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    qry = "alter table TSPL_BulkSalePrice_MASTER drop column Fat_Percentage"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_BulkSalePrice_MASTER", "Snf_Percentage", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_BulkSalePrice_MASTER drop column Snf_Percentage"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.48", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.48", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_vendor_master alter column service_charge_type varchar(20) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.70", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_milk_price_master_history alter column rate_type varchar(20) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.1.81", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.81", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CInt(CheckColumnExist("tspl_enquiry_master", "customer_name", DBDataType.NotApplicable, 0, 0, trans))

                If check > 0 Then
                    qry = "alter table tspl_enquiry_master drop column customer_name"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '=============Richa Ticket No. BM00000003712 on 08/09/2014
        If (clsCommon.CompairString("5.0.1.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.86", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_mcc_dispatch_challan alter column Tanker_KM_Reading Float"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '==============================================
        '=============Richa on 09/09/2014
        If (clsCommon.CompairString("5.0.1.88", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.88", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If clsPostCreateTable.CheckPrimaryKey("TSPL_Dispatch_BulkSale", "Customer_Code", trans) = True Then
                Else
                    qry = "  alter table TSPL_Dispatch_BulkSale add FOREIGN KEY(Customer_Code) references TSPL_CUSTOMER_MASTER(Cust_Code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If clsPostCreateTable.CheckPrimaryKey("TSPL_INVOICE_MASTER_BULKSALE", "Customer_Code", trans) = True Then
                Else
                    qry = "alter table TSPL_INVOICE_MASTER_BULKSALE add FOREIGN KEY(Customer_Code) references TSPL_CUSTOMER_MASTER(Cust_Code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==============================================

        '==============================================
        '=============Pankaj jha on 12/09/2014
        If (clsCommon.CompairString("5.0.1.92", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.92", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  alter table TSPL_PARAMETER_RANGE_MASTER_QC_history  alter column value1 varchar(max)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "  alter table TSPL_PARAMETER_RANGE_MASTER_QC  alter column value1 varchar(max)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==============================================
        '=============priti on 15/09/2014
        If (clsCommon.CompairString("5.0.1.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.95", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  alter table TSPL_BOOKING_DETAIL alter column item_code varchar(50)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==============================================
        'richa 16/09/2014 to drop column with constraint

        If (clsCommon.CompairString("5.0.1.97", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.97", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_dispatch_detail_bulksale", "Rate", trans)
                check = CheckColumnExist("tspl_dispatch_detail_bulksale", "Rate", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE tspl_dispatch_detail_bulksale DROP COLUMN Rate", trans)
                End If

                check = CheckColumnExist("TSPL_CSA_TRANSFER_HEAD", "comments", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CSA_TRANSFER_HEAD drop  column comments", trans)
                End If

                check = CheckColumnExist("TSPL_CSA_TRANSFER_HEAD", "Created_Date", DBDataType.varchar_Type, 10, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CSA_TRANSFER_HEAD drop column  Created_Date ", trans)
                End If

                check = CheckColumnExist("TSPL_CSA_TRANSFER_HEAD", "Modify_Date", DBDataType.varchar_Type, 10, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CSA_TRANSFER_HEAD drop column  Modify_Date ", trans)
                End If

                check = CheckColumnExist("TSPL_CSA_TRANSFER_HEAD", "Posting_Date", DBDataType.varchar_Type, 10, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CSA_TRANSFER_HEAD drop column  Posting_Date ", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If


        '================================
        ' ''richa 17/09/2014 Against Ticket No BM00000003892
        If (clsCommon.CompairString("5.0.1.98", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.98", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  Alter Table TSPL_Bulk_MILK_SRN Alter column Gate_Entry_No varchar(50) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " Alter Table TSPL_Bulk_MILK_SRN Alter column Weighment_No varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " Alter Table TSPL_Bulk_MILK_SRN Alter column Weighment_Date datetime null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " Alter Table TSPL_Bulk_MILK_SRN Alter column QC_No varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " Alter Table TSPL_Bulk_MILK_SRN Alter column Qc_Date datetime null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " Alter table TSPL_Bulk_MILK_SRN Alter Column Tanker_No varchar(30) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '=========================================

        If (clsCommon.CompairString("5.0.2.10", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.2.10", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_INVOICE_DETAIL_BULKSALE", "Dispatch_Code", trans)

                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_ITEM_QC_PARAMETER_MASTER alter column value1 varchar(max) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_ITEM_QC_PARAMETER_MASTER alter column Actual_Value varchar(max) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BATCH_ORDER_HEAD alter column Sub_Batch_Code varchar(max) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If clsCommon.CompairString("5.0.2.11", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.2.11") = CompairStringResult.Equal Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_DEDUCTION_HEAD_INSERT]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[SP_DEDUCTION_HEAD_INSERT]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "CREATE procedure [dbo].[SP_DEDUCTION_HEAD_INSERT](@deduction_code as varchar(12),@description as varchar(50),@TDS_Section as varchar(12),@cumm_cutoff as decimal(18, 2),@percent_Amount as char(1),@inactive as char(1),@comment as varchar(200),@createdby varchar(12),@createddate varchar(10),@modifiedby varchar(12),@modifieddate varchar(10),@compcode varchar(8),@Gl_Account varchar(50))as begin insert into TSPL_TDS_DEDUCTION_HEAD(Deduction_Code,Description,TDS_Section,Cumm_Cutoff,Percent_Amount,Inactive,Comment,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Gl_Account) values(@deduction_code,@description,@TDS_Section,@cumm_cutoff,@percent_Amount,@inactive,@comment,@createdby,@createddate,@modifiedby,@modifieddate,@compcode,@Gl_Account )End"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BATCH_ORDER_HEAD alter column Sub_Batch_Code varchar(max) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_ISSUE_QC_DETAIL alter column Value1 varchar(max) null", trans)
                trans.Commit()
            Catch ex As Exception

                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        'Anand on 23/09/2014
        If (clsCommon.CompairString("5.0.2.11", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.2.11", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_CUSTOMER_MASTER ALTER COLUMN Zone_Code varchar(30) null", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_CUSTOMER_MASTER_HISTORY ALTER COLUMN Zone_Code varchar(30) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        'Rohit on 29/09/2014
        If (clsCommon.CompairString("5.0.2.22", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.2.22", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MCC_MASTER ALTER COLUMN MCC_Code varchar(30) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.2.24", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.2.24", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_Receipt_Adjustment_Header", "IsMilkType", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    DropConstraint("TSPL_Receipt_Adjustment_Header", "IsMilkType", trans)
                    qry = "alter table TSPL_Receipt_Adjustment_Header drop column IsMilkType"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_Receipt_Adjustment_Detail", "Fat", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    DropConstraint("TSPL_Receipt_Adjustment_Detail", "Fat", trans)
                    qry = "alter table TSPL_Receipt_Adjustment_Detail drop column Fat"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_Receipt_Adjustment_Detail", "SNF", DBDataType.NotApplicable, 0, 0, trans)

                If check > 0 Then
                    DropConstraint("TSPL_Receipt_Adjustment_Detail", "SNF", trans)
                    qry = "alter table TSPL_Receipt_Adjustment_Detail drop column SNF"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ' Anubhooti 29-Sep-2014
        If clsCommon.CompairString("5.0.2.23", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.2.23") = CompairStringResult.Equal Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AccountGroups_insert]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[sp_AccountGroups_insert]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "CREATE procedure [dbo].[sp_AccountGroups_insert] (@accgpcode varchar(12),@des varchar(50),@createby varchar(12),@createdate varchar(10),@modifyby varchar(12),@modifydate varchar(10),@companycode varchar(8),@PrntOrdrno Integer,@Group_Type varchar(50)) As begin insert into TSPL_ACCOUNT_GROUPS(Account_Group_Code,Account_Group_Desc,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Print_Order,GROUP_TYPE) values (@accgpcode,@des,@createby,@createdate,@modifyby,@modifydate,@companycode, @PrntOrdrno,@Group_Type )End "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        ' Anubhooti 29-Sep-2014 (5.0.2.24 Should come first then exeVersion)
        If clsCommon.CompairString("5.0.2.24", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.2.24") = CompairStringResult.Equal Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_tspl_user_master_update]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[sp_tspl_user_master_update]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "CREATE proc [dbo].[sp_tspl_user_master_update](@UserCode varchar(12),@UserName varchar(50),@EmployeeCode varchar(12),@EmployeeName varchar(50),@Password varchar(200),@UserType varchar(12),@Level1 varchar(12),@Level2 varchar(12),@Level3 varchar(12),@Level4 varchar(12),@Createdby varchar(12),@Createddate varchar(10),@Modifiedby varchar(12),@Modifieddate varchar(10),@CompCode varchar(8), @ApprovalLevel Integer) as begin update TSPL_USER_MASTER set User_Code=@UserCode,User_Name=@UserName,Emp_Code=@EmployeeCode,Emp_Name=@EmployeeName,Password=@Password,User_Type=@UserType,Level1_Code=@Level1,Level2_Code=@Level2,Level3_Code=@Level3,Level4_Code=@Level4,Created_By=@Createdby,Created_Date=@Createddate,Modify_By=@Modifiedby,Modify_date=@Modifieddate,Comp_Code=@CompCode, ApprovalLevel=@ApprovalLevel ,Default_Location =null  where User_Code=@UserCode end"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_CForm_DETAIL", "Document_No", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '  Panch Raj
        If (clsCommon.CompairString("5.0.2.35", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.35") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_VLC_MASTER_HEAD", "Vehical_Name", DBDataType.varchar_Type, 100, 0, trans)

                If check > 0 Then
                    qry = "alter table TSPL_VLC_MASTER_HEAD alter column Vehical_Name varchar(100) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                'Drop contratil
                DropConstraint("TSPL_GL_ACCOUNTS", "Sub_Group_Code", trans)
                check = CheckColumnExist("TSPL_GL_ACCOUNTS", "Sub_Group_Code", DBDataType.varchar_Type, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_GL_ACCOUNTS drop column Sub_Group_Code"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                check = CheckColumnExist("TSPL_GATEENTRY_SALE", "Posted", DBDataType.char_Type, 1, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_GATEENTRY_SALE", "Posted", trans)
                    qry = "alter table TSPL_GATEENTRY_SALE alter column Posted int not null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                    qry = "alter table TSPL_GATEENTRY_SALE add default 0 for Posted"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '--------------------------
        'richa
        If (clsCommon.CompairString("5.0.2.38", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.38") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_Bulk_MILK_SRN", "SRN_Date", DBDataType.date_Type, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_Bulk_MILK_SRN alter column SRN_Date datetime not null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '===========================

        'Priti
        If (clsCommon.CompairString("5.0.2.42", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.42") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update  tspl_sd_shipment_head set WayBillDate=Document_Date where WayBillDate is null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update  TSPL_SD_SALE_INVOICE_HEAD set WayBillDate=Document_Date where WayBillDate is null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "delete from  TSPL_PROGRAM_MASTER where Program_Code='SALE-SET-NEW' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        '===========================

        If (clsCommon.CompairString("5.0.2.43", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.43") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_GL_ACCOUNTS", "Account_Group_Code", trans)

                check = CheckColumnExist("TSPL_MF_BOM_DETAIL", "CONSM_ITEM_CATEGORY_CODE", DBDataType.varchar_Type, 30, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MF_BOM_DETAIL alter column CONSM_ITEM_CATEGORY_CODE VARCHAR(30) NULL", trans)
                End If

                DropConstraint("TSPL_CSA_SALE_TRANSFER_DETAIL", "Against_Transfer_Code", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CSA_SALE_TRANSFER_DETAIL alter column Against_Transfer_Code varchar(30) null", trans)

                check = CheckColumnExist("TSPL_SHIFT_MASTER", "FROM_Date", DBDataType.datetime_Type, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SHIFT_MASTER alter column FROM_Date datetime NULL", trans)
                End If

                check = CheckColumnExist("TSPL_SHIFT_MASTER", "TO_Date", DBDataType.datetime_Type, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SHIFT_MASTER alter column TO_Date datetime NULL", trans)
                End If

                check = CheckColumnExist("TSPL_SHIFT_MASTER", "Posted", DBDataType.char_Type, 1, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SHIFT_MASTER alter column Posted char(1) NULL", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.2.63", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.63") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_VLC_ROUTE_SHIFT_MASTER", "Existing_Vill_Code", trans)
                DropConstraint("TSPL_VLC_ROUTE_SHIFT_MASTER", "New_Vill_Code", trans)
                check = CheckColumnExist("TSPL_VLC_ROUTE_SHIFT_MASTER", "Existing_Vill_Code", DBDataType.varchar_Type, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_VLC_ROUTE_SHIFT_MASTER drop column Existing_Vill_Code", trans)
                End If

                check = CheckColumnExist("TSPL_VLC_ROUTE_SHIFT_MASTER", "New_Vill_Code", DBDataType.varchar_Type, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_VLC_ROUTE_SHIFT_MASTER drop column New_Vill_Code", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        ' Anubhooti 30-Oct-2014 
        If (clsCommon.CompairString("5.0.2.65", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.65") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_VENDOR_MASTER alter column Bank_Code varchar(50)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        ' Richa 04-11-2014 
        If (clsCommon.CompairString("5.0.2.72", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.72") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "Delete from TSPL_PROGRAM_MASTER where Program_Code in ('MSaleDairy','SMSALEDSetup')  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("tspl_branch_account_Mapping", "From_Location", trans)
                DropConstraint("tspl_branch_account_Mapping", "To_Location", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        'Priti
        If (clsCommon.CompairString("5.0.2.81", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.80") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter  table TSPL_SD_SALES_ORDER_DETAIL alter column item_cost float "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter  table TSPL_SD_SHIPMENT_DETAIL alter column item_cost float"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter  table TSPL_DELIVERY_ORDER_DETAIL_PRODUCTSALE alter column item_cost float"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter  table TSPL_SD_SALE_INVOICE_DETAIL alter column item_cost float"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==========================

        'Panch Raj
        If (clsCommon.CompairString("5.0.2.83", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.83") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_DAILY_ATTENDANCE_DETAIL alter column FIRST_HALF varchar(5) not null  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_DAILY_ATTENDANCE_DETAIL alter column SECOND_HALF varchar(5) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_HOURLY_ATTENDANCE_DETAIL alter column FIRST_HALF varchar(5) not null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_HOURLY_ATTENDANCE_DETAIL alter column SECOND_HALF varchar(5) not null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_PP_LOG_SHEET_DETAIL", "parameter_Code", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==========================
        'RICHA AGARWAL AGAINST TICKET NO BM00000004602 12/11/2014
        If (clsCommon.CompairString("5.0.2.89", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.89") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                DropConstraint("TSPL_MP_MASTER", "City_code", trans)
                qry = "alter table TSPL_MP_MASTER alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_ROUTE_FREIGHT_DETAILS", "City_code", trans)
                qry = "alter table TSPL_ROUTE_FREIGHT_DETAILS alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_EMPLOYEE_MASTER", "PRESENT_CITY_CODE", trans)
                qry = "alter table TSPL_EMPLOYEE_MASTER alter column PRESENT_CITY_CODE VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_EMPLOYEE_MASTER", "PERMA_CITY_CODE", trans)
                qry = "alter table TSPL_EMPLOYEE_MASTER alter column PERMA_CITY_CODE VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_HR_APPLICANT_ENTRY", "City_code", trans)
                qry = "alter table TSPL_HR_APPLICANT_ENTRY alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("Tspl_Trainer_Master", "City_code", trans)
                qry = "alter table Tspl_Trainer_Master alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("Tspl_Trainer_Master_City", "City_code", trans)
                qry = "alter table Tspl_Trainer_Master_City alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                DropConstraint("TSPL_MCC_MASTER", "City_code", trans)
                qry = "alter table TSPL_MCC_MASTER alter column City_code VARCHAR(50) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                Dim strprimaryname As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select CONSTRAINT_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name='TSPL_CITY_MASTER' and column_name='City_Code'", trans))
                clsDBFuncationality.ExecuteNonQuery("Alter Table TSPL_CITY_MASTER DROP Constraint " & strprimaryname & "", trans)
                qry = "alter table TSPL_CITY_MASTER alter column City_Code VARCHAR(50) Not Null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                '---------------------------------------------
                If clsPostCreateTable.CheckPrimaryKey("TSPL_CITY_MASTER", "City_code", trans) = True Then
                Else
                    qry = "alter table TSPL_CITY_MASTER add primary key(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_MP_MASTER", "City_code", trans) = True Then
                Else
                    qry = "  alter table TSPL_MP_MASTER add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_ROUTE_FREIGHT_DETAILS", "City_code", trans) = True Then
                Else
                    qry = "  alter table TSPL_ROUTE_FREIGHT_DETAILS add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_EMPLOYEE_MASTER", "PRESENT_CITY_CODE", trans) = True Then
                Else
                    qry = "  alter table TSPL_EMPLOYEE_MASTER add FOREIGN KEY(PRESENT_CITY_CODE) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_EMPLOYEE_MASTER", "PERMA_CITY_CODE", trans) = True Then
                Else
                    qry = "  alter table TSPL_EMPLOYEE_MASTER add FOREIGN KEY(PERMA_CITY_CODE) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_HR_APPLICANT_ENTRY", "City_code", trans) = True Then
                Else
                    qry = "  alter table TSPL_HR_APPLICANT_ENTRY add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("Tspl_Trainer_Master", "City_code", trans) = True Then
                Else
                    qry = "  alter table Tspl_Trainer_Master add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("Tspl_Trainer_Master_City", "City_code", trans) = True Then
                Else
                    qry = "  alter table Tspl_Trainer_Master_City add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_MCC_MASTER", "City_code", trans) = True Then
                Else
                    qry = "  alter table TSPL_MCC_MASTER add FOREIGN KEY(City_code) references TSPL_CITY_MASTER(City_code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                'richa agarwal against ticket no BM00000004604 
                qry = "ALTER PROCEDURE [dbo].[sp_TSPL_VENDOR_MASTER_INSERT](@Vendor_Code varchar(12) ,@Vendor_Name varchar(50) = null,@Vendor_Group_Code varchar(12) = null,@Vendor_Group_Des varchar(50) = null,@Status char(1),@OnHold char(1),@transporter char(1),@Closing_Date varchar(10)  = null,@Add1 varchar(50) = null,@Add2 varchar(50)  = null,@Add3 varchar(50) = null,@City_Code varchar(12) = null,@City_Des varchar(50) = null,@State varchar(50) = null,@Country varchar(50) = null,@Phone1 varchar(20) = null,@Phone2 varchar(20) = null,@Fax varchar(20) = null,@Email varchar(50) = null,@WebSite varchar(50) = null,@Contact_Person_Name varchar(50) = null,@Contact_Person_Phone varchar(20) = null,@Contact_Person_Fax varchar(20) = null,@Contact_Person_Website varchar(50) = null,@Contact_Person_Email varchar(50) = null,@Terms_Code varchar(20) = null,@Terms_Code_Des varchar(50) = null,@Vendor_Account varchar(12) = null,@Vendor_Account_Set_Des varchar(50) = null,@Payment_Code varchar(12) = null,@Payment_Code_Des varchar(50) = null,@Vendor_Type_Code varchar(12) = null,@Vendor_Type_Des varchar(50) = null,@Bank_Code varchar(12) = null,@Bank_Code_Des varchar(50) = null,@Service_Tax_No varchar(50) = null,@Lst_No varchar(50) = null,@Tin_No varchar(50) = null,@Credit_Limit decimal(18, 2) = null,@Tax_Group varchar(12) = null,@Tax_Group_Des varchar(50)=null,@TAX1 varchar(12) = null,@TAX1_Rate decimal(18, 0) = null,@TAX2 varchar(12) = null,@TAX2_Rate decimal(18, 0) = null,@TAX3 varchar(12) = null,@TAX3_Rate decimal(18, 0) = null,@TAX4 varchar(12) = null,@TAX4_Rate decimal(18, 0) = null,@TAX5 varchar(12) = null,@TAX5_Rate decimal(18, 0) = null,@TAX6 varchar(12) = null,@TAX6_Rate decimal(18, 0) = null,@TAX7 varchar(12) = null,@TAX7_Rate decimal(18, 0) = null,@TAX8 varchar(12) = null,@TAX8_Rate decimal(18, 0) = null,@TAX9 varchar(12) = null,@TAX9_Rate decimal(18, 0) = null,@TAX10 varchar(12) = null,@TAX10_Rate decimal(18, 0) = null,	@Remarks1 varchar(200) = null,@Remarks2 varchar(200) = null,@Additional1 varchar(50) = null,@Additional2 varchar(50)= null,@Additional3 varchar(50) = null,@cst varchar(30) = null,@ecc varchar(30) = null,@range varchar(30) = null,@collectorate varchar(30) = null,@pan varchar(30) = null,@Created_By varchar(12),@Created_Date varchar(10),@Modify_By varchar(12),@Modify_Date varchar(10),@Comp_Code varchar(8),@is_Gross_Receipt integer,@Inter_branch char(1),@Branch_Name varchar(150) = null,@Account_No varchar(50) = null,@Bank_Name varchar(50) = null,@IFSC_Code varchar(50)= null,@Account_Type varchar(10)= null,@Vendor_Type varchar(10)= null) AS BEGIN insert into TSPL_VENDOR_MASTER (Vendor_Code,Vendor_Name,Vendor_Group_Code,Vendor_Group_Code_Desc,Status,OnHold,Transporter,Closing_Date,Add1,Add2,Add3,City_Code,City_Code_Desc,State,Country,Phone1,Phone2,Fax,Email,WebSite,Contact_Person_Name,Contact_Person_Phone,Contact_Person_Fax,Contact_Person_Website,Contact_Person_Email,Terms_Code,Terms_Code_Desc,Vendor_Account,Vendor_Account_Desc,Payment_Code,Payment_Code_Desc,Ven_Type_Code,Ven_Type_Desc,Bank_Code,Bank_Code_Desc,Service_Tax_No,Lst_No,Tin_No,Credit_Limit,Tax_Group,Tax_Group_Desc,TAX1,TAX1_Rate,TAX2,TAX2_Rate,TAX3,TAX3_Rate,TAX4,TAX4_Rate,TAX5,TAX5_Rate,TAX6,TAX6_Rate,TAX7,TAX7_Rate,TAX8,TAX8_Rate,TAX9,TAX9_Rate,TAX10,TAX10_Rate,Remarks1,Remarks2,Additional1 ,Additional2 ,Additional3,CST,ECC,Range,Collectorate,Pan,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,is_Gross_Receipt,Inter_branch,Branch_Name,Account_No,Bank_Name,IFSC_Code,Account_Type,Vendor_Type)values (@Vendor_Code,@Vendor_Name,@Vendor_Group_Code,@Vendor_Group_Des,@Status,@OnHold,@transporter,@Closing_Date,@Add1,@Add2,@Add3,@City_Code,@City_Des,@State,@Country,@Phone1,@Phone2,@Fax,@Email,@WebSite,@Contact_Person_Name,@Contact_Person_Phone,@Contact_Person_Fax,@Contact_Person_Website,@Contact_Person_Email,@Terms_Code,@Terms_Code_Des,@Vendor_Account,@Vendor_Account_Set_Des,@Payment_Code,@Payment_Code_Des,@Vendor_Type_Code,@Vendor_Type_Des,@Bank_Code,@Bank_Code_Des,@Service_Tax_No,@Lst_No,@Tin_No,@Credit_Limit,@Tax_Group,@Tax_Group_Des,@TAX1,@TAX1_Rate,@TAX2,@TAX2_Rate,@TAX3,@TAX3_Rate,@TAX4,@TAX4_Rate,@TAX5,@TAX5_Rate,@TAX6,@TAX6_Rate,@TAX7,@TAX7_Rate,@TAX8,@TAX8_Rate,@TAX9,@TAX9_Rate,@TAX10,@TAX10_Rate,@Remarks1,@Remarks2,@Additional1,@Additional2,@Additional3,@cst,@ecc,@Range,@collectorate,@pan,@Created_By,@Created_Date,@Modify_By,@Modify_Date,@Comp_Code,@is_Gross_Receipt,@Inter_branch,@Branch_Name,@Account_No,@Bank_Name,@IFSC_Code,@Account_Type,@Vendor_Type)END"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER PROCEDURE  [dbo].[sp_TSPL_VENDOR_MASTER_UPDATE] (@Vendor_Code varchar(12),@Vendor_Name varchar(50) = null,@Vendor_Group_Code varchar(12) = null,@Vendor_Group_Des varchar(50) = null,@Status char(1),@OnHold char(1),@transporter char(1),@Closing_Date varchar(10)  = null,@Add1 varchar(50) = null,@Add2 varchar(50) = null,@Add3 varchar(50) = null,@City_Code varchar(12) = null,@City_Des varchar(50) = null,@State varchar(50) = null,@Country varchar(50) = null,@Phone1 varchar(20) = null,@Phone2 varchar(20) = null,@Fax varchar(20) = null,@Email varchar(50) = null,@WebSite varchar(50) = null,@Contact_Person_Name varchar(50) = null,@Contact_Person_Phone varchar(20) = null,@Contact_Person_Fax varchar(20) = null,@Contact_Person_Website varchar(50) = null,@Contact_Person_Email varchar(50) = null,@Terms_Code varchar(20) = null,@Terms_Code_Des varchar(50) = null,@Vendor_Account varchar(12) = null,@Vendor_Account_Set_Des varchar(50) = null,@Payment_Code varchar(12) = null,@Payment_Code_Des varchar(50) = null,@Vendor_Type_Code varchar(12) = null,@Vendor_Type_Des varchar(50) = null,@Bank_Code varchar(12) = null,@Bank_Code_Des varchar(50) = null,@Service_Tax_No varchar(50) = null,@Lst_No varchar(50) = null,@Tin_No varchar(50) = null,@Credit_Limit decimal(18, 2) = null,@Tax_Group varchar(12) = null,@Tax_Group_Des varchar(50)=null,@TAX1 varchar(12) = null,@TAX1_Rate decimal(18, 0) = null,@TAX2 varchar(12) = null,@TAX2_Rate decimal(18, 0) = null,@TAX3 varchar(12) = null,@TAX3_Rate decimal(18, 0) = null,@TAX4 varchar(12) = null,@TAX4_Rate decimal(18, 0) = null,@TAX5 varchar(12) = null,@TAX5_Rate decimal(18, 0) = null,@TAX6 varchar(12) = null,@TAX6_Rate decimal(18, 0) = null,@TAX7 varchar(12) = null,@TAX7_Rate decimal(18, 0) = null,@TAX8 varchar(12) = null,@TAX8_Rate decimal(18, 0) = null,@TAX9 varchar(12) = null,@TAX9_Rate decimal(18, 0) = null,@TAX10 varchar(12) = null,@TAX10_Rate decimal(18, 0) = null,@Remarks1 varchar(200) = null,@Remarks2 varchar(200) = null,@Additional1 varchar(50) = null,@Additional2 varchar(50)= null,@Additional3 varchar(50) = null,@cst varchar(30) = null,@ecc varchar(30) = null,@range varchar(30) = null,@collectorate varchar(30) = null,@pan varchar(30) = null,@Modify_By varchar(12),@Modify_Date varchar(10),@Comp_Code varchar(8),@is_Gross_Receipt integer,@InterBranch char(1),@Branch_Name varchar(150) = null,@Account_No varchar(50) = null,@Bank_Name varchar(50) = null,@IFSC_Code varchar(50) = null,@Account_Type varchar(10) = null,@Vendor_Type varchar(10) = null) AS BEGIN UPDATE TSPL_VENDOR_MASTER SET vendor_Code= @Vendor_Code,Vendor_Name=@Vendor_Name,Vendor_Group_Code=@Vendor_Group_Code,Vendor_Group_Code_Desc=@Vendor_Group_Des,Status=@Status,OnHold=@OnHold,Transporter=@transporter,Closing_Date=@Closing_Date,Add1 =@Add1,Add2=@Add2,Add3=@Add3,City_Code=@City_Code,City_Code_Desc=@City_Des,State=@State,Country=@Country,Phone1=@Phone1,Phone2=@Phone2,Fax=@Fax,Email=@Email,WebSite=@WebSite,Contact_Person_Name=@Contact_Person_Name,Contact_Person_Phone=@Contact_Person_Phone,Contact_Person_Fax=@Contact_Person_Fax,Contact_Person_Website=@Contact_Person_Website,Contact_Person_Email=@Contact_Person_Email,Terms_Code=@Terms_Code,Terms_Code_Desc=@Terms_Code_Des,Vendor_Account =@Vendor_Account,Vendor_Account_Desc=@Vendor_Account_Set_Des,Payment_Code=@Payment_Code,Payment_Code_Desc=@Payment_Code_Des,Ven_Type_Code=@Vendor_Type_Code,Ven_Type_Desc=@Vendor_Type_Des,Bank_Code=@Bank_Code,Bank_Code_Desc=@Bank_Code_Des,Service_Tax_No=@Service_Tax_No,Lst_No=@Lst_No,Tin_No=@Tin_No,Credit_Limit=@Credit_Limit,Tax_Group=@Tax_Group,Tax_Group_Desc=@Tax_Group_Des,TAX1=@TAX1,TAX1_Rate=@TAX1_Rate,TAX2=@TAX2,TAX2_Rate=@TAX2_Rate,TAX3=@TAX3,TAX3_Rate=@TAX3_Rate,TAX4=@TAX4,TAX4_Rate=@TAX4_Rate,TAX5=@TAX5,@TAX5_Rate=@TAX5_Rate,TAX6=@TAX6,TAX6_Rate =@TAX6_Rate,TAX7=@TAX7,TAX7_Rate=@TAX7_Rate,TAX8=@TAX8,TAX8_Rate=@TAX8_Rate,TAX9 =@TAX9,TAX9_Rate=@TAX9_Rate,TAX10=@TAX10,TAX10_Rate=@TAX10_Rate,Remarks1=@Remarks1,Remarks2=@Remarks2,Additional1=@Additional1,Additional2=@Additional2,Additional3=@Additional3,CST=@cst,ECC=@ecc,Range=@range,Collectorate=@collectorate,PAN=@pan,Modify_By =@Modify_By,Modify_Date=@Modify_Date,Comp_Code=@Comp_Code,is_Gross_Receipt=@is_Gross_Receipt,Inter_Branch = @InterBranch,Branch_Name=@Branch_Name,Account_No=@Account_No,Bank_Name=@Bank_Name,IFSC_Code=@IFSC_Code,Account_Type=@Account_Type,Vendor_Type=@Vendor_Type WHERE  Vendor_Code= @Vendor_Code END"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
                '------------------------------
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '===================
        'richa against ticket no BM00000004638
        If (clsCommon.CompairString("5.0.2.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.91") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "alter table TSPL_CUSTOMER_MASTER alter column Contact_Person_Phone varchar(50) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_CUSTOMER_MASTER alter column City_Code varchar(50) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '===========================
        ' Anubhooti 14-Nov-2014
        If (clsCommon.CompairString("5.0.2.92", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.92") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_VENDOR_MASTER", "TDS_State_Code", trans)
                DropConstraint("TSPL_TDS_VENDOR_DETAILS", "State_Code", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        'rohit nov 18,2014

        If (clsCommon.CompairString("5.0.2.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.2.95", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_VSPItem_HEAD", "Doc_Date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    qry = "alter table TSPL_VSPItem_HEAD drop column Doc_Date"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    qry = "alter table TSPL_VSPItem_HEAD add Doc_Date date null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.2.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.2.99") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PP_STAGE_PROCESS_QC_LOG_SHEET alter column Parameter_Code varchar(30) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        If (clsCommon.CompairString("5.0.3.18", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.18") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_VLC_MASTER_HEAD alter column Route_code varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_PAYMENT_HEADER alter column Entry_Desc varchar(250) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        'richa agarwal 02/12/2014
        If (clsCommon.CompairString("5.0.3.22", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.22") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_CUSTOMER_INFO alter column Contact_Person_Phone varchar(50) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ASSET_INSTALL_PULLOUT_NEW alter column agreement_no varchar(100) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ASSET_INSTALL_PULLOUT_NEW alter column Cheque_No_Sec varchar(100) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VISI_MASTER alter column agreement_no varchar(100) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VISI_MASTER alter column CHEQUE_NO varchar(100) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        'Pankaj Jha  03/12/2014
        If (clsCommon.CompairString("5.0.3.23", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.23") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table gridlayout alter column ReportID varchar(100) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.3.25", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.25", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("tspl_mp_master", "city_code", DBDataType.varchar_Type, 50, Nothing, trans) Then
                    DropConstraint("tspl_mp_master", "city_code", trans)
                End If
                If CheckColumnExist("tspl_mp_master", "state_code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    DropConstraint("tspl_mp_master", "state_code", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.3.27", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.27", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='tspl_weight_conversion' and COLUMN_NAME='Product_Type'", trans))
                If check > 0 Then 'when primary key is not found on product_type then drop first other two primary keys.
                    DropConstraint("tspl_weight_conversion", "Contained_UOM", trans)
                    DropConstraint("tspl_weight_conversion", "Container_UOM", trans)
                End If

                If CheckColumnExist("TSPL_SD_SALE_INVOICE_HEAD", "CSA_FOC_STATUS", DBDataType.int_Type, Nothing, Nothing, trans) Then
                    DropConstraint("TSPL_SD_SALE_INVOICE_HEAD", "CSA_FOC_STATUS", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALE_INVOICE_HEAD drop column CSA_FOC_STATUS", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.3.32", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.32") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "ALTER TABLE TSPL_EMPLOYEE_MASTER ALTER COLUMN PRESENT_MOBILE_NO VARCHAR(50) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_EMPLOYEE_MASTER ALTER COLUMN Phone VARCHAR(50) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_EMPLOYEE_MASTER ALTER COLUMN PERMA_MOBILE_NO VARCHAR(50) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_EMPLOYEE_MASTER ALTER COLUMN PERMA_PHONE_NO VARCHAR(50) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.33", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.33") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                DropConstraint("TSPL_PAYMENT_TERMS_GROUP_MASTER_MT", "Terms_Code", trans)
                check = CheckColumnExist("TSPL_PAYMENT_TERMS_GROUP_MASTER_MT", "Terms_Code", DBDataType.varchar_Type, 0, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_PAYMENT_TERMS_GROUP_MASTER_MT drop column Terms_Code"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.35", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.35") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_FAT_SNF_UPLOADER_Chart_Detail", "vsp_code", DBDataType.varchar_Type, Nothing, Nothing, trans) Then
                    DropConstraint("TSPL_FAT_SNF_UPLOADER_Chart_Detail", "vsp_code", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_FAT_SNF_UPLOADER_Chart_Detail drop column vsp_code", trans)
                End If

                If CheckColumnExist("tspl_sd_sales_quotation_head", "Comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_sd_sales_quotation_head alter column comments varchar(5000) null", trans)
                End If
                If CheckColumnExist("TSPL_SD_SALES_ORDER_HEAD", "Comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_ORDER_HEAD alter column comments varchar(5000) null", trans)
                End If
                If CheckColumnExist("TSPL_EX_PI_HEAD", "comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD alter column comments varchar(5000) null", trans)
                End If
                If CheckColumnExist("TSPL_EX_PI_HEAD", "Cust_PODate", DBDataType.datetime_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD drop column Cust_PODate", trans)
                End If
                If CheckColumnExist("TSPL_EX_COMMERCIAL_INVOICE_HEAD", "comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_COMMERCIAL_INVOICE_HEAD alter column comments varchar(5000) null", trans)
                End If
                If CheckColumnExist("TSPL_EX_COMMERCIAL_INVOICE_HEAD", "Cust_PODate", DBDataType.datetime_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_COMMERCIAL_INVOICE_HEAD drop column Cust_PODate", trans)
                End If
                If CheckColumnExist("TSPL_EX_PI_HEAD_HISTORY", "comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD_HISTORY alter column comments varchar(5000) null", trans)
                End If
                If CheckColumnExist("TSPL_EX_PI_HEAD_HISTORY", "Cust_PODate", DBDataType.datetime_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD_HISTORY drop column Cust_PODate", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.3.39", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.39", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_GATEENTRY_SALE", "Tanker_No", trans)
                DropConstraint("TSPL_WEIGHMENT_DETAIL_BULKSALE", "Tanker_No", trans)
                DropConstraint("TSPL_LOADING_TANKER_DETAIL_BULKSALE", "Tanker_No", trans)
                DropConstraint("TSPL_Quality_Check_BulkSale", "Tanker_No", trans)
                DropConstraint("TSPL_Dispatch_BulkSale", "Tanker_Code", trans)
                DropConstraint("TSPL_INVOICE_DETAIL_BULKSALE", "Tanker_Code", trans)
                DropConstraint("TSPL_GATEOUT_SALE", "Tanker_No", trans)
                DropConstraint("TSPL_SALE_RETURN_DETAIL_BULKSALE", "Tanker_Code", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.3.46", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.44", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_csa_transfer_head", "Vehicle_Id", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_csa_transfer_head alter column Vehicle_Id varchar(30) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.47", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.47", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_BankReco_Detail", "Document_Type", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_BankReco_Detail alter column Document_Type varchar(50) null", trans)

                qry = "ALTER procedure [dbo].[sp_tspl_banktransfer_insert](@Transfer_No varchar(30),@Transfer_Date date,@Transfer_Posting_Date date,@Description varchar(100),@Reference varchar(100),@From_Bank_Code varchar(12),@From_Bank_Name varchar(60),@From_Bank_Acc_No varchar(30),@Transfer_Amount decimal(18,2),@From_Bank_GL_Acc varchar(50)=null,@From_Bank_GLAcc_Desc varchar(100)=null,@From_Bank_GL_Amount decimal(18,2)=null,@To_Bank_Code varchar(12),@To_Bank_Name varchar(60),@To_Bank_Acc_No varchar(30),@Deposit_Amount decimal(18,2),@To_Bank_GL_Acc varchar(50)=null,@To_Bank_GLAcc_Desc varchar(100)=null,@To_Bank_GL_Amount decimal(18,2)=null,@Post char(1)=null,@Created_By varchar(12),@Created_Date varchar(10),@Modify_By varchar(12),@Modify_Date varchar(10),@Comp_Code varchar(8),@Cheque_No varchar(12)=null,@Cheque_Date Date=null , @Payment_Mode Varchar(20)=NULL,@frmbnkaccno varchar(50)=null,@tobnkaccno varchar(30) =null) as begin insert into TSPL_BANK_TRANSFER (Transfer_No,Transfer_Date,Transfer_Posting_Date,Description,Reference,From_Bank_Code,From_Bank_Name,From_Bank_Acc_No,Transfer_Amount,From_Bank_GL_Acc,From_Bank_GLAcc_Desc,From_Bank_GL_Amount,To_Bank_Code,To_Bank_Name,To_Bank_Acc_No,Deposit_Amount ,To_Bank_GL_Acc,To_Bank_GLAcc_Desc,To_Bank_GL_Amount,Post,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Cheque_No,Cheque_Date, Payment_Mode,From_BANKACCNUMBER ,TO_BANKACCNUMBER ) values(@Transfer_No,@Transfer_Date,@Transfer_Posting_Date,@Description,@Reference,@From_Bank_Code,@From_Bank_Name,@From_Bank_Acc_No,@Transfer_Amount,@From_Bank_GL_Acc,@From_Bank_GLAcc_Desc,@From_Bank_GL_Amount,@To_Bank_Code,@To_Bank_Name,@To_Bank_Acc_No,@Deposit_Amount ,@To_Bank_GL_Acc,@To_Bank_GLAcc_Desc,@To_Bank_GL_Amount,@Post,@Created_By,@Created_Date,@Modify_By,@Modify_Date,@Comp_Code,@Cheque_No,@Cheque_Date, @Payment_Mode,@frmbnkaccno,@tobnkaccno) END"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER procedure [dbo].[sp_tspl_banktransfer_update](@Transfer_No varchar(30),@Transfer_Date date,@Transfer_Posting_Date date,@Description varchar(100),@Reference varchar(100),@From_Bank_Code varchar(12),@From_Bank_Name varchar(60),@From_Bank_Acc_No varchar(30),@Transfer_Amount decimal(18,2),@From_Bank_GL_Acc varchar(50),@From_Bank_GLAcc_Desc varchar(100),@From_Bank_GL_Amount decimal(18,2),@To_Bank_Code varchar(12)=null,@To_Bank_Name varchar(60)=null,@To_Bank_Acc_No varchar(30)=null,@Deposit_Amount decimal(18,2),@To_Bank_GL_Acc varchar(50)=null,@To_Bank_GLAcc_Desc varchar(100)=null,@To_Bank_GL_Amount decimal(18,2),@Post char(1),@Created_By varchar(12),@Created_Date varchar(10),@Modify_By varchar(12),@Modify_Date varchar(10),@Comp_Code varchar(8),@Cheque_No varchar(12)=null,@Cheque_Date Date=null, @Payment_Mode Varchar(20)=NULL,@frmbnkaccno varchar(30),@tobnkaccno varchar(30)) as begin update TSPL_BANK_TRANSFER set Transfer_No = @Transfer_No ,Transfer_Date = @Transfer_Date ,Transfer_Posting_Date = @Transfer_Posting_Date ,Description = @Description ,Reference = @Reference ,From_Bank_Code = @From_Bank_Code ,From_Bank_Name = @From_Bank_Name ,From_Bank_Acc_No = @From_Bank_Acc_No ,Transfer_Amount = @Transfer_Amount ,From_Bank_GL_Acc = @From_Bank_GL_Acc ,From_Bank_GLAcc_Desc = @From_Bank_GLAcc_Desc  ,From_Bank_GL_Amount = @From_Bank_GL_Amount ,To_Bank_Code = @To_Bank_Code ,To_Bank_Name = @To_Bank_Name ,To_Bank_Acc_No = @To_Bank_Acc_No ,Deposit_Amount = @Deposit_Amount  ,To_Bank_GL_Acc = @To_Bank_GL_Acc ,To_Bank_GLAcc_Desc = @To_Bank_GLAcc_Desc ,To_Bank_GL_Amount = @To_Bank_GL_Amount ,Post = @Post ,Created_By = @Created_By ,Created_Date = @Created_Date ,Modify_By = @Modify_By ,Modify_Date = @Modify_Date ,Comp_Code = @Comp_Code,Cheque_No=@Cheque_No ,Cheque_Date=@Cheque_Date, Payment_Mode=@Payment_Mode,From_BANKACCNUMBER=@frmbnkaccno,TO_BANKACCNUMBER=@tobnkaccno where Transfer_No = @Transfer_No END"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.56", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.56", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_PP_LOG_SHEET_DETAIL", "Time_Value", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_LOG_SHEET_DETAIL alter column Time_Value varchar(30) NULL", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.58", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.58", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                DropConstraint("TSPL_EX_PI_DETAIL", "shipping_mark", trans)
                DropConstraint("TSPL_EX_PI_DETAIL_history", "shipping_mark", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_DETAIL alter column shipping_mark varchar(100) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_DETAIL_history alter column shipping_mark varchar(100) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '==================rohit================
        If (clsCommon.CompairString("5.0.3.58", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.58", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_mcc_master alter column Payment_Cycle varchar(30) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '========================================================
        If (clsCommon.CompairString("5.0.3.62", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.62", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_Account_Groups alter column Account_Group_Desc varchar(100) null", trans)
                trans.Commit()
            Catch ex As Exception
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
                trans.Rollback()
            End Try
        End If

        If (clsCommon.CompairString("5.0.3.64", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.64", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_VLC_DATA_UPLOADER_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_INVENTORY_MOVEMENT_NEW ALTER COLUMN trans_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_VENDOR_INVOICE_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_JOURNAL_Details ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_PURCHASE_INVOICE_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_RECEIPT_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_SAMPLE_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_SAMPLE_DETAIL_History ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_Shift_End_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_Shift_End_Route_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_SRN_DETAIL ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_SRN_Price_Charge_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MILK_SRN_VSP_Charge_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE Tspl_Milk_Truck_Sheet_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)

                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_Milk_Purchase_Invoice_Incentive_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MCC_RATE_UPLOADER_MCC ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_MCC_RATE_UPLOADER_Detail ALTER COLUMN pk_id add NOT FOR REPLICATION", trans)
                qry = " ALTER TABLE TSPL_VLC_DATA_UPLOADER" & _
                      " ADD CONSTRAINT   PK_TSPL_VLC_DATA_UPLOADER PRIMARY KEY (Doc_No,PK_Id);"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_VLC_DATA_UPLOADER_Detail" & _
                      " ADD CONSTRAINT   PK_TSPL_VLC_DATA_UPLOADER_Detail PRIMARY KEY (Document_Code,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_INVENTORY_MOVEMENT_NEW" & _
                      " ADD CONSTRAINT   PK_TSPL_INVENTORY_MOVEMENT_NEW PRIMARY KEY (Source_Doc_No,Trans_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_JOURNAL_Details" & _
                      " ADD CONSTRAINT   PK_TSPL_JOURNAL_Details PRIMARY KEY (Voucher_No,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_RECEIPT_DETAIL" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_RECEIPT_DETAIL PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_SAMPLE_DETAIL" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_SAMPLE_DETAIL PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_SAMPLE_DETAIL_History" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_SAMPLE_DETAIL_History PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_Shift_End_DETAIL" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_Shift_End_DETAIL PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_SRN_DETAIL" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_SRN_DETAIL PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MILK_SRN_Price_Charge_Detail" & _
                      " ADD CONSTRAINT   PK_TSPL_MILK_SRN_Price_Charge_Detail PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE Tspl_Milk_Truck_Sheet_Detail" & _
                      " ADD CONSTRAINT   PK_Tspl_Milk_Truck_Sheet_Detail PRIMARY KEY (DOC_CODE,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_Milk_Purchase_Invoice_Incentive_Detail" & _
                      " ADD CONSTRAINT   PK_TSPL_Milk_Purchase_Invoice_Incentive_Detail PRIMARY KEY (AP_Invoice_Code,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MCC_RATE_UPLOADER_MCC" & _
                      " ADD CONSTRAINT   PK_TSPL_MCC_RATE_UPLOADER_MCC PRIMARY KEY (Code,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " ALTER TABLE TSPL_MCC_RATE_UPLOADER_Detail" & _
                      " ADD CONSTRAINT   PK_TSPL_MCC_RATE_UPLOADER_Detail PRIMARY KEY (Code,PK_Id)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        ' Priti 14-Nov-2014
        If (clsCommon.CompairString("5.0.3.66", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.66") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_TRANSACTION_APPROVAL", "Document_No", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.70") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " alter table TSPL_ADJUSTMENT_HEADER alter column description varchar(300)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.70") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " update TSPL_EMPLOYEE_MASTER set emp_type='Salesman'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.70") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_MCC_Dispatch_Challan", "Payment_Rate", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    qry = " alter table TSPL_MCC_Dispatch_Challan alter column Payment_Rate varchar(5000) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If



                If Not CheckPrimaryKey("tspl_inventory_movement_new", "PI_Cost", trans, True) Then
                    qry = " alter table tspl_inventory_movement_new add default 0.00 for PI_Cost"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If Not CheckPrimaryKey("tspl_journal_master", "Provisional_Post", trans, True) Then
                    qry = " alter table tspl_journal_master add default 'N' for Provisional_Post"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                ' It Should be 'N'
                If Not CheckPrimaryKey("tspl_journal_master", "Authorized", trans, True) Then
                    qry = " alter table tspl_journal_master add default 'A' for Authorized"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                If Not CheckPrimaryKey("tspl_journal_master", "sendToTally", trans, True) Then
                    qry = "alter table tspl_journal_master add default 0 for sendToTally"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                If Not CheckPrimaryKey("tspl_journal_master", "ConvRate", trans, True) Then
                    qry = "alter table tspl_journal_master add default 1.000000 for ConvRate"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                If Not CheckPrimaryKey("tspl_journal_master", "ConvRateOld", trans, True) Then
                    qry = "alter table tspl_journal_master add default 1.000000 for ConvRateOld"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.74", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.74") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_physical_stock", "Item_Code", trans)
                DropConstraint("tspl_physical_stock", "Location", trans)
                DropConstraint("tspl_physical_stock", "MRP", trans)
                DropConstraint("tspl_physical_stock", "Stock_Date", trans)
                qry = "alter table tspl_bank_guarantee_master alter column extended_date varchar(50)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_bank_guarantee_master alter column end_date varchar(50)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_bank_guarantee_master alter column start_date varchar(50)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.80", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.80") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_dontdeletecreatedsample_sampleno_fromReceipt", trans) = 0 Then
                    qry = "Create trigger [dbo].[trg_dontdeletecreatedsample_sampleno_fromReceipt] on [dbo].[TSPL_milk_receipt_detail] " _
                            & " for delete " _
                            & " as " _
                            & " begin try " _
                            & " declare @POstFlag as integer  " _
                            & " declare @Doc_Code as varchar(30) " _
                            & " declare @Sample_No as integer " _
                            & " Select @Doc_Code=i.doc_code from deleted i; " _
                            & " Select @Sample_No=i.sample_No from deleted i; " _
                            & " select @POstFlag=count(*) from TSPL_MILK_SAMPLE_DETAIL where   (doc_code =(select DOC_CODE from TSPL_MILK_SAMPLE_HEAD where MILK_RECEIPT_CODE=@Doc_Code) and sample_No =@Sample_No ) " _
                            & " if  @POstFlag>0 " _
                            & " raiserror ('Cannot delete entry',16,1) " _
                            & " Rollback " _
                            & " end try  " _
                            & " begin catch  " _
                            & " raiserror ('Cannot delete entry',16,1) " _
                            & " Rollback " _
                            & " end catch  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckTriggerExits("trg_dontdeletecreatedsrnsampleno", trans) = 0 Then
                    qry = "create trigger [dbo].[trg_dontdeletecreatedsrnsampleno] on [dbo].[TSPL_milk_sample_detail] " _
                                & " for delete   " _
                                & " as " _
                                & " begin try " _
                                & " declare @POstFlag as integer " _
                                & " declare @Doc_Code as varchar(30) " _
                                & " declare @Sample_No as integer " _
                                & " Select @Doc_Code=i.doc_code from deleted i; " _
                                & " Select @Sample_No=i.sample_No from deleted i; " _
                                & " select @POstFlag=count(*) from TSPL_milk_srn_Head where   (Milk_sample_Code =@Doc_Code and sample_No =@Sample_No ) " _
                                & " if  @POstFlag>0 " _
                                & " raiserror ('Cannot delete entry',16,1) " _
                                & " Rollback  " _
                                & " end try " _
                                & " begin catch " _
                                & " raiserror ('Cannot delete entry',16,1) " _
                                & " Rollback " _
                                & " end catch "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.85", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.85") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PP_STAGE_PROCESS_HEAD alter column Issue_Code varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.3.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.86") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_SECTION_STAGE_MAPPING alter column Log_Sheet_No varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '--------------------------
        '=============Richa  on 12/01/2015
        If (clsCommon.CompairString("5.0.3.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.86", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "UPDATE TSPL_INVOICE_DETAIL_BULKSALE SET InvoiceFatKG=ROUND(((InvoiceQty * InvoiceFatPer )/100),3),InvoiceSNFKG =ROUND(((InvoiceQty * InvoiceSNFPer   )/100),3)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '--- Pankaj Jha 14-01-2015
        If (clsCommon.CompairString("5.0.3.88", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.88", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "delete from TSPL_PROGRAM_MASTER where Program_Code='MMproc'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.89", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.89", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_mcc_dispatch_challan", "mcc_code", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.97", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.97", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("tspl_quality_check", "remarks", DBDataType.varchar_Type, 200, 0, trans)
                If check > 0 Then
                    qry = "alter table tspl_quality_check alter column remarks varchar(8000)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        'richa agarwal
        If (clsCommon.CompairString("5.0.3.98", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.98", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_LC_CREATION_MT", "FDPeriod", DBDataType.float_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_LC_CREATION_MT", "FDPeriod", trans)
                    qry = "alter table TSPL_LC_CREATION_MT drop column FDPeriod"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                check = CheckColumnExist("TSPL_LC_CREATION_MT", "FDPeriodType", DBDataType.varchar_Type, 30, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_LC_CREATION_MT drop column FDPeriodType"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.3.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.3.99", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_LC_CREATION_MT", "LCNo", DBDataType.varchar_Type, 30, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_LC_CREATION_MT drop column LCNo"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '==============================================
        '=====================ADD TRIGGER IN OPEN MCCC SHIFT=================
        If (clsCommon.CompairString("5.0.3.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.3.99") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_dontdeleteOpenShift", trans) = 0 Then
                    qry = "create trigger [dbo].[trg_dontdeleteOpenShift] on [dbo].[TSPL_OPEN_MCC_SHIFT] " _
                          & " for delete " _
                          & " as " _
                          & "  begin" _
                          & "  declare @POstFlag as integer " _
                          & "  declare @Mcc_Code as varchar(30) " _
                          & "   declare @Shift as Varchar(1) " _
                          & "  declare @Doc_Date as date " _
                          & " Select @Mcc_Code=i.Mcc_code from deleted i; " _
                          & " Select @Shift=i.Shift from deleted i;" _
                          & " Select @Doc_Date=i.Mcc_Shift_Date from deleted i;" _
                          & "   select @POstFlag=count(*) from TSPL_MILK_Receipt_Head where   (Mcc_code =@Mcc_code and shift =@Shift and doc_date= @Doc_Date) " _
                          & "  if  @POstFlag>0" _
                          & "         begin " _
                          & "  raiserror ('Cannot delete entry',16,1) " _
                          & "  Rollback tran;" _
                          & "     End" _
                          & " end "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ' ======================================================================
        '=====================ADD TRIGGER IN VSP Charge Detail=================
        If (clsCommon.CompairString("5.0.4.57", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.57") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_CreateChargeHistory_update", trans) = 0 Then
                    qry = "Create trigger [dbo].[trg_CreateChargeHistory_update] on [dbo].[TSPL_MCC_VSP_ChargeCategory_MAPPING] " _
                          & " for update   as  begin try  " _
                          & "  declare @VSP_Code as varchar(30) " _
                          & " declare @Charge_Code as varchar(30) " _
                          & " Select @VSP_Code=i.VSP_COde from deleted i; " _
                          & "Select @Charge_Code=i.Charge_COde from deleted i; " _
                          & "  insert into TSPL_MCC_VSP_ChargeCategory_MAPPING_history(vsp_code,charge_code,Rate,history_By,History_date)  " _
                          & " select vsp_code,charge_code,Rate,(select Max(Modify_By) from tspl_Vendor_master where Vendor_code =@VSP_Code),(select Max(Updated_date) " _
                          & " from TSPL_MCC_VSP_ChargeCategory_MAPPING where Vsp_code =@VSP_Code and Charge_CODE=@charge_code) from deleted " _
                          & "   end try  " _
                          & " begin catch " _
                          & " end catch  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '======================================================================
        If (clsCommon.CompairString("5.0.4.12", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.12") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_PP_ISSUE_ITEM_DETAIL", "from_loaction_code", DBDataType.varchar_Type, 12, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_PP_ISSUE_ITEM_DETAIL alter column from_loaction_code varchar(12) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_PP_ISSUE_ITEM_DETAIL", "to_location_code", DBDataType.varchar_Type, 12, 0, trans)
                If check > 0 Then
                    qry = "alter table TSPL_PP_ISSUE_ITEM_DETAIL alter column to_location_code varchar(12) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.14", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.14") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  alter table tspl_item_master add FOREIGN KEY(GL_Account) references TSPL_GL_ACCOUNTS(Account_Code)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.19", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.4.19", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_EX_COMMERCIAL_INVOICE_DETAIL", "shipping_mark", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_COMMERCIAL_INVOICE_DETAIL alter column shipping_mark varchar(100) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.29", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.4.29", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("tspl_mrp_head", "BOM_CODE", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_mrp_head alter column BOM_CODE varchar(30) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.32", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.4.32", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_MRP_PO_DETAIL", "Bill_To_Location", DBDataType.varchar_Type, 12, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MRP_PO_DETAIL alter column Bill_To_Location varchar(12) null", trans)
                End If
                If CheckColumnExist("TSPL_MRP_PO_DETAIL", "PurchaseOrder_No", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MRP_PO_DETAIL alter column PurchaseOrder_No varchar(30) null", trans)
                End If
                If CheckColumnExist("TSPL_MRP_PO_DETAIL", "PurchaseOrder_Date", DBDataType.datetime_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MRP_PO_DETAIL alter column PurchaseOrder_Date datetime null", trans)
                End If
                If CheckColumnExist("tspl_mrp_head", "MRP_REMARKS", DBDataType.varchar_Type, -1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_mrp_head alter column MRP_REMARKS varchar(max) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.34", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.34") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                check = CheckColumnExist("tspl_journal_master_history", "History_By", DBDataType.varchar_Type, 0, 0, trans)

                If check <= 0 Then
                    'qry = "drop table tspl_journal_master_history"
                    'clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    'qry = "drop table tspl_journal_details_history"
                    'clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("tspl_journal_master_history", "Last_Modify_By", DBDataType.varchar_Type, 0, 0, trans)

                If check > 0 Then
                    qry = "Alter table tspl_journal_master_history drop column Last_Modify_By"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    qry = "Alter table tspl_journal_master_history drop column Last_Modify_date"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("tspl_milk_srn_detail_history", "Last_Modify_By", DBDataType.varchar_Type, 0, 0, trans)

                If check > 0 Then
                    qry = "Alter table tspl_milk_srn_detail_history drop column Last_Modify_By"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    qry = "Alter table tspl_milk_srn_detail_history drop column Last_Modify_date"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckTriggerExits("trg_CreatejournalEntryHistory", trans) = 0 Then
                    qry = "Create trigger [dbo].[trg_CreatejournalEntryHistory] on [dbo].[tspl_journal_details]  " _
                      & " for delete    " _
                      & "   as    " _
                      & "     begin try   " _
                      & "     declare @POstFlag as integer     " _
                      & "     declare @Doc_Code as varchar(30)  " _
                      & "     Select @Doc_Code=i.voucher_No from deleted i;    " _
                      & "     select @POstFlag=count(*) from TSPL_Journal_Master where   (Voucher_No =@Doc_Code and Authorized ='A' )  " _
                      & "     if  @POstFlag>0    " _
                      & "       insert into tspl_journal_master_History(Journal_No,Voucher_No,Voucher_Date,Source_Code,Source_Desc,Source_Doc_No,Source_Doc_Date,Posting_Date,Voucher_Desc,Source_Narration,Remarks,Comments,Auto_Reverse,Reverse_Date,Source_Type,CustVend_Code,CustVend_Name,Transaction_Type,Provisional_Post,Authorized,Total_Debit_Amt,Total_Credit_Amt,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Type,SendToTally,CURRENCY_CODE,ConvRate,ApplicableFrom,ConvRateOld,Segment_code) select Journal_No,Voucher_No,Voucher_Date,Source_Code,Source_Desc,Source_Doc_No,Source_Doc_Date,Posting_Date,Voucher_Desc,Source_Narration,Remarks,Comments,Auto_Reverse,Reverse_Date,Source_Type,CustVend_Code,CustVend_Name,Transaction_Type,Provisional_Post,Authorized,Total_Debit_Amt,Total_Credit_Amt,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Type,SendToTally,CURRENCY_CODE,ConvRate,ApplicableFrom,ConvRateOld,Segment_code   " _
                      & "    from tspl_journal_master  where Voucher_No =@Doc_Code  " _
                      & "     if  @POstFlag>0    " _
                      & " 	insert into tspl_journal_details_history(journal_No,voucher_No,Detail_Line_No,Account_COde,Account_Desc,Amount,Description,reference,Posting_date" _
                      & " ,Account_Type,Account_group_Code,Account_Seg_Code1,Account_Seg_Desc1,Account_Seg_Code2,Account_Seg_Desc2,Account_Seg_Code3,Account_Seg_Desc3," _
                      & " Account_Seg_Code4,Account_Seg_Desc4,Account_Seg_Code5,Account_Seg_Desc5,Account_Seg_Code6,Account_Seg_Desc6,Account_Seg_Code7,Account_Seg_Desc7" _
                      & " ,Account_Seg_Code8,Account_Seg_Desc8,Account_Seg_Code9,Account_Seg_Desc9,Account_Seg_Code10,Account_Seg_Desc10,custVend_Code,CustVend_Name) " _
                      & " select journal_No,voucher_No,Detail_Line_No,Account_COde,Account_Desc,Amount,Description,reference,Posting_date,Account_Type,Account_group_Code," _
                      & " Account_Seg_Code1,Account_Seg_Desc1,Account_Seg_Code2,Account_Seg_Desc2,Account_Seg_Code3,Account_Seg_Desc3,Account_Seg_Code4,Account_Seg_Desc4," _
                      & " Account_Seg_Code5,Account_Seg_Desc5,Account_Seg_Code6,Account_Seg_Desc6,Account_Seg_Code7,Account_Seg_Desc7,Account_Seg_Code8,Account_Seg_Desc8," _
                      & " Account_Seg_Code9,Account_Seg_Desc9,Account_Seg_Code10,Account_Seg_Desc10,custVend_Code,CustVend_Name from deleted " _
                      & " end try " _
                      & " begin catch " _
                      & " end catch "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckTriggerExits("trg_CreatejournalEntryHistory_update", trans) = 0 Then
                    qry = "Create trigger [dbo].[trg_CreatejournalEntryHistory_update] on [dbo].[tspl_journal_Master]  " _
                          & " for update  " _
                          & " as " _
                          & " begin try " _
                          & " declare @POstFlag as integer   " _
                          & " declare @Doc_Code as varchar(30) " _
                          & " Select @Doc_Code=i.voucher_No from deleted i;  " _
                          & "select @POstFlag=count(*) from TSPL_Journal_Master where   (Voucher_No =@Doc_Code and Authorized ='A' )" _
                          & " if  @POstFlag>0 and update(Total_Credit_Amt)   " _
                          & "  insert into tspl_journal_master_History(Journal_No,Voucher_No,Voucher_Date,Source_Code,Source_Desc,Source_Doc_No,Source_Doc_Date,Posting_Date," _
                          & " Voucher_Desc,Source_Narration,Remarks,Comments,Auto_Reverse,Reverse_Date,Source_Type,CustVend_Code,CustVend_Name,Transaction_Type,Provisional_Post" _
                          & " ,Authorized,Total_Debit_Amt,Total_Credit_Amt,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Type,SendToTally,CURRENCY_CODE,ConvRate," _
                          & " ApplicableFrom,ConvRateOld,Segment_code) select Journal_No,Voucher_No,Voucher_Date,Source_Code,Source_Desc,Source_Doc_No,Source_Doc_Date," _
                          & " Posting_Date,Voucher_Desc,Source_Narration,Remarks,Comments,Auto_Reverse,Reverse_Date,Source_Type,CustVend_Code,CustVend_Name,Transaction_Type," _
                          & " Provisional_Post,Authorized,Total_Debit_Amt,Total_Credit_Amt,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code,Type,SendToTally,CURRENCY_CODE" _
                          & " ,ConvRate,ApplicableFrom,ConvRateOld,Segment_code from deleted " _
                          & " if  @POstFlag>0 and update(Total_Credit_Amt)   " _
                          & "  update tspl_journal_master_History set History_By=(select Modify_By from tspl_journal_master where Voucher_No =@Doc_Code)," _
                          & " History_Date=(select Posting_date from tspl_journal_master where Voucher_No =@Doc_Code) where voucher_No=@Doc_Code " _
                          & " if  @POstFlag>0 and update(Total_Credit_Amt)   " _
                          & " 	insert into tspl_journal_details_history(journal_No,voucher_No,Detail_Line_No,Account_COde,Account_Desc,Amount,Description," _
                          & " reference,Posting_date,Account_Type,Account_group_Code,Account_Seg_Code1,Account_Seg_Desc1,Account_Seg_Code2,Account_Seg_Desc2," _
                          & " Account_Seg_Code3,Account_Seg_Desc3,Account_Seg_Code4,Account_Seg_Desc4,Account_Seg_Code5,Account_Seg_Desc5,Account_Seg_Code6," _
                          & " Account_Seg_Desc6,Account_Seg_Code7,Account_Seg_Desc7,Account_Seg_Code8,Account_Seg_Desc8,Account_Seg_Code9,Account_Seg_Desc9," _
                          & " Account_Seg_Code10,Account_Seg_Desc10,custVend_Code,CustVend_Name) select journal_No,voucher_No,Detail_Line_No,Account_COde," _
                          & " Account_Desc,Amount,Description,reference,Posting_date,Account_Type,Account_group_Code,Account_Seg_Code1,Account_Seg_Desc1," _
                          & " Account_Seg_Code2,Account_Seg_Desc2,Account_Seg_Code3,Account_Seg_Desc3,Account_Seg_Code4,Account_Seg_Desc4,Account_Seg_Code5," _
                          & " Account_Seg_Desc5,Account_Seg_Code6,Account_Seg_Desc6,Account_Seg_Code7,Account_Seg_Desc7,Account_Seg_Code8,Account_Seg_Desc8," _
                          & " Account_Seg_Code9,Account_Seg_Desc9,Account_Seg_Code10,Account_Seg_Desc10,custVend_Code,CustVend_Name from tspl_Journal_details " _
                          & " where voucher_No=@Doc_Code " _
                          & " end try " _
                          & " begin catch " _
                          & " end catch  "

                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.36", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.36") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_MF_PROD_PLAN_DETAIL", "BOM_CODE", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MF_PROD_PLAN_DETAIL alter column BOM_CODE varchar(30) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.36", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.36") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_INVENTORY_MOVEMENT_NEW", "IS_CONSUMPTION", DBDataType.bit_Type, 0, Nothing, trans) Then
                    qry = "  DECLARE @ObjectName NVARCHAR(100)" & _
                          "  SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS " & _
                          "  WHERE [object_id] = OBJECT_ID('TSPL_INVENTORY_MOVEMENT_NEW') AND [name] = 'IS_CONSUMPTION'; " & _
                          "  if LEN(@ObjectName)>0 " & _
                          "  EXEC('ALTER TABLE TSPL_INVENTORY_MOVEMENT_NEW DROP CONSTRAINT ' + @ObjectName) " & _
                          "  alter table TSPL_INVENTORY_MOVEMENT_NEW alter column IS_CONSUMPTION integer not null  ;" & _
                          "  ALTER TABLE TSPL_INVENTORY_MOVEMENT_NEW ADD CONSTRAINT DF_TSPL_INVENTORY_MOVEMENT_NEW_IS_CONSUMPTION DEFAULT 0 FOR IS_CONSUMPTION "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckColumnExist("TSPL_INVENTORY_MOVEMENT", "IS_CONSUMPTION", DBDataType.bit_Type, 0, Nothing, trans) Then
                    qry = "  DECLARE @ObjectName NVARCHAR(100)" & _
                          "  SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS " & _
                          "  WHERE [object_id] = OBJECT_ID('TSPL_INVENTORY_MOVEMENT') AND [name] = 'IS_CONSUMPTION'; " & _
                          "  if LEN(@ObjectName)>0 " & _
                          "  EXEC('ALTER TABLE TSPL_INVENTORY_MOVEMENT DROP CONSTRAINT ' + @ObjectName) " & _
                          "  alter table TSPL_INVENTORY_MOVEMENT alter column IS_CONSUMPTION integer not null  ;" & _
                          "  ALTER TABLE TSPL_INVENTORY_MOVEMENT ADD CONSTRAINT DF_TSPL_INVENTORY_MOVEMENT_IS_CONSUMPTION DEFAULT 0 FOR IS_CONSUMPTION "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckColumnExist("TSPL_PP_PRODUCTION_CONSUMPTION_DETAIL", "PROD_ENTRY_CODE", DBDataType.varchar_Type, 30, Nothing, trans) Then

                    qry = "  alter table TSPL_PP_PRODUCTION_CONSUMPTION_DETAIL alter column PROD_ENTRY_CODE varchar(30) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.37", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.37") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_GRN_HEAD", "PurchaseOrder_Type", DBDataType.varchar_Type, 1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_GRN_HEAD set TSPL_GRN_HEAD.PurchaseOrder_Type=(select purchaseorder_type from TSPL_PURCHASE_ORDER_HEAD where TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No=TSPL_GRN_HEAD.Against_PO) where ISNULL(TSPL_GRN_HEAD.PurchaseOrder_Type,'')=''", trans)
                End If
                If CheckColumnExist("TSPL_MRN_HEAD", "PurchaseOrder_Type", DBDataType.varchar_Type, 1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_MRN_HEAD set TSPL_MRN_HEAD.PurchaseOrder_Type=(select purchaseorder_type from TSPL_GRN_HEAD where TSPL_GRN_HEAD.GRN_No=TSPL_MRN_HEAD.Against_GRN) where ISNULL(TSPL_MRN_HEAD.PurchaseOrder_Type,'')=''", trans)
                End If
                If CheckColumnExist("TSPL_SRN_HEAD", "PurchaseOrder_Type", DBDataType.varchar_Type, 1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_SRN_HEAD set TSPL_SRN_HEAD.PurchaseOrder_Type=(select purchaseorder_type from TSPL_PURCHASE_ORDER_HEAD where TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No=TSPL_SRN_HEAD.Against_PO) where ISNULL(TSPL_SRN_HEAD.PurchaseOrder_Type,'')=''", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.40", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.40") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE tspl_mp_master alter column city_code varchar (50) null", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE tspl_mp_master alter column state_code varchar (30) null", trans)
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE tspl_sd_sale_invoice_head alter column Comments varchar (500) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        '5.0.4.39
        If (clsCommon.CompairString("5.0.4.41", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.41") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("Tspl_Trainer_Master_Course", "Course_Code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    DropConstraint("Tspl_Trainer_Master_Course", "Course_Code", trans)
                End If
                If CheckColumnExist("TSPL_Schedule_Training_Employee_DETAIL", "DOC_DOCE", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    DropConstraint("TSPL_Schedule_Training_Employee_DETAIL", "DOC_DOCE", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_Schedule_Training_Employee_DETAIL drop column  DOC_DOCE ", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.41", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.4.41") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_BANK_CHECK_PRINTING_STATUS", "CHECK_CODE", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_BANK_CHECK_PRINTING_STATUS alter column CHECK_CODE VARCHAR(30) NULL", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ' Richa 20/02/2015 against ticket no BM00000005629
        If (clsCommon.CompairString("5.0.4.42", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.42") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "Delete from TSPL_PROGRAM_MASTER where Program_Code in ('STR-LGR-RPT','DRN-SUM-RPT','DLY-REC-N','RM-CONS-RPT')  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.44", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.44") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_PARAMETER_RANGE_MASTER_QC", "Code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_parameter_range_master_qc alter column code varchar(30) null", trans)
                End If
                If CheckColumnExist("TSPL_PARAMETER_RANGE_MASTER_QC_HISTORY", "Code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PARAMETER_RANGE_MASTER_QC_HISTORY alter column code varchar(30) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.48", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.48") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_CSA_TRANSFER_DETAIL", "DELEVERY_ORDER_NO", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_CSA_TRANSFER_DETAIL set tspl_csa_transfer_detail.DELEVERY_ORDER_NO=(select DELEVERY_ORDER_NO from TSPL_CSA_TRANSFER_HEAD where TSPL_CSA_TRANSFER_DETAIL.DOC_CODE=TSPL_CSA_TRANSFER_HEAD.DOC_CODE) where len(isnull(TSPL_CSA_TRANSFER_DETAIL.DELEVERY_ORDER_NO,''))<=0 and ISNULL(TSPL_CSA_TRANSFER_DETAIL.FOC,'N')<>'Y'", trans)
                End If
                If CheckColumnExist("TSPL_CSA_TRANSFER_DETAIL", "Do_qty", DBDataType.float_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_CSA_TRANSFER_DETAIL set tspl_csa_transfer_detail.Do_qty=(select qty from TSPL_CSA_DO_DETAIL where TSPL_CSA_DO_DETAIL.Doc_No=TSPL_CSA_TRANSFER_DETAIL.delevery_order_no and TSPL_CSA_DO_DETAIL.Item_Code=TSPL_CSA_TRANSFER_DETAIL.Item_Code) where ISNULL(TSPL_CSA_TRANSFER_DETAIL.FOC,'N')<>'Y'", trans)
                End If
                If CheckColumnExist("TSPL_CSA_TRANSFER_DETAIL", "DO_Pending_Qty", DBDataType.float_Type, Nothing, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_CSA_TRANSFER_DETAIL set DO_Pending_Qty=DO_Qty where ISNULL(do_pending_qty,'')=''", trans)
                End If
                If CheckColumnExist("TSPL_PURCHASE_ORDER_HEAD", "Comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PURCHASE_ORDER_HEAD alter column Comments text", trans)
                End If
                If CheckColumnExist("TSPL_PP_ISSUE_HEAD", "Batch_Code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_pp_issue_head alter column Batch_Code varchar(30) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.54", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.54") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "Delete from TSPL_PROGRAM_MASTER where Program_Code in ('CRATE_ACC_FS')  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.56", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.56") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_PP_BATCH_ORDER_BOM_DETAIL", "Plan_Code", DBDataType.varchar_Type, 30, Nothing, trans) Then
                    qry = "update TSPL_PP_BATCH_ORDER_BOM_DETAIL set plan_code=(select plan_code from TSPL_PP_BATCH_ORDER_HEAD where TSPL_PP_BATCH_ORDER_BOM_DETAIL.Batch_Code=TSPL_PP_BATCH_ORDER_HEAD.Batch_Code) where len(ISNULL(plan_code,''))<=0 and Item_Code in (select Item_Code from TSPL_PP_PRODUCTION_PLAN_DETAIL where Plan_Code in (select plan_code from TSPL_PP_BATCH_ORDER_HEAD))"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        'richa agarwal 24/03/2015
        If (clsCommon.CompairString("5.0.4.65", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.65") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckColumnExist("TSPL_Vendor_Bank_MASTER", "IFSC_Code", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_Vendor_Bank_MASTER alter column IFSC_Code varchar (200) null", trans)
                End If
                If CheckColumnExist("TSPL_Vendor_Bank_MASTER", "Branch_Code", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_Vendor_Bank_MASTER alter column Branch_Code varchar (200) null", trans)
                End If
                If CheckColumnExist("TSPL_Vendor_Bank_MASTER", "Branch_Name", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_Vendor_Bank_MASTER alter column Branch_Name varchar (200) null", trans)
                End If
                If CheckColumnExist("tspl_pp_bom_head", "revision_no", DBDataType.varchar_Type, 100, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update tspl_pp_bom_head set revision_no=bom_code+'.'+(select cast(nos as varchar) from (select count(tspl_pp_bom_head_history.bom_code) as nos,tspl_pp_bom_head_history.bom_code from tspl_pp_bom_head_history where tspl_pp_bom_head_history.bom_code=tspl_pp_bom_head.bom_code group by bom_code)a)", trans)

                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD_HISTORY set Revision_No=(select ax.rev from (select a.BOM_CODE+'.'+CAST(a.sno as varchar) as rev,a.History_No,a.BOM_CODE,a.Modified_Date from (select ROW_NUMBER() over(partition by bom_code order by modified_date) as sno,History_No,Modified_Date,BOM_CODE,Revision_No from TSPL_PP_BOM_HEAD_HISTORY)a)ax where ax.History_No=TSPL_PP_BOM_HEAD_HISTORY.History_No)", trans)
                End If
                clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD set STATUS='Approved' where Is_Post=1 and STATUS<>'Approved'", trans)

                clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BATCH_ORDER_HEAD set Description=(select a.descp from (select case when isnull(description,'')<>'' then substring(description,3,len(description)-2) end as descp,Batch_Code from TSPL_PP_BATCH_ORDER_HEAD where (case when isnull(description,'')<>'' then left(description,3) end)='Is')a where a.Batch_Code=TSPL_PP_BATCH_ORDER_HEAD.Batch_Code)", trans)

                clsDBFuncationality.ExecuteNonQuery("insert into TSPL_QC_LOG_SHEET_USER_MASTER select a.code,b.user_code from (select code,1 as id from TSPL_QC_LOG_SHEET_MASTER)a left outer join (select user_code,1 as id from TSPL_USER_MASTER)b on a.id=b.id where a.Code not in (select Code from TSPL_QC_LOG_SHEET_USER_MASTER)", trans)

                clsDBFuncationality.ExecuteNonQuery("insert into TSPL_SECTION_STAGE_USER_DETAIL select distinct TSPL_SECTION_STAGE_MAPPING_HEAD.Section_Code,TSPL_SECTION_STAGE_MAPPING.Stage_Code,aa.Emp_Code,TSPL_SECTION_STAGE_MAPPING_HEAD.Doc_Code,TSPL_SECTION_STAGE_MAPPING_HEAD.Comp_Code from TSPL_SECTION_STAGE_MAPPING_HEAD left outer join TSPL_SECTION_STAGE_MAPPING on TSPL_SECTION_STAGE_MAPPING.Section_Code=TSPL_SECTION_STAGE_MAPPING_HEAD.Section_Code and TSPL_SECTION_STAGE_MAPPING.Doc_Code=TSPL_SECTION_STAGE_MAPPING_HEAD.Doc_Code left outer join (select emp_code,TSPL_PP_LOG_SHEET_DETAIL.Doc_No from TSPL_QC_LOG_SHEET_USER_MASTER left outer join TSPL_PP_LOG_SHEET_DETAIL on TSPL_PP_LOG_SHEET_DETAIL.Parameter_Code=TSPL_QC_LOG_SHEET_USER_MASTER.Code)aa on aa.Doc_No=TSPL_SECTION_STAGE_MAPPING.Log_Sheet_No where isnull(TSPL_SECTION_STAGE_MAPPING.Log_Sheet_No,'') <>'' and TSPL_SECTION_STAGE_MAPPING_HEAD.Doc_Code not in (select Doc_Code from TSPL_SECTION_STAGE_USER_DETAIL)", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '---------------------
        '===================== CHANGES IN PAYMENT HEADER TRIGGER 30-Mar-2015 =================


        If (clsCommon.CompairString("5.0.4.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.70") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_isApInvoiceExits", trans) = 0 Then
                    qry = "Create trigger [dbo].[trg_isApInvoiceExits] on [dbo].[TSPL_JOURNAL_MASTER] " _
                            & " for insert " _
                            & " as" _
                            & " declare @POstFlag as integer " _
                            & " declare @Source_Code as varchar(30), @Desc Varchar(500) " _
                            & " declare @Source_Doc_No as varchar(30) " _
                            & " Select @Source_Code=i.Source_Code from Inserted i; " _
                            & " Select @Source_Doc_No=i.Source_Doc_No, @Desc=i.Voucher_Desc from Inserted i; " _
                            & " if @Source_Code='AP-IN'  " _
                           & " select @POstFlag=count(*) from TSPL_VENDOR_INVOICE_HEAD where Document_No =@Source_Doc_No  " _
                           & " if  @POstFlag<1 " _
                           & " begin  " _
                           & " Print 'Document No : ' + @Source_Doc_No" _
                           & " rollback " _
                           & " raiserror ('No AP Invoice Entry Exits',16,1) " _
                           & " End"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.70") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_isJournalEntryExits", trans) = 0 Then
                    qry = " Create trigger [dbo].[trg_isJournalEntryExits] on [dbo].[TSPL_Vendor_Invoice_Head] " _
                        & " for Delete  " _
                        & " as " _
                        & " declare @POstFlag as integer " _
                        & " declare @Source_Code as varchar(30), @Desc Varchar(500) " _
                        & " declare @Source_Doc_No as varchar(30) " _
                        & " Select @Source_Code=i.Document_No from deleted i; " _
                        & " select @POstFlag=count(*) from TSPL_Journal_master where Source_Doc_No =@Source_Code and Source_Code='AP-IN'  " _
                        & " if  @POstFlag>0 " _
                       & " begin " _
                       & " Print 'Document No : ' + @Source_Code " _
                       & " rollback " _
                       & " raiserror ('Journal Entry Exits',16,1) " _
                       & " End "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.81", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.81") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_mcc_dispatch_challan", "Tanker_No", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.82", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.82") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_SD_SALES_Quotation_HEAD", "SalesOrder_Type", DBDataType.varchar_Type, 1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_Quotation_HEAD alter column SalesOrder_Type varchar(2)", trans)
                End If
                If CheckColumnExist("TSPL_SD_SALES_Quotation_HEAD", "Mode_Of_Transport", DBDataType.varchar_Type, 12, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_sd_sales_quotation_head alter column Mode_Of_Transport varchar(max)", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.4.82", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.82") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_pp_standardization_head alter column Created_Date datetime", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_pp_standardization_head alter column Modified_Date datetime", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_STAGE_PROCESS_HEAD alter column Created_Date datetime", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_STAGE_PROCESS_HEAD alter column Modified_Date datetime", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.87", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.87") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("tspl_pp_bom_head", "created_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD set created_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_pp_bom_head alter column created_date datetime", trans)
                End If
                If CheckColumnExist("tspl_pp_bom_head", "modified_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD set modified_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_pp_bom_head alter column modified_date datetime", trans)
                End If

                If CheckColumnExist("TSPL_PP_BOM_HEAD_HISTORY", "created_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD_HISTORY set created_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BOM_HEAD_HISTORY alter column created_date datetime", trans)
                End If
                If CheckColumnExist("TSPL_PP_BOM_HEAD_HISTORY", "modified_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BOM_HEAD_HISTORY set modified_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BOM_HEAD_HISTORY alter column modified_date datetime", trans)
                End If

                If CheckColumnExist("TSPL_PP_PRODUCTION_PLAN_HEAD", "created_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_PRODUCTION_PLAN_HEAD set created_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_PRODUCTION_PLAN_HEAD alter column created_date datetime", trans)
                End If
                If CheckColumnExist("TSPL_PP_PRODUCTION_PLAN_HEAD", "modified_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_PRODUCTION_PLAN_HEAD set modified_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_PRODUCTION_PLAN_HEAD alter column modified_date datetime", trans)
                End If

                If CheckColumnExist("TSPL_PP_BATCH_ORDER_HEAD", "created_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BATCH_ORDER_HEAD set created_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BATCH_ORDER_HEAD alter column created_date datetime", trans)
                End If
                If CheckColumnExist("TSPL_PP_BATCH_ORDER_HEAD", "modified_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_BATCH_ORDER_HEAD set modified_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_BATCH_ORDER_HEAD alter column modified_date datetime", trans)
                End If

                If CheckColumnExist("TSPL_PP_ISSUE_HEAD", "created_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_ISSUE_HEAD set created_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_ISSUE_HEAD alter column created_date datetime", trans)
                End If
                If CheckColumnExist("TSPL_PP_ISSUE_HEAD", "modified_date", DBDataType.varchar_Type, 10, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("update TSPL_PP_ISSUE_HEAD set modified_date=convert(date,modified_date,103)", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_ISSUE_HEAD alter column modified_date datetime", trans)
                End If

                If CheckColumnExist("TSPL_EX_PI_HEAD_HISTORY", "Pre_Carriage_By", DBDataType.varchar_Type, 100, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD_HISTORY alter column Pre_Carriage_By varchar(max)", trans)
                End If
                If CheckColumnExist("TSPL_EX_PI_HEAD", "Pre_Carriage_By", DBDataType.varchar_Type, 100, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_PI_HEAD alter column Pre_Carriage_By varchar(max)", trans)
                End If

                If CheckColumnExist("TSPL_EX_COMMERCIAL_INVOICE_HEAD", "Pre_Carriage_By", DBDataType.varchar_Type, 100, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_EX_COMMERCIAL_INVOICE_HEAD alter column Pre_Carriage_By varchar(max)", trans)
                End If

                If CheckColumnExist("TSPL_SD_SALES_ORDER_HEAD", "SalesOrder_Type", DBDataType.varchar_Type, 1, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_ORDER_HEAD alter column SalesOrder_Type varchar(2)", trans)
                End If
                If CheckColumnExist("TSPL_SD_SALES_ORDER_HEAD", "Mode_Of_Transport", DBDataType.varchar_Type, 12, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_ORDER_HEAD alter column Mode_Of_Transport varchar(max)", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        'Balwinder on 10/04/2015
        If (clsCommon.CompairString("5.0.4.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.86") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()

            Try
                qry = " Select  SysObjects.[Name] As [Name] From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj] Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] where Tab.name='tspl_journal_master' and Col.name ='Authorized'"
                If clsCommon.CompairString("DF__TSPL_JOUR__Autho__0958649A", clsCommon.myCstr(clsDBFuncationality.getSingleValue(qry, trans))) = CompairStringResult.Equal Then
                    qry = " alter table TSPL_JOURNAL_MASTER drop  DF__TSPL_JOUR__Autho__0958649A"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                    Try
                        qry = " alter table tspl_journal_master add default 'N' for Authorized"
                        clsDBFuncationality.ExecuteNonQuery(qry, trans)
                    Catch ex As Exception
                    End Try
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.4.90", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.90") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_CHA_CHARGE_MASTER", "cha_type", DBDataType.varchar_Type, 3, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_CHA_CHARGE_MASTER alter column cha_type varchar(max)", trans)
                End If

                DropConstraint("TSPL_CHA_CHARGE_MASTER", "doc_no", trans)

                If CheckColumnExist("TSPL_Ex_Incentive_Detail", "item_code", DBDataType.varchar_Type, 50, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_ex_incentive_detail alter column Item_Code varchar(50) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.91") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_SD_SALE_INVOICE_HEAD", "Comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_sd_sale_invoice_head alter column Comments varchar(max)", trans)
                End If
                If CheckColumnExist("TSPL_SD_SALE_RETURN_HEAD", "Comments", DBDataType.varchar_Type, 200, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_sd_sale_return_head alter column Comments varchar(max)", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.4.99") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("tspl_rgp_head", "doc_type", DBDataType.varchar_Type, 4, Nothing, trans) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table tspl_rgp_head alter column doc_type varchar(5)", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.4.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.4.99", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_mcc_dispatch_challan alter column Modified_Date varchar(30) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_mcc_dispatch_challan alter column Created_Date varchar(30) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_mcc_dispatch_challan_History alter column Modified_Date varchar(30) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_mcc_dispatch_challan_History alter column Created_Date varchar(30) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_TANKER_MASTER alter column Modified_Date varchar(30) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                'TSPL_TANKER_MASTER
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '''' script by priti on 20/04/2015
        If (clsCommon.CompairString("5.0.5.12", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.12", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update tspl_inventory_movement set Trans_Type='FS-SH' where Trans_Type='SD-SH'  and Source_Doc_No in (select Document_Code from TSPL_SD_SHIPMENT_HEAD where Trans_Type='FS')"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set Trans_Type='PS-SH' where Trans_Type='SD-SH'  and Source_Doc_No in (select Document_Code from TSPL_SD_SHIPMENT_HEAD where Trans_Type='PS')"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_SD_SHIPMENT_HEAD set Invoice_Type='N' where Trans_Type='FS'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_SD_SALE_INVOICE_HEAD set Invoice_Type='N' where Trans_Type='FS'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set Trans_Type='FS-SR' where Trans_Type='Sale Return'  and Source_Doc_No in (select Document_Code from TSPL_SD_SALE_RETURN_HEAD where Trans_Type='FS')"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set Trans_Type='PS-SR' where Trans_Type='Sale Return'  and Source_Doc_No in (select Document_Code from TSPL_SD_SALE_RETURN_HEAD where Trans_Type='PS')"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '''' script ends here


        '''' script by priti on 22/04/2015
        If (clsCommon.CompairString("5.0.5.14", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.14", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " update TSPL_SD_SALE_INVOICE_DETAIL set delivery_code=(select TSPL_SD_SHIPMENT_DETAIL.Delivery_Code from TSPL_SD_SALE_INVOICE_detail as a left outer join TSPL_SD_SHIPMENT_DETAIL on a.Shipment_Code=TSPL_SD_SHIPMENT_DETAIL.DOCUMENT_CODE " & _
                " where a.DOCUMENT_CODE=TSPL_SD_SALE_INVOICE_DETAIL.DOCUMENT_CODE and TSPL_SD_SHIPMENT_DETAIL.DOCUMENT_CODE=a.Shipment_Code and TSPL_SD_SHIPMENT_DETAIL.Line_No=TSPL_SD_SALE_INVOICE_DETAIL.Line_No and a.Line_No=TSPL_SD_SALE_INVOICE_DETAIL.Line_No )   from TSPL_SD_SALE_INVOICE_DETAIL " & _
                "left outer join TSPL_SD_SALE_INVOICE_HEAD on TSPL_SD_SALE_INVOICE_DETAIL.DOCUMENT_CODE=TSPL_SD_SALE_INVOICE_HEAD.Document_Code  where  Trans_Type='FS' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '''' script ends here

        '''' script by Rohit on 20/04/2015
        If (clsCommon.CompairString("5.0.5.12", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.12", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update tspl_inventory_movement_new set trans_type='MCC-MSRN' where trans_type='SRN' and source_doc_no in (select doc_code from tspl_milk_srn_Head)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set trans_type='MCC-MSALE' where trans_type='SD-SH'  and source_doc_no in (select document_code from TSPL_SD_SHIPMENT_HEAD where trans_type='MCC')"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set trans_type='MCC-AISSUE' where trans_type='VSPTRAN'  and source_doc_no in (select Doc_no from tspl_vspAsset_Head)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update tspl_inventory_movement set trans_type='MCC-IISSUE' where trans_type='VSPTRAN'  and source_doc_no in (select Doc_no from tspl_vspItem_Head)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '''' script ends here


        If (clsCommon.CompairString("5.0.5.23", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.23", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("tspl_bulk_milk_srn", "Weighment_no", trans)
                DropConstraint("tspl_bulk_milk_srn", "QC_No", trans)
                DropConstraint("tspl_gate_out", "Weighment_no", trans)
                DropConstraint("tspl_cleaning", "Weighment_no", trans)
                DropConstraint("tspl_milk_unloading", "Weighment_no", trans)
                DropConstraint("tspl_weighment_detail", "Gate_Entry_no", trans)
                DropConstraint("TSPL_Cleaning", "QC_No", trans)
                DropConstraint("TSPL_Gate_Out", "QC_No", trans)
                DropConstraint("TSPL_Gate_Out", "Gate_Entry_no", trans)
                DropConstraint("TSPL_MILK_UNLOADING", "QC_No", trans)
                DropConstraint("TSPL_QC_Parameter_Detail", "QC_No", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.5.28", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.28") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                'If CheckTriggerExits("trg_CreateMccMasterHistory_update", trans) = 0 Then
                '    qry = "Create trigger [dbo].[trg_CreateMccMasterHistory_update] on [dbo].[TSPL_Mcc_MASTER] " _
                '            & " for update  " _
                '            & " as  " _
                '            & " begin try " _
                '            & " declare @POstFlag as integer  " _
                '            & " declare @Doc_Code as varchar(30)  " _
                '            & " Select @Doc_Code=i.MCC_Code from deleted i;  " _
                '            & " Insert into tspl_mcc_Master_History(MCC_Code,MCC_Type,MCC_NAME,Chilling_Vendor,Add1,Add2,Tehsil,City_code,State_Code,Country_code,Pin_code," _
                '            & " Telphone,Email,Fax,MCC_Area,Area_Of_Store,Area_Of_Office,Open_Area_For_tanker,Area_Of_LAB,No_Of_SILO,Total_Storage_capacity,Area_Of_Receiving_DOCK," _
                '            & " No_Of_Chiller,Chiller_Brand_Name,Chiller_Capacity,No_Of_MilkPump,MilkPump_Capacity,DripSaver,CanWasher,CanScrubber,FSSAI_NO,ETP,Earthing,Coil_Length," _
                '            & " Electricity_Connection,Boiler,NoOfDG,NoOfCompressor,PayeeName,BankName,BankBranch,BankCityCode,BankStateCode,IFCICode,AccountNO,Created_By," _
                '            & " Created_Date,Modified_By,Modified_Date,Comp_Code,Industry_Type,Industry_Person,Chilling_Rate,Lease_Rate,Chilling_KG_Ltr,Chilling_Dispatch_Qty," _
                '            & " Chilling_Assure_Qty,Chilling_Assure_Period,Agreement_Status,Agreement_Date,Agrmnt_Expired_Date,Security_Status,Cheque_Amt,Cheque_No,Cheque_Date," _
                '            & " Bank_Code,FAT_SNF_SAVE,FAT_SNF_CALC,Mcc_Code_VLC_Uploader,Guarantee_Amount,MCC_In_Charge,Start_Date,End_Date,Silo_Capacity,Unit_Code,Unit_Desc," _
                '            & " Payment_Cycle,Unit_MccSuperArea,Unit_AreaofStore,Unit_AreaOfOffice,Unit_OpenAreaForTankerMovement,Unit_AreaOfLab,Unit_AreaOfReceivingDock," _
                '            & " Unit_ChillingOn,Unit_ChillingOnQty,Unit_ChillingMinGuaranteePeriod,Unit_RateOfLeasedCharges,Pan_No,Standard_Security_Amount,Chilling_Period_Starting_Date" _
                '            & " ,Default_Weighing_Machine,Default_Sample_Machine,Is_Truck_Sheet_Mandatory,Default_Weighing_Comport,Default_Sample_Comport,In_active,incentive_code," _
                '            & " EmpOnAmountOnly) " _
                '            & " select MCC_Code,MCC_Type,MCC_NAME,Chilling_Vendor,Add1,Add2,Tehsil,City_code,State_Code,Country_code,Pin_code,Telphone,Email,Fax,MCC_Area,Area_Of_Store" _
                '            & " ,Area_Of_Office,Open_Area_For_tanker,Area_Of_LAB,No_Of_SILO,Total_Storage_capacity,Area_Of_Receiving_DOCK,No_Of_Chiller,Chiller_Brand_Name" _
                '            & " ,Chiller_Capacity,No_Of_MilkPump,MilkPump_Capacity,DripSaver,CanWasher,CanScrubber,FSSAI_NO,ETP,Earthing,Coil_Length,Electricity_Connection,Boiler," _
                '            & " NoOfDG,NoOfCompressor,PayeeName,BankName,BankBranch,BankCityCode,BankStateCode,IFCICode,AccountNO,Created_By,Created_Date,Modified_By,Modified_Date," _
                '            & " Comp_Code,Industry_Type,Industry_Person,Chilling_Rate,Lease_Rate,Chilling_KG_Ltr,Chilling_Dispatch_Qty,Chilling_Assure_Qty,Chilling_Assure_Period," _
                '            & " Agreement_Status,Agreement_Date,Agrmnt_Expired_Date,Security_Status,Cheque_Amt,Cheque_No,Cheque_Date,Bank_Code,FAT_SNF_SAVE,FAT_SNF_CALC," _
                '            & " Mcc_Code_VLC_Uploader,Guarantee_Amount,MCC_In_Charge,Start_Date,End_Date,Silo_Capacity,Unit_Code,Unit_Desc,Payment_Cycle,Unit_MccSuperArea," _
                '            & " Unit_AreaofStore,Unit_AreaOfOffice,Unit_OpenAreaForTankerMovement,Unit_AreaOfLab,Unit_AreaOfReceivingDock,Unit_ChillingOn,Unit_ChillingOnQty," _
                '            & " Unit_ChillingMinGuaranteePeriod,Unit_RateOfLeasedCharges,Pan_No,Standard_Security_Amount,Chilling_Period_Starting_Date,Default_Weighing_Machine," _
                '            & " Default_Sample_Machine,Is_Truck_Sheet_Mandatory,Default_Weighing_Comport,Default_Sample_Comport,In_active,incentive_code" _
                '            & " ,EmpOnAmountOnly  from deleted " _
                '            & "  end try " _
                '            & " begin catch  " _
                '            & " end catch    "
                '    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                'End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.5.24", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.24", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_weighment_detail  add  unique( gate_entry_no)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_Quality_check  add  unique( gate_entry_no)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_milk_unloading  add  unique( gate_entry_no)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_Cleaning  add  unique( gate_entry_no)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_gate_out  add  unique( gate_entry_no)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_milk_transfer_in  add  unique( gate_entry_no)", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.5.31", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.31") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "select count(*) from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where TABLE_NAME='Tspl_Milk_Srn_Head' and CONSTRAINT_NAME='PK_MilkSampleCodeSample_No'"
                check = clsDBFuncationality.getSingleValue(qry, trans)
                If check <= 0 Then
                    qry = "ALTER TABLE dbo.tspl_milk_srn_Head ADD CONSTRAINT PK_MilkSampleCodeSample_No  unique (MIlk_Sample_Code, Sample_NO) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        'richa 15/05/2015 user default located updated or inserted into gl security automatically
        If (clsCommon.CompairString("5.0.5.46", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.46") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                dt = clsDBFuncationality.GetDataTable("Select TSPL_USER_MASTER.User_Code,TSPL_USER_MASTER.Default_Location,TSPL_LOCATION_MASTER.Loc_Segment_Code  from TSPL_USER_MASTER Left outer Join TSPL_LOCATION_MASTER on TSPL_USER_MASTER.Default_Location=TSPL_LOCATION_MASTER.Location_Code  where TSPL_USER_MASTER.Default_Location is not null", trans)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        Dim str As String = "UPDATE TSPL_GL_SEGMENT_PERMISSION SET Default_Segment ='N' WHERE User_Code ='" & clsCommon.myCstr(dr("User_Code")) & "'"
                        clsDBFuncationality.ExecuteNonQuery(str, trans)

                        If clsDBFuncationality.getSingleValue("Select count(*) from TSPL_GL_SEGMENT_PERMISSION where User_Code ='" & clsCommon.myCstr(dr("User_Code")) & "' and Segment_Code ='" & clsCommon.myCstr(dr("Loc_Segment_Code")) & "'", trans) <= 0 Then
                            'clsDBFuncationality.SaveAStorePorcedure(trans, "SP_TSPL_GL_SEGMENT_PERMISSION_INSERT", New SqlParameter("@usercode", "" & clsCommon.myCstr(dr("User_Code")) & ""), New SqlParameter("@glsegment", "7"), New SqlParameter("@segmentcode", "" & clsCommon.myCstr(dr("Loc_Segment_Code")) & ""), New SqlParameter("@createdby", objCommonVar.CurrentUserCode), New SqlParameter("@createddate", clsCommon.GETSERVERDATE(trans)), New SqlParameter("@modifydate", clsCommon.GETSERVERDATE(trans)), New SqlParameter("@modifyby", objCommonVar.CurrentUserCode), New SqlParameter("@compcode", objCommonVar.CurrentCompanyCode), New SqlParameter("@Default_Segment", "Y"))
                        Else
                            clsDBFuncationality.ExecuteNonQuery("UPDATE TSPL_GL_SEGMENT_PERMISSION SET Default_Segment ='Y' WHERE User_Code ='" & clsCommon.myCstr(dr("User_Code")) & "' and Segment_Code ='" & clsCommon.myCstr(dr("Loc_Segment_Code")) & "' ", trans)
                        End If
                    Next
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If clsCommon.CompairString("5.0.5.47", exeVersion) = CompairStringResult.Greater OrElse clsCommon.CompairString(exeVersion, "5.0.5.47") = CompairStringResult.Equal Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TSPL_COMPANY_MASTER_insert]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[sp_TSPL_COMPANY_MASTER_insert]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TSPL_COMPANY_MASTER_update]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[sp_TSPL_COMPANY_MASTER_update]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "Create  Proc [dbo].[sp_TSPL_COMPANY_MASTER_insert] (@CompCode varchar(8),@CompName varchar(100),@Add1 varchar(50),@Add2 varchar(50),@Add3 varchar(50),@CityCode varchar(50),@Phone1 varchar(50),@Phone2 varchar(50),@Fax varchar(12),@Email varchar(50),@PinCode varchar(20),@State varchar(30),@TinNo varchar(20),@CstLst varchar(30),@RegdNo varchar(30),@CForm char(1),@ModeofTransport varchar(30),@CreatedBy varchar(12),@Createddate varchar(10),@ModifiedBy varchar(12),@ModifiedDate varchar(10),@CompCode1 varchar(8),@DBName varchar(100),@VatRegNo varchar(30)=null,@PanNo varchar(30)=null,@ServiceTaxReg varchar(30)=null,@TanNo varchar(30)=null,@AccessOfficer varchar(30)=null,@TCanNo varchar(30)=null,@CERange varchar(30)=null,@CircleNo varchar(30)=null,@CECommissionerate decimal(18,2)=null,@WardNo varchar(30)=null,@CEDivision varchar(30)=null,@EccNo varchar(30)=null,@PFNo varchar(30)=null,@ESICNo varchar(30)=null)as begin insert into TSPL_COMPANY_MASTER (Comp_Code,Comp_Name,Add1,Add2,Add3,City_Code,Phone1,Phone2,Fax,Email,Pincode,State,Tin_No,CST_LST,Regn_No,Cform,Mode_of_Trans,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code1,DataBase_Name,Vat_Reg_No,ServiceTax_Reg_No,Ecc_No,CE_Range,CE_Commissionerate,CE_Division,Pan_No,Tan_No,Tcan_No,Circle_No,Ward_No,Access_Officer,comp_Pf_No,comp_Esic_No) values(@CompCode,@CompName,@Add1,@Add2,@Add3,@CityCode,@Phone1,@Phone2,@Fax,@Email,@PinCode,@State,@TinNo,@CstLst,@RegdNo,@CForm,@ModeofTransport,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@CompCode1,@DBName,@VatRegNo,@ServiceTaxReg,@EccNo,@CERange,@CECommissionerate,@CEDivision,@PanNo,@TanNo,@TCanNo,@CircleNo,@WardNo,@AccessOfficer,@PFNo,@ESICNo) end"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "Create proc [dbo].[sp_TSPL_COMPANY_MASTER_update](@CompCode varchar(8),@CompName varchar(100),@Add1 varchar(50),@Add2 varchar(50),@Add3 varchar(50),@CityCode varchar(50),@Phone1 varchar(50),@Phone2 varchar(50),@Fax varchar(12),@Email varchar(50),@PinCode varchar(20),@State varchar(30),@TinNo varchar(20),@CstLst varchar(30),@RegdNo varchar(30),@CForm char(1),@ModeofTransport varchar(30),@CreatedBy varchar(12),@Createddate varchar(10),@ModifiedBy varchar(12),@ModifiedDate varchar(10),@CompCode1 varchar(8),@DBName varchar(100),@VatRegNo varchar(30),@PanNo varchar(30),@ServiceTaxReg varchar(30),@TanNo varchar(30),@AccessOfficer varchar(30),@TCanNo varchar(30),@CERange varchar(30),@CircleNo varchar(30),@CECommissionerate Varchar(30),@WardNo varchar(30),@CEDivision varchar(30),@EccNo varchar(30),@PFNo varchar(30)=null,@ESICNo varchar(30)=null)as begin update  TSPL_COMPANY_MASTER set Comp_Name=@CompName,Add1=@Add1,Add2=@Add2,Add3=@Add3,City_Code=@CityCode,Phone1=@Phone1,Phone2=@Phone2,Fax=@Fax,Email=@Email,Pincode=@PinCode,State=@State,Tin_No=@TinNo,Cst_Lst=@CstLst,Regn_No=@RegdNo,Cform=@CForm,Mode_of_Trans=@ModeofTransport,Created_By=@CreatedBy,Created_Date=@Createddate,Modify_By=@ModifiedBy,Modify_Date=@ModifiedDate,Comp_Code1=@CompCode1,DataBase_Name=@DBName , Vat_Reg_No=@VatRegNo,ServiceTax_Reg_No=@ServiceTaxReg ,Ecc_No=@EccNo,CE_Range=@CERange ,CE_Commissionerate=@CECommissionerate,CE_Division=@CEDivision,Pan_No=@PanNo,Tan_No=@TanNo,Tcan_No=@TCanNo, Circle_No=@CircleNo,Ward_No=@WardNo,Access_Officer=@AccessOfficer,Comp_PF_NO=@PFNo,Comp_ESIC_NO=@ESICNo where Comp_Code=@CompCode end"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If
        'richa 19/05/2015  against ticket no BM00000006589
        If (clsCommon.CompairString("5.0.5.49", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.49") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_EX_PI_DETAIL alter column Qty float null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_EX_PI_DETAIL_HISTORY alter column Qty float null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '----------------

        'richa 21/05/2015 against ticket no BM00000006589
        If (clsCommon.CompairString("5.0.5.52", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.52") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_EX_COMMERCIAL_INVOICE_DETAIL alter column Qty float null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '----------------


        ' Richa 26/05/2015 against ticket no BM00000006805,BM00000006804,BM00000006802,BM00000006811,BM00000006841
        If (clsCommon.CompairString("5.0.5.56", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.56") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "Delete from TSPL_PROGRAM_MASTER where Program_Code in ('GATE-PASS-PS','ITEM-LOC','ITM-LOCD-RPT','SEURTY-RPT','CUS-OUTS-RPT','VEN-OUTS-RPT','ITM-MOV-RPT')  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                '  clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
            ' FrmUtility.UpdateInventoryMovementCustomerVendorLocation(trans)
        End If

        ''richa agarwal against ticket no BM00000007036
        '=====================ADD TRIGGER IN AR Invoice to stop creating duplicate ar invoice no against bulk invoice=================
        If (clsCommon.CompairString("5.0.5.74", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.74") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_UniqueARInvoiceNowithDocNO", trans) = 0 Then
                    qry = "CREATE TRIGGER [dbo].[trg_UniqueARInvoiceNowithDocNO] ON  [dbo].[TSPL_Customer_Invoice_Head]" _
                    & " for insert " _
                    & " AS " _
                    & " declare @POstFlag as integer " _
                    & " declare @Against_Sale_No as varchar(30)" _
                    & " Select @Against_Sale_No=i.Against_Sale_No from inserted i;" _
                    & " select @POstFlag=count(*) from TSPL_Customer_Invoice_Head where Against_Sale_No  =@Against_Sale_No  and (Trans_Type ='BS' or Trans_Type ='BST')" _
                    & " if  @POstFlag>1 " _
                    & " BEGIN" _
                    & " raiserror ('Cannot create duplicate entry',16,1)" _
                    & " End"
                Else
                    qry = "Alter TRIGGER [dbo].[trg_UniqueARInvoiceNowithDocNO] ON  [dbo].[TSPL_Customer_Invoice_Head]" _
                   & " for insert " _
                   & " AS " _
                   & " declare @POstFlag as integer " _
                   & " declare @Against_Sale_No as varchar(30)" _
                   & " Select @Against_Sale_No=i.Against_Sale_No from inserted i;" _
                   & " select @POstFlag=count(*) from TSPL_Customer_Invoice_Head where Against_Sale_No  =@Against_Sale_No  and (Trans_Type ='BS' or Trans_Type ='BST')" _
                   & " if  @POstFlag>1 " _
                   & " BEGIN" _
                   & " raiserror ('Cannot create duplicate entry',16,1)" _
                   & " End"

                End If
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        'priti 15/06/2015 against
        If (clsCommon.CompairString("5.0.5.77", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.77") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_sd_sale_return_detail alter column item_cost float null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.5.79", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.79") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PI_DETAIL alter column Landed_Cost_Rate decimal(18,4)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '----------------
        ' Against BM00000007197
        If (clsCommon.CompairString("5.0.5.89", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.89") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "ALTER TABLE TSPL_VENDOR_INVOICE_HEAD ALTER COLUMN Vendor_Name VARCHAR(200) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.5.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.91") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_deduction alter column emp_code varchar(12) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table tspl_allowance alter column emp_code varchar(12) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ''----------------------
        ''richa agarwal 06/07/2015 BM00000007328
        If (clsCommon.CompairString("5.0.5.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.95", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_DOCPREFIX_MASTER alter column Doc_Prfeix varchar(15)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '====shivani(07/07/2015)
        If (clsCommon.CompairString("5.0.5.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.5.99", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist_New alter column comments text"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_PURCHASE_ORDER_DETAIL_Hist_New alter column Item_Desc varchar(5000)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '====priti(20/01/2016)
        If (clsCommon.CompairString("5.0.9.82", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.82", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_SCHEME_MASTER_NEW set MaxlimitStart_Date=Start_Date,MaxlimitEnd_Date=End_Date"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ''-----------------------
        ''richa agarwal 09/07/2015 BM00000007334
        'If (clsCommon.CompairString("5.0.5.98", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.5.98") = CompairStringResult.Equal) Then
        '    trans = clsDBFuncationality.GetTransactin()
        '    Try
        '        If CheckTriggerExits("TrgPaymentHeader", trans) = 0 Then
        '            qry = "CREATE TRIGGER [dbo].[TrgPaymentHeader] ON [dbo].[TSPL_PAYMENT_HEADER]" _
        '            & " FOR Update,Insert " _
        '            & " AS " _
        '            & " declare @Payment_No varchar(30),@Payment_Type char(2),@Payment_Date date,@Vendor_Code varchar(12),@Vendor_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date date,@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Payment_Amount decimal(18,2),@Posted char(1),@IsChkReverse char(1),@Bank_Charges decimal(18,2), @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float, @Advance_Against_Salary Bit, @Is_Opening int " _
        '            & " select @Bank_Charges=isnull(Bank_Charges,0),@Payment_No=Payment_No, @Payment_Type=Payment_Type,@Posted=Posted,@IsChkReverse=IsChkReverse, @Advance_Against_Salary=Advance_Against_Salary, @Is_Opening= is_Opening from inserted " _
        '            & " if  @IsChkReverse='N' " _
        '            & " begin " _
        '            & " if (@Payment_Type='PY' or @Payment_Type='AV' or @Payment_Type='OA')  " _
        '            & " begin " _
        '            & " SELECT  @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  " _
        '            & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount, " _
        '            & " @Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate " _
        '            & " FROM TSPL_Payment_HEADER INNER JOIN " _
        '            & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '            & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No" _
        '            & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No          " _
        '            & " If (CONVERT(Int,@Advance_Against_Salary)+@Is_Opening<>2)" _
        '            & " Begin " _
        '            & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',@Payment_Amount,0,'Payment',@Payment_Type, @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '            & " End" _
        '            & " End" _
        '            & " if @Payment_Type='RC' " _
        '            & " begin" _
        '            & " SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code, " _
        '            & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount  + isnull(Bank_Charges,0)," _
        '            & " @Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate" _
        '            & " FROM TSPL_Payment_HEADER INNER JOIN" _
        '            & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '            & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No" _
        '            & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No          " _
        '            & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',0,@Payment_Amount,'Payment',@Payment_Type, @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '            & " End" _
        '            & " if @Posted <> 'P'" _
        '            & " begin" _
        '            & "	if @Payment_Type='MI'" _
        '            & " begin" _
        '            & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No   " _
        '            & " End" _
        '            & " End" _
        '            & " if @Payment_Type='MI'  and @Bank_Charges > 0" _
        '            & " begin" _
        '            & " SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code, " _
        '            & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=CREDITACC ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount= isnull(Bank_Charges,0)," _
        '            & "	@Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate" _
        '            & "	FROM TSPL_Payment_HEADER INNER JOIN" _
        '            & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '            & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_Payment_HEADER.Bank_Charges_Ac=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No     " _
        '            & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No   and    TransactionType= 'MIOther'   " _
        '            & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_rate,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',@Payment_Amount,0,'Payment','MIOther', @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '            & " End" _
        '            & " End"
        '        Else
        '            qry = "ALTER TRIGGER [dbo].[TrgPaymentHeader] ON [dbo].[TSPL_PAYMENT_HEADER]" _
        '             & " FOR Update,Insert " _
        '             & " AS " _
        '             & " declare @Payment_No varchar(30),@Payment_Type char(2),@Payment_Date date,@Vendor_Code varchar(12),@Vendor_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date date,@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Payment_Amount decimal(18,2),@Posted char(1),@IsChkReverse char(1),@Bank_Charges decimal(18,2), @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float, @Advance_Against_Salary Bit, @Is_Opening int " _
        '             & " select @Bank_Charges=isnull(Bank_Charges,0),@Payment_No=Payment_No, @Payment_Type=Payment_Type,@Posted=Posted,@IsChkReverse=IsChkReverse, @Advance_Against_Salary=Advance_Against_Salary, @Is_Opening= is_Opening from inserted " _
        '             & " if  @IsChkReverse='N' " _
        '             & " begin " _
        '             & " if (@Payment_Type='PY' or @Payment_Type='AV' or @Payment_Type='OA')  " _
        '             & " begin " _
        '             & " SELECT  @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  " _
        '             & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount, " _
        '             & " @Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate " _
        '             & " FROM TSPL_Payment_HEADER INNER JOIN " _
        '             & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '             & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No" _
        '             & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No          " _
        '             & " If (CONVERT(Int,@Advance_Against_Salary)+@Is_Opening<>2)" _
        '             & " Begin " _
        '             & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',@Payment_Amount,0,'Payment',@Payment_Type, @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '             & " End" _
        '             & " End" _
        '             & " if @Payment_Type='RC' " _
        '             & " begin" _
        '             & " SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code, " _
        '             & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount  + isnull(Bank_Charges,0)," _
        '             & " @Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate" _
        '             & " FROM TSPL_Payment_HEADER INNER JOIN" _
        '             & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '             & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No" _
        '             & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No          " _
        '             & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',0,@Payment_Amount,'Payment',@Payment_Type, @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '             & " End" _
        '             & " if @Posted <> 'P'" _
        '             & " begin" _
        '             & " if @Payment_Type='MI'" _
        '             & " begin" _
        '             & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No   " _
        '             & " End" _
        '             & " End" _
        '             & " if @Payment_Type='MI'  and @Bank_Charges > 0" _
        '             & " begin" _
        '             & " SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code, " _
        '             & " @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=CREDITACC ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount= isnull(Bank_Charges,0)," _
        '             & " @Currency=TSPL_Payment_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_Payment_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_Payment_HEADER.ConvRate" _
        '             & " FROM TSPL_Payment_HEADER INNER JOIN" _
        '             & " TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN" _
        '             & " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_Payment_HEADER.Bank_Charges_Ac=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No     " _
        '             & " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No   and    TransactionType= 'MIOther'   " _
        '             & " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_rate,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,'',@Payment_Amount,0,'Payment','MIOther', @Currency, @Base_Currency, @Conversion_Rate,1)" _
        '             & " End" _
        '             & " End"
        '        End If
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        '        trans.Commit()
        '    Catch ex As Exception
        '        trans.Rollback()
        '        clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
        '    End Try
        'End If
        ''preeti gupta  16/07/2015
        If (clsCommon.CompairString("5.0.6.11", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.11", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_EMPLOYEE_MASTER", "[Hold Salary]", trans)
                check = CheckColumnExist("TSPL_EMPLOYEE_MASTER", "[Hold Salary]", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_EMPLOYEE_MASTER DROP COLUMN [Hold Salary]", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '============================Rohit gupta================20-Jul-2015=========================================
        If (clsCommon.CompairString("5.0.6.14", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.14", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_Milk_RGP_Head", "Posting_Date", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_Milk_RGP_Head alter column Posting_Date datetime null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '==============================================================================================================
        '============================Rohit gupta================29-Jul-2015=========================================
        If (clsCommon.CompairString("5.0.6.20", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.20", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("delete from TSPL_PROGRAM_MASTER where Program_Code='MI_CLEANI'", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If
        '===========================================SHIVANI===================================================================
        If (clsCommon.CompairString("5.0.6.20", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.20", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_Milk_QC_Parameter_Detail", "QC_No", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_Milk_QC_Parameter_Detail add constraint QC_NO_ForeignKey FOREIGN KEY (qc_no) REFERENCES tspl_Milk_quality_check(qc_no)", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '============================Rohit gupta================31-Jul-2015=========================================


        If (clsCommon.CompairString("5.0.6.21", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.21", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_Cancel_After_Posting_Tables_Details", "Inactive_date", DBDataType.NotApplicable, 0, 0, trans)
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_Cancel_After_Posting_Tables_Details alter column Inactive_date varchar(30) null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If


        ''--------------------------------------

        If (clsCommon.CompairString("5.0.6.25", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.25", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PP_PRODUCTION_ENTRY alter column RECEIVED_BY varchar(12) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.6.26", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.26", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_ASSET_WORK_HEAD alter column vendor_code varchar(12) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If



        If (clsCommon.CompairString("5.0.6.26", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.26", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PURCHASE_ORDER_DETAIL alter column PurchaseOrder_Qty FLOAT null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If


        If (clsCommon.CompairString("5.0.6.29", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.29", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALE_INVOICE_DETAIL alter column Qty FLOAT null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_ORDER_DETAIL alter column Qty FLOAT null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALE_RETURN_DETAIL alter column Qty FLOAT null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_PI_DETAIL alter column PI_Qty FLOAT null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If


        If (clsCommon.CompairString("5.0.6.30", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.30", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                clsDBFuncationality.ExecuteNonQuery("drop index  tspl_srn_detail._dta_index_TSPL_SRN_DETAIL_10_1709965168__K8_K3_K9_K1_K58_K4_5_74_75_88", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SRN_DETAIL alter column SRN_Qty FLOAT null", trans)
                clsDBFuncationality.ExecuteNonQuery("CREATE NONCLUSTERED INDEX Index_tspl_srn_detail_free_burst_leak_srn_qty ON dbo.TSPL_SRN_DETAIL (Unit_code, Item_Code, Location, SRN_No, MRP, Item_Desc) INCLUDE (free_qty,srn_qty,leak_qty,burst_qty)", trans)
                clsDBFuncationality.ExecuteNonQuery("update TSPL_SRN_HEAD set TSPL_SRN_HEAD.Document_Type='MT' where 1=1 and TSPL_SRN_HEAD.Against_PO not in ( Select TSPL_SRN_HEAD.Against_PO  from TSPL_SRN_HEAD left Outer Join TSPL_PURCHASE_ORDER_HEAD on TSPL_SRN_HEAD.Against_PO =TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No where TSPL_PURCHASE_ORDER_HEAD.MT_Is_Merchant_Trade =0)", trans)
                clsDBFuncationality.ExecuteNonQuery("UPDATE TSPL_SRN_HEAD set TSPL_SRN_HEAD.Document_Type='SRN' where 1=1 and isnull(TSPL_SRN_HEAD.Against_PO,'') not in ( Select TSPL_SRN_HEAD.Against_PO  from TSPL_SRN_HEAD left Outer Join TSPL_PURCHASE_ORDER_HEAD on TSPL_SRN_HEAD.Against_PO =TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No where TSPL_PURCHASE_ORDER_HEAD.MT_Is_Merchant_Trade =1)", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If



        If (clsCommon.CompairString("5.0.6.31", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.31", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("update TSPL_PI_HEAD set TSPL_PI_HEAD.Document_Type ='PI' from TSPL_PI_HEAD left outer join TSPL_SRN_HEAD  on TSPL_PI_HEAD.Against_SRN =TSPL_SRN_HEAD.SRN_No   where TSPL_SRN_HEAD.Document_Type ='SRN'", trans)
                clsDBFuncationality.ExecuteNonQuery("update TSPL_PI_HEAD set TSPL_PI_HEAD.Document_Type ='MT' from TSPL_PI_HEAD left outer join TSPL_SRN_HEAD  on TSPL_PI_HEAD.Against_SRN =TSPL_SRN_HEAD.SRN_No   where TSPL_SRN_HEAD.Document_Type ='MT'", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SRN_DETAIL alter column PO_Qty FLOAT null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SRN_DETAIL alter column MRN_Qty FLOAT null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If



        If (clsCommon.CompairString("5.0.6.35", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.35", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If (clsCommon.CompairString("5.0.6.36", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.36", exeVersion) = CompairStringResult.Equal) Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_INVOICE_DETAIL_BULKSALE_HISTORY alter column History_date varchar(30) null", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_INVOICE_Master_BULKSALE_HISTORY alter column History_date varchar(30) null", trans)
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MRN_DETAIL_HISTORY alter column MRN_No Varchar(30) Null", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.6.40", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.40", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_MRN_DETAIL_HISTORY", "MRN_No", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.6.44", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.44", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("update TSPL_DOCPREFIX_MASTER set Doc_Trans_Type='Direct AP' where Doc_Type in ('AP Debit Note','AP Credit Note','AP Invoice') and len(isnull(Doc_Trans_Type,''))<=0", trans) ''BM00000007767 by balwinder on 31-08-2015

                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_Port_setting alter column machineName varchar(30)", trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If
        If (clsCommon.CompairString("5.0.6.51", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.51", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_LOCATION_MASTER alter column mcc_type char(1) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_milk_rgP_head alter column Remarks varchar(max)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_milk_rgP_detail alter column Remarks varchar(max)", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_vendor_Master alter column status char(1) null", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_vendor_Master alter column Onhold char(1) null", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
      
        If (clsCommon.CompairString("5.0.6.57", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.57", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("delete from TSPL_PROGRAM_MASTER where Program_Name='Account set' and sno='1.29.01.01'", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ''---------------------------

        '==============preeti gupta============
        If (clsCommon.CompairString("5.0.6.58", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.58") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_VLC_DATA_UPLOADER set fat_KG=qty *fat* tt /100,snf_KG=qty *snf* tt /100 from (select Uom_Code, case when Uom_Code='KG' then 1 when Uom_Code='Ltr' then 1.03 end as tt,TSPL_Mcc_UOM_DETAIL.Mcc_Code from TSPL_Mcc_UOM_DETAIL where Stocking_Unit='Y') tt  where tt.MCC_CODE=TSPL_VLC_DATA_UPLOADER.MCC_Code and coalesce(fat_KG,0)<=0"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        ' Size from 20 to 30 Modified By Pankaj Jha Against Ticket No BM00000007872
        If (clsCommon.CompairString("5.0.6.58", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.58") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_Payment_Adjustment_Header alter column doc_no varchar(30)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        ' Size from 1 to 12 Modified By Rohit Against Ticket No BM00000007908
        If (clsCommon.CompairString("5.0.6.65", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.65") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_Mcc_Master alter column Default_Weighing_Machine varchar(12) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        '#Rohit======================================================
        ' Modified By Rohit Against Ticket No BM00000007945,BM00000007946
        If (clsCommon.CompairString("5.0.6.66", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.66") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_Customer_Master_History alter column Customer_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                DropConstraint("TSPL_MRN_HEAD_History", "Against_RGP_No", trans)
                DropConstraint("TSPL_MRN_Head_HISTORY", "Against_Schedule_Code", trans)
                DropConstraint("TSPL_MRN_Head_HISTORY", "CURRENCY_CODE", trans)
                DropConstraint("TSPL_MRN_Head_HISTORY", "Against_GRN", trans)
                DropConstraint("TSPL_MRN_Head_HISTORY", "Against_PO", trans)
                DropConstraint("TSPL_MRN_Head_HISTORY", "Against_Requisition", trans)
                DropConstraint("TSPL_MRN_Detail_HISTORY", "GRN_Id", trans)
                DropConstraint("TSPL_MRN_Detail_HISTORY", "PO_Id", trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        ' Anubhooti Against Ticket No BM00000007930 (Customer Name Length modified from 50 to 200 acc. to TSPL_CUSTOMER_MASTER )
        If (clsCommon.CompairString("5.0.6.66", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.66") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_RECEIPT_HEADER alter column Customer_Name varchar(200) NOT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        If (clsCommon.CompairString("5.0.6.81", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.81", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  Alter Table TSPL_ASSET_WORK_DETAIL Alter column Add_Charges_Code varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        If (clsCommon.CompairString("5.0.6.84", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.1.84", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                Try
                    qry = " alter table TSPL_Cancel_Table_Details add  Update_Column_Name text null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                Catch ex As Exception
                End Try
                qry = "update TSPL_Cancel_Table_Details set Update_Column_Name='MRN_Qty=0,Balance_Qty=0,Item_Cost=0,TAX1_Base_Amt=0,TAX1_Rate=0,TAX1_Amt=0,TAX2_Base_Amt=0,TAX2_Rate=0,TAX2_Amt=0,TAX3_Base_Amt=0,TAX3_Rate=0,TAX3_Amt=0,TAX4_Base_Amt=0,TAX4_Rate=0,TAX4_Amt=0,TAX5_Base_Amt=0,TAX5_Rate=0,TAX5_Amt=0,TAX6_Base_Amt=0,TAX6_Rate=0,TAX6_Amt=0,TAX7_Base_Amt=0,TAX7_Rate=0,TAX7_Amt=0,TAX8_Base_Amt=0,TAX8_Rate=0,TAX8_Amt=0,TAX9_Base_Amt=0,TAX9_Rate=0,TAX9_Amt=0,TAX10_Base_Amt=0,TAX10_Rate=0,TAX10_Amt=0,Amount=0,Disc_Per=0,Disc_Amt=0,Amt_Less_Discount=0,Total_Tax_Amt=0,Item_Net_Amt=0' where Form_Id='PO-MRN' and tb_name='Tspl_MRN_Detail'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_Cancel_Table_Details set Update_Column_Name='TAX1_Rate=0,TAX1_Amt=0,TAX1_Base_Amt=0,TAX2_Rate=0,TAX2_Amt=0,TAX2_Base_Amt=0,TAX3_Rate=0,TAX3_Amt=0,TAX3_Base_Amt=0,TAX4_Rate=0,TAX4_Amt=0,TAX4_Base_Amt=0,TAX5_Rate=0,TAX5_Amt=0,TAX5_Base_Amt=0,TAX6_Rate=0,TAX6_Amt=0,TAX6_Base_Amt=0,TAX7_Rate=0,TAX7_Amt=0,TAX7_Base_Amt=0,TAX8_Rate=0,TAX8_Amt=0,TAX8_Base_Amt=0,TAX9_Rate=0,TAX9_Amt=0,TAX9_Base_Amt=0,TAX10_Rate=0,TAX10_Amt=0,TAX10_Base_Amt=0,Discount_Base=0,Discount_Amt=0,Amount_Less_Discount=0,Total_Tax_Amt=0,MRN_Total_Amt=0' where Form_Id='PO-MRN' and tb_name='Tspl_MRN_Head'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "Update TSPL_CANCEL_TABLE_VALIDATE_DETAILS set Return_Query='select count(*) from tspl_SRN_Return where srn_No=(select SRN_No from tspl_SRn_Head where Against_MRN=''@Value'')' where Form_Id='PO-MRN' and Valicate_Tb_Name='Tspl_SRN_Head'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "Update TSPL_CANCEL_TABLE_VALIDATE_DETAILS set Return_Query='select count(*) from tspl_SRN_Return where srn_No=(select SRN_No from tspl_SRn_Head where Against_MRN=(select MRN_No from tspl_MRN_Head where Against_GRN=''@Value''))' where Form_Id='PO-GRN' and Valicate_Tb_Name='Tspl_MRN_Head'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_Cancel_Table_Details set Update_Column_Name='TAX1_Rate=0,TAX1_Amt=0,TAX1_Base_Amt=0,TAX2_Rate=0,TAX2_Amt=0,TAX2_Base_Amt=0,TAX3_Rate=0,TAX3_Amt=0,TAX3_Base_Amt=0,TAX4_Rate=0,TAX4_Amt=0,TAX4_Base_Amt=0,TAX5_Rate=0,TAX5_Amt=0,TAX5_Base_Amt=0,TAX6_Rate=0,TAX6_Amt=0,TAX6_Base_Amt=0,TAX7_Rate=0,TAX7_Amt=0,TAX7_Base_Amt=0,TAX8_Rate=0,TAX8_Amt=0,TAX8_Base_Amt=0,TAX9_Rate=0,TAX9_Amt=0,TAX9_Base_Amt=0,TAX10_Rate=0,TAX10_Amt=0,TAX10_Base_Amt=0,Discount_Base=0,Discount_Amt=0,Amount_Less_Discount=0,Total_Tax_Amt=0,GRN_Total_Amt=0' where Form_Id='PO-GRN' and tb_name='Tspl_GRN_Head'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_Cancel_Table_Details set Update_Column_Name='GRN_Qty=0,Balance_Qty=0,Item_Cost=0,TAX1_Base_Amt=0,TAX1_Rate=0,TAX1_Amt=0,TAX2_Base_Amt=0,TAX2_Rate=0,TAX2_Amt=0,TAX3_Base_Amt=0,TAX3_Rate=0,TAX3_Amt=0,TAX4_Base_Amt=0,TAX4_Rate=0,TAX4_Amt=0,TAX5_Base_Amt=0,TAX5_Rate=0,TAX5_Amt=0,TAX6_Base_Amt=0,TAX6_Rate=0,TAX6_Amt=0,TAX7_Base_Amt=0,TAX7_Rate=0,TAX7_Amt=0,TAX8_Base_Amt=0,TAX8_Rate=0,TAX8_Amt=0,TAX9_Base_Amt=0,TAX9_Rate=0,TAX9_Amt=0,TAX10_Base_Amt=0,TAX10_Rate=0,TAX10_Amt=0,Amount=0,Disc_Per=0,Disc_Amt=0,Amt_Less_Discount=0,Total_Tax_Amt=0,Item_Net_Amt=0' where Form_Id='PO-GRN' and tb_name='Tspl_GRN_Detail'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        If (clsCommon.CompairString("5.0.6.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.6.91", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "Update TSPL_CANCEL_TABLE_VALIDATE_DETAILS set Return_Query='select count(*) from tspl_SRN_Return where srn_No=(select SRN_No from tspl_SRn_Head where Against_MRN=(select MRN_No from tspl_MRN_Head where Against_GRN=''@Value''))' where Form_Id='PO-GRN' and Valicate_Tb_Name='Tspl_MRN_Head'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '===========================update by Preeti Gupta Against Ticket No[BM00000008123]
        If (clsCommon.CompairString("5.0.6.83", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.83") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_SD_SHIPMENT_HEAD alter column Road_Permit_No varchar(35)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_CSA_TRANSFER_HEAD alter column WayBill_No varchar(35)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_TRANSFER_ORDER_HEAD alter column WayBill_No varchar(35)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '=====================Update by preeti gupta against ticket no[BM00000008202]
        If (clsCommon.CompairString("5.0.6.85", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.85") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_MCC_MASTER alter column Bank_Code varchar(30)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '=====================================Update by Parteek==========================================
        If (clsCommon.CompairString("5.0.9.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.17", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "  alter table TSPL_STATE_MASTER_DETAIL  alter column Zone_Code varchar(20) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '======================end==========

        If (clsCommon.CompairString("5.0.6.92", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.92") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='BulkSRN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='OT' where code='BulkSRNRet'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='BulkSRNTrade'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='CSA-SALE'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='DispatchBS'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='DispatchBSTrade'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='DispChallan'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='EX_SALE_IN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='FS-SH'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='FS-SR'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='AD',Out_Category='IS' where code='IC-AD'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='AD',Out_Category='IS' where code='ISSTRAN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='MCC-AISSUE'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='MCC-IISSUE'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='MCC-MSALE'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='MCC-MSR'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='MCC-MSRN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='MilkTransferIn'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='MJ-SR'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='MT_SALE_IN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='NRGP'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='PP_ISSUE'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='AD',Out_Category='IS' where code='PP_STDN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='AD',Out_Category='IS' where code='PRD_STG_PROC'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='AD',Out_Category=NULL where code='PROD_ENTRY'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='PS-SH'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='PS-SR'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='Purchase Return'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='RGP'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='SALE RETURN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='SALERETURNBS'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='ScrapIn'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='SD-CSATRANS'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='OT',Out_Category=NULL where code='SD-CSATRANS-RETURN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='SA' where code='SD-SH'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='SRN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='OT' where code='SRN-RET'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category='SA' where code='Transfer'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category='PU',Out_Category=NULL where code='TRN-RET'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_INVENTORY_SOURCE_CODE set In_Category=NULL,Out_Category='IS' where code='VSPTRAN'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If


        ''=====================Update by preeti gupt
        'If (clsCommon.CompairString("5.0.6.30", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.30") = CompairStringResult.Equal) Then
        '    trans = clsDBFuncationality.GetTransactin()
        '    Try
        '        qry = "create function [dbo].[getWeekNo](@P_Date date) returns int as begin return ("
        '        qry += " Select DATEPART(week, @P_Date )- datepart(week,('01/'+left(DATENAME(month,@P_date),3)+'/' + convert(varchar, datepart(year,@p_date) )))+1"
        '        qry += ") end"
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        '        trans.Commit()
        '    Catch ex As Exception
        '        trans.Rollback()
        '        clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

        '    End Try
        'End If
        '===============================================================================

        If (clsCommon.CompairString("5.0.6.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.6.95") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column PRINCIPAL_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column ARREAR_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column CoEPF_AMT_AC01_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column CoEPS_AMT_AC10_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column PRINCIPAL_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column ARREAR_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column CoEPF_AMT_AC01_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column CoEPS_AMT_AC10_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column PRINCIPAL_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column ARREAR_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column CoEPF_AMT_AC01_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column CoEPS_AMT_AC10_ROUND_OFF FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column ACTUAL_AMOUNT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column ACTUAL_AMOUNT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column ACTUAL_AMOUNT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GENERATE_SALARY_PAYHEADS alter column ARREAR_AMT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALARY_CALCULATION alter column ARREAR_AMT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ARREAR_CALCULATION alter column ARREAR_AMT FLOAT NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                clsCommon.MyMessageBoxShow("Executing Update")
                qry = "alter table tspl_vendor_master alter column vendor_name varchar(200) NULL"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try

        End If
        '=====================Update by preeti gupta==================
        If (clsCommon.CompairString("5.0.7.51", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.51") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update tspl_BankReco_Detail set Payment_Code_reco=xxx.NewPaymentMode from ("
                qry += "  select tspl_BankReco_Detail.Reconciliation_Id,tspl_BankReco_Detail.Document_No,tspl_BankReco_Detail.Payment_code_reco as OldPaymentMode,"
                qry += " (TSPL_PAYMENT_HEADER.Payment_Code) as NewPaymentMode  from tspl_BankReco_Detail"
                qry += " left outer join TSPL_PAYMENT_HEADER on TSPL_PAYMENT_HEADER.Payment_No=tspl_BankReco_Detail.Document_No"
                qry += " where tspl_BankReco_Detail.Document_Type='Payment' and len(isnull(tspl_BankReco_Detail.Payment_code_reco,''))<=0 )xxx"
                qry += " inner join tspl_BankReco_Detail on tspl_BankReco_Detail.Reconciliation_Id=xxx.Reconciliation_Id and tspl_BankReco_Detail.Document_No=xxx.Document_No"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update tspl_BankReco_Detail set Payment_Code_reco=xxx.NewPaymentMode from ("
                qry += " select tspl_BankReco_Detail.Reconciliation_Id,tspl_BankReco_Detail.Document_No,tspl_BankReco_Detail.Payment_code_reco as OldPaymentMode,"
                qry += "(TSPL_BANK_TRANSFER.Payment_Mode ) as NewPaymentMode  from tspl_BankReco_Detail"
                qry += " left outer join TSPL_BANK_TRANSFER on TSPL_BANK_TRANSFER.Transfer_No =tspl_BankReco_Detail.Document_No"
                qry += "  where tspl_BankReco_Detail.Document_Type='BankTransfer' and len(isnull(tspl_BankReco_Detail.Payment_code_reco,''))<=0"
                qry += " )xxx"
                qry += " inner join tspl_BankReco_Detail on tspl_BankReco_Detail.Reconciliation_Id=xxx.Reconciliation_Id and tspl_BankReco_Detail.Document_No=xxx.Document_No"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update tspl_BankReco_Detail set Payment_Code_reco=xxx.NewPaymentMode from ("
                qry += " select tspl_BankReco_Detail.Reconciliation_Id,tspl_BankReco_Detail.Document_No,tspl_BankReco_Detail.Payment_code_reco as OldPaymentMode,"
                qry += " (TSPL_RECEIPT_HEADER.Payment_Code  ) as NewPaymentMode  from tspl_BankReco_Detail"
                qry += " left outer join TSPL_RECEIPT_HEADER on TSPL_RECEIPT_HEADER.Receipt_No  =tspl_BankReco_Detail.Document_No"
                qry += " where tspl_BankReco_Detail.Document_Type='Receipt' and len(isnull(tspl_BankReco_Detail.Payment_code_reco,''))<=0"
                qry += " )xxx"
                qry += " inner join tspl_BankReco_Detail on tspl_BankReco_Detail.Reconciliation_Id=xxx.Reconciliation_Id and tspl_BankReco_Detail.Document_No=xxx.Document_No"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update tspl_BankReco_Detail set Payment_Code_reco=xxx.NewPaymentMode from ("
                qry += " select tspl_BankReco_Detail.Reconciliation_Id,tspl_BankReco_Detail.Document_No,tspl_BankReco_Detail.Payment_code_reco as OldPaymentMode,"
                qry += " (case when len(isnull( TSPL_PAYMENT_HEADER.Payment_No,''))>0 then TSPL_PAYMENT_HEADER.Payment_Code else case when len(ISNULL(TSPL_RECEIPT_HEADER.Receipt_No,''))>0 then TSPL_RECEIPT_HEADER.Payment_Code else '' end end   ) as NewPaymentMode "

                qry += "  ,TSPL_PAYMENT_HEADER.Payment_No,TSPL_RECEIPT_HEADER.Receipt_No"
                qry += "   from tspl_BankReco_Detail"
                qry += " left outer join TSPL_BANK_REVERSE on TSPL_BANK_REVERSE.Reverse_Code=tspl_BankReco_Detail.Document_No  "
                qry += "  left outer join TSPL_PAYMENT_HEADER on TSPL_PAYMENT_HEADER.Payment_No=TSPL_BANK_REVERSE.Document_No  and TSPL_BANK_REVERSE.Reverse_Document='Payments  '"
                qry += " left outer join TSPL_RECEIPT_HEADER on TSPL_RECEIPT_HEADER.Receipt_No=TSPL_BANK_REVERSE.Document_No and TSPL_BANK_REVERSE.Reverse_Document='Receipts  '"
                qry += " where tspl_BankReco_Detail.Document_Type='Reverse' and len(isnull(tspl_BankReco_Detail.Payment_code_reco,''))<=0"
                qry += " )xxx"
                qry += " inner join tspl_BankReco_Detail on tspl_BankReco_Detail.Reconciliation_Id=xxx.Reconciliation_Id and tspl_BankReco_Detail.Document_No=xxx.Document_No"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '===============================================================================


        ' Richa 
        If (clsCommon.CompairString("5.0.7.38", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.38") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try


                check = CheckColumnExist("TSPL_MILK_SAMPLE_DETAIL", "Commission_Pers", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SAMPLE_DETAIL", "Commission_Pers", trans)

                    qry = "alter table TSPL_MILK_SAMPLE_DETAIL add default 0 for Commission_Pers"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_SAMPLE_DETAIL", "Commission_Amount", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SAMPLE_DETAIL", "Commission_Amount", trans)

                    qry = "alter table TSPL_MILK_SAMPLE_DETAIL add default 0 for Commission_Amount"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_SAMPLE_DETAIL", "EMP_Pers", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SAMPLE_DETAIL", "EMP_Pers", trans)

                    qry = "alter table TSPL_MILK_SAMPLE_DETAIL add default 0 for EMP_Pers"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_SAMPLE_DETAIL", "EMP_Amount", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SAMPLE_DETAIL", "EMP_Amount", trans)

                    qry = "alter table TSPL_MILK_SAMPLE_DETAIL add default 0 for EMP_Amount"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_SAMPLE_DETAIL", "NET_AMOUNT", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SAMPLE_DETAIL", "NET_AMOUNT", trans)

                    qry = "alter table TSPL_MILK_SAMPLE_DETAIL add default 0 for NET_AMOUNT"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_SRN_HEAD", "Is_Incentive_Created", DBDataType.varchar_Type, 1, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_SRN_HEAD", "Is_Incentive_Created", trans)

                    qry = "alter table TSPL_MILK_SRN_HEAD add default 'N' for Is_Incentive_Created"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_Shift_End_HEAD", "Provision_Amount", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_Shift_End_HEAD", "Provision_Amount", trans)

                    qry = "alter table TSPL_MILK_Shift_End_HEAD add default 0 for Provision_Amount"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                check = CheckColumnExist("TSPL_MILK_Shift_End_HEAD", "Deduction_of_Transporter", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_MILK_Shift_End_HEAD", "Deduction_of_Transporter", trans)

                    qry = "alter table TSPL_MILK_Shift_End_HEAD add default 0 for Deduction_of_Transporter"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                ''-------------------------------------
                'check = CheckColumnExist("TSPL_MILK_Shift_End_HEAD", "Provision_Amount", DBDataType.decimal_Type, 0, 0, trans)
                'If check > 0 Then
                '    DropConstraint("TSPL_MILK_Shift_End_HEAD", "Provision_Amount", trans)

                '    qry = "alter table TSPL_MILK_Shift_End_HEAD add default 'N' for Provision_Amount"
                '    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                'End If






                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.7.60", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.60") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                check = CheckColumnExist("TSPL_EX_PI_DETAIL", "Conv_Factor", DBDataType.decimal_Type, 18, 2, trans)
                If check > 0 Then
                    DropConstraint("TSPL_EX_PI_DETAIL", "Conv_Factor", trans)
                    qry = "alter table TSPL_EX_PI_DETAIL alter column Conv_Factor float null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                check = CheckColumnExist("TSPL_EX_COMMERCIAL_INVOICE_DETAIL", "Conv_Factor", DBDataType.decimal_Type, 18, 2, trans)
                If check > 0 Then
                    DropConstraint("TSPL_EX_COMMERCIAL_INVOICE_DETAIL", "Conv_Factor", trans)
                    qry = "alter table TSPL_EX_COMMERCIAL_INVOICE_DETAIL alter column Conv_Factor float null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                check = CheckColumnExist("TSPL_EX_PI_DETAIL_HISTORY", "Conv_Factor", DBDataType.decimal_Type, 18, 2, trans)
                If check > 0 Then
                    DropConstraint("TSPL_EX_PI_DETAIL_HISTORY", "Conv_Factor", trans)
                    qry = "alter table TSPL_EX_PI_DETAIL_HISTORY alter column Conv_Factor float null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                check = CheckColumnExist("TSPL_EX_SALE_INVOICE_DETAIL", "Conv_Factor", DBDataType.decimal_Type, 18, 2, trans)
                If check > 0 Then
                    DropConstraint("TSPL_EX_SALE_INVOICE_DETAIL", "Conv_Factor", trans)
                    qry = "alter table TSPL_EX_SALE_INVOICE_DETAIL alter column Conv_Factor float null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '============================Preeti gupta================01-Mar-2016=========================================
        If (clsCommon.CompairString("5.0.7.66", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.7.66", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("delete from TSPL_PROGRAM_MASTER where Program_Code='MP_ISS_ID'", trans)
                clsDBFuncationality.ExecuteNonQuery("delete from TSPL_PROGRAM_MASTER where Program_Code='MP_ISS_RPT'", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If


        If (clsCommon.CompairString("5.0.7.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.7.70", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("update TSPL_PROVISION_ENTRY set Vendor_Type='Primary Transporter' where Vendor_Type='Chilling Vendor'", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If
        If (clsCommon.CompairString("5.0.7.85", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.85") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_REQUISITION_HEAD alter column remarks varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '===================== richa agarwal ADD non clustering index for ap invoice entry and ar invoice entry================= BM00000009054
        If (clsCommon.CompairString("5.0.7.85", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.85") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckIndexExists("forARInvoiceEntryTable", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [forARInvoiceEntryTable] " & _
                    " ON [dbo].[TSPL_Customer_Invoice_Detail] ([SNo]) " & _
                    " INCLUDE ([Document_No])  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If

                If CheckIndexExists("forApInvoiceEntryTable", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [forApInvoiceEntryTable] " & _
                    " ON [dbo].[TSPL_VENDOR_INVOICE_DETAIL] ([Detail_Line_No]) " & _
                    " INCLUDE([Document_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                If CheckIndexExists("forApInvoiceEntryTable2", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [forApInvoiceEntryTable2]" & _
                    " ON [dbo].[TSPL_VENDOR_INVOICE_HEAD] ([is_For_TDS],[Invoice_Type]) " & _
                    " INCLUDE([Vendor_Code], [Vendor_Name], [Vendor_Invoice_No], [Vendor_Invoice_Date], [Document_No], [Invoice_Entry_Date], [Posting_Date], " & _
                    " [Account_Set], [Document_Type], [RefDocNo], [Against_POInvoice_No], [Against_PurchaseReturn_No], [Against_Acquisition], [Against_MillkPurchaseInvoice_No], [Against_BulkMillkPurchaseInvoice_No], [Against_Asset_Work], [Against_VCGL], [Hirerachy_Level_Code], [Cost_Centre_Fin_Level_Code])"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '======================================================================


        '===================== richa agarwal ADD non clustering index for item master finder================= BM00000009054
        If (clsCommon.CompairString("5.0.7.88", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.0.7.88") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckIndexExists("ForItemMasterFinder1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForItemMasterFinder1] " & _
                        " ON [dbo].[TSPL_SD_SALE_INVOICE_DETAIL] ([Item_Code],[Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If

                If CheckIndexExists("ForItemMasterFinder2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForItemMasterFinder2] " & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_DETAIL] ([Item_Code],[Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                If CheckIndexExists("ForItemMasterFinder3", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForItemMasterFinder3] " & _
                    " ON [dbo].[TSPL_IssueReturn_DETAIL] ([Item_Code],[Unit_code])"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForItemMasterFinder4", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForItemMasterFinder4] " & _
                    " ON [dbo].[TSPL_PURCHASE_ORDER_DETAIL] ([Item_Code],[Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                If CheckIndexExists("ForItemMasterFinder5", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForItemMasterFinder5] " & _
                    " ON [dbo].[TSPL_SD_SALES_ORDER_DETAIL] ([Item_Code],[Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForItemMasterFinder6", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForItemMasterFinder6] " & _
                    " ON [dbo].[TSPL_ADJUSTMENT_DETAIL] ([Item_Code],[Unit_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If


        ''richa agarwal ticket no. BM00000008890
        If (clsCommon.CompairString("5.0.7.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.7.95", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_SD_SALES_ORDER_DETAIL alter column Conv_Factor decimal(18,6) ", trans)

                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MILK_SRN_DETAIL alter column FAT_KG decimal(10,3) not null ", trans)

                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_MILK_SRN_DETAIL alter column SNF_KG decimal(10,3) not null  ", trans)

                If CheckIndexExists("forJournalBookIndex3", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [forJournalBookIndex3] " & _
                    " ON [dbo].[TSPL_JOURNAL_MASTER] ([Authorized],[Voucher_Date]) " & _
                    " INCLUDE ([Voucher_No],[Voucher_Desc]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If

                If CheckIndexExists("forJournalBookIndex2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [forJournalBookIndex2] " & _
                    " ON [dbo].[TSPL_JOURNAL_DETAILS] ([Account_code]) " & _
                    " INCLUDE ([Voucher_No],[Account_Desc],[Amount]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If


        ''richa agarwal ticket no. BM00000008800
        If (clsCommon.CompairString("5.0.7.96", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.7.96", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckIndexExists("ForMilkReceiptIndex1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForMilkReceiptIndex1] " & _
                    " ON [dbo].[TSPL_MILK_SAMPLE_HEAD] ([MILK_RECEIPT_CODE])"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If

                If CheckIndexExists("ForMilkReceiptIndex2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForMilkReceiptIndex2] " & _
                    " ON [dbo].[TSPL_MILK_RECEIPT_DETAIL] ([DOC_CODE],[VLC_DOC_CODE],[SAMPLE_NO]) " & _
                    " INCLUDE ([VLC_CODE],[ROUTE_CODE],[VSP_CODE],[VEHICLE_CODE],[NO_OF_CANS],[MILK_WEIGHT],[TYPE],[MILK_TYPE],[SAMPLE_NO_VALUES],[MCC_CODE],[DOC_DATE],[SHIFT],[COMM_PORT],[MACHINE_NO],[IS_MANUAL],[Item_Code],[IS_SAMPLEED],[Eco_Pro_Name],[ACC_WEIGHT],[UOM_Code],[Conversion_factor],[ACC_WEIGHT_LTR],[Other_VEHICLE],[OTHER_VLC]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                If CheckIndexExists("ForMilkReceiptIndex4", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForMilkReceiptIndex4] " & _
                    " ON [dbo].[TSPL_MILK_RECEIPT_DETAIL] ([VLC_CODE]) " & _
                    " INCLUDE ([VLC_DOC_NUM]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_Dispatch_Detail_BulkSaleIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_Dispatch_Detail_BulkSaleIndex]" & _
                    " ON [dbo].[TSPL_Dispatch_Detail_BulkSale] ([Item_Code],[Document_No],[Qty])" & _
                    " INCLUDE ([Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_Dispatch_Detail_BulkSaleIndex1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_Dispatch_Detail_BulkSaleIndex1] " & _
                    " ON [dbo].[TSPL_Dispatch_Detail_BulkSale_Trade] ([Item_Code],[Document_No],[Qty]) " & _
                    " INCLUDE ([Unit_code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("forTSPL_PP_ISSUE_ITEM_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [forTSPL_PP_ISSUE_ITEM_DETAILIndex] " & _
                    " ON [dbo].[TSPL_PP_ISSUE_ITEM_DETAIL] ([From_Loaction_Code],[Item_Code],[Issue_Code],[Qty]) " & _
                    " INCLUDE ([Unit_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_CSA_TRANSFER_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_CSA_TRANSFER_DETAILIndex]" & _
                    " ON [dbo].[TSPL_CSA_TRANSFER_DETAIL] ([Item_Code],[DOC_CODE],[Qty]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckIndexExists("forTSPL_Quality_Check_BulkSaleIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [forTSPL_Quality_Check_BulkSaleIndex]" & _
                    " ON [dbo].[TSPL_Quality_Check_BulkSale] ([LoadingTanker_No]) " & _
                    " INCLUDE ([QC_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_LOADING_TANKER_DETAIL_BULKSALEIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_LOADING_TANKER_DETAIL_BULKSALEIndex] " & _
                    " ON [dbo].[TSPL_LOADING_TANKER_DETAIL_BULKSALE] ([Location_Code],[Item_Code],[LoadingTanker_No],[Quantity]) " & _
                    " INCLUDE ([Silo_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_ADJUSTMENT_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_ADJUSTMENT_DETAILIndex]" & _
                    " ON [dbo].[TSPL_ADJUSTMENT_DETAIL] ([Item_Code],[Adjustment_No],[Item_Quantity]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_ADJUSTMENT_HEADERIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_ADJUSTMENT_HEADERIndex] " & _
                    " ON [dbo].[TSPL_ADJUSTMENT_HEADER] ([Posted],[Trans_Type],[IsMilkType],[MainLocationCode]) " & _
                    " INCLUDE ([Adjustment_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_MCC_Dispatch_ChallanIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_MCC_Dispatch_ChallanIndex]" & _
                    " ON [dbo].[TSPL_MCC_Dispatch_Challan] ([MCC_Code],[isPosted],[Item_Code],[Chalan_NO],[Net_Qty]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_PP_STD_ADD_REMOVE_ITEM_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_PP_STD_ADD_REMOVE_ITEM_DETAILIndex]" & _
                    " ON [dbo].[TSPL_PP_STD_ADD_REMOVE_ITEM_DETAIL] ([Item_Code],[Loaction_Code],[Standardization_Code]) " & _
                    " INCLUDE ([Unit_Code],[ADD_REMOVE_QTY],[ADD_REMOVE_TYPE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_PP_BATCH_ITEM_PRODUCTION_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_PP_BATCH_ITEM_PRODUCTION_DETAILIndex]" & _
                    " ON [dbo].[TSPL_PP_BATCH_ITEM_PRODUCTION_DETAIL] ([Item_Code],[STD_Loaction_Code],[Standardization_Code]) " & _
                    " INCLUDE ([Unit_Code],[Produced_Qty]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_PP_PRODUCTION_CONSUMPTION_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_PP_PRODUCTION_CONSUMPTION_DETAILIndex]" & _
                    " ON [dbo].[TSPL_PP_PRODUCTION_CONSUMPTION_DETAIL] ([CONSM_ITEM_CODE],[LOCATION_CODE],[PROD_ENTRY_CODE]) " & _
                    " INCLUDE ([CONSM_QTY],[UNIT_CODE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_PP_SP_ADD_REMOVE_ITEM_DETAILIndex", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_PP_SP_ADD_REMOVE_ITEM_DETAILIndex]" & _
                    " ON [dbo].[TSPL_PP_SP_ADD_REMOVE_ITEM_DETAIL] ([Item_Code],[Loaction_Code],[STAGE_PROCESS_CODE]) " & _
                    " INCLUDE ([Unit_Code],[ADD_REMOVE_QTY],[ADD_REMOVE_TYPE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("ForTSPL_PP_PRODUCTION_ENTRY_DETAILIndex5", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [ForTSPL_PP_PRODUCTION_ENTRY_DETAILIndex5]" & _
                    " ON [dbo].[TSPL_PP_PRODUCTION_ENTRY_DETAIL] ([ITEM_CODE],[PROD_ENTRY_CODE]) " & _
                    " INCLUDE ([RECEIPT_QTY],[UNIT_CODE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If


        ''richa agarwal ticket no. BM00000008890
        If (clsCommon.CompairString("5.0.8.12", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.12", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckIndexExists("ForTSPL_MILK_SRN_HEADTableIndex", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [ForTSPL_MILK_SRN_HEADTableIndex] " & _
                    " ON [dbo].[TSPL_MILK_SRN_HEAD] ([Posted]) " & _
                    " INCLUDE ([DOC_CODE],[DOC_DATE],[MILK_SAMPLE_CODE],[VLC_CODE],[ROUTE_CODE],[VSP_CODE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        ''richa agarwal 
        If (clsCommon.CompairString("5.0.8.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.17", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckIndexExists("TSPL_MP_MASTER_ForIndex1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [TSPL_MP_MASTER_ForIndex1]" & _
                    " ON [dbo].[TSPL_MP_MASTER] ([VLC_Code]) " & _
                    " INCLUDE ([MP_Code_VLC_Uploader]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("TSPL_MILK_SRN_HEAD_Index1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [TSPL_MILK_SRN_HEAD_Index1]" & _
                    " ON [dbo].[TSPL_MILK_SRN_HEAD] ([MCC_CODE],[ROUTE_CODE])" & _
                    " INCLUDE ([DOC_CODE],[DOC_DATE],[SHIFT],[VLC_CODE]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("TSPL_VLC_DATA_UPLOADER_ForIndex5", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [TSPL_VLC_DATA_UPLOADER_ForIndex5]" & _
                    " ON [dbo].[TSPL_VLC_DATA_UPLOADER] ([MCC_Code],[Route_No]) " & _
                    " INCLUDE ([File_Date],[shift],[VLC_CODE],[MP_CODE],[qty],[Amount],[fat_KG],[snf_KG],[Uom_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        ''richa agarwal 
        If (clsCommon.CompairString("5.0.8.18", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.18", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckIndexExists("TSPL_PAYMENT_DETAIL_iNDEX_NonClustered", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [TSPL_PAYMENT_DETAIL_iNDEX_NonClustered] " & _
                    " ON [dbo].[TSPL_PAYMENT_DETAIL] ([Document_No],[Post]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("TSPL_PAYMENT_DETAIL_iNDEX_NonClustered1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [TSPL_PAYMENT_DETAIL_iNDEX_NonClustered1] " & _
                    " ON [dbo].[TSPL_PAYMENT_DETAIL] ([Payment_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.8.24", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.24", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_prospect_head alter column QueryRecBy varchar(50) null ", trans)
                clsDBFuncationality.ExecuteNonQuery("alter table tspl_prospect_head alter column Query_Value varchar(50) null ", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        ''richa agarwal 
        If (clsCommon.CompairString("5.0.8.39", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.39", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If CheckIndexExists("For_VndrLdgr_NCIndex1", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex1]" & _
                    " ON [dbo].[TSPL_VENDOR_INVOICE_HEAD] ([Vendor_Invoice_No],[Against_POInvoice_No]) " & _
                    " INCLUDE([Vendor_Invoice_Date], [Document_No], [Document_Type], [Document_Total], [Balance_Amt], [Due_Date], [CURRENCY_CODE], [ConvRate]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex2", trans) = 0 Then
                    qry = "CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex2] " & _
                    " ON [dbo].[TSPL_TRANSFER_ORDER_HEAD] ([Status],[RMDA_Code]) " & _
                   " INCLUDE([DOC_Total_Amt]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex3", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex3] " & _
                    " ON [dbo].[TSPL_PAYMENT_DETAIL] ([Payment_No]) " & _
                    " INCLUDE([Document_No], [Applied_Amount]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex4", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex4] " & _
                    " ON [dbo].[TSPL_VENDOR_INVOICE_HEAD] ([Against_VCGL]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex5", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex5] " & _
                    " ON [dbo].[TSPL_VCGL_Detail] ([Row_Type]) " & _
                    " INCLUDE([Document_No], [VCGL_Code], [VCGL_Name], [Dr_Amount], [Cr_Amount]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex6", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex6]" & _
                    " ON [dbo].[TSPL_VCGL_Head] ([Status]) " & _
                    " INCLUDE([Document_No], [Document_Date], [Location_Segment], [Remarks], [Posting_Date], [Comp_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex7", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex7] " & _
                    " ON [dbo].[TSPL_PAYMENT_HEADER] ([Payment_Type],[Is_Security]) " & _
                    " INCLUDE([Payment_No], [Payment_Date], [Payment_Post_Date], [Vendor_Code], [Cheque_No], [Cheque_Date], [Posted], [Comp_Code], [Debit_Account], [CURRENCY_CODE], [ConvRate], [EXCHANGE_LOSS_AMT], [EXCHANGE_GAIN_AMT]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_VndrLdgr_NCIndex8", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_VndrLdgr_NCIndex8] " & _
                    " ON [dbo].[TSPL_VCGL_Head] ([Document_Type],[Status]) " & _
                    " INCLUDE([Document_No], [Document_Date], [Location_Segment], [VC_Code], [VC_Name], [Remarks], [Posting_Date], [Amount_Type], [Amount], [Comp_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If
        If (clsCommon.CompairString("5.0.8.41", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.41", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckIndexExists("For_SaleRegister_Tax", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_SaleRegister_Tax]" & _
                      " ON [dbo].[TSPL_SD_SALE_INVOICE_DETAIL] (Tax1,Tax2,Tax3,Tax4,Tax5,Tax6,Tax7,Tax8,Tax9,Tax10)" & _
                      " INCLUDE (Tax1_Amt,Tax2_Amt,Tax3_Amt,Tax4_Amt,Tax5_Amt,Tax6_Amt,Tax7_Amt,Tax8_Amt,Tax9_Amt,Tax10_Amt)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.8.44", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.44", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckIndexExists("For_transfer_NC_Index1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_transfer_NC_Index1] " & _
                    " ON [dbo].[TSPL_TRANSFER_ORDER_HEAD] ([Transfer_Type],[Status],[Is_Status_IN]) " & _
                    " INCLUDE([Document_No], [Document_Date], [From_Location], [To_Location], [Tax_Group]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_transfer_NC_Index2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_transfer_NC_Index2] " & _
                    " ON [dbo].[TSPL_TRANSFER_ORDER_DETAIL] ([Document_No]) " & _
                    " INCLUDE([Line_No], [Row_Type], [Item_Code], [MRP], [Out_Qty], [Unit_code], [Item_Cost], [TAX1_Rate], [TAX2_Rate], [TAX3_Rate], [TAX4_Rate], [TAX5_Rate], [TAX6_Rate], [TAX7_Rate], [TAX8_Rate], [TAX9_Rate], [TAX10_Rate], [Disc_Per], [Alt_Unit_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("For_transfer_NC_Index3", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_transfer_NC_Index3] " & _
                    " ON [dbo].[TSPL_TRANSFER_ORDER_HEAD] ([Transfer_Type],[TransferOutNo]) " & _
                    " INCLUDE([Document_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                qry = "alter table TSPL_VCGL_Head alter column VC_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.8.51", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.51", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "alter table TSPL_TANKER_MASTER alter column Description varchar(200) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table Tspl_Gate_Entry_Details alter column Vendor_Desc varchar(200) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Weighment_Detail alter column Vendor_Desc varchar(200) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                qry = "alter table TSPL_QUALITY_CHECK alter column Vendor_Desc varchar(200) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.8.51", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.51", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_LEAVE_ALLOTMENT alter column EMP_CODE VARCHAR(12) NULL", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If



        If (clsCommon.CompairString("5.0.8.57", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.57", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try


                If clsPostCreateTable.CheckPrimaryKey("TSPL_JOURNAL_MASTER", "Voucher_No", trans) = True Then
                Else

                    qry = "alter table TSPL_JOURNAL_MASTER add primary key(Voucher_No) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_JOURNAL_DETAILS", "Voucher_No", trans) = True Then
                Else

                    qry = "alter table TSPL_JOURNAL_DETAILS add FOREIGN KEY(Voucher_No) references TSPL_JOURNAL_MASTER(Voucher_No)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                '=================shivani
                If CheckIndexExists("For_TSPL_JOURNAL_MASTER_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_TSPL_JOURNAL_MASTER_NC_INDEX1]" & _
                          " ON [dbo].[TSPL_JOURNAL_MASTER] ([Voucher_Date])" & _
                          " INCLUDE([Journal_No], [Voucher_No], [Source_Code], [Posting_Date], [Voucher_Desc], [Remarks], [CustVend_Name], [Authorized], [Total_Debit_Amt], [Total_Credit_Amt], [Created_By], [Modify_By], [Comp_Code])"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckIndexExists("For_TSPL_JOURNAL_MASTER_NC_Index5", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [For_TSPL_JOURNAL_MASTER_NC_Index5] " & _
                          " ON [dbo].[TSPL_JOURNAL_MASTER] ([Type])"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.8.63", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.63", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_IssueItemToAssembledAsset_Head ALTER COLUMN ASSET_CODE VARCHAR(50) NULL", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.0.8.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.70", exeVersion) = CompairStringResult.Equal) Then
            Try
                Dim strCon As String = clsERPFuncationality.GetConstraint("TSPL_JOURNAL_MASTER", "Authorized")
                If clsCommon.myLen(strCon) > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("alter table TSPL_JOURNAL_MASTER DROP  " + strCon + "")
                    clsDBFuncationality.ExecuteNonQuery("ALTER TABLE TSPL_JOURNAL_MASTER ADD DEFAULT 'N' FOR Authorized")
                End If
            Catch ex As Exception

                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.8.71", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.71", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "select count(*) from information_schema.tables where table_name='tspl_csa_sale_patti_return_detail' "
                check = clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry, trans))
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("drop table tspl_csa_sale_patti_return_detail", trans)
                End If

                qry = "select count(*) from information_schema.tables where table_name='tspl_csa_sale_patti_return_head' "
                check = clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qry, trans))
                If check > 0 Then
                    clsDBFuncationality.ExecuteNonQuery("drop table tspl_csa_sale_patti_return_head", trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        ''RICHA AGARWAL CREATE NON CLUSTERED INDEX
        If (clsCommon.CompairString("5.0.8.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.86", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckIndexExists("FOR_PENDINGDISPATCH_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_PENDINGDISPATCH_NC_INDEX1] " & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_HEAD] ([Status],[Trans_Type]) " & _
                    " INCLUDE ([Document_Code],[Customer_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_PENDINGDISPATCH_NC_INDEX2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_PENDINGDISPATCH_NC_INDEX2]" & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_DETAIL] ([Scheme_Item]) " & _
                    " INCLUDE ([DOCUMENT_CODE],[Line_No],[Item_Code],[Qty],[Unit_code],[Location],[MRP],[Assessable],[Scheme_Code],[Delivery_Code_PS]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_PRODUCTiNVOICE_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_PRODUCTiNVOICE_NC_INDEX1] " & _
                    " ON [dbo].[TSPL_SD_SALE_INVOICE_HEAD] ([Trans_Type],[Invoice_Type]) " & _
                    " INCLUDE ([Document_Code],[Document_Date],[Customer_Code],[Status],[Total_Amt],[Comments],[Against_Shipment_No]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_sHIPMENTHEAD_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_sHIPMENTHEAD_NC_INDEX1] " & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_HEAD] ([Against_Sales_Order]) " & _
                    " INCLUDE ([Document_Code]) "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_ITEM_UOM_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_ITEM_UOM_NC_INDEX1] " & _
                    " ON [dbo].[TSPL_ITEM_UOM_DETAIL] ([Item_Code],[UOM_Code]) " & _
                    " INCLUDE ([Conversion_Factor])  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_sHIPMENTHEADDetail_NC_INDEX1", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_sHIPMENTHEADDetail_NC_INDEX1]" & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_DETAIL] ([Scheme_Item]) " & _
                    " INCLUDE ([DOCUMENT_CODE],[Item_Code],[Qty],[Unit_code],[Delivery_Code])  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_sHIPMENTHEAD_NC_INDEX2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_sHIPMENTHEAD_NC_INDEX2] " & _
                    " ON [dbo].[TSPL_SD_SHIPMENT_HEAD] ([Status],[Trans_Type]) " & _
                    " INCLUDE ([Document_Code],[Vehicle_Code])  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If CheckIndexExists("FOR_DELIVERY_NOTE_NC_INDEX2", trans) = 0 Then
                    qry = " CREATE NONCLUSTERED INDEX [FOR_DELIVERY_NOTE_NC_INDEX2]" & _
                    " ON [dbo].[TSPL_DELIVERY_NOTE_DETAIL_FRESHSALE] ([Document_No]) " & _
                    " INCLUDE ([Item_Code],[Unit_code],[Qty])  "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.0.8.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.8.91", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                check = CheckColumnExist("TSPL_TRANSFER_ORDER_HEAD", "Price_Code", DBDataType.int_Type, 0, 0, trans)

                If check > 0 Then
                    DropConstraint("TSPL_TRANSFER_ORDER_HEAD", "Price_Code", trans)
                    qry = "alter table TSPL_TRANSFER_ORDER_HEAD drop column Price_Code"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.9.26", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.26", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "Item_Basic_Net", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "Item_Basic_Net", trans)
                End If

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "Item_Basic_Price", DBDataType.decimal_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "Item_Basic_Price", trans)
                End If

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "Item_Code", DBDataType.varchar_Type, 50, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "Item_Code", trans)
                End If

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "Price_Code", DBDataType.varchar_Type, 12, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "Price_Code", trans)
                End If

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "Start_Date", DBDataType.date_Type, 0, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "Start_Date", trans)
                End If

                check = CheckColumnExist("TSPL_ITEM_PRICE_MASTER", "UOM", DBDataType.varchar_Type, 12, 0, trans)
                If check > 0 Then
                    DropConstraint("TSPL_ITEM_PRICE_MASTER", "UOM", trans)
                End If
                qry = "alter table TSPL_ITEM_PRICE_MASTER add primary key (Item_Code, UOM, Start_Date, Price_Code, Item_Basic_Net, Item_Basic_Price,Location_Code)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.0.9.31", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.31", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                ''By Balwinder 
                qry = "alter table TSPL_ITEM_PRICE_MASTER alter column Item_Selling_Price decimal(18,6) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If


        If (clsCommon.CompairString("5.0.9.53", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.53", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_FIXED_PARAMETER set Type='ASM',Code='ASM' where Type='ASM/ZM' and Description='Employee Type'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_EMPLOYEE_MASTER set emp_Type='ASM' where emp_Type='ASM/ZM'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try

        End If

        If (clsCommon.CompairString("5.0.9.92", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.0.9.92", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_SCHEME_DETAIL_NEW", "Item_Code", DBDataType.varchar_Type, 12, Nothing, trans) Then
                    qry = "alter table TSPL_SCHEME_DETAIL_NEW alter column Item_Code varchar(12) null"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If


        ''====================================update itemwise additional charge on purchase cycle screen =======================================
        If (clsCommon.CompairString("5.1.0.25", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.25", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                ''1. ====================PO==============================
                qry = "update TSPL_PURCHASE_ORDER_DETAIL set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from TSPL_PURCHASE_ORDER_DETAIL left outer join " & _
                    " (select TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No,TSPL_PURCHASE_ORDER_DETAIL.Line_No,TSPL_PURCHASE_ORDER_DETAIL.Item_Code,TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,TSPL_PURCHASE_ORDER_DETAIL.Unit_code,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code1,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code2,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code3,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code4,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code5,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code6,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code7,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code8,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code9,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Code10,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt1,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt2,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt3,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_amt4,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt5,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt6,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt7,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt8,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt9,TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt10,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt1,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt2,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt3,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3 " & _
                    " ,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt4,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt5,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt6,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt7,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt8,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt9,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(TSPL_PURCHASE_ORDER_HEAD.Add_Charge_Amt10,0) * isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 from TSPL_PURCHASE_ORDER_DETAIL left outer join TSPL_PURCHASE_ORDER_HEAD on TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No=TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_No left outer join ( " & _
                    " select sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty,0)) as totalqty,PurchaseOrder_No from TSPL_PURCHASE_ORDER_DETAIL group by PurchaseOrder_No)PDel on PDel.PurchaseOrder_No=TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No)final on final.PurchaseOrder_No=TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_No and final.Line_No=TSPL_PURCHASE_ORDER_DETAIL.Line_No and final.Item_Code=TSPL_PURCHASE_ORDER_DETAIL.Item_Code and final.Unit_code=TSPL_PURCHASE_ORDER_DETAIL.Unit_code and final.PurchaseOrder_Qty=TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_Qty "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by TSPL_PURCHASE_ORDER_DETAIL.purchaseorder_no order by TSPL_PURCHASE_ORDER_DETAIL.purchaseorder_no) as rsno,TSPL_PURCHASE_ORDER_DETAIL.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_purchase_order_detail left outer join (select TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_No,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(TSPL_PURCHASE_ORDER_DETAIL.ItemAdd_Org_Charge_Amt10,0)) as Org_10 " & _
                      " from TSPL_PURCHASE_ORDER_DETAIL group by TSPL_PURCHASE_ORDER_DETAIL.PurchaseOrder_No)sub on sub.PurchaseOrder_No=TSPL_PURCHASE_ORDER_DETAIL.purchaseorder_no ) " + Environment.NewLine & _
                      " update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''2. ====================GRN==============================
                qry = "update tspl_grn_detail set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from tspl_grn_detail left outer join (" & _
                      " select tspl_grn_head.GRN_No,tspl_grn_detail.Line_No,tspl_grn_detail.Item_Code,tspl_grn_detail.GRN_Qty,tspl_grn_detail.Unit_code,tspl_grn_head.Add_Charge_Code1,tspl_grn_head.Add_Charge_Code2,tspl_grn_head.Add_Charge_Code3,tspl_grn_head.Add_Charge_Code4,tspl_grn_head.Add_Charge_Code5,tspl_grn_head.Add_Charge_Code6,tspl_grn_head.Add_Charge_Code7,tspl_grn_head.Add_Charge_Code8,tspl_grn_head.Add_Charge_Code9,tspl_grn_head.Add_Charge_Code10,tspl_grn_head.Add_Charge_Amt1,tspl_grn_head.Add_Charge_Amt2,tspl_grn_head.Add_Charge_Amt3,tspl_grn_head.Add_Charge_amt4,tspl_grn_head.Add_Charge_Amt5,tspl_grn_head.Add_Charge_Amt6,tspl_grn_head.Add_Charge_Amt7,tspl_grn_head.Add_Charge_Amt8,tspl_grn_head.Add_Charge_Amt9,tspl_grn_head.Add_Charge_Amt10,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt1,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt2,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt3,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt4,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt5,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5 " & _
                      " ,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt6,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt7,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt8,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt9,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_grn_head.Add_Charge_Amt10,0) * isnull(tspl_grn_detail.GRN_Qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 from tspl_grn_detail left outer join tspl_grn_head on tspl_grn_head.GRN_No=tspl_grn_detail.GRN_No left outer join (select sum(isnull(tspl_grn_detail.GRN_Qty,0)) as totalqty,GRN_No from tspl_grn_detail group by GRN_No)PDel on PDel.GRN_No=tspl_grn_head.GRN_No)final on final.GRN_No=tspl_grn_detail.GRN_No and final.Line_No=tspl_grn_detail.Line_No and final.Item_Code=tspl_grn_detail.Item_Code and final.Unit_code=tspl_grn_detail.Unit_code and final.GRN_No=tspl_grn_detail.GRN_No "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by tspl_grn_detail.grn_no order by tspl_grn_detail.grn_no) as rsno,tspl_grn_detail.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_grn_detail left outer join (select tspl_grn_detail.grn_no,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(tspl_grn_detail.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(tspl_grn_detail.ItemAdd_Org_Charge_Amt10,0)) as Org_10 from tspl_grn_detail group by tspl_grn_detail.grn_no)sub on sub.grn_no=tspl_grn_detail.grn_no) " + Environment.NewLine & _
                      " update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1 "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''3. ====================MRN==============================
                qry = "update tspl_mrn_detail set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from tspl_mrn_detail left outer join " & _
                      " (select tspl_mrn_head.mrn_no,tspl_mrn_detail.Line_No,tspl_mrn_detail.Item_Code,tspl_mrn_detail.mrn_qty,tspl_mrn_detail.Unit_code,tspl_mrn_head.Add_Charge_Code1,tspl_mrn_head.Add_Charge_Code2,tspl_mrn_head.Add_Charge_Code3,tspl_mrn_head.Add_Charge_Code4,tspl_mrn_head.Add_Charge_Code5,tspl_mrn_head.Add_Charge_Code6,tspl_mrn_head.Add_Charge_Code7,tspl_mrn_head.Add_Charge_Code8,tspl_mrn_head.Add_Charge_Code9,tspl_mrn_head.Add_Charge_Code10,tspl_mrn_head.Add_Charge_Amt1,tspl_mrn_head.Add_Charge_Amt2,tspl_mrn_head.Add_Charge_Amt3,tspl_mrn_head.Add_Charge_amt4,tspl_mrn_head.Add_Charge_Amt5,tspl_mrn_head.Add_Charge_Amt6,tspl_mrn_head.Add_Charge_Amt7,tspl_mrn_head.Add_Charge_Amt8,tspl_mrn_head.Add_Charge_Amt9,tspl_mrn_head.Add_Charge_Amt10,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt1,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt2,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt3,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt4,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt5,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt6,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt7,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt8,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt9,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_mrn_head.Add_Charge_Amt10,0) * isnull(tspl_mrn_detail.mrn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 " & _
                      " from tspl_mrn_detail left outer join tspl_mrn_head on tspl_mrn_head.mrn_no=tspl_mrn_detail.mrn_no left outer join (select sum(isnull(tspl_mrn_detail.mrn_qty,0)) as totalqty,mrn_no from tspl_mrn_detail group by mrn_no)PDel on PDel.mrn_no=tspl_mrn_head.mrn_no)final on final.mrn_no=tspl_mrn_detail.mrn_no and final.Line_No=tspl_mrn_detail.Line_No and final.Item_Code=tspl_mrn_detail.Item_Code and final.Unit_code=tspl_mrn_detail.Unit_code and final.mrn_no=tspl_mrn_detail.mrn_no "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by tspl_mrn_detail.mrn_no order by tspl_mrn_detail.mrn_no) as rsno,tspl_mrn_detail.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_mrn_detail left outer join (select tspl_mrn_detail.mrn_no,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(tspl_mrn_detail.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(tspl_mrn_detail.ItemAdd_Org_Charge_Amt10,0)) as Org_10 from tspl_mrn_detail group by tspl_mrn_detail.mrn_no " & _
                      " )sub on sub.mrn_no=tspl_mrn_detail.mrn_no) " + Environment.NewLine & _
                      " update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1 "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''4. ====================SRN==============================
                qry = "update tspl_srn_detail set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from tspl_srn_detail left outer join " & _
                      " (select tspl_srn_head.srn_no,tspl_srn_detail.Line_No,tspl_srn_detail.Item_Code,tspl_srn_detail.srn_qty,tspl_srn_detail.Unit_code,tspl_srn_head.Add_Charge_Code1,tspl_srn_head.Add_Charge_Code2,tspl_srn_head.Add_Charge_Code3,tspl_srn_head.Add_Charge_Code4,tspl_srn_head.Add_Charge_Code5,tspl_srn_head.Add_Charge_Code6,tspl_srn_head.Add_Charge_Code7,tspl_srn_head.Add_Charge_Code8,tspl_srn_head.Add_Charge_Code9,tspl_srn_head.Add_Charge_Code10 " & _
                      " ,tspl_srn_head.Add_Charge_Amt1,tspl_srn_head.Add_Charge_Amt2,tspl_srn_head.Add_Charge_Amt3,tspl_srn_head.Add_Charge_amt4,tspl_srn_head.Add_Charge_Amt5,tspl_srn_head.Add_Charge_Amt6,tspl_srn_head.Add_Charge_Amt7,tspl_srn_head.Add_Charge_Amt8,tspl_srn_head.Add_Charge_Amt9,tspl_srn_head.Add_Charge_Amt10,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt1,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt2,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt3,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt4,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt5,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt6,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt7,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt8,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt9,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_srn_head.Add_Charge_Amt10,0) * isnull(tspl_srn_detail.srn_qty,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 " & _
                      " from tspl_srn_detail left outer join tspl_srn_head on tspl_srn_head.srn_no=tspl_srn_detail.srn_no left outer join (select sum(isnull(tspl_srn_detail.srn_qty,0)) as totalqty,srn_no from tspl_srn_detail group by srn_no)PDel on PDel.srn_no=tspl_srn_head.srn_no)final on final.srn_no=tspl_srn_detail.srn_no and final.Line_No=tspl_srn_detail.Line_No and final.Item_Code=tspl_srn_detail.Item_Code and final.Unit_code=tspl_srn_detail.Unit_code and final.srn_no=tspl_srn_detail.srn_no "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by tspl_srn_detail.srn_no order by tspl_srn_detail.srn_no) as rsno,tspl_srn_detail.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_srn_detail left outer join (select tspl_srn_detail.srn_no,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(tspl_srn_detail.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(tspl_srn_detail.ItemAdd_Org_Charge_Amt10,0)) as Org_10 from tspl_srn_detail group by tspl_srn_detail.srn_no " & _
                      " )sub on sub.srn_no=tspl_srn_detail.srn_no ) " + Environment.NewLine & _
                      "	update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1 "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ''==========================================end here======================================================================================

        If (clsCommon.CompairString("5.1.0.30", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.30", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_ROUTE_FREIGHT_DETAILS", "Type", DBDataType.varchar_Type, 20, Nothing, trans) > 0 Then
                    qry = "update TSPL_ROUTE_FREIGHT_DETAILS set type='MT' where coalesce(type,'')=''"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.0.46", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.46", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckColumnExist("TSPL_CSA_PRICE_HEAD", "Doc_Date", DBDataType.datetime_Type, Nothing, Nothing, trans) > 0 Then
                    qry = "update TSPL_CSA_PRICE_HEAD set Doc_Date=convert(date,Created_Date,103) where coalesce(Doc_Date,'')=''"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                DropConstraint("TSPL_WRECKAGE_BOOKING", "WRECKAGE_CODE", trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.0.50", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.50", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                ''5. ====================PI==============================
                qry = "update tspl_pi_detail set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from tspl_pi_detail left outer join " & _
                      " (select tspl_pi_head.pi_no,tspl_pi_detail.Line_No,tspl_pi_detail.Item_Code,tspl_pi_detail.pi_qty,tspl_pi_detail.Unit_code,tspl_pi_head.Add_Charge_Code1,tspl_pi_head.Add_Charge_Code2,tspl_pi_head.Add_Charge_Code3,tspl_pi_head.Add_Charge_Code4,tspl_pi_head.Add_Charge_Code5,tspl_pi_head.Add_Charge_Code6,tspl_pi_head.Add_Charge_Code7,tspl_pi_head.Add_Charge_Code8,tspl_pi_head.Add_Charge_Code9,tspl_pi_head.Add_Charge_Code10,tspl_pi_head.Add_Charge_Amt1,tspl_pi_head.Add_Charge_Amt2,tspl_pi_head.Add_Charge_Amt3,tspl_pi_head.Add_Charge_amt4,tspl_pi_head.Add_Charge_Amt5,tspl_pi_head.Add_Charge_Amt6,tspl_pi_head.Add_Charge_Amt7,tspl_pi_head.Add_Charge_Amt8,tspl_pi_head.Add_Charge_Amt9,tspl_pi_head.Add_Charge_Amt10 " & _
                      " ,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt1,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt2,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt3,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt4,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt5,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt6,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt7,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt8,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt9,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pi_head.Add_Charge_Amt10,0) * isnull(tspl_pi_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 " & _
                      " from tspl_pi_detail left outer join tspl_pi_head on tspl_pi_head.pi_no=tspl_pi_detail.pi_no left outer join (select sum(isnull(tspl_pi_detail.landed_cost_amount,0)) as totalqty,pi_no from tspl_pi_detail group by pi_no)PDel on PDel.pi_no=tspl_pi_head.pi_no)final on final.pi_no=tspl_pi_detail.pi_no and final.Line_No=tspl_pi_detail.Line_No and final.Item_Code=tspl_pi_detail.Item_Code and final.Unit_code=tspl_pi_detail.Unit_code and final.pi_no=tspl_pi_detail.pi_no "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by tspl_pi_detail.pi_no order by tspl_pi_detail.pi_no) as rsno,tspl_pi_detail.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_pi_detail left outer join (select tspl_pi_detail.pi_no,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(tspl_pi_detail.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(tspl_pi_detail.ItemAdd_Org_Charge_Amt10,0)) as Org_10 from tspl_pi_detail group by tspl_pi_detail.pi_no " & _
                      " )sub on sub.pi_no=tspl_pi_detail.pi_no ) " + Environment.NewLine & _
                      "	update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1 "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''6. ====================PR==============================
                qry = "update tspl_pr_detail set ItemAdd_Charge_Code1=final.Add_Charge_Code1,ItemAdd_Charge_Code2=final.Add_Charge_Code2,ItemAdd_Charge_Code3=final.Add_Charge_Code3,ItemAdd_Charge_Code4=final.Add_Charge_Code4,ItemAdd_Charge_Code5=final.Add_Charge_Code5,ItemAdd_Charge_Code6=final.Add_Charge_Code6,ItemAdd_Charge_Code7=final.Add_Charge_Code7,ItemAdd_Charge_Code8=final.Add_Charge_Code8,ItemAdd_Charge_Code9=final.Add_Charge_Code9,ItemAdd_Charge_Code10=final.Add_Charge_Code10,ItemAdd_Org_Charge_Amt1=final.Add_Charge_Amt1,ItemAdd_Org_Charge_Amt2=final.Add_Charge_Amt2,ItemAdd_Org_Charge_Amt3=final.Add_Charge_Amt3,ItemAdd_Org_Charge_Amt4=final.Add_Charge_Amt4,ItemAdd_Org_Charge_Amt5=final.Add_Charge_Amt5,ItemAdd_Org_Charge_Amt6=final.Add_Charge_Amt6,ItemAdd_Org_Charge_Amt7=final.Add_Charge_Amt7,ItemAdd_Org_Charge_Amt8=final.Add_Charge_Amt8,ItemAdd_Org_Charge_Amt9=final.Add_Charge_Amt9,ItemAdd_Org_Charge_Amt10=final.Add_Charge_Amt10, ItemAdd_Calc_Charge_Amt1=final.cal_1,ItemAdd_Calc_Charge_Amt2=final.cal_2,ItemAdd_Calc_Charge_Amt3=final.cal_3,ItemAdd_Calc_Charge_Amt4=final.cal_4,ItemAdd_Calc_Charge_Amt5=final.cal_5,ItemAdd_Calc_Charge_Amt6=final.cal_6,ItemAdd_Calc_Charge_Amt7=final.cal_7,ItemAdd_Calc_Charge_Amt8=final.cal_8,ItemAdd_Calc_Charge_Amt9=final.cal_9,ItemAdd_Calc_Charge_Amt10=final.cal_10,Total_ItemAdd_Charge=isnull(final.cal_1,0) + isnull(final.cal_2,0) + isnull(final.cal_3,0) + isnull(final.cal_4,0) + isnull(final.cal_5,0) + isnull(final.cal_6,0) + isnull(final.cal_7,0) + isnull(final.cal_8,0) + isnull(final.cal_9,0) + isnull(final.cal_10,0) from tspl_pr_detail left outer join " & _
                      " (select tspl_pr_head.pr_no,tspl_pr_detail.Line_No,tspl_pr_detail.Item_Code,tspl_pr_detail.pr_qty,tspl_pr_detail.Unit_code,tspl_pr_head.Add_Charge_Code1,tspl_pr_head.Add_Charge_Code2,tspl_pr_head.Add_Charge_Code3,tspl_pr_head.Add_Charge_Code4,tspl_pr_head.Add_Charge_Code5,tspl_pr_head.Add_Charge_Code6,tspl_pr_head.Add_Charge_Code7,tspl_pr_head.Add_Charge_Code8,tspl_pr_head.Add_Charge_Code9,tspl_pr_head.Add_Charge_Code10,tspl_pr_head.Add_Charge_Amt1,tspl_pr_head.Add_Charge_Amt2,tspl_pr_head.Add_Charge_Amt3,tspl_pr_head.Add_Charge_amt4,tspl_pr_head.Add_Charge_Amt5,tspl_pr_head.Add_Charge_Amt6,tspl_pr_head.Add_Charge_Amt7,tspl_pr_head.Add_Charge_Amt8,tspl_pr_head.Add_Charge_Amt9,tspl_pr_head.Add_Charge_Amt10,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt1,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_1,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt2,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_2,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt3,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_3,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt4,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_4,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt5,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_5,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt6,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_6 " & _
                      " ,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt7,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_7,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt8,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_8,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt9,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_9,round(case when isnull(PDel.totalqty,0)>0 then isnull(tspl_pr_head.Add_Charge_Amt10,0) * isnull(tspl_pr_detail.landed_cost_amount,0) / isnull(PDel.totalqty,0) else 0 end,3) as cal_10 from tspl_pr_detail left outer join tspl_pr_head on tspl_pr_head.pr_no=tspl_pr_detail.pr_no left outer join (select sum(isnull(tspl_pr_detail.landed_cost_amount,0)) as totalqty,pr_no from tspl_pr_detail group by pr_no)PDel on PDel.pr_no=tspl_pr_head.pr_no " & _
                      " )final on final.pr_no=tspl_pr_detail.pr_no and final.Line_No=tspl_pr_detail.Line_No and final.Item_Code=tspl_pr_detail.Item_Code and final.Unit_code=tspl_pr_detail.Unit_code and final.pr_no=tspl_pr_detail.pr_no "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ''=====================adjust of total additional amount ,respect of item
                qry = ";with test_Cte as " + Environment.NewLine & _
                      " (select ROW_NUMBER() over (partition by tspl_pr_detail.pr_no order by tspl_pr_detail.pr_no) as rsno,tspl_pr_detail.*,sub.cal_1,sub.cal_10,sub.cal_2,sub.cal_3,sub.cal_4,sub.cal_5,sub.cal_6,sub.cal_7,sub.cal_8,sub.cal_9,sub.Org_1,sub.Org_10,sub.Org_2,sub.Org_3,sub.Org_4,sub.Org_5,sub.Org_6,sub.Org_7,sub.Org_8,sub.Org_9 " & _
                      " from tspl_pr_detail left outer join (select tspl_pr_detail.pr_no,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt1,0)) as cal_1,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt2,0)) as cal_2,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt3,0)) as cal_3,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt4,0)) as cal_4,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt5,0)) as cal_5,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt6,0)) as cal_6,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt7,0)) as cal_7,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt8,0)) as cal_8,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt9,0)) as cal_9,sum(isnull(tspl_pr_detail.ItemAdd_Calc_Charge_Amt10,0)) as cal_10,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt1,0)) as Org_1,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt2,0)) as Org_2,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt3,0)) as Org_3,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt4,0)) as Org_4,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt5,0)) as Org_5,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt6,0)) as Org_6,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt7,0)) as Org_7,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt8,0)) as Org_8,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt9,0)) as Org_9,max(isnull(tspl_pr_detail.ItemAdd_Org_Charge_Amt10,0)) as Org_10 from tspl_pr_detail group by tspl_pr_detail.pr_no " & _
                      " )sub on sub.pr_no=tspl_pr_detail.pr_no ) " & _
                      "	update test_Cte set ItemAdd_Calc_Charge_Amt1= round(case when cal_1 > Org_1 then isnull(ItemAdd_Calc_Charge_Amt1,0) - (cal_1 - Org_1) else isnull(ItemAdd_Calc_Charge_Amt1,0) + (Org_1 - cal_1) end,3),ItemAdd_Calc_Charge_Amt2= round(case when cal_2 > Org_2 then isnull(ItemAdd_Calc_Charge_Amt2,0) - (cal_2 - Org_2) else isnull(ItemAdd_Calc_Charge_Amt2,0) + (Org_2 - cal_2) end,3),ItemAdd_Calc_Charge_Amt3= round(case when cal_3 > Org_3 then isnull(ItemAdd_Calc_Charge_Amt3,0) - (cal_3 - Org_3) else isnull(ItemAdd_Calc_Charge_Amt3,0) + (Org_3 - cal_3) end,3),ItemAdd_Calc_Charge_Amt4= round(case when cal_4 > Org_4 then isnull(ItemAdd_Calc_Charge_Amt4,0) - (cal_4 - Org_4) else isnull(ItemAdd_Calc_Charge_Amt4,0) + (Org_4 - cal_4) end,3),ItemAdd_Calc_Charge_Amt5= round(case when cal_5 > Org_5 then isnull(ItemAdd_Calc_Charge_Amt5,0) - (cal_5 - Org_5) else isnull(ItemAdd_Calc_Charge_Amt5,0) + (Org_5 - cal_5) end,3),ItemAdd_Calc_Charge_Amt6= round(case when cal_6 > Org_6 then isnull(ItemAdd_Calc_Charge_Amt6,0) - (cal_6 - Org_6) else isnull(ItemAdd_Calc_Charge_Amt6,0) + (Org_6 - cal_6) end,3),ItemAdd_Calc_Charge_Amt7= round(case when cal_7 > Org_7 then isnull(ItemAdd_Calc_Charge_Amt7,0) - (cal_7 - Org_7) else isnull(ItemAdd_Calc_Charge_Amt7,0) + (Org_7 - cal_7) end,3),ItemAdd_Calc_Charge_Amt8= round(case when cal_8 > Org_8 then isnull(ItemAdd_Calc_Charge_Amt8,0) - (cal_8 - Org_8) else isnull(ItemAdd_Calc_Charge_Amt8,0) + (Org_8 - cal_8) end,3),ItemAdd_Calc_Charge_Amt9= round(case when cal_9 > Org_9 then isnull(ItemAdd_Calc_Charge_Amt9,0) - (cal_9 - Org_9) else isnull(ItemAdd_Calc_Charge_Amt9,0) + (Org_9 - cal_9) end,3),ItemAdd_Calc_Charge_Amt10= round(case when cal_10 > Org_10 then isnull(ItemAdd_Calc_Charge_Amt10,0) - (cal_10 - Org_10) else isnull(ItemAdd_Calc_Charge_Amt10,0) + (Org_10 - cal_10) end,3) where rsno=1 "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.0.61", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.61", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_BANK_BOOK set CHEQUE_DATE=convert(varchar,DATEFROMPARTS(left(CHEQUE_DATE,4), SUBSTRING(CHEQUE_DATE,6,2), right(CHEQUE_DATE,2)),103) where len(coalesce(Cheque_Date ,''))>0 and DocType in ('Payment') "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_BANK_BOOK set CHEQUE_DATE=convert(varchar,DATEFROMPARTS(left(CHEQUE_DATE,4), SUBSTRING(CHEQUE_DATE,6,2), right(CHEQUE_DATE,2)),103) where len(coalesce(Cheque_Date ,''))>0 and DocType in ('Reverse') and TransactionType='AP'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.0.67", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.67", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " update TSPL_VENDOR_INVOICE_HEAD set TSPL_VENDOR_INVOICE_HEAD.Total_Add_Charge=TSPL_PI_HEAD.Total_Add_Charge," & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code1=TSPL_PI_HEAD.Add_Charge_Code1,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt1=TSPL_PI_HEAD.Add_Charge_Amt1 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name1=TSPL_PI_HEAD.Add_Charge_Name1, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code2=TSPL_PI_HEAD.Add_Charge_Code2,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt2=TSPL_PI_HEAD.Add_Charge_Amt2 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name2=TSPL_PI_HEAD.Add_Charge_Name2, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code3=TSPL_PI_HEAD.Add_Charge_Code3,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt3=TSPL_PI_HEAD.Add_Charge_Amt3 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name3=TSPL_PI_HEAD.Add_Charge_Name3, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code4=TSPL_PI_HEAD.Add_Charge_Code4,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt4=TSPL_PI_HEAD.Add_Charge_Amt4 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name4=TSPL_PI_HEAD.Add_Charge_Name4, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code5=TSPL_PI_HEAD.Add_Charge_Code5,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt5=TSPL_PI_HEAD.Add_Charge_Amt5 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name5=TSPL_PI_HEAD.Add_Charge_Name5, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code6=TSPL_PI_HEAD.Add_Charge_Code6,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt6=TSPL_PI_HEAD.Add_Charge_Amt6 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name6=TSPL_PI_HEAD.Add_Charge_Name6, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code7=TSPL_PI_HEAD.Add_Charge_Code7,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt7=TSPL_PI_HEAD.Add_Charge_Amt7 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name7=TSPL_PI_HEAD.Add_Charge_Name7, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code8=TSPL_PI_HEAD.Add_Charge_Code8,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt8=TSPL_PI_HEAD.Add_Charge_Amt8 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name8=TSPL_PI_HEAD.Add_Charge_Name8, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code9=TSPL_PI_HEAD.Add_Charge_Code9,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt9=TSPL_PI_HEAD.Add_Charge_Amt9 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name9=TSPL_PI_HEAD.Add_Charge_Name9, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code10=TSPL_PI_HEAD.Add_Charge_Code10,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt10=TSPL_PI_HEAD.Add_Charge_Amt10 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name10=TSPL_PI_HEAD.Add_Charge_Name10 " & _
                      " from TSPL_PI_HEAD where TSPL_VENDOR_INVOICE_HEAD.Against_POInvoice_No=TSPL_PI_HEAD.PI_No " & _
                      " and  coalesce(TSPL_VENDOR_INVOICE_HEAD.Total_Add_Charge,0)<>coalesce(TSPL_PI_HEAD.Total_Add_Charge,0)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_VENDOR_INVOICE_HEAD set TSPL_VENDOR_INVOICE_HEAD.Total_Add_Charge=TSPL_PR_HEAD.Total_Add_Charge," & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code1=TSPL_PR_HEAD.Add_Charge_Code1,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt1=TSPL_PR_HEAD.Add_Charge_Amt1 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name1=TSPL_PR_HEAD.Add_Charge_Name1, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code2=TSPL_PR_HEAD.Add_Charge_Code2,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt2=TSPL_PR_HEAD.Add_Charge_Amt2 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name2=TSPL_PR_HEAD.Add_Charge_Name2, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code3=TSPL_PR_HEAD.Add_Charge_Code3,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt3=TSPL_PR_HEAD.Add_Charge_Amt3 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name3=TSPL_PR_HEAD.Add_Charge_Name3, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code4=TSPL_PR_HEAD.Add_Charge_Code4,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt4=TSPL_PR_HEAD.Add_Charge_Amt4 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name4=TSPL_PR_HEAD.Add_Charge_Name4, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code5=TSPL_PR_HEAD.Add_Charge_Code5,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt5=TSPL_PR_HEAD.Add_Charge_Amt5 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name5=TSPL_PR_HEAD.Add_Charge_Name5, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code6=TSPL_PR_HEAD.Add_Charge_Code6,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt6=TSPL_PR_HEAD.Add_Charge_Amt6 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name6=TSPL_PR_HEAD.Add_Charge_Name6, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code7=TSPL_PR_HEAD.Add_Charge_Code7,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt7=TSPL_PR_HEAD.Add_Charge_Amt7 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name7=TSPL_PR_HEAD.Add_Charge_Name7, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code8=TSPL_PR_HEAD.Add_Charge_Code8,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt8=TSPL_PR_HEAD.Add_Charge_Amt8 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name8=TSPL_PR_HEAD.Add_Charge_Name8, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code9=TSPL_PR_HEAD.Add_Charge_Code9,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt9=TSPL_PR_HEAD.Add_Charge_Amt9 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name9=TSPL_PR_HEAD.Add_Charge_Name9, " & _
                      " TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Code10=TSPL_PR_HEAD.Add_Charge_Code10,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Amt10=TSPL_PR_HEAD.Add_Charge_Amt10 " & _
                      " ,TSPL_VENDOR_INVOICE_HEAD.Add_Charge_Name10=TSPL_PR_HEAD.Add_Charge_Name10 " & _
                      " from TSPL_PR_HEAD where TSPL_VENDOR_INVOICE_HEAD.Against_PurchaseReturn_No=TSPL_PR_HEAD.PR_No " & _
                      " and  coalesce(TSPL_VENDOR_INVOICE_HEAD.Total_Add_Charge,0)<>coalesce(TSPL_PR_HEAD.Total_Add_Charge,0)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        If (clsCommon.CompairString("5.1.0.87", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.87", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "IF NOT EXISTS(SELECT 1 FROM TSPL_POS_GROUP_MASTER WHERE GROUP_CODE='CNF') " & _
                "BEGIN " & _
                "INSERT [dbo].[TSPL_POS_GROUP_MASTER] ([GROUP_CODE], [DOC_DATE], [DESCRIPTION], [LEVEL], [CREATE_DATE], [CREATE_BY], [MODIFY_DATE], [MODIFY_BY]) VALUES (N'CNF', '18/Jan/2017', N'C & F', 1,'18/Jan/2017', N'ADMIN', '18/Jan/2017', N'ADMIN') " & _
                "END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "IF NOT EXISTS(SELECT 1 FROM TSPL_POS_GROUP_MASTER WHERE GROUP_CODE='DISTRIBUTER')  " & _
                    " BEGIN " & _
                    " INSERT [dbo].[TSPL_POS_GROUP_MASTER] ([GROUP_CODE], [DOC_DATE], [DESCRIPTION], [LEVEL], [CREATE_DATE], [CREATE_BY], [MODIFY_DATE], [MODIFY_BY]) VALUES (N'DISTRIBUTER','18/Jan/2017', N'Distributor', 2, '18/Jan/2017', N'ADMIN','18/Jan/2017', N'ADMIN')  " & _
                     " End "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "IF NOT EXISTS(SELECT 1 FROM TSPL_POS_GROUP_MASTER WHERE GROUP_CODE='RETAILER') " & _
                        "BEGIN " & _
                        "INSERT [dbo].[TSPL_POS_GROUP_MASTER] ([GROUP_CODE], [DOC_DATE], [DESCRIPTION], [LEVEL], [CREATE_DATE], [CREATE_BY], [MODIFY_DATE], [MODIFY_BY]) VALUES (N'RETAILER', '18/Jan/2017', N'Retailer', 3,'18/Jan/2017', N'ADMIN', '18/Jan/2017', N'ADMIN') " & _
                        "END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.0.88", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.88", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckPrimaryKey("TSPL_VSP_INCENTIVE_Detail", "Doc_Code", trans) Then
                    DropConstraint("TSPL_VSP_INCENTIVE_Detail", "Doc_Code", trans)
                End If

                qry = "insert into TSPL_INVENTORY_MOVEMENT_NEW (Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty)  " + _
                " select 'DispChallan-RET' as Trans_Type,'I' as InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,RetDet.Document_No as Source_Doc_No,convert(varchar, RetDet.Document_Date,103) as  Source_Doc_Date,                               " + _
                " convert(varchar, RetDet.Document_Date,103) as Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,RetDet.Document_Date  as Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty " + _
                " from TSPL_INVENTORY_MOVEMENT_NEW " + _
                " inner join (select Challan_No,Document_No,TSPL_MCC_DISPATCH_CHALLAN_RETURN.Document_Date from TSPL_MCC_DISPATCH_CHALLAN_RETURN where  not exists (select 1 from TSPL_INVENTORY_MOVEMENT_NEW  where Source_Doc_No=TSPL_MCC_DISPATCH_CHALLAN_RETURN.Document_No and  Trans_Type='DispChallan-RET'  " + _
                " )) as RetDet  on RetDet.challan_no =Source_Doc_No" + _
                " where Trans_Type='DispChallan'"

                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.0.91", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.91", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_QC_Parameter_Detail alter column Remarks varchar(1000)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_QC_Parameter_Detail_History alter column Remarks varchar(1000)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.0.94", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.94", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_BANK_GUARANTEE_MASTER  alter column vendor_name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_BANK_REVERSE  alter column vendor_name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_BOOKING_DETAIL_PRODUCTSALE  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Bulk_Price_MASTER  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_CUSTOMER_CRATE_QTY_DETAIL  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_DELIVERY_ORDER_DETAIL_PRODUCTSALE  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_DISPATCH_ADVICE_DETAIL_PRODUCTSALE  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_EX_COMMERCIAL_INVOICE_DETAIL  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_EX_PI_DETAIL  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_EX_PI_DETAIL_HISTORY  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_EX_SALE_INVOICE_DETAIL  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table Tspl_Gate_Entry_Details_History  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD  alter column Vendor_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY  alter column Vendor_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Mcc_Milk_Transport_Invoice_HEAD  alter column Vendor_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_GATE_ENTRY_DETAILS  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_GATE_ENTRY_DETAILS_HISTORY  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_QUALITY_CHECK  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_QUALITY_CHECK_HISTORY  alter column vendor_desc varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_RGP_HEAD  alter column Vendor_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_WEIGHMENT_DETAIL  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MILK_WEIGHMENT_DETAIL_HISTORY  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY  alter column Vendor_Name varchar(200)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRP_PO_DETAIL  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRP_SRN_DETAIL  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Payment_Adjustment_Header  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PAYMENT_HEADER  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PAYMENT_PROCESS_CREDIT_NOTE  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PAYMENT_PROCESS_DEDUCTION  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PAYMENT_PROCESS_ITEM_ISSUE  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PAYMENT_PROCESS_ITEM_ISSUE_RETURN  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PI_HEAD  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PI_REMITTANCE  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PJV_HEAD  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PR_HEAD  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PROSPECT_DETAIL  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PROVISION_ENTRY  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist_New  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Quality_Check_History  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_RECEIPT_ENTRY_HEAD  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Recurring_Payable_INVOICE_HEAD  alter column vendor_name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_REMITTANCE  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_REMITTANCE_ENTRY_DETAIL  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_RGP_HEAD  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALES_ORDER_DETAIL_PRODUCTSALE  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_QUOTATION_DETAIL  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SALE_INVOICE_DETAIL  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SALE_INVOICE_DETAIL_HISTORY  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SALE_RETURN_DETAIL  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SALES_ORDER_DETAIL  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SHIPMENT_DETAIL  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SD_SHIPMENT_DETAIL_HISTORY  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD  alter column Vendor_Name varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY  alter column Vendor_Name varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_ITEM_DETAIL  alter column vendor_desc varchar(200)  "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_ITEM_DETAIL_HIST  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Weighment_Detail_History  alter column vendor_desc varchar(200) "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        ''=============================ADded by preeti Gupta[11/01/2017]
        'If (clsCommon.CompairString("5.1.0.82", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.82", exeVersion) = CompairStringResult.Equal) Then
        '    trans = clsDBFuncationality.GetTransactin()
        '    Try
        '        qry = "Alter Table TSPL_ADJUSTMENT_DETAIL DROP Constraint DF__TSPL_ADJU__Unit___60A82766 "
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)

        '        qry = "alter table TSPL_ADJUSTMENT_DETAIL alter column unit_cost float   null "
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)

        '        trans.Commit()
        '    Catch ex As Exception
        '        trans.Rollback()
        '        clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
        '    End Try
        'End If

        If (clsCommon.CompairString("5.1.0.99", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.0.99", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If clsCommon.CompairString(objCommonVar.CurrentCompanyCode, "UDL") = CompairStringResult.Equal Then
                    qry = "UPDATE TSPL_SD_SALE_INVOICE_HEAD SET Invoice_Type='T'  where trans_type='CSA' AND coalesce(Invoice_Type,'')<>'E'"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.1.22", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.22", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_Payment_Adjustment_Header alter column description varchar(200) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_Payment_Adjustment_detail alter column Remarks varchar(200) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.1.33", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.33", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='R') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ([ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'R', 'Raw Material', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Raw Material') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='F') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'F', 'Finished Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Finished Goods') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='S') " & _
               " BEGIN " & _
               " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
               " ( 'S', 'Semi Finished Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Semi Finished Good') " & _
               " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='A') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'A', 'Asset', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Asset') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='T') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'T', 'Trading Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Trading Good') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='O') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'O', 'Other', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Other') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='J') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ('J', 'Job Work', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Job Work') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='N') " & _
               " BEGIN " & _
               " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
               " ('N', 'Non-Inventory', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,1,'Non-Inventory') " & _
               " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " UPDATE TSPL_ITEM_TYPE_MASTER SET IS_NON_INVENTORY=1 WHERE ITEM_TYPE_CODE='N' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " UPDATE TSPL_ITEM_TYPE_MASTER SET IS_NON_INVENTORY=0 WHERE ITEM_TYPE_CODE!='N' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.1.30", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.30", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update tspl_purchase_order_head set Amendment_By=Modify_By,Amendment_Date=convert(date,Modify_Date,103),Amendment_Code=purchaseOrder_no+'$'+convert(varchar,abandonment_no) where abandonment_no>0 and isnull(amendment_code,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update tspl_purchase_order_head_hist_new set Amendment_By=Modify_By,Amendment_Date=convert(date,Modify_Date,103),Amendment_Code=purchaseOrder_no+'$'+convert(varchar,abandonment_no) where abandonment_no>0 and isnull(amendment_code,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.1.48", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.30", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_PURCHASE_ORDER_HEAD set Closed_By=xx.Modify_By,Closed_Date=convert(date,xx.Modify_Date,103) from TSPL_PURCHASE_ORDER_HEAD left outer join (select max(TSPL_GRN_HEAD.Modify_By) as modify_by, max(TSPL_GRN_HEAD.Modify_Date) as Modify_Date,TSPL_GRN_DETAIL.PO_Id from TSPL_GRN_HEAD left outer join TSPL_GRN_DETAIL on TSPL_GRN_DETAIL.GRN_No=TSPL_GRN_HEAD.GRN_No group by TSPL_GRN_DETAIL.PO_Id)xx on isnull(xx.PO_Id,'')=TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No where isnull(xx.PO_Id,'')=TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No and isnull(TSPL_PURCHASE_ORDER_HEAD.close_yn,'N')='Y'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_PURCHASE_ORDER_HEAD_Hist_New set Closed_By=xx.Modify_By,Closed_Date=convert(date,xx.Modify_Date,103) from TSPL_PURCHASE_ORDER_HEAD_Hist_New left outer join (select max(TSPL_GRN_HEAD.Modify_By) as modify_by, max(TSPL_GRN_HEAD.Modify_Date) as Modify_Date,TSPL_GRN_DETAIL.PO_Id from TSPL_GRN_HEAD left outer join TSPL_GRN_DETAIL on TSPL_GRN_DETAIL.GRN_No=TSPL_GRN_HEAD.GRN_No group by TSPL_GRN_DETAIL.PO_Id)xx on isnull(xx.PO_Id,'')=TSPL_PURCHASE_ORDER_HEAD_Hist_New.PurchaseOrder_No where isnull(xx.PO_Id,'')=TSPL_PURCHASE_ORDER_HEAD_Hist_New.PurchaseOrder_No and isnull(TSPL_PURCHASE_ORDER_HEAD_Hist_New.close_yn,'N')='Y'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.1.49", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.30", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_PURCHASE_ORDER_HEAD set posted_by=(case when isnull(TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.User_Code,'')='' then TSPL_PURCHASE_ORDER_HEAD.Modify_By else TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.User_Code end) from TSPL_PURCHASE_ORDER_HEAD left outer join TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL on TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.Document_Code=TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No where isnull(posted_by,'')='' and TSPL_PURCHASE_ORDER_HEAD.Status=1"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_PURCHASE_ORDER_HEAD_Hist_New set posted_by=(case when isnull(TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.User_Code,'')='' then TSPL_PURCHASE_ORDER_HEAD_Hist_New.Modify_By else TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.User_Code end) from TSPL_PURCHASE_ORDER_HEAD_Hist_New left outer join TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL on TSPL_APPROVAL_LEVEL_TRANSACTION_DETAIL.Document_Code=TSPL_PURCHASE_ORDER_HEAD_Hist_New.PurchaseOrder_No where isnull(posted_by,'')='' and TSPL_PURCHASE_ORDER_HEAD_Hist_New.Status=1"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.1.47", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.47", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='R') " & _
               " BEGIN " & _
               " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ([ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
               " ( 'R', 'Raw Material', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Raw Material') " & _
               " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='F') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'F', 'Finished Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Finished Goods') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='S') " & _
               " BEGIN " & _
               " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
               " ( 'S', 'Semi Finished Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'SemiFinished Goods') " & _
               " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='A') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'A', 'Asset', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Asset') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='T') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'T', 'Trading Good', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Trading') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='O') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ( 'O', 'Other', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Other') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='J') " & _
                " BEGIN " & _
                " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
                " ('J', 'Job Work', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,0,'Job Work') " & _
                " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " IF NOT EXISTS(SELECT 1 FROM TSPL_ITEM_TYPE_MASTER WHERE ITEM_TYPE_CODE='N') " & _
               " BEGIN " & _
               " INSERT [dbo].[TSPL_ITEM_TYPE_MASTER] ( [ITEM_TYPE_CODE], [ITEM_TYPE_NAME], [IS_FREEZ], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE],[IS_NON_INVENTORY],[PREFIX_CODE]) VALUES  " & _
               " ('N', 'Non-Inventory', 1, 'ADMIN', CAST('2017-03-09 00:00:00.000' AS DateTime), NULL, NULL,1,'Non-Inventory') " & _
               " END "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " UPDATE TSPL_ITEM_TYPE_MASTER SET IS_NON_INVENTORY=1 WHERE ITEM_TYPE_CODE='N' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " UPDATE TSPL_ITEM_TYPE_MASTER SET IS_NON_INVENTORY=0 WHERE ITEM_TYPE_CODE!='N' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Asset' WHERE ITEM_TYPE_CODE='A'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Finished Goods' WHERE ITEM_TYPE_CODE='F'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Job Work' WHERE ITEM_TYPE_CODE='J'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Non-Inventory' WHERE ITEM_TYPE_CODE='N'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Other' WHERE ITEM_TYPE_CODE='O'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Raw Material' WHERE ITEM_TYPE_CODE='R'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='SemiFinished Goods' WHERE ITEM_TYPE_CODE='S'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "update TSPL_ITEM_TYPE_MASTER SET PREFIX_CODE='Trading' WHERE ITEM_TYPE_CODE='T'"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        '==========================Preeti[13/04/2017]==============================
        If (clsCommon.CompairString("5.1.1.57", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.1.57", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "	 alter table TSPL_ASSET_DEPRECIATION alter column RoundOff float Null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "	 alter table TSPL_ASSET_DEPRECIATION alter column DepRate float Null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "	 alter table TSPL_ASSET_DEPRECIATION alter column DepRateTax float Null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        If (clsCommon.CompairString("5.1.1.68", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.1.68") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If CheckTriggerExits("trg_Inventory_Movement_WIN", trans) > 0 Then
                    qry = "DROP TRIGGER trg_Inventory_Movement_WIN "
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If
                If CheckTriggerExits("trg_Inventory_Movement_NEW_WIN", trans) = 0 Then
                    qry = "DROP TRIGGER trg_Inventory_Movement_NEW_WIN"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)

                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.1.95", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.1.95") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_ADJUSTMENT_HEADER set Adjustment_Type='ADJ' where coalesce(Adjustment_Type,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '' Parteek Update Length of tax mAster
        If (clsCommon.CompairString("5.1.2.14", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.14") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_tax_master alter column Type varchar(5)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.2.14", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.14") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_CUSTOMER_INVOICE_HEAD alter column Description varchar(250)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.2.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.17") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table tspl_purchase_order_detail alter column Disc_Per decimal(18,8)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.2.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.17") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_GRN_DETAIL alter column Disc_Per decimal(18,8)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.2.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.17") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_MRN_DETAIL alter column Disc_Per decimal(18,8)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.2.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.17") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_SRN_DETAIL alter column Disc_Per decimal(18,8)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.2.17", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString(exeVersion, "5.1.2.17") = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_TAX_MASTER_Hist_Data alter column Type varchar(5)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '=============Richa Ticket No. BM00000003712 on 08/09/2014
        If (clsCommon.CompairString("5.1.2.19", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.19", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_DOCPREFIX_MASTER set Doc_Trans_Type ='NA' where doc_type='CSA Transfer' and ISNULL(Doc_Trans_Type ,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_DOCPREFIX_MASTER set Doc_Trans_Type ='NA' where doc_type='CSA Sale Invoice' and ISNULL(Doc_Trans_Type ,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        ''--End


        If (clsCommon.CompairString("5.1.2.29", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.29", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If clsPostCreateTable.CheckPrimaryKey("TSPL_VENDOR_INVOICE_DETAIL", "SAC_Code", trans) = True Then
                    DropConstraint("TSPL_VENDOR_INVOICE_DETAIL", "SAC_Code", trans)
                End If

                DropConstraint("TSPL_SAC_MASTER", "Code", trans)

                qry = "alter table TSPL_SAC_MASTER alter column Code varchar(12) not null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                If clsPostCreateTable.CheckPrimaryKey("TSPL_SAC_MASTER", "Code", trans) = True Then

                Else
                    qry = "alter table TSPL_SAC_MASTER add primary key(Code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                If clsPostCreateTable.CheckPrimaryKey("TSPL_VENDOR_INVOICE_DETAIL", "SAC_Code", trans) = True Then
                Else
                    qry = "alter table TSPL_VENDOR_INVOICE_DETAIL add FOREIGN KEY(SAC_Code) references TSPL_SAC_MASTER(Code)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                qry = "alter table TSPL_Additional_Charges alter column SAC_Code varchar(12) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_ADDITIONAL_CHARGES_Hist_Data alter column SAC_Code varchar(12) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_VENDOR_MASTER set GSTRegistered =1"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        '=============Richa
        If (clsCommon.CompairString("5.1.2.37", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.37", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "update TSPL_DOCPREFIX_MASTER set Doc_Trans_Type ='NA' where doc_type='Bulk Invoice' and ISNULL(Doc_Trans_Type ,'')=''"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If


        If (clsCommon.CompairString("5.1.2.41", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.41", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                If clsPostCreateTable.CheckPrimaryKey("TSPL_PAYMENT_HEADER", "PurchaseOrder_No_GST", trans) = True Then
                Else
                    qry = "alter table TSPL_PAYMENT_HEADER add FOREIGN KEY(PurchaseOrder_No_GST) references TSPL_PURCHASE_ORDER_HEAD(PurchaseOrder_No)"
                    clsDBFuncationality.ExecuteNonQuery(qry, trans)
                End If

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If

        If (clsCommon.CompairString("5.1.2.47", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.47", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                qry = "update TSPL_VENDOR_INVOICE_HEAD set gstregistered=1 where convert(date,Invoice_Entry_Date ,103)<=convert(date,'30/Jun/2017',103) and gstregistered=0"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_PURCHASE_ORDER_HEAD set gstregistered=1 where convert(date,PurchaseOrder_Date ,103)<=convert(date,'30/Jun/2017',103) and gstregistered=0"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_PI_HEAD set gstregistered=1 where convert(date,PI_Date ,103)<=convert(date,'30/Jun/2017',103) and gstregistered=0"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")

            End Try
        End If
        ''========= Added by Parteek
        If (clsCommon.CompairString("5.1.2.65", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.65", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_SCRAPSALE_HEAD_RETURN alter column Vehicle_code varchar(20) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " update TSPL_PR_HEAD set TSPL_PR_HEAD.GSTRegistered=1 where isnull(TSPL_PR_HEAD.Against_PI ,'')='' "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "update TSPL_PR_HEAD set TSPL_PR_HEAD.GSTRegistered =final.GSTRegistered  from ( Select TSPL_PI_HEAD.GSTRegistered,TSPL_PR_HEAD.PR_No from TSPL_PR_HEAD left outer join TSPL_PI_HEAD on TSPL_PR_HEAD.Against_PI = TSPL_PI_HEAD.PI_No WHERE isnull(TSPL_PR_HEAD.Against_PI ,'')<>'' )final where TSPL_PR_HEAD.PR_No =final.PR_No"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.2.86", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.86", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_JOB_OUTWARD_PRICE_HEAD alter column StartDate varchar(20) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = " alter table TSPL_JOB_OUTWARD_PRICE_HEAD alter column EndDate varchar(20) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_HEAD alter column Created_Date varchar(20) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_HEAD alter column Modified_Date varchar(20) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_detail drop column Created_By"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_detail drop column Modified_By"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_detail drop column Created_Date"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_JOB_OUTWARD_PRICE_detail drop column Modified_Date"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If


        ''--End

        '****************************************************************************************************************
        If (clsCommon.CompairString("5.1.2.78", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.78", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                ' For Additional charge & Additional charge Hist Table 
                qry = "alter table tspl_Additional_Charges alter column description varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_ADDITIONAL_CHARGES_Hist_Data alter column Description varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' For AP INVOICE Entry Screen 
                qry = "alter table TSPL_VENDOR_INVOICE_DETAIL alter column AddChargeDesc varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_VENDOR_INVOICE_HEAD alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' Purchase Order
                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_Purchase_order_head alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' Purchase Order Hist

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' Purchase Invoice

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' MRN

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' MRN Hist

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' SRN

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' SRN Hist
                

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' GRN

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                ' GRN HIST

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name2 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name3 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name4 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name5 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name6 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name7 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name8 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name9 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name10 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

                'sanjay
        If (clsCommon.CompairString("5.1.2.83", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.2.83", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try

                'Previous version missing script
                qry = "alter table TSPL_PURCHASE_ORDER_HEAD_Hist alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_PI_Head alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_MRN_Head alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_MRN_HEAD_HISTORY alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table tspl_SRN_Head alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SRN_HEAD_HISTORY alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_GRN_HEAD_HISTORY alter column Add_Charge_Name1 varchar(500)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                'Previous version missing script

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name1 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name2 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name3 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name4 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name5 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name6 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name7 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name8 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name9 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head alter column Add_Charge_Name10 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                'HIST

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name1 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name2 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name3 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name4 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name5 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name6 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name7 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name8 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name9 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_Customer_Invoice_Head_History alter column Add_Charge_Name10 varchar(500) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)


                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
                'sanjay
        '=============================ADded by Sanjeet [29/09/2017]
        If (clsCommon.CompairString("5.1.3.28", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.28", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "Alter Table TSPL_MCC_RATE_UPLOADER_MCC drop constraint FK__TSPL_MCC___MCC_C__407DB0FB "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.3.35", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.35", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PP_PRODUCTION_CONSUMPTION_DETAIL alter column Location_Code varchar(12) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_PP_CONSUMPTION_WITHOUT_BATCH alter column Location_Code varchar(12) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        If (clsCommon.CompairString("5.1.3.51", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.51", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                If clsPostCreateTable.CheckPrimaryKey("TSPL_TDS_PAYMENT_HEADER", "Location_Code", trans) = True Then
                    DropConstraint("TSPL_TDS_PAYMENT_HEADER", "Location_Code", trans)
                End If
                If clsPostCreateTable.CheckPrimaryKey("TSPL_TDS_PAYMENT_DETAIL", "Location_Code", trans) = True Then
                    DropConstraint("TSPL_TDS_PAYMENT_DETAIL", "Location_Code", trans)
                End If
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        'If (clsCommon.CompairString("5.1.3.64", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.64", exeVersion) = CompairStringResult.Equal) Then
        '    trans = clsDBFuncationality.GetTransactin()
        '    Try
        '        DropConstraint("TSPL_MILK_SHIFT_END_HEAD", "Deduction_of_Transporter", trans)
        '        DropConstraint("TSPL_MILK_SAMPLE_HEAD_SYNC", "MILK_RECEIPT_CODE", trans)
        '        qry = "alter table TSPL_MILK_SHIFT_END_HEAD drop column Reason"
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        '        qry = "alter table TSPL_MILK_SHIFT_END_HEAD drop column Deduction_of_Transporter"
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        '        trans.Commit()
        '    Catch ex As Exception
        '        trans.Rollback()
        '        clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
        '    End Try
        'End If

        If (clsCommon.CompairString("5.1.3.68", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.68", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                DropConstraint("TSPL_GATEPASS_MASTER_DAIRYSALE", "Delivery_Code", trans)
                DropConstraint("TSPL_GATEPASS_DETAIL_DAIRYSALE", "Delivery_Code", trans)
                qry = "alter table TSPL_GATEPASS_MASTER_DAIRYSALE add foreign key (Delivery_Code) REFERENCES TSPL_BOOKING_MATSER(Document_No)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "alter table TSPL_GATEPASS_DETAIL_DAIRYSALE add foreign key (Delivery_Code) REFERENCES TSPL_BOOKING_MATSER(Document_No)"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '================================================Added by preeti Gupta========================
        If (clsCommon.CompairString("5.1.3.70", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.70", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "ALTER TABLE TSPL_MF_MO_MATERIAL alter column CONSM_ITEM_CATEGORY_CODE varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER TABLE TSPL_MF_ISSUE_DETAIL alter column REQ_CODE varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_MF_ISSUE_DETAIL alter column PRODUCTION_LINE_CODE varchar(30) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER TABLE TSPL_MF_RETURN_DETAIL alter column PRODUCTION_LINE_CODE varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_MF_RECEIPT alter column BO_CODE varchar(30) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER TABLE TSPL_MF_RECEIPT_DETAIL alter column PRODUCTION_LINE_CODE varchar(30) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "ALTER TABLE TSPL_MF_RECEIPT_DETAIL alter column PROD_PLAN_CODE varchar(30) null"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                qry = "ALTER TABLE TSPL_MF_CONSUMPTION_DETAIL alter column PRODUCTION_LINE_CODE varchar(30) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.3.78", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.3.78", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = " update TSPL_ITEM_MASTER set STD_FatPer=QC.Fat_Per,STD_SNFPer=QC.SNF_Per from (select Item_Code,MAX(Fat_Per) as Fat_Per,MAX(SNF_Per) as SNF_Per from ( select Item_QCP.Item_Code,Item_QCP.Code as Parameter_Code,(case when QCP.Type='FAT' then Item_QCP.Actual_Range else 0 end) as Fat_Per, " & _
                      " (case when QCP.Type='SNF' then Item_QCP.Actual_Range else 0  end) as SNF_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QCP  left join TSPL_PARAMETER_MASTER QCP  on Item_QCP.Code=QCP.Code ) as QC  group by Item_Code) QC " & _
                      " where TSPL_ITEM_MASTER.Item_Code=QC.Item_Code "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        If (clsCommon.CompairString("5.1.4.54", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.4.54", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_PP_BATCH_ORDER_HEAD alter column Sub_Batch_Code varchar(500) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If

        '=======================Added by preeti Gupta Against Ticket No[KDI/03/05/18-000290]===================
        If (clsCommon.CompairString("5.1.4.90", exeVersion) = CompairStringResult.Greater Or clsCommon.CompairString("5.1.4.90", exeVersion) = CompairStringResult.Equal) Then
            trans = clsDBFuncationality.GetTransactin()
            Try
                qry = "alter table TSPL_SALE_RETURN_GATE_ENTRY_HEAD alter column Man_Transport varchar(500) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                qry = "alter table TSPL_SALE_RETURN_GATE_ENTRY_HEAD alter column Man_vehicle_code varchar(500) null "
                clsDBFuncationality.ExecuteNonQuery(qry, trans)

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
            End Try
        End If
        '==============================
        '****************************************************************************************************************

        '===========================================SHIVANI===================================================================
        '' below code will always be the last statement
        'Try
        '    trans = clsDBFuncationality.GetTransactin()
        '    qry = " select distinct OtherAssemblyFilePathAndName  from TSPL_PROGRAM_MASTER  where isnull(IsLoadFromOtherAssembly ,0)=1"
        '    dt = clsDBFuncationality.GetDataTable(qry, trans)
        '    Dim obj(0 To 1) As Object
        '    obj(0) = exeVersion
        '    obj(1) = trans
        '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt.Rows.Count - 1
        '            Dim AsmName As String = clsCommon.myCstr(dt.Rows(i)("OtherAssemblyFilePathAndName"))
        '            clsCreateAllTables.InvokeMethodSlow(AsmName, "clsPostCreateTable", "Post_AlterOrUpdateAllTables", obj)
        '        Next
        '    End If
        '    trans.Commit()
        'Catch ex As Exception
        '    trans.Rollback()
        '    clsCommon.MyMessageBoxShow(ex.Message, "Script Error")
        'End Try


    End Sub

    '------------------New Subroutine Make For Script Running-----Done By-Monika-28/05/2014---BM00000003099-------------------------------------------
    Private Shared Function CheckPrimaryKey(ByVal table_name As String, ByVal column_name As String, ByVal trans As SqlTransaction, Optional ByVal isDefault_Type As Boolean = False) As Boolean
        Dim qry As String = "select count(*) from INFORMATION_SCHEMA.TABLES where table_name='" + table_name + "'"
        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

        If check <= 0 Then
            Return True
        End If

        If isDefault_Type = False Then
            qry = "select column_name from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name='" + table_name + "' and column_name='" + column_name + "'"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            qry = "Select  SysObjects.[Name] As [Name] From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj] Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] where Tab.name='" + table_name + "' and Col.name ='" + column_name + "'"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Private Shared Sub DropConstraint(ByVal table_name As String, ByVal column_name As String, ByVal trans As SqlTransaction, Optional ByVal CONSTRAINTUNIQUE As String = "N")
        Dim qry As String = "select count(*) from INFORMATION_SCHEMA.TABLES where table_name='" + table_name + "'"
        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

        If check <= 0 Then
            Exit Sub
        End If

        qry = "select CONSTRAINT_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name='" + table_name + "' and column_name='" + column_name + "'"
        If clsCommon.CompairString(CONSTRAINTUNIQUE, "Y") = CompairStringResult.Equal Then
            qry += ""
        End If
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                qry = "alter table " + table_name + " drop constraint [" + clsCommon.myCstr(dr("CONSTRAINT_NAME")) + "]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
            Next
        Else
            'Exit Function
        End If

        'qry = "select name from sys.objects where type_desc like '%constraint%' and object_name(parent_object_id)='" + table_name + "' and name like '%_" + column_name + "_%'"
        'dt = New DataTable()
        'dt = clsDBFuncationality.GetDataTable(qry, trans)

        'If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '    For Each dr As DataRow In dt.Rows
        '        qry = "alter table " + table_name + " drop constraint " + clsCommon.myCstr(dr("NAME")) + ""
        '        clsDBFuncationality.ExecuteNonQuery(qry, trans)
        '    Next
        'Else
        '    ' Exit Function
        'End If

        ''added by richa agarwal on 29/09/2014
        qry = "Select  SysObjects.[Name] As [Name] From SysObjects Inner Join (Select [Name],[ID] From SysObjects) As Tab On Tab.[ID] = Sysobjects.[Parent_Obj] Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID] where Tab.name='" + table_name + "' and Col.name ='" + column_name + "'"
        dt = clsDBFuncationality.GetDataTable(qry, trans)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                qry = "alter table " + table_name + " drop constraint [" + clsCommon.myCstr(dr("NAME")) + "]"
                clsDBFuncationality.ExecuteNonQuery(qry, trans)
            Next
        Else
            Exit Sub
        End If
        ''========================================
    End Sub

    'Private Shared Function CheckColumnExist(ByVal table_name As String, ByVal column_name As String, ByVal datatype As String, ByVal trans As SqlTransaction) As Integer
    '    Dim qry As String = ""
    '    If clsCommon.myLen(datatype) > 0 Then
    '        qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='" + datatype + "'"
    '    Else
    '        qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
    '    End If
    '    Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

    '    Return check
    'End Function
    ''By Balwinder due to not work properly on 2014-10-07
    Public Shared Function CheckColumnExist(ByVal table_name As String, ByVal column_name As String, ByVal datatype As DBDataType, ByVal MaxLength As Integer, ByVal ScaleForDecimal As Integer, ByVal trans As SqlTransaction) As Integer
        Dim qry As String = ""
        'If clsCommon.myLen(datatype) > 0 Then
        '    qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='" + datatype + "'"
        'Else
        '    qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
        'End If

        Select Case datatype
            Case DBDataType.image_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='image'"
            Case DBDataType.int_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='int'"
            Case DBDataType.decimal_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='decimal'"
                If MaxLength > 0 Then
                    qry += " and NUMERIC_PRECISION='" + clsCommon.myCstr(MaxLength) + "'"
                End If
                If ScaleForDecimal > 0 Then
                    qry += " and NUMERIC_SCALE='" + clsCommon.myCstr(ScaleForDecimal) + "'"
                End If
            Case DBDataType.varbinary_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='varbinary'"
            Case DBDataType.text_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='text' "
            Case DBDataType.datetime_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='datetime' "
            Case DBDataType.time_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='time' "
            Case DBDataType.varchar_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='varchar' "
                If MaxLength > 0 Then
                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
                End If
            Case DBDataType.numeric_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='numeric'"
                If MaxLength > 0 Then
                    qry += " and NUMERIC_PRECISION='" + clsCommon.myCstr(MaxLength) + "'"
                End If
                If ScaleForDecimal > 0 Then
                    qry += " and NUMERIC_SCALE='" + clsCommon.myCstr(ScaleForDecimal) + "'"
                End If
            Case DBDataType.nchar_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='nchar'"
            Case DBDataType.float_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='float'"
            Case DBDataType.date_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='date'"
            Case DBDataType.char_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='char'"
                If MaxLength > 0 Then
                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
                End If
            Case DBDataType.bigint_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='bigint'"
            Case DBDataType.bit_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='bit'"
            Case DBDataType.nvarchar_Type
                qry = "select count(*) from information_schema.columns where table_name='" + table_name + "' and column_name='" + column_name + "' and data_type='nvarchar'"
                If MaxLength > 0 Then
                    qry += " and CHARACTER_MAXIMUM_LENGTH='" + clsCommon.myCstr(MaxLength) + "'"
                End If
            Case Else
                qry = "select count(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table_name + "' and COLUMN_NAME='" + column_name + "'"
        End Select

        Dim check As Integer = clsDBFuncationality.getSingleValue(qry, trans)

        Return check
    End Function

    Public Shared Function CheckTriggerExits(ByVal trg_name As String, ByVal trans As SqlTransaction) As Integer
        Dim check As Integer = 0
        Try
            Dim sQuery = "SELECT count(*) FROM sys.triggers where name='" & trg_name & "'"
            check = clsDBFuncationality.getSingleValue(sQuery, trans)
        Catch ex As Exception
            Throw New Exception(ex.ToString)
        End Try
        Return check
    End Function
    Public Shared Function CheckTypeExits(ByVal type_name As String, ByVal trans As SqlTransaction) As Integer
        Dim check As Integer = 0
        Try
            Dim sQuery = "SELECT count(*) FROM sys.types where name='" & type_name & "'"
            check = clsDBFuncationality.getSingleValue(sQuery, trans)
        Catch ex As Exception
            Throw New Exception(ex.ToString)
        End Try
        Return check
    End Function
    ''richa agawral
    Public Shared Function CheckIndexExists(ByVal index_name As String, ByVal trans As SqlTransaction) As Integer
        Dim check As Integer = 0
        Try
            Dim sQuery = "SELECT count(*) FROM sys.indexes  where type_desc ='NONCLUSTERED' and  name='" & index_name & "'"
            check = clsDBFuncationality.getSingleValue(sQuery, trans)
        Catch ex As Exception
            Throw New Exception(ex.ToString)
        End Try
        Return check
    End Function
    ''------------------
End Class

