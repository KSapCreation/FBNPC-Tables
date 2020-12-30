
Imports common
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Telerik.WinControls.UI
Imports System.Drawing
Imports System


Public Class clsCreateAllTables
    Public Shared IsShowMenuOnRightClick As Boolean = False

    Public Shared Sub CreateAllTable()
        Dim timeSpam1 As TimeSpan = TimeSpan.FromMilliseconds(DateTime.Now.Millisecond)
        Try
            clsCommon.ProgressBarPercentShow()
            clsCommonFunctionality.TableCounter = 0
            clsCommonFunctionality.TableTotal = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select COUNT(1) from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE='BASE TABLE'"))
            Dim chk As Integer = 0
            chk = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("SELECT COUNT(OBJECT_ID) AS TotalTables FROM sys.tables"))
            If chk = 0 Then
                '   Dim strScript As String = XpertERPBlankTableScript.MainClass.GetQry()
                '  clsDBFuncationality.ExecuteNonQuery(strScript)
                clsCommonFunctionality.TableTotal = clsCommon.myCdbl(clsDBFuncationality.getSingleValue("select COUNT(1) from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE='BASE TABLE'"))
            End If

            Dim coll As Dictionary(Of String, String)

            '**************************************************************************************
            coll = New Dictionary(Of String, String)()
            coll.Add("User_Code", "Varchar(12) not null Primary key")
            coll.Add("First_Name", "varchar(50) NOT NULL")
            coll.Add("Last_Name", "varchar(50)  NOT NULL")
            coll.Add("Password", "varchar(100) null")
            coll.Add("ConformPassword", "Varchar(100) NULL ")
            coll.Add("Gender", "varchar(10)   NULL")
            coll.Add("DOB", "varchar(10) NULL")
            coll.Add("E_mail", "varchar(30) NULL")
            coll.Add("Phone", "varchar(20) NULL")
            coll.Add("MachineName", "varchar(50)  NULL")
            coll.Add("IP_Address", "varchar(20)  NULL")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("ExamName", "varchar(30) NULL")
            coll.Add("PraticeType", "integer  NOT NULL DEFAULT 0")
            coll.Add("AdminGroup", "integer  NOT NULL DEFAULT 0")
            coll.Add("LoginDate", "datetime NULL")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_USER_MASTER", coll)

            Try
                clsDBFuncationality.ExecuteNonQuery("alter table FBNPC_USER_MASTER alter column E_mail varchar(50)  NOT NULL")
            Catch ex As Exception
            End Try

            coll = New Dictionary(Of String, String)()
            coll.Add("ProgramsID", "integer IDENTITY(1,1) NOT NULL")
            coll.Add("TitleName", "varchar(20) null")
            coll.Add("TitleDescription", "Varchar(max) null")
            coll.Add("IsActive", "integer null")
            coll.Add("FileType", "Varchar(50) null")
            coll.Add("FileName", "Varchar(50) null")
            coll.Add("FileData", "varbinary(max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("Type", "Varchar(50) null")
            coll.Add("Country", "Varchar(50) null")
            coll.Add("State", "Varchar(50) null")
            coll.Add("City", "Varchar(50) null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Programs_Insert", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("SliderCode", "varchar(12) null")
            coll.Add("ShortName", "varchar(12) null")
            coll.Add("Description", "Varchar(max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_INFO_SLIDERS", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("GalleryID", "varchar(50) null")
            coll.Add("Name", "varchar(50) null")
            coll.Add("GalleryType", "Varchar(20) null")
            coll.Add("Category", "Varchar(20) null")
            coll.Add("CompanyName", "Varchar(100) null")
            coll.Add("YoutubeLink", "Varchar(150) null")
            coll.Add("FileName", "Varchar(80) null")
            coll.Add("FileType", "Varchar(50) null")
            coll.Add("FileData", "varbinary(Max) null")
            coll.Add("Description", "Varchar(Max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("Isactive", "Varchar(5) null")
            clsCommonFunctionality.CreateOrAlterTable("fbnpc_gallery_master", coll)

            'coll = New Dictionary(Of String, String)()
            'coll.Add("GalleryID", "varchar(50) not null")
            'coll.Add("Name", "varchar(50) null")
            'coll.Add("GalleryType", "Varchar(20) null")
            'coll.Add("Category", "Varchar(20) null")
            'coll.Add("CompanyName", "Varchar(100) null")
            'coll.Add("YoutubeLink", "Varchar(150) null")
            'coll.Add("FileType", "Varchar(50) null")
            'coll.Add("FileName", "Varchar(50) null")
            'coll.Add("FileData", "varbinary(max) null")
            'coll.Add("Description", "varchar(max) null")
            'coll.Add("CreatedBy", "Varchar(50) null")
            'coll.Add("CreatedDate", "datetime null")
            'coll.Add("ModifyBy", "Varchar(50) null")
            'coll.Add("ModifyDate", "datetime null")
            'coll.Add("IsActive", "integer null")
            'clsCommonFunctionality.CreateOrAlterTable("FBNPC_Programs_Insert", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("CategoryID", "varchar(20) not null")
            coll.Add("Name", "varchar(50) null")
            coll.Add("Description", "Varchar(max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("Isactive", "Varchar(10) null")
            coll.Add("DocType", "Varchar(20) null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Category_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("RegisterID", "Varchar(12) not null Primary key")
            coll.Add("FirstName", "varchar(50) not null")
            coll.Add("LastName", "Varchar(50) null")
            coll.Add("Address", "Varchar(max) null")
            coll.Add("City", "Varchar(30) null")
            coll.Add("State", "Varchar(20) null")
            coll.Add("Postal", "Varchar(20) null")
            coll.Add("PhoneNo", "Varchar(20) null")
            coll.Add("EmailID", "Varchar(30) null")
            coll.Add("ClassOption", "Varchar(20) null")
            coll.Add("ExamDate", "Varchar(20) null")
            coll.Add("SpecialRequest", "Varchar(max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Registration", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("BookID", "Varchar(12) not null Primary key")
            coll.Add("BookTitle", "varchar(50) not null")
            coll.Add("SortDesc", "Varchar(300) not null")
            coll.Add("Description", "Varchar(max) null")
            coll.Add("Isactive", "Varchar(10) null")
            coll.Add("Price", "Varchar(10) null")
            coll.Add("FileType", "Varchar(50) null")
            coll.Add("FileName", "Varchar(50) null")
            coll.Add("FileData", "varbinary(max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("Category", "varchar(30) not null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_StudyBooks", coll)

            Try
                clsDBFuncationality.ExecuteNonQuery("alter table FBNPC_StudyBooks alter column BookTitle varchar(250)  NOT NULL")
                clsDBFuncationality.ExecuteNonQuery("alter table FBNPC_StudyBooks alter column SortDesc varchar(800)  NOT NULL")

            Catch ex As Exception
            End Try

            coll = New Dictionary(Of String, String)()
            coll.Add("BatchID", "Varchar(12) not null Primary key")
            coll.Add("BatchName", "varchar(50) not null")
            coll.Add("BranchName", "Varchar(50) not null")
            coll.Add("Course", "Varchar(50) null")
            coll.Add("BatchDate", "Varchar(50) null")
            coll.Add("StartTime", "Varchar(50) null")
            coll.Add("EndTime", "Varchar(50) null")
            coll.Add("Isactive", "Varchar(10) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Batches", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("SubjectID", "Varchar(12) not null Primary key")
            coll.Add("SubjectName", "varchar(50) not null")
            coll.Add("Description", "Varchar(500) not null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Subjects", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("SectionID", "Varchar(12) not null Primary key")
            coll.Add("SectionName", "varchar(50) not null")
            coll.Add("Description", "Varchar(500) not null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("DocType", "Varchar(20) not null default 'Multiple'")
            coll.Add("SectionTime", "decimal(18,2) not null default 0")
            coll.Add("TimeType", "varchar(20) null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Sections", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("QuestionID", "Varchar(20) not null Primary key")
            coll.Add("Question", "varchar(max) not null")
            coll.Add("OptionOne", "Varchar(max) not null")
            coll.Add("OptionTwo", "Varchar(max) not null")
            coll.Add("OptionThree", "Varchar(max) not null")
            coll.Add("OptionFour", "Varchar(max) not null")
            coll.Add("CorrectAns", "Varchar(20) not null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_QustionsSheet", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("AVID", "Varchar(12) not null Primary key")
            coll.Add("SubjectName", "varchar(350)  null")
            coll.Add("FileName", "Varchar(200)  null")
            coll.Add("FileData", "varbinary(max)  null")
            coll.Add("FileType", "Varchar(200)  null")
            coll.Add("TransType", "Varchar(200)  null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Audio_Video_Master", coll)


            coll = New Dictionary(Of String, String)()
            coll.Add("ExamID", "Varchar(12) not null Primary key")
            coll.Add("ExamName", "varchar(50) not null")
            coll.Add("Description", "Varchar(500) not null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_ExamListName", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("PaperID", "Varchar(30) not null Primary key")
            coll.Add("Subject", "varchar(50) not null")
            coll.Add("Section", "Varchar(50) not null")
            coll.Add("ExamID", "Varchar(12)  not null references FBNPC_ExamListName(ExamID)")
            coll.Add("Posted", "integer  NOT NULL DEFAULT 0")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Paper_Set_Head", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("PaperID", "VARCHAR(30) not null REFERENCES FBNPC_Paper_Set_Head(PaperID)")
            coll.Add("Section", "Varchar(50) not null")
            coll.Add("ExamID", "Varchar(50)  not null")
            coll.Add("QuestionID", "Varchar(50) null")
            coll.Add("AVID", "datetime null")
            coll.Add("QusSelect", "bit not null")
            coll.Add("VideoID", "Varchar(200) null")
            coll.Add("ComprehensionID", "Varchar(30) null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Paper_Set_Detail", coll)


            Try
                clsDBFuncationality.ExecuteNonQuery("alter table FBNPC_Paper_Set_Detail alter column AVID varchar(250)  NOT NULL")
            Catch ex As Exception

            End Try



            coll = New Dictionary(Of String, String)()
            coll.Add("ReportID", "Varchar(100) null")
            coll.Add("DeptID", "Varchar(20) null")
            coll.Add("UserID", "Varchar(20) null")
            coll.Add("GridLayout", "Text null")
            coll.Add("GridColumns", "integer not null default 0")
            clsCommonFunctionality.CreateOrAlterTable("GridLayout", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("TearmsID", "Varchar(12) not null Primary key")
            coll.Add("TearmsCondition", "varchar(max) not null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_TearmsConditions", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("PaperID", "VARCHAR(30) not null REFERENCES FBNPC_Paper_Set_Head(PaperID)")
            coll.Add("ExamID", "varchar(20) not null")
            coll.Add("UserID", "varchar(20) not null")
            coll.Add("Submit", "integer  NOT NULL DEFAULT 0")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Select_User_Exam", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("AudioID", "Varchar(12) not null Primary key")
            coll.Add("SubjectName", "varchar(350)  null")
            coll.Add("FileName", "Varchar(200)  null")
            coll.Add("TransType", "Varchar(200)  null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Video_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("ExamName", "Varchar(12)  not null references FBNPC_ExamListName(ExamID)")
            coll.Add("StudentName", "Varchar(30) null")
            coll.Add("PaperID", "VARCHAR(30) not null REFERENCES FBNPC_Paper_Set_Head(PaperID)")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Exam_Question_Validtion", coll)


            coll = New Dictionary(Of String, String)()
            coll.Add("QuestionID", "Varchar(12) not null")
            coll.Add("OptionA", "integer not null default 0")
            coll.Add("OptionB", "integer not null default 0")
            coll.Add("OptionC", "integer not null default 0")
            coll.Add("OptionD", "integer not null default 0")
            coll.Add("ExamName", "Varchar(12)  not null references FBNPC_ExamListName(ExamID)")
            coll.Add("StudentName", "Varchar(30) null")
            coll.Add("PaperID", "VARCHAR(30) not null REFERENCES FBNPC_Paper_Set_Head(PaperID)")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("DocType", "varchar(20) null ")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Submit_Exam", coll)


            Try
                clsDBFuncationality.ExecuteNonQuery("alter table FBNPC_Submit_Exam alter column QuestionID varchar(300)  NOT NULL")
            Catch ex As Exception

            End Try

            coll = New Dictionary(Of String, String)()
            coll.Add("Hist_By", "Varchar(20)  not null ")
            coll.Add("Hist_Date", "Varchar(20)  not null ")
            coll.Add("Hist_Version", "Varchar(20)  not null ")
            coll.Add("ExamName", "Varchar(12)  not null ")
            coll.Add("StudentName", "Varchar(30) null")
            coll.Add("PaperID", "VARCHAR(30) not null ")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Exam_Question_Validtion_History", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("Hist_By", "Varchar(20)  not null ")
            coll.Add("Hist_Date", "Varchar(20)  not null ")
            coll.Add("Hist_Version", "Varchar(20)  not null ")
            coll.Add("QuestionID", "Varchar(300) not null")
            coll.Add("OptionA", "integer not null default 0")
            coll.Add("OptionB", "integer not null default 0")
            coll.Add("OptionC", "integer not null default 0")
            coll.Add("OptionD", "integer not null default 0")
            coll.Add("ExamName", "Varchar(12)  not null ")
            coll.Add("StudentName", "Varchar(30) null")
            coll.Add("PaperID", "VARCHAR(30) not null ")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Submit_Exam_History", coll)


            coll = New Dictionary(Of String, String)()
            coll.Add("ReadingID", "Varchar(12) not null Primary key")
            coll.Add("ComprehensionName", "varchar(350)  null")
            coll.Add("ComprehensionDesc", "Varchar(max)  null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Comprehension_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("ESM_Code", "Varchar(30) not null Primary key")
            coll.Add("StudentName", "varchar(350)  null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Exam_Student_Mapping_Head", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("ESM_Code", "VARCHAR(30) not null REFERENCES FBNPC_Exam_Student_Mapping_Head(ESM_Code)")
            coll.Add("ExamCode", "Varchar(12)  not null references FBNPC_ExamListName(ExamID)")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("QusSelect", "bit not null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Exam_Student_Mapping_Detail", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("IP_Address", "varchar(20) null")
            coll.Add("MachineName", "varchar(50) null")
            coll.Add("LoginDate", "datetime null")
            coll.Add("UserCode", "Varchar(50) null")
            coll.Add("MacAddress", "varchar(50) null")
            coll.Add("UserName", "Varchar(50) null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Login_History", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("UpdatesID", "varchar(20) not null Primary key")
            coll.Add("StudentName", "varchar(50) null")
            coll.Add("Description", "varchar(Max) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("FBNPC_Updates", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("Licence_ExpiredDate_Specification_B", "varchar(500) null")
            coll.Add("Licence_ExpiredDate_Description_A", "varchar(500) null")
            coll.Add("LicenceNoOf_Connection", "varchar(500) null")
            coll.Add("LicenceNoOf_Entry", "Varchar(500) null")
            coll.Add("LicenceNoOf_User", "Varchar(50) null")
            coll.Add("CompanyName", "Varchar(50) null")
            clsCommonFunctionality.CreateOrAlterTable("Licence_Master", coll)


            coll = New Dictionary(Of String, String)()
            coll.Add("Comp_Code", "varchar(8)  NOT NULL PRIMARY KEY ")
            coll.Add("Comp_Name", "varchar(100)  NOT NULL")
            coll.Add("Add1", "varchar(50) NULL")
            coll.Add("Add2", "varchar(50) NULL")
            coll.Add("Add3", "varchar(50) NULL")
            coll.Add("City_Code", "varchar(50) NULL")
            coll.Add("Fax", "varchar(12) NULL")
            coll.Add("Email", "varchar(50) NULL")
            coll.Add("Pincode", "varchar(20) NULL")
            coll.Add("State", "varchar(30) NULL")
            coll.Add("Tin_No", "varchar(20) NULL")
            coll.Add("CST_LST", "varchar(30) NULL")
            coll.Add("Regn_No", "varchar(30) NULL")
            coll.Add("Cform", "char(1) NULL")
            coll.Add("Mode_of_Trans", "varchar(30) NULL")
            coll.Add("Created_By", "varchar(12)  NOT NULL")
            coll.Add("Created_Date", "varchar(10)  NOT NULL")
            coll.Add("Modify_By", "varchar(12)  NOT NULL")
            coll.Add("Modify_Date", "varchar(10)  NOT NULL")
            coll.Add("Comp_Code1", "varchar(8)  NOT NULL")
            coll.Add("DataBase_Name", "Varchar(100) null")
            coll.Add("Logo_Img", "image null")
            coll.Add("Logo_Img2", "image null")
            coll.Add("Vat_Reg_No", "Varchar(30) null")
            coll.Add("ServiceTax_Reg_No", "Varchar(30) null")
            coll.Add("Ecc_No", "Varchar(30) null")
            coll.Add("CE_Range", "Varchar(30) null")
            coll.Add("CE_Commissionerate", "Decimal(18,2) null")
            coll.Add("CE_Division", "Varchar(30) null")
            coll.Add("Pan_No", "Varchar(30) null")
            coll.Add("Tan_No", "Varchar(30) null")
            coll.Add("Tcan_No", "Varchar(30) null")
            coll.Add("Circle_No", "Varchar(30) null")
            coll.Add("Ward_No", "Varchar(30) null")
            coll.Add("Access_Officer", "Varchar(30) null")
            coll.Add("NameInTally", "Varchar(100) null")
            coll.Add("IntegrateWithTally", "BIT NOT NULL DEFAULT '0'")
            coll.Add("BaseCurrencyCode", "VARCHAR(30)  NULL ")
            coll.Add("ApplyMultiCurrency", "bit not null default 0")
            coll.Add("Phone1", "Varchar(50) null")
            coll.Add("Phone2", "Varchar(50) null")
            coll.Add("Is_Main_Company", "char(1) NOT NULL DEFAULT '0'")
            coll.Add("Cust_Code", "varchar(12)")
            coll.Add("CINNo", "Varchar(50) null ")
            coll.Add("IECode", "Varchar(30) null ")
            coll.Add("Comp_ESIC_NO", "Varchar(30) null ")
            coll.Add("Comp_PF_NO", "Varchar(30) null ")
            coll.Add("Employer_Name", "Varchar(50) null ")
            coll.Add("Employer_Desg", "Varchar(50) null ")
            coll.Add("Employer_Add1", "Varchar(50) null ")
            coll.Add("Employer_Add2", "Varchar(50) null ")
            coll.Add("Employer_Add3", "Varchar(50) null ")
            coll.Add("Insurance_No", "varchar(50) null")
            coll.Add("Insurance_Comp_Name", "varchar(200) null")
            coll.Add("Insurance_Valid_Date", "datetime null")
            coll.Add("TinNo_Issue_Date", "datetime null")
            coll.Add("PanNo_Issue_Date", "datetime null")
            coll.Add("GSTReg_No", "Varchar(30) null ")
            coll.Add("GSTINNo", "Varchar(30) null ")
            coll.Add("Bank_Name", "Varchar(50) null ")
            coll.Add("BankAccountNo", "Varchar(30) null ")
            coll.Add("BankIFSCCode", "Varchar(30) null ")
            coll.Add("BankBranchAddress", "Varchar(100) null ")
            coll.Add("BackgroundImage", "image null")
            clsCommonFunctionality.CreateOrAlterTable("COMPANY_MASTER", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("ActivationID", "varchar(12) NOT NULL PRIMARY KEY ")
            coll.Add("UserName", "varchar(12) null")
            coll.Add("ActivationDateEnd", "datetime null")
            coll.Add("CompanyID", "Varchar(50) null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_Activation_Key", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("AchieverID", "varchar(20) not null PRIMARY KEY")
            coll.Add("FirstName", "varchar(30) null")
            coll.Add("LastName", "varchar(30) null")
            coll.Add("City", "varchar(30) null")
            coll.Add("Country", "varchar(30) null")
            coll.Add("OnLandingPage", "integer not null default 0")
            coll.Add("Description", "Varchar(max) null")
            coll.Add("Isactive", "Varchar(10) null")
            coll.Add("FileName", "Varchar(200)  null")
            coll.Add("FileData", "varbinary(max)  null")
            coll.Add("FileType", "Varchar(200)  null")
            coll.Add("CreatedBy", "Varchar(50) null")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyBy", "Varchar(50) null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("State", "varchar(30) null")
            coll.Add("DocType", "varchar(30) not null default 'Achiever'")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_Achiever_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("CountryID", "varchar(50) not null PRIMARY KEY")
            coll.Add("Name", "varchar(500) null")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_Country_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("StateID", "varchar(50) not null PRIMARY KEY")
            coll.Add("CountryID", "varchar(50)  not null references KSCN_Country_Master(CountryID)")
            coll.Add("Name", "varchar(500) null")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_State_Master", coll)

            coll = New Dictionary(Of String, String)()
            coll.Add("CityID", "varchar(50) not null PRIMARY KEY")
            coll.Add("CountryID", "varchar(50)  not null references KSCN_Country_Master(CountryID)")
            coll.Add("StateID", "varchar(50)  not null references KSCN_State_Master(StateID)")
            coll.Add("Name", "varchar(500) null")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_City_Master", coll)

            '  ExecuteAfterCreateTable()

            coll = New Dictionary(Of String, String)()
            coll.Add("QuestionID", "Varchar(12) not null")
            coll.Add("OptionA", "integer not null default 0")
            coll.Add("OptionB", "integer not null default 0")
            coll.Add("OptionC", "integer not null default 0")
            coll.Add("OptionD", "integer not null default 0")
            coll.Add("ExamName", "Varchar(12)  not null references FBNPC_ExamListName(ExamID)")
            coll.Add("StudentName", "Varchar(30) null")
            coll.Add("PaperID", "VARCHAR(30) not null REFERENCES FBNPC_Paper_Set_Head(PaperID)")
            coll.Add("CreatedDate", "datetime null")
            coll.Add("ModifyDate", "datetime null")
            coll.Add("DocType", "varchar(20) null ")
            coll.Add("QusNo", "varchar(20) null ")
            clsCommonFunctionality.CreateOrAlterTable("KSCN_Temp_Table_Exam", coll)

            clsCommon.ProgressBarPercentHide()

            '' adding Standard Methods List 
            '   clsStandardMethods.AddStandardFunction()
            '  clsProgramIdFormNameMapping.setAllProgramFormName()
            ''

            'Try
            '    Dim qry As String = " select distinct OtherAssemblyFilePathAndName  from TSPL_PROGRAM_MASTER  where isnull(IsLoadFromOtherAssembly ,0)=1"
            '    Dim dt As DataTable = clsDBFuncationality.GetDataTable(qry)
            '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            '        For i As Integer = 0 To dt.Rows.Count - 1
            '            Dim AsmName As String = clsCommon.myCstr(dt.Rows(i)("OtherAssemblyFilePathAndName"))
            '            InvokeMethodSlow(AsmName, "clsCreateAllTableCustom", "CreateAllTable", Nothing)
            '        Next
            '    End If
            'Catch ex As Exception
            'End Try
        Catch ex As Exception
            clsCommon.ProgressBarPercentHide()
            Throw New Exception(ex.Message)
        End Try
    End Sub




    Shared Sub ExecuteAfterCreateTable()
        Try
            clsCommon.ProgressBarUpdate("Running Alter Table scripts")

            Dim qry As String = "alter table TSPL_PURCHASE_ORDER_DETAIL alter column PurchaseOrder_Qty decimal(18,10) null"
            ExecuteQeuryWithCatch(qry)

            

        Catch ex As Exception
        End Try
    End Sub

    Shared Sub ExecuteQeuryWithCatch(ByVal qry As String)
        Try
            clsDBFuncationality.ExecuteNonQuery(qry)
        Catch ex As Exception
            'clsCommon.MyMessageBoxShow("Error in After Create All table ")
        End Try
    End Sub

    Public Shared Function CreateInventorySummaryTables()
        '' Inventory Management Tables
        Dim coll As Dictionary(Of String, String)
        coll = New Dictionary(Of String, String)()
        coll.Add("INV_ID", "integer NOT NULL identity PRIMARY KEY")
        coll.Add("TRANS_DATE", "DATE")
        coll.Add("Location_Code", "VARCHAR(12) NOT NULL")
        coll.Add("Item_Code", "VARCHAR(50) NOT NULL")
        coll.Add("Item_Desc", "VARCHAR(100) NOT NULL")
        coll.Add("Stock_UOM", "VARCHAR(12) NOT NULL")

        coll.Add("FIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("LIFO_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("In_Avg_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("Out_Avg_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("Avg_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("IN_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("Out_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("TRANS_QTY", "decimal(28,2) NOT NULL default 0")

        coll.Add("In_Fat_KG", "FLOAT NOT NULL default 0")
        coll.Add("Out_Fat_KG", "FLOAT NOT NULL default 0")
        coll.Add("Fat_KG", "FLOAT NOT NULL default 0")

        coll.Add("In_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("Out_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("SNF_KG", "FLOAT NOT NULL default 0")

        coll.Add("CL_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("CL_SNF_KG", "FLOAT NOT NULL default 0")

        coll.Add("CL_FIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_LIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_Avg_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("AGEING_Flag", "bit NOT NULL default 0")
        coll.Add("AGEING_QTY", "decimal(28,2) NOT NULL default 0")

        coll.Add("QC_In_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_Out_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_FAT_KG", "FLOAT NOT NULL default 0 ")

        coll.Add("QC_In_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_Out_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_SNF_KG", "FLOAT NOT NULL default 0") '' QC_SNF_KG = QC_In_SNF_KG - QC_Out_SNF_KG 

        coll.Add("CL_QC_FAT_KG", "FLOAT NOT NULL default 0") ''Op_QC_Fat_KG = CL_QC_FAT_KG - QC_SNF_KG
        coll.Add("CL_QC_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("IsFromMilk", "bit NOT NULL default 0")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_INV_MOVE_DL", coll, " unique (TRANS_DATE,LOCATION_CODE,ITEM_CODE,STOCK_UOM)")

        '**************************************************************************************
        coll = New Dictionary(Of String, String)()
        coll.Add("HCODE", "Varchar(30) not null Primary key")
        coll.Add("DOC_DATE", "date NOT NULL")
        coll.Add("Type", "char(1)  NOT NULL")
        coll.Add("Status", "integer not null default 0")
        coll.Add("Description", "Varchar(100) NULL ")
        coll.Add("Created_By", "varchar(12)  NOT NULL")
        coll.Add("Created_Date", "datetime  NOT NULL")
        coll.Add("Modify_By", "varchar(12)  NOT NULL")
        coll.Add("Modify_Date", "datetime  NOT NULL")
        coll.Add("Posted_By", "varchar(12)  NULL")
        coll.Add("Posted_Date", "Datetime  NULL")
        coll.Add("Comp_code", "varchar(8)  NOT NULL")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_WISE_TAX", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("DCODE", "Varchar(30) not null Primary key")
        coll.Add("SNO", "integer null")
        coll.Add("HCODE", "Varchar(30) not null references TSPL_ITEM_WISE_TAX(HCODE)")
        coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        coll.Add("Tax_Group_Code", "Varchar(12) not null")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_WISE_TAX_GROUP", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("DDCODE", "Varchar(30) not null Primary key")
        coll.Add("SNO", "integer null")
        coll.Add("DCODE", "Varchar(30) not null references TSPL_ITEM_WISE_TAX_GROUP(DCODE)")
        coll.Add("HCODE", "Varchar(30) not null references TSPL_ITEM_WISE_TAX(HCODE)")
        coll.Add("Tax_Authority", "varchar(12)  NULL")
        coll.Add("TAX_Rate", "decimal (18,2) NULL")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_WISE_TAX_AUTHORITY", coll)
        '==============================================================

        ' Inventory Management Tables
        coll = New Dictionary(Of String, String)()
        coll.Add("INV_ID", "integer NOT NULL identity PRIMARY KEY")
        coll.Add("TRANS_DATE", "DATE")
        coll.Add("Location_Code", "VARCHAR(12) NOT NULL")
        coll.Add("Item_Code", "VARCHAR(50) NOT NULL")
        coll.Add("Item_Desc", "VARCHAR(100) NOT NULL")
        coll.Add("Stock_UOM", "VARCHAR(12) NOT NULL")

        coll.Add("Trans_Type", "varchar(30) NULL")
        coll.Add("FIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("LIFO_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("In_Avg_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("Out_Avg_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("Avg_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("IN_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("Out_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("TRANS_QTY", "decimal(28,2) NOT NULL default 0")

        coll.Add("In_Fat_KG", "FLOAT NOT NULL default 0")
        coll.Add("Out_Fat_KG", "FLOAT NOT NULL default 0")
        coll.Add("Fat_KG", "FLOAT NOT NULL default 0")

        coll.Add("In_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("Out_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("SNF_KG", "FLOAT NOT NULL default 0")

        coll.Add("CL_QTY", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("CL_SNF_KG", "FLOAT NOT NULL default 0")

        coll.Add("CL_FIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_LIFO_Cost", "decimal(28,2) NOT NULL default 0")
        coll.Add("CL_Avg_Cost", "decimal(28,2) NOT NULL default 0")

        coll.Add("AGEING_Flag", "bit NOT NULL default 0")
        coll.Add("AGEING_QTY", "decimal(28,2) NOT NULL default 0")

        coll.Add("QC_In_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_Out_FAT_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_FAT_KG", "FLOAT NOT NULL default 0 ")

        coll.Add("QC_In_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_Out_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("QC_SNF_KG", "FLOAT NOT NULL default 0") '' QC_SNF_KG = QC_In_SNF_KG - QC_Out_SNF_KG 

        coll.Add("CL_QC_FAT_KG", "FLOAT NOT NULL default 0") ''Op_QC_Fat_KG = CL_QC_FAT_KG - QC_SNF_KG
        coll.Add("CL_QC_SNF_KG", "FLOAT NOT NULL default 0")
        coll.Add("IsFromMilk", "bit NOT NULL default 0")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_INV_MOVE_TRANS_DL", coll, " unique (TRANS_DATE,LOCATION_CODE,ITEM_CODE,STOCK_UOM,Trans_Type)")

        Try
            clsDBFuncationality.ExecuteNonQuery(" drop table TSPL_ITEM_WISE_TAX_DETAILS ")
        Catch ex As Exception

        End Try
        Try
            clsDBFuncationality.ExecuteNonQuery(" drop table TSPL_ITEM_WISE_TAX_HEAD")
        Catch ex As Exception

        End Try

        'coll = New Dictionary(Of String, String)()
        'coll.Add("DOC_CODE", "Varchar(30) not null Primary key")
        'coll.Add("DOC_DATE", "datetime NOT NULL")
        'coll.Add("Tax_Group_Type", "char(1)  NOT NULL")
        'coll.Add("Created_By", "varchar(12)  NOT NULL")
        'coll.Add("Created_Date", "date  NOT NULL")
        'coll.Add("Modify_By", "varchar(12)  NOT NULL")
        'coll.Add("Modify_Date", "date  NOT NULL")
        'coll.Add("Comp_code", "varchar(8)  NOT NULL")
        'clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_WISE_TAX_HEAD", coll)

        'coll = New Dictionary(Of String, String)()
        'coll.Add("DOC_CODE", "Varchar(30) not null references TSPL_ITEM_WISE_TAX_HEAD(DOC_CODE)")
        'coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        'coll.Add("lineNo", "integer not null default 0")
        'coll.Add("Tax_Group_Code", "Varchar(12) not null")
        'coll.Add("Tax1_Code", "varchar(12)  NULL")
        'coll.Add("TAX1_Rate", "decimal (18,2) NULL")
        'coll.Add("Tax2_Code", "varchar(12)  NULL")
        'coll.Add("TAX2_Rate", "decimal (18,2) NULL")
        'coll.Add("Tax3_Code", "varchar(12)  NULL")
        'coll.Add("TAX3_Rate", "decimal (18,2) NULL")
        'coll.Add("Tax4_Code", "varchar(12)  NULL")
        'coll.Add("TAX4_Rate", "decimal (18,2) NULL")
        'coll.Add("Tax5_Code", "varchar(12)  NULL")
        'coll.Add("TAX5_Rate", "decimal (18,2) NULL")
        'clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_WISE_TAX_DETAILS", coll)

        '***************************************************************************************
        coll = New Dictionary(Of String, String)()
        coll.Add("TRANSFER_NO", "Varchar(30) not null Primary key")
        coll.Add("TRANSFER_DATE", "datetime NOT NULL")
        coll.Add("From_Locaction", "varchar(12)   NULL")
        coll.Add("To_Locaction", "varchar(12)   NULL")
        coll.Add("Vendor_Code", "varchar(12)   NULL")
        coll.Add("Remarks", "varchar(200)   NULL")
        coll.Add("Vehicle_Code", "varchar(12)   NULL")
        coll.Add("Vehicle_No", "varchar(50)   NULL")
        coll.Add("Status", "integer not null default 0")
        coll.Add("Created_By", "varchar(12)  NOT NULL")
        coll.Add("Created_Date", "datetime  NOT NULL")
        coll.Add("Modify_By", "varchar(12)  NOT NULL")
        coll.Add("Modify_Date", "datetime  NOT NULL")
        coll.Add("Posted_By", "varchar(12)  NULL")
        coll.Add("Post_Date", "datetime  NULL")
        coll.Add("Comp_code", "varchar(8)  NOT NULL")
        coll.Add("AgainstSRN_No", "Varchar(30)  null ")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_JOB_WORK_OUTWARD_TRANSFER_HEAD", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("TRANSFER_NO", "Varchar(30) not null references TSPL_JOB_WORK_OUTWARD_TRANSFER_HEAD(TRANSFER_NO)")
        coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        coll.Add("UOM", "varchar(12)  NULL")
        coll.Add("Qty", "decimal(18, 2) NULL")
        coll.Add("line_No", "integer not null default 0")
        coll.Add("Rate", "decimal(18, 2) not null default 0")
        coll.Add("Amount", "decimal(18, 2) not null default 0")


        clsCommonFunctionality.CreateOrAlterTable("TSPL_JOB_WORK_OUTWARD_TRANSFER_DETAILS", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("COST_CODE", "Varchar(30) not null Primary key")
        coll.Add("COST_DATE", "datetime NOT NULL")
        coll.Add("Description", "Varchar(200) NULL ")
        coll.Add("Created_By", "varchar(12)  NOT NULL")
        coll.Add("Created_Date", "datetime  NOT NULL")
        coll.Add("Modify_By", "varchar(12)  NOT NULL")
        coll.Add("Modify_Date", "datetime  NOT NULL")
        coll.Add("Comp_code", "varchar(8)  NOT NULL")
        coll.Add("GL_Acc", "VARCHAR(50) NULL")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_OVERHEAD_COST", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("GROUP_CODE", "Varchar(30) not null Primary key")
        coll.Add("GROUP_DATE", "datetime NOT NULL")
        coll.Add("Description", "Varchar(200) NULL ")
        coll.Add("Created_By", "varchar(12)  NOT NULL")
        coll.Add("Created_Date", "datetime  NOT NULL")
        coll.Add("Modify_By", "varchar(12)  NOT NULL")
        coll.Add("Modify_Date", "datetime  NOT NULL")
        coll.Add("Comp_code", "varchar(8)  NOT NULL")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_OVERHEAD_COST_GROUP_HEAD", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("GROUP_CODE", "Varchar(30) not null references TSPL_OVERHEAD_COST_GROUP_HEAD(GROUP_CODE)")
        coll.Add("SNO", "integer null")
        coll.Add("COST_CODE", "Varchar(30) not NULL References TSPL_OVERHEAD_COST(COST_CODE)")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_OVERHEAD_COST_GROUP_DETAILS", coll)

        Try
            clsDBFuncationality.ExecuteNonQuery(" drop table TSPL_ITEM_COST_MAPPING_DETAILS ")
        Catch ex As Exception

        End Try
        Try
            clsDBFuncationality.ExecuteNonQuery(" drop table TSPL_ITEM_COST_MAPPING_HEAD")
        Catch ex As Exception

        End Try

        'coll = New Dictionary(Of String, String)()
        'coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        'coll.Add("UOM", "varchar(50)  NOT NULL")
        'coll.Add("GROUP_CODE", "Varchar(30) not null references TSPL_OVERHEAD_COST_GROUP_HEAD(GROUP_CODE)")
        'coll.Add("Description", "Varchar(200) NULL ")
        'coll.Add("Created_By", "varchar(12)  NOT NULL")
        'coll.Add("Created_Date", "datetime  NOT NULL")
        'coll.Add("Modify_By", "varchar(12)  NOT NULL")
        'coll.Add("Modify_Date", "datetime  NOT NULL")
        'coll.Add("Comp_code", "varchar(8)  NOT NULL")
        'clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_COST_MAPPING_HEAD", coll)

        'coll = New Dictionary(Of String, String)()
        'coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        'coll.Add("UOM", "varchar(50)  NOT NULL")
        'coll.Add("SNO", "integer null")
        'coll.Add("COST_CODE", "Varchar(30) not NULL References TSPL_OVERHEAD_COST(COST_CODE)")
        'coll.Add("COST", "decimal(18, 2) not null default 0")
        'clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_COST_MAPPING_DETAILS", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("HCODE", "Varchar(30) not null Primary key")
        coll.Add("DOC_DATE", "date NOT NULL")
        coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        coll.Add("UOM", "varchar(50)  NOT NULL")
        coll.Add("GROUP_CODE", "Varchar(30) not null references TSPL_OVERHEAD_COST_GROUP_HEAD(GROUP_CODE)")
        coll.Add("Description", "Varchar(200) NULL ")
        coll.Add("Start_Date", "datetime  NOT NULL")
        coll.Add("End_Date", "datetime   NULL")
        coll.Add("TOTAL_COST", "decimal(18, 2) not null default 0")
        coll.Add("Status", "integer not null default 0")
        coll.Add("Created_By", "varchar(12)  NOT NULL")
        coll.Add("Created_Date", "datetime  NOT NULL")
        coll.Add("Modify_By", "varchar(12)  NOT NULL")
        coll.Add("Modify_Date", "datetime  NOT NULL")
        coll.Add("Posted_By", "varchar(12)   NULL")
        coll.Add("Posted_Date", "datetime   NULL")
        coll.Add("Comp_code", "varchar(8)  NOT NULL")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_COST_MAPPING_HEADS", coll)

        coll = New Dictionary(Of String, String)()
        coll.Add("DCODE", "Varchar(30) not null Primary key")
        coll.Add("HCODE", "Varchar(30) not null references TSPL_ITEM_COST_MAPPING_HEADS(HCODE)")
        coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        coll.Add("UOM", "varchar(50)  NOT NULL")
        coll.Add("SNO", "integer null")
        coll.Add("COST_CODE", "Varchar(30) not NULL References TSPL_OVERHEAD_COST(COST_CODE)")
        coll.Add("COST", "decimal(18, 2) not null default 0")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_COST_MAPPING_DETAIL", coll)


        coll = New Dictionary(Of String, String)()
        coll.Add("DDCODE", "Varchar(30) not null Primary key")
        coll.Add("HCODE", "Varchar(30) not null references TSPL_ITEM_COST_MAPPING_HEADS(HCODE)")
        coll.Add("Item_Code", "Varchar(50) not NULL References TSPL_ITEM_MASTER(Item_Code)")
        coll.Add("UOM", "varchar(50)  NOT NULL")
        coll.Add("SNO", "integer null")
        coll.Add("COST_CODE", "Varchar(30) not NULL References TSPL_OVERHEAD_COST(COST_CODE)")
        coll.Add("COST", "decimal(18, 2) not null default 0")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_ITEM_COST_MAPPING_DETAILS_ALL", coll)


        coll = New Dictionary(Of String, String)()
        coll.Add("Document_Code", "Varchar(30)")
        coll.Add("SNO", "integer null")
        Coll.add("COST_CODE", "Varchar(30) not NULL References TSPL_OVERHEAD_COST(COST_CODE)")
        coll.Add("HCODE", "Varchar(30) not null references TSPL_ITEM_COST_MAPPING_HEADS(HCODE)")
        coll.Add("OverHead_Cost", "decimal(18, 2) not null default 0")
        clsCommonFunctionality.CreateOrAlterTable("TSPL_BOM_OVERHEAD_COST_MAPPING_DETAILS", coll)

        Return True
    End Function
    Public Shared Sub InvokeMethodSlow(AssemblyName As String, ClassName As String, MethodName As String, args As Object())
        Try
            Dim ass As Assembly = Assembly.LoadFrom(Application.StartupPath & "\" & AssemblyName)
            Dim FileAtt As String = IO.Path.GetFileNameWithoutExtension(AssemblyName)
            Dim factory As Object = ass.CreateInstance(FileAtt & "." & ClassName, True)
            Dim t As Type = factory.GetType
            Dim method As MethodInfo = t.GetMethod(MethodName)
            Dim obj As Object = method.Invoke(factory, args)
        Catch ex As Exception
        End Try


    End Sub

    Public Shared Sub InvokeMethodFromCurrentAssembly(ClassName As String, MethodName As String, args As Object())
        Try
            clsCommon.MyMessageBoxShow("Hi")
            Dim ass As Assembly = Assembly.GetExecutingAssembly()
            Dim factory As Object = ass.CreateInstance(ClassName, True)
            Dim t As Type = factory.GetType
            Dim method As MethodInfo = t.GetMethod(MethodName)
            'Dim Mymethodbase As MethodBase = t.GetMethod(MethodName)
            'Dim Myarray As ParameterInfo() = Mymethodbase.GetParameters()
            'If Myarray.Length <> args.Length Then Exit Sub
            'Dim i As Integer = 0
            'If Myarray IsNot Nothing AndAlso Myarray.Length > 0 Then
            '    For Each Myparam As ParameterInfo In Myarray
            '        If TypeOf Myparam.ParameterType Is System.Int16 Then
            '            args(i) = clsCommon.myCdbl(args(i))
            '        End If

            '        i = i + 1
            '    Next
            'End If
            Dim obj As Object = method.Invoke(factory, args)
        Catch ex As Exception
        End Try


    End Sub


    Public Sub testMethod(a As Double, b As Double)
        '  clsCommon.MyMessageBoxShow("Hello!! This is Method Calling Example From Custom field Button")
        'Dim rString As List(Of String) = getControlsOnForm()
        'If rString IsNot Nothing AndAlso rString.Count > 0 Then
        '    clsCommon.MyMessageBoxShow("Control Name: " & rString(0) & ", Type:" & rString(1))
        'End If
        clsCommon.MyMessageBoxShow("Value Passed Are: " & a & " And " & b & " and Its Sum is : " & (a + b))
    End Sub

    Public Shared Function getControlsOnForm() As List(Of String)
        Dim dt As DataTable = Nothing
        Dim rString As New List(Of String)
        Dim ctr As New List(Of Control)
        Try
            clsCommon.MyMessageBoxShow("Hi")
            Dim assName As String = Application.StartupPath & "\" & "ERP.EXE"
            Dim ClassName As String = "ERP.FrmMccDispatch"
            Dim FormName As String = clsCommon.myCstr(ClassName)
            Dim AsmName As String = clsCommon.myCstr(assName)
            Dim AsmToLoad As Assembly = Nothing
            Dim obj As Object = Nothing
            AsmToLoad = Assembly.LoadFile(AsmName)
            Dim classType As Type = AsmToLoad.[GetType](FormName)
            obj = AsmToLoad.CreateInstance(FormName, True)
            Dim frm As FrmMainTranScreen = TryCast(obj, RadForm)
            findAndReturnContols(frm, ctr, Nothing)
            Dim qry As String = ""
            If ctr IsNot Nothing AndAlso ctr.Count > 0 Then
                For i As Integer = 0 To ctr.Count - 1
                    qry = qry & "select " & i + 1 & " as SLNO, '" & ctr(i).Name & "' as ControlName, '" & clsCommon.myCstr(ctr(i).GetType().Name) & "' as ControlType " & Environment.NewLine
                    If i < ctr.Count - 1 Then
                        qry = qry & " union all " & Environment.NewLine
                    End If
                Next
                Dim controlNum As Integer = clsCommon.myCdbl(clsCommon.ShowSelectForm("ControlList", qry, "SLNO", "", "", "", True)) - 1
                If controlNum >= 0 Then
                    Dim ControlName As String = ctr(controlNum).Name
                    Dim ControlType As String = ctr(controlNum).GetType().Name

                    rString.Add(ControlName)
                    rString.Add(ControlType)
                End If
            End If

        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try
        Return rString
    End Function

    Public Shared Sub getControlsOnForm(formId As String, ByRef gv As common.UserControls.MyRadGridView)
        Dim dt As DataTable = Nothing
        Dim rString As New List(Of String)
        Dim ctr As New List(Of Control)
        Try
            clsCommon.MyMessageBoxShow("Hi")
            Dim formName As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select isnull(MainFormName,'') as MainFormName from TSPL_PROGRAM_MASTER where program_code='" & formId & "'"))
            If clsCommon.myLen(formName) <= 0 Then
                Throw New Exception("Screen Not Mapped for Screen : " & formId)
            End If

            Dim asmnm As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select isnull(AsmName,'') as AsmName from TSPL_PROGRAM_MASTER where program_code='" & formId & "'"))
            If clsCommon.myLen(asmnm) <= 0 Then
                Throw New Exception("Assambly Not Mapped for Screen : " & formId)
            End If

            Dim assName As String = Application.StartupPath & "\" & asmnm
            Dim className As String = formName
            formName = "ERP." & formName

            Dim AsmName As String = clsCommon.myCstr(assName)
            Dim AsmToLoad As Assembly = Nothing
            Dim obj As Object = Nothing
            AsmToLoad = Assembly.LoadFile(AsmName)
            Dim classType As Type = AsmToLoad.[GetType](formName)
            obj = AsmToLoad.CreateInstance(formName, True)
            Dim frm As FrmMainTranScreen = TryCast(obj, RadForm)
            findAndReturnContols(frm, ctr, Nothing)
            Dim qry As String = ""
            If ctr IsNot Nothing AndAlso ctr.Count > 0 Then
                For i As Integer = 0 To ctr.Count - 1
                    Dim desc As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select description from TSPL_SCREEN_CONTROL_MASTER where programCode='" & formId & "' and controlName='" & clsCommon.myCstr(ctr(i).Name) & "'"))
                    Dim TableName As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select TableName from TSPL_SCREEN_CONTROL_MASTER where programCode='" & formId & "' and controlName='" & clsCommon.myCstr(ctr(i).Name) & "'"))
                    Dim FieldName As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select FieldName from TSPL_SCREEN_CONTROL_MASTER where programCode='" & formId & "' and controlName='" & clsCommon.myCstr(ctr(i).Name) & "'"))
                    Dim ProgramName As String = clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Program_Name from tspl_program_master where program_Code='" & formId & "'"))
                    qry = qry & "select " & i + 1 & " as SLNO, '" & formId & "' as ScreenCode, '" & className & "' as ScreenDesc,'" & ProgramName & "' as ProgramName, '" & ctr(i).Name & "' as ControlName, '" & clsCommon.myCstr(ctr(i).GetType().Name) & "' as ControlType, '" & IIf(clsCommon.myLen(desc) > 0, desc, "") & "' as Description,'" & TableName & "' as TableName,'" & FieldName & "' as FieldName " & Environment.NewLine
                    If i < ctr.Count - 1 Then
                        qry = qry & " union all " & Environment.NewLine
                    End If
                Next
                dt = clsDBFuncationality.GetDataTable(qry)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    gv.DataSource = dt
                    gv.Columns(0).ReadOnly = True
                    gv.Columns(1).ReadOnly = True
                    gv.Columns(2).ReadOnly = True
                    gv.Columns(3).ReadOnly = True
                    gv.Columns(2).IsVisible = False
                    gv.Columns(4).ReadOnly = True
                    gv.Columns(5).ReadOnly = True
                    gv.Columns("Description").ReadOnly = False
                    gv.Columns(6).ReadOnly = False
                    gv.Columns(7).ReadOnly = False
                    gv.BestFitColumns()
                    gv.EnableFiltering = True
                    gv.AllowAddNewRow = False

                Else
                    Throw New Exception("No Control Found")
                End If
                'Dim controlNum As Integer = clsCommon.myCdbl(clsCommon.ShowSelectForm("ControlList", qry, "SLNO", "", "", "", True)) - 1
                'If controlNum >= 0 Then
                '    Dim ControlName As String = ctr(controlNum).Name
                '    Dim ControlType As String = ctr(controlNum).GetType().Name

                '    rString.Add(ControlName)
                '    rString.Add(ControlType)
                'End If
            End If

        Catch ex As Exception
            clsCommon.MyMessageBoxShow(ex.Message)
        End Try
    End Sub

    Public Shared Sub findAndReturnContols(ByRef formname As FrmMainTranScreen, ByRef ctr As List(Of Control), Optional ByVal contrl As Control = Nothing)

        If IsNothing(contrl) Then
            For Each ctrl As Control In formname.Controls
                If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
                    findAndReturnContols(formname, ctr, ctrl)
                End If
                If Not (TypeOf ctrl Is RadGroupBox OrElse TypeOf ctrl Is SplitContainer OrElse TypeOf ctrl Is RadPanel OrElse TypeOf ctrl Is Panel OrElse TypeOf ctrl Is GroupBox OrElse TypeOf ctrl Is common.UserControls.MyRadGridView) AndAlso clsCommon.myLen(ctrl.Name) > 0 Then
                    ctr.Add(ctrl)
                End If
            Next
        Else
            For Each ctrl As Control In contrl.Controls
                If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
                    findAndReturnContols(formname, ctr, ctrl)
                End If
                If Not (TypeOf ctrl Is RadGroupBox OrElse TypeOf ctrl Is SplitContainer OrElse TypeOf ctrl Is RadPanel OrElse TypeOf ctrl Is Panel OrElse TypeOf ctrl Is GroupBox OrElse TypeOf ctrl Is common.UserControls.MyRadGridView) AndAlso clsCommon.myLen(ctrl.Name) > 0 Then
                    ctr.Add(ctrl)
                End If
            Next
        End If
    End Sub

    Public Shared Sub FindAnyCntrolByFieldName(ByRef formname As FrmMainTranScreen, ByRef ctr As Control, ctrName As String, Optional ByVal contrl As Control = Nothing)

        If IsNothing(contrl) Then
            For Each ctrl As Control In formname.Controls
                If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
                    FindAnyCntrolByFieldName(formname, ctr, ctrName, ctrl)
                End If
                If TypeOf ctrl Is MyNumBox Then
                    If clsCommon.CompairString(TryCast(ctrl, MyNumBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyTextBox Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyTextBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.UserControls.txtFinder Then
                    If clsCommon.CompairString(TryCast(ctrl, common.UserControls.txtFinder).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.UserControls.txtNavigator Then
                    If clsCommon.CompairString(TryCast(ctrl, common.UserControls.txtNavigator).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyDateTimePicker Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyDateTimePicker).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyComboBox Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyComboBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyLabel Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyLabel).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
            Next
        Else
            For Each ctrl As Control In contrl.Controls
                If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
                    FindAnyCntrolByFieldName(formname, ctr, ctrName, ctrl)
                End If
                If TypeOf ctrl Is MyNumBox Then
                    If clsCommon.CompairString(TryCast(ctrl, MyNumBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyTextBox Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyTextBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.UserControls.txtFinder Then
                    If clsCommon.CompairString(TryCast(ctrl, common.UserControls.txtFinder).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.UserControls.txtNavigator Then
                    If clsCommon.CompairString(TryCast(ctrl, common.UserControls.txtNavigator).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyDateTimePicker Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyDateTimePicker).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyComboBox Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyComboBox).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
                If TypeOf ctrl Is common.Controls.MyLabel Then
                    If clsCommon.CompairString(TryCast(ctrl, common.Controls.MyLabel).FieldName, ctrName) = CompairStringResult.Equal Then
                        ctr = ctrl
                    End If
                End If
            Next
        End If
    End Sub

    'Public Shared Sub FindAnyCntrolByFieldName(ByRef formname As XpertERPEngine.FrmMainTranScreen, ByRef ctr As Control, ctrName As String, Optional ByVal contrl As Control = Nothing)

    '    If IsNothing(contrl) Then
    '        For Each ctrl As Control In formname.Controls
    '            If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
    '                FindAnyCntrolByFieldName(formname, ctr, ctrName, ctrl)
    '            End If
    '            If TypeOf ctrl Is MyNumBox Then
    '                If clsCommon.CompairString(TryCast(ctr, MyNumBox).FieldName, ctrName) = CompairStringResult.Equal Then
    '                    ctr = ctrl
    '                End If
    '            End If
    '        Next
    '    Else
    '        For Each ctrl As Control In contrl.Controls
    '            If ctrl.HasChildren = True AndAlso Not (TypeOf ctrl Is common.UserControls.txtFinder OrElse TypeOf ctrl Is common.UserControls.txtNavigator) Then
    '                FindAnyCntrolByFieldName(formname, ctr, ctrName, ctrl)
    '            End If
    '            If TypeOf ctrl Is MyNumBox Then
    '                If clsCommon.CompairString(TryCast(ctr, MyNumBox).FieldName, ctrName) = CompairStringResult.Equal Then
    '                    ctr = ctrl
    '                End If
    '            End If
    '        Next
    '    End If
    'End Sub

    Public Shared Function FindControlAtPoint(container As Control, pos As Point) As Control
        Dim child As Control
        For Each c As Control In container.Controls
            If c.Visible AndAlso c.Bounds.Contains(pos) Then
                child = FindControlAtPoint(c, New Point(pos.X - c.Left, pos.Y - c.Top))
                If child Is Nothing Then
                    Return c
                Else
                    Return child
                End If
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function FindControlAtCursor(form As FrmMainTranScreen) As Control
        Dim pos As Point = Cursor.Position
        If form.Bounds.Contains(pos) Then
            Return FindControlAtPoint(form, form.PointToClient(Cursor.Position))
        End If
        Return Nothing
    End Function
End Class
