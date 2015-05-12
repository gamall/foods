$(function () {

    var isIntro = false,
    isHighQuality = false,

    handlers = {
        onCompleteDownload: function () {
            isIntro = $(this).hasClass("intro") ? true : false;
            isHighQuality = $(this).hasClass("high") ? true : false;
            helpers.setHidIntro();
            helpers.setHidHiqhQuality();
            $(views.formSelect).prop("action", "savecompletebook");
            $(views.formSelect).submit();
            return false;
        },

        onIndividualDownload: function () {
            isIntro = $(this).hasClass("intro") ? true : false;
            isHighQuality = $(this).hasClass("high") ? true : false;
            helpers.setHidIntro();
            helpers.setHidHiqhQuality();
            $(views.formSelect).prop("action", "assembly");
            $(views.formSelect).submit();
            return false;
        }
    };

    config = {

    },

    helpers = {
        setHidIntro: function () {
            $(views.hidIsIntro).val(isIntro);
        },

        setHidHiqhQuality: function () {
            $(views.hidIsHighQuality).val(isHighQuality);
        },

        bindEvents: function () {
            $(views.butIntroCompleteHigh).bind('click', handlers.onCompleteDownload);
            $(views.butIntroCompleteLow).bind('click', handlers.onCompleteDownload);
            $(views.butAdvancedCompleteHigh).bind('click', handlers.onCompleteDownload);
            $(views.butAdvancedCompleteLow).bind('click', handlers.onCompleteDownload);
            $(views.butIntroIndividualHigh).bind('click', handlers.onIndividualDownload);
            $(views.butIntroIndividualLow).bind('click', handlers.onIndividualDownload);
            $(views.butAdvancedIndividualHigh).bind('click', handlers.onIndividualDownload);
            $(views.butAdvancedIndividualLow).bind('click', handlers.onIndividualDownload);
        }
    },

    views = {
        butIntroCompleteHigh: "#butIntroCompleteHigh",
        butIntroCompleteLow: "#butIntroCompleteLow",
        butIntroIndividualHigh: "#butIntroIndividualHigh",
        butIntroIndividualLow: "#butIntroIndividualLow",
        butAdvancedCompleteHigh: "#butAdvancedCompleteHigh",
        butAdvancedCompleteLow: "#butAdvancedCompleteLow",
        butAdvancedIndividualHigh: "#butAdvancedIndividualHigh",
        butAdvancedIndividualLow: "#butAdvancedIndividualLow",
        hidIsIntro: "#IsIntro",
        hidIsHighQuality: "#IsHighQuality",
        formSelect: "#select-form"
    },

    init = function () {
        helpers.bindEvents();
        helpers.setHidIntro();
        helpers.setHidHiqhQuality();        
    };

    init();  

});