using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace qiwi
{
    public static class lib
    {
        public static string getPost(string uriString, string postData)
        {
            HttpWebRequest httpRequest = null;
            Uri uri = new Uri(uriString);
            // create the initial request
            httpRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            httpRequest.UserAgent = "delfin";
            //httpRequest.KeepAlive = false;

            httpRequest.Method = "POST";

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData);

            // Set the content type of the data being posted.
            //httpRequest.ContentType = ContentType; //"text/xml; encoding='utf-8'";
            //"application/x-www-form-urlencoded";

            httpRequest.ContentType = "application/json";
            // Set the content length of the string being posted.
            httpRequest.ContentLength = postData.Length;

            // Get the request stream and write the post data in
            System.IO.Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            // Done updating for POST so close the stream
            requestStream.Close();

            var response = (HttpWebResponse)httpRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}