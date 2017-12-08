using System.Net;
using System.IO;

namespace DIplomaSchool.LogIn
{
    class InternetConection
    {
        public string GetInetStaus()
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry("dns.msftncsi.com");
                if (entry.AddressList.Length == 0)
                {
                    return "No access to the Internet";
                }
                else
                {
                    if (!entry.AddressList[0].ToString().Equals("131.107.255.255"))
                    {

                        return "Limited access";
                    }
                }
            }
            catch
            {
                return "No access to the Internet";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.msftncsi.com/ncsi.txt");
            try
            {
                HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                if (responce.StatusCode != HttpStatusCode.OK)
                {
                    return "Limited access";
                }
                using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                {
                    if (sr.ReadToEnd().Equals("Microsoft NCSI"))
                    {
                        return "Internet connection";
                    }
                    else
                    {
                        return "Limited access";
                    }
                }
            }
            catch
            {
                return "No access to the Internet";
            }
        }

        
        public void GetAccess()
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry("dns.msftncsi.com");
                if (entry.AddressList.Length == 0)
                {
                    //return "No access to the Internet";
                }
                else
                {
                    if (!entry.AddressList[0].ToString().Equals("131.107.255.255"))
                    {

                        //return "Limited access";
                    }
                }
            }
            catch
            {
               //return "No access to the Internet";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.msftncsi.com/ncsi.txt");
            try
            {
                HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                if (responce.StatusCode != HttpStatusCode.OK)
                {
                   // return "Limited access";
                }
                using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                {
                    if (sr.ReadToEnd().Equals("Microsoft NCSI"))
                    {
                        //return "Internet connection";
                    }
                    else
                    {
                       // return "Limited access";
                    }
                }
            }
            catch
            {
               // return "No access to the Internet";
            }
        }
    }
}
