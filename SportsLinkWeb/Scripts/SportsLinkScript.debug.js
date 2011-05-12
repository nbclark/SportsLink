
Type.registerNamespace('SportsLinkScript.Controls');

////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.UserOffers

SportsLinkScript.Controls.UserOffers = function SportsLinkScript_Controls_UserOffers(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    SportsLinkScript.Controls.UserOffers.initializeBase(this, [ element ]);
    var cancelMatch = this.obj.find('.cancelMatch');
    cancelMatch.button({ text: false, icons: { primary: 'ui-icon-closethick' } });
    cancelMatch.click(ss.Delegate.create(this, this._cancelOffer$1));
}
SportsLinkScript.Controls.UserOffers.prototype = {
    
    _cancelOffer$1: function SportsLinkScript_Controls_UserOffers$_cancelOffer$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var button = $(e.currentTarget);
        var parameters = { offerId: button.attr('data-offerId') };
        $.post('/services/CancelOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), ss.Delegate.create(this, function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        }));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Controls.Players

SportsLinkScript.Controls.Players = function SportsLinkScript_Controls_Players(element) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <field name="_page$1" type="Number" integer="true">
    /// </field>
    SportsLinkScript.Controls.Players.initializeBase(this, [ element ]);
    this._page$1 = parseInt($('#playersPage').val());
    var requestMatch = this.obj.find('.requestMatch');
    requestMatch.button({ text: true, icons: { secondary: 'ui-icon-carat-1-e' } });
    requestMatch.click(ss.Delegate.create(this, this._requestMatch$1));
    var prev = $('#playersPrev');
    var next = $('#playersNext');
    prev.button();
    next.button();
    prev.click(ss.Delegate.create(this, this._pagePrev$1));
    next.click(ss.Delegate.create(this, this._pageNext$1));
}
SportsLinkScript.Controls.Players.prototype = {
    _page$1: 0,
    
    _requestMatch$1: function SportsLinkScript_Controls_Players$_requestMatch$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var dialog = $('#challengeDialog');
        var datePicker = dialog.find('.datepicker');
        var id = button.get(0).id;
        datePicker.datepicker('disable');
        dialog.dialog({ width: '260', height: '254', modal: true, title: button.attr('Title'), buttons: { 'Challenge!': ss.Delegate.create(this, function(ex) {
            this._createMatch$1(id);
        }) }, open: ss.Delegate.create(this, function() {
            dialog.find('.comments').focus();
            datePicker.datepicker('enable');
        }) });
    },
    
    _pagePrev$1: function SportsLinkScript_Controls_Players$_pagePrev$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var button = $(e.currentTarget);
        var parameters = { page: this._page$1 - 1 };
        $.post('/services/Players?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), ss.Delegate.create(this, function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        }));
    },
    
    _pageNext$1: function SportsLinkScript_Controls_Players$_pageNext$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        this.obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
        var button = $(e.currentTarget);
        var parameters = { page: this._page$1 + 1 };
        $.post('/services/Players?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), ss.Delegate.create(this, function(data, textStatus, request) {
            SportsLinkScript.Shared.Utility.processResponse(data);
        }));
    },
    
    _createMatch$1: function SportsLinkScript_Controls_Players$_createMatch$1(id) {
        /// <param name="id" type="String">
        /// </param>
        var dialog = $('#challengeDialog');
        var date = dialog.find('.datepicker').val();
        var time = dialog.find('.time').val();
        var ampm = dialog.find('.ampm').val();
        var comments = dialog.find('.comments').val();
        var datetime = date + ' ' + time + ampm;
        var ids = [];
        dialog.find('.cities input').each(ss.Delegate.create(this, function(index, element) {
            ids.add((element).value);
        }));
        var parameters = { date: datetime, locations: ids, comments: comments, opponentId: 0 };
        SportsLinkScript.Controls.QuickMatch.doCreateMatch(dialog, datetime, ids, comments, id, ss.Delegate.create(this, function() {
            dialog.dialog('close');
        }));
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
        $.post('/services/PostScore?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), ss.Delegate.create(this, function(data, textStatus, request) {
            dialog.dialog('destroy');
            SportsLinkScript.Shared.Utility.processResponse(data);
        }));
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
    SportsLinkScript.Controls.PotentialOffers.initializeBase(this, [ element ]);
    var acceptMatch = this.obj.find('.acceptMatch');
    var rejectMatch = this.obj.find('.rejectMatch');
    acceptMatch.button({ text: false, icons: { primary: 'ui-icon-check' } });
    rejectMatch.button({ text: false, icons: { primary: 'ui-icon-closethick' } });
    acceptMatch.click(ss.Delegate.create(this, this._acceptMatch$1));
    rejectMatch.click(ss.Delegate.create(this, this._rejectMatch$1));
}
SportsLinkScript.Controls.PotentialOffers.prototype = {
    
    _acceptMatch$1: function SportsLinkScript_Controls_PotentialOffers$_acceptMatch$1(e) {
        /// <param name="e" type="jQueryEvent">
        /// </param>
        var button = $(e.currentTarget);
        var offerId = button.siblings('input').val();
        $.post('/services/AcceptOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify({ id: offerId }), ss.Delegate.create(this, function(data, textStatus, request) {
            button.parent().children('a').fadeOut('slow');
        }));
    },
    
    _rejectMatch$1: function SportsLinkScript_Controls_PotentialOffers$_rejectMatch$1(e) {
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
    /// <param name="callback" type="Callback">
    /// </param>
    var parameters = { date: datetime, locations: ids, comments: comments, opponentId: opponentId };
    obj.attr('disabled', 'disabled').addClass('ui-state-disabled');
    alert(document.getElementById('signed_request').getAttribute('value'));
    $.post('/services/CreateOffer?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
        obj.attr('disabled', '').removeClass('ui-state-disabled');
        SportsLinkScript.Shared.Utility.processResponse(data);
        if (null !== callback) {
            callback.invoke();
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
        module.find('.cities input').each(ss.Delegate.create(this, function(index, element) {
            ids.add((element).value);
        }));
        SportsLinkScript.Controls.QuickMatch.doCreateMatch(this.obj, datetime, ids, comments, 0, null);
    }
}


Type.registerNamespace('SportsLinkScript.Pages');

////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Pages._index

SportsLinkScript.Pages._index = function SportsLinkScript_Pages__index() {
}


////////////////////////////////////////////////////////////////////////////////
// SportsLinkScript.Pages.Login

SportsLinkScript.Pages.Login = function SportsLinkScript_Pages_Login() {
    /// <field name="appId" type="String" static="true">
    /// </field>
    /// <field name="accessToken" type="String" static="true">
    /// </field>
}
SportsLinkScript.Pages.Login.init = function SportsLinkScript_Pages_Login$init(action) {
    /// <param name="action" type="String">
    /// </param>
    var isLoginPage = action === 'Login' || action === 'Register';
    if (isLoginPage) {
        $('#main').show('fast');
    }
    window.fbAsyncInit = function() {
        FB.init({ appId: SportsLinkScript.Pages.Login.appId, cookie: true, status: true, xfbml: true });
        FB.getLoginStatus(function(r) {
            var response = r;
            if (response.status === 'connected') {
                if (action === 'Login') {
                    $('#login').hide();
                    window.location.href = '/home/index';
                }
                else {
                    $('#main').show('slow');
                }
            }
            else if (response.status === 'notConnected' && action !== 'Register') {
                $('#login').hide();
                window.location.href = '/home/register';
            }
        });
        FB.Event.subscribe('auth.login', function(r) {
            var response = r;
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
        return null;
    };
    var e = document.createElement('script');
    e.async = true;
    e.src = '/scripts/fb.js';
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
}
SportsLinkScript.Shared.Utility._getSignedRequest = function SportsLinkScript_Shared_Utility$_getSignedRequest() {
    /// <returns type="String"></returns>
    return document.getElementById('signed_request').getAttribute('value');
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
        container.html('Loading...');
        container.attr('title', name);
        var parameters = { id: id };
        $.post('/services/PlayerDetails?signed_request=' + SportsLinkScript.Shared.Utility._getSignedRequest(), JSON.stringify(parameters), function(data, textStatus, request) {
            var html = (data)['PlayerDetails'];
            container.html(html);
        });
        container.dialog({ title: name, width: '340', height: '150', modal: 'true' });
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
    var element = content.children().first().get(0);
    var module = SportsLinkScript.Controls.Module.getModule(element);
    debugger;
    content.fadeOut(500, function() {
        if (null !== module) {
            module.unload();
        }
        content.html(value);
        content.fadeIn(500);
        SportsLinkScript.Shared.Utility._loadModule(content.children().first().get(0));
    });
}


Type.registerNamespace('SportsLinkScript.Shared.Facebook');

SportsLinkScript.Controls.Module.registerClass('SportsLinkScript.Controls.Module');
SportsLinkScript.Controls.UserOffers.registerClass('SportsLinkScript.Controls.UserOffers', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.Players.registerClass('SportsLinkScript.Controls.Players', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.ModuleInstance.registerClass('SportsLinkScript.Controls.ModuleInstance');
SportsLinkScript.Controls.Results.registerClass('SportsLinkScript.Controls.Results', SportsLinkScript.Controls.Module);
SportsLinkScript.Controls.PotentialOffers.registerClass('SportsLinkScript.Controls.PotentialOffers', SportsLinkScript.Controls.Module);
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
    });
})();
SportsLinkScript.Pages.Login.appId = '197465840298266';
SportsLinkScript.Pages.Login.accessToken = null;
SportsLinkScript.Shared.SessionContext.instance = new SportsLinkScript.Shared.SessionContext();
