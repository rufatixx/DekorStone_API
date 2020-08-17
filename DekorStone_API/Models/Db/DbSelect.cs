using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DekorStone_API.Models.Structs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace DekorStone_API.Models.Db
{
    public class DbSelect
    {
        private readonly string ConnectionString;
        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DbSelect(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value;
            _hostingEnvironment = hostingEnvironment;

        }
        public long regexPhone(long phone)
        {
            long formattedPhone = 0;


            if (Regex.Match(phone.ToString(), @"[0-9]{12}").Success)
            {
                formattedPhone = Convert.ToInt64(phone.ToString().Substring(3));
            }

            return formattedPhone;
        }
        public LogInStruct LogIn(long phone, string pass)
        {

            long formattedPhone = regexPhone(phone);
            LogInStruct status = new LogInStruct();
            if (formattedPhone > 0 && !string.IsNullOrEmpty(pass))
            {
                Security security = new Security(Configuration, _hostingEnvironment);
                long userID = 0;
                int isActive = 0;

                try
                {

                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();

                        using (MySqlCommand com = new MySqlCommand("select * from users where pass=SHA2(@pass,256) and phone = @phone order by id desc limit 1", connection))
                        {

                            com.Parameters.AddWithValue("@pass", pass);
                            com.Parameters.AddWithValue("@phone", phone);
                            using (MySqlDataReader reader = com.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        userID = Convert.ToInt32(reader["id"]);
                                        isActive = Convert.ToInt32(reader["isActive"]);
                                        status.name = reader["name"].ToString();
                                        status.surname = reader["surname"].ToString();
                                    }


                                }
                                else
                                {
                                    status.status = 2;
                                    // status.requestToken = security.requestTokenGenerator(userToken, userID);

                                  //  status.responseString = "Access danied";
                                }
                            }

                        }
                        if (userID > 0)
                        {
                            
                                status.userToken = security.userTokenGenerator(userID);
                                status.requestToken = security.requestTokenGenerator(status.userToken, userID);




                            
                            switch (isActive)
                            {
                                case 1:
                                    status.status = 1;
                                    
                                    //status.responseString = "welcome";
                                    break;
                                case 0:
                                    status.status = 3;
                                   // status.responseString = "continue registration";
                                    break;

                            }




                        }


                        connection.Close();

                    }

                   
                }
                catch (Exception ex)
                {
                    status.status = 4;

                    Console.WriteLine($"Exception: {ex.Message}");
                   // status.responseString = $"Exception: {ex.Message}";
                }
            }
            else
            {
                status.status = 5;
              //  status.responseString = "wrong param format";
            }
            return status;




        }

        public DailySumStruct GetYesterdaySum()
        {
            DailySumStruct sum = new DailySumStruct();
            DateTime yesterday = DateTime.Today;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                connection.Open();
                using (MySqlCommand com = new MySqlCommand(@"select * from daily_sum WHERE DATE(`cdate`) = @yesterday", connection))

                {
                    com.Parameters.AddWithValue("@yesterday", yesterday.AddDays(-1));
                    using (MySqlDataReader reader = com.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                sum.dayEndMoney = Convert.ToDouble(reader["dayEndMoney"]);
                                sum.dayStartMoney = Convert.ToDouble(reader["dayStartMoney"]);
                                sum.income = Convert.ToDouble(reader["income"]);
                                sum.outcome = Convert.ToDouble(reader["outcome"]);
                            }

                        }

                    }

                }
                connection.Close();
            }
            return sum;

        }
        public long GetTodaySumID() {
            long todaySumID = 0;
            DateTime today = DateTime.Today;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                connection.Open();
                using (MySqlCommand com = new MySqlCommand(@"select * from daily_sum WHERE DATE(`cdate`) = @today", connection))

                {
                    com.Parameters.AddWithValue("@today",today);
                    using (MySqlDataReader reader = com.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todaySumID = Convert.ToInt64(reader["id"]);
                            }

                        }

                    }

                }
                connection.Close();
            }
            return todaySumID;

        }

        public ResponseStruct<ProductModel> GetModels(string userToken,string requestToken)

        {
            ResponseStruct<ProductModel> response = new ResponseStruct<ProductModel>();
            response.data = new List<ProductModel>();
            try
            {
                Security security = new Security(Configuration, _hostingEnvironment);
                int userID1 = security.selectUserToken(userToken);
                int userID2 = security.selectRequestToken(requestToken);

                if (userID1 == userID2&&userID1>0&&userID2>0)
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString)) {


                    connection.Open();

                    using (MySqlCommand com = new MySqlCommand("Select * from product_models order by id desc", connection)) {

                        MySqlDataReader reader = com.ExecuteReader();
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {

                                ProductModel pModel = new ProductModel();
                                pModel.id = Convert.ToInt64(reader["id"]);
                                pModel.name = reader["model_name"].ToString();
                                pModel.price = Convert.ToDouble(reader["price"]);



                                response.data.Add(pModel);


                            }
                            

                            response.status = 1;
                        }
                        else
                        {
                            response.status = 2;

                            

                        }
                    }


                    connection.Close();
                        response.requestToken = security.requestTokenGenerator(userToken, userID1);

                    }

                    }
                else
                {
                    response.status = 3;
                }

            }
            catch (Exception ex)
            {
                response.status = 4;
            }

           
            return response;
        }
        public ResponseStruct<DailySumStruct> GetDailySum(string userToken, string requestToken)

        {
            ResponseStruct<DailySumStruct> response = new ResponseStruct<DailySumStruct>();
            response.data = new List<DailySumStruct>();
            try
            {
                Security security = new Security(Configuration, _hostingEnvironment);
                int userID1 = security.selectUserToken(userToken);
                int userID2 = security.selectRequestToken(requestToken);

                if (userID1 == userID2 && userID1 > 0 && userID2 > 0)
                {
                    
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {


                        connection.Open();

                        using (MySqlCommand com = new MySqlCommand("Select * from daily_sum order by cdate desc", connection))
                        {

                            MySqlDataReader reader = com.ExecuteReader();
                            if (reader.HasRows)
                            {


                                while (reader.Read())
                                {
                                  
                                    DailySumStruct dSumStruct = new DailySumStruct();
                                    dSumStruct.ID = Convert.ToInt64(reader["id"]);
                                    dSumStruct.cDate = Convert.ToDateTime(reader["cdate"]);
                                    dSumStruct.dayStartMoney = Convert.ToDouble(reader["dayStartMoney"]);
                                    dSumStruct.dayEndMoney = Convert.ToDouble(reader["dayEndMoney"]);
                                    dSumStruct.income = Convert.ToDouble(reader["income"]);
                                    dSumStruct.outcome = Convert.ToDouble(reader["outcome"]);



                                    response.data.Add(dSumStruct);


                                }


                                response.status = 1;
                            }
                            else
                            {
                                response.status = 2;



                            }
                        }


                        connection.Close();
                        response.requestToken = security.requestTokenGenerator(userToken, userID1);

                    }

                }
                else
                {
                    response.status = 3;
                }

            }
            catch (Exception ex)
            {
                response.status = 4;
            }


            return response;
        }

        public long getUserID(long phone)
        {
            long userID = 0;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {


                try
                {




                    connection.Open();

                    using (MySqlCommand com = new MySqlCommand("select userID from user where mobile=@phone order by userID desc limit 1", connection))
                    {
                        com.Parameters.AddWithValue("@phone", phone);

                        MySqlDataReader reader = com.ExecuteReader();
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {
                                userID = Convert.ToInt32(reader[0]);
                            }
                        }


                        connection.Close();


                    }



                }
                catch
                {
                    connection.Close();


                }

            }
            return userID;
        }
        //public UserStruct getProfile(string userToken, string requestToken)
        //{
        //    Security security = new Security(Configuration, _hostingEnvironment);
        //    int userID1 = security.selectUserToken(userToken);
        //    int userID2 = security.selectRequestToken(requestToken);
        //    UserStruct user = new UserStruct();
        //    if (userID1 == userID2)
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        //        {


        //            try
        //            {




        //                connection.Open();

        //                using (MySqlCommand com = new MySqlCommand("select * from user where userID=@userID order by userID desc limit 1", connection))
        //                {
        //                    com.Parameters.AddWithValue("@userID", userID1);

        //                    MySqlDataReader reader = com.ExecuteReader();
        //                    if (reader.HasRows)
        //                    {


        //                        while (reader.Read())
        //                        {
        //                            user.name = reader["name"] == DBNull.Value ? "" : reader["name"].ToString();
        //                            user.surname = reader["surname"] == DBNull.Value ? "" : reader["surname"].ToString();
        //                            user.email = reader["email"] == DBNull.Value ? "" : reader["email"].ToString();
        //                            user.phone = reader["mobile"] == DBNull.Value ? "" : reader["mobile"].ToString();
        //                        }
        //                        user.response = 0;
        //                        user.responseString = "OK";
        //                        user.requestToken = security.requestTokenGenerator(userToken, userID1);
        //                    }
        //                    else
        //                    {
        //                        user.response = 2;
        //                        user.responseString = "user not found";
        //                    }


        //                    connection.Close();


        //                }



        //            }
        //            catch (Exception ex)
        //            {
        //                connection.Close();
        //                user.response = 1;
        //                user.responseString = $"Exception:{ex.Message}";


        //            }

        //        }
        //    }
        //    else
        //    {
        //        user.response = 3;
        //        user.responseString = $"Access danied!";
        //    }



        //    return user;
        //}
//        public ResponseStruct<ServiceStruct> getServices(string userToken, string requestToken, string administrative_area_level_2)

//        {
//            Security security = new Security(Configuration, _hostingEnvironment);
//            int userID1 = security.selectUserToken(userToken);
//            int userID2 = security.selectRequestToken(requestToken);
//            ResponseStruct<ServiceStruct> serviceResponse = new ResponseStruct<ServiceStruct>();
//            serviceResponse.data = new List<ServiceStruct>();
//            UserStruct user = new UserStruct();
//            try
//            {
//                if (userID1 == userID2)
//                {

//                    serviceResponse.status = 1;//authorized

//                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
//                    {

//                        connection.Open();

//                        using (MySqlCommand com = new MySqlCommand(@"select *,
//(select name from service where serviceId=a.serviceid)as serviceName,
//(select serviceImgUrl from service where serviceId=a.serviceid)as serviceImgUrl from relservicecity a
//where cityId = (select CityId from city where administrative_area_level_2 = @administrative_area_level_2)
//", connection))
//                        {
//                            com.Parameters.AddWithValue("@administrative_area_level_2", administrative_area_level_2);



//                            MySqlDataReader reader = com.ExecuteReader();
//                            if (reader.HasRows)
//                            {
//                                serviceResponse.status = 1;//authorized and has rows

//                                while (reader.Read())
//                                {

//                                    ServiceStruct service = new ServiceStruct();
//                                    service.id = Convert.ToInt32(reader["serviceID"]);
//                                    service.name = reader["serviceName"].ToString();
//                                    service.serviceImgUrl = reader["serviceImgUrl"].ToString();
//                                    //ads.countryID = Convert.ToInt32(reader["countryId"]);



//                                    serviceResponse.data.Add(service);


//                                }
//                                connection.Close();


//                            }
//                            else
//                            {

//                                connection.Close();
//                                serviceResponse.status = 2;//authorized and no rows

//                            }
//                            serviceResponse.requestToken = security.requestTokenGenerator(userToken, userID1);

//                        }

//                    }




//                }
//                else
//                {
//                    serviceResponse.status = 3;//access danied
//                }
//            }
//            catch (Exception ex)
//            {
//                serviceResponse.status = 4; //error
//            }

//            return serviceResponse;


//        }
    }
}
