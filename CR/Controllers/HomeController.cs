using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CR.Models;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

using iTextSharp.text;
using iTextSharp.tool;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using HtmlAgilityPack;
using iTextSharp.text.html.simpleparser;

namespace CR.Controllers
{
    public class HomeController : Controller
    {
        IHD_DBEntities db = new IHD_DBEntities();
        [HttpGet]
        public ActionResult New(string id, New newinfo)

        {
            //New newinfo = new Models.New();
          
            var rid = Decrypt(id);
            newinfo.hiddenid = long.Parse(rid);
            newinfo.hiddenemail = getemail(newinfo.hiddenid);
            var data = db.IHD_CR.Where(x => x.RID == newinfo.hiddenid && x.MailStatus == "Submitted").FirstOrDefault();
            
            if (data==null)
            {
                return View(newinfo);
            }
            else
            {
                TempData["RID"] = data.RID;
                TempData["msg"] = "You already Submitted your data!";
                var rinfo = data.RID;
                return RedirectToAction("viewdata",  new { id = rinfo });
            }
           
        }
        public string getemail(long rid)
        {
            string emailid = db.IHD_CR.Where(x => x.RID == rid).Select(x => x.EmailID).FirstOrDefault(); 
            return emailid;
        }
        public byte[] GetPDF(string pHTML,New newinfo)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            //HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();
            PdfPTable table = new PdfPTable(2);

            PdfPCell cell = new PdfPCell(new Phrase("Candidate Info"));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Basic Information"));
            cell.Colspan = 2;
            table.AddCell(cell);

            table.AddCell(" Contact Person:");
            table.AddCell(newinfo.personal.contactperson);
            table.AddCell(" Position Desired:");
            table.AddCell(newinfo.personal.Posdesire);
            table.AddCell(" Expected Salary:");
            table.AddCell(newinfo.personal.expsalary.ToString());
            table.AddCell(" Preferred Work Locations:");
            table.AddCell(newinfo.personal.Preferloc);
            table.AddCell(" Availability On-Board:");
            DateTime onboard = (DateTime)newinfo.personal.Datetimetomeet;
            table.AddCell(onboard.ToShortDateString());
            table.AddCell(" Current CTC:");
            table.AddCell(newinfo.personal.CurrentCTC.ToString());
            table.AddCell(" Inperson Interview:");
            DateTime tomeet = (DateTime)newinfo.personal.Datetimetomeet;
            table.AddCell(tomeet.ToShortDateString());
            table.AddCell(" Venue:");
            table.AddCell(newinfo.personal.Placetomeet);

            cell = new PdfPCell(new Phrase(" "));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Personal Data"));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(" Name:");
            table.AddCell(newinfo.personal.name);
            table.AddCell(" Address:");
            table.AddCell(newinfo.personal.Address);
            table.AddCell(" EmailID:");
            table.AddCell(newinfo.personal.EmailId);
            table.AddCell(" Mobile:");
            table.AddCell(newinfo.personal.Mobile);
            if (newinfo.personal.Phone_ != null)
            {
                table.AddCell(" Phone:");
                table.AddCell(newinfo.personal.Phone_);
            }
            table.AddCell(" Gender:");
            table.AddCell(newinfo.personal.Gender);
            table.AddCell(" Date of Birth:");
            table.AddCell(newinfo.personal.Dob.ToShortDateString());
            table.AddCell(" Age:");
            table.AddCell(newinfo.personal.Age.ToString());
            table.AddCell(" Marital Status:");
            table.AddCell(newinfo.personal.Maritalstatus);
            cell = new PdfPCell(new Phrase(" "));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Additional Information"));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(" Language Known:");
            table.AddCell(newinfo.personal.LanguagesKnown);
            table.AddCell(" How did you get to know about Collabera ?");
            table.AddCell(newinfo.personal.HowKnowAboutCollabera);
            table.AddCell("What do you Know about Collabera ?");
            table.AddCell(newinfo.personal.WhatKnownAboutCollabera);
            table.AddCell(" What are your Career aspirations ?");
            table.AddCell(newinfo.personal.Careeraspirations);
            table.AddCell(" Why do you think you will be suitable for this job?");
            table.AddCell(newinfo.personal.strengthsandweakness);
            cell = new PdfPCell(new Phrase(" "));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Educational Background"));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(" Course:");
            table.AddCell(newinfo.edu.course);
            table.AddCell(" Year Graduated:");
            table.AddCell(newinfo.edu.Yrgraduated);
            table.AddCell(" School/University:");
            table.AddCell(newinfo.edu.SchoolUniversity);
            table.AddCell(" Other Trainings Attended:");
            table.AddCell(newinfo.edu.Othertrainings);
            cell = new PdfPCell(new Phrase(" "));
            cell.Colspan = 2;
            table.AddCell(cell);
            if (newinfo.employeer != null)
            {
                cell = new PdfPCell(new Phrase("Recent Employeer"));
                cell.Colspan = 2;
                table.AddCell(cell);
                table.AddCell(" Position:");
                table.AddCell(newinfo.employeer.position);
                table.AddCell(" Company:");
                table.AddCell(newinfo.employeer.company);
                table.AddCell(" From Date:");
                DateTime fromdate = (DateTime)newinfo.fromdate;
                table.AddCell(fromdate.ToShortDateString());
                table.AddCell(" To Date:");
                DateTime todate = (DateTime)newinfo.todate;
                table.AddCell(todate.ToShortDateString());
                table.AddCell(" Skills/Technology Used:");
                table.AddCell(newinfo.employeer.skills);
                cell = new PdfPCell(new Phrase(" "));
                cell.Colspan = 2;
                table.AddCell(cell);
            }
            cell = new PdfPCell(new Phrase("References"));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(" Name:");
            table.AddCell(newinfo.refrence.name);
            table.AddCell(" Relationship:");
            table.AddCell(newinfo.refrence.relationship);
            table.AddCell(" Contact Number:");
            table.AddCell(newinfo.refrence.contactno);



            doc.Add(table);
          

            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }
        [HttpPost]
        [ValidateInput(false)]
        public void Export(string GridHtml,New newinfo)
        {
            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(GridHtml);
            GridHtml = doc.DocumentNode.OuterHtml;

            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    StringReader sr = new StringReader(GridHtml);
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            //    pdfDoc.Open();

            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //    pdfDoc.Close();
            //    return File(stream.ToArray(), "application/pdf", "Info.pdf");
            //}
            //string HTMLContent = "Hello <b>World</b>";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + newinfo.personal.name+".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(GridHtml,newinfo));
            Response.End();
        }
        public ActionResult sendotp(string emailid)
        {
            Random r = new Random();
            long no = r.Next(0, 999999);
            try
            {
               
                var fromadrress = "garima.thakore" + "@collabera.com";
                var message = new MailMessage();
                message.From = new MailAddress(fromadrress);  // replace with valid value
                message.Subject = "OTP Verification";
                message.Body = "<p>Dear Applicant,</p><p>Your One Time Password (OTP) for Email Verification is " + no + " </p><p>Please enter the code and verify Email-Id.</p><p><span style='font - size:12.0pt; font - family:&quot; Times New Roman & amp; quot;,&quot; serif & amp; quot; ; mso - fareast - font - family:Calibri; mso - fareast - theme - font:minor - latin; mso - ansi - language:EN - US; mso - fareast - language:EN - US; mso - bidi - language:AR - SA'><br></span></p><p class='MsoNormal'><strong>Thanks &amp; Regards</strong></p><p><strong>COLLABERA</strong></p>";
                message.IsBodyHtml = true;

                var smtp = new SmtpClient
                {
                    Host = "mailbrd.collabera.com",

                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                };
                message.To.Add(new MailAddress(emailid));
              
                smtp.Send(message);

            }
            catch (Exception ex)
            {
                string ip = Request.UserHostAddress;
                errorinfo.SendErrorToText(ex, ip);

            }
            return Json(no, JsonRequestBehavior.AllowGet);
        }
        public void download(long id)
        {
            byte[] bytes;
            string fileName, contentType;
            var rid = db.personal_data.Where(x => x.CRRID == id).OrderByDescending(x=>x.Created_Date).Select(x => x.RID).FirstOrDefault();
            var info = db.IHD_TABDOCS.Where(x => x.PRID == rid).FirstOrDefault();
            if (info == null) {
                Response.Clear();
                Response.Write("No Resume is Attached");
                Response.Flush();
                Response.End();

            }
            else
            {
                fileName = info.Name;
                contentType = info.ContentType;
                bytes = info.Data;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }
        public ActionResult viewdata(long id=0)
        {
            if (TempData["RID"] != null)
            {
                id = (long)TempData["RID"];
            }
           
             if (TempData["msg"] != null)
            {
                ViewBag.message = TempData["msg"].ToString();
            }
            Models.New newinfo = new Models.New();
            var personal = db.personal_data.Where(x => x.CRRID == id).FirstOrDefault();
            var personalrid = personal.RID;
            var edu = db.Edu_Background.Where(x => x.resid == personalrid).FirstOrDefault();
            var refer= db.References.Where(x => x.resid == personalrid).FirstOrDefault();
            var employee = db.Recent_Employeer.Where(x => x.resid == personalrid).FirstOrDefault();
            newinfo.personal = personal;
            newinfo.edu = edu;
            newinfo.age = personal.Age;
            newinfo.employeer = employee;
            newinfo.refrence = refer;
            if (employee!=null) {
                newinfo.fromdate = employee.fromdate;
                newinfo.todate = employee.todate;
            }
            if (TempData["RID"] != null)
            {
                id = (long)TempData["RID"];
            }

            if (TempData["msg"] != null)
            {
                ViewBag.message = TempData["msg"].ToString();
            }
            return View(newinfo);

        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        
        [HttpPost]
        public ActionResult New(New newinfo, HttpPostedFileBase file_uploader,DateTime? fromDate1=null,DateTime? toDate1=null)
        {
            try
            {
                newinfo.personal.Age = newinfo.age;
                newinfo.personal.Created_Date = DateTime.Now;
                newinfo.personal.CRRID = newinfo.hiddenid;

                    db.personal_data.Add(newinfo.personal);
                    db.SaveChanges();
                    var personalrid = db.personal_data.Where(x => x.EmailId == newinfo.personal.EmailId && x.Posdesire == newinfo.personal.Posdesire && x.Mobile == newinfo.personal.Mobile).OrderByDescending(x => x.Created_Date).Select(x => x.RID).FirstOrDefault();
                    newinfo.edu.resid = personalrid;
                    db.Edu_Background.Add(newinfo.edu);
                    newinfo.refrence.resid = personalrid;
                    db.References.Add(newinfo.refrence);
                    newinfo.employeer.resid = personalrid;
                if (newinfo.employeer.company!=null) {
                    newinfo.employeer.todate = newinfo.todate;
                    newinfo.employeer.fromdate = newinfo.fromdate;
                    db.Recent_Employeer.Add(newinfo.employeer);
                }
                db.SaveChanges();
                changemailstatus(newinfo.hiddenid);
                if (file_uploader!=null) {
                    upload(file_uploader, personalrid);
                }
                TempData["msg"] = "Your Data is Submitted!";
                //TempData["RID"] = newinfo.hiddenid;
               var rid = newinfo.hiddenid;
                return RedirectToAction("viewdata", new { id = rid });

            }
            catch(Exception ex)
            {
                string ip = Request.UserHostAddress;
                errorinfo.SendErrorToText(ex, ip);
            }
            return View(newinfo);
        }

        public ActionResult upload(HttpPostedFileBase file_uploader,long personalrid)
        {
            Models.New newinfo = new Models.New();
            IHD_TABDOCS tabdocs = new IHD_TABDOCS();
            string filename = file_uploader.FileName;
            string contentType = file_uploader.ContentType;
            using (Stream fs = file_uploader.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    tabdocs.ContentType = contentType;
                    tabdocs.Data = bytes;
                    tabdocs.Name = filename;
                    tabdocs.PRID = personalrid;
                    db.IHD_TABDOCS.Add(tabdocs);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
            }

            return null;
        }
        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("Login","Account");
        }
        public ActionResult search(string name,string status)
        {
            IHD_CR cr = new IHD_CR();
            var crinfo = new List<IHD_CR>();
            if (name == "")
            {
                if (status == "")
                {
                    crinfo = db.IHD_CR.OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                }
                else {
                    if (status != "All")
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == status).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }
                    else
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" || x.CandidateStatus == "Rejected" || x.CandidateStatus == "Hired").OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }

                }
              

            }
            else
            {
                if (status == "")
                {
                    crinfo = db.IHD_CR.Where(x => x.FullName.Contains(name) || x.EmailID.Contains(name)).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                }
                else
                {
                    if (status != "All")
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == status && (x.FullName.Contains(name) || x.EmailID.Contains(name))).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }
                    else
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" || x.CandidateStatus == "Rejected" || x.CandidateStatus == "Hired" ).Where(x=>x.FullName.Contains(name) || x.EmailID.Contains(name)).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }
                    
                }
                
            }

            return Json(crinfo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult searchstatus(string status,string name)
        {
            IHD_CR cr = new IHD_CR();
            var crinfo = new List<IHD_CR>();
            if (status == "")
           
            {
                if (name=="")
                {
                    crinfo = db.IHD_CR.OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                }
                else
                {
                    crinfo = db.IHD_CR.Where(x => x.FullName.Contains(name) || x.EmailID.Contains(name)).OrderByDescending(x => x.CreatedDate).Take(8).ToList();

                }
              
            }
            else
            {
                if (status == "All") {
                    if (name == "")
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" || x.CandidateStatus== "Rejected" || x.CandidateStatus== "Hired").OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }
                    else
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" || x.CandidateStatus == "Rejected" || x.CandidateStatus == "Hired").Where(x => x.FullName.Contains(name) || x.EmailID.Contains(name)).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
                    }
                }
                else
                {
                    if (name == "")
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == status).Take(8).OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == status && (x.FullName.Contains(name) || x.EmailID.Contains(name))).Take(8).OrderByDescending(x => x.CreatedDate).ToList();
                    }
                }
              
            }
            return Json(crinfo, JsonRequestBehavior.AllowGet);
    
        }
        public ActionResult changemailstatus(long rid)
        {
            try
            {
                var data = db.IHD_CR.Where(x => x.RID == rid).FirstOrDefault();
                data.MailStatus = "Submitted";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                var to = db.IHD_USER_MAIN.Where(x => x.RID == data.CreatedBy).Select(x => x.EmailID).FirstOrDefault();
                var name= db.IHD_USER_MAIN.Where(x => x.RID == data.CreatedBy).Select(x => x.FirstName).FirstOrDefault();
                var fromadrress = "garima.thakore" + "@collabera.com";
                var message = new MailMessage();
                message.From = new MailAddress(fromadrress);  // replace with valid value
                message.Subject = "Candidate Submitted Information";
                message.Body = "<p>Hello "+ name + ",</p><p><br></p><p>"+data.FullName+" has submitted the application form. Please check the filled details on the portal</p><p class='MsoNormal'><o:p>&nbsp;</o:p></p><p class='MsoNormal'><strong>Thanks &amp; Regards</strong></p><p></p><p class='MsoNormal'><strong>COLLABERA</strong></p>";
                message.IsBodyHtml = true;

                var smtp = new SmtpClient
                {
                    Host = "mailbrd.collabera.com",

                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                };
                message.To.Add(new MailAddress(to));
              

                smtp.Send(message);


            }
            catch (Exception ex)
            {
                string ip = Request.UserHostAddress;
                errorinfo.SendErrorToText(ex, ip);
            }
            return null;
        }
        public ActionResult changestatus(long rid,string status)
        {
          
            var data = db.IHD_CR.Where(x => x.RID == rid).FirstOrDefault();
            data.CandidateStatus = status;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return null;
        }
        public ActionResult Dashboard()
        {
            mail mailinfo = new mail();
            var crrinfo = db.IHD_CR.OrderByDescending(x=>x.CreatedDate).Take(4).ToList();
            var crrinfo2 = db.IHD_CR.OrderByDescending(x => x.CreatedDate).Take(8).ToList();
            if (Session["username"]==null)
            {
                return RedirectToAction("Login","Account");
            }
            var info1= new List<IHD_CR>();
            var info2= new List<IHD_CR>();
           
            ViewBag.info1 = crrinfo;
            ViewBag.info = crrinfo2.Skip(4);
       
            mailinfo.emailbody = "<div><b>Dear Candidate</b></div><div><b><br></b></div><div><b><br></b></div><div><b><br></b></div><div><b>We are pleased to inform you that we are moving ahead with your candidature with Collabera.</b></div><div><b><br></b></div><div><b>Please help us with with your further details mentioned on below given link:</b></div><div><b><br></b></div><div>&nbsp;&nbsp;&nbsp;&nbsp;<a href='http://172.20.30.147:8096/Home/New?id=crrid'>Click Here</a><b></b></div><div><b><br></b></div><div><b><br></b></div><div><b><br></b></div><div><b><br></b></div><div><b>Please feel free to reach out to us on below details:</b></div><div><b><br></b></div><div><b>Email ID: hcv@collabera.com</b></div><div><b><br></b></div><div><b>Contact No: +0265 457896123/5789461323</b></div><div><b><br></b></div><div><b><br></b></div><div><b><br></b></div><div><b>Thanks &amp; Regards</b></div><div><b><br></b></div><div><b>COLLABERA</b></div>";

            return View(mailinfo);
        }
        public  string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "Collabera@123";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch(Exception ex)
            {
                string ip = Request.UserHostAddress;
                errorinfo.SendErrorToText(ex, ip);
                Response.Clear();
                Response.Write("Invalid Link.");
                Response.Flush();
                Response.End();
                return "";
            }
        }
        public static string encrypt(string encryptString)
        {
            string EncryptionKey = "Collabera@123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Email(mail mailinfo)
        {
            long createdby = (long)Session["rid"];
            IHD_CR crinfo = new IHD_CR();
            crinfo.FullName = mailinfo.FirstName + " " + mailinfo.LastName;
            crinfo.EmailID = mailinfo.To;
            crinfo.CandidateStatus = "pending";
            crinfo.CreatedBy = createdby;
            crinfo.CreatedDate = DateTime.Now;
            crinfo.MailStatus = "Sent";
            crinfo.Mobile = mailinfo.Mobile;
            crinfo.Position = mailinfo.Position;
            crinfo.Type = mailinfo.hiretype;
            db.IHD_CR.Add(crinfo);
            db.SaveChanges();
            var rid = db.IHD_CR.Where(x => x.EmailID == mailinfo.To && x.CreatedBy == createdby).OrderByDescending(x => x.CreatedDate).Select(x => x.RID).FirstOrDefault().ToString();

            try
            {
                var body = mailinfo.emailbody.Replace("Candidate", mailinfo.FirstName + " " + mailinfo.LastName);
                var fromadrress = "garima.thakore" + "@collabera.com";
                var message = new MailMessage();
                message.From = new MailAddress(fromadrress);  // replace with valid value
                message.Subject = mailinfo.Subject;
                message.Body = body.Replace("crrid", encrypt(rid));
                message.IsBodyHtml = true;
               
                var smtp = new SmtpClient
                {
                    Host = "mailbrd.collabera.com",
                 
                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                };
                message.To.Add(new MailAddress(mailinfo.To));
                if (mailinfo.CC != null)
                {
                    message.CC.Add(new MailAddress(mailinfo.CC));
                }
                if (mailinfo.BCC != null)
                {
                    message.Bcc.Add(new MailAddress(mailinfo.BCC));
                }

                smtp.Send(message);

              


            }
            catch(Exception ex)
            {
                string ip = Request.UserHostAddress;
                errorinfo.SendErrorToText(ex, ip);

            }
            return RedirectToAction("Dashboard", "Home");
        }
    }
}