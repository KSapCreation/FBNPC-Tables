'=================BM00000007858===============
Imports common
Public Class clsAllStoreProcedure
    Public Shared Sub CreateAllStoreProcedure()
        Try

            clsCommon.ProgressBarShow()
            Dim strProcedureBody As String = "@RegisterID varchar(20),@FirstName varchar(50),@LastName varchar(30),@Address varchar(max),@City varchar(20),@State varchar(20)," & _
" @PostalCode varchar(20),@PhoneNo varchar(15),@EmailID varchar(30),@ClassOption varchar(20),@ExamDate varchar(20),@SpecialRequest varchar(max),@ModifyBy varchar(10),@CreatedBy varchar(10) " & _
            " as      " & _
            " insert into FBNPC_REGISTRATION (RegisterID,FirstName,LastName,Address,City,State,Postal,PhoneNo,EmailID,ClassOption,ExamDate,SpecialRequest,CreatedBy,CreatedDate,modifyBy,modifyDate) " & _
" values(@RegisterID,@firstName,@lastName,@address,@city,@state,@PostalCode,@PhoneNo,@EmailID,@ClassOption,@ExamDate,@SpecialRequest,@createdBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'))"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Registration_Insert", strProcedureBody)


            strProcedureBody = "@CategoryID varchar(20),@CategoryName varchar(20),@isActive varchar(2),@Desc varchar(max),@ModifyBy varchar(20),@CreatedBy varchar(20),@DocTypes varchar(20)" &
" as BEGIN if not exists (select * from FBNPC_Category_Master where CategoryID=@CategoryID) begin insert into FBNPC_Category_Master (CategoryID,Name,Description,Isactive,CreatedBy,CreatedDate,ModifyBy,ModifyDate,Doctype) " &
" values(@CategoryID,@CategoryName,@Desc,@isActive,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@Doctypes) " &
            " End  else  begin update FBNPC_Category_Master set Name=@CategoryName,Description=@Desc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),Doctype=@DocTypes where CategoryID=@CategoryID End  End "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Category_Insert", strProcedureBody)

            strProcedureBody = "@BookID varchar(20),@BookTitle varchar(200),@ShortTitle varchar(798),@Desc varchar(max),@isActive varchar(2),@Price varchar(10),@ModifyBy varchar(10),@CreatedBy varchar(10),@Category varchar(20)" & _
" as BEGIN if not exists (select * from FBNPC_StudyBooks where BookID=@BookID) begin insert into FBNPC_StudyBooks (BookID,BookTitle,SortDesc,Description,Isactive,Price,CreatedBy,CreatedDate,ModifyBy,ModifyDate,Category) " & _
" values(@BookID,@BookTitle,@ShortTitle,@Desc,@isActive,@Price,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@Category) " & _
           " End  else  begin update FBNPC_StudyBooks set BookTitle=@BookTitle,SortDesc=@ShortTitle,Description=@Desc,Price=@Price,isactive=@isActive,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where BookID=@BookID End  End "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_StudyBooks_Insert", strProcedureBody)

            strProcedureBody = "@BatchID varchar(20),@BatchName varchar(20),@BranchName varchar(50),@Caurse varchar(50),@BatchDate varchar(50),@StartTime varchar(50),@EndTime varchar(50),@isActive varchar(2),@ModifyBy varchar(10),@CreatedBy varchar(10)" & _
" as BEGIN if not exists (select * from FBNPC_Batches where BatchID=@BatchID) begin insert into FBNPC_Batches (BatchID,BatchName,BranchName,Course,BatchDate,StartTime,EndTime,Isactive,CreatedBy,CreatedDate,ModifyBy,ModifyDate) " & _
" values(@BatchID,@BatchName,@BranchName,@Caurse,@BatchDate,@StartTime,@EndTime,@isActive,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
           " End  else  begin update FBNPC_Batches set BatchName=@BatchName,Course=@Caurse,BatchDate=@BatchDate,StartTime=@StartTime,EndTime=@EndTime,isActive=@isActive,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where BatchID=@BatchID End  End "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Batches_Insert", strProcedureBody)

            strProcedureBody = " @FirstName varchar(20),@LastName varchar(20),@Gender varchar(20),@DOB varchar(20),@Password varchar(200),@CPwd varchar(200),@EmailID varchar(50),@PhoneNo varchar(20),@CreatedBy varchar(20),@ModifyBy varchar(20),@IPAddress varchar(30),@ID varchar(30),@ExamName varchar(30),@Practice integer,@AdminGroup integer " & _
                "As BEGIN if not exists (select * from FBNPC_USER_MASTER where User_Code=@ID) BEGIN insert into FBNPC_USER_MASTER (User_Code,First_Name,Last_Name,Gender,DOB,Password,ConformPassword,E_mail,Phone,CreatedBy,ModifyBy,CreatedDate,ModifyDate,IP_Address,ExamName,PraticeType,AdminGroup) " & _
              " values(@ID,@FirstName,@LastName,@Gender,@DOB,@Password,@CPwd,@EmailID,@PhoneNo,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@IPAddress,@ExamName,@Practice,@AdminGroup) " & _
              " end else begin update FBNPC_USER_MASTER set First_Name=@FirstName,Last_Name=@LastName,Gender=@Gender,E_mail=@EmailID,Phone=@PhoneNo,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),IP_Address=@IPAddress,ExamName=@ExamName,PraticeType=@Practice,AdminGroup=@AdminGroup where User_Code=@ID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_User_Master_Insert", strProcedureBody)

            strProcedureBody = " @SubjectID varchar(15),@SubjectName varchar(20),@SubjectDesc varchar(500),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
                "As BEGIN if not exists (select * from FBNPC_Subjects where SubjectID=@SubjectID) BEGIN insert into FBNPC_Subjects (SubjectID,SubjectName,Description,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
              " values(@SubjectID,@SubjectName,@SubjectDesc,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
              " end else begin update FBNPC_Subjects set SubjectName=@SubjectName,Description=@SubjectDesc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where SubjectID=@SubjectID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Subjects_Insert", strProcedureBody)

            strProcedureBody = " @SectionID varchar(15),@SectionName varchar(20),@SectionDesc varchar(500),@CreatedBy varchar(20),@ModifyBy varchar(20),@DocType varchar(20),@SectionTime decimal(18,2),@TimeType varchar(20) " &
               "As BEGIN if not exists (select * from FBNPC_Sections where SectionID=@SectionID) BEGIN insert into FBNPC_Sections (SectionID,SectionName,Description,CreatedBy,ModifyBy,CreatedDate,ModifyDate,DocType,SectionTime,TimeType) " &
             " values(@SectionID,@SectionName,@SectionDesc,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@DocType,@SectionTime,@TimeType) " &
             " end else begin update FBNPC_Sections set SectionName=@SectionName,Description=@SectionDesc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),DocType=@DocType,SectionTime=@SectionTime,timetype=@timetype where SectionID=@SectionID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Sections_Insert", strProcedureBody)

            strProcedureBody = " @QuestionID varchar(20),@Question varchar(max),@OptionA varchar(max),@OptionB varchar(max),@OptionC varchar(max),@OptionD varchar(max),@CorrectAns varchar(max),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
             "As BEGIN if not exists (select * from FBNPC_QustionsSheet where QuestionID=@QuestionID) BEGIN insert into FBNPC_QustionsSheet (QuestionID,Question,OptionOne,OptionTwo,OptionThree,OptionFour,CorrectAns,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
           " values(@QuestionID,@Question,@OptionA,@OptionB,@OptionC,@OptionD,@CorrectAns,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
           " end else begin update FBNPC_QustionsSheet set Question=@Question,OptionOne=@OptionA,OptionTwo=@OptionB,OptionThree=@OptionC,OptionFour=@OptionD,CorrectAns=@CorrectAns,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where QuestionID=@QuestionID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Questions_Insert", strProcedureBody)

            strProcedureBody = " @AVID varchar(20),@SubjectName varchar(300),@TransType varchar(20),@FileName varchar(200),@filedata varbinary(max),@FileType varchar(200),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
            "As BEGIN if not exists (select * from FBNPC_Audio_Video_Master where AvID=@AVID) BEGIN insert into FBNPC_Audio_Video_Master (AVID,SubjectName,TransType,FileName,FileData,FileType,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
          " values(@AVID,@SubjectName,@TransType,@FileName,@Filedata,@FileType,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
          " end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Audio_Video_Insert", strProcedureBody)

            strProcedureBody = " @AudioID varchar(15),@SubjectName varchar(300),@TransType varchar(20),@FileName varchar(200),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
                   "As BEGIN if not exists (select * from FBNPC_Video_Master where AudioID=@AudioID) BEGIN insert into FBNPC_Video_Master (AudioID,SubjectName,FileName,TransType,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
                 " values(@AudioID,@SubjectName,@FileName,@TransType,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
                 " end else begin update FBNPC_Video_Master set SubjectName=@SubjectName,FileName=@FileName,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where AudioID=@AudioID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Video_Insert", strProcedureBody)

            strProcedureBody = " @ExamID varchar(15),@ExamName varchar(20),@ExamDesc varchar(500),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
            "As BEGIN if not exists (select * from FBNPC_ExamListName where ExamID=@ExamID) BEGIN insert into FBNPC_ExamListName (ExamID,ExamName,Description,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
          " values(@ExamID,@ExamName,@ExamDesc,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
          " end else begin update FBNPC_ExamListName set ExamName=@ExamName,Description=@ExamDesc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where ExamID=@ExamID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_ExamListName_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@Password varchar(50),@Conform varchar(50) as update FBNPC_User_Master set Password=@Password,ConformPassword=@Conform where User_Code=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Change_Password", strProcedureBody)


            strProcedureBody = " @TearmsID varchar(15),@TearmsCondition varchar(max),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
              "As BEGIN if not exists (select * from FBNPC_TearmsConditions where TearmsID=@TearmsID) BEGIN insert into FBNPC_TearmsConditions (TearmsID,TearmsCondition,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
            " values(@TearmsID,@TearmsCondition,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
            " end else begin update FBNPC_TearmsConditions set TearmsCondition=@TearmsCondition,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where TearmsID=@TearmsID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_TearmsCondition_Insert", strProcedureBody)


            strProcedureBody = " @OptionA integer,@OptionB integer,@OptionC integer,@OptionD integer,@QuestionID varchar(30),@ExamName varchar(30),@StudentName varchar(30),@PaperID varchar(30),@DocType varchar(20) " &
           "As BEGIN insert into FBNPC_Submit_Exam (OptionA,OptionB,OptionC,OptionD,QuestionID,ExamName,StudentName,PaperID,CreatedDate,ModifyDate,DocType) " &
         " values(@OptionA,@OptionB,@OptionC,@OptionD,@QuestionID,@ExamName,@StudentName,@PaperID,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@DocType) " &
         " end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Submit_Exam_Insert", strProcedureBody)

            strProcedureBody = " @OptionA integer,@OptionB integer,@OptionC integer,@OptionD integer,@QuestionID varchar(30),@ExamName varchar(30),@StudentName varchar(30),@PaperID varchar(30),@DocType varchar(20),@QusNo varchar(20) " &
           "As BEGIN if not exists (select * from KSCN_Temp_Table_Exam where QuestionID=@QuestionID and ExamName=@ExamName and StudentName=@StudentName) BEGIN insert into KSCN_Temp_Table_Exam (OptionA,OptionB,OptionC,OptionD,QuestionID,ExamName,StudentName,PaperID,CreatedDate,ModifyDate,DocType,QusNo) " &
         " values(@OptionA,@OptionB,@OptionC,@OptionD,@QuestionID,@ExamName,@StudentName,@PaperID,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@DocType,@QusNo) " &
         " end else begin update KSCN_Temp_Table_Exam set OptionA=@OptionA,OptionB=@OptionB,OptionC=@OptionC,OptionD=@OptionD where QuestionID=@QuestionID and ExamName=@ExamName and StudentName=@StudentName end end "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Temp_Exam_Insert", strProcedureBody)


            strProcedureBody = " @UpdateID varchar(15),@StudentName varchar(20),@Desc varchar(max),@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
           "As BEGIN if not exists (select * from FBNPC_Updates where UpdatesID=@UpdateID) BEGIN insert into FBNPC_Updates (UpdatesID,StudentName,Description,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
         " values(@UpdateID,@StudentName,@Desc,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
         " end else begin update FBNPC_Updates set Description=@Desc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where UpdatesID=@UpdateID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Updates_Insert", strProcedureBody)


            strProcedureBody = " @ExamName varchar(30),@StudentName varchar(30),@PaperID varchar(30) " & _
           "As BEGIN insert into FBNPC_Exam_Question_Validtion (ExamName,StudentName,PaperID) " & _
         " values(@ExamName,@StudentName,@PaperID) " & _
         " end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Validation_Insert", strProcedureBody)


            strProcedureBody = " @ReadingID varchar(15),@ComprehensionName varchar(20),@ComprehensionDesc text,@CreatedBy varchar(20),@ModifyBy varchar(20) " & _
              "As BEGIN if not exists (select * from FBNPC_Comprehension_Master where ReadingID=@ReadingID) BEGIN insert into FBNPC_Comprehension_Master (ReadingID,ComprehensionName,ComprehensionDesc,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " & _
            " values(@ReadingID,@ComprehensionName,@ComprehensionDesc,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " & _
            " end else begin update FBNPC_Comprehension_Master set ComprehensionName=@ComprehensionName,ComprehensionDesc=@ComprehensionDesc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where ReadingID=@ReadingID end end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Comprehension_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as delete from FBNPC_Category_Master where CategoryID=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Category_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@EmailID varchar(50),@PhoneNo varchar(10),@ModifyBy varchar(10)" & _
" as BEGIN update FBNPC_USER_MASTER set E_mail=@EmailID,Phone=@PhoneNo,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where User_Code=@ID  End "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_User_Update", strProcedureBody)


            strProcedureBody = "@ID varchar(10) as select case when classoption=1 then 'NCLEX' when classoption=2 then 'CPNRE' when classoption=3 then 'IELTS' when classoption=4 then 'CELBAN' when classoption=5 then 'OTHERS' end as ClassType,* from FBNPC_REGISTRATION  order by RegisterID desc "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Register_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as select * from FBNPC_Category_Master where CategoryID=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Category_Edit", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Category_Master where DocType=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Category_BindFORStudy", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_StudyBooks where isactive=1 "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_StudyBooks_Web", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_StudyBooks"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_StudyBook", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select convert(varchar(20),batchDate,103) as BDate,case when isactive=1 then 'Visible' else 'Not Visible' end as Active,* from FBNPC_Batches"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Batches", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select convert(varchar(20),batchDate,103) as BDate,case when isactive=1 then 'Visible' else 'Not Visible' end as Active,* from FBNPC_Batches where isactive=1"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Batches_Web", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Batches where BatchID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_BindEdit_Batches", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_StudyBooks where BookID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_StudyBook_Edit", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as Delete from FBNPC_StudyBooks where BookID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_StudyBook_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as Delete from FBNPC_Batches where BatchID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Batches_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select fbnpc_user_master.*,FBNPC_EXAMLISTNAME.ExamName as Exam from fbnpc_user_master left outer join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=fbnpc_user_master.ExamName "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_User_Master_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as Delete from fbnpc_user_master where User_Code=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_User_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select fbnpc_user_master.*,FBNPC_EXAMLISTNAME.ExamName as Exam from fbnpc_user_master left outer join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=fbnpc_user_master.ExamName where User_Code=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_UserMaster_Edit", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@Email varchar(50) as select E_mail,User_Code from fbnpc_user_master where User_Code=@ID and E_mail=@Email"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_ForgetPwd_OTP", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Subjects"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Subjects", strProcedureBody)

            strProcedureBody = "@SubjectID varchar(30) as select * from FBNPC_Subjects where SubjectID=@SubjectID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_Subjects", strProcedureBody)

            strProcedureBody = "@SubjectID varchar(30) as DELETE from FBNPC_Subjects where SubjectID=@SubjectID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_Subjects", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Sections"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Sections", strProcedureBody)

            strProcedureBody = "@SectionID varchar(30) as select * from FBNPC_Sections where SectionID=@SectionID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_Sections", strProcedureBody)

            strProcedureBody = "@SectionID varchar(30) as DELETE from FBNPC_Sections where SectionID=@SectionID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_Sections", strProcedureBody)

            strProcedureBody = "@QuestionID varchar(30) as DELETE from FBNPC_QustionsSheet where QuestionID=@QuestionID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_QuestionSheet", strProcedureBody)

            strProcedureBody = "@QuestionID varchar(30) as select * from FBNPC_QustionsSheet where QuestionID=@QuestionID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_Questions", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_QustionsSheet"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_QuestionSheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select SubjectName,FileName,AVID,TransType,ModifyBy,ModifyDate from FBNPC_Audio_video_master"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Audio_Video_Master", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as Delete from FBNPC_Audio_video_master where AVID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_Audio_Video_Master", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as Select SubjectName,AVID,TransType,FileData from FBNPC_Audio_video_master where AVID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_Audio_Video_Master", strProcedureBody)

            strProcedureBody = " @ID varchar(30),@FromDate varchar(10),@ToDate varchar(10),@DocType varchar(10) as 
                        if (@DocType='not')
                            begin
                    select case when classoption=1 then 'NCLEX' when classoption=2 then 'CPNRE' when classoption=3 then 'IELTS' when classoption=4 then 'CELBAN' when classoption=5 
                    then 'OTHERS' end as ClassType,* from FBNPC_Registration 
                    where FirstName like '%' + @ID + '%' 
                            end
                        else
                            begin
                        select case when classoption=1 then 'NCLEX' when classoption=2 then 'CPNRE' when classoption=3 then 'IELTS' when classoption=4 then 'CELBAN' when classoption=5 
                then 'OTHERS' end as ClassType,* from FBNPC_Registration 
                where FirstName like '%' + @ID + '%' 
            and (CreatedDate>=@FromDate ) and (CreatedDate<=@ToDate ) 
                            end"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_SearchList", strProcedureBody)

            strProcedureBody = "@ExamID varchar(30) as select * from FBNPC_ExamListName"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_ExamListName", strProcedureBody)
               
            strProcedureBody = "@ExamID varchar(30) as select * from FBNPC_ExamListName where ExamID=@ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_ExamListName", strProcedureBody)

            strProcedureBody = "@ExamID varchar(30) as DELETE from FBNPC_ExamListName where ExamID=@ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_ExamListName", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_TearmsConditions "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Select_TearmsConditions", strProcedureBody)


            strProcedureBody = "@EmailID varchar(30),@Password varchar(30) as select * from FBNPC_USER_MASTER where E_mail=@EmailID and Password=@Password and isnull(ExamName,'')<>'Select' "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_LoginUser", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select ROW_NUMBER() OVER (ORDER BY (FBNPC_PAPER_SET_DETAIL.QuestionID)) as num,* from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID inner join FBNPC_USer_MAster on FBNPC_USer_MAster.ExamName=FBNPC_PAPER_SET_DETAIL.ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_QuestionSheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select distinct FBNPC_PAPER_SET_DETAIL.AVID from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID  where isnull(AVID,'')=@id and isnull(AVID,'')<>'' "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Audio_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select distinct FBNPC_PAPER_SET_DETAIL.VideoID,FBNPC_VIDEO_MASTER.FileName from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID  inner join FBNPC_VIDEO_MASTER on FBNPC_VIDEO_MASTER.AudioID=FBNPC_PAPER_SET_DETAIL.VideoID where isnull(VideoID,'')=@id "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Video_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select distinct FBNPC_PAPER_SET_DETAIL.ComprehensionID,FBNPC_COMPREHENSION_MASTER.ComprehensionDesc from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID inner join FBNPC_COMPREHENSION_MASTER on FBNPC_COMPREHENSION_MASTER.ReadingID=FBNPC_PAPER_SET_DETAIL.ComprehensionID where isnull(ComprehensionID,'')=@id "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Comprehension_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20) as select ROW_NUMBER() OVER (ORDER BY (AVID)) as num,* from (select distinct FBNPC_PAPER_SET_DETAIL.AVID from FBNPC_PAPER_SET_DETAIL"
            strProcedureBody += " where FBNPC_PAPER_SET_DETAIL.ExamID=@ID and isnull(AVID,'')<>'' and PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName))xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Serial_Wise_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20) as select ROW_NUMBER() OVER (ORDER BY (ComprehensionID)) as num,* from (select distinct FBNPC_PAPER_SET_DETAIL.ComprehensionID from FBNPC_PAPER_SET_DETAIL"
            strProcedureBody += " where FBNPC_PAPER_SET_DETAIL.ExamID=@ID and isnull(ComprehensionID,'')<>'' and PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName))xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Reading_Serial_Wise_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@ExamID varchar(20) as select distinct FBNPC_QustionsSheet.*,'' num,FBNPC_PAPER_SET_DETAIL.avid,FBNPC_PAPER_SET_DETAIL.paperid from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID where AVID=@ID  and FBNPC_PAPER_SET_DETAIL.examid=@ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Listening_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@ExamID varchar(20) as select distinct FBNPC_QustionsSheet.*,ROW_NUMBER() OVER (ORDER BY (FBNPC_QustionsSheet.questionid)) as  num,FBNPC_PAPER_SET_DETAIL.avid,FBNPC_PAPER_SET_DETAIL.paperid from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID where FBNPC_QustionsSheet.questionid=@ID and FBNPC_PAPER_SET_DETAIL.examid=@ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Individual_Sheet_Question", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20) as select distinct FBNPC_QustionsSheet.*,ROW_NUMBER() OVER (ORDER BY (FBNPC_QustionsSheet.questionid)) as  num,FBNPC_PAPER_SET_DETAIL.avid,FBNPC_PAPER_SET_DETAIL.paperid from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID 
where FBNPC_PAPER_SET_DETAIL.examid=@ID and FBNPC_PAPER_SET_DETAIL.PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName) "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Individual_Sheet", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@ExamID varchar(20) as select distinct FBNPC_QustionsSheet.*,'' as num,FBNPC_PAPER_SET_DETAIL.paperid,FBNPC_PAPER_SET_DETAIL.videoID from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID where VideoID=@ID  and FBNPC_PAPER_SET_DETAIL.examid=@ExamID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Speaking_Sheet_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@ExamID varchar(20) as select distinct FBNPC_QustionsSheet.*,'' num,FBNPC_PAPER_SET_DETAIL.ComprehensionID,FBNPC_PAPER_SET_DETAIL.paperid from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID where ComprehensionID=@ID  and FBNPC_PAPER_SET_DETAIL.examid=@ExamID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Reading_Sheet_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Video_Master"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Bind_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Video_Master where AudioID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Edit_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as delete from FBNPC_Video_Master where AudioID=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Delete_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20) as select ROW_NUMBER() OVER (ORDER BY (VideoID)) as num,* from (select distinct FBNPC_PAPER_SET_DETAIL.VideoID from FBNPC_PAPER_SET_DETAIL"
            strProcedureBody += " where FBNPC_PAPER_SET_DETAIL.ExamID=@ID and isnull(videoid,'')<>'' and PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName))xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Serial_Wise_Sheet_Video", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Comprehension_Master "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Comprehension_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_Comprehension_Master where ReadingID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Comprehension_Edit", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as delete from FBNPC_Comprehension_Master where ReadingID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Comprehension_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += " ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  from FBNPC_SUBMIT_EXAM left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " where studentname = @ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Results", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select count(*) as cnt from FBNPC_EXAM_STUDENT_MAPPING_HEAD "
            strProcedureBody += " inner join FBNPC_EXAM_STUDENT_MAPPING_DETAIL on FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ESM_Code=FBNPC_EXAM_STUDENT_MAPPING_HEAD.ESM_Code"
            strProcedureBody += " where FBNPC_EXAM_STUDENT_MAPPING_HEAD.StudentName=@ID "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Student_Mapping_Count", strProcedureBody)


            strProcedureBody = "@ID varchar(20) as select distinct ExamCode,FBNPC_EXAMLISTNAME.ExamName,FBNPC_EXAMLISTNAME.Description as [Exam Description],FBNPC_USER_MASTER.First_NAME,fbnpc_sections.doctype  from FBNPC_EXAM_STUDENT_MAPPING_HEAD"
            strProcedureBody += " inner join  FBNPC_EXAM_STUDENT_MAPPING_DETAIL on FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ESM_Code=FBNPC_EXAM_STUDENT_MAPPING_head.ESM_Code"
            strProcedureBody += " inner join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ExamCode"
            strProcedureBody += "   inner join FBNPC_USER_MASTER on FBNPC_USER_MASTER.USER_CODE=FBNPC_EXAM_STUDENT_MAPPING_head.studentname"
            strProcedureBody += "   inner join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.examid=FBNPC_EXAMLISTNAME.ExamID "
            strProcedureBody += "  inner join fbnpc_sections on fbnpc_sections.sectionID=FBNPC_PAPER_SET_HEAD.section "
            strProcedureBody += "  where FBNPC_EXAM_STUDENT_MAPPING_HEAD.StudentName=@ID and fbnpc_sections.doctype ='Multiple' "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Student_Mapping_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select distinct ExamCode,FBNPC_EXAMLISTNAME.ExamName,FBNPC_EXAMLISTNAME.Description as [Exam Description],FBNPC_USER_MASTER.First_NAME,fbnpc_sections.doctype  from FBNPC_EXAM_STUDENT_MAPPING_HEAD"
            strProcedureBody += " inner join  FBNPC_EXAM_STUDENT_MAPPING_DETAIL on FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ESM_Code=FBNPC_EXAM_STUDENT_MAPPING_head.ESM_Code"
            strProcedureBody += " inner join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ExamCode"
            strProcedureBody += "   inner join FBNPC_USER_MASTER on FBNPC_USER_MASTER.USER_CODE=FBNPC_EXAM_STUDENT_MAPPING_head.studentname"
            strProcedureBody += "   inner join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.examid=FBNPC_EXAMLISTNAME.ExamID "
            strProcedureBody += "  inner join fbnpc_sections on fbnpc_sections.sectionID=FBNPC_PAPER_SET_HEAD.section "
            strProcedureBody += "  where FBNPC_EXAM_STUDENT_MAPPING_HEAD.StudentName=@ID and fbnpc_sections.doctype ='Individual' "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Student_Mapping_Select_Individual", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@DocType varchar(20) as select distinct ExamCode,FBNPC_EXAMLISTNAME.ExamName,FBNPC_EXAMLISTNAME.Description as [Exam Description],FBNPC_USER_MASTER.First_NAME,fbnpc_sections.doctype  from FBNPC_EXAM_STUDENT_MAPPING_HEAD"
            strProcedureBody += " inner join  FBNPC_EXAM_STUDENT_MAPPING_DETAIL on FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ESM_Code=FBNPC_EXAM_STUDENT_MAPPING_head.ESM_Code"
            strProcedureBody += " inner join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ExamCode"
            strProcedureBody += "   inner join FBNPC_USER_MASTER on FBNPC_USER_MASTER.USER_CODE=FBNPC_EXAM_STUDENT_MAPPING_head.studentname"
            strProcedureBody += "   inner join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.examid=FBNPC_EXAMLISTNAME.ExamID "
            strProcedureBody += "  inner join fbnpc_sections on fbnpc_sections.sectionID=FBNPC_PAPER_SET_HEAD.section "
            strProcedureBody += "  where FBNPC_EXAM_STUDENT_MAPPING_HEAD.StudentName=@ID and fbnpc_sections.doctype =@DocType "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Bind_For_Result", strProcedureBody)

            'strProcedureBody = "@ID varchar(20) as select ROW_NUMBER() OVER(ORDER BY FBNPC_EXAM_STUDENT_MAPPING_HEAD.ESM_Code ASC) AS num,FBNPC_EXAM_STUDENT_MAPPING_HEAD.ESM_Code,StudentName,ExamCode,FBNPC_EXAMLISTNAME.ExamName,FBNPC_EXAMLISTNAME.Description as [Exam Description],FBNPC_USER_MASTER.First_NAME from FBNPC_EXAM_STUDENT_MAPPING_HEAD"
            'strProcedureBody += " inner join  FBNPC_EXAM_STUDENT_MAPPING_DETAIL on FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ESM_Code=FBNPC_EXAM_STUDENT_MAPPING_head.ESM_Code"
            'strProcedureBody += " inner join FBNPC_EXAMLISTNAME on FBNPC_EXAMLISTNAME.ExamID=FBNPC_EXAM_STUDENT_MAPPING_DETAIL.ExamCode"
            'strProcedureBody += "   inner join FBNPC_USER_MASTER on FBNPC_USER_MASTER.USER_CODE=FBNPC_EXAM_STUDENT_MAPPING_head.studentname"
            'strProcedureBody += "  where FBNPC_EXAM_STUDENT_MAPPING_HEAD.StudentName=@ID "
            'clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Student_Mapping_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select * from FBNPC_QUSTIONSSHEET where Question like '%' + @ID + '%'"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Search_Questions", strProcedureBody)

            strProcedureBody = "@UserName varchar(20),@Pwd varchar(20) as select * from FBNPC_USER_MASTER where First_Name=@UserName and Password=@Pwd and AdminGroup=1"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Login", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select Password from FBNPC_USER_MASTER where First_Name=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_LoginDetail_Find", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20),@ExamName varchar(20),@DocType varchar(20) as select FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += " ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  from FBNPC_SUBMIT_EXAM left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " where studentname = @StudentName and (FBNPC_QUSTIONSSHEET.Question like '%' + @ID + '%' or isnull(FBNPC_QUSTIONSSHEET.Question,'')='') and (FBNPC_SUBMIT_EXAM.ExamName=@ExamName or isnull(FBNPC_SUBMIT_EXAM.ExamName,'')='')"
            strProcedureBody += " and (FBNPC_SUBMIT_EXAM.DocType=@DocType  or isnull(FBNPC_SUBMIT_EXAM.DocType,'')='')"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Student_Search_Questions", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select  'Right' as SectionName,sum(finalresult.FinalAns) as finalans from (select *,case when StudentANS=[Correct ANS] then 1 else 0 end FinalAns,case when StudentANS<>[Correct ANS] then 1 else 0 end WrongAns from (select  FBNPC_SUBMIT_EXAM.studentname,FBNPC_PAPER_SET_HEAD.Section,FBNPC_SECTIONS.SectionName,FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += "  ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  "
            strProcedureBody += " from FBNPC_SUBMIT_EXAM"
            strProcedureBody += " left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " 			left outer join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.PaperID=FBNPC_SUBMIT_EXAM.PaperID"
            strProcedureBody += " left outer join FBNPC_SECTIONS on FBNPC_SECTIONS.SectionID=FBNPC_PAPER_SET_HEAD.Section"
            strProcedureBody += " 			)xx"
            strProcedureBody += " where xx.studentname = @ID and xx.sectionName='listening' ) finalresult"
            strProcedureBody += " group by finalresult.studentname,finalresult.SectionName"
            strProcedureBody += " union all"
            strProcedureBody += " select 'Wrong' as SectionName,sum(finalresult.WrongAns) as WrongAns  from (select *,case when StudentANS=[Correct ANS] then 1 else 0 end FinalAns,case when StudentANS<>[Correct ANS] then 1 else 0 end WrongAns from (select  FBNPC_SUBMIT_EXAM.studentname,FBNPC_PAPER_SET_HEAD.Section,FBNPC_SECTIONS.SectionName,FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += " ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  "
            strProcedureBody += " from FBNPC_SUBMIT_EXAM"
            strProcedureBody += " left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " left outer join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.PaperID=FBNPC_SUBMIT_EXAM.PaperID"
            strProcedureBody += " left outer join FBNPC_SECTIONS on FBNPC_SECTIONS.SectionID=FBNPC_PAPER_SET_HEAD.Section"
            strProcedureBody += " 			)xx"
            strProcedureBody += " where xx.studentname = @ID and xx.sectionName='listening') finalresult"
            strProcedureBody += " group by finalresult.studentname,finalresult.SectionName"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Student_DashboardScore_Board", strProcedureBody)


            strProcedureBody = "@ID varchar(20) as select max(FBNPC_EXAMLISTNAME.ExamName) as ExamName,count(FBNPC_PAPER_SET_DETAIL.QusSelect) as QusSelect from FBNPC_EXAMLISTNAME "
            strProcedureBody += " left outer join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.ExamID=FBNPC_EXAMLISTNAME.ExamID"
            strProcedureBody += " left outer join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.PaperID=FBNPC_PAPER_SET_HEAD.PaperID"
            strProcedureBody += " group by FBNPC_EXAMLISTNAME.ExamID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exams_Count_Question_Dashboard", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select  top 5* from fbnpc_user_master order by createddate desc "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_User_list_Dashboard", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select 'With Exam' as Category,   sum(counted) as counted from (select distinct firstName,FBNPC_USER_MASTER.user_code,case when FBNPC_USER_MASTER.user_code=FBNPC_REGISTRATION.firstName then 1 else 0 end counted from FBNPC_REGISTRATION"
            strProcedureBody += " left outer join FBNPC_USER_MASTER on FBNPC_USER_MASTER.User_Code=FBNPC_REGISTRATION.FirstName)xx"
            strProcedureBody += " union all"
            strProcedureBody += " select 'Without Exam' as Category,sum(counted) as counted from (select distinct firstName,FBNPC_USER_MASTER.user_code,case when isnull(FBNPC_USER_MASTER.user_code,'')<>FBNPC_REGISTRATION.firstName then 1 else 0 end counted from FBNPC_REGISTRATION"
            strProcedureBody += " left outer join FBNPC_USER_MASTER on FBNPC_USER_MASTER.User_Code=FBNPC_REGISTRATION.FirstName)xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Registrations_Exam_Dashboard", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select  top 5* from FBNPC_REGISTRATION order by createddate desc "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Registration_list_Dashboard", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select 'Audio' as Screen,count(*) as cnt from (select distinct AVID from FBNPC_PAPER_SET_DETAIL"
            strProcedureBody += " where isnull(AVID,'')<>'')xx"
            strProcedureBody += " union all"
            strProcedureBody += " select 'Video' as Screen,count(*) as cnt from (select distinct videoid from FBNPC_PAPER_SET_DETAIL"
            strProcedureBody += " where isnull(videoid,'')<>'')xx"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_No_Of_Audio_Video_Dashboard", strProcedureBody)

            strProcedureBody = " @IPAddress varchar(30),@MachineName varchar(50),@ID varchar(30),@LoginDate varchar(30) " & _
        "As BEGIN update FBNPC_USER_MASTER set IP_Address=@IPAddress,MachineName=@MachineName,LoginDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where User_Code=@ID end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Login_Current_History", strProcedureBody)

            strProcedureBody = " @IPAddress varchar(30),@MachineName varchar(50),@ID varchar(30),@LoginDate varchar(30),@MacAddress varchar(50),@UserName varchar(50) " & _
      "As BEGIN insert into FBNPC_Login_History(IP_Address,MachineName,LoginDate,UserCode,MacAddress,UserName) values(@IPAddress,@MachineName,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ID,@MacAddress,@UserName) end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Maintain_Login_History", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(50) as select 'Total Question' as [Category],count(*) as cnt from (select distinct FBNPC_PAPER_SET_DETAIL.* from FBNPC_PAPER_SET_HEAD"
            strProcedureBody += " left outer join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL .PaperID=FBNPC_PAPER_SET_HEAD.PaperID"
            strProcedureBody += " where  FBNPC_PAPER_SET_DETAIL.examid=@ExamName)xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Re_loader_CountExam_Student_Total", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(50) as select 'Attempted Question' as [Category],count(*) as cnt from (select distinct FBNPC_SUBMIT_EXAM.* from FBNPC_PAPER_SET_HEAD"
            strProcedureBody += " left outer join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL .PaperID=FBNPC_PAPER_SET_HEAD.PaperID"
            strProcedureBody += " left outer join FBNPC_SUBMIT_EXAM on FBNPC_SUBMIT_EXAM .ExamName=FBNPC_PAPER_SET_DETAIL.ExamID"
            strProcedureBody += " where FBNPC_SUBMIT_EXAM.studentname=@ID and FBNPC_SUBMIT_EXAM.examname=@ExamName)xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Re_loader_CountExam_Student_Attempted", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(50) as select questionid,optionA,optionB,OptionC,optionD,examName,studentName,paperid from FBNPC_SUBMIT_EXAM "
            strProcedureBody += " where FBNPC_SUBMIT_EXAM.studentname=@ID and FBNPC_SUBMIT_EXAM.examname=@ExamName "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Re_loader_Submit_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(50) as select StudentNAme,Examname,paperid from FBNPC_Exam_Question_Validtion  "
            strProcedureBody += " where studentname=@ID and examname=@ExamName "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Re_loader_Validation_Select", strProcedureBody)

            strProcedureBody = "@hist_By varchar(50),@Hist_Version varchar(50), @OptionA integer,@OptionB integer,@OptionC integer,@OptionD integer,@QuestionID varchar(30),@ExamName varchar(30),@StudentName varchar(30),@PaperID varchar(30) " & _
        "As BEGIN insert into FBNPC_Submit_Exam_History (Hist_By,Hist_Version,Hist_Date,OptionA,OptionB,OptionC,OptionD,QuestionID,ExamName,StudentName,PaperID) " & _
      " values(@hist_By,@Hist_Version,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@OptionA,@OptionB,@OptionC,@OptionD,@QuestionID,@ExamName,@StudentName,@PaperID) " & _
      " end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Submit_Exam_History_Insert", strProcedureBody)


            strProcedureBody = " @hist_By varchar(50),@Hist_Version varchar(50),@ExamName varchar(30),@StudentName varchar(30),@PaperID varchar(30) " & _
           "As BEGIN insert into FBNPC_Exam_Question_Validtion_History (Hist_By,Hist_Version,Hist_Date,ExamName,StudentName,PaperID) " & _
         " values(@hist_By,@Hist_Version,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ExamName,@StudentName,@PaperID) " & _
         " end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Validation_History_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(30) as delete from FBNPC_Exam_Question_Validtion where studentname=@id and examname=@ExamName "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Exam_Validation_History_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(20),@ExamName varchar(30) as delete from FBNPC_Submit_Exam where studentname=@id and examname=@ExamName "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Submit_Exam_History_Delete", strProcedureBody)

            strProcedureBody = "@ExamID varchar(30) as select  distinct * from (select 'Student Mapping' as typed,examcode from FBNPC_EXAM_STUDENT_MAPPING_DETAIL where examcode=@ExamID"
            strProcedureBody += " union all select 'Question Validation' as typed,examname from FBNPC_Exam_Question_Validtion where examname=@ExamID "
            strProcedureBody += " union all select 'Submit Exam' as typed,examname from FBNPC_Submit_Exam where examname=@ExamID"
            strProcedureBody += " union all select 'Paper Set' as typed,examid from FBNPC_Paper_Set_Head where examid=@ExamID)xx "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Student_Mapping_Select", strProcedureBody)


            strProcedureBody = "@ID varchar(10) as delete from FBNPC_Updates where UpdatesID=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Updates_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as select * from FBNPC_Updates where UpdatesID=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Updates_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as select * from FBNPC_Updates "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Updates_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as select * from FBNPC_Updates where StudentName='All' union all select * from FBNPC_Updates where StudentName=@ID"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Students_Updates_Bind", strProcedureBody)

            strProcedureBody = "  (@CompCode varchar(8),@CompName varchar(100),@Add1 varchar(50),@Add2 varchar(50),@Add3 varchar(50),@CityCode varchar(50),@Phone1 varchar(50),@Phone2 varchar(50),@Fax varchar(12),@Email varchar(50),@PinCode varchar(20),@State varchar(30),@TinNo varchar(20),@CstLst varchar(30),@RegdNo varchar(30),@CForm char(1),@ModeofTransport varchar(30),@CreatedBy varchar(12),@Createddate varchar(10),@ModifiedBy varchar(12),@ModifiedDate varchar(10),@CompCode1 varchar(8),@DBName varchar(100),@VatRegNo varchar(30)=null,@PanNo varchar(30)=null,@ServiceTaxReg varchar(30)=null,@TanNo varchar(30)=null,@AccessOfficer varchar(30)=null,@TCanNo varchar(30)=null,@CERange varchar(30)=null,@CircleNo varchar(30)=null,@CECommissionerate decimal(18,2)=null,@WardNo varchar(30)=null,@CEDivision varchar(30)=null,@EccNo varchar(30)=null) as begin insert into COMPANY_MASTER  (Comp_Code,Comp_Name,Add1,Add2,Add3,City_Code,Phone1,Phone2,Fax,Email,Pincode,State,Tin_No,CST_LST,Regn_No,Cform,Mode_of_Trans,Created_By,Created_Date,Modify_By,Modify_Date,Comp_Code1,DataBase_Name,Vat_Reg_No,ServiceTax_Reg_No,Ecc_No,CE_Range,CE_Commissionerate,CE_Division,Pan_No,Tan_No,Tcan_No,Circle_No,Ward_No,Access_Officer)   values(@CompCode,@CompName,@Add1,@Add2,@Add3,@CityCode,@Phone1,@Phone2,@Fax,@Email,@PinCode,@State,@TinNo,@CstLst,@RegdNo,@CForm,@ModeofTransport,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@CompCode1,@DBName,@VatRegNo,@ServiceTaxReg,@EccNo,@CERange,@CECommissionerate,@CEDivision,@PanNo,@TanNo,@TCanNo,@CircleNo,@WardNo,@AccessOfficer) end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_COMPANY_MASTER_insert", strProcedureBody)

            strProcedureBody = " @Licence_ExpiredDate_Specification_B varchar(500),@Licence_ExpiredDate_Description_A varchar(500),@CompanyName varchar(50)" & _
           "As BEGIN insert into Licence_Master (Licence_ExpiredDate_Specification_B,Licence_ExpiredDate_Description_A,CompanyName) " & _
         " values(@Licence_ExpiredDate_Specification_B,@Licence_ExpiredDate_Description_A,@CompanyName) " & _
         " end "
            clsCommonFunctionality.CreateStoreProcedure("Licence_Master_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(10) as select * from Licence_Master  "
            clsCommonFunctionality.CreateStoreProcedure("Licence_Master_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(12) as delete from FBNPC_Registration where RegisterID=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("Registration_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(12) as select * from FBNPC_Submit_Exam where StudentName=@ID  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_CheckUser_for_furtherUseOrNot", strProcedureBody)

            strProcedureBody = "@ID varchar(12),@ExamID varchar(20) as select dateadd(HOUR, final.sectiontime, final.CurrentDate) as time_added,* from (select convert(datetime,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) as CurrentDate,FBNPC_Paper_Set_Head.section,fbnpc_sections.sectionname,FBNPC_Paper_Set_Head.examid,convert(int,fbnpc_sections.sectiontime) as sectiontime,fbnpc_sections.timetype from FBNPC_Exam_Student_Mapping_Detail
left join FBNPC_Paper_Set_Head on FBNPC_Paper_Set_Head.examid=FBNPC_Exam_Student_Mapping_Detail.Examcode
left join fbnpc_sections on fbnpc_sections.sectionid=FBNPC_Paper_Set_Head.section
left join FBNPC_Exam_Student_Mapping_Head on FBNPC_Exam_Student_Mapping_Head.esm_code=FBNPC_Exam_Student_Mapping_detail.esm_code
where FBNPC_Exam_Student_Mapping_Head.studentname=@ID and FBNPC_Paper_Set_Head.examid=@ExamID )final  "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_GetSectionWiseTime", strProcedureBody)

            strProcedureBody = "@ID varchar(12) as select convert(datetime,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) as CurrentDate  "
            clsCommonFunctionality.CreateStoreProcedure("CurrentDateTime", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select GalleryID,filedata,filetype,filename,description from fbnpc_gallery_master where galleryType=1 and isactive=1 order by GalleryID desc"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Picture_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select GalleryID,youtubelink,description  from fbnpc_gallery_master where galleryType=2 and isactive=1 order by GalleryID desc"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Video_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(30) as select case when isactive=1 then 'Visible' else 'Not Visible' end as Active,* from FBNPC_Category_Master "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Category_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select *,KSCN_City_master.Name as CityName,KSCN_COuntry_master.Name as CountryName from KSCN_Achiever_master
left join KSCN_COuntry_master on KSCN_COuntry_master.CountryID=KSCN_Achiever_master.Country
left join KSCN_City_master on KSCN_City_master.CityID=KSCN_Achiever_master.City "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Achiever_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select *,KSCN_City_master.Name as CityName,KSCN_COuntry_master.Name as CountryName from KSCN_Achiever_master
left join KSCN_COuntry_master on KSCN_COuntry_master.CountryID=KSCN_Achiever_master.Country
left join KSCN_City_master on KSCN_City_master.CityID=KSCN_Achiever_master.City where 2=2 and DocType=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Achiever_DocType", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_Achiever_Master where AchieverId=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Achiever_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as delete from KSCN_Achiever_Master where AchieverId=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Achiever_Delete", strProcedureBody)

            strProcedureBody = " @AchieverID varchar(15),@FirstName varchar(30),@LastName varchar(30),@City varchar(20),@Country varchar(20),@OnLandingPage integer,@Desc text,@ISActive varchar(10),@FileName varchar(50),@FileType varchar(50),@FileData varbinary(max),@CreatedBy varchar(30),@ModifyBy varchar(30) " &
              "As BEGIN if not exists (select * from KSCN_Achiever_Master where AchieverID=@AchieverID) BEGIN insert into KSCN_Achiever_Master (AchieverID,FirstName,LastName,City,Country,OnLandingPage,Description,Isactive,FileName,FileData,FileType,CreatedBy,ModifyBy,CreatedDate,ModifyDate) " &
            " values(@AchieverID,@FirstName,@Lastname,@City,@Country,@OnLandingPage,@Desc,@ISActive,@FileName,@FileData,@FileType,@CreatedBy,@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00')) " &
            " end else begin update KSCN_Achiever_Master set FirstName=@FirstName,Lastname=@LastName,Description=@Desc,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where AchieverID=@AchieverID end end "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Achiever_Insert", strProcedureBody)

            strProcedureBody = "@Title varchar(20),@Desc text,@ISActive varchar(10),@PicType varchar(500),@PicData varbinary(max),@PicName varchar(500),@CreatedBy varchar(20),@ModifyBy varchar(20),@DocType varchar(10),@ID varchar(10),@ImageID varchar(5),@Country varchar(20),@State varchar(20),@City varchar(20) " &
                "As BEGIN if not exists (select * from FBNPC_Programs_Insert where ProgramsID=@ID) 
   begin insert into FBNPC_Programs_Insert (titleName,TitleDescription,IsActive,filetype,filedata,filename,CreatedBy,CreatedDate,ModifyBy,ModifyDate,Type,Country,State,City)
values(@Title,@Desc,@ISActive,@pictype,@picdata,@picname,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@DocType,@Country,@State,@City)
end
else
if (@imageID='1')
begin
update FBNPC_Programs_Insert set TitleName=@Title,TitleDescription=@Desc,IsActive=@ISActive,filename=@picname,filetype=@pictype,filedata=@picdata,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),Country=@Country,State=@State,City=@City where ProgramsID=@ID
end
else
begin
update FBNPC_Programs_Insert set TitleName=@Title,TitleDescription=@Desc,IsActive=@ISActive,ModifyBy=@ModifyBy,ModifyDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),Country=@Country,State=@State,City=@City where ProgramsID=@ID
end END "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Our_Programs_Insert", strProcedureBody)


            strProcedureBody = "@CountryID varchar(20),@Name varchar(200) " &
" as BEGIN if not exists (select * from KSCN_Country_Master where CountryID=@CountryID) begin insert into KSCN_Country_Master (CountryID,Name) " &
" values(@CountryID,@Name) " &
           " End  else  begin update KSCN_Country_Master set Name=@Name where CountryID=@CountryID End  End "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Country_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as  select * from kscn_country_master "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Country_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from kscn_country_master where CountryID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Country_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as delete from KSCN_City_Master where CountryID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Country_Delete", strProcedureBody)

            strProcedureBody = "@CountryID varchar(20),@Name varchar(200),@StateID varchar(20) " &
" as BEGIN if not exists (select * from KSCN_State_Master where StateID=@StateID) begin insert into KSCN_State_Master (StateID,CountryID,Name) " &
" values(@StateID,@CountryID,@Name) " &
           " End  else  begin update KSCN_State_Master set Name=@Name,CountryID=@CountryID where StateID=@StateID End  End "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_State_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_State_Master "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_State_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_State_Master where StateID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_State_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as delete from KSCN_State_Master where StateID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_State_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_State_Master where CountryID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_State_Country_Wise", strProcedureBody)

            strProcedureBody = "@CityID varchar(20),@CountryID varchar(20),@Name varchar(200),@StateID varchar(20) " &
" as BEGIN if not exists (select * from KSCN_City_Master where CityID=@CityID) begin insert into KSCN_City_Master (CityID,StateID,CountryID,Name) " &
" values(@CityID,@StateID,@CountryID,@Name) " &
           " End  else  begin update KSCN_City_Master set Name=@Name,CountryID=@CountryID,StateID=@StateID where CityID=@CityID End  End "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_City_Insert", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_City_Master "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_City_Bind", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_City_Master where CityID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_City_Select", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as delete from KSCN_City_Master where CityID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_City_Delete", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select * from KSCN_City_Master where StateID=@ID "
            clsCommonFunctionality.CreateStoreProcedure("KSCN_City_State_Wise", strProcedureBody)

            strProcedureBody = "@ID varchar(20) as select  'Right' as SectionName,sum(finalresult.FinalAns) as finalans from (select *,case when StudentANS=[Correct ANS] then 1 else 0 end FinalAns,case when StudentANS<>[Correct ANS] then 1 else 0 end WrongAns from (select  FBNPC_SUBMIT_EXAM.studentname,FBNPC_PAPER_SET_HEAD.Section,FBNPC_SECTIONS.SectionName,FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += "  ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  "
            strProcedureBody += " from FBNPC_SUBMIT_EXAM"
            strProcedureBody += " left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " 			left outer join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.PaperID=FBNPC_SUBMIT_EXAM.PaperID"
            strProcedureBody += " left outer join FBNPC_SECTIONS on FBNPC_SECTIONS.SectionID=FBNPC_PAPER_SET_HEAD.Section"
            strProcedureBody += " 			)xx"
            strProcedureBody += " where xx.studentname = @ID and xx.sectionName='Individual' ) finalresult"
            strProcedureBody += " group by finalresult.studentname,finalresult.SectionName"
            strProcedureBody += " union all"
            strProcedureBody += " select 'Wrong' as SectionName,sum(finalresult.WrongAns) as WrongAns  from (select *,case when StudentANS=[Correct ANS] then 1 else 0 end FinalAns,case when StudentANS<>[Correct ANS] then 1 else 0 end WrongAns from (select  FBNPC_SUBMIT_EXAM.studentname,FBNPC_PAPER_SET_HEAD.Section,FBNPC_SECTIONS.SectionName,FBNPC_QUSTIONSSHEET.Question,case when FBNPC_SUBMIT_EXAM.OptionA=1 then 'A' else case when OptionB=1 then 'B' else case when FBNPC_SUBMIT_EXAM.OptionC=1 then 'C' else case when FBNPC_SUBMIT_EXAM.OptionD=1 then 'D' end end end end as StudentANS"
            strProcedureBody += " ,FBNPC_QUSTIONSSHEET.correctAns as [Correct ANS]  "
            strProcedureBody += " from FBNPC_SUBMIT_EXAM"
            strProcedureBody += " left outer join FBNPC_QUSTIONSSHEET on FBNPC_QUSTIONSSHEET.QuestionID=FBNPC_SUBMIT_EXAM.QuestionID"
            strProcedureBody += " left outer join FBNPC_PAPER_SET_HEAD on FBNPC_PAPER_SET_HEAD.PaperID=FBNPC_SUBMIT_EXAM.PaperID"
            strProcedureBody += " left outer join FBNPC_SECTIONS on FBNPC_SECTIONS.SectionID=FBNPC_PAPER_SET_HEAD.Section"
            strProcedureBody += " 			)xx"
            strProcedureBody += " where xx.studentname = @ID and xx.sectionName='Individual') finalresult"
            strProcedureBody += " group by finalresult.studentname,finalresult.SectionName"
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Student_DashboardScore_Board_Individual", strProcedureBody)

            strProcedureBody = " 
@GalleryID varchar(20),
@CompanyName varchar(60),
@category varchar(20),
@GalleryType varchar(10),
@Name varchar(30),
@Desc varchar(max),
@FileName varchar(50),
@FileType varchar(50),
@FileData varbinary(max),
@ModifyBy varchar(10),
@CreatedBy varchar(10),
@ISActive varchar(10),
@ImageID varchar(5)

As
BEGIN
   if not exists (select * from FBNPC_Gallery_Master where GalleryID=@GalleryID) 
   begin
insert into FBNPC_Gallery_Master (GalleryID,Name,GalleryType,Category,CompanyName,Description,fileName,FileType,FileData,CreatedBy,CreatedDate,ModifyBy,ModifyDate,IsActive)
values(@GalleryID,@Name,@GalleryType,@category,@CompanyName,@Desc,@fileName,@fileType,@FileData,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@isactive)
end
else
begin
 if (@imageID='1') 
 begin
 update FBNPC_Gallery_Master set name=@name,companyname=@companyname,description=@desc,filename=@filename,filetype=@filetype,filedata=@filedata,isactive=@isactive,modifyBy=@ModifyBy,modifydate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') where GalleryID=@GalleryID
 end
 else
 begin
  update FBNPC_Gallery_Master set name=@name,companyname=@companyname,description=@desc,isactive=@isactive,modifyBy=@ModifyBy,modifydate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') where GalleryID=@GalleryID

end
END
end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_GalleryImage_Insert", strProcedureBody)

            strProcedureBody = " @AchieverID varchar(20),@FirstName varchar(30),@LastName varchar(30),@City varchar(30),@Country varchar(30),@OnLandingPage integer,@Desc varchar(max),@ISActive varchar(10),@FileName varchar(50),@FileType varchar(50),@FileData varbinary(max),@ModifyBy varchar(30),@CreatedBy varchar(30),@ImageID varchar(5),@State varchar(20),@DocType varchar(30)
As BEGIN if not exists (select * from KSCN_Achiever_Master where AchieverID=@AchieverID) 
   begin
insert into KSCN_Achiever_Master (AchieverID,FirstName,LastName,City,Country,OnLandingPage,Description,Isactive,fileName,FileType,FileData,CreatedBy,CreatedDate,ModifyBy,ModifyDate,State,DocType)
values(@AchieverID,@FirstName,@LastName,@City,@Country,@OnLandingPage,@Desc,@isactive,@fileName,@fileType,@FileData,@CreatedBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@ModifyBy,SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00'),@State,@DocType)
end
else
begin
 if (@imageID='1') 
 begin
 update KSCN_Achiever_Master set Firstname=@Firstname,Lastname=@Lastname,OnLandingPage=@OnLandingPage,description=@desc,Country=@Country,State=@State,City=@City,filename=@filename,filetype=@filetype,filedata=@filedata,isactive=@isactive,modifyBy=@ModifyBy,modifydate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where AchieverID=@AchieverID
 end
 else
 begin
  update KSCN_Achiever_Master set Firstname=@Firstname,Lastname=@Lastname,OnLandingPage=@OnLandingPage,description=@desc,Country=@Country,State=@State,City=@City,isactive=@isactive,modifyBy=@ModifyBy,modifydate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '-06:00') where AchieverID=@AchieverID

end
END
end "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_AchieverImage_Insert", strProcedureBody)

#Region "Next / Previous"
            strProcedureBody = "@ID varchar(30),@StudentName varchar(20),@QusNo integer as select * from (select distinct FBNPC_QustionsSheet.*,ROW_NUMBER() OVER (ORDER BY (FBNPC_QustionsSheet.questionid)) as  num,FBNPC_PAPER_SET_DETAIL.avid,FBNPC_PAPER_SET_DETAIL.paperid,case when KSCN_Temp_Table_Exam.optionA=1 then 1 else case when KSCN_Temp_Table_Exam.optionb=1 then 2 else case when  KSCN_Temp_Table_Exam.optionC=1 then 3 else case when  KSCN_Temp_Table_Exam.optionD=1 then 4 else 0 end end end end as UserAns from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID 
left join KSCN_Temp_Table_Exam on KSCN_Temp_Table_Exam.QuestionID=FBNPC_QustionsSheet.QuestionID
where FBNPC_PAPER_SET_DETAIL.examid=@ID and FBNPC_PAPER_SET_DETAIL.PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName))final where 2=2 
and num=@QusNo-1 "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Individual_Sheet_Question_Previous", strProcedureBody)

            strProcedureBody = "@ID varchar(30),@StudentName varchar(20),@QusNo integer as select * from (select distinct FBNPC_QustionsSheet.*,ROW_NUMBER() OVER (ORDER BY (FBNPC_QustionsSheet.questionid)) as  num,FBNPC_PAPER_SET_DETAIL.avid,FBNPC_PAPER_SET_DETAIL.paperid,case when KSCN_Temp_Table_Exam.optionA=1 then 1 else case when KSCN_Temp_Table_Exam.optionb=1 then 2 else case when  KSCN_Temp_Table_Exam.optionC=1 then 3 else case when  KSCN_Temp_Table_Exam.optionD=1 then 4 else 0 end end end end as UserAns from FBNPC_QustionsSheet inner join FBNPC_PAPER_SET_DETAIL on FBNPC_PAPER_SET_DETAIL.QuestionID=FBNPC_QustionsSheet.QuestionID 
left join KSCN_Temp_Table_Exam on KSCN_Temp_Table_Exam.QuestionID=FBNPC_QustionsSheet.QuestionID
where FBNPC_PAPER_SET_DETAIL.examid=@ID and FBNPC_PAPER_SET_DETAIL.PaperID not in (select PaperId from FBNPC_Exam_Question_Validtion where StudentName=@StudentName))final where 2=2 
and num=@QusNo+1 "
            clsCommonFunctionality.CreateStoreProcedure("FBNPC_Show_Individual_Sheet_Question_Next", strProcedureBody)

            strProcedureBody = "@ExamID varchar(30),@StudentID varchar(20) as select * from KSCN_Temp_Table_Exam where examname=@ExamID and Studentname=@StudentID"
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Temp_To_Orignal_Show", strProcedureBody)

            strProcedureBody = "@ExamID varchar(30),@StudentID varchar(20),@PaperID varchar(20) as delete from KSCN_Temp_Table_Exam where examname=@ExamID and Studentname=@StudentID and PaperID=@PaperID"
            clsCommonFunctionality.CreateStoreProcedure("KSCN_Temp_Delete", strProcedureBody)
#End Region


            clsCommon.ProgressBarHide()
        Catch ex As Exception
            clsCommon.ProgressBarHide()
            'clsCommon.MyMessageBoxShow(ex.Message, "Store Procedure")
        End Try
    End Sub
End Class

Public Class clsAllSQLFunction
    Public Shared Sub CreateAllSQLFunction()
        Try

            clsCommon.ProgressBarShow()
            Dim strFunctionBody As String = " CREATE FUNCTION ExplodeDates(@startdate datetime, @enddate datetime)" & _
            " returns table as " & _
            " return ( " & _
            " with  " & _
            "  N0 as (SELECT 1 as n UNION ALL SELECT 1) " & _
            " ,N1 as (SELECT 1 as n FROM N0 t1, N0 t2) " & _
            " ,N2 as (SELECT 1 as n FROM N1 t1, N1 t2) " & _
            " ,N3 as (SELECT 1 as n FROM N2 t1, N2 t2) " & _
            " ,N4 as (SELECT 1 as n FROM N3 t1, N3 t2) " & _
            " ,N5 as (SELECT 1 as n FROM N4 t1, N4 t2) " & _
            " ,N6 as (SELECT 1 as n FROM N5 t1, N5 t2) " & _
            " ,nums as (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as num FROM N6) " & _
            " SELECT DATEADD(day,num-1,@startdate) as thedate " & _
            " FROM nums " & _
            " WHERE num <= DATEDIFF(day,@startdate,@enddate) + 1 " & _
            " ); "
            clsCommonFunctionality.CreateSQLFunctioin("ExplodeDates", strFunctionBody)
            '=========================================================================

            'clsDBFuncationality.QueryAnalyzerStart()

            strFunctionBody = " CREATE FUNCTION TSPL_FUN_LEAVE_STATUS (@PAY_PERIOD_CODE VARCHAR(30))" & _
                              " RETURNS @TSPL_FUN_LEAVE_STATUS_TYPE TABLE " & _
                              " ( " & _
                              " EMP_CODE VARCHAR(30),   " & _
                              " LEAVE_CODE VARCHAR(30), " & _
                              " OPENING NUMERIC(12,2),  " & _
                              " ALLOTED NUMERIC(12,2),  " & _
                              " AVAILED NUMERIC(12,2),  " & _
                              " ADJUSTMENT_PLUS NUMERIC(12,2), " & _
                              " ADJUSTMENT_MINUS NUMERIC(12,2)," & _
                              " BALANCE NUMERIC(12,2)    " & _
                              " )  " & _
                              " AS " & _
                              " BEGIN " & _
                              " DECLARE @FROM_DATE DATE; " & _
                              " DECLARE @TO_DATE DATE;   " & _
                              " SELECT @FROM_DATE=DATE_FROM,@TO_DATE=DATE_TO FROM TSPL_PAYPERIOD_MASTER WHERE PAY_PERIOD_CODE=@PAY_PERIOD_CODE; " & _
                              " INSERT INTO @TSPL_FUN_LEAVE_STATUS_TYPE(EMP_CODE,LEAVE_CODE,OPENING,ALLOTED,AVAILED,ADJUSTMENT_PLUS,ADJUSTMENT_MINUS,BALANCE) " & _
                              " ( " & _
                              " SELECT T1.EMP_CODE,T1.LEAVE_CODE,COALESCE(T2.BALANCE,0) AS OPENING,COALESCE(T3.ALLOTED,0) AS ALLOTED,COALESCE(T4.AVAILED,0) AS AVAILED, " & _
                              " COALESCE(T5.ADJUST_PLUS,0) AS ADJUSTMENT_PLUS,COALESCE(T5.ADJUST_MINUS,0) AS ADJUSTMENT_MINUS, " & _
                              " (COALESCE(T2.BALANCE,0)+COALESCE(T3.ALLOTED,0)-COALESCE(T4.AVAILED,0)+ " & _
                              " COALESCE(T5.ADJUST_PLUS,0)-COALESCE(T5.ADJUST_MINUS,0)) AS BALANCE FROM (SELECT T1.*,T2.LEAVE_CODE FROM ( " & _
                              " SELECT T1.EMP_STATUS_CODE,T1.ATTENDANCE_CODE,T1.EMP_CODE,T1.WORKING_STATUS  FROM TSPL_EMPLOYEE_STATUS T1 " & _
                              " INNER JOIN (SELECT EMP_CODE,MAX(EMP_STATUS_CODE) AS EMP_STATUS_CODE,MAX(REVISION_NO) AS REVISION_NO " & _
                              " FROM TSPL_EMPLOYEE_STATUS  WHERE WORKING_STATUS='WORKING' GROUP BY EMP_CODE) AS T2 " & _
                              " ON T1.EMP_STATUS_CODE=T2.EMP_STATUS_CODE) AS T1, TSPL_LEAVE_MASTER AS T2) AS T1 " & _
                              " LEFT JOIN ( " & _
                              " SELECT EMP_CODE,LEAVE_CODE,(COALESCE(SUM(ALLOTED),0)-COALESCE(SUM(AVAILED),0)) AS BALANCE FROM  TSPL_VIEW_LEAVE_LEDGER WHERE (TR_TYPE='OB' and DATE_FROM<=@FROM_DATE ) or (TR_TYPE<>'OB' and DATE_FROM<@FROM_DATE) " & _
                              " GROUP BY EMP_CODE,LEAVE_CODE " & _
                              " ) AS T2 ON T1.EMP_CODE=T2.EMP_CODE AND T1.LEAVE_CODE=T2.LEAVE_CODE " & _
                              " LEFT JOIN ( " & _
                              " SELECT EMP_CODE,LEAVE_CODE,COALESCE(SUM(ALLOTED),0) AS ALLOTED ,COALESCE(SUM(AVAILED),0) AS AVAILED FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE='ALLOT' " & _
                              " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                              " ) AS T3 ON T1.EMP_CODE=T3.EMP_CODE AND T1.LEAVE_CODE=T3.LEAVE_CODE " & _
                              " LEFT JOIN ( " & _
                              " SELECT EMP_CODE,LEAVE_CODE,COALESCE(SUM(ALLOTED),0) AS ALLOTED ,COALESCE(SUM(AVAILED),0) AS AVAILED FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE='AVAIL' " & _
                              " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                              " ) AS T4 ON T1.EMP_CODE=T4.EMP_CODE AND T1.LEAVE_CODE=T4.LEAVE_CODE " & _
                              " LEFT JOIN ( " & _
                              " SELECT EMP_CODE,LEAVE_CODE,SUM(ALLOTED) AS ADJUST_PLUS ,SUM(AVAILED)AS ADJUST_MINUS FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE IN ('ADJ(+)','ADJ(-)' ) " & _
                              " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                              " ) AS T5 ON T1.EMP_CODE=T5.EMP_CODE AND T1.LEAVE_CODE=T5.LEAVE_CODE); " & _
                              " RETURN; " & _
                              " End "
            clsCommonFunctionality.CreateSQLFunctioin("TSPL_FUN_LEAVE_STATUS", strFunctionBody)
            'clsDBFuncationality.QueryAnalyzerStop()
            '=====================================================================
            strFunctionBody = " CREATE FUNCTION funMismatchVoucher(@Voucher varchar(30))" & _
                            " returns varchar(30) " & _
                             " as begin " & _
                            " declare @Result varchar(30) " & _
                            " select  top 1 @Result= Voucher_No  from ( " & _
                            " select * from( " & _
                            " select Voucher_No,MAX(Voucher_Date) as Voucher_Date,LOC,SUM(case when Amount>0 then Amount else 0 end) as DrAmt,SUM(case when Amount<0 then -1*Amount else 0 end) as CrAmt,MAX(Source_Code) as Source_Code,max(Source_Doc_No) as Source_Doc_No,max(Type) as Type  from ( " & _
                             " select TSPL_JOURNAL_DETAILS.Voucher_No, TSPL_JOURNAL_DETAILS.Account_code,TSPL_JOURNAL_DETAILS.Amount " & _
                            " ,SUBSTRING(TSPL_JOURNAL_DETAILS.Account_code,LEN(TSPL_JOURNAL_DETAILS.Account_code)-2,3) as LOC,TSPL_JOURNAL_MASTER.Source_Code,TSPL_JOURNAL_MASTER.Voucher_Date,TSPL_JOURNAL_MASTER.Source_Doc_No,TSPL_JOURNAL_MASTER.Type " & _
                            " from TSPL_JOURNAL_DETAILS " & _
                            " left outer join TSPL_JOURNAL_MASTER on TSPL_JOURNAL_MASTER .Voucher_No=TSPL_JOURNAL_DETAILS.Voucher_No " & _
                            " )xxx group by Voucher_No,LOC  " & _
                            " )xxxxx where DrAmt<>CrAmt   " & _
                            " )xxxx where xxxx.Voucher_No=@Voucher group by  Voucher_No   " & _
                            " return @Result End "
            clsCommonFunctionality.CreateSQLFunctioin("funMismatchVoucher", strFunctionBody)
            '=========================================================================================
            strFunctionBody = "CREATE FUNCTION ProperCase(@Text as varchar(8000)) " & _
                             " returns varchar(8000) " & _
                             " as begin " & _
                             " declare @Reset bit; " & _
                            " declare @Ret varchar(8000); " & _
                            " declare @i int;" & _
                            " declare @c char(1); " & _
                            " select @Reset = 1, @i=1, @Ret = ''; " & _
                            " while (@i <= len(@Text)) " & _
                             " select @c= substring(@Text,@i,1), " & _
                            " @Ret = @Ret + case when @Reset=1 then UPPER(@c) else LOWER(@c) end, " & _
                            " @Reset = case when @c like '[a-zA-Z]' then 0 else 1 end, " & _
                            " @i = @i +1 " & _
                             " return @Ret " & _
                             "  End "
            clsCommonFunctionality.CreateSQLFunctioin("ProperCase", strFunctionBody)
            '===========================
            strFunctionBody = " create function getWeekNo(@P_Date date) returns int as begin return (" & _
             " Select DATEPART(week, @P_Date )- datepart(week,('01/'+left(DATENAME(month,@P_date),3)+'/' + convert(varchar, datepart(year,@p_date) )))+1 " & _
             " ) end"
            clsCommonFunctionality.CreateSQLFunctioin("getWeekNo", strFunctionBody)
            '==========================added by shivani
            strFunctionBody = " CREATE FUNCTION [TSPL_FUN_LEAVE_STATUS_PERIOD] ( @FROM_DATE DATE,@TO_DATE DATE)" & _
                             " RETURNS @TSPL_FUN_LEAVE_STATUS_TYPE TABLE " & _
                             " ( " & _
                             " EMP_CODE VARCHAR(30),   " & _
                             " LEAVE_CODE VARCHAR(30), " & _
                             " OPENING NUMERIC(12,2),  " & _
                             " ALLOTED NUMERIC(12,2),  " & _
                             " AVAILED NUMERIC(12,2),  " & _
                             " ADJUSTMENT_PLUS NUMERIC(12,2), " & _
                             " ADJUSTMENT_MINUS NUMERIC(12,2)," & _
                             " BALANCE NUMERIC(12,2)    " & _
                             " )  " & _
                             " AS " & _
                             " BEGIN " & _
                             " INSERT INTO @TSPL_FUN_LEAVE_STATUS_TYPE(EMP_CODE,LEAVE_CODE,OPENING,ALLOTED,AVAILED,ADJUSTMENT_PLUS,ADJUSTMENT_MINUS,BALANCE) " & _
                             " ( " & _
                             " SELECT T1.EMP_CODE,T1.LEAVE_CODE,COALESCE(T2.BALANCE,0) AS OPENING,COALESCE(T3.ALLOTED,0) AS ALLOTED,COALESCE(T4.AVAILED,0) AS AVAILED, " & _
                             " COALESCE(T5.ADJUST_PLUS,0) AS ADJUSTMENT_PLUS,COALESCE(T5.ADJUST_MINUS,0) AS ADJUSTMENT_MINUS, " & _
                             " (COALESCE(T2.BALANCE,0)+COALESCE(T3.ALLOTED,0)-COALESCE(T4.AVAILED,0)+ " & _
                             " COALESCE(T5.ADJUST_PLUS,0)-COALESCE(T5.ADJUST_MINUS,0)) AS BALANCE FROM (SELECT T1.*,T2.LEAVE_CODE FROM ( " & _
                             " SELECT T1.EMP_STATUS_CODE,T1.ATTENDANCE_CODE,T1.EMP_CODE,T1.WORKING_STATUS  FROM TSPL_EMPLOYEE_STATUS T1 " & _
                             " INNER JOIN (SELECT EMP_CODE,MAX(EMP_STATUS_CODE) AS EMP_STATUS_CODE,MAX(REVISION_NO) AS REVISION_NO " & _
                             " FROM TSPL_EMPLOYEE_STATUS  WHERE WORKING_STATUS='WORKING' GROUP BY EMP_CODE) AS T2 " & _
                             " ON T1.EMP_STATUS_CODE=T2.EMP_STATUS_CODE) AS T1, TSPL_LEAVE_MASTER AS T2) AS T1 " & _
                             " LEFT JOIN ( " & _
                             " SELECT EMP_CODE,LEAVE_CODE,(COALESCE(SUM(ALLOTED),0)-COALESCE(SUM(AVAILED),0)) AS BALANCE FROM  TSPL_VIEW_LEAVE_LEDGER WHERE (TR_TYPE='OB' and DATE_FROM<=@FROM_DATE ) or (TR_TYPE<>'OB' and DATE_FROM<@FROM_DATE) " & _
                             " GROUP BY EMP_CODE,LEAVE_CODE " & _
                             " ) AS T2 ON T1.EMP_CODE=T2.EMP_CODE AND T1.LEAVE_CODE=T2.LEAVE_CODE " & _
                             " LEFT JOIN ( " & _
                             " SELECT EMP_CODE,LEAVE_CODE,COALESCE(SUM(ALLOTED),0) AS ALLOTED ,COALESCE(SUM(AVAILED),0) AS AVAILED FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE='ALLOT' " & _
                             " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                             " ) AS T3 ON T1.EMP_CODE=T3.EMP_CODE AND T1.LEAVE_CODE=T3.LEAVE_CODE " & _
                             " LEFT JOIN ( " & _
                             " SELECT EMP_CODE,LEAVE_CODE,COALESCE(SUM(ALLOTED),0) AS ALLOTED ,COALESCE(SUM(AVAILED),0) AS AVAILED FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE='AVAIL' " & _
                             " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                             " ) AS T4 ON T1.EMP_CODE=T4.EMP_CODE AND T1.LEAVE_CODE=T4.LEAVE_CODE " & _
                             " LEFT JOIN ( " & _
                             " SELECT EMP_CODE,LEAVE_CODE,SUM(ALLOTED) AS ADJUST_PLUS ,SUM(AVAILED)AS ADJUST_MINUS FROM  TSPL_VIEW_LEAVE_LEDGER WHERE TR_TYPE IN ('ADJ(+)','ADJ(-)' ) " & _
                             " AND DATE_FROM BETWEEN @FROM_DATE AND @TO_DATE GROUP BY EMP_CODE,LEAVE_CODE " & _
                             " ) AS T5 ON T1.EMP_CODE=T5.EMP_CODE AND T1.LEAVE_CODE=T5.LEAVE_CODE); " & _
                             " RETURN; " & _
                             " End "
            clsCommonFunctionality.CreateSQLFunctioin("TSPL_FUN_LEAVE_STATUS_PERIOD", strFunctionBody)

            '' Function Created by Panch Raj
            strFunctionBody = "create function [dbo].[GetConversion](@Item_Code  varchar(50), @Unit_Code varchar(30)) returns float as " & _
                              " begin " & _
                              " declare @Kg_Value float; " & _
                              " declare @Product_Type varchar(10); " & _
                              " declare @Wt_uom varchar(30); " & _
                              " declare @Wt_Value float; " & _
                              " declare @Cnvsrn_Factr float; " & _
                              " declare @Weight_KG_Unit varchar(30); " & _
                              " declare @KG_Cnvrsn_Value float; " & _
                              " select @Product_Type=Product_Type from TSPL_ITEM_MASTER where Item_Code=@Item_Code; " & _
                              " select @Wt_uom=Weight_UOM,@Wt_Value=Weight_Value from TSPL_ITEM_MASTER where Item_Code=@Item_Code;" & _
                              " select @Cnvsrn_Factr=Conversion_Factor from TSPL_ITEM_UOM_DETAIL where Item_Code=@Item_Code and UOM_Code=@Unit_Code;" & _
                              " select @Weight_KG_Unit=Description from TSPL_FIXED_PARAMETER where TYPE='ProductionFATSNF_KG_Unit' and Code='ProductionFATSNF_KG_Unit';" & _
                              " If @Wt_uom= @Weight_KG_Unit " & _
                              " set  @KG_Cnvrsn_Value = 1; " & _
                              " else " & _
                              " begin " & _
                              " select @KG_Cnvrsn_Value=Conversion_Factor from TSPL_ITEM_UOM_DETAIL where Item_Code=@Item_Code and UOM_Code=@Weight_KG_Unit; " & _
                              " if @KG_Cnvrsn_Value<>0 " & _
                              " begin " & _
                              " set @KG_Cnvrsn_Value=(1/@KG_Cnvrsn_Value); " & _
                              " set @Wt_Value=1; " & _
                              " End " & _
                              " else " & _
                              " select top 1  @KG_Cnvrsn_Value=CF from (Select (case when (Container_UOM=@Wt_uom and Contained_UOM=@Weight_KG_Unit)  then round(Contained_Qty/Container_Qty,8) else case when (Container_UOM=@Weight_KG_Unit and Contained_UOM=@Wt_uom)  then round(Container_Qty/Contained_Qty,8) end end) as CF,product_type from TSPL_WEIGHT_CONVERSION   " & _
                              " where product_type in ('ALL',@Product_Type))aa where isnull(cast(CF as float),0)<>0 order by Product_Type desc " & _
                              " End " & _
                              " Return @KG_Cnvrsn_Value; " & _
                              " End "

            clsCommonFunctionality.CreateSQLFunctioin("GetConversion", strFunctionBody)

            ' '' Function Created by Panch Raj
            'strFunctionBody = "Create FUNCTION [dbo].[TSPL_FUN_GET_STOCK_BASE_DATA] (@ITEM_CODE VARCHAR(50),@LOCATION_CODE VARCHAR(12),@TRANS_DATE DATE) RETURNS TABLE as " & Environment.NewLine & _
            '     " RETURN (select Final.Product_Type,Final.Trans_Type, " & Environment.NewLine & _
            '     " Final.InOut,Final.Location_Code,Final.Source_Doc_No,Final.Item_Code,Item.Item_Desc, " & Environment.NewLine & _
            '     " Final.Stock_Qty,(CASE WHEN INOUT='I' THEN Final.Stock_Qty ELSE 0 END) AS IN_QTY,(CASE WHEN INOUT='O' THEN Final.Stock_Qty ELSE 0 END) AS OUT_QTY, " & Environment.NewLine & _
            '     " Final.Stock_UOM,Final.Net_Cost ,Final.Avg_Cost,Final.FIFO_COST,Final.LIFO_COST, " & Environment.NewLine & _
            '     " Final.Basic_Cost,Final.Location_Code+'Qty' as LocQty ,Final.Location_Code+'Cost' as LocCost, " & Environment.NewLine & _
            '     " (case when Final.Product_Type='MI' then Final.Fat_Per else  Item_Fat.Fat_Per end) as Fat_Per, " & Environment.NewLine & _
            '     " (case when Final.Product_Type='MI' then Final.SNF_Per else  Item_SNF.SNF_Per end) as SNF_Per, " & Environment.NewLine & _
            '     " (case when Final.Product_Type='MI' then Final.FAT_Kg else  (case when coalesce(StockKG.Conversion_Factor,0)=0 then 0 " & Environment.NewLine & _
            '     " else cast((Final.Stock_Qty*Item_Fat.Fat_Per*Stock_SU.Conversion_Factor)/(coalesce(StockKG.Conversion_Factor,1)*100) as float) end) end) as FAT_Kg, " & Environment.NewLine & _
            '     " (case when Final.Product_Type='MI' then Final.SNF_Kg else  (case when coalesce(StockKG.Conversion_Factor,0)=0 then 0 " & Environment.NewLine & _
            '     " else cast((Final.Stock_Qty*Item_SNF.SNF_Per*Stock_SU.Conversion_Factor)/(coalesce(StockKG.Conversion_Factor,1)*100) as float) end) end) as SNF_Kg, " & Environment.NewLine & _
            '     " Punching_Date,'' AS OP_TYPE " & Environment.NewLine & _
            '     " from (  " & Environment.NewLine & _
            '     " select 'MP' as Product_Type,Trans_Type,InOut,Location_Code,Source_Doc_No,Item_Code,Stock_Qty,Stock_UOM,Net_Cost,Avg_Cost,FIFO_COST,LIFO_COST,Basic_Cost, " & Environment.NewLine & _
            '     " 0 as Fat_Per,0 as SNF_Per,0 as FAT_Kg ,0 as SNF_Kg,cast(Punching_Date as date) as Punching_Date,'' AS OP_TYPE " & Environment.NewLine & _
            '     " from TSPL_INVENTORY_MOVEMENT_WIN where 2=2  AND CAST(PUNCHING_DATE AS DATE)<=@TRANS_DATE AND LOCATION_CODE=@LOCATION_CODE " & Environment.NewLine & _
            '     " AND ITEM_CODE=(CASE WHEN LEN(@ITEM_CODE)>0 THEN @ITEM_CODE ELSE ITEM_CODE END) " & Environment.NewLine & _
            '     " union all " & Environment.NewLine & _
            '     " select  'MI' as Product_Type,Trans_Type,InOut,Location_Code,Source_Doc_No,Item_Code,Stock_Qty,Stock_UOM,Net_Cost,Avg_Cost,FIFO_COST,LIFO_COST,Basic_Cost, " & Environment.NewLine & _
            '     " Fat_Per,SNF_Per,FAT_Kg,SNF_Kg,cast(Punching_Date as date) as Punching_Date,'' AS OP_TYPE " & Environment.NewLine & _
            '     " from TSPL_INVENTORY_MOVEMENT_NEW_WIN  where 2=2 AND CAST(PUNCHING_DATE AS DATE)<=@TRANS_DATE AND LOCATION_CODE=@LOCATION_CODE " & Environment.NewLine & _
            '     " AND ITEM_CODE=(CASE WHEN LEN(@ITEM_CODE)>0 THEN @ITEM_CODE ELSE ITEM_CODE END) " & Environment.NewLine & _
            '     " ) as Final " & Environment.NewLine & _
            '     " left join TSPL_ITEM_MASTER Item on Final.Item_Code=Item.Item_Code " & Environment.NewLine & _
            '     " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL) as Stock_SU on Final.Item_Code=Stock_SU.Item_Code " & Environment.NewLine & _
            '     " and Final.Stock_UOM=Stock_SU.UOM_Code " & Environment.NewLine & _
            '     " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL where UOM_Code='KG') as StockKG on Final.Item_Code=StockKG.Item_Code " & Environment.NewLine & _
            '     " left join (select Item_QC.Item_Code,max(Item_QC.Actual_Range) as Fat_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & Environment.NewLine & _
            '     " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='FAT' " & Environment.NewLine & _
            '     " group by Item_QC.Item_Code) as Item_Fat on Final.Item_Code=Item_Fat.Item_Code " & Environment.NewLine & _
            '     " left join (select  Item_QC.Item_Code,max(Item_QC.Actual_Range) as SNF_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & Environment.NewLine & _
            '     " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='SNF' " & Environment.NewLine & _
            '     " group by Item_QC.Item_Code) Item_SNF on Final.Item_Code=Item_SNF.Item_Code " & Environment.NewLine & _
            '     " left join TSPL_LOCATION_MASTER Loc on Final.Location_Code=Loc.Location_Code );"
            'clsCommonFunctionality.CreateSQLFunctioin("TSPL_FUN_GET_STOCK_BASE_DATA", strFunctionBody)
            '' Function Created by Panch Raj
            strFunctionBody = "Create FUNCTION [dbo].[TSPL_FUN_ITEM_LOC_BALANCE] (@ITEM_CODE VARCHAR(50),@LOCATION_CODE VARCHAR(12),@TRANS_DATE DATE) RETURNS TABLE as " & Environment.NewLine & _
            " RETURN (SELECT MAX(TRANS_DATE) AS TRANS_DATE,Location_Code,Item_Code,Stock_UOM,sum(FIFO_Cost) as FIFO_Cost,sum(LIFO_Cost) as LIFO_Cost, " & Environment.NewLine & _
            " sum(Avg_Cost) as Avg_Cost,sum(IN_QTY) as IN_QTY,sum(Out_QTY) as Out_QTY,sum(TRANS_QTY) as TRANS_QTY,sum(Fat_KG) as Fat_KG,sum(SNF_KG) as SNF_KG," & Environment.NewLine & _
            " sum(IN_QTY+Out_QTY) as CL_QTY,sum(In_FAT_KG+Out_Fat_KG) as CL_FAT_KG,sum(In_SNF_KG+Out_SNF_KG) as CL_SNF_KG, sum(CL_FIFO_Cost) as CL_FIFO_Cost,sum(CL_LIFO_Cost) as CL_LIFO_Cost," & Environment.NewLine & _
            " sum(In_Avg_Cost+Out_Avg_Cost) as CL_Avg_Cost,max(AGEING_Flag) as AGEING_Flag,sum(In_Avg_Cost) as In_Avg_Cost,sum(Out_Avg_Cost) as Out_Avg_Cost, " & Environment.NewLine & _
            " sum(In_Fat_KG) as In_Fat_KG,sum(Out_Fat_KG) as Out_Fat_KG,sum(In_SNF_KG) as In_SNF_KG,sum(Out_SNF_KG) as Out_SNF_KG," & Environment.NewLine & _
            " sum(Trans_FAT_KG) as Trans_FAT_KG,sum(Trans_SNF_KG) as Trans_SNF_KG " & Environment.NewLine & _
            " FROM (select TRANS_DATE,Location_Code,Item_Code,Stock_UOM,sum(FIFO_Cost) as FIFO_Cost,sum(LIFO_Cost) as LIFO_Cost, " & Environment.NewLine & _
            " sum(Avg_Cost) as Avg_Cost,sum(IN_QTY) as IN_QTY,sum(Out_QTY) as Out_QTY,sum(TRANS_QTY) as TRANS_QTY,sum(Fat_KG) as Fat_KG,sum(SNF_KG) as SNF_KG," & Environment.NewLine & _
            " sum(IN_QTY+Out_QTY) as CL_QTY,sum(In_FAT_KG+Out_Fat_KG) as CL_FAT_KG,sum(In_SNF_KG+Out_SNF_KG) as CL_SNF_KG, sum(CL_FIFO_Cost) as CL_FIFO_Cost,sum(CL_LIFO_Cost) as CL_LIFO_Cost," & Environment.NewLine & _
            " sum(In_Avg_Cost+Out_Avg_Cost) as CL_Avg_Cost,max(AGEING_Flag) as AGEING_Flag,sum(In_Avg_Cost) as In_Avg_Cost,sum(Out_Avg_Cost) as Out_Avg_Cost, " & Environment.NewLine & _
            " sum(In_Fat_KG) as In_Fat_KG,sum(Out_Fat_KG) as Out_Fat_KG,sum(In_SNF_KG) as In_SNF_KG,sum(Out_SNF_KG) as Out_SNF_KG, sum(In_Fat_KG+Out_Fat_KG) as Trans_FAT_KG," & Environment.NewLine & _
            " sum(In_SNF_KG+Out_SNF_KG) as Trans_SNF_KG " & Environment.NewLine & _
            " from (select TRANS_DATE,Location_Code,Item_Code,Item_Desc,Stock_UOM,FIFO_Cost,LIFO_Cost,Avg_Cost,IN_QTY,Out_QTY,TRANS_QTY,Fat_KG,SNF_KG,CL_QTY,CL_FAT_KG,CL_SNF_KG," & Environment.NewLine & _
            " CL_FIFO_Cost,CL_LIFO_Cost,CL_Avg_Cost,AGEING_Flag,In_Avg_Cost,Out_Avg_Cost,In_Fat_KG,Out_Fat_KG,In_SNF_KG,Out_SNF_KG " & Environment.NewLine & _
            " from TSPL_INV_MOVE_DL WHERE TRANS_DATE<=@TRANS_DATE AND LOCATION_CODE=@LOCATION_CODE AND ITEM_CODE=(CASE WHEN LEN(@ITEM_CODE)>0 THEN @ITEM_CODE ELSE ITEM_CODE END)" & Environment.NewLine & _
            " union all " & Environment.NewLine & _
            " select Punching_Date as TRANS_DATE,Location_Code,Item_Code,Item_Desc,Stock_UOM,(case when OP_TYPE='D' then -1 else 1 end)*FIFO_COST*(case when INOUT='I' then 1 else -1 end) as FIFO_COST,(case when OP_TYPE='D' then -1 else 1 end)*LIFO_COST*(case when INOUT='I' then 1 else -1 end) as LIFO_COST,(case when OP_TYPE='D' then -1 else 1 end)*Avg_Cost*(case when INOUT='I' then 1 else -1 end) as Avg_Cost,(case when OP_TYPE='D' then -1 else 1 end)*IN_QTY*(case when INOUT='I' then 1 else -1 end) as IN_QTY,(case when OP_TYPE='D' then -1 else 1 end)*Out_QTY*(case when INOUT='I' then 1 else -1 end) as Out_QTY,(case when OP_TYPE='D' then -1 else 1 end)*(IN_QTY-Out_QTY) as Trans_Qty," & Environment.NewLine & _
            " (case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='I' then Fat_KG else -FAT_Kg end) as Fat_KG,(case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='I' then SNF_KG else -SNF_Kg end) as SNF_KG,0 as CL_QTY,0 as CL_FAT_KG,0 as CL_SNF_KG,0 as CL_FIFO_Cost,0 as CL_LIFO_Cost,0 as CL_Avg_Cost,0 as AGEING_Flag," & Environment.NewLine & _
            " (case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='I' then Avg_Cost else 0 end) In_Avg_Cost,(case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='O' then -Avg_Cost else 0 end) as Out_Avg_Cost, " & Environment.NewLine & _
            " (case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='I' then Fat_KG else 0 end) as  In_Fat_KG,(case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='O' then -Fat_KG else 0 end) as Out_Fat_KG, " & Environment.NewLine & _
            " (case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='I' then SNF_KG else 0 end) as In_SNF_KG, (case when OP_TYPE='D' then -1 else 1 end)*(case when INOUT='O' then -SNF_KG else 0 end) as Out_SNF_KG from " & Environment.NewLine & _
            " (SELECT * FROM [dbo].View_STOCK_DATA_GIT where Item_Code=(CASE WHEN LEN(@ITEM_CODE)>0 THEN @ITEM_CODE ELSE ITEM_CODE END) and Location_Code=@LOCATION_CODE and Punching_Date<=@TRANS_DATE) GIT ) as  Opening " & Environment.NewLine & _
            " group by TRANS_DATE,Location_Code,Item_Code,Stock_UOM) FINAL GROUP BY Location_Code,Item_Code,Stock_UOM )"
            clsCommonFunctionality.CreateSQLFunctioin("TSPL_FUN_ITEM_LOC_BALANCE", strFunctionBody)
            '' Function Created by Panch Raj
            strFunctionBody = " Create function [dbo].[Get_Location_FatSNF](@Item_Code  varchar(50),@Location_Code  varchar(50),@Trans_Date varchar(20),@SOURCE_DOC_NO VARCHAR(30),@FAT_KG Numeric(18,3),@SNF_KG AS Numeric(18,3),@_QTY AS Numeric(18,2)) returns varchar(max) as  " & Environment.NewLine & _
                              " begin  declare @STOCK_FaT_KG numeric(18,3);" & Environment.NewLine & _
                              " declare @STOCK_SNF_KG numeric(18,3); " & Environment.NewLine & _
                              " declare @STOCK_QTY numeric(18,2); " & Environment.NewLine & _
                              " declare @Stock_UOM VARCHAR(50); " & Environment.NewLine & _
                              " declare @Msg varchar(max); " & Environment.NewLine & _
                              " declare @Sett varchar(1); " & Environment.NewLine & _
                              " declare @GIT_Type varchar(1); " & Environment.NewLine & _
                              " /* select @STOCK_FaT_KG=sum(((case when Final.Product_Type='MI' then Final.FAT_Kg else (case when coalesce(StockKG.Conversion_Factor,0)=0 then 0 " & Environment.NewLine & _
                              " else  cast((Final.Stock_Qty*coalesce(Item_Fat.Fat_Per,0)*Stock_SU.Conversion_Factor)/(coalesce(StockKG.Conversion_Factor,1)*100) as float) end) end) " & Environment.NewLine & _
                              " *(case when Final.InOut='I' then 1 else -1 end))), @STOCK_SNF_KG=sum(((case when Final.Product_Type='MI' then Final.SNF_Kg " & Environment.NewLine & _
                              " else (case when coalesce(StockKG.Conversion_Factor,0)=0 then 0  else cast((Final.Stock_Qty*coalesce(Item_SNF.SNF_Per,0) " & Environment.NewLine & _
                              " *Stock_SU.Conversion_Factor)/(coalesce(StockKG.Conversion_Factor,1)*100) as float) end) end)*  (case when Final.InOut='I' then 1 else -1 end))) from ( " & Environment.NewLine & _
                              " select 'MP' as Product_Type,Trans_Type,InOut,Location_Code,Source_Doc_No,Item_Code,Stock_Qty,Stock_UOM,Net_Cost,Avg_Cost, " & Environment.NewLine & _
                              " 0 as Fat_Per,0 as SNF_Per,0 as FAT_Kg ,0 as SNF_Kg,cast(Punching_Date as date) as Punching_Date " & Environment.NewLine & _
                              " from TSPL_INVENTORY_MOVEMENT where Location_Code=@Location_Code and CAST(Punching_Date AS DATE)<=@Trans_Date " & Environment.NewLine & _
                              " union all " & Environment.NewLine & _
                              " select  'MI' as Product_Type,Trans_Type,InOut,Location_Code,Source_Doc_No,Item_Code,Stock_Qty,Stock_UOM,Net_Cost,Avg_Cost, " & Environment.NewLine & _
                              " Fat_Per,SNF_Per,FAT_Kg,SNF_Kg,cast(Punching_Date as date) as Punching_Date " & Environment.NewLine & _
                              " from TSPL_INVENTORY_MOVEMENT_NEW  where Location_Code=@Location_Code and CAST(Punching_Date AS DATE)<=@Trans_Date " & Environment.NewLine & _
                              " ) as Final " & Environment.NewLine & _
                              " left join TSPL_ITEM_MASTER Item on Final.Item_Code=Item.Item_Code " & Environment.NewLine & _
                              " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL) as Stock_SU on Final.Item_Code=Stock_SU.Item_Code and Final.Stock_UOM=Stock_SU.UOM_Code " & Environment.NewLine & _
                              " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL where UOM_Code='KG') as StockKG on Final.Item_Code=StockKG.Item_Code " & Environment.NewLine & _
                              " left join (select Item_QC.Item_Code,max(Item_QC.Actual_Range) as Fat_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & Environment.NewLine & _
                              " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='FAT' " & Environment.NewLine & _
                              " group by Item_QC.Item_Code) as Item_Fat on Final.Item_Code=Item_Fat.Item_Code " & Environment.NewLine & _
                              " left join (select  Item_QC.Item_Code,max(Item_QC.Actual_Range) as SNF_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & Environment.NewLine & _
                              " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='SNF' " & Environment.NewLine & _
                              " group by Item_QC.Item_Code) Item_SNF on Final.Item_Code=Item_SNF.Item_Code " & Environment.NewLine & _
                              " left join TSPL_LOCATION_MASTER Loc on Final.Location_Code=Loc.Location_Code where 2=2 " & Environment.NewLine & _
                              " AND ( COALESCE(Item_Fat.Fat_Per,0)<>0 OR COALESCE(Item_SNF.SNF_Per,0) <>0) " & Environment.NewLine & _
                              " group by Final.Location_Code; */ " & Environment.NewLine & _
                              " set @Msg =''; " & Environment.NewLine & _
                              " select @GIT_Type=coalesce(Git_Type,'N') from TSPL_LOCATION_MASTER where Location_Code=@Location_Code; " & Environment.NewLine & _
                              " if (@GIT_Type='Y') " & Environment.NewLine & _
                              " Return ''; " & Environment.NewLine & _
                              " SELECT @STOCK_FaT_KG= coalesce(sum(FAT_KG),0),@STOCK_SNF_KG=coalesce(sum(SNF_KG),0),@STOCK_QTY=coalesce(sum(CL_QTY),0),@Stock_UOM=COALESCE(MAX(Stock_UOM),'') FROM DBO.[TSPL_FUN_ITEM_LOC_BALANCE](@Item_Code,@Location_Code,@Trans_Date) " & Environment.NewLine & _
                              " set @STOCK_FaT_KG=(coalesce(@STOCK_FaT_KG,0)+@FAT_KG); " & Environment.NewLine & _
                              " set @STOCK_SNF_KG=(coalesce(@STOCK_SNF_KG,0)+@SNF_KG); " & Environment.NewLine & _
                              " set @STOCK_QTY=(coalesce(@STOCK_QTY,0)+@_QTY); " & Environment.NewLine & _
                              " if coalesce(@STOCK_QTY,0)<@_QTY and abs(coalesce(@STOCK_QTY,0)-@_QTY)>=0.01" & Environment.NewLine & _
                              " set @Msg=@Msg + 'Required Qty :' + cast(@_QTY as varchar) + ' Available Qty:' + cast(@STOCK_QTY as varchar) + ' ' + @Stock_UOM + CHAR(10); " & Environment.NewLine & _
                              " if coalesce(@STOCK_FaT_KG,0)<@FAT_KG and abs(coalesce(@STOCK_FaT_KG,0)-@FAT_KG)>=0.1" & Environment.NewLine & _
                              " set @Msg=@Msg + 'Required Fat KG :' + cast(@FAT_KG as varchar) + ' Available Fat KG:' + cast(@STOCK_FaT_KG as varchar) + CHAR(10); " & Environment.NewLine & _
                              " if coalesce(@STOCK_SNF_KG,0)<@SNF_KG and abs(coalesce(@STOCK_SNF_KG,0)-@SNF_KG) >=0.1 " & Environment.NewLine & _
                              " set @Msg=@Msg + 'Required SNF KG :' + cast(@SNF_KG  as varchar) + ' Available SNF KG:' + cast(@STOCK_SNF_KG as varchar) + CHAR(10); " & Environment.NewLine & _
                              " return @Msg; " & Environment.NewLine & _
                              " End "
                clsCommonFunctionality.CreateSQLFunctioin("Get_Location_FatSNF", strFunctionBody)

                '' Function Created by Panch Raj
            strFunctionBody = " Create function [dbo].[Check_Stock_OnReverse](@Item_Code  varchar(50),@Location_Code  varchar(50),@Trans_Date varchar(20),@FAT_KG Numeric(18,3),@SNF_KG AS Numeric(18,3),@_QTY AS Numeric(18,2)) returns varchar(max) as " & _
                              " begin " & _
                              " declare @Msg varchar(200); " & _
                              " declare @GIT_Type varchar(1); " & _
                              " select @GIT_Type=coalesce(Git_Type,'N') from TSPL_LOCATION_MASTER where Location_Code=@Location_Code; " & _
                              " if (@GIT_Type='Y') " & _
                              " Return ''; " & _
                              " set @Msg=(select (select top 1  (convert(varchar,Punching_Date,103) + ' Avail/Req Qty:'+ cast(round(CL_Qty+@_QTY,2) as varchar) + '/'  + cast(@_QTY as varchar) + ',Avail/Req FAT:'+ cast(round(CL_FAT_KG+@FAT_KG,2) as varchar) + '/'+ cast(@FAT_KG as varchar) + ',Avail/Req SNF:'+ cast(round(CL_SNF_KG+@SNF_KG,2) as varchar) + '/' + cast(@SNF_KG as varchar)) AS [text()]   from ( " & _
                              " select Item_Code,Location_Code,Punching_Date,sum(Stock_Qty) over (partition by Item_Code,Location_Code order by Item_Code,Location_Code,Punching_Date) as CL_Qty," & _
                              " sum(FAT_KG) over (partition by Item_Code,Location_Code order by Item_Code,Location_Code,Punching_Date) as CL_FAT_KG," & _
                              " sum(SNF_KG) over (partition by Item_Code,Location_Code order by Item_Code,Location_Code,Punching_Date) as CL_SNF_KG " & _
                              " from ( " & _
                              " select Item_Code,Location_Code,Punching_Date,sum((case when InOut='I' then 1 else -1 end)*Stock_Qty) as Stock_Qty," & _
                              " sum((case when InOut='I' then 1 else -1 end)*FAT_KG) as FAT_KG, " & _
                              " sum((case when InOut='I' then 1 else -1 end)*SNF_KG) as SNF_KG  from View_STOCK_DATA " & _
                              " where Item_Code=@Item_Code and Location_Code=@Location_Code " & _
                              " group by Item_Code,Location_Code,Punching_Date " & _
                              " ) as Stock " & _
                              " )as Final where Punching_Date>=@Trans_Date AND (ROUND(CL_Qty,2)<0 OR ROUND(CL_FAT_KG,2)<0 OR ROUND(CL_SNF_KG,2)<0)   order by Punching_Date) as All_Date) " & _
                              "  return COALESCE(@Msg,''); " & _
                              " End "
                clsCommonFunctionality.CreateSQLFunctioin("Check_Stock_OnReverse", strFunctionBody)

                strFunctionBody = " CREATE FUNCTION [dbo].[SPLIT_STRING] " & Environment.NewLine & _
                                    " (     " & Environment.NewLine & _
                                    " @INPUT NVARCHAR(MAX), " & Environment.NewLine & _
                                    " @CHARACTER CHAR(1) " & Environment.NewLine & _
                                    " ) " & Environment.NewLine & _
                                    " RETURNS @OUTPUT TABLE ( " & Environment.NewLine & _
                                    " ITEM NVARCHAR(1000) " & Environment.NewLine & _
                                    " ) " & Environment.NewLine & _
                                    " AS " & Environment.NewLine & _
                                    " BEGIN " & Environment.NewLine & _
                                    " DECLARE @STARTINDEX INT, @ENDINDEX INT " & Environment.NewLine & _
                                    " SET @STARTINDEX = 1 " & Environment.NewLine & _
                                    " IF SUBSTRING(@INPUT, LEN(@INPUT) - 1, LEN(@INPUT)) <> @CHARACTER " & Environment.NewLine & _
                                    " BEGIN " & Environment.NewLine & _
                                    " SET @INPUT = @INPUT + @CHARACTER " & Environment.NewLine & _
                                    " END " & Environment.NewLine & _
                                    " WHILE CHARINDEX(@CHARACTER, @INPUT) > 0 " & Environment.NewLine & _
                                    " BEGIN " & Environment.NewLine & _
                                    " SET @ENDINDEX = CHARINDEX(@CHARACTER, @INPUT) " & Environment.NewLine & _
                                    " INSERT INTO @OUTPUT(ITEM) " & Environment.NewLine & _
                                    " SELECT SUBSTRING(@INPUT, @STARTINDEX, @ENDINDEX - 1) " & Environment.NewLine & _
                                    " SET @INPUT = SUBSTRING(@INPUT, @ENDINDEX + 1, LEN(@INPUT)) " & Environment.NewLine & _
                                    " END " & Environment.NewLine & _
                                    " RETURN " & Environment.NewLine & _
                                    " END "
                clsCommonFunctionality.CreateSQLFunctioin("SPLIT_STRING", strFunctionBody)

                clsCommon.ProgressBarHide()
        Catch ex As Exception
            clsCommon.ProgressBarHide()
        End Try
    End Sub
End Class

Public Class clsAllSQLView
    Public Shared Sub CreateAllSQLView()
        Try

            clsCommon.ProgressBarShow()

            Dim strSQLViewBody As String = "  "
            '=========================================================
            strSQLViewBody = " SELECT     TEMP_PROVISIONAL_SALES.Item_Code, '' AS heading2, '' AS HierCode, TSPL_EMPLOYEE_MASTER_1.Emp_Name AS Hier_Desc, " & _
                      " TSPL_EMPLOYEE_MASTER_1.Emp_Name AS HierDesc, TEMP_PROVISIONAL_SALES.Item_Desc, TEMP_PROVISIONAL_SALES.Transfer_No, CONVERT(DECIMAL(18, " & _
                      " 2), CASE WHEN TEMP_PROVISIONAL_SALES.Unit_Code <> 'SH' THEN (TEMP_PROVISIONAL_SALES.LoadOutQty / TEMP_PROVISIONAL_SALES.Conversion_Factor - (ISNULL(TEMP_PROVISIONAL_SALES.LoadInQty " & _
                      " / TEMP_PROVISIONAL_SALES.Conversion_Factor, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Breakage, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Leak, 0) " & _
                      " + ISNULL(TEMP_PROVISIONAL_SALES.Shortage, 0))) * 1 ELSE 0 END) AS sale, TEMP_PROVISIONAL_SALES.RouteNo AS Route_No, CONVERT(date, " & _
                      " TEMP_PROVISIONAL_SALES.Transfer_Date, 103) AS Transfer_Date, TEMP_PROVISIONAL_SALES.Salesmancode, TSPL_EMPLOYEE_MASTER.Emp_Name, " & _
                      " TSPL_ROUTE_MASTER.Route_Desc, TEMP_PROVISIONAL_SALES.LoadOut_Location AS Location, TSPL_LOCATION_MASTER.Location_Desc, " & _
                      " TEMP_PROVISIONAL_SALES.Comp_Code, TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, CONVERT(DECIMAL(18, 2), " & _
                      " (TEMP_PROVISIONAL_SALES.Loadout_Amount - TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue) - (TEMP_PROVISIONAL_SALES.Amount - TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue)) AS Value, TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                      " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                      " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                      " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                      " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) END AS [Converted Qty], " & _
                      " CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                      " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty]," & _
                      " TEMP_PROVISIONAL_SALES.LoadOutQty, TEMP_PROVISIONAL_SALES.LoadInQty, TEMP_PROVISIONAL_SALES.Breakage, TEMP_PROVISIONAL_SALES.Leak, " & _
                      " TEMP_PROVISIONAL_SALES.MRP, TEMP_PROVISIONAL_SALES.Amount, TEMP_PROVISIONAL_SALES.Loadout_Amount, TEMP_PROVISIONAL_SALES.Shortage, " & _
                      " TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue, TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue " & _
                       " FROM         TSPL_ROUTE_MASTER RIGHT OUTER JOIN TSPL_ITEM_DETAILS RIGHT OUTER JOIN TSPL_COMPANY_MASTER RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN TEMP_PROVISIONAL_SALES ON TSPL_ITEM_UOM_DETAIL_2.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                      " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON TSPL_ITEM_UOM_DETAIL_1.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                      " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' LEFT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code ON " & _
                      " TSPL_COMPANY_MASTER.Comp_Code = TEMP_PROVISIONAL_SALES.Comp_Code LEFT OUTER JOIN " & _
                      " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 ON TEMP_PROVISIONAL_SALES.HOS = TSPL_EMPLOYEE_MASTER_1.EMP_CODE ON " & _
                      " TSPL_ITEM_DETAILS.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                      " TSPL_ITEM_DETAILS.Class_Code = TEMP_PROVISIONAL_SALES.Pack_Code LEFT OUTER JOIN " & _
                      " TSPL_EMPLOYEE_MASTER ON TEMP_PROVISIONAL_SALES.Salesmancode = TSPL_EMPLOYEE_MASTER.EMP_CODE ON " & _
                      " TSPL_ROUTE_MASTER.Route_No = TEMP_PROVISIONAL_SALES.RouteNo LEFT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER ON TEMP_PROVISIONAL_SALES.LoadOut_Location = TSPL_LOCATION_MASTER.Location_Code LEFT OUTER JOIN " & _
                      " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 ON TEMP_PROVISIONAL_SALES.Flavour_Code = TSPL_ITEM_DETAILS_1.Class_Code AND " & _
                     " TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_DETAILS_1.Item_Code LEFT OUTER JOIN " & _
                      " TSPL_ITEM_MASTER ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_MASTER.Item_Code LEFT OUTER JOIN " & _
                      " TSPL_ROUTE_TYPE ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type " & _
                       " WHERE     (TEMP_PROVISIONAL_SALES.RouteNo <> '') AND (TEMP_PROVISIONAL_SALES.Unit_Code <> 'sh') " & _
                    " UNION ALL SELECT     TSPL_SALE_INVOICE_DETAIL.Item_Code, '' AS heading2, TSPL_SALE_INVOICE_DETAIL.Level2_User_Code AS HierCode, " & _
                     " TSPL_EMPLOYEE_MASTER.Emp_Name AS Hier_Desc, TSPL_EMPLOYEE_MASTER.Emp_Name AS HierDesc, TSPL_ITEM_MASTER.Item_Desc, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No AS Transfer_no, CONVERT(decimal(18, 2), " & _
                      " TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / TSPL_ITEM_UOM_DETAIL.Conversion_Factor) * 1 AS sale, TSPL_SALE_INVOICE_HEAD.Route_No, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_Date AS transfer_date, TSPL_SALE_INVOICE_HEAD.Salesman_Code, TSPL_EMPLOYEE_MASTER_1.Emp_Name, " & _
                      " TSPL_ROUTE_MASTER.Route_Desc, TSPL_SALE_INVOICE_HEAD.Location, TSPL_LOCATION_MASTER.Location_Desc, TSPL_SALE_INVOICE_HEAD.Comp_Code, " & _
                     " TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, 0 AS value, dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) " & _
                     " THEN invoice_qty ELSE dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) " & _
                     " END AS [Converted Qty], CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                      " THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty], 0, 0, 0, 0, TSPL_SALE_INVOICE_DETAIL.MRP_Amt, " & _
                    " TSPL_SALE_INVOICE_DETAIL.Item_Net_Amt, 0, 0, 0, 0 FROM         TSPL_ITEM_UOM_DETAIL RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_MASTER RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_DETAILS INNER JOIN " & _
                      " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 INNER JOIN " & _
                      " TSPL_SALE_INVOICE_DETAIL ON TSPL_ITEM_DETAILS_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON " & _
                     " TSPL_ITEM_DETAILS.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON " & _
                     " TSPL_ITEM_UOM_DETAIL_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' ON " & _
                     " TSPL_ITEM_UOM_DETAIL_2.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON " & _
                     " TSPL_ITEM_MASTER.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON TSPL_ITEM_UOM_DETAIL.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL.UOM_Code = TSPL_SALE_INVOICE_DETAIL.Unit_code RIGHT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER RIGHT OUTER JOIN " & _
                      " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 RIGHT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER RIGHT OUTER JOIN " & _
                      " TSPL_SALE_INVOICE_HEAD ON TSPL_EMPLOYEE_MASTER.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Level2_User_code ON " & _
                     " TSPL_EMPLOYEE_MASTER_1.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Salesman_Code ON " & _
                     " TSPL_LOCATION_MASTER.Location_Code = TSPL_SALE_INVOICE_HEAD.Location ON " & _
                     " TSPL_SALE_INVOICE_DETAIL.Sale_Invoice_No = TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No FULL OUTER JOIN " & _
                     " TSPL_ROUTE_TYPE INNER JOIN " & _
                     " TSPL_COMPANY_MASTER ON TSPL_ROUTE_TYPE.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code RIGHT OUTER JOIN " & _
                     " TSPL_ROUTE_MASTER ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type ON " & _
                    " TSPL_SALE_INVOICE_HEAD.Route_No = TSPL_ROUTE_MASTER.Route_No And TSPL_SALE_INVOICE_HEAD.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code "
            clsCommonFunctionality.CreateSQLView("combinesaletemp", strSQLViewBody)
            '==============================================
            strSQLViewBody = " SELECT  a.Item_Code, a.UOM_Code, b.Conversion_Factor FROM dbo.TSPL_ITEM_UOM_DETAIL AS a INNER JOIN " & _
                             " dbo.TSPL_ITEM_UOM_DETAIL AS b ON a.Item_Code = b.Item_Code AND a.UOM_Code = 'FC' AND b.UOM_Code = 'FB'"
            clsCommonFunctionality.CreateSQLView("Conversion Code", strSQLViewBody)
            '======================================================
            strSQLViewBody = " SELECT     dbo.TSPL_ITEM_MASTER.Item_Code, dbo.TSPL_ITEM_MASTER.Item_Desc, dbo.TSPL_ITEM_MASTER.Unit_Code, dbo.TSPL_ITEM_MASTER.Server_Type, " & _
                      " dbo.TSPL_ITEM_MASTER.Batch_No, dbo.TSPL_ITEM_MASTER.Mfg_Date, dbo.TSPL_ITEM_MASTER.Best_Befor_UseDate, dbo.TSPL_ITEM_MASTER.Item_Type, " & _
                      " dbo.TSPL_ITEM_MASTER.Flavour_Seq, dbo.TSPL_ITEM_MASTER.Pack_Seq, dbo.TSPL_ITEM_MASTER.Sku_Seq, dbo.TSPL_ITEM_MASTER.Sub_item_category, " & _
                     " dbo.TSPL_ITEM_MASTER.TypeOfItm, dbo.TSPL_ITEM_DETAILS.Class_Desc AS [Packing Type], TSPL_ITEM_DETAILS_2.Class_Desc AS [Pack Size], " & _
                     " TSPL_ITEM_DETAILS_1.Class_Desc AS Flavour, TSPL_ITEM_DETAILS_3.Class_Desc AS 'Type', TSPL_ITEM_DETAILS_4.Class_Desc AS 'Pack Type' " & _
                      " FROM dbo.TSPL_ITEM_MASTER LEFT OUTER JOIN " & _
                      " dbo.TSPL_ITEM_DETAILS ON dbo.TSPL_ITEM_MASTER.Item_Code = dbo.TSPL_ITEM_DETAILS.Item_Code AND " & _
                     " dbo.TSPL_ITEM_DETAILS.Class_Name = 'Category' LEFT OUTER JOIN " & _
                      " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_2 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_2.Item_Code AND  " & _
                      " TSPL_ITEM_DETAILS_2.Class_Name = 'Size' LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_1.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS_1.Class_Name = 'Flavour' LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_3 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_3.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS_3.Class_Name = 'Pack Type' LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_4 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_4.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS_4.Class_Name = 'Pack'"
            clsCommonFunctionality.CreateSQLView("Item_Master", strSQLViewBody)
            '=================================================
            strSQLViewBody = " SELECT dbo.TSPL_SALE_INVOICE_HEAD.Cust_Code, dbo.TSPL_SALE_INVOICE_HEAD.Cust_Name, dbo.TSPL_CUSTOMER_MASTER.Channel_Code, " & _
                    "  dbo.TSPL_CUSTOMER_MASTER.Channel_Desc, dbo.TSPL_CHANNEL_CATEGORY_MASTER.Channel_category_Name, " & _
                    "  dbo.TSPL_CHANNEL_MASTER.Channel_Category, dbo.TSPL_CUSTOMER_MASTER.Cust_Type_Code, dbo.TSPL_ITEM_MASTER.Pack_Seq, " & _
                     " dbo.TSPL_ITEM_MASTER.Flavour_Seq, dbo.TSPL_ITEM_MASTER.Server_Type AS [Serve Type], dbo.TSPL_LOCATION_MASTER.Location_Desc, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Item_Code, dbo.TSPL_SALE_INVOICE_DETAIL.Item_Desc, dbo.TSPL_SALE_INVOICE_DETAIL.RAW_Qty, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Converted_Qty, dbo.TSPL_SALE_INVOICE_DETAIL.OZ_Qty, dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Balance_Qty, dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt AS MRP, dbo.TSPL_SALE_INVOICE_DETAIL.Basic_Rate, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Disc_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Item_Assessable_Rate, dbo.TSPL_SALE_INVOICE_DETAIL.Item_Net_Amt, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Item_Tax, dbo.TSPL_SALE_INVOICE_DETAIL.Total_Assessable_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Total_MRP_Amt, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Total_Basic_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Total_Disc_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Total_net_Amt, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Total_Tax_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Total_Item_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Empty_Value, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TPT, dbo.TSPL_SALE_INVOICE_DETAIL.Total_TPT, dbo.TSPL_SALE_INVOICE_DETAIL.Cust_Discount, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Total_Cust_Discount, dbo.TSPL_SALE_INVOICE_DETAIL.Level1_User_Code, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Level2_User_Code AS HOS, dbo.TSPL_SALE_INVOICE_DETAIL.Level3_User_Code AS TDM, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Level4_User_Code AS ADE, dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Target_Discount_Amt, dbo.TSPL_SALE_INVOICE_DETAIL.Sale_Account_Amount, dbo.TSPL_SALE_INVOICE_HEAD.Route_No, " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Route_Desc, dbo.TSPL_SALE_INVOICE_HEAD.Level5_User_code AS [CE/PSR], " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Salesman_Code AS [Route Agent], dbo.TSPL_SALE_INVOICE_HEAD.Vehicle_No, TSPL_ITEM_DETAILS_1.Class_Desc AS Flavour, " & _
                     " TSPL_ITEM_DETAILS_2.Class_Desc AS [Pack Size], dbo.TSPL_ITEM_DETAILS.Class_Desc AS [Packing Type], " & _
                     " dbo.TSPL_Discount_Master.Description AS [Discount Desc], " & _
                     " CASE WHEN dbo.TSPL_Discount_Master.Discount = 'Y' THEN 'D&A' WHEN dbo.TSPL_Discount_Master.VSND_Type = 'Y' THEN 'VSND' WHEN dbo.TSPL_Discount_Master.Other" & _
                     " = 'Y' THEN 'Other' WHEN dbo.TSPL_Discount_Master.Sampling = 'Y' THEN 'Sampling' ELSE '' END AS [Discount Hierarchy], " & _
                     " dbo.TSPL_Discount_Master.Discount_category_Code, dbo.TSPL_Discount_Master.skuwise AS [Discount SKU Wise], " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No, dbo.TSPL_SALE_INVOICE_HEAD.Sale_Invoice_Date, dbo.TSPL_LOCATION_MASTER.Location_Code, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Applicable, dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item, dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Code_Qty, " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Is_Post, dbo.TSPL_SALE_INVOICE_HEAD.Invoice_Type, dbo.TSPL_SALE_INVOICE_HEAD.is_Route_Jumped, " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Is_Scheduled, dbo.TSPL_EMPLOYEE_MASTER.Emp_Name AS Level1, TSPL_EMPLOYEE_MASTER_1.Emp_Name AS [HOS Name], " & _
                     " TSPL_EMPLOYEE_MASTER_2.Emp_Name AS [TDM Name], TSPL_EMPLOYEE_MASTER_3.Emp_Name AS [ADC Name], " & _
                     " TSPL_EMPLOYEE_MASTER_4.Emp_Name AS [CE Name], TSPL_EMPLOYEE_MASTER_5.Emp_Name AS [Salesman Name], dbo.visi.Customer_Id AS [Visi Customer], " & _
                     " dbo.TSPL_VEHICLE_MASTER.Transport_Id, dbo.TSPL_TRANSPORT_MASTER.Transporter_Name, dbo.TSPL_VEHICLE_MASTER.Type, " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Vehicle_Code, dbo.TSPL_ITEM_MASTER.item_category, dbo.TSPL_ITEM_MASTER.Sub_item_category, " & _
                     " dbo.TSPL_ITEM_MASTER.Sku_Seq, dbo.TSPL_ITEM_MASTER.Item_Type, dbo.TSPL_LOCATION_MASTER.Loc_Segment_Code, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TAX1_Amt * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty AS [Excise/Tax], " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TAX2_Amt * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty AS Ecess, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TAX3_Amt * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty AS SHEcess, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TAX4_Amt * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty AS [VAT/CST], " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.TAX5_Amt * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty AS [AD Tax], " & _
                     " dbo.TSPL_Item_Category.Category_Name AS [Item Category Name], dbo.TSPL_ITEM_SUB_CATEGORY.Description AS [Item Sub Category], " & _
                     " dbo.TSPL_CUSTOMER_TYPE_MASTER.Cust_Type_Desc, " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt * dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor - dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount1 AS [Trade Price], " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt * dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor AS [MRP Case], " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.UOM_Code = 'FC' THEN dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt / dbo.[Conversion Code].Conversion_Factor ELSE	 dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt " & _
                     "  END AS [MRP BTLS], CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Applicable = 'Y' OR " & _
                     " (dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Code_Qty <> '' AND dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item = 'N') " & _
                     " THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor ELSE 0 END AS Sale, " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item = 'Y' AND " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code = '' THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor * (dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt " & _
                     "  * dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor - (dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount1 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount2 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount3 " & _
                      " + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount4 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount5 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount6 +	dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount7 " & _
                      " + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount8 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount9)) ELSE 0 END AS [Trade Scheme], " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Cust_Discount <> 0 AND dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item <> 'Y' AND " & _
                     " (dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code = '' OR " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code IS NULL) " & _
                     " THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor ELSE 0 END AS [Cash Discount Qty], " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Cust_Discount <> 0 AND dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item <> 'Y' AND " & _
                     " (dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code = '' OR " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code IS NULL) THEN (dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty * dbo.TSPL_SALE_INVOICE_DETAIL.Cust_Discount) " & _
                     " / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor ELSE 0 END AS [Cash Discount Amount], CASE WHEN (dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount2 <> 0 OR " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount3 <> 0) AND  " & _
                      " dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item = 'N' THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor ELSE 0" & _
                      " END AS [Key Acct And MT Qty], CASE WHEN (dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount2 <> 0 OR " & _
                      " dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount3 <> 0) AND " & _
                      " dbo.TSPL_SALE_INVOICE_DETAIL.Scheme_Item = 'N' THEN (dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount2 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount3) " & _
                     " * dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor ELSE 0 END AS [Key Acct And MT Amount], " & _
                     " dbo.TSPL_Discount_Master.Account_Code, dbo.TSPL_Discount_Master.Account_Description, " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code <> '' THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor" & _
                      " ELSE 0 END AS [Discount Quantity], " & _
                      " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code <> '' THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor" & _
                      " * (dbo.TSPL_SALE_INVOICE_DETAIL.MRP_Amt * dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor - (dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount1 +dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount2 " & _
                       " +dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount3 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount4 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount5 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount6 " & _
                      " + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount7 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount8 + dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount9)) " & _
                      " ELSE 0 END AS [Discount Amount], dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor AS [Total Quantity], " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount5 <> 0 THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor" & _
                     "  ELSE 0 END AS [Agency Gross Qty], " & _
                      " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount5 <> 0 THEN (dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor)" & _
                       " * dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount5 ELSE 0 END AS [Agency Gross Amount], " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount4 <> 0 THEN dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor" & _
                     "  ELSE 0 END AS [Distributer Gross Qty], " & _
                     " CASE WHEN dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount4 <> 0 THEN (dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor)" & _
                     "  * dbo.TSPL_SALE_INVOICE_DETAIL.Price_Amount4 ELSE 0 END AS [Distributer Gross Amount], dbo.TSPL_SALE_INVOICE_HEAD.Shipment_Type, " & _
                     " dbo.TSPL_SHIPMENT_MASTER.Transfer_No AS [Load Out No], dbo.[Conversion Code].Conversion_Factor " & _
                     " FROM  dbo.TSPL_SHIPMENT_MASTER RIGHT OUTER JOIN " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD INNER JOIN " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL ON dbo.TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No = dbo.TSPL_SALE_INVOICE_DETAIL.Sale_Invoice_No INNER JOIN " & _
                     " dbo.TSPL_CUSTOMER_MASTER ON dbo.TSPL_SALE_INVOICE_HEAD.Cust_Code = dbo.TSPL_CUSTOMER_MASTER.Cust_Code INNER JOIN " & _
                     " dbo.TSPL_ITEM_MASTER ON dbo.TSPL_SALE_INVOICE_DETAIL.Item_Code = dbo.TSPL_ITEM_MASTER.Item_Code ON  " & _
                     " dbo.TSPL_SHIPMENT_MASTER.Shipment_No = dbo.TSPL_SALE_INVOICE_HEAD.Shipment_No LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_UOM_DETAIL LEFT OUTER JOIN " & _
                     " dbo.[Conversion Code] ON dbo.TSPL_ITEM_UOM_DETAIL.Item_Code = dbo.[Conversion Code].Item_Code AND " & _
                     " dbo.TSPL_ITEM_UOM_DETAIL.UOM_Code = dbo.[Conversion Code].UOM_Code ON " & _
                     "  dbo.TSPL_SALE_INVOICE_DETAIL.Item_Code = dbo.TSPL_ITEM_UOM_DETAIL.Item_Code And " & _
                     " dbo.TSPL_SALE_INVOICE_DETAIL.Unit_code = dbo.TSPL_ITEM_UOM_DETAIL.UOM_Code LEFT OUTER JOIN " & _
                     " dbo.TSPL_CUSTOMER_TYPE_MASTER ON " & _
                     " dbo.TSPL_CUSTOMER_MASTER.Cust_Type_Code = dbo.TSPL_CUSTOMER_TYPE_MASTER.Cust_Type_Code LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_SUB_CATEGORY ON dbo.TSPL_ITEM_MASTER.Sub_item_category = dbo.TSPL_ITEM_SUB_CATEGORY.Sub_Category_Code LEFT OUTER JOIN " & _
                     " dbo.TSPL_Item_Category ON dbo.TSPL_ITEM_MASTER.item_category = dbo.TSPL_Item_Category.Category_Code LEFT OUTER JOIN " & _
                     " dbo.TSPL_TRANSPORT_MASTER RIGHT OUTER JOIN " & _
                     " dbo.TSPL_VEHICLE_MASTER ON dbo.TSPL_TRANSPORT_MASTER.Transport_Id = dbo.TSPL_VEHICLE_MASTER.Transport_Id ON  " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Vehicle_Code = dbo.TSPL_VEHICLE_MASTER.Vehicle_Id LEFT OUTER JOIN " & _
                     " dbo.visi ON dbo.TSPL_SALE_INVOICE_HEAD.Cust_Code = dbo.visi.Customer_Id LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_5 ON " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Salesman_Code = TSPL_EMPLOYEE_MASTER_5.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_4 ON " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Level5_User_code = TSPL_EMPLOYEE_MASTER_4.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_3 ON " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Level4_User_code = TSPL_EMPLOYEE_MASTER_3.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_2 ON " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Level3_User_code = TSPL_EMPLOYEE_MASTER_2.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 ON " & _
                     " dbo.TSPL_SALE_INVOICE_HEAD.Level2_User_code = TSPL_EMPLOYEE_MASTER_1.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_EMPLOYEE_MASTER ON dbo.TSPL_SALE_INVOICE_HEAD.Level1_User_code = dbo.TSPL_EMPLOYEE_MASTER.EMP_CODE LEFT OUTER JOIN " & _
                     " dbo.TSPL_CHANNEL_MASTER ON dbo.TSPL_CUSTOMER_MASTER.Channel_Code = dbo.TSPL_CHANNEL_MASTER.Channel_Id LEFT OUTER JOIN " & _
                     " dbo.TSPL_CHANNEL_CATEGORY_MASTER ON " & _
                     " dbo.TSPL_CHANNEL_MASTER.Channel_Category = dbo.TSPL_CHANNEL_CATEGORY_MASTER.Channel_Category_Id LEFT OUTER JOIN " & _
                     " dbo.TSPL_Discount_Master ON dbo.TSPL_SALE_INVOICE_DETAIL.Discount_Code = dbo.TSPL_Discount_Master.Code LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS ON dbo.TSPL_ITEM_MASTER.Item_Code = dbo.TSPL_ITEM_DETAILS.Item_Code AND " & _
                     " dbo.TSPL_ITEM_DETAILS.Class_Name = 'Category' LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_2 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_2.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS_2.Class_Name = 'Size' LEFT OUTER JOIN " & _
                     " dbo.TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 ON dbo.TSPL_ITEM_MASTER.Item_Code = TSPL_ITEM_DETAILS_1.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS_1.Class_Name = 'Flavour' LEFT OUTER JOIN " & _
                     " dbo.TSPL_LOCATION_MASTER ON dbo.TSPL_SALE_INVOICE_HEAD.Location = dbo.TSPL_LOCATION_MASTER.Location_Code "
            clsCommonFunctionality.CreateSQLView("Sales Cube", strSQLViewBody)
            '=============================================
            strSQLViewBody = " SELECT     Cust_Code, Cust_Name, Channel_Code, Channel_Desc, Channel_category_Name, Channel_Category, Cust_Type_Code, Pack_Seq, Flavour_Seq, [Serve Type], " & _
                      " Location_Desc, Item_Code, Item_Desc, RAW_Qty, Converted_Qty, OZ_Qty, Invoice_Qty, MRP, Basic_Rate, Item_Net_Amt, Total_MRP_Amt, Total_Basic_Amt, " & _
                      " Total_Disc_Amt, Total_net_Amt, Total_Tax_Amt, Total_Item_Amt, Cust_Discount, Discount_Code, Route_No, Route_Desc, [Route Agent], Vehicle_No, Flavour, " & _
                      " [Pack Size], [Packing Type], [Discount Desc], [Discount Hierarchy], Sale_Invoice_No, Sale_Invoice_Date, Location_Code, Scheme_Applicable, Scheme_Item, " & _
                      " Scheme_Code_Qty, Is_Post, Invoice_Type, is_Route_Jumped, Is_Scheduled, [HOS Name], [TDM Name], [ADC Name], [CE Name], [Salesman Name], [Visi Customer], " & _
                      " Transport_Id, Transporter_Name, Type, Vehicle_Code, item_category, Sub_item_category, Sku_Seq, Item_Type, Loc_Segment_Code, [Excise/Tax], Ecess, SHEcess, " & _
                     "  [VAT/CST], [AD Tax], [Item Category Name], [Item Sub Category], Cust_Type_Desc, [Load Out No], Shipment_Type, [Trade Price], [MRP Case], [MRP BTLS], Sale, " & _
                     "  [Trade Scheme], [Cash Discount Qty], [Cash Discount Amount], [Key Acct And MT Qty], [Key Acct And MT Amount], Account_Code, Account_Description, " & _
                     " [Discount Quantity], [Discount Amount], [Total Quantity], [Agency Gross Qty], [Agency Gross Amount], [Distributer Gross Qty], [Distributer Gross Amount], " & _
                     " CASE WHEN [Agency Gross Amount] <> 0 OR " & _
                     " [Distributer Gross Qty] <> 0 OR " & _
                     " ([Discount Quantity] <> 0 AND [Discount Hierarchy] = 'VSND') THEN 'VSND' ELSE '' END AS [PDL VSND Discount], CASE WHEN Sale <> 0 OR " & _
                     " [Trade Scheme] <> 0 OR " & _
                     " [Cash Discount Qty] <> 0 OR " & _
                     " [Key Acct And MT Qty] <> 0 OR " & _
                     " ([Discount Quantity] <> 0 AND [Discount Hierarchy] = 'D&A') THEN 'D&A' ELSE '' END AS [PDL D&A Discount], CASE WHEN ([Discount Quantity] <> 0 AND  " & _
                     " [Discount Hierarchy] = 'Sampling') THEN 'Sampling' ELSE '' END AS [PDL Sampling Discount], CASE WHEN ([Discount Quantity] <> 0 AND  " & _
                     " [Discount Hierarchy] = 'Other') THEN 'Other' ELSE '' END AS [PDL Other Discount], Conversion_Factor,  " & _
                     " [Trade Scheme] / [Total Quantity] / [Trade Price] / Conversion_Factor AS [BS Scheme], " & _
                     " [Trade Scheme] + [Cash Discount Amount] + [Key Acct And MT Amount] + [Discount Amount] AS [Total Discount Amount]" & _
                     "  FROM         dbo.[Sales Cube]"
            clsCommonFunctionality.CreateSQLView("Sales_Details", strSQLViewBody)
            '============================================================
            strSQLViewBody = " SELECT     TEMP_PROVISIONAL_SALES.Item_Code, '' AS heading2, '' AS HierCode, TSPL_EMPLOYEE_MASTER_1.Emp_Name AS Hier_Desc, " & _
                     " TSPL_EMPLOYEE_MASTER_1.Emp_Name AS HierDesc, TEMP_PROVISIONAL_SALES.Item_Desc, TEMP_PROVISIONAL_SALES.Transfer_No, CONVERT(DECIMAL(18, " & _
                     " 2), CASE WHEN TEMP_PROVISIONAL_SALES.Unit_Code <> 'SH' THEN (TEMP_PROVISIONAL_SALES.LoadOutQty / TEMP_PROVISIONAL_SALES.Conversion_Factor - (ISNULL(TEMP_PROVISIONAL_SALES.LoadInQty " & _
                     "  / TEMP_PROVISIONAL_SALES.Conversion_Factor, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Breakage, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Leak, 0) " & _
                     " + ISNULL(TEMP_PROVISIONAL_SALES.Shortage, 0))) * 1 ELSE 0 END) AS sale, TEMP_PROVISIONAL_SALES.RouteNo AS Route_No, CONVERT(date, " & _
                     " TEMP_PROVISIONAL_SALES.Transfer_Date, 103) AS Transfer_Date, TEMP_PROVISIONAL_SALES.Salesmancode, TSPL_EMPLOYEE_MASTER.Emp_Name, " & _
                     " TSPL_ROUTE_MASTER.Route_Desc, TEMP_PROVISIONAL_SALES.LoadOut_Location AS Location, TSPL_LOCATION_MASTER.Location_Desc, " & _
                     " TEMP_PROVISIONAL_SALES.Comp_Code, TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, CONVERT(DECIMAL(18, 2), " & _
                     " (TEMP_PROVISIONAL_SALES.Loadout_Amount - TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue)" & _
                     " - (TEMP_PROVISIONAL_SALES.Amount - TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue)) AS Value, TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) END AS [Converted Qty], " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty], " & _
                     "   TEMP_PROVISIONAL_SALES.LoadOutQty, TEMP_PROVISIONAL_SALES.LoadInQty, TEMP_PROVISIONAL_SALES.Breakage, TEMP_PROVISIONAL_SALES.Leak," & _
            " TEMP_PROVISIONAL_SALES.MRP, TEMP_PROVISIONAL_SALES.Amount, TEMP_PROVISIONAL_SALES.Loadout_Amount, TEMP_PROVISIONAL_SALES.Shortage," & _
                   " TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue, TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue " & _
                     " FROM         TSPL_ROUTE_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_DETAILS RIGHT OUTER JOIN " & _
                     " TSPL_COMPANY_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN " & _
                     " TEMP_PROVISIONAL_SALES ON TSPL_ITEM_UOM_DETAIL_2.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON TSPL_ITEM_UOM_DETAIL_1.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code ON " & _
                     " TSPL_COMPANY_MASTER.Comp_Code = TEMP_PROVISIONAL_SALES.Comp_Code LEFT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 ON TEMP_PROVISIONAL_SALES.HOS = TSPL_EMPLOYEE_MASTER_1.EMP_CODE ON  " & _
                      " TSPL_ITEM_DETAILS.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS.Class_Code = TEMP_PROVISIONAL_SALES.Pack_Code LEFT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER ON TEMP_PROVISIONAL_SALES.Salesmancode = TSPL_EMPLOYEE_MASTER.EMP_CODE ON " & _
                     " TSPL_ROUTE_MASTER.Route_No = TEMP_PROVISIONAL_SALES.RouteNo LEFT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER ON TEMP_PROVISIONAL_SALES.LoadOut_Location = TSPL_LOCATION_MASTER.Location_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 ON TEMP_PROVISIONAL_SALES.Flavour_Code = TSPL_ITEM_DETAILS_1.Class_Code AND " & _
                     " TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_DETAILS_1.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_MASTER ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_MASTER.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ROUTE_TYPE ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type " & _
                     " WHERE     (TEMP_PROVISIONAL_SALES.RouteNo <> '') AND (TEMP_PROVISIONAL_SALES.Unit_Code <> 'sh') " & _
                    " UNION ALL " & _
                    "  SELECT     TSPL_SALE_INVOICE_DETAIL.Item_Code, '' AS heading2, TSPL_SALE_INVOICE_DETAIL.Level2_User_Code AS HierCode, " & _
                    "  TSPL_EMPLOYEE_MASTER.Emp_Name AS Hier_Desc, TSPL_EMPLOYEE_MASTER.Emp_Name AS HierDesc, TSPL_ITEM_MASTER.Item_Desc, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No AS Transfer_no, CONVERT(decimal(18, 2),  " & _
                    "  TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / TSPL_ITEM_UOM_DETAIL.Conversion_Factor) * 1 AS sale, TSPL_SALE_INVOICE_HEAD.Route_No, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_Date AS transfer_date, TSPL_SALE_INVOICE_HEAD.Salesman_Code, TSPL_EMPLOYEE_MASTER_1.Emp_Name, " & _
                     " TSPL_ROUTE_MASTER.Route_Desc, TSPL_SALE_INVOICE_HEAD.Location, TSPL_LOCATION_MASTER.Location_Desc, TSPL_SALE_INVOICE_HEAD.Comp_Code, " & _
                     " TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, 0 AS value, dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) " & _
                     " THEN invoice_qty ELSE dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) " & _
                     " END AS [Converted Qty], CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                     " THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty], 0, 0, 0, 0, TSPL_SALE_INVOICE_DETAIL.MRP_Amt, " & _
                    " TSPL_SALE_INVOICE_DETAIL.Item_Net_Amt, 0, 0, 0, 0 " & _
                     "  FROM         TSPL_ITEM_UOM_DETAIL RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_DETAILS INNER JOIN " & _
                     " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 INNER JOIN " & _
                     " TSPL_SALE_INVOICE_DETAIL ON TSPL_ITEM_DETAILS_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON " & _
                     " TSPL_ITEM_DETAILS.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON " & _
                     " TSPL_ITEM_UOM_DETAIL_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' ON " & _
                     " TSPL_ITEM_UOM_DETAIL_2.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON " & _
                     " TSPL_ITEM_MASTER.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON TSPL_ITEM_UOM_DETAIL.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL.UOM_Code = TSPL_SALE_INVOICE_DETAIL.Unit_code RIGHT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 RIGHT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_SALE_INVOICE_HEAD ON TSPL_EMPLOYEE_MASTER.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Level2_User_code ON " & _
                     " TSPL_EMPLOYEE_MASTER_1.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Salesman_Code ON " & _
                     " TSPL_LOCATION_MASTER.Location_Code = TSPL_SALE_INVOICE_HEAD.Location ON " & _
                     " TSPL_SALE_INVOICE_DETAIL.Sale_Invoice_No = TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No FULL OUTER JOIN " & _
                     " TSPL_ROUTE_TYPE INNER JOIN " & _
                     " TSPL_COMPANY_MASTER ON TSPL_ROUTE_TYPE.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code RIGHT OUTER JOIN " & _
                     " TSPL_ROUTE_MASTER ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type ON  " & _
                     " TSPL_SALE_INVOICE_HEAD.Route_No = TSPL_ROUTE_MASTER.Route_No And TSPL_SALE_INVOICE_HEAD.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code "
            clsCommonFunctionality.CreateSQLView("salsetemp", strSQLViewBody)
            '=============================================================

            strSQLViewBody = " SELECT T1.*,T2.DATE_FROM,T2.DATE_TO FROM (" & _
                             " SELECT T1.LVALLOTMENT_CODE AS TR_CODE,T1.PAY_PERIOD_CODE,COALESCE(T2.EMP_CODE,T1.EMP_CODE) AS EMP_CODE,T2.LEAVE_CODE,ALLOTMENT_DATE AS TR_DATE, " & _
                             " T2.ALLOTED_LEAVE AS ALLOTED,0 AS AVAILED,  'OB' AS TR_TYPE,'OPENING BALANCE' AS REMARKS FROM TSPL_LEAVE_ALLOTMENT T1 " & _
                             " INNER JOIN TSPL_LEAVE_ALLOTMENTDETAIL T2 ON T1.LVALLOTMENT_CODE=T2.LVALLOTMENT_CODE where T1.Document_Type='O' " & _
                             " UNION ALL " & _
                             " SELECT T1.LVALLOTMENT_CODE AS TR_CODE,T1.PAY_PERIOD_CODE,COALESCE(T2.EMP_CODE,T1.EMP_CODE) AS EMP_CODE,T2.LEAVE_CODE,NULL AS TR_DATE,T2.ALLOTED_LEAVE AS ALLOTED,0 AS AVAILED, " & _
                             " 'ALLOT' AS TR_TYPE,'ALLOTED' FROM TSPL_LEAVE_ALLOTMENT T1 INNER JOIN TSPL_LEAVE_ALLOTMENTDETAIL T2 ON T1.LVALLOTMENT_CODE=T2.LVALLOTMENT_CODE where T1.Document_Type='L' " & _
                             " UNION ALL " & _
                             " SELECT ADJ.LVADJUSTMENT_CODE AS TR_CODE,PAY_PERIOD_CODE,EMP_CODE,LEAVE_CODE,ADJUSTMENT_DATE,ADJUST_ALLOTED AS ALLOTED,ADJUST_AVAILED AS AVAILED, " & _
                             " (CASE WHEN ADJUST_AVAILED>0 THEN 'ADJ(+)' ELSE 'ADJ(-)' END) AS TR_TYPE,LEAVE_REASON FROM TSPL_LEAVE_ADJUSTMENT ADJ WHERE ADJUST_ALLOTED>0 " & _
                             " UNION ALL " & _
                             " select da.DLA_CODE TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,DAD.FIRST_HALF,DAD.ATTENDANCE_DATE,0 AS Alloted,(CASE WHEN DAD.FIRST_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') THEN 0.5 ELSE 0 END) as Availed,'AVAIL',DA.DESCRIPTION from TSPL_DAILY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_DAILY_ATTENDANCE DA on DAD.DLA_CODE=DA.DLA_CODE where DAD.FIRST_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') " & _
                             " union all " & _
                             " select da.DLA_CODE TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,DAD.SECOND_HALF,DAD.ATTENDANCE_DATE,0 AS Alloted,(CASE WHEN DAD.SECOND_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') THEN 0.5 ELSE 0 END) as Availed,'AVAIL',DA.DESCRIPTION from TSPL_DAILY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_DAILY_ATTENDANCE DA on DAD.DLA_CODE=DA.DLA_CODE where DAD.SECOND_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') " & _
                             " union all " & _
                             " select da.DLA_CODE TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,DAD.FIRST_HALF,DAD.ATTENDANCE_DATE,0 AS Alloted,(CASE WHEN DAD.FIRST_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') THEN 0.5 ELSE 0 END) as Availed,'AVAIL',DA.DESCRIPTION from TSPL_HOURLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_HOURLY_ATTENDANCE DA on DAD.DLA_CODE=DA.DLA_CODE where DAD.FIRST_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') " & _
                             " union all " & _
                             " select da.DLA_CODE TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,DAD.SECOND_HALF,DAD.ATTENDANCE_DATE,0 AS Alloted,(CASE WHEN DAD.SECOND_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') THEN 0.5 ELSE 0 END) as Availed,'AVAIL',DA.DESCRIPTION from TSPL_HOURLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_HOURLY_ATTENDANCE DA on DAD.DLA_CODE=DA.DLA_CODE where DAD.SECOND_HALF NOT IN ('A','P','H','WO','OD','T','NJ','SEP') " & _
                             " union all " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code) " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE='EL')>0 then (SELECT Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE='EL') else 'EL' end ) AS LEAVE_CODE ,PP.DATE_TO as TR_Date,0 AS Alloted,DAD.Earned_Leave as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Earned_Leave>0 " & _
                             " UNION ALL " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code) " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE='Maternity')>0 then (SELECT Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE='Maternity') else 'Maternity' end ) AS LEAVE_CODE ,PP.DATE_TO,0 AS Alloted,DAD.Maternity_Leave as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Maternity_Leave>0 " & _
                             " UNION ALL " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code) " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE='MED')>0 then (SELECT Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE='MED') else 'Medical' end ) AS LEAVE_CODE ,PP.DATE_TO,0 AS Alloted,DAD.Medical_Leave as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Medical_Leave>0 " & _
                             " UNION ALL " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code) " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE='CL')>0 then (SELECT Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE='CL') else 'CL' end ) AS LEAVE_CODE ,PP.DATE_TO,0 AS Alloted,DAD.Casual_Leave as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Casual_Leave>0 " & _
                             " UNION ALL " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code)  " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE='COFF')>0 then (SELECT Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE='COFF') else 'COFF' end ) AS LEAVE_CODE ,PP.DATE_TO,0 AS Alloted,DAD.Coff as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Coff>0 " & _
                             " UNION ALL " & _
                             " select DA.MTA_CODE AS TR_CODE,DA.PAY_PERIOD_CODE,DAD.EMP_CODE,(case when (SELECT count(Leave_Code) " & _
                             " from TSPL_LEAVE_MASTER where LEAVE_TYPE NOT IN ('EL','CL','Maternity','COFF','MED'))>0 then (SELECT  top 1 Leave_Code from TSPL_LEAVE_MASTER where LEAVE_TYPE not in ('EL','CL','Maternity','COFF','MED')) else 'Other' end ) AS LEAVE_CODE ,PP.DATE_TO,0 AS Alloted,DAD.Other_Leave as Availed,'AVAIL',DA.DESCRIPTION from TSPL_MONTHLY_ATTENDANCE_DETAIL DAD " & _
                             " inner join TSPL_MONTHLY_ATTENDANCE DA on DAD.MTA_CODE=DA.MTA_CODE " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER PP ON DA.PAY_PERIOD_CODE=PP.PAY_PERIOD_CODE WHERE DAD.Other_Leave>0 " & _
                             " ) AS T1  " & _
                             " INNER JOIN TSPL_PAYPERIOD_MASTER T2 ON T1.PAY_PERIOD_CODE=T2.PAY_PERIOD_CODE "
            clsCommonFunctionality.CreateSQLView("TSPL_VIEW_LEAVE_LEDGER", strSQLViewBody)
            '============================================================
            strSQLViewBody = " SELECT  [Transfer_No] as [Document No],[Transfer_Date] as [Document Date] ,[Item_Code] " & _
                            " ,[Route_No],[LoadOutCOnvertedQty]-isnull([LoadInConvertedQty ],0) as [ConvertedQty],[8Oz Qty]-isnull([loadin8oz],0) as [8Ozqty] " & _
                            " ,[RawQty]-isnull([loadinraw],0) as [Rawqty],[From_Location] as [Location],[To_Location],[Salesmancode] " & _
                            " ,[HOS],[TDM],[ADC],[CE],[Vehicle_Code],[Vehicle_No],[Post],[MRP],[Total_Item_Amt],[Empty_Value],[TPT_Value] " & _
                            " ,'N' as [Scheme],'' as [CustCode],'' as [RouteJump], 0 as [Total_net_Amt],0 as [liquid amt],0 as [Liquid Rate], " & _
                            " 'Transfer' as [Flag] FROM [ViewTransferDetails] " & _
                            " union all " & _
                            " SELECT  [Sale_Invoice_No],[Sale_Invoice_Date],[Item_Code],[Route_No] ,[Converted Qty],[8Oz Qty] " & _
                            " ,[RawQty],[Location],'' as [Tolocation] ,[Salesman_Code],[Level2_User_code],[Level3_User_code] " & _
                            " ,[Level4_User_code],[Level5_User_code],[Vehicle_Code],[Vehicle_No],[Is_Post] ,[MRP_Amt] " & _
                            " ,0 as [TotalItemAmt],0 as emptyVal,0 as TPT,[Scheme_Item] " & _
                            " ,[Cust_Code],[is_Route_Jumped]  ,[Total_net_Amt],[liquid amt],[Liquid Rate] " & _
                             " ,'Sale' as Flag FROM [ViewSaleCombine] "
            clsCommonFunctionality.CreateSQLView("View_LoadInLoadOut", strSQLViewBody)
            '========================================================
            strSQLViewBody = " SELECT     TOP (100) PERCENT I.Item_Code, I.Item_Desc, CONVERT(varchar(10), P.Start_Date, 103) AS Start_Date, P.UOM, P.Price_Code, P.Item_Basic_Net,i.Batch_No , P.Tax_group  , P.Item_Basic_Price, P.Empty_Value_Shell, P.Empty_Value_Bottle, I.Item_Type, I.show, I.Sku_Seq, P.TAX1_Rate,P.TAX2_Rate,P.TAX3_Rate,P.TAX4_Rate,P.TAX5_Rate,P.TAX6_Rate,P.TAX7_Rate,P.TAX8_Rate,P.TAX9_Rate,P.TAX10_Rate , P.TAX1_Amt ,P.TAX2_Amt,P.TAX3_Amt,P.TAX4_Amt,P.TAX5_Amt,P.TAX6_Amt,P.TAX7_Amt,P.TAX8_Amt,P.TAX9_Amt,P.TAX10_Amt " & _
                             " ,P.Abatement_Rate FROM dbo.TSPL_ITEM_PRICE_MASTER AS P INNER JOIN (SELECT     Item_Code, UOM, MAX(Start_Date) AS MaxDateTime, Item_Basic_Net,  Price_Code, Tax_group  FROM  dbo.TSPL_ITEM_PRICE_MASTER GROUP BY Item_Code, UOM, Item_Basic_Net,  Price_Code, Tax_group ) AS groupedP ON P.Item_Code = groupedP.Item_Code AND  P.Start_Date = groupedP.MaxDateTime AND P.UOM = groupedP.UOM AND P.Item_Basic_Net = groupedP.Item_Basic_Net  AND P.Price_Code = groupedP.Price_Code and P.Tax_group = groupedP.Tax_group   INNER JOIN dbo.TSPL_ITEM_MASTER AS I ON P.Item_Code = I.Item_Code ORDER BY I.Item_Code "
            clsCommonFunctionality.CreateSQLView("View_TSPL_SHIPMENT_ITEMS", strSQLViewBody)
            '===================================================
            strSQLViewBody = "SELECT     TOP (100) PERCENT I.Item_Code, I.Item_Desc, CONVERT(varchar(10), P.Start_Date, 103) AS Start_Date, P.UOM, P.Price_Code, P.Item_Basic_Net,i.Batch_No , P.Item_Basic_Price, P.Empty_Value_Shell, P.Empty_Value_Bottle, I.Item_Type, I.show, I.Sku_Seq, P.TAX1_Amt ,P.TAX2_Amt,P.TAX3_Amt,P.TAX4_Amt,P.TAX5_Amt,P.TAX6_Amt,P.TAX7_Amt,P.TAX8_Amt,P.TAX9_Amt,P.TAX10_Amt FROM dbo.TSPL_ITEM_PRICE_MASTER AS P INNER JOIN (SELECT     Item_Code, UOM, MAX(Start_Date) AS MaxDateTime, Item_Basic_Net, Item_Basic_Price, Price_Code FROM  dbo.TSPL_ITEM_PRICE_MASTER GROUP BY Item_Code, UOM, Item_Basic_Net, Item_Basic_Price, Price_Code) AS groupedP ON P.Item_Code = groupedP.Item_Code AND  P.Start_Date = groupedP.MaxDateTime AND P.UOM = groupedP.UOM AND P.Item_Basic_Net = groupedP.Item_Basic_Net AND P.Item_Basic_Price = groupedP.Item_Basic_Price AND P.Price_Code = groupedP.Price_Code INNER JOIN dbo.TSPL_ITEM_MASTER AS I ON P.Item_Code = I.Item_Code ORDER BY I.Item_Code "
            clsCommonFunctionality.CreateSQLView("View_TSPL_SHIPMENT_ITEMS123", strSQLViewBody)
            '=======================
            strSQLViewBody = " SELECT TEMP_PROVISIONAL_SALES.Item_Code, '' AS heading2, '' AS HierCode, TSPL_EMPLOYEE_MASTER_1.Emp_Name AS Hier_Desc, " & _
                     " TSPL_EMPLOYEE_MASTER_1.Emp_Name AS HierDesc, TEMP_PROVISIONAL_SALES.Item_Desc, TEMP_PROVISIONAL_SALES.Transfer_No, CONVERT(DECIMAL(18, " & _
                     " 2), CASE WHEN TEMP_PROVISIONAL_SALES.Unit_Code <> 'SH' THEN (TEMP_PROVISIONAL_SALES.LoadOutQty / TEMP_PROVISIONAL_SALES.Conversion_Factor - (ISNULL(TEMP_PROVISIONAL_SALES.LoadInQty / TEMP_PROVISIONAL_SALES.Conversion_Factor, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Breakage, 0) + ISNULL											(TEMP_PROVISIONAL_SALES.Leak, 0) + ISNULL(TEMP_PROVISIONAL_SALES.Shortage, 0))) * 1 ELSE 0 END) AS sale, TEMP_PROVISIONAL_SALES.RouteNo AS Route_No, CONVERT					(date, TEMP_PROVISIONAL_SALES.Transfer_Date, 103) AS Transfer_Date, TEMP_PROVISIONAL_SALES.Salesmancode, TSPL_EMPLOYEE_MASTER.Emp_Name, " & _
                     " TSPL_ROUTE_MASTER.Route_Desc, TEMP_PROVISIONAL_SALES.LoadOut_Location AS Location, TSPL_LOCATION_MASTER.Location_Desc, " & _
                     " TEMP_PROVISIONAL_SALES.Comp_Code, TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, CONVERT(DECIMAL(18, 2), " & _
                     " (TEMP_PROVISIONAL_SALES.Loadout_Amount - TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue) " & _
                     " - (TEMP_PROVISIONAL_SALES.Amount - TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue)) AS Value, TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) " & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage)" & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) END AS [Converted Qty]," & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL THEN LoadOutQty - (LoadInQty + Breakage + Leak + Shortage)" & _
                     " ELSE LoadOutQty - (LoadInQty + Breakage + Leak + Shortage) / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty], " & _
                     " TEMP_PROVISIONAL_SALES.LoadOutQty, TEMP_PROVISIONAL_SALES.LoadInQty, TEMP_PROVISIONAL_SALES.Breakage, TEMP_PROVISIONAL_SALES.Leak, " & _
                     " TEMP_PROVISIONAL_SALES.MRP, TEMP_PROVISIONAL_SALES.Amount, TEMP_PROVISIONAL_SALES.Loadout_Amount, TEMP_PROVISIONAL_SALES.Shortage, " & _
                     " TEMP_PROVISIONAL_SALES.LoadOut_EmptyValue, TEMP_PROVISIONAL_SALES.LoadIn_EmptyValue, TEMP_PROVISIONAL_SALES.HOS, " & _
                     " TEMP_PROVISIONAL_SALES.TDM, TEMP_PROVISIONAL_SALES.ADC, TEMP_PROVISIONAL_SALES.CE,'' as Scheme_Item,'' as Cust_Code, " & _
                     " '' as [Is Post], '' as [Is Route Jump] " & _
                        " FROM  TSPL_ROUTE_MASTER RIGHT OUTER JOIN " & _
                      " TSPL_ITEM_DETAILS RIGHT OUTER JOIN " & _
                      " TSPL_COMPANY_MASTER RIGHT OUTER JOIN    " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN " & _
                     " TEMP_PROVISIONAL_SALES ON TSPL_ITEM_UOM_DETAIL_2.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON TSPL_ITEM_UOM_DETAIL_1.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code ON " & _
                     " TSPL_COMPANY_MASTER.Comp_Code = TEMP_PROVISIONAL_SALES.Comp_Code LEFT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 ON TEMP_PROVISIONAL_SALES.HOS = TSPL_EMPLOYEE_MASTER_1.EMP_CODE ON " & _
                     " TSPL_ITEM_DETAILS.Item_Code = TEMP_PROVISIONAL_SALES.Item_Code AND " & _
                     " TSPL_ITEM_DETAILS.Class_Code = TEMP_PROVISIONAL_SALES.Pack_Code LEFT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER ON TEMP_PROVISIONAL_SALES.Salesmancode = TSPL_EMPLOYEE_MASTER.EMP_CODE ON " & _
                     " TSPL_ROUTE_MASTER.Route_No = TEMP_PROVISIONAL_SALES.RouteNo LEFT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER ON TEMP_PROVISIONAL_SALES.LoadOut_Location = TSPL_LOCATION_MASTER.Location_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 ON TEMP_PROVISIONAL_SALES.Flavour_Code = TSPL_ITEM_DETAILS_1.Class_Code AND " & _
                     " TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_DETAILS_1.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_MASTER ON TEMP_PROVISIONAL_SALES.Item_Code = TSPL_ITEM_MASTER.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ROUTE_TYPE ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type " & _
                    " WHERE     (TEMP_PROVISIONAL_SALES.RouteNo <> '') AND (TEMP_PROVISIONAL_SALES.Unit_Code <> 'sh') " & _
                    " Union All " & _
                     " SELECT     TSPL_SALE_INVOICE_DETAIL.Item_Code, '' AS heading2, TSPL_SALE_INVOICE_DETAIL.Level2_User_Code AS HierCode,  " & _
                     " TSPL_EMPLOYEE_MASTER.Emp_Name AS Hier_Desc, TSPL_EMPLOYEE_MASTER.Emp_Name AS HierDesc, TSPL_ITEM_MASTER.Item_Desc, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No AS Transfer_no, CONVERT(decimal(18, 2), " & _
                     " TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / TSPL_ITEM_UOM_DETAIL.Conversion_Factor) * 1 AS sale, TSPL_SALE_INVOICE_HEAD.Route_No, " & _
                     " TSPL_SALE_INVOICE_HEAD.Sale_Invoice_Date AS transfer_date, TSPL_SALE_INVOICE_HEAD.Salesman_Code, TSPL_EMPLOYEE_MASTER_1.Emp_Name, " & _
                     " TSPL_ROUTE_MASTER.Route_Desc, TSPL_SALE_INVOICE_HEAD.Location, TSPL_LOCATION_MASTER.Location_Desc, TSPL_SALE_INVOICE_HEAD.Comp_Code, " & _
                     " TSPL_COMPANY_MASTER.Comp_Name, 'Raw' AS Convertion, 0 AS value, TSPL_ITEM_UOM_DETAIL.Conversion_Factor, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) " & _
                     " THEN invoice_qty ELSE dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor) " & _
                     " END AS [Converted Qty], CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                     " THEN invoice_qty ELSE invoice_qty / (TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor) END AS [8Oz Qty], 0 AS Expr1, 0 AS Expr2, 0 AS Expr3, 0 AS Expr4, " & _
                     " TSPL_SALE_INVOICE_DETAIL.MRP_Amt, TSPL_SALE_INVOICE_DETAIL.Item_Net_Amt, 0 AS Expr5, 0 AS Expr6, 0 AS Expr7, 0 AS Expr8, " & _
                     " TSPL_SALE_INVOICE_HEAD.Level2_User_code, TSPL_SALE_INVOICE_HEAD.Level3_User_code, TSPL_SALE_INVOICE_HEAD.Level4_User_code, " & _
                     " TSPL_SALE_INVOICE_HEAD.Level5_User_code, TSPL_SALE_INVOICE_DETAIL.Scheme_Item, TSPL_SALE_INVOICE_HEAD.Cust_Code, " & _
                     " TSPL_SALE_INVOICE_HEAD.Is_Post, TSPL_SALE_INVOICE_HEAD.is_Route_Jumped " & _
                    "	FROM         TSPL_ITEM_UOM_DETAIL RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 RIGHT OUTER JOIN " & _
                     " TSPL_ITEM_DETAILS INNER JOIN " & _
                     " TSPL_ITEM_DETAILS AS TSPL_ITEM_DETAILS_1 INNER JOIN " & _
                     " TSPL_SALE_INVOICE_DETAIL ON TSPL_ITEM_DETAILS_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON " & _
                     " TSPL_ITEM_DETAILS.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON  " & _
                     " TSPL_ITEM_UOM_DETAIL_1.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' ON " & _
                     " TSPL_ITEM_UOM_DETAIL_2.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' ON " & _
                     " TSPL_ITEM_MASTER.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code ON TSPL_ITEM_UOM_DETAIL.Item_Code = TSPL_SALE_INVOICE_DETAIL.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL.UOM_Code = TSPL_SALE_INVOICE_DETAIL.Unit_code RIGHT OUTER JOIN " & _
                     " TSPL_LOCATION_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER AS TSPL_EMPLOYEE_MASTER_1 RIGHT OUTER JOIN " & _
                     " TSPL_EMPLOYEE_MASTER RIGHT OUTER JOIN " & _
                     " TSPL_SALE_INVOICE_HEAD ON TSPL_EMPLOYEE_MASTER.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Level2_User_code ON  " & _
                     " TSPL_EMPLOYEE_MASTER_1.EMP_CODE = TSPL_SALE_INVOICE_HEAD.Salesman_Code ON  " & _
                     " TSPL_LOCATION_MASTER.Location_Code = TSPL_SALE_INVOICE_HEAD.Location ON  " & _
                     " TSPL_SALE_INVOICE_DETAIL.Sale_Invoice_No = TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No FULL OUTER JOIN " & _
                     " TSPL_ROUTE_TYPE INNER JOIN " & _
                     " TSPL_COMPANY_MASTER ON TSPL_ROUTE_TYPE.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code RIGHT OUTER JOIN " & _
                      " TSPL_ROUTE_MASTER ON TSPL_ROUTE_TYPE.Route_Type_Id = TSPL_ROUTE_MASTER.Type ON " & _
                    " TSPL_SALE_INVOICE_HEAD.Route_No = TSPL_ROUTE_MASTER.Route_No And TSPL_SALE_INVOICE_HEAD.Comp_Code = TSPL_COMPANY_MASTER.Comp_Code "
            clsCommonFunctionality.CreateSQLView("viewCombineSale_temp", strSQLViewBody)
            '=================================================================================
            strSQLViewBody = " SELECT  TSPL_TRANSFER_HEAD.Load_Out_No, TSPL_TRANSFER_DETAIL.Item_Code, TSPL_TRANSFER_DETAIL.LoadIn_Qty, TSPL_TRANSFER_DETAIL.Burst, " & _
                      " TSPL_TRANSFER_DETAIL.Leak, TSPL_TRANSFER_DETAIL.Shortage, TSPL_TRANSFER_DETAIL.TPT_Value, " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) THEN (LoadIn_Qty + Leak + Burst + Shortage) ELSE (LoadIn_Qty + Leak + Burst + Shortage) " & _
                      " / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL " & _
                     " THEN (LoadIn_Qty + Leak + Burst + Shortage) " & _
                     " ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor / TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * ((LoadIn_Qty + Leak + Burst + Shortage) " & _
                     " / (TSPL_ITEM_UOM_DETAIL.Conversion_Factor)) END AS [Converted Qty], CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                     " THEN (LoadIn_Qty + Leak + Burst + Shortage) " & _
                     " ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor / TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * ((LoadIn_Qty + Leak + Burst + Shortage) " & _
                     " / (TSPL_ITEM_UOM_DETAIL.Conversion_Factor)) * TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor END AS [8Oz Qty], TSPL_TRANSFER_DETAIL.Amount, " & _
                     " TSPL_TRANSFER_DETAIL.Empty_Value, TSPL_TRANSFER_DETAIL.MRP*TSPL_ITEM_UOM_DETAIL.Conversion_Factor as MRP,TSPL_TRANSFER_DETAIL.Uom " & _
                    "	FROM  TSPL_TRANSFER_HEAD INNER JOIN " & _
                     " TSPL_TRANSFER_DETAIL ON TSPL_TRANSFER_HEAD.Transfer_No = TSPL_TRANSFER_DETAIL.Transfer_No LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_3 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_3.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_3.UOM_Code = 'FB' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL ON TSPL_TRANSFER_DETAIL.Uom = TSPL_ITEM_UOM_DETAIL.UOM_Code AND  " & _
                     " TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_1.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_2.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8Oz' " & _
                    " WHERE (TSPL_TRANSFER_HEAD.Transfer_Type = 'LI') AND (TSPL_TRANSFER_HEAD.Route_No <> '')"
            clsCommonFunctionality.CreateSQLView("VIewLOadIN", strSQLViewBody)
            '===================================================================================================
            strSQLViewBody = " SELECT SUM(Burst) AS Brust, SUM(Leak) AS Leak, SUM(Shortage) AS Shortage, SUM(RawQty) AS RawQty, SUM([Converted Qty]) AS [Converted Qty], SUM([8Oz Qty]) " & _
                                " AS [8Oz Qty], SUM(Amount) AS Amount, SUM(Empty_Value) AS Empty_Value, Load_Out_No, Item_Code, SUM(LoadIn_Qty) AS LoadIn_Qty, MRP FROM    VIewLOadIN " & _
                                " GROUP BY Load_Out_No, Item_Code, MRP"
            clsCommonFunctionality.CreateSQLView("ViewLoadINCOnverted", strSQLViewBody)
            '=====================================================

            strSQLViewBody = " SELECT TSPL_TRANSFER_HEAD.Transfer_No, TSPL_TRANSFER_HEAD.Transfer_Date, TSPL_TRANSFER_HEAD.Posting_Date, TSPL_TRANSFER_HEAD.Transfer_Type, " & _
                      " TSPL_TRANSFER_HEAD.Load_Out_No, TSPL_TRANSFER_HEAD.From_Location, TSPL_TRANSFER_HEAD.To_Location, TSPL_TRANSFER_HEAD.Route_No,  " & _
                     " TSPL_TRANSFER_HEAD.Salesmancode, TSPL_TRANSFER_HEAD.Price_Code, TSPL_TRANSFER_HEAD.Vehicle_Code, TSPL_TRANSFER_HEAD.Vehicle_No, " & _
                     " TSPL_TRANSFER_HEAD.Post, TSPL_TRANSFER_HEAD.HOS, TSPL_TRANSFER_HEAD.TDM, TSPL_TRANSFER_HEAD.ADC, TSPL_TRANSFER_HEAD.CE, " & _
                     " TSPL_TRANSFER_HEAD.Item_Type, TSPL_TRANSFER_HEAD.FromLoc_Desc, TSPL_TRANSFER_HEAD.ToLoc_Desc, TSPL_TRANSFER_HEAD.Route_Desc, " & _
                     " TSPL_TRANSFER_HEAD.Price_Desc, TSPL_TRANSFER_HEAD.Vehicle_Desc, TSPL_TRANSFER_DETAIL.Item_Code, TSPL_TRANSFER_DETAIL.Item_Desc, " & _
                     " TSPL_TRANSFER_DETAIL.Price_Date, TSPL_TRANSFER_DETAIL.Item_Qty, TSPL_TRANSFER_DETAIL.MRP, TSPL_TRANSFER_DETAIL.Item_Price, " & _
                     " TSPL_TRANSFER_DETAIL.Amount, TSPL_TRANSFER_DETAIL.Pending_Qty, TSPL_TRANSFER_DETAIL.Net_Amount, TSPL_TRANSFER_DETAIL.Total_Tax, " & _
                     " TSPL_TRANSFER_DETAIL.Total_Item_Amt, TSPL_TRANSFER_DETAIL.LoadIn_Qty, TSPL_TRANSFER_DETAIL.Uom, TSPL_TRANSFER_DETAIL.Burst, " & _
                     " TSPL_TRANSFER_DETAIL.Leak, TSPL_TRANSFER_DETAIL.Shortage, TSPL_TRANSFER_DETAIL.TPT_Value, TSPL_TRANSFER_DETAIL.Empty_Value, " & _
                     " TSPL_TRANSFER_DETAIL.BasicPrice_WithTax, TSPL_TRANSFER_DETAIL.Batch_No, TSPL_TRANSFER_DETAIL.Total_Item_Cost, " & _
                     " TSPL_TRANSFER_DETAIL.MRP_In_Bottle, TSPL_TRANSFER_DETAIL.Total_QtyInCase," & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) " & _
                     " THEN Item_Qty ELSE dbo.TSPL_TRANSFER_DETAIL.Item_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty " & _
                     " , CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN Item_Qty ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor / TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * (dbo.TSPL_TRANSFER_DETAIL.Item_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor) " & _
                     " END AS [Converted Qty]," & _
                    " CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                     " THEN Item_Qty ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor /TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * (dbo.TSPL_TRANSFER_DETAIL.Item_Qty /dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor)  * TSPL_ITEM_UOM_DETAIL_2 .Conversion_Factor END AS [8Oz Qty] " & _
                      " FROM         TSPL_TRANSFER_HEAD INNER JOIN " & _
                     " TSPL_TRANSFER_DETAIL ON TSPL_TRANSFER_HEAD.Transfer_No = TSPL_TRANSFER_DETAIL.Transfer_No LEFT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_3 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_3.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL_3.UOM_Code = 'FB' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL ON TSPL_TRANSFER_DETAIL.Uom = TSPL_ITEM_UOM_DETAIL.UOM_Code AND " & _
                     " TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_1.Item_Code AND " & _
                     " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 ON TSPL_TRANSFER_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_2.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8Oz' " & _
                    " WHERE  (TSPL_TRANSFER_HEAD.Transfer_Type = 'LO') AND (TSPL_TRANSFER_HEAD.Route_No <> '') "
            clsCommonFunctionality.CreateSQLView("VIewLOadOut", strSQLViewBody)
            '==========================================
            strSQLViewBody = " SELECT TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No, TSPL_SALE_INVOICE_HEAD.Sale_Invoice_Date, TSPL_SALE_INVOICE_HEAD.Shipment_Type, " & _
                     " TSPL_SALE_INVOICE_HEAD.is_Route_Jumped, TSPL_SALE_INVOICE_HEAD.Cust_Code, TSPL_SALE_INVOICE_HEAD.Location, " & _
                    "  TSPL_SALE_INVOICE_HEAD.Salesman_Code, TSPL_SALE_INVOICE_HEAD.Mode_Of_Transport, TSPL_SALE_INVOICE_HEAD.Vehicle_Code, " & _
                     " TSPL_SALE_INVOICE_HEAD.Vehicle_No, TSPL_SALE_INVOICE_HEAD.Route_No, TSPL_SALE_INVOICE_HEAD.Level1_User_code, " & _
                    "  TSPL_SALE_INVOICE_HEAD.Level2_User_code, TSPL_SALE_INVOICE_HEAD.Level3_User_code, TSPL_SALE_INVOICE_HEAD.Level4_User_code, " & _
                    "  TSPL_SALE_INVOICE_HEAD.Level5_User_code, TSPL_SALE_INVOICE_HEAD.Is_Post, TSPL_SALE_INVOICE_DETAIL.Item_Code, " & _
                    "  TSPL_SALE_INVOICE_DETAIL.Invoice_Qty, TSPL_SALE_INVOICE_DETAIL.Unit_code, TSPL_SALE_INVOICE_DETAIL.MRP_Amt, " & _
                    "  TSPL_SALE_INVOICE_DETAIL.Scheme_Item, TSPL_SALE_INVOICE_DETAIL.Basic_Rate, TSPL_SALE_INVOICE_DETAIL.Total_net_Amt, " & _
                    "  TSPL_SALE_INVOICE_DETAIL.Total_Item_Amt AS [liquid amt], TSPL_SALE_INVOICE_DETAIL.Price_To_Show AS [Liquid Rate], " & _
                     " CASE WHEN dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor IN (0, NULL) " & _
                    "  THEN Invoice_Qty ELSE dbo.TSPL_SALE_INVOICE_DETAIL.Invoice_Qty  / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor END AS RawQty, " & _
                    "  CASE WHEN TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor IS NULL THEN Invoice_Qty ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor /TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * (Invoice_Qty / dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor) " & _
                     " END AS [Converted Qty]," & _
                     " CASE WHEN TSPL_ITEM_UOM_DETAIL_2.Conversion_Factor IS NULL " & _
                    "  THEN Invoice_Qty ELSE TSPL_ITEM_UOM_DETAIL_3.Conversion_Factor /TSPL_ITEM_UOM_DETAIL_1.Conversion_Factor * (Invoice_Qty /	dbo.TSPL_ITEM_UOM_DETAIL.Conversion_Factor)  * TSPL_ITEM_UOM_DETAIL_2 .Conversion_Factor END AS [8Oz Qty]" & _
                    " FROM  TSPL_SALE_INVOICE_DETAIL INNER JOIN " & _
                    "  TSPL_SALE_INVOICE_HEAD ON TSPL_SALE_INVOICE_DETAIL.Sale_Invoice_No = TSPL_SALE_INVOICE_HEAD.Sale_Invoice_No INNER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL ON TSPL_SALE_INVOICE_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL.Item_Code AND  " & _
                     " TSPL_SALE_INVOICE_DETAIL.Unit_code = TSPL_ITEM_UOM_DETAIL.UOM_Code LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_3 ON TSPL_SALE_INVOICE_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_3.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_3.UOM_Code = 'FB' LEFT OUTER JOIN " & _
                      " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_2 ON TSPL_SALE_INVOICE_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_2.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_2.UOM_Code = '8oz' LEFT OUTER JOIN " & _
                     " TSPL_ITEM_UOM_DETAIL AS TSPL_ITEM_UOM_DETAIL_1 ON TSPL_SALE_INVOICE_DETAIL.Item_Code = TSPL_ITEM_UOM_DETAIL_1.Item_Code AND  " & _
                     " TSPL_ITEM_UOM_DETAIL_1.UOM_Code = 'Con' " & _
                    " WHERE     (TSPL_SALE_INVOICE_HEAD.Shipment_Type = 'Sale')"
            clsCommonFunctionality.CreateSQLView("ViewSaleCombine", strSQLViewBody)
            '==================================================================================
            strSQLViewBody = " SELECT VIewLOadOut.Transfer_No, VIewLOadOut.Transfer_Date, ViewLoadINCOnverted.Load_Out_No, VIewLOadOut.Item_Code, VIewLOadOut.Item_Qty, " & _
                      " ViewLoadINCOnverted.LoadIn_Qty, ViewLoadINCOnverted.[Converted Qty] AS [LoadInConvertedQty ], VIewLOadOut.[Converted Qty] AS LoadOutCOnvertedQty, " & _
                     " VIewLOadOut.[8Oz Qty], VIewLOadOut.RawQty, ViewLoadINCOnverted.[8Oz Qty] AS loadin8oz, ViewLoadINCOnverted.RawQty AS loadinraw, " & _
           " VIewLOadOut.From_Location, VIewLOadOut.To_Location, VIewLOadOut.Route_No, VIewLOadOut.Salesmancode, VIewLOadOut.HOS, VIewLOadOut.TDM," & _
           " VIewLOadOut.ADC, VIewLOadOut.CE, VIewLOadOut.Vehicle_Code, VIewLOadOut.Vehicle_No, VIewLOadOut.Post, VIewLOadOut.MRP, VIewLOadOut.Total_Item_Amt, " & _
           " VIewLOadOut.Empty_Value, VIewLOadOut.TPT_Value " & _
            " FROM  VIewLOadOut LEFT OUTER JOIN ViewLoadINCOnverted ON VIewLOadOut.MRP = ViewLoadINCOnverted.MRP AND VIewLOadOut.Transfer_No =	ViewLoadINCOnverted.Load_Out_No AND " & _
            " VIewLOadOut.Item_Code = ViewLoadINCOnverted.Item_Code"
            clsCommonFunctionality.CreateSQLView("ViewTransferDetails", strSQLViewBody)
            '=============================================================
            strSQLViewBody = " Select Customer_Id FROM dbo.TSPL_VISI_MASTER GROUP BY Customer_Id"
            clsCommonFunctionality.CreateSQLView("visi", strSQLViewBody)

            '' Create view for Stock In GIT
            '   strSQLViewBody = clsInventoryMovement.GetBaseQuery("", True)
            'clsCommonFunctionality.CreateSQLView("View_STOCK_DATA_GIT", strSQLViewBody)

            '' Create view for Stock In GIT
            '  strSQLViewBody = clsInventoryMovement.GetBaseQuery("", False)
            ' clsCommonFunctionality.CreateSQLView("View_STOCK_DATA", strSQLViewBody)

            ' '' create view for customer git and actual            
            'strSQLViewBody = clsCustomerMaster.GetCustomerBaseQry(False, False, "", False, "ConvRate", False, False, True, True)
            'clsCommonFunctionality.CreateSQLView("View_CUSTOMER_DATA_GIT", strSQLViewBody)

            ' '' create view for customer git and actual            
            'strSQLViewBody = clsCustomerMaster.GetCustomerBaseQry(False, False, "", False, "ConvRate", False, False, True)
            'clsCommonFunctionality.CreateSQLView("View_CUSTOMER_DATA", strSQLViewBody)

            ' '' create view for customer git and actual            
            'strSQLViewBody = clsCustomerMaster.GetCustomerBaseQryforCustomerCurrency(False, False, "", False, "1", False, False, True, True)
            'clsCommonFunctionality.CreateSQLView("View_CUSTOMER_DATA_Currency_GIT", strSQLViewBody)

            ' '' create view for customer git and actual            
            'strSQLViewBody = clsCustomerMaster.GetCustomerBaseQryforCustomerCurrency(False, False, "", False, "1", False, False, True)
            'clsCommonFunctionality.CreateSQLView("View_CUSTOMER_DATA_Currency", strSQLViewBody)

            ' '' create view for vendor git and actual 
            'Dim objFilter As New structVendorFilter
            'objFilter.CurrencyType = "ConvRate"
            'objFilter.DocumentWise = False
            'objFilter.FormType = clsUserMgtCode.VendorLedgerReport
            'objFilter.IncludeApplyDoc = True
            'objFilter.IsOnlyForAgainstSalary = False
            'objFilter.strPortrait = True
            'objFilter.strLandscape = False
            'objFilter.strtempBaseQryforopening = ""
            'objFilter.strtempBaseQryforopeningForMIS = ""
            'objFilter.VendorGroupWise = False
            'objFilter.VendorWise = False
            'objFilter.IS_GIT = True
            'objFilter.isOpening = True
            'strSQLViewBody = clsVendorMaster.GetVendorBaseQry(objFilter)
            'clsCommonFunctionality.CreateSQLView("View_VENDOR_DATA_GIT", strSQLViewBody)

            ' '' create view for vendor actual            
            'objFilter.IS_GIT = False
            'objFilter.isOpening = True
            'strSQLViewBody = clsVendorMaster.GetVendorBaseQry(objFilter)
            'clsCommonFunctionality.CreateSQLView("View_VENDOR_DATA", strSQLViewBody)

            ' '' create view for customer git and actual  currency   
            'objFilter.CurrencyType = "1"
            'objFilter.IS_GIT = True
            'objFilter.isOpening = True
            'strSQLViewBody = clsVendorMaster.GetVendorBaseQry(objFilter)
            'clsCommonFunctionality.CreateSQLView("View_VENDOR_DATA_Currency_GIT", strSQLViewBody)

            ' '' create view for customer git and actual            
            'objFilter.CurrencyType = "1"
            'objFilter.IS_GIT = False
            'objFilter.isOpening = True
            'strSQLViewBody = clsVendorMaster.GetVendorBaseQry(objFilter)
            'clsCommonFunctionality.CreateSQLView("View_VENDOR_DATA_Currency", strSQLViewBody)

            '======================Added by Preeti Gupta[create view for KG to Ltr and Ltr to KG conversion]
            'strSQLViewBody = ClsUDLSalesQuery.ConversionKGtoLTRorLTRtoKG("")
            'clsCommonFunctionality.CreateSQLView("View_GetConversion", strSQLViewBody)

            clsCommon.ProgressBarHide()
        Catch ex As Exception
            clsCommon.ProgressBarHide()
        End Try
    End Sub
End Class


Public Class clsAllSQLTrigger
    ''Ticket no BM00000008632
    Public Shared Sub CreateAllTrigger()
        clsCommon.ProgressBarShow()
        Try
            clsCommon.ProgressBarUpdate("verifying SQL Triggers")
            '' done by Panch Raj agaist Ticket No:BM00000008470
            '' create Triggers of Bank for bank book
            Dim CreateAletr As String
            Dim qryTrig As String
            '''''''''''create or alter TrgPaymentHeader trigger'''''''''''''''''''''''''
            If clsPostCreateTable.CheckTriggerExits("TrgPaymentHeader", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            ' qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentHeader] ON [dbo].[TSPL_PAYMENT_HEADER] FOR Update,Insert  AS declare @Payment_No varchar(30),@Payment_Type char(2),@Payment_Date date,@Vendor_Code varchar(12),@Vendor_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date date,@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Payment_Amount decimal(18,2),@Posted char(1),@IsChkReverse char(1),@Bank_Charges decimal(18,2)  select @Bank_Charges=isnull(Bank_Charges,0),@Payment_No=Payment_No,@Payment_Type=Payment_Type,@Posted=Posted,@IsChkReverse=IsChkReverse from inserted  if  @IsChkReverse='N'  begin  if @Payment_Type='PY' or @Payment_Type='AV' or @Payment_Type='OA'  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount   FROM TSPL_Payment_HEADER INNER JOIN TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN  TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on CASE WHEN PAYMENT_TYPE='PY' THEN TSPL_VENDOR_ACCOUNT_SET.Payable_Account  WHEN (TSPL_Payment_HEADER.PAYMENT_TYPE='AV' or TSPL_Payment_HEADER.PAYMENT_TYPE='OA') and TSPL_Payment_HEADER.advance_against_salary=0 THEN TSPL_VENDOR_ACCOUNT_SET.Advance_Account  WHEN (TSPL_Payment_HEADER.PAYMENT_TYPE='AV' or TSPL_Payment_HEADER.PAYMENT_TYPE='OA') and TSPL_Payment_HEADER.advance_against_salary=1 THEN TSPL_VENDOR_ACCOUNT_SET.Advance_Against_Salary END=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Payment_Amount,0,'Payment',@Payment_Type,1)  End  if @Payment_Type='RC'  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount    FROM TSPL_Payment_HEADER INNER JOIN TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN  TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Advance_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Payment_Amount,'Payment',@Payment_Type,1)  End  if @Posted <> '1'  begin  if @Payment_Type='MI'  begin  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No  End  End  if @Payment_Type='MI'  and @Bank_Charges > 0  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=CREDITACC ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount= isnull(Bank_Charges,0)  FROM TSPL_Payment_HEADER INNER JOIN  TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_Payment_HEADER.Bank_Charges_Ac=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No and TransactionType= 'MIOther'  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Payment_Amount,0,'Payment','MIOther',1)   End  end "
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentHeader] ON [dbo].[TSPL_PAYMENT_HEADER] FOR Update,Insert  AS declare @Payment_No varchar(30),@Payment_Type char(2),@Payment_Date date,@Vendor_Code varchar(12),@Vendor_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date date,@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Payment_Amount decimal(18,2),@Posted char(1),@IsChkReverse char(1),@Bank_Charges decimal(18,2),@Is_Security char(1),@ERPstartDate date  select @Bank_Charges=isnull(Bank_Charges,0),@Payment_No=Payment_No,@Payment_Type=Payment_Type,@Posted=Posted,@IsChkReverse=IsChkReverse,@Is_Security=Is_Security,@Payment_Date=Payment_Date from inserted Select @ERPstartDate=Description from TSPL_FIXED_PARAMETER where code ='ERPStartDate' and Type ='ERPStartDate'  if  @IsChkReverse='N'  begin  if @Payment_Type='PY' or @Payment_Type='AV' or @Payment_Type='OA'  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount   FROM TSPL_Payment_HEADER INNER JOIN TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN  TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on CASE WHEN PAYMENT_TYPE='PY' THEN TSPL_VENDOR_ACCOUNT_SET.Payable_Account  WHEN (TSPL_Payment_HEADER.PAYMENT_TYPE='AV' or TSPL_Payment_HEADER.PAYMENT_TYPE='OA') and TSPL_Payment_HEADER.advance_against_salary=0 THEN TSPL_VENDOR_ACCOUNT_SET.Advance_Account  WHEN (TSPL_Payment_HEADER.PAYMENT_TYPE='AV' or TSPL_Payment_HEADER.PAYMENT_TYPE='OA') and TSPL_Payment_HEADER.advance_against_salary=1 THEN TSPL_VENDOR_ACCOUNT_SET.Advance_Against_Salary END=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Payment_Amount,0,'Payment',@Payment_Type,1)  End  if @Payment_Type='RC'  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Payable_Account ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount    FROM TSPL_Payment_HEADER INNER JOIN TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN  TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_Payment_HEADER.Vendor_Account_Set=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Advance_Account=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No if @Payment_Date <@ERPstartDate and @Is_Security =1 begin Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No end if  @Payment_Date >=@ERPstartDate or (@Payment_Date <@ERPstartDate and  @Is_Security <>1) begin insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Payment_Amount,'Payment',@Payment_Type,1) end  End  if @Posted <> '1'  begin  if @Payment_Type='MI'  begin  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No  End  End  if @Payment_Type='MI'  and @Bank_Charges > 0  begin  SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=CREDITACC ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount= isnull(Bank_Charges,0)  FROM TSPL_Payment_HEADER INNER JOIN  TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_Payment_HEADER.Bank_Charges_Ac=tspl_GL_Accounts1.Account_Code where Payment_No=@Payment_No  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No and TransactionType= 'MIOther'  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Payment_Amount,0,'Payment','MIOther',1)   End  end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)


            '''''''''''create or alter TrgPaymentHeaderDelete trigger'''''''''''''''''''''''''
            If clsPostCreateTable.CheckTriggerExits("TrgPaymentHeaderDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentHeaderDelete] ON [dbo].[TSPL_PAYMENT_HEADER] FOR Delete AS " & _
                      " declare @Payment_No varchar(30) " & _
                      " select @Payment_No=Payment_No from deleted " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No"

            'qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentHeaderDelete] ON [dbo].[TSPL_PAYMENT_HEADER] INSTEAD OF DELETE AS " & _
            '          " declare @Payment_No varchar(30) " & _
            '          " select @Payment_No=deleted.Payment_No from deleted " & _
            '          " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Payment_No"

            clsDBFuncationality.ExecuteNonQuery(qryTrig)



            '''''''''''create or alter TrgReceiptHeaderDelete trigger'''''''''''''''''''''''''
            If clsPostCreateTable.CheckTriggerExits("TrgReceiptHeaderDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgReceiptHeaderDelete] ON [dbo].[TSPL_RECEIPT_HEADER] FOR Delete AS " & _
                      " declare @Receipt_No varchar(30) " & _
                      " select @Receipt_No=Receipt_No from deleted " & _
                      " Delete from  TSPL_BANK_BOOK where  SOURCEDOC_NO=@Receipt_No "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)



            ''--Balwinder Added missing trigger Ticket no-:BM00000008632
            If clsPostCreateTable.CheckTriggerExits("TrgLoadOutTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgLoadOutTransType] ON [dbo].[TSPL_SHIPMENT_MASTER] FOR Update AS declare @post char(1), @Shipment_No varchar(30),@Shipment_Type varchar(20),@Location varchar(12),@Excisable char(1)  select @post=Is_Post,@Shipment_No=Shipment_No,@Shipment_Type=Shipment_Type,@Location=Location from inserted select @Excisable=Excisable from TSPL_LOCATION_MASTER where Location_Code=@Location if @post='Y' begin  if @Shipment_Type='Sale' begin 	 if  @Excisable='T' update TSPL_JOURNAL_MASTER set Type='Excisable LoadOut' where Source_Doc_No=@Shipment_No  if  @Excisable='F'  update TSPL_JOURNAL_MASTER set Type='NonExcisable LoadOut' where Source_Doc_No=@Shipment_No end if @Shipment_Type='Transfer' begin  update TSPL_JOURNAL_MASTER set Type='Route LoadOut' where Source_Doc_No=@Shipment_No end end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_isApInvoiceExits", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            qryTrig = "" & CreateAletr & "  trigger [dbo].[trg_isApInvoiceExits] on [dbo].[TSPL_JOURNAL_MASTER]  for insert  as declare @POstFlag as integer  declare @Source_Code as varchar(30), @Desc Varchar(500)  declare @Source_Doc_No as varchar(30)  Select @Source_Code=i.Source_Code from Inserted i;  Select @Source_Doc_No=i.Source_Doc_No, @Desc=i.Voucher_Desc from Inserted i;  if @Source_Code='AP-IN'   select @POstFlag=count(*) from TSPL_VENDOR_INVOICE_HEAD where Document_No =@Source_Doc_No   if  @POstFlag<1  begin   Print 'Document No : ' + @Source_Doc_No rollback  raiserror ('No AP Invoice Entry Exits',16,1)  End "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_MisMatchBal", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
            End If
            qryTrig = "" & CreateAletr & " trigger [dbo].[trg_MisMatchBal] on [dbo].[TSPL_JOURNAL_MASTER] for update as declare @VoucherNo as varchar(30) declare @InsertedVoucher as varchar(30) declare @POstFlag as char(1) declare @InactiveGLAccount as varchar(30)" + Environment.NewLine + _
            "Select @POstFlag=i.authorized from inserted i; " + Environment.NewLine + _
            "Select @InsertedVoucher=i.voucher_no from deleted i;	 " + Environment.NewLine + _
            "select @VoucherNo= dbo.funMismatchVoucher(@InsertedVoucher) " + Environment.NewLine + _
            "if len(@VoucherNo) >0 and @POstFlag='A' " + Environment.NewLine + _
            "begin" + Environment.NewLine + _
            "rollback tran" + Environment.NewLine + _
            "raiserror ('Location Wise Debit is not Equal To Credit.Voucher Cannot Be Posted.',16,1) " + Environment.NewLine + _
            "End " + Environment.NewLine + _
            "if len(@InsertedVoucher) >0 and @POstFlag='A'" + Environment.NewLine + _
            "begin" + Environment.NewLine + _
            "select top 1 @InactiveGLAccount=TSPL_JOURNAL_DETAILS.Account_code from TSPL_JOURNAL_DETAILS " + Environment.NewLine + _
            "left outer join TSPL_GL_ACCOUNTS on TSPL_GL_ACCOUNTS.Account_Code=TSPL_JOURNAL_DETAILS.Account_code" + Environment.NewLine + _
            "where Voucher_No=@InsertedVoucher and TSPL_GL_ACCOUNTS.Status='N'	" + Environment.NewLine + _
            "if len(isnull( @InactiveGLAccount,'')) >0 " + Environment.NewLine + _
           " begin" + Environment.NewLine + _
            "rollback tran" + Environment.NewLine + _
            "declare @Msg as varchar(1000)='Voucher No: ' + @InsertedVoucher + ' Having inactive GL Account: '+@InactiveGLAccount  " + Environment.NewLine + _
            "raiserror (@Msg,16,1)" + Environment.NewLine + _
            "End" + Environment.NewLine + _
            "end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_UniqueSrcCodeAndDocNO", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  trigger [dbo].[trg_UniqueSrcCodeAndDocNO] on [dbo].[TSPL_JOURNAL_MASTER] for insert  as declare @POstFlag as integer declare @Source_Code as varchar(30), @Desc Varchar(500) declare @Source_Doc_No as varchar(30) Select @Source_Code=i.Source_Code from inserted i; Select @Source_Doc_No=i.Source_Doc_No, @Desc=i.Voucher_Desc from inserted i; select @POstFlag=count(*) from TSPL_JOURNAL_MASTER where Source_Code<>'GL-JE' and  (Source_Code =@Source_Code and Source_Doc_No =@Source_Doc_No AND Voucher_Desc=@Desc) if  @POstFlag>1 begin rollback tran raiserror ('Cannot create duplicate entry',16,1) end  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)


            If clsPostCreateTable.CheckTriggerExits("trg_dontdeleteOpenShift", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  trigger [dbo].[trg_dontdeleteOpenShift] on [dbo].[TSPL_OPEN_MCC_SHIFT]  for delete  as   begin  declare @POstFlag as integer   declare @Mcc_Code as varchar(30)    declare @Shift as Varchar(1)   declare @Doc_Date as date  Select @Mcc_Code=i.Mcc_code from deleted i;  Select @Shift=i.Shift from deleted i; Select @Doc_Date=i.Mcc_Shift_Date from deleted i;   select @POstFlag=count(*) from TSPL_MILK_Receipt_Head where   (Mcc_code =@Mcc_code and shift =@Shift and doc_date= @Doc_Date)   if  @POstFlag>0         begin   raiserror ('Cannot delete entry',16,1)   Rollback tran;     End end   "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgBankReverse", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgBankReverse] ON [dbo].[TSPL_BANK_REVERSE] " & _
                      " FOR Update,Insert AS declare @Receipt_No varchar(30),@Reverse_Code varchar(30),@Reversal_Date date,@Cust_Code varchar(12),@Cust_Name varchar(60),@Vendor_Code varchar(12),@Vendor_Name varchar(60),@Bank_Code varchar(12),@Bank_Name varchar(60),@LocCode varchar(12),@LocName varchar(50),@ToLocCode varchar(12),@ToLocName varchar(50),@Bank_Acc_No varchar(30),@Bank_AccDesc varchar(60),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@GLAcc_No varchar(30),@GLAccDesc varchar(60),@Payment_Amount decimal(18,2),@Posted char(1),@Receipt_Amount decimal(18,2),@Source_Type char(2),@Receipt_Type char(1),@Count int,@LineNo int,@NarrationDetail varchar(200),@Payment_No varchar(30),@Payment_Type char(2), @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float " & _
                      " select @Posted=Post, @Reverse_Code=Reverse_Code,@Source_Type=Source_Type,@Cheque_No=Cheque_No ,@Receipt_No=Document_No,@Payment_No=Document_No from inserted  " & _
                      " ----For AR Reverse Entry " + Environment.NewLine & _
                      " if @Source_Type='AR' " & _
                      " begin " & _
                      " select @Receipt_No=Receipt_No,@Receipt_Type=Receipt_Type,@Cheque_Date=Cheque_Date, @Currency=CURRENCY_CODE, @Base_Currency=BASE_CURRENCY_CODE, @Conversion_Rate=ConvRate from TSPL_RECEIPT_HEADER where Receipt_No=@Receipt_No " & _
                      " ----For AR Reverse Entry ( Receipt and Prepayment ) " + Environment.NewLine & _
                      " if @Receipt_Type not in ( 'M','S','F','A') " & _
                      " begin SELECT   @Reversal_Date=Reversal_Date, @Cust_Code=TSPL_CUSTOMER_MASTER.Cust_Code,  @Cust_Name=Cust_Name, @Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LocName=TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC ,@Bank_AccDesc=TSPL_GL_ACCOUNTS.Description ,@GLAcc_No= Receivable_Control_acct ,@GLAccDesc=tspl_GL_Accounts1.Description ,@Narration=Reason,@Cheque_No=TSPL_RECEIPT_HEADER.Cheque_No,@Cheque_Date=Cheque_Date,@Receipt_Amount=Amount, @Currency=TSPL_RECEIPT_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_RECEIPT_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_RECEIPT_HEADER.ConvRate " & _
                      " FROM    TSPL_BANK_REVERSE INNER JOIN TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_CUSTOMER_MASTER on TSPL_BANK_REVERSE.Cust_Code=TSPL_CUSTOMER_MASTER.Cust_Code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_CUSTOMER_ACCOUNT_SET on TSPL_CUSTOMER_MASTER.Cust_Account=TSPL_CUSTOMER_ACCOUNT_SET.Cust_Account  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_CUSTOMER_ACCOUNT_SET.Receivable_Control_acct=tspl_GL_Accounts1.Account_Code inner join TSPL_RECEIPT_HEADER on TSPL_BANK_REVERSE.Document_No=TSPL_RECEIPT_HEADER.Receipt_No where Reverse_Code=@Reverse_Code and Receipt_No <> ISNULL(UnApplied_No, '') " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Reverse_Code,@Reversal_Date,@Cust_Code,@Cust_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Receipt_Amount,0,'Reverse','AR', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
                      " end	  " & _
                      " if @Receipt_Type ='F' " & _
                      " begin " & _
                      " SELECT   @Reversal_Date=Reversal_Date, @Cust_Code=TSPL_CUSTOMER_MASTER.Cust_Code,  " & _
                      " @Cust_Name=Cust_Name, @Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=				TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LocName=TSPL_GL_SEGMENT_CODE.		Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC ,@Bank_AccDesc=TSPL_GL_ACCOUNTS.Description ,@GLAcc_No=		Receivable_Control_acct ,@GLAccDesc=tspl_GL_Accounts1.Description ,@Narration=Reason,@Cheque_No=TSPL_RECEIPT_HEADER.Cheque_No,@Cheque_Date=Cheque_Date,@Receipt_Amount=Amount, @Currency=TSPL_RECEIPT_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_RECEIPT_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_RECEIPT_HEADER.ConvRate " & _
                      " 	FROM    TSPL_BANK_REVERSE INNER JOIN " & _
                      " 					  TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_CUSTOMER_MASTER on TSPL_BANK_REVERSE.Cust_Code=TSPL_CUSTOMER_MASTER.Cust_Code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_CUSTOMER_ACCOUNT_SET on TSPL_CUSTOMER_MASTER.Cust_Account=TSPL_CUSTOMER_ACCOUNT_SET.Cust_Account  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_CUSTOMER_ACCOUNT_SET.Receivable_Control_acct=tspl_GL_Accounts1.Account_Code inner join TSPL_RECEIPT_HEADER on TSPL_BANK_REVERSE.Document_No=TSPL_RECEIPT_HEADER.Receipt_No where Reverse_Code=@Reverse_Code and Receipt_No <> ISNULL(UnApplied_No, '') " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Reverse_Code,@Reversal_Date,@Cust_Code,@Cust_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Receipt_Amount,'Reverse','AR', @Currency, @Base_Currency, @Conversion_Rate,1) " & _
                      " end	  " & _
                      " ----For AR Reverse Entry ( For Misc Entry) " + Environment.NewLine & _
                      " if @Receipt_Type = 'M'  begin select @Count=count(Receipt_No) from TSPL_RECEIPT_DETAIL where Receipt_No=@Receipt_No  " & _
                      " set @Lineno=1 " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code " & _
                      " while @Lineno <= @Count " & _
                      " begin " & _
                      " SELECT   @Reversal_Date=Reversal_Date,@Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC FROM    TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code  where Reverse_Code=@Reverse_Code " & _
                      " select @NarrationDetail=Remarks,@Receipt_Amount=Applied_Amount,@GLAcc_No=Account_Code,@GLAccDesc=Description, @Currency=CURRENCY_CODE, @Base_Currency=BASE_CURRENCY_CODE, @Conversion_Rate=ConvRate from TSPL_RECEIPT_DETAIL LEFT OUTER JOIN TSPL_RECEIPT_HEADER ON TSPL_RECEIPT_HEADER.Receipt_No=TSPL_RECEIPT_DETAIL.Receipt_No where TSPL_RECEIPT_HEADER.Receipt_No=@Receipt_No  and Receipt_Line_No=@Lineno " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Reverse_Code,@Reversal_Date,@Cust_Code,@Cust_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,@NarrationDetail,@Receipt_Amount,0,'Reverse','AR', @Currency, @Base_Currency, @Conversion_Rate,@Lineno) " & _
                      " set @Lineno=@LineNo + 1 " & _
                      " end  " & _
                      " end	  " & _
                      " ----For AR Reverse Entry ( For Misc Refund Entry) " + Environment.NewLine & _
                      " if @Receipt_Type = 'S'  " & _
                      " begin " & _
                      " select @Count=count(Receipt_No) from TSPL_RECEIPT_DETAIL where Receipt_No=@Receipt_No  " & _
                      " set @Lineno=1 " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code " & _
                      " while @Lineno <= @Count " & _
                      " begin " & _
                      " SELECT   @Reversal_Date=Reversal_Date,@Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC FROM    TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code  where Reverse_Code=@Reverse_Code " & _
                      " select @NarrationDetail=Remarks,@Receipt_Amount=Applied_Amount,@GLAcc_No=Account_Code,@GLAccDesc=Description, @Currency=CURRENCY_CODE, @Base_Currency=BASE_CURRENCY_CODE, @Conversion_Rate=ConvRate from TSPL_RECEIPT_DETAIL LEFT OUTER JOIN TSPL_RECEIPT_HEADER ON TSPL_RECEIPT_HEADER.Receipt_No=TSPL_RECEIPT_DETAIL.Receipt_No where TSPL_RECEIPT_HEADER.Receipt_No=@Receipt_No   and Receipt_Line_No=@Lineno " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Reverse_Code,@Reversal_Date,@Cust_Code,@Cust_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,@NarrationDetail,0,@Receipt_Amount,'Reverse','AR', @Currency, @Base_Currency, @Conversion_Rate, @Lineno) " & _
                      " set @Lineno=@LineNo + 1 " & _
                      " end  " & _
                      " end	  " & _
                      " end " & _
                      " ----For AP Reverse Entry " + Environment.NewLine & _
                      " if @Source_Type='AP' begin " & _
                      " select @Payment_No=Payment_No,@Payment_Type=Payment_Type,@Cheque_Date=convert(varchar,Cheque_Date,103),@Cheque_No=Cheque_No, @Currency=CURRENCY_CODE, @Base_Currency=BASE_CURRENCY_CODE, @Conversion_Rate=ConvRate from TSPL_payment_HEADER where Payment_No=@Payment_No  " & _
                      " Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code " & _
                      " ----For AP Reverse Entry  (payment and prepayment) " + Environment.NewLine & _
                      " if @Payment_Type not in ( 'MI','RC','AD')  " & _
                      " begin " & _
                      " SELECT  @Reversal_Date=Reversal_Date, @Vendor_Code=TSPL_BANK_REVERSE.Vendor_Code, " & _
                      " @Vendor_Name=TSPL_BANK_REVERSE.Vendor_Name, @Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC ,@Bank_AccDesc=TSPL_GL_ACCOUNTS.Description ,@GLAcc_No=Payable_Account ,@GLAccDesc=tspl_GL_Accounts1.Description ,@Narration=Reason,@Cheque_No=TSPL_Payment_HEADER.Cheque_No,@Cheque_Date=Convert(Varchar,Cheque_Date,103),@Payment_Amount=case when (Payment_Type='OA' or Payment_Type ='AV') then Payment_Amount   else Amount  end " & _
                      " FROM  TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
                      " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_VENDOR_MASTER on TSPL_BANK_REVERSE.Vendor_Code=TSPL_VENDOR_MASTER.Vendor_Code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_VENDOR_MASTER.Vendor_Account=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code inner join TSPL_Payment_HEADER on TSPL_BANK_REVERSE.Document_No=TSPL_Payment_HEADER.Payment_No where Reverse_Code=@Reverse_Code " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Reverse_Code,@Reversal_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Payment_Amount,'Reverse','AP', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
                      " end " & _
                      " ----For Misc Other charges entry " + Environment.NewLine & _
                      " if @Payment_Type ='MI' " & _
                      " begin " & _
                      " SELECT  @Reversal_Date=Reversal_Date, @Vendor_Code=TSPL_BANK_REVERSE.Vendor_Code, " & _
                      " @Vendor_Name=TSPL_BANK_REVERSE.Vendor_Name, @Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC ,@Bank_AccDesc=TSPL_GL_ACCOUNTS.Description ,@GLAcc_No=CREDITACC ,@GLAccDesc=tspl_GL_Accounts1.Description ,@Narration=Reason,@Cheque_No=TSPL_Payment_HEADER.Cheque_No,@Cheque_Date=Convert(Varchar,Cheque_Date,103),@Payment_Amount=isnull(Bank_Charges,0) " & _
                      " FROM  TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
                      " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code  inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code  inner join TSPL_Payment_HEADER on TSPL_BANK_REVERSE.Document_No=TSPL_Payment_HEADER.Payment_No inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_Payment_HEADER.Bank_Charges_Ac=tspl_GL_Accounts1.Account_Code where Reverse_Code=@Reverse_Code " & _
                      " if   @Payment_Amount > 0  " & _
                      " begin  " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Reverse_Code,@Reversal_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Payment_Amount,'Reverse','AP', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
                      " end " & _
                      " end " & _
                      " ----For AP Reverse Entry (For Misc entry) " + Environment.NewLine & _
                      " if @Payment_Type = 'MI'  " & _
                      " begin " & _
                      " select @Count=count(Payment_No) from TSPL_Payment_DETAIL where Payment_No=@Payment_No  " & _
                      " set @Lineno=1 " & _
                      " while @Lineno <= @Count " & _
                      " begin " & _
                      " SELECT   @Reversal_Date=Reversal_Date,@Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC FROM    TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code  where Reverse_Code=@Reverse_Code " & _
                      " select @NarrationDetail=Remarks,@Payment_Amount=Net_Balance,@GLAcc_No=Account_Code,@GLAccDesc=Description from  TSPL_Payment_DETAIL where Payment_No=@Payment_No   and Payment_Line_No=@LineNo " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Reverse_Code,@Reversal_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,@NarrationDetail,0,@Payment_Amount,'Reverse','AP', @Currency, @Base_Currency, @Conversion_Rate, @LineNo) " & _
                      " set @Lineno=@LineNo + 1 " & _
                      " 	end " & _
                      " end " & _
                      " if @Payment_Type = 'RC'  " & _
                      " begin " & _
                      " SELECT  @Reversal_Date=Reversal_Date, @Vendor_Code=TSPL_BANK_REVERSE.Vendor_Code, " & _
                      " @Vendor_Name=TSPL_BANK_REVERSE.Vendor_Name, @Bank_Code=TSPL_BANK_REVERSE.Bank_Code,@Bank_Name=TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3),@LocName= TSPL_GL_SEGMENT_CODE.Description ,@Bank_Acc_No=TSPL_BANK_MASTER.BANKACC ,@Bank_AccDesc=TSPL_GL_ACCOUNTS.Description ,@GLAcc_No=Payable_Account ,@GLAccDesc=tspl_GL_Accounts1.Description ,@Narration=Reason,@Cheque_No=TSPL_Payment_HEADER.Cheque_No,@Cheque_Date=Convert(varchar,Cheque_Date,103),@Payment_Amount=Amount  " & _
                      " FROM  TSPL_BANK_REVERSE INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_BANK_REVERSE.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
                      " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_VENDOR_MASTER on TSPL_BANK_REVERSE.Vendor_Code=TSPL_VENDOR_MASTER.Vendor_Code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_VENDOR_ACCOUNT_SET on TSPL_VENDOR_MASTER.Vendor_Account=TSPL_VENDOR_ACCOUNT_SET.Acct_Set_Code  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_VENDOR_ACCOUNT_SET.Payable_Account=tspl_GL_Accounts1.Account_Code inner join TSPL_Payment_HEADER on TSPL_BANK_REVERSE.Document_No=TSPL_Payment_HEADER.Payment_No where Reverse_Code=@Reverse_Code " & _
                      " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Reverse_Code,@Reversal_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@Bank_Name,@LocCode,@LocName,@Bank_Acc_No,@Bank_AccDesc,@GLAcc_No,@GLAccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Payment_Amount,0,'Reverse','AP', @Currency, @Base_Currency, @Conversion_Rate,1) " & _
                      " end " & _
                      " end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)


            If clsPostCreateTable.CheckTriggerExits("TrgBankReverseDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgBankReverseDelete] ON [dbo].[TSPL_BANK_REVERSE] FOR Delete AS declare @Reverse_Code varchar(30),@Reversal_Date varchar(10),@Cust_Code varchar(12),@Cust_Name varchar(60),@Vendor_Code varchar(12),@Vendor_Name varchar(60),@Bank_Code varchar(12),@Bank_Name varchar(60),@LocCode varchar(12),@LocName varchar(50),@ToLocCode varchar(12),@ToLocName varchar(50),@Bank_Acc_No varchar(30),@Bank_AccDesc varchar(60),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@GLAcc_No varchar(30),@GLAccDesc varchar(60),@Payment_Amount decimal(18,2),@Posted char(1),@Receipt_Amount decimal(18,2),@Source_Type char(2) select  @Reverse_Code=Reverse_Code from deleted  Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Reverse_Code  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgARAdjustmentTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgARAdjustmentTransType] ON [dbo].[TSPL_Receipt_Adjustment_Header] FOR Update AS declare @Is_Post char(1), @Adjustment_No varchar(30) select @Is_Post=Is_Post,@Adjustment_No=Adjustment_No from inserted if @Is_Post = 'Y' begin  update TSPL_JOURNAL_MASTER set Type='Settlement' where Source_Doc_No=@Adjustment_No end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgAPInvoice_SaveUpdate", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  Trigger [dbo].[TrgAPInvoice_SaveUpdate] ON [dbo].[TSPL_VENDOR_INVOICE_HEAD] For Insert, Update as Declare @Document_No Varchar(30), @Currency_Code Varchar(30) Select @Document_No=Document_No, @Currency_Code=CURRENCY_CODE From inserted if ISNULL(@Currency_Code,'')='' Begin Update TSPL_VENDOR_INVOICE_HEAD Set CURRENCY_CODE='INR', ConvRate=1.0 WHERE Document_No=@Document_No End "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_isJournalEntryExits", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "   trigger [dbo].[trg_isJournalEntryExits] on [dbo].[TSPL_VENDOR_INVOICE_HEAD]  for Delete   as  declare @POstFlag as integer  declare @Source_Code as varchar(30), @Desc Varchar(500)  declare @Source_Doc_No as varchar(30)  Select @Source_Code=i.Document_No from deleted i;  select @POstFlag=count(*) from TSPL_Journal_master where Source_Doc_No =@Source_Code and Source_Code='AP-IN'   if  @POstFlag>0  begin  Print 'Document No : ' + @Source_Code  rollback  raiserror ('Journal Entry Exits',16,1)  End  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgPaymentTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentTransType] ON [dbo].[TSPL_PAYMENT_HEADER] " & _
" FOR Update " & _
" AS " & _
 " declare @post char(1), @Payment_No varchar(30),@Payment_Type char(2),@Bank_Type char(1),@Bank_Code varchar(12) " & _
" select @post=Posted,@Payment_No=Payment_No,@Payment_Type=Payment_Type,@Bank_Code=Bank_Code from inserted " & _
" select @Bank_Type=Bank_type from TSPL_BANK_MASTER  where BANK_CODE=@Bank_Code " & _
" if @post='P' " & _
" begin  " & _
" if @Payment_Type='PY' " & _
" begin 	 " & _
    " 	if  @Bank_Type='B' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Payment bank' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type='C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Payment Settlement' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Payment Cash' where Source_Doc_No=@Payment_No			 " & _
" end " & _
" if @Payment_Type='AV' " & _
" begin 	 " & _
    " 	if  @Bank_Type='B' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Advance bank Payment' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type='C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Advance Settlement Payment' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Advance Cash Payment' where Source_Doc_No=@Payment_No			 " & _
" end " & _
" if @Payment_Type='OA' " & _
" begin 	 " & _
    " 	if  @Bank_Type='B' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='OnAccount bank Payment' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type='C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='OnAccount Settlement Payment' where Source_Doc_No=@Payment_No " & _
    " 	if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
    " 	update TSPL_JOURNAL_MASTER set Type='OnAccount Cash Payment' where Source_Doc_No=@Payment_No			 " & _
" end " & _
" if @Payment_Type='MI' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous bank Payment' where Source_Doc_No=@Payment_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous Settlement Payment' where Source_Doc_No=@Payment_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous Cash Payment' where Source_Doc_No=@Payment_No			 " & _
" end " & _
" if @Payment_Type='AD' " & _
" begin 	 " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Apply Document Payment' where Source_Doc_No=@Payment_No " & _
" end " & _
" end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgPaymentDetail", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPaymentDetail] ON [dbo].[TSPL_PAYMENT_DETAIL] " & _
" FOR Update,insert " & _
" AS " & _
 " declare @Payment_No varchar(30),@Payment_Type char(2),@Payment_Date date,@Vendor_Code varchar(12),@Vendor_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Payment_Amount decimal(18,2),@Posted char(1),@NarrationDetail varchar(200),@Applied_Amount decimal(18,2),@Payment_Line_No int, " & _
" @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float " & _
 " select @Payment_No=Payment_No,@Payment_Line_No=Payment_Line_No from inserted " & _
 " select @Payment_No=Payment_No,@Payment_type=Payment_Type,@Posted=Posted,@Vendor_Code=Vendor_Code, @Currency=CURRENCY_CODE, @Base_Currency=BASE_CURRENCY_CODE, @Conversion_Rate=ConvRate from TSPL_Payment_HEADER where Payment_No=@Payment_No " & _
 " if @Payment_type='MI'  " & _
 " begin " & _
 " SELECT @Payment_Date=TSPL_Payment_HEADER.Payment_Date, @Vendor_Code=TSPL_Payment_HEADER.Vendor_Code,  " & _
" @Vendor_Name=TSPL_Payment_HEADER.Vendor_Name,@Bank_Code= TSPL_Payment_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Payment_Amount=Payment_Amount " & _
" FROM TSPL_Payment_HEADER INNER JOIN " & _
" TSPL_BANK_MASTER ON TSPL_Payment_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
" TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code  where Payment_No=@Payment_No  " & _
" select @Applied_Amount=Net_Balance,@NarrationDetail=Remarks,@Payment_Amount=Net_Balance,@GlAcct=Account_Code,@GlAcctName=Description from  TSPL_Payment_DETAIL where Payment_No=@Payment_No   and Payment_Line_No=@Payment_Line_No " & _
" Delete from tspl_Bank_book where SOURCEDOC_NO=@Payment_No and GL_Account_Code=@GlAcct and line_No=@Payment_Line_No " & _
" insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Payment_No,@Payment_Date,@Vendor_Code,@Vendor_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,@NarrationDetail,@Payment_Amount,0,'Payment','MI', @Currency, @Base_Currency, @Conversion_Rate, @Payment_Line_No) " & _
" END "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        'If clsPostCreateTable.CheckTriggerExits("trg_dontdeletecreatedsrnsampleno", Nothing) = 0 Then
                                        '    CreateAletr = "Create "
                                        'Else
                                        '    CreateAletr = "Alter "
                                        'End If
                                        'qryTrig = "" & CreateAletr & "  trigger [dbo].[trg_dontdeletecreatedsrnsampleno] on [dbo].[TSPL_MILK_SAMPLE_DETAIL]  for delete    as  begin try  declare @POstFlag as integer  declare @Doc_Code as varchar(30)  declare @Sample_No as integer  Select @Doc_Code=i.doc_code from deleted i;  Select @Sample_No=i.sample_No from deleted i;  select @POstFlag=count(*) from TSPL_milk_srn_Head where   (Milk_sample_Code =@Doc_Code and sample_No =@Sample_No )  if  @POstFlag>0  raiserror ('Cannot delete entry',16,1)  Rollback   end try  begin catch  raiserror ('Cannot delete entry',16,1)  Rollback  end catch  "
                                        'clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgAdjustmentTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgAdjustmentTransType] ON [dbo].[TSPL_ADJUSTMENT_HEADER] " & _
" FOR Update " & _
" AS " & _
 " declare @post char(1), @adjustment_no varchar(30),@Trans_Type varchar(30),@ItemType char(2) " & _
" select @post=posted,@adjustment_no=adjustment_no,@Trans_Type=Trans_Type,@ItemType=ItemType from inserted " & _
" if @post='Y' " & _
" begin  " & _
" if @ItemType='E' " & _
" begin  " & _
" 	if @Trans_Type='In' " & _
" 	update TSPL_JOURNAL_MASTER set Type='Empty In' where Source_Doc_No=@adjustment_no " & _
" 	else " & _
" 	update TSPL_JOURNAL_MASTER set Type='Empty out' where Source_Doc_No=@adjustment_no " & _
" end " & _
" if @ItemType='FM' " & _
" begin  " & _
" 	if @Trans_Type='In' " & _
" 	update TSPL_JOURNAL_MASTER set Type='Finished Good In' where Source_Doc_No=@adjustment_no " & _
" 	else " & _
" 	update TSPL_JOURNAL_MASTER set Type='Finished Good out' where Source_Doc_No=@adjustment_no " & _
" end " & _
" if @ItemType='FT' " & _
" begin  " & _
" 	if @Trans_Type='In' " & _
" 	update TSPL_JOURNAL_MASTER set Type='Finished Trading In' where Source_Doc_No=@adjustment_no " & _
" 	else " & _
" 	update TSPL_JOURNAL_MASTER set Type='Finished Trading out' where Source_Doc_No=@adjustment_no " & _
" end " & _
" if @ItemType='RM' " & _
" begin  " & _
" 	if @Trans_Type='In' " & _
" 	update TSPL_JOURNAL_MASTER set Type='Raw Material In' where Source_Doc_No=@adjustment_no " & _
" 	else " & _
" 	update TSPL_JOURNAL_MASTER set Type='Raw Material out' where Source_Doc_No=@adjustment_no " & _
" end " & _
" if @ItemType='OT' " & _
" begin  " & _
" 	if @Trans_Type='In' " & _
" 	update TSPL_JOURNAL_MASTER set Type='Adjustment In' where Source_Doc_No=@adjustment_no " & _
" 	else " & _
" 	update TSPL_JOURNAL_MASTER set Type='Adjustment out' where Source_Doc_No=@adjustment_no " & _
" end " & _
" end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        'If clsPostCreateTable.CheckTriggerExits("trg_CreateMccMasterHistory_update", Nothing) = 0 Then
                                        '    CreateAletr = "Create "
                                        'Else
                                        '    CreateAletr = "Alter "
                                        'End If
                                        'qryTrig = "" & CreateAletr & "  trigger [dbo].[trg_CreateMccMasterHistory_update] on [dbo].[TSPL_MCC_MASTER]  for update   as   begin try  declare @POstFlag as integer   declare @Doc_Code as varchar(30)   Select @Doc_Code=i.MCC_Code from deleted i;   Insert into tspl_mcc_Master_History(MCC_Code,MCC_Type,MCC_NAME,Chilling_Vendor,Add1,Add2,Tehsil,City_code,State_Code,Country_code,Pin_code, Telphone,Email,Fax,MCC_Area,Area_Of_Store,Area_Of_Office,Open_Area_For_tanker,Area_Of_LAB,No_Of_SILO,Total_Storage_capacity,Area_Of_Receiving_DOCK, No_Of_Chiller,Chiller_Brand_Name,Chiller_Capacity,No_Of_MilkPump,MilkPump_Capacity,DripSaver,CanWasher,CanScrubber,FSSAI_NO,ETP,Earthing,Coil_Length, Electricity_Connection,Boiler,NoOfDG,NoOfCompressor,PayeeName,BankName,BankBranch,BankCityCode,BankStateCode,IFCICode,AccountNO,Created_By, Created_Date,Modified_By,Modified_Date,Comp_Code,Industry_Type,Industry_Person,Chilling_Rate,Lease_Rate,Chilling_KG_Ltr,Chilling_Dispatch_Qty, Chilling_Assure_Qty,Chilling_Assure_Period,Agreement_Status,Agreement_Date,Agrmnt_Expired_Date,Security_Status,Cheque_Amt,Cheque_No,Cheque_Date, Bank_Code,FAT_SNF_SAVE,FAT_SNF_CALC,Mcc_Code_VLC_Uploader,Guarantee_Amount,MCC_In_Charge,Start_Date,End_Date,Silo_Capacity,Unit_Code,Unit_Desc, Payment_Cycle,Unit_MccSuperArea,Unit_AreaofStore,Unit_AreaOfOffice,Unit_OpenAreaForTankerMovement,Unit_AreaOfLab,Unit_AreaOfReceivingDock, Unit_ChillingOn,Unit_ChillingOnQty,Unit_ChillingMinGuaranteePeriod,Unit_RateOfLeasedCharges,Pan_No,Standard_Security_Amount,Chilling_Period_Starting_Date ,Default_Weighing_Machine,Default_Sample_Machine,Is_Truck_Sheet_Mandatory,Default_Weighing_Comport,Default_Sample_Comport,In_active,incentive_code, EmpOnAmountOnly)  select MCC_Code,MCC_Type,MCC_NAME,Chilling_Vendor,Add1,Add2,Tehsil,City_code,State_Code,Country_code,Pin_code,Telphone,Email,Fax,MCC_Area,Area_Of_Store ,Area_Of_Office,Open_Area_For_tanker,Area_Of_LAB,No_Of_SILO,Total_Storage_capacity,Area_Of_Receiving_DOCK,No_Of_Chiller,Chiller_Brand_Name ,Chiller_Capacity,No_Of_MilkPump,MilkPump_Capacity,DripSaver,CanWasher,CanScrubber,FSSAI_NO,ETP,Earthing,Coil_Length,Electricity_Connection,Boiler, NoOfDG,NoOfCompressor,PayeeName,BankName,BankBranch,BankCityCode,BankStateCode,IFCICode,AccountNO,Created_By,Created_Date,Modified_By,Modified_Date, Comp_Code,Industry_Type,Industry_Person,Chilling_Rate,Lease_Rate,Chilling_KG_Ltr,Chilling_Dispatch_Qty,Chilling_Assure_Qty,Chilling_Assure_Period, Agreement_Status,Agreement_Date,Agrmnt_Expired_Date,Security_Status,Cheque_Amt,Cheque_No,Cheque_Date,Bank_Code,FAT_SNF_SAVE,FAT_SNF_CALC, Mcc_Code_VLC_Uploader,Guarantee_Amount,MCC_In_Charge,Start_Date,End_Date,Silo_Capacity,Unit_Code,Unit_Desc,Payment_Cycle,Unit_MccSuperArea, Unit_AreaofStore,Unit_AreaOfOffice,Unit_OpenAreaForTankerMovement,Unit_AreaOfLab,Unit_AreaOfReceivingDock,Unit_ChillingOn,Unit_ChillingOnQty, Unit_ChillingMinGuaranteePeriod,Unit_RateOfLeasedCharges,Pan_No,Standard_Security_Amount,Chilling_Period_Starting_Date,Default_Weighing_Machine, Default_Sample_Machine,Is_Truck_Sheet_Mandatory,Default_Weighing_Comport,Default_Sample_Comport,In_active,incentive_code ,EmpOnAmountOnly  from deleted   end try  begin catch   end catch "
                                        'clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgBankTransfer", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgBankTransfer] ON [dbo].[TSPL_BANK_TRANSFER] " & _
" FOR Insert ,Update " & _
" AS " & _
 " declare @Transfer_No varchar(30),@Transfer_Date date,@From_Bank_Code varchar(12),@From_Bank_Name varchar(60),@To_Bank_Code varchar(12),@To_Bank_Name varchar(60),@FromLocCode varchar(12),@FromLocName varchar(50),@ToLocCode varchar(12),@ToLocName varchar(50),@From_Bank_Acc_No varchar(30),@From_Bank_AccDesc varchar(60),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@To_Bank_Acc_No varchar(30),@To_Bank_AccDesc varchar(60),@Transfer_Amount decimal(18,2),@Posted char(1),@Deposit_Amount decimal(18,2), @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float, @Transaction_Type as Char(1) " & _
"  select @Posted=Post, @Transfer_No=Transfer_No from inserted      " & _
" select @Transfer_No=Transfer_No,@Transfer_Date=Transfer_Date,@From_Bank_Code=From_Bank_Code,@From_Bank_Name=From_Bank_Name,@From_Bank_Acc_No=From_Bank_Acc_No,@From_Bank_AccDesc=TSPL_GL_ACCOUNTS.Description,@FromLocCode=RIGHT(From_Bank_Acc_No, 3),@FromLocName=TSPL_GL_SEGMENT_CODE.Description,@Cheque_No = TSPL_BANK_TRANSFER.Cheque_No ,@Cheque_Date =Convert(varchar,TSPL_BANK_TRANSFER.Cheque_Date,103) ,@To_Bank_Acc_No=To_Bank_Acc_No,@To_Bank_AccDesc=TSPL_GL_ACCOUNTS_1.Description,@Narration=TSPL_BANK_TRANSFER.Description,@Transfer_Amount=Transfer_Amount,@Deposit_Amount=Deposit_Amount ,@To_Bank_Code=To_Bank_Code,@To_Bank_Name=To_Bank_Name,@ToLocCode=RIGHT(To_Bank_Acc_No, 3),@ToLocName=TSPL_GL_SEGMENT_CODE_1.Description, @Currency='INR', @Base_Currency='INR', @Conversion_Rate=1.00, @Transaction_Type=TSPL_BANK_TRANSFER.Transaction_Type FROM  TSPL_BANK_TRANSFER  INNER JOIN TSPL_GL_SEGMENT_CODE ON RIGHT(From_Bank_Acc_No, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on From_Bank_Acc_No=TSPL_GL_ACCOUNTS.Account_Code  inner join TSPL_GL_ACCOUNTS as TSPL_GL_ACCOUNTS_1 on To_Bank_Acc_No=TSPL_GL_ACCOUNTS_1.Account_Code  INNER JOIN TSPL_GL_SEGMENT_CODE as TSPL_GL_SEGMENT_CODE_1 ON RIGHT(To_Bank_Acc_No, 3) = TSPL_GL_SEGMENT_CODE_1.Segment_code where Transfer_No=@Transfer_No " & _
" Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Transfer_No " & _
 " if @Transaction_Type='B' " & _
 " Begin " & _
 "  insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Transfer_No,@Transfer_Date,'','',@From_Bank_Code,@From_Bank_Name,@FromLocCode,@FromLocName,@From_Bank_Acc_No,@From_Bank_AccDesc,@To_Bank_Acc_No,@To_Bank_AccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Transfer_Amount,0,'BankTransfer','FromLoc', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
 " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Transfer_No,@Transfer_Date,'','',@To_Bank_Code,@To_Bank_Name,@TOLocCode,@ToLocName,@To_Bank_Acc_No,@To_Bank_AccDesc,@From_Bank_Acc_No,@From_Bank_AccDesc,'','',@Narration,'',0,@Deposit_Amount,'BankTransfer','ToLoc', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
 " End " & _
 " if @Transaction_Type='W' " & _
 " Begin " & _
  " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate, line_No) values (@Transfer_No,@Transfer_Date,'','',@From_Bank_Code,@From_Bank_Name,@FromLocCode,@FromLocName,@From_Bank_Acc_No,@From_Bank_AccDesc,@To_Bank_Acc_No,@To_Bank_AccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Transfer_Amount,0,'BankTransfer','FromLoc', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
" End " & _
 " if @Transaction_Type='R' " & _
 " Begin " & _
 " insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Transfer_No,@Transfer_Date,'','',@To_Bank_Code,@To_Bank_Name,@TOLocCode,@ToLocName,@To_Bank_Acc_No,@To_Bank_AccDesc,@From_Bank_Acc_No,@From_Bank_AccDesc,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@Deposit_Amount,'BankTransfer','ToLoc', @Currency, @Base_Currency, @Conversion_Rate, 1) " & _
 " End "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgBankTransferDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgBankTransferDelete] ON [dbo].[TSPL_BANK_TRANSFER] FOR delete AS declare @Transfer_No varchar(30) select  @Transfer_No=Transfer_No from deleted      Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Transfer_No "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgBankTransferDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgBankTransferDelete] ON [dbo].[TSPL_BANK_TRANSFER] FOR delete AS declare @Transfer_No varchar(30) select  @Transfer_No=Transfer_No from deleted      Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO= @Transfer_No  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgReceiptHeader", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgReceiptHeader] ON [dbo].[TSPL_RECEIPT_HEADER] " & _
" FOR Update,Insert " & _
" AS " & _
" declare @Receipt_No varchar(30),@UnApplied_No varchar(30),@Receipt_Type char(1),@Receipt_Date date,@Cust_Code varchar(12),@Customer_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Receipt_Amount decimal(18,2),@Posted char(1),@UnApply_Amt decimal(18,2),@Count int,@IsChkReverse char(1), @Currency Varchar(30), @Base_Currency Varchar(30), @Conversion_Rate Float,@Skip varchar(1) " & _
" select @Receipt_No=Receipt_No,@Receipt_type=Receipt_Type,@Posted=Posted, @UnApplied_No=isnull(UnApplied_No,1),@IsChkReverse=IsChkReverse,@Skip=(case when Set_Off_Date is not null and SetOffSkipJE='1' then '1' else '0' end) from inserted " & _
" if @Skip='1' " & _
" return ; " & _
" if  @IsChkReverse='N' " & _
" begin " & _
" select @Count=count(UnApplied_No) from TSPL_RECEIPT_HEADER where UnApplied_No=@UnApplied_No " & _
" if @Receipt_type='R' or @Receipt_type='P'  or @Receipt_type='O'  or @Receipt_type='F' or @Receipt_type='M' or @Receipt_type='S'  " & _
"      begin " & _
"        SELECT      @UnApplied_No=UnApplied_No, @Receipt_Date=TSPL_RECEIPT_HEADER.Receipt_Date, @Cust_Code=TSPL_RECEIPT_HEADER.Cust_Code,  " & _
"                       @Customer_Name=TSPL_RECEIPT_HEADER.Customer_Name,@Bank_Code= TSPL_RECEIPT_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@GlAcct=Receivable_Control_acct ,@GlAcctName=tspl_GL_Accounts1.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date,@Receipt_Amount=Receipt_Amount,@UnApply_Amt=UnApply_Amt, @Currency=TSPL_RECEIPT_HEADER.CURRENCY_CODE, @Base_Currency=TSPL_RECEIPT_HEADER.BASE_CURRENCY_CODE, @Conversion_Rate=TSPL_RECEIPT_HEADER.ConvRate " & _
"        FROM         TSPL_RECEIPT_HEADER INNER JOIN " & _
"                       TSPL_BANK_MASTER ON TSPL_RECEIPT_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
"                       TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code inner join TSPL_CUSTOMER_ACCOUNT_SET on TSPL_RECEIPT_HEADER.Cust_Account=TSPL_CUSTOMER_ACCOUNT_SET.Cust_Account  inner join TSPL_GL_ACCOUNTS as tspl_GL_Accounts1 on TSPL_CUSTOMER_ACCOUNT_SET.Receivable_Control_acct=tspl_GL_Accounts1.Account_Code where Receipt_No=@Receipt_No " & _
" 	if  @Receipt_type='R' or @Receipt_type='P'  or @Receipt_type='O' " & _
" 	  begin   " & _
" 	 Delete from  TSPL_BANK_BOOK where  SOURCEDOC_NO=@Receipt_No                                 " & _
" 	 insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Receipt_No,@Receipt_Date,@Cust_Code,@Customer_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',0,@UnApply_Amt,'Receipt',@Receipt_type, @Currency, @Base_Currency, @Conversion_Rate,1) " & _
" 	 end " & _
" 	if  @Receipt_type='F'  " & _
" 	  begin   " & _
" 	 Delete from  TSPL_BANK_BOOK where  SOURCEDOC_NO=@Receipt_No                                 " & _
" 	 insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType, Currency, Base_Currency, Conversion_Rate,line_No) values (@Receipt_No,@Receipt_Date,@Cust_Code,@Customer_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,'',@Receipt_Amount,0,'Receipt',@Receipt_type, @Currency, @Base_Currency, @Conversion_Rate,1) " & _
" 	 end " & _
" if  @Receipt_type in ('M','S') " & _
" begin  " & _
" Delete from  TSPL_BANK_BOOK where  SOURCEDOC_NO=@Receipt_No " & _
" and not exists (select Account_Code from TSPL_RECEIPT_DETAIL where TSPL_BANK_BOOK.SOURCEDOC_NO=TSPL_RECEIPT_DETAIL.Receipt_No " & _
" and TSPL_BANK_BOOK.line_No= TSPL_RECEIPT_DETAIL.Receipt_Line_No " & _
" and TSPL_BANK_BOOK.GL_Account_Code=TSPL_RECEIPT_DETAIL.Account_Code) " & _
" End " & _
" end " & _
" end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgReceiptTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgReceiptTransType] ON [dbo].[TSPL_RECEIPT_HEADER] " & _
" FOR Update " & _
" AS " & _
 " declare @post char(1), @Receipt_No varchar(30),@Receipt_Type char(1),@Bank_Type char(1),@Bank_Code varchar(12),@Skip varchar(1) " & _
" select @post=Posted,@Receipt_No=Receipt_No,@Receipt_Type=Receipt_Type,@Bank_Code=Bank_Code,@Skip=(case when Set_Off_Date is not null and SetOffSkipJE='1' then '1' else '0' end) from inserted " & _
" if @Skip='1' " & _
" return ;" & _
" select @Bank_Type=Bank_type from TSPL_BANK_MASTER  where BANK_CODE=@Bank_Code " & _
" if @post='Y' " & _
" begin  " & _
" if @Receipt_Type='R' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Receipt bank' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Receipt Settlement' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Receipt Cash' where Source_Doc_No=@Receipt_No			 " & _
" end " & _
" if @Receipt_Type='P' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Advance bank Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Advance Settlement Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Advance Cash Receipt' where Source_Doc_No=@Receipt_No			 " & _
" end " & _
" if @Receipt_Type='U' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='UnApplied bank Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='UnApplied Settlement Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='UnApplied Cash Receipt' where Source_Doc_No=@Receipt_No			 " & _
" end " & _
" if @Receipt_Type='M' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous bank Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous Settlement Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='Miscellaneous Cash Receipt' where Source_Doc_No=@Receipt_No			 " & _
" end " & _
" if @Receipt_Type='O' " & _
" begin 	 " & _
" 		if  @Bank_Type='B' " & _
" 		update TSPL_JOURNAL_MASTER set Type='OnAccount bank Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type='C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='OnAccount Settlement Receipt' where Source_Doc_No=@Receipt_No " & _
" 		if  @Bank_Type <> 'B' and @Bank_Type <> 'C' " & _
" 		update TSPL_JOURNAL_MASTER set Type='OnAccount Cash Receipt' where Source_Doc_No=@Receipt_No			 " & _
" end " & _
" if @Receipt_Type='A' " & _
" begin 	 " & _
" 		update TSPL_JOURNAL_MASTER set Type='Apply Document Receipt' where Source_Doc_No=@Receipt_No " & _
" end " & _
" end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgReceiptDetail", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgReceiptDetail] ON [dbo].[TSPL_RECEIPT_DETAIL] " & _
" FOR Update,Insert " & _
" AS " & _
 " declare @Receipt_No varchar(30),@Receipt_Type char(1),@Receipt_Date date,@Cust_Code varchar(12),@Customer_Name varchar(50),@Bank_Code varchar(12),@BankName varchar(50),@LocCode varchar(12),@LocName varchar(50),@BankAcctCode varchar(30),@BankAcctDesc varchar(50),@Cheque_No varchar(20),@Cheque_Date varchar(10),@Narration varchar(200),@GlAcct varchar(30),@GlAcctName varchar(50),@Receipt_Amount decimal(18,2),@Posted char(1),@NarrationDetail varchar(200),@Applied_Amount decimal(18,2),@Receipt_Line_No int " & _
 " select @Receipt_No=Receipt_No,@Receipt_Line_No=Receipt_Line_No from inserted " & _
 " select @Receipt_No=Receipt_No,@Receipt_type=Receipt_Type,@Posted=Posted,@Cust_Code=Cust_Code from TSPL_RECEIPT_HEADER where Receipt_No=@Receipt_No " & _
 " if @Receipt_type='M'  " & _
 " begin " & _
 " SELECT  @Receipt_Date=TSPL_RECEIPT_HEADER.Receipt_Date, @Cust_Code=TSPL_RECEIPT_HEADER.Cust_Code,  " & _
   "                    @Customer_Name=TSPL_RECEIPT_HEADER.Customer_Name,@Bank_Code= TSPL_RECEIPT_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date  " & _
     "                  FROM         TSPL_RECEIPT_HEADER INNER JOIN " & _
       "                TSPL_BANK_MASTER ON TSPL_RECEIPT_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
         "              TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code  where Receipt_No=@Receipt_No      " & _
  " select @Applied_Amount=Applied_Amount,@NarrationDetail=Remarks,@Receipt_Amount=Applied_Amount,@GlAcct=Account_Code,@GlAcctName=Description from TSPL_RECEIPT_DETAIL where Receipt_No=@Receipt_No   and Receipt_Line_No=@Receipt_Line_No " & _
" Delete from TSPL_BANK_BOOK where SOURCEDOC_NO=@Receipt_No and GL_Account_Code=@GlAcct and line_No=@Receipt_Line_No " & _
" insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Receipt_No,@Receipt_Date,@Cust_Code,@Customer_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,@Cheque_Date,@Narration,@NarrationDetail,0,@Applied_Amount,'Receipt','M',@Receipt_Line_No) " & _
" END " & _
" if @Receipt_type='S'  " & _
" begin " & _
" SELECT  @Receipt_Date=TSPL_RECEIPT_HEADER.Receipt_Date, @Cust_Code=TSPL_RECEIPT_HEADER.Cust_Code,  " & _
                      " @Customer_Name=TSPL_RECEIPT_HEADER.Customer_Name,@Bank_Code= TSPL_RECEIPT_HEADER.Bank_Code,@BankName= TSPL_BANK_MASTER.DESCRIPTION ,@LocCode=RIGHT(TSPL_BANK_MASTER.BANKACC, 3), @LOCNAME= TSPL_GL_SEGMENT_CODE.Description ,@BankAcctCode=TSPL_BANK_MASTER.BANKACC ,@BankAcctDesc=TSPL_GL_ACCOUNTS.Description ,@Narration=Narration,@Cheque_No=Cheque_No,@Cheque_Date=Cheque_Date  " & _
                      " FROM         TSPL_RECEIPT_HEADER INNER JOIN " & _
                      " TSPL_BANK_MASTER ON TSPL_RECEIPT_HEADER.Bank_Code = TSPL_BANK_MASTER.BANK_CODE INNER JOIN " & _
                      " TSPL_GL_SEGMENT_CODE ON RIGHT(TSPL_BANK_MASTER.BANKACC, 3) = TSPL_GL_SEGMENT_CODE.Segment_code inner join TSPL_GL_ACCOUNTS on TSPL_BANK_MASTER.BANKACC=TSPL_GL_ACCOUNTS.Account_Code  where Receipt_No=@Receipt_No      " & _
" select @Applied_Amount=Applied_Amount,@NarrationDetail=Remarks,@Receipt_Amount=Applied_Amount,@GlAcct=Account_Code,@GlAcctName=Description from TSPL_RECEIPT_DETAIL where Receipt_No=@Receipt_No   and Receipt_Line_No=@Receipt_Line_No " & _
" Delete from TSPL_BANK_BOOK where SOURCEDOC_NO=@Receipt_No and GL_Account_Code=@GlAcct and line_No=@Receipt_Line_No " & _
" insert into TSPL_BANK_BOOK(SOURCEDOC_NO,SOURCEDOC_DATE,SOURCE_CODE,SOURCE_NAME,BANK_CODE,BANK_NAME,LOC_CODE,LOC_NAME,BANKGL_Account_Code,BANKGL_Account_Name,GL_Account_Code,GL_Account_Name,CHEQUE_NO,CHEQUE_DATE,NARR_MASTER,NARR_DETAIL,Credit_Amount,Debit_Amount,DocType,TransactionType,line_No) values (@Receipt_No,@Receipt_Date,@Cust_Code,@Customer_Name,@Bank_Code,@BankName,@LocCode,@LocName,@BankAcctCode,@BankAcctDesc,@GlAcct,@GlAcctName,@Cheque_No,convert(varchar,@Cheque_Date,103),@Narration,@NarrationDetail,@Applied_Amount,0,'Receipt','S',@Receipt_Line_No) " & _
" END "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_UniqueARInvoiceNowithDocNO", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[trg_UniqueARInvoiceNowithDocNO] ON  [dbo].[TSPL_Customer_Invoice_Head] for insert  AS  declare @POstFlag as integer  declare @Against_Sale_No as varchar(30) Select @Against_Sale_No=i.Against_Sale_No from inserted i; select @POstFlag=count(*) from TSPL_Customer_Invoice_Head where Against_Sale_No  =@Against_Sale_No  and (Trans_Type ='BS' or Trans_Type ='BST') if  @POstFlag>1  BEGIN raiserror ('Cannot create duplicate entry',16,1) End  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        '            If clsPostCreateTable.CheckTriggerExits("TrgARInvoiceTransType", Nothing) = 0 Then
                                        '                CreateAletr = "Create "
                                        '            Else
                                        '                CreateAletr = "Alter "
                                        '            End If
                                        '            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgARInvoiceTransType] ON [dbo].[TSPL_Customer_Invoice_Head] " & _
                                        '" FOR Update " & _
                                        '" AS " & _
                                        '"  declare @Posting_Date datetime, @Document_No varchar(30),@Document_Type char(1),@Bank_Type char(1),@Bank_Code varchar(12) " & _
                                        '" select @Posting_Date=Posting_Date,@Document_No=Document_No,@Document_Type=Document_Type from inserted " & _
                                        '" if @Posting_Date <> '' " & _
                                        '" begin  " & _
                                        '" if @Document_Type='I' " & _
                                        '" begin 		 " & _
                                        '" 		update TSPL_JOURNAL_MASTER set Type='Invoice AR' where Source_Doc_No=@Document_No " & _
                                        '" end " & _
                                        '" if @Document_Type='D' " & _
                                        '" begin 	 " & _
                                        '" 		update TSPL_JOURNAL_MASTER set Type='DebitNote AR' where Source_Doc_No=@Document_No " & _
                                        '" end " & _
                                        '" if @Document_Type='C' " & _
                                        '" begin 	 " & _
                                        '" 		update TSPL_JOURNAL_MASTER set Type='CreditNote AR' where Source_Doc_No=@Document_No		 " & _
                                        '" end " & _
                                        '" end "
                                        '            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            ''UDL/16/05/18-000167
            If clsPostCreateTable.CheckTriggerExits("TrgARInvoiceTransType", Nothing) = 1 Then
                CreateAletr = "drop trigger TrgARInvoiceTransType"
                clsDBFuncationality.ExecuteNonQuery(CreateAletr)
                                        End If

            If clsPostCreateTable.CheckTriggerExits("TrgARInvoice_SaveUpdate", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " Trigger [dbo].[TrgARInvoice_SaveUpdate] ON [dbo].[TSPL_Customer_Invoice_Head] For Insert, Update as Declare @Document_No Varchar(30), @Currency_Code Varchar(30) Select @Document_No=Document_No, @Currency_Code=CURRENCY_CODE From inserted  if ISNULL(@Currency_Code,'')='' Begin Update TSPL_CUSTOMER_INVOICE_HEAD Set CURRENCY_CODE='INR', ConvRate=1.0 WHERE Document_No=@Document_No End  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgSRNTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgSRNTransType] ON [dbo].[TSPL_SRN_HEAD] FOR Update AS declare @Posting_Date datetime, @SRN_No varchar(30),@Item_Type char(1) select @Posting_Date=Posting_Date,@SRN_No=SRN_No,@Item_Type=Item_Type from inserted if @Posting_Date <> '' begin  if @Item_Type='F' begin 	 update TSPL_JOURNAL_MASTER set Type='SRN Trading Item' where Source_Doc_No=@SRN_No			 end if @Item_Type='O' begin  update TSPL_JOURNAL_MASTER set Type='SRN RM and Others' where Source_Doc_No=@SRN_No		 end  end  "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("trg_CreateChargeHistory_update", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "   trigger [dbo].[trg_CreateChargeHistory_update] on [dbo].[TSPL_MCC_VSP_ChargeCategory_MAPPING]  for update   as  begin try    declare @VSP_Code as varchar(30)  declare @Charge_Code as varchar(30)  Select @VSP_Code=i.VSP_COde from deleted i; Select @Charge_Code=i.Charge_COde from deleted i;   insert into TSPL_MCC_VSP_ChargeCategory_MAPPING_history(vsp_code,charge_code,Rate,history_By,History_date)   select vsp_code,charge_code,Rate,(select Max(Modify_By) from tspl_Vendor_master where Vendor_code =@VSP_Code),(select Max(Updated_date)  from TSPL_MCC_VSP_ChargeCategory_MAPPING where Vsp_code =@VSP_Code and Charge_CODE=@charge_code) from deleted    end try   begin catch  end catch   "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

            If clsPostCreateTable.CheckTriggerExits("TrgPurchaseReturnTransType", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & " TRIGGER [dbo].[TrgPurchaseReturnTransType] ON [dbo].[TSPL_PR_HEAD]" & _
" FOR Update" & _
" AS" & _
 " declare @Posting_Date datetime, @PR_No varchar(30),@Item_Type char(1)" & _
" select @Posting_Date=Posting_Date,@PR_No=PR_No,@Item_Type=Item_Type from inserted" & _
" if @Posting_Date <> ''" & _
" begin " & _
" if @Item_Type='F'" & _
" begin 	" & _
        " update TSPL_JOURNAL_MASTER set Type='Purchase Return Trading Item' where Source_Doc_No=@PR_No			" & _
" end" & _
" if @Item_Type='O'" & _
" begin " & _
    " 	update TSPL_JOURNAL_MASTER set Type='Purchase Return RM and Others' where Source_Doc_No=@PR_No		" & _
" end " & _
"   end "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        ' For Bank transfer Delete
            If clsPostCreateTable.CheckTriggerExits("TrgBankTransferDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgBankTransferDelete] ON [dbo].[TSPL_BANK_TRANSFER] FOR delete AS declare @Transfer_No varchar(30)  " & _
            "Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO in (select Transfer_No from deleted ) "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        ' For Bank Reverse Delete
            If clsPostCreateTable.CheckTriggerExits("TrgBankReverseDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgBankReverseDelete] ON [dbo].[TSPL_BANK_REVERSE] FOR Delete AS declare @Reverse_Code varchar(30) " & _
            "Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO in (select  Reverse_Code from deleted  ) "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        ' For Payment Delete
            If clsPostCreateTable.CheckTriggerExits("TrgPaymentHeaderDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgPaymentHeaderDelete] ON [dbo].[TSPL_PAYMENT_HEADER] FOR Delete AS  declare @Payment_No varchar(30)  " & _
            "Delete from  TSPL_BANK_BOOK where SOURCEDOC_NO in (select Payment_No from deleted ) "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        ' For Receipt Delete
            If clsPostCreateTable.CheckTriggerExits("TrgReceiptHeaderDelete", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = "" & CreateAletr & "  TRIGGER [dbo].[TrgReceiptHeaderDelete] ON [dbo].[TSPL_RECEIPT_HEADER] FOR Delete AS  declare @Receipt_No varchar(30) " & _
            "Delete from  TSPL_BANK_BOOK where  SOURCEDOC_NO in (select Receipt_No from deleted  ) "
            clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        'If clsPostCreateTable.CheckTriggerExits("Production_Entry", Nothing) = 0 Then
                                        '    CreateAletr = "Create "
                                        'Else
                                        '    CreateAletr = "Alter "
                                        'End If
                                        'qryTrig = "" & CreateAletr & " ALTER trigger [dbo].[Production_Entry] on [dbo].[TSPL_PP_PRODUCTION_ENTRY]  for insert,update as  begin  update TSPL_PP_PRODUCTION_ENTRY set RECEIVED_BY='E0001' where isnull(RECEIVED_BY,'')='' end   "
                                        'clsDBFuncationality.ExecuteNonQuery(qryTrig, Nothing)

            clsCommon.ProgressBarHide()

            If clsPostCreateTable.CheckTriggerExits("trg_Inventory_Movement_New", Nothing) = 0 Then
                CreateAletr = "Create "
            Else
                CreateAletr = "Alter "
                                        End If
            qryTrig = " " & CreateAletr & " trigger [dbo].[trg_Inventory_Movement_New] on [dbo].[TSPL_INVENTORY_MOVEMENT_NEW] for Insert,UPDATE, DELETE  as " & _
                      " declare @Msg as varchar(max); " & _
                      " declare @Location_Code as varchar(50);" & _
                      " declare @Punching_Date as Date;       " & _
                      " declare @SOURCE_DOC_NO as varchar(50);" & _
                      " declare @FAT_KG as numeric(18,3);     " & _
                      " declare @SNF_KG as numeric(18,3);     " & _
                      " declare @IO as varchar(1);            " & _
                      " declare @Item_Code  varchar(50);      " & _
                      " declare @Stock_UOM varchar(30);       " & _
                      " declare @Stock_Qty Numeric(18,2);     " & _
                      " declare @UOM  varchar(30);            " & _
                      " declare @Sett varchar(1);             " & _
                      " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='UpdateInventorySummaryTable';  if coalesce(@Sett,'')<>'1'  return;" & _
                      " INSERT into TSPL_INVENTORY_MOVEMENT_NEW_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date, " & _
                      " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost, " & _
                      " Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code," & _
                      " Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,OP_TYPE) " & _
                      " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost, " & _
                      " Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date, " & _
                      " PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name, " & _
                      " Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,'I' from inserted; " & _
                      " INSERT into TSPL_INVENTORY_MOVEMENT_NEW_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date," & _
                      " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost, " & _
                      " Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code, " & _
                      " Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,OP_TYPE) " & _
                      " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost, " & _
                      " Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost, " & _
                      " Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code, " & _
                      " Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,'D' from deleted; " & _
                      " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='FatSNFStockControl'; " & _
                      " if coalesce(@Sett,'')<>'1' " & _
                      " return; " & _
                      " IF (SELECT count(*) FROM deleted)>0 " & _
                      " BEGIN " & _
                      " DECLARE cursorName CURSOR LOCAL SCROLL STATIC FOR " & _
                      " Select Item_Code,InOut,Stock_UOM,Stock_Qty,UOM,Location_Code, cast(Punching_Date as Date) as Punching_Date,Source_Doc_No FROM deleted " & _
                      " OPEN cursorName " & _
                      " FETCH NEXT FROM cursorName " & _
                      " INTO @Item_Code,@IO,@Stock_UOM,@Stock_Qty,@UOM,@Location_Code, @Punching_Date,@SOURCE_DOC_NO " & _
                      " if @IO='I' " & _
                      " begin " & _
                      " SET @Msg= [dbo].[Check_Stock_OnReverse](@Item_Code,@Location_Code ,@Punching_Date,@FAT_KG,@SNF_KG,@Stock_Qty); " & _
                      " if len(@Msg)>0  " & _
                      " begin " & _
                      " set @Msg ='Qty/Fat/SNF Stock will be negative due to delete/update of ' + 'Doc No :' +@SOURCE_DOC_NO + ' on Date:'+ @Msg + ',Item Code '" & _
                      " + @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM ;  " & _
                      " rollback tran raiserror (@Msg,16,1)   return; " & _
                      " End " & _
                      " End " & _
                      " WHILE @@FETCH_STATUS = 0 " & _
                      " BEGIN " & _
                      " FETCH NEXT FROM cursorName " & _
                      " INTO @Item_Code,@IO,@Stock_UOM,@Stock_Qty,@UOM,@Location_Code, @Punching_Date,@SOURCE_DOC_NO; " & _
                      " if @IO='I' " & _
                      " begin " & _
                      " SET @Msg= [dbo].[Check_Stock_OnReverse](@Item_Code,@Location_Code ,@Punching_Date,@FAT_KG,@SNF_KG,@Stock_Qty); " & _
                      " if len(@Msg)>0  " & _
                      " begin " & _
                      " set @Msg ='Qty/Fat/SNF Stock will be negative due to delete/update of ' + 'Doc No :' +@SOURCE_DOC_NO + ' on Date:'+ @Msg + ',Item Code ' " & _
                      " + @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM ;" & _
                      " rollback tran raiserror (@Msg,16,1)  return; " & _
                      " End " & _
                      " End " & _
                      " End " & _
                      " CLOSE cursorName; " & _
                      " DEALLOCATE cursorName; " & _
                      " End " & _
                      " else " & _
                      " begin " & _
                      " Select @Item_Code=I.Item_Code,@Stock_UOM=I.Stock_UOM,@UOM=I.UOM,@Location_Code=I.Location_Code,@Punching_Date=cast(I.Punching_Date as date),@SOURCE_DOC_NO=I.Source_Doc_No,@FAT_KG=I.Fat_KG, " & _
                      " @SNF_KG=I.SNF_KG,@IO=I.InOut,@Stock_Qty=I.Stock_Qty from inserted I " & _
                      " if (cast(@Punching_Date as date)< cast(GETDATE() as date)) " & _
                      " begin " & _
                      " set @Msg=[dbo].[Check_Stock_OnReverse](@Item_Code,@Location_Code ,@Punching_Date,@FAT_KG,@SNF_KG,@Stock_Qty); " & _
                      " if len(@Msg)>0  " & _
                      " begin  set @Msg ='Qty/Fat/SNF Stock will be negative due to back date transaction ' + 'Doc No :' +@SOURCE_DOC_NO + ' on Date:'+ " & _
                      " @Msg + ',Item Code ' + @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM ;" & _
                      " rollback tran raiserror (@Msg,16,1)  " & _
                      " return;  " & _
                      " End " & _
                      " End " & _
                      " if (@IO='I' or (@Stock_Qty<=0 and @FAT_KG<=0 and @SNF_KG<=0)) return; " & _
                      " select @Msg=   [dbo].[Get_Location_FatSNF](@Item_Code,@Location_Code ,@Punching_Date,@SOURCE_DOC_NO,@FAT_KG,@SNF_KG,@Stock_Qty) " & _
                      " if len(@Msg)>0  " & _
                      " Begin " & _
                      " set @Msg ='Insufficient Qty/Fat/SNF Stock: ' + 'Doc No :' +@SOURCE_DOC_NO + ', Date:'+ cast(@Punching_Date as varchar) + ', Item Code:'+ @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM + char(10)+ @Msg; " & _
                      " rollback tran raiserror (@Msg,16,1) " & _
                      " End " & _
                      " End "
                clsDBFuncationality.ExecuteNonQuery(qryTrig)

                If clsPostCreateTable.CheckTriggerExits("trg_Inventory_Movement", Nothing) = 0 Then
                    CreateAletr = "Create "
                Else
                    CreateAletr = "Alter "
                                        End If
            qryTrig = " " & CreateAletr & " trigger [dbo].[trg_Inventory_Movement] on [dbo].[TSPL_INVENTORY_MOVEMENT] for Insert,UPDATE, DELETE  as " & _
                      " declare @Msg as varchar(max); " & _
                      " declare @Location_Code as varchar(50); " & _
                      " declare @Punching_Date as Date; " & _
                      " declare @SOURCE_DOC_NO as varchar(50); " & _
                      " declare @FAT_KG as numeric(18,3); " & _
                      " declare @SNF_KG as numeric(18,3); " & _
                      " declare @IO as varchar(1); " & _
                      " declare @Fat_Per numeric(18,3); " & _
                      " declare @SNF_Per numeric(18,3); " & _
                      " declare @Item_Code  varchar(50); " & _
                      " declare @Stock_UOM varchar(30); " & _
                      " declare @Stock_Qty Numeric(18,2);     " & _
                      " declare @UOM  varchar(30); " & _
                      " declare @Sett varchar(1); " & _
                      " declare @Product_Type varchar(20) " & _
                      " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='UpdateInventorySummaryTable';  if coalesce(@Sett,'')<>'1'  return;" & _
                      " INSERT into TSPL_INVENTORY_MOVEMENT_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date, " & _
                      " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date, " & _
                      " PI_Cost,Stock_UOM,Stock_Qty,MFG_Date,Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code, " & _
                      " Other_Location_Desc,OP_TYPE,Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt) " & _
                      " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost,Add_Cost," & _
                      " Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,MFG_Date, " & _
                      " Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,'D',Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt from deleted " & _
                      " INSERT into TSPL_INVENTORY_MOVEMENT_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date, " & _
                      " Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM, " & _
                      " Stock_Qty,MFG_Date,Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,OP_TYPE,Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt) " & _
                      " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost, " & _
                      " Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,MFG_Date,Expiry_Date,Item_Status, " & _
                      " Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,'I',Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt from inserted;" & _
                      " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='FatSNFStockControl'; " & _
                      " if coalesce(@Sett,'')<>'1' " & _
                      " return; " & _
                      " IF (SELECT count(*) FROM deleted)>0 " & _
                      " BEGIN " & _
                      " DECLARE cursorName CURSOR LOCAL SCROLL STATIC FOR " & _
                      " Select Item_Code,InOut,Stock_UOM,Stock_Qty,UOM,Location_Code, cast(Punching_Date as Date) as Punching_Date,Source_Doc_No FROM deleted " & _
                      " OPEN cursorName " & _
                      " FETCH NEXT FROM cursorName " & _
                      " INTO @Item_Code,@IO,@Stock_UOM,@Stock_Qty,@UOM,@Location_Code, @Punching_Date,@SOURCE_DOC_NO " & _
                      " if @IO='I' " & _
                      " begin " & _
                      " SET @Msg= [dbo].[Check_Stock_OnReverse](@Item_Code,@Location_Code ,@Punching_Date,0,0,@Stock_Qty); " & _
                      " if len(@Msg)>0  " & _
                      " begin " & _
                      " set @Msg ='Qty/Fat/SNF Stock will be negative due to delete/update of ' + 'Doc No :' +@SOURCE_DOC_NO + ' on Date:'+ @Msg + ',Item Code '" & _
                      " + @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM ;  " & _
                      " rollback tran raiserror (@Msg,16,1)   return; " & _
                      " End " & _
                      " End " & _
                      " WHILE @@FETCH_STATUS = 0 " & _
                      " BEGIN " & _
                      " FETCH NEXT FROM cursorName " & _
                      " INTO @Item_Code,@IO,@Stock_UOM,@Stock_Qty,@UOM,@Location_Code, @Punching_Date,@SOURCE_DOC_NO; " & _
                      " if @IO='I' " & _
                      " begin " & _
                      " SET @Msg= [dbo].[Check_Stock_OnReverse](@Item_Code,@Location_Code ,@Punching_Date,0,0,@Stock_Qty); " & _
                      " if len(@Msg)>0  " & _
                      " begin " & _
                      " set @Msg ='Qty/Fat/SNF Stock will be negative due to delete/update of ' + 'Doc No :' +@SOURCE_DOC_NO + ' on Date:'+ @Msg + ',Item Code ' " & _
                      " + @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM ;" & _
                      " rollback tran raiserror (@Msg,16,1)  return; " & _
                      " End " & _
                      " End " & _
                      " End " & _
                      " CLOSE cursorName; " & _
                      " DEALLOCATE cursorName; " & _
                      " End " & _
                      " else " & _
                      " select @Item_Code=I.Item_Code,@IO=i.InOut,@Stock_UOM=I.Stock_UOM,@Stock_Qty=i.Stock_Qty,@UOM=i.UOM,@Product_Type=tspl_item_master.Product_Type,@Location_Code=i.Location_Code," & _
                      " @Punching_Date=cast(i.Punching_Date as date),@SOURCE_DOC_NO=i.Source_Doc_No from inserted I " & _
                      " left join tspl_item_master on i.Item_Code=tspl_item_master.Item_Code; " & _
                      " /*Select @Location_Code=i.Location_Code,@Punching_Date=cast(i.Punching_Date as date),@SOURCE_DOC_NO=i.Source_Doc_No,@Fat_Per=coalesce(Item_Fat.Fat_Per,0), " & _
                      " @SNF_Per=coalesce(Item_SNF.SNF_Per,0), " & _
                      " @FAT_KG=round(((case when coalesce(StockKG.Conversion_Factor,0)=0 then 0 else " & _
                      " cast((i.Stock_Qty*coalesce(Item_Fat.Fat_Per,0)*Stock_SU.Conversion_Factor)/ " & _
                      " (coalesce(StockKG.Conversion_Factor,1)*100) as numeric(18,3)) end)),3)," & _
                      " @SNF_KG=round(((case when coalesce(StockKG.Conversion_Factor,0)=0 then 0 " & _
                      " else cast((i.Stock_Qty*coalesce(Item_SNF.SNF_Per,0)*Stock_SU.Conversion_Factor)/ " & _
                      " (coalesce(StockKG.Conversion_Factor,1)*100) as Numeric(18,3)) end)),3),@IO=i.InOut,@Item_Code=i.Item_Code,@Stock_Qty=i.Stock_Qty from inserted i " & _
                      " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL) as Stock_SU on @Item_Code=Stock_SU.Item_Code and @Stock_UOM=Stock_SU.UOM_Code " & _
                      " left join (select Item_Code,UOM_Code,Conversion_Factor from TSPL_ITEM_UOM_DETAIL where UOM_Code='KG') as StockKG on @Item_Code=StockKG.Item_Code " & _
                      " left join (select Item_QC.Item_Code,coalesce(max(Item_QC.Actual_Range),0) as Fat_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & _
                      " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='FAT' " & _
                      " and Item_QC.Item_Code=@Item_Code group by Item_QC.Item_Code) Item_Fat on @Item_Code=Item_Fat.Item_Code " & _
                      " left join (select Item_QC.Item_Code,coalesce(max(Item_QC.Actual_Range),0) as SNF_Per from TSPL_ITEM_QC_PARAMETER_MASTER Item_QC " & _
                      " left outer join TSPL_PARAMETER_MASTER Params on Params.Code=Item_QC.Code where Params.Type='SNF' " & _
                      " and Item_QC.Item_Code=@Item_Code group by Item_QC.Item_Code) Item_SNF on @Item_Code=Item_SNF.Item_Code */" & _
                      " if (@IO='I' or @Stock_Qty<=0)  Return;" & _
                      " if  @IO='O' and (@Product_Type='MP' or @Product_Type='MI') return; " & _
                      " begin " & _
                      " select @Msg=[dbo].[Get_Location_FatSNF](@Item_Code,@Location_Code ,@Punching_Date,@SOURCE_DOC_NO,0,0,@Stock_Qty)  " & _
                      " if len(@Msg)>0  " & _
                      " Begin " & _
                      " set @Msg ='Insufficient Qty/Fat/SNF Stock: ' + 'Doc No :' +@SOURCE_DOC_NO + ', Date:'+ cast(@Punching_Date as varchar) + ', Item Code:'+ @Item_Code + ', Location Code:'+ @Location_Code + ', UOM Code :' + @UOM + char(10)+ @Msg; " & _
                      " rollback tran raiserror (@Msg,16,1) " & _
                      " End " & _
                      " End "
                clsDBFuncationality.ExecuteNonQuery(qryTrig)

                If clsPostCreateTable.CheckTriggerExits("trigDuplicateReceiptNoOnSampleHead", Nothing) = 0 Then
                    CreateAletr = "Create "
                Else
                    CreateAletr = "Alter "
                                        End If
                qryTrig = "" & CreateAletr & "   trigger [dbo].[trigDuplicateReceiptNoOnSampleHead] on [dbo].[TSPL_MILK_SAMPLE_HEAD] for Insert,update as" + _
                " declare @i as integer,@DOC_CODE as varchar(100),@MILK_RECEIPT_CODE as varchar(100),@OLDDOCNO as varchar(100) " + _
                " select @DOC_CODE=DOC_CODE,@MILK_RECEIPT_CODE=MILK_RECEIPT_CODE from inserted" + _
                " select @OLDDOCNO=max(DOC_CODE) from TSPL_MILK_SAMPLE_HEAD where MILK_RECEIPT_CODE=@MILK_RECEIPT_CODE and DOC_CODE<>@DOC_CODE" + _
                " if  @OLDDOCNO is not null" + _
                " begin" + _
                " Print 'Sample No : ' + @OLDDOCNO +' already generated '" + _
                " raiserror ('Duplicate entry generating' ,16,1) end "

                clsDBFuncationality.ExecuteNonQuery(qryTrig)


                If clsPostCreateTable.CheckTriggerExits("TRG_JD_FiscaYearEndNoUpdateNoDelete", Nothing) = 0 Then
                    CreateAletr = "Create "
                Else
                    CreateAletr = "Alter "
                                        End If
                qryTrig = "" & CreateAletr & "   trigger [dbo].[TRG_JD_FiscaYearEndNoUpdateNoDelete] on [dbo].[TSPL_JOURNAL_DETAILS] for update,delete" + Environment.NewLine + _
    " as declare  @Count as integer,@VoucherNo as varchar(100),@Msg as varchar(200)" + Environment.NewLine + _
    "select  @Count= count(*), @VoucherNo=max(deleted.Voucher_No) from deleted" + Environment.NewLine + _
    "left outer join TSPL_JOURNAL_MASTER on TSPL_JOURNAL_MASTER.Voucher_No=deleted.Voucher_No" + Environment.NewLine + _
    "left outer join TSPL_Fiscal_Year_Master on TSPL_Fiscal_Year_Master.Start_Date<= TSPL_JOURNAL_MASTER.Voucher_Date and TSPL_JOURNAL_MASTER.Voucher_Date<=TSPL_Fiscal_Year_Master.End_Date" + Environment.NewLine + _
    "where TSPL_Fiscal_Year_Master.is_End_Year_Proceed=1 and TSPL_JOURNAL_MASTER.Authorized='A'" + Environment.NewLine + _
    "if len(isnull(@VoucherNo,'')) >0 " + Environment.NewLine + _
    "begin" + Environment.NewLine + _
    "rollback tran " + Environment.NewLine + _
    "set @Msg ='Year End Processed.You can not change financial entry.Voucher No-' + @VoucherNo " + Environment.NewLine + _
    "raiserror (@Msg,16,1) " + Environment.NewLine + _
    "end"
                clsDBFuncationality.ExecuteNonQuery(qryTrig)






                If clsPostCreateTable.CheckTriggerExits("TRG_JM_FiscaYearEndNoUpdateNoDelete", Nothing) = 0 Then
                    CreateAletr = "Create "
                Else
                    CreateAletr = "Alter "
                                        End If
                qryTrig = "" & CreateAletr & "   trigger [dbo].[TRG_JM_FiscaYearEndNoUpdateNoDelete] on [dbo].[TSPL_JOURNAL_MASTER] for update,delete" + Environment.NewLine + _
    "as declare @Count as integer,@VoucherNo as varchar(100),@Msg as varchar(200)" + Environment.NewLine + _
    "select @Count= count(*),@VoucherNo=max(deleted.Voucher_No) from deleted" + Environment.NewLine + _
    "left outer join TSPL_Fiscal_Year_Master on TSPL_Fiscal_Year_Master.Start_Date<= deleted.Voucher_Date and deleted.Voucher_Date<=TSPL_Fiscal_Year_Master.End_Date" + Environment.NewLine + _
    "where TSPL_Fiscal_Year_Master.is_End_Year_Proceed=1 and deleted.Authorized='A'  " + Environment.NewLine + _
    "if @Count >0 " + Environment.NewLine + _
    "begin" + Environment.NewLine + _
    "rollback tran " + Environment.NewLine + _
    "set @Msg ='Year End Processed.You can not change financial entry.Voucher No-' + @VoucherNo " + Environment.NewLine + _
    "raiserror (@Msg,16,1) " + Environment.NewLine + _
    "end"
                clsDBFuncationality.ExecuteNonQuery(qryTrig)

            '                        ''Delete due to setoff of customer and vendor as said by ranjan mam on 04/10/2017
            'If clsCommon.CompairString(clsCommon.myCstr(clsDBFuncationality.getSingleValue("select Comp_Code  from tspl_company_master")), "UDL") = CompairStringResult.Equal Then
            '    Try
            '        clsDBFuncationality.ExecuteNonQuery("drop trigger TRG_JD_FiscaYearEndNoUpdateNoDelete")
            '    Catch ex As Exception
            '                            End Try
            '    Try
            '        clsDBFuncationality.ExecuteNonQuery("drop trigger TRG_JM_FiscaYearEndNoUpdateNoDelete")
            '    Catch ex As Exception
            '                            End Try
            '                        End If



                                        '' '' triggers for inventory management
                                        'If clsPostCreateTable.CheckTriggerExits("trg_Inventory_Movement_WIN", Nothing) = 0 Then
                                        '    CreateAletr = "Create "
                                        'Else
                                        '    CreateAletr = "Alter "
                                        'End If
                                        'qryTrig = "" & CreateAletr & " trigger [dbo].[trg_Inventory_Movement_WIN] on [dbo].[TSPL_INVENTORY_MOVEMENT] AFTER INSERT,UPDATE, DELETE as  " & _
                                        '          " declare @Sett varchar(1);" & _
                                        '          " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='UpdateInventorySummaryTable';  if coalesce(@Sett,'')<>'1'  return;" & _
                                        '          " INSERT into TSPL_INVENTORY_MOVEMENT_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date, " & _
                                        '          " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date, " & _
                                        '          " PI_Cost,Stock_UOM,Stock_Qty,MFG_Date,Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code, " & _
                                        '          " Other_Location_Desc,OP_TYPE,Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt) " & _
                                        '          " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost,Add_Cost," & _
                                        '          " Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,MFG_Date, " & _
                                        '          " Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,'D',Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt from deleted " & _
                                        '          " INSERT into TSPL_INVENTORY_MOVEMENT_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date, " & _
                                        '          " Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM, " & _
                                        '          " Stock_Qty,MFG_Date,Expiry_Date,Item_Status,Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,OP_TYPE,Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt) " & _
                                        '          " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost, " & _
                                        '          " Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,MFG_Date,Expiry_Date,Item_Status, " & _
                                        '          " Assmbly_Status,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,'I',Fat_Per,SNF_Per,Fat_KG,SNF_KG,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt from inserted;"
                                        'clsDBFuncationality.ExecuteNonQuery(qryTrig)

                                        'If clsPostCreateTable.CheckTriggerExits("trg_Inventory_Movement_NEW_WIN", Nothing) = 0 Then
                                        '    CreateAletr = "Create "
                                        'Else
                                        '    CreateAletr = "Alter "
                                        'End If
                                        'qryTrig = "" & CreateAletr & " trigger [dbo].[trg_Inventory_Movement_NEW_WIN] on [dbo].[TSPL_INVENTORY_MOVEMENT_NEW] AFTER INSERT,UPDATE, DELETE as  " & _
                                        '          " declare @Sett varchar(1);" & _
                                        '          " select @Sett=Description from TSPL_FIXED_PARAMETER where Type='UpdateInventorySummaryTable';  if coalesce(@Sett,'')<>'1'  return;" & _
                                        '          " INSERT into TSPL_INVENTORY_MOVEMENT_NEW_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date, " & _
                                        '          " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost, " & _
                                        '          " Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code," & _
                                        '          " Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,OP_TYPE) " & _
                                        '          " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost, " & _
                                        '          " Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date, " & _
                                        '          " PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name, " & _
                                        '          " Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,'I' from inserted; " & _
                                        '          " INSERT into TSPL_INVENTORY_MOVEMENT_NEW_WIN(Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date," & _
                                        '          " Entry_Date,Basic_Cost,Rec_Cost,Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost, " & _
                                        '          " Avg_Cost,Posting_Date,PI_Cost,Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code, " & _
                                        '          " Cust_Name,Vendor_Code,Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,OP_TYPE) " & _
                                        '          " select Trans_Id,Trans_Type,InOut,Location_Code,Item_Code,Item_Desc,Qty,UOM,Source_Doc_No,Source_Doc_Date,Entry_Date,Basic_Cost,Rec_Cost, " & _
                                        '          " Add_Cost,Net_Cost,Created_By,Comp_Code,ItemType,Punching_Date,MRP,Batch_No,MFG_Date,Expiry_Date,FIFO_Cost,LIFO_Cost,Avg_Cost,Posting_Date,PI_Cost, " & _
                                        '          " Stock_UOM,Stock_Qty,Item_Status,Assmbly_Status,Fat_Per,SNF_Per,Fat_KG,SNF_KG,main_location,IS_CONSUMPTION,Cust_Code,Cust_Name,Vendor_Code, " & _
                                        '          " Vendor_Name,Other_Location_Code,Other_Location_Desc,Fat_Rate,SNF_Rate,Fat_Amt,SNF_Amt,Std_Qty,'D' from deleted; "
                                        'clsDBFuncationality.ExecuteNonQuery(qryTrig)
                                        ' '' end triggers for inventory management


                If clsPostCreateTable.CheckTriggerExits("Trig_Check_Duplicate_AR_Entry", Nothing) = 0 Then
                    CreateAletr = "Create "
                Else
                    CreateAletr = "Alter "
                                        End If
                qryTrig = "" & CreateAletr & "   trigger [dbo].[Trig_Check_Duplicate_AR_Entry] on [dbo].[TSPL_Customer_Invoice_Head] after insert" + Environment.NewLine + _
                            " as " + Environment.NewLine + _
                    " begin " + Environment.NewLine + _
                    " declare @Count int " + Environment.NewLine + _
                    " declare @Against_Sale_No VARCHAR(25) " + Environment.NewLine + _
                    " declare @AR_Doc_No VARCHAR(25) " + Environment.NewLine + _
                    " declare @message VARCHAR(200) " + Environment.NewLine + _
                    " SELECT @Against_Sale_No = INSERTED.Against_Sale_No FROM INSERTED " + Environment.NewLine + _
                    " SELECT @AR_Doc_No = INSERTED.Document_No FROM INSERTED " + Environment.NewLine + _
                    " if @Against_Sale_No <> '' " + Environment.NewLine + _
                    " begin " + Environment.NewLine + _
                    " select @Count = count(*) from TSPL_Customer_Invoice_Head where Trans_Type='FS'and Document_Type='I' AND Against_Sale_No= @Against_Sale_No " + Environment.NewLine + _
                    " if @Count > 1 " + Environment.NewLine + _
                    " begin " + Environment.NewLine + _
                    " select @message= 'AR Invoive Entry-'+ @AR_Doc_No + ' already exist agaist Sale Invoice-' + @Against_Sale_No +'' " + Environment.NewLine + _
                    " raiserror(@message, 15, 1) " + Environment.NewLine + _
                    " End " + Environment.NewLine + _
                    " End " + Environment.NewLine + _
                    " End " + Environment.NewLine

                clsDBFuncationality.ExecuteNonQuery(qryTrig)



        Catch ex As Exception
            clsCommon.ProgressBarHide()
            clsCommon.MyMessageBoxShow(ex.Message, "Error in Trigger")
        End Try
    End Sub

    Public Shared Function trg_TSPL_SD_SHIPMENT_DETAIL_Scheme() As String
        Dim qry As String = " Create  trigger [dbo].[trg_TSPL_SD_SHIPMENT_DETAIL_Scheme] on [dbo].[TSPL_SD_SHIPMENT_DETAIL] " & _
                            " for Insert  as  declare @Msg as varchar(max); " & _
                            " declare @Row_No as Integer;declare @Doc_No as varchar(30);declare @Scheme_code as varchar(30);declare @Item_Code varchar(50); " & _
                            " select @Scheme_code=coalesce(Scheme_Code,''),@Row_No=I.Line_No,@Doc_No=I.DOCUMENT_CODE,@Item_Code=I.Item_Code " & _
                            " from inserted I where 2=2 and len(coalesce(Scheme_Code,''))<=0 and Scheme_Item='Y'; " & _
                            " if len(@Scheme_code)<=0  " & _
                            " Begin " & _
                            " set @Msg ='Invalid Scheme Applied at: ' + 'Line No :' +@Row_No + ', Document Code:'+ @Doc_No + ', Item Code :' + @Item_Code + char(10)+ @Msg;" & _
                            " raiserror (@Msg,16,1); " & _
                            " End "
        Return qry
    End Function
End Class
