"use strict";
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result["default"] = mod;
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = __importStar(require("@microsoft/signalr"));
var utilities_1 = __importDefault(require("./utilities"));
var EUserType;
(function (EUserType) {
    EUserType[EUserType["Pharmacy"] = 1] = "Pharmacy";
    EUserType[EUserType["Stock"] = 2] = "Stock";
    EUserType[EUserType["Admin"] = 3] = "Admin";
})(EUserType || (EUserType = {}));
var questionNotificationStoreName = "quesNotifications";
var SelectedNotifyquestion = "selectedNotifQues";
var NotificationStore = /** @class */ (function () {
    function NotificationStore() {
        this.checkIfStoreExists();
    }
    NotificationStore.prototype.checkIfStoreExists = function () {
        if (!localStorage.getItem(questionNotificationStoreName)) {
            localStorage.setItem(questionNotificationStoreName, JSON.stringify([]));
        }
    };
    NotificationStore.prototype.getStore = function () {
        return JSON.parse(localStorage.getItem(questionNotificationStoreName)) || [];
    };
    NotificationStore.prototype.setStore = function (_store) {
        localStorage.setItem(questionNotificationStoreName, JSON.stringify(_store));
    };
    Object.defineProperty(NotificationStore.prototype, "count", {
        get: function () {
            return this.getStore().length;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationStore.prototype, "notificationDisplayMessage", {
        get: function () {
            return "\u0644\u062F\u064A\u0643  " + this.count + " \u0627\u0634\u0639\u0627\u0631\u0627\u062A";
        },
        enumerable: true,
        configurable: true
    });
    NotificationStore.prototype.getAll = function () {
        return this.getStore();
    };
    NotificationStore.prototype.getById = function (id) {
        return this.getAll().find(function (e) { return e.id == id; });
    };
    NotificationStore.prototype.add = function (q) {
        var store = this.getStore();
        q.viewed = false;
        store.push(q);
        this.setStore(store);
        return store;
    };
    NotificationStore.prototype.setQuesList = function (qArr) {
        this.setStore(qArr);
        return qArr;
    };
    NotificationStore.prototype.removeAll = function () {
        this.setStore([]);
    };
    NotificationStore.prototype.remove = function (qId) {
        var store = this.getStore();
        store = store.filter(function (q) { return q.id != qId; });
        this.setStore(store);
        return store;
    };
    NotificationStore.prototype.setAsSeen = function (qId) {
        var store = this.getStore();
        var ind = store.findIndex(function (q) { return q.id == qId; });
        if (ind > -1) {
            store[ind].viewed = true;
            this.setStore(store);
        }
        return store;
    };
    return NotificationStore;
}());
var techHubOperations = {
    notifStore: new NotificationStore(),
    setStore: function (_notifStore) {
        this.notifStore = _notifStore;
    },
    connection: {},
    getToken: function () {
        return localStorage.getItem('token');
    },
    hubUrl: "/hub/techsupport",
    startConnection: function () {
        this.connection.start().catch(function (err) { return document.write(err); });
    },
    onreconnected: function (connectionId) {
        console.log('reconnect on connection id =' + connectionId);
    },
    onCustomerAddQuestion: function (quesData) {
        var questions = this.notifStore.add(quesData);
        NotificationDom.setNotifications(questions);
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            TechSupportPageDom.quesCRUD.addNewQuestion(quesData);
        }
    },
    onGetNotSeenQuestions: function (data) {
        console.log('list of not seen questions');
        TechSupportPageDom.reDisplayMessages(data);
    },
    refreshNotifications: function () {
        this.setStore(new NotificationStore());
        var AllQues = this.notifStore.getAll().filter(function (e) { return !e.viewed; });
        this.notifStore.setQuesList(AllQues);
        NotificationDom.setNotifications(AllQues);
    },
    init: function () {
        this.refreshNotifications();
        var token = this.getToken();
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl, { accessTokenFactory: function () { return token; } })
            .build();
        this.onreconnected = this.connection.onreconnected;
        this.connection.start();
        this.connection.on("onQuestionAdded", this.onCustomerAddQuestion.bind(this));
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            TechSupportPageDom.reDisplayMessages([]);
            this.connection.on("onGetNotSeenQuestions", this.onGetNotSeenQuestions.bind(this));
        }
    }
};
var NotificationDom = {
    quesCRUD: null,
    get notifEl() { return $('#techNotification'); },
    clonedNotif: $('#cloned-notif'),
    storeSelectedNotificationAndRefresh: function (q) {
        localStorage.setItem(SelectedNotifyquestion, JSON.stringify(q));
        location.href = utilities_1.default.Urls.adminTechSupportUrl;
    },
    notifTitleCountEl: $('#techNotification .title-count'),
    notifTitleEl: $('#techNotification .app-notification__title'),
    notifNumber: $('#techNotification .notif-number:eq(0)'),
    notifContent: $('#techNotification .notif-content'),
    notifBodyContainer: $('#techNotification .app-notification__content'),
    reDisplayNotificationCount: function (count) {
        if (count < 1) {
            this.notifNumber.hide();
            this.notifTitleEl.text('ليس لديك اشعارات');
        }
        else {
            this.notifNumber.text(count).show();
            this.notifTitleCountEl.text(count);
        }
    },
    generateNewNotifEl: function () {
        var el = this.clonedNotif.clone(true);
        el.removeClass('d-none').removeAttr('id').removeProp('id');
        return el;
    },
    reDisplayNotificationContent: function (notifArr) {
        var _this = this;
        var container = $();
        var elements = [];
        notifArr.forEach(function (notif) {
            var notifEl = _this.generateNewNotifEl();
            notifEl.data('id', notif.id);
            if (notif.viewed) {
                notifEl.addClass('seen');
            }
            notifEl.find('p:eq(0)').text(notif.message);
            notifEl.find('p:eq(1)').text(utilities_1.default.getDateObj(notif.createdAt).toLocaleString());
            elements.unshift(notifEl);
        });
        var dom = elements.forEach(function (e) {
            container = container.add(e);
        });
        this.notifBodyContainer.empty().append(container);
    },
    setNotifications: function (notifArr) {
        if (notifArr) {
            this.reDisplayNotificationCount(notifArr.filter(function (n) { return !n.viewed; }).length || 0);
            this.reDisplayNotificationContent(notifArr);
        }
    },
    onSelectedQuesMessage: function (id) {
        var qs = techHubOperations.notifStore.setAsSeen(id);
        qs = qs.filter(function (e) { return e.id != id; });
        this.setNotifications(qs);
    },
    handleOnNotifElClicked: function () {
        var $this = this;
        $(document.body).on('click', '.app-notification_li', function () {
            var $el = $(this);
            var id = $el.data('id');
            var qes = techHubOperations.notifStore.setAsSeen(id);
            var q = techHubOperations.notifStore.getById(id);
            $this.setNotifications(qes);
            if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
                $this.quesCRUD.setSelectedQuestion(q);
            }
            else {
                $this.storeSelectedNotificationAndRefresh(q);
            }
        });
    },
    init: function () {
        this.handleOnNotifElClicked();
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
        }
    }
};
var TechSupportPageDom = {
    quesCRUD: null,
    storeCurrentSelectedMessage: function (q) {
        localStorage.setItem(SelectedNotifyquestion, JSON.stringify(q));
    },
    get clonedResponseMessageContaner() { return $('#cloned-reponse-message'); },
    get messageResponsesEl() { return $('#responses'); },
    get submitResponseBtn() { return $('#sendResponseBtn'); },
    get faildToSendResponseBtn() { return $('#faildToSendResponseBtn'); },
    get messageTextInpEl() { return $('#responseTextBox'); },
    _lastAppendedTextEl: null,
    _generateResponseMessageContainer: function (res) {
        var el = this.clonedResponseMessageContaner.find('.response-message-container:eq(0)').clone();
        el.find('.response-text').text(res.message);
        return el;
    },
    get messageCard() { return $(this.messageCardselector); },
    getStoredSelectedNotifiedQuestionIfExsists: function () {
        var qstr = localStorage.getItem(SelectedNotifyquestion);
        var q = qstr ? JSON.parse(qstr) : null;
        //if (q) localStorage.removeItem(SelectedNotifyquestion);
        return q;
    },
    get messageCardselector() { return ".message-card"; },
    get loadingOgverlay() { return $('.ContainerOverlay'); },
    stopLoading: function () {
        this.loadingOgverlay.hide();
    },
    startLoading: function () { this.loadingOgverlay.show(); },
    get checkIfCurrentPageIsTechSupport() {
        return this.messageCardColumnContainerEl.length > 0;
    },
    userTypeText: function (type) {
        if (type == EUserType.Pharmacy)
            return "ص";
        if (type == EUserType.Stock)
            return "م";
        return "";
    },
    fullUserTypeText: function (type) {
        if (type == EUserType.Pharmacy)
            return "صيدلية";
        if (type == EUserType.Stock)
            return "مخزن";
        return "";
    },
    get clonedMessageCard() { return $('#cloned-message-card'); },
    get selectedMessageWrapperEl() { return $('.message-chatting-column:eq(0)'); },
    get selectedMessageEl() { return $('.message-chatting-column .sender-message:eq(0)'); },
    _redisplaySelectedMessageEl: function (q, isNewResponse) {
        if (isNewResponse === void 0) { isNewResponse = false; }
        if (!q) {
            this.selectedMessageWrapperEl.hide();
        }
        else {
            this.selectedMessageWrapperEl.show();
            this.selectedMessageEl.find('.sender-type').text(this.fullUserTypeText(q.userType));
            this.selectedMessageEl.find('.sender-name').text(q.senderName);
            this.selectedMessageEl.find('.sender-name').text(q.senderName);
            this.selectedMessageEl.find('.sender-address').text(q.senderAddress);
            this.selectedMessageEl.find('.created-at').text(utilities_1.default.getDateObj(q.createdAt).toLocaleString());
            this.selectedMessageEl.find('.message-text').text(q.message);
            var responsesContainer = $();
            if (q.responses && q.responses.length > 0) {
                for (var k = q.responses.length - 1; k >= 0; k--) {
                    var _element = this._generateResponseMessageContainer(q.responses[k]);
                    if (isNewResponse && k == q.responses.length - 1) {
                        this._lastAppendedTextEl = _element;
                    }
                    responsesContainer = responsesContainer.add(_element);
                }
                this.messageResponsesEl.empty().append(responsesContainer);
            }
            else {
                this.messageResponsesEl.empty();
            }
        }
    },
    _generateNewMessageCard: function (dataModel) {
        var _newCard = this.clonedMessageCard.find('.cloned-card:eq(0)').clone(true);
        _newCard.find(this.messageCardselector).data('id', dataModel.id);
        _newCard.find('.message-content .message').text(dataModel.message);
        _newCard.find('.customer-title-suffex').text(this.userTypeText(dataModel.userType));
        _newCard.find('.customer-title-text').text(dataModel.senderName);
        _newCard.find('.customer-address').text(dataModel.senderAddress);
        _newCard.find('.message-time').text(utilities_1.default.getDateObj(dataModel.createdAt).toLocaleString());
        return _newCard;
    },
    _getNoMessagesDisplay: function () {
        var el = this.clonedMessageCard.find('.cloned-no-message:eq(0)').clone(true);
        return el;
    },
    onGetNewQuestion: function (q) {
        var el = this._generateNewMessageCard(q);
        this.messageCardColumnContainerEl.prepend(el);
    },
    get messageCardColumnContainerEl() {
        return $('.message_card-column:eq(0)');
    },
    reDisplayMessages: function (dataArr) {
        var _this = this;
        if (!dataArr || dataArr.length == 0) {
            this.messageCardColumnContainerEl.empty().append(this._getNoMessagesDisplay());
            return;
        }
        var conatiner = $();
        dataArr.forEach(function (e) {
            conatiner = conatiner.add(_this._generateNewMessageCard(e));
        });
        this.messageCardColumnContainerEl.empty().append(conatiner);
    },
    setSelectedQuestion: function (q) {
        this.storeCurrentSelectedMessage(q);
        this._redisplaySelectedMessageEl(q);
    },
    handleOnMessageCardClick: function () {
        var $context = this;
        $(document.body).on('click', this.messageCardselector, function () {
            var $this = $(this);
            var selectedId = $this.data('id');
            $context.quesCRUD.setSelectedQuestionById(selectedId);
            NotificationDom.onSelectedQuesMessage(selectedId);
        });
        var selectedQuesAtInit = this.getStoredSelectedNotifiedQuestionIfExsists();
        if (selectedQuesAtInit) {
            $context.quesCRUD.setSelectedQuestion(selectedQuesAtInit);
        }
    },
    onQuesRespondedOn: function (q) {
        this.storeCurrentSelectedMessage(q);
        this._redisplaySelectedMessageEl(q, true);
    },
    removeFaildResponseError: function () {
        this._lastAppendedTextEl.removeClass('error');
    },
    onFailToSendRespons: function () {
        this._lastAppendedTextEl.addClass('error');
    },
    handleOnSubmitResponse: function () {
        var _this = this;
        this.submitResponseBtn.on('click', function () {
            var val = _this.messageTextInpEl.val();
            val ? val.trim() : null;
            if (val) {
                _this.quesCRUD.respondOnQuestion(val);
                _this.messageTextInpEl.val('');
            }
        });
    },
    onSuccessToSendFaildMessage: function () {
        this.removeFaildResponseError();
    },
    handleOntryToSendFaildMessageAgain: function () {
        var _this = this;
        $(document.body).on('click', '#faildToSendResponseBtn', function () {
            _this.quesCRUD.sendFaildResponseAgain();
        });
    },
    init: function (crud) {
        this.quesCRUD = crud;
        this.handleOnMessageCardClick();
        this.handleOnSubmitResponse();
        this.handleOntryToSendFaildMessageAgain();
        this.quesCRUD.subscribe(this);
    },
};
var QuestionsCRUD = /** @class */ (function () {
    function QuestionsCRUD() {
        this._questions = [];
        this._obj = {};
        this._subscripers = [];
    }
    QuestionsCRUD.prototype.publishDataList = function () {
        var _this = this;
        this._subscripers.forEach(function (s) {
            if (s.onGetDataList)
                s.onGetDataList(_this._questions.slice());
        });
    };
    QuestionsCRUD.Create = function () {
        if (!this.prototype._obj) {
            this.prototype._obj = new QuestionsCRUD();
        }
        return this.prototype._obj;
    };
    QuestionsCRUD.prototype.getCustomerQuestions = function () {
        var _this = this;
        TechSupportPageDom.startLoading();
        $.get(utilities_1.default.Urls.techSupportUrl + "/notResponded")
            .done(function (data) {
            _this._questions = data;
            _this.publishDataList();
            setTimeout(function () {
            }, 3000);
        }).fail(function (err) {
            alert('cannot get data');
        }).always(function (e) {
            TechSupportPageDom.stopLoading();
        });
    };
    QuestionsCRUD.prototype.subscribe = function (_subscriber) {
        this._subscripers.push(_subscriber);
    };
    QuestionsCRUD.prototype._setSelectedQuestion = function (q) {
        this.selectedQues = q;
        this._subscripers.forEach(function (s) {
            if (s.onQuesSelected)
                s.onQuesSelected(q);
        });
        if (q && q.id) {
            this.markAsSeen();
        }
    };
    QuestionsCRUD.prototype.setSelectedQuestion = function (q) {
        this._setSelectedQuestion(q);
    };
    QuestionsCRUD.prototype.setSelectedQuestionById = function (id) {
        var q = this._questions.find(function (e) { return e.id == id; });
        this._setSelectedQuestion(q);
    };
    QuestionsCRUD.prototype._markAsSeen = function () {
        var _this = this;
        this.selectedQues.seenAt = new Date().toDateString();
        var qInd = this._questions.findIndex(function (e) { return e.id == _this.selectedQues.id; });
        this._questions[qInd] = this.selectedQues;
    };
    QuestionsCRUD.prototype.markAsSeen = function () {
        var _this = this;
        if (this.selectedQues.seenAt)
            return;
        var setting = {
            url: utilities_1.default.Urls.techSupportUrl + "/" + this.selectedQues.id,
            method: 'PUT'
        };
        $.ajax(setting)
            .done(function () {
            _this._markAsSeen();
        })
            .fail(function () {
            console.log('faild to mark messsage as seen');
        })
            .always(function () {
        });
    };
    QuestionsCRUD.prototype._responseOnQuestion = function (id, response) {
        var _this = this;
        var qInd = this._questions.findIndex(function (e) { return e.id == _this.selectedQues.id; });
        if (!this.selectedQues.responses)
            this.selectedQues.responses = [];
        this.selectedQues.responses.push({
            id: id,
            createdAt: new Date().toDateString(),
            message: response,
            relatedToId: this.selectedQues.id,
            seenAt: null
        });
        this._questions[qInd] = this.selectedQues;
        this._subscripers.forEach(function (s) {
            if (s.onQuesRespondedOn)
                s.onQuesRespondedOn(_this.selectedQues);
        });
    };
    QuestionsCRUD.prototype._onSuccessToSendFaildResponse = function () {
        this._subscripers.forEach(function (s) {
            if (s.onSuccessToSendFaildMessage)
                s.onSuccessToSendFaildMessage();
        });
    };
    QuestionsCRUD.prototype._failToSendResponse = function () {
        this._subscripers.forEach(function (s) {
            if (s.onFailToSendRespons)
                s.onFailToSendRespons();
        });
    };
    QuestionsCRUD.prototype.respondOnQuestion = function (response) {
        var _this = this;
        if (!this.selectedQues)
            return;
        this.lastRespondedMessage = response;
        var data = {
            response: response,
            relatedToId: this.selectedQues.id
        };
        this._responseOnQuestion(null, response);
        $.post("" + utilities_1.default.Urls.techSupportUrl, JSON.stringify(data))
            .fail(function (err) {
            _this._failToSendResponse();
        });
    };
    QuestionsCRUD.prototype.sendFaildResponseAgain = function () {
        var _this = this;
        if (!this.selectedQues)
            return;
        var data = {
            response: this.lastRespondedMessage,
            relatedToId: this.selectedQues.id
        };
        this._onSuccessToSendFaildResponse();
        $.post("" + utilities_1.default.Urls.techSupportUrl, data)
            .fail(function (err) {
            _this._failToSendResponse();
        });
    };
    QuestionsCRUD.prototype.addNewQuestion = function (q) {
        this._questions.unshift(q);
        this._subscripers.forEach(function (s) {
            if (s.onGetNewQuestion)
                s.onGetNewQuestion(q);
        });
    };
    return QuestionsCRUD;
}());
var TechSupportSquestionManager = {
    quesCRUD: null,
    onGetDataList: function (data) {
        TechSupportPageDom.reDisplayMessages(data);
    },
    handleOnGetData: function () {
        this.quesCRUD.subscribe(this);
    },
    onQuesSelected: function (q) {
        TechSupportPageDom.setSelectedQuestion(q);
    },
    start: function () {
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
            this.quesCRUD.getCustomerQuestions();
            this.handleOnGetData();
            TechSupportPageDom.init(this.quesCRUD);
        }
    }
}.start();
var notificationManager = {
    startHandle: function () {
        NotificationDom.init();
        setTimeout(function () {
            techHubOperations.init();
        }, 0);
    }
}.startHandle();
window['techDom'] = TechSupportPageDom;
window['notifDom'] = NotificationDom;
//# sourceMappingURL=techSupportMessaging.js.map