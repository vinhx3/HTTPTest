using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Net;
using System.IO;

namespace Notification_HTTP_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //https://docs.microsoft.com/de-de/dotnet/framework/network-programming/how-to-request-data-using-the-webrequest-class
            // Create a request for the URL.
            WebRequest request = WebRequest.Create(textBox1.Text);

            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            //// Display the status.
            //textBox3.Text = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                listBox1.Items.Add(responseFromServer);
                //textBox2.Text = (responseFromServer);
            }

            // Close the response.
            response.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //int zahl = 243242;
            //string json = "{\"status\":\"" + zahl +  "\"}";
            ////string json = "{\"status\":\"0\"}";
            //textBox2.Text = json;

            // https://docs.microsoft.com/de-de/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class
            // https://stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(textBox1.Text);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"status\":\"" + textBox2.Text +  "\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
   

        private void button3_Click(object sender, EventArgs e)
        {
            // https://www.c-sharpcorner.com/article/create-simple-web-api-in-asp-net-mvc/
            ManagementObjectCollection collection;
            using (var finddevice = new ManagementObjectSearcher(@"Select DeviceID from Win32_USBHub"))
                collection = finddevice.Get();
            
            int number = 0;

            foreach (var device in collection)
            {

                if (Convert.ToString(device.GetPropertyValue("DeviceID")) == "USB\\VID_20EF&PID_0410\\J0000033")
                {
                     number = 1;
                }
                else
                {
                     number = 0;
                }
            }
           
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(textBox1.Text);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"status\":\"" + number + "\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }


        }


    }
}
