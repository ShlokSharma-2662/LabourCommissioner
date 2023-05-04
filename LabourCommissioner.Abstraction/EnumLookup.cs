using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction
{
    public static class EnumLookup
    {
        public enum applicationfrom
        {
            web = 1,
            mobile = 2
            
        }

        public enum HasENirmancard
        {
            Yes = 1,
            No = 0
        }

        public enum ResponseMsgType
        {
            success = 1,
            error = 2,
            warning = 3,
            info = 4
        }
        public enum ResourcesType
        {
            Education = 1,
            Degree = 2,
            Semester = 3
        }
        public enum EmailPurpose
        {
            V = 1,
            A = 2,
            R = 3
        }
        public enum CRUDPurpose
        {
            I = 1,
            U = 2,
            D = 3
        }
        public enum BeneficiaryType
        {
            BOCW = 1,
            GLWB = 2,
        }
        public enum UserType
        {
            Citizen = 1,
            Employee = 2,
        }

        public enum ApplicationStatus
        {
            Pending = 1,
            Approve = 2,
            Reject = 3,
            SendBack = 1,
            ReSubmitted = 4
        }

        public enum schemaname
        {
            bocw_ssy = 7,
            bocw_bpsy = 3,
            glwb_hsc = 19,
            bocw_psy = 2,
            glwb_ssc = 20,
            glwb_psy = 23,
            bocw_asy = 5,
            bocw_tsy = 8,
            bocw_pip = 4,
            glwb_msl = 22,
            bocw_acsy = 6,
            glwb_asy = 27,
            glwb_adsy = 15,
            bocw_tbsy= 12,
            bocw_tbsy_claim= 36,
            glwb_csy = 14,
           
            glwb_hls=18,
            glwb_tsy = 24,
            glwb_hty = 16,
            glwb_hty_claim = 32,
            glwb_tsy_claim = 34,
            bocw_vr=11,
            
           
            bocw_nanji = 13,
            bocw_vcy = 9,
            glwb_hss = 21,
            glwb_lsy = 30,
            bocw_hssy = 10,
            cesscollection = 33
        }

        public enum tablename
        {
            personaldetails = 1,
            schemedetails = 2,
            documentdetails = 3,
            approvaldetails = 4,
            tabentry = 5,
            familydetails=6,
            childrenfamilydetails=7,
            childrenhosteldetails=8,
            otherschemedetails=9,
            companyworkerdetails= 10,
            applicationdetails = 11,
            traveldetails = 6,
        }
        public enum smstype
        {
            AppRequested = 1,
            AppAccepted = 2,
            AppRejected = 3,
            AppReverted = 4,
        }
        public enum AadeshPaymentInfoStatus
        {
            Pending = 1,
            Approve = 2
        }
        public enum exceptionlogfrom
        {
            WEB = 1,
            API = 2,
        }

        public static string DataTableToJson(this DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col].ToString().StartsWith('{') || row[col].ToString().StartsWith('['))
                    {
                        dict[col.ColumnName] = JsonConvert.DeserializeObject(row[col].ToString());
                    }
                    else
                    {
                        dict[col.ColumnName] = row[col];
                    }
                }
                list.Add(dict);
            }
            return JsonConvert.SerializeObject(list);
        }

        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? string.Empty;
        }


        //Status Code for Api Response
        public enum StatusCode
        {
            Bad_Request = 400,
            Sucess = 200,
            Internal_Server_Error = 500,
            Not_Found = 404,
            Unauthorized = 401
        }

        public enum Status
        {
            [Description("Success")]
            Success = 1,
            [Description("Not Found")]
            Not_Found = 2,
            [Description("Fail")]
            Fail = 3,
            [Description("Unauthorized")]
            Unauthorized = 4

        }
        
        public enum Message
        {
            [Description("User Successfully Register")]
            Register_Success = 1,

            [Description("User Is Not Successfully Register")]
            Register_Not_Added = 2,

            [Description("Token Generated Successfully")]
            Token_Success = 3,

            [Description("User Successfully Login")]
            User_Login = 4,

            [Description("User Login Failed")]
            Login_Failed = 5,

            [Description("Successfull")]
            Successfull = 6,

            [Description("Unsuccessfull")]
            Not_Successfull = 7,

            [Description("Description Succesfully Display")]
            Description_Successfull = 8,

            [Description("Description not Display")]
            Description_Not_Successfull = 9,

            [Description("Application Details Listed Successfully")]
            ApplicationDetails_Succefull = 10,

            [Description("Application Details not Listed")]
            ApplicationDetails_Not_Successfull = 11,

            [Description("જે તે શૈક્ષણિક વર્ષમાં અરજદાર વધુમાં વધુ ૨ અરજીઓ કરી શકે છે.")]
            SSY_Eligible = 12,

            [Description("પ્રસુતિ સહાય યોજનામાં અરજદાર વધુમાં વધુ ૨ અરજીઓ કરી શકે છે.")]
            PSY_Eligible = 13,

            [Description("પ્રસુતિ સહાય યોજના અને મુખ્યમંત્રી ભાગ્યલક્ષ્મી બોન્ડ યોજનામાં અરજદાર વધુમાં વધુ ૨ અરજીઓ કરી શકે છે.")]
            BPSY_Eligible = 14,

            [Description("TabSequence Succesfully Display")]
            TabSequence_Display = 15,

            [Description("TabSequence Not Display Successfully")]
            TabSequence_Not_Display = 16,

            [Description("PersonnalDetails Added Successfully")]
            PersonnalDetails_Added = 17,

            [Description("PersonnalDetails Not Added")]
            PersonnalDetails_Not_Added = 18,

            [Description("SchemeDetails Added Successfully")]
            schemeDetails_Added = 19,

            [Description("SchemeDetails Not Added")]
            schemeDetails_Not_Added = 20,

            [Description("PersonnalDetails Display Successfully")]
            Get_PersonnalDetails = 21,


            [Description("PersonnalDetails Not Display")]
            Not_Get_PersonnalDetails = 22,            

            [Description("SchemeDetails Display Successfully")]
            Get_SchemeDetails = 23,

            [Description("SchemeDetails Not Display")]
            Not_Get_SchemeDetails = 24,

            [Description("DocumentDetails Display Successfully")]
            Get_DocumentDetails = 25,

            [Description("DocumentDetails Not Display")]
            Not_Get_DocumentDetails = 26,

            [Description("Successfully Submitted Application")]
            Send_FinalSubmit = 27,

            [Description("Application Not Submitted")]
            FinalSubmit_Not_Send = 28,

            [Description("States Display Successfully")]
            States_Display = 29,

            [Description("States Not Display Successfully")]
            No_States_Display = 30,

            [Description("Districts Display Successfully")]
            District_Display = 31,

            [Description("District Not Display Successfully")]
            No_District_Display = 32,

            [Description("Villages Display Successfully")]
            Village_Display = 33,

            [Description("Villages Not Display Successfully")]
            No_Village_Display = 34,

            [Description("Subjects Display Successfully")]
            Subject_Display = 35,

            [Description("Subjects Not Display Successfully")]
            No_Subjects_Display = 36,

            [Description("Semesters Display Successfully")]
            Semester_Display = 37,

            [Description("Semesters Not Display Successfully")]
            No_Semester_Display = 38,

            [Description("Benifits Display Successfully")]
            Benifits_Display = 39,

            [Description("Benifits Not Display Successfully")]
            No_Benifits_Display = 40,
               [Description("district  Display Successfully")]
            districtId_Display = 41,
               [Description("district Not Display Successfully")]
            No_districtId_Display = 42,
                 [Description("Education Display Successfully")]
            Education_Display = 43,
                 [Description("Education Not Display Successfully")]
            No_Education_Display = 44,
                   [Description("RegistrationDetails Display Successfully")]
            Get_RegistrationDetails = 45,
                   [Description("RegistrationDetails Not Display Successfully")]
            Not_Get_RegistrationDetails = 46,

            [Description("Documents Display Successfully")]
            Documents_Display = 47,

            [Description("Documents Not Display Successfully")]
            No_Documents_Display = 48,

             [Description("Dieases Display Display Successfully")]
            Dieases_Display = 49,
             [Description("Dieases Not Display Successfully")]
            No_Dieases_Display = 50,

            [Description("Details get successfully")]
            Details_Get_Success = 51,
            [Description("Records not found")]
            No_Record_Found = 52,
            [Description("Application Details get successfully")]
            Application_Details_Get_Success = 53,
            [Description("Application Details not found")]
            Application_No_Record_Found = 54,
            [Description("Report Display successfully")]
            Report_Get_Success = 55,
            [Description("Report not found")]
            Report_No_Record_Found = 56,
            [Description("User Get successfully")]
            User_get_Successfull = 57,
            [Description("User not found")]
            User_Not_found = 58,
             [Description("Reset Password successfully")]
            Reset_Password_Successfull = 59,
            [Description("Reset not successfully")]
            Reset_Not_successfully = 60,
             [Description("Password Change Successfully....!")]
            Password_Change_Successfull = 61,
            [Description("Password Does Not Changed...!")]
            Password_Does_Not_Changed = 62,

        }

    }
}

