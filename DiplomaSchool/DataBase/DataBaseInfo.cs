namespace DiplomaSchool.DataBase
{
    class DataBaseInfo
    {
        private string serverName = "localhost";
        private string userName = "root";
        private string userPassword = "masterkey";
        private string charset = "utf8";
        private string databaseName = "DB_School";
        private string port = "3306";

        private string ServerName
        {
            get => serverName;
            set => serverName = value;
        }
        private string UserName
        {
            get => userName;
            set => userName = value;
        }
        private string UserPassword
        {
            get => userPassword;
            set => userPassword = value;
        }
        private string Charset
        {
            get => charset;
            set => charset = value;
        }
        private string DatabaseName
        {
            get => databaseName;
            set => databaseName = value;
        }
        private string Port
        {
            get => port;
            set => port = value;
        }

        public string GetConnectInfo()
        {
            return "server=" + this.serverName + ";user=" + this.userName +
                   ";charset=" + this.charset + ";database=" + this.databaseName +
                   ";port=" + this.port + ";password=" + this.userPassword + ";";
        }
    }
}
