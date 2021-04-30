"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Urls = {
    apiUrl: '/api/admins/drgsReq',
    getDrugInfoUrl: '/api/admins/drgs',
    getPharmaInfoUrl: '/api/admins/pharmacies',
    getVstockApiUrl: '/api/admins/vstock',
    techSupportUrl: '/api/admins/techsupport'
};
exports.default = {
    getDateObj: function (dateStr) {
        return new Date(dateStr);
    },
    Urls: Urls
};
//# sourceMappingURL=utilities.js.map