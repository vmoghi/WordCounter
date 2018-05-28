<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<WordModel>>" %>
<%@ Import Namespace="WebWordCounter.Models" %>


<div class="containerTags panel-body">
<% 

    foreach(var word in Model){ %>

    <% var name = word.Word;
       var count = word.Counter ;
      
       var tag = word.Tag;

        %>
    <label class="<%=tag %>">
        
        <%=name %>
    </label>

<%} %>
</div>



