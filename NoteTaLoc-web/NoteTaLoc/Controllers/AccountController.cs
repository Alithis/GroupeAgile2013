using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NoteTaLoc.Models;
using Recaptcha;
using System.Security.Cryptography;
using System.Text;


namespace NoteTaLoc.Controllers
{
    public class AccountController : Controller
    {

        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        public ActionResult Activation(string activationKey)
        {
            string getActivationKey = activationKey;

            //Look for the account activation key in the database with value of activated != null
            if (isValidationDone(getActivationKey))
            {
                // Update the value of the InscriptionConfirm
                UserTable usertable = db.UserTables.SingleOrDefault(p => p.ValidationToken == activationKey);
                usertable.InscriptionConfirm = true;
                db.SaveChanges();
                return View("ActivationSuccessful");
            }
            else
            {
                return View("ActivationAlreadyDone");
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecaptchaControlMvc.CaptchaValidator]
        public ActionResult Register(RegisterModel model, bool captchaValid, string captchaErrorMessage)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    UserTable usertable = new UserTable();
                    usertable.UserId = GetNextUserId();
                    usertable.Nom = model.UserLName;
                    usertable.Prenom = model.UserFName;
                    usertable.Pseudo = model.UserName;
                    usertable.Courriel = model.EmailAddress;
                    usertable.MotDePasse = model.Password;
                    if (model.TermAndConditions)
                        usertable.SiteCondAccept = true;
                    else
                        usertable.SiteCondAccept = false;

                    if (!captchaValid)
                    {
                        ModelState.AddModelError("recaptcha", captchaErrorMessage);
                        return View(model);
                    }
                    else
                    {
                        if (!DoesUserNameExist(model.UserName))
                        {
                            //Generate key to validate registration
                            String keyRegistration = usertable.Prenom + "-" + usertable.Nom + "-"+usertable.UserId;

                            MD5 md5Hash = MD5.Create();
                            usertable.ValidationToken = GetMd5Hash(md5Hash, keyRegistration);

                            // Send email to user to confirm account registration.
                            if (SendAccountConfimration(model, usertable.ValidationToken))
                            {
                                db.UserTables.Add(usertable);
                                db.SaveChanges();

                                return RedirectToAction("AfterRegister", "Account");
                            }
                            else
                                ModelState.AddModelError("", "An error has occurred attempting to send an email.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username already exist.  Please select another username.");
                        }
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Registration Error");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public Boolean DoesUserNameExist(string username)
        {
            string sqlcmd = "SELECT * FROM dbo.usertable WHERE pseudo = '" + username + "'";
            //string sqlcmd = "SELECT COUNT(pseudo) FROM dbo.usertable WHERE pseudo = '" + username + "'";

            var result = db.UserTables.SqlQuery(sqlcmd);
            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        public Boolean ValidateUser_Password(string username, string pw)
        {
            string sqlcmd = "SELECT * FROM dbo.usertable WHERE pseudo = '" + username + "' and motdepasse = '" + pw + "'";

            var result = db.UserTables.SqlQuery(sqlcmd);
            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        public Boolean isValidationDone(string tokenValue)
        {
            string sqlcmd = "SELECT * FROM dbo.usertable WHERE ValidationToken = '" + tokenValue + "' and InscriptionConfirm is null";

            var result = db.UserTables.SqlQuery(sqlcmd);
            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        public int GetNextUserId()
        {
            int nValue = 0;
            bool bNextValueFound = false;
            /*
             * ToDo Get last ID
            string sqlcmd = "select UserId from dbo.usertable";

            var result = db.UserTables.SqlQuery(sqlcmd);

            string strValue;
            if (result.Count() > 0)
                strValue = result.ToString();
            */
            while (!bNextValueFound)
            {
                nValue++;
                UserTable usertable = db.UserTables.Find(nValue);
                if (usertable == null)
                    bNextValueFound = true;
            }
            
            return nValue;
        }

       public Boolean SendAccountConfimration(RegisterModel model, String tokenBody)
        {
            Boolean bRetCode = true;
            //Send confirmation email.
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                String prefix = "http://notetaloc.azurewebsites.net/Account/Activation?activationKey=";
                String validationLink = prefix + tokenBody;

                message.To.Add(model.EmailAddress); //recipient 
                message.Subject = "RateYourRent - confirmation email";
                message.From = new System.Net.Mail.MailAddress("no.reply@alithis.com"); //from email 
                message.Body = "Please click on the link to confirm your registration: "+validationLink;
                
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.cia.ca");
                smtp.Send(message); 
            }
            catch
            {
                bRetCode = false;
            }
            return bRetCode;
        }

        //
        // GET: /Account/AfterRegister
        
        [AllowAnonymous]
        public ActionResult AfterRegister()
        {
            return View();
        }
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            bool bValid = true;
            if (ModelState.IsValid )
            {
                if (!DoesUserNameExist(model.UserName))
                {
                    ModelState.AddModelError("", "The user name provided does not exist.");
                    bValid = false;
                }
                if (!ValidateUser_Password(model.UserName, model.Password))
                {
                    ModelState.AddModelError("", "The password provided is incorrect.");
                    bValid = false;
                }
            }

            if (bValid)
                return RedirectToLocal(returnUrl);
            else
                return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // ToDo WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

    }
}
