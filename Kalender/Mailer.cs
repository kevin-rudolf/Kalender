﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Dynamic;
using System.Reflection.Emit;

namespace Kalender
{
    public static class Mailer
    {
        //change to model
        public static void SendMail(string titel, string plz, string city, string adrs, string notes, string employee, string date)
        {
            string mailserver = "smtp.office365.com";
            string email = "kevin.rudolf@hak-kitz.ac.at";
            string pw = TempPW.mailpw;
            string emailto = "kevin.rudolf@hak-kitz.ac.at";

            try
            {
                SmtpClient client = new SmtpClient(mailserver);
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential login = new System.Net.NetworkCredential(email, pw);
                client.Credentials = login;
                client.EnableSsl = true;

                MailAddress from = new MailAddress(email, "Terminplanung");
                MailAddress to = new MailAddress(emailto);
                MailMessage mail = new MailMessage(from, to);

                mail.Subject = "Sie wurden zu einem neuen Termin - "+titel+" - hinzugefügt";

                mail.SubjectEncoding = Encoding.UTF8;

                if (plz == null)
                {
                    mail.Body = "Sehr geehrter Herr " + employee + ",<br>Sie wurden gerade zu einem neuem Termin am " + date + " hinzugefügt." +
                    "<br><br>Weitere Informationen: " + notes;
                }
                else
                {
                    mail.Body = "Sehr geehrter Herr " + employee + ",<br>Sie wurden gerade zu einem neuem Termin am " + date + " hinzugefügt." +
                    "<br>Der folgende Termin findet in "+plz+" "+city+", "+adrs+" statt.<br><br>Weitere Informationen: " + notes;
                }
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;

                client.Send(mail);

                Application.Current.MainPage.DisplayAlert("Erfolg", "Die Mail wurde erfolgreich versendet!", "OK");
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
        }
    }

}

