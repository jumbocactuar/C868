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

        public User CurrentUser;

        public int SelectedTerm;

        public int SelectedCourse;

        public int SelectedAssessment;

        public int SelectedOA;

        public int SelectedPA;

        public PlannerRepository(string dbPath)
        {
            cxn = new SQLiteConnection(dbPath);
            cxn.CreateTable<User>();
            cxn.CreateTable<Term>();
            cxn.CreateTable<Course>();
            cxn.CreateTable<Assessment>();
            cxn.CreateTable<FlashCard>();
            cxn.CreateTable<Requirement>();
        }

        public void AddAssessment(int courseID, string name, string type, DateTime start, DateTime end, bool notify)
        {
            cxn.Insert(new Assessment { CourseID = courseID, Name = name, Type = type, Start = start, End = end, NotificationsEnabled = notify });
        }

        public void AddCourse(int termID, string courseName, DateTime start, DateTime end, bool notify, string status, string instName, string instPhone, string instEmail, string notes, string grade)
        {
            cxn.Insert(new Course { TermID = termID, CourseName = courseName, Start = start, End = end, NotificationsEnabled = notify, Status = status, InstName = instName, InstPhone = instPhone, InstEmail = instEmail, Notes = notes, Grade = grade });
        }

        public void AddFlashCard(int assessmentID, string question, string answer, string confidence)
        {
            cxn.Insert(new FlashCard { AssessmentID = assessmentID, Question = question, Answer = answer, Confidence = confidence });
        }

        public void AddRequirement(int assessmentID, string req, string notes, bool satisfied)
        {
            cxn.Insert(new Requirement { AssessmentID = assessmentID, Req = req, Notes = notes, Satisfied = satisfied });
        }

        public void AddTerm(int userID, string title, DateTime start, DateTime end)
        {
            cxn.Insert(new Term { UserID = userID, Title = title, Start = start, End = end });
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

        public void DeleteFlashCard(int id)
        {
            cxn.Query<FlashCard>("DELETE FROM flashcards WHERE CardID = ?", id);
        }

        public void DeleteRequirement(int id)
        {
            cxn.Query<Requirement>("DELETE FROM requirements WHERE ReqID = ?", id);
        }

        public void DeleteTerm(int id)
        {
            cxn.Query<Term>("DELETE FROM terms WHERE TermID = ?", id);
        }

        public ObservableCollection<Course> GenerateSearchResults(string argument)
        {
            ObservableCollection<Course> allCourses = GetAllCourses();
            ObservableCollection<Course> filteredCourses = new ObservableCollection<Course>();

            foreach (Course course in allCourses)
            {
                // Put CourseName and argument into comparable forms
                string lowerName = course.CourseName.ToLower();
                string lowerArg = argument.ToLower();

                if (lowerName.Contains(lowerArg) == true)
                {
                    filteredCourses.Add(course);
                }
            }

            return filteredCourses;
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

        public ObservableCollection<FlashCard> GetFlashCardList()
        {
            int oaID = App.PlannerRepo.SelectedAssessment;

            var filteredList = cxn.Query<FlashCard>("SELECT * FROM flashcards WHERE AssessmentID = ?", oaID);

            ObservableCollection<FlashCard> filteredCollection = new ObservableCollection<FlashCard>();

            foreach (FlashCard card in filteredList)
            {
                filteredCollection.Add(card);
            }

            return filteredCollection;
        }

        public ObservableCollection<Requirement> GetRequirementsList()
        {
            int paID = App.PlannerRepo.SelectedAssessment;

            var filteredList = cxn.Query<Requirement>("SELECT * FROM requirements WHERE AssessmentID = ?", paID);

            ObservableCollection<Requirement> filteredCollection = new ObservableCollection<Requirement>();

            foreach (Requirement req in filteredList)
            {
                filteredCollection.Add(req);
            }

            return filteredCollection;
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

        public void UpdateAssessment(int id, string name, string type, DateTime start, DateTime end, bool notify)
        {
            cxn.Query<Assessment>("UPDATE assessments SET Name = ?, Type = ?, Start = ?, End = ?, NotificationsEnabled = ? WHERE AssessmentID = ?", name, type, start, end, notify, id);
        }

        public void UpdateCourse(int id, string courseName, DateTime start, DateTime end, bool notify, string status, string instName, string instPhone, string instEmail, string notes, string grade)
        {
            cxn.Query<Course>("UPDATE courses SET CourseName = ?, Start = ?, End = ?, NotificationsEnabled = ?, Status = ?, InstName = ?, InstPhone = ?, InstEmail = ?, Notes = ?, Grade = ? WHERE CourseID = ?", courseName, start, end, notify, status, instName, instPhone, instEmail, notes, grade, id);
        }

        public void UpdateFlashCard(int id, string question, string answer, string confidence)
        {
            cxn.Query<FlashCard>("UPDATE flashcards SET Question = ?, Answer = ?, Confidence = ? WHERE CardID = ?", question, answer, confidence, id);
        }

        public void UpdateRequirement(int id, string req, string notes, bool satisfied)
        {
            cxn.Query<Requirement>("UPDATE requirements SET Req = ?, Notes = ?, Satisfied = ? WHERE ReqID = ?", req, notes, satisfied, id);
        }

        public void UpdateTerm(int id, string title, DateTime start, DateTime end)
        {
            cxn.Query<Term>("UPDATE terms SET Title = ?, Start = ?, End = ? WHERE TermID = ?", title, start, end, id);
        }

        public void UpdateUser(int id, string password)
        {
            cxn.Query<User>("UPDATE users SET Password = ? WHERE UserID = ?", password, id);
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
                    CurrentUser = user;

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

        public List<bool> PasswordChecker(string current, string newPass, string confirm)
        {
            List<bool> results = new List<bool> { false, false };

            // If the Current Password entry matches that of the current user, mark it as valid
            if (current == App.PlannerRepo.CurrentUser.Password)
            {
                results[0] = true;
            }

            // If the proposed new password matches the Confirm Password entry, mark it as valid
            if (newPass == confirm)
            {
                results[1] = true;
            }

            return results;
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

    public class CourseDeletionException : Exception
    {
        public CourseDeletionException()
            : base("This course cannot be deleted because it is associated with one or more assessments.")
        {

        }

        public CourseDeletionException(string messageValue)
            : base(messageValue)
        {

        }

        public CourseDeletionException(string messageValue, Exception inner)
            : base(messageValue, inner)
        {

        }
    }

    public class TermDeletionException : Exception
    {
        public TermDeletionException()
            : base("This term cannot be deleted because it is associated with one or more courses.")
        {

        }

        public TermDeletionException(string messageValue)
            : base(messageValue)
        {

        }

        public TermDeletionException(string messageValue, Exception inner)
            : base(messageValue, inner)
        {

        }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid user name or password")
        {

        }

        public InvalidCredentialsException(string messageValue)
            : base(messageValue)
        {

        }

        public InvalidCredentialsException(string messageValue, Exception inner)
            : base(messageValue, inner)
        {

        }
    }
}
