
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
    FilterSpecifications: FilterItemValue[];
    FilterChemodanTypes: FilterItemValue[];
    FilterCategoryes: FilterItemValue[];

    listProductsQuery: JQuery;

    constructor() {
        this.FilterSpecifications = [];
        this.FilterChemodanTypes = [];
        this.FilterCategoryes = [];

        this.listProductsQuery = $(".js-list-products");
    }

    ActivateFilter(id: number, type: string, isSelected: boolean, value: string): void {

        var filterValue = new FilterItemValue(id,type, true, value);
        if (this.FilterCategoryes.length===0) {
            this.FilterCategoryes.push(new FilterItemValue($(".js-filter-cat").val(), " ", true, " "));
        }
        switch (type) {
            case "Specification":

                if (this.FilterSpecifications.length === 0) {
                    this.FilterSpecifications.push(filterValue);
                } else {

                    if (isSelected) {
                        var tmpArr = [];
                        this.FilterSpecifications.forEach((item) => {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {

                            } else {
                                tmpArr.push(item);
                            }
                        });
                        this.FilterSpecifications = tmpArr;
                    } else {
                        this.FilterSpecifications.push(filterValue);
                    }
                }

                break;
            case "Category":

                break;

            case "ChemodanType":
                if (this.FilterChemodanTypes.length === 0) {
                    this.FilterChemodanTypes.push(filterValue);
                } else {

                    if (isSelected) {
                        var tmpArr = [];
                        this.FilterChemodanTypes.forEach((item) => {
                            if (item.Value === filterValue.Value && item.Id === filterValue.Id) {

                            } else {
                                tmpArr.push(item);
                            }
                        });
                        this.FilterChemodanTypes = tmpArr;
                    } else {
                        this.FilterChemodanTypes.push(filterValue);
                    }

                }
                break;
        default:
        }


        $.post('/Home/ListProducts/', {
            filtersSp: JSON.stringify(this.FilterSpecifications),
            filtersTp: JSON.stringify(this.FilterChemodanTypes),
            filtersCt: JSON.stringify(this.FilterCategoryes)
        })
            .done((data) => {
                this.listProductsQuery.html(data);
            });
    }


       
       // console.log(this.FilterSpecifications);


    
    
   }


var filterModel: FilterModel;

$(() => {

    if ($(".js-filter-item").length>0) {
        filterModel = new FilterModel();

        $(document).on("click", ".js-filter-item", (e) => {
            filterModel.ActivateFilter(
                $(e.currentTarget).data("filter-id"),
                $(e.currentTarget).data("filter-type"),
                $(e.currentTarget).data("is-selected"),
                $(e.currentTarget).html());

          if ($(e.currentTarget).hasClass("active")) {
              $(e.currentTarget).removeClass("active");
              $(e.currentTarget).data("is-selected", false);
          } else {
              $(e.currentTarget).addClass("active");
              $(e.currentTarget).data("is-selected",true);
          }
        });
    }




});