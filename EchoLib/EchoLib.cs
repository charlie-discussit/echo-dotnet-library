using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DiscussIt.Echo
{
    public class EchoLib
    {
        public string API_URL_SUBMIT = "http://api.js-kit.com/v1/submit";
	    public string API_URL_SEARCH = "http://api.js-kit.com/v1/search";
        public string API_URL_COUNT = "http://api.echoenabled.com/v1/count";
        public string API_URL_MUX = "http://api.echoenabled.com/v1/mux";
	    public string API_URL_LIST = "http://api.js-kit.com/v1/feeds/list";
	    public string API_URL_REGISTER = "http://api.js-kit.com/v1/feeds/register";
        public string API_URL_UNREGISTER = "http://api.js-kit.com/v1/feeds/unregister";
        public string API_URL_USERGET = "http://api.echoenabled.com/v1/users/get";
        public string API_URL_USERUPDATE = "http://api.echoenabled.com/v1/users/update";
        public string API_URL_USERWHOAMI = "http://api.echoenabled.com/v1/users/whoami";

        private string last_api_call;
	    private string http_status;

        private string consumer_key;

        public string Consumer_Key
        {
          get { return consumer_key; }
          set { consumer_key = value; }
        }

        private string consumer_secret;

        public string Consumer_Secret
        {
          get { return consumer_secret; }
          set { consumer_secret = value; }
        }

        /**
	    * Constructor
	    **/
	    public EchoLib(string consumer_key, string consumer_secret) {
            this.Consumer_Key = consumer_key;
            this.Consumer_Secret = consumer_secret;
	    }

        /**
	    * Debug Helpers
	    */
	    public string lastStatusCode() { return this.http_status; }
	    public string lastApiCall() { return this.last_api_call; }

        /// <summary> 
        /// Submit API Method
        /// http://wiki.js-kit.com/API-Method-submit
        /// </summary> 
        /// <param name="content">URL-encoded Activity Streams XML with one or more activity entries.</param> 
        /// <returns>Returns a result status string in JSON format.</returns> 
        public string method_submit(string content) {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            qparms.Add(new OAuth.QueryParameter("content", content));
            string rsp = eoa.oAuthRequest("http://api.js-kit.com/v1/submit", qparms, "POST");
            return rsp;
        }

        /// <summary> 
        /// Search API Method
        /// http://wiki.js-kit.com/API-Method-search
        /// </summary> 
        /// <param name="query">Specifies a search query.</param>
        /// <param name="since">Show items since this value, obtained from the 'nextSince' value of the previous search.</param> 
        /// <returns>Returns information about items that match the specified query in Activity Stream format.</returns> 
        public string method_search(string query, string since)
        {
            string rsp = this.http(this.API_URL_SEARCH + "?q=" + query + "&appkey=" + this.consumer_key + "&since=" + since, "", "GET");
            return rsp;
        }

        /// <summary> 
        /// Count API Method
        /// http://wiki.aboutecho.com/w/page/27888212/API-method-count
        /// </summary> 
        /// <param name="query">Specifies a search query.</param> 
        /// <returns>Returns a number of items that match the specified query in JSON format.</returns> 
        public string method_count(string query)
        {
            string rsp = this.http(this.API_URL_COUNT + "?q=" + query + "&appkey=" + this.consumer_key, "", "GET");
            return rsp;
        }

        /// <summary> 
        /// Mux Item API Method
        /// http://wiki.aboutecho.com/w/page/32433803/API-method-mux
        /// </summary> 
        /// <param name="requests">URL-encoded JSON structure detailing the API calls to be multiplexed.</param> 
        /// <returns>The response's body is an Activity Stream JSON, or a JSON format error result.</returns> 
        public string method_mux(string requests)
        {
            string rsp = this.http(this.API_URL_MUX + "?requests=" + requests + "&appkey=" + this.consumer_key, "", "GET");
            return rsp;
        }

        /// <summary> 
        /// List API Method
        /// http://wiki.js-kit.com/API-Method-feeds-list
        /// </summary> 
        /// <returns>Returns the list of registered feeds for a specified API key in XML format.</returns> 
        public string method_list()
        {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            string rsp = eoa.oAuthRequest(this.API_URL_LIST, qparms, "GET");
            return rsp;
        }

        /// <summary> 
        /// Registers a new Activity Stream feed by URL.
        /// http://wiki.js-kit.com/API-Method-feeds-register
        /// </summary> 
        /// <param name="url">URL of the page with feed in Activity Streams XML format.</param> 
        /// <param name="interval">Feed refresh rate in seconds (time between polls).</param> 
        /// <returns>Returns a result status string in JSON format.</returns> 
        public string method_register(string url, int interval)
        {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            qparms.Add(new OAuth.QueryParameter("url", url));
            qparms.Add(new OAuth.QueryParameter("interval", interval.ToString()));
            string rsp = eoa.oAuthRequest(this.API_URL_REGISTER, qparms, "POST");
            return rsp;
        }

        /// <summary> 
        /// Unregisters an Activity Stream feed by URL.
        /// http://wiki.js-kit.com/API-Method-feeds-unregister
        /// </summary> 
        /// <param name="url">URL of the page with feed in Activity Streams XML format.</param> 
        /// <returns>Returns a result status string in JSON format.</returns> 
        public string method_unregister(string url)
        {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            qparms.Add(new OAuth.QueryParameter("url", url));
            string rsp = eoa.oAuthRequest(this.API_URL_UNREGISTER, qparms, "POST");
            return rsp;
        }

        /// <summary> 
        /// User Get API Method for fetching user information.
        /// http://wiki.aboutecho.com/w/page/35104884/API-method-users-get
        /// </summary> 
        /// <param name="identityURL">User identity URL.</param> 
        /// <returns>Returns a user object or a result status string in JSON format.</returns> 
        public string method_user_get(string identityURL)
        {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            qparms.Add(new OAuth.QueryParameter("identityURL", identityURL));
            string rsp = eoa.oAuthRequest(this.API_URL_USERGET, qparms, "GET");
            return rsp;
        }

        /// <summary> 
        /// User Update API Method for updating user information.
        /// http://wiki.aboutecho.com/w/page/35060726/API-method-users-update
        /// </summary> 
        /// <param name="identityURL">User identity URL (if subject=poco this parameter should be empty string).</param> 
        /// <param name="subject">Shows which user parameter should be updated.</param> 
        /// <param name="content">Contains data to be used for the user update.</param> 
        /// <returns>Returns a user object or a result status string in JSON format.</returns> 
        public string method_user_update(string identityURL, string subject, string content)
        {
            EchoOAuth eoa = new EchoOAuth(this.consumer_key, this.consumer_secret, null, null);
            List<OAuth.QueryParameter> qparms = new List<OAuth.QueryParameter>();
            qparms.Add(new OAuth.QueryParameter("identityURL", identityURL));
            qparms.Add(new OAuth.QueryParameter("subject", subject));
            qparms.Add(new OAuth.QueryParameter("content", content));
            string rsp = eoa.oAuthRequest(this.API_URL_USERUPDATE, qparms, "POST");
            return rsp;
        }

        /// <summary> 
        /// User whoami API Method for retrieving currently logged in user information by session ID.
        /// http://wiki.aboutecho.com/w/page/35485894/API-method-users-whoami
        /// </summary> 
        /// <param name="sessionID">Backplane channel ID, which is the user session ID at the same time.</param> 
        /// <returns>Returns a user object or a result status string in JSON format</returns> 
        public string method_user_whoami(string sessionID)
        {
            string rsp = this.http(this.API_URL_MUX + "?sessionID=" + sessionID + "&appkey=" + this.consumer_key, "", "GET");
            return rsp;
        }

    	/**
	    * Method to send data
	    */
        private string http(string url, string post_data, string method)
        {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToUpper();
                request.Timeout = 30000;
                if (method == "POST")
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(post_data);
                    // Set the ContentType property of the WebRequest.
                    request.ContentType = "application/x-www-form-urlencoded";
                    // Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                }
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream responseStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(responseStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Clean up the streams.
                reader.Close();
                responseStream.Close();
                response.Close();
                //set debug values
                last_api_call = url + post_data;
                http_status = response.StatusCode.ToString();
                return responseFromServer;
            }
            catch (Exception err)
            {
                return "Error: " + err.Message;
            }
        }

    }

    public class EchoOAuth
    {
        /* Contains the last HTTP status code returned */
        private string http_status;

        /* Contains the last API call */
        private string last_api_call;

        private OAuth.SignatureTypes signaturetype;

        internal OAuth.SignatureTypes SignatureType
        {
          get { return signaturetype; }
          set { signaturetype = value; }
        }

        private OAuth.OAuthConsumer consumer;
        private OAuth.OAuthToken token;

        /* Set up the API root URL */
        public string TO_API_ROOT = "http://api.js-kit.com/v1";
          
        /**
        * Set API URLS
        */
        public string requestTokenURL() { return TO_API_ROOT + "/oauth/request_token"; }
        public string authorizeURL() { return TO_API_ROOT + "/oauth/authorize"; }
        public string accessTokenURL() { return TO_API_ROOT + "/oauth/access_token"; }

        /**
        * Debug helpers
        */
        public string lastStatusCode() { return this.http_status; }
        public string lastAPICall() { return this.last_api_call; }

        /**
        * construct OAuth object
        */
        public EchoOAuth(string consumer_key, string consumer_secret, string oauth_token, string oauth_token_secret) {
            this.SignatureType = OAuth.SignatureTypes.HMACSHA1;
            this.consumer = new OAuth.OAuthConsumer(consumer_key, consumer_secret);
            if (!string.IsNullOrEmpty(oauth_token) && !string.IsNullOrEmpty(oauth_token_secret))
            {
                this.token = new OAuth.OAuthToken(oauth_token, oauth_token_secret);
            }
            else
            {
                this.token = null;
            }
        }

        /**
        * Format and sign an OAuth / API request
        */
        public string oAuthRequest(string url, List<OAuth.QueryParameter> args, string method) {
            OAuth.OAuthRequest oar = new OAuth.OAuthRequest(this.consumer, this.token, method, url, args);
            oar.sign_request("HMAC-SHA1", this.consumer, this.token);
            if (method == "GET")
                return http(oar.to_url(), "", method);
            else
                return http(oar.NormalizedUrl, oar.to_postdata(), method);
        }

        public string http(string url, string post_data, string method)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToUpper();
                request.Timeout = 30000;
                if (method == "POST")
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(post_data);
                    // Set the ContentType property of the WebRequest.
                    request.ContentType = "application/x-www-form-urlencoded";
                    // Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                }
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream responseStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(responseStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Clean up the streams.
                reader.Close();
                responseStream.Close();
                response.Close();
                last_api_call = url + post_data;
                http_status = response.StatusCode.ToString();
                return responseFromServer;
            }
            catch (Exception err)
            {
                return "Error: " + err.Message;
            }
        }
    }
}
