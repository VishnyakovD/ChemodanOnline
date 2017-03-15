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
    FilterModel.prototype.createCheckedFilter = function (filterId, filterType, filterText) {
        this.selectedFiltersQuery.prepend("\n                <span data-filter-id=\"" + filterId + "\" data-filter-type=\"" + filterType + "\">\n                    " + filterText + "\n                    <span class=\"glyphicon glyphicon-remove-sign filter-clear\"></span>\n                </span>\n                                        ");
    };
    FilterModel.prototype.removeCheckedFilter = function (filterId, filterType, filterText) {
        this.selectedFiltersQuery.find("span[data-filter-id=\"" + filterId + "\"][data-filter-type=\"" + filterType + "\"]").remove();
    };
    return FilterModel;
}());
var filterModel;
$(function () {
    if ($(".js-filter-item").length > 0) {
        filterModel = new FilterModel();
        $(document).on("click", ".js-filter-item", function (e) {
            var fId = $(e.currentTarget).data("filter-id");
            var type = $(e.currentTarget).data("filter-type");
            // var isSelected = $(e.currentTarget).data("is-selected");
            var isSelected = $(e.currentTarget).hasClass("active");
            var text = $(e.currentTarget).find(".js-txt").html();
            filterModel.activateFilter(fId, type, isSelected, text);
            if ($(e.currentTarget).hasClass("active")) {
                filterModel.unCheckFilter($(e.currentTarget));
                filterModel.removeCheckedFilter(fId, type, text);
            }
            else {
                filterModel.checkFilter($(e.currentTarget));
                filterModel.createCheckedFilter(fId, type, text);
            }
        });
        $(document).on("click", ".js-filter-clear", function (e) {
            //var fId = $(e.currentTarget).data("filter-id");
            //var type = $(e.currentTarget).data("filter-type");
            filterModel.activateFilter(null, "ClearAll", false, null);
            var filterItems = $(".js-filter-item.active");
            filterItems.find("input").prop("checked", false);
            filterItems.removeClass("active");
            filterModel.selectedFiltersQuery.find("span:not('.js-filter-clear')").remove();
        });
    }
});
//# sourceMappingURL=filters.js.map