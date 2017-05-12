var FilterItemValue = (function () {
    function FilterItemValue(id, type, isSelected, value) {
        this.Id = id;
        this.Type = type;
        this.IsSelected = isSelected;
        this.Value = value;
    }
    return FilterItemValue;
}());
var FilterModel = (function () {
    function FilterModel() {
        this.filterSpecifications = [];
        this.filterChemodanTypes = [];
        this.filterCategoryes = [];
        this.listProductsQuery = $(".js-list-products");
        this.selectedFiltersQuery = $(".js-selected-filters");
        this.filterBlockQuery = $(".js-product-filter");
    }
    FilterModel.prototype.activateFilter = function (id, type, isSelected, value) {
        var _this = this;
        var filterValue = new FilterItemValue(id, type, true, value);
        if (this.filterCategoryes.length === 0) {
            this.filterCategoryes.push(new FilterItemValue($(".js-filter-cat").val(), " ", true, " "));
        }
        switch (type) {
            case "Specification":
                if (this.filterSpecifications.length === 0) {
                    this.filterSpecifications.push(filterValue);
                }
                else {
                    if (isSelected) {
                        var tmpArr = [];
                        this.filterSpecifications.forEach(function (item) {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {
                            }
                            else {
                                tmpArr.push(item);
                            }
                        });
                        this.filterSpecifications = tmpArr;
                    }
                    else {
                        this.filterSpecifications.push(filterValue);
                    }
                }
                break;
            case "Category":
                break;
            case "ChemodanType":
                if (this.filterChemodanTypes.length === 0) {
                    this.filterChemodanTypes.push(filterValue);
                }
                else {
                    if (isSelected) {
                        var tmpArr = [];
                        this.filterChemodanTypes.forEach(function (item) {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {
                            }
                            else {
                                tmpArr.push(item);
                            }
                        });
                        this.filterChemodanTypes = tmpArr;
                    }
                    else {
                        this.filterChemodanTypes.push(filterValue);
                    }
                }
                break;
            case "ClearAll":
                this.filterSpecifications = [];
                this.filterChemodanTypes = [];
                //this.filterCategoryes = [];
                break;
            default:
        }
        $.post('/Home/ListProducts/', {
            filtersSp: JSON.stringify(this.filterSpecifications),
            filtersTp: JSON.stringify(this.filterChemodanTypes),
            filtersCt: JSON.stringify(this.filterCategoryes)
        })
            .done(function (data) {
            _this.listProductsQuery.html(data);
        });
    };
    FilterModel.prototype.filterViewer = function (e) {
        if (this.filterBlockQuery.hasClass("hide-mobile")) {
            this.filterBlockQuery.removeClass("hide-mobile");
            $(e.currentTarget).find(".glyphicon").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
        }
        else {
            this.filterBlockQuery.addClass("hide-mobile");
            $(e.currentTarget).find(".glyphicon").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
        }
    };
    FilterModel.prototype.unCheckFilter = function (element) {
        element.removeClass("active");
        element.data("is-selected", "false");
        element.find("input").prop("checked", false);
    };
    FilterModel.prototype.checkFilter = function (element) {
        element.addClass("active");
        element.data("is-selected", "true");
        element.find("input").prop("checked", true);
    };
    FilterModel.prototype.createCheckedFilter = function (filterId, filterType, valueId, filterText) {
        this.selectedFiltersQuery.prepend("\n                <span class=\"js-filter-remove-one\" data-filter-id=\"" + filterId + "\" data-filter-type=\"" + filterType + "\" data-filter-text=\"" + valueId + "\">\n                    " + filterText + "\n                    <span class=\"glyphicon glyphicon-remove-sign filter-clear\"></span>\n                </span>\n                                        ");
        if (this.selectedFiltersQuery.find(".js-filter-remove-one").length < 2) {
            this.selectedFiltersQuery.append("<span class=\"js-filter-clear\">\u043E\u0447\u0438\u0441\u0442\u0438\u0442\u044C</span>");
        }
    };
    FilterModel.prototype.removeCheckedFilter = function (filterId, filterType, filterText) {
        this.selectedFiltersQuery.find("span[data-filter-id=\"" + filterId + "\"][data-filter-type=\"" + filterType + "\"][data-filter-text=\"" + filterText + "\"]").remove();
        if (this.selectedFiltersQuery.find(".js-filter-remove-one").length < 1) {
            this.selectedFiltersQuery.find(".js-filter-clear").remove();
        }
    };
    return FilterModel;
}());
var filterModel;
$(function () {
    var filterItems = $(".js-filter-item");
    if (filterItems.length > 0) {
        filterModel = new FilterModel();
        $(document).on("click", ".js-filter-item", function (e) {
            var fId = $(e.currentTarget).data("filter-id");
            var type = $(e.currentTarget).data("filter-type");
            var valueId = $(e.currentTarget).data("filter-text");
            // var isSelected = $(e.currentTarget).data("is-selected");
            var isSelected = $(e.currentTarget).hasClass("active");
            var text = $(e.currentTarget).find(".js-txt").html();
            filterModel.activateFilter(fId, type, isSelected, valueId);
            if ($(e.currentTarget).hasClass("active")) {
                filterModel.unCheckFilter($(e.currentTarget));
                filterModel.removeCheckedFilter(fId, type, valueId);
            }
            else {
                filterModel.checkFilter($(e.currentTarget));
                filterModel.createCheckedFilter(fId, type, valueId, text);
            }
        });
        $(document).on("click", ".js-filter-clear", function (e) {
            //var fId = $(e.currentTarget).data("filter-id");
            //var type = $(e.currentTarget).data("filter-type");
            filterModel.activateFilter(null, "ClearAll", false, null);
            var filterItems = $(".js-filter-item.active");
            filterItems.find("input").prop("checked", false);
            filterItems.removeClass("active");
            filterModel.selectedFiltersQuery.find("span").remove(); //:not('.js-filter-clear')
        });
        $(document).on("click", ".js-filter-remove-one", function (e) {
            var fId = $(e.currentTarget).data("filter-id");
            var type = $(e.currentTarget).data("filter-type");
            var text = $(e.currentTarget).data("filter-text");
            filterModel.activateFilter(fId, type, true, text);
            filterModel.removeCheckedFilter(fId, type, text);
            var filterItem = $(".js-filter-item.active[data-filter-id=\"" + fId + "\"][data-filter-type=\"" + type + "\"][data-filter-text=\"" + text + "\"]");
            filterItem.find("input").prop("checked", false);
            filterItem.removeClass("active");
            $(e.currentTarget).remove();
        });
        filterItems.filter(".active").each(function (i, element) {
            var fId = $(element).data("filter-id");
            var type = $(element).data("filter-type");
            var valueId = $(element).data("filter-text");
            var text = $(element).find(".js-txt").html();
            //filterModel.activateFilter(fId, type, true, valueId);
            filterModel.filterChemodanTypes.push(new FilterItemValue(fId, type, true, valueId));
            filterModel.createCheckedFilter(fId, type, valueId, text);
        });
    }
});
