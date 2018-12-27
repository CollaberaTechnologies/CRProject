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
using CR.Models;

namespace CR.Controllers
{
    public class HomeController : Controller
    {
        IHD_DBEntities db = new IHD_DBEntities();
        [HttpGet]
        public ActionResult New(string id)
        {
            New newinfo = new Models.New();
           
            var rid = Decrypt(id);
            newinfo.hiddenid = long.Parse(rid);
            var data = db.IHD_CR.Where(x => x.RID == newinfo.hiddenid && x.MailStatus == "Submitted").FirstOrDefault();
            if (data==null)
            {
              
                return View(newinfo);
            }
            else
            {
                TempData["RID"] = data.RID;
                TempData["msg"] = "You already Submitted your data!";
                return RedirectToAction("viewdata");
            }
           
        }
        public void download(long id)
        {
            byte[] bytes;
            string fileName, contentType;
            var rid = db.personal_data.Where(x => x.CRRID == id).OrderByDescending(x=>x.Created_Date).Select(x => x.RID).FirstOrDefault();
            var info = db.IHD_TABDOCS.Where(x => x.PRID == rid).FirstOrDefault();
            if (info == null) { }
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
            
            return View(newinfo);

        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        
        [HttpPost]
        public ActionResult New(New newinfo, HttpPostedFileBase file_uploader)
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
                    db.Recent_Employeer.Add(newinfo.employeer);
                }
                db.SaveChanges();
                changemailstatus(newinfo.hiddenid);
                if (file_uploader!=null) {
                    upload(file_uploader, personalrid);
                }
                TempData["msg"] = "Your Data is Submitted!";
                TempData["RID"] = newinfo.hiddenid;
                return RedirectToAction("viewdata");

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
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" || x.CandidateStatus == "Rejected" || x.CandidateStatus == "Hired" && (x.FullName.Contains(name) || x.EmailID.Contains(name))).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
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
                        crinfo = db.IHD_CR.Where(x => x.CandidateStatus == "Pending" && (x.FullName.Contains(name) || x.EmailID.Contains(name)) || x.CandidateStatus == "Rejected" || x.CandidateStatus == "Hired").OrderByDescending(x => x.CreatedDate).ToList();
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
            var data = db.IHD_CR.Where(x => x.RID == rid).FirstOrDefault();
            data.MailStatus = "Submitted";
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
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
        public static string Decrypt(string cipherText)
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