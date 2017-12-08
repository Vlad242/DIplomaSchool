using MySql.Data.MySqlClient;

namespace DiplomaSchool.DataBase
{
    class DataBaseConnection
    {
        public void SetConection(MySqlConnection conn)
        {
            DataBaseInfo dataBase = new DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        public void CloseConnection(MySqlConnection conn)
        {
            conn.Close();
        }
    }
}
