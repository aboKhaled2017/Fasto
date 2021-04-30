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
        }
        this.setStore(store);
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
        console.log(quesData);
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
        var _this = this;
        this.refreshNotifications();
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl, { accessTokenFactory: function () { return _this.getToken(); } })
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
    get notifEl() { return $('#techNotification'); },
    clonedNotif: $('#cloned-notif'),
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
        notifArr.forEach(function (notif) {
            var notifEl = _this.generateNewNotifEl();
            notifEl.data('id', notif.id);
            if (notif.viewed) {
                notifEl.addClass('seen');
            }
            notifEl.find('p:eq(0)').text(notif.message);
            notifEl.find('p:eq(1)').text(utilities_1.default.getDateObj(notif.createdAt).toLocaleString());
            container = container.add(notifEl);
        });
        this.notifBodyContainer.empty().append(container);
    },
    setNotifications: function (notifArr) {
        if (notifArr) {
            this.reDisplayNotificationCount(notifArr.filter(function (n) { return !n.viewed; }).length || 0);
            this.reDisplayNotificationContent(notifArr);
        }
    },
    handleOnNotifElClicked: function () {
        var $this = this;
        $(document.body).on('click', '.app-notification_li', function () {
            console.log('clicked');
            var $el = $(this);
            var id = $el.data('id');
            console.log('id is ' + id);
            var qes = techHubOperations.notifStore.setAsSeen(id);
            $this.setNotifications(qes);
        });
    },
    init: function () {
        this.handleOnNotifElClicked();
    }
};
var TechSupportPageDom = {
    get checkIfCurrentPageIsTechSupport() {
        return !!this.messageCardColumnContainerEl;
    },
    userTypeText: function (type) {
        if (type == EUserType.Pharmacy)
            return "ص";
        if (type == EUserType.Stock)
            return "م";
        return "";
    },
    get clonedMessageCard() { return $('#cloned-message-card'); },
    _generateNewMessageCard: function (dataModel) {
        var _newCard = this.clonedMessageCard.find('.cloned-card:eq(0)').clone(true);
        _newCard.find('.message-content .message').text(dataModel.message);
        _newCard.find('.customer-title-suffex').text(this.userTypeText(dataModel.userType));
        _newCard.find('.customer-title-text').text(dataModel.SenderName);
        _newCard.find('.customer-address').text(dataModel.SenderAddress);
        _newCard.find('.message-time').text(utilities_1.default.getDateObj(dataModel.createdAt).toLocaleString());
        return _newCard;
    },
    _getNoMessagesDisplay: function () {
        var el = this.clonedMessageCard.find('.cloned-no-message:eq(0)').clone(true);
        return el;
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
    }
};
var QuestionsCRUD = /** @class */ (function () {
    function QuestionsCRUD() {
        this._questions = [];
        this._subscripers = [];
    }
    QuestionsCRUD.prototype.publishDataList = function () {
        var _this = this;
        this._subscripers.forEach(function (s) {
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
        $.get(utilities_1.default.Urls.techSupportUrl + "/notResponded")
            .done(function (data) {
            _this._questions = data;
            _this.publishDataList();
            setTimeout(function () {
            }, 3000);
        }).fail(function (err) {
            alert('cannot get data');
        }).always(function (e) {
        });
    };
    QuestionsCRUD.prototype.subscribe = function (_subscriber) {
        this._subscripers.push(_subscriber);
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
    start: function () {
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
            this.quesCRUD.getCustomerQuestions();
            this.handleOnGetData();
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
//# sourceMappingURL=techSupportMessaging.js.map