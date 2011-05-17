
Type.registerNamespace('SportsLinkScript.Controls');

////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.Calendar

SportsLinkScript.Controls.Calendar = function SportsLinkScript_Controls_Calendar(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.Calendar.initializeBase(this, [ element ]);
    this.serviceName = 'Calendar';
}
SportsLinkScript.Controls.Calendar.prototype = {
    
    _acceptMatch$3: function SportsLinkScript_Controls_Calendar$_acceptMatch$3(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var offerId = button.siblings('input').val();
        var parentRow = button.parents('.offer');
        parentRow.attr('disabled', 'disabled').addClass('ui-state-disabled');
        $.post('/services/AcceptOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify({ id: offerId }), function(data, textStatus, request) {
            parentRow.attr('disabled', '').removeClass('ui-state-disabled');
            button.parent().children('a').fadeOut('slow');
        });
    },
    
    _rejectMatch$3: function SportsLinkScript_Controls_Calendar$_rejectMatch$3(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        alert('reject');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.PlayerGrid

SportsLinkScript.Controls.PlayerGrid = function SportsLinkScript_Controls_PlayerGrid(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.PlayerGrid.initializeBase(this, [ element, 'PlayerGrid' ]);
    var requestMatch = this.obj.find('.requestMatch');
    requestMatch.button({ text: true, icons: { secondary: 'ui-icon-carat-1-e' } });
    requestMatch.click(SportsLinkScript.Controls.Players.requestMatch);
    var selects = this.obj.find('th select');
    selects.each(ss.Delegate.create(this, function(index, el) {
        ($(el)).multiselect({ header: false, minWidth: '80', height: 'auto', noneSelectedText: el.title, selectedText: el.title, close: ss.Delegate.create(this, function() {
            this.doFilter(this.obj, true);
        }) });
    }));
    this.doFilter(this.obj, false);
}
SportsLinkScript.Controls.PlayerGrid.prototype = {
    
    doFilter: function SportsLinkScript_Controls_PlayerGrid$doFilter(obj, postBack) {
        /// <param name="obj" type="jQueryObject">
        /// </param>
        /// <param name="postBack" type="Boolean">
        /// </param>
        var selects = obj.find('th select');
        var filterValue = '';
        selects.each(function(index, el) {
            var select = $(el);
            var checkedItems = select.multiselect('getChecked');
            if (checkedItems.length > 0) {
                if (filterValue.length > 0) {
                    filterValue = filterValue + ',,';
                }
                filterValue = filterValue + select.attr('name') + '=';
                for (var i = 0; i < checkedItems.length; ++i) {
                    if (i > 0) {
                        filterValue = filterValue + '||';
                    }
                    filterValue = filterValue + (checkedItems[i]).value;
                }
            }
        });
        if (this.filter !== filterValue) {
            this.filter = filterValue;
            if (postBack) {
                this.postBack(0);
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.UserDetails

SportsLinkScript.Controls.UserDetails = function SportsLinkScript_Controls_UserDetails(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <field name="_editButton$1" type="jQueryUIObject">
    /// </field>
    /// <field name="_saveButton$1" type="jQueryUIObject">
    /// </field>
    SportsLinkScript.Controls.UserDetails.initializeBase(this, [ element ]);
    this._editButton$1 = this.obj.find('a.edit');
    this._editButton$1.click(ss.Delegate.create(this, this._editDetails$1));
    this._saveButton$1 = this.obj.find('a.save');
    this._saveButton$1.click(ss.Delegate.create(this, this._saveDetails$1));
}
SportsLinkScript.Controls.UserDetails.prototype = {
    _editButton$1: null,
    _saveButton$1: null,
    
    _editDetails$1: function SportsLinkScript_Controls_UserDetails$_editDetails$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var edits = this.obj.find('.keyvaluerow .edit');
        edits.show('fast');
        edits.prev('.value').hide('fast');
        this._editButton$1.hide('fast');
        this._saveButton$1.show('fast');
    },
    
    _saveDetails$1: function SportsLinkScript_Controls_UserDetails$_saveDetails$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var edits = this.obj.find('.keyvaluerow .edit');
        edits.hide('fast');
        edits.prev('.value').show('fast');
        this._editButton$1.show('fast');
        this._saveButton$1.hide('fast');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.PaginatedModule

SportsLinkScript.Controls.PaginatedModule = function SportsLinkScript_Controls_PaginatedModule(element, serviceName) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <param name="serviceName" type="String">
    /// </param>
    /// <field name="page" type="Number" integer="true">
    /// </field>
    /// <field name="serviceName" type="String">
    /// </field>
    /// <field name="filter" type="String">
    /// </field>
    this.filter = String.Empty;
    SportsLinkScript.Controls.PaginatedModule.initializeBase(this, [ element ]);
    this.page = parseInt(this.obj.find('.page').val());
    this.serviceName = serviceName;
    var prev = this.obj.find('.prev');
    var next = this.obj.find('.next');
    prev.button();
    next.button();
    prev.click(ss.Delegate.create(this, this._pagePrev$1));
    next.click(ss.Delegate.create(this, this._pageNext$1));
}
SportsLinkScript.Controls.PaginatedModule.prototype = {
    page: 0,
    serviceName: null,
    
    _pagePrev$1: function SportsLinkScript_Controls_PaginatedModule$_pagePrev$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.postBack(this.page - 1);
    },
    
    _pageNext$1: function SportsLinkScript_Controls_PaginatedModule$_pageNext$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.postBack(this.page + 1);
    },
    
    postBack: function SportsLinkScript_Controls_PaginatedModule$postBack(page) {
        /// <param name="page" type="Number" integer="true">
        /// </param>
        this.obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var parameters = { page: page, filter: this.filter };
        $.post('/services/' + this.serviceName + '?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.PlayerDetails

SportsLinkScript.Controls.PlayerDetails = function SportsLinkScript_Controls_PlayerDetails(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.PlayerDetails.initializeBase(this, [ element ]);
    var sendMessage = this.obj.find('#playerMessage .sendMessage');
    sendMessage.button({ text: true, icons: { secondary: 'ui-icon-carat-1-e' } });
    sendMessage.click(ss.Delegate.create(this, this._sendMessage$1));
}
SportsLinkScript.Controls.PlayerDetails.prototype = {
    
    _sendMessage$1: function SportsLinkScript_Controls_PlayerDetails$_sendMessage$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var dialog = $('#playerDetailsCard');
        var text = $('#playerDetailsCard .comments').val();
        var id = dialog.attr('data-id');
        debugger;
        dialog.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var parameters = { userId: id, comments: text };
        $.post('/services/SendMessage?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            dialog.attr('disabled', '').removeClass('ui-state-disabled');
            dialog.dialog('close');
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.UserOffers

SportsLinkScript.Controls.UserOffers = function SportsLinkScript_Controls_UserOffers(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.UserOffers.initializeBase(this, [ element, 'UserOffers' ]);
    var cancelMatch = this.obj.find('.cancelMatch');
    cancelMatch.button({ text: false, icons: { primary: 'ui-icon-closethick' } });
    cancelMatch.click(ss.Delegate.create(this, this._cancelOffer$2));
}
SportsLinkScript.Controls.UserOffers.prototype = {
    
    _cancelOffer$2: function SportsLinkScript_Controls_UserOffers$_cancelOffer$2(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var button = $(e.currentTarget);
        var parameters = { offerId: button.attr('data-offerId') };
        $.post('/services/CancelOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.Players

SportsLinkScript.Controls.Players = function SportsLinkScript_Controls_Players(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.Players.initializeBase(this, [ element ]);
    var moreButton = this.obj.find('.more');
    var requestMatch = this.obj.find('.requestMatch');
    requestMatch.button({ text: true, icons: { secondary: 'ui-icon-carat-1-e' } });
    requestMatch.click(SportsLinkScript.Controls.Players.requestMatch);
    moreButton.click(ss.Delegate.create(this, this._moreClick$1));
}
SportsLinkScript.Controls.Players.requestMatch = function SportsLinkScript_Controls_Players$requestMatch(e) {
    /// <param name="e" type="jQueryEvent">
    /// </param>
    var button = $(e.currentTarget);
    var dialog = $('#challengeDialog');
    var datePicker = dialog.find('.datepicker');
    SportsLinkScript.Shared.Utility._wireAutoComplete(dialog.find('.placesAutoFill'), dialog.find('.placesAutoValue'));
    var id = button.get(0).id;
    datePicker.datepicker('disable');
    dialog.dialog({ width: '260', height: '324', modal: true, title: button.attr('Title'), buttons: { 'Challenge!': function(ex) {
        SportsLinkScript.Controls.Players._createMatch$1(id);
    } }, open: function() {
        dialog.find('.comments').focus();
        datePicker.datepicker('enable');
    } });
}
SportsLinkScript.Controls.Players._createMatch$1 = function SportsLinkScript_Controls_Players$_createMatch$1(id) {
    /// <param name="id" type="String">
    /// </param>
    var dialog = $('#challengeDialog');
    var date = dialog.find('.datepicker').val();
    var time = dialog.find('.time').val();
    var ampm = dialog.find('.ampm').val();
    var comments = dialog.find('.comments').val();
    var datetime = date + ' ' + time + ampm;
    var ids = [];
    dialog.find('.cities input').each(function(index, element) {
        ids.add((element).value);
    });
    var parameters = { date: datetime, locations: ids, comments: comments, opponentId: 0 };
    SportsLinkScript.Controls.QuickMatch.doCreateMatch(dialog, datetime, ids, comments, id, function() {
        dialog.dialog('close');
    });
}
SportsLinkScript.Controls.Players.prototype = {
    
    _moreClick$1: function SportsLinkScript_Controls_Players$_moreClick$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var dialog = $('#playerGridCard');
        dialog.children().first().html('Loading...');
        var parameters = { page: 0 };
        $.post('/services/PlayerGrid?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
        dialog.dialog({ width: $(window).width() - 40, height: $(window).height() - 20, modal: true, title: 'Similar Players', open: function() {
            dialog.find('.comments').focus();
        } });
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.ModuleInstance

SportsLinkScript.Controls.ModuleInstance = function SportsLinkScript_Controls_ModuleInstance(element, instance) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <param name="instance" type="SportsLinkScript.Controls.Module">
    /// </param>
    /// <field name="element" type="Object" domElement="true">
    /// </field>
    /// <field name="instance" type="SportsLinkScript.Controls.Module">
    /// </field>
    this.element = element;
    this.instance = instance;
}
SportsLinkScript.Controls.ModuleInstance.prototype = {
    element: null,
    instance: null
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.Results

SportsLinkScript.Controls.Results = function SportsLinkScript_Controls_Results(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.Results.initializeBase(this, [ element ]);
    $('.inputScore').click(ss.Delegate.create(this, this._reportScore$1));
}
SportsLinkScript.Controls.Results.prototype = {
    
    _reportScore$1: function SportsLinkScript_Controls_Results$_reportScore$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var dialog = $('#scoredialog');
        var score = button.siblings('.score').val();
        var offerId = button.siblings('.offerId').val();
        var requestName = button.siblings('.requestName').val();
        var acceptName = button.siblings('.acceptName').val();
        dialog.find('input').val('');
        var scores = score.split(', ');
        for (var i = 0; i < scores.length; ++i) {
            var parts = scores[i].split('-');
            $('#request' + i).val(parts[0]);
            $('#accept' + i).val(parts[1]);
        }
        dialog.find('.requestName').html(requestName);
        dialog.find('.acceptName').html(acceptName);
        dialog.dialog({ width: '210', height: '370', modal: 'true', buttons: { 'Report Score': ss.Delegate.create(this, function(ex) {
            this._postResults$1(dialog, offerId);
        }) } });
    },
    
    _postResults$1: function SportsLinkScript_Controls_Results$_postResults$1(dialog, offerId) {
        /// <param name="dialog" type="jQueryUIObject">
        /// </param>
        /// <param name="offerId" type="String">
        /// </param>
        var comments = $('#scoreComments').val();
        var score = '';
        for (var i = 0; i < 5; ++i) {
            var requestValue = $('#request' + i).val();
            var acceptValue = $('#accept' + i).val();
            if (String.isNullOrEmpty(requestValue) || String.isNullOrEmpty(acceptValue)) {
                break;
            }
            if (score.length > 0) {
                score = score + ', ';
            }
            score = score + requestValue + '-' + acceptValue;
        }
        var parameters = { offerId: offerId, comments: comments, scores: score };
        dialog.attr('disabled', 'disabled').addClass('ui-state-disabled');
        $.post('/services/PostScore?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            dialog.dialog('destroy');
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
    },
    
    unload: function SportsLinkScript_Controls_Results$unload() {
        var dialog = $('#scoredialog');
        dialog.remove();
        SportsLinkScript.Controls.Results.callBaseMethod(this, 'unload');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.PotentialOffers

SportsLinkScript.Controls.PotentialOffers = function SportsLinkScript_Controls_PotentialOffers(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.PotentialOffers.initializeBase(this, [ element, 'PotentialOffers' ]);
    var acceptMatch = this.obj.find('.acceptMatch');
    var rejectMatch = this.obj.find('.rejectMatch');
    acceptMatch.button({ text: true, icons: { secondary: 'ui-icon-check' } });
    rejectMatch.button({ text: false, icons: { primary: 'ui-icon-closethick' } });
    acceptMatch.click(ss.Delegate.create(this, this._acceptMatch$2));
    rejectMatch.click(ss.Delegate.create(this, this._rejectMatch$2));
}
SportsLinkScript.Controls.PotentialOffers.prototype = {
    
    _acceptMatch$2: function SportsLinkScript_Controls_PotentialOffers$_acceptMatch$2(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var offerId = button.siblings('input').val();
        var parentRow = button.parents('.offer');
        parentRow.attr('disabled', 'disabled').addClass('ui-state-disabled');
        $.post('/services/AcceptOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify({ id: offerId }), function(data, textStatus, request) {
            parentRow.attr('disabled', '').removeClass('ui-state-disabled');
            button.parent().children('a').fadeOut('slow');
        });
    },
    
    _rejectMatch$2: function SportsLinkScript_Controls_PotentialOffers$_rejectMatch$2(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        alert('reject');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.Module

SportsLinkScript.Controls.Module = function SportsLinkScript_Controls_Module(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <field name="obj" type="jQueryObject">
    /// </field>
    /// <field name="_element" type="Object" domElement="true">
    /// </field>
    /// <field name="loadingElement" type="Object" domElement="true">
    /// </field>
    /// <field name="needsData" type="Boolean">
    /// </field>
    /// <field name="instances" type="Array" static="true">
    /// </field>
    SportsLinkScript.Controls.Module.instances.add(new SportsLinkScript.Controls.ModuleInstance(element, this));
    this._element = element;
    this.obj = $(element);
    this.needsData = element.getAttribute('data-async') === 'true';
    if (this.needsData) {
        this.loadingElement = document.createElement('div');
        this.loadingElement.className = 'loading';
        this.loadingElement.innerHTML = 'Loading...';
        $(this.loadingElement).insertAfter(this.obj.find('.data'));
        this.obj.find('.data').hide(0);
        this.loadData();
    }
}
SportsLinkScript.Controls.Module.getModule = function SportsLinkScript_Controls_Module$getModule(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <returns type="SportsLinkScript.Controls.Module"></returns>
    for (var i = 0; i < SportsLinkScript.Controls.Module.instances.length; ++i) {
        var instance = SportsLinkScript.Controls.Module.instances[i];
        if (instance.element === element) {
            return instance.instance;
        }
    }
    return null;
}
SportsLinkScript.Controls.Module.prototype = {
    obj: null,
    _element: null,
    loadingElement: null,
    needsData: false,
    
    loadData: function SportsLinkScript_Controls_Module$loadData() {
    },
    
    unload: function SportsLinkScript_Controls_Module$unload() {
        for (var i = 0; i < SportsLinkScript.Controls.Module.instances.length; ++i) {
            var instance = SportsLinkScript.Controls.Module.instances[i];
            if (instance.element === this._element) {
                SportsLinkScript.Controls.Module.instances.remove(instance);
                return;
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.QuickMatch

SportsLinkScript.Controls.QuickMatch = function SportsLinkScript_Controls_QuickMatch(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.QuickMatch.initializeBase(this, [ element ]);
    this.obj.find('.findMatch').click(ss.Delegate.create(this, this._createMatch$1));
    (this.obj.find('.datepicker')).datepicker();
    (this.obj.find('.findMatch')).button();
    (this.obj.find('select')).selectmenu();
    SportsLinkScript.Shared.Utility._wireAutoComplete(this.obj.find('.placesAutoFill'), this.obj.find('.placesAutoValue'));
}
SportsLinkScript.Controls.QuickMatch.doCreateMatch = function SportsLinkScript_Controls_QuickMatch$doCreateMatch(obj, datetime, ids, comments, opponentId, callback) {
    /// <param name="obj" type="jQueryObject">
    /// </param>
    /// <param name="datetime" type="String">
    /// </param>
    /// <param name="ids" type="Object">
    /// </param>
    /// <param name="comments" type="String">
    /// </param>
    /// <param name="opponentId" type="Object">
    /// </param>
    /// <param name="callback" type="SportsLinkScript.Callback">
    /// </param>
    var parameters = { date: datetime, locations: ids, comments: comments, opponentId: opponentId };
    obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
    $.post('/services/CreateOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
        obj.attr('disabled', '').removeClass('ui-state-disabled');
        SportsLinkScript.Shared.Utility.processResponse(data);
        if (null !== callback) {
            callback();
        }
    });
}
SportsLinkScript.Controls.QuickMatch.prototype = {
    
    _createMatch$1: function SportsLinkScript_Controls_QuickMatch$_createMatch$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var module = button.parents('.module').first();
        var date = module.find('.datepicker').val();
        var time = module.find('.time').val();
        var ampm = module.find('.ampm').val();
        var comments = module.find('.comments').val();
        var datetime = date + ' ' + time + ampm;
        var ids = [];
        module.find('.cities input').each(function(index, element) {
            ids.add((element).value);
        });
        SportsLinkScript.Controls.QuickMatch.doCreateMatch(this.obj, datetime, ids, comments, 0, null);
    }
}


Type.registerNamespace('SportsLinkScript.Pages');

////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Pages._index

SportsLinkScript.Pages._index = function SportsLinkScript_Pages__index() {
}
SportsLinkScript.Pages._index._calendar = function SportsLinkScript_Pages__index$_calendar(ev) {
    /// <param name="ev" type="jQueryEvent">
    /// </param>
    var dialog = $('#calendarCard');
    dialog.children().first().html('Loading...');
    var parameters = { page: 0 };
    $.post('/services/Calendar?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
        SportsLinkScript.Shared.Utility.processResponse(data);
    });
    dialog.dialog({ width: $(window).width() / 1.5, height: $(window).height() - 20, modal: true, title: 'Calendar', open: function() {
        dialog.find('.comments').focus();
    } });
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Pages.Login

SportsLinkScript.Pages.Login = function SportsLinkScript_Pages_Login() {
    /// <field name="accessToken" type="String" static="true">
    /// </field>
}
SportsLinkScript.Pages.Login.init = function SportsLinkScript_Pages_Login$init(appId, action) {
    /// <param name="appId" type="String">
    /// </param>
    /// <param name="action" type="String">
    /// </param>
    var isLoginPage = action === 'Login' || action === 'Register';
    window.fbAsyncInit = function() {
        FB.init({ appId: appId, cookie: true, status: true, xfbml: true });
        FB.Event.subscribe('auth.statusChange', function(r) {
        });
        FB.Event.subscribe('auth.login', function(r) {
            var response = r;
            return;
            if (response.status === 'connected') {
                $('#login').hide();
                return;
            }
            if (isLoginPage) {
                SportsLinkScript.Pages.Login.accessToken = response.session.access_token;
                var query = FB.Data.query('select first_name, last_name, birthday, uid from user where uid={0}', response.session.uid);
                query.wait(SportsLinkScript.Pages.Login.processLogin);
            }
            else {
                $('#main').show('slow');
            }
        });
    };
    var e = document.createElement('script');
    e.async = true;
    e.src = 'http://connect.facebook.net/en_US/all.js';
    document.getElementById('fb-root').appendChild(e);
}
SportsLinkScript.Pages.Login.processLogin = function SportsLinkScript_Pages_Login$processLogin(rows) {
    /// <param name="rows" type="Array">
    /// </param>
    var row = rows[0];
    $.post('/Services/AddUser?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), { member: {} }, function(data, textStatus, request) {
        var response = data;
        var addUser = response.d;
    });
}


Type.registerNamespace('SportsLinkScript');

Type.registerNamespace('SportsLinkScript.Shared');

////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Shared.SessionContext

SportsLinkScript.Shared.SessionContext = function SportsLinkScript_Shared_SessionContext() {
    /// <field name="instance" type="SportsLinkScript.Shared.SessionContext" static="true">
    /// </field>
    /// <field name="familyId" type="Number" integer="true">
    /// </field>
    /// <field name="activeUsers" type="Array">
    /// </field>
    /// <field name="challengId" type="Number" integer="true">
    /// </field>
    this.activeUsers = [];
}
SportsLinkScript.Shared.SessionContext.prototype = {
    familyId: 0,
    challengId: 0
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Shared.Utility

SportsLinkScript.Shared.Utility = function SportsLinkScript_Shared_Utility() {
    /// <field name="_cache" type="Object" static="true">
    /// </field>
    /// <field name="_lastRequest" type="jQueryXmlHttpRequest`1" static="true">
    /// </field>
}
SportsLinkScript.Shared.Utility._getSignedRequest = function SportsLinkScript_Shared_Utility$_getSignedRequest() {
    /// <returns type="String"></returns>
    return document.getElementById('signed_request').getAttribute('value');
}
SportsLinkScript.Shared.Utility._wireAutoComplete = function SportsLinkScript_Shared_Utility$_wireAutoComplete(autoFill, hiddenField) {
    /// <param name="autoFill" type="jQueryUIObject">
    /// </param>
    /// <param name="hiddenField" type="jQueryUIObject">
    /// </param>
    var accessToken = autoFill.attr('data-accesstoken');
    var location = autoFill.attr('data-location');
    autoFill.autocomplete({ minLength: 2, open: function() {
        $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
    }, close: function() {
        $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
    }, select: function(ev, obj) {
        if (null !== hiddenField) {
            var data = obj;
            autoFill.val(data.item.label);
            hiddenField.val(data.item.value);
            ev.stopPropagation();
        }
        return false;
    }, source: function(request, response) {
        var term = request.term;
        if (!!SportsLinkScript.Shared.Utility._cache[term]) {
            response(SportsLinkScript.Shared.Utility._cache[term]);
            return;
        }
        SportsLinkScript.Shared.Utility._lastRequest = $.post('/services/serviceproxy', JSON.stringify({ url: 'https://maps.googleapis.com/maps/api/place/search/json?location=' + encodeURIComponent(location) + '&radius=5000&name=' + encodeURIComponent(term) + '&sensor=false&key=AIzaSyBnD3R38Jh9IhcT7VOJ4Mh8vE7AkSuP_zE' }), function(data, textStatus, xhr) {
            if (xhr === SportsLinkScript.Shared.Utility._lastRequest) {
                var placesData = data;
                var places = [];
                for (var i = 0; i < placesData.results.length; ++i) {
                    var item = placesData.results[i];
                    places.add({ value: item.id, label: item.name, icon: item.icon, description: item.vicinity });
                }
                SportsLinkScript.Shared.Utility._cache[term] = places;
                response(places);
            }
        });
    } }).data('autocomplete')._renderItem = function(element, item) {
        var acItem = item;
        return $("<li class='acItem'></li>").data('item.autocomplete', item).append("<a><div class='acName'>" + acItem.label + "</div><div class='acLoc'>" + acItem.description + '</div></a>').appendTo(element);
    };
}
SportsLinkScript.Shared.Utility.showPlayerDetails = function SportsLinkScript_Shared_Utility$showPlayerDetails(dialogContainerId, name, id) {
    /// <param name="dialogContainerId" type="String">
    /// </param>
    /// <param name="name" type="String">
    /// </param>
    /// <param name="id" type="Number" integer="true">
    /// </param>
    var container = $('#' + dialogContainerId);
    if (container.length > 0) {
        container.children().first().html('Loading...');
        container.attr('title', name);
        container.attr('data-id', id.toString());
        var parameters = { id: id };
        $.post('/services/PlayerDetails?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        });
        container.dialog({ title: name, width: '340', height: '160', modal: 'true' });
    }
}
SportsLinkScript.Shared.Utility.processResponse = function SportsLinkScript_Shared_Utility$processResponse(obj) {
    /// <param name="obj" type="Object">
    /// </param>
    var keys = ss.getKeys(obj);
    for (var i = 0; i < keys.length; ++i) {
        var keyId = keys[i];
        var name = '#module_' + keyId;
        var content = $(name);
        if (content.length > 0) {
            SportsLinkScript.Shared.Utility._updateModule(content, obj[keyId]);
        }
    }
}
SportsLinkScript.Shared.Utility._loadModule = function SportsLinkScript_Shared_Utility$_loadModule(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    var dataType = element.getAttribute('data-type');
    if (null !== dataType) {
        var type = Type.parse('SportsLinkScript.Controls.' + dataType);
        if (!ss.isNullOrUndefined(type)) {
            new type(element);
        }
    }
}
SportsLinkScript.Shared.Utility._updateModule = function SportsLinkScript_Shared_Utility$_updateModule(content, value) {
    /// <param name="content" type="jQueryObject">
    /// </param>
    /// <param name="value" type="String">
    /// </param>
    var dataTypes = content.children('*[data-type]');
    var module = null;
    if (dataTypes.length > 0) {
        var element = dataTypes.first().get(0);
        module = SportsLinkScript.Controls.Module.getModule(element);
    }
    content.fadeOut(500, function() {
        if (null !== module) {
            module.unload();
        }
        content.html(value);
        content.fadeIn(500);
        dataTypes = content.children('*[data-type]');
        if (dataTypes.length > 0) {
            SportsLinkScript.Shared.Utility._loadModule(dataTypes.first().get(0));
        }
    });
}


Type.registerNamespace('SportsLinkScript.Shared.Facebook');

SportsLinkScript.Controls.Module.registerClass('SportsLinkScript.Controls.Module');
SportsLinkScript.Controls.PaginatedModule.registerClass('SportsLinkScript.Controls.PaginatedModule', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.PotentialOffers.registerClass('SportsLinkScript.Controls.PotentialOffers', SportsLinkScript.Controls.PaginatedModule);
SportsLinkScript.Controls.Calendar.registerClass('SportsLinkScript.Controls.Calendar', SportsLinkScript.Controls.PotentialOffers);
SportsLinkScript.Controls.PlayerGrid.registerClass('SportsLinkScript.Controls.PlayerGrid', SportsLinkScript.Controls.PaginatedModule);
SportsLinkScript.Controls.UserDetails.registerClass('SportsLinkScript.Controls.UserDetails', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.PlayerDetails.registerClass('SportsLinkScript.Controls.PlayerDetails', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.UserOffers.registerClass('SportsLinkScript.Controls.UserOffers', SportsLinkScript.Controls.PaginatedModule);
SportsLinkScript.Controls.Players.registerClass('SportsLinkScript.Controls.Players', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.ModuleInstance.registerClass('SportsLinkScript.Controls.ModuleInstance');
SportsLinkScript.Controls.Results.registerClass('SportsLinkScript.Controls.Results', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.QuickMatch.registerClass('SportsLinkScript.Controls.QuickMatch', SportsLinkScript.Controls.Module);
SportsLinkScript.Pages._index.registerClass('SportsLinkScript.Pages._index');
SportsLinkScript.Pages.Login.registerClass('SportsLinkScript.Pages.Login');
SportsLinkScript.Shared.SessionContext.registerClass('SportsLinkScript.Shared.SessionContext');
SportsLinkScript.Shared.Utility.registerClass('SportsLinkScript.Shared.Utility');
SportsLinkScript.Controls.Module.instances = [];
(function () {
    $.ajaxSetup({ contentType: 'application/json; charset=utf-8', dataType: 'json' });
    $(function() {
        $('.module').each(function(index, element) {
            SportsLinkScript.Shared.Utility._loadModule(element);
            return true;
        });
        $('#header .calendar').click(SportsLinkScript.Pages._index._calendar);
    });
})();
SportsLinkScript.Pages.Login.accessToken = null;
SportsLinkScript.Shared.SessionContext.instance = new SportsLinkScript.Shared.SessionContext();
SportsLinkScript.Shared.Utility._cache = {};
SportsLinkScript.Shared.Utility._lastRequest = null;
