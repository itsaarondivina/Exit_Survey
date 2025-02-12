using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VXI_EXIT_INTERVIEW_v2.Models;
using VXI_EXIT_INTERVIEW_v2.Models.VM;
using VXI_EXIT_INTERVIEW_v2.Utilities;



namespace VXI_EXIT_INTERVIEW_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly VXI_EXIT_INTERVIEW_FINAL_PAYEntities db = new VXI_EXIT_INTERVIEW_FINAL_PAYEntities();

        public ActionResult Index()
        {
            if (Session["Id"] == null || Session["Id"].ToString() == "" || Session["Id"].ToString() == "0")
            {
                return Redirect("~/Login/Index");
            }
            else
                return View();
        }

        public ActionResult SurveyPage()
        {
            if (Session["Id"] == null || Session["Id"].ToString() == "" || Session["Id"].ToString() == "0")
                return Redirect("~/Login/Index");
            else
                return View();
        }

        [HttpGet]
        public JsonResult GetSurvey()
        {
            AnswerModel dataModel = new AnswerModel();
            try
            {
                using (VXI_EXIT_INTERVIEW_FINAL_PAYEntities obj = new VXI_EXIT_INTERVIEW_FINAL_PAYEntities())
                {
                    var questionList = (from s in obj.Question
                                        where s.IsActive == true
                                        select new
                                        {
                                            QuestionId = s.QuestionID,
                                            SurveyId = s.SurveyID,
                                            QuestionType = s.QuestionType,
                                            Description = s.QuestionDescription
                                        }).ToList();

                    var answerList = (from s in obj.Answer
                                      where s.IsActive == true
                                      select new
                                      {
                                          AnswerId = s.AnswerID,
                                          QuestionId = s.QuestionID,
                                          Description = s.AnswerDescription
                                      }).ToList();

                    return Json(new { questionList, answerList }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult saveSurvey(List<AnswerModel> answers)
        {
            var result = false;
            var UserAccount = (AccountInformation)Session["Account"];

            try
            {
                using (VXI_EXIT_INTERVIEW_FINAL_PAYEntities obj = new VXI_EXIT_INTERVIEW_FINAL_PAYEntities())
                {
                    for (int i = 0; i < answers.Count(); i++)
                    {
                        var answerId = new Guid(answers[i].answerId);
                        var questionId = new Guid(answers[i].questionId);
                        var answerDescription = answers[i].answer.ToString();
                        var surveyId = new Guid("3618F4BC-C5A9-4A1F-8898-7EBC55CA4E78");
                        DateTime now = DateTime.Now;
                        
                        obj.UserAnswer.Add(new UserAnswer()
                        {
                            UserAnswerID = answerId,
                            SurveyID = surveyId,
                            QuestionID = questionId,
                            CreatedAt = now,
                            IsActive = true,
                            OtherResponse = "",
                            Response = "",
                            Createdby = UserAccount.AccountId,
                            AccountId = UserAccount.AccountId
                        });

                        obj.SaveChanges();
                    }

                    var hrid = UserAccount.HrId;
                    var distro = (from s in obj.EmailerDistro select s).ToList();
                    var otherDetails = (from s in obj.OtherEmployeeDetails where s.Hrid == hrid select s).FirstOrDefault();

                    //var Requestor = "";
                    //string send_to_name;

                    
                    StringBuilder header = new StringBuilder();
                    StringBuilder bodyrequestor = new StringBuilder();
                    StringBuilder footerrequestor = new StringBuilder();
                    var mail = new MailMessage
                    {
                        From = new MailAddress("exitsurveytool@vxi.com")
                    };


                    mail.Subject = "Exit Survey #: " + UserAccount.FullName.ToString() + " | " + hrid;

                    header.AppendFormat($"<div><p>Hi, Site Hr Personnel, </p>");
                 
                    bodyrequestor.Append($"<p> <b> " + UserAccount.FullName.ToString() +  " </b> has completed exit interview questionnaire. </p>");
                    bodyrequestor.Append($"<p> Answers to the questionnaire are now saved in the  <br/></p>");
                    bodyrequestor.Append($"<p> To experience maximum application functionality, please use these web browser: <b> Google Chrome, Mozilla Firefox, Safari, Microsoft Edge, Opera.</b> </p>");
                    //bodyrequestor.Append($"<p> Employee Email Address #: " + otherDetails.EmailAddress + " <b></b> </p>");
                    //bodyrequestor.Append($"<p> Employee Employee Mobile #: " + otherDetails.MobileNumber + " <b></b> </p>");
                    bodyrequestor.Append($"<p> Separation Date: " + UserAccount.SeparationDate + " <b></b> </p>");
                    bodyrequestor.Append($"<p> Site: " + UserAccount.SeparationDate + " <b></b> </p>");

                    footerrequestor.Append("<span>Note: This is an <b>auto-generated</b> notification. Please don’t reply to this email.</span>");

                    mail.Body = header.ToString() + bodyrequestor.ToString() + footerrequestor.ToString();
                    mail.IsBodyHtml = true;
                    mail.To.Add("gabriel.mariano@vxi.com");

                    var client = new SmtpClient
                    {
                        Host = "vximailboxmk01.vxi.com.ph",
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new System.Net.NetworkCredential("exitsurvey", "Welcome123456789", "vxi.com")
                    };

                    client.Send(mail);
                }

                return Json(new { result });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginVM data)
        {
            try
            {
                var apiBaseUrl = ConfigurationManager.AppSettings["PrismApi"];
                var apiUrl = $"{apiBaseUrl}{data.Hrid}";
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.ReasonPhrase == "OK")
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var parsedApiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<InfoModel>(apiResponse);
                        string formattedDate = parsedApiResponse.dateBirth.ToString("MMddyyyy");
                        string formattedDateHired = parsedApiResponse.dateHired.ToString("MMddyyyy");
                        string anotherformattedDateHired = parsedApiResponse.dateHired.ToString("MM/dd/yyyy");

                        if (formattedDate != data.BirthDate)
                            return Json(new { status = false });

                        Session["Id"] = parsedApiResponse.id;
                        var CurrentDate = DateTime.Now;
                        var TempId = Guid.NewGuid();

                        using (VXI_EXIT_INTERVIEW_FINAL_PAYEntities Obj = new VXI_EXIT_INTERVIEW_FINAL_PAYEntities())
                        {
                            var accountInfoList = (from s in Obj.AccountInformation
                                                   where s.HrId == data.Hrid
                                                   select s).ToList();

                            if (accountInfoList.Count == 0)
                            {
                                var newAccountInfo = new AccountInformation
                                {
                                    AccountId = TempId,
                                    HrId = parsedApiResponse.id,
                                    Hiredate = formattedDate,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = parsedApiResponse.id,
                                    IsActive = true,
                                    FullName = parsedApiResponse.lastName + ", " + parsedApiResponse.firstName,
                                    Site = parsedApiResponse.buildingAssignment,
                                    SeparationDate = parsedApiResponse.separatedDate,
                                    HiringDate = anotherformattedDateHired,
                                    LineOfBusiness = parsedApiResponse.lineOfBusiness
                                };

                                db.AccountInformation.Add(newAccountInfo);
                                db.SaveChanges();
                            }

                            var accountInfoLists = (from s in Obj.AccountInformation
                                                    where s.HrId == data.Hrid
                                                    select s).FirstOrDefault();

                            Session["Account"] = accountInfoLists;
                        }

                        return Json(new { status = true, details = parsedApiResponse });
                    }
                    else
                        return Json(new { status = false, error = "API request failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, error = ex.Message });
            }
        }
    }
}