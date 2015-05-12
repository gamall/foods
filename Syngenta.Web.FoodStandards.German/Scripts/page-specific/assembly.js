$(function () {

    var selectedLevel = "intro",
    currentLeftPageIndex = 1,
    currentLevelPageSize = 0,
    listOfPages = "";
    listOfIntroPages = "",
    listOfAdvancedPages = "",
    page = 1,
    language = "GER",

    handlers = {
        onPageTurn: function () {

            var isIncrease = ($(this).attr("id") == "rightSideArrow");

            if (isIncrease) {
                currentLeftPageIndex += config.pageSize;
            }
            else {
                currentLeftPageIndex -= config.pageSize;
            }

            helpers.toggleNavigations();
            helpers.setImages();
            helpers.toggleAddPageCheckboxes();

            return false;
        },

        onSelectedLevel: function () {

            if (($(this).attr("id") == "advancedLevelButton")) {
                selectedLevel = "advanced";
            }
            else if (($(this).attr("id") == "standardLevelButton")) {
                selectedLevel = "intro";
            }

            helpers.setLevelButtonAndLabel();
            currentLevelPageSize = helpers.getNumberOfPages();
            currentLeftPageIndex = 1;
            helpers.setImages();
            helpers.toggleNavigations();
            helpers.toggleAddPageCheckboxes();
            helpers.setHiddenListOfPagesValue();

            return false;
        },

        onChangeAddPage: function () {

            page = $(this).hasClass('leftcheck') ? currentLeftPageIndex : (currentLeftPageIndex + 1);

            if ($(this).is(':checked')) {

                if (selectedLevel == "intro") {
                    listOfIntroPages += page + ",";
                }
                else {
                    listOfAdvancedPages += page + ",";
                }

                dataServices.addPage().done(callbacks.afterUpdateSelectedPage);
            }
            else {

                if (selectedLevel == "intro") {                    
                    listOfIntroPages = listOfIntroPages.replace(page + ",", "");
                }
                else {
                    listOfAdvancedPages = listOfAdvancedPages.replace(page + ",", "");
                }

                dataServices.removePage();
            }

            helpers.setHiddenListOfPagesValue();
            
        },

        onSaveBook: function () {

            var isValid = true;

            if ($(this).hasClass('preview')) {
                $(views.formAssembly).prop("action", config.previewPostUrl);
            }
            else {
                $(views.formAssembly).prop("action", config.downloadPostUrl);
            }

            if (!$(views.checkBoxLow).is(':checked') && !$(views.checkBoxHigh).is(':checked')) {
                $(views.errorMessage).html("Bitte wählen Sie Hohe Auflösung oder Niedrige Auflösung des PDF oder beidesaus.");
                isValid = false;
            }

            if (($(views.hidSelectedPagesIntro).val().length == 0) && ($(views.hidSelectedPagesAdvanced).val().length == 0)) {
                $(views.errorMessage).html("Bitte wählen Sie mindestens eine Seite aus, die in Ihrem E-Book enthalten ist.");
                isValid = false;
            }

            if (isValid) {
                $(views.formAssembly).submit();
            }

            return false;
        }

    };

    callbacks = {
        afterUpdateSelectedPage: function (data) {
            //$(views.errorMessage).html(data)
        }
    },

    config = {
        baseImageFolder: "/de/resources/images/",
        pageSize: 2,
        previewPostUrl: $("#PreviewPostUrl").val(),
        downloadPostUrl: $("#DownloadPostUrl").val(),
    },

    dataServices = {
        addPage: function () {
            
            var postData = {                
                IsIntroductory: selectedLevel == "intro" ? true : false,
                PageNumber: page,
                UserBook: {
                    LowQuality: $(views.checkBoxLow).is(':checked') ? true : false,
                    HighQuality: $(views.checkBoxHigh).is(':checked') ? true : false
                }
            };

            return $.ajax({
                url: "/de/api/debookapi",
                data: JSON.stringify(postData),
                type: "POST",
                contentType: "application/json; charset=utf-8"
            });

        },
        removePage: function () {

            var postData = {
                IsIntroductory: selectedLevel == "intro" ? true : false,
                PageNumber: page,
            };

            return $.ajax({
                url: "/de/api/debookapi",
                data: JSON.stringify(postData),
                type: "DELETE",
                contentType: "application/json; charset=utf-8"
            });
        }
    },

    helpers = {
        getNumberOfPages: function () {
            if (selectedLevel == "intro") {
                return $(views.hidIntroPageNumber).val();
            }
            else {
                return $(views.hidAdvancedPageNumber).val();
            }
        },

        toggleNavigations: function () {

            if (currentLeftPageIndex == 1) {
                $(views.leftNavigationArrow).hide();
            } else {
                $(views.leftNavigationArrow).show();
            }

            if (currentLeftPageIndex == (currentLevelPageSize - 1)) {
                $(views.rightNavigationArrow).hide();
            } else {
                $(views.rightNavigationArrow).show();
            }
        },

        setImages: function () {
            $(views.leftPageFrame).html(templates.image(currentLeftPageIndex));
            $(views.rightPageFrame).html(templates.image(currentLeftPageIndex + 1));
        },

        setLevelButtonAndLabel: function () {

            if (selectedLevel == "intro") {
                $(views.standardLevelButton).hide();
                $(views.standardLevelLabel).show();
                $(views.advancedLevelButton).show();
                $(views.advancedLevelLabel).hide();

            }
            else if (selectedLevel == "advanced") {
                $(views.standardLevelButton).show();
                $(views.standardLevelLabel).hide();
                $(views.advancedLevelButton).hide();
                $(views.advancedLevelLabel).show();
            }
        },

        toggleAddPageCheckboxes: function () {
            
            var selectedPages = [];
            if (selectedLevel == "intro") {
                selectedPages = $(views.hidSelectedPagesIntro).val().split(',');
                listOfPages = listOfIntroPages;
            }
            else {
                selectedPages = $(views.hidSelectedPagesAdvanced).val().split(',');
                listOfPages = listOfAdvancedPages;
            }
          
            var isLeftPageInTheList = false;
            var isRightPageInTheList = false;
            
            if (listOfPages.length > 0) {


                for (var i = 0; i < selectedPages.length; i++) {
                    if (selectedPages[i] == currentLeftPageIndex) {
                        
                        isLeftPageInTheList = true;
                    }
                    if (selectedPages[i] == (currentLeftPageIndex + 1)) {
                        isRightPageInTheList = true;
                    }

                    if (isLeftPageInTheList && isRightPageInTheList) {
                        break;
                    }
                }

            }            

            $(views.addLeftPage).prop('checked', isLeftPageInTheList);
            $(views.addRightPage).prop('checked', isRightPageInTheList);   

        },

        setHiddenListOfPagesValue: function () {

            if (listOfAdvancedPages != "" && listOfIntroPages != "") {
                if (selectedLevel == "advanced") {                
                    $(views.hidSelectedPagesAdvanced).val(listOfAdvancedPages.substr(0, listOfAdvancedPages.length - 1));
                }
                else {
                    $(views.hidSelectedPagesIntro).val(listOfIntroPages.substr(0, listOfIntroPages.length - 1));
                }
            }

        },

        setSelectedLevel: function () {
            selectedLevel = $(views.hidSelectedLevel).val() == "advanced" ? "advanced" : "intro";
        },

        bindEvents: function () {

            $(views.leftNavigationArrow).bind('click', handlers.onPageTurn);
            $(views.rightNavigationArrow).bind('click', handlers.onPageTurn);

            $(views.standardLevelButton).bind('click', handlers.onSelectedLevel);
            $(views.advancedLevelButton).bind('click', handlers.onSelectedLevel);

            $(views.addLeftPage).bind('click', handlers.onChangeAddPage);
            $(views.addRightPage).bind('click', handlers.onChangeAddPage);

            $(views.butDownload).bind('click', handlers.onSaveBook);
            $(views.butPreview).bind('click', handlers.onSaveBook);

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

        checkBoxLow: "#LowQuality",
        checkBoxHigh: "#HighQuality",
        errorMessage: "#errorMessage",

        hidSelectedPagesIntro: "#SelectedPagesIntro",
        hidSelectedPagesAdvanced: "#SelectedPagesAdvanced",

        butDownload: "#butSubmit",
        butPreview: "#butPreview",

        hidIntroPageNumber: "#IntroPageNumber",
        hidAdvancedPageNumber: "#AdvancedPageNumber",

        hidSelectedLevel: "#SelectedLevel",

        formAssembly: "#assembly-form"        
    },

    templates = {
        image: function (obj) {
            var levelType = selectedLevel == "intro" ? "background" : selectedLevel;
            if (obj < 10) obj = "0" + obj;
            return "<img src='" + config.baseImageFolder + selectedLevel + "/" + language + "-" + levelType + "-LR-" + obj + ".jpg' width='420px' height='594px' />";
        }
    },

    init = function () {       

        listOfIntroPages = $(views.hidSelectedPagesIntro).val() + ",";
        listOfAdvancedPages = $(views.hidSelectedPagesAdvanced).val() + ",";

        helpers.setSelectedLevel();

        //constructor or load logic
        helpers.bindEvents();
        helpers.toggleNavigations();
        helpers.setImages();

        helpers.setLevelButtonAndLabel();
        currentLevelPageSize = helpers.getNumberOfPages();
        //helpers.setHiddenListOfPagesValue();
        
        helpers.toggleAddPageCheckboxes();               

    };

    init();    

});