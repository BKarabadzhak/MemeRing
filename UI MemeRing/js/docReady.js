$(document).ready(function () {

    var win = $(window);

    win.scroll(function () {

        if ($(document).height() - win.height() == win.scrollTop()) {
            if (window.perPage == 'infinite') {
                if (window.paginationMode === 'All') {

                    const currentPage = getFilename(window.location.pathname);
                    if (currentPage === 'HomePage.html') {
                        getPhotos();
                    }
                    else if (currentPage === 'Cabinet.html') {
                        getCurrentUserPhotos();
                    }
                }
                else if (window.paginationMode === 'Description') {
                    findByDescription(window.searchText, window.currentPage + 1);
                }
            }
        }
    });


    $('.add-image').click(function () {
        $("#Description").val('');
        $("#imageId").attr('src', '');
        $(".fileName").text('');
        $('#uploader').modal();
    });


    $('#searchBtnId').on('click', function () {

        const text = document.getElementById("searchText").value;
        if (text.length > 0) {
            $('#mainContainer').empty();
            findByDescription(text);
        }
        else {
            const currentPage = getFilename(window.location.pathname);
            if (currentPage === 'HomePage.html') {
                getPhotos();
            }
            else if (currentPage === 'Cabinet.html') {
                getCurrentUserPhotos();
            }
        }
    })


    $(document).on('click', '.delete-image', function () {
        const id = $(this).attr('data-id');
        window.localStorage.setItem('idToDelete', id);
        $('#confirmDeletion').modal();
    });


    $(document).on('click', '#yesSelect', function () {
        deletePhoto(window.localStorage.getItem('idToDelete'));
    });


    $(document).on('click', '.homePage', function () {
        window.localStorage.setItem('mode', 'false');
    });


    $(document).on('click', '#usernameLink', function () {
        //if clicks on link of random user
        const id = $(this).attr('data-idd');
        window.localStorage.setItem('mode', 'true');
        window.localStorage.setItem('randomUserId', id);
        window.localStorage.setItem('randomUserName', $(this).text());
        let s = window.localStorage.getItem('randomUserName');
        getPhotos(1, window.perPage, id);
        $('.homePage').removeClass('active');
    })


    $(document).on('click', '.edit-image', function () {

        const id = $(this).attr('data-id');
        $('#editor .description-input').val('');
        $('#editor').modal();

        let type = 'GET';
        let data;
        let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
        let url = 'http://localhost:5000/api/Photos/' + id + '/object';
        let suc = function (response) {
            editing = response;
            $('#editor .description-input').val(editing.description);
            $('#editor .edit-preview').attr('src', 'http://localhost:5000/api/Photos/' + editing.id);
        };
        let err = function (response) {
            if (window.localStorage.getItem('Token') === null) {
                notify(response.statusText);
            }
            else {
                notify(response.responseJSON.message);
            }
        };
    
        request(type, data, url, headers, suc, err);
    });


    $(document).on('keyup', '#searchText', function () {

        if ($("#customSelect").val() == 'on page') {
            const searchstring = $(this).val();
            $('.image-card').each(function () {
                if ($(this).find('.description').text().toLowerCase().includes(searchstring.toLowerCase())) $(this).show();
                else $(this).hide();
            });
        } else {
            $('.search-btn').removeClass('isDisabled');
        }

    });


    $(document).on('click', '.image-card .image', function () {
        const id = $(this).attr('data-idcard');
        $('#exampleModalLong').empty();
        $('#preview img').attr('src', $(this).find('img').attr('src'));
        $('#preview').modal();

        let type = 'GET';
        let data;
        let headers =  { "Authorization": "Bearer " + window.localStorage.getItem('Token'), };
        let url = 'http://localhost:5000/api/Photos/' + id + '/object';
        let suc = function (response) {
            $('#exampleModalLong').append(response.description);
        };
        let err = function (response) {
            if (window.localStorage.getItem('Token') === null) {
                notify(response.statusText);
            }
            else {
                notify(response.responseJSON.message);
            }
        };

        request(type, data, url, headers, suc, err);
    });


    $(document).on('change', '#file', function () {
        $('.fileName').text($(this).val());
    });


    $(document).on('change', '#customSelect', function () {
        const selectItem = document.getElementById("customSelect");

        if (selectItem.value === 'on page') {

            //if searchbar is not empty show all images
            if (document.getElementById("searchText").value.length > 0) {
                const currentPage = getFilename(window.location.pathname);
                if (currentPage === 'HomePage.html') {
                    getPhotos();
                }
                else if (currentPage === 'Cabinet.html') {
                    getCurrentUserPhotos();
                }
            }

            //clear search bar
            $("#searchText").val('');
            $('.search-btn').addClass('isDisabled');

        }
        else if (selectItem.value === 'in system') {
            if (document.getElementById("searchText").value.length > 0) {
                const currentPage = getFilename(window.location.pathname);
                if (currentPage === 'HomePage.html') {
                    getPhotos();
                }
                else if (currentPage === 'Cabinet.html') {
                    getCurrentUserPhotos();
                }
            }

            $("#searchText").val('');
            $('.search-btn').addClass('isDisabled');
        }
    })


    $(document).on('click', '.perpage a', function () {
        let perpage = ($(this).hasClass('infinite')) ? 'infinite' : $(this).text();
        if (perpage === 'infinite')
            $('#mainContainer').empty();
        const currentPage = getFilename(window.location.pathname);
        if (currentPage === 'HomePage.html') {
            getPhotos(1, perpage);
        }
        else if (currentPage === 'Cabinet.html') {
            getCurrentUserPhotos(1, perpage);
        }

        $('.perpage a').removeClass('active');
        $(this).addClass('active');
    });


    $(document).on('click', '.page-link', function () {

        if (window.paginationMode == "All") {
            const currentPage = getFilename(window.location.pathname);
            if (currentPage === 'HomePage.html') {
                getPhotos($(this).attr('data-page'), $(this).attr('data-perpage'));
            }
            else if (currentPage === 'Cabinet.html') {
                getCurrentUserPhotos($(this).attr('data-page'), $(this).attr('data-perpage'));
            }
        }
        else if (window.paginationMode == 'Description') {
            $('#mainContainer').empty();
            findByDescription(window.searchText, $(this).attr('data-page'));
        }
    });


    $(document).on('click', '.searchClear', function () {

        window.paginationMode = "All    ";
        $(this).removeClass('active');
        $("#searchText").val('');
        $('#mainContainer').empty();
        const currentPage = getFilename(window.location.pathname);
        if (currentPage === 'HomePage.html') {
            getPhotos(1);
        }
        else if (currentPage === 'Cabinet.html') {
            getCurrentUserPhotos(1);
        }
    });

    $("#file").change(function() {
        readURL(this);
    });

});