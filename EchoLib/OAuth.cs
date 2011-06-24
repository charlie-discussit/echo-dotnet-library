using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DiscussIt.Echo
{
    public class OAuth
    {
        /// <summary> 
        /// Provides a predefined set of algorithms that are supported officially by the protocol 
        /// </summary> 
        public enum SignatureTypes
        {
            HMACSHA1,
            PLAINTEXT,
            RSASHA1
        }

        /// <summary> 
        /// Provides an internal structure to sort the query parameter 
        /// </summary> 
        public class QueryParameter
        {
            private string name = null;
            private string value = null;
            public QueryParameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
            public string Name
            {
                get { return name; }
            }
            public string Value
            {
                get { return value; }
            }
        }

        /// <summary> 
        /// Comparer class used to perform the sorting of the query parameters 
        /// </summary> 
        protected class QueryParameterComparer : IComparer<QueryParameter>
        {
            #region IComparer<QueryParameter> Members
            public int Compare(QueryParameter x, QueryParameter y)
            {
                if (x.Name == y.Name)
                {
                    return string.Compare(x.Value, y.Value);
                }
                else
                {
                    return string.Compare(x.Name, y.Name);
                }
            }
            #endregion
        }
        protected const string OAuthVersion = "1.0";
        protected const string OAuthParameterPrefix = "oauth_";
        // 
        // List of know and used oauth parameters' names 
        // 
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";
        protected const string HMACSHA1SignatureType = "HMAC-SHA1";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected const string RSASHA1SignatureType = "RSA-SHA1";
       

        public class OAuthConsumer
        {
            public string key;
            public string secret;

            public OAuthConsumer(string key, string secret)
            {
                this.key = key;
                this.secret = secret;
            }

            public string ToString()
            {
                return "OAuthConsumer[key=" + this.key + ",secret=" + this.secret + "]";
            }
        }

        public class OAuthToken
        {
            public string key;
            public string secret;

            public OAuthToken(string key, string secret)
            {
                this.key = key;
                this.secret = secret;
            }

            public string ToString()
            {
                return "OAuthToken[key=" + this.key + ",secret=" + this.secret + "]";
            }
        }

        public class OAuthRequest
        {
            private List<QueryParameter> parameters;
            private string http_method;
            private string http_url;
            // for debug
            public string base_string;
            public string version = "1.0";
            private string normalizedUrl;

            public string NormalizedUrl
            {
                get { return normalizedUrl; }
                set { normalizedUrl = value; }
            }

            protected Random random = new Random();
            private string signature;
            protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            public OAuthRequest(string http_method, string http_url, List<QueryParameter> parameters)
            {
                this.parameters = parameters;
                this.http_method = http_method;
                this.http_url = http_url;
            }

            /**
            * pretty much a helper function to set up the request
            */
            public OAuthRequest(OAuthConsumer consumer, OAuthToken token, string http_method, string http_url, List<QueryParameter> parameters)
            {
                this.http_method = http_method;
                this.http_url = http_url;
                parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
                parameters.Add(new QueryParameter(OAuthNonceKey, GenerateNonce()));
                parameters.Add(new QueryParameter(OAuthTimestampKey, GenerateTimeStamp()));
                parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumer.key));
                this.parameters = parameters;
                //Echo is not using Token so leave it out
            }

            public void sign_request(string signature_method, OAuthConsumer consumer, OAuthToken token)
            {
                parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signature_method));
                signature = build_signature(signature_method, consumer, token);
                parameters.Add(new QueryParameter(OAuthSignatureKey, signature));
            }

            public string build_signature(string signature_method, OAuthConsumer consumer, OAuthToken token)
            {
                //hard coding signature_method to use HMAC_SHA1 for echo
                //since the Echo doesn't use tokens the hmac key is just the comsumer.secret with an & on the end.
                string base_string = get_signature_base_string();
                this.base_string = base_string;

                HMACSHA1 hmacsha1 = new HMACSHA1();
                hmacsha1.Key = Encoding.ASCII.GetBytes(UrlEncode(consumer.secret) + "&");
                byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(base_string);
                byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

                string tmp = Convert.ToBase64String(hashBytes);
                return Convert.ToBase64String(hashBytes);
            }

            /**
            * Returns the base string of this request
            *
            * The base string defined as the method, the url
            * and the parameters (normalized), each urlencoded
            * and the concated with &.
            */
            public string get_signature_base_string()
            {
                parameters.Sort(new QueryParameterComparer());
                Uri url = new Uri(http_url);
                normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
                if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
                {
                    normalizedUrl += ":" + url.Port;
                }
                normalizedUrl += url.AbsolutePath;
                string normalizedRequestParameters = NormalizeRequestParameters(parameters);
                StringBuilder signatureBase = new StringBuilder();
                signatureBase.AppendFormat("{0}&", http_method.ToUpper());
                signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
                signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));
                return signatureBase.ToString();
            }

            /**
            * builds a url usable for a GET request
            */
            public string to_url()
            {
                string rsp = this.NormalizedUrl + "?";
                rsp += this.to_postdata();
                return rsp;
            }

            public string to_postdata()
            {
                //this.parameters.Sort(new QueryParameterComparer());
                return NormalizeRequestParameters(this.parameters);
            }

            /// <summary> 
            /// This is a different Url Encode implementation since the
            /// default .NET one outputs the percent encoding in lower case. 
            /// While this is not a problem with the percent encoding 
            /// spec, it is used in upper case throughout OAuth 
            /// </summary> 
            /// <param name="value">The value to Url encode</param> 
            /// <returns>Returns a Url encoded string</returns> 
            public string UrlEncode(string value)
            {
                StringBuilder result = new StringBuilder();
                foreach (char symbol in value)
                {
                    if (unreservedChars.IndexOf(symbol) != -1)
                    {
                        result.Append(symbol);
                    }
                    else
                    {
                        result.Append('%' + String.Format("{0:X2}", (int)symbol));
                    }
                }
                return result.ToString();
            }

            /// <summary> 
            /// Normalizes the request parameters according to the spec 
            /// </summary> 
            /// <param name="parameters">The list of parameters already sorted</param> 
            /// <returns>a string representing the normalized parameters</returns> 
            protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
            {
                StringBuilder sb = new StringBuilder();
                QueryParameter p = null;
                for (int i = 0; i < parameters.Count; i++)
                {
                    p = parameters[i];
                    sb.AppendFormat("{0}={1}", UrlEncode(p.Name), UrlEncode(p.Value));
                    if (i < parameters.Count - 1)
                    {
                        sb.Append("&");
                    }
                }
                return sb.ToString();
            }

            /// <summary> 
            /// Generate the timestamp for the signature 
            /// </summary> 
            /// <returns></returns> 
            public virtual string GenerateTimeStamp()
            {
                // Default implementation of UNIX time of the current UTC time 
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string timeStamp = ts.TotalSeconds.ToString();
                timeStamp = timeStamp.Substring(0, timeStamp.IndexOf("."));
                return timeStamp;
            }

            /// <summary> 
            /// Generate a nonce 
            /// </summary> 
            /// <returns></returns> 
            public virtual string GenerateNonce()
            {
                // Just a simple implementation of a random number between 123400 and 9999999 
                return random.Next(123400, 9999999).ToString();
            }
        }
    }
}
