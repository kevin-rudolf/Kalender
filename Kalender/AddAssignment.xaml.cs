using System.Reflection.Metadata.Ecma335;
using System.Text;
using Amazon.Runtime.EventStreams.Internal;
using Kalender.Models;
using Kalender.Utils;

namespace Kalender;


public partial class AddAssignment : ContentPage
{
    private string pickervalue;
    private bool alreadyfilled = false;
    private string alreadyfilledid = "";
    public AddAssignment()
    {
        InitializeComponent();

        FillData();
        SessionData.plzdata.Clear();
        ReadTextFile();

        if (DBHandler.AssignmentAlreadyExisting(SessionData.CurrentDate.ToLocalTime()) != null)
        {
            alreadyfilled = true;
            CalendarModel cm = DBHandler.AssignmentAlreadyExisting(SessionData.CurrentDate.ToLocalTime());

            datetitel.Text = cm.titel;
            lbl_plz.Text = cm.plz;
            lbl_destination.Text = cm.city;
            lbl_adress.Text = cm.address;
            lbl_customers.SelectedItem = cm.eid;
            lbl_importance.SelectedIndex = cm.importance;
            lbl_repeat.SelectedIndex = cm.repeat;
            lbl_notes.Text = cm.note;

            alreadyfilledid = cm._id;
            btn_save.Text = "Termin ändern";
        }
    }
    private List<string> customerlist = new List<string>();
    private void FillData()
    {
        lbl_editor.Text = SessionData.editorsurname + " " + SessionData.editorname;

        //fills custom data - will be changed to api
        customerlist.Add("Bitte wählen Sie einen Mitarbeiter aus:");

        var task = Task.Run<List<string>>(async () => await DBHandler.GetAllEmployeesName());
        var result = task.Result;

        customerlist.AddRange(result);

        List<string> importancelist = new List<string>();
        importancelist.Add("Wählen Sie die Dringlichkeit Ihres Termins");
        importancelist.Add("Normal");
        importancelist.Add("Wichtig");
        importancelist.Add("Sehr Wichtig");

        List<string> repeatlist = new List<string>();
        repeatlist.Add("Soll der Termin monatlich wiederhohlt werden?");
        repeatlist.Add("Jedes Monat wiederholen");

        lbl_customers.ItemsSource = customerlist;
        lbl_customers.SelectedItem = customerlist[0];
        lbl_importance.ItemsSource = importancelist;
        lbl_importance.SelectedItem = importancelist[0];
        lbl_repeat.ItemsSource = repeatlist;
        lbl_repeat.SelectedItem = repeatlist[0];

        cp_main.Title = SessionData.CurrentDate.ToString("dd.MM.yyyy");
    }

    public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            string selectedentry;
            selectedentry = (string)picker.ItemsSource[selectedIndex];
            pickervalue = selectedentry;
        }
        else { }

        if (DBHandler.GetEmployeeData(pickervalue) != null)
        {
            EmployeeModel em = DBHandler.GetEmployeeData(pickervalue);

            lbl_id.Text = em._id;
            lbl_vorname_e.Text = em.surname;
            lbl_nachname_e.Text = em.name;
            lbl_email_e.Text = em.email;
            lbl_plz_e.Text = em.plz;
            lbl_city_e.Text = em.city;
            lbl_address_e.Text = em.address;
            lbl_birthday_e.Date = em.birthday;

            lbl_vorname_e.IsVisible = true;
            lbl_nachname_e.IsVisible = true;
            lbl_email_e.IsVisible = true;
            lbl_plz_e.IsVisible = true;
            lbl_city_e.IsVisible = true;
            lbl_address_e.IsVisible = true;
            lbl_birthday_e.IsVisible = true;

            btn_saveemployee.Text = "Benutzer ändern";
            btn_saveemployee.IsVisible = true;
            btn_deleteemployee.IsVisible = true;
        }
        else
        {
            lbl_vorname_e.IsVisible = false;
            lbl_nachname_e.IsVisible = false;
            lbl_email_e.IsVisible = false;
            lbl_plz_e.IsVisible = false;
            lbl_city_e.IsVisible = false;
            lbl_address_e.IsVisible = false;
            lbl_birthday_e.IsVisible = false;
            btn_saveemployee.IsVisible = false;
            btn_deleteemployee.IsVisible = false;
        }
    }

    public async void ReadTextFile()
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("PLZ_Verzeichnis_Österreich.csv");
        using StreamReader reader = new StreamReader(fileStream, UTF8Encoding.Default);

        while (!reader.EndOfStream)
        {
            try
            {
                string line = reader.ReadLine();
                string[] temp = line.Split(";");
                SessionData.plzdata.Add(temp[0], temp[1]);
            }
            catch { }
        }
    }

    private void OnPLZChanged(object sender, EventArgs args)
    {
        try
        {
            lbl_destination.Text = "";
            lbl_destination.Text = SessionData.plzdata[lbl_plz.Text];
        }
        catch { }
    }

    private void OnEmployeeClicked(object sender, EventArgs args)
    {
        lbl_vorname_e.IsVisible = true;
        lbl_nachname_e.IsVisible = true;
        lbl_email_e.IsVisible = true;
        lbl_plz_e.IsVisible = true;
        lbl_city_e.IsVisible = true;
        lbl_address_e.IsVisible = true;
        lbl_birthday_e.IsVisible = true;

        btn_saveemployee.Text = "Mitarbeiter speichern";
        btn_saveemployee.IsVisible = true;
        btn_deleteemployee.IsVisible = true;

        lbl_id.Text = "";
        lbl_vorname_e.Text = "";
        lbl_nachname_e.Text = "";
        lbl_email_e.Text = "";
        lbl_plz_e.Text = "";
        lbl_city_e.Text = "";
        lbl_address_e.Text = "";
        lbl_birthday_e.Date = new DateTime(1920,01,01);
    }

    private async void OnButtonAddClicked(object sender, EventArgs args)
    {
        if (alreadyfilled != true)
        {
            CalendarModel cm = new CalendarModel();
            cm.titel = datetitel.Text;
            cm.date = Convert.ToDateTime(SessionData.CurrentDate.ToString("dd.MM.yyyy")).ToLocalTime();
            cm.plz = lbl_plz.Text;
            cm.address = lbl_adress.Text;
            cm.city = lbl_destination.Text;
            cm.eid = lbl_customers.SelectedItem.ToString();
            cm.importance = lbl_importance.SelectedIndex;
            cm.repeat = lbl_repeat.SelectedIndex;
            cm.note = lbl_notes.Text;

            if (cm.repeat == 1)
            {
                int importantday = cm.date.Day;
                DBHandler.InsertAssignment(cm);
                for (int i = 1; i < 100; i++)
                {
                    CalendarModel cmtemp = new CalendarModel();
                    cmtemp.titel = cm.titel;
                    int monthtemp = (cm.date.Month + i - 1) / 12;
                    int x = (cm.date.Month + i) % 12;
                    cmtemp.date = new DateTime(cm.date.Year + monthtemp, x + 12 * (int)Math.Pow(0, x), importantday).ToLocalTime();
                    cmtemp.plz = cm.plz;
                    cmtemp.address = cm.address;
                    cmtemp.city = cm.city;
                    cmtemp.eid = cm.eid;
                    cmtemp.importance = cm.importance;
                    cmtemp.repeat = cm.repeat;
                    cmtemp.note = cm.note;
                    DBHandler.InsertAssignment(cmtemp);
                }
            }
            DBHandler.InsertAssignment(cm);

            bool result = await Application.Current.MainPage.DisplayAlert("E-Mail senden?", "Wollen Sie dem Mitarbeiter " + lbl_customers.SelectedItem.ToString() + " eine E-Mail senden?", "Ja", "Nein");

            if (result)
            {
                if (DBHandler.LoginGetUsers(SessionData.username).smtps != "")
                {
                    if (pickervalue != null)
                    {
                        if (datetitel.Text != null && lbl_notes.Text != null)
                            Mailer.SendMail(datetitel.Text, lbl_plz.Text, lbl_destination.Text, lbl_adress.Text, lbl_notes.Text, SessionData.editorname, SessionData.CurrentDate.ToString("dd.MM.yyyy"), DBHandler.GetEmployeeData(lbl_customers.SelectedItem.ToString()).email);
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Um diese Funktion zu nutzen müssen Sie bei Ihren Benutzerdaten die SMTP-Daten hinzufügen", "OK");
                }
            }
        }
        else
        {
            CalendarModel cm = new CalendarModel();
            cm._id = alreadyfilledid;
            cm.titel = datetitel.Text;
            cm.date = SessionData.CurrentDate.ToLocalTime();
            cm.plz = lbl_plz.Text;
            cm.address = lbl_adress.Text;
            cm.city = lbl_destination.Text;
            cm.eid = lbl_customers.SelectedItem.ToString();
            cm.importance = lbl_importance.SelectedIndex;
            cm.repeat = lbl_repeat.SelectedIndex;
            cm.note = lbl_notes.Text;
            DBHandler.ModifyAssignment(cm);

            bool result = await Application.Current.MainPage.DisplayAlert("E-Mail senden?", "Wollen Sie dem Mitarbeiter " + lbl_customers.SelectedItem.ToString() + " eine E-Mail mit den geänderten Daten senden?", "Ja", "Nein");

            if (result)
            {
                if (DBHandler.LoginGetUsers(SessionData.username).smtps != "")
                {
                    if (pickervalue != null)
                    {
                        if (datetitel.Text != null && lbl_notes.Text != null)
                            Mailer.SendMail(datetitel.Text, lbl_plz.Text, lbl_destination.Text, lbl_adress.Text, lbl_notes.Text, SessionData.editorname, SessionData.CurrentDate.ToString("dd.MM.yyyy"), DBHandler.GetEmployeeData(lbl_customers.SelectedItem.ToString()).email);
                    }
                }
                else
                {
                    await DisplayAlert("Fehler", "Um diese Funktion zu nutzen müssen Sie bei Ihren Benutzerdaten die SMTP-Daten hinzufügen", "OK");
                }
            }
        }

        MessagingCenter.Send(Application.Current, "RefreshMainPage");
        await Navigation.PopAsync(true);
    }
    private void OnButtonDeleteClicked(object sender, EventArgs args)
    {
        if (alreadyfilled)
        {
            DBHandler.AdvancedDeleteAssignment(alreadyfilledid);
        }

        MessagingCenter.Send(Application.Current, "RefreshMainPage");
        Navigation.PopAsync(true);
    }

    private void SaveButtonClicked(object sender, EventArgs args)
    {
        if(btn_saveemployee.Text == "Mitarbeiter speichern")
        {
            EmployeeModel em = new EmployeeModel();
            em.surname = lbl_vorname_e.Text;
            em.name = lbl_nachname_e.Text;
            em.plz = lbl_plz_e.Text;
            em.city = lbl_city_e.Text;
            em.address = lbl_address_e.Text;
            em.email = lbl_email_e.Text;
            em.birthday = lbl_birthday_e.Date;

            DBHandler.InsertEmployee(em);

            //Wie geht das mit Clearen --> sollte dann autoamtisch wenn mitarbeiter erstellt --> zurückgesetzt werden und zu liste hinzugefügt werden
            customerlist.Clear();
            //customerlist.Add("Bitte wählen Sie einen Mitarbeiter aus:");

            //var task = Task.Run<List<string>>(async () => await DBHandler.GetAllEmployeesName());
            //var result = task.Result;

            //customerlist.AddRange(result);
        }
        else
        {
            EmployeeModel em = new EmployeeModel();
            em._id = lbl_id.Text;
            em.surname = lbl_vorname_e.Text;
            em.name = lbl_nachname_e.Text;
            em.plz = lbl_plz_e.Text;
            em.city = lbl_city_e.Text;
            em.address = lbl_address_e.Text;
            em.email = lbl_email_e.Text;
            em.birthday = lbl_birthday_e.Date;

            DBHandler.ModifyEmployee(em);

            customerlist.Clear();
            customerlist.Add("Bitte wählen Sie einen Mitarbeiter aus:");

            var task = Task.Run<List<string>>(async () => await DBHandler.GetAllEmployeesName());
            var result = task.Result;

            customerlist.AddRange(result);
        }
    }

    private void DeleteButtonClicked(object sender, EventArgs args)
    {
        EmployeeModel em = new EmployeeModel();
        em._id = lbl_id.Text;
        em.surname = lbl_vorname_e.Text;
        em.name = lbl_nachname_e.Text;
        em.plz = lbl_plz_e.Text;
        em.city = lbl_city_e.Text;
        em.address = lbl_address_e.Text;
        em.email = lbl_email_e.Text;
        em.birthday = lbl_birthday_e.Date;

        DBHandler.RemoveEmployee(em._id);

        customerlist.Clear();
        customerlist.Add("Bitte wählen Sie einen Mitarbeiter aus:");

        var task = Task.Run<List<string>>(async () => await DBHandler.GetAllEmployeesName());
        var result = task.Result;

        customerlist.AddRange(result);
    }
}