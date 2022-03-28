import { Utils } from '../js/utils/index.js'


$(document).ready(() => {
    console.log("Category");
    //GetCategories();
    $("#categoryError").hide();

    $("#categoryTitle").focus();
    $("#categoryTitle").on("input", () => {
        $("#categoryError").hide();
    })

    $("#saveCategoryButton").click(() => AddCategory());
    $("#editCategoryButton").click(() => EditCategory());
    $(".removeCategoryButton").click((e) => DeleteCategory(e));
})



function AddCategory() {

    let categoryTitle = $("#categoryTitle").val().trim();

    if (categoryTitle == "") {
        $("#categoryError").html("The title cannot be empty");
        $("#categoryError").show();
        return;
    }

    let url = Utils.url(`/categories?title=${categoryTitle}`);
    $.post({
        url: url,
        crossDomain: true,
        data: {
            title: categoryTitle
        },
        success: (response, status, xhr) => {
            window.location.href = window.location.origin;
        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#categoryError").html("This category already exists");
                $("#categoryError").show();
            }
        }
    });

}


function EditCategory() {
    let categoryId = $("#categoryTitle").data("id");
    let categoryTitle = $("#categoryTitle").val().trim();

    if (categoryTitle == "") {
        $("#categoryError").html("The title cannot be empty");
        $("#categoryError").show();
        return;
    }

    let url = Utils.url(`/categories/${categoryId}?title=${categoryTitle}`);
    $.ajax({
        url: url,
        type: 'PUT',
        crossDomain: true,
        success: (response, status, xhr) => {
            window.location.href = window.location.origin;
        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#categoryError").html("This category already exists");
                $("#categoryError").show();
            }
        }
    });
}

function DeleteCategory(e) {
    console.log("Delete Category");
    let categoryId = $(e.target).data("id");
    let url = Utils.url(`/categories/${categoryId}`);
    $.ajax({
        url: url,
        type: 'DELETE',
        crossDomain: true,
        success: (response, status, xhr) => {
            window.location.href = window.location.origin;
        },
        error: (e, xhr) => {
            switch (e.status) {
                case 400:
                    $("#categoryError").html("You cannot delete a category that has posts");
                    $("#categoryError").show();
            }
        }
        
    });
}