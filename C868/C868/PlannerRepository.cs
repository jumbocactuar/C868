using C868.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace C868
{
    public class PlannerRepository
    {
        SQLiteConnection cxn;

        public int SelectedTerm;

        public int SelectedCourse;

        public int SelectedAssessment;

        public PlannerRepository(string dbPath)
        {
            cxn = new SQLiteConnection(dbPath);
            cxn.CreateTable<User>();
            cxn.CreateTable<Term>();
            cxn.CreateTable<Course>();
            cxn.CreateTable<Assessment>();
        }

        public void AddAssessment(int courseID, string name, string type, DateTime start, DateTime end, bool notify)
        {
            cxn.Insert(new Assessment { CourseID = courseID, Name = name, Type = type, Start = start, End = end, NotificationsEnabled = notify });
        }

        public void AddCourse(int termID, string courseName, DateTime start, DateTime end, bool notify, string status, string instName, string instPhone, string instEmail, string notes, string grade)
        {
            cxn.Insert(new Course { TermID = termID, CourseName = courseName, Start = start, End = end, NotificationsEnabled = notify, Status = status, InstName = instName, InstPhone = instPhone, InstEmail = instEmail, Notes = notes, Grade = grade });
        }

        public void AddTerm(string title, DateTime start, DateTime end)
        {
            cxn.Insert(new Term { Title = title, Start = start, End = end });
        }

        public void AddUser(string userName, string password)
        {
            cxn.Insert(new User { UserName = userName, Password = password });
        }

        public void DeleteAssessment(int id)
        {
            cxn.Query<Assessment>("DELETE FROM assessments WHERE AssessmentID = ?", id);
        }

        public void DeleteCourse(int id)
        {
            cxn.Query<Course>("DELETE FROM courses WHERE CourseID = ?", id);
        }

        public void DeleteTerm(int id)
        {
            cxn.Query<Term>("DELETE FROM terms WHERE TermID = ?", id);
        }

        public ObservableCollection<Assessment> GetAllAssessments()
        {
            var filteredList = cxn.Query<Assessment>("SELECT * FROM assessments");

            ObservableCollection<Assessment> filteredCollection = new ObservableCollection<Assessment>();

            foreach (Assessment assessment in filteredList)
            {
                filteredCollection.Add(assessment);
            }

            return filteredCollection;
        }

        public ObservableCollection<Course> GetAllCourses()
        {
            var filteredList = cxn.Query<Course>("SELECT * FROM courses");

            ObservableCollection<Course> filteredCollection = new ObservableCollection<Course>();

            foreach (Course course in filteredList)
            {
                filteredCollection.Add(course);
            }

            return filteredCollection;
        }

        public List<User> GetAllUsers()
        {
            var list = cxn.Query<User>("SELECT * FROM users");

            return list;
        }

        public ObservableCollection<Assessment> GetAssessmentsList()
        {
            int courseID = App.PlannerRepo.SelectedCourse;

            var filteredList = cxn.Query<Assessment>("SELECT * FROM assessments WHERE CourseID = ?", courseID);

            ObservableCollection<Assessment> filteredCollection = new ObservableCollection<Assessment>();

            foreach (Assessment assessment in filteredList)
            {
                filteredCollection.Add(assessment);
            }

            return filteredCollection;
        }

        public ObservableCollection<Course> GetCoursesList()
        {
            int termID = App.PlannerRepo.SelectedTerm;

            var filteredList = cxn.Query<Course>("SELECT * FROM courses WHERE TermID = ?", termID);

            ObservableCollection<Course> filteredCollection = new ObservableCollection<Course>();

            foreach (Course course in filteredList)
            {
                filteredCollection.Add(course);
            }

            return filteredCollection;
        }

        public ObservableCollection<Term> GetTermsList()
        {
            var temp = cxn.Table<Term>().ToList();

            ObservableCollection<Term> collection = new ObservableCollection<Term>();

            foreach (Term term in temp)
            {
                collection.Add(term);
            }

            return collection;
        }

        public Assessment GetSelectedAssessment()
        {
            int assessmentID = App.PlannerRepo.SelectedAssessment;

            var result = cxn.Query<Assessment>("SELECT * FROM assessments WHERE AssessmentID = ?", assessmentID);

            var selectedAssessment = result[0];

            return selectedAssessment;
        }

        public Course GetSelectedCourse()
        {
            int courseID = App.PlannerRepo.SelectedCourse;

            var result = cxn.Query<Course>("SELECT * FROM courses WHERE CourseID = ?", courseID);

            var selectedCourse = result[0];

            return selectedCourse;
        }

        public Term GetSelectedTerm()
        {
            int termID = App.PlannerRepo.SelectedTerm;

            var result = cxn.Query<Term>("SELECT * FROM terms WHERE TermID = ?", termID);

            var selectedTerm = result[0];

            return selectedTerm;
        }

        public void UpdateAssessment(int id, string name, string type, DateTime start, DateTime end, bool notify)
        {
            cxn.Query<Assessment>("UPDATE assessments SET Name = ?, Type = ?, Start = ?, End = ?, NotificationsEnabled = ? WHERE AssessmentID = ?", name, type, start, end, notify, id);
        }

        public void UpdateCourse(int id, string courseName, DateTime start, DateTime end, bool notify, string status, string instName, string instPhone, string instEmail, string notes, string grade)
        {
            cxn.Query<Course>("UPDATE courses SET CourseName = ?, Start = ?, End = ?, NotificationsEnabled = ?, Status = ?, InstName = ?, InstPhone = ?, InstEmail = ?, Notes = ?, Grade = ? WHERE CourseID = ?", courseName, start, end, notify, status, instName, instPhone, instEmail, notes, grade, id);
        }

        public void UpdateTerm(int id, string title, DateTime start, DateTime end)
        {
            cxn.Query<Term>("UPDATE terms SET Title = ?, Start = ?, End = ? WHERE TermID = ?", title, start, end, id);
        }

        public bool AddAssessmentTypeChecker(object proposedType)
        {
            bool result = true;
            ObservableCollection<Assessment> assessmentList = App.PlannerRepo.GetAssessmentsList();

            if (proposedType == null)
            {
                return result = true;
            }

            if (assessmentList.Count > 0)
            {
                if (assessmentList[0].Type == proposedType.ToString())
                {
                    return result = false;
                }
            }

            return result;
        }

        public bool EditAssessmentTypeChecker(object proposedType)
        {
            bool result = true;
            ObservableCollection<Assessment> assessmentList = App.PlannerRepo.GetAssessmentsList();

            if (proposedType == null)
            {
                return result = false;
            }

            if (assessmentList.Count == 1)
            {
                foreach (Assessment assessment in assessmentList)
                {
                    if (assessment.Type == proposedType.ToString())
                    {
                        if (assessment.AssessmentID == App.PlannerRepo.SelectedAssessment)
                        {
                            return result = true;
                        }

                        else
                        {
                            return result = false;
                        }
                    }
                }
            }

            if (assessmentList.Count == 2)
            {
                foreach (Assessment assessment in assessmentList)
                {
                    if (assessment.Type == proposedType.ToString())
                    {
                        if (assessment.AssessmentID == App.PlannerRepo.SelectedAssessment)
                        {
                            return result = true;
                        }

                        else
                        {
                            return result = false;
                        }
                    }
                }
            }

            return result;
        }

        public bool DateChecker(DateTime start, DateTime end)
        {
            bool result = true;

            if (start > end)
            {
                return result = false;
            }

            return result;
        }

        public bool EmailChecker(string email)
        {
            bool result = false;
            // Allow the 'at' symbol and period
            bool atPresent = false;
            bool periodPresent = false;

            // If the email is null or empty, mark it as invalid
            if (email == null || email == "")
            {
                return result = false;
            }

            else
            {
                // If the email does not contain an 'at' symbol and period, mark it as invalid
                foreach (char c in email)
                {
                    if (c == '@')
                    {
                        atPresent = true;
                    }

                    if (c == '.')
                    {
                        periodPresent = true;
                    }
                }

                if (atPresent == true && periodPresent == true)
                {
                    return result = true;
                }
            }

            return result;
        }

        public bool EntryChecker(string valueEntered)
        {
            bool result = true;

            if (valueEntered == null || valueEntered == "")
            {
                return result = false;
            }

            return result;
        }

        public bool LoginChecker(string userName, string password)
        {
            bool result = false;
            bool userNameOK = false;
            bool passwordOK = false;

            List<User> userList = GetAllUsers();

            // If the user name and/or password is null or empty, mark it/them as invalid
            if (userName == null || userName == "")
            {
                return result = false;
            }

            if (password == null || password == "")
            {
                return result = false;
            }

            else
            {
                // If both the user name and password match those of a user in the database, mark it as valid
                foreach (User user in userList)
                {
                    if (user.UserName == userName)
                    {
                        userNameOK = true;
                    }

                    if (user.Password == password)
                    {
                        passwordOK = true;
                    }
                }

                if (userNameOK == true && passwordOK == true)
                {
                    return result = true;
                }
            }

            return result;
        }

        public bool PhoneChecker(string phone)
        {
            bool result = true;
            // Allow parentheses for area codes; plus symbol for international numbers; 'x' for extensions; dashes, periods, and spaces per the user's format preferences
            List<char> allowed = new List<char> { '(', ')', '-', '+', ' ', 'x', '.' };

            // If the phone number is null or empty, mark it as invalid
            if (phone == null || phone == "")
            {
                return result = false;
            }

            else
            {
                // If the phone number does not contain only numbers or allowed chatacters, mark it as invalid
                foreach (char c in phone)
                {
                    if (c < '0' || c > '9')
                    {
                        if (allowed.Contains(c) == false)
                        {
                            return result = false;
                        }
                    }
                }
            }

            return result;
        }

        public bool PickerChecker(object selection)
        {
            bool result = true;

            if (selection == null)
            {
                return result = false;
            }

            return result;
        }
    }
}
