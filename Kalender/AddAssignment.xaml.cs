using System.Reflection.Metadata.Ecma335;
using System.Text;
using Kalender.Models;

namespace Kalender;


public partial class AddAssignment : ContentPage
{
    private string pickervalue;

	public AddAssignment()
	{
		InitializeComponent();
		FillData();
		SessionData.plzdata.Clear();
        ReadTextFile();
    }

	private void FillData()
	{
		lbl_editor.Text = SessionData.editorsurname + " " + SessionData.editorname;

		//fills custom data - will be changed to api
		List<string> customerlist = new List<string>();
		customerlist.Add("Moritz Jöchl");
        customerlist.Add("David Mader");
        customerlist.Add("Dominik Walch");
		customerlist.Add("Bitte wählen Sie den gewünschten Kunden/in aus");

		lbl_customers.ItemsSource = customerlist;
		lbl_customers.SelectedItem = customerlist[3];

		cp_main.Title =	SessionData.CurrentDate.ToString("dd.MM.yyyy");

		try
        {
            if (SessionData.datadic[SessionData.CurrentDate] != "")
            {
                datetitel.Text = SessionData.datadic[SessionData.CurrentDate];
            }
            else
            {
                datetitel.Text = "";
            }
        }
		catch
		{
			//if Dictionary doesn't contain CurrentDate
		}

        var mailclick = new TapGestureRecognizer();
        mailclick.Tapped += async (s, e) =>
        {
            //Get selected customers email
            if(pickervalue != null)
            {
                if(datetitel.Text != null && lbl_notes.Text != null)
                Mailer.SendMail(datetitel.Text, lbl_plz.Text, lbl_destination.Text, lbl_adress.Text, lbl_notes.Text, SessionData.editorname, SessionData.CurrentDate.ToString("dd.MM.yyyy"));
            }
        };
        mail_image.GestureRecognizers.Add(mailclick);
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

	private void OnButtonMailClicked(object sender, EventArgs args)
	{

	}

	private void OnButtonAddClicked(object sender, EventArgs args)
    {
		//Outdated creation just testing some stuff 
		//will soon be changed to dictionary / api
		
		//SessionData.titel = datetitel.Text;
		//string tempdate = dpp_return.Date.ToString().Remove(10);
		//if (tempdate.StartsWith("08"))
		//{
		//	SessionData.assdate = tempdate.Replace("08.12.2022","8.12.2022");
		//}
		//else
		//{
  //          SessionData.assdate = tempdate;
  //      }



		Navigation.PopAsync();
		CalendarView cv = new CalendarView();
		cv.CreateGrid();
        Navigation.PushAsync(cv);
    }
    private void OnButtonDeleteClicked(object sender, EventArgs args)
    {
		//still missing
    }
}