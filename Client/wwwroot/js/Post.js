import { Utils } from '../js/utils/index.js'


let selectedCategory;

$(document).ready(() => {
    $("#createCategoryButton a").hide();
    console.log("Post");
    //GetCategories();
    $("#postError").hide();

    $("#postTitle").focus();



    GetCategories();

    $("#postTitle").on("input", () => {
        $("#postError").hide();
    })
    $("#postContent").on("input", () => {
        $("#postContentError").hide();
    })
    

    $("#savePostButton").click(() => AddPost());
    $("#editPostButton").click(() => EditPost());
    $(".removePostButton").click((e) => DeletePost(e));

    //console.log($("#dropdownCategoryList").data("selectedcategory"));
})


function GetCategories() {



    let url = Utils.url(`/categories`);
    $.get({
        url: url,
        crossDomain: true,
        success: (response, status, xhr) => {

            if (response == undefined || response == null)
                response = [];


            if (response.length != 0) {
                $("#createCategoryButton a").hide();
                $("#dropdownCategory").show();
                selectedCategory = response[0].id;
                $("#dropdownCategory:first-child").text(response[0].title);
                $("#dropdownCategory:first-child").val(response[0].title);
            } else {
                $("#createCategoryButton a").show();
                $("#dropdownCategory").hide();
            }


            if ($("#dropdownCategoryList").data("selectedcategory") != undefined)
                selectedCategory = $("#dropdownCategoryList").data("selectedcategory");

            console.log(selectedCategory)
            response.forEach(r => {
                $("#dropdownCategoryList").append(` <a class="dropdown-item" href="#" data-id="${r.id}">${r.title}</a>`);
                    if (selectedCategory == r.id) {
                        $("#dropdownCategory").text(r.title);
                        $("#dropdownCategory").val(r.title);
                    }
                console.log(selectedCategory)
                console.log(r.id);
                console.log($("#dropdownCategory"));
            })



            $("#dropdownCategoryList a").click(function () {
                selectedCategory = $(this).data("id");
                $("#dropdownCategory").text($(this).text());
                $("#dropdownCategory").val($(this).text());
            });
            
        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#postError").html("This post already exists");
                $("#postError").show();
            }
        }
    });
}


function AddPost() {

    let postTitle = $("#postTitle").val().trim();
    let postContent = $("#postContent").val().trim();
    let postDate = $("#postPubDate").val().trim();
    console.log(postDate);
    if (postTitle == "") {
        $("#postError").html("The title cannot be empty");
        $("#postError").show();
        $("#postTitle").focus();
        return;
    }

    if (!selectedCategory) {
        $("#postDropdownError").html("You have to select or create a category!");
        $("#postDropdownError").show();
        $("#postDropdownError").focus();
        return;
    }

    if (postContent == "") {
        $("#postContentError").show();
        $("#postContentError").focus();
        return;
    }

    let url = Utils.url(`/posts`);
    console.log(url);
    $.post({
        url: url,
        crossDomain: true,
        contentType: "application/json",
        data: JSON.stringify({
            title: postTitle,
            content: postContent,
            pubdate: postDate,
            categoryid: selectedCategory
        }),
        success: (response, status, xhr) => {
            console.log(response);
            window.location.href = window.location.origin;

        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#postError").html("This post already exists");
                $("#postError").show();
            }
        }
    });

}


function EditPost() {
    let postTitle = $("#postTitle").val().trim();
    let postContent = $("#postContent").val().trim();
    let postDate = $("#postPubDate").val().trim();
    let postId = $("#postTitle").data("id");
    console.log(postDate);
    if (postTitle == "") {
        $("#postError").html("The title cannot be empty");
        $("#postError").show();
        $("#postTitle").focus();
        return;
    }

    if (!selectedCategory) {
        $("#postDropdownError").html("You have to select or create a category!");
        $("#postDropdownError").show();
        $("#postDropdownError").focus();
        return;
    }

    if (postContent == "") {
        $("#postContentError").show();
        $("#postContentError").focus();
        return;
    }

    let url = Utils.url(`/posts/${postId}`);
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: "application/json",
        data: JSON.stringify({
            //id: postId,
            title: postTitle,
            content: postContent,
            pubdate: postDate,
            categoryid: selectedCategory
        }),
        crossDomain: true,
        success: (response, status, xhr) => {
            window.location.href = window.location.origin;
        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#postError").html("This post already exists");
                $("#postError").show();
            }
        }
    });
}

function DeletePost(e) {
    console.log("Delete Post");
    let postId = $(e.target).data("id");
    let url = Utils.url(`/posts/${postId}`);
    $.ajax({
        url: url,
        type: 'DELETE',
        crossDomain: true,
        success: (response, status, xhr) => {
            window.location.href = window.location.origin;
        },
        error: (e, xhr) => {
            if (e.status == 409) {
                $("#postError").html("This post already exists");
                $("#postError").show();
            }
        }
    });
}