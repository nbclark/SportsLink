
Type.registerNamespace('SportsLinkScript.Controls');SportsLinkScript.Controls.Calendar=function(element){SportsLinkScript.Controls.Calendar.initializeBase(this,[element]);this.serviceName='Calendar';}
SportsLinkScript.Controls.Calendar.prototype={$6:function($p0){var $0=$($p0.currentTarget);var $1=$0.siblings('input').val();var $2=$0.parents('.offer');$2.attr('disabled','disabled').addClass('ui-state-disabled');$.post('/services/AcceptOffer?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify({id:$1}),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
$2.attr('disabled','').removeClass('ui-state-disabled');$0.parent().children('a').fadeOut('slow');}));},$7:function($p0){alert('reject');}}
SportsLinkScript.Controls.UserDetails=function(element){SportsLinkScript.Controls.UserDetails.initializeBase(this,[element]);this.$1=this.obj.find('a.edit');this.$1.click(ss.Delegate.create(this,this.$3));this.$2=this.obj.find('a.save');this.$2.click(ss.Delegate.create(this,this.$4));}
SportsLinkScript.Controls.UserDetails.prototype={$1:null,$2:null,$3:function($p0){var $0=this.obj.find('.keyvaluerow .edit');$0.show('fast');$0.prev('.value').hide('fast');this.$1.hide('fast');this.$2.show('fast');},$4:function($p0){var $0=this.obj.find('.keyvaluerow .edit');$0.hide('fast');$0.prev('.value').show('fast');this.$1.show('fast');this.$2.hide('fast');}}
SportsLinkScript.Controls.PaginatedModule=function(element,serviceName){SportsLinkScript.Controls.PaginatedModule.initializeBase(this,[element]);this.$1=parseInt(this.obj.find('.page').val());this.serviceName=serviceName;var $0=this.obj.find('.prev');var $1=this.obj.find('.next');$0.button();$1.button();$0.click(ss.Delegate.create(this,this.$2));$1.click(ss.Delegate.create(this,this.$3));}
SportsLinkScript.Controls.PaginatedModule.prototype={$1:0,serviceName:null,$2:function($p0){this.obj.attr('disabled','disabled').addClass('ui-state-disabled');var $0=$($p0.currentTarget);var $1={page:this.$1-1};$.post('/services/'+this.serviceName+'?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($1),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
SportsLinkScript.Shared.Utility.processResponse($p1_0);}));},$3:function($p0){this.obj.attr('disabled','disabled').addClass('ui-state-disabled');var $0=$($p0.currentTarget);var $1={page:this.$1+1};$.post('/services/'+this.serviceName+'?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($1),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
SportsLinkScript.Shared.Utility.processResponse($p1_0);}));}}
SportsLinkScript.Controls.PlayerDetails=function(element){SportsLinkScript.Controls.PlayerDetails.initializeBase(this,[element]);var $0=this.obj.find('#playerMessage .sendMessage');$0.button({text:true,icons:{secondary:'ui-icon-carat-1-e'}});$0.click(ss.Delegate.create(this,this.$1));}
SportsLinkScript.Controls.PlayerDetails.prototype={$1:function($p0){var $0=$($p0.currentTarget);var $1=$('#playerDetailsCard');var $2=$('#playerDetailsCard .comments').val();var $3=$1.attr('data-id');debugger;$1.attr('disabled','disabled').addClass('ui-state-disabled');var $4={userId:$3,comments:$2};$.post('/services/SendMessage?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($4),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
$1.attr('disabled','').removeClass('ui-state-disabled');$1.dialog('close');SportsLinkScript.Shared.Utility.processResponse($p1_0);}));}}
SportsLinkScript.Controls.UserOffers=function(element){SportsLinkScript.Controls.UserOffers.initializeBase(this,[element,'UserOffers']);var $0=this.obj.find('.cancelMatch');$0.button({text:false,icons:{primary:'ui-icon-closethick'}});$0.click(ss.Delegate.create(this,this.$4));}
SportsLinkScript.Controls.UserOffers.prototype={$4:function($p0){this.obj.attr('disabled','disabled').addClass('ui-state-disabled');var $0=$($p0.currentTarget);var $1={offerId:$0.attr('data-offerId')};$.post('/services/CancelOffer?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($1),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
SportsLinkScript.Shared.Utility.processResponse($p1_0);}));}}
SportsLinkScript.Controls.Players=function(element){SportsLinkScript.Controls.Players.initializeBase(this,[element,'Players']);var $0=this.obj.find('.requestMatch');$0.button({text:true,icons:{secondary:'ui-icon-carat-1-e'}});$0.click(ss.Delegate.create(this,this.$4));}
SportsLinkScript.Controls.Players.prototype={$4:function($p0){var $0=$($p0.currentTarget);var $1=$('#challengeDialog');var $2=$1.find('.datepicker');SportsLinkScript.Shared.Utility.$3($1.find('.placesAutoFill'),$1.find('.placesAutoValue'));var $3=$0.get(0).id;$2.datepicker('disable');$1.dialog({width:'260',height:'324',modal:true,title:$0.attr('Title'),buttons:{'Challenge!':ss.Delegate.create(this,function($p1_0){
this.$5($3);})},open:ss.Delegate.create(this,function(){
$1.find('.comments').focus();$2.datepicker('enable');})});},$5:function($p0){var $0=$('#challengeDialog');var $1=$0.find('.datepicker').val();var $2=$0.find('.time').val();var $3=$0.find('.ampm').val();var $4=$0.find('.comments').val();var $5=$1+' '+$2+$3;var $6=[];$0.find('.cities input').each(ss.Delegate.create(this,function($p1_0,$p1_1){
$6.add(($p1_1).value);}));var $7={date:$5,locations:$6,comments:$4,opponentId:0};SportsLinkScript.Controls.QuickMatch.doCreateMatch($0,$5,$6,$4,$p0,ss.Delegate.create(this,function(){
$0.dialog('close');}));}}
SportsLinkScript.Controls.ModuleInstance=function(element,instance){this.element=element;this.instance=instance;}
SportsLinkScript.Controls.ModuleInstance.prototype={element:null,instance:null}
SportsLinkScript.Controls.Results=function(element){SportsLinkScript.Controls.Results.initializeBase(this,[element]);$('.inputScore').click(ss.Delegate.create(this,this.$1));}
SportsLinkScript.Controls.Results.prototype={$1:function($p0){var $0=$($p0.currentTarget);var $1=$('#scoredialog');var $2=$0.siblings('.score').val();var $3=$0.siblings('.offerId').val();var $4=$0.siblings('.requestName').val();var $5=$0.siblings('.acceptName').val();$1.find('input').val('');var $6=$2.split(', ');for(var $7=0;$7<$6.length;++$7){var $8=$6[$7].split('-');$('#request'+$7).val($8[0]);$('#accept'+$7).val($8[1]);}$1.find('.requestName').html($4);$1.find('.acceptName').html($5);$1.dialog({width:'210',height:'370',modal:'true',buttons:{'Report Score':ss.Delegate.create(this,function($p1_0){
this.$2($1,$3);})}});},$2:function($p0,$p1){var $0=$('#scoreComments').val();var $1='';for(var $3=0;$3<5;++$3){var $4=$('#request'+$3).val();var $5=$('#accept'+$3).val();if(String.isNullOrEmpty($4)||String.isNullOrEmpty($5)){break;}if($1.length>0){$1=$1+', ';}$1=$1+$4+'-'+$5;}var $2={offerId:$p1,comments:$0,scores:$1};$p0.attr('disabled','disabled').addClass('ui-state-disabled');$.post('/services/PostScore?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($2),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
$p0.dialog('destroy');SportsLinkScript.Shared.Utility.processResponse($p1_0);}));},unload:function(){var $0=$('#scoredialog');$0.remove();SportsLinkScript.Controls.Results.callBaseMethod(this, 'unload');}}
SportsLinkScript.Controls.PotentialOffers=function(element){SportsLinkScript.Controls.PotentialOffers.initializeBase(this,[element,'PotentialOffers']);var $0=this.obj.find('.acceptMatch');var $1=this.obj.find('.rejectMatch');$0.button({text:true,icons:{secondary:'ui-icon-check'}});$1.button({text:false,icons:{primary:'ui-icon-closethick'}});$0.click(ss.Delegate.create(this,this.$4));$1.click(ss.Delegate.create(this,this.$5));}
SportsLinkScript.Controls.PotentialOffers.prototype={$4:function($p0){var $0=$($p0.currentTarget);var $1=$0.siblings('input').val();var $2=$0.parents('.offer');$2.attr('disabled','disabled').addClass('ui-state-disabled');$.post('/services/AcceptOffer?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify({id:$1}),ss.Delegate.create(this,function($p1_0,$p1_1,$p1_2){
$2.attr('disabled','').removeClass('ui-state-disabled');$0.parent().children('a').fadeOut('slow');}));},$5:function($p0){alert('reject');}}
SportsLinkScript.Controls.Module=function(element){SportsLinkScript.Controls.Module.instances.add(new SportsLinkScript.Controls.ModuleInstance(element,this));this.$0=element;this.obj=$(element);this.needsData=element.getAttribute('data-async')==='true';if(this.needsData){this.loadingElement=document.createElement('div');this.loadingElement.className='loading';this.loadingElement.innerHTML='Loading...';$(this.loadingElement).insertAfter(this.obj.find('.data'));this.obj.find('.data').hide(0);this.loadData();}}
SportsLinkScript.Controls.Module.getModule=function(element){for(var $0=0;$0<SportsLinkScript.Controls.Module.instances.length;++$0){var $1=SportsLinkScript.Controls.Module.instances[$0];if($1.element===element){return $1.instance;}}return null;}
SportsLinkScript.Controls.Module.prototype={obj:null,$0:null,loadingElement:null,needsData:false,loadData:function(){},unload:function(){for(var $0=0;$0<SportsLinkScript.Controls.Module.instances.length;++$0){var $1=SportsLinkScript.Controls.Module.instances[$0];if($1.element===this.$0){SportsLinkScript.Controls.Module.instances.remove($1);return;}}}}
SportsLinkScript.Controls.QuickMatch=function(element){SportsLinkScript.Controls.QuickMatch.initializeBase(this,[element]);this.obj.find('.findMatch').click(ss.Delegate.create(this,this.$1));(this.obj.find('.datepicker')).datepicker();(this.obj.find('.findMatch')).button();(this.obj.find('select')).selectmenu();SportsLinkScript.Shared.Utility.$3(this.obj.find('.placesAutoFill'),this.obj.find('.placesAutoValue'));}
SportsLinkScript.Controls.QuickMatch.doCreateMatch=function(obj,datetime,ids,comments,opponentId,callback){var $0={date:datetime,locations:ids,comments:comments,opponentId:opponentId};obj.attr('disabled','disabled').addClass('ui-state-disabled');$.post('/services/CreateOffer?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($0),function($p1_0,$p1_1,$p1_2){
obj.attr('disabled','').removeClass('ui-state-disabled');SportsLinkScript.Shared.Utility.processResponse($p1_0);if(null!==callback){callback.invoke();}});}
SportsLinkScript.Controls.QuickMatch.prototype={$1:function($p0){var $0=$($p0.currentTarget);var $1=$0.parents('.module').first();var $2=$1.find('.datepicker').val();var $3=$1.find('.time').val();var $4=$1.find('.ampm').val();var $5=$1.find('.comments').val();var $6=$2+' '+$3+$4;var $7=[];$1.find('.cities input').each(ss.Delegate.create(this,function($p1_0,$p1_1){
$7.add(($p1_1).value);}));SportsLinkScript.Controls.QuickMatch.doCreateMatch(this.obj,$6,$7,$5,0,null);}}
Type.registerNamespace('SportsLinkScript.Pages');SportsLinkScript.Pages._Index=function(){}
SportsLinkScript.Pages._Index.$0=function($p0){var $0=$('#calendarCard');$0.children().first().html('Loading...');var $1={page:0};$.post('/services/Calendar?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($1),function($p1_0,$p1_1,$p1_2){
SportsLinkScript.Shared.Utility.processResponse($p1_0);});$0.dialog({width:$(window).width()/1.5,height:$(window).height()-20,modal:true,title:'Calendar',open:function(){
$0.find('.comments').focus();}});}
SportsLinkScript.Pages.Login=function(){}
SportsLinkScript.Pages.Login.init=function(appId,action){var $0=action==='Login'||action==='Register';window.fbAsyncInit=function(){
FB.init({appId:appId,cookie:true,status:true,xfbml:true});FB.Event.subscribe('auth.statusChange',function($p2_0){
});FB.Event.subscribe('auth.login',function($p2_0){
var $2_0=$p2_0;return;if($2_0.status==='connected'){$('#login').hide();return;}if($0){SportsLinkScript.Pages.Login.accessToken=$2_0.session.access_token;var $2_1=FB.Data.query('select first_name, last_name, birthday, uid from user where uid={0}',$2_0.session.uid);$2_1.wait(SportsLinkScript.Pages.Login.processLogin);}else{$('#main').show('slow');}});return null;};var $1=document.createElement('script');$1.async=true;$1.src='http://connect.facebook.net/en_US/all.js';document.getElementById('fb-root').appendChild($1);}
SportsLinkScript.Pages.Login.processLogin=function(rows){var $0=rows[0];$.post('/Services/AddUser?signed_request='+SportsLinkScript.Shared.Utility.$2(),{member:{}},function($p1_0,$p1_1,$p1_2){
var $1_0=$p1_0;var $1_1=$1_0.d;});}
Type.registerNamespace('SportsLinkScript.Shared');SportsLinkScript.Shared.SessionContext=function(){this.activeUsers=[];}
SportsLinkScript.Shared.SessionContext.prototype={familyId:0,challengId:0}
SportsLinkScript.Shared.Utility=function(){}
SportsLinkScript.Shared.Utility.$2=function(){return document.getElementById('signed_request').getAttribute('value');}
SportsLinkScript.Shared.Utility.$3=function($p0,$p1){var $0=$p0.attr('data-accesstoken');var $1=$p0.attr('data-location');$p0.autocomplete({minLength:2,open:function(){
$(this).removeClass('ui-corner-all').addClass('ui-corner-top');},close:function(){
$(this).removeClass('ui-corner-top').addClass('ui-corner-all');},select:function($p1_0,$p1_1){
if(null!==$p1){var $1_0=$p1_1;$p0.val($1_0.item.label);$p1.val($1_0.item.value);$p1_0.stopPropagation();}return false;},source:function($p1_0,$p1_1){
var $1_0=$p1_0.term;if(SportsLinkScript.Shared.Utility.$0[$1_0]!=null){$p1_1.invoke(SportsLinkScript.Shared.Utility.$0[$1_0]);return;}SportsLinkScript.Shared.Utility.$1=$.post('/services/serviceproxy',JSON.stringify({url:'https://maps.googleapis.com/maps/api/place/search/json?location='+encodeURIComponent($1)+'&radius=5000&name='+encodeURIComponent($1_0)+'&sensor=false&key=AIzaSyBnD3R38Jh9IhcT7VOJ4Mh8vE7AkSuP_zE'}),function($p2_0,$p2_1,$p2_2){
if($p2_2===SportsLinkScript.Shared.Utility.$1){var $2_0=$p2_0;var $2_1=[];for(var $2_2=0;$2_2<$2_0.results.length;++$2_2){var $2_3=$2_0.results[$2_2];$2_1.add({value:$2_3.id,label:$2_3.name,icon:$2_3.icon,description:$2_3.vicinity});}SportsLinkScript.Shared.Utility.$0[$1_0]=$2_1;$p1_1.invoke($2_1);}});}}).data('autocomplete')._renderItem=function($p1_0,$p1_1){
var $1_0=$p1_1;return $('<li class=\'acItem\'></li>').data('item.autocomplete',$p1_1).append('<a><div class=\'acName\'>'+$1_0.label+'</div><div class=\'acLoc\'>'+$1_0.description+'</div></a>').appendTo($p1_0);};}
SportsLinkScript.Shared.Utility.showPlayerDetails=function(dialogContainerId,name,id){var $0=$('#'+dialogContainerId);if($0.length>0){$0.children().first().html('Loading...');$0.attr('title',name);$0.attr('data-id',id.toString());var $1={id:id};$.post('/services/PlayerDetails?signed_request='+SportsLinkScript.Shared.Utility.$2(),JSON.stringify($1),function($p1_0,$p1_1,$p1_2){
SportsLinkScript.Shared.Utility.processResponse($p1_0);});$0.dialog({title:name,width:'340',height:'160',modal:'true'});}}
SportsLinkScript.Shared.Utility.processResponse=function(obj){var $0=ss.getKeys(obj);for(var $1=0;$1<$0.length;++$1){var $2=$0[$1];var $3='#module_'+$2;var $4=$($3);if($4.length>0){SportsLinkScript.Shared.Utility.$5($4,obj[$2]);}}}
SportsLinkScript.Shared.Utility.$4=function($p0){var $0=$p0.getAttribute('data-type');if(null!==$0){var $1=Type.parse('SportsLinkScript.Controls.'+$0);if(!ss.isNullOrUndefined($1)){new $1($p0);}}}
SportsLinkScript.Shared.Utility.$5=function($p0,$p1){var $0=$p0.children('*[data-type]');var $1=null;if($0.length>0){var $2=$0.first().get(0);$1=SportsLinkScript.Controls.Module.getModule($2);}$p0.fadeOut(500,function(){
if(null!==$1){$1.unload();}$p0.html($p1);$p0.fadeIn(500);$0=$p0.children('*[data-type]');if($0.length>0){SportsLinkScript.Shared.Utility.$4($0.first().get(0));}});}
Type.registerNamespace('SportsLinkScript.Shared.Facebook');SportsLinkScript.Controls.Module.registerClass('SportsLinkScript.Controls.Module');SportsLinkScript.Controls.PaginatedModule.registerClass('SportsLinkScript.Controls.PaginatedModule',SportsLinkScript.Controls.Module);SportsLinkScript.Controls.PotentialOffers.registerClass('SportsLinkScript.Controls.PotentialOffers',SportsLinkScript.Controls.PaginatedModule);SportsLinkScript.Controls.Calendar.registerClass('SportsLinkScript.Controls.Calendar',SportsLinkScript.Controls.PotentialOffers);SportsLinkScript.Controls.UserDetails.registerClass('SportsLinkScript.Controls.UserDetails',SportsLinkScript.Controls.Module);SportsLinkScript.Controls.PlayerDetails.registerClass('SportsLinkScript.Controls.PlayerDetails',SportsLinkScript.Controls.Module);SportsLinkScript.Controls.UserOffers.registerClass('SportsLinkScript.Controls.UserOffers',SportsLinkScript.Controls.PaginatedModule);SportsLinkScript.Controls.Players.registerClass('SportsLinkScript.Controls.Players',SportsLinkScript.Controls.PaginatedModule);SportsLinkScript.Controls.ModuleInstance.registerClass('SportsLinkScript.Controls.ModuleInstance');SportsLinkScript.Controls.Results.registerClass('SportsLinkScript.Controls.Results',SportsLinkScript.Controls.Module);SportsLinkScript.Controls.QuickMatch.registerClass('SportsLinkScript.Controls.QuickMatch',SportsLinkScript.Controls.Module);SportsLinkScript.Pages._Index.registerClass('SportsLinkScript.Pages._Index');SportsLinkScript.Pages.Login.registerClass('SportsLinkScript.Pages.Login');SportsLinkScript.Shared.SessionContext.registerClass('SportsLinkScript.Shared.SessionContext');SportsLinkScript.Shared.Utility.registerClass('SportsLinkScript.Shared.Utility');SportsLinkScript.Controls.Module.instances=[];(function(){$.ajaxSetup({contentType:'application/json; charset=utf-8',dataType:'json'});$(function(){
$('.module').each(function($p2_0,$p2_1){
SportsLinkScript.Shared.Utility.$4($p2_1);return true;});$('#header .calendar').click(SportsLinkScript.Pages._Index.$0);});})();
SportsLinkScript.Pages.Login.accessToken=null;SportsLinkScript.Shared.SessionContext.instance=new SportsLinkScript.Shared.SessionContext();SportsLinkScript.Shared.Utility.$0={};SportsLinkScript.Shared.Utility.$1=null;