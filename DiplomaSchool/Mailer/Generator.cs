using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DiplomaSchool.Mailer
{
    class Generator
    {
        public MySqlConnection conn;

        private void SetConnection()
        {
            DataBase.DataBaseInfo dataBase = new DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void CloseCOnnection()
        {
            conn.Close();
        }

        public string GenerateBody(string login, int order_id, string ComandName)
        {
            string result = "";
            MailerConfig mailer = new MailerConfig();
            string mail = mailer.userName;
            List<string> detail = new List<string>();
            string user = login;
            string status_name = "";
            string service_name = "";
            string service_price = "";
            string order_date = "";
            try
            {
                SetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Services.Service_name, Services.Service_price, Status.Status_name, Orders.Order_date FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Orders.Order_id=" + order_id + ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    service_name = reader.GetString(0);
                    service_price = reader.GetString(1);
                    status_name = reader.GetString(2);
                    order_date = reader.GetString(3);
                }
                reader.Close();

                result = "Dear user " + user + "." + "<br>" +
                                "Your order #" + order_id + " is registered in the system base, the status of the order" + status_name + "." + "<br>" +
                                "Order detail:" + "<br>" +
                                "Service: " + service_name + ";" + "<br>" +
                                "Price: " + service_price + ";" + "<br>" +
                                "Order date: " + order_date + ";" + "<br>"+ "<br>"+
                                "For more information, please visit our office at: Cherkasy, General Momot Street 54, office 5, or call 0638952423;" + "<br>" +
                    "Email: " + mail + ";" + "<br>" +
                    "Phone for help: 937-99-92" + "<br>" + "<br>" +
                    "Sincerely team " + ComandName + " ! :)";

            }
            catch (Exception)
            {

            }
            CloseCOnnection();
            return result;
        }


        public string GenerateSubject(string CompanyName, int orderId)
        {
            return CompanyName + " order number " + orderId;
        }

        public string GenerateCompleteBody(string login, int order_id, string ComandName)
        {
            string result = "";
            MailerConfig mailer = new MailerConfig();
            string mail = mailer.userName;
            List<string> detail = new List<string>();
            string user = login;
            string status_name = "";
            string service_name = "";
            string service_price = "";
            string order_date = "";
            try
            {
                SetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Services.Service_name, Services.Service_price, Status.Status_name, Orders.Order_date FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) WHERE Orders.Order_id=" + order_id + ";")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    service_name = reader.GetString(0);
                    service_price = reader.GetString(1);
                    status_name = reader.GetString(2);
                    order_date = reader.GetString(3);
                }
                reader.Close();

                result = "Dear user " + user + "." + "<br>" +
                                "Your order #" + order_id + " is " + status_name + "." + "<br>" +
                                "Order detail:" + "<br>" +
                                "Service: " + service_name + ";" + "<br>" +
                                "Price: " + service_price + ";" + "<br>" +
                                "Order date: " + order_date + ";" + "<br>" + "<br>" +
                                "For more information, please visit our office at: Cherkasy, General Momot Street 54, office 5, or call 0638952423;" + "<br>" +
                    "Email: " + mail + ";" + "<br>" +
                    "Phone for help: 937-99-92" + "<br>" + "<br>" +
                    "Sincerely team " + ComandName + " ! :)";

            }
            catch (Exception)
            {

            }
            CloseCOnnection();
            return result;
        }

    }
}
