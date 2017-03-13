﻿
class FilterItemValue {
    Id: number;
    Value: string;
    IsSelected: boolean;
    Type: string;

    constructor(id: number, type: string, isSelected: boolean, value: string) {
        this.Id = id;
        this.Type = type;
        this.IsSelected = isSelected;
        this.Value = value;
    }
}

class FilterModel {
    filterSpecifications: FilterItemValue[];
    filterChemodanTypes: FilterItemValue[];
    filterCategoryes: FilterItemValue[];

    listProductsQuery: JQuery;

    constructor() {
        this.filterSpecifications = [];
        this.filterChemodanTypes = [];
        this.filterCategoryes = [];

        this.listProductsQuery = $(".js-list-products");
    }

   activateFilter(id: number, type: string, isSelected: boolean, value: string): void {

        var filterValue = new FilterItemValue(id,type, true, value);
        if (this.filterCategoryes.length===0) {
            this.filterCategoryes.push(new FilterItemValue($(".js-filter-cat").val(), " ", true, " "));
        }
        switch (type) {
            case "Specification":

                if (this.filterSpecifications.length === 0) {
                    this.filterSpecifications.push(filterValue);
                } else {

                    if (isSelected) {
                        var tmpArr = [];
                        this.filterSpecifications.forEach((item) => {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {

                            } else {
                                tmpArr.push(item);
                            }
                        });
                        this.filterSpecifications = tmpArr;
                    } else {
                        this.filterSpecifications.push(filterValue);
                    }
                }

                break;
            case "Category":

                break;

            case "ChemodanType":
                if (this.filterChemodanTypes.length === 0) {
                    this.filterChemodanTypes.push(filterValue);
                } else {

                    if (isSelected) {
                        var tmpArr = [];
                        this.filterChemodanTypes.forEach((item) => {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {

                            } else {
                                tmpArr.push(item);
                            }
                        });
                        this.filterChemodanTypes = tmpArr;
                    } else {
                        this.filterChemodanTypes.push(filterValue);
                    }

                }
                break;
        default:
        }


        $.post('/Home/ListProducts/', {
            filtersSp: JSON.stringify(this.filterSpecifications),
            filtersTp: JSON.stringify(this.filterChemodanTypes),
            filtersCt: JSON.stringify(this.filterCategoryes)
        })
            .done((data) => {
                this.listProductsQuery.html(data);
            });
    }
   }


var filterModel: FilterModel;

$(() => {

    if ($(".js-filter-item").length>0) {
        filterModel = new FilterModel();

        $(document).on("click", ".js-filter-item", (e) => {
            
            filterModel.activateFilter(
                $(e.currentTarget).data("filter-id"),
                $(e.currentTarget).data("filter-type"),
                $(e.currentTarget).data("is-selected"),
                $(e.currentTarget).find(".js-txt").html());

          if ($(e.currentTarget).hasClass("active")) {
              $(e.currentTarget).removeClass("active");
              $(e.currentTarget).data("is-selected", false);
              $(e.currentTarget).find("input").prop("checked",false);
          } else {
              $(e.currentTarget).addClass("active");
              $(e.currentTarget).data("is-selected", true);
              $(e.currentTarget).find("input").prop("checked", true);
          }
        });
    }




});