<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var potentialOffers = (PotentialOffersModel)Model; %>
    <% ViewData["TennisUser"] = potentialOffers.TennisUser; %>
    <% var perPage = 5; %>
    <% var pageModel = PageModel<OfferModel>.Create((int)ViewData["page"], perPage, potentialOffers.PotentialOffers); %>

    <div class="module" data-type="PotentialOffers" style='display:<%=pageModel.Items.Count() > 0 ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">
                <div>Match Requests</div>
                <a href="#" class="more" style="display: inline; float:right; white-space:nowrap" title="View More"><span style="float:left">All</span> <span class="ui-icon ui-icon-grid" style="display: inline; float:right;"></span></a>
            </h3>
            <div class="data">
                <table width="100%" cellpadding="0" cellspacing="0">
                <% foreach (OfferModel o in pageModel.Items) { %>
                <% Html.RenderPartial("Offer", o); %>
                <% } %>
                </table>
                <!--
                <% Html.RenderPartial("Paginator", pageModel); %>
                -->
            </div>
        </div>
    </div>