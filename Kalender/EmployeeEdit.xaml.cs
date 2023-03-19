using Kalender.Models;
using Kalender.Utils;

namespace Kalender;

public partial class EmployeeEdit : ContentPage
{
	public EmployeeEdit()
	{
		InitializeComponent();
	}

	public void SaveButtonClicked(object sender, EventArgs e)
	{
		EmployeeModel em = new EmployeeModel();
		em.surname = lbl_vorname.Text;
		em.name = lbl_nachname.Text;
		em.plz = lbl_plz.Text;
		em.city = lbl_city.Text;
		em.address = lbl_address.Text;
		em.email = lbl_email.Text;
		em.birthday = lbl_birthday.Date;

        DBHandler.InsertEmployee(em);
		Navigation.PopAsync();
	}

	public void DeleteButtonClicked(object sender, EventArgs e)
	{
	}

 }