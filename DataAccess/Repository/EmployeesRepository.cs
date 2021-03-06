﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data;
using System.Web.Configuration;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;

namespace DataAccess.Repository
{
    public class EmployeesRepository
    {
        private ConstructionCompanyEntities database;
        private ConstructionCompanyEntities DB = new ConstructionCompanyEntities();

        public EmployeesRepository()
        {
            database = new ConstructionCompanyEntities();
        }

        public Employee ChekEmployee(string txt)
        {
            return DB.Employees.Where(p => p.Email == txt || p.UserName == txt).FirstOrDefault();
        }

        public DataTable searchEmployeeForScores(string s)
        {
            int a = -1;
            try
            {
                a = s.ToInt();
            }
            catch
            {
            }
            List<EmployeeScore> result = new List<EmployeeScore>();
            var pl = (
              from r in database.EmployeeScores
              where r.FullName.Contains(s) || r.StateCity.Contains(s) || r.UserName.Contains(s)
              orderby r.Score
              select r);
            result = pl.ToList();
            return OnlineTools.ToDataTable(result.ToList());
        }

        public EmployeeScore getEmployeeScore(int id)
        {
            EmployeeScore em = new EmployeeScore();
            em = (
             from r in database.EmployeeScores
             where r.EmployeeID == id
             orderby r.Score
             select r).FirstOrDefault();
            return em;
        }

        public DataTable getTopEmploees()
        {
            var pl =
                (from r in database.EmployeeScores
                 join h in database.Employees on r.EmployeeID equals h.EmployeeID
                 orderby r.Score
                 select new { r.FullName, r.Score, r.EmployeeID, h.empImage }).Take(5);

            return OnlineTools.ToDataTable(pl.ToList());
        }

        public Employee getEmployeeByID(int id)
        {
            Employee result = new Employee();
            result =
                (from r in database.Employees
                 where r.EmployeeID == id
                 select r).FirstOrDefault();

            return result;
        }

        public DataTable getEmployeeForScore()
        {
            List<EmployeeScore> result = new List<EmployeeScore>();

            var pl = (
                from r in database.EmployeeScores

                orderby r.Score descending
                select r);
            result = pl.ToList();
            return OnlineTools.ToDataTable(result.ToList());
        }

        public int getEmployeeIDByUsername_Password(string username, string password)
        {
            int query =
                (from r in database.Employees
                 where ((r.UserName == username) && (r.Password == password))
                 select r.EmployeeID).FirstOrDefault();

            return query;
        }

        public int getLastEmployeeID()
        {
            int query =
               (from r in database.Employees
                orderby r.EmployeeID descending
                select r.EmployeeID).FirstOrDefault();
            return query;
        }

        public void SaveEmployees(Employee kramand)
        {
            if (kramand.EmployeeID > 0)
            {
                //==== UPDATE ====
                database.Employees.Attach(kramand);
                database.Entry(kramand).State = EntityState.Modified;
            }
            else
            {
                //==== INSERT ====
                database.Employees.Add(kramand);
            }

            database.SaveChanges();
        }

        public void setEmployeeImage(int empid, string cnts)
        {
            Employee e =
                  (
                      from r in database.Employees
                      where r.EmployeeID == empid
                      select r
                  ).FirstOrDefault();

            e.empImage = cnts;
            database.SaveChanges();
        }

        public void setEmployeeResume(int empid, byte[] cnts)
        {
            Employee e =
                  (
                      from r in database.Employees
                      where r.EmployeeID == empid
                      select r
                  ).FirstOrDefault();

            e.CV = cnts;
            database.SaveChanges();
        }

        public bool isThereEmail(string value)
        {
            int cnt1 =
                (from r in database.Employees where r.Email == value select r).Count();
            int cnt2 =
                (from r in database.Users where r.Email == value select r).Count();
            int res = cnt1 + cnt2;
            if (res == 0) return false;
            return true;
        }

        public bool isThereUsername(string uname)
        {
            int cnt =
                (from r in database.Employees where r.UserName == uname select r).Count();

            if (cnt == 0) return false;
            return true;
        }

        public DataTable getEmployeeProfileInfo(int id)
        {
            string Command = string.Format("select *,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID = {0}", id);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public bool DeleteEmployee(int EID)
        {
            bool dontChange = false;
            Employee selectedEmployee = database.Employees.Where(p => p.EmployeeID == EID).FirstOrDefault();
            int changes = 0;
            if (selectedEmployee != null)
            {
                try
                {
                    List<EmployeeProject> ep = database.EmployeeProjects.Where(p => p.EmployeeID == EID).ToList();
                    if (ep.Count > 0)
                    {
                        foreach (EmployeeProject e in ep)
                        {
                            database.EmployeeProjects.Remove(e);
                        }
                    }

                }
                catch (Exception m)
                {
                    dontChange = true;
                    string txt = m.Message;
                }

                try
                {
                    List<JobEmployee> je = database.JobEmployees.Where(p => p.EmployeeID == EID).ToList();
                    if (je.Count > 0)
                    {
                        foreach (JobEmployee j in je)
                        {
                            database.JobEmployees.Remove(j);
                        }
                    }
                }

                catch (Exception er)
                {
                    string Message = er.Message;
                    dontChange = true;
                }
                try
                {
                    List<Chat> c = database.Chats.Where(p => p.User_Employee_ID == EID).ToList();
                    if (c.Count > 0)
                    {
                        List<int> ids = new List<int>();

                        foreach (Chat ch in c)
                        {
                            ids.Add(ch.ChatID);
                        }
                        foreach (int id in ids)
                        {
                            List<Message> message = database.Messages.Where(p => p.ChatID == id).ToList();
                            foreach (Message m in message)
                            {
                                database.Messages.Remove(m);
                            }
                        }
                        foreach (Chat chat in c)
                        {
                            database.Chats.Remove(chat);
                        }
                    }

                }
                catch (Exception ek)
                {
                    dontChange = true;
                    string mess = ek.Message;
                }

                if (!dontChange)
                {
                    database.Employees.Remove(selectedEmployee);
                    changes = database.SaveChanges();
                }
            }
            if (changes > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteAll()
        {
            System.Configuration.ConnectionStringSettingsCollection connectionStrings =
                WebConfigurationManager.ConnectionStrings as ConnectionStringSettingsCollection;

            if (connectionStrings.Count > 0)
            {
                System.Data.Linq.DataContext db = new System.Data.Linq.DataContext(connectionStrings["ConnectionString"].ConnectionString);

                db.ExecuteCommand("TRUNCATE TABLE Employees");
            }
        }

        public void setRegSeenToTrue(int empid)
        {
            SqlConnection conn = new SqlConnection(OnlineTools.conString);
            conn.Open();
            string sql2 = string.Format("update Employees set RegSeen = 1 where EmployeeID = {0}", empid);
            SqlCommand myCommand2 = new SqlCommand(sql2, conn);
            myCommand2.ExecuteNonQuery();
            conn.Close();
        }

        public DataTable getEmployeesExceptList(List<int> l, int jobid)
        {
            List<int> result1 = new List<int>();

            IEnumerable<int> pl =
                from r in database.Employees
                select r.EmployeeID;

            result1 = pl.ToList().Except(l).ToList();

            if (result1.Count == 0)
                return null;
            string str = "(";
            str += result1[0].ToString();
            str += ",";

            for (int i = 1; i < result1.Count - 1; i++)
            {
                str += result1[i].ToString();
                str += ",";
            }
            str += result1[result1.Count - 1].ToString();
            str += ")";

            string Command = string.Format("select *,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID IN {0} and  EmployeeID IN( select EmployeeID from JobEmployee where JobID = {1})", str, jobid);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public DataTable getEmployeesExceptList_All(List<int> l)
        {
            List<int> result1 = new List<int>();

            IEnumerable<int> pl =
                from r in database.Employees
                select r.EmployeeID;

            result1 = pl.ToList().Except(l).ToList();

            if (result1.Count == 0)
                return null;
            string str = "(";
            str += result1[0].ToString();
            str += ",";

            for (int i = 1; i < result1.Count - 1; i++)
            {
                str += result1[i].ToString();
                str += ",";
            }
            str += result1[result1.Count - 1].ToString();
            str += ")";

            string Command = string.Format("select *,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID IN {0}", str);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public DataTable getEmployeesExceptList_Search(List<int> l, string txt)
        {
            List<int> result1 = new List<int>();

            IEnumerable<int> pl =
                from r in database.Employees
                select r.EmployeeID;

            result1 = pl.ToList().Except(l).ToList();

            if (result1.Count == 0)
                return null;
            string str = "(";
            str += result1[0].ToString();
            str += ",";

            for (int i = 1; i < result1.Count - 1; i++)
            {
                str += result1[i].ToString();
                str += ",";
            }
            str += result1[result1.Count - 1].ToString();
            str += ")";

            string Command = string.Format("select*,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID IN {0} and(FirstName+' '+LastName like N'%{1}%' or StateName+' - '+CityName like N'%{1}%' or convert(nvarchar(50),EmployeeID) like N'%{1}%' or UserName like N'%{1}%' or Mobile like N'%{1}%' or Email like N'%{1}%')", str, txt);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public DataTable getEmployeesExceptList_JobGroup(List<int> l, int jobGroupid)
        {
            List<int> result1 = new List<int>();

            IEnumerable<int> pl =
                from r in database.Employees
                select r.EmployeeID;

            result1 = pl.ToList().Except(l).ToList();

            if (result1.Count == 0)
                return null;
            string str = "(";
            str += result1[0].ToString();
            str += ",";

            for (int i = 1; i < result1.Count - 1; i++)
            {
                str += result1[i].ToString();
                str += ",";
            }
            str += result1[result1.Count - 1].ToString();
            str += ")";

            string Command = string.Format("select*,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID IN {0} and(EmployeeID IN(select EmployeeID from JobEmployee where JobID in(select JobID from Jobs where JobGroupID = {1}) ))", str, jobGroupid);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public DataTable getEmployeesInfoInList(List<int> loid)
        {
            string str = "(";
            str += loid[0].ToString();
            str += ",";

            for (int i = 1; i < loid.Count - 1; i++)
            {
                str += loid[i].ToString();
                str += ",";
            }
            str += loid[loid.Count - 1].ToString();
            str += ")";

            string Command = string.Format("select *,FirstName+' '+LastName as fullName,StateName+' - '+CityName as addr from Employees left outer join States on Employees.State = States.StateID left outer join Cities on Employees.City = Cities.CityID where EmployeeID IN {0}", str);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }

        public List<int> getEmployeeIDsFromProjectID(int pid)
        {
            List<int?> l = (from r in DB.EmployeeProjects where r.ProjectID == pid select r.EmployeeID).ToList();
            List<int> l2 = new List<int>();
            foreach (int i in l)
            {
                l2.Add(i);
            }
            return l2;
        }
        public DataTable searchEmployeesWithJobName(string jobname)
        {
            string Command = string.Format("select EmployeeID as UserID,RegSeen,UserName,FirstName+' '+LastName as FullName,Mobile,CityName +' - '+StateName as fullAddress,N'کارمند' as urole from Employees left outer join Cities on Employees.City = Cities.CityID left outer join States on Employees.State = States.StateID where EmployeeID in(select EmployeeID from JobEmployee where JobID in(select JobID from Jobs where JobTitle like N'%{0}%'))", jobname);
            SqlConnection myConnection = new SqlConnection(OnlineTools.conString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection);
            DataTable dtResult = new DataTable();
            myDataAdapter.Fill(dtResult);
            return dtResult;
        }
    }
}