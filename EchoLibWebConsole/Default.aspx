<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="EchoLibWebConsole._Default" validateRequest="false"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        if ($("#MainContent_last_method").attr("value") != "") {
            $("#method-" + $("#MainContent_last_method").attr("value")).slideDown("slow");
            //
            $("#form-field-method").attr("value", $("#MainContent_last_method").attr("value"));
            //
            $("#buttons").show();
        }
        $("#method-container").find("a").bind("click", function () {
            //
            var rel = $(this).attr("rel");
            // hide all
            $(".forms-hidden").hide();
            //
            $("#method-" + rel).slideDown("slow");
            //
            $("#form-field-method").attr("value", rel);
            //
            $("#buttons").show();
            //
            $('#output').remove();
        });
    });
</script> 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<div class="page"> 
<div class="container"> 
<div id="method-container"> 
	<div class="wrapper"> 
        <h3>Methods</h3> 
        <ul> 
            <li><a href="javascript:void(0);" rel="submit">Item - Submit</a></li> 
            <li><a href="javascript:void(0);" rel="search">Item - Search</a></li> 
            <li><a href="javascript:void(0);" rel="count">Item - Count</a></li> 
            <li><a href="javascript:void(0);" rel="mux">Item - MUX</a></li> 
            <li><a href="javascript:void(0);" rel="list">Feeds - List</a></li> 
            <li><a href="javascript:void(0);" rel="register">Feeds - Register</a></li> 
            <li><a href="javascript:void(0);" rel="unregister">Feeds - Unregister</a></li> 
            <li><a href="javascript:void(0);" rel="userget">User - Get</a></li> 
            <li><a href="javascript:void(0);" rel="userupdate">User - Update</a></li> 
            <li><a href="javascript:void(0);" rel="userwhoami">User - WhoAmI</a></li> 
        </ul> 
    </div> 
</div> 
 
<div id="method-output"> 
<div class="wrapper"> 
 

	<input type="hidden" id="form-field-method" name="method" value="" /> 
 
<div style="border-bottom:1px solid #ccc;"> 
	<p>Key: <asp:TextBox ID="txt_consumer_key" runat="server" Width="300px"></asp:TextBox> <!--<input type="text" name="consumer_key" value="" style="width:300px;" />--></p> 
    <p>Secret: <asp:TextBox ID="txt_consumer_secret" runat="server" Width="300px"></asp:TextBox> <!--<input type="text" name="consumer_secret" value="" style="width:300px;" />--></p> 
</div> 
 
<div class="content"> 
    <div id="method-submit" class="forms-hidden" style="display:none;"> 
    <h3>API Method - submit</h3> 
		<p>Submit items in the Activity Streams XML format. <a href="http://wiki.aboutecho.com/w/page/35059196/API-method-submit">Documentation.</a></p> 
        Content: <br/> 
        <asp:TextBox ID="txt_content" runat="server" Rows="10" Width="95%"></asp:TextBox>
        <br /> <small>URL-encoded Activity Streams XML with one or more activity entries</small> 
    </div> 
<!-- --> 
    <div id="method-search" class="forms-hidden" style="display:none;"> 
    <h3>API Method - search</h3> 
		<p>Returns items that match a specified query in Activity Stream format. <a href="http://wiki.aboutecho.com/w/page/23491639/API-method-search">Documentation.</a></p> 
    
        Query: <br/> 
        <asp:TextBox ID="txt_query" runat="server" Width="500px"></asp:TextBox>
        <br/> <small> Specify a search query, ex: "scope:http://aboutecho.com/"</small> 
        <br/> 
        Since: <br/> 
        <asp:TextBox ID="txt_since" runat="server" Width="100px"></asp:TextBox>
        <br /> 
    </div> 
<!-- --> 
<!-- --> 
    <div id="method-count" class="forms-hidden" style="display:none;"> 
    <h3>API Method - Count</h3> 
		<p>Returns a number of items that match the specified query in JSON format. <a href="http://wiki.aboutecho.com/w/page/27888212/API-method-count">Documentation.</a></p> 
    
        Query: <br/>
        <asp:TextBox ID="txt_query2" runat="server" Width="500px"></asp:TextBox> 
        <br/><small> Specify a search query, ex: "scope:http://aboutecho.com/"</small><br/> 
    </div> 
<!-- --> 
<!-- --> 
    <div id="method-mux" class="forms-hidden" style="display:none;"> 
    <h3>API Method - MUX</h3> 
		<p>The API method "mux" allows to "multiplex" requests, i.e. use a single API call to "wrap" several requests. The multiplexed 
        requests are executed concurrently and independently by the Echo server. <a href="http://wiki.aboutecho.com/w/page/32433803/API-method-mux">Documentation.</a></p> 
    
        Requests: <br/> 
        <asp:TextBox ID="txt_requests" rows="10" runat="server" Width="95%"></asp:TextBox> 
        <br/> <small>URL-encoded JSON structure detailing the API calls to be multiplexed.</small><br/> 
    </div> 
<!-- --> 
    <div id="method-list" class="forms-hidden" style="display:none;"> 
    <h3>API Method - feeds/list</h3> 
        <p>Echo Platform allows you to register Activity Stream feeds with the system. We will then agressively poll those feeds looking 
        for new data. This method returns a list of registered feeds for specified API key. <a href="http://wiki.aboutecho.com/w/page/25625870/API-method-feeds-list">Documentation.</a></p> 
		No properties to configure.
    </div> 
    <div id="method-register" class="forms-hidden" style="display:none;"> 
        <h3>API Method - feeds/register</h3> 
        <p>Echo Platform allows you to register Activity Stream feeds with the system. We will then agressively poll those feeds looking 
        for new data. This method registers a new Activity Stream feed by URL. <a href="http://wiki.aboutecho.com/w/page/25664206/API-method-feeds-register">Documentation.</a></p>    
        <p> 
        	Url: <br/> 
            <asp:TextBox ID="txt_url" runat="server" Width="500px"></asp:TextBox> 
        	<br /> <small>URL of page with feed in Activity Streams XML format</small> 
        </p> 
        <p>Interval: <br/> 
        <asp:TextBox ID="txt_interval" runat="server" Width="50px"></asp:TextBox> seconds
        <br /> <small>Feed refresh rate in seconds (time between poll actions)</small> 
        </p> 
    </div> 
    <div id="method-unregister" class="forms-hidden" style="display:none;"> 
    	<h3>API Method - feeds/unregister</h3> 
		<p>Echo Platform allows you to register Activity Stream feeds with the system. We will then agressively poll those feeds looking 
        for new data. This method unregisters an Activity Stream feed by URL. <a href="http://wiki.aboutecho.com/w/page/25664249/API-method-feeds-unregister">Documentation.</a></p> 
        <p> 
        	Url: <br/> 
            <asp:TextBox ID="txt_url2" runat="server" Width="500px"></asp:TextBox> 
        	<br /> <small>URL of page with feed in Activity Streams XML format</small> 
        </p> 
    </div> 
<!-- --> 
    <div id="method-userget" class="forms-hidden" style="display:none;"> 
    <h3>API Method - User Get</h3> 
		<p>Endpoint for fetching user information. <a href="http://wiki.aboutecho.com/w/page/35104884/API-method-users-get">Documentation.</a></p> 
    
        identityURL: <br/>
        <asp:TextBox ID="txt_identityURL" runat="server" Width="500px"></asp:TextBox>  
        <br/><small>User identity URL</small><br/> 
    </div> 
<!-- --> 
<!-- --> 
    <div id="method-userupdate" class="forms-hidden" style="display:none;"> 
    <h3>API Method - User Update</h3> 
		<p>Endpoint for updating user information. <a href="http://wiki.aboutecho.com/w/page/35060726/API-method-users-update">Documentation.</a></p> 
    
        identityURL: <br/> 
        <asp:TextBox ID="txt_identityURL2" runat="server" Width="500px"></asp:TextBox>  
        <br/><small>User identity URL</small><br/> 
        subject: <br/> 
        <asp:TextBox ID="txt_subject" runat="server" Width="500px"></asp:TextBox>  
        <br/><small>Shows which user parameter should be updated.</small><br/> 
        content: <br/> 
        <asp:TextBox ID="txt_content2" runat="server" Width="95%"></asp:TextBox>  
        <br/><small>Contains data to be used for the user update.</small><br/> 
    </div> 
<!-- --> 
<!-- --> 
    <div id="method-userwhoami" class="forms-hidden" style="display:none;"> 
    <h3>API Method - User WhoAmI</h3> 
		<p>Endpoint for retrieving currently logged in user information by session ID, which equals to Backplane channel ID for 
        this user. <a href="http://wiki.aboutecho.com/w/page/35485894/API-method-users-whoami">Documentation.</a></p> 
    
        sessionID: <br/> 
        <asp:TextBox ID="txt_sessionID" runat="server" Width="500px"></asp:TextBox>  
        <br/><small>Backplane channel ID, which is the user session ID at the same time.</small><br/> 
    </div> 
<!-- --> 
</div> 
 
   <br /> 
   <div id="buttons" style=""> 
    <input type="submit" id="form-button-submit" value="Send to Echo" /> 
    <input type="button" name="" value="Clear Output" onclick="$('#output').remove();" /> 
    </div> 

	</div> 
</div> 
 
<div id="right_column"> 
 
</div> 
 
<div id="output"> 
<asp:HiddenField ID="last_method" runat="server" />
<div class="response">
<p>
    <b>API Call:</b> 
    <asp:Label ID="test_API" runat="server"></asp:Label>
</p>
<p>
    <b>Response:</b>&nbsp;<asp:Label ID="test_status" runat="server"></asp:Label>
    <pre>
        <asp:Label ID="test_result" runat="server"></asp:Label>
    </pre>
</p>
</div></div> 
 
 
</div></div> 
</asp:Content>
