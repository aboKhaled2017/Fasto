
import * as signalR from "@microsoft/signalr";
import helper from "./utilities";
enum EUserType {
    Pharmacy = 1,
    Stock = 2,
    Admin = 3
}
interface questionModel {
    createdAt: string
    id: string
    message: string
    relatedTo: string
    relatedToId: string
    seenAt:string
    senderId: string
    userType: EUserType
    viewed: boolean
}
interface questionDetailModel {
    createdAt: string
    id: string
    SenderName: string
    SenderAddress:string
    message: string
    relatedToId: string
    seenAt: string
    senderId: string
    userType: EUserType
    viewed: boolean
}
interface ISubscriber {
    onGetDataList: (data: questionDetailModel[]) => void;
}

const questionNotificationStoreName = "quesNotifications";
class NotificationStore {

    constructor() {
        this.checkIfStoreExists();
    }
    private checkIfStoreExists() {
        if (!localStorage.getItem(questionNotificationStoreName)) {
            localStorage.setItem(questionNotificationStoreName, JSON.stringify([]));
        }
    }
    private getStore() {
        return JSON.parse(localStorage.getItem(questionNotificationStoreName)) as questionModel[] || [];
    }
    private setStore(_store: questionModel[]) {
        localStorage.setItem(questionNotificationStoreName, JSON.stringify(_store));
    }
    get count() {
        return this.getStore().length;
    }
    get notificationDisplayMessage() {

        return `لديك  ${this.count} اشعارات`;
    }
    getAll() {
        return this.getStore();
    }
    add(q: questionModel) {
        let store = this.getStore();
        q.viewed = false;
        store.push(q);
        this.setStore(store);
        return store;
    }
    setQuesList(qArr: questionModel[]) {     
        this.setStore(qArr);
        return qArr;
    }
    removeAll() {
        this.setStore([]);
    }
    remove(qId: string) {
        let store = this.getStore();
        store = store.filter(q => q.id != qId);
        this.setStore(store);
        return store;
    }
    setAsSeen(qId: string) {
        let store = this.getStore();
        let ind = store.findIndex(q => q.id == qId);
        if (ind > -1) {
            store[ind].viewed = true;
        }
        this.setStore(store);
        return store;
    }
}

const techHubOperations = {
    notifStore: new NotificationStore() as NotificationStore,
    setStore(_notifStore: NotificationStore) {
        this.notifStore = _notifStore;
    },
    connection: {} as signalR.HubConnection,
    getToken() {
        return localStorage.getItem('token');
    },
    hubUrl: "/hub/techsupport",
    startConnection() {
        this.connection.start().catch(err => document.write(err));
    },
    onreconnected(connectionId:string) {
        console.log('reconnect on connection id =' + connectionId)
    },
    onCustomerAddQuestion(quesData: questionModel) {
        let questions = this.notifStore.add(quesData);
        NotificationDom.setNotifications(questions);
        console.log(quesData);
    },
    onGetNotSeenQuestions(data: questionDetailModel[]) {
        console.log('list of not seen questions')
        TechSupportPageDom.reDisplayMessages(data);
    },
    refreshNotifications() {
        this.setStore(new NotificationStore());
        let AllQues = this.notifStore.getAll().filter(e => !e.viewed);
        this.notifStore.setQuesList(AllQues);
        NotificationDom.setNotifications(AllQues);
    },
    init() {
        this.refreshNotifications();
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl, { accessTokenFactory: () => this.getToken() })
            .build();
        this.onreconnected = this.connection.onreconnected;
        this.connection.start();
        this.connection.on("onQuestionAdded", this.onCustomerAddQuestion.bind(this));
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            TechSupportPageDom.reDisplayMessages([]);
            this.connection.on("onGetNotSeenQuestions", this.onGetNotSeenQuestions.bind(this));
        }
    }
}

const NotificationDom = {
    get notifEl() { return $('#techNotification');},
    clonedNotif: $('#cloned-notif'),
    notifTitleCountEl: $('#techNotification .title-count'),
    notifTitleEl: $('#techNotification .app-notification__title'),
    notifNumber: $('#techNotification .notif-number:eq(0)'),
    notifContent: $('#techNotification .notif-content'),
    notifBodyContainer: $('#techNotification .app-notification__content'),
    reDisplayNotificationCount(count:number) {
        if (count < 1) {
            this.notifNumber.hide();
            this.notifTitleEl.text('ليس لديك اشعارات');
        }
        else {
            this.notifNumber.text(count).show();
            this.notifTitleCountEl.text(count);
           
        }
    },
    generateNewNotifEl() {
        let el = this.clonedNotif.clone(true);
        el.removeClass('d-none').removeAttr('id').removeProp('id');
        return el;
    },
    reDisplayNotificationContent(notifArr: questionModel[]) {
        let container = $();
        notifArr.forEach(notif => {
            let notifEl = this.generateNewNotifEl();
            notifEl.data('id', notif.id);
            if (notif.viewed) {
                notifEl.addClass('seen');
            }
            notifEl.find('p:eq(0)').text(notif.message);
            notifEl.find('p:eq(1)').text(helper.getDateObj(notif.createdAt).toLocaleString());
            container=container.add(notifEl);
        });
        this.notifBodyContainer.empty().append(container);
    },
    setNotifications(notifArr: questionModel[]) {
        if (notifArr) {
            this.reDisplayNotificationCount(notifArr.filter(n => !n.viewed).length || 0);
            this.reDisplayNotificationContent(notifArr);
        }
    },
    handleOnNotifElClicked() {
        let $this = this;
        $(document.body).on('click','.app-notification_li', function () {
            console.log('clicked');
            const $el = $(this);
            let id = $el.data('id');
            console.log('id is ' + id);
            let qes = techHubOperations.notifStore.setAsSeen(id);
            $this.setNotifications(qes);
        });
    },
    init() {
        this.handleOnNotifElClicked();
    }
}

const TechSupportPageDom = {
    get checkIfCurrentPageIsTechSupport() {
        return !!this.messageCardColumnContainerEl;
    },
    userTypeText(type: EUserType) {
        if (type == EUserType.Pharmacy) return "ص";
        if (type == EUserType.Stock) return "م";
        return "";
    },
    get clonedMessageCard() { return $('#cloned-message-card') },
    _generateNewMessageCard(dataModel: questionDetailModel) {
        let _newCard = this.clonedMessageCard.find('.cloned-card:eq(0)').clone(true);
        _newCard.find('.message-content .message').text(dataModel.message);
        _newCard.find('.customer-title-suffex').text(this.userTypeText(dataModel.userType));
        _newCard.find('.customer-title-text').text(dataModel.SenderName);
        _newCard.find('.customer-address').text(dataModel.SenderAddress);
        _newCard.find('.message-time').text(helper.getDateObj(dataModel.createdAt).toLocaleString());
        return _newCard;
    },
    _getNoMessagesDisplay() {
        var el = this.clonedMessageCard.find('.cloned-no-message:eq(0)').clone(true);
        return el;
    },
    get messageCardColumnContainerEl() {
        return $('.message_card-column:eq(0)');
    },
    reDisplayMessages(dataArr: questionDetailModel[]) {
        if (!dataArr || dataArr.length == 0) {
            this.messageCardColumnContainerEl.empty().append(this._getNoMessagesDisplay());
            return;
        }
        let conatiner = $();
        dataArr.forEach(e => {
            conatiner = conatiner.add(this._generateNewMessageCard(e));
        });
        this.messageCardColumnContainerEl.empty().append(conatiner);
    }
}

class QuestionsCRUD  {
    private _questions: questionDetailModel[] = [];
    private _obj: QuestionsCRUD;
    private _subscripers: ISubscriber[] = [];
    private publishDataList() {
        this._subscripers.forEach(s => {
            s.onGetDataList(this._questions.slice());
        });
    }
    private constructor() { }
    static Create() {
        if (!this.prototype._obj) {
            this.prototype._obj = new QuestionsCRUD();
        }
        return this.prototype._obj;
    }
    getCustomerQuestions() {
        $.get(`${helper.Urls.techSupportUrl}/notResponded`)
            .done(data => {
                this._questions = data;
                this.publishDataList();
                setTimeout(() => {
                   
                }, 3000);
            }).fail(err => {
                alert('cannot get data')
            }).always(e => {

            });
    }
    subscribe(_subscriber: ISubscriber) {
        this._subscripers.push(_subscriber);
    }
}

const TechSupportSquestionManager = {
    quesCRUD: null as QuestionsCRUD,
    onGetDataList(data: questionDetailModel[]) {
        TechSupportPageDom.reDisplayMessages(data);
    },
    handleOnGetData() {
        this.quesCRUD.subscribe(this);
    },
    start() {
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
            this.quesCRUD.getCustomerQuestions();
            this.handleOnGetData();
        }
    }
}.start();

const notificationManager = {
    startHandle() {
        NotificationDom.init(); 
        setTimeout(() => {
            techHubOperations.init();
        }, 0);
    }
}.startHandle();
