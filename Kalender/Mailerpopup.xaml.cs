using System.Security.Cryptography;
using System.IO;
namespace Kalender;

public partial class Mailerpopup : ContentPage
{
    private string maildatapath = "";
    public Mailerpopup(string maildatapath)
	{
		InitializeComponent();
        this.maildatapath = maildatapath;
        lbl_passwort.IsPassword = true;
        lbl_passwort_repeat.IsPassword = true;
	}

    public void SaveButtonClicked(object sender, EventArgs e)
    {
        if (lbl_passwort.Text.Equals(lbl_passwort_repeat.Text))
        {
            using(AesManaged aes = new AesManaged())
            {
                byte[] encrypted = Encrypt(lbl_passwort.Text, aes.Key, aes.IV);
                StreamWriter sw = new StreamWriter(maildatapath);
                sw.WriteLine(lbl_vorname.Text);
                sw.WriteLine(lbl_nachname.Text);
                sw.WriteLine(lbl_email.Text);
                sw.WriteLine(lbl_emailserver.Text);
                sw.WriteLine(encrypted);
                sw.Close();
                sw.Dispose();
            }

            SessionData.editorsurname = lbl_vorname.Text;
            SessionData.editorname = lbl_nachname.Text;
            Navigation.PopAsync();
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Passwort-Fehler", "Bitte überprüfen Sie, ob beide Passwörter übereinstimmen", "OK");
        }
    }

    private byte[] Encrypt(string text, byte[] key, byte[] IV)
    {
        byte[] encrypted;

        using(AesManaged aes = new AesManaged())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(text);
                    encrypted = ms.ToArray();
                }
            }
        }
        return encrypted;
    }

    public void CancelButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}