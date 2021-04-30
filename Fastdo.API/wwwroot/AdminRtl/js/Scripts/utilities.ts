
const Urls= {
    apiUrl: '/api/admins/drgsReq',
    getDrugInfoUrl: '/api/admins/drgs',
    getPharmaInfoUrl: '/api/admins/pharmacies',
    getVstockApiUrl: '/api/admins/vstock',
    techSupportUrl:'/api/admins/techsupport'
}
export default {
    getDateObj(dateStr: string) {
        return new Date(dateStr);
    },
    Urls
}