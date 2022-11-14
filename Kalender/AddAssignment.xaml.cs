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

		List<string> customerlist = new List<string>();
		customerlist.Add("Moritz J�chl");
        customerlist.Add("David Mader");
        customerlist.Add("Dominik Walch");
		customerlist.Add("Bitte w�hlen Sie den gew�nschten Kunden/in aus");

		lbl_customers.ItemsSource = customerlist;
		lbl_customers.SelectedItem = customerlist[3];

		dpp_departure.Date = CalendarModel.CurrentDate;
		dpp_return.Date =	CalendarModel.CurrentDate.AddDays(7);
	}
}