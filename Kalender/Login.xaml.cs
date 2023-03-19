using Kalender.Utils;
using System.Security.Cryptography;

namespace Kalender;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	public void OnButtonLoginClicked(object sender, EventArgs e)
	{
		if(DBHandler.Login(lbl_username.Text, lbl_password.Text))
		{
            SessionData.username = lbl_username.Text;
			SessionData.editorsurname = DBHandler.LoginGetUsers(lbl_username.Text).surname;
			SessionData.editorname = DBHandler.LoginGetUsers(lbl_username.Text).name;
            Navigation.PushAsync(new CalendarView());
		}
		else
		{
			DisplayAlert("FEHLER", "Die Benutzerdaten waren falsch", "OK");
		}
    }
    public void OnButtonChangeClicked(object sender, EventArgs e)
    {
		if(BCrypt.Net.BCrypt.Verify(lbl_password.Text, DBHandler.LoginGetUsers(lbl_username.Text).password))
		{
            Navigation.PushAsync(new LoginReg(lbl_username.Text, lbl_password.Text));
        }
		else
		{
			DisplayAlert("Fehler", "Anmeldendaten sind falsch", "OK");
		}
    }

	public void OnLabelRegTapped(object sender, EventArgs args)
	{
		Navigation.PushAsync(new LoginReg(lbl_username.Text, lbl_password.Text));
	}
}