using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kalender.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using BCrypt;

namespace Kalender.Utils
{
    public static class DBHandler
    {
        private static readonly IMongoCollection<EmployeeModel> _employees;
        private static readonly IMongoCollection<CalendarModel> _assignments;
        private static readonly IMongoCollection<LoginModel> _login;
        static DBHandler()
        {
            var client = new MongoClient("mongodb://mongoadmin:%2Bgrant2DB%21@54.37.204.21:6969/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            var database = client.GetDatabase("Calendar");
            _employees = database.GetCollection<EmployeeModel>("Employees");
            _assignments = database.GetCollection<CalendarModel>("Assignment");
            _login = database.GetCollection<LoginModel>("Login");
        }
        public static List<EmployeeModel> GetAllEmployees() =>
            _employees.Find(employee => true).ToList();

        public static async Task<List<string>> GetAllEmployeesName()
        {
            List<string> temp = new List<string>();
            (await _employees.Find(employee => true).ToListAsync()).ForEach(x => temp.Add(x.name));
            return temp;
        }

        public static EmployeeModel GetEmployeeData(string name) =>
            _employees.Find(employee => employee.name.Equals(name)).FirstOrDefault();

        public static void InsertEmployee(EmployeeModel employee) =>
            _employees.InsertOne(employee);

        public static void ModifyEmployee(EmployeeModel em) =>
            _employees.ReplaceOne(employee => employee._id.Equals(em._id), em);

        public static void RemoveEmployee(string id) =>
            _employees.DeleteOne(r => r._id.Equals(id));

        public static List<CalendarModel> GetAllAssignments() =>
            _assignments.Find(assignment => true).ToList();

        public static void InsertAssignment(CalendarModel assignment)
        {
            try { _assignments.InsertOne(assignment); } catch { }
        }

        public static void RemoveAssignment(string id) =>
            _assignments.DeleteOne(r => r._id.Equals(id));

        public static CalendarModel AssignmentAlreadyExisting(DateTime assdate) =>
            _assignments.Find(assignment => assignment.date.Equals(assdate)).FirstOrDefault();

        public static CalendarModel GetAssignmentDateTime(DateTime assdate)
        {
            CalendarModel cm = new CalendarModel();
            _assignments.Find(assignment => assignment.date.Equals(assdate)).FirstOrDefault();
            return cm;
        }

        public static int GetAssignmentCountTitel(string titel)
        {
            List<string> temp = new List<string>();
            _assignments.Find(assignment => assignment.titel.Equals(titel)).ToList().ForEach(x => temp.Add(x.titel));
            return temp.Count();
        }

        public static void ModifyAssignment(CalendarModel cm) =>
            _assignments.ReplaceOne(assignment => assignment._id.Equals(cm._id), cm);

        public static void DeleteAssignment(string id) =>
            _assignments.DeleteOne(assignment => assignment._id.Equals(id));

        public static void DeleteManyAssignments(string titel) =>
            _assignments.DeleteMany(assignment => assignment.titel.Equals(titel));

        public static string GetAssignmentTitel(string id)
        {
            CalendarModel cm = new CalendarModel();
            cm = _assignments.Find(assignment => assignment._id.Equals(id)).FirstOrDefault();
            return cm.titel;
        }
        public static void AdvancedDeleteAssignment(string id)
        {
            string titel = GetAssignmentTitel(id);
            if (GetAssignmentCountTitel(titel) > 1)
            {
                DeleteManyAssignments(titel);
            }
            else
            {
                DeleteAssignment(id);
            }
        }

        public static void InsertUser(LoginModel login)
        {
            if(login.username == LoginGetUsers(login.username).username)
            {
                try { _login.InsertOne(login); } catch { }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Fehler", "Benutzer existiert bereits", "OK");
            }
        }

        public static LoginModel LoginGetUsers(string username) =>
            _login.Find(login => login.username.Equals(username)).FirstOrDefault();

        public static bool Login(string username, string passwort)
        {
            try
            {
                LoginModel lm = LoginGetUsers(username);

                if (BCrypt.Net.BCrypt.Verify(passwort, lm.password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Fehler", "Der angegebene Benutzername existiert nicht!", "OK");
                return false;
            }
        }

        public static void ModifyLoginUser(LoginModel lm) =>
            _login.ReplaceOne(login => login._id.Equals(lm._id), lm);
    }
}
