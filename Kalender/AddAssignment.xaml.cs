using System.Reflection.Metadata.Ecma335;

namespace Kalender;


public partial class AddAssignment : ContentPage
{
	public AddAssignment()
	{
		InitializeComponent();
		FillData();
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

		dpp_return.Date =	CalendarModel.CurrentDate;

		try
        {
            if (SessionData.datadic[CalendarModel.CurrentDate] != "")
            {
                datetitel.Text = SessionData.datadic[CalendarModel.CurrentDate];
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
	}
	private void OnButtonAddClicked(object sender, EventArgs args)
    {
		//Outdated creation just testing some stuff 
		//will soon be changed to dictionary / api
		
		SessionData.titel = datetitel.Text;
		string tempdate = dpp_return.Date.ToString().Remove(10);
		if (tempdate.StartsWith("08"))
		{
			SessionData.assdate = tempdate.Replace("08.12.2022","8.12.2022");
		}
		else
		{
            SessionData.assdate = tempdate;
        }

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