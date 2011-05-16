﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (ModuleModel)Model; %>

    <div id="userdetails" class="module" data-type="UserDetails">
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">
                Your Details
                <a href="#" class="edit ui-icon ui-icon-pencil" style="display: inline; float:right" title="Edit"></a>
                <a href="#" class="save ui-icon ui-icon-disk" style="display: none; float:right" title="Save"></a>
            </h3>
            <table class="data">
                <tr class="keyvaluerow">
                    <td class="name">Name</td>
                    <td class="value"><%=userModel.TennisUser.Name%></td>
                </tr>
                <tr class="keyvaluerow">
                    <td class="name">Location</td>
                    <td class="value"><%=userModel.TennisUser.City.Name%></td>
                </tr>
                <tr class="keyvaluerow">
                    <td class="name">Rating</td>
                    <td>
                        <div class="value"><%=IndexModel.FormatRating(userModel.TennisUser.Rating)%></div>
                        <div class="edit">
                            <select>
                            <% for (double i = 1.5; i <= 5.0; i += 0.25) { %>
                                <option value='<%=i %>' <%=(i==userModel.TennisUser.Rating) ? "selected=selected" : "" %>><%=IndexModel.FormatRating(i)%></option>
                            <% } %>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr class="keyvaluerow">
                    <td class="name">Type</td>
                    <td>
                        <div class="value"><%=userModel.TennisUser.SinglesDoubles%></div>
                        <div class="edit">
                            <select>
                            <% foreach (string pref in new string[] { "Singles", "Doubles", "Either" }) { %>
                                <option value='<%=pref %>' <%=(pref==userModel.TennisUser.SinglesDoubles) ? "selected=selected" : "" %>><%=pref%></option>
                            <% } %>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr class="keyvaluerow">
                    <td class="name">Style</td>
                    <td>
                        <div class="value"><%=userModel.TennisUser.PlayStyle%></div>
                        <div class="edit"><input type="text" value='<%=userModel.TennisUser.PlayStyle%>' /></div>
                    </td>
                </tr>
            </table>
            <div class="form" style="display:none">
            edit me
            </div>
        </div>
    </div>