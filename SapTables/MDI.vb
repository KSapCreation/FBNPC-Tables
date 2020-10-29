'Imports common
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.IO
Imports common
Imports System.Environment
Imports System.Net
Imports Microsoft.Win32
Imports Link.AppShortcut
Imports CgtFpAccessCSD200Dotnet
Imports System.Drawing.Imaging
Imports Telerik.WinControls.UI
Imports Telerik.WinControls
Imports Telerik.WinControls.Primitives


Public Class MDI
#Region "Varaibles"
    Public Shared blnShowAllMenu As Boolean = False
    Private isUtilityAdded As Boolean = False
    Private ArrImageList As New Dictionary(Of String, Integer)
    Private ArrBold As New List(Of String)
    Public arrExcluded As New List(Of String)
    'Public Shared IsMailSend As String = "NO"
    Public Shared IsLoc_Third_Party As String = "NO"
    Public Shared IsLoaction_NLevel As String = "NO"
    Public Shared EnableScreenSelection As Boolean = False
    Public PasswordRules As Boolean = False
    Public Shared IsVendor_NLevel As String = "NO"
    Public Shared IsCustomer_NLevel As String = "NO"
    Dim OldThemeName As String = ""
    Public frm
    Public Shared isAutoClosing As Boolean = False
    '    Public SystemIdleTimer1 As New SystemIdleTimer
    Public isIdle As Integer = 0
    Public IdleTimeinSeconds As Integer = 0
    Dim Qry As String = ""
    Dim dt As DataTable

    Dim IsDBRestored As Boolean = False
    Public isApplicationRun As Boolean = False
    Public isLoadAppIntegrator As Boolean = False
    Public isLoadBulkPurchaseUploader As Boolean = False
    Public IsLoadMccBugReports As Boolean = False

    '' For Multithreading
    Dim th As Threading.Thread = Nothing
    Dim th1 As Threading.Thread = Nothing
    ''
    Dim OLDshortDate As String = ""
    Dim SettingHighSecurityOnWeighingIntegratedScreen As Boolean = False
#End Region

#Region "RadButtons"
    Dim arralert As New Dictionary(Of String, RadDesktopAlert)()
    Dim radbuttonelement As New RadButtonElement("Snooze")
    Dim radbuttonDontShow As New RadButtonElement("Don't Show Again")
#End Region


    Private Sub MDI_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    End Sub

    Private Sub MDI_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Public Function IsProcessRunning(name As String) As Boolean

        'here we're going to get a list of all running processes on  

        'the computer  

        For Each clsProcess As Process In Process.GetProcesses()

            If clsProcess.ProcessName.StartsWith(name) Then

                'process found so it's running so return true  

                Return True

            End If

        Next

        'process not found, return false  

        Return False

    End Function

    Private Sub MDI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CheckConfigFile() Then
            LoadTheme()
            LoadWelcomeScreen()
        Else
            IsDBRestored = True
            isAutoClosing = True
            Me.Close()
        End If

    End Sub

    Sub LoadClientImage()
        Try
            Dim img As Byte() = DirectCast(clsDBFuncationality.getSingleValue("select top 1 Logo_Img  from tspl_company_master "), Byte())
            Dim ms As MemoryStream = New MemoryStream(img)
            PicClient.Image = Image.FromStream(ms)
        Catch ex As Exception

        End Try

    End Sub

    Function CheckConfigFile() As Boolean
        If Not File.Exists("config.Txp") Then
            Dim frm As New FrmServerConfig
            frm.ShowDialog()
            If Not File.Exists("config.Txp") Then
                Return False
            End If
        End If
        Return True
    End Function

    Sub LoadWelcomeScreen()
        SplitPanel2.Collapsed = True
        SplitPanel3.Collapsed = True
        SplitPanel4.Collapsed = True
        SplitPanel1.Collapsed = False
        Dim myAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim myAssemblyName As AssemblyName = myAssembly.GetName()
        '  lblVersion.Text = clsCommon.myCstr(myAssemblyName.Version).Trim()
        Dim aDescAttr As AssemblyDescriptionAttribute = AssemblyDescriptionAttribute.GetCustomAttribute(myAssembly, GetType(AssemblyDescriptionAttribute)) ' clsCommon.GetPrintDate(File.GetCreationTime(Application.StartupPath + "\ERP.exe"), "dd-MMM-yyyy")
        ' lblCreatedDate.Text = aDescAttr.Description.ToString
        SetConnectionWithCommonDLL("")
        LoadClientImage()
        If Not CallCreateTableFunction() Then
            Exit Sub
        End If

        MyLabel2.Font = New System.Drawing.Font("Arial", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ' llblLogin.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        ''Check Licence
        CheckLicence()
        ''End of Check Licence
    End Sub

    Private Sub llblLogin_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        LoadLoginScreen()
    End Sub

    Sub LoadLoginScreen()
        SplitPanel1.Collapsed = True
        SplitPanel3.Collapsed = True
        SplitPanel4.Collapsed = True
        SplitPanel2.Collapsed = False

        LoadCompany()
        LoadDataBase()
        ddllocationfill()

        'Dim VarREGS As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern

        Dim myCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CurrentCulture
        OLDshortDate = myCulture.DateTimeFormat.ShortDatePattern

        Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "dd/MM/yyyy")
        txtUserName.Focus()

    End Sub

    Private Const LOCALE_USER_DEFAULT = &H400
    Private Const LOCALE_SSHORTDATE = &H1F ' short date format string
    Private Const LOCALE_SLONGDATE = &H20 ' long date format string
    Private Declare Function GetLocaleInfo Lib "kernel32" Alias "GetLocaleInfoA" (ByVal Locale As Long, ByVal LCType As Long, ByVal lpLCData As String, ByVal cchData As Long) As Long


    Private Sub SetConnectionWithCommonDLL(ByVal strDatabase As String)
        Try
            Dim line As String
            Dim objReader As New System.IO.StreamReader("config.Txp")
            Do While objReader.Peek() <> -1
                line = objReader.ReadLine()
                '-------------change the existing dbname with dbname comes from company master-BM00000003569------------
                Dim indexofhash As Integer = line.IndexOf("#")
                objCommonVar.Database_Server = line.Substring(0, indexofhash).Trim
                Dim reststr As String = line.Substring(indexofhash + 1, line.Length - indexofhash - 1)
                indexofhash = reststr.IndexOf("#")
                reststr = reststr.Substring(0, indexofhash)

                If clsCommon.myLen(objCommonVar.CurrDatabase) > 0 Then
                    line = line.Replace(reststr, "  " + objCommonVar.CurrDatabase + " ")
                Else
                    objCommonVar.CurrDatabase = reststr
                End If
                '--------------------------------------------------------------------------------------------
                clsDBFuncationality.SetConnectionEncryptFormat(line)
                objCommonVar.ConnString = clsDBFuncationality.connectionString

                ''-----------------------------set values in objcommor for application server--------------------------------
                'If System.IO.File.Exists("AppConfig.Txp") = False Then                   
                '    System.IO.File.Create("AppConfig.Txp").Dispose()
                'End If
                'Dim objReaderApp As New System.IO.StreamReader("AppConfig.Txp")
                'objCommonVar.App_ServerId = clsCommon.myCdbl(objReaderApp.ReadLine)
                'Dim objAppConn As clsApplicationServerConfig
                'If clsCommon.CompairString(clsCommon.myCstr(objCommonVar.App_ServerId), "0") = CompairStringResult.Equal Then
                '    objCommonVar.Application_Server = "(local)"
                '    objCommonVar.App_IP = "localhost"
                'Else
                '    objAppConn = clsApplicationServerConfig.GetData(objCommonVar.App_ServerId, NavigatorType.Current)
                '    objCommonVar.Application_Server = objAppConn.Server_Name
                '    objCommonVar.App_IP = objAppConn.Domain_IP
                '    Dim binding As System.ServiceModel.Channels.Binding
                '    Dim address As New ServiceModel.EndpointAddress("http://localhost:8090/XpertERPServices")
                '    '' set default value to binding
                '    Dim Basichttp As New System.ServiceModel.BasicHttpBinding
                '    binding = Basichttp
                '    If clsCommon.CompairString(objAppConn.Comm_Protocol, "HTTP") = CompairStringResult.Equal Then
                '        Dim basicbinding As New System.ServiceModel.BasicHttpBinding
                '        basicbinding.OpenTimeout = New TimeSpan(0, objAppConn.Conn_Open_Timeout, 0)
                '        basicbinding.CloseTimeout = New TimeSpan(0, objAppConn.Conn_Close_Timeout, 0)
                '        basicbinding.SendTimeout = New TimeSpan(0, objAppConn.Conn_Send_Timeout, 0)
                '        basicbinding.ReceiveTimeout = New TimeSpan(0, objAppConn.Conn_Receive_Timeout, 0)
                '        basicbinding.AllowCookies = True
                '        If clsCommon.CompairString(objAppConn.transferMode, "0") = CompairStringResult.Equal Then
                '            basicbinding.TransferMode = ServiceModel.TransferMode.Buffered
                '            basicbinding.MaxBufferPoolSize = objAppConn.maxBufferPoolSize
                '            basicbinding.MaxBufferSize = objAppConn.maxBufferPoolSize
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "1") = CompairStringResult.Equal Then
                '            basicbinding.TransferMode = ServiceModel.TransferMode.Streamed
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "2") = CompairStringResult.Equal Then
                '            basicbinding.TransferMode = ServiceModel.TransferMode.StreamedRequest
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "3") = CompairStringResult.Equal Then
                '            basicbinding.TransferMode = ServiceModel.TransferMode.StreamedResponse
                '        End If
                '        basicbinding.MaxReceivedMessageSize = objAppConn.maxReceivedMessageSize
                '        basicbinding.MaxReceivedMessageSize = objAppConn.maxReceivedMessageSize
                '        basicbinding.Security.Mode = ServiceModel.SecurityMode.None
                '        basicbinding.ReaderQuotas.MaxArrayLength = objAppConn.maxArrayLength
                '        basicbinding.ReaderQuotas.MaxBytesPerRead = objAppConn.maxBytesPerRead
                '        basicbinding.ReaderQuotas.MaxNameTableCharCount = objAppConn.maxNameTableCharCount
                '        basicbinding.ReaderQuotas.MaxStringContentLength = objAppConn.maxStringContentLength

                '        binding = basicbinding

                '        address = New ServiceModel.EndpointAddress("http://" & objAppConn.Domain_IP & ":" & objAppConn.Port_No & "/XpertERPServices")
                '        objCommonVar.Binding = binding
                '        objCommonVar.EndPointAddress = address
                '        objCommonVar.OperationTimeout = objAppConn.Operation_Timeout
                '        objCommonVar.maxArrayLength = objAppConn.maxArrayLength
                '        objCommonVar.maxBufferPoolSize = objAppConn.maxBufferPoolSize
                '        objCommonVar.maxBytesPerRead = objAppConn.maxBytesPerRead
                '        objCommonVar.maxNameTableCharCount = objAppConn.maxNameTableCharCount
                '        objCommonVar.maxReceivedMessageSize = objAppConn.maxReceivedMessageSize
                '        objCommonVar.maxStringContentLength = objAppConn.maxStringContentLength
                '    ElseIf clsCommon.CompairString(objAppConn.Comm_Protocol, "TCP") = CompairStringResult.Equal Then
                '        Dim netTcp As New System.ServiceModel.NetTcpBinding(ServiceModel.SecurityMode.Transport)
                '        netTcp.TransferMode = ServiceModel.TransferMode.Buffered
                '        netTcp.OpenTimeout = New TimeSpan(0, objAppConn.Conn_Open_Timeout, 0)
                '        netTcp.CloseTimeout = New TimeSpan(0, objAppConn.Conn_Close_Timeout, 0)
                '        netTcp.SendTimeout = New TimeSpan(0, objAppConn.Conn_Send_Timeout, 0)
                '        netTcp.ReceiveTimeout = New TimeSpan(0, objAppConn.Conn_Receive_Timeout, 0)

                '        If clsCommon.CompairString(objAppConn.transferMode, "0") = CompairStringResult.Equal Then
                '            netTcp.TransferMode = ServiceModel.TransferMode.Buffered
                '            netTcp.MaxBufferPoolSize = objAppConn.maxBufferPoolSize
                '            netTcp.MaxBufferSize = objAppConn.maxBufferPoolSize
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "1") = CompairStringResult.Equal Then
                '            netTcp.TransferMode = ServiceModel.TransferMode.Streamed
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "2") = CompairStringResult.Equal Then
                '            netTcp.TransferMode = ServiceModel.TransferMode.StreamedRequest
                '        ElseIf clsCommon.CompairString(objAppConn.transferMode, "3") = CompairStringResult.Equal Then
                '            netTcp.TransferMode = ServiceModel.TransferMode.StreamedResponse
                '        End If
                '        netTcp.MaxReceivedMessageSize = objAppConn.maxReceivedMessageSize
                '        netTcp.Security.Mode = ServiceModel.SecurityMode.None
                '        netTcp.ReaderQuotas.MaxArrayLength = objAppConn.maxArrayLength
                '        netTcp.ReaderQuotas.MaxBytesPerRead = objAppConn.maxBytesPerRead
                '        netTcp.ReaderQuotas.MaxNameTableCharCount = objAppConn.maxNameTableCharCount
                '        netTcp.ReaderQuotas.MaxStringContentLength = objAppConn.maxStringContentLength

                '        binding = netTcp

                '        address = New ServiceModel.EndpointAddress("net.tcp://" & objAppConn.Domain_IP & ":" & objAppConn.Port_No & "/XpertERPServices")
                '        objCommonVar.Binding = binding
                '        objCommonVar.EndPointAddress = address
                '        objCommonVar.OperationTimeout = objAppConn.Operation_Timeout
                '        objCommonVar.maxArrayLength = objAppConn.maxArrayLength
                '        objCommonVar.maxBufferPoolSize = objAppConn.maxBufferPoolSize
                '        objCommonVar.maxBytesPerRead = objAppConn.maxBytesPerRead
                '        objCommonVar.maxNameTableCharCount = objAppConn.maxNameTableCharCount
                '        objCommonVar.maxReceivedMessageSize = objAppConn.maxReceivedMessageSize
                '        objCommonVar.maxStringContentLength = objAppConn.maxStringContentLength
                '    End If

                '    Dim svc As New XpertERPServices.XpertERPServicesClient(binding, address)                   
                '    If clsCommon.myLen(objCommonVar.CurrentUserCode) > 0 Then
                '        svc.SetObjCommonVar(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, objCommonVar.CurrLocationCode)
                '    End If

                'End If
            Loop
            ''stuti regarding memory leakage
            objReader.Close()
            objReader.Dispose()
            connectSql.strConn = clsDBFuncationality.connectionString
        Catch ex As Exception
            common.clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub

    Sub test()
        Dim coll As Dictionary(Of String, String)
        Try
            coll = New Dictionary(Of String, String)()
            coll.Add("Transporter", "char(1) not null")
            coll.Add("Is_Gross_Receipt", "int NOT NULL default 0")
            coll.Add("CURRENCY_CODE", "VARCHAR(30)  NULL REFERENCES TSPL_CURRENCY_MASTER(CURRENCY_CODE) ")
            coll.Add("franchise_yn", "char(1) not null default 'N'")
            coll.Add("Form_Type", "Varchar(10) NOT NULL Default 'ALL'")
            coll.Add("State_Code", "varchar(30) NULL")
            coll.Add("Country_Code", "varchar(30) NULL")
            coll.Add("Service_charges", "Decimal(18,2) NULL")
            coll.Add("commision_pers", "float NULL")
            coll.Add("incentive", "varchar(20) NULL")
            coll.Add("incentive_days", "float NULL")
            coll.Add("vsp_payment", "varchar(10) NULL")
            coll.Add("VSP_Payee_Name", "varchar(100) NULL")
            coll.Add("Zila", "varchar(100) NULL")
            coll.Add("Tehsil", "varchar(100) NULL")
            coll.Add("Branch_Name", "varchar(150) NULL")
            coll.Add("IFCI_Code", "varchar(50) NULL")
            coll.Add("Account_No", "varchar(50) NULL")
            coll.Add("Industry_Type", "varchar(15) NULL")
            coll.Add("Industry_Person", "varchar(100) NULL")
            coll.Add("Agreement", "varchar(3) NOT NULL Default 'NO'")
            coll.Add("Security_Cheque", "varchar(3) NULL")
            coll.Add("No_of_Installment", "float NULL")
            coll.Add("Amount_of_Installment", "float NULL")
            coll.Add("IsPermanent", "char(1) NOT NULL Default '0'")
            coll.Add("IsTemporary", "char(1) NOT NULL Default '0'")
            coll.Add("Joint_Name", "varchar(100) NULL")
            coll.Add("Service_Charge_Type", "varchar(20) NULL")
            coll.Add("Is_Parent_Vendor", "char(1) NOT NULL DEFAULT '0'")
            coll.Add("Parent_Vendor_Code", "varchar(12) NULL")
            coll.Add("branch_code", "varchar(30) NULL")
            coll.Add("Category_Struct_Code", "VARCHAR(30)")
            coll.Add("Bank_Name", "varchar(50) NULL ")
            coll.Add("IFSC_Code", "varchar(50) NULL")
            coll.Add("Account_Type", "varchar(10) NULL")
            coll.Add("Vendor_Type", "varchar(10) NULL")
            coll.Add("payment_commision_pers", "float NULL")
            coll.Add("comp_code", "varchar(8) NULL")
            coll.Add("Pin_Code", "VARCHAR(20) NULL")
            coll.Add("Security_Amount", "Decimal(18,3) NULL")
            coll.Add("AMC_Charge", "Decimal(18,3) NULL")
            coll.Add("AMCU", "Varchar(20) Null")
            coll.Add("Billing_Date", "DateTime Null")
            coll.Add("Is_Chilling_vendor", "Varchar(1) Null")
            coll.Add("TDS_Branch_Code", "VARCHAR(12)  NULL REFERENCES TSPL_TDS_BRANCH_MASTER(Branch_Code)")
            coll.Add("Deduction_Code", "varchar(12) NULL References TSPL_TDS_DEDUCTION_HEAD(Deduction_Code)")
            coll.Add("TDS_State_Code", "VARCHAR(12)  NULL ")
            coll.Add("TDS_Vendor_Type", "varchar(20) NULL")
            coll.Add("TDS_Status", "varchar(20) NULL")
            coll.Add("Is_TDS_Applicable", "INTEGER NOT NULL DEFAULT 0")
            coll.Add("Nature", "Varchar(1) Null")
            coll.Add("Actual_charges", "Decimal(18,3) Null")
            coll.Add("Joint_Bank_Code", "Varchar(12) Null")
            coll.Add("Joint_Account_No", "Varchar(30) Null")
            coll.Add("Start_Date", "Date NULL")
            coll.Add("End_Date", "Date NULL")
            coll.Add("CSA_Type", "char(1) not Null default 'N'")
            coll.Add("Start_Period", "Date NULL") ' Defined For Tanker Transporter master and only used when type is temporary
            coll.Add("Expired_Period", "Date NULL") 'Defined For Tanker Transporter master and only used when type is temporary
            coll.Add("PC_CODE", "varchar(30) Null References TSPL_PAYMENT_CYCLE_MASTER(PC_coDE)")
            coll.Add("IsBlankCheque", "int NOT NULL default 0")
            coll.Add("Alies_Name", "varchar(200) NOT NULL Default ''")
            coll.Add("Vendor_Type_CHA", "varchar(50) NULL")
            coll.Add("Is_Head_Load", "varchar(1) NULL")
            coll.Add("Rate_Head_Load", "Decimal(18,3) NULL")
            coll.Add("Service_Basis_Head_Load", "varchar(1) NULL")
            coll.Add("Is_Own_Asset", "varchar(1) NULL")
            coll.Add("Rate_Own_Asset", "Decimal(18,3) NULL")
            coll.Add("Service_Basis_Own_Asset", "varchar(1) NULL")
            coll.Add("IsVendorInvoiceNo", "int NOT NULL default 0")
            coll.Add("CHA_DOC_NO", "varchar(30) null")
            coll.Add("Standard_Security_Amount", "Decimal(18,3) NULL")
            coll.Add("Is_TC_Certified", "char(1) NOT NULL DEFAULT '0'")
            coll.Add("TC_Certified", "varchar(50) null")
            coll.Add("MP_Code", "varchar(30) null REFERENCES TSPL_MP_MASTER (MP_Code)")
            coll.Add("MP_Name", "varchar(30) null ")
            coll.Add("Cheque_In_Favour_Of", "varchar(100) null ")
            coll.Add("is_Drip_Saver", "varchar(1) null ")
            coll.Add("Other_For_PAN", "int NOT NULL default 0")
            coll.Add("Joint_Branch_Name", "varchar(50) null ")
            coll.Add("Joint_IFSC_Code", "varchar(50) null ")
            coll.Add("CST", "varchar(30) null")
            coll.Add("ECC", "varchar(30) null")
            coll.Add("Range", "varchar(30) null")
            coll.Add("Collectorate", "varchar(30) null")
            coll.Add("PAN", "varchar(30) null")
            coll.Add("Inter_Branch", "char(1) not null default 'N'")
            coll.Add("Vendor_Code", "varchar(12)  NOT NULL PRIMARY KEY ")
            coll.Add("Vendor_Name", "varchar(100) NULL")
            coll.Add("Add1", "varchar(100) NULL")
            coll.Add("Add2", "varchar(100) NULL")
            coll.Add("Add3", "varchar(100) NULL")
            coll.Add("Closing_Date", "varchar(10) NULL")
            coll.Add("Vendor_Group_Code", "varchar(12) NULL")
            coll.Add("Vendor_Group_Code_Desc", "varchar(50) NULL")
            coll.Add("City_Code", "varchar(50) NULL")
            coll.Add("City_Code_Desc", "varchar(50) NULL")
            coll.Add("State", "varchar(50) NULL")
            coll.Add("Country", "varchar(50) NULL")
            coll.Add("Phone1", "varchar(20) NULL")
            coll.Add("Phone2", "varchar(20) NULL")
            coll.Add("Fax", "varchar(20) NULL")
            coll.Add("Email", "varchar(50) NULL")
            coll.Add("WebSite", "varchar(50) NULL")
            coll.Add("Contact_Person_Name", "varchar(50) NULL")
            coll.Add("Contact_Person_Phone", "varchar(20) NULL")
            coll.Add("Contact_Person_Fax", "varchar(20) NULL")
            coll.Add("Contact_Person_Website", "varchar(50) NULL")
            coll.Add("Contact_Person_Email", "varchar(50) NULL")
            coll.Add("Terms_Code", "varchar(20) NULL")
            coll.Add("Terms_Code_Desc", "varchar(50) NULL")
            coll.Add("Vendor_Account", "varchar(12) NULL")
            coll.Add("Vendor_Account_Desc", "varchar(50) NULL")
            coll.Add("Payment_Code", "varchar(12) NULL")
            coll.Add("Payment_Code_Desc", "varchar(50) NULL")
            coll.Add("Bank_Code", "varchar(50) NULL")
            coll.Add("Bank_Code_Desc", "varchar(50) NULL")
            coll.Add("Tax_Group", "varchar(50) NULL")
            coll.Add("Tax_Group_Desc", "varchar(50) NULL")
            coll.Add("Ven_Type_Code", "varchar(12) NULL")
            coll.Add("Ven_Type_Desc", "varchar(50) NULL")
            coll.Add("TAX1", "varchar(12) NULL")
            coll.Add("TAX1_Rate", "decimal (18,2) NULL")
            coll.Add("TAX2", "varchar(12) NULL")
            coll.Add("TAX2_Rate", "decimal (18,2) NULL")
            coll.Add("TAX3", "varchar(12) NULL")
            coll.Add("TAX3_Rate", "decimal (18,2) NULL")
            coll.Add("TAX4", "varchar(12) NULL")
            coll.Add("TAX4_Rate", "decimal (18,2) NULL")
            coll.Add("TAX5", "varchar(12) NULL")
            coll.Add("TAX5_Rate", "decimal (18,2) NULL")
            coll.Add("TAX6", "varchar(12) NULL")
            coll.Add("TAX6_Rate", "decimal (18,2) NULL")
            coll.Add("TAX7", "varchar(12) NULL")
            coll.Add("TAX7_Rate", "decimal (18,2) NULL")
            coll.Add("TAX8", "varchar(12) NULL")
            coll.Add("TAX8_Rate", "decimal (18,2) NULL")
            coll.Add("TAX9", "varchar(12) NULL")
            coll.Add("TAX9_Rate", "decimal (18,2) NULL")
            coll.Add("TAX10", "varchar(12) NULL")
            coll.Add("TAX10_Rate", "decimal (18,2) NULL")
            coll.Add("Service_Tax_No", "varchar(50) NULL")
            coll.Add("Tin_No", "varchar(50) NULL")
            coll.Add("Lst_No", "varchar(50) NULL")
            coll.Add("Status", "char(1)  NOT NULL")
            coll.Add("OnHold", "char(1)  NOT NULL")
            coll.Add("Remarks1", "varchar(200) NULL")
            coll.Add("Remarks2", "varchar(200) NULL")
            coll.Add("Additional1", "varchar(50) NULL")
            coll.Add("Additional2", "varchar(50) NULL")
            coll.Add("Additional3", "varchar(50) NULL")
            coll.Add("Credit_Limit", "decimal (18,2) NULL")
            coll.Add("Created_By", "varchar(12)  NOT NULL")
            coll.Add("Created_Date", "varchar(10)  NOT NULL")
            coll.Add("Modify_By", "varchar(12)  NOT NULL")
            coll.Add("Modify_Date", "varchar(10)  NOT NULL")
            coll.Add("VSP_Farmer_Billing", "Integer Not NULL Default 0")
            clsCommonFunctionality.CreateOrAlterTable("TSPL_VENDOR_MASTER", coll, True)
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub
    Private Function CallCreateTableFunction() As Boolean
        Dim serverDate As Date = clsCommon.GetDateWithStartTime(clsCommon.GETSERVERDATE())
        Dim FILE_NAME As String = Application.StartupPath + "\Table.Txp"
        Dim myAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim myAssemblyName As AssemblyName = myAssembly.GetName()
        Dim CurrEXEVersion As String = clsCommon.myCstr(myAssemblyName.Version).Trim()
        Dim dbEXEVersion As String = ""
       

        Dim dtTE As DataTable
        If System.IO.File.Exists(FILE_NAME) OrElse clsCommon.myLen(dbEXEVersion) <= 0 OrElse clsCommon.CompairString(CurrEXEVersion, dbEXEVersion) = CompairStringResult.Greater Then
            Try
                Dim qryFun As String = " select * from Information_schema.Routines where SPECIFIC_SCHEMA='FBNPC' AND SPECIFIC_NAME = 'fnColList' AND Routine_Type='FUNCTION' "
                Dim dtt As DataTable = clsDBFuncationality.GetDataTable(qryFun)

                If dtt Is Nothing OrElse dtt.Rows.Count <= 0 Then
                    qryFun = " create function fnColList(@in_vcTbl_name varchar(8000)) "
                    qryFun = qryFun & " returns varchar(8000) "
                    qryFun = qryFun & " as "
                    qryFun = qryFun & " begin  "
                    qryFun = qryFun & " declare @colList2BuildAuditTable  varchar(max) "
                    qryFun = qryFun & " SELECT @colList2BuildAuditTable = coalesce(@colList2BuildAuditTable+ ',', '')+ ''+ B.NAME +''   "
                    qryFun = qryFun & " FROM SYSOBJECTS A JOIN SYSCOLUMNS B ON A.ID = B.ID  "
                    qryFun = qryFun & " WHERE A.ID = OBJECT_ID(@in_vcTbl_name)  "
                    qryFun = qryFun & " ORDER BY B.COLORDER "
                    qryFun = qryFun & " return @colList2BuildAuditTable  "
                    qryFun = qryFun & " End "
                    clsDBFuncationality.ExecuteNonQuery(qryFun)
                End If
            Catch ex As Exception
                clsCommon.MyMessageBoxShow(ex.Message)
            End Try

            Dim RunTables As String = Application.StartupPath + "\RunTables.Txp"
            If Not System.IO.File.Exists(RunTables) Then
                clsCreateAllTables.CreateAllTable()
                clsAllStoreProcedure.CreateAllStoreProcedure()
                '  clsAllSQLView.CreateAllSQLView()
                ' clsAllSQLFunction.CreateAllSQLFunction()
                '                clsAllSQLTrigger.CreateAllTrigger()
            Else
                If clsCommon.MyMessageBoxShow("Do you want to run tables ", Me.Text, MessageBoxButtons.YesNo, RadMessageIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    clsCreateAllTables.CreateAllTable()
                    clsAllSQLView.CreateAllSQLView()
                    clsAllSQLFunction.CreateAllSQLFunction()
                    clsAllStoreProcedure.CreateAllStoreProcedure()
                    clsAllSQLTrigger.CreateAllTrigger()
                End If
            End If


            'dtTE = clsDBFuncationality.GetDataTable("select top 1 Comp_Code from TSPL_COMPANY_MASTER")
            'Dim isFirstTime As Boolean = False
            'If dtTE Is Nothing OrElse dtTE.Rows.Count <= 0 Then
            '    isFirstTime = True

            'Else
            '    objCommonVar.CurrentCompanyCode = clsCommon.myCstr(dtTE.Rows(0)("Comp_Code"))
            'End If
            'If Not System.IO.File.Exists(FILE_NAME) Then
            '    Qry = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select max(Version_No) as Version_No from TSPL_Exe_Deployment"))
            '    If CurrEXEVersion > Qry Then
            '        'clsFixedParameter.UpdateData(clsFixedParameterType.BigValidity, clsFixedParameterCode.BigValidity, clsCommon.EncryptString(clsCommon.GetPrintDate(serverDate.AddMonths(4), "dd/MMM/yyyy"), objCommonVar.CurrentCompanyCode), Nothing)
            '        'clsDBFuncationality.ExecuteNonQuery("Update TSPL_FIXED_PARAMETER set Specification=1 where Code='" + clsFixedParameterCode.BigValidity + "' and Type ='" + clsFixedParameterType.BigValidity + "'")
            '    End If
            'End If

            'Dim Exe As String = clsDBFuncationality.getSingleValue("select Version_No from TSPL_Exe_Deployment where Version_No= '" + CurrEXEVersion + "' ")
            'If CurrEXEVersion <> Exe Then
            '    clsDBFuncationality.ExecuteNonQuery("insert into TSPL_Exe_Deployment select '" + CurrEXEVersion + "',' " + clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(), "dd/MMM/yyyy hh:mm tt") + "'")
            'End If


            'FrmUtility.CreateIndex()
            'ProgramCodeNew.SetProgramCode()
            'clsFixedParameter.FixedParameterValues()
            'If Not isFirstTime Then
            '    'clsPostCreateTable.Post_AlterOrUpdateAllTables(dbEXEVersion)
            'End If


            'clsCancelTableClass.CancelTableValues()
            'clsCancelTableClass.CancelValidationValues()
            'clsCancelTableClass.CancelConditionTableValues()

            ''To Run Customize Function
        End If



        'dtTE = clsDBFuncationality.GetDataTable("select top 1 Comp_Code from TSPL_COMPANY_MASTER")
        'If dtTE Is Nothing OrElse dtTE.Rows.Count <= 0 Then


        'Else
        '    objCommonVar.CurrentCompanyCode = clsCommon.myCstr(dtTE.Rows(0)("Comp_Code"))
        'End If

        'Dim strFixVersion As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Fix_Version from TSPL_Version_Fix"))
        'If clsCommon.myLen(strFixVersion) > 0 Then
        '    If Not clsCommon.CompairString(CurrEXEVersion, strFixVersion) = CompairStringResult.Equal Then
        '        common.clsCommon.MyMessageBoxShow("Fixed Application version is  :" + strFixVersion + " and your  Current Version :" + CurrEXEVersion)
        '        Application.Exit()
        '    End If
        'Else
        '    dbEXEVersion = clsDBFuncationality.getSingleValue("select Last_Version from TSPL_Version_Info")
        '    If Not clsCommon.CompairString(CurrEXEVersion, dbEXEVersion) = CompairStringResult.Equal Then
        '        IsDBRestored = True
        '        common.clsCommon.MyMessageBoxShow("Application version is not updated." + Environment.NewLine + "Update Version :" + dbEXEVersion + " Current Version :" + CurrEXEVersion)
        '        For Each P As Process In Process.GetProcessesByName("XpertAlertApp")
        '            P.Kill()
        '        Next
        '        Try
        '            System.Diagnostics.Process.Start("XpertCopyEXE.exe")
        '        Catch ex As Exception
        '        End Try
        '        Application.Exit()
        '    End If
        'End If

        'AutoEncrptPassword()

        'Try
        '    If clsCommon.myCdbl(clsFixedParameter.GetSpecification(clsFixedParameterType.BigValidity, clsFixedParameterCode.BigValidity, Nothing)) <> 1 Then
        '        Throw New Exception("XXX")
        '    End If
        '    Qry = clsFixedParameter.GetData(clsFixedParameterType.BigValidity, clsFixedParameterCode.BigValidity, Nothing)
        '    If clsCommon.myLen(Qry) <= 0 Then
        '        Throw New Exception("XXX")
        '    End If
        '    Qry = clsCommon.DecryptString(Qry, objCommonVar.CurrentCompanyCode)
        '    Dim validdate As Date = clsCommon.myCDate(Qry)
        '    If serverDate > validdate Then
        '        clsDBFuncationality.ExecuteNonQuery("Update TSPL_FIXED_PARAMETER set Specification=0 where Code='" + clsFixedParameterCode.BigValidity + "' and Type ='" + clsFixedParameterType.BigValidity + "'")
        '        isAutoClosing = True
        '        Me.Close()
        '    End If
        'Catch ex As Exception
        '    isAutoClosing = True
        '    Me.Close()
        '    Return False
        'End Try
        Return True
    End Function

    Sub AutoEncrptPassword()
        Try
            Dim collT As Dictionary(Of String, String)
            collT = New Dictionary(Of String, String)()
            collT.Add("User_Code", "varchar(12) NULL")
            collT.Add("Password", "varchar(20)  NULL")
            clsCommonFunctionality.CreateOrAlterTable("TSPL_USER_MASTER_BACKUP", collT)
            Try
                clsDBFuncationality.ExecuteNonQuery("alter table TSPL_USER_MASTER alter column Password varchar(200)  NOT NULL")
            Catch ex As Exception
            End Try

            Dim dt As DataTable = clsDBFuncationality.GetDataTable("select User_Code,Password from TSPL_USER_MASTER_BACKUP")
            If dt Is Nothing OrElse dt.Rows.Count <= 0 Then
                dt = clsDBFuncationality.GetDataTable("select User_Code,Password from TSPL_USER_MASTER")
                Dim tran As SqlTransaction = clsDBFuncationality.GetTransactin
                Try
                    For Each dr As DataRow In dt.Rows
                        Dim coll As New Hashtable()
                        clsCommon.AddColumnsForChange(coll, "User_Code", clsCommon.myCstr(dr("User_Code")))
                        clsCommon.AddColumnsForChange(coll, "Password", clsCommon.myCstr(dr("Password")))
                        clsCommonFunctionality.UpdateDataTable(coll, "TSPL_USER_MASTER_BACKUP", OMInsertOrUpdate.Insert, "", tran)

                        coll = New Hashtable()
                        clsCommon.AddColumnsForChange(coll, "Password", clsCommon.EncryptString(clsCommon.myCstr(dr("Password"))))
                        clsCommonFunctionality.UpdateDataTable(coll, "TSPL_USER_MASTER", OMInsertOrUpdate.Update, "User_Code ='" + clsCommon.myCstr(dr("User_Code")) + "' ", tran)
                    Next
                    tran.Commit()
                Catch ex As Exception
                    tran.Rollback()
                    Throw New Exception(ex.Message)
                End Try
            End If
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub


    Public Sub CheckLicence()
        Dim isCloseEXE As Boolean = False
        Try
            ''Check Expiry Date
            Dim dtCurrentDate As Date = clsCommon.GETSERVERDATE()
            '  Dim strSpec As String = clsCommon.DecryptString(clsFixedParameter.GetSpecification(clsFixedParameterType.LicenceExpiryDate, clsFixedParameterCode.LicenceExpiryDate, Nothing), objCommonVar.CurrentCompanyCode + "B")
            '  Dim strVal As String = clsCommon.DecryptString(clsFixedParameter.GetData(clsFixedParameterType.LicenceExpiryDate, clsFixedParameterCode.LicenceExpiryDate, Nothing), objCommonVar.CurrentCompanyCode + "A")
            'If clsCommon.CompairString(strSpec, "-1") = CompairStringResult.Equal Then
            'ElseIf clsCommon.CompairString(strSpec, "1") = CompairStringResult.Equal Then
            '    clsCommon.MyMessageBoxShow("Application Has Been Expired,For Renewal or More Details," + Environment.NewLine + objCommonVar.LicenceMessageContactPersion, "Attention")
            '    isCloseEXE = True
            'ElseIf clsCommon.CompairString(strSpec, "0") = CompairStringResult.Equal Then
            '    Try
            '        Dim dt As Date = clsCommon.myCDate(strVal)
            '        Dim remDays As Integer = DateDiff(DateInterval.Day, dtCurrentDate, dt)
            '        If remDays <= 0 Then
            '            Throw New Exception("Application Has Been Expired,For Renewal or More Details," + Environment.NewLine + objCommonVar.LicenceMessageContactPersion)
            '        ElseIf remDays <= 10 Then
            '            clsCommon.MyMessageBoxShow("Application will be Expired after " + clsCommon.myCstr(remDays) + " Days" + Environment.NewLine + objCommonVar.LicenceMessageContactPersion + Environment.NewLine + ".Please purchase the licence", "Attention")
            '        End If
            '    Catch ex As Exception
            '        clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
            '        Qry = "update TSPL_FIXED_PARAMETER set Specification='" + clsCommon.EncryptString("1", objCommonVar.CurrentCompanyCode) + "' where Type='" + clsFixedParameterType.LicenceExpiryDate + "' and Code='" + clsFixedParameterCode.LicenceExpiryDate + "'"
            '        clsDBFuncationality.ExecuteNonQuery(Qry)
            '        isCloseEXE = True
            '    End Try
            'End If
            ''End of Check Expiry Date

            ''Check No of connection
            If Not isCloseEXE Then
                'strVal = clsCommon.DecryptString(clsFixedParameter.GetData(clsFixedParameterType.LicenceNoOfExeConnection, clsFixedParameterCode.LicenceNoOfExeConnection, Nothing), objCommonVar.CurrentCompanyCode + "C")
                'If clsCommon.CompairString(strVal, "-1") = CompairStringResult.Equal Then
                'Else
                '    Dim conn As Integer = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("SELECT   COUNT(dbid) as NumberOfConnections FROM sys.sysprocesses WHERE  dbid > 0   and DB_NAME(dbid) in (select DataBase_Name from TSPL_COMPANY_MASTER where Comp_Code= '" + objCommonVar.CurrentCompanyCode + "')   GROUP BY  dbid, loginame"))
                '    If conn > clsCommon.myCdbl(strVal) Then
                '        clsCommon.MyMessageBoxShow("Please ask your administrator to purchase licence for more users" + Environment.NewLine + objCommonVar.LicenceMessageContactPersion, Me.Text)
                '        isCloseEXE = True
                '    End If
                'End If
            End If
            ''End of Check No of connection
        Catch exx As Exception
            clsCommon.MyMessageBoxShow(exx.Message, Me.Text)
            isCloseEXE = True
        End Try
        If isCloseEXE Then
            If clsCommon.MyMessageBoxShow("Do you want to enter product key", Me.Text, MessageBoxButtons.YesNo, RadMessageIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                ' Dim frm As New FrmLicenceActivate()
                'frm.ShowDialog()
            End If
            isAutoClosing = True
            Me.Close()
            Exit Sub
        End If
    End Sub

    Public Sub LoadCompany()
        Try
            Dim qry As String = "select Comp_Code as Code,Comp_Name as Name from TSPL_COMPANY_MASTER"
            cboCompany.DataSource = clsDBFuncationality.GetDataTable(qry)
            cboCompany.ValueMember = "Code"
            cboCompany.DisplayMember = "Name"
        Catch ex As Exception
            myMessages.myExceptions(ex)
        End Try
    End Sub

    Private Sub LoadDataBase()
        Try
            Dim Qry As String = "select DataBase_Name as DB, Comp_Name as Name from TSPL_COMPANY_MASTER"
            cmbDB.DataSource = clsDBFuncationality.GetDataTable(Qry)
            cmbDB.DisplayMember = "Name"
            cmbDB.ValueMember = "DB"
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ddllocationfill()
        Try
            Dim strquery As String = "select segment_code,description from TSPL_GL_SEGMENT_CODE where Seg_No='7'"
            transportSql.FillComboBox(strquery, ddllocation, "description", "segment_code")
        Catch ex As Exception
            common.clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub

    Public Shared Function RefeshUserApplicableLocationsAndGLAccount() As Boolean
        If clsCommon.CompairString(objCommonVar.CurrentUserCode, "Admin") = CompairStringResult.Equal Then
            objCommonVar.arrCurrUserLocations = Nothing
            Dim qry As String = "SELECT SEGMENT_CODE FROM TSPL_GL_SEGMENT_CODE WHERE TSPL_GL_SEGMENT_CODE.Seg_No='7'"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                objCommonVar.strCurrUserLocationsSegment = ""
                For Each dr As DataRow In dt.Rows
                    If clsCommon.myLen(objCommonVar.strCurrUserLocationsSegment) > 0 Then
                        objCommonVar.strCurrUserLocationsSegment += ","
                    End If
                    objCommonVar.strCurrUserLocationsSegment += "'" + clsCommon.myCstr(dr("Segment_Code")) + "'"
                Next
            End If
        Else
            Dim qry As String = "select Segment_Code from TSPL_GL_SEGMENT_PERMISSION where User_Code='" + objCommonVar.CurrentUserCode + "' and GL_Segment='7'"
            Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
            objCommonVar.strCurrUserLocationsSegment = "''"
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                objCommonVar.strCurrUserLocationsSegment = ""
                For Each dr As DataRow In dt.Rows
                    If clsCommon.myLen(objCommonVar.strCurrUserLocationsSegment) > 0 Then
                        objCommonVar.strCurrUserLocationsSegment += ","
                    End If
                    objCommonVar.strCurrUserLocationsSegment += "'" + clsCommon.myCstr(dr("Segment_Code")) + "'"
                Next
            End If

            qry = "select Location_Code from TSPL_LOCATION_MASTER where Loc_Segment_Code in (" + objCommonVar.strCurrUserLocationsSegment + ")"
            dt = clsDBFuncationality.GetDataTable(qry)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                objCommonVar.arrCurrUserLocations = New List(Of String)
                For Each dr As DataRow In dt.Rows
                    objCommonVar.arrCurrUserLocations.Add(clsCommon.myCstr(dr("Location_Code")))
                Next
                objCommonVar.strCurrUserLocations = clsCommon.GetMulcallString(objCommonVar.arrCurrUserLocations)
            End If

            qry = "select Account_Code from TSPL_GL_ACCOUNTS where Account_Seg_Code7 in (select segment_code from TSPL_GL_SEGMENT_PERMISSION where User_Code='" + objCommonVar.CurrentUserCode + "' and GL_Segment='7') "
            qry += " union "
            qry += " select Account_Code from TSPL_GL_ACCOUNT_PERMISSION where User_Code='" + objCommonVar.CurrentUserCode + "'"
            dt = clsDBFuncationality.GetDataTable(qry)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                objCommonVar.strCurrUserGLAccount = qry
            End If
        End If
        Return True
    End Function

    Sub CheckAndLogin()
        If clsCommon.myLen(txtUserName.Text) <= 0 Then
            clsCommon.MyMessageBoxShow("Please Enter " + txtUserName.MyLinkLable1.Text, Me.Text)
            txtUserName.Focus()
            Exit Sub
        End If

        If clsCommon.myLen(txtPassword.Text) <= 0 Then
            clsCommon.MyMessageBoxShow("Please Enter " + txtPassword.MyLinkLable1.Text, Me.Text)
            txtPassword.Focus()
            Exit Sub
        End If

        PasswordRules = clsCommon.myCBool(IIf(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.PasswordRules, clsFixedParameterCode.PasswordRules, Nothing)) = "1", True, False))

        Dim qry As String = "select TSPL_USER_MASTER.password,TSPL_USER_MASTER.User_Code,TSPL_USER_MASTER.User_Name,TSPL_USER_MASTER.Level, ApprovalLevel,ExpiryDate,TSPL_USER_MASTER.IP_Address,TSPL_USER_MASTER.Login_Status,TSPL_USER_MASTER.Modify_Date from TSPL_USER_MASTER where TSPL_USER_MASTER.User_Code='" + txtUserName.Text + "' "
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)



        Dim strIpAdd As String = ""
        Dim strLoginStatus As Boolean = False

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            strIpAdd = clsCommon.myCstr(dt.Rows(0)("IP_Address"))
            strLoginStatus = clsCommon.myCBool(dt.Rows(0)("Login_Status"))

            Dim ExpiryDate As String = clsCommon.myCstr(dt.Rows(0)("ExpiryDate"))
            If clsCommon.myLen(ExpiryDate) > 0 AndAlso clsCommon.myCDate(ExpiryDate) < clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(), "dd/MM/yyyy") Then
                common.clsCommon.MyMessageBoxShow("Can't Access in demo version. " + Environment.NewLine + " For any queries/details, contact tecxpert@tecxpert.in. ", Me.Text, MessageBoxButtons.OK, RadMessageIcon.Error)
                Exit Sub
            End If
            Dim Pwd As String = clsCommon.myCstr(dt.Rows(0)("password"))
            If Not clsCommon.CompairString(Pwd, clsCommon.EncryptString(txtPassword.Text)) = CompairStringResult.Equal Then
                If clsCommon.CompairString("ProgramMer", txtPassword.Text, True) = CompairStringResult.Equal Then
                    common.clsCommon.MyMessageBoxShow("Correct Password is: " & clsCommon.DecryptString(Pwd), Me.Text, MessageBoxButtons.OK, RadMessageIcon.Error)
                Else
                    common.clsCommon.MyMessageBoxShow("Please enter Correct User ID and Password ", Me.Text, MessageBoxButtons.OK, RadMessageIcon.Error)
                End If
                Exit Sub
            End If
            ''=========check that user is login multiple times or not----------------------
            Dim IPAddress As String = Nothing
            IPAddress = CType(Dns.GetHostByName(Dns.GetHostName()).AddressList.GetValue(0), IPAddress).ToString()

            If strLoginStatus Then ''if user already logged in only then check below condition
                Dim strChkonMultipleMachine As Boolean = False
                strChkonMultipleMachine = clsCommon.myCBool(IIf(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.SameuserCanNotloginmultipletimes, clsFixedParameterCode.SameuserCanNotloginmultipletimes, Nothing)) = "1", True, False))

                If strChkonMultipleMachine Then
                    common.clsCommon.MyMessageBoxShow("User is already logged in on " + clsCommon.myCstr(strIpAdd) + "", Me.Text, MessageBoxButtons.OK, RadMessageIcon.Error)
                    Exit Sub
                End If
            End If

            qry = "update tspl_user_master set IP_For_Alert=null where IP_For_Alert='" + clsCommon.myCstr(IPAddress) + "'"
            clsDBFuncationality.ExecuteNonQuery(qry)

            qry = "update tspl_user_master set Login_Status=1,IP_Address='" + clsCommon.myCstr(IPAddress) + "',IP_For_Alert='" + clsCommon.myCstr(IPAddress) + "' where user_code='" + clsCommon.myCstr(txtUserName.Text) + "'"
            clsDBFuncationality.ExecuteNonQuery(qry)
            ''=============end here=======================================================

      

            objCommonVar.CurrentUserCode = clsCommon.myCstr(dt.Rows(0)("User_Code"))
            clsCommon.LoginId = objCommonVar.CurrentUserCode
            objCommonVar.CurrentUser = clsCommon.myCstr(dt.Rows(0)("User_Name"))
            objCommonVar.CurrUserLevel = clsCommon.myCdbl(dt.Rows(0)("ApprovalLevel"))
            qry = "select Comp_Code,Comp_Name,DataBase_Name,BaseCurrencyCode, Case When ApplyMultiCurrency=1 Then 'True' Else 'False' End as ApplyMultiCurrency from TSPL_COMPANY_MASTER where Comp_Code='" + clsCommon.myCstr(cboCompany.SelectedValue) + "'"
            dt = clsDBFuncationality.GetDataTable(qry)
            objCommonVar.CurrentCompanyCode = clsCommon.myCstr(dt.Rows(0)("Comp_Code"))
            objCommonVar.CurrentCompanyName = clsCommon.myCstr(dt.Rows(0)("Comp_Name"))
            objCommonVar.CurrDatabase = clsCommon.myCstr(dt.Rows(0)("DataBase_Name"))
            objCommonVar.BaseCurrencyCode = clsCommon.myCstr(dt.Rows(0)("BaseCurrencyCode"))
            objCommonVar.IsMultiCurrencyCompany = clsCommon.myCstr(dt.Rows(0)("ApplyMultiCurrency"))
            objCommonVar.CurrLocationCode = clsCommon.myCstr(ddllocation.SelectedValue)
            'objCommonVar.CurrLocationCode = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Default_Location from TSPL_USER_MASTER where User_Code='" + objCommonVar.CurrentUserCode + "' "))
            objCommonVar.CurrLocationName = clsCommon.myCstr(ddllocation.Text)

            objCommonVar.RefreshCommonVar()

            SetConnectionWithCommonDLL(objCommonVar.CurrDatabase)

            RefeshUserApplicableLocationsAndGLAccount()

            CreateAutoIndentAccordingReorderLevel()

            common.clsUserInfo.CurrentUserCode = objCommonVar.CurrentUserCode
            qry = "select 1 from sys.databases where name = '" + objCommonVar.CurrDatabase + "'"
            dt = clsDBFuncationality.GetDataTable(qry)
            

         

        Else
            common.clsCommon.MyMessageBoxShow("User Name or Password is not Correct.Please provide the correct login information.")
        End If
        Dim AllowAutoLockTransaction As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowAutoLockTransaction, clsFixedParameterCode.AllowAutoLockTransaction, Nothing))
        If AllowAutoLockTransaction = 1 Then
            'ShowUnPostedDocument()
            AUTOLOCKTRANSACTION()
        End If
        '-------------------------------------------------------------------------------
        'qry = "select description from TSPL_FIXED_PARAMETER where code='MAILOFF'"
        'IsMailSend = clsDBFuncationality.getSingleValue(qry)

        'If IsMailSend = "1" Then
        '    IsMailSend = "YES"
        'Else
        '    IsMailSend = "NO"
        'End If


        qry = "select IsThirdPartyLocationOnERP from TSPL_INV_PARAMETERS"
        IsLoc_Third_Party = clsDBFuncationality.getSingleValue(qry)

        If IsLoc_Third_Party = "1" Then
            IsLoc_Third_Party = "YES"
        Else
            IsLoc_Third_Party = "NO"
        End If
        '-------------------------------------------------------------------------------

        '    clsScreenNotificationSchedule.ShowLoginNotifications(objCommonVar.CurrentUserCode)
        Timer1.Start()

        qry = "Select User_Code from TSPL_LOCATION_SETTING  where User_Code='" + objCommonVar.CurrentUserCode + "' "
        Dim usercode = clsDBFuncationality.getSingleValue(qry)
        If clsCommon.myLen(usercode) > 0 Then
           
        End If

        '-------------------04/07/2014----------BM00000003039
        'ReminderTimer.Interval = 100000
        'ReminderTimer.Enabled = True
        'RadDesktopAlert1.ButtonItems.Add(radbuttonelement)
        'RadDesktopAlert1.ButtonItems.Add(radbuttonDontShow)
        AddHandler radbuttonelement.Click, AddressOf radbuttonelement_Click
        AddHandler radbuttonDontShow.Click, AddressOf DontShowAgain_Click

        ' GetPendingSaleOrder()
        'GetPendingSaleBooking()
        If clsCommon.myLen(objCommonVar.CurrentUserCode) > 0 Then
            GetMccFssaiPopUp()
            'Dim objsms As New FrmMccSMSSetting
            'objsms.SendMail("", clsCommon.GETSERVERDATE().AddDays(-1), "", clsUserMgtCode.frmMilkShiftEndMCC, "")
        End If
        '---------------end here
        '------For Application Idle State Checking
        Dim FILE_NAME As String = Application.StartupPath + "\Table.Txp"
        If Not System.IO.File.Exists(FILE_NAME) Then
            isIdle = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.Idle, clsFixedParameterCode.isIdleTimerOn, Nothing))
            IdleTimeinSeconds = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.Idle, clsFixedParameterCode.idleTime, Nothing)) * 60
            If isIdle = 1 Then
                If IdleTimeinSeconds > 0 Then
                    'If SystemIdleTimer1.IsRunning = False Then
                    '    SystemIdleTimer1.MaxIdleTime = IdleTimeinSeconds
                    '    SystemIdleTimer1.Start()
                    'Else
                    '    SystemIdleTimer1.Stop()
                    'End If
                    '  Timer3.Enabled = True
                End If
            End If
            'End of Application Idle State Checking
        End If
        ' OpenFormFromOtherDLL()


      

       

    End Sub

    Private Function LastDayOfMonth(aDate As DateTime) As Date
        Return New DateTime(aDate.Year, aDate.Month, DateTime.DaysInMonth(aDate.Year, aDate.Month))
    End Function
    Private Function LastDayOfPreviousMonth(aDate As DateTime) As Date
        Return New DateTime(aDate.Year, aDate.Month - 1, DateTime.DaysInMonth(aDate.Year, aDate.Month - 1))
    End Function
    Sub ShowUnPostedDocument()
        Try
            Dim qry As String = ""
            Dim currentDate As Date = clsCommon.GETSERVERDATE()
            Dim datLastDay As Date = LastDayOfMonth(currentDate)
            Dim AllowAutoLockTransaction As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowAutoLockTransaction, clsFixedParameterCode.AllowAutoLockTransaction, Nothing))
            Dim PromptTimeToPostTransactions As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.PromptTimeToPostTransactions, clsFixedParameterCode.PromptTimeToPostTransactions, Nothing))

            If AllowAutoLockTransaction = 1 AndAlso PromptTimeToPostTransactions > 0 Then
                Dim PromptDateToPostTransactions As Date = datLastDay.AddDays(-PromptTimeToPostTransactions)
                If clsCommon.myCDate(currentDate) >= clsCommon.myCDate(PromptDateToPostTransactions) Then

                    qry = "select * from ( " & _
                               "select 'Common Module' as Module,'Bank Reverse' as TransactionName,isnull( ( select ' '+tspl_bank_reverse.Document_No+' ,  '    from tspl_bank_reverse where isnull(Post,'N')='N' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'Bank Transfer' as TransactionName, isnull(( select ' '+TSPL_BANK_TRANSFER.Transfer_No +' ,  '    from TSPL_BANK_TRANSFER where isnull(Post,'N')='N' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'Bank Reco' as TransactionName, isnull(( select ' '+tspl_BankReco_Head.Reconciliation_Id +' ,  '    from tspl_BankReco_Head where isnull(Post,'N')='N' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'CForm Header' as TransactionName, isnull(( select ' '+TSPL_CForm_HEADER.Document_No +' ,  '    from TSPL_CForm_HEADER where isnull(Posted,'N')='N' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'Bank Gauantee' as TransactionName, isnull(( select ' '+tspl_bank_guarantee_master.DocNo +' ,  '    from tspl_bank_guarantee_master where isnull(status,'N')='N' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'Bank Opening Reco' as TransactionName, isnull(( select ' '+TSPL_BANK_OPENING_RECO.Code +' ,  '    from TSPL_BANK_OPENING_RECO where isnull(status,'0')='0' and Created_By=''  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Common Module' as Module,'Revaluation Entry' as TransactionName, isnull(( select ' '+TSPL_REVALUATION_HEAD.Document_No +' ,  '    from TSPL_REVALUATION_HEAD where isnull(status,'0')='0' and Created_By=''  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Receivables' as Module,'Receipt Entry' as TransactionName, isnull(( select ' '+TSPL_RECEIPT_HEADER.Receipt_No +' ,  '    from TSPL_RECEIPT_HEADER where isnull(Posted,'N')='N' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Receivables' as Module,'Receipt Adjustment' as TransactionName, isnull(( select ' '+TSPL_Receipt_Adjustment_Header.Adjustment_No +' ,  '    from TSPL_Receipt_Adjustment_Header where isnull(is_post,'N')='N' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Receivables' as Module,'AR Invoice Entry' as TransactionName, isnull(( select ' '+TSPL_Customer_Invoice_Head.Document_No +' ,  '    from TSPL_Customer_Invoice_Head where isnull(Status,0)='0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payable' as Module,'Payment Entry' as TransactionName, isnull(( select ' '+TSPL_PAYMENT_HEADER.Document_No +' ,  '    from TSPL_PAYMENT_HEADER where isnull(Posted,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payable' as Module,'AP invoice Entry' as TransactionName, isnull(( select ' '+TSPL_VENDOR_INVOICE_HEAD.Document_No +' ,  '    from TSPL_VENDOR_INVOICE_HEAD where isnull(Posting_Date,'')='' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payable' as Module,'Payment Adjusment Entry' as TransactionName, isnull(( select ' '+TSPL_Payment_Adjustment_Header.Adjustment_No +' ,  '    from TSPL_Payment_Adjustment_Header where isnull(is_post,'N')='N' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                                "select 'Payable' as Module,'Supplier Registration' as TransactionName, isnull(( select ' '+TSPL_SUPPLIER_REGISTRATION.Registration_No +' ,  '    from TSPL_SUPPLIER_REGISTRATION where isnull(Posted,'0')='0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                                " union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Purchase Indent' as TransactionName, isnull(( select ' '+TSPL_REQUISITION_HEAD.Requisition_Id +' ,  '    from TSPL_REQUISITION_HEAD where isnull(Status,'0')='0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'RFQ Detail' as TransactionName, isnull(( select ' '+TSPL_RFQ_HEAD.RFQ_NO +' ,  '    from TSPL_RFQ_HEAD where isnull(Is_Post,'0')='0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Vendor Quotation' as TransactionName, isnull(( select ' '+TSPL_VENDOR_QUOTATION_HEAD.Quotation_No +' ,  '    from TSPL_VENDOR_QUOTATION_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Purchase Order' as TransactionName, isnull(( select ' '+TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No +' ,  '    from TSPL_PURCHASE_ORDER_HEAD where isnull(Status,0)=0 and Created_By='" & objCommonVar.CurrentUserCode & "'  and MT_Is_Merchant_Trade=0 for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Gate Receipt Note' as TransactionName, isnull(( select ' '+TSPL_GRN_HEAD.GRNo +' ,  '    from TSPL_GRN_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'PO Weighment' as TransactionName, isnull(( select ' '+TSPL_PO_WEIGHTMENT_HEAD.Weighment_Code +' ,  '    from TSPL_PO_WEIGHTMENT_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               " and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'MRN Head' as TransactionName, isnull(( select ' '+TSPL_MRN_HEAD.MRN_No +' ,  '    from TSPL_MRN_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'SRN Head' as TransactionName, isnull(( select ' '+TSPL_SRN_HEAD.SRN_No +' ,  '    from TSPL_SRN_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'   for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Purchase Invoice' as TransactionName, isnull(( select ' '+TSPL_PI_HEAD.PI_No +' ,  '    from TSPL_PI_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Purchase Return' as TransactionName, isnull(( select ' '+TSPL_PR_HEAD.PR_No +' ,  '    from TSPL_PR_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'NRGP Request' as TransactionName, isnull(( select ' '+TSPL_NRGP_REQUEST_HEAD.BOOKING_NO +' ,  '    from TSPL_NRGP_REQUEST_HEAD where isnull(Posted,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'RGP/NRGP' as TransactionName, isnull(( select ' '+TSPL_RGP_head.rgp_no +' ,  '    from TSPL_RGP_head where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Purchase' as Module,'Issue/Return/Transfer' as TransactionName, isnull(( select ' '+TSPL_IssueReturn_HEAD.Doc_No +' ,  '    from TSPL_IssueReturn_HEAD where isnull(Status,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'TDS Deduction' as Module,'TDS Payment' as TransactionName, isnull(( select ' '+TSPL_TDS_PAYMENT_HEADER.Document_No +' ,  '    from TSPL_TDS_PAYMENT_HEADER where isnull(posted,0)=0 " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               " union all " + Environment.NewLine & _
                               "select 'General Ledger' as Module,'TDS Payment' as TransactionName, isnull(( select ' '+TSPL_JOURNAL_MASTER.Voucher_No +' ,  '    from TSPL_JOURNAL_MASTER where isnull(Authorized,'N') = 'N' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'General Ledger' as Module,'VCGL Entry' as TransactionName, isnull(( select ' '+TSPL_VCGL_Head.Document_No +' ,  '    from TSPL_VCGL_Head where isnull(Status,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Material Management' as Module,'Transfer' as TransactionName, isnull(( select ' '+TSPL_TRANSFER_ORDER_HEAD.Document_No +' ,  '    from TSPL_TRANSFER_ORDER_HEAD where isnull(Status,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Material Management' as Module,'Store/Production/Empty Adjustment' as TransactionName, isnull(( select ' '+TSPL_adjustment_header.adjustment_no +' ,  '    from TSPL_adjustment_header where isnull(posted,'N') = 'N' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fixed Assets' as Module,'Acquisition Entry' as TransactionName, isnull(( select ' '+TSPL_ACQUISITION_HEAD.Acquisition_Code +' ,  '    from TSPL_ACQUISITION_HEAD where isnull(Status,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fixed Assets' as Module,'Disposal Entry' as TransactionName, isnull(( select ' '+TSPL_ASSET_SCRAP_HEAD.Document_No +' ,  '    from TSPL_ASSET_SCRAP_HEAD where isnull(Status,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fixed Assets' as Module,'Asset Work Expense' as TransactionName, isnull(( select ' '+TSPL_ASSET_WORK_HEAD.Document_Code +' ,  '    from TSPL_ASSET_WORK_HEAD where isnull(Status,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Allowance Details' as TransactionName, isnull(( select ' '+TSPL_ALLOWANCE.ALLOWANCE_CODE +' ,  '    from TSPL_ALLOWANCE where isnull(Posted,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Deduction Detail' as TransactionName, isnull(( select ' '+TSPL_DEDUCTION.DEDUCTION_CODE +' ,  '    from TSPL_DEDUCTION where isnull(Posted,'0') = '0' " + Environment.NewLine & _
                               "and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Loan Application' as TransactionName, isnull(( select ' '+TSPL_LOAN_APPLICATION.LOAN_CODE +' ,  '    from TSPL_LOAN_APPLICATION where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Generate Bonus' as TransactionName, isnull(( select ' '+TSPL_EMPLOYEE_BONUS.EMP_BONUS_CODE +' ,  '    from TSPL_EMPLOYEE_BONUS where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Loan Generation' as TransactionName, isnull(( select ' '+TSPL_LOAN_GENERATION.LOAN_GENERATION_CODE +' ,  '    from TSPL_LOAN_GENERATION where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Daily Attendance' as TransactionName, isnull(( select ' '+TSPL_DAILY_ATTENDANCE.DLA_CODE +' ,  '    from TSPL_DAILY_ATTENDANCE where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Monthly Attendance' as TransactionName, isnull(( select ' '+TSPL_MONTHLY_ATTENDANCE.MTA_CODE +' ,  '    from TSPL_MONTHLY_ATTENDANCE where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Salary Grneration' as TransactionName, isnull(( select ' '+TSPL_GENERATE_SALARY.SALARY_GENERATION_CODE +' ,  '    from TSPL_GENERATE_SALARY where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Payroll' as Module,'Employee Increment' as TransactionName, isnull(( select ' '+TSPL_EMPLOYEE_INCREMENT_HEAD.INCREMENT_CODE +' ,  '    from TSPL_EMPLOYEE_INCREMENT_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Gate Entry In' as TransactionName, isnull(( select ' '+TSPL_MILK_GATE_ENTRY_IN.Entry_Code +' ,  '    from TSPL_MILK_GATE_ENTRY_IN where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Gate Entry Weighment' as TransactionName, isnull(( select ' '+TSPL_MILK_GATE_ENTRY_WEIGHTMENT.Weighment_Code +' ,  '    from TSPL_MILK_GATE_ENTRY_WEIGHTMENT where isnull(GW_Status,'0') = '0'and GW_Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Receipt' as TransactionName, isnull(( select ' '+TSPL_MILK_RECEIPT_HEAD.DOC_CODE +' ,  '    from TSPL_MILK_RECEIPT_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Sample' as TransactionName, isnull(( select ' '+TSPL_MILK_SAMPLE_HEAD.DOC_CODE +' ,  '    from TSPL_MILK_SAMPLE_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk SRN' as TransactionName, isnull(( select ' '+TSPL_MILK_SRN_HEAD.DOC_CODE +' ,  '    from TSPL_MILK_SRN_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Truck Sheet' as TransactionName, isnull(( select ' '+tspl_milk_truck_sheet_Head.DOC_CODE +' ,  '    from tspl_milk_truck_sheet_Head where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Shift End' as TransactionName, isnull(( select ' '+TSPL_MILK_Shift_End_HEAD.DOC_CODE +' ,  '    from TSPL_MILK_Shift_End_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Tanker Dispatch' as TransactionName, isnull(( select ' '+TSPL_MCC_Dispatch_Challan.Chalan_NO +' ,  '    from TSPL_MCC_Dispatch_Challan where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Tanker Location Charge' as TransactionName, isnull(( select ' '+tspl_MCC_dispatch_transfer.Doc_No +' ,  '    from tspl_MCC_dispatch_transfer where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Purchase Invoice Head' as TransactionName, isnull(( select ' '+TSPL_MILK_PURCHASE_INVOICE_HEAD.DOC_CODE +' ,  '    from TSPL_MILK_PURCHASE_INVOICE_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'VSP Asset Issue' as TransactionName, isnull(( select ' '+TSPL_VSPAsset_HEAD.Doc_No +' ,  '    from TSPL_VSPAsset_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'MCC Material Sale' as TransactionName, isnull(( select ' '+TSPL_SD_SHIPMENT_HEAD.Document_Code +' ,  '    from TSPL_SD_SHIPMENT_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SHIPMENT_HEAD.Trans_Type='MCC' for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'MCC Material Sale Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='MCC' for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'VSP Item Issue' as TransactionName, isnull(( select ' '+TSPL_VSPItem_HEAD.Doc_No +' ,  '    from TSPL_VSPItem_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Payment Process' as TransactionName, isnull(( select ' '+TSPL_PAYMENT_PROCESS_HEAD.Doc_No +' ,  '    from TSPL_PAYMENT_PROCESS_HEAD where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'Milk Recurring Payable Invoice' as TransactionName, isnull(( select ' '+TSPL_Recurring_Payable_INVOICE_Head.Document_No +' ,  '    from TSPL_Recurring_Payable_INVOICE_Head where isnull(Posting_Date,'') = '' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Procurement MCC' as Module,'MCC tanker Dispatch Return' as TransactionName, isnull(( select ' '+TSPL_MCC_Tanker_Dispatch_Return_head.Return_NO +' ,  '    from TSPL_MCC_Tanker_Dispatch_Return_head where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Gate Entry' as TransactionName, isnull(( select ' '+Tspl_Gate_Entry_Details.Gate_Entry_No +' ,  '    from Tspl_Gate_Entry_Details where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Weighment' as TransactionName, isnull(( select ' '+TSPL_Weighment_Detail.Weighment_No +' ,  '    from TSPL_Weighment_Detail where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Quality Check' as TransactionName, isnull(( select ' '+TSPL_QUALITY_CHECK.QC_No +' ,  '    from TSPL_QUALITY_CHECK where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Unloading' as TransactionName, isnull(( select ' '+TSPL_MILK_UNLOADING.Unloading_No +' ,  '    from TSPL_MILK_UNLOADING where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Unloading' as TransactionName, isnull(( select ' '+TSPL_MILK_UNLOADING.Unloading_No +' ,  '    from TSPL_MILK_UNLOADING where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Cleaning' as TransactionName, isnull(( select ' '+TSPL_Cleaning.Doc_No +' ,  '    from TSPL_Cleaning where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Bulk Milk SRN' as TransactionName, isnull(( select ' '+TSPL_Bulk_MILK_SRN.SRN_NO +' ,  '    from TSPL_Bulk_MILK_SRN where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Bulk Milk Purchase Invoice' as TransactionName, isnull(( select ' '+tspl_Bulk_milk_purchase_Invoice_head.DOC_NO +' ,  '    from tspl_Bulk_milk_purchase_Invoice_head where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Milk Transfer In' as TransactionName, isnull(( select ' '+TSPL_MILK_TRANSFER_IN.Receipt_Challan_No +' ,  '    from TSPL_MILK_TRANSFER_IN where isnull(isPosted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Bulk Procurement' as Module,'Provision Entry' as TransactionName, isnull(( select ' '+TSPL_PROVISION_ENTRY.Doc_No +' ,  '    from TSPL_PROVISION_ENTRY where isnull(isPosted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Gate Entry' as TransactionName, isnull(( select ' '+TSPL_GATEENTRY_SALE.Document_No +' ,  '    from TSPL_GATEENTRY_SALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Weighment' as TransactionName, isnull(( select ' '+TSPL_WEIGHMENT_DETAIL_BULKSALE.Weighment_No +' ,  '    from TSPL_WEIGHMENT_DETAIL_BULKSALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'LoadIn Tanker Detais' as TransactionName, isnull(( select ' '+TSPL_LOADING_TANKER_DETAIL_BULKSALE.LoadingTanker_No +' ,  '    from TSPL_LOADING_TANKER_DETAIL_BULKSALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Fat/SNF Check  /  QC Details' as TransactionName, isnull(( select ' '+TSPL_QUALITY_CHECK_BULKSALE.QC_No +' ,  '    from TSPL_QUALITY_CHECK_BULKSALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Bulk Dispatch' as TransactionName, isnull(( select ' '+TSPL_Dispatch_BulkSale.Document_No +' ,  '    from TSPL_Dispatch_BulkSale where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Bulk Invoice' as TransactionName, isnull(( select ' '+TSPL_INVOICE_MASTER_BULKSALE.Document_No +' ,  '    from TSPL_INVOICE_MASTER_BULKSALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Bulk Dispatch Trade' as TransactionName, isnull(( select ' '+TSPL_Dispatch_BulkSale_Trade.Document_No +' ,  '    from TSPL_Dispatch_BulkSale_Trade where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Bulk Sale' as Module,'Bulk Sale Return' as TransactionName, isnull(( select ' '+TSPL_SALE_RETURN_MASTER_BULKSALE.Document_No +' ,  '    from TSPL_SALE_RETURN_MASTER_BULKSALE where isnull(Posted,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'CSA Sale' as Module,'CSA Delivery Order' as TransactionName, isnull(( select ' '+TSPL_CSA_DO_HEAD.Doc_No +' ,  '    from TSPL_CSA_DO_HEAD where isnull(Is_Post,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'CSA Sale' as Module,'CSA Transfer' as TransactionName, isnull(( select ' '+TSPL_CSA_transfer_head.DOC_CODE +' ,  '    from TSPL_CSA_transfer_head where isnull(Status,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'CSA Sale' as Module,'Sale Patti' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_INVOICE_HEAD.Trans_Type='CSA' for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'CSA Sale' as Module,'CSA Transfer Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='CSA'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'CSA Sale' as Module,'CSA Sale Patti Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0' and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='CPR'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh  Booking' as TransactionName, isnull(( select ' '+TSPL_BOOKING_MATSER.Document_No +' ,  '    from TSPL_BOOKING_MATSER where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh  Delivery Order' as TransactionName, isnull(( select ' '+TSPL_DELIVERY_NOTE_MASTER_FRESHSALE.Document_No +' ,  '    from TSPL_DELIVERY_NOTE_MASTER_FRESHSALE where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh  Dispatch' as TransactionName, isnull(( select ' '+TSPL_SD_SHIPMENT_HEAD.Document_Code +' ,  '    from TSPL_SD_SHIPMENT_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SHIPMENT_HEAD.Trans_Type='FS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh Sale Invoice' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_INVOICE_HEAD.Trans_Type='FS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh Sale Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='FS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Fresh Sale' as Module,'Fresh Crate Received' as TransactionName, isnull(( select ' '+TSPL_CRATE_RECEIVED_HEAD_FRESHSALE.Document_No +' ,  '    from TSPL_CRATE_RECEIVED_HEAD_FRESHSALE where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Product  Booking' as TransactionName, isnull(( select ' '+TSPL_BOOKING_MASTER_PRODUCTSALE.Document_Code +' ,  '    from TSPL_BOOKING_MASTER_PRODUCTSALE where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Product  Delivery Order' as TransactionName, isnull(( select ' '+TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE.Document_Code +' ,  '    from TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Product  Dispatch' as TransactionName, isnull(( select ' '+TSPL_SD_SHIPMENT_HEAD.Document_Code +' ,  '    from TSPL_SD_SHIPMENT_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SHIPMENT_HEAD.Trans_Type='PS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Product Sale Invoice' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_INVOICE_HEAD.Trans_Type='PS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Product Sale Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='PS'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Product Sale' as Module,'Sale Order' as TransactionName, isnull(( select ' '+TSPL_SD_SALES_ORDER_HEAD.Document_Code +' ,  '    from TSPL_SD_SALES_ORDER_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALES_ORDER_HEAD.Trans_Type='PS'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Sale Quotaion' as TransactionName, isnull(( select ' '+TSPL_SD_QUOTATION_HEAD.Document_Code +' ,  '    from TSPL_SD_QUOTATION_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_QUOTATION_HEAD.Trans_Type='EXP'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Export Sale Order' as TransactionName, isnull(( select ' '+TSPL_SD_SALES_ORDER_HEAD.Document_Code +' ,  '    from TSPL_SD_SALES_ORDER_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALES_ORDER_HEAD.Trans_Type='EXP'  and TSPL_SD_SALES_ORDER_HEAD.salesorder_type='EX'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Export  Performa Invoice' as TransactionName, isnull(( select ' '+TSPL_EX_PI_HEAD.Document_Code +' ,  '    from TSPL_EX_PI_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_EX_PI_HEAD.document_type='EX'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Export Commercial Invoice' as TransactionName, isnull(( select ' '+TSPL_EX_COMMERCIAL_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_EX_COMMERCIAL_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and document_type='EX'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Export Sale Invoice' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_INVOICE_HEAD.Trans_Type='EXP' and TSPL_SD_SALE_INVOICE_HEAD.Document_Type='EX'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Export Sale' as Module,'Export Sale Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='EXP' and TSPL_SD_SALE_RETURN_HEAD.Document_Code='EX'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Trade' as Module,'Merchant Purchase Order' as TransactionName, isnull(( select ' '+TSPL_PURCHASE_ORDER_HEAD.PurchaseOrder_No +' ,  '    from TSPL_PURCHASE_ORDER_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and  TSPL_PURCHASE_ORDER_HEAD.MT_Is_Merchant_Trade=1  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'Merchant Sale Order' as TransactionName, isnull(( select ' '+TSPL_SD_SALES_ORDER_HEAD.Document_Code +' ,  '    from TSPL_SD_SALES_ORDER_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALES_ORDER_HEAD.Trans_Type='EXP'  and TSPL_SD_SALES_ORDER_HEAD.salesorder_type='MT'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'Merchant  Performa Invoice' as TransactionName, isnull(( select ' '+TSPL_EX_PI_HEAD.Document_Code +' ,  '    from TSPL_EX_PI_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_EX_PI_HEAD.document_type='MT'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'LC Request' as TransactionName, isnull(( select ' '+TSPL_LC_REQUEST_MT.LCRequestNo +' ,  '    from TSPL_LC_REQUEST_MT where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'LC Creation' as TransactionName, isnull(( select ' '+TSPL_LC_CREATION_MT.LCCreationNo +' ,  '    from TSPL_LC_CREATION_MT where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'Document Acceptance' as TransactionName, isnull(( select ' '+TSPL_DOCUMENT_ACCEPTANCE_MT.DocumentAcceptanceNo +' ,  '    from TSPL_DOCUMENT_ACCEPTANCE_MT where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'   for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'Merchant Sale Invoice' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_INVOICE_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_INVOICE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_INVOICE_HEAD.Trans_Type='EXP' and TSPL_SD_SALE_INVOICE_HEAD.Document_Type='MT'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Merchant Sale' as Module,'Merchant Sale Return' as TransactionName, isnull(( select ' '+TSPL_SD_SALE_RETURN_HEAD.Document_Code +' ,  '    from TSPL_SD_SALE_RETURN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "' and TSPL_SD_SALE_RETURN_HEAD.Trans_Type='EXP' and TSPL_SD_SALE_RETURN_HEAD.Document_Code='MT'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Planning' as TransactionName, isnull(( select ' '+TSPL_PP_PRODUCTION_PLAN_HEAD.Plan_Code +' ,  '    from TSPL_PP_PRODUCTION_PLAN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Planning' as TransactionName, isnull(( select ' '+TSPL_PP_PRODUCTION_PLAN_HEAD.Plan_Code +' ,  '    from TSPL_PP_PRODUCTION_PLAN_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Batch Order' as TransactionName, isnull(( select ' '+TSPL_PP_BATCH_ORDER_HEAD.Plan_Code +' ,  '    from TSPL_PP_BATCH_ORDER_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Issue Entry' as TransactionName, isnull(( select ' '+TSPL_PP_ISSUE_HEAD.Issue_Code +' ,  '    from TSPL_PP_ISSUE_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Standardization' as TransactionName, isnull(( select ' '+TSPL_PP_STANDARDIZATION_HEAD.Standardization_Code +' ,  '    from TSPL_PP_STANDARDIZATION_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Stage Process' as TransactionName, isnull(( select ' '+TSPL_PP_STAGE_PROCESS_HEAD.STAGE_PROCESS_CODE +' ,  '    from TSPL_PP_STAGE_PROCESS_HEAD where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Production Entry' as TransactionName, isnull(( select ' '+TSPL_PP_PRODUCTION_ENTRY.PROD_ENTRY_CODE +' ,  '    from TSPL_PP_PRODUCTION_ENTRY where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No] " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'Assemblies' as TransactionName, isnull(( select ' '+TSPL_PROD_ASSEMBLIES.CODE +' ,  '    from TSPL_PROD_ASSEMBLIES where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Production' as Module,'WRECKAGE ENTRY' as TransactionName, isnull(( select ' '+TSPL_WRECKAGE_ENTRY.WRECKAGE_ENTRY_CODE +' ,  '    from TSPL_WRECKAGE_ENTRY where isnull(Posted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Milk RGP' as TransactionName, isnull(( select ' '+TSPL_Milk_RGP_HEAD.RGP_No +' ,  '    from TSPL_Milk_RGP_HEAD where isnull(Status,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Milk Gate Entry' as TransactionName, isnull(( select ' '+TSPL_MILK_GATE_ENTRY_DETAILS.Gate_Entry_No +' ,  '    from TSPL_MILK_GATE_ENTRY_DETAILS where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Weighment Detail' as TransactionName, isnull(( select ' '+tspl_Milk_weighment_detail.Weighment_No +' ,  '    from tspl_Milk_weighment_detail where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Quality Check' as TransactionName, isnull(( select ' '+tspl_Milk_quality_check.QC_No +' ,  '    from tspl_Milk_quality_check where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Unloading' as TransactionName, isnull(( select ' '+TSPL_JOB_MILK_UNLOADING.Unloading_No +' ,  '    from TSPL_JOB_MILK_UNLOADING where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  " + Environment.NewLine & _
                               "union all " + Environment.NewLine & _
                               "select 'Milk Job Work' as Module,'Milk SRN' as TransactionName, isnull(( select ' '+tspl_Job_milk_srn.SRN_NO +' ,  '    from tspl_Job_milk_srn where isnull(isPosted,'0') = '0'and Created_By='" & objCommonVar.CurrentUserCode & "'  for xml path('')  ),'') as [Document No]  )  aa  where [Document No] <> ''"
                    Dim msg As String = ""
                    Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim strModule As String = ""
                        Dim strTrans As String = ""
                        Dim strDocument As String = ""
                        For Each dr As DataRow In dt.Rows
                            strModule = clsCommon.myCstr(dr("Module"))
                            strTrans = clsCommon.myCstr(dr("TransactionName"))
                            strDocument = clsCommon.myCstr(dr("Document No"))
                            msg += "Module - " + strModule + "  Transaction - " + strTrans + "  Document - " + clsCommon.myCstr(strDocument) + Environment.NewLine
                        Next
                        clsCommon.MyMessageBoxShow(msg)
                    End If
                End If
            End If

        Catch ex As Exception
            common.clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub
    '' TO Auto Lock All Transaction location and location segment wise
    Function AUTOLOCKTRANSACTION() As Boolean
        Dim qry As String = ""
        Dim currentDate As Date = clsCommon.GETSERVERDATE()
        Dim dblLockdays As Double = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.DaysToStartAutoLock, clsFixedParameterCode.DaysToStartAutoLock, Nothing))
        Dim datLastDay As Date = currentDate.AddDays(-dblLockdays)
        'Dim datLastDay As Date = LastDayOfPreviousMonth(currentDate)
        Dim intCount As Integer = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select count(*) from TSPL_LOCK_LOCATION where End_Date='" & clsCommon.GetPrintDate(datLastDay, "dd/MMM/yyyy") & "'"))

        If intCount <= 0 Then
            Dim trans As SqlTransaction = clsDBFuncationality.GetTransactin()

            Try
                Dim ArrLoc As New ArrayList

                '' Location Segement wise
                clsDBFuncationality.ExecuteNonQuery("Delete from TSPL_LOCK_LOCATION_SEGMENT ", trans)
                clsDBFuncationality.ExecuteNonQuery("Delete from TSPL_LOCK_LOCATION_SEGMENT_USER ", trans)
                qry = " Select Segment_code as Code, Description from TSPL_GL_SEGMENT_CODE where Seg_No=7 "
                Dim dtLoc As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

                

                '' Location wise
                clsDBFuncationality.ExecuteNonQuery("Delete from TSPL_LOCK_LOCATION ", trans)
                clsDBFuncationality.ExecuteNonQuery("Delete from TSPL_LOCK_LOCATION_USER ", trans)
                qry = " Select Location_Code as Code from TSPL_LOCATION_MASTER Where Location_Type='Physical' "
                Dim dtLocSeg As DataTable = clsDBFuncationality.GetDataTable(qry, trans)

               
                trans.Commit()
                clsCommon.MyMessageBoxShow("Transaction Locked Successfully", Me.Text)
            Catch ex As Exception
                trans.Rollback()
                common.clsCommon.MyMessageBoxShow(ex.Message)
            End Try
        End If

    End Function
    Public Shared Sub CreateAutoIndentAccordingReorderLevel()
        Try
            Dim EnableMsgPopupforReorderLevel As Boolean = False
            Dim qry As String = Nothing
            Dim dt As New DataTable()
            Dim strlocation = Nothing
            EnableMsgPopupforReorderLevel = clsCommon.myCBool(clsDBFuncationality.getSingleValue("Select TSPL_PURCHASE_SETTINGS.ENABLE_POPUP_REORDERLEVEL from TSPL_PURCHASE_SETTINGS", Nothing))
            If EnableMsgPopupforReorderLevel Then
                strlocation = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select TSPL_USER_MASTER.Default_Location from TSPL_USER_MASTER where TSPL_USER_MASTER.User_Code='" + objCommonVar.CurrentUserCode + "'", Nothing))
                qry = "select z.ItemCode,z.ItemType,z.ItemDesc ,z.Qty,z.Unit  from (select coalesce(xx.ItemCode,TSPL_ITEM_REORDER_LEVEL_NEW.item_Code) as ItemCode,(((TSPL_ITEM_REORDER_LEVEL_NEW.Reorder_Qty)*uom1.Conversion_Factor)/TSPL_ITEM_UOM_DETAIL.Conversion_Factor-isnull(xxx.Qty,0)) as Qty,TSPL_ITEM_MASTER.Item_Type as ItemType," & _
                        " TSPL_ITEM_MASTER.Item_Desc as ItemDesc ,TSPL_ITEM_UOM_DETAIL.UOM_Code as Unit from TSPL_ITEM_REORDER_LEVEL_NEW left outer join (select TSPL_INVENTORY_MOVEMENT.Item_Code as ItemCode," & _
                        " (SUM(TSPL_INVENTORY_MOVEMENT.Stock_Qty * case when TSPL_INVENTORY_MOVEMENT.InOut ='I' then 1 else -1 end)) as Balance from TSPL_INVENTORY_MOVEMENT where TSPL_INVENTORY_MOVEMENT.Location_Code='" + strlocation + "'  group by TSPL_INVENTORY_MOVEMENT.Item_Code)xx on xx.ItemCode=TSPL_ITEM_REORDER_LEVEL_NEW.Item_Code" & _
                        " left outer join TSPL_ITEM_MASTER ON TSPL_ITEM_MASTER.Item_Code=TSPL_ITEM_REORDER_LEVEL_NEW.Item_Code" & _
                        " left outer join (select TSPL_REQUISITION_DETAIL.Item_Code,SUM(TSPL_REQUISITION_DETAIL.Requisition_Qty) AS Qty from TSPL_REQUISITION_DETAIL " & _
                        " left outer join TSPL_REQUISITION_HEAD ON TSPL_REQUISITION_HEAD.Requisition_Id=TSPL_REQUISITION_DETAIL.Requisition_Id " & _
                        " WHERE TSPL_REQUISITION_HEAD.Status=0  group by TSPL_REQUISITION_DETAIL.Item_Code)xxx on xxx.Item_Code = TSPL_ITEM_REORDER_LEVEL_NEW.Item_Code" & _
                        " left outer join TSPL_ITEM_UOM_DETAIL on TSPL_ITEM_UOM_DETAIL.Item_Code=TSPL_ITEM_REORDER_LEVEL_NEW.Item_Code left outer join TSPL_ITEM_UOM_DETAIL uom1 on uom1.Item_Code = TSPL_ITEM_REORDER_LEVEL_NEW.Item_Code and uom1.UOM_Code=(case when isnull(TSPL_ITEM_REORDER_LEVEL_NEW.UOM_Code,'')='' then uom1.UOM_Code else TSPL_ITEM_REORDER_LEVEL_NEW.Uom_Code end)" & _
                        " where TSPL_ITEM_REORDER_LEVEL_NEW.Location_Code='" + strlocation + "' and TSPL_ITEM_REORDER_LEVEL_NEW.Reorder_Level>coalesce(xx.Balance,0)" & _
                        " and TSPL_ITEM_UOM_DETAIL.Stocking_Unit='Y' and TSPL_ITEM_REORDER_LEVEL_NEW.Apply='Y')z where z.Qty>0"

                dt = clsDBFuncationality.GetDataTable(qry, Nothing)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    If clsCommon.CompairString(objCommonVar.CurrentCompanyCode, "KL") = CompairStringResult.Equal Then
                        clsCommon.MyMessageBoxShow("Some items reached their re-order level.", "Warning", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    '' done by Panch Raj on 29-11-2017
                    If clsCommon.CompairString(objCommonVar.CurrentUserCode, "admin") <> CompairStringResult.Equal Then
                        Dim qryCheck As String = " select count(*) as rec from TSPL_GROUP_PROGRAM_MAPPING " & _
                                        " inner join TSPL_USER_GROUP_MAPPING on TSPL_GROUP_PROGRAM_MAPPING.Group_Code=TSPL_USER_GROUP_MAPPING.Group_Code " & _
                                        " where TSPL_GROUP_PROGRAM_MAPPING.Program_Code='ITM-REOD-M' and TSPL_USER_GROUP_MAPPING.User_Code='" & objCommonVar.CurrentUserCode & "' and TSPL_GROUP_PROGRAM_MAPPING.Modify_Flag=1"
                        If clsCommon.myCdbl(clsDBFuncationality.getSingleValue(qryCheck)) <= 0 Then
                            qryCheck = Nothing
                            Exit Sub
                        End If
                    End If


                    If clsCommon.MyMessageBoxShow("Some items reached their re-order level. Do you want to create auto indent ?", "Question", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then

                       
                        'Dim obj As New clsRequistionHead()
                        'Dim drrow As DataRow() = Nothing
                        'For i As Integer = 0 To 6
                        '    If i = 0 Then
                        '        drrow = dt.Select("ItemType = 'F'")
                        '        obj.Item_Type = "F"
                        '    ElseIf i = 1 Then
                        '        drrow = dt.Select("ItemType = 'S'")
                        '        obj.Item_Type = "S"
                        '    ElseIf i = 2 Then
                        '        drrow = dt.Select("ItemType = 'R'")
                        '        obj.Item_Type = "R"
                        '    ElseIf i = 3 Then
                        '        drrow = dt.Select("ItemType = 'A'")
                        '        obj.Item_Type = "A"
                        '    ElseIf i = 4 Then
                        '        drrow = dt.Select("ItemType = 'T'")
                        '        obj.Item_Type = "T"
                        '    ElseIf i = 5 Then
                        '        drrow = dt.Select("ItemType = 'N'")
                        '        obj.Item_Type = "N"
                        '    ElseIf i = 6 Then
                        '        drrow = dt.Select("ItemType = 'O'")
                        '        obj.Item_Type = "O"
                        '    End If
                        '    If drrow IsNot Nothing AndAlso drrow.Count > 0 Then
                        '        obj.Requisition_Id = ""
                        '        obj.Requisition_Date = clsCommon.GETSERVERDATE(Nothing)
                        '        obj.On_Hold = 0
                        '        obj.Location = strlocation
                        '        obj.RQ_Detail_Total_Amt = 0
                        '        obj.Total_RQ_Amt = 0
                        '        obj.Mode_Of_Transport = "By Road"
                        '        obj.Is_Internal = "N"
                        '        'obj.Item_Type = clsCommon.myCstr(cboItemType.SelectedValue)
                        '        'obj.Dept = txtDept.Value
                        '        'obj.Dept_Desc = lblDept.Text
                        '        obj.Requisition_Type = "L"
                        '        obj.Category = "Regular"
                        '        obj.close_yn = "N"
                        '        obj.Approvel_Level_Required = 2

                        '        obj.ArrTr = New List(Of clsRequistionDetail)
                        '        For Each drow As DataRow In drrow
                        '            Dim objTr As New clsRequistionDetail()
                        '            objTr.Item_Code = clsCommon.myCstr(drow("ItemCode"))
                        '            objTr.Item_Desc = clsCommon.myCstr(drow("ItemDesc"))
                        '            objTr.Requisition_Qty = clsCommon.myCdbl(drow("Qty"))
                        '            objTr.Balance_Qty = clsCommon.myCdbl(drow("Qty"))
                        '            objTr.Item_Cost = 0
                        '            objTr.Item_Net_Amt = 0
                        '            objTr.Location = strlocation
                        '            objTr.Unit_Code = clsCommon.myCstr(drow("Unit"))
                        '            objTr.Status = "N"
                        '            If (clsCommon.myLen(objTr.Item_Code) > 0) Then
                        '                obj.ArrTr.Add(objTr)
                        '            End If
                        '        Next
                        '        If obj.ArrTr IsNot Nothing AndAlso obj.ArrTr.Count > 0 Then
                        '            obj.SaveData(obj, "True")
                        '        End If
                        '    End If
                        'Next
                    End If
                End If
            End If
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message.ToString())
        End Try
    End Sub





    Public Sub GetMccFssaiPopUp()
      
    End Sub

    Public Sub GetPendingSaleOrder()
        Try
            Dim qry As String = "select isnull((Select distinct '['+TSPL_SD_SALES_ORDER_HEAD.Document_Code+']  ' from TSPL_SD_SALES_ORDER_HEAD left outer join TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE on TSPL_SD_SALES_ORDER_HEAD.Document_Code=TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE.Against_Sales_Order where  TSPL_SD_SALES_ORDER_HEAD.Status=1 and TSPL_SD_SALES_ORDER_HEAD.Delivery_date > '" & clsCommon.GetPrintDate(clsCommon.GETSERVERDATE, "dd/MMM/yyyy") & "' and TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE.Document_Code not in (select isnull(Delivery_Code_PS,'')   from TSPL_SD_SHIPMENT_HEAD ) for xml path('')),'')  as DocNo "
            Dim strDocNo As String = clsDBFuncationality.getSingleValue(qry)
            If clsCommon.myLen(strDocNo) > 0 Then
                clsCommon.MyMessageBoxShow("Delivery Date will expired for these Sales order " & strDocNo & " ")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub GetPendingSaleBooking()
        Try
            Dim qry As String = "select isnull((Select distinct '['+TSPL_BOOKING_MASTER_PRODUCTSALE.Document_Code+']  ' from TSPL_BOOKING_MASTER_PRODUCTSALE left outer join TSPL_SD_SALES_ORDER_HEAD on TSPL_BOOKING_MASTER_PRODUCTSALE.Document_Code=TSPL_SD_SALES_ORDER_HEAD.Against_Booking_No where  TSPL_BOOKING_MASTER_PRODUCTSALE.Status=1 and TSPL_BOOKING_MASTER_PRODUCTSALE.BookValidity_date > '" & clsCommon.GetPrintDate(clsCommon.GETSERVERDATE, "dd/MMM/yyyy") & "' and ( TSPL_BOOKING_MASTER_PRODUCTSALE.Document_Code not in (select isnull(Against_Booking_No,'')   from TSPL_SD_SALES_ORDER_HEAD ) or TSPL_BOOKING_MASTER_PRODUCTSALE.Document_Code not in (select isnull(Against_Booking_No,'')   from TSPL_DELIVERY_ORDER_HEAD_PRODUCTSALE ) ) for xml path('')),'')  as DocNo "
            Dim strDocNo As String = clsDBFuncationality.getSingleValue(qry)
            If clsCommon.myLen(strDocNo) > 0 Then
                clsCommon.MyMessageBoxShow("Booking Date will expired for these Booking " & strDocNo & " ")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub btnLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        CheckAndLogin()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Sub LoadWorkingScreen()
        SplitPanel1.Collapsed = True
        SplitPanel2.Collapsed = True
        SplitPanel4.Collapsed = True
        SplitPanel3.Collapsed = False
        LoadImageList()

        'ToolWindow2.ToolCaptionButtons = ToolStripCaptionButtons.AutoHide




        Dim splitPanelElement = TryCast(RadDock1.RootElement.Children(0), SplitPanelElement)
        Dim imagePrimitive = New ImagePrimitive()
        'imagePrimitive.Image = Global.ERP.My.Resources.Resources.BackImageXpertERP 'BackImageDemo
        If Not objCommonVar.IsDemoERP Then
            '   imagePrimitive.Image = Global.ERP.My.Resources.Resources.BackImageXpertERPFMCGN
        End If


        imagePrimitive.Alignment = ContentAlignment.TopRight
        imagePrimitive.StretchHorizontally = True
        imagePrimitive.StretchVertically = True

        imagePrimitive.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed
        RadDock1.MainDocumentContainer.SplitPanelElement.Children.Add(imagePrimitive)

        Try
            imagePrimitive = New ImagePrimitive()
            Dim img As Byte() = DirectCast(clsDBFuncationality.getSingleValue("select top 1 Logo_Img  from tspl_company_master "), Byte())
            Dim ms As MemoryStream = New MemoryStream(img)
            imagePrimitive.Image = Image.FromStream(ms)
            imagePrimitive.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed
            imagePrimitive.Alignment = ContentAlignment.TopLeft
            imagePrimitive.Size = New Point(184, 112)

            RadDock1.MainDocumentContainer.SplitPanelElement.Children.Add(imagePrimitive)
        Catch ex As Exception

        End Try


        '   lblUserCode.Text = objCommonVar.CurrentUserCode
        ' lblUser.Text = objCommonVar.CurrentUser
        ' lblCompanyCode.Text = objCommonVar.CurrentCompanyCode
        ' lblCompany.Text = objCommonVar.CurrentCompanyName.Trim()
        ' strUserCode = objCommonVar.CurrentUserCode
        '  strCompany = objCommonVar.CurrentCompanyCode
        ' lblDataBase.Text = objCommonVar.CurrDatabase.Trim()
        ' lblLocation.Text = objCommonVar.CurrLocationName.Trim()
        '  lblLocation.Text = clsLocation.GetName(clsGateEntry.getUsersDefaultLocation(), Nothing)
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height
        Me.Location = New Point(0, 0)
        RadDock1.BackColor = Color.Transparent
        RadDock1.BackgroundImage = ImageList2.Images.Item(0)
        RadDock1.BackgroundImageLayout = ImageLayout.Center
       

        LoadMenuInCombo()
        '' alert reminder for bday/anniversary 
        Dim UserCode As String = objCommonVar.CurrentUserCode
        If clsFixedParameter.GetData(clsFixedParameterType.AllowToDispalyAlertForBDayAnniversary, clsFixedParameterType.AllowToDispalyAlertForBDayAnniversary, Nothing) = "1" Then
            'If clsEmployeeMaster.CheckUserForHRDepartment(UserCode) Then
            '    Dim msg As String = clsEmployeeMaster.GetBdayAnniversaryMSG()
            '    If clsCommon.myLen(msg) > 0 Then
            '        'clsERPFuncationality.ShowAlert(msg, "B'Day/Anniversary Reminder")
            '    End If
            'End If
        End If
        '' email reminder for bday/anniversary 
        If clsFixedParameter.GetData(clsFixedParameterType.AllowToSendEmailForBDayAnniversary, clsFixedParameterType.AllowToSendEmailForBDayAnniversary, Nothing) = "1" Then
         
        End If


    End Sub

    'Public Sub LoadMenu()
    '    Try
    '        arrExcluded.Clear()
    '        If Not isUtilityAdded Then
    '            arrExcluded.Add(clsUserMgtCode.ModuleUtility)
    '        End If
    '        arrExcluded.Add(clsUserMgtCode.frmJEReverse)
    '        If objCommonVar.IsDemoERP Then
    '            ''Menu item Only for  FMCG  
    '            arrExcluded.Add(clsUserMgtCode.ModuleSales)
    '            arrExcluded.Add(clsUserMgtCode.Indent)
    '            'arrExcluded.Add(clsUserMgtCode.Transfer)
    '            arrExcluded.Add(clsUserMgtCode.CreateTransfer)
    '            arrExcluded.Add(clsUserMgtCode.FrmItemMcMapping)
    '            arrExcluded.Add(clsUserMgtCode.mbtnEmptyTrans)
    '            arrExcluded.Add(clsUserMgtCode.frmMorningReport)
    '            arrExcluded.Add(clsUserMgtCode.StockStatement)

    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.SHowBulkMilkWeighment, clsFixedParameterCode.SHowBulkMilkWeighment, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.frmWeighment)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.isIntimationRequired, clsFixedParameterCode.isIntimationRequired, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.frmIntimation)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowGateEntryAgainstPO, clsFixedParameterCode.AllowGateEntryAgainstPO, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.frmPOBulkProc)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowFreshPriceChartOnProductSale, clsFixedParameterCode.AllowFreshPriceChartOnProductSale, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.frmdispatchAdviceProductSale)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowGateReturn, clsFixedParameterCode.AllowGateReturn, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.frmGateEntryReturnTransfer)
    '                arrExcluded.Add(clsUserMgtCode.frmGateEntryReturnPS)
    '                arrExcluded.Add(clsUserMgtCode.frmGateEntryReturnCS)
    '            End If
    '            arrExcluded.Add(clsUserMgtCode.packType)
    '            'arrExcluded.Add(clsUserMgtCode.PriceMaster)
    '            arrExcluded.Add(clsUserMgtCode.SchemeMaster)
    '            arrExcluded.Add(clsUserMgtCode.mbtnBreakageHead1)
    '            arrExcluded.Add(clsUserMgtCode.ItemExciseMapping)
    '            arrExcluded.Add(clsUserMgtCode.ItemBasicPrice)

    '            arrExcluded.Add(clsUserMgtCode.ItemPrice)
    '            arrExcluded.Add(clsUserMgtCode.StockRecoReport)
    '            arrExcluded.Add(clsUserMgtCode.FrmStockDispatchReport)
    '            arrExcluded.Add(clsUserMgtCode.FrmAdjustmentStatusReport1)
    '            arrExcluded.Add(clsUserMgtCode.BreakageReportSummary)
    '            arrExcluded.Add(clsUserMgtCode.mbtnBreakageReport)
    '            arrExcluded.Add(clsUserMgtCode.RoutewiseBreakageReport)
    '            arrExcluded.Add(clsUserMgtCode.SchemeReport)
    '            arrExcluded.Add(clsUserMgtCode.StockReportForFinishedGoods)
    '            arrExcluded.Add(clsUserMgtCode.FrmAdjustmentReport)
    '            arrExcluded.Add(clsUserMgtCode.rptVehicleWiseLoadout)
    '            arrExcluded.Add(clsUserMgtCode.FrmPendingIndentTransferReport)
    '            arrExcluded.Add(clsUserMgtCode.FrmExpiredItemDetails)
    '            arrExcluded.Add(clsUserMgtCode.itemMaster)
    '            arrExcluded.Add(clsUserMgtCode.ShiptoLocation)
    '            'KUNAL > KDIL > REMOVED THE EMPTY LINK AS PER RANJANA MADAM DISCUSSION. > DATE 11-NOV-2016
    '            arrExcluded.Add(clsUserMgtCode.RptPaymentRelization)
    '            If clsCommon.CompairString(objCommonVar.CurrentIndustryType, "D") = CompairStringResult.Equal Then
    '                'arrExcluded.Add(clsUserMgtCode.ACCSETMFG)
    '                'arrExcluded.Add(clsUserMgtCode.COSTMAINTAIN)
    '                'arrExcluded.Add(clsUserMgtCode.SETT)
    '                'arrExcluded.Add(clsUserMgtCode.EXPENSE)
    '                'arrExcluded.Add(clsUserMgtCode.PRO)
    '                'arrExcluded.Add(clsUserMgtCode.ITEMCATEGORY)
    '                'arrExcluded.Add(clsUserMgtCode.frmBOMImport)
    '                'arrExcluded.Add(clsUserMgtCode.ALTER)
    '                'arrExcluded.Add(clsUserMgtCode.frmBillOfMaterial)

    '                'arrExcluded.Add(clsUserMgtCode.frmProductionPlanning)
    '                'arrExcluded.Add(clsUserMgtCode.frmBatchOrder)
    '                'arrExcluded.Add(clsUserMgtCode.frmProductionRequisition)
    '                'arrExcluded.Add(clsUserMgtCode.frmStoreIssue)
    '                'arrExcluded.Add(clsUserMgtCode.frmProductionReturn)
    '                'arrExcluded.Add(clsUserMgtCode.frmProductionReceipt)

    '                'arrExcluded.Add(clsUserMgtCode.LALT)
    '                'arrExcluded.Add(clsUserMgtCode.LACCt)
    '                'arrExcluded.Add(clsUserMgtCode.frmListOfBOM)
    '                'arrExcluded.Add(clsUserMgtCode.LOIC)
    '                'arrExcluded.Add(clsUserMgtCode.PRODREPORT)
    '                'arrExcluded.Add(clsUserMgtCode.frmListofRequisition)
    '                'arrExcluded.Add(clsUserMgtCode.Resource)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.ShowGRN, clsFixedParameterCode.ShowGRN, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.mbtnGRN)
    '            End If
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.ShowMRN, clsFixedParameterCode.ShowMRN, Nothing)) = 0 Then
    '                arrExcluded.Add(clsUserMgtCode.mbtnMRN)
    '            End If
    '            arrExcluded.Add(clsUserMgtCode.frmMilkCollectionArea)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkVehicleTypeMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkTransportRateMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkComponentMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkComponentRateList)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkAdvanceMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkRateTypeMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkShiftMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmSeasonMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmUOMMaster)

    '            arrExcluded.Add(clsUserMgtCode.frmMilkSuppliers)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCRouteMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCSuperwiserMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCSupplierMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkCollection)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkQualityCheck)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkRateProcessingScheme)
    '            arrExcluded.Add(clsUserMgtCode.frmVehicleMovement)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkBillGeneration)

    '            If IsLoaction_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmLocationCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmLocationCategoryStructure)
    '            End If
    '            If IsCustomer_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmCustomerCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmCustomerCategoryStructure)
    '            End If
    '            If IsVendor_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmVendorCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmVendorCategoryStructure)
    '            End If
    '            arrExcluded.Add(clsUserMgtCode.AssetSegment)
    '            'arrExcluded.Add(clsUserMgtCode.frmSecondaryCustomer)

    '        Else

    '            ''Menu item Only for Xpert ERP
    '            arrExcluded.Add(clsUserMgtCode.frmProductionReceiptDemo)
    '            arrExcluded.Add(clsUserMgtCode.frmProductionItemSerialReplace)
    '            arrExcluded.Add(clsUserMgtCode.frmProductionSerializedReport)

    '            arrExcluded.Add(clsUserMgtCode.DVAT30)
    '            arrExcluded.Add(clsUserMgtCode.DVAT31)
    '            'arrExcluded.Add(clsUserMgtCode.frmBalanceSheetPerforma)
    '            'arrExcluded.Add(clsUserMgtCode.rptBalanceSheet)
    '            arrExcluded.Add(clsUserMgtCode.ModuleSalesNew)
    '            arrExcluded.Add(clsUserMgtCode.frmBarCodeGenerator)
    '            arrExcluded.Add(clsUserMgtCode.frmRequisitionApproval)
    '            arrExcluded.Add(clsUserMgtCode.RequisitSubTypeMaster)
    '            arrExcluded.Add(clsUserMgtCode.mbtnPendingApprovalOfReq)
    '            arrExcluded.Add(clsUserMgtCode.RFQ)
    '            arrExcluded.Add(clsUserMgtCode.VendorQuotation)
    '            arrExcluded.Add(clsUserMgtCode.VendorComparison)
    '            arrExcluded.Add(clsUserMgtCode.VendorComparisonApproval)
    '            arrExcluded.Add(clsUserMgtCode.ModuleBI)
    '            arrExcluded.Add(clsUserMgtCode.FrmCFormEntry)
    '            arrExcluded.Add(clsUserMgtCode.frmMapLedgerAccToTally)
    '            arrExcluded.Add(clsUserMgtCode.frmPostAllGLToTally)
    '            arrExcluded.Add(clsUserMgtCode.frmCFormReport)
    '            arrExcluded.Add(clsUserMgtCode.frmPurchaseOrderList)
    '            arrExcluded.Add(clsUserMgtCode.FrmUserApproval)
    '            arrExcluded.Add(clsUserMgtCode.FrmBudgetMaintenance)
    '            arrExcluded.Add(clsUserMgtCode.ModuleProjectManagement)
    '            arrExcluded.Add(clsUserMgtCode.FrmExpenseType)
    '            arrExcluded.Add(clsUserMgtCode.FrmProjectStatus)
    '            arrExcluded.Add(clsUserMgtCode.FrmPJCExpense)
    '            arrExcluded.Add(clsUserMgtCode.stockRecoNew)
    '            arrExcluded.Add(clsUserMgtCode.FrmProcessMaster1)
    '            arrExcluded.Add(clsUserMgtCode.frmLabourWorkingSheet)
    '            arrExcluded.Add(clsUserMgtCode.frmOperaterEfficiencyReport)
    '            'arrExcluded.Add(clsUserMgtCode.ACCSETMFG)
    '            arrExcluded.Add(clsUserMgtCode.frmDemoProductionPlanning)
    '            arrExcluded.Add(clsUserMgtCode.COSTMAINTAIN)
    '            'arrExcluded.Add(clsUserMgtCode.SETT)
    '            arrExcluded.Add(clsUserMgtCode.EXPENSE)
    '            arrExcluded.Add(clsUserMgtCode.TOOLTYPE)
    '            arrExcluded.Add(clsUserMgtCode.frmWorkCenterMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmResourceMaster)
    '            arrExcluded.Add(clsUserMgtCode.TOOL)
    '            arrExcluded.Add(clsUserMgtCode.frmOperationMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmBOMImport)
    '            arrExcluded.Add(clsUserMgtCode.ALTER)
    '            arrExcluded.Add(clsUserMgtCode.FrmProcessMaster1)
    '            arrExcluded.Add(clsUserMgtCode.frmBillOfMaterialCosting)
    '            arrExcluded.Add(clsUserMgtCode.frmManufacturingOrder)
    '            arrExcluded.Add(clsUserMgtCode.AssetSegment)
    '            arrExcluded.Add(clsUserMgtCode.FrmApprovalSetting)
    '            arrExcluded.Add(clsUserMgtCode.frmBarCodeGenerator1)
    '            arrExcluded.Add(clsUserMgtCode.WarrantyMaster)
    '            arrExcluded.Add(clsUserMgtCode.FrmItemSerialTrackingReport)
    '            arrExcluded.Add(clsUserMgtCode.ChangeItemSerialNumber)
    '            arrExcluded.Add(clsUserMgtCode.frmSchemeMasterNew) ' New Scheme Master @ Material Mgmt>>Master
    '            arrExcluded.Add(clsUserMgtCode.frmWeightConversion) ' New Scheme Master @ Material Mgmt>>Master

    '            '' SETUP REPORTS PRODUCTION DEMO
    '            arrExcluded.Add(clsUserMgtCode.LALT)
    '            arrExcluded.Add(clsUserMgtCode.LACCt)
    '            arrExcluded.Add(clsUserMgtCode.LOIC)
    '            arrExcluded.Add(clsUserMgtCode.LOPER)
    '            arrExcluded.Add(clsUserMgtCode.Resource)
    '            arrExcluded.Add(clsUserMgtCode.LToolT)
    '            arrExcluded.Add(clsUserMgtCode.LTOOL)
    '            arrExcluded.Add(clsUserMgtCode.LWC)

    '            arrExcluded.Add(clsUserMgtCode.frmMilkCollectionArea)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkVehicleTypeMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkTransportRateMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkComponentMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkComponentRateList)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkAdvanceMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkRateTypeMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkShiftMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmSeasonMaster)
    '            arrExcluded.Add(clsUserMgtCode.frmUOMMaster)

    '            arrExcluded.Add(clsUserMgtCode.frmMilkSuppliers)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCRouteMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCSuperwiserMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMCCSupplierMapping)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkCollection)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkQualityCheck)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkRateProcessingScheme)
    '            arrExcluded.Add(clsUserMgtCode.frmVehicleMovement)
    '            arrExcluded.Add(clsUserMgtCode.frmMilkBillGeneration)
    '            If IsLoaction_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmLocationCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmLocationCategoryStructure)
    '            End If
    '            If IsCustomer_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmCustomerCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmCustomerCategoryStructure)
    '            End If
    '            If IsVendor_NLevel = "NO" Then
    '                arrExcluded.Add(clsUserMgtCode.frmVendorCategoryLevel)
    '                arrExcluded.Add(clsUserMgtCode.frmVendorCategoryStructure)
    '            End If
    '        End If

    '        If clsCommon.CompairString(objCommonVar.CurrentIndustryType, "R") <> CompairStringResult.Equal Then
    '            arrExcluded.Add(clsUserMgtCode.frmRiceBOM)
    '            arrExcluded.Add(clsUserMgtCode.frmRiceMixingEntry)
    '            arrExcluded.Add(clsUserMgtCode.frmRiceProcessingEntry)
    '        End If

    '        If clsCommon.CompairString(clsFixedParameter.GetData("MilkProc", "EnableMilkProc", Nothing), "1") = CompairStringResult.Equal Then
    '            arrExcluded.Add(clsUserMgtCode.ModuleMilkProcurement)
    '        End If

    '        ' Hiding Redundant Copy of Price Chart Master , Done By Pankaj Jha As suggested by Ranjana MAM
    '        arrExcluded.Add(clsUserMgtCode.frmPriceChartMaster_Bulk)
    '        arrExcluded.Add(clsUserMgtCode.rptDailyProgressReport)
    '        arrExcluded.Add(clsUserMgtCode.rptShiftCodeWise)
    '        'arrExcluded.Add(clsUserMgtCode.RptBulkMilkRegister)
    '        arrExcluded.Add(clsUserMgtCode.rptSectionWiseStockReport)
    '        arrExcluded.Add(clsUserMgtCode.frmAssetRequisition)

    '        '' payroll reports 
    '        arrExcluded.Add(clsUserMgtCode.frmAditionalEarning_DeductionReport)
    '        arrExcluded.Add(clsUserMgtCode.frmAttendedDaysReport)
    '        arrExcluded.Add(clsUserMgtCode.frmDeductionDetailsReport)
    '        arrExcluded.Add(clsUserMgtCode.frmOT_Reports)
    '        arrExcluded.Add(clsUserMgtCode.frmSalaryComponentDetails)
    '        arrExcluded.Add(clsUserMgtCode.frmSalaryIncrementReport)
    '        arrExcluded.Add(clsUserMgtCode.frmSalarySheet_Reports)
    '        arrExcluded.Add(clsUserMgtCode.frmSalaryVoucher_Reports)
    '        arrExcluded.Add(clsUserMgtCode.frmVarianceReport)
    '        arrExcluded.Add(clsUserMgtCode.FrmSalarySlipRpt)
    '        arrExcluded.Add(clsUserMgtCode.FrmAMAcquisitionCode)

    '        '===shivani against ticket[BM00000009243,BM00000009240] 
    '        arrExcluded.Add(clsUserMgtCode.RptMultiplePaymentAdvice1)
    '        arrExcluded.Add(clsUserMgtCode.RptVehicleWiseReport)
    '        '==============
    '        'arrExcluded.Add(clsUserMgtCode.FrmEmployeePFRpt)
    '        ' arrExcluded.Add(clsUserMgtCode.RptBOILetterReport)
    '        'arrExcluded.Add(clsUserMgtCode.frmSalaryCertificate)
    '        'arrExcluded.Add(clsUserMgtCode.frmNewSalCertificate)
    '        arrExcluded.Add(clsUserMgtCode.frmEmployeeIncrement)
    '        'arrExcluded.Add(clsUserMgtCode.rptMCCMilkRegisterDripSaver)
    '        arrExcluded.Add(clsUserMgtCode.rptVSPOrVLCVarationRpt)
    '        '=======================
    '        arrExcluded.Add(clsUserMgtCode.frmLeaveAllotment)
    '        arrExcluded.Add(clsUserMgtCode.frmLeaveOpeningBalance)
    '        ''===against [BM00000008017]
    '        arrExcluded.Add(clsUserMgtCode.frmVisi_Install_Pullout_Report)
    '        arrExcluded.Add(clsUserMgtCode.frmDistributor_VS_SecondaryCustomer_Sale)
    '        arrExcluded.Add(clsUserMgtCode.frmSecondaryCustomer)
    '        arrExcluded.Add(clsUserMgtCode.frmSecondaryCustomerSale)
    '        arrExcluded.Add(clsUserMgtCode.frmVisi_Install_Pullout)

    '        arrExcluded.Add(clsUserMgtCode.mbtnItemMovement)
    '        arrExcluded.Add(clsUserMgtCode.DVAT30)
    '        arrExcluded.Add(clsUserMgtCode.DVAT31)
    '        arrExcluded.Add(clsUserMgtCode.CrptRG1Detail1)
    '        arrExcluded.Add(clsUserMgtCode.frmExciseChapterWise)
    '        arrExcluded.Add(clsUserMgtCode.FrmPurchasebookReport1)
    '        arrExcluded.Add(clsUserMgtCode.frmEmp_Id)
    '        arrExcluded.Add(clsUserMgtCode.frmLabelPrinting)
    '        arrExcluded.Add(clsUserMgtCode.frmPF_Covering_Letter)
    '        arrExcluded.Add(clsUserMgtCode.frmPF_Covering_Letter)
    '        arrExcluded.Add(clsUserMgtCode.frmBankStatement_Reports)
    '        arrExcluded.Add(clsUserMgtCode.frmSalaryCertificate)
    '        arrExcluded.Add(clsUserMgtCode.frmESICRpt)
    '        arrExcluded.Add(clsUserMgtCode.frmNewSalCertificate)
    '        arrExcluded.Add(clsUserMgtCode.RptBOILetterReport)
    '        arrExcluded.Add(clsUserMgtCode.rptMilkPaymentRegister)
    '        arrExcluded.Add(clsUserMgtCode.rptCollectionCenterChart)
    '        arrExcluded.Add(clsUserMgtCode.rptCollectionLevelChart)
    '        arrExcluded.Add(clsUserMgtCode.CustomerDetails)
    '        arrExcluded.Add(clsUserMgtCode.mbtnCustomerEmptyTrial)
    '        '' TDS EXCLUSION -Master
    '        arrExcluded.Add(clsUserMgtCode.FinancialYear)
    '        arrExcluded.Add(clsUserMgtCode.BranchDetails)
    '        arrExcluded.Add(clsUserMgtCode.ResponsiblePerson)
    '        arrExcluded.Add(clsUserMgtCode.StateCode)

    '        '' TDS EXCLUSION -Transaction
    '        arrExcluded.Add(clsUserMgtCode.mbtnCreateRemittance)
    '        arrExcluded.Add(clsUserMgtCode.remittanceentry)

    '        '' TDS EXCLUSION -Reports
    '        arrExcluded.Add(clsUserMgtCode.TDSForm26Q)
    '        arrExcluded.Add(clsUserMgtCode.Form16AReport)
    '        arrExcluded.Add(clsUserMgtCode.TDSSectionSummaryReport)
    '        ''Purchase Report
    '        arrExcluded.Add(clsUserMgtCode.Parti_VS_Rejected)
    '        arrExcluded.Add(clsUserMgtCode.frmPendingSaleInvoiceforChilpPO)
    '        If Not clsCommon.CompairString(objCommonVar.CurrentUserCode, "Admin") = CompairStringResult.Equal Then
    '            Dim dtNEWSC As DataTable = clsDBFuncationality.GetDataTable("select TSPL_USER_MASTER.Default_Location,TSPL_Mcc_MASTER.MCC_NAME,TSPL_MCC_MASTER.is_Reuired_Gate_Entry from TSPL_USER_MASTER  inner join TSPL_Mcc_MASTER on TSPL_Mcc_MASTER.mcc_code=Default_Location where TSPL_USER_MASTER.User_Code='" + objCommonVar.CurrentUserCode + "' and isnull( is_Reuired_Gate_Entry,0)=1")
    '            If dtNEWSC Is Nothing OrElse dtNEWSC.Rows.Count <= 0 Then
    '                arrExcluded.Add(clsUserMgtCode.MilkGateEntryIn)
    '                arrExcluded.Add(clsUserMgtCode.MilkGateEntryWeightment)
    '                arrExcluded.Add(clsUserMgtCode.MilkGateEntryOut)
    '                arrExcluded.Add(clsUserMgtCode.MilkReject)
    '            Else
    '                dtNEWSC = Nothing
    '            End If
    '        End If


    '        If Not isLoadAppIntegrator Then
    '            arrExcluded.Add(clsUserMgtCode.frmAppIntegrator)
    '        End If
    '        If Not IsLoadMccBugReports Then
    '            arrExcluded.Add("SRNWtSample")
    '            arrExcluded.Add("SAMWTSRNRPT")
    '            arrExcluded.Add("RecWtSmpRpt")
    '            arrExcluded.Add("RcpWtDifRpt")
    '        End If

    '        If Not isLoadBulkPurchaseUploader Then
    '            arrExcluded.Add(clsUserMgtCode.frmBulkPurchaseUploader)
    '        End If


    '        If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.OpenPOforRejectShortageQty, clsFixedParameterCode.OpenPOforRejectShortageQty, Nothing)) = 0 Then
    '            arrExcluded.Add(clsUserMgtCode.RptPendingPO)
    '        End If

    '        'End Of code 
    '        '-----------------------------If not Process Production then Excluded menu's---------------------------------
    '        'Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '        'If Not OpenProcessProductionBOm Then
    '        '    arrExcluded.Add(clsUserMgtCode.frmProcessProductionIssueEntry)
    '        'End If
    '        '----------------------------------------------------------------------------------------------------------------

    '        '======================exclude schedule form is po scheduling off---
    '        If clsCommon.CompairString(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.AllowPOScheduling, clsFixedParameterCode.AllowPOScheduling, Nothing)), "0") = CompairStringResult.Equal Then
    '            arrExcluded.Add(clsUserMgtCode.frmPurchaseSchedule)
    '        End If
    '        arrExcluded.Add(clsUserMgtCode.frmProductionEntryWithoutBatch)


    '        '=======================================================

    '        Dim strGrpWhrClas As String = ""
    '        Dim strReadPermission As String = ""
    '        If blnShowAllMenu = False Then
    '            strReadPermission = "TSPL_GROUP_PROGRAM_MAPPING.Read_Flag=1 and "
    '        End If
    '        If Not clsCommon.CompairString(objCommonVar.CurrentUserCode, "Admin") = CompairStringResult.Equal Then
    '            strGrpWhrClas += " and exists(select 1 from TSPL_GROUP_PROGRAM_MAPPING where " & strReadPermission & " TSPL_GROUP_PROGRAM_MAPPING.Program_Code=TSPL_PROGRAM_MASTER.Program_Code and TSPL_GROUP_PROGRAM_MAPPING.Group_Code in (select Group_Code  from TSPL_USER_GROUP_MAPPING where User_Code='" + objCommonVar.CurrentUserCode + "')) " + Environment.NewLine
    '        End If
    '        '===========Updated by rohit on may 27,2014. form will display according to module permission ===========
    '        Dim sQuery As String = "select Module_Name from TSPL_MODULE_PERMISSION"
    '        Dim dtmodule As DataTable = clsDBFuncationality.GetDataTable(sQuery)
    '        For Each rowModule As DataRow In dtmodule.Rows()
    '            If arrExcluded.Contains(rowModule("Module_Name")) Then
    '                arrExcluded.Remove(rowModule("Module_Name"))
    '            End If
    '        Next

    '        If clsCommon.CompairString(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.INDUSTRYTYPE, clsFixedParameterCode.INDUSTRYTYPE, Nothing)), "A") <> CompairStringResult.Equal Then
    '            'arrExcluded.Add(clsUserMgtCode.frmPartNoMaster)
    '            arrExcluded.Add(clsUserMgtCode.ModuleServiceAndWarranty)
    '        End If

    '        If clsCommon.CompairString(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.AllowQualityModuleInERP, clsFixedParameterCode.AllowQualityModuleInERP, Nothing)), "1") <> CompairStringResult.Equal Then
    '            arrExcluded.Add(clsUserMgtCode.ModuleQualityControl)
    '        End If
    '        arrExcluded.Add(clsUserMgtCode.frmPaySlip_Reports)
    '        arrExcluded.Add(clsUserMgtCode.rptFromNO21)
    '        arrExcluded.Add(clsUserMgtCode.rptFARReport)
    '        ' arrExcluded.Add(clsUserMgtCode.FrmItemTypeMaster)
    '        arrExcluded.Add(clsUserMgtCode.rptVLCwiseTPTimeTable)

    '        Dim qry As String = ""

    '        '' Ticket NO TEC/16/03/18-000101 for Module screen wise rights
    '        If EnableScreenSelection = True Then
    '            qry = "select distinct tt.* from (select sno AS [SERNO],Program_Code,Name,Parent_Code,Counted from (" + Environment.NewLine '"select Program_Code,Name,Parent_Code from (" + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,null as Counted from TSPL_PROGRAM_MASTER " + Environment.NewLine
    '            qry += " where 2=2 and  Parent_Code is null " + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,null as Counted from TSPL_PROGRAM_MASTER " + Environment.NewLine


    '            qry += " where 2=2 and  Type In ('SM') and Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where 2=2 " + strGrpWhrClas + " )" + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,null as Counted from TSPL_PROGRAM_MASTER " + Environment.NewLine


    '            qry += " where 2=2 and  Type In ('M') and Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where 2=2 " + strGrpWhrClas + "))" + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,null as Counted from TSPL_PROGRAM_MASTER " + Environment.NewLine
    '            qry += " where Program_Code='" + clsUserMgtCode.ModuleFavourite + "' " + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            If EnableScreenSelection = True Then

    '                Dim strCodeColumn As String = ""

    '                Dim qry1 As String = "select distinct P_Code from TSPL_MODULE_SCREEN_PERMISSION "
    '                Dim dt1 As DataTable = clsDBFuncationality.GetDataTable(qry1)
    '                For ii As Integer = 0 To dt1.Rows.Count - 1
    '                    If ii <> 0 Then
    '                        strCodeColumn += "','"
    '                    End If
    '                    strCodeColumn += "" + clsCommon.myCstr(dt1.Rows(ii)("P_Code")).Trim() + ""
    '                Next


    '                qry += "  select * from "
    '                qry += " (  select  Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,case when TSPL_MODULE_SCREEN_PERMISSION.Screen_Name=TSPL_PROGRAM_MASTER.Program_Code then 1 else 0 end as Counted from TSPL_PROGRAM_MASTER"
    '                qry += " left join TSPL_MODULE_SCREEN_PERMISSION on TSPL_MODULE_SCREEN_PERMISSION.Screen_Name=TSPL_PROGRAM_MASTER.Program_Code where Parent_Code in ('" & strCodeColumn & "') "
    '                qry += " union select  Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo,null Counted from TSPL_PROGRAM_MASTER"
    '                qry += " where Program_Code not in (select Screen_Name from TSPL_MODULE_SCREEN_PERMISSION) and Parent_Code not in ('" & strCodeColumn & "')  "
    '                qry += "  )xx "

    '            End If



    '            qry += " union all " + Environment.NewLine
    '            qry += " select TSPL_FAVOURITE_MENU.Program_Code,case when LEN(isnull(TSPL_PROGRAM_MASTER.Re_Name,''))>0 then TSPL_PROGRAM_MASTER.Re_Name else TSPL_PROGRAM_MASTER.Program_Name end as Name,'" + clsUserMgtCode.ModuleFavourite + "' as Parent_Code,TSPL_FAVOURITE_MENU.SNo,null as Counted from TSPL_FAVOURITE_MENU " + Environment.NewLine
    '            qry += " left outer join  TSPL_PROGRAM_MASTER on TSPL_PROGRAM_MASTER.Program_Code= TSPL_FAVOURITE_MENU.Program_Code  where 2=2 and TSPL_FAVOURITE_MENU.User_Code='" + objCommonVar.CurrentUserCode + "' " + strGrpWhrClas + Environment.NewLine
    '            qry += " )xxx where 2=2 "
    '            qry += " and Program_Code not in (" + clsCommon.GetMulcallString(arrExcluded) + ")"
    '            qry += ") tt inner join (select Module_Name,Program_Code as [prg_Code] from tspl_Program_Master tpm inner join tspl_Module_Permission tmm on " _
    '            & " tpm.Parent_Code=tmm.Module_Name union select 'MFavourite','MFavourite' " & IIf(isUtilityAdded, " union select Program_Code as [Module_Name],Program_Code as [prg_Code] from tspl_Program_Master where Parent_Code ='Mutility'", "") & ") " _
    '            & " tpm on (tpm.module_Name=Parent_Code or tpm.prg_Code=Parent_Code or tpm.module_Name =Program_Code  or Parent_Code is NULL  or Parent_Code ='ExpertERP') " _
    '            & " and Program_Code not in (select distinct Program_Code as [prg_Code] from tspl_Program_Master tpm Left join tspl_Module_Permission tmm on tpm.Program_Code=tmm.Module_Name where Type='M' and module_Name is null " & IIf(isUtilityAdded, " and Program_Code <>'Mutility'", "") & ") and (Counted is  null or Counted=1) order by SERNO" '" order by SNo"

    '        Else

    '            qry = "select distinct tt.* from (select sno AS [SERNO],Program_Code,Name,Parent_Code from (" + Environment.NewLine '"select Program_Code,Name,Parent_Code from (" + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo from TSPL_PROGRAM_MASTER " + Environment.NewLine
    '            qry += " where 2=2 and  Parent_Code is null " + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo from TSPL_PROGRAM_MASTER " + Environment.NewLine


    '            qry += " where 2=2 and  Type In ('SM') and Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where 2=2 " + strGrpWhrClas + " )" + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo from TSPL_PROGRAM_MASTER " + Environment.NewLine


    '            qry += " where 2=2 and  Type In ('M') and Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where Program_Code in (select distinct Parent_Code from TSPL_PROGRAM_MASTER where 2=2 " + strGrpWhrClas + "))" + Environment.NewLine
    '            qry += " union " + Environment.NewLine
    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo from TSPL_PROGRAM_MASTER " + Environment.NewLine
    '            qry += " where Program_Code='" + clsUserMgtCode.ModuleFavourite + "' " + Environment.NewLine
    '            qry += " union " + Environment.NewLine

    '            qry += " select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as Name,Parent_Code,SNo from TSPL_PROGRAM_MASTER "
    '            qry += " where 2=2 " + strGrpWhrClas + Environment.NewLine

    '            qry += " union all " + Environment.NewLine
    '            qry += " select TSPL_FAVOURITE_MENU.Program_Code,case when LEN(isnull(TSPL_PROGRAM_MASTER.Re_Name,''))>0 then TSPL_PROGRAM_MASTER.Re_Name else TSPL_PROGRAM_MASTER.Program_Name end as Name,'" + clsUserMgtCode.ModuleFavourite + "' as Parent_Code,TSPL_FAVOURITE_MENU.SNo from TSPL_FAVOURITE_MENU " + Environment.NewLine
    '            qry += " left outer join  TSPL_PROGRAM_MASTER on TSPL_PROGRAM_MASTER.Program_Code= TSPL_FAVOURITE_MENU.Program_Code  where 2=2 and TSPL_FAVOURITE_MENU.User_Code='" + objCommonVar.CurrentUserCode + "' " + strGrpWhrClas + Environment.NewLine
    '            qry += " )xxx where 2=2 "
    '            qry += " and Program_Code not in (" + clsCommon.GetMulcallString(arrExcluded) + ")"
    '            qry += ") tt inner join (select Module_Name,Program_Code as [prg_Code] from tspl_Program_Master tpm inner join tspl_Module_Permission tmm on " _
    '            & " tpm.Parent_Code=tmm.Module_Name union select 'MFavourite','MFavourite' " & IIf(isUtilityAdded, " union select Program_Code as [Module_Name],Program_Code as [prg_Code] from tspl_Program_Master where Parent_Code ='Mutility'", "") & ") " _
    '            & " tpm on (tpm.module_Name=Parent_Code or tpm.prg_Code=Parent_Code or tpm.module_Name =Program_Code  or Parent_Code is NULL  or Parent_Code ='ExpertERP') " _
    '            & " and Program_Code not in (select distinct Program_Code as [prg_Code] from tspl_Program_Master tpm Left join tspl_Module_Permission tmm on tpm.Program_Code=tmm.Module_Name where Type='M' and module_Name is null " & IIf(isUtilityAdded, " and Program_Code <>'Mutility'", "") & ") order by SERNO" '" order by SNo"
    '        End If

    '        '' End
    '        '============================================
    '        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)


    '        RTV2.DataSource = Nothing
    '        RTV2.TreeViewElement.AutoSizeItems = True
    '        RTV2.ShowLines = True
    '        RTV2.ShowRootLines = True
    '        RTV2.TreeViewElement.ViewElement.Margin = New Padding(4)
    '        RTV2.ShowExpandCollapse = True
    '        RTV2.TreeIndent = 15
    '        RTV2.FullRowSelect = False
    '        RTV2.ShowLines = True
    '        RTV2.LineStyle = TreeLineStyle.Dot
    '        RTV2.LineColor = Color.FromArgb(110, 153, 210)
    '        RTV2.ExpandAnimation = ExpandAnimation.Opacity
    '        RTV2.AllowEdit = False
    '        RTV2.ShowRootLines = False
    '        'RTV2.TreeViewElement.AllowAlternatingRowColor = True
    '        'RTV2.TreeViewElement.AlternatingRowColor = Color.AliceBlue
    '        'RTV2.TreeViewElement.AngleTransform = 270
    '        'RTV2.TreeViewElement.RightToLeft = True
    '        'RTV2.TreeViewElement.DrawBorder = True
    '        RTV2.ValueMember = "Program_Code"
    '        RTV2.DisplayMember = "Name"
    '        RTV2.ChildMember = "Program_Code"
    '        RTV2.ParentMember = "Parent_Code"
    '        RTV2.DataSource = dt

    '        LoadMenuInCombo()
    '        ' Set Image
    '        For i As Integer = 0 To RTV2.Nodes.Count - 1
    '            SetImage(RTV2.Nodes(i))
    '        Next
    '        RTV2.Nodes.Add("")
    '        RTV2.Nodes.Add("")
    '        RTV2.Nodes.Add("")
    '        RTV2.AllowEdit = False
    '    Catch ex As Exception
    '        clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
    '    End Try

    '    RTV2.CollapseAll()
    '    If RTV2.Nodes.Count > 0 Then
    '        RTV2.Nodes(0).Expand()
    '    End If

    'End Sub

    Protected Sub SetImage(ByVal subRoot As RadTreeNode)
        ' check for null (this can be removed since within th
        If (subRoot Is Nothing) Then
            Exit Sub
        End If
        If ArrImageList.ContainsKey(clsCommon.myCstr(subRoot.Value)) Then
            subRoot.Image = ImageList1.Images.Item(ArrImageList(clsCommon.myCstr(subRoot.Value)))
        End If
        ' add all it's children
        For i As Integer = 0 To subRoot.Nodes.Count - 1
            SetImage(subRoot.Nodes(i))
        Next
    End Sub

    Public Sub LoadImageList()
        ArrImageList.Clear()
        Dim qry As String = "select Program_Code,Image_Number from TSPL_PROGRAM_MASTER"
        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
        For Each dr As DataRow In dt.Rows
            ArrImageList.Add(clsCommon.myCstr(dr("Program_Code")), clsCommon.myCdbl(dr("Image_Number")))
        Next

        qry = "select Program_Code  from TSPL_PROGRAM_MASTER where Parent_Code is null or Type in ('M')"
        dt = clsDBFuncationality.GetDataTable(qry)
        For Each dr As DataRow In dt.Rows
            ArrBold.Add(clsCommon.myCstr(dr("Program_Code")))
        Next
    End Sub

    Sub LoadMenuInCombo()
        'GC.Collect()
        Try
            If clsCommon.myLen(clsDBFuncationality.connectionString) > 0 Then
                Dim strGrpWhrClas As String = ""
                Dim strReadPermission As String = ""
                If blnShowAllMenu = False Then
                    strReadPermission = "TSPL_GROUP_PROGRAM_MAPPING.Read_Flag=1 and "
                End If
                If Not clsCommon.CompairString(objCommonVar.CurrentUserCode, "Admin") = CompairStringResult.Equal Then
                    strGrpWhrClas += " and exists(select 1 from TSPL_GROUP_PROGRAM_MAPPING where " & strReadPermission & " TSPL_GROUP_PROGRAM_MAPPING.Program_Code=TSPL_PROGRAM_MASTER.Program_Code and TSPL_GROUP_PROGRAM_MAPPING.Group_Code in (select Group_Code  from TSPL_USER_GROUP_MAPPING where User_Code='" + objCommonVar.CurrentUserCode + "')) " + Environment.NewLine
                End If
                Dim qry As String = "select Program_Code,case when LEN(isnull(Re_Name,''))>0 then Re_Name else Program_Name end as PROGRAM_NAME from TSPL_PROGRAM_MASTER  inner join (select Module_Name,Program_Code as [prg_Code] from tspl_Program_Master tpm inner join tspl_Module_Permission tmm on tpm.Parent_Code=tmm.Module_Name" _
                & " union select 'MFavourite','MFavourite' " & IIf(isUtilityAdded, "Union select Program_Code as [Module_Name],Program_Code as [prg_Code] from tspl_Program_Master where Parent_Code ='Mutility'", "") & ") tmm on tspl_Program_Master.Parent_Code=tmm.prg_Code where 2=2 and  TSPL_PROGRAM_MASTER.Program_Code not in (" + Environment.NewLine
                qry += " select Program_Code from TSPL_PROGRAM_MASTER where Parent_Code in (select Program_Code from TSPL_PROGRAM_MASTER where Parent_Code in (select Program_Code from TSPL_PROGRAM_MASTER as innerProgramMaster where innerProgramMaster.Program_Code in (" + clsCommon.GetMulcallString(arrExcluded) + ") and Type='M') and Type='SM')"
                qry += " union "
                qry += " select Program_Code from TSPL_PROGRAM_MASTER where Program_Code in (" + clsCommon.GetMulcallString(arrExcluded) + ") and type=''"

                qry += " )  " + strGrpWhrClas + " and Type Not in ('M','SM')  and Parent_Code is not null   order by PROGRAM_NAME "
                Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
                Dim dr As DataRow = dt.NewRow()
                dr("Program_Code") = Nothing
                dr("PROGRAM_NAME") = Nothing
                dt.Rows.InsertAt(dr, 0)
                cboMenu.DataSource = dt
                cboMenu.ValueMember = "Program_Code"
                cboMenu.DisplayMember = "PROGRAM_NAME"
                cboMenu.SelectedIndex = 0
                cboMenu.DropDownListElement.AutoCompleteSuggest.SuggestMode = SuggestMode.Contains
            End If
        Catch ex As Exception

        End Try
        cboMenu.NullText = "Quick Menu"

    End Sub

    Private Sub MDI_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        
    End Sub


    Private Sub cboMenu_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboMenu.KeyDown
        Try

            If clsCommon.myLen(clsCommon.myCstr(cboMenu.SelectedValue)) > 0 AndAlso clsCommon.myLen(clsCommon.myCstr(cboMenu.Text)) > 0 Then
                If e.KeyCode = Keys.Enter Then
                    ShowForm(clsCommon.myCstr(cboMenu.SelectedValue), clsCommon.myCstr(cboMenu.SelectedText), True)
                    RTV2.CollapseAll()
                    RTV2.Nodes(0).Expand()
                    Try
                        RTV2.SelectedNode = RTV2.Nodes(0)
                        RTV2.SelectedNode = RTV2.Find(cboMenu.SelectedText)
                        RTV2.SelectedNode.Expand()
                    Catch ex As Exception
                    End Try
                End If
            Else
                cboMenu.SelectedIndex = 0
            End If

        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Private Sub RTV2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RTV2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If RTV2.SelectedNode IsNot Nothing Then
                Dim strCode As String = clsCommon.myCstr(RTV2.SelectedNode.Value)
                If clsCommon.myLen(strCode) > 0 Then
                    ShowForm(strCode, clsCommon.myCstr(RTV2.SelectedNode.Text), True)
                End If
            End If
        End If
    End Sub

    Public Sub ShowForm(ByVal strProgramCode As String, ByVal strProgramName As String, ByVal isOpenInMDI As Boolean)
        'ShowForm(strProgramCode, strProgramName, isOpenInMDI, "")
    End Sub
    'Public Sub ShowForm(ByVal strProgramCode As String, ByVal strProgramName As String, ByVal isOpenInMDI As Boolean, ByVal strDocNo As String, Optional ByVal IFTrueShowFormElseShowDialog As Boolean = True, Optional ByVal IsAllowModificationByApprovalUser As Boolean = False)
    '    GC.Collect()

    '    If Not strProgramCode Is Nothing Then
    '        strProgramName = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select case when LEN(ISNULL(Re_Name,''))>0 then Re_Name else Program_Name end as Program_Name from TSPL_PROGRAM_MASTER where Program_Code='" + strProgramCode + "'"))

    '        Dim qry As String = " select * from tspl_Program_master where Program_code='" & strProgramCode & "'"

    '        Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)

    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            Dim IsRunFromOtherAsm As Integer = clsCommon.myCdbl(dt.Rows(0)("IsLoadFromOtherAssembly"))
    '            If IsRunFromOtherAsm = 1 Then
    '                Dim FormName As String = clsCommon.myCstr(dt.Rows(0)("FormName"))
    '                Dim AsmName As String = clsCommon.myCstr(Application.StartupPath & "\" & dt.Rows(0)("OtherAssemblyFilePathAndName"))
    '                Dim AsmToLoad As Assembly = Nothing
    '                Dim obj As Object = Nothing
    '                AsmToLoad = Assembly.LoadFile(AsmName)
    '                Dim classType As Type = AsmToLoad.[GetType](FormName)
    '                'obj = AsmToLoad.CreateInstance("ERP." & FormName, True)
    '                ' Dim M As Assembly.Module = AsmToLoad.FrmMainTranScreen
    '                obj = AsmToLoad.CreateInstance(FormName, True)
    '                Dim frm As RadForm = TryCast(obj, RadForm)
    '                If isOpenInMDI Then
    '                    frm.MdiParent = Me
    '                    frm.Text = strProgramName
    '                    frm.Show()
    '                Else
    '                    If clsCommon.myLen(strDocNo) > 0 Then
    '                        frm.Tag = strDocNo
    '                    End If
    '                    frm.WindowState = FormWindowState.Maximized
    '                    frm.Text = strProgramName
    '                    frm.ShowDialog()
    '                End If
    '                Exit Sub
    '            End If
    '        End If
    '        '------------------ Common services Masters---------------------------------------
    '        If clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCompanyMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCompanyMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        End If

    '        If clsCommon.CompairString(strProgramCode, clsUserMgtCode.CostCenter) = CompairStringResult.Equal Then
    '            frm = New FrmCostCenter(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        End If
    '        If clsCommon.CompairString(strProgramCode, clsUserMgtCode.CostFACenter) = CompairStringResult.Equal Then
    '            frm = New FrmFACostCenter(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        End If
    '        If clsCommon.CompairString(strProgramCode, clsUserMgtCode.ReverseEntry) = CompairStringResult.Equal Then
    '            frm = New FrmReverseEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        End If
    '        If clsCommon.CompairString(strProgramCode, clsUserMgtCode.cityMaster) = CompairStringResult.Equal Then
    '            frm = New frmCityMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.taxAuthority) = CompairStringResult.Equal Then
    '            frm = New frmTaxAuthority(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.taxRate) = CompairStringResult.Equal Then
    '            frm = New FrmTaxRates(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.taxGroup) = CompairStringResult.Equal Then
    '            frm = New FrmTaxGroups(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.paymentTerms) = CompairStringResult.Equal Then
    '            frm = New frmPaymentTerms(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.paymentCodes) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.employeeMaster) = CompairStringResult.Equal  Then
    '            '    frm=New frmEmployeeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.designationMaster) = CompairStringResult.Equal Then
    '            '    frm=New frmDesignationMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.bankMaster) = CompairStringResult.Equal Then
    '            frm = New frmBankMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.bankBranchMaster) = CompairStringResult.Equal Then
    '            frm = New FrmBankBrachMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmNotificationScreen) = CompairStringResult.Equal Then
    '            frm = New FrmNotificationScreen
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ChangePwd) = CompairStringResult.Equal Then
    '            frm = New FrmChangePassword()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAbateMentMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAbateMentMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PrefixGeneration) = CompairStringResult.Equal Then
    '            Dim frm1 As New FrmPrefixGenerationNew()
    '            formShow(frm1, strProgramCode, strProgramName, True, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FormMaster) = CompairStringResult.Equal Then
    '            frm = New FrmFormMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFormSerialNoMaster) = CompairStringResult.Equal Then
    '            frm = New FrmFormSerialNoMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAdditionalCharges) = CompairStringResult.Equal Then
    '            frm = New FrmAdditionalCharges(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf strProgramCode = "FrmChangePassword" Then
    '            frm = New FrmChangePassword()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCurrencyConversion) = CompairStringResult.Equal Then
    '            frm = New frmCurrencyConversion
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomFieldMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCustomFieldMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomFieldMapping) = CompairStringResult.Equal Then
    '            frm = New frmCustomFieldMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmModuleCurrencyMapping) = CompairStringResult.Equal Then
    '            frm = New frmModuleCurrencyMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CommonServicesSetting) = CompairStringResult.Equal Then
    '            frm = New frmCommonServicesSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRegionMaster) = CompairStringResult.Equal Then
    '            frm = New FrmRegionMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCountryMaster1) = CompairStringResult.Equal Then
    '            frm = New frmCountryMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStateMaster1) = CompairStringResult.Equal Then
    '            frm = New frmStateMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DistrictMaster) = CompairStringResult.Equal Then
    '            frm = New frmDistrictMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AreaMaster) = CompairStringResult.Equal Then
    '            frm = New frmAreaMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPromptMsgRelatedtopending) = CompairStringResult.Equal Then
    '            frm = New FrmPromptMsgRelatedToPendency
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSaleReco) = CompairStringResult.Equal Then
    '            frm = New rptSaleRecoNew
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'Re-added by stuti on my computer ---
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPurReco) = CompairStringResult.Equal Then
    '            frm = New rptPurchaseReco
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVendorReco) = CompairStringResult.Equal Then
    '            frm = New rptVendReco
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCustomerReco) = CompairStringResult.Equal Then
    '            frm = New rptCustomerReco
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptTransporterProvisionReport) = CompairStringResult.Equal Then
    '            frm = New frmRptTransporterProvision()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBranchAccountMapping) = CompairStringResult.Equal Then
    '            frm = New FrmBranchAccountMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMCCDiscountMaster) = CompairStringResult.Equal Then
    '            frm = New FrmDiscountMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "CaLSCRN") = CompairStringResult.Equal Then
    '            System.Diagnostics.Process.Start("calc.exe")

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBranchAccountMapping) = CompairStringResult.Equal Then
    '            frm = New RptBranchAccountMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLockTransactionReport) = CompairStringResult.Equal Then
    '            frm = New FrmLockTransactionReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '------------------ Common services Transactions---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.bankTransfer) = CompairStringResult.Equal Then
    '            frm = New FrmBankTransfer(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf strProgramCode = "bankEntry" Then

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.reverseTransaction) = CompairStringResult.Equal Then
    '            frm = New frmReverseTransaction(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankBook) = CompairStringResult.Equal Then
    '            frm = New FrmBankBook(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankBookLocationDetail) = CompairStringResult.Equal Then
    '            frm = New FrmBankBookLocationDetail(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankBookChart) = CompairStringResult.Equal Then
    '            frm = New FrmBankBookChart()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankBookClosing) = CompairStringResult.Equal Then
    '            frm = New FrmBankBookClosing()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCustomerAgingSummary) = CompairStringResult.Equal Then
    '            frm = New FrmBICustomerAgeing()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorAgingSummary) = CompairStringResult.Equal Then
    '            frm = New FrmBIVendorAgeing()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFormCollection) = CompairStringResult.Equal Then
    '            frm = New FrmFormCollection()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLoadReport) = CompairStringResult.Equal Then
    '            frm = New FrmLoadReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLoadOutInvoiceRecoReport) = CompairStringResult.Equal Then
    '            frm = New FrmLoadOutInvoiceRecoReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCFormEntry) = CompairStringResult.Equal Then
    '            frm = New FrmCFormEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBankGuaranteeMaster1) = CompairStringResult.Equal Then
    '            frm = New FrmBankGuaranteeMaster1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BankOpeningReco) = CompairStringResult.Equal Then
    '            frm = New frmBankOpeningReco
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '-------------------Common Services Report -----------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DVAT30) = CompairStringResult.Equal Then
    '            frm = New FrmDVAT30()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DVAT31) = CompairStringResult.Equal Then
    '            frm = New FrmDVAT31()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDetailsOfForm2B) = CompairStringResult.Equal Then
    '            frm = New FrmDetailsOfForm2B()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCashVoucher) = CompairStringResult.Equal Then
    '            frm = New FrmCashVoucher
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TaxTracking) = CompairStringResult.Equal Then
    '            frm = New FrmTaxTracking
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FormIssue) = CompairStringResult.Equal Then
    '            frm = New FrmFormIssueDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBankReconcilliation) = CompairStringResult.Equal Then
    '            frm = New RptBankReconcilliation
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RevaluationEntry) = CompairStringResult.Equal Then
    '            frm = New frmRevaluationEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------Receivables---------------------------------------
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ShiptoLocation) = CompairStringResult.Equal Then
    '            '    frm=New frmShipToLocation(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmShipToLocationDetails) = CompairStringResult.Equal Then
    '            frm = New FrmShipToLocationDetails(lblUser.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmReceivablePaymentTerms) = CompairStringResult.Equal Then
    '            frm = New FrmReceivablePaymentTerms
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmReceivableSettings) = CompairStringResult.Equal Then
    '            frm = New FrmReceivableSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerType) = CompairStringResult.Equal Then
    '            frm = New frmCustomerType(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomeCategory) = CompairStringResult.Equal Then
    '            frm = New frmCustomerCategory(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.mbtnCustomerInfo) = CompairStringResult.Equal Then
    '            '    frm=New FrmCustomerInfo(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerMaster) = CompairStringResult.Equal Then
    '            frm = New frmCustomer(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SecondaryCustomerMaster) = CompairStringResult.Equal Then
    '            frm = New FrmSecondaryCustomerMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPOSGRoupMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPOSGRoupMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerGroup) = CompairStringResult.Equal Then
    '            frm = New frmCustomerGroup(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerAccountSet) = CompairStringResult.Equal Then
    '            frm = New frmCustomerAccountSet(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-----------------Receivables Transactions----------

    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.ReceiptEntry) = CompairStringResult.Equal Then
    '            '    'Xtra.UpdateSaleInvoiceBalanceAmt()
    '            '    frm=New FrmReceiptNew(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ReceiptEntry) = CompairStringResult.Equal Then
    '            frm = New FrmReceipttNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FinanceAdjustment) = CompairStringResult.Equal Then
    '            '    frm = New frmAdj()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCustomersSetOff) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerSetOff()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCustomersSetOff) = CompairStringResult.Equal Then
    '            frm = New RptCustomerSetOff()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ReceiptAdjustmentEntry) = CompairStringResult.Equal Then
    '            frm = New frmAdj()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnARInvoiceEntry) = CompairStringResult.Equal Then
    '            frm = New FrmARInvoiceEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQuickBook) = CompairStringResult.Equal Then
    '            frm = New FrmQuickEntry1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBankReco) = CompairStringResult.Equal Then
    '            frm = New FrmBankReco(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCustomerInquiry) = CompairStringResult.Equal Then
    '            'Xtra.UpdateSaleInvoiceBalanceAmt()
    '            frm = New FrmCustomerInquiry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-----------------Receivables Reports----------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerGroupReport) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerGroupReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerDetails) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerMasterReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.SaleRegister) = CompairStringResult.Equal Then
    '            ''    frm=New FrmRptSales()
    '            ''         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCustomerLedger) = CompairStringResult.Equal Then
    '            frm = New FrmRptCustomerLedgerDemo(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRoute_CustomerOutstanding) = CompairStringResult.Equal Then
    '            frm = New FrmRoute_CustomerOutStanding()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerAgeing) = CompairStringResult.Equal Then
    '            'If objCommonVar.IsDemoERP Then
    '            '    'frm=New FrmCustomerAgingDEMO()
    '            '    '     formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '    frm = New rptCustomerAgeingDrillDown(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'Else
    '            '    frm = New FrmCustomerAgeing()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'End If
    '            frm = New rptCustomerAgeingDrillDown(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomersListReport) = CompairStringResult.Equal Then
    '            frm = New frmCustomerListRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RouteListReport11) = CompairStringResult.Equal Then
    '            frm = New frmRouteListReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCustomerEmptyTrial) = CompairStringResult.Equal Then  ''Added By Pankaj
    '            frm = New FrmCustomerEmptyTrial2()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSecurityLevel) = CompairStringResult.Equal Then
    '            frm = New FrmSecurityLevel
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVendorSecurity) = CompairStringResult.Equal Then
    '            frm = New FrmVendorSecurity
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCarteJaliRpt) = CompairStringResult.Equal Then
    '            frm = New FrmCrateJaliReport()

    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCrateJalliReportForTransfer) = CompairStringResult.Equal Then
    '            frm = New RptCrateJalliBoxTransferDS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmExciseChapterWise) = CompairStringResult.Equal Then  ''Added By Manoj

    '            If objCommonVar.IsDemoERP Then
    '                frm = New frmER1Demo()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmExciseChapterWise()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If


    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSecurityDeposit1) = CompairStringResult.Equal Then  ''Added By Manoj
    '            '    frm = New FrmSecurityDeposit1()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBankReverse) = CompairStringResult.Equal Then  ''Added By Abhishek  as on 27 Nov 2012
    '            frm = New FrmBankReverse()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCustomerOutstanding) = CompairStringResult.Equal Then
    '            '    frm = New FrmCustomerOutstanding()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptQualityStatus) = CompairStringResult.Equal Then
    '            frm = New RptQualityStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '------------------ GL Masters---------------------------------------

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.glOptions) = CompairStringResult.Equal Then
    '            frm = New frmgloption(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.segmentCode) = CompairStringResult.Equal Then
    '            frm = New Frmsegmentcode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.accountStructure) = CompairStringResult.Equal Then
    '            frm = New frmGLStructure(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.accountGroup) = CompairStringResult.Equal Then
    '            frm = New frmAccountGroup(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.glAccount) = CompairStringResult.Equal Then
    '            frm = New frmGLAccount(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.createAccounts) = CompairStringResult.Equal Then
    '            frm = New frmCreateAccountNew(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.sourceCode) = CompairStringResult.Equal Then
    '            frm = New FrmSourceCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.glsecurity) = CompairStringResult.Equal Then
    '            frm = New Frmglsecurity(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBalanceSheetPerforma) = CompairStringResult.Equal Then
    '            frm = New FrmBalanceSheetPerforma1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGL_account_excluded) = CompairStringResult.Equal Then
    '            frm = New FrmGL_account_excluded()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGLControlAccountMapping) = CompairStringResult.Equal Then
    '            frm = New frmGLControlAccountMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frm_Account_Mapping) = CompairStringResult.Equal Then
    '            Dim fmr As New Frm_Account_Mapping()
    '            formShow(fmr, strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMapLedgerAccToTally) = CompairStringResult.Equal Then
    '            frm = New frmMapLedgerAccToTally()
    '            If objCommonVar.IsSendToTally Then
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPostAllGLToTally) = CompairStringResult.Equal Then
    '            frm = New frmPostAllGLToTally()
    '            If objCommonVar.IsSendToTally Then
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FiscalYear) = CompairStringResult.Equal Then
    '            frm = New frmFinancialYearMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CostCentreFinancial) = CompairStringResult.Equal Then
    '            frm = New FrmCostCentreFinancial()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AccountMainGroup) = CompairStringResult.Equal Then
    '            frm = New FrmAccountMainGroup()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AccountSubGroup) = CompairStringResult.Equal Then
    '            frm = New FrmAccountSubGroup()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AccountGLMainAccount) = CompairStringResult.Equal Then
    '            frm = New frmAccountMainGLAccount()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ GL Transactions---------------------------------------

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.journalEntry) = CompairStringResult.Equal Then
    '            frm = New frmJournalEntry(lblUserCode.Text, lblCompanyCode.Text, strDocNo, clsUserMgtCode.journalEntry)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ReversejournalEntry) = CompairStringResult.Equal Then
    '            frm = New frmJournalEntry(lblUserCode.Text, lblCompanyCode.Text, strDocNo, clsUserMgtCode.ReversejournalEntry)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnVCGLEntry) = CompairStringResult.Equal Then
    '            frm = New frmVCGLEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJEReverse) = CompairStringResult.Equal Then
    '            frm = New FrmJEReverse()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSettingDetails) = CompairStringResult.Equal Then
    '            frm = New frmSettingDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '------------------ GL Transactions Report---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCostCenterAnalysisRpt) = CompairStringResult.Equal Then
    '            frm = New FrmCostCenterAnalysisRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGLTransReport) = CompairStringResult.Equal Then
    '            frm = New GLTransReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptChartOfAccount) = CompairStringResult.Equal Then
    '            frm = New RptChartOfAccount()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJrnlVoucher) = CompairStringResult.Equal Then
    '            frm = New JrnlVoucherReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf strFormName = "Trial Balance Report" Then
    '            '    frm=New frmTrialBalanceReport(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JECheckSystem) = CompairStringResult.Equal Then
    '            frm = New rptJECheck()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptTrialBalance) = CompairStringResult.Equal Then
    '            frm = New frmRptTrialBalanceNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptTrialBalanceCV) = CompairStringResult.Equal Then
    '            frm = New frmRptTrialBalanceVC()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnJournalBook) = CompairStringResult.Equal Then
    '            frm = New frmRptDayWiseJournalBook()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptBalanceSheet) = CompairStringResult.Equal Then
    '            frm = New frmRptBalanceSheet()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankBookDayWise) = CompairStringResult.Equal Then
    '            frm = New FrmBankBookDayWise()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Added By abhishek as on 26/11/2012
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUnpostedJV) = CompairStringResult.Equal Then
    '            frm = New FrmUnpostedJV()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Code End 
    '            '------------------ Administrative Services---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmployeeMaster) = CompairStringResult.Equal Then
    '            frm = New frmEmployeeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ' KUNAL > TICKET : BM00000009879 > 30 - SEP - 2016 
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DesignationMaster) = CompairStringResult.Equal Then
    '            frm = New frmDesignationMaster(lblUserCode.Text, lblCompanyCode.Text, clsUserMgtCode.DesignationMaster)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DesignationMasterHierarchy) = CompairStringResult.Equal Then
    '            frm = New frmDesignationHierarchyMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.userMaster) = CompairStringResult.Equal Then
    '            frm = New FrmUserMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TimeTable) = CompairStringResult.Equal Then
    '            frm = New frmTimeTable
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Security_Matr) = CompairStringResult.Equal Then
    '            frm = New RptSecurityMatrix
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDocumentVersionReport) = CompairStringResult.Equal Then
    '            frm = New FrmDocumentVersionReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.userGroupMaster) = CompairStringResult.Equal Then
    '            frm = New FrmUserGroupMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.userGroupMapping) = CompairStringResult.Equal Then
    '            frm = New FrmUserGroupMapping(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.groupProgramMapping) = CompairStringResult.Equal Then
    '            frm = New GroupProgramMapping(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmScheduling) = CompairStringResult.Equal Then
    '            frm = New FrmScheduling()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmApprovalLevelScreen) = CompairStringResult.Equal Then
    '            frm = New frmApprovalScreen()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmApprovalAlertSumm) = CompairStringResult.Equal Then
    '            frm = New FrmApprovalAlertSumm()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLocationSetting) = CompairStringResult.Equal Then
    '            frm = New frmLocationLogin()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSynchronization) = CompairStringResult.Equal Then
    '            frm = New frmSynchronization
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FisaclYearEndProcess) = CompairStringResult.Equal Then
    '            frm = New FrmFiscalYearEndProcess
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAppIntegrator) = CompairStringResult.Equal Then
    '            frm = New frmAppIntegrator
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ Purchase Masters---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vendormaster) = CompairStringResult.Equal Then
    '            frm = New frmVendorMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.capexmaster) = CompairStringResult.Equal Then
    '            frm = New FrmCapexMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.capexbudget) = CompairStringResult.Equal Then
    '            frm = New FrmCapexBudget()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCapxRevHis) = CompairStringResult.Equal Then
    '            frm = New RptCapexBudgetRevHis
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorRegistration) = CompairStringResult.Equal Then
    '            frm = New FrmVendorReg()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmployeeBandMaster) = CompairStringResult.Equal Then
    '            frm = New FrmEmployeeBandMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TankerMasterSale) = CompairStringResult.Equal Then
    '            frm = New frmTankerMasterSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vendoraccountset) = CompairStringResult.Equal Then
    '            frm = New frmvendoraccountset(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vendorgroup) = CompairStringResult.Equal Then
    '            frm = New frmVendorGroup(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vendortype) = CompairStringResult.Equal Then
    '            frm = New frmVendorType(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRequisitionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmRequisitionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RequisitSubTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmRequisitSubTypeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHirerachyLevelMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHirerachyLevelMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Anubhooti 02-Sep-2014 
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPayableSettings) = CompairStringResult.Equal Then
    '            frm = New FrmPayableSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Anubhooti 05-Sep-2014 BM00000003755
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPaymentUploader) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentUploader
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorBankMaster) = CompairStringResult.Equal Then
    '            frm = New FrmVendorBankMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vendorSubgroup) = CompairStringResult.Equal Then
    '            frm = New frmVendorsubGroup
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '-----------------Purchase Transactions--------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PaymentEntryNew) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVendorSetOff) = CompairStringResult.Equal Then
    '            frm = New FrmVendorSetOff()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVSPCSASetOff) = CompairStringResult.Equal Then
    '            frm = New FrmVSPCSASetOff()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMultipleRTGS) = CompairStringResult.Equal Then
    '            frm = New RptMultipleRTGS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMultiplePaymentAdvice1) = CompairStringResult.Equal Then
    '            frm = New RptMultiplePaymentAdvice1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PaymentAdjustmentEntry) = CompairStringResult.Equal Then
    '            frm = New frmPaymentAdjEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnAPInvoiceEntry) = CompairStringResult.Equal Then
    '            frm = New FrmAPInvoiceEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVendorService) = CompairStringResult.Equal Then
    '            frm = New FrmVendorService()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVendorInquiry) = CompairStringResult.Equal Then
    '            frm = New FrmVendorInquiry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchaseHistory) = CompairStringResult.Equal Then
    '            frm = New FrmPurchaseHistory()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHSNMaster) = CompairStringResult.Equal Then
    '            frm = New frmHSNMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOverheadCostMaster) = CompairStringResult.Equal Then
    '            frm = New frmOverheadCostMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmOverheadCostGroup) = CompairStringResult.Equal Then
    '            frm = New FrmOverheadCostGroup()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemCostMapping) = CompairStringResult.Equal Then
    '            frm = New FrmItemCostMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSupplierReg) = CompairStringResult.Equal Then
    '            frm = New FrmSupplierReg()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmApprovedSuppliers) = CompairStringResult.Equal Then
    '            frm = New FrmApprovedSuppliers()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '----------------------------Purchase Report --------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmScrapSaleInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmScrapSaleInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaymentEntry) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnAPInvoiceReport) = CompairStringResult.Equal Then
    '            frm = New frmRptAPInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptAPInvoiceDetailsReport) = CompairStringResult.Equal Then
    '            frm = New FrmRptAPInvoiceDetailsReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptHierarchyWiseReport) = CompairStringResult.Equal Then
    '            frm = New RptHierarchyWiseReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAdvancePaymentRegister) = CompairStringResult.Equal Then
    '            frm = New FrmAdvancePaymentRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptAPReport) = CompairStringResult.Equal Then
    '            frm = New rptAPReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptARReport) = CompairStringResult.Equal Then
    '            frm = New RptARReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorLedgerReport) = CompairStringResult.Equal Then
    '            frm = New frmRptVendorLedger(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorCustomerLedgerReport) = CompairStringResult.Equal Then
    '            frm = New frmRptVendorCustomerLedger(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAgingPayble) = CompairStringResult.Equal Then
    '            frm = New rptAPAgeingDrillDown(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRptVendorList) = CompairStringResult.Equal Then
    '            frm = New FrmRptVendorList()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptVendorAgeingDetails) = CompairStringResult.Equal Then
    '            frm = New FrmRptVendorTransList()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptVendorTransList) = CompairStringResult.Equal Then
    '            frm = New FrmRptVendorTransHistory()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSrnReport) = CompairStringResult.Equal Then
    '            frm = New FrmSrnReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAgingDrillDown) = CompairStringResult.Equal Then
    '            '    frm=New rptAPAgeingDrillDown(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '            '------------------ (Material Management)Inventory Masters---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PricePlan) = CompairStringResult.Equal Then
    '            frm = New frmPriceMasterPlan()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PriceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPriceMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PriceComponentMasters) = CompairStringResult.Equal Then
    '            frm = New FrmPriceComponantMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PriceComponentMapping) = CompairStringResult.Equal Then
    '            frm = New FrmPriceComponantMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SchemeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmSchmeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.inventorySetting) = CompairStringResult.Equal Then
    '            frm = New frmInventorySetting(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.chapterhead) = CompairStringResult.Equal Then
    '            frm = New frmChapterHead(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.itemStructure) = CompairStringResult.Equal Then
    '            frm = New frmItemStructure(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.itemGroups) = CompairStringResult.Equal Then
    '            frm = New frmItemGroup(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.itemMaster) = CompairStringResult.Equal Then
    '            frm = New frmItemMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.locationMaster) = CompairStringResult.Equal Then
    '            frm = New frmLocationMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerLocationMapping) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerLocationMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.itemPurchaseAccount) = CompairStringResult.Equal Then
    '            frm = New frmPurcahseAccountSetCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.itemSaleAccount) = CompairStringResult.Equal Then
    '            frm = New frmSaleAccountSetCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "itemPriceList") = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.unitMaster) = CompairStringResult.Equal Then
    '            frm = New frmUnitOfCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "conversionMaster") = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, "itemPriceMaster") = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.packType) = CompairStringResult.Equal Then
    '            frm = New Frmpacktype(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "frmExcisableLocationDetails") = CompairStringResult.Equal Then
    '            frm = New FrmExcisableLocationDetails(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemMasterRMOther) = CompairStringResult.Equal Then
    '            frm = New FrmItemMasterRMOther()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPartNoMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPartNoMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.InvetorySourceCode) = CompairStringResult.Equal Then
    '            frm = New frmInventorySourceCode()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '===============Greivance Type ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGrievanceTypeMaster) = CompairStringResult.Equal Then
    '            frm = New frmGrievanceTypeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemRackBinMapping) = CompairStringResult.Equal Then
    '            frm = New frmItemRackBinMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '===============Greivance Logging ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGrievanceLogging) = CompairStringResult.Equal Then
    '            frm = New frmGrievanceLogging()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '            '===============Greivance Allocation ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGrievanceAllocation) = CompairStringResult.Equal Then
    '            frm = New FrmGrievanceAllocation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '            '===============Greivance Resolution ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGrievanceResolution) = CompairStringResult.Equal Then
    '            frm = New FrmGrievanceResolution()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================


    '            '========================================================================================================================================
    '            '==============================Employee Equipment Tracking==========================================================================
    '            '===============Asset Category Master ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAssetTypeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '            '===============Asset Sub Category Master ======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetSubCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAssetSubCategoryMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '            '===============Asset Master======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAssetDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================
    '            '===============Asset Issue Return======================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetIssueReturn) = CompairStringResult.Equal Then
    '            frm = New frmAssetsIssueReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '====================================================================================================================================
    '            '=========================================================================================================================================

    '            '===============Exit Management ======================

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRSettings) = CompairStringResult.Equal Then
    '            frm = New FrmHRSettings()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmResignationLetter) = CompairStringResult.Equal Then
    '            frm = New FrmResignationLetter()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '======================HR Reports
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptRegisterOfDeduction) = CompairStringResult.Equal Then
    '            frm = New RptRegisterOfDeduction()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTerminationLetter) = CompairStringResult.Equal Then
    '            frm = New FrmHREXTerminationLetter()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmResignationAcceptanceOrRejection) = CompairStringResult.Equal Then
    '            frm = New FrmResignationAcceptanceORRejection()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHREMInterviewQuestion) = CompairStringResult.Equal Then
    '            frm = New FrmHREMInterviewQuestion()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHREMExitInterview) = CompairStringResult.Equal Then
    '            frm = New FrmExitInterview
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceRatingRpt) = CompairStringResult.Equal Then
    '            frm = New rptPerformanceRating()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '===================End Exit Management===============================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemListRpt) = CompairStringResult.Equal Then
    '            frm = New FrmItemListRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVendorListRPT) = CompairStringResult.Equal Then
    '            frm = New FrmVendorListRPT()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemLocationDetails) = CompairStringResult.Equal Then
    '            '    frm = New frmItemLocationDetails(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemReorderLevel) = CompairStringResult.Equal Then
    '            frm = New frmItemReorderLevel1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnItemCategory) = CompairStringResult.Equal Then
    '            frm = New FrmItemCategory1(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnItemSubCategory) = CompairStringResult.Equal Then
    '            frm = New FrmItemSubCategory(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemExciseMapping) = CompairStringResult.Equal Then
    '            frm = New FrmItemExciseMapping(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemBasicPrice) = CompairStringResult.Equal Then
    '            frm = New FrmItemBasicPrice(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStandardscheme) = CompairStringResult.Equal Then
    '            frm = New frmStandardscheme(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStandardRateItem) = CompairStringResult.Equal Then
    '            frm = New frmStandardRateItem(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemCategoryLevel) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryLevel(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "ITEM")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorCategoryLevel) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryLevel(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "VENDOR")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerCategoryLevel) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryLevel(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "CUSTOMER")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLocationCategoryLevel) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryLevel(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "LOCATION")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemCategoryStructure) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryStructure(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "ITEM")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorCategoryStructure) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryStructure(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "VENDOR")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerCategoryStructure) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryStructure(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "CUSTOMER")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLocationCategoryStructure) = CompairStringResult.Equal Then
    '            frm = New frmItemCategoryStructure(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode, "LOCATION")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''richa 21/08/2014
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCatalogMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCatalogMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBarCodeGenerator) = CompairStringResult.Equal Then
    '            frm = New FrmBarCodeGenerator()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBarCodeGenerator1) = CompairStringResult.Equal Then
    '            frm = New FrmBarCodeGenerator1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.WarrantyMaster) = CompairStringResult.Equal Then
    '            frm = New frmWarrentyMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSchemeMasterNew) = CompairStringResult.Equal Then
    '            frm = New FrmSchemeMasterNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWeightConversion) = CompairStringResult.Equal Then
    '            frm = New FrmWeightCoversion()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInventoryAgeingReport) = CompairStringResult.Equal Then
    '            frm = New frmStockAgeingReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPriceGroupMapping) = CompairStringResult.Equal Then
    '            frm = New FrmPriceGroupMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTragetMaster) = CompairStringResult.Equal Then
    '            frm = New FrmTragetMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPrintProductInvoiceStatement) = CompairStringResult.Equal Then
    '            frm = New FrmPrintProductInvoiceStatement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptProductSaleRegister1) = CompairStringResult.Equal Then
    '            'frm = New RptProductSaleRegister1()
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DispatchChecklist) = CompairStringResult.Equal Then
    '            frm = New FrmDispatchChecklist()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptProductBookingStatus) = CompairStringResult.Equal Then
    '            frm = New RptProductBookingStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptProductDispatchStatus) = CompairStringResult.Equal Then
    '            frm = New RptProductDispatchStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptProductDOStatus) = CompairStringResult.Equal Then
    '            frm = New RptProductDOStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptProductSaleOrderStatus) = CompairStringResult.Equal Then
    '            frm = New RptProductSaleOrderStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)




    '            '------------------ Inventory Transactions---------------------------------------
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.adjust) = CompairStringResult.Equal Then
    '            '    frm=New FrmAdjustments1(lblUserCode.Text, lblCompanyCode.Text, "Adjustment Entry")
    '            '    frm.Text = "Adjustment Entry"
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnEmptyTrans) = CompairStringResult.Equal Then
    '            frm = New frmAdjustmentEmpty()
    '            'frm.Text = "Empty Transactions"
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnProductionEntry) = CompairStringResult.Equal Then
    '            frm = New frmAdjustmentProduction()
    '            'frm.Text = "Production Entry"
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnStoreAdjustment) = CompairStringResult.Equal Then
    '            frm = New frmAdjustmentStore()
    '            frm.Text = "Store Adjustment"
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmJobWorkInventory) = CompairStringResult.Equal Then
    '            frm = New frmJobWorkInventory()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRawMilkConsumtion) = CompairStringResult.Equal Then
    '            frm = New frmRawMilkConsumption()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Transfer) = CompairStringResult.Equal Then
    '            If clsCommon.CompairString(objCommonVar.CurrentCompanyCode, "KL") = CompairStringResult.Equal OrElse clsCommon.CompairString(objCommonVar.CurrentIndustryType, "D") = CompairStringResult.Equal Then
    '                frm = New FrmTransferKDIL()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmTransferDCC()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTransferGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmTransferGateOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TransferReturn) = CompairStringResult.Equal Then
    '            frm = New frmTransferKDILReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CreateTransfer) = CompairStringResult.Equal Then
    '            frm = New FrmTransfer3rdDoc()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Indent) = CompairStringResult.Equal Then
    '            frm = New frmIndent(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "transferEntry") = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, "adjustmentEntry") = CompairStringResult.Equal Then
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemMcMapping) = CompairStringResult.Equal Then
    '            frm = New FrmItemMcMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWarehouseBreakage) = CompairStringResult.Equal Then
    '            frm = New FrmWarehouseBreakage()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmExpiryDateEntry) = CompairStringResult.Equal Then
    '            frm = New FrmExpiryDateEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPhysicalStock) = CompairStringResult.Equal Then
    '            frm = New FrmPhysicalStock()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ChangeItemSerialNumber) = CompairStringResult.Equal Then
    '            frm = New frmChangeSerialNumber()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemStockConversion) = CompairStringResult.Equal Then
    '            frm = New frmItemToItemStockConverion
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ Inventory Reports---------------------------------------

    '            'ElseIf clsCommon.CompairString(strProgramCode, "ItemLocationReport") = CompairStringResult.Equal Then
    '            'frm=New FrmLocationsReport()
    '            '     formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnItemMovement) = CompairStringResult.Equal Then
    '            frm = New frmRptInventoryMovement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "mtbnTransfer") = CompairStringResult.Equal Then
    '            frm = New frmRptTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemLocationDetailsReport) = CompairStringResult.Equal Then
    '            '    frm = New RptItemLocationDetailsNewVersion()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.KeyValue) = CompairStringResult.Equal Then
    '            frm = New FrmKeyvalueReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLeakageBreakage) = CompairStringResult.Equal Then
    '            frm = New FrmLeakageBreakage()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.GatePass_Vs_actual) = CompairStringResult.Equal Then
    '            frm = New RptGatePassVSActual()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptInvoiceAgainstInward) = CompairStringResult.Equal Then
    '            frm = New RptInvoiceAgainstInward()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemPrice) = CompairStringResult.Equal Then
    '            frm = New frmItemPrice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.StockRecoReport) = CompairStringResult.Equal Then
    '            frm = New FrmShippingStockreport1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmStockDispatchReport) = CompairStringResult.Equal Then
    '            frm = New FrmStockDispatchReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''Added By PANKAJ on 02/Counter/2011
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnStockAdjustmentReport) = CompairStringResult.Equal Then
    '            frm = New FrmStockAdjustmentReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAdjustmentStatusReport1) = CompairStringResult.Equal Then
    '            frm = New FrmAdjustmentStatusReport1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnBreakageReport) = CompairStringResult.Equal Then
    '            frm = New FrmBreakageReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BreakageReportSummary) = CompairStringResult.Equal Then
    '            frm = New FrmBreakageReportSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RoutewiseBreakageReport) = CompairStringResult.Equal Then
    '            frm = New FrmRoutewiseBreakageSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strFormName, "StockReport") = CompairStringResult.Equal Then
    '            '    frm=New FrmStockReport(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ReportTransfer) = CompairStringResult.Equal Then
    '            frm = New ReportTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "SamplingReportSummary") = CompairStringResult.Equal Then
    '            frm = New FrmSamplingReportSummary1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SchemeReport) = CompairStringResult.Equal Then
    '            frm = New FrmSchemeReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.StockReportForFinishedGoods) = CompairStringResult.Equal Then
    '            frm = New FrmStockReportFinishedGoods()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAdjustmentReport) = CompairStringResult.Equal Then
    '            frm = New FrmAdjustmentReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVehicleWiseLoadout) = CompairStringResult.Equal Then
    '            frm = New frmVehicleWiseTransfe()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingIndentTransferReport) = CompairStringResult.Equal Then
    '            frm = New FrmPendingIndentTransferReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.stockRecoNew) = CompairStringResult.Equal Then
    '            frm = New FrmStockReco(strProgramCode)
    '            'frm=New FrmStockRecoNewNew
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.stockRecoNewJR) = CompairStringResult.Equal Then
    '            frm = New FrmStockReco(strProgramCode)
    '            'frm=New FrmStockRecoNewNew
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.stockRecoBatch) = CompairStringResult.Equal Then
    '            frm = New FrmStockRecoBatch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemSerialTrackingReport) = CompairStringResult.Equal Then
    '            frm = New FrmItemSerialTrackingReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTransferRegister) = CompairStringResult.Equal Then
    '            frm = New FrmTransferRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFatSnfStockReport) = CompairStringResult.Equal Then
    '            frm = New FrmFatSNFStock
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDatewiseQtyFatSnfStockReport) = CompairStringResult.Equal Then
    '            frm = New FrmDatewiseMilkStock
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMilkLossGain) = CompairStringResult.Equal Then
    '            frm = New frmMCCMilkLossGain
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MeterialstockReco) = CompairStringResult.Equal Then
    '            frm = New FrmMeterialStockReco(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptUnpostingTransItemQty) = CompairStringResult.Equal Then
    '            frm = New RptUnpostingTransItemQty
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '            ''''End
    '            '------------------ Sales And Distribution Masters---------------------------------------


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Sampling_Master) = CompairStringResult.Equal Then
    '            frm = New Sampling_Master()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.cash_Register_Details4) = CompairStringResult.Equal Then
    '            frm = New Cash_Register_Details4()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQuickSettlement) = CompairStringResult.Equal Then
    '            frm = New FrmQuickSettlement(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSettlementMaster) = CompairStringResult.Equal Then
    '            frm = New FrmSettlementMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.groupMasterRoute) = CompairStringResult.Equal Then
    '            frm = New frmRouteGroupMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.routeMaster) = CompairStringResult.Equal Then
    '            frm = New frmRouteMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.transportMaster) = CompairStringResult.Equal Then
    '            frm = New frmTransportMaster(lblUserCode.Text, lblCompanyCode.Text, clsUserMgtCode.transportMaster)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.transportMasterVendor) = CompairStringResult.Equal Then
    '            frm = New frmTransportMaster(lblUserCode.Text, lblCompanyCode.Text, clsUserMgtCode.transportMasterVendor)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vhicleMaster) = CompairStringResult.Equal Then
    '            frm = New frmVehicleMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.channelCategory) = CompairStringResult.Equal Then
    '            frm = New frmchannelCategory(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.channelMaster) = CompairStringResult.Equal Then
    '            frm = New frmChannelMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.visiMaster) = CompairStringResult.Equal Then
    '            frm = New frmvisimaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.customerType) = CompairStringResult.Equal Then
    '            '    frm=New frmCustomerType(lblUserCode.Text, lblCompanyCode.Text)
    '            '    If funSetUserAccess("CUST-TYPE", frm) = False Then Exit Sub
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.priceMaster) = CompairStringResult.Equal Then
    '            '    frm=New FrmPriceMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.priceComponentMaster) = CompairStringResult.Equal  Then
    '            '    frm=New FrmPriceComponantMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.priceComponentMapping) = CompairStringResult.Equal Then
    '            '    frm=New FrmPriceComponantMapping(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.customerCategory) = CompairStringResult.Equal Then
    '            '    frm=New frmCustomerCategory(lblUserCode.Text, lblCompanyCode.Text)
    '            '    If funSetUserAccess("CUST-CAT-M", frm) = False Then Exit Sub
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.customerMaster) = CompairStringResult.Equal Then
    '            '    frm=New frmCustomer(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.customerGroup) = CompairStringResult.Equal Then
    '            '    frm=New frmCustomerGroup(lblUserCode.Text, lblCompanyCode.Text)
    '            '    If funSetUserAccess("CUST-GRP-M", frm) = False Then Exit Sub
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.customerAccountSet) = CompairStringResult.Equal Then
    '            '    frm=New frmCustomerAccountSet(lblUserCode.Text, lblCompanyCode.Text)
    '            '    If funSetUserAccess("CUST-ACT-ST", frm) = False Then Exit Sub
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCommissionMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCommissionMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, "customer/OutletMaster") = CompairStringResult.Equal Then
    '            'ElseIf clsCommon.CompairString(strFormName, clsUserMgtCode.PriceMaster) = CompairStringResult.Equal strFormName = "schemeMaster" Then
    '            '    frm=New FrmSchmeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ShiptoLocation) = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAbateMentMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAbateMentMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.transportType) = CompairStringResult.Equal Then
    '            frm = New FrmTransportType(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.routetypemaster) = CompairStringResult.Equal Then
    '            frm = New FrmRouteTypeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            ' Paravet Services

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCattleType) = CompairStringResult.Equal Then
    '            frm = New FrmCattleTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBredType) = CompairStringResult.Equal Then
    '            frm = New FrmBredTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCattleColor) = CompairStringResult.Equal Then
    '            frm = New FrmCattleColorMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCattleMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCattleMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceGroup) = CompairStringResult.Equal Then
    '            frm = New FrmServiceGroup
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceName) = CompairStringResult.Equal Then
    '            '    frm = New FrmServiceName
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmServiceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmNDDBMaster) = CompairStringResult.Equal Then
    '            frm = New FrmNDDBMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBullMaster) = CompairStringResult.Equal Then
    '            frm = New FrmBullMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFarmerServiceOrderWithRate) = CompairStringResult.Equal Then
    '            frm = New FrmFarmerServiceOrderWithRate
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSMSDetails) = CompairStringResult.Equal Then
    '            frm = New frmFarmerServiceOrder
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesManHierarchy) = CompairStringResult.Equal Then
    '            frm = New FrmSalesManHierarchy(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnBreakageHead1) = CompairStringResult.Equal Then
    '            frm = New FrmBreakagehead()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCashDiscountReport) = CompairStringResult.Equal Then
    '            frm = New FrmCashDiscountReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerVendorMapping) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerVendorMapping()
    '            frm.formtype = clsUserMgtCode.CustomerVendorMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCheckPrinting) = CompairStringResult.Equal Then
    '            frm = New frmPrintCheckMultiple
    '            'frm.formtype = clsUserMgtCode.CustomerVendorMappingVendor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmNEFTUploader) = CompairStringResult.Equal Then
    '            frm = New FrmNEFTUploader(strProgramCode)

    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmNEFTUploaderFarmer) = CompairStringResult.Equal Then
    '            frm = New FrmNEFTUploader(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerVendorMappingVendor) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerVendorMapping()
    '            frm.formtype = clsUserMgtCode.CustomerVendorMappingVendor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TDMTARGET) = CompairStringResult.Equal Then
    '            frm = New TDMwiseTarget()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRouteShifting) = CompairStringResult.Equal Then
    '            frm = New FrmRoute_Shifting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDiscountMaster) = CompairStringResult.Equal Then
    '            frm = New FrmDiscountMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDiscountCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New FrmDiscountCategoryMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnTargetMaster) = CompairStringResult.Equal Then
    '            frm = New FrmTargetMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnTmplateCreation) = CompairStringResult.Equal Then
    '            frm = New FrmTemplateCreation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmViewPunchingInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmViewPunchingInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDayWiseLoadOutEntered) = CompairStringResult.Equal Then
    '            frm = New FrmDayWiseLoadOutEntered()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerTargetFixing) = CompairStringResult.Equal Then
    '            frm = New frmCustomerTargetFixing()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmClaimMaster) = CompairStringResult.Equal Then
    '            frm = New frmClaimMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNShipmentImportExport) = CompairStringResult.Equal Then
    '            frm = New frmShipmentImportExport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRemarMaster) = CompairStringResult.Equal Then
    '            frm = New frmRemarkMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '' Zone Master
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmZoneMaster) = CompairStringResult.Equal Then
    '            frm = New FrmZoneMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''
    '            '------------------ Sales And Distribution Transactions---------------------------------------


    '            ''----------------- Sales And Distribution Transaction NEW--------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.customerItemDetails) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerItemDetails()
    '            frm.isFromApprovalForm = False
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBookingEntry) = CompairStringResult.Equal Then
    '            frm = New FrmBookingEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmShortCloseDO) = CompairStringResult.Equal Then
    '            frm = New FrmShortCloseDO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmShortCloseDOPS) = CompairStringResult.Equal Then
    '            frm = New FrmShortCloseDOPS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmShortCloseDOCS) = CompairStringResult.Equal Then
    '            frm = New FrmShortCloseDOCS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDispatchFreshSale) = CompairStringResult.Equal Then
    '            frm = New frmDispatchNoteFreshSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDispatchMultipleFreshSale) = CompairStringResult.Equal Then
    '            frm = New frmDispatchMultipleFreshSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.GatePassTransfer) = CompairStringResult.Equal AndAlso clsCommon.CompairString(clsFixedParameter.GetData(clsFixedParameterType.CreateTransferFromBooking, clsFixedParameterCode.CreateTransferFromBooking, Nothing), "1") = CompairStringResult.Equal Then
    '            frm = New FrmGatePassTransfer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TransferCrateReceived) = CompairStringResult.Equal Then
    '            frm = New frmCreateReceivedCustomerDairySale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCreditLimitApproval) = CompairStringResult.Equal Then
    '            frm = New FrmCreditLimitApprovalMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFreshCreditLimitApproval) = CompairStringResult.Equal Then
    '            frm = New FrmCreditLimitApprovalMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesLevelHierarchy) = CompairStringResult.Equal Then
    '            frm = New FrmSalesLevelhierarchy
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesHierarchy) = CompairStringResult.Equal Then
    '            frm = New FrmsalesHierarchy
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesHierarchyMapping) = CompairStringResult.Equal Then
    '            frm = New FrmSalesHierarchyMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkCreditLimitApproval) = CompairStringResult.Equal Then
    '            frm = New FrmCreditLimitApprovalMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInvoiceFreshSale) = CompairStringResult.Equal Then
    '            frm = New frmInvoiceFreshSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmInvoiceCrateLinerDetail) = CompairStringResult.Equal Then
    '            frm = New FrmInvoiceCrateLinerDetail
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCreateReceived) = CompairStringResult.Equal Then
    '            frm = New frmCreateReceived
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSalesOrderBS) = CompairStringResult.Equal Then
    '            frm = New FrmSalesOrderBS_Pavitra()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGateEntrySale) = CompairStringResult.Equal Then
    '            frm = New FrmGateEntrySale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmWeighmentEntry) = CompairStringResult.Equal Then
    '            frm = New FrmWeighmentEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCGateEntry) = CompairStringResult.Equal Then
    '            frm = New frmMCCGateEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCWeighment) = CompairStringResult.Equal Then
    '            frm = New frmMCCTankerWeighment()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLoadingTanker) = CompairStringResult.Equal Then
    '            frm = New FrmLoadingTanker()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmQualityCheckBulkSale) = CompairStringResult.Equal Then
    '            frm = New FrmQualityCheckBulkSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTranReverse) = CompairStringResult.Equal Then
    '            frm = New frmTransactionReverse()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDispatchBulkSale) = CompairStringResult.Equal Then
    '            frm = New FrmDispatchBulkSale()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmInvoiceBulkSale) = CompairStringResult.Equal Then
    '            frm = New FrmInvoiceBulkSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCreateAutoInvoiceBS) = CompairStringResult.Equal Then
    '            frm = New FrmCreateAutoInvoiceBS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkSaleReturn) = CompairStringResult.Equal Then
    '            frm = New FrmBulkSaleReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCanSaleUploader) = CompairStringResult.Equal Then
    '            frm = New FrmCanSaleUploader()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCanSale) = CompairStringResult.Equal Then
    '            frm = New FrmCanSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCanReceived) = CompairStringResult.Equal Then
    '            frm = New frmCanReceived()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkDispatchReturnSale) = CompairStringResult.Equal Then
    '            frm = New FrmBulkDispatchReturnSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTankerOut) = CompairStringResult.Equal Then
    '            frm = New FrmTankerOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDispatchBulkSaleTrade) = CompairStringResult.Equal Then
    '            frm = New FrmDispatchBulkSaleTrade()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDispatchBulkSaleTradeReturn) = CompairStringResult.Equal Then
    '            frm = New FrmDispatchBulkSaleTradereturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFixedDeposit) = CompairStringResult.Equal Then
    '            frm = New FrmFixedDeposit()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProformaInvoiceMT) = CompairStringResult.Equal Then
    '            frm = New frmEXPorformaInvoice(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchaseOrderMT) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseOrder(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkCloser) = CompairStringResult.Equal Then
    '            frm = New FrmBulkCloser()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesOrderMT) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesOrder(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXSalesOrderR) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesOrderReturn(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCommercialInvoiceMT) = CompairStringResult.Equal Then
    '            frm = New frmEXCommercialInvoice(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSRNMT) = CompairStringResult.Equal Then
    '            frm = New frmSRN(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesInvoiceMT) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesInvoice(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesReturnMT) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesReturn(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLCRequest) = CompairStringResult.Equal Then
    '            frm = New FrmLCRequest()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmLCCreation) = CompairStringResult.Equal Then
    '            frm = New FrmLCCreation()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDocumentAcceptance) = CompairStringResult.Equal Then
    '            frm = New FrmDocumentAcceptance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''shivani
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCashAgainstDocs) = CompairStringResult.Equal Then
    '            frm = New RptCashAgainstDocs()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMTPIInHead) = CompairStringResult.Equal Then
    '            frm = New RptMTPIInHead()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptEXProductWiseDetail) = CompairStringResult.Equal Then
    '            frm = New RptMTProductWiseDetailReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptdateOfArrivalofCons) = CompairStringResult.Equal Then
    '            frm = New RptDateOfArrivalOfCon()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkSalePriceChart) = CompairStringResult.Equal Then
    '            frm = New FrmBulkSalePriceChart()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPrintBulkInvoiceStatement) = CompairStringResult.Equal Then
    '            frm = New FrmPrintBulkInvoiceStatement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBulkSaleRegister) = CompairStringResult.Equal Then
    '            'frm = New RptBulkSaleRegister()
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' changed by panch raj on 08-05-18 against ticket No: KDI/04/05/18-000295
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEnquiryMaster) = CompairStringResult.Equal Then
    '            frm = New FrmEnquiryMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCHAChargeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCHAChargeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmExIncentiveMaster) = CompairStringResult.Equal Then
    '            frm = New FrmExIncentiveMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmNotifiedPartyMaster) = CompairStringResult.Equal Then
    '            frm = New FrmNotifiedPartyMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXSalesQuotation) = CompairStringResult.Equal Then
    '            frm = New FrmEXSalesQuotation(clsUserMgtCode.frmEXSalesQuotation)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXSalesOrder) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesOrder(clsUserMgtCode.frmEXSalesOrder)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXPorformaInvoice) = CompairStringResult.Equal Then
    '            frm = New frmEXPorformaInvoice(clsUserMgtCode.frmEXPorformaInvoice)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXCommercialInvoice) = CompairStringResult.Equal Then
    '            frm = New frmEXCommercialInvoice(clsUserMgtCode.frmEXCommercialInvoice)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXSalesInvoice) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesInvoice(clsUserMgtCode.frmEXSalesInvoice)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEXSalesReturn) = CompairStringResult.Equal Then
    '            frm = New frmEXSalesReturn(clsUserMgtCode.frmEXSalesReturn)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptExportSaleRegister) = CompairStringResult.Equal Then
    '            'frm = New RptExportSaleRegister()
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' changed by panch raj on 08-05-18 against ticket No: KDI/04/05/18-000295
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSAPriceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCSAPriceMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSACommissionItemWise) = CompairStringResult.Equal Then
    '            frm = New FrmCSACommissionItemWise()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSAAccountSet) = CompairStringResult.Equal Then
    '            frm = New FrmCSAAccountSet()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateEntry_JWO) = CompairStringResult.Equal Then
    '            frm = New FrmMilkGateEntry_JWO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWeighment_JWO) = CompairStringResult.Equal Then
    '            frm = New FrmMilkWeighment_JWO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQC_JWO) = CompairStringResult.Equal Then
    '            frm = New FrmMilkQualityCheck_JWO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUnloading_JWO) = CompairStringResult.Equal Then
    '            frm = New FrmMilkUnloading_JWO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JWO_SRN) = CompairStringResult.Equal Then
    '            frm = New FrmJWOSRN()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JWO_SRN_Return) = CompairStringResult.Equal Then
    '            frm = New FrmJWOSRNReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobWorkConsumption) = CompairStringResult.Equal Then
    '            frm = New frmJobWorkConsumption()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSABooking) = CompairStringResult.Equal Then
    '            frm = New frmCSABooking(clsUserMgtCode.frmCSABooking)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSARequest) = CompairStringResult.Equal Then
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.ShowCSARequestScreen, clsFixedParameterCode.ShowCSARequestScreen, Nothing)) = 1 Then
    '                frm = New frmCSABooking(clsUserMgtCode.frmCSARequest)
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                clsCommon.MyMessageBoxShow("You are not authorize to access CSA Request.")
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSADeliveryOrder) = CompairStringResult.Equal Then
    '            frm = New FrmCSADeliveryOrder
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSATransfer) = CompairStringResult.Equal Then
    '            frm = New frmCSATransfer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSASaleInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmCSASaleInvoice
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSASalePattiReturn) = CompairStringResult.Equal Then
    '            frm = New FrmCSASalePattiReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSATransferReturn) = CompairStringResult.Equal Then
    '            frm = New FrmCSATransferReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSATransferReport) = CompairStringResult.Equal Then
    '            frm = New FrmCSATransferReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCSADOReport) = CompairStringResult.Equal Then
    '            frm = New FrmCSADOReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCSASaleRegister) = CompairStringResult.Equal Then
    '            'frm = New RptCSASaleRegister
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '' changed by panch raj on 08-05-18 against ticket No: KDI/04/05/18-000295
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCSACustomerLedger) = CompairStringResult.Equal Then
    '            frm = New frmRptCSACustomerLedger(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPartyWiseSale) = CompairStringResult.Equal Then
    '            frm = New RptPartyWiseSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCSAmonthlywisereport) = CompairStringResult.Equal Then
    '            frm = New Frm_MW_SaleAnalysiReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''Test
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkSaleSettings) = CompairStringResult.Equal Then
    '            frm = New FrmBulkSaleSettings()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMultCustBookingDisp) = CompairStringResult.Equal Then
    '            frm = New FrmMultCustBookingDispatch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeliveryNoteFreshSale) = CompairStringResult.Equal Then
    '            frm = New frmDeliveryNoteFreshSale()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBookingProductSale) = CompairStringResult.Equal Then
    '            frm = New frmBookingProductSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSchemeMasterDairy) = CompairStringResult.Equal Then
    '            frm = New FrmSchemeMasterDairy()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRouteMaster) = CompairStringResult.Equal Then
    '            frm = New frmRouteMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDistanceMappingMaster) = CompairStringResult.Equal Then
    '            '    frm = New FrmDistanceMappingMaster()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesOrderProductSale) = CompairStringResult.Equal Then
    '            frm = New frmDeliveryOrderProductSale()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeliveryPrderProductSale) = CompairStringResult.Equal Then
    '            frm = New frmSaleOrderProductSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmShipmentProductSale) = CompairStringResult.Equal Then
    '            frm = New frmShipmentProductSale()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateEntryReturnPS) = CompairStringResult.Equal Then
    '            frm = New frmGateEntryReturnPS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateEntryReturnCS) = CompairStringResult.Equal Then
    '            frm = New frmGateEntryReturnCSA()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateEntryReturnTransfer) = CompairStringResult.Equal Then
    '            frm = New frmGateEntryReturnTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCSATransferGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmCSATransferGateOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProductDispatchGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmProductDispatchGateOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmdispatchAdviceProductSale) = CompairStringResult.Equal Then
    '            frm = New frmDispatchAdviceProductSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleInvoiceProductSale) = CompairStringResult.Equal Then
    '            frm = New frmSaleInvoiceProductSale(strProgramCode)
    '            'frm = New frmSaleInvoiceProductSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleReturnProductSale) = CompairStringResult.Equal Then
    '            frm = New frmSaleReturnProductSale()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptsaleRegisterReport) = CompairStringResult.Equal Then
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSalesHierarchyReport) = CompairStringResult.Equal Then
    '            frm = New rptSalesHierarchyReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVehicleWiseReport) = CompairStringResult.Equal Then
    '            '    frm = New RptVehicleWiseReport
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGatePassFS) = CompairStringResult.Equal Then
    '            frm = New FrmGatePassFS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGatePassPS) = CompairStringResult.Equal Then
    '            '    frm = New FrmGatePassPS()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleReturnFreshSale) = CompairStringResult.Equal Then
    '            frm = New frmSaleReturnFreshSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRouteFreightDetails) = CompairStringResult.Equal Then
    '            frm = New FrmRouteFreightDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDispatchFreshSale) = CompairStringResult.Equal Then
    '            ''    frm=New FrmDispatchFreshSale()
    '            ''         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPendingQuotationApproval) = CompairStringResult.Equal Then
    '            frm = New FrmPendingQuotationApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmApprovalSetting) = CompairStringResult.Equal Then
    '            frm = New FrmApprovalSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalesmanTarget) = CompairStringResult.Equal Then
    '            frm = New FrmSalesmanTarget()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.customerItemDetails) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerItemDetails()
    '            frm.isFromApprovalForm = False
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerItemDetailApproval) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerItemDetails()
    '            frm.isFromApprovalForm = True
    '            frm.Text = "Item Price List Approval"
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.customerItemMapping) = CompairStringResult.Equal Then
    '            frm = New frmCustomerItemMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemPriceListLevel3) = CompairStringResult.Equal Then
    '            frm = New FrmItemPriceListLevel3()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frm_User_Customer_Rate_Settings) = CompairStringResult.Equal Then
    '            frm = New Frm_User_Customer_Rate_Settings()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.routeMaster) = CompairStringResult.Equal Then
    '            frm = New frmRouteMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.routetypemaster) = CompairStringResult.Equal Then
    '            frm = New FrmRouteTypeMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleQuotation) = CompairStringResult.Equal Then
    '            frm = New frmSNSalesQuotation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNSalesOrder) = CompairStringResult.Equal Then
    '            frm = New frmSNSalesOrder()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNShipment) = CompairStringResult.Equal Then
    '            frm = New frmSNShipment()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNSaleInvoice) = CompairStringResult.Equal Then
    '            frm = New frmSNSaleInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNServiceInvoice) = CompairStringResult.Equal Then
    '            frm = New frmSNServiceInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNSaleReturn) = CompairStringResult.Equal Then
    '            frm = New frmSNSaleReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAutoSTN) = CompairStringResult.Equal Then
    '            frm = New FrmAutoSTN()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProspect) = CompairStringResult.Equal Then
    '            '    frm = New frmProspectDetail()
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '            ''richa
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSetting) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingFresh) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingBulk) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingMerchant) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMerchantPaymentTerms) = CompairStringResult.Equal Then
    '            frm = New FrmMerchantPaymentTerms()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMerchantPaymentTermsGroup) = CompairStringResult.Equal Then
    '            frm = New FrmMerchantPaymentTermsGroup()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMTReportContextFormat) = CompairStringResult.Equal Then
    '            frm = New FrmMTReportContextFormat()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingExport) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingCSA) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingProduct) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''=================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNPOS) = CompairStringResult.Equal Then
    '            frm = New frmSNPOS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSNReceiptChallan) = CompairStringResult.Equal Then
    '            frm = New frmReceiptChallan()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSalesmanTarget) = CompairStringResult.Equal Then
    '            frm = New frmRptSalesmanTarge()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleOrderDetail) = CompairStringResult.Equal Then
    '            frm = New frmSaleOrderDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleOrderSummary) = CompairStringResult.Equal Then
    '            frm = New FrmSaleOrderSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleInvoiceSummary) = CompairStringResult.Equal Then
    '            frm = New FrmSaleInvoiceSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleInvoiceDetail) = CompairStringResult.Equal Then
    '            frm = New FrmSaleInvoiceDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmShipmentDetail) = CompairStringResult.Equal Then
    '            frm = New FrmShipmentDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmShipmentSummary) = CompairStringResult.Equal Then
    '            frm = New FrmShipmentSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleRegisterDemo) = CompairStringResult.Equal Then
    '            ' frm = New FrmSaleRegisterDemo()
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSaleRegisterForAdv) = CompairStringResult.Equal Then
    '            ' frm = New FrmSaleRegisterDemo()
    '            frm = New RptSaleRegisterReportForAdv(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISSaleRegisterWithCSASalePatti) = CompairStringResult.Equal Then
    '            'frm = New RptSaleRegisterReportWithCSASalePatti(strProgramCode)
    '            '' changed by panch raj on 02-05-18 against ticket No: UDL/27/04/18-000143
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSKUWiseSale) = CompairStringResult.Equal Then
    '            frm = New rptSKUWiseSale(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISSaleRegisterWithCSASalePattiProductLocationWise) = CompairStringResult.Equal Then
    '            frm = New RptSaleRegisterReportWithCSASalePattiProductLocationWise(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISSaleRegisterWithCSASalePattiProductPackWise) = CompairStringResult.Equal Then
    '            frm = New RptSaleRegisterReportWithCSASalePattiProductPackWise(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPendingSaleInvoiceforChilpPO) = CompairStringResult.Equal Then
    '            frm = New frmPendingSaleInvoiceforChilpPO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ORDNEW) = CompairStringResult.Equal Then
    '            frm = New FrmOrdertracking()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnItemMovement) = CompairStringResult.Equal Then
    '            '    frm = New frmRptInventoryMovement()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReceiptChallanReport) = CompairStringResult.Equal Then
    '            frm = New frmReceiptChallanReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProspectDetailReport) = CompairStringResult.Equal Then
    '            '    frm = New frmProspectDetailReport()
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, "quotation") = CompairStringResult.Equal Then

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.saleOrders) = CompairStringResult.Equal Then
    '            frm = New frmSaleOrder()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCheckSlipEntry) = CompairStringResult.Equal Then
    '            frm = New FrmCheckSlipEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LoadOut) = CompairStringResult.Equal Then
    '            frm = New frmShipmentInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "saleInvoice") = CompairStringResult.Equal Then
    '            ''frm=New FrmSaleInvoice(lblUserCode.Text, lblCompanyCode.Text)
    '            ''     formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.saleReturn) = CompairStringResult.Equal Then
    '            frm = New frmSalesReturnNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SaleReturnInterCompany) = CompairStringResult.Equal Then
    '            'frm = New frmSaleReturnInter()
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCompleteTransfer) = CompairStringResult.Equal Then
    '            frm = New FrmCompleteTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LoadOutStatus) = CompairStringResult.Equal Then
    '            frm = New FrmCompleteLoadout(lblUserCode.Text, lblCompany.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ScrapSale) = CompairStringResult.Equal Then
    '            frm = New frmScrapSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JobWorkDispatch) = CompairStringResult.Equal Then
    '            frm = New frmJobWorkDispatch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JobWorkDispatchProduction) = CompairStringResult.Equal Then
    '            frm = New frmJobWorkDispatch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmScrapSaleGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmScrapSaleGateOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, "ScrapInvoice") = CompairStringResult.Equal Then
    '            frm = New frmScrapInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSettlementEntry) = CompairStringResult.Equal Then
    '            frm = New FrmSettlementEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTransferIncompleteRemarks1) = CompairStringResult.Equal Then
    '            frm = New FrmTransferIncompleteRemarks1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "FrmReverseEntry") = CompairStringResult.Equal Then
    '            Dim frmP As New FrmPWD(Nothing)
    '            frmP.strCode = "TempProvisional"
    '            frmP.strType = "TempProvisional"
    '            frmP.ShowDialog()
    '            If frmP.isPasswordCorrect Then
    '                Try
    '                    frm = New FrmReverseEntry()
    '                    formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '                Catch ex As Exception
    '                    common.clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
    '                End Try
    '            End If

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ScrapSaleRetrun) = CompairStringResult.Equal Then
    '            frm = New frmScrapSaleReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ChangeInvoiceSalesman) = CompairStringResult.Equal Then
    '            frm = New FrmChangeInvoiceSalesman()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.frmtransfer) = CompairStringResult.Equal Then
    '            '    frm=New frmTransfer(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.FrmReceipt) = CompairStringResult.Equal Then
    '            '    frm=New FrmReceipt(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleHistory) = CompairStringResult.Equal Then
    '            frm = New FrmSaleHistory()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmExpiryDate) = CompairStringResult.Equal Then
    '            frm = New FrmExpiryDate()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTransactionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmTransactionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFreshTransactionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmTransactionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkTransactionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmTransactionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMCCFarmerMapping) = CompairStringResult.Equal Then
    '            frm = New FrmMCCFarmerMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VLCMappingForMPAmount) = CompairStringResult.Equal Then
    '            frm = New frmVLCMappingForMPAmount()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VLCMappingForMP_PP) = CompairStringResult.Equal Then
    '            frm = New frmVLCMappingForMP_PaymentProcess()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FreightChargesMaster) = CompairStringResult.Equal Then
    '            frm = New frmFreightChargesMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMCCTransactionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmTransactionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPrintFreshInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmPrintFreshInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ' ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptFreshSaleRegister1) = CompairStringResult.Equal Then
    '            'frm = New RptFreshSaleRegister1()
    '            'formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptFreshSaleRegister1) = CompairStringResult.Equal Then
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCrateAccounting) = CompairStringResult.Equal Then
    '            frm = New rptCrateAccounting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCrateAccountingReport) = CompairStringResult.Equal Then
    '            frm = New RptCrateAccountingReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptFreshBookingStatus) = CompairStringResult.Equal Then
    '            frm = New RptFreshBookingStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)



    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptZoneWiseFreshSaleReport) = CompairStringResult.Equal Then
    '            frm = New RptZoneWiseFreshSaleReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDispatchChallanReportFresh) = CompairStringResult.Equal Then
    '            frm = New RptDispatchChallanReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMatrixFreshSalesReport) = CompairStringResult.Equal Then
    '            frm = New RptMatrixFreshSalesReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMatrixFreshSalesReportSaleDairy) = CompairStringResult.Equal Then
    '            frm = New RptMatrixFreshSalesReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''----Parteek ---''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPriceChartFreshSalesReport) = CompairStringResult.Equal Then
    '            frm = New frmRptPriceChartMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''---End---''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSaleReturnGateEntryReport) = CompairStringResult.Equal Then
    '            frm = New RptSaleReturnGateEntryReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCrateLinerReport) = CompairStringResult.Equal Then
    '            frm = New rptCrateLinerReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVehicleCapacityFreshSaleReport) = CompairStringResult.Equal Then
    '            frm = New RptVehicleCapacityFreshSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBulkMultipleDispatch) = CompairStringResult.Equal Then
    '            frm = New RptBulkMultipleDispatch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ Sales And Distribution Report---------------------------------------

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.vehicle_Details_Report1) = CompairStringResult.Equal Then
    '            frm = New Vehicle_Details_Report()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptTransfer_IncompleteReport) = CompairStringResult.Equal Then
    '            frm = New RptTransfer_IncompleteReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.reportQuickSettlement) = CompairStringResult.Equal Then
    '            frm = New FrmReportForQuickSettlement(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.visiDetail1) = CompairStringResult.Equal Then
    '            frm = New VisiDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PendingSaleOrderReport) = CompairStringResult.Equal Then
    '            frm = New PendingSaleOrderReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCDiscountReport) = CompairStringResult.Equal Then
    '            frm = New FrmMCDiscReport(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.crptLoadOut) = CompairStringResult.Equal Then
    '            frm = New FrmLoadOutRpt(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.receiptFillreport) = CompairStringResult.Equal Then
    '            frm = New FrmRECEIPTSAGAINSTSALES_FILLED_()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.receiptWOTreport) = CompairStringResult.Equal Then
    '            frm = New FrmInvoiceswithoutreceipt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.TransporterRpt) = CompairStringResult.Equal Then
    '            '    frm=New frmTransportMasterRpt(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.VehicleMasterRpt) = CompairStringResult.Equal Then
    '            '    frm=New FrmVehicleMasterRpt(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.receiptreport) = CompairStringResult.Equal Then
    '            frm = New Frmreceiptvoucher2(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.rptCustomerGroupDetails) = CompairStringResult.Equal Then
    '            '    frm=New FrmCustomerGroupReport()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf  clsCommon.CompairString(strFormName, clsUserMgtCode.frmCustomerDetails) = CompairStringResult.Equal Then
    '            '    frm=New FrmCustomerMasterReport()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.nrptSales) = CompairStringResult.Equal Then
    '            frm = New FrmRptSales()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnRptSalesManSalesReport) = CompairStringResult.Equal Then
    '            frm = New frmRptSalesManReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Settlement) = CompairStringResult.Equal Then
    '            frm = New FrmSettlementReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strFormName, "ProvSales") = CompairStringResult.Equal Then
    '            '    frm=New FrmProvionalSalesReport(lblUserCode.Text, lblCompanyCode.Text)
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CustomerRouteHistoryReport) = CompairStringResult.Equal Then
    '            frm = New FrmRptCustomerRouteHistory(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemDiscountReport) = CompairStringResult.Equal Then
    '            frm = New FrmDiscountReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "ItemCommissionReport") = CompairStringResult.Equal Then
    '            frm = New FrmItemCommissionReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LoadOutStatusreport1) = CompairStringResult.Equal Then
    '            frm = New FrmLoadOutStatusreport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LoadOutReport1) = CompairStringResult.Equal Then
    '            frm = New LoadOut()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "EmptyInwardRegisterSummary") = CompairStringResult.Equal Then
    '            frm = New FrmEmptyInwardRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnNetSaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmNetSaleReport1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptNetSaleDetailReport) = CompairStringResult.Equal Then
    '            '    frm=New frmRptNetSaleDetailReport()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDistrbutorSaleTarget) = CompairStringResult.Equal Then
    '            frm = New FrmDistrbutorSaleTarget()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDayReportDirectSale) = CompairStringResult.Equal Then
    '            frm = New FrmDayReportDirectSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ExciseSummary1) = CompairStringResult.Equal Then
    '            'If objCommonVar.IsDemoERP Then
    '            '    frm = New FrmExciseSummary_DEMO()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'ElseIf clsCommon.CompairString(objCommonVar.CurrentCompanyCode, "Guntur") = CompairStringResult.Equal Then
    '            '    frm = New FrmExciseSummaryNew()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'Else
    '            '    frm = New FrmExciseSummaryReport()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'End If
    '            frm = New FrmExciseSummaryNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.OtherPartySale) = CompairStringResult.Equal Then
    '            frm = New FrmOtherPartySale1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTransitBreakageReport1) = CompairStringResult.Equal Then
    '            frm = New FrmTransitBreakageReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCreditSaleReport) = CompairStringResult.Equal Then
    '            frm = New frmRptCreditSales()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.NoSaleReport) = CompairStringResult.Equal Then
    '            frm = New frmRptNoSales()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPenetration) = CompairStringResult.Equal Then
    '            frm = New frmRptPenetration()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FilloutwardRegisterReport1) = CompairStringResult.Equal Then
    '            frm = New FrmFilledOutwardRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmTDMSaleReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PrimarySales) = CompairStringResult.Equal Then
    '            frm = New FrmPrimarySalesReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SecondarySales) = CompairStringResult.Equal Then
    '            frm = New FrmSecondarySales(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmptyInwardSaleRegister1) = CompairStringResult.Equal Then
    '            frm = New frmInwardRegister(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ProvSaleDetail) = CompairStringResult.Equal Then
    '            frm = New FrmProvisionalSalesRoutewise(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGatePassENtry1) = CompairStringResult.Equal Then
    '            frm = New FrmGatePassENtry1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SaleAccountBreakDetail) = CompairStringResult.Equal Then
    '            frm = New FrmSaleAccountBreakOrCashDisc(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SaleAccountBreakage) = CompairStringResult.Equal Then
    '            frm = New FrmSaleAccountBreakageReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "DailyStockAccount") = CompairStringResult.Equal Then
    '            frm = New FrmDailyStockAccountRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "OverAllDisc") = CompairStringResult.Equal Then
    '            frm = New FrmOverallDiscountReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CrptRG1Detail1) = CompairStringResult.Equal Then
    '            If objCommonVar.IsDemoERP Then
    '                frm = New frmRG1Demo()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmRG1()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCFormReport) = CompairStringResult.Equal Then
    '            If objCommonVar.IsDemoERP Then
    '                frm = New FrmCFormReport()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOpeningBalance) = CompairStringResult.Equal Then
    '            frm = New FrmOpenningBalance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptInventoryMovement) = CompairStringResult.Equal Then
    '            frm = New RptInventoryMovement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptServiceTaxDetail) = CompairStringResult.Equal Then
    '            frm = New RptServiceTaxDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBankWiseChequeIssue) = CompairStringResult.Equal Then
    '            frm = New RptBankWiseChequeIssue()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ProvisionalSaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmProvionalSalesReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemCommissionSummary) = CompairStringResult.Equal Then
    '            frm = New FrmItemCommissionSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DistributedDiscountReport) = CompairStringResult.Equal Then
    '            frm = New FrmDistribuorDiscount(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleDiscount1) = CompairStringResult.Equal Then
    '            frm = New FrmSaleDiscount1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CEAllocationReport) = CompairStringResult.Equal Then
    '            frm = New FrmCEAllocationRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SalesCollection) = CompairStringResult.Equal Then
    '            frm = New FrmSalesCollection(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DailySettlement) = CompairStringResult.Equal Then
    '            frm = New frmDailySettlement(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMismatchSettlement) = CompairStringResult.Equal Then
    '            frm = New FrmMismatchSettlement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSettlementSheetReconcilationeport) = CompairStringResult.Equal Then
    '            frm = New FrmSettlementSheetReconcilationReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.OutletEmpty1) = CompairStringResult.Equal Then
    '            frm = New FrmOutletEmptyReport1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VehiclewiseSale1) = CompairStringResult.Equal Then
    '            frm = New FrmVehiclewiseSale(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Channelwisecustomer1) = CompairStringResult.Equal Then
    '            frm = New FrmChannelwiseCustomer1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCustomerRanking) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerRankingReport1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnVisiVPO1) = CompairStringResult.Equal Then
    '            frm = New FrmVisiVPOReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnMismatchReport) = CompairStringResult.Equal Then
    '            frm = New FrmMismatchRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnRouteSale) = CompairStringResult.Equal Then
    '            frm = New RouteSaleReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerTargetReport) = CompairStringResult.Equal Then
    '            frm = New frmCustomerTargetReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTDMReport) = CompairStringResult.Equal Then
    '            frm = New frmTDMReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SalesmanSalesOrderReport) = CompairStringResult.Equal Then
    '            frm = New FrmSalemanSaleOrder()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDealerManagementReport) = CompairStringResult.Equal Then
    '            frm = New FrmDealerManagementReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSalesAnalysis) = CompairStringResult.Equal Then
    '            frm = New frmSalesAnalysisReport(strUserCode, strCompany)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRptFormOfGuarntee) = CompairStringResult.Equal Then
    '            frm = New frmRptFormOfGuarntee
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCustomerWiseMonthlySalesAnalysis) = CompairStringResult.Equal Then
    '            frm = New frmCustomerWiseMonthlySalesAnalysis
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaySlipReport) = CompairStringResult.Equal Then
    '            frm = New frmCheckDepositPaySlip
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSalarySlipRpt) = CompairStringResult.Equal Then
    '            frm = New FrmSalarySlipRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSalarySummaryRpt) = CompairStringResult.Equal Then
    '            frm = New FrmSalarySummary
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmEmployeePFRpt) = CompairStringResult.Equal Then
    '            frm = New FrmEmployeePF
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmESICRpt) = CompairStringResult.Equal Then
    '            frm = New FrmESICRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptForm34) = CompairStringResult.Equal Then
    '            frm = New RptForm34
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBonusStatement) = CompairStringResult.Equal Then
    '            frm = New RptBonusStatement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBOILetterReport) = CompairStringResult.Equal Then
    '            frm = New RptBOILetterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptForm22) = CompairStringResult.Equal Then
    '            frm = New RptForm22
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmployeeWiseReport) = CompairStringResult.Equal Then
    '            frm = New EmployeeWiseReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptESIHalfYearly) = CompairStringResult.Equal Then
    '            frm = New RptESIHalfYearly
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm3A) = CompairStringResult.Equal Then
    '            frm = New RptPFForm3A
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm5) = CompairStringResult.Equal Then
    '            frm = New RptPFForm5
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm10) = CompairStringResult.Equal Then
    '            frm = New RptPFForm10
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RPtPFForm11_Revised_) = CompairStringResult.Equal Then
    '            frm = New RPtPFForm11_Revised_
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm12A_revised_) = CompairStringResult.Equal Then
    '            frm = New RptPFForm12A_revised_
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFChallanStatement) = CompairStringResult.Equal Then
    '            frm = New RptPFChallanStatement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptKDILSalarySlip) = CompairStringResult.Equal Then
    '            frm = New RptKDILSalarySlip
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPerformaForContributiondetail) = CompairStringResult.Equal Then
    '            frm = New RptPayrollPerformaforcontribution
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaymentMode) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentMode
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBankTransferDetail) = CompairStringResult.Equal Then
    '            frm = New RptBankTransferDetail
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDepartmentwiseSalarySheetRpt) = CompairStringResult.Equal Then
    '            frm = New RptDepartmentWiseSalarySheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDetailOfWelfareFundAmount) = CompairStringResult.Equal Then
    '            frm = New RptDetailOfWelfareFundAmount
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptESICStatement) = CompairStringResult.Equal Then
    '            frm = New RptESICStatement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFStatement) = CompairStringResult.Equal Then
    '            frm = New RptPFStatement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm6) = CompairStringResult.Equal Then
    '            frm = New RptPFForm6
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptESICForm6) = CompairStringResult.Equal Then
    '            frm = New RptESICForm6
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptESICChallan) = CompairStringResult.Equal Then
    '            frm = New RptESICChallan
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptESICDeclarationForm) = CompairStringResult.Equal Then
    '            frm = New RptESICDeclarationForm
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptActurialValuation) = CompairStringResult.Equal Then
    '            frm = New RptActurialValuation
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPFForm2) = CompairStringResult.Equal Then
    '            frm = New RptPFForm2
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDVAT32) = CompairStringResult.Equal Then
    '            frm = New frmRptDVAT32
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerBillWiseDetail) = CompairStringResult.Equal Then
    '            frm = New frmCustomerBillWiseDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleSummaryAgainstPO) = CompairStringResult.Equal Then
    '            frm = New frmSaleSummaryAgainstPO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmYearMonthWiseSaleComparison) = CompairStringResult.Equal Then
    '            frm = New frmYearMonthWiseSaleComparison()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCompanyMonthWiseSaleComparison) = CompairStringResult.Equal Then
    '            frm = New frmCompanyMonthWiseSaleComparison()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDistributerPerformance) = CompairStringResult.Equal Then
    '            frm = New rptDistributerPerformance(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerBillWiseDuesSummary) = CompairStringResult.Equal Then
    '            frm = New frmCustomerBillWiseDuesSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorGroupWiseSaleReport) = CompairStringResult.Equal Then
    '            frm = New frmVendorGroupWiseSaleReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBatchReceiptSTD) = CompairStringResult.Equal Then
    '            frm = New FrmBatchReceipt(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBatchReceiptPepsi) = CompairStringResult.Equal Then
    '            frm = New FrmBatchReceipt(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFilledOutWard) = CompairStringResult.Equal Then
    '            frm = New FrmFilledOutWard()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CashDiscount) = CompairStringResult.Equal Then
    '            frm = New rptCashDiscountReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CashDiscountReport) = CompairStringResult.Equal Then
    '            frm = New frmCashDiscountNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TransferRegister) = CompairStringResult.Equal Then
    '            frm = New TransferRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmptyReportDetail) = CompairStringResult.Equal Then
    '            frm = New EmptyReportDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPendingSettlement) = CompairStringResult.Equal Then
    '            frm = New RptPendingSettlement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDetailOfForm2A) = CompairStringResult.Equal Then
    '            frm = New FrmDetailOfForm2A()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTargetReport1) = CompairStringResult.Equal Then
    '            frm = New FrmTargetReport1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDailySettlementActualAndProvisionalReport) = CompairStringResult.Equal Then
    '            frm = New FrmDailySettlementActualAndProvisionalReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSettlement_CashMemoStatus) = CompairStringResult.Equal Then
    '            frm = New FrmSettlement_CashMemoStatus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReverseSettlementDetail) = CompairStringResult.Equal Then
    '            frm = New FrmReverseSettlement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMismatchCashMemo) = CompairStringResult.Equal Then
    '            frm = New FrmMismatchCashMemo()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCanceledSaleInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmCanceledSaleInvoice1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleVolumeTracker) = CompairStringResult.Equal Then
    '            frm = New FrmSaleVolumeTracker()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmEmptyTransactionReport) = CompairStringResult.Equal Then
    '            frm = New FrmRptEmptyTransaction()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleOrderSummary) = CompairStringResult.Equal Then
    '            frm = New FrmSaleOrderSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDiscountAnalysis) = CompairStringResult.Equal Then
    '            frm = New FrmDiscountAnalysis()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '''''' Added BY Abhishek a s on 3 Nov 2012---
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmpendingLoadin) = CompairStringResult.Equal Then
    '            frm = New FrmPendingLoadIn_Transfer_Type
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptSalesReturn) = CompairStringResult.Equal Then
    '            frm = New FrmRptSalesReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmQuickSettlementHead) = CompairStringResult.Equal Then
    '            frm = New FrmQuickSettlementHead
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVendorsOutstandings) = CompairStringResult.Equal Then
    '            '    frm = New FrmVendorsOutstandings
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LO_vs_Vechile) = CompairStringResult.Equal Then
    '            frm = New FrmloadoutVSvechileCapacity2
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptClaimMaster) = CompairStringResult.Equal Then
    '            frm = New rptClaimMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVehiclePendingStatusRpt) = CompairStringResult.Equal Then
    '            frm = New FrmVehiclePendingStatusRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '--------------------TDS Master-----------------------------

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.NatureOfDeduction) = CompairStringResult.Equal Then
    '            frm = New frmNatureOfDeduction(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PartyDetails) = CompairStringResult.Equal Then
    '            frm = New frmPartyDetails(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TDSSection) = CompairStringResult.Equal Then
    '            frm = New frmTDSSection(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BranchDetails) = CompairStringResult.Equal Then
    '            frm = New frmBranchDetails(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FinancialYear) = CompairStringResult.Equal Then
    '            frm = New frmFinancialYear(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.StateCode) = CompairStringResult.Equal Then
    '            frm = New frmStateCode(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ResponsiblePerson) = CompairStringResult.Equal Then
    '            frm = New frmResponsiblePerson(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCreateRemittance) = CompairStringResult.Equal Then
    '            frm = New FrmCreateRemittance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.remittanceentry) = CompairStringResult.Equal Then
    '            frm = New Frmremittanceentry(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnAPInvoiceEntryTDS) = CompairStringResult.Equal Then
    '            frm = New FrmAPInvoiceEntryTDS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-------------------------TDS Report -----------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmrptTDSLedger) = CompairStringResult.Equal Then
    '            frm = New FrmrptTDSLedger()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TDSForm26Q) = CompairStringResult.Equal Then
    '            frm = New form26Q27Q()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TDSSectionSummaryReport) = CompairStringResult.Equal Then
    '            frm = New FrmTDSsectionSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Form16AReport) = CompairStringResult.Equal Then
    '            frm = New FrmForm16A()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ Purchase Order Master---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPurchaseSetting) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPurchaseRegisterReport) = CompairStringResult.Equal Then
    '            frm = New RptPurchaseRegisterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorItemDetails) = CompairStringResult.Equal Then
    '            frm = New frmVendorItemDetails(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ Purchase Order---------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseRequistion) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseRequistion()
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStoreRequistion) = CompairStringResult.Equal Then
    '            frm = New frmStoreRequistion()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPendingApprovalOfReq) = CompairStringResult.Equal Then
    '            frm = New FrmPendingReqForApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RequisitSubTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmRequisitSubTypeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf strProgramCode = clsUserMgtCode.RFQ Then
    '            frm = New FrmRFQ()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorQuotation) = CompairStringResult.Equal Then
    '            frm = New frmVendorQuotation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorComparison) = CompairStringResult.Equal Then
    '            frm = New FrmVendorComparison1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.VendorComparisonApproval) = CompairStringResult.Equal Then
    '            frm = New frmVendorComparisonApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseOrder) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseOrder(strProgramCode)
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseOrder) = CompairStringResult.Equal Then
    '            '    frm = New frmPurchaseOrder()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPurchaseSchedule) = CompairStringResult.Equal Then
    '            frm = New FrmPurchaseSchedule()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnMRN) = CompairStringResult.Equal Then
    '            frm = New frmMRN()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnSRN) = CompairStringResult.Equal Then
    '            frm = New frmSRN(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SRNReturn) = CompairStringResult.Equal Then
    '            frm = New frmSRNReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseInvoice) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseReturn) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'KUNAL > CLIENT : UDIL > TICKET : BM00000010226 
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnNRGP) = CompairStringResult.Equal Then
    '            frm = New frmNRGPBooking(clsUserMgtCode.mbtnNRGP)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnGatePass) = CompairStringResult.Equal Then
    '            frm = New frmRGP()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnIssueReturn) = CompairStringResult.Equal Then
    '            frm = New frmIssueReturn()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRoutewiseSaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmRoutewiseSaleReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmKPIReport) = CompairStringResult.Equal Then
    '            frm = New FrmKPIReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProvSaleExcel) = CompairStringResult.Equal Then
    '            frm = New FrmProvSaleExcel(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            ''richa
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemQuantityInformation) = CompairStringResult.Equal Then
    '            '    frm=New FrmItemQuantityInformation()
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '------------------------Purchase Order Report -------------------------------

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RM_Consumption_Detail) = CompairStringResult.Equal Then
    '            frm = New RM_Consumption_Detail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMaterialSendforJobWork) = CompairStringResult.Equal Then
    '            frm = New RptMaterialSendForJobWork()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMaterialReceivedAfterJobWork) = CompairStringResult.Equal Then
    '            frm = New RptMaterialReceivedAfterJobWork()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptBalanceStockForJobWork) = CompairStringResult.Equal Then
    '            frm = New RptBalanceStockForJobWork()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptRGPWiseJobWork) = CompairStringResult.Equal Then
    '            frm = New RptRGPWiseJobWork()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmIndentReport) = CompairStringResult.Equal Then
    '            frm = New FrmIndentReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmExpiredItemDetails) = CompairStringResult.Equal Then
    '            frm = New FrmExpiredItemDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmConsumptionReport1) = CompairStringResult.Equal Then
    '            frm = New FrmConsumptionReport1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DebitAdviseReport) = CompairStringResult.Equal Then
    '            frm = New FrmDebitAdviseReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Vendor_Rating_Rejection) = CompairStringResult.Equal Then
    '            frm = New Vendor_Rating_Rejection()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Store_Receipt_Note) = CompairStringResult.Equal Then
    '            frm = New Store_Receipt_Note()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnDailyRcptNoteSummary) = CompairStringResult.Equal Then
    '            frm = New FrmDailyReceipNoteSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Parti_VS_Rejected) = CompairStringResult.Equal Then
    '            frm = New Parti_VS_Rejected()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchaseOrderReport) = CompairStringResult.Equal Then
    '            frm = New FrmPurchaseOrderReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPo_action) = CompairStringResult.Equal Then
    '            frm = New frmPo_action()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingRequisitionQty) = CompairStringResult.Equal Then
    '            frm = New FrmPendingRequisitionQty()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "FrmPendingPO_Qty") = CompairStringResult.Equal Then
    '            frm = New FrmPendingPO_Qty()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingGrn_Qty) = CompairStringResult.Equal Then
    '            frm = New FrmPendingGrn_Qty()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingMrn_Qty) = CompairStringResult.Equal Then
    '            frm = New FrmPendingMrn_Qty
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingSrn_Qty) = CompairStringResult.Equal Then
    '            frm = New FrmPendingSrn_Qty
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, "FrmPendingInvoice_Qty") = CompairStringResult.Equal Then
    '            frm = New FrmPendingInvoice_Qty()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MRDAReport) = CompairStringResult.Equal Then
    '            frm = New FrmMRDAReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmIssueOrReturnItemWiseSummary) = CompairStringResult.Equal Then
    '            frm = New FrmIssueOrReturnItemWiseSummary()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchasebookReport1) = CompairStringResult.Equal Then
    '            frm = New FrmPurchasebookReport1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStockAnalysis) = CompairStringResult.Equal Then
    '            frm = New FrmStockAnalysis1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMorningReport) = CompairStringResult.Equal Then
    '            frm = New FrmMorningReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AddCharge) = CompairStringResult.Equal Then
    '            frm = New FrmAdditionalCharge1(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSrnReport) = CompairStringResult.Equal Then
    '            frm = New FrmSrnReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PJVReport) = CompairStringResult.Equal Then
    '            frm = New FrmPJVReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.StockStatement) = CompairStringResult.Equal Then
    '            frm = New FrmStockStatementReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchaseOrderRegister) = CompairStringResult.Equal Then
    '            frm = New FrmPurchaseOrderRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemWiseDispatchLedger3) = CompairStringResult.Equal Then
    '            'frm = New FrmItemWiseDispatchLedger3()
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' changed by panch raj on 08-05-18 against ticket No: KDI/04/05/18-000295
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnRGP_NRGP_Rpt) = CompairStringResult.Equal Then
    '            frm = New frmRGP_NRGP_Rpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DetailofWtdPriceofRawMaterial) = CompairStringResult.Equal Then
    '            frm = New FrmWTDRpt()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorWiseReturnableGoodBalance) = CompairStringResult.Equal Then
    '            frm = New FrmVendorWiseReturnableGoodBalance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnStoreLedger) = CompairStringResult.Equal Then
    '            frm = New FrmStoresLedgerNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnStoresLedger) = CompairStringResult.Equal Then
    '            frm = New FrmStoresLedger()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRGP_Register_NRGP) = CompairStringResult.Equal Then
    '            frm = New FrmRGP_Register_NRGP()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'KUNAL > TICKET : BM00000010298 > CLIENT : UDL > DATE : 28-NOV-2016
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRpt_OutStnd_Items_RGP) = CompairStringResult.Equal Then
    '            frm = New FrmRpt_OutStnd_Items_RGP()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFreightCosting) = CompairStringResult.Equal Then
    '            frm = New FrmFreightCosting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptPurchaseReturnBook) = CompairStringResult.Equal Then
    '            frm = New FrmRptPurchaseReturnBook()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPurchaseOrderList) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseOrderList()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'added by stuti on 01/03/2017 for kdil
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPurchaseOrderAmd) = CompairStringResult.Equal Then
    '            frm = New frmPOAmendmentReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SRNReturnListCancellation) = CompairStringResult.Equal Then
    '            frm = New frmSRNReturnListForCancellation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCapexRegister) = CompairStringResult.Equal Then
    '            frm = New rptCapexPurchaseRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDayWisePurchasePriceReport) = CompairStringResult.Equal Then
    '            frm = New RptDayWisePurchasePriceReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPurchasePlanReport) = CompairStringResult.Equal Then
    '            frm = New RptPurchasePlanReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPurchaseMaterialRegister) = CompairStringResult.Equal Then
    '            frm = New RptPurchaseMaterialRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnGRN) = CompairStringResult.Equal Then
    '            frm = New frmGRN
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'Pankaj------------------------GRN Report ------------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.POWeighment) = CompairStringResult.Equal Then
    '            frm = New frmPOWeighment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.POUnloading) = CompairStringResult.Equal Then
    '            frm = New frmPOUnloading
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnGRNReport) = CompairStringResult.Equal Then
    '            frm = New FrmGRNReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptUnpostedPO) = CompairStringResult.Equal Then
    '            frm = New frmRptUnpostedPO()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            'DATE : 14-FEB-2017 , CLIENT : UDL -- MONTHLY CONSUMPTION REPORT  =====
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMonthlyConsumptionReport) = CompairStringResult.Equal Then
    '            frm = New frmMonthlyConsumptionReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '            '--------Added By--Pankaj Kumar----------------Fixed Assets----------------------------------------------
    '            '================================Master======================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.fixedsetting) = CompairStringResult.Equal Then
    '            frm = New FrmFixedSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AssetSegment) = CompairStringResult.Equal Then
    '            frm = New FrmAssetSegment()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Template) = CompairStringResult.Equal Then
    '            frm = New FrmTemplateMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DepAccSets) = CompairStringResult.Equal Then
    '            frm = New FrmDepAccountSet()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Categories) = CompairStringResult.Equal Then
    '            frm = New FrmCategories()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDepreciationField) = CompairStringResult.Equal Then
    '            frm = New FrmDepreciationField()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetGroups) = CompairStringResult.Equal Then
    '            frm = New FrmGroups()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetBookMaster) = CompairStringResult.Equal Then
    '            frm = New frmAssetBookMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDepreciationMethod) = CompairStringResult.Equal Then
    '            frm = New frmDepreciationMethod()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DepPeriod) = CompairStringResult.Equal Then
    '            frm = New FrmDepreciationPeriods()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAMAcquisitionCode) = CompairStringResult.Equal Then
    '            frm = New FrmAMAcquisitionCode()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FAMergeAcquisitionEntry) = CompairStringResult.Equal Then
    '            frm = New frmFAMergeAsset()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSecondaryCustomer) = CompairStringResult.Equal Then
    '            frm = New FrmSecondaryCustomer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmConsumerDetailsForm) = CompairStringResult.Equal Then
    '            frm = New frmConsumerMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==============================Transaction===================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FAAcquisitionEntry) = CompairStringResult.Equal Then
    '            frm = New frmAcquisionEntry()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FAAssetDepreciation) = CompairStringResult.Equal Then
    '            frm = New FrmAssetDepreciation()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FADisposalEntry) = CompairStringResult.Equal Then
    '            frm = New frmAssetScrapSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVisi_Install_Pullout) = CompairStringResult.Equal Then
    '            frm = New FrmVisi_Install_Pullout()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAsset_Issue_Return) = CompairStringResult.Equal Then
    '            frm = New FrmAsset_Issue_Return()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetRequisition) = CompairStringResult.Equal Then
    '            frm = New frmAssetRequisition(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetStoreRequistion) = CompairStringResult.Equal Then
    '            frm = New frmAssetStoreRequistion()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSecondaryCustomerSale) = CompairStringResult.Equal Then
    '            frm = New FrmSecondaryCustomerSale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIssueItemsToAsset) = CompairStringResult.Equal Then
    '            frm = New frmItemIssueToAssembledAsset
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FAAssetWork) = CompairStringResult.Equal Then
    '            frm = New frmAssetWork
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '===============================Report=======================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAssetRegister) = CompairStringResult.Equal Then
    '            frm = New FrmAssetRegister()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAssetDetail) = CompairStringResult.Equal Then
    '            frm = New FrmAssetDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDisposalDetail) = CompairStringResult.Equal Then
    '            frm = New FrmDisposalDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVisi_Install_Pullout_Report) = CompairStringResult.Equal Then
    '            frm = New FrmVisi_Install_Pullout_Report()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAsset_Issue_Return_Report) = CompairStringResult.Equal Then
    '            frm = New FrmAsset_Issue_Return_Report()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDistributor_VS_SecondaryCustomer_Sale) = CompairStringResult.Equal Then
    '            frm = New FrmDistributor_VS_SecondaryCustomer_Sale()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptFARReport) = CompairStringResult.Equal Then
    '            frm = New RptFAFARReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-------------------------------------Fixed Assets(Ends Here)--------------------------------------------


    '            'Dipti----------------------Utility---------------------------------------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCreateReceiptAgainstSale) = CompairStringResult.Equal Then
    '            frm = New FrmCreateReceiptAgainstSales()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnCreateReceiptAgainstInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmCreateReceiptAgainstInvoice()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnTakeBackup) = CompairStringResult.Equal Then
    '            frm = New FrmBackup()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnRestoreDB) = CompairStringResult.Equal Then
    '            Dim strMsg As String = "Your current tasks will be closed." + Environment.NewLine
    '            strMsg += "Do you want to continue?"
    '            If clsCommon.MyMessageBoxShow(strMsg, Me.Text, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
    '                RadDock1.RemoveAllDocumentWindows()
    '                SplitPanel3.Collapsed = True
    '                SplitPanel2.Collapsed = True
    '                SplitPanel3.Collapsed = True
    '                SplitPanel4.Collapsed = False
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, "calc") = CompairStringResult.Equal Then
    '            System.Diagnostics.Process.Start("calc.exe")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPendingApproval1) = CompairStringResult.Equal Then
    '            frm = New FrmPendingAproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBulkPostingNew) = CompairStringResult.Equal Then
    '            frm = New FrmBulkPostingNew()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.lockTransaction) = CompairStringResult.Equal Then
    '            frm = New FrmLockTransaction1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUserPerformanceDetail) = CompairStringResult.Equal Then
    '            frm = New FrmUserPerformanceDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStockReport) = CompairStringResult.Equal Then
    '            frm = New FrmStockReport(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmReconciliationSetting) = CompairStringResult.Equal Then
    '            frm = New FrmReconciliationSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmUtilityForm) = CompairStringResult.Equal Then
    '            frm = New FrmPWD(Nothing)
    '            frm.strCode = "TempProvisional"
    '            frm.strType = "TempProvisional"
    '            frm.ShowDialog()
    '            If frm.isPasswordCorrect Then
    '                Dim frmNew As New FrmUtility()
    '                formShow(frmNew, strProgramCode, strProgramName, True, "")
    '                'Try
    '                '    clsSaleHead.SetTempProvisionSale()
    '                'Catch ex As Exception
    '                '    common.clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
    '                'End Try
    '            End If

    '            '==============================PAYROLL===================================
    '            '===============================Setup=======================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryAccountSetting) = CompairStringResult.Equal Then
    '            frm = New frmSalaryAccountSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDepartmentMaster) = CompairStringResult.Equal Then
    '            frm = New frmDepartmentMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSubDepartmentMaster) = CompairStringResult.Equal Then
    '            frm = New frmSubDepartmentMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmEmployeeTransfer) = CompairStringResult.Equal Then
    '            frm = New FrmEmployeeTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSkillMaster) = CompairStringResult.Equal Then
    '            frm = New frmSkillMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLanguageMaster) = CompairStringResult.Equal Then
    '            frm = New frmLanguageMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCourseMaster) = CompairStringResult.Equal Then
    '            frm = New frmCourseMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmShiftMaster) = CompairStringResult.Equal Then
    '            frm = New frmShiftMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDocumentMaster) = CompairStringResult.Equal Then
    '            frm = New frmDocumentMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPayPeriodMaster) = CompairStringResult.Equal Then
    '            frm = New frmPayPeriodMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCountryMaster) = CompairStringResult.Equal Then
    '            frm = New frmCountryMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReligionMaster) = CompairStringResult.Equal Then
    '            frm = New frmReligionMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCurrencyMaster) = CompairStringResult.Equal Then
    '            frm = New frmCurrencyMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCastCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New frmCastCategoryMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGradeMaster) = CompairStringResult.Equal Then
    '            frm = New frmGradeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStateMaster) = CompairStringResult.Equal Then
    '            frm = New frmStateMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDevisionMaster) = CompairStringResult.Equal Then
    '            frm = New frmDevisionMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOccupationMaster) = CompairStringResult.Equal Then
    '            frm = New frmOccupationMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPFRulesMaster) = CompairStringResult.Equal Then
    '            frm = New frmPFRulesMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmESIRulesMaster) = CompairStringResult.Equal Then
    '            frm = New frmESIRulesMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOTMaster) = CompairStringResult.Equal Then
    '            frm = New frmOTMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAttendanceMaster) = CompairStringResult.Equal Then
    '            frm = New frmAttendanceMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBranchMaster) = CompairStringResult.Equal Then
    '            frm = New frmBranchMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBonusMaster) = CompairStringResult.Equal Then
    '            frm = New frmBonusMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOTSlab) = CompairStringResult.Equal Then
    '            frm = New frmOTSlab(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPTSlab) = CompairStringResult.Equal Then
    '            frm = New frmPTSlab(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmConveyanceRateMaster) = CompairStringResult.Equal Then
    '            frm = New frmConveyanceRateMaster(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmODMaster) = CompairStringResult.Equal Then
    '            frm = New frmODMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ' KUNAL > TICKET : BM00000009879 > 30 - SEP - 2016 
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPayrollDesignationMaster) = CompairStringResult.Equal Then
    '            frm = New frmDesignationMaster(lblUserCode.Text, lblCompanyCode.Text, clsUserMgtCode.frmPayrollDesignationMaster)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==============================Transaction===================================

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOTSheet) = CompairStringResult.Equal Then
    '            frm = New frmOTSheet()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveMaster) = CompairStringResult.Equal Then
    '            frm = New frmLeaveMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveSetting) = CompairStringResult.Equal Then
    '            frm = New frmLeaveSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGeneralHolidays) = CompairStringResult.Equal Then
    '            frm = New frmGeneralHolidays()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveOpeningBalance) = CompairStringResult.Equal Then
    '            frm = New frmLeaveOpeningBalance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveStartingDateSetting) = CompairStringResult.Equal Then
    '            frm = New frmLeaveStartingDateSetting()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveAllotment) = CompairStringResult.Equal Then
    '            frm = New frmLeaveAllotment()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPayHeadDefinitions) = CompairStringResult.Equal Then
    '            frm = New frmPayHeadDefinitions()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryStructure) = CompairStringResult.Equal Then
    '            frm = New frmSalaryStructure()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMapPayHeadsToSalaStructure) = CompairStringResult.Equal Then
    '            frm = New frmMapPayHeadsToSalaStructure()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWeeklyHolidays) = CompairStringResult.Equal Then
    '            frm = New frmWeeklyHolidays()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveApplication) = CompairStringResult.Equal Then
    '            frm = New frmLeaveApplication()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMonthlyAttendance) = CompairStringResult.Equal Then
    '            frm = New frmMonthlyAttendance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveAdjustment) = CompairStringResult.Equal Then
    '            frm = New frmLeaveAdjustment()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGenerateBonus) = CompairStringResult.Equal Then
    '            frm = New frmGenerateBonus()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDailyAttendance) = CompairStringResult.Equal Then
    '            frm = New frmDailyAttendance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHourlyAttendance) = CompairStringResult.Equal Then
    '            frm = New frmHourlyAttendance()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAdjustmentVoucher) = CompairStringResult.Equal Then
    '            frm = New frmAdjustmentVoucher()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TOOLTYPE) = CompairStringResult.Equal Then
    '            frm = New FrmToolType()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReimbursementDetails) = CompairStringResult.Equal Then
    '            frm = New frmReimbursementDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAllowanceDetails) = CompairStringResult.Equal Then
    '            frm = New frmAllowanceDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeductionDetails) = CompairStringResult.Equal Then
    '            frm = New frmDeductionDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmApplyLoan) = CompairStringResult.Equal Then
    '            frm = New frmApplyLoan()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployee_Master) = CompairStringResult.Equal Then
    '            frm = New frmEmployee_Master()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmpSalary) = CompairStringResult.Equal Then
    '            frm = New frmEmployee_Salary
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployeeStatus) = CompairStringResult.Equal Then
    '            frm = New frmEmployee_Status
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLoanAdjustment) = CompairStringResult.Equal Then
    '            frm = New frmLoanAdjustment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLoanGeneration) = CompairStringResult.Equal Then
    '            frm = New frmLoanGeneration
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployeeIncrement) = CompairStringResult.Equal Then
    '            '    frm = New frmEmployeeIncrement
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryGeneration) = CompairStringResult.Equal Then
    '            frm = New frmSalaryGeneration
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptEmployeeAdvanceLedger) = CompairStringResult.Equal Then
    '            frm = New rptEmployeeAdvanceLedger
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployeeGratuity) = CompairStringResult.Equal Then
    '            frm = New FrmEmployeeGratuity
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLTAClaim) = CompairStringResult.Equal Then
    '            frm = New frmLTAClaim
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMediclaimEntry) = CompairStringResult.Equal Then
    '            frm = New FrmMediclaimEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFullAndFinalSettlement) = CompairStringResult.Equal Then
    '            Dim frm As New frmEmpFullAndFinalSettlement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployeeShiftChange) = CompairStringResult.Equal Then
    '            Dim frm As New frmEmployeeShiftChange
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmODSheet) = CompairStringResult.Equal Then
    '            Dim frm As New frmODSheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmConveyanceClaim) = CompairStringResult.Equal Then
    '            Dim frm As New frmConveyanceClaim
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPayrollSetting) = CompairStringResult.Equal Then
    '            Dim frm As New frmPayrollSetting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmEmpIncrement) = CompairStringResult.Equal Then
    '            Dim frm As New FrmEmpIncrement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSentSalarySlip) = CompairStringResult.Equal Then
    '            Dim frm As New FrmSentSalarySlip
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAllotmentOfLeaves) = CompairStringResult.Equal Then
    '            Dim frm As New FrmAllotmentOfLeaves
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==============================Report===================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryGenerationRegister) = CompairStringResult.Equal Then
    '            frm = New frmSalaryGenerationRegister(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryGenerationRegisterArrear) = CompairStringResult.Equal Then
    '            frm = New frmSalaryGenerationRegister(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAllownceRegister) = CompairStringResult.Equal Then
    '            frm = New frmAllownceRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeductionRegister) = CompairStringResult.Equal Then
    '            frm = New frmDeductionRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReimbursementRegister) = CompairStringResult.Equal Then
    '            frm = New frmReimbursementRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAdjustmentRegister) = CompairStringResult.Equal Then
    '            frm = New frmAdjustmentRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAttendanceRegister) = CompairStringResult.Equal Then
    '            frm = New frmAttendanceRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLeaveRegisterReport) = CompairStringResult.Equal Then
    '            frm = New frmLeaveRegisterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmployeeRegister) = CompairStringResult.Equal Then
    '            frm = New frmEmployeeRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPF_ESI_Reports) = CompairStringResult.Equal Then
    '            frm = New frmPF_ESI_Reports
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptEmployeeBday6) = CompairStringResult.Equal Then
    '            frm = New RptEmployeeBday6
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-------------------------------------Monthly Report -----------------------------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaySlip_Reports) = CompairStringResult.Equal Then
    '            frm = New frmPaySlip_Reports
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalarySheet_Reports) = CompairStringResult.Equal Then
    '            frm = New frmSalaryGenerationRegister(strProgramCode)
    '            frm.Text = "Salary Sheet "
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryAbstractReport) = CompairStringResult.Equal Then
    '            frm = New frmSalaryAbstractReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAnnualCensusReport) = CompairStringResult.Equal Then
    '            frm = New frmAnnualCensusReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAttendedDaysReport) = CompairStringResult.Equal Then
    '            frm = New frmAttendedDaysReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryVoucher_Reports) = CompairStringResult.Equal Then
    '            frm = New frmSalaryVoucher_Reports
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankStatement_Reports) = CompairStringResult.Equal Then
    '            frm = New frmBankStatement_Reports
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOT_Reports) = CompairStringResult.Equal Then
    '            frm = New frmOT_Reports
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVarianceReport) = CompairStringResult.Equal Then
    '            frm = New frmVarianceReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryComponentDetails) = CompairStringResult.Equal Then
    '            frm = New frmSalaryComponentDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAditionalEarning_DeductionReport) = CompairStringResult.Equal Then
    '            frm = New frmAditionalEarning_DeductionReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryCertificate) = CompairStringResult.Equal Then
    '            'frm = New frmSalaryCertificate
    '            '     formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPF_Covering_Letter) = CompairStringResult.Equal Then
    '            frm = New frmPF_Covering_Letter
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryIncrementReport) = CompairStringResult.Equal Then
    '            frm = New frmSalaryIncrementReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBankSummary_Report) = CompairStringResult.Equal Then
    '            frm = New frmBankSummary_Report
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeductionDetailsReport) = CompairStringResult.Equal Then
    '            frm = New frmDeductionDetailsReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVoucherPaymentsRegister) = CompairStringResult.Equal Then
    '            frm = New frmVoucherPaymentsRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmEmp_Id) = CompairStringResult.Equal Then
    '            frm = New frmEmp_Id
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLabelPrinting) = CompairStringResult.Equal Then
    '            frm = New frmLabelPrinting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmForm_T) = CompairStringResult.Equal Then
    '            frm = New frmForm_T
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            ''''''''''''''''''''''''''''''''''''''''''''Production Added by shipra------------------'''''''''''''
    '            ''''''''''''''''''''''''''''''''''''''''''''Master------------------'''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ACCSETMFGSTD) = CompairStringResult.Equal Then
    '            frm = New FrmAccountSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ACCSETMFGDairy) = CompairStringResult.Equal Then
    '            frm = New FrmAccountSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ACCSETMFGPepsi) = CompairStringResult.Equal Then
    '            frm = New FrmAccountSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ITEMCATEGORY) = CompairStringResult.Equal Then
    '            frm = New FrmItemProductionCategory
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TOOL) = CompairStringResult.Equal Then
    '            frm = New FrmToolMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SETTSTD) = CompairStringResult.Equal Then
    '            frm = New FrmSettings(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SETTPep) = CompairStringResult.Equal Then
    '            frm = New FrmSettings(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EXPENSE) = CompairStringResult.Equal Then
    '            frm = New FrmExpenseHead
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWorkCenterMaster) = CompairStringResult.Equal Then
    '            frm = New frmWorkCenterMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmResourceMaster) = CompairStringResult.Equal Then
    '            frm = New frmResourceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ALTER) = CompairStringResult.Equal Then
    '            frm = New FrmAlternateItem
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBillOfMaterialPepsi) = CompairStringResult.Equal Then
    '            frm = New frmBillOfMaterial
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '            'If OpenProcessProductionBOm Then
    '            '    frm = New frmBOM
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'Else
    '            '    frm = New frmBillOfMaterial
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBillOfMaterialDairy) = CompairStringResult.Equal Then
    '            frm = New frmBOM
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProductionLogSheet) = CompairStringResult.Equal Then
    '            'Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '            'If OpenProcessProductionBOm Then
    '            frm = New FrmProcessProductionLogSheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPPLogSheetMaster) = CompairStringResult.Equal Then
    '            frm = New frmPPLogSheetMaster(clsUserMgtCode.frmPPLogSheetMaster)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPPLogSheetMaster_QC) = CompairStringResult.Equal Then
    '            frm = New frmPPLogSheetMaster(clsUserMgtCode.frmPPLogSheetMaster_QC)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorItemQCMapping) = CompairStringResult.Equal Then
    '            frm = New FrmVendorItemQCMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQualityCheckForSRN) = CompairStringResult.Equal Then
    '            frm = New FrmQualityCheckForSRN(clsUserMgtCode.frmQualityCheckForSRN, "Incoming")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog) 'frmQualityCheckApprovalForSRN
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQualityCheckApprovalForSRN) = CompairStringResult.Equal Then
    '            frm = New FrmQualityCheckApprovalForSRN(clsUserMgtCode.frmQualityCheckApprovalForSRN, "Incoming")
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOperationMaster) = CompairStringResult.Equal Then
    '            frm = New frmOperationMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBOMImport) = CompairStringResult.Equal Then
    '            frm = New frmBOMImport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.COSTMAINTAIN) = CompairStringResult.Equal Then
    '            frm = New FrmCostMaintainance
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PRO) = CompairStringResult.Equal Then
    '            frm = New FrmProductionLines
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProcessMaster1) = CompairStringResult.Equal Then
    '            frm = New FrmProcessMaster1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSectionMaster) = CompairStringResult.Equal Then
    '            frm = New FrmSectionMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmStageMaster) = CompairStringResult.Equal Then
    '            frm = New frmStageMasters
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSectionStageMapping) = CompairStringResult.Equal Then
    '            frm = New FrmSectionStageMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''''''''''''''''''''''''''''''''''''''''''End of Master''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            ''''''''''''''''''''''''''''''''''''''''''''Transaction'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBillOfMaterialCosting) = CompairStringResult.Equal Then
    '            'Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '            'If OpenProcessProductionBOm Then
    '            '    frm = New frmBOM
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            'Else
    '            frm = New frmBillOfMaterialCosting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'End If
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanning) = CompairStringResult.Equal Then
    '            '    Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '            '    If OpenProcessProductionBOm Then
    '            '        frm = New FrmProcessProductionPlanning
    '            '             formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '    Else
    '            '        frm = New frmProductionPlanning
    '            '             formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '    End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanningSTD) = CompairStringResult.Equal Then
    '            frm = New frmProductionPlanning(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMRPAutoMobile) = CompairStringResult.Equal Then
    '            frm = New frmMRPAutoMobile()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanningPepsi) = CompairStringResult.Equal Then
    '            frm = New FrmProcessProductionPlanning
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanningDairy) = CompairStringResult.Equal Then
    '            frm = New FrmProcessProductionPlanning
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProductionIssueEntry) = CompairStringResult.Equal Then
    '            frm = New FrmProcessProductionIssueEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProductionStandardization) = CompairStringResult.Equal Then
    '            frm = New frmProcessProductionStandardization
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProductionStageProcess) = CompairStringResult.Equal Then
    '            frm = New frmProcessProductionStageProcess
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '  ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProductionStageProcess) = CompairStringResult.Equal Then
    '            '     frm = New RptDairyProductionWreckageReport
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionEntry) = CompairStringResult.Equal Then
    '            Dim ActivateProductionWithoutBatch As String = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.ActivateProductionWithoutBatch, clsFixedParameterCode.ActivateProductionWithoutBatch, Nothing))
    '            If clsCommon.CompairString(ActivateProductionWithoutBatch, "1") = CompairStringResult.Equal Then
    '                frm = New frmProductionEntryWithoutBatch
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmProductionEntry
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '            ActivateProductionWithoutBatch = Nothing
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionEntryWithoutBatch) = CompairStringResult.Equal Then
    '            '    frm = New frmProductionEntryWithoutBatch
    '            '    formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWreckageBooking) = CompairStringResult.Equal Then
    '            frm = New frmWreckage
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProcessProdReturn) = CompairStringResult.Equal Then
    '            frm = New frmProcessProductionReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssembDis) = CompairStringResult.Equal Then
    '            frm = New frmAssembDis
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMRP) = CompairStringResult.Equal Then
    '            frm = New frmMRP
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDemoProductionPlanning) = CompairStringResult.Equal Then
    '            'frm=New frmDemoProductionPlanning
    '            frm = New frmProductionPlanningDemo
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionRequisition) = CompairStringResult.Equal Then
    '            frm = New frmProductionRequisition
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRiceBOM) = CompairStringResult.Equal Then
    '            frm = New frmRiceBOM
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRiceMixingEntry) = CompairStringResult.Equal Then
    '            frm = New FrmRiceMixingEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRiceProcessingEntry) = CompairStringResult.Equal Then
    '            frm = New FrmRiceProcessingEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionItemSerialReplace) = CompairStringResult.Equal Then
    '            frm = New frmProductionItemSerialReplace
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionSerializedReport) = CompairStringResult.Equal Then
    '            frm = New frmProductionSerializedReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionReceiptDemo) = CompairStringResult.Equal Then
    '            frm = New FrmProductionReceiptDemo
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmManufacturingOrder) = CompairStringResult.Equal Then
    '            frm = New frmManufacturingOrder
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrder) = CompairStringResult.Equal Then
    '            '    Dim OpenProcessProductionBOm As Boolean = clsDBFuncationality.getSingleValue("select IsBOMFromProcessProduction from TSPL_INV_PARAMETERS")
    '            '    If OpenProcessProductionBOm Then
    '            '        frm = New FrmProcessBatchOrder
    '            '             formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '    Else
    '            '        frm = New frmBatchOrder
    '            '             formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '    End If
    '            '==================preeti gupta==============
    '            '=============Production Report==============
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptproductionEntryReport) = CompairStringResult.Equal Then
    '            frm = New RptproductionEntryReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptItemConsumptionReport) = CompairStringResult.Equal Then
    '            frm = New RptItemConsumptionReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptProductionIssueStatusReport) = CompairStringResult.Equal Then
    '            frm = New RptProductionIssueStatus
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptWTPReport) = CompairStringResult.Equal Then
    '            frm = New RptWIPReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDairyProductionWreckageReport) = CompairStringResult.Equal Then
    '            frm = New RptWreckageReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptJobWorkProduction) = CompairStringResult.Equal Then
    '            frm = New rptJobWorkProduction
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptAvailableQtyForProduction) = CompairStringResult.Equal Then
    '            frm = New rptAvailableQtyForProduction
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptBatchStatus) = CompairStringResult.Equal Then
    '            frm = New RptBatchStatusReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSectionWiseStockReport) = CompairStringResult.Equal Then
    '            frm = New RptSectionWiseStockReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrderSTD) = CompairStringResult.Equal Then
    '            frm = New frmBatchOrder(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrderPepsi) = CompairStringResult.Equal Then
    '            frm = New frmBatchOrder(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrderDairy) = CompairStringResult.Equal Then
    '            frm = New FrmProcessBatchOrder
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStoreIssueSTD) = CompairStringResult.Equal Then
    '            frm = New frmStoreIssue(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmStoreIssuePepsi) = CompairStringResult.Equal Then
    '            frm = New frmStoreIssue(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionReturnSTD) = CompairStringResult.Equal Then
    '            frm = New frmProductionReturn(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionReturnPep) = CompairStringResult.Equal Then
    '            frm = New frmProductionReturn(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionReceiptSTD) = CompairStringResult.Equal Then
    '            frm = New frmProductionReceipt(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLabourWorkingSheet) = CompairStringResult.Equal Then
    '            frm = New FrmLabourWorkingSheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''''''''''''''''''''''''''''''''''''''''''End of Transaction''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            ''''''''''''''''''''''''''''''''''''''''''''Reports''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Resource) = CompairStringResult.Equal Then
    '            frm = New RptListOf_Resource
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LTOOL) = CompairStringResult.Equal Then
    '            frm = New RptListOfToolType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LACCt) = CompairStringResult.Equal Then
    '            frm = New RptListOfAccountSet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LALT) = CompairStringResult.Equal Then
    '            frm = New RptListOfAlternateItem
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LWC) = CompairStringResult.Equal Then
    '            frm = New RptListOfWorkCenter
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LOIC) = CompairStringResult.Equal Then
    '            frm = New rptListOfItemCost
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LOPER) = CompairStringResult.Equal Then
    '            frm = New RptListOfOperations
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.PRODREPORT) = CompairStringResult.Equal Then
    '            frm = New FrmListOfProductionLines
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LToolT) = CompairStringResult.Equal Then
    '            frm = New RptListOfTools
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmListOfBOM) = CompairStringResult.Equal Then
    '            frm = New frmListOfBOM
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIssueReturnItemWiseReportSTD) = CompairStringResult.Equal Then
    '            frm = New frmIssueReturnItemWiseReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIssueReturnItemWiseReportPepsi) = CompairStringResult.Equal Then
    '            frm = New frmIssueReturnItemWiseReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDatewiseProduction) = CompairStringResult.Equal Then
    '            frm = New frmDatewiseProduction
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanReportSTD) = CompairStringResult.Equal Then
    '            frm = New frmProductionPlanReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionPlanReportPepsi) = CompairStringResult.Equal Then
    '            frm = New frmProductionPlanReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLineProductivity) = CompairStringResult.Equal Then
    '            frm = New frmLineProductivity
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrderReportSTD) = CompairStringResult.Equal Then
    '            frm = New frmBatchOrderReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBatchOrderReportPepsi) = CompairStringResult.Equal Then
    '            frm = New frmBatchOrderReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGraphicalBatchOrder) = CompairStringResult.Equal Then
    '            frm = New frmGraphicalBatchOrder
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGraphicalCategorywiseProduction) = CompairStringResult.Equal Then
    '            frm = New frmGraphicalCategorywiseProduction
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmListofRequisition) = CompairStringResult.Equal Then
    '            frm = New frmListofRequisition
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOperaterEfficiencyReport) = CompairStringResult.Equal Then
    '            frm = New FrmOperatorEfficiencyReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            ''BI Forms
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BICreateReport) = CompairStringResult.Equal Then
    '            frm = New frmCreateBIReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BICreateFilter) = CompairStringResult.Equal Then
    '            frm = New frmCreateBIFilter
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BICreateDashBoard) = CompairStringResult.Equal Then
    '            frm = New FrmCreateDashBoard
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BIMonthWiseSale) = CompairStringResult.Equal Then
    '            frm = New FrmBIMonthWiseSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BITopCustomer) = CompairStringResult.Equal Then
    '            frm = New frmBITopCustomer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BITopItemCategory) = CompairStringResult.Equal Then
    '            frm = New frmBITopItemCategory
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionVarianceSTD) = CompairStringResult.Equal Then
    '            frm = New frmProductionVariance(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductionVariancePepsi) = CompairStringResult.Equal Then
    '            frm = New frmProductionVariance(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BIMonthWisePurchase) = CompairStringResult.Equal Then
    '            frm = New FrmBIMonthWisePurchase
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BITopVendor) = CompairStringResult.Equal Then
    '            frm = New frmBITopVendor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BITopItemCategoryPurchase) = CompairStringResult.Equal Then
    '            frm = New frmBITopItemCategoryPurchase
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BIMonthWiseAssset) = CompairStringResult.Equal Then
    '            frm = New frmBIMonthWiseAsset
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BITopExpence) = CompairStringResult.Equal Then
    '            frm = New frmBITopExpence
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BIDashBoadr) = CompairStringResult.Equal Then
    '            frm = New frmBIDashBoard
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''''''''''''''''''''''''''''''''''''''''''End of Reports''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            ''''''''''''''''''''''''''''''''''''''''''''Project Management''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            ''''''''''''''''''''''''''''''''''''''''''''Setup (Project Management)''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPJCSettings) = CompairStringResult.Equal Then
    '            frm = New frmPJCSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCostTypes) = CompairStringResult.Equal Then
    '            frm = New frmCostType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPJCAccountSets) = CompairStringResult.Equal Then
    '            frm = New frmPJCAccountSetting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobMaster) = CompairStringResult.Equal Then
    '            frm = New frmJobMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTaskMaster) = CompairStringResult.Equal Then
    '            frm = New frmTaskMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPJCEmployeeMaster) = CompairStringResult.Equal Then
    '            frm = New frmPJCEmployeeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmUserApproval) = CompairStringResult.Equal Then
    '            frm = New FrmUserApproval
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBudgetMaintenance) = CompairStringResult.Equal Then
    '            frm = New FrmBudgetMaintenance
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ProjectMaster) = CompairStringResult.Equal Then
    '            frm = New FrmProjectMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmExpenseType) = CompairStringResult.Equal Then
    '            frm = New FrmExpenseType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' transaction
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTimeSheet) = CompairStringResult.Equal Then
    '            frm = New frmTimesheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUserLog) = CompairStringResult.Equal Then
    '            frm = New FrmUserLog
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProjectStatus) = CompairStringResult.Equal Then
    '            frm = New FrmProjectStatus
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssemblies) = CompairStringResult.Equal Then
    '            frm = New frmAssemblies
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPJCExpense) = CompairStringResult.Equal Then
    '            frm = New FrmPJCExpense
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '-Reports-------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProjectListReport) = CompairStringResult.Equal Then
    '            frm = New FrmProjectListReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProjectDetails) = CompairStringResult.Equal Then
    '            frm = New FrmProjectDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProjectProfitReport) = CompairStringResult.Equal Then
    '            frm = New FrmProjectProfitReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProfitAndLossPerforma) = CompairStringResult.Equal Then
    '            frm = New frmProfitAndLossPerforma
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptProfitAndLoss) = CompairStringResult.Equal Then
    '            frm = New frmRptProfitAndLoss
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''''''''''''''''''''''''''''''''''''''''''''end Project Management''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''Service''''''''''''''''''''''''''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmComplaintGroupMaster) = CompairStringResult.Equal Then
    '            frm = New frmComplaintGroupMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPrimaryReasonMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPrimaryReasonMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmComplaintMaster) = CompairStringResult.Equal Then
    '            frm = New frmComplaintMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPendingReasonMaster) = CompairStringResult.Equal Then
    '            frm = New frmPendingReasonMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemChargeCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New frmItemChargeCategoryMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemChargeFranchiseMappingMaster) = CompairStringResult.Equal Then
    '            frm = New FrmItemChargeFranchiseMappingMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetServiceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAssetServiceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------ TRANSACTION ---------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetAgreement) = CompairStringResult.Equal Then
    '            frm = New FrmAssetAgreement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetInstallPullOut) = CompairStringResult.Equal Then
    '            frm = New frmAssetInstallPullOut
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmComplaintDetailEntry) = CompairStringResult.Equal Then
    '            frm = New FrmComplaintDetailEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQuickComplaintDetailEntry) = CompairStringResult.Equal Then
    '            frm = New FrmQuickComplaintDetailEntry(objCommonVar.CurrentUserCode, objCommonVar.CurrentCompanyCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetDistatch) = CompairStringResult.Equal Then
    '            frm = New frmAssetDispatch
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCartMaintenanceEntry) = CompairStringResult.Equal Then
    '            frm = New FrmCartMaintenanceEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingComplaintDetail) = CompairStringResult.Equal Then
    '            frm = New FrmPendingComplaintDetail
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '--------------------REPORT-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFranchiseChargesReport) = CompairStringResult.Equal Then
    '            frm = New FrmFranchiseChargesReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomersListReport) = CompairStringResult.Equal Then
    '            frm = New FrmCustomersListReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPullOutRedeployReport) = CompairStringResult.Equal Then
    '            frm = New frmPullOutRedeployReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorBillDetails) = CompairStringResult.Equal Then
    '            frm = New frmVendorBillDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDaywisePendingComplaint) = CompairStringResult.Equal Then
    '            frm = New FrmDaywisePendingComplaint
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmClaimReport) = CompairStringResult.Equal Then
    '            frm = New FrmClaimReportNew
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAssetDetailReport) = CompairStringResult.Equal Then
    '            frm = New FrmAssetDetailReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmspareStockReport2) = CompairStringResult.Equal Then
    '            '    frm=New FrmspareStockReport2
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '            '--------------------MILK PROCUREMENT MASTER-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkCollectionLevels) = CompairStringResult.Equal Then
    '            frm = New frmMilkCollectionLevelsMain
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkAdvanceMaster) = CompairStringResult.Equal Then
    '            frm = New frmJWPriceCodeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkVehicleTypeMaster) = CompairStringResult.Equal Then
    '            frm = New VehicleTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkComponentMaster) = CompairStringResult.Equal Then
    '            frm = New MilkComponentMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkRateTypeMaster) = CompairStringResult.Equal Then
    '            frm = New MilkRateType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkShiftMaster) = CompairStringResult.Equal Then
    '            frm = New ShiftMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSeasonMaster) = CompairStringResult.Equal Then
    '            frm = New SeasonMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkRouteMaster) = CompairStringResult.Equal Then
    '            frm = New FrmMilkRouteMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkVehicleMaster) = CompairStringResult.Equal Then
    '            frm = New frmVehicleMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkCollectionArea) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkTransportRateMaster) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkComponentRateList) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVillageMaster) = CompairStringResult.Equal Then
    '            frm = New FrmVillageMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVSPMaster) = CompairStringResult.Equal Then
    '            frm = New frmVSPMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPrimaryTransporterMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPrimaryTransporterMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTankerTransporterMaster) = CompairStringResult.Equal Then
    '            frm = New frmTankerTransporterMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaster) = CompairStringResult.Equal Then
    '            frm = New FrmMCCMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmTankerMaster) = CompairStringResult.Equal Then
    '            frm = New FrmTankerMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVLCMaster) = CompairStringResult.Equal Then
    '            frm = New FrmVLCMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVLCMasterTarget) = CompairStringResult.Equal Then
    '            frm = New FrmVlcTargetMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeliveryTermsMaster) = CompairStringResult.Equal Then
    '            frm = New frmDeliveryTermsMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPriceChartUploader) = CompairStringResult.Equal Then
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.MilkProcuremntPickCLRInsteadOfSNF, clsFixedParameterCode.MilkProcuremntPickCLRInsteadOfSNF, Nothing)) > 0 Then
    '                frm = New frmPriceChartUploaderCLR
    '            Else
    '                frm = New FrmPriceChartUploader
    '            End If
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPriceChartMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPriceChartMaster(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkPricePlanning) = CompairStringResult.Equal Then
    '            Dim intPricePlan As Integer = clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.OpenPriceChartPlanningScreenOnTotalSolid, clsFixedParameterCode.OpenPriceChartPlanningScreenOnTotalSolid, Nothing))
    '            If intPricePlan = 1 Then
    '                frm = New frmPriceChartPlanMasterGHO
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ElseIf intPricePlan = 2 Then
    '                frm = New frmPriceChartPlanMasterGK
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmPriceChartPlanMaster
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPriceChartMaster_Bulk) = CompairStringResult.Equal Then
    '            frm = New FrmPriceChartMaster(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFormIssueReceiptEntry) = CompairStringResult.Equal Then
    '            frm = New FrmFormIssueReceiptEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmReceiptInvoiceMapping) = CompairStringResult.Equal Then
    '            frm = New FrmReceiptInvoiceMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCostCentreGroupStores) = CompairStringResult.Equal Then
    '            frm = New FrmCostCentreGroupStores
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVLCUploader) = CompairStringResult.Equal Then
    '            frm = New FrmVLCUploader
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMPMaster) = CompairStringResult.Equal Then
    '            frm = New FrmMPMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmParameterMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmParameterRangeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterRangeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVLCRouteShiftMaster) = CompairStringResult.Equal Then
    '            frm = New FrmVLCRouteShiftMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPrimaryTransporterVehicalMaster) = CompairStringResult.Equal Then
    '            frm = New FrmPrimaryTransporterVehicalMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOpenMCCShift) = CompairStringResult.Equal Then
    '            '==Add New Variable in Open Mcc Shift .it is Running Two Screens on Basis on It By: Rohit Gupta=========s
    '            frm = New FrmOpenMCCShift(clsUserMgtCode.frmOpenMCCShift)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOpenMCCShiftManual) = CompairStringResult.Equal Then
    '            frm = New FrmOpenMCCShift(clsUserMgtCode.frmOpenMCCShiftManual)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmParameterRangeMasterForQC) = CompairStringResult.Equal Then
    '            frm = New frmParameterRangeMasterForQC(clsUserMgtCode.frmParameterRangeMasterForQC)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQualityModuleParameterRangeMaster) = CompairStringResult.Equal Then
    '            frm = New frmParameterRangeMasterForQC(clsUserMgtCode.frmQualityModuleParameterRangeMaster)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPortSettings) = CompairStringResult.Equal Then
    '            frm = New FrmPortSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPriceChartBulkProc) = CompairStringResult.Equal Then
    '            frm = New frmPriceChartBulkProc
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ParameterValueMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterValueMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LostDefectSealNo) = CompairStringResult.Equal Then
    '            frm = New FrmLostDefectSealNo
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.LocationDistanceMapping) = CompairStringResult.Equal Then
    '            frm = New FrmLocationDistanceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MccMilkTransferPrice) = CompairStringResult.Equal Then
    '            frm = New FrmMccMilkTransferPrice
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorPriceChartMapping) = CompairStringResult.Equal Then
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.AllowBulkPriceChartMultiplepriceToMultipleVendor, clsFixedParameterCode.AllowBulkPriceChartMultiplepriceToMultipleVendor, Nothing)) > 0 Then
    '                frm = New frmVendorPriceChartMappingUDL()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New frmVendorPriceChartMapping()
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmdeductionGroup) = CompairStringResult.Equal Then
    '            frm = New FrmDeductionGroup1()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeductionMaster) = CompairStringResult.Equal Then
    '            frm = New FrmDeductionMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DeductionMapping) = CompairStringResult.Equal Then
    '            frm = New frmDeductionMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.CanMaster) = CompairStringResult.Equal Then
    '            frm = New frmCanMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.DockMaster) = CompairStringResult.Equal Then
    '            frm = New frmDockMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTankerDispatchPriceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmTankerDispatchPrice_Master()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGroupOfDeduction) = CompairStringResult.Equal Then
    '            frm = New FrmGroupOfDeduction()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmChildRouteFreight) = CompairStringResult.Equal Then
    '            frm = New frmchildRouteFreight()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '            '--------------------MILK PROCUREMENT TRANSACTION-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkCollectionCenters) = CompairStringResult.Equal Then
    '            frm = New FrmMilkCollectionCenters
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkSuppliers) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCRouteMapping) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCSuperwiserMapping) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCSupplierMapping) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkCollection) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkQualityCheck) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkRateProcessingScheme) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVehicleMovement) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkBillGeneration) = CompairStringResult.Equal Then
    '            clsCommon.MyMessageBoxShow("Under Development")
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkGateEntryIn) = CompairStringResult.Equal Then
    '            frm = New frmMilkGateEntryIn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkGateEntryWeightment) = CompairStringResult.Equal Then
    '            frm = New frmMilkGateEntryWeighment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkGateEntryOut) = CompairStringResult.Equal Then
    '            frm = New frmMilkGateEntryOut
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkReceipt) = CompairStringResult.Equal Then
    '            frm = New frmMilkReceiptMCC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCancelAfterPosting) = CompairStringResult.Equal Then
    '            frm = New FrmCancelAfterPosting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMCCMilkTransPortorInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmRecurringPayableInvoice 'frmMccMilkTransportorInvoice
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateEntry) = CompairStringResult.Equal Then
    '            frm = New FrmGateEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCDispatch) = CompairStringResult.Equal Then
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.IsChamberWiseTanker, clsFixedParameterCode.IsChamberWiseTanker, Nothing)) = 1 Then
    '                frm = New frmMccDispatchChamber
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            Else
    '                frm = New FrmMccDispatch
    '                formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            End If

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCTankerDispatchReturn) = CompairStringResult.Equal Then
    '            frm = New FrmMccTankerDispatchReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWeighment) = CompairStringResult.Equal Then
    '            frm = New FrmWeighment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SecondarySettingForQC) = CompairStringResult.Equal Then
    '            frm = New FrmSecondarySettingForQC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TDSPAYMENT) = CompairStringResult.Equal Then
    '            frm = New FrmTDSPayment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQualityCheck) = CompairStringResult.Equal Then
    '            frm = New FrmQualityCheck
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkSample) = CompairStringResult.Equal Then
    '            If clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.MilkProcuremntPickCLRInsteadOfSNF, clsFixedParameterCode.MilkProcuremntPickCLRInsteadOfSNF, Nothing)) > 0 Then
    '                frm = New frmMilkSampleMCCOddEvenCLR
    '            ElseIf clsCommon.myCdbl(clsFixedParameter.GetData(clsFixedParameterType.OpenODDEvenForm, clsFixedParameterCode.OpenODDEvenForm, Nothing)) = 1 Then
    '                frm = New frmMilkSampleMCCOddEven
    '            Else
    '                frm = New frmMilkSampleMCC
    '            End If
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCSMSSettiing) = CompairStringResult.Equal Then
    '            frm = New FrmMccSMSSetting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaterial) = CompairStringResult.Equal Then
    '            frm = New frmMCCMaterialSale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaterialSaleReturn) = CompairStringResult.Equal Then
    '            frm = New frmMccMaterialSaleReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaterialSalePriceChart) = CompairStringResult.Equal Then
    '            frm = New FrmMCCMaterialSalePriceChart
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkSRN) = CompairStringResult.Equal Then
    '            frm = New frmMilkSRNMCC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MCCSetting) = CompairStringResult.Equal Then
    '            frm = New frmMCCProcurementSetting(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVSPIncentiveTagging) = CompairStringResult.Equal Then
    '            frm = New FrmVSPIncentiveTagging
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkPurchaseInvoice) = CompairStringResult.Equal Then
    '            frm = New frmMilkPurchaseInvoiceMCC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkShiftEndMCC) = CompairStringResult.Equal Then
    '            frm = New frmMilkShiftClosingMCC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkTransferIn) = CompairStringResult.Equal Then
    '            frm = New FrmMilkTransferIn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkTransferInReturn) = CompairStringResult.Equal Then
    '            frm = New frmMilkTransferInReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBulkMilkSRN) = CompairStringResult.Equal Then
    '            frm = New FrmBulkMilkSRN
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBulkMilkSRNReturn) = CompairStringResult.Equal Then
    '            frm = New FrmBulkMilkSRNReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUnloading) = CompairStringResult.Equal Then
    '            frm = New FrmUnloading
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBulkMilkPurchaseInvoice) = CompairStringResult.Equal Then
    '            frm = New FrmMilkPurchaseInvoice
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.BulkMilkPurchaseInvoiceMultiple) = CompairStringResult.Equal Then
    '            frm = New frmBulkMilkPurchaseInvoiceMultiple
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkPurchaseReturn) = CompairStringResult.Equal Then
    '            frm = New FrmMilkPurchaseReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCleaning) = CompairStringResult.Equal Then
    '            frm = New FrmCleaning
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmGateOut
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBulkPurchaseUploader) = CompairStringResult.Equal Then
    '            frm = New frmBulkPurchaseUploader
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVlcdataUploadar) = CompairStringResult.Equal Then
    '            frm = New FrmVlcDataUploadar
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVLCDataUploaderManual) = CompairStringResult.Equal Then
    '            frm = New FrmVLCDataUploaderManual
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCShiftReportRouteWise) = CompairStringResult.Equal Then
    '            frm = New FrmMCCShiftReportRouteWise
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDispatchTransfer) = CompairStringResult.Equal Then
    '            frm = New FrmDispatchTransfer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '--------------------MILK PROCUREMENT Report-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCollectionLevelChart) = CompairStringResult.Equal Then
    '            frm = New rptCollectionLevelChart
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCollectionCenterChart) = CompairStringResult.Equal Then
    '            frm = New rptCollectionCenterChart
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptvillageslip) = CompairStringResult.Equal Then
    '            frm = New RptVillageSlip
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMilkBillMCC) = CompairStringResult.Equal Then
    '            frm = New RptMilkBillMCC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMilkBillRouteWise) = CompairStringResult.Equal Then
    '            frm = New RptMilkBillRouteWise
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCMilkBillSummary) = CompairStringResult.Equal Then
    '            frm = New RptMccMilkBillSummary
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptTankerSummaryReport) = CompairStringResult.Equal Then
    '            frm = New RptTankerSummaryReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptShiftCodeWise) = CompairStringResult.Equal Then
    '            '    frm = New RptShiftReportCodeWise
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptShifReportZeroAmtSample) = CompairStringResult.Equal Then
    '            frm = New RptShiftReportZeroAmtSample
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMilkPaymentRegister) = CompairStringResult.Equal Then
    '            frm = New RptPaymentRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptLowProcurement) = CompairStringResult.Equal Then
    '            frm = New RptLowProcurement
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMccSaleRegister) = CompairStringResult.Equal Then
    '            'frm = New RptMCCsaleRegister
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' changed by panch raj on 08-05-18 against ticket No: KDI/04/05/18-000295
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBulkMilkRegister) = CompairStringResult.Equal Then
    '            frm = New RptBulkMilkRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMDConversion) = CompairStringResult.Equal Then
    '            frm = New RptMDConversionAtUDL
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMilkRouteVehicleReport) = CompairStringResult.Equal Then
    '            frm = New RptMilkRouteVehicleReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCDA) = CompairStringResult.Equal Then
    '            frm = New RptCDA
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDailyProgressReport) = CompairStringResult.Equal Then
    '            '    frm = New RptDailyProgressReport
    '            '         formShow(frm,strProgramCode, strProgramName, isOpenInMDI,strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMonthlyVLCProcurement) = CompairStringResult.Equal Then
    '            frm = New RptMonthlyVLCProcurement1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSecondaryQuality) = CompairStringResult.Equal Then
    '            frm = New RptSecondaryQualityReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDailyDifferentReport) = CompairStringResult.Equal Then
    '            frm = New RptDailyDifferentRow_vb
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVSPAssetIssue) = CompairStringResult.Equal Then
    '            frm = New frmVSPAssetIssue
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVSPItemIssue) = CompairStringResult.Equal Then
    '            frm = New frmVSPItemIssue
    '            frm.AllowModifcationByApprovalUser = IsAllowModificationByApprovalUser
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMillPurchaseBill) = CompairStringResult.Equal Then
    '            frm = New RptMilkPurchaseBill
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptGainSheetPeriod) = CompairStringResult.Equal Then
    '            frm = New RptGainSheetPeriod
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptWeighment) = CompairStringResult.Equal Then
    '            frm = New RptWeightment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptTankerVariation) = CompairStringResult.Equal Then
    '            frm = New RptTankerVariationReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDailyGainDay) = CompairStringResult.Equal Then
    '            frm = New RptDailyGainDay
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPendingMilkSRN) = CompairStringResult.Equal Then
    '            frm = New RptPendingMilkSRN
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPendingProvisionReport) = CompairStringResult.Equal Then
    '            frm = New FrmPendingProvisionReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPTReport) = CompairStringResult.Equal Then
    '            frm = New FrmPTReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MCCMilkRegister) = CompairStringResult.Equal Then
    '            frm = New FrmMCCMilkRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDairyBookingDistributorReport) = CompairStringResult.Equal Then
    '            frm = New RptDairyBookingDistributorReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDairyTruckSheetReport) = CompairStringResult.Equal Then
    '            frm = New rptDairyTruckSheetReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptSaleRegisterDetail) = CompairStringResult.Equal Then
    '            frm = New rptSaleRegisterDetail()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVSPIncentiveRegister) = CompairStringResult.Equal Then
    '            frm = New rptVSPIncentiveRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MccSummaryReport) = CompairStringResult.Equal Then
    '            frm = New FrmMCCSummary
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMilkStockLedgerSummary) = CompairStringResult.Equal Then
    '            frm = New RptMilkStockLegderSummary
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMilkWeigmentRegister) = CompairStringResult.Equal Then
    '            frm = New RptMilkWeigmentRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMemberPaymentSlip) = CompairStringResult.Equal Then
    '            frm = New Rptmemberpaymentslip3
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMPWiseMilkCollection) = CompairStringResult.Equal Then
    '            frm = New RptMPWiseMilkCollection
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMPWiseMilkCollectionATPoolingPoint) = CompairStringResult.Equal Then
    '            frm = New RptMPWiseMilkCollectionAtPoolingPoint3
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVillageDiffReport) = CompairStringResult.Equal Then
    '            frm = New RptVillageDiffReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVillageDiffReportParas) = CompairStringResult.Equal Then
    '            frm = New RptVillageDifferenceREportParas
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCVLCVariationReportPourersNo) = CompairStringResult.Equal Then
    '            frm = New rptMCCVLCVariationReportPourersNo
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)



    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPrimaryTransporter) = CompairStringResult.Equal Then
    '            frm = New RptPrimaryTransporter
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCMilkRegisterDripSaver) = CompairStringResult.Equal Then
    '            frm = New RptMCCMilkRegisterDripSaver
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCVLCVarationReport) = CompairStringResult.Equal Then
    '            frm = New FrmMCCVLCVarationReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVSPOrVLCVarationRpt) = CompairStringResult.Equal Then
    '            frm = New RptVSPOrVLCVarationReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptCollectionAnalysis) = CompairStringResult.Equal Then
    '            frm = New RptCollectionAnalysis
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMPIDReport) = CompairStringResult.Equal Then
    '            frm = New RptMPIDReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProvisionEntry) = CompairStringResult.Equal Then
    '            frm = New FrmProvisionEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaymentProcess) = CompairStringResult.Equal Then
    '            frm = New FrmPaymentProcess
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptTankerStatusReport) = CompairStringResult.Equal Then
    '            frm = New rptTankerStatusReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaymentProcessFarmer) = CompairStringResult.Equal Then
    '            frm = New frmPaymentProcessFarmer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVSPItemIssue) = CompairStringResult.Equal Then
    '            frm = New RptVSPItemIssue
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVSPAssetIssue1) = CompairStringResult.Equal Then
    '            frm = New RptVSPAssetIssue1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPriceRateDifferenceReport) = CompairStringResult.Equal Then
    '            frm = New RptPriceRateDifferenceReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMCCMilkStatus) = CompairStringResult.Equal Then
    '            frm = New RptMCCMilkStatus
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDispatchOfMilkTransfer) = CompairStringResult.Equal Then
    '            frm = New RptDispatchofmilkTransfer2
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVLCTragetMasterReport) = CompairStringResult.Equal Then
    '            frm = New RptVLVCTragetMasterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCVLCTragetMonthWiseReport) = CompairStringResult.Equal Then
    '            frm = New RptMCCVLCTragetMonthWiseReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptBulkMilkMultiplePurchaseInvoice) = CompairStringResult.Equal Then
    '            frm = New RptBulkMilkMultiplePurchaseInvoice
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptRoutewiseTPTimeTable) = CompairStringResult.Equal Then
    '            frm = New RptRoutewiseTPTimeTable
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '-------------------Milk Procurement module end------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.SublocationMaster) = CompairStringResult.Equal Then
    '            frm = New frmSubLocationMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.ItemLocationMapping) = CompairStringResult.Equal Then
    '            frm = New frmTransferLocationMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '------------------Sale Purchase Security------
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBankPermission) = CompairStringResult.Equal Then
    '            frm = New FrmBankPermission
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerPermission) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerPermission
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorPermission) = CompairStringResult.Equal Then
    '            frm = New FrmVendorPermission
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Anubhooti 05-Aug-2014
    '            '--------------------Human Resource-------------------------------------'
    '            '--------------------SetUp-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JobTitle) = CompairStringResult.Equal Then
    '            frm = New FrmJobTilte
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRParameterMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRParameterMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.EmployeeTypeMaster) = CompairStringResult.Equal Then
    '            frm = New frmHREmployeeTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmQualificationMaster) = CompairStringResult.Equal Then
    '            frm = New FrmQualificationMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProfileMaster) = CompairStringResult.Equal Then
    '            frm = New FrmProfileMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRoundMaster) = CompairStringResult.Equal Then
    '            frm = New FrmRoundMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmchkList) = CompairStringResult.Equal Then
    '            frm = New FrmCheckListMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRelationMaster) = CompairStringResult.Equal Then
    '            frm = New FrmRelationMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSourceTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmSourceTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSourceTypeDetail) = CompairStringResult.Equal Then
    '            frm = New FrmSourceTypeDetail
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.HRBudgeting) = CompairStringResult.Equal Then
    '            frm = New FrmHRBudgeting
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RequesitionEntry) = CompairStringResult.Equal Then
    '            frm = New FrmRequesitionEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RequesitionApproval) = CompairStringResult.Equal Then
    '            frm = New FrmRequesitionApprovel
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RequesitionClose) = CompairStringResult.Equal Then
    '            frm = New FrmCloseRequestion
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.HRIndustryType) = CompairStringResult.Equal Then
    '            frm = New frmHRIndustryType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.HRVerticalMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRVerticalMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''
    '            ' Transaction
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmApplicantEntry) = CompairStringResult.Equal Then
    '            frm = New FrmApplicantEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmShortlist) = CompairStringResult.Equal Then
    '            frm = New FrmShortlist
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInterviewSchedule) = CompairStringResult.Equal Then
    '            frm = New FrmInterviewSchedule
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInterviewFeedback) = CompairStringResult.Equal Then
    '            frm = New FrmInterviewFeedback
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.OfferChkList) = CompairStringResult.Equal Then
    '            frm = New FrmOfferCheckList
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.JOININGCHECKLIST) = CompairStringResult.Equal Then
    '            frm = New FrmJoiningChecklist
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmReferenceCheck) = CompairStringResult.Equal Then
    '            frm = New FrmReferenceCheck
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSalaryFitment) = CompairStringResult.Equal Then
    '            frm = New FrmSalaryFitment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmOfferLetterHR) = CompairStringResult.Equal Then
    '            frm = New FrmOfferLetterHR
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmAppointmentLetterHR) = CompairStringResult.Equal Then
    '            frm = New frmAppointmentLetterHR
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHireEmployee) = CompairStringResult.Equal Then
    '            frm = New FrmHireEmployee
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHrTraineeFeedBack) = CompairStringResult.Equal Then
    '            frm = New FrmTraineeFeedback
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHrTrainerFeedBack) = CompairStringResult.Equal Then
    '            frm = New FrmHrTrainerFeedback
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDamageCaused) = CompairStringResult.Equal Then
    '            frm = New FrmDamageCaused
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.AgencyMaster) = CompairStringResult.Equal Then
    '            frm = New FrmAgencyMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDamageMaster) = CompairStringResult.Equal Then
    '            frm = New FrmDamageMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TrainingMaster) = CompairStringResult.Equal Then
    '            frm = New frmTrainingMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TrainingResourceMaster) = CompairStringResult.Equal Then
    '            frm = New frmTrainingResourceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TrainingInstituteMaster) = CompairStringResult.Equal Then
    '            frm = New frmInstituteMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TrainingRequestMaster) = CompairStringResult.Equal Then
    '            frm = New frmRequestForTrainingMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TRAINER_MASTER) = CompairStringResult.Equal Then
    '            frm = New frmTrainerMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Schedule_Training) = CompairStringResult.Equal Then
    '            frm = New frmScheduleForTraining
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.Training_Attendance) = CompairStringResult.Equal Then
    '            frm = New frmTrainingAttendance
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.TrainingMaster) = CompairStringResult.Equal Then
    '            frm = New frmTrainingMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Performance Evaluation
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New frmHRPerformanceCategoryMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRPerformanceMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceGroup) = CompairStringResult.Equal Then
    '            frm = New FrmHRPerformanceGroup
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceGroupMapping) = CompairStringResult.Equal Then
    '            frm = New FrmPerformanceGroupMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRPerformanceRating) = CompairStringResult.Equal Then
    '            frm = New FrmPerformanceRating
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' Reimbursement
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRReimbursementTypeMaster) = CompairStringResult.Equal Then
    '            frm = New frmHRReimbursementTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelPurposeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelPurposeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelCategoryMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelCategoryMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelBookedForMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelBookedForMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelModeTypeMaster) = CompairStringResult.Equal Then
    '            frm = New frmHRTravelModeTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelCityMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelCityMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelClassTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelClassTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRHotelRatingMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRHotelRatingMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelRoomTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelRoomTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelCarTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelCarTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRRaiseTravelRequisition) = CompairStringResult.Equal Then
    '            frm = New FrmHRRaiseTravelRequisition
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelReqApproval) = CompairStringResult.Equal Then
    '            frm = New frmHRTravelReqApproval
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelReimbursementExpense) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelReimbursementExpense
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRTravelExpenseApproval) = CompairStringResult.Equal Then
    '            frm = New FrmHRTravelExpenseApproval
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmHRApprovarCreationMaster) = CompairStringResult.Equal Then
    '            frm = New FrmApproverCreationMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' ---------------------------- End HR --------------------------------------
    '            ''''''''''''''''''''''''''''''''''''''''''''Visual Process Flow''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVPFSettings) = CompairStringResult.Equal Then
    '            frm = New FrmVPFSettings
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmVPFActiveReport) = CompairStringResult.Equal Then
    '            frm = New FrmVPFActiveScreens
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmGLCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmGLCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCommonCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmCommonCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmAPCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmAPCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmARCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmARCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkSaleCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmBulkSaleCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFreshSaleCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmFreshSaleCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProductSaleCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmProductSaleCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCSASaleCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmCSASaleCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMMCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmMMCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMCCProcurementCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmMCCProcurementCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmBulkProcurementCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmBulkProcurementCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPurchaseCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmPurchaseCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDProductionCycle) = CompairStringResult.Equal Then
    '            frm = New FrmModuleCycle(clsUserMgtCode.FrmDProductionCycle)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '''''''''''''''''''''''''''''''''''''''''''' End Visual Process Flow ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  

    '            '' Anubhooti 28-Aug-2015
    '            '''''''''''''''''''''''''''''''''''''''''''' Service And Warranty ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            '--------------------SetUp-------------------------------------'
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFaultCategory) = CompairStringResult.Equal Then
    '            frm = New FrmFaultCategory
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFaultMaster) = CompairStringResult.Equal Then
    '            frm = New FrmFaultMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceChargeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmServiceChargeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmProblemType) = CompairStringResult.Equal Then
    '            frm = New FrmProblemType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCallType) = CompairStringResult.Equal Then
    '            frm = New FrmCallType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmActivityType) = CompairStringResult.Equal Then
    '            frm = New FrmActivityType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSolutionKnowledgeBase) = CompairStringResult.Equal Then
    '            frm = New FrmSolutionKnowledgeBase
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceCall) = CompairStringResult.Equal Then
    '            frm = New FrmServiceCall
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceEnquiry) = CompairStringResult.Equal Then
    '            frm = New FrmServiceEnquiry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceAllocation) = CompairStringResult.Equal Then
    '            frm = New FrmServiceAllocation
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmServiceVisitDetails) = CompairStringResult.Equal Then
    '            frm = New FrmServiceVisitDetails
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '''''''''''''''''''''''''''''''''''''''''''' End Service And Warranty ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemConversion) = CompairStringResult.Equal Then
    '            frm = New FrmItemConversion
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkReasonMaster) = CompairStringResult.Equal Then
    '            frm = New frmMilkReasonMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPaymentCycleMaster) = CompairStringResult.Equal Then
    '            frm = New frmPaymentCycleMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkMPPayment) = CompairStringResult.Equal Then
    '            frm = New FrmMilkVSPPayment(clsUserMgtCode.MilkMPPayment)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIncentiveMaster) = CompairStringResult.Equal Then
    '            frm = New frmIncentiveMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkTruckSheet) = CompairStringResult.Equal Then
    '            frm = New FrmMilkTruckSheet
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkVSPPayment) = CompairStringResult.Equal Then
    '            frm = New FrmMilkVSPPayment(clsUserMgtCode.MilkVSPPayment)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MPBillGeneration) = CompairStringResult.Equal Then
    '            frm = New FrmMilkVSPPayment(clsUserMgtCode.MPBillGeneration)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkVSPIssuePayment) = CompairStringResult.Equal Then
    '            frm = New FrmMilkVSPPayment(clsUserMgtCode.MilkVSPIssuePayment)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkRecurringScheduler) = CompairStringResult.Equal Then
    '            frm = New FrmMilkRecurringScheduler
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '========================================MIS Reports===========================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISDebtorReport) = CompairStringResult.Equal Then
    '            frm = New FrmRptCustomerLedgerDemo(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISStockReco) = CompairStringResult.Equal Then
    '            frm = New FrmStockReco(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISSaleRegister) = CompairStringResult.Equal Then
    '            frm = New RptSaleRegisterReport(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISCreditorReport) = CompairStringResult.Equal Then
    '            frm = New frmRptVendorLedger(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptCustomerTransList) = CompairStringResult.Equal Then
    '            frm = New FrmRptCustomerTransList()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRptCustomerTransHistory) = CompairStringResult.Equal Then
    '            frm = New FrmRptCustomerTransHistory()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISStockLedgerReport) = CompairStringResult.Equal Then
    '            '    frm = New FrmStockReco(strProgramCode)
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MISStockLedgerReport) = CompairStringResult.Equal Then
    '            frm = New FrmStockReco(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemReloadReport) = CompairStringResult.Equal Then
    '            frm = New FrmItemReloadReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkJobWork) = CompairStringResult.Equal Then
    '            frm = New frmMilkRGP
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkJobWorkTransfer) = CompairStringResult.Equal Then
    '            frm = New frmMilkJobWorkTransfer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkJobWorkTransferReturn) = CompairStringResult.Equal Then
    '            frm = New frmMilkJobWorkTransferReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkJobWorkTransferOther) = CompairStringResult.Equal Then
    '            frm = New frmJWOTransferOther
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkJobWorkTransferOtherReturn) = CompairStringResult.Equal Then
    '            frm = New frmJWOTransferOtherReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkGateEntry) = CompairStringResult.Equal Then
    '            frm = New FrmMilkGateEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkWeighment) = CompairStringResult.Equal Then
    '            frm = New FrmMilkWeighment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmJobMilkQualityCheck) = CompairStringResult.Equal Then
    '            frm = New FrmJobMilkQualityCheck
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmJobMilkSRN) = CompairStringResult.Equal Then
    '            frm = New FrmJobMilkSRN
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptJobWorkStatus) = CompairStringResult.Equal Then
    '            frm = New RptJobWorkStatus
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkUnloading) = CompairStringResult.Equal Then
    '            frm = New FrmMilkUnloading
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptJobWorkRegister) = CompairStringResult.Equal Then
    '            frm = New RptJobWorkRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptJobWorOutwardPurchasekRegister) = CompairStringResult.Equal Then
    '            frm = New RptJobWorktPurchaseRegisterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobworkTransfer) = CompairStringResult.Equal Then
    '            frm = New frmJobworkTransfer()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobworkChargesReport) = CompairStringResult.Equal Then
    '            frm = New FrmJobworkChargesReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobworkSRNReceiptReport) = CompairStringResult.Equal Then
    '            frm = New frmJobworkSRNReceiptReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMilkCleaning) = CompairStringResult.Equal Then
    '            '    frm = New FrmMilkCleaning
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            '========================================MIS Reports============================Ends Here======
    '            '=================================TDS Module ====================Added by Preeti Gupta========================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIncomeTaxSlab) = CompairStringResult.Equal Then
    '            frm = New frmIncomeTaxSlab
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmITSection) = CompairStringResult.Equal Then
    '            frm = New FrmITSection
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInvestmentType) = CompairStringResult.Equal Then
    '            frm = New FrmInvestmentType
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHouseRentDeclaration) = CompairStringResult.Equal Then
    '            frm = New FrmHouseRentDeclaration
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmHRAExemptionRule) = CompairStringResult.Equal Then
    '            frm = New FrmHRAExemptionRule
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmInvestmentDeclaration) = CompairStringResult.Equal Then
    '            frm = New FrmInvestmentDeclaration
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MCCProvisonReport) = CompairStringResult.Equal Then
    '            frm = New frmRptMCCProvision
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MCCProvisonReport) = CompairStringResult.Equal Then
    '            frm = New frmRptMCCProvision
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmContractTanker) = CompairStringResult.Equal Then
    '            frm = New frmContractTanker
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSupplierMaster) = CompairStringResult.Equal Then
    '            frm = New frmSupplierMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDivertedContractor) = CompairStringResult.Equal Then
    '            frm = New frmDivertedContractor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkTypeMast) = CompairStringResult.Equal Then
    '            frm = New frmMilkTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmIntimation) = CompairStringResult.Equal Then
    '            frm = New frmIntimation
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPOBulkProc) = CompairStringResult.Equal Then
    '            frm = New frmPoBulkProc
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkGradeMaster) = CompairStringResult.Equal Then
    '            frm = New frmMilkGradeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '========================================END TDS Module======================================================

    '            '==========================================================================================================
    '            ' ADDED BY KUNAL TICKET : BM00000009674
    '            '==========================================================================================================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleSettingFreshDS) = CompairStringResult.Equal Then
    '            frm = New FrmSaleSetting(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRouteMasterDS) = CompairStringResult.Equal Then
    '            frm = New frmRouteMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmFreshTransactionApprovalDS) = CompairStringResult.Equal Then
    '            frm = New FrmTransactionApproval()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSchemeMasterDairyDS) = CompairStringResult.Equal Then
    '            frm = New FrmSchemeMasterDairy()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmRouteFreightDetailsDS) = CompairStringResult.Equal Then
    '            frm = New FrmRouteFreightDetails()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLocationItemMapping) = CompairStringResult.Equal Then
    '            frm = New RptLocationItemMappingDS()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPlantCustomerDemand) = CompairStringResult.Equal Then
    '            frm = New RptPlantCustomerDemandReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDemandForsingleBranch) = CompairStringResult.Equal Then
    '            frm = New RptDemandForSingleBranch()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmPrintDistributerInvoiceStatement) = CompairStringResult.Equal Then
    '            frm = New FrmPrintDistributerInvoiceStatement()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDistributerLedgerReport) = CompairStringResult.Equal Then
    '            frm = New frmDistributerLedgerReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPendingBooking) = CompairStringResult.Equal Then
    '            frm = New FrmPendingBookingReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCustomerZone) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerZoneReport()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==========================================================================================================
    '            ' ADDED BY KUNAL TICKET : BM00000009674 ENDED HERE
    '            '==========================================================================================================

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmbookingdairy) = CompairStringResult.Equal Then
    '            frm = New frmBookingDairyMultipleCustomer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeliveryOrderDairy) = CompairStringResult.Equal Then
    '            frm = New frmDeliveryNoteDairySale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleDispatchDairy) = CompairStringResult.Equal Then
    '            frm = New frmShipmentDairy
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmGatePassDairy) = CompairStringResult.Equal Then
    '            frm = New frmGatePassDairySale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleInvoicedairy) = CompairStringResult.Equal Then
    '            'sanjay
    '            'frm = New frmSaleInvoiceDairy
    '            frm = New frmSaleInvoiceProductSale(strProgramCode)
    '            'frm = New frmSaleInvoiceProductSale()
    '            'sanjay
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSaleReturndairy) = CompairStringResult.Equal Then
    '            frm = New frmSaleReturnDairy
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCrateReceviedDairySale) = CompairStringResult.Equal Then
    '            frm = New frmCreateReceivedDairySale
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDairyBookingCustomer) = CompairStringResult.Equal Then
    '            frm = New frmDairyBookingCustomer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPerformaInvoiceDairy) = CompairStringResult.Equal Then
    '            frm = New frmPerformaInvoiceDairy
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '=========
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptEffectiveRateReport1) = CompairStringResult.Equal Then
    '            frm = New RptEffectiveRateReport1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCustomerEffective_ItemRate) = CompairStringResult.Equal Then
    '            frm = New RptCustomerEffective_ItemRate
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDeliveryOrderReport1) = CompairStringResult.Equal Then
    '            frm = New RptDeliveryOrderReport1
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmBookingDairyMultipleDistributor) = CompairStringResult.Equal Then
    '            frm = New frmBookingDairyMultipleDistributor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmPOSBookingDairyMultipleDistributor) = CompairStringResult.Equal Then
    '            frm = New frmPOSBookingDairyMultipleDistributor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)



    '            '=======================================================================================================================
    '            ' = SILAGE PRODUCTION FORMS ==
    '            '=======================================================================================================================
    '            '== Added by kunal for Silage Production Forms ========================================================================

    '            '    ''comment by balwinder due to solution auto close
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageAreaMaster) = CompairStringResult.Equal Then
    '            '    frm = New frmSilageAreaMaster
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageCriteriaMaster) = CompairStringResult.Equal Then
    '            '    frm = New frmSilageCriteriaMaster
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageProductionApplication) = CompairStringResult.Equal Then
    '            '    frm = New frmSilageProductionApplication
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageEnterPrenur) = CompairStringResult.Equal Then
    '            '    frm = New frmSilageEnterPrenur
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageFormerProduction) = CompairStringResult.Equal Then
    '            '    frm = New frmSilageFarmerSelection
    '            '    formShow(frm,strProgramCode, strProgramName, isOpenInMDI, strDocNo)

    '            '    ''end of comment by balwinder due to solution auto close


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageTankerTransporterMaster) = CompairStringResult.Equal Then
    '            frm = New frmTankerTransporterMaster(lblUserCode.Text, lblCompanyCode.Text)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageTankerMaster) = CompairStringResult.Equal Then
    '            frm = New FrmTankerMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageParameterMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSialgeParameterRangeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterRangeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSialgeParameterRangeMasterForQC) = CompairStringResult.Equal Then
    '            frm = New frmParameterRangeMasterForQC(clsUserMgtCode.frmParameterRangeMasterForQC)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSialgeParameterValueMaster) = CompairStringResult.Equal Then
    '            frm = New FrmParameterValueMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilagePriceChartBulkProc) = CompairStringResult.Equal Then
    '            frm = New frmPriceChartBulkProc
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageVendorPriceChartMapping) = CompairStringResult.Equal Then
    '            frm = New frmPriceChartBulkProc
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSialgeSupplierMaster) = CompairStringResult.Equal Then
    '            frm = New frmSupplierMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageDivertedContractor) = CompairStringResult.Equal Then
    '            frm = New frmDivertedContractor
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageGradeMaster) = CompairStringResult.Equal Then
    '            frm = New frmGradeMaster()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '' transaction 
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageGateEntry) = CompairStringResult.Equal Then
    '            frm = New FrmGateEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageWeighment) = CompairStringResult.Equal Then
    '            frm = New FrmWeighment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageQualityCheck) = CompairStringResult.Equal Then
    '            frm = New FrmQualityCheck
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageUnloading) = CompairStringResult.Equal Then
    '            frm = New FrmUnloading
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSialgeCleaning) = CompairStringResult.Equal Then
    '            frm = New FrmCleaning
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageGateOut) = CompairStringResult.Equal Then
    '            frm = New FrmGateOut
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageBulkSRN) = CompairStringResult.Equal Then
    '            frm = New FrmBulkMilkSRN
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmSilageBulkSRNReturn) = CompairStringResult.Equal Then
    '            frm = New FrmBulkMilkSRNReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkReject) = CompairStringResult.Equal Then
    '            frm = New frmMilkRejectEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptTankerDispatchWidthDeduction) = CompairStringResult.Equal Then
    '            frm = New frmRptTankerDispatchWithDeduction
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MCCDispatchReturn) = CompairStringResult.Equal Then
    '            frm = New frmTankerDispatchReturn
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ' ================== END OF SILAGE PRODUCTION FORMS ===========================================================================
    '            '======Sanjeet(21/11/2016)============
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmTruckSheetRouteWiseRpt) = CompairStringResult.Equal Then
    '            frm = New FrmTruckSheetRouteWiseRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '23/11/2016
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMccWeightDifferenceRpt) = CompairStringResult.Equal Then
    '            frm = New FrmMCCWeightDiifferenceRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '09/01/2017
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptAClassMilkRate) = CompairStringResult.Equal Then
    '            frm = New RptAClassMilkRate
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '11/01/2017
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPendingPO) = CompairStringResult.Equal Then
    '            frm = New RptPendingPO
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '12/01/2017
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptIssueReturnHirerachyWise) = CompairStringResult.Equal Then
    '            frm = New RptIssueReturnHirerachyWise
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMccSaleAdjustment) = CompairStringResult.Equal Then
    '            frm = New RptMccSaleAdjustment
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==========
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptVLCwiseTPTimeTable) = CompairStringResult.Equal Then
    '            frm = New RptVLCwiseTPTimeTable
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '=========Parteek added form 10-09-2017
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmProductBooking) = CompairStringResult.Equal Then
    '            frm = New frmProductBooking()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMccBulkmilkRegister) = CompairStringResult.Equal Then
    '            frm = New RptMccBulkMilkRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDailyStanderdMilkQtyMCCWise) = CompairStringResult.Equal Then
    '            frm = New RptDailyStanderdMilkQtyMCCWise
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptMCCRouteTimeTable) = CompairStringResult.Equal Then
    '            frm = New rptMCCRouteTimeTable
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptTankerInTransit) = CompairStringResult.Equal Then
    '            frm = New RptTankerInTransit
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)


    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptWeighmentRegister) = CompairStringResult.Equal Then
    '            frm = New RptWeighmentRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptVLCVehicleWeigmentRegister) = CompairStringResult.Equal Then
    '            frm = New RptVLCVehicleWeigmentRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMilkReceiptImproperWeight) = CompairStringResult.Equal Then
    '            frm = New FrmRptMilkReceiptImproperWeight
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptDailyWiseMilkCost) = CompairStringResult.Equal Then
    '            frm = New RptDailyWiseMilkCost
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptDailyLandedCost) = CompairStringResult.Equal Then
    '            frm = New FrmVLCDailyLandedCost
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSaleVsReceipReport) = CompairStringResult.Equal Then
    '            frm = New FrmSaleVsReceipReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCostCentreConsumptionRpt) = CompairStringResult.Equal Then
    '            frm = New FrmCostCentreConsumptionRpt
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDairyGatePass) = CompairStringResult.Equal Then
    '            frm = New frmDairyGatePass
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmUnitMaster) = CompairStringResult.Equal Then
    '            frm = New FrmUnitMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmCostCenterTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmCostCetreTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptPendingDocumentList) = CompairStringResult.Equal Then
    '            frm = New RptPendingDocumentList
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '===========Sanjeet(21/02/2017)======

    '            ''Added by Parteek on 23/08/2017
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmJobWorkoutwordMaster) = CompairStringResult.Equal Then
    '            frm = New frmJWPriceCodeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmVendorItemChargeMaster) = CompairStringResult.Equal Then
    '            frm = New frmVendorItemChargeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            ''======End
    '            '===========Panch Raj(27/02/2017)======
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFarmerLedgerReport) = CompairStringResult.Equal Then
    '            frm = New frmFarmerLedgerReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCFarmerMappingFP) = CompairStringResult.Equal Then
    '            frm = New FrmMCCFarmerMapping()
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaterialFarmer) = CompairStringResult.Equal Then
    '            frm = New frmMCCMaterialSaleFarmer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCMaterialSaleReturnFarmer) = CompairStringResult.Equal Then
    '            frm = New frmMccMaterialSaleReturnFarmer
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFarmerPaymentAdjustment) = CompairStringResult.Equal Then
    '            frm = New frmFarmerPaymentAdjEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmDailySaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmDailySaleReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmMonthlySaleReport) = CompairStringResult.Equal Then
    '            frm = New FrmMonthlySaleReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCustomerGroupOutstanding) = CompairStringResult.Equal Then
    '            frm = New FrmCustomerGroupOutstanding
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmStockAgeingAnalysisReport) = CompairStringResult.Equal Then
    '            frm = New FrmStockAgeingAnalysisReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.mbtnPurchaseJobWork) = CompairStringResult.Equal Then
    '            frm = New frmPurchaseJobwork(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.MonthlyProgressReport) = CompairStringResult.Equal Then
    '            frm = New frmMonthlyProgressReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmCategoryAnalysisReport) = CompairStringResult.Equal Then
    '            frm = New FrmCategoryAnalysisReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptPromptMsgPendindDoc) = CompairStringResult.Equal Then
    '            frm = New RptPrmoptMsgRelatedToPendencyDoc
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptSecondaryTransporterReport) = CompairStringResult.Equal Then
    '            frm = New RptSecondaryTransporterReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptFlavouredMilk) = CompairStringResult.Equal Then
    '            frm = New RptFlavouredMilk
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemTypeMaster) = CompairStringResult.Equal Then
    '            frm = New FrmItemTypeMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptMonthWiseSaleAnalysis) = CompairStringResult.Equal Then
    '            frm = New RptMonthWiseSaleAnalysis
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmjobWorkDebitNote) = CompairStringResult.Equal Then
    '            frm = New frmjobWorkDebitNote
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.RptJobWorkDebitNoteReport) = CompairStringResult.Equal Then
    '            frm = New RptJobWorkDebitNoteReport
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmSAC) = CompairStringResult.Equal Then
    '            frm = New frmSAC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmRackBinMaster) = CompairStringResult.Equal Then
    '            frm = New frmRackBinMaster
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmDeptHeadCustomerMapping) = CompairStringResult.Equal Then
    '            frm = New frmDeptHeadCustomerMapping
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '            '===========end======
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmItemWiseTax) = CompairStringResult.Equal Then
    '            frm = New FrmItemWiseTax
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmConfigureSynchronization) = CompairStringResult.Equal Then
    '            frm = New frmConfigureSynchronization
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmLockMPCollectionPC) = CompairStringResult.Equal Then
    '            frm = New frmLockMPCollectionPC
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmItemTaxRate) = CompairStringResult.Equal Then
    '            'frm = New frmItemTaxRate
    '            'formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFarmerPaymentEntry) = CompairStringResult.Equal Then
    '            frm = New frmFarmerPaymentEntry
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmFarmerMilkPurchaseInvoice) = CompairStringResult.Equal Then
    '            frm = New frmFarmerMilkPurchaseInvoice
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.rptCustomerAdvanceReg) = CompairStringResult.Equal Then
    '            frm = New frmAdvanceRegister
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '====================Added by preeti Gupta[14/03/2018]=========================
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryFS) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryPS) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryMISSAle) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryMCCSAle) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            ' Ticket No : KDI/26/04/18-000277  By prabhakar  ( Tester Remarks )
    '            'ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryBulkSAle) = CompairStringResult.Equal Then
    '            '    frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            '    formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)

    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryExportSAle) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '        ElseIf clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmsaleReturnGateEntryCSATransfer) = CompairStringResult.Equal Then
    '            frm = New FrmsaleReturnGateEntry(strProgramCode)
    '            formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '            '==================================================================
    '        Else
    '            Dim dtt As DataTable = clsDBFuncationality.GetDataTable("select 'BI-RPT' as Code from TSPL_CREATE_BI_REPORT where Code='" + strProgramCode + "' union select 'BI-DBR' as Code from TSPL_CREATE_DASHBOARD where code='" + strProgramCode + "' ")
    '            If dtt IsNot Nothing AndAlso dtt.Rows.Count > 0 Then
    '                If clsCommon.CompairString(clsCommon.myCstr(dtt.Rows(0)("Code")), "BI-RPT") = CompairStringResult.Equal Then
    '                    frm = New FrmBIReport
    '                    frm.obj = clsCreateBIReport.GetData(strProgramCode, True, NavigatorType.Current)
    '                    formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '                ElseIf clsCommon.CompairString(clsCommon.myCstr(dtt.Rows(0)("Code")), "BI-DBR") = CompairStringResult.Equal Then
    '                    frm = New frmDashboard
    '                    frm.objDB = clsCreateDashboard.GetData(strProgramCode, NavigatorType.Current)
    '                    formShow(frm, strProgramCode, strProgramName, isOpenInMDI, strDocNo, IFTrueShowFormElseShowDialog)
    '                End If


    '            End If
    '        End If
    '    End If

    '    frm = Nothing
    'End Sub

    Private Sub RadContextMenu2_DropDownOpening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadContextMenu2.DropDownOpening
        Try
            'RadContextMenu2.Items.Clear()
            'If clsCommon.CompairString(clsCommon.myCstr(RTV2.SelectedNode.Value), clsUserMgtCode.ExpertERP) = CompairStringResult.Equal OrElse clsCommon.CompairString(clsCommon.myCstr(RTV2.SelectedNode.Value), clsUserMgtCode.ModuleFavourite) = CompairStringResult.Equal Then
            '    e.Cancel = True
            'End If

            'Dim mbtnChangeCaption As New RadMenuItem("Change Caption")
            'AddHandler mbtnChangeCaption.Click, AddressOf ChangeCaption
            'RadContextMenu2.Items.Add(mbtnChangeCaption)
            'If Not clsCommon.CompairString(clsCommon.myCstr(RTV2.SelectedNode.Value), clsUserMgtCode.ExpertERP) = CompairStringResult.Equal Then
            '    If clsCommon.CompairString(clsCommon.myCstr(RTV2.SelectedNode.Parent.Value), clsUserMgtCode.ModuleFavourite) = CompairStringResult.Equal Then
            '        Dim mbtnRemoveFromFavourite As New RadMenuItem("Remove from Favourite")
            '        AddHandler mbtnRemoveFromFavourite.Click, AddressOf RemoveFromFavourite
            '        RadContextMenu2.Items.Add(mbtnRemoveFromFavourite)
            '    Else
            '        Dim mbtnAddToFavourite As New RadMenuItem("Add To Favourite")
            '        AddHandler mbtnAddToFavourite.Click, AddressOf AddToFavourite
            '        RadContextMenu2.Items.Add(mbtnAddToFavourite)
            '    End If
            'End If
        Catch ex As Exception
            RadMessageBox.Show(ex.Message, Me.Text)
        End Try
    End Sub

    Private Sub ChangeCaption()
  
    End Sub

    Private Sub AddToFavourite()
        Try
 
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Private Sub RemoveFromFavourite()
        Try
            '  clsFavouriteMenu.DeleteData(RTV2.SelectedNode.Value)
            ' LoadMenu()
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Public Sub formShow(ByVal frm As FrmMainTranScreen, ByVal strProgramCode As String, ByVal strProgramName As String, ByVal isOpenInMDI As Boolean, ByVal strDocNo As String, Optional ByVal IFTrueShowFormElseShowDialog As Boolean = True)
        Try
            If SettingHighSecurityOnWeighingIntegratedScreen Then
                'If clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMCCWeighment) = CompairStringResult.Equal OrElse clsCommon.CompairString(strProgramCode, clsUserMgtCode.MilkGateEntryWeightment) = CompairStringResult.Equal OrElse clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmMilkReceipt) = CompairStringResult.Equal OrElse clsCommon.CompairString(strProgramCode, clsUserMgtCode.frmWeighment) = CompairStringResult.Equal OrElse clsCommon.CompairString(strProgramCode, clsUserMgtCode.POWeighment) = CompairStringResult.Equal OrElse clsCommon.CompairString(strProgramCode, clsUserMgtCode.FrmWeighmentEntry) = CompairStringResult.Equal Then
                '    If Me.RadDock1.DockWindows.DocumentWindows.Where(Function(w) w.Text = strProgramName).Count() > 0 Then
                '        Me.RadDock1.ActivateWindow(Me.RadDock1.DockWindows.DocumentWindows.Where(Function(w) w.Text = strProgramName).First())
                '        Return
                '    End If
                'End If
            End If

            frm.Tag = strDocNo
            frm.Text = strProgramName
            frm.SetUserMgmt(strProgramCode)

            If IFTrueShowFormElseShowDialog Then
                If isOpenInMDI Then
                    frm.MdiParent = Me
                Else
                    frm.WindowState = FormWindowState.Maximized
                End If
                frm.Focus()
                frm.Show()
                If isApplicationRun Then
                    isApplicationRun = False
                    'frm.WindowState = FormWindowState.Maximized
                    Application.Run(frm)
                Else
                    'frm.WindowState = FormWindowState.Maximized
                    frm.Show()
                End If
            Else
                frm.ShowDialog(Me)
                frm.TopMost = True


            End If
        Catch ex As Exception
            If Not ex.Message.Contains("Object reference not set to an instance of an object.") Then ''becuase when need to close the form this message come.
                common.clsCommon.MyMessageBoxShow(ex.Message)
                frm.Close()
            End If
        End Try
    End Sub

    Private Sub btnEditCaption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditCaption.Click
        RTV2.CollapseAll()
        RTV2.Nodes(0).Expand()
    End Sub

    Private Sub RadButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton2.Click
        RTV2.ExpandAll()
    End Sub

    Private Sub RadButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton1.Click
       
    End Sub

    Private Sub txtUserName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUserName.KeyDown, txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            CheckAndLogin()
        End If
    End Sub

    Private Sub RTV2_NodeFormatting(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.TreeNodeFormattingEventArgs) Handles RTV2.NodeFormatting
        If ArrBold.Contains(clsCommon.myCstr(e.Node.Value)) Then
            e.NodeElement.ContentElement.Text = "<html><b>" & e.Node.Text
        End If
    End Sub

    Private Sub RadMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim objLogin As UserLoginInfo = New UserLoginInfo
        'objLogin.Show()
    End Sub

    Private Sub RadMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RadDock1.RemoveAllDocumentWindows()
        SplitPanel3.Collapsed = True
        SplitPanel1.Collapsed = True
        SplitPanel4.Collapsed = True
        SplitPanel2.Collapsed = False

        txtUserName.Text = ""
        txtPassword.Text = ""
    End Sub

    Private Sub RadMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        'txtPassword.Text = String.Empty
        'txtUserName.Text = String.Empty
        'LoadLoginScreen()
    End Sub

    Private Sub RadDock1_DockStateChanged(ByVal sender As System.Object, ByVal e As Telerik.WinControls.UI.Docking.DockWindowEventArgs) Handles RadDock1.DockStateChanged
        ' Set Image
        For i As Integer = 0 To RTV2.Nodes.Count - 1
            SetImage(RTV2.Nodes(i))
        Next
    End Sub

    Private Sub MDI_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            Dim strQ As String = "update tspl_user_master set IP_Address=NULL,Login_Status=0 where user_code='" + objCommonVar.CurrentUserCode + "'"
            clsDBFuncationality.ExecuteNonQuery(strQ)

            Dim frmCollection As New FormCollection()
            frmCollection = Application.OpenForms()
           
        Catch ex As Exception

        End Try

        If Not IsDBRestored Then
            If Not isAutoClosing Then
                If clsCommon.MyMessageBoxShow("Do you want to close the KSap Creation DB", Me.Text, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    e.Cancel = True
                    'Else
                    '    'GC.Collect()
                Else

                End If
            End If
        End If

        If th IsNot Nothing Then
            Try
                th.Abort()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub SplitPanel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitPanel2.Click

    End Sub

    Private Sub RadMenuItem4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Diagnostics.Process.Start(Application.StartupPath & "\KIWI help.chm")
    End Sub

    Private Function GetMasterDBConnectionStr(ByVal strDBName As String) As String
        Try
            Dim strConn As String = clsDBFuncationality.connectionString ''clsCommon.myCstr(Configuration.ConfigurationSettings.AppSettings("connectionString"))
            strConn = clsCommon.ReplaceString(strConn, strDBName, "Master")
            Return strConn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        Try
            If clsCommon.myLen(txtDBSource.Text) <= 0 Then
                bDestination.Focus()
                Throw New Exception("Please select source file.")
            End If

            Dim strMsg As String = "You are going to restore" + Environment.NewLine
            strMsg += " DataBase : - '" + cmbDB.SelectedValue + "'" + Environment.NewLine
            strMsg += " Company : - '" + cmbDB.SelectedText + "'" + Environment.NewLine
            strMsg += " Are you sure?"
            If clsCommon.MyMessageBoxShow(strMsg, Me.Text, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                If RestoreDataBase("" + cmbDB.SelectedValue + "") Then
                    clsCommon.ProgressBarHide()
                    common.clsCommon.MyMessageBoxShow("DataBase Restored Sucessfully.")
                    IsDBRestored = True
                    Application.Restart()
                End If
            End If
        Catch ex As Exception
            clsCommon.ProgressBarHide()
            common.clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Private Function RestoreDataBase(ByVal strDBName As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand
        Try
            clsDBFuncationality.ExecuteNonQuery("Update TSPL_UserLogin_Info set Logout_DateTime=' " + clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(), "dd/MMM/yyyy hh:mm tt") + "'  where Login_Code ='" + objCommonVar.CurrentLoginID + "'")
            clsDBFuncationality.ExecuteNonQuery("ALTER DATABASE " + cmbDB.SelectedValue + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE")
            Dim ConStr As String = GetMasterDBConnectionStr(strDBName)
            conn = clsDBFuncationality.GetConnnection()
            If conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
            conn = New SqlConnection(ConStr)
            cmd = New SqlCommand("sp_StopDBProcess", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn
            conn.Open()
            cmd.Parameters.Add("@strDBName", SqlDbType.VarChar).Value = strDBName
            cmd.ExecuteNonQuery()
            Dim qry As String = "Restore database " + strDBName + " from Disk = '" + txtDBSource.Text + "'"
            cmd = New SqlCommand(qry, conn)
            cmd.CommandTimeout = 3600
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            clsCommon.ProgressBarHide()
            cmd = New SqlCommand("ALTER DATABASE " + cmbDB.SelectedValue + "  SET MULTI_USER", conn)
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            clsDBFuncationality.SetConnection(objCommonVar.ConnString)
        End Try
        Return True
    End Function

    Private Sub bDestination_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bDestination.Click
        Try
            OpenFileDialog1.InitialDirectory = "C:\"
            OpenFileDialog1.Title = "Open a DataBase File"
            OpenFileDialog1.Filter = "DataBase Files|*.bak"
            If OpenFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                txtDBSource.Text = OpenFileDialog1.FileName
            Else
                txtDBSource.Text = ""
            End If
        Catch ex As Exception
            RadMessageBox.Show("Error: " + ex.Message)
        End Try
    End Sub

    Private Sub RadButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadButton13.Click
        LoadLoginScreen()
    End Sub

    Private Sub RTV2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles RTV2.DoubleClick


    End Sub

    Private Sub RTV2_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.RadTreeViewEventArgs) Handles RTV2.NodeMouseDoubleClick
        Try
            Dim strCode As String = clsCommon.myCstr(RTV2.SelectedNode.Value)
            If clsCommon.myLen(strCode) > 0 Then
                ShowForm(strCode, RTV2.SelectedNode.Text, True)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MDI_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

    End Sub

    Private Sub mnuRefreshMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        clsCommon.MyMessageBoxShow("Memory Refreshed ")
    End Sub

    Private Sub RadMenuItem3_Disposing(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#Region "Reminder Code" '-----------By Monika--------04/07/2014----------BM00000003039
    'Private Sub ReminderTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReminderTimer.Tick
    '    Try
    '        Dim ccccc As Integer = 0
    '        Dim cn As SqlConnection = clsDBFuncationality.GetConnnection()
    '        Dim Qry As String = "select count(*) from sys.dm_tran_database_transactions where isnull(database_transaction_status,0)>0"
    '        Dim cmd As SqlCommand = New SqlCommand(Qry, cn)
    '        cn.Close()
    '        cn.Open()
    '        Try
    '            ccccc = CInt(cmd.ExecuteScalar())
    '        Catch exx As Exception
    '            ccccc = 0
    '        End Try
    '        cn.Close()

    '        If ccccc > 0 Then
    '            Exit Sub
    '        End If

    '        Qry = "select count(*) from information_schema.TABLES where table_name='TSPL_DISPLAY_NOTIFICATIONS'"
    '        ccccc = clsDBFuncationality.getSingleValue(Qry)
    '        If ccccc <= 0 Then
    '            Return
    '        End If

    '        Dim xtime As String = ""
    '        xtime = clsCommon.myCstr(clsCommon.GetPrintDate(clsCommon.GETSERVERDATE(), "dd/MMM/yyyy hh:mm:ss tt"))
    '        Qry = "select 'Trans_Id : '+doc_id+' Notification : '+message+'(Detail :'+item_name+ ')' as values1 from TSPL_DISPLAY_NOTIFICATIONS where user_code='" + objCommonVar.CurrentUserCode + "' and status<>'1' and isnull(snooze_time,'')<='" + xtime + "'"
    '        Dim dt As DataTable = clsDBFuncationality.GetDataTable(Qry)
    '        Dim str As String = ""

    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            For Each dr As DataRow In dt.Rows()
    '                str = clsCommon.myCstr(dr("values1"))
    '                If clsCommon.myLen(str) > 0 Then
    '                    RadDesktopAlert1.AutoClose = False
    '                    RadDesktopAlert1.ShowOptionsButton = False
    '                    RadDesktopAlert1.ShowCloseButton = False


    '                    radbuttonelement.Tag = str
    '                    radbuttonDontShow.Tag = str


    '                    RadDesktopAlert1.FixedSize = New Size(529, 100)
    '                    RadDesktopAlert1.CaptionText = "Notification :"
    '                    RadDesktopAlert1.PopupAnimation = True
    '                    RadDesktopAlert1.ContentText = str
    '                    RadDesktopAlert1.Show()

    '                    arralert.Add(str, RadDesktopAlert1)
    '                    ReminderTimer.Enabled = False
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Private Sub radbuttonelement_Click(ByVal sender As Object, ByVal e As EventArgs)
        snoozeOrDontShowAgain(sender, True)
        '  ReminderTimer.Enabled = True
    End Sub

    Private Sub DontShowAgain_Click(ByVal sender As Object, ByVal e As EventArgs)
        snoozeOrDontShowAgain(sender, False)
        ' ReminderTimer.Enabled = True
    End Sub

    Sub snoozeOrDontShowAgain(ByVal sender As Object, ByVal issnoozed As Boolean)
        Dim radButtonElement As RadButtonElement = TryCast(sender, RadButtonElement)
        Dim strCode As String = clsCommon.myCstr(radButtonElement.Tag)
        If clsCommon.myLen(strCode) > 0 Then
            If arralert.ContainsKey(strCode) Then
                arralert(strCode).Hide()
                arralert.Remove(strCode)
                If issnoozed Then
                    '  clsfrmNotificationScreen.Snooze(strCode)
                Else
                    '  clsfrmNotificationScreen.DontShowAgain(strCode)
                End If
            End If
        End If
    End Sub
#End Region

    Private Sub RadMenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim OldThemeName As String = ""
            Dim clickedCtrl As Telerik.WinControls.UI.RadMenuItem = DirectCast(sender, Telerik.WinControls.UI.RadMenuItem)
            ThemeResolutionService.ApplicationThemeName = clickedCtrl.Text
            OldThemeName = ""
            Dim FILE_NAME As String = Application.StartupPath + "\Theme.Txp"
            If System.IO.File.Exists(FILE_NAME) Then
                '==============read theme name from existing file============
                Dim objreader As New System.IO.StringReader(FILE_NAME)
                If objreader IsNot Nothing AndAlso clsCommon.myLen(objreader) > 0 Then
                    OldThemeName = clsCommon.myCstr(objreader.ReadToEnd())

                End If
                '==================================
                System.IO.File.Delete(FILE_NAME)
            End If
            File.Create(FILE_NAME).Dispose()
            Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
            objWriter.Write(clickedCtrl.Text)
            Me.OldThemeName = clickedCtrl.Text
            objWriter.Close()
            'Me.OldThemeName = FILE_NAME
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try
    End Sub

    Sub LoadTheme()
        Try
            Dim line As String
            Dim objReader As New System.IO.StreamReader("Theme.Txp")
            Do While objReader.Peek() <> -1
                line = objReader.ReadLine()
                ThemeResolutionService.ApplicationThemeName = line
                OldThemeName = line
            Loop
            ''stuti regarding memory leakage
            objReader.Close()
            objReader.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub RadMenuItem5_DropDownOpening()
        Try

            'If OldThemeName IsNot Nothing AndAlso OldThemeName.Length > 0 Then
            '    RadMenuItem6.IsChecked = False
            '    RadMenuItem7.IsChecked = False
            '    RadMenuItem8.IsChecked = False
            '    RadMenuItem9.IsChecked = False
            '    RadMenuItem10.IsChecked = False
            '    RadMenuItem11.IsChecked = False
            '    RadMenuItem12.IsChecked = False
            '    RadMenuItem13.IsChecked = False
            '    RadMenuItem14.IsChecked = False
            '    RadMenuItem15.IsChecked = False
            '    RadMenuItem16.IsChecked = False
            '    RadMenuItem17.IsChecked = False
            '    RadMenuItem18.IsChecked = False
            '    RadMenuItem19.IsChecked = False
            '    RadMenuItem20.IsChecked = False
            '    RadMenuItem21.IsChecked = False
            '    RadMenuItem22.IsChecked = False
            '    If clsCommon.CompairString(RadMenuItem6.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem6.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem7.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem7.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem8.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem8.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem9.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem9.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem10.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem10.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem11.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem11.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem12.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem12.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem13.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem13.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem14.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem14.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem15.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem15.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem16.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem16.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem17.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem17.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem18.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem18.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem19.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem19.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem20.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem20.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem21.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem21.IsChecked = True
            '    ElseIf clsCommon.CompairString(RadMenuItem22.Text, OldThemeName) = CompairStringResult.Equal Then
            '        RadMenuItem22.IsChecked = True


            '    End If
            'End If

        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try

    End Sub

    Private Sub RadMenuItem5_DropDownOpening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
        RadMenuItem5_DropDownOpening()
    End Sub

    Private Sub Timer3_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If clsDBFuncationality.isQueryRun Then
                FrmMainTranScreen.LastWorkingTime = DateTime.Now()
            End If
            Dim IdleSec As Long = DateDiff(DateInterval.Second, FrmMainTranScreen.LastWorkingTime, DateTime.Now)
            '    Me.Text = IdleSec
            If IdleSec > 0 Then
                If IdleSec > IdleTimeinSeconds Then
                    isAutoClosing = True
                    'clsERPFuncationality.closeForm(Me)
                    Application.Restart()
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub MDI_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Try
            FrmMainTranScreen.LastWorkingTime = DateTime.Now()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub MDI_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            FrmMainTranScreen.LastWorkingTime = DateTime.Now()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub RadDock1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RadDock1.MouseMove

    End Sub

    Private Sub lblChangePWD_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        Try
            PasswordRules = clsCommon.myCBool(IIf(clsCommon.myCstr(clsFixedParameter.GetData(clsFixedParameterType.PasswordRules, clsFixedParameterCode.PasswordRules, Nothing)) = "1", True, False))
            If clsCommon.myLen(txtUserName.Text) <= 0 Then
                Throw New Exception("Please Enter User Name")
            End If
            Dim isUserfound As Integer = 0
            isUserfound = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select count(*) from tspl_user_master where user_code='" & txtUserName.Text & "'"))
            If isUserfound = 0 Then
                Throw New Exception("Invalid User Name")
            End If
            objCommonVar.CurrentUserCode = txtUserName.Text
            objCommonVar.CurrentUser = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select user_name from tspl_user_master where user_code='" & txtUserName.Text & "'"))

           



        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub

    Private Sub Receipt_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadMenuItem23_Click(sender As Object, e As EventArgs)
        'Dim frm As New FrmLicenceActivate()
        'frm.ShowDialog()
    End Sub

    Private Sub OpenFormFromOtherDLL()
        Try
            th = New Threading.Thread(AddressOf OpenFormFromOtherDLLMain)
            th.Start()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub OpenFormFromOtherDLLMain()
        If clsCommon.myLen(objCommonVar.ScreenToOpen) > 0 Then
            th1 = New Threading.Thread(AddressOf ShowFormFinal)
            th1.Start()
        End If

        Dim i As Int64 = 0
        While i <= 400000000
            i = i + 1
        End While
        'Try
        '    th1.Abort()
        'Catch ex As Exception
        'End Try

        OpenFormFromOtherDLLMain()
    End Sub
    Sub ShowFormFinal()
        Dim localScreenToOpen As String = objCommonVar.ScreenToOpen
        Dim localDocToOpen As String = objCommonVar.ScreenToOpenDocNo
        objCommonVar.ScreenToOpenDocNo = ""
        objCommonVar.ScreenToOpen = ""
   
      

    End Sub



    Private csd200Obj As CgtFpAccessCSD200Dotnet.MMMCogentCSD200APIs
    Private Sub RadButton17_Click(sender As Object, e As EventArgs) Handles RadButton17.Click
        Try
            FingerPrintScanner()
        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message, Me.Text)
        End Try

    End Sub

    Private Sub FingerPrintScanner()
        Try
            csd200Obj = New MMMCogentCSD200APIs()
            csd200Obj.initializeScanner()
            Dim num As Integer = -1
            Dim captureBytes As Byte() = Nothing
            Dim width As Integer = 0
            Dim height As Integer = 0
            Dim isoTemplateBytes As Byte() = Nothing
            Dim nfiq As Integer = 0
            pBoxFingerPrint1.Image = Nothing
            pBoxFingerPrint1.Refresh()
            pBoxFingerPrint2.Image = Nothing
            pBoxFingerPrint2.Refresh()

            num = csd200Obj.captureFP(&H7530, captureBytes, width, height, nfiq, isoTemplateBytes)
            Dim isBiomatrikFound As Boolean = False
            If (num = CSD200APICodes.SUCCESS) AndAlso (captureBytes IsNot Nothing) Then

                Dim ms As New MemoryStream()
                pBoxFingerPrint1.Image.Save(ms, ImageFormat.Bmp)
                Dim data As Byte() = ms.GetBuffer()
                Dim isoTemplateBytesForMatch1 As Byte() = Nothing
                ExtractTemplate(data, width, height, isoTemplateBytesForMatch1)



                Dim qry As String = "select User_Code,Password,biometric_image from tspl_user_master where biometric_image is not null"
                Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        ms = New MemoryStream()

                        data = DirectCast(dr("biometric_image"), Byte())
                        ms = New MemoryStream(data)
                        pBoxFingerPrint2.Image = Image.FromStream(ms)

                        ms = New MemoryStream()
                        pBoxFingerPrint2.Image.Save(ms, ImageFormat.Bmp)
                        Dim dataNew As Byte() = ms.GetBuffer()
                        Dim isoTemplateBytesForMatch2 As Byte() = Nothing

                        ExtractTemplate(dataNew, width, height, isoTemplateBytesForMatch2)

                        'clsCommon.MyMessageBoxShow("Next " + clsCommon.myCstr(dr("User_Code")))
                        If csd200Obj.matchTemplates(isoTemplateBytesForMatch1, isoTemplateBytesForMatch2) Then
                            txtUserName.Text = clsCommon.myCstr(dr("User_Code"))
                            txtPassword.Text = clsCommon.DecryptString(clsCommon.myCstr(dr("Password")))
                            CheckAndLogin()
                            isBiomatrikFound = True
                        End If

                    Next
                End If
                If Not isBiomatrikFound Then
                    clsCommon.MyMessageBoxShow("Invalid Fingerprint...")
                End If
            ElseIf num = CSD200APICodes.ERROR_TIMEOUT Then
                clsCommon.MyMessageBoxShow("Fingerprint Capture Timeout")
            Else
                clsCommon.MyMessageBoxShow("Fingerprint Capture Failed. ErrorCode: " + num)
            End If
        Catch exception As Exception
            MessageBox.Show(exception.Message)
        End Try
    End Sub


    Private Function ExtractTemplate(bImage As Byte(), width As Integer, height As Integer, ByRef bTemplateData As Byte()) As Integer
        Dim bExtract_Init As Boolean = True
        Dim [error] As Integer = -1
        Dim inParameter As Integer = 0
        Dim outMessage As New System.Text.StringBuilder(&H100)
        If Not bExtract_Init Then
            [error] = BioSdk710Wrapper.InitExtract("", "", inParameter, outMessage)
            If [error] < 0 Then
                Throw New BioSDK710Exception([error])
            End If
            bExtract_Init = True
        End If
        Dim outTemplateSize As Integer = 0
        outMessage = New System.Text.StringBuilder(&H100)
        Dim outTemplateData As Byte() = New Byte(2047) {}
        Dim destinationArray As Byte() = Nothing
        Dim num4 As Integer = BioSdk710Wrapper.ExtractTemplate(bImage, height, width, &HC5, &HC5, 0, outTemplateData, outTemplateSize, outMessage)
        If num4 < 0 Then
            Return num4
        End If
        If outTemplateSize > 0 Then
            destinationArray = New Byte(outTemplateSize - 1) {}
            Array.Copy(outTemplateData, destinationArray, outTemplateSize)
        End If
        bTemplateData = destinationArray
        Return outTemplateSize
    End Function



    Private Sub RadButton18_Click(sender As Object, e As EventArgs) Handles RadButton18.Click
        'Dim frm As New frmVersion
        'frm.MdiParent = Me
        'frm.Show()
    End Sub
End Class
