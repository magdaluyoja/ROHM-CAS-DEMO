using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ROHM_CAS_DEMO.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ROHM_CAS_DEMO.Areas.MasterMaintenance.Models;

namespace ROHM_CAS_DEMO.Areas.MasterMaintenance.Controllers
{
    public class UserMasterController : Controller
    {
        private ReturnObject retObj = new ReturnObject();
        private JsonResult jsonResponse = new JsonResult();


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PopulateData()
        {
            ArrayList DivisionList = new ArrayList();
            ArrayList ModuleList = new ArrayList();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.Text;
                        cmdSql.CommandText = "SELECT * FROM [Division]";
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                DivisionList.Add(new { value = sdr["DivCode"].ToString(), text = sdr["DivName"].ToString() });
                            }

                        }
                        cmdSql.CommandText = "SELECT * FROM [ModuleMaster]";
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                ModuleList.Add(new { value = sdr["ID"].ToString(), text = sdr["ModuleName"].ToString() });
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = new { divList = DivisionList, modList = ModuleList } }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserList()
        {
            List<User> data = new List<User>();
            DatatableHelper TypeHelper = new DatatableHelper();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
            string sortDirection = Request["order[0][dir]"];

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.Text;
                        cmdSql.CommandText = "SELECT u.[ID], u.[UserID], u.[Password], u.[FirstName], u.[Middlename], u.[LastName], u.[WorkstationName], " +
                                                " u.[AccessType], u.[REPIDiv], d.DivName " +
                                                " FROM [ROHM_CAS_DEMO].[dbo].[User] AS u " +
                                                " JOIN [ROHM_CAS_DEMO].[dbo].[Division] AS d " +
                                                " ON u.REPIDiv = d.DivCode " +
                                                " Where u.[IsDeleted] = '0' ";
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                data.Add(new User()
                                {
                                    ID = Convert.ToInt32(sdr["ID"]),
                                    UserID = sdr["UserID"].ToString(),
                                    Password = sdr["Password"].ToString(),
                                    FirstName = sdr["FirstName"].ToString(),
                                    MiddleName = sdr["MiddleName"].ToString(),
                                    LastName = sdr["LastName"].ToString(),
                                    AccessType = Convert.ToInt32(sdr["AccessType"]),
                                    REPIDiv = sdr["DivName"].ToString(),
                                });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            int totalrows = data.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
                data = data.Where(x => x.UserID.ToLower().Contains(searchValue.ToLower()) || x.FirstName.ToLower().Contains(searchValue.ToLower()) || x.MiddleName.ToLower().Contains(searchValue.ToLower()) || x.LastName.ToString().Contains(searchValue.ToLower()) || x.AccessType.ToString().Contains(searchValue.ToLower()) || x.REPIDiv.ToString().Contains(searchValue.ToLower())).ToList<User>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            data = data.Skip(start).Take(length).ToList<User>();


            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveUser(User user)
        {
            User userData = new User();
            var modelErrors = new List<string>();
            ModelState.Remove("ID");
            if (ModelState.IsValid)
            {
                Security ph = new Security();
                string password = ph.EncodePasswordMd5(user.Password).ToString();
                try
                {
                    userData = getUser(user.ID);
                    if (userData.UserID == user.UserID)
                    {
                        return Json(new { success = false, errors = "User ID already exist. Please try again." });
                    }
                    retObj = SaveUserData(user);
                    if (retObj.flag)
                    {
                        retObj = SaveUserModule(user);
                    }
                }
                catch (Exception err)
                {
                    string errmsg;
                    if (err.InnerException != null)
                        errmsg = "An error occured: " + err.InnerException.ToString();
                    else
                        errmsg = "An error occured: " + err.ToString();

                    return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        modelErrors.Add(errMessage);
                    }
                }
            }
            if (modelErrors.Count != 0)
                return Json(new { success = false, errors = modelErrors });
            else
                return Json(new { success = true, msg = "User was successfully saved." });
        }
        public ReturnObject SaveUserData(User data)
        {
            Security ph = new Security();
            string password = ph.EncodePasswordMd5(data.Password).ToString();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "INSERT_UserData";
                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@UserID", data.UserID);
                        cmdSql.Parameters.AddWithValue("@Password", password);
                        cmdSql.Parameters.AddWithValue("@FirstName", data.FirstName);
                        cmdSql.Parameters.AddWithValue("@MiddleName", data.MiddleName != null ? data.MiddleName : "");
                        cmdSql.Parameters.AddWithValue("@LastName", data.LastName);

                        cmdSql.Parameters.AddWithValue("@AccessType", data.AccessType);
                        cmdSql.Parameters.AddWithValue("@REPIDiv", data.REPIDiv);
                        cmdSql.Parameters.AddWithValue("@CreateID", "Test");
                        cmdSql.Parameters.AddWithValue("@UpdateID", "TEST");

                        SqlParameter out_message = cmdSql.Parameters.Add("@Message", SqlDbType.VarChar, 50);
                        SqlParameter out_result = cmdSql.Parameters.Add("@Return", SqlDbType.Bit);

                        out_result.Direction = ParameterDirection.Output;
                        out_message.Direction = ParameterDirection.Output;

                        cmdSql.ExecuteNonQuery();
                        retObj.flag = Convert.ToBoolean(out_result.Value);
                        retObj.message = out_message.Value.ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.ToString();

                retObj.flag = false;
                retObj.message = errmsg;
            }
            return retObj;
        }

        public ReturnObject SaveUserModule(User data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ToString()))
                {
                    conn.Open();

                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        var arrmodules = data.StrModuleID.Split(",".ToCharArray());
                        for (int i = 0; i < arrmodules.Length; i++)
                        {
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "INSERT_Modulemaster";

                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@UserID", data.UserID);
                            cmdSql.Parameters.AddWithValue("@ModuleID", Convert.ToInt32(arrmodules[i]));
                            cmdSql.Parameters.AddWithValue("@CreateID", "Test");
                            cmdSql.Parameters.AddWithValue("@UpdateID", "TEST");

                            SqlParameter out_message = cmdSql.Parameters.Add("@Message", SqlDbType.VarChar, 50);
                            SqlParameter out_result = cmdSql.Parameters.Add("@Return", SqlDbType.Bit);

                            out_result.Direction = ParameterDirection.Output;
                            out_message.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();
                            retObj.flag = Convert.ToBoolean(out_result.Value);
                            retObj.message = out_message.Value.ToString();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                retObj.flag = false;
                retObj.message = e.Message;
            }
            return retObj;
        }
        public ActionResult getUserDetails(int ID)
        {
            User userDetails = new User();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ConnectionString.ToString()))
                {
                    conn.Open();
                    string sql = " SELECT u.[ID], u.[UserID], u.[Password], u.[FirstName], u.[Middlename], u.[LastName], u.[WorkstationName], " +
                                                " u.[AccessType], u.[REPIDiv], d.DivCode " +
                                                " FROM [ROHM_CAS_DEMO].[dbo].[User] AS u " +
                                                " JOIN [ROHM_CAS_DEMO].[dbo].[Division] AS d " +
                                                " ON u.REPIDiv = d.DivCode " +
                                                " Where u.ID = '" + ID + "'";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        if (!reader.Read())
                            throw new InvalidOperationException("No records were returned.");

                        userDetails.ID = Convert.ToInt32(reader["ID"]);
                        userDetails.UserID = reader["UserID"].ToString();
                        userDetails.Password = reader["Password"].ToString();
                        userDetails.FirstName = reader["FirstName"].ToString();
                        userDetails.MiddleName = reader["MiddleName"].ToString();
                        userDetails.LastName = reader["LastName"].ToString();
                        userDetails.AccessType = Convert.ToInt32(reader["AccessType"]);
                        userDetails.REPIDiv = reader["DivCode"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string b;
                if (err.InnerException != null)
                    b = "An error occured: " + err.InnerException.ToString();
                else
                    b = "An error occured: " + err.ToString();
                return Json(new { a = b }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = new { userDetails = userDetails, userModules = "" } }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUser(User user)
        {
            var modelErrors = new List<string>();
            ReturnObject retObj = new ReturnObject();
            User userData = new User();
            if (ModelState.IsValid)
            {
                try
                {
                    userData = getUser(user.ID);
                    if (userData.UserID == user.UserID && userData.ID != user.ID)
                    {
                        return Json(new { success = false, errors = "User ID already exist. Please try again." });
                    }
                    retObj = UpdateUserData(user, userData);
                }
                catch (Exception e)
                {
                    retObj.flag = false;
                    retObj.message = "Error! An Error occured.";
                }
            }
            else
            {
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        modelErrors.Add(errMessage);
                    }
                }
            }
            if (modelErrors.Count != 0)
            {
                return Json(new { success = false, errors = modelErrors });
            }
            else
            {
                return Json(new { success = true, msg = "User was successfully updated." });

            }
        }
        private static User getUser(int ID)
        {
            User user = new User();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ToString()))
            {
                conn.Open();
                string sql = @" SELECT * FROM [dbo].[User] where ID = @ID";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@ID", ID);
                    var reader = comm.ExecuteReader();
                    bool found = reader.Read();
                    if (found)
                    {
                        user.ID = Convert.ToInt32(reader["ID"]);
                        user.UserID = reader["UserID"].ToString();
                        user.Password = reader["Password"].ToString();
                    }
                }
                conn.Close();
            }
            return user;
        }
        private static ReturnObject UpdateUserData(User user, User userDataRow)
        {
            ReturnObject retObj = new ReturnObject();
            try
            {
                string newPassword = "";
                if (user.Password == userDataRow.Password)
                {
                    newPassword = user.Password;
                }
                else
                {
                    Security ph = new Security();
                    newPassword = ph.EncodePasswordMd5(user.Password).ToString();
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "UPDATE_UserData";

                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@ID", user.ID);
                        cmdSql.Parameters.AddWithValue("@UserID", user.UserID);
                        cmdSql.Parameters.AddWithValue("@Password", newPassword);
                        cmdSql.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmdSql.Parameters.AddWithValue("@MiddleName", user.MiddleName != null ? user.MiddleName : "");
                        cmdSql.Parameters.AddWithValue("@LastName", user.LastName);
                        cmdSql.Parameters.AddWithValue("@REPIDiv", user.REPIDiv);
                        cmdSql.Parameters.AddWithValue("@AccessType", user.AccessType);
                        cmdSql.Parameters.AddWithValue("@UpdateID", "TEST");
                        cmdSql.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToString());


                        SqlParameter outResult = cmdSql.Parameters.Add("@Return", SqlDbType.Bit);
                        SqlParameter outMsg = cmdSql.Parameters.Add("@Message", SqlDbType.NVarChar, 50);
                        outResult.Direction = ParameterDirection.Output;
                        outMsg.Direction = ParameterDirection.Output;

                        cmdSql.ExecuteNonQuery();
                        retObj.flag = Convert.ToBoolean(outResult.Value);
                        retObj.message = outMsg.Value.ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                retObj.flag = false;
                retObj.message = "Error! An error occured in updating data.";
            }
            return retObj;
        }
        public ActionResult DeleteUser(int ID)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ROHM_CAS_DEMO"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "DELETE_UserData";

                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@ID", ID);
                        cmdSql.Parameters.AddWithValue("@UpdateID", "TEST");
                        cmdSql.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToShortDateString());

                        SqlParameter outResult = cmdSql.Parameters.Add("@Return", SqlDbType.Bit);
                        SqlParameter outMsg = cmdSql.Parameters.Add("@Message", SqlDbType.NVarChar, 50);

                        outResult.Direction = ParameterDirection.Output;
                        outMsg.Direction = ParameterDirection.Output;

                        cmdSql.ExecuteNonQuery();
                        retObj.flag = Convert.ToBoolean(outResult.Value);
                        retObj.message = outMsg.Value.ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                retObj.flag = false;
                retObj.message = "Error! User Deletion Failed! Try again.";
            }
            return Json(new { success = true, msg = "User was successfully deleted." });

        }
    }
}