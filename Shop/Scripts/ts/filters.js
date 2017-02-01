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
        this.FilterSpecifications = [];
        this.FilterChemodanTypes = [];
        this.FilterCategoryes = [];
        this.listProductsQuery = $(".js-list-products");
    }
    FilterModel.prototype.ActivateFilter = function (id, type, isSelected, value) {
        var _this = this;
        var filterValue = new FilterItemValue(id, type, true, value);
        switch (type) {
            case "Specification":
                if (this.FilterSpecifications.length === 0) {
                    this.FilterSpecifications.push(filterValue);
                }
                else {
                    if (isSelected) {
                        var tmpArr = [];
                        this.FilterSpecifications.forEach(function (item) {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {
                            }
                            else {
                                tmpArr.push(item);
                            }
                        });
                        this.FilterSpecifications = tmpArr;
                    }
                    else {
                        this.FilterSpecifications.push(filterValue);
                    }
                }
                break;
            case "Category":
                break;
            case "ChemodanType":
                break;
            default:
        }
        console.log(this.FilterSpecifications);
        $.ajax({
            type: "POST",
            url: "/Home/ListProducts",
            success: function (data) {
                _this.listProductsQuery.html(data);
            },
            data: { filtersSP: JSON.stringify(this.FilterSpecifications) }
        });
    };
    return FilterModel;
}());
var filterModel;
$(function () {
    if ($(".js-filter-item").length > 0) {
        filterModel = new FilterModel();
        $(document).on("click", ".js-filter-item", function (e) {
            filterModel.ActivateFilter($(e.currentTarget).data("filter-id"), $(e.currentTarget).data("filter-type"), $(e.currentTarget).data("is-selected"), $(e.currentTarget).html());
            if ($(e.currentTarget).hasClass("active")) {
                $(e.currentTarget).removeClass("active");
                $(e.currentTarget).data("is-selected", false);
            }
            else {
                $(e.currentTarget).addClass("active");
                $(e.currentTarget).data("is-selected", true);
            }
        });
    }
});
//# sourceMappingURL=filters.js.map