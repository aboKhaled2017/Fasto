
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
interface adminResponsModel {
    id: string 
    createdAt: string 
    message: string
    seenAt: string
    relatedToId: string
}
interface questionDetailModel {
    createdAt: string
    id: string
    senderName: string
    senderAddress:string
    message: string
    seenAt: string
    senderId: string
    userType: EUserType
    viewed: boolean
    responses: adminResponsModel[]
}
interface ISubscriber {
    onGetDataList: (data: questionDetailModel[]) => void;
    onQuesSelected: (q: questionDetailModel) => void
    onQuesRespondedOn: (q: questionDetailModel) => void
    onFailToSendRespons: () => void
    onSuccessToSendFaildMessage: () => void
    onGetNewQuestion: (q: questionDetailModel)=>void
}

const questionNotificationStoreName = "quesNotifications";
const SelectedNotifyquestion = "selectedNotifQues";
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
        return JSON.parse(localStorage.getItem(questionNotificationStoreName)) as questionDetailModel[] || [];
    }
    private setStore(_store: questionDetailModel[]) {
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
    getById(id: string) {
        return this.getAll().find(e => e.id == id);
    }
    add(q: questionDetailModel) {
        let store = this.getStore();
        q.viewed = false;
        store.push(q);
        this.setStore(store);
        return store;
    }
    setQuesList(qArr: questionDetailModel[]) {     
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
            this.setStore(store);          
        }  
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
        this.connection.start().catch((err:any) => document.write(err));
    },
    onreconnected(connectionId?: string | undefined) {
        console.log('reconnect on connection id =' + connectionId)
    },
    onCustomerAddQuestion(quesData: questionDetailModel) {
        let questions = this.notifStore.add(quesData);
        NotificationDom.setNotifications(questions);
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            TechSupportPageDom.quesCRUD.addNewQuestion(quesData);
        }
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
        let token = this.getToken() as string;
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl, { accessTokenFactory: () => token })
            .build();
        this.onreconnected = this.connection.onreconnected as any;
        this.connection.start();
        this.connection.on("onQuestionAdded", this.onCustomerAddQuestion.bind(this));
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            TechSupportPageDom.reDisplayMessages([]);
            this.connection.on("onGetNotSeenQuestions", this.onGetNotSeenQuestions.bind(this));
        }
    }
}

const NotificationDom = {
    quesCRUD: null as QuestionsCRUD,
    get notifEl() { return $('#techNotification');},
    clonedNotif: $('#cloned-notif'),
    storeSelectedNotificationAndRefresh(q: questionDetailModel) {
        localStorage.setItem(SelectedNotifyquestion, JSON.stringify(q));
        location.href = helper.Urls.adminTechSupportUrl;
    },
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
    reDisplayNotificationContent(notifArr: questionDetailModel[]) {
        let container = $();
        let elements = [];
        notifArr.forEach(notif => {
            let notifEl = this.generateNewNotifEl();
            notifEl.data('id', notif.id);
            if (notif.viewed) {
                notifEl.addClass('seen');
            }
            notifEl.find('p:eq(0)').text(notif.message);
            notifEl.find('p:eq(1)').text(helper.getDateObj(notif.createdAt).toLocaleString());
            elements.unshift(notifEl);           
        });
  
        var dom = elements.forEach(e => {
            container = container.add(e);
        })
        this.notifBodyContainer.empty().append(container);
    },
    setNotifications(notifArr: questionDetailModel[]) {
        if (notifArr) {
            this.reDisplayNotificationCount(notifArr.filter(n => !n.viewed).length || 0);
            this.reDisplayNotificationContent(notifArr);
        }
    },
    onSelectedQuesMessage(id: string) {
        let qs = techHubOperations.notifStore.setAsSeen(id);
        qs = qs.filter(e => e.id != id);
        this.setNotifications(qs);
    },
    handleOnNotifElClicked() {
        let $this = this;
        $(document.body).on('click','.app-notification_li', function () {         
            const $el = $(this);
            let id = $el.data('id');
            let qes = techHubOperations.notifStore.setAsSeen(id);
            let q = techHubOperations.notifStore.getById(id);
            $this.setNotifications(qes);
            
            if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
                $this.quesCRUD.setSelectedQuestion(q);
            }
            else {
                $this.storeSelectedNotificationAndRefresh(q);
            }
        });
    },
    init() {
        this.handleOnNotifElClicked();
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
        }
    }
}

const TechSupportPageDom = {
    quesCRUD: null as QuestionsCRUD,
    storeCurrentSelectedMessage(q: questionDetailModel) {
        localStorage.setItem(SelectedNotifyquestion, JSON.stringify(q));
    },
    get clonedResponseMessageContaner() { return $('#cloned-reponse-message'); },
    get messageResponsesEl() { return $('#responses'); },
    get submitResponseBtn() { return $('#sendResponseBtn'); },
    get faildToSendResponseBtn() { return $('#faildToSendResponseBtn');},
    get messageTextInpEl() { return $('#responseTextBox') },
    _lastAppendedTextEl: null as JQuery,
    _generateResponseMessageContainer(res: adminResponsModel) {
        let el = this.clonedResponseMessageContaner.find('.response-message-container:eq(0)').clone();
        el.find('.response-text').text(res.message);
        return el;
    },
    get messageCard() { return $(this.messageCardselector); },
    getStoredSelectedNotifiedQuestionIfExsists() {
        let qstr = localStorage.getItem(SelectedNotifyquestion);
        let q = qstr ? JSON.parse(qstr) as questionDetailModel : null;
        //if (q) localStorage.removeItem(SelectedNotifyquestion);
        return q;
    },
    get messageCardselector() { return ".message-card";},
    get loadingOgverlay() { return $('.ContainerOverlay'); },
    stopLoading() {
        this.loadingOgverlay.hide();
    },
    startLoading() { this.loadingOgverlay.show();},
    get checkIfCurrentPageIsTechSupport() {
        return this.messageCardColumnContainerEl.length>0;
    },
    userTypeText(type: EUserType) {
        if (type == EUserType.Pharmacy) return "ص";
        if (type == EUserType.Stock) return "م";
        return "";
    },
    fullUserTypeText(type: EUserType) {
        if (type == EUserType.Pharmacy) return "صيدلية";
        if (type == EUserType.Stock) return "مخزن";
        return "";
    },
    get clonedMessageCard() { return $('#cloned-message-card') },
    get selectedMessageWrapperEl() { return $('.message-chatting-column:eq(0)');},
    get selectedMessageEl() { return $('.message-chatting-column .sender-message:eq(0)'); },
    _redisplaySelectedMessageEl(q: questionDetailModel, isNewResponse: boolean=false) {
        if (!q) {
            this.selectedMessageWrapperEl.hide();
        }
        else {
            this.selectedMessageWrapperEl.show();
            this.selectedMessageEl.find('.sender-type').text(this.fullUserTypeText(q.userType));
            this.selectedMessageEl.find('.sender-name').text(q.senderName);
            this.selectedMessageEl.find('.sender-name').text(q.senderName);
            this.selectedMessageEl.find('.sender-address').text(q.senderAddress);
            this.selectedMessageEl.find('.created-at').text(helper.getDateObj(q.createdAt).toLocaleString());
            this.selectedMessageEl.find('.message-text').text(q.message);
            let responsesContainer = $();
            if (q.responses && q.responses.length > 0) {
               
                for (let k = q.responses.length - 1; k >= 0; k--) {
                    let _element = this._generateResponseMessageContainer(q.responses[k]);
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
    _generateNewMessageCard(dataModel: questionDetailModel) {
        
        let _newCard = this.clonedMessageCard.find('.cloned-card:eq(0)').clone(true);
        _newCard.find(this.messageCardselector).data('id', dataModel.id);
        _newCard.find('.message-content .message').text(dataModel.message);
        _newCard.find('.customer-title-suffex').text(this.userTypeText(dataModel.userType));
        _newCard.find('.customer-title-text').text(dataModel.senderName);
        _newCard.find('.customer-address').text(dataModel.senderAddress);
        _newCard.find('.message-time').text(helper.getDateObj(dataModel.createdAt).toLocaleString());
        return _newCard;
    },
    _getNoMessagesDisplay() {
        var el = this.clonedMessageCard.find('.cloned-no-message:eq(0)').clone(true);
        return el;
    },
    onGetNewQuestion(q: questionDetailModel) {
        const el = this._generateNewMessageCard(q);
        this.messageCardColumnContainerEl.prepend(el);
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
    },
    setSelectedQuestion(q: questionDetailModel) {
        this.storeCurrentSelectedMessage(q);
        this._redisplaySelectedMessageEl(q);
    },
    handleOnMessageCardClick() {
        const $context = this;
        $(document.body).on('click', this.messageCardselector, function () {
            let $this = $(this);
            let selectedId = $this.data('id');
            $context.quesCRUD.setSelectedQuestionById(selectedId);
            NotificationDom.onSelectedQuesMessage(selectedId);
        });
        let selectedQuesAtInit = this.getStoredSelectedNotifiedQuestionIfExsists();
        if (selectedQuesAtInit) {
            $context.quesCRUD.setSelectedQuestion(selectedQuesAtInit);
        }
    },
    onQuesRespondedOn(q: questionDetailModel) {
        this.storeCurrentSelectedMessage(q);
        this._redisplaySelectedMessageEl(q,true);
    },
    removeFaildResponseError() {
        this._lastAppendedTextEl.removeClass('error');
    },
    onFailToSendRespons() {
        this._lastAppendedTextEl.addClass('error');
    },
    handleOnSubmitResponse() {
        this.submitResponseBtn.on('click', () => {
            let val = this.messageTextInpEl.val() as string;
            val ? val.trim() : null;
            if (val) {
                this.quesCRUD.respondOnQuestion(val);
                this.messageTextInpEl.val('');
            }
        });
    },
    onSuccessToSendFaildMessage() {
        this.removeFaildResponseError();
    },
    handleOntryToSendFaildMessageAgain() {
        $(document.body).on('click', '#faildToSendResponseBtn', () => {
            this.quesCRUD.sendFaildResponseAgain();
        });
    },
    init(crud: QuestionsCRUD) {      
        this.quesCRUD = crud;
        this.handleOnMessageCardClick();
        this.handleOnSubmitResponse();
        this.handleOntryToSendFaildMessageAgain();
        this.quesCRUD.subscribe(this as any as ISubscriber);
    },
}

class QuestionsCRUD  {
    selectedQues: questionDetailModel;
    lastRespondedMessage: string;
    private _questions: questionDetailModel[] = [];
    private _obj: QuestionsCRUD = {} as any;
    private _subscripers: ISubscriber[] = [];
    private publishDataList() {
        this._subscripers.forEach(s => {
            if (s.onGetDataList) s.onGetDataList(this._questions.slice());
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
        TechSupportPageDom.startLoading();
        $.get(`${helper.Urls.techSupportUrl}/notResponded`)
            .done(data => {
                this._questions = data;
                this.publishDataList();
                setTimeout(() => {
                   
                }, 3000);
            }).fail(err => {
                alert('cannot get data')
            }).always(e => {
                TechSupportPageDom.stopLoading();
            });
    }
    subscribe(_subscriber: ISubscriber) {
        this._subscripers.push(_subscriber);
    }
    private _setSelectedQuestion(q: questionDetailModel) {
        this.selectedQues = q;
        this._subscripers.forEach(s => {
            if (s.onQuesSelected) s.onQuesSelected(q);
        });
        if (q && q.id) {
            this.markAsSeen();
        }
    }
    setSelectedQuestion(q: questionDetailModel) {
        this._setSelectedQuestion(q);
    }
    setSelectedQuestionById(id: string) {
        let q = this._questions.find(e => e.id == id);
        this._setSelectedQuestion(q);
    }
    private _markAsSeen() {
        this.selectedQues.seenAt = new Date().toDateString();
        let qInd = this._questions.findIndex(e => e.id == this.selectedQues.id);
        this._questions[qInd] = this.selectedQues;
    }
    markAsSeen() {
        if (this.selectedQues.seenAt) return;
        let setting: JQuery.AjaxSettings = {
            url: `${helper.Urls.techSupportUrl}/${this.selectedQues.id}`,
            method:'PUT'
        };
        $.ajax(setting)
            .done(() => {
                this._markAsSeen();
            })
            .fail(() => {
                console.log('faild to mark messsage as seen')
            })
            .always(() => {
            });
    }
    private _responseOnQuestion(id,response: string) {
        let qInd = this._questions.findIndex(e => e.id == this.selectedQues.id);
        if (!this.selectedQues.responses) this.selectedQues.responses = [];
        this.selectedQues.responses.push({
            id:id,
            createdAt:new Date().toDateString(),
            message:response,
            relatedToId:this.selectedQues.id,
            seenAt: null
        });
        this._questions[qInd] = this.selectedQues;
        this._subscripers.forEach(s => {
            if (s.onQuesRespondedOn) s.onQuesRespondedOn(this.selectedQues);
        });
    }
    private _onSuccessToSendFaildResponse() {
        this._subscripers.forEach(s => {
            if (s.onSuccessToSendFaildMessage) s.onSuccessToSendFaildMessage();
        });
    }
    private _failToSendResponse() {
        this._subscripers.forEach(s => {
            if (s.onFailToSendRespons) s.onFailToSendRespons();
        });
    }
    respondOnQuestion(response: string) {
        if (!this.selectedQues) return;
        this.lastRespondedMessage = response;
        let data = {
            response,
            relatedToId: this.selectedQues.id,
            customerId: this.selectedQues.senderId
        };
        this._responseOnQuestion(null, response);
        $.post(`${helper.Urls.techSupportUrl}`, JSON.stringify(data))
            .fail(err => {
                this._failToSendResponse();
            });
    }
    sendFaildResponseAgain() {
        if (!this.selectedQues) return;
        let data = {
            response: this.lastRespondedMessage,
            relatedToId: this.selectedQues.id
        };
        this._onSuccessToSendFaildResponse();
        $.post(`${helper.Urls.techSupportUrl}`, data)
            .fail(err => {
                this._failToSendResponse();
            });
    }
    addNewQuestion(q: questionDetailModel) {
        this._questions.unshift(q);
        this._subscripers.forEach(s => {
            if (s.onGetNewQuestion) s.onGetNewQuestion(q);
        });
    }
}
const TechSupportSquestionManager = {
    quesCRUD: null as QuestionsCRUD,
    onGetDataList(data: questionDetailModel[]) {
        TechSupportPageDom.reDisplayMessages(data);
    },
    handleOnGetData() {
        this.quesCRUD.subscribe(this as any as ISubscriber);
    },
    onQuesSelected(q: questionDetailModel) {
        TechSupportPageDom.setSelectedQuestion(q);
    },
    start() {
        if (TechSupportPageDom.checkIfCurrentPageIsTechSupport) {
            this.quesCRUD = QuestionsCRUD.Create();
            this.quesCRUD.getCustomerQuestions();
            this.handleOnGetData();
            TechSupportPageDom.init(this.quesCRUD);
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
//window['techDom'] = TechSupportPageDom;
//window['notifDom'] = NotificationDom;