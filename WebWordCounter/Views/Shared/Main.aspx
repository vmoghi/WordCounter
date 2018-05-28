<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<%  
    using (Ajax.BeginForm("Main", "Home", null, new AjaxOptions
    {
        UpdateTargetId = "WordCloudPanel",
        HttpMethod = "post"
    }, new { @class = "sortingSerchingForm" }))
    {
%>
    <div class="container">
        <div class="row" >
            <div class="jumbotron text-center">

                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <div class="input-group pull-right" >
                            <input name="urlPath" placeholder="Type an URL" id="urlPath" type="text" class="input-lg" value="" />
                            <i class="form-control-feedback glyphicon glyphicon-search"></i>
                        </div>
                    </div>
                </div>
                <div  class="col-xs-12 col-md-6 opacityMobile">
                    <div class="form-group">
                        <div class='input-group pull-left' >
                            <button type="submit" class="btn btn-info btn-lg"><span class="glyphicon glyphicon-search"></span> Search</button> 
                        </div>    
                    </div>
                </div>
            </div>
        </div>
    </div>
   

<%} %>
    <div class="container">
        <div class="row" >
            <div id="WordCloudPanel" class="col-md-4 col-md-offset-4"></div>
        </div>
    </div>
</asp:Content>


