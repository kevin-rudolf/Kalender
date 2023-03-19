using Kalender.Models;
using Kalender.Utils;
using System.Security.Cryptography;
using BCrypt;
using MongoDB.Driver.Core.Authentication;

namespace Kalender;

public partial class LoginReg : ContentPage
{
    private string benutzername = "";
    private string passwort = "";
    private string loginuserid = "";
	public LoginReg(string username, string password)
	{
		InitializeComponent();
        benutzername = username;
        passwort = password;
        if(username != "" && password != "")
        {
            btn_register.Text = "Benutzer ändern";

            LoginModel lm = DBHandler.LoginGetUsers(username);
            loginuserid = lm._id;
            lbl_name.Text = lm.name;
            lbl_surname.Text = lm.surname;
            lbl_email.Text = lm.email;
            lbl_smtp.Text = lm.smtps;
            lbl_username.Text = lm.username;
        }
        else
        {
            btn_register.Text = "Benutzer registrieren";
        }
	}
	public async void OnButtonRegClicked(object sender, EventArgs e)
	{
        if (btn_register.Text == "Benutzer registrieren")
        {
            if (lbl_password.Text == lbl_password_again.Text)
            {
                if (lbl_name.Text != "" || lbl_surname.Text != "" || lbl_password.Text != "" || lbl_username.Text != "")
                {
                    if (lbl_smtp.Text == "" || lbl_smtppassword.Text == "" || lbl_email.Text == "")
                    {
                        bool result = await DisplayAlert("ACHTUNG", "Ihre Email-Daten sind nicht vollständig - wollen Sie trotzdem fortfahren?", "JA", "NEIN");
                        if (result)
                        {
                            LoginModel lm = new LoginModel();
                            lm.name = lbl_name.Text;
                            lm.surname = lbl_surname.Text;
                            lm.email = lbl_email.Text;
                            lm.smtps = lbl_smtp.Text;
                            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                            lm.emailpassword = BCrypt.Net.BCrypt.HashPassword(lbl_smtppassword.Text, salt);
                            lm.username = lbl_username.Text;
                            lm.password = BCrypt.Net.BCrypt.HashPassword(lbl_password.Text, salt);

                            DBHandler.InsertUser(lm);
                            
                            await Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        LoginModel lm = new LoginModel();
                        lm.name = lbl_name.Text;
                        lm.surname = lbl_surname.Text;
                        lm.email = lbl_email.Text;
                        lm.smtps = lbl_smtp.Text;
                        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                        lm.emailpassword = BCrypt.Net.BCrypt.HashPassword(lbl_smtppassword.Text, salt);
                        lm.username = lbl_username.Text;
                        lm.password = BCrypt.Net.BCrypt.HashPassword(lbl_password.Text, salt);
                        DBHandler.InsertUser(lm);

                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    await DisplayAlert("FEHLER", "Bitte geben Sie die Plichtfelder (Vorname | Nachname | Benutzername | Passwort) an!", "OK");
                }
            }
            else
            {
                await DisplayAlert("FEHLER", "Ihre Passwörter stimmen nicht überein", "OK");
            } 
        }
        else
        {
            LoginModel lm = new LoginModel();
            lm._id = loginuserid;
            lm.name = lbl_name.Text;
            lm.surname = lbl_surname.Text;
            lm.email = lbl_email.Text;
            lm.smtps = lbl_smtp.Text;
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            lm.emailpassword = BCrypt.Net.BCrypt.HashPassword(lbl_smtppassword.Text, salt);
            lm.username = lbl_username.Text;
            lm.password = BCrypt.Net.BCrypt.HashPassword(lbl_password.Text, salt);
            DBHandler.ModifyLoginUser(lm);

            await Navigation.PopAsync();
        }

	}
}