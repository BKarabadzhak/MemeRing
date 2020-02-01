var currentPage = 1, perPage = 8, paginationMode = 'All', searchText = '';

function request(requestname, data, url, headers, suc, err){
    $.ajax({
        type: requestname,
        dataType: 'json',
        data: JSON.stringify(data),
        contentType: "application/json",
        url: url,
        headers: headers,
        success: suc,
        error: err
    });
}

function getPhotos(page, perpage, userid) {

    let header;
    if (window.localStorage.getItem('Token') === null) {
        header = '<a class="homePage" href="HomePage.html">Home page</a>\
                <a class="#MAIN_PAGE" href="Login.html">Login</a>\
                <a class="#MAIN_PAGE" href="Register.html">Registration</a>';
    }
    else {
        header = '<a class="homePage" href="HomePage.html">Home page</a>\
                <a class="cabinetPage" href="Cabinet.html">Cabinet</a>\
                <a class="#MAIN_PAGE" href="HomePage.html" onclick="logout()">Log out</a>';
    }

    $('.header-right').empty();
    $('.header-right').append(header);

    $('.cabinetPage').removeClass('active');
    $('.homePage').addClass('active');

    if (!page) {
        page = (window.perPage == 'infinite') ? window.currentPage + 1 : window.currentPage;
    }
    if (!perpage) {
        perpage = window.perPage;
    }

    window.currentPage = page;
    window.perPage = perpage;
    window.paginationMode = 'All';

    if (perpage !== 'infinite') {
        $('#mainContainer').empty();
    }
    else {
        perpage = 8;
    }

    if (window.localStorage.getItem('mode') === 'true') {
        $('.usernameTitle2').empty();
        let name = "<b>" + window.localStorage.getItem('randomUserName') + "</b>";
        $('.usernameTitle2').append(name);
    }

    if (!userid){
        userid = window.localStorage.getItem('randomUserId');
    }

    let url;
    if (window.localStorage.getItem('mode') === 'true') {
        url = 'http://localhost:5000/api/Photos/UserPhotos/' + userid;
    }
    else {
        url = 'http://localhost:5000/api/Photos/Pagination';
    }

    let type = 'POST';
    let data = { PageNumber: page, PageSize: perpage };
    let headers = { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let suc = function (response) {
        if (response) {
            for (let i = 0; i < response.pagination.photos.length; i++) {
                getUsername(response.pagination.photos[i]);
            }
            renderPagination(page, response.pagination.allPagesNumber, perpage);
        }
        else if (window.currentPage === 1) {
            notify("There are no memes.");
        }
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null){
            notify(response.statusText);
        }
        else {
            notify(response.responseJSON.message);
        }
    };

    request(type, data, url, headers, suc, err);
}


function getCurrentUserPhotos(page, perpage) {

    let header;
    if (window.localStorage.getItem('Token') === null) {
        header = '<a class="#MAIN_PAGE" href="Login.html">Login</a>\
                <a class="#MAIN_PAGE" href="Register.html">Registration</a>';
    }
    else {
        header = '<a class="homePage" href="HomePage.html">Home page</a>\
                <a class="cabinetPage" href="Cabinet.html">Cabinet</a>\
                <a class="#MAIN_PAGE" href="HomePage.html" onclick="logout()">Log out</a>';
    }

    $('.header-right').empty();
    $('.header-right').append(header);

    $('.homePage').removeClass('active');
    $('.cabinetPage').addClass('active');

    if (window.localStorage.getItem('CurrentUserName') !== null) {
        $('.usernameTitle').empty();
        let name = "<b>" + window.localStorage.getItem('CurrentUserName') + "</b>";
        $('.usernameTitle').append(name);
    }

    if (!page) {
        page = (window.perPage == 'infinite') ? window.currentPage + 1 : window.currentPage;
    }
    if (!perpage) {
        perpage = window.perPage;
    }

    window.currentPage = page;
    window.perPage = perpage;
    window.paginationMode = 'All';

    if (perpage !== 'infinite') {
        $('#mainContainer').empty();
    }
    else {
        perpage = 8;
    }

    let type = 'POST';
    let data = { PageNumber: page, PageSize: perpage };
    let url = 'http://localhost:5000/api/Photos/UserPhotos';
    let headers = { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let suc = function (response) {
        if (response) {
            for (let i = 0; i < response.pagination.photos.length; i++) {
                getUsername(response.pagination.photos[i]);
            }
            renderPagination(page, response.pagination.allPagesNumber, perpage);
        }
        else if (window.currentPage === 1) {
            notify("There are no memes.");
        }
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null)
            notify(response.statusText);
    };

    request(type, data, url, headers, suc, err);
}


function logout() {
    window.localStorage.removeItem('Token');
    window.localStorage.removeItem('CurrentUserId');
    window.localStorage.removeItem('CurrentUserName');
    window.localStorage.removeItem('idToDelete');
    window.localStorage.removeItem('mode');
    window.localStorage.removeItem('randomUserId');
    window.localStorage.removeItem('randomUserName');
}


function getUsername(responseFromGetPhotos) {

    let type = 'GET';
    let data;
    let headers ={ "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let url = 'http://localhost:5000/api/Users/' + responseFromGetPhotos.userId;
    let suc = function (response) {
        if (response) {
            const currentPage = getFilename(window.location.pathname);
            if (currentPage === 'HomePage.html') {
                renderCard(response.username, responseFromGetPhotos.description, 'http://localhost:5000/api/Photos/' + responseFromGetPhotos.id, responseFromGetPhotos.id, response.id);
            }
            else if (currentPage === 'Cabinet.html') {
                renderCardRegister(response.username, responseFromGetPhotos.description, 'http://localhost:5000/api/Photos/' + responseFromGetPhotos.id, responseFromGetPhotos.id);
            }
        }
        else {
            notify("No users in system.");
        }
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null)
            notify(response.statusText);
        else
            notify(response.responseJSON.message);
    };

    request(type, data, url, headers, suc, err);
}


function renderCard(username, description, image, id, useridd) {

    const card = '<div class="image-card text-center">\
        <div class="image" data-idcard="' + id + '">\
            <span class="helperToImage"></span><img width="100%" height="100%" src="' + image + '">\
        </div>\
    <div class="info">\
        <div class="description">\
            ' + description + '\
        </div>\
        <div class="username">\
            <i class="fa fa-user" aria-hidden="true"></i>\
            <a href="javascript://" id="usernameLink" data-idd="' + useridd + '">' + username + '</a>\
        </div>\
        <div class="buttons">\
        </div>\
    </div>\
</div>';
    $('#mainContainer').append(card);
}

function renderCardRegister(username, description, image, id) {

    const card = '<div class="image-card text-center">\
        <div class="image" data-idcard="' + id + '">\
            <span class="helperToImage"></span><img width="100%" height="100%" src="' + image + '">\
        </div>\
    <div class="info">\
        <div class="description">\
            ' + description + '\
        </div>\
        <div class="username">\
            <i class="fa fa-user" aria-hidden="true"></i>\
            ' + username + '\
        </div>\
        <div class="buttons">\
        <a href="javascript://" class="btn btn-warning edit-image" data-id="' + id + '">Edit</a>\
        <a href="javascript://" class="btn btn-danger delete-image" id="deleteImage" data-id="' + id + '">Delete</a>\
        </div>\
    </div>\
</div>';
    $('#mainContainer').append(card);
}


function renderPagination(page, pages, perpage) {
    $('#pagination').empty();

    if (window.perPage !== 'infinite')
        for (let i = 1; i <= pages; i++) {
            $('#pagination').append('<li class="page-item ' + ((page == i) ? 'disabled' : '') + '"><a class="page-link" href="#" data-page="' + i + '" data-perpage="' + perpage + '">' + i + '</a></li>');
        }
}


function upload() {
    var files = $('#file')[0].files[0];

    var reader = new FileReader();

    reader.onloadend = function () {

        const base = reader.result;

        const description = document.getElementById("Description").value;

        const fileToUploadDto = { "FileName": files.name, "Description": description, "Content": base };

        let type = 'POST';
        let data = fileToUploadDto;
        let url = 'http://localhost:5000/api/Photos';
        let headers = { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
        let suc = function (response) {
            notify(response.responseString);
            if (window.perPage == 'infinite') {
                $('#mainContainer').empty();
                getCurrentUserPhotos(1);
            }
            getCurrentUserPhotos();
        };
        let err = function (response) {
            if (window.localStorage.getItem('Token') === null){
                notify(response.statusText);
            }
            else {
                notify(response.responseJSON.message);
            }
        };
        
        request(type, data, url, headers, suc, err);
    }

    reader.readAsDataURL(files);
}


function findByDescription(text, page) {

    if (page) {
        window.currentPage = page;
    }
    else {
        window.currentPage = 1;
    }

    window.searchText = text;

    let photoToSearchDto;
    if (window.perPage !== 'infinite')
        photoToSearchDto = { "Description": text, PageNumber: window.currentPage, PageSize: window.perPage };
    else {
        photoToSearchDto = { "Description": text, PageNumber: window.currentPage, PageSize: 8 };
    }

    const currentPage = getFilename(window.location.pathname);
    let url;
    if (currentPage === 'HomePage.html') {
        if (window.localStorage.getItem('mode') === 'true') {
            url = 'http://localhost:5000/api/Photos/UserDescription/' + window.localStorage.getItem('randomUserId');
        }
        else {
            url = 'http://localhost:5000/api/Photos/Description';
        }
    }
    else if (currentPage === 'Cabinet.html') {
        url = 'http://localhost:5000/api/Photos/UserDescription';
    }

    let type = 'POST';
    let data = photoToSearchDto;
    let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let suc = function (response) {
        if (response) {
            for (let i = 0; i < response.photo.length; i++) {
                getUsername(response.photo[i]);
            }

            if (window.perPage !== 'infinite') {
                renderPagination(window.currentPage, response.allPagesNumber, window.perPage);
            }
            window.paginationMode = 'Description';
        }
        else if (window.currentPage === 1) {
            notify("There are no memes to search on this description.");
        }

        $('.searchClear').addClass('active');
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null) {
            notify(response.statusText);
        }
        $('.searchClear').addClass('active');
    };

    request(type, data, url, headers, suc, err);
}


function notify(message) {
    $('.modal').modal('hide');
    $('#notification .modal-body p').text(message);
    $('#notification').modal();
}


var editing = {};
function update() {

    editing.description = $('#editor .description-input').val();

    let type = 'PUT';
    let data = editing;
    let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let url = 'http://localhost:5000/api/Photos/' + editing.id;
    let suc = function (response) {
        notify(response.responseString);
        if (window.perPage == 'infinite') {
            $('#mainContainer').empty();
            getCurrentUserPhotos(1);
        }
        getCurrentUserPhotos();
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null)
            notify(response.statusText);
        else
            notify(response.responseJSON.message);
    };

    request(type, data, url, headers, suc, err);
};


function deletePhoto(id) {

    const fileToDeleteDto = { "PhotoId": id };

    let type = 'DELETE';
    let data = fileToDeleteDto;
    let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
    let url = 'http://localhost:5000/api/Photos/' + id;
    let suc = function (response) {
        notify(response.responseString);
        if (window.perPage == 'infinite') {
            $('#mainContainer').empty();
            getCurrentUserPhotos(1);
        }
        getCurrentUserPhotos();
    };
    let err = function (response) {
        if (window.localStorage.getItem('Token') === null)
            notify(response.statusText);
        else
            notify(response.responseJSON.message);
    };

    request(type, data, url, headers, suc, err);
};

function getFilename(fullPath) {
    return fullPath.substring(fullPath.lastIndexOf('/') + 1);
}

//display preview image after uploading in modal
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        
        reader.onload = function (e) {
            $('#imageId').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

// $(document).ready(function () {

//     var win = $(window);

//     win.scroll(function () {

//         if ($(document).height() - win.height() == win.scrollTop()) {
//             if (window.perPage == 'infinite') {
//                 if (window.paginationMode === 'All') {

//                     const currentPage = getFilename(window.location.pathname);
//                     if (currentPage === 'HomePage.html') {
//                         getPhotos();
//                     }
//                     else if (currentPage === 'Cabinet.html') {
//                         getCurrentUserPhotos();
//                     }
//                 }
//                 else if (window.paginationMode === 'Description') {
//                     findByDescription(window.searchText, window.currentPage + 1);
//                 }
//             }
//         }
//     });


//     $('.add-image').click(function () {
//         $("#Description").val('');
//         $("#imageId").attr('src', '');
//         $(".fileName").text('');
//         $('#uploader').modal();
//     });


//     $('#searchBtnId').on('click', function () {

//         const text = document.getElementById("searchText").value;
//         if (text.length > 0) {
//             $('#mainContainer').empty();
//             findByDescription(text);
//         }
//         else {
//             const currentPage = getFilename(window.location.pathname);
//             if (currentPage === 'HomePage.html') {
//                 getPhotos();
//             }
//             else if (currentPage === 'Cabinet.html') {
//                 getCurrentUserPhotos();
//             }
//         }
//     })


//     $(document).on('click', '.delete-image', function () {
//         const id = $(this).attr('data-id');
//         window.localStorage.setItem('idToDelete', id);
//         $('#confirmDeletion').modal();
//     });


//     $(document).on('click', '#yesSelect', function () {
//         deletePhoto(window.localStorage.getItem('idToDelete'));
//     });


//     $(document).on('click', '.homePage', function () {
//         window.localStorage.setItem('mode', 'false');
//     });


//     $(document).on('click', '#usernameLink', function () {
//         //if clicks on link of random user
//         const id = $(this).attr('data-idd');
//         window.localStorage.setItem('mode', 'true');
//         window.localStorage.setItem('randomUserId', id);
//         window.localStorage.setItem('randomUserName', $(this).text());
//         let s = window.localStorage.getItem('randomUserName');
//         getPhotos(1, window.perPage, id);
//         $('.homePage').removeClass('active');
//     })


//     $(document).on('click', '.edit-image', function () {

//         const id = $(this).attr('data-id');
//         $('#editor .description-input').val('');
//         $('#editor').modal();

//         let type = 'GET';
//         let data;
//         let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
//         let url = 'http://localhost:5000/api/Photos/' + id + '/object';
//         let suc = function (response) {
//             editing = response;
//             $('#editor .description-input').val(editing.description);
//             $('#editor .edit-preview').attr('src', 'http://localhost:5000/api/Photos/' + editing.id);
//         };
//         let err = function (response) {
//             if (window.localStorage.getItem('Token') === null) {
//                 notify(response.statusText);
//             }
//             else {
//                 notify(response.responseJSON.message);
//             }
//         };
    
//         request(type, data, url, headers, suc, err);
//     });


//     $(document).on('keyup', '#searchText', function () {

//         if ($("#customSelect").val() == 'on page') {
//             const searchstring = $(this).val();
//             $('.image-card').each(function () {
//                 if ($(this).find('.description').text().toLowerCase().includes(searchstring.toLowerCase())) $(this).show();
//                 else $(this).hide();
//             });
//         } else {
//             $('.search-btn').removeClass('isDisabled');
//         }

//     });


//     $(document).on('click', '.image-card .image', function () {
//         const id = $(this).attr('data-idcard');
//         $('#exampleModalLong').empty();
//         $('#preview img').attr('src', $(this).find('img').attr('src'));
//         $('#preview').modal();

//         let type = 'GET';
//         let data;
//         let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
//         let url = 'http://localhost:5000/api/Photos/' + id + '/object';
//         let suc = function (response) {
//             $('#exampleModalLong').append(response.description);
//         };
//         let err = function (response) {
//             if (window.localStorage.getItem('Token') === null) {
//                 notify(response.statusText);
//             }
//             else {
//                 notify(response.responseJSON.message);
//             }
//         };

//         request(type, data, url, headers, suc, err);
//     });


//     $(document).on('change', '#file', function () {
//         $('.fileName').text($(this).val());
//     });


//     $(document).on('change', '#customSelect', function () {
//         const selectItem = document.getElementById("customSelect");

//         if (selectItem.value === 'on page') {

//             //if searchbar is not empty show all images
//             if (document.getElementById("searchText").value.length > 0) {
//                 const currentPage = getFilename(window.location.pathname);
//                 if (currentPage === 'HomePage.html') {
//                     getPhotos();
//                 }
//                 else if (currentPage === 'Cabinet.html') {
//                     getCurrentUserPhotos();
//                 }
//             }

//             //clear search bar
//             $("#searchText").val('');
//             $('.search-btn').addClass('isDisabled');

//         }
//         else if (selectItem.value === 'in system') {
//             if (document.getElementById("searchText").value.length > 0) {
//                 const currentPage = getFilename(window.location.pathname);
//                 if (currentPage === 'HomePage.html') {
//                     getPhotos();
//                 }
//                 else if (currentPage === 'Cabinet.html') {
//                     getCurrentUserPhotos();
//                 }
//             }

//             $("#searchText").val('');
//             $('.search-btn').addClass('isDisabled');
//         }
//     })


//     $(document).on('click', '.perpage a', function () {
//         let perpage = ($(this).hasClass('infinite')) ? 'infinite' : $(this).text();
//         if (perpage === 'infinite')
//             $('#mainContainer').empty();
//         const currentPage = getFilename(window.location.pathname);
//         if (currentPage === 'HomePage.html') {
//             getPhotos(1, perpage);
//         }
//         else if (currentPage === 'Cabinet.html') {
//             getCurrentUserPhotos(1, perpage);
//         }

//         $('.perpage a').removeClass('active');
//         $(this).addClass('active');
//     });


//     $(document).on('click', '.page-link', function () {

//         if (window.paginationMode == "All") {
//             const currentPage = getFilename(window.location.pathname);
//             if (currentPage === 'HomePage.html') {
//                 getPhotos($(this).attr('data-page'), $(this).attr('data-perpage'));
//             }
//             else if (currentPage === 'Cabinet.html') {
//                 getCurrentUserPhotos($(this).attr('data-page'), $(this).attr('data-perpage'));
//             }
//         }
//         else if (window.paginationMode == 'Description') {
//             $('#mainContainer').empty();
//             findByDescription(window.searchText, $(this).attr('data-page'));
//         }
//     });


//     $(document).on('click', '.searchClear', function () {

//         window.paginationMode = "All    ";
//         $(this).removeClass('active');
//         $("#searchText").val('');
//         $('#mainContainer').empty();
//         const currentPage = getFilename(window.location.pathname);
//         if (currentPage === 'HomePage.html') {
//             getPhotos(1);
//         }
//         else if (currentPage === 'Cabinet.html') {
//             getCurrentUserPhotos(1);
//         }
//     });

//     $("#file").change(function() {
//         readURL(this);
//     });

// });