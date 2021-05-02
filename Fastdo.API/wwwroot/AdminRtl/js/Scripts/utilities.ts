
const Urls= {
    apiUrl: '/api/admins/drgsReq',
    getDrugInfoUrl: '/api/admins/drgs',
    getPharmaInfoUrl: '/api/admins/pharmacies',
    getVstockApiUrl: '/api/admins/vstock',
    techSupportUrl: '/api/admins/techsupport',
    adminTechSupportUrl:'/AdminPanel/TechSupport'
}
export default {
    getDateObj(dateStr: string) {
        return new Date(dateStr);
    },
    Urls
}