import { Utils } from '../utils/index.js'

$(document).ready(() => {
    console.log("Testr");
    GetCategories();
})

function GetCategories() {
    $.get({
        url: Utils.url('/categories'),
        crossDomain: true,
        success: (response, status, xhr) => {
            if (xhr.status != 204) {
                console.log(response);
            } else
                console.log("Empty list");
        },
        error: (e) => {
            console.error(e);
        }
    });
}