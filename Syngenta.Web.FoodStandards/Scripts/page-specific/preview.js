$(function () {

    var selectedLevel = "intro",
    currentLeftPageIndex = 0,
    currentRightPageIndex = 0,
    currentLevelPageSize = 0,
    listOfPages = "";
    arrayPage = [],
    page = 1,
    pageIndex = 0,
    language = "ENG",

    handlers = {
        onPageTurn: function () {

            var isIncrease = ($(this).attr("id") == "rightSideArrow");

            if (isIncrease) {
                pageIndex += config.pageSize;
            }
            else {
                pageIndex -= config.pageSize;
            }

            helpers.setCurrentPageIndex(pageIndex);
            helpers.toggleNavigations();
            helpers.setImages();
            helpers.toggleRemoveLinks();

            return false;
        },

        onChangeRemovePage: function () {
            var arrIndex = 0;

            if ($(this).hasClass('leftlink')) {
                page = currentLeftPageIndex;
                arrIndex = pageIndex;
            }
            else {
                page = currentRightPageIndex;
                arrIndex = pageIndex + 1;
            }
            
            helpers.spliceArrayPage(arrIndex);

            if (arrayPage.length == pageIndex && arrayPage.length > 1) {
                pageIndex -= config.pageSize;
            }

            helpers.setCurrentPageIndex(pageIndex);
            currentLevelPageSize = helpers.getNumberOfPages();
            helpers.toggleNavigations();
            helpers.setImages();
            helpers.toggleRemoveLinks();
            
            dataServices.removePage();
            
            return false;
        },

    };

    callbacks = {
        afterUpdateSelectedPage: function (data) {
            $(views.errorMessage).html(data)
        }
    },

    config = {
        baseImageFolder: "/en/resources/images/",
        pageSize: 2,
    },

    dataServices = {

        removePage: function () {

            var postData = {
                IsIntroductory: page.substr(page.length - 1, 1) == "i" ? true : false,
                PageNumber: page.substr(0, page.length - 1),
            };

            return $.ajax({
                url: "/en/api/bookapi",
                data: JSON.stringify(postData),
                type: "DELETE",
                contentType: "application/json; charset=utf-8"
            });
        }
    },

    helpers = {
        getNumberOfPages: function () {
            return arrayPage.length;
        },

        toggleNavigations: function () {
            if (pageIndex == 0) {
                $(views.leftNavigationArrow).hide();
            } else {
                $(views.leftNavigationArrow).show();
            }

            if (pageIndex >= (currentLevelPageSize - config.pageSize)) {
                $(views.rightNavigationArrow).hide();
            } else {
                $(views.rightNavigationArrow).show();
            }
        },

        setImages: function () {
            $(views.leftPageFrame).html(templates.image(currentLeftPageIndex));
            $(views.rightPageFrame).html(templates.image(currentRightPageIndex));
        },

        setPageArray: function () {
            arrayPage = $(views.hidSelectedPages).val().split(',');
        },

        setCurrentPageIndex: function (obj) {           
            currentLeftPageIndex = arrayPage[obj];
            currentRightPageIndex = arrayPage[obj + 1];
        },

        spliceArrayPage: function (obj) {
            arrayPage.splice(obj, 1);
        },

        toggleRemoveLinks: function () {
            if (currentLeftPageIndex == undefined || currentLeftPageIndex == "") {
                $(views.linkRemoveLeft).hide();
            }
            else {
                $(views.linkRemoveLeft).show();
            }

            if (currentRightPageIndex == undefined) {
                $(views.linkRemoveRight).hide();
            }
            else {
                $(views.linkRemoveRight).show();
            }
        },

        bindEvents: function () {

            $(views.leftNavigationArrow).bind('click', handlers.onPageTurn);
            $(views.rightNavigationArrow).bind('click', handlers.onPageTurn);
            $(views.linkRemoveLeft).bind('click', handlers.onChangeRemovePage);
            $(views.linkRemoveRight).bind('click', handlers.onChangeRemovePage);

        }
    },

    views = {
        leftNavigationArrow: "#leftSideArrow",
        rightNavigationArrow: "#rightSideArrow",
        leftPageFrame: "#leftPage",
        rightPageFrame: "#rightPage",
        standardLevelButton: "#standardLevelButton",
        advancedLevelButton: "#advancedLevelButton",
        standardLevelLabel: "#standardLevelLabel",
        advancedLevelLabel: "#advancedLevelLabel",
        addLeftPage: "#addleftpage",
        addRightPage: "#addrightpage",
        errorMessage: "#errorMessage",
        hidSelectedPages: "#SelectedPages",
        linkRemoveLeft: "#linkRemoveLeft",
        linkRemoveRight: "#linkRemoveRight"
       
    },

    templates = {
        image: function (obj) {
            var levelType = "";
            if (obj != undefined && obj != "") {
                if (obj.substr(obj.length - 1, 1) == "i") {
                    selectedLevel = "intro";
                    levelType = "background";
                }
                else {
                    selectedLevel = "advanced";
                    levelType = "advanced";
                }
                if (obj.length < 3) obj = "0" + obj;
                return "<img src='" + config.baseImageFolder + selectedLevel + "/" + language + "-" + levelType + "-LR-" + obj.substr(0, obj.length - 1) + ".jpg' width='420px' height='594px' />";
            }
            else {
                return "";
            }
        }
    },

    init = function () {       

        helpers.setPageArray();
        currentLevelPageSize = helpers.getNumberOfPages();
        helpers.setCurrentPageIndex(pageIndex);
        helpers.bindEvents();
        helpers.toggleNavigations();
        helpers.setImages();
        helpers.toggleRemoveLinks();
    };

    init();    

});