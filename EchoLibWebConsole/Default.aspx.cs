using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiscussIt.Echo;

namespace EchoLibWebConsole
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rsp = "";
            string last_api_call = "";
            string last_status_code = "";
            string method = "";

            if (IsPostBack)
            {
                string consumer_key = txt_consumer_key.Text; 
                string consumer_secret = txt_consumer_secret.Text;
                method = Request.Form["method"];


                EchoLib el = new EchoLib(consumer_key, consumer_secret);
                
                switch (method)
                {
                    case "submit":
                        rsp = el.method_submit(txt_content.Text);
                        break;
                    case "search":
                        rsp = el.method_search(txt_query.Text, txt_since.Text);
                        break;
                    case "count":
                        rsp = el.method_count(txt_query2.Text);
                        break;
                    case "mux":
                        rsp = el.method_mux(txt_requests.Text);
                        break;
                    case "list":
                        rsp = el.method_list();
                        break;
                    case "register":
                        rsp = el.method_register(txt_url.Text, Convert.ToInt32(txt_since.Text));
                        break;
                    case "unregister":
                        rsp = el.method_unregister(txt_url2.Text);
                        break;
                    case "userget":
                        rsp = el.method_user_get(txt_identityURL.Text);
                        break;
                    case "userupdate":
                        rsp = el.method_user_update(txt_identityURL2.Text, txt_subject.Text, txt_content2.Text);
                        break;
                    case "userwhoami":
                        rsp = el.method_user_whoami(txt_sessionID.Text);
                        break;
                }
                last_api_call = el.lastApiCall();
                last_status_code = "Status = " + el.lastStatusCode();
            }

            test_API.Text = last_api_call;
            test_status.Text = last_status_code;
            test_result.Text = rsp;
            last_method.Value = method;
        }

    }
}
